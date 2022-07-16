using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Globalization;
using System.IO;

namespace DESPLWEB
{
    public partial class RouteWiseDetails : System.Web.UI.Page
    {
        clsData db = new clsData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "RouteWise Business";
                ViewState["Details"] = null;

            }
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void grdReportStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // you already know you're looking at this row, so check your cell text
                TableCell statusCell = e.Row.Cells[1];
                statusCell.Width = 250;

                if (e.Row.Cells[0].Text == "&nbsp;" && e.Row.Cells[1].Text == "&nbsp;" && e.Row.Cells[2].Text == "&nbsp;")
                {
                    e.Row.BackColor = System.Drawing.Color.DarkGray;
                    e.Row.Cells[0].Width = 100;

                }
                else if (e.Row.Cells[0].Text == "Total")
                {
                    e.Row.BackColor = System.Drawing.Color.DarkGray;
                }
                else
                {

                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[0].Width = 100;

                }

            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //if (grdReportStatus.Rows.Count > 0)
            //{
            //    //PrintGrid.PrintGridView(grdReportStatus, "Routewise Business", "RoutewiseBusiness");
            //  // ExportToExcel(grdReportStatus, "E:\\DTtoEXCEL.xls");
            //}

            if (ViewState["Details"] != null)
            {
                DataTable dt = (DataTable)ViewState["Details"];
               // ExportToExcel(dt, "C:\\temp\\RoutewiseBusiness.xls");
                ExporttoExcel(dt);
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (rbRouteWise.Checked)
            {
                getRouteWiseDetailCollection();
            }
            else if (rbMaterialWise.Checked)
            {
                getMaterialWiseDetailCollection();
            }
        }

        private void getMaterialWiseDetailCollection()
        {
            DataRow dr1 = null; // dr2 = null, dr3 = null;
            DataTable distinctMonthYear = null, distinctTestName = null, dtable = null, dt = null;

            dt = db.getMaterialCollectionDetails();
            dtable = new DataTable();
            dtable.Columns.Add("Materials/Months");

            DataView view = new DataView(dt);
            distinctMonthYear = view.ToTable(true, "Mname", "Year");
            distinctTestName = view.ToTable(true, "TestName");

            foreach (DataRow dr in distinctMonthYear.Rows)
            {
                string monYear = dr["Mname"].ToString() + "-" + dr["Year"].ToString();
                dtable.Columns.Add(monYear);
            }
            dtable.Columns.Add("Total");
            foreach (DataRow dr in distinctTestName.Rows)
            {
                dr1 = dtable.NewRow();
                dr1["Materials/Months"] = dr["TestName"].ToString();
                dtable.Rows.Add(dr1);

            }
            dr1 = dtable.NewRow();
            dr1["Materials/Months"] = "zTotal";
            dtable.Rows.Add(dr1);

            int idxx = 0, sumH = 0, sumV = 0, colValue = 0; string month = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (month != (dt.Rows[i]["Mname"].ToString() + "-" + dt.Rows[i]["Year"].ToString()))
                    sumH = 0;

                DataRow[] foundRows = dtable.Select("[Materials/Months] like '%" + dt.Rows[i]["TestName"].ToString() + "%'");
                foreach (var row1 in foundRows)
                {
                    idxx = dtable.Rows.IndexOf(row1);
                    month = dt.Rows[i]["Mname"].ToString() + "-" + dt.Rows[i]["Year"].ToString();

                }

                dtable.Rows[idxx][month] = dt.Rows[i]["totalEnq"].ToString();


                sumH += Convert.ToInt32(dt.Rows[i]["totalEnq"].ToString());

                dtable.Rows[dtable.Rows.Count - 1][month] = sumH;

            }
            for (int i = 0; i < dtable.Rows.Count; i++)
            {
                sumV = 0;
                for (int j = 1; j < dtable.Columns.Count - 1; j++)
                {
                    if (dtable.Rows[i][j].ToString() == "")
                        colValue = 0;
                    else
                        colValue = Convert.ToInt32(dtable.Rows[i][j].ToString());

                    sumV += colValue;
                }

                dtable.Rows[i]["Total"] = sumV;
            }



            var newDataTable = dtable.AsEnumerable()
                   .OrderBy(r => r.Field<string>("Materials/Months"))
                   .CopyToDataTable();
            newDataTable.Rows[newDataTable.Rows.Count - 1][0] = "Total";
            grdReportStatus.DataSource = newDataTable;
            grdReportStatus.DataBind();
            ViewState["Details"] = newDataTable;
        }
        private void getRouteWiseDetailCollection()
        {
            DataRow dr1 = null, dr3 = null; //dr2 = null
            string[] arrMonth = new string[] { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            DataTable dt1 = db.getCollectionDetails();


            DataTable dtBillIdColl = new DataTable(); //= db.getBillEntriesMonthly();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mon");
            dt.Columns.Add("RouteName");
            dt.Columns.Add("Details");

            DataTable dt3 = new DataTable();
            dt3.Columns.Add("Mon");
            dt3.Columns.Add("RouteName");
            dt3.Columns.Add("Collection");

            int siteCount = 0, idxx = 0; double sum = 0;
            bool flgPrvCol = false;
            double billingmat = 0;

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Month");
            dt2.Columns.Add(new DataColumn("BillingAmount", typeof(decimal)));

            DataTable uniqueCols = dt1.DefaultView.ToTable(true, "Mon", "Year");
            DataView view = uniqueCols.DefaultView;
            view.Sort = "Year ASC,Mon ASC";
            DataTable sortedDt = view.ToTable();

            //int minYear = dt1.AsEnumerable()
            //      .Min(r => r.Field<int>("Year"));
            string monthNm = "", prvRouteNm = "", RouteNm = "", routeId = "", mon = "", year = "";

            if (dt1.Rows.Count > 0)
            {
                //var Date1 = Convert.ToDateTime(minYear + "-" + dt1.Rows[0]["Mon"].ToString() + "-01");
                //var Date2 = DateTime.Today;
                //Date2 = new DateTime(Date2.Year, Date2.Month, DateTime.DaysInMonth(Date2.Year, Date2.Month));

                //var colNmList = Enumerable.Range(0, Int32.MaxValue)
                //                     .Select(e => Date1.AddMonths(e))
                //                     .TakeWhile(e => e <= Date2)
                //                     .Select(e => e.ToString("MMMM-yyyy"));


                //foreach (var supp in colNmList)
                //{
                //    dt.Columns.Add(supp);
                //}

                for (int col = 0; col < sortedDt.Rows.Count; col++)
                {
                    dt.Columns.Add(arrMonth[Convert.ToInt32(sortedDt.Rows[col]["Mon"])] + "-" + sortedDt.Rows[col]["Year"].ToString());
                }

                int idx = 0;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {

                    monthNm = arrMonth[Convert.ToInt32(dt1.Rows[i]["Mon"])] + "-" + dt1.Rows[i]["Year"].ToString();
                    //if (i == 0)
                    //{
                    if (dt1.Rows[i]["ROUTE_Name_var"].ToString() != "")
                        RouteNm = dt1.Rows[i]["ROUTE_Name_var"].ToString();
                    else
                        RouteNm = "At-Lab";


                    // }

                    //DataRow[] foundRows = dt.Select("RouteName like '%" + RouteNm + "%'");
                    DataRow[] foundRows = dt.Select("RouteName='" + RouteNm + "'");
                    foreach (var row in foundRows)
                    {
                        idx = dt.Rows.IndexOf(row);
                        flgPrvCol = true;
                        // prvMon = dt.Rows[idx][0].ToString();
                        if (dt3.Rows.Count > 0)
                        {
                            DataRow[] foundRows1 = dt3.Select("RouteName like '%" + RouteNm + "%'");
                            foreach (var row1 in foundRows1)
                            {
                                idxx = dt3.Rows.IndexOf(row1);
                            }
                            if (dt3.Rows[idxx][1].ToString() != RouteNm)
                            {
                                dr3 = dt3.NewRow();
                                dr3["Mon"] = dt1.Rows[i]["Mon"].ToString();
                                dr3["RouteName"] = RouteNm;
                                dt3.Rows.Add(dr3);

                                idxx = dt3.Rows.Count - 1;
                            }
                            else
                            {
                                if (dt3.Rows[idxx][0].ToString() != dt1.Rows[i]["Mon"].ToString())
                                {
                                    dr3 = dt3.NewRow();
                                    dr3["Mon"] = dt1.Rows[i]["Mon"].ToString();
                                    dr3["RouteName"] = RouteNm;
                                    dt3.Rows.Add(dr3);
                                    idxx = dt3.Rows.Count - 1;
                                }
                            }

                        }
                        else
                        {
                            dr3 = dt3.NewRow();
                            dr3["Mon"] = dt1.Rows[i]["Mon"].ToString();
                            dr3["RouteName"] = RouteNm;
                            dt3.Rows.Add(dr3);
                        }
                    }



                    if (!flgPrvCol)
                    {

                        if (i != 0)
                        {
                            dr1 = dt.NewRow();
                            dt.Rows.Add(dr1);
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            dr1 = dt.NewRow();

                            if (j == 0)
                            {

                                dr1["RouteName"] = RouteNm;
                                prvRouteNm = RouteNm;


                                dr1["Mon"] = dt1.Rows[i]["Mon"].ToString();
                                dr1["Details"] = "No of Sites";
                                dr1[monthNm] = dt1.Rows[i]["SiteCount"].ToString();
                                dt.Rows.Add(dr1);
                            }
                            else if (j == 1)
                            {
                                dr1["Mon"] = "";
                                dr1["RouteName"] = "";
                                dr1["Details"] = "Total Business";
                                dr1[monthNm] = dt1.Rows[i]["BillingAmount"].ToString();
                                dt.Rows.Add(dr1);
                            }
                            else
                            {
                                dr1["Mon"] = "";
                                dr1["RouteName"] = "";
                                dr1["Details"] = "Total Collection";
                                routeId = dt1.Rows[i]["Route_Id"].ToString();
                                mon = dt1.Rows[i]["Mon"].ToString();
                                year = dt1.Rows[i]["Year"].ToString();
                                dtBillIdColl = db.getBillEntriesMonthly(routeId, mon, year);
                                double summ = 0;
                                if (dtBillIdColl.Rows.Count > 0)
                                {
                                    for (int k = 0; k < dtBillIdColl.Rows.Count; k++)
                                    {
                                        summ += db.getBillEntriesMonthlySum(dtBillIdColl.Rows[k][0].ToString());
                                    }
                                }
                                dr1[monthNm] = Math.Abs(summ).ToString("0.00");
                                dt.Rows.Add(dr1);


                            }
                        }

                    }
                    else
                    {
                        if (RouteNm == dt.Rows[idx][1].ToString())
                        {
                            mon = dt1.Rows[i]["Mon"].ToString();
                            //if (mon == dt3.Rows[idxx][0].ToString() && dt3.Rows[idxx][2].ToString() == "")
                            //{
                            routeId = dt1.Rows[i]["Route_Id"].ToString();
                            year = dt1.Rows[i]["Year"].ToString();
                            dtBillIdColl = db.getBillEntriesMonthly(routeId, mon, year);

                            if (dtBillIdColl.Rows.Count > 0)
                            {
                                sum = 0;
                                for (int k = 0; k < dtBillIdColl.Rows.Count; k++)
                                {
                                    sum += db.getBillEntriesMonthlySum(dtBillIdColl.Rows[k][0].ToString());
                                }
                                dt3.Rows[dt3.Rows.Count - 1]["Collection"] = sum;
                            }

                            //}

                            if (dt.Columns.Contains(monthNm))
                            {
                                if (dt.Rows[idx][monthNm].ToString() != "")
                                {
                                    siteCount = Convert.ToInt32(dt.Rows[idx][monthNm]) + Convert.ToInt32(dt1.Rows[i]["SiteCount"].ToString());
                                    billingmat = Convert.ToDouble(dt.Rows[idx + 1][monthNm]) + Convert.ToDouble(dt1.Rows[i]["BillingAmount"].ToString());

                                }
                                else
                                {
                                    siteCount = Convert.ToInt32(dt1.Rows[i]["SiteCount"].ToString());
                                    billingmat = Convert.ToDouble(dt1.Rows[i]["BillingAmount"].ToString());
                                }


                                dt.Rows[idx][monthNm] = siteCount;
                                dt.Rows[idx + 1][monthNm] = billingmat.ToString("0.00");
                                dt.Rows[idx + 2][monthNm] = Math.Abs(sum).ToString("0.00");
                                flgPrvCol = false;
                            }
                        }

                    }
                }

            }


            dt.Columns.Remove("Mon");
            grdReportStatus.DataSource = dt;
            grdReportStatus.DataBind();
            //grdReportStatus.Columns[0].Visible = false;
            ViewState["Details"] = dt;

        }

        private void ExportToExcel(DataTable table, string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            sw.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            sw.Write("<BR><BR><BR>");
            sw.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {
                sw.Write("<Td>");
                sw.Write("<B>");
                sw.Write(table.Columns[j].ToString());
                sw.Write("</B>");
                sw.Write("</Td>");
            }
            sw.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {
                sw.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sw.Write("<Td>");
                    sw.Write(row[i].ToString());
                    sw.Write("</Td>");
                }
                sw.Write("</TR>");
            }
            sw.Write("</Table>");
            sw.Write("</font>");
            sw.Close();
        }

        private void ExporttoExcel(DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=RoutewiseBusiness.xls");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = grdReportStatus.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(grdReportStatus.Columns[j].HeaderText.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}