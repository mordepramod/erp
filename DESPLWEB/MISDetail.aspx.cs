using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
//using Excel=Microsoft.Office.Interop.Excel;

namespace DESPLWEB
{
    public partial class MISDetail : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
          
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "MIS Detail";
                getCurrentDate();
                LoadInwardType();
                ddl_To.Items.Insert(0, "---Select---");
            }
        }
        public void getCurrentDate()
        {
            txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
            txt_Todate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        private void LoadInwardType()
        {
            ddl_InwardType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardType.DataValueField = "MATERIAL_RecordType_var";
            var inwd = dc.Material_View("", "");
            ddl_InwardType.DataSource = inwd;
            ddl_InwardType.DataBind();
            ddl_InwardType.Items.Insert(0, "---Select---");
        }
        protected void ddl_InwardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTimereqd.Text = "";
            txtEnq.Visible = false;
            lblenq.Visible = false;
            lblInwd.Visible = false;
            txtInwd.Visible = false;
            txtReport.Visible = false;
            lblRpt.Visible = false;
            lblAvgres.Text = string.Empty;
            grdMISDetail.Visible = false;
            lblTotalRecord.Text = "Total No of Records : 0 ";
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {   
            if (ddl_InwardType.SelectedItem.Text != "---Select---")
            {
                Bindgrd();
                DisplayAvg();
                DisplayTimewise();
            }
        }
        private void Bindgrd()
        {
            txtEnq.Visible = false;
            lblenq.Visible = false;
            lblInwd.Visible = false;
            txtInwd.Visible = false;
            txtReport.Visible = false;
            lblRpt.Visible = false;
            grdMISDetail.Visible = true;
            grdMISDetail.Columns[13].Visible = false;
            grdMISDetail.Columns[14].Visible = false;
            grdMISDetail.Columns[15].Visible = false;
            grdMISDetail.Columns[16].Visible = false;
            grdMISDetail.Columns[0].Visible = true;
            grdMISDetail.Columns[4].Visible = true;
            grdMISDetail.Columns[5].Visible = true;
            grdMISDetail.Columns[6].Visible = true;
            grdMISDetail.Columns[7].Visible = true;
            grdMISDetail.Columns[8].Visible = true;
            grdMISDetail.Columns[9].Visible = true;
            grdMISDetail.Columns[10].Visible = true;
            grdMISDetail.Columns[11].Visible = true;
            grdMISDetail.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdMISDetail.Columns[3].Visible = false;
            if (ddl_InwardType.SelectedValue == "MF")
            {
                grdMISDetail.Columns[3].Visible = true;
            }
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            var data = dc.MISDetailView(ddl_InwardType.SelectedValue, Fromdate, Todate);
            grdMISDetail.DataSource = data;
            grdMISDetail.DataBind();
           
          
            countTimeSlot();
            lblTotalRecord.Text = "Total No of Records : " + grdMISDetail.Rows.Count.ToString();
            lbl_Avg.Visible = true;
            lblAvgres.Visible = true;
            lblTimereqd.Text = "";
            for (int i = 0; i < grdMISDetail.Rows.Count; i++)
            {
                for (int j = i + 1; j < grdMISDetail.Rows.Count; j++)
                {
                    if (grdMISDetail.Rows[i].Cells[0].Text == grdMISDetail.Rows[j].Cells[0].Text)
                    {
                        grdMISDetail.Rows[j].Cells[0].Text = "";
                        grdMISDetail.Rows[j].Cells[1].Text = "";
                    }
                }
                for (int j = i + 1; j < grdMISDetail.Rows.Count; j++)
                {
                    if (grdMISDetail.Rows[i].Cells[2].Text == grdMISDetail.Rows[j].Cells[2].Text)
                    {
                        grdMISDetail.Rows[j].Cells[2].Text = "";
                    }
                }

            }
        }
        private void countTimeSlot()
        {
            try
            {
                int totalNoOfReports = 0, totalNoOfPer = 0; decimal per = 0;
                string[,] timeSlot = new string[6, 2] { { "4.0-8.0", "0" }, { "8.1-18.0", "0" }, { "18.1-24", "0" }, { "24.1-30", "0" }, { "30.1-50", "0" }, { "50.1-72", "0" } };

                lblTimeSlot.Text = "";
                for (int i = 0; i < grdMISDetail.Rows.Count; i++)
                {
                    string time = grdMISDetail.Rows[i].Cells[12].Text;
                    if (time != "")
                    {
                        for (int k = 0; k < timeSlot.Length / 2; k++)
                        {
                            string[] Val = timeSlot[k, 0].Split('-');
                            decimal minVal = Convert.ToDecimal(Val[0]);
                            decimal maxVal = Convert.ToDecimal(Val[1]);
                            decimal count = Convert.ToDecimal(timeSlot[k, 1]);

                            if (Convert.ToDecimal(time) >= minVal && Convert.ToDecimal(time) <= maxVal)
                                count++;

                            if (count > 0)
                                timeSlot[k, 1] = count.ToString();

                        }
                    }
                }

                for (int i = 0; i < timeSlot.Length / 2; i++)
                {
                    if (i == 0)
                    {
                        totalNoOfReports = Convert.ToInt32(timeSlot[0, 1]) + Convert.ToInt32(timeSlot[1, 1]) + Convert.ToInt32(timeSlot[2, 1]) + Convert.ToInt32(timeSlot[3, 1]) + Convert.ToInt32(timeSlot[4, 1]) + Convert.ToInt32(timeSlot[5, 1]);
                    }

                    if (totalNoOfReports != 0)
                        per = Math.Round((Convert.ToDecimal(timeSlot[i, 1]) * 100) / totalNoOfReports);
                    lblTimeSlot.Text += timeSlot[i, 0] + ":" + timeSlot[i, 1] + ":" + per + ";";

                    totalNoOfPer += Convert.ToInt32(per);
                }
                lblTimeSlot.Text += totalNoOfReports + "-" + totalNoOfPer;
            }
            catch { }

        }

        protected void lnk_TimeReport_Click(object sender, EventArgs e)
        {
            if (lblTimeSlot.Text != "")
            {
                string[] arr1 = lblTimeSlot.Text.Split(';');
                DataTable dtTable = new DataTable();
                dtTable.Columns.Add(new DataColumn("Time Range(Hrs.)", typeof(string)));
                dtTable.Columns.Add(new DataColumn("No of Reports", typeof(string)));
                dtTable.Columns.Add(new DataColumn("Percentage", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (i == arr1.Length - 1)
                    {
                        string[] arr2 = arr1[i].Split('-');
                        dr = dtTable.NewRow();
                        dr["Time Range(Hrs.)"] = "Total";
                        dr["No of Reports"] = arr2[0];
                        dr["Percentage"] = arr2[1];
                        dtTable.Rows.Add(dr);
                    }
                    else
                    {
                        string[] arr2 = arr1[i].Split(':');
                        dr = dtTable.NewRow();
                        dr["Time Range(Hrs.)"] = arr2[0];
                        dr["No of Reports"] = arr2[1];
                        dr["Percentage"] = arr2[2];
                        dtTable.Rows.Add(dr);
                    }
                }

                if (dtTable.Rows.Count > 0)
                {
                    PrintGrid.PrintTimeReport(dtTable, "TimeWise_MIS_Report");
                    //Excel.Application xla = new Excel.Application();
                    //xla.Visible = true;
                    //Excel.Workbook wb = xla.Workbooks.Add(Excel.XlSheetType.xlWorksheet);
                    //Excel.Worksheet ws = (Excel.Worksheet)wb.ActiveSheet;
                    ////********************** Now create the chart. *****************************
                    //Excel.ChartObjects chartObjs = (Excel.ChartObjects)ws.ChartObjects(Type.Missing);
                    //Excel.ChartObject chartObj = chartObjs.Add(300, 40, 300, 300);
                    //Excel.Chart xlChart = chartObj.Chart;

                    //int nRows = 6;
                    //int nColumns = dtTable.Columns.Count;
                    //string upperLeftCell = "B2";
                    //int endRowNumber = System.Int32.Parse(upperLeftCell.Substring(1))
                    //    + nRows ;
                    //char endColumnLetter = System.Convert.ToChar(
                    //    Convert.ToInt32(upperLeftCell[0]) + nColumns-1);
                    //string upperRightCell = System.String.Format("{0}{1}",
                    //    endColumnLetter, System.Int32.Parse(upperLeftCell.Substring(1)));
                    //string lowerRightCell = System.String.Format("{0}{1}",
                    //    endColumnLetter, endRowNumber);
                    //char endColumnLetterForChart = System.Convert.ToChar(
                    //    Convert.ToInt32(upperLeftCell[0]) + nColumns - 2);
                    //string lowerRightCellForChart = System.String.Format("{0}{1}",
                    // endColumnLetterForChart, endRowNumber);

                    //Excel.Range rg = ws.get_Range(upperLeftCell, lowerRightCell);
                    //for (int i = 0; i <= dtTable.Rows.Count-1; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        rg[i, 1] = dtTable.Columns[0].ColumnName;          //For Adding Header Text
                    //        rg[i, 2] = dtTable.Columns[1].ColumnName;
                    //        rg[i, 3] = dtTable.Columns[2].ColumnName; 
                    //    }
                    //    else
                    //    {
                    //        rg[i, 1] = dtTable.Rows[i - 1][0].ToString();          //For Adding Header Text
                    //        rg[i, 2] = int.Parse(dtTable.Rows[i - 1][1].ToString());  //For Adding Datarow Value
                    //        rg[i, 3] = int.Parse(dtTable.Rows[i - 1][2].ToString());
                    //    }
                    //}


                    //Excel.Range chartRange = ws.get_Range(upperLeftCell, lowerRightCellForChart);
                    //xlChart.SetSourceData(chartRange, Type.Missing);
                    //xlChart.ChartType = Excel.XlChartType.xlLine;

                    //// *******************Customize axes: ***********************
                    //Excel.Axis xAxis = (Excel.Axis)xlChart.Axes(Excel.XlAxisType.xlCategory,
                    //     Excel.XlAxisGroup.xlPrimary);
                    //xAxis.HasTitle = true;
                    //xAxis.AxisTitle.Text = "No of Reports";

                    //Excel.Axis yAxis = (Excel.Axis)xlChart.Axes(Excel.XlAxisType.xlSeriesAxis,
                    //     Excel.XlAxisGroup.xlPrimary);
                    //yAxis.HasTitle = true;
                    //yAxis.AxisTitle.Text = "Time Range(Hrs.)";

                    //Excel.Axis zAxis = (Excel.Axis)xlChart.Axes(Excel.XlAxisType.xlValue,
                    //     Excel.XlAxisGroup.xlPrimary);
                    ////zAxis.HasTitle = true;
                    ////zAxis.AxisTitle.Text = "Z Axis";

                    //// *********************Add title: *******************************
                    //xlChart.HasTitle = true;
                    //xlChart.ChartTitle.Text = "TimeWise MIS Report Graph";

                    //// *****************Set legend:***************************
                    //xlChart.HasLegend = true;
                    //wb.SaveCopyAs(Server.MapPath(@"File/Graph.xls"));

                    //// ****************For Quiting The Excel Aplication ***********************
                    ////if (xla != null)
                    ////{
                    ////    xla.DisplayAlerts = false;
                    ////    wb.Close();
                    ////    wb = null;
                    ////    xla.Quit();
                    ////    xla = null;
                    ////}
                }

            }
        }
        private void DisplayTimewise()
        {
            if (txt_Time.Text != "")
            {
                int c = 0;
                decimal Column = 0;
                for (int i = 0; i < grdMISDetail.Rows.Count; i++)
                {
                    if (decimal.TryParse(grdMISDetail.Rows[i].Cells[12].Text, out Column))
                    {
                        if (Convert.ToDecimal(grdMISDetail.Rows[i].Cells[12].Text) > Convert.ToDecimal(txt_Time.Text))
                        {
                            c = c + 1;
                            grdMISDetail.Rows[i].BackColor = System.Drawing.Color.LightPink;
                            //grdMISDetail.Rows[i].ForeColor = System.Drawing.Color.White;
                            lblTimereqd.Text = "Total time required above normal time : " + c;
                        }
                    }
                }
            }
        }
       
        protected void lnk_Filter_Click(object sender, EventArgs e)
        {
            if (ddl_To.SelectedValue != "---Select---" && ddl_InwardType.SelectedValue != "---Select---")
            {
                Bindgrd();
                grdMISDetail.Columns[13].Visible = false;
                grdMISDetail.Columns[14].Visible = false;
                grdMISDetail.Columns[15].Visible = false;
                grdMISDetail.Columns[16].Visible = false;
                grdMISDetail.Columns[4].Visible = false;
                grdMISDetail.Columns[5].Visible = false;
                grdMISDetail.Columns[6].Visible = false;
                grdMISDetail.Columns[7].Visible = false;
                grdMISDetail.Columns[8].Visible = false;
                grdMISDetail.Columns[9].Visible = false;
                grdMISDetail.Columns[10].Visible = false;
                grdMISDetail.Columns[11].Visible = false;

                if (ddl_To.SelectedValue == "Enquiry Date")
                {
                    grdMISDetail.Columns[4].Visible = true;
                }
                else if (ddl_To.SelectedValue == "Collection Date")
                {
                    grdMISDetail.Columns[4].Visible = true;
                    grdMISDetail.Columns[5].Visible = true;
                }
                else if (ddl_To.SelectedValue == "Recieved Date")
                {
                    grdMISDetail.Columns[4].Visible = true;
                    grdMISDetail.Columns[5].Visible = true;
                    grdMISDetail.Columns[6].Visible = true;
                }
                else if (ddl_To.SelectedValue == "Entered Date")
                {
                    grdMISDetail.Columns[4].Visible = true;
                    grdMISDetail.Columns[5].Visible = true;
                    grdMISDetail.Columns[6].Visible = true;
                    grdMISDetail.Columns[7].Visible = true;
                }
                else if (ddl_To.SelectedValue == "Checked Date")
                {
                    grdMISDetail.Columns[4].Visible = true;
                    grdMISDetail.Columns[5].Visible = true;
                    grdMISDetail.Columns[6].Visible = true;
                    grdMISDetail.Columns[7].Visible = true;
                    grdMISDetail.Columns[8].Visible = true;
                }
                else if (ddl_To.SelectedValue == "Approved Date")
                {
                    grdMISDetail.Columns[4].Visible = true;
                    grdMISDetail.Columns[5].Visible = true;
                    grdMISDetail.Columns[6].Visible = true;
                    grdMISDetail.Columns[7].Visible = true;
                    grdMISDetail.Columns[8].Visible = true;
                    grdMISDetail.Columns[9].Visible = true;
                }
                else if (ddl_To.SelectedValue == "Print Date")
                {
                    grdMISDetail.Columns[4].Visible = true;
                    grdMISDetail.Columns[5].Visible = true;
                    grdMISDetail.Columns[6].Visible = true;
                    grdMISDetail.Columns[7].Visible = true;
                    grdMISDetail.Columns[8].Visible = true;
                    grdMISDetail.Columns[9].Visible = true;
                    grdMISDetail.Columns[10].Visible = true;
                }
                else if (ddl_To.SelectedValue == "Outward")
                {
                    grdMISDetail.Columns[4].Visible = true;
                    grdMISDetail.Columns[5].Visible = true;
                    grdMISDetail.Columns[6].Visible = true;
                    grdMISDetail.Columns[7].Visible = true;
                    grdMISDetail.Columns[8].Visible = true;
                    grdMISDetail.Columns[9].Visible = true;
                    grdMISDetail.Columns[10].Visible = true;
                    grdMISDetail.Columns[11].Visible = true;
                }
                if (ddl_StageFrom.SelectedValue == "2")
                {
                    grdMISDetail.Columns[4].Visible = false;
                }
                else if (ddl_StageFrom.SelectedValue == "3")
                {
                    grdMISDetail.Columns[4].Visible = false;
                    grdMISDetail.Columns[5].Visible = false;
                }
                else if (ddl_StageFrom.SelectedValue == "4")
                {
                    grdMISDetail.Columns[4].Visible = false;
                    grdMISDetail.Columns[5].Visible = false;
                    grdMISDetail.Columns[6].Visible = false;
                }
                else if (ddl_StageFrom.SelectedValue == "5")
                {
                    grdMISDetail.Columns[4].Visible = false;
                    grdMISDetail.Columns[5].Visible = false;
                    grdMISDetail.Columns[6].Visible = false;
                    grdMISDetail.Columns[7].Visible = false;
                }
                else if (ddl_StageFrom.SelectedValue == "6")
                {
                    grdMISDetail.Columns[4].Visible = false;
                    grdMISDetail.Columns[5].Visible = false;
                    grdMISDetail.Columns[6].Visible = false;
                    grdMISDetail.Columns[7].Visible = false;
                    grdMISDetail.Columns[8].Visible = false;
                }
                else if (ddl_StageFrom.SelectedValue == "7")
                {
                    grdMISDetail.Columns[4].Visible = false;
                    grdMISDetail.Columns[5].Visible = false;
                    grdMISDetail.Columns[6].Visible = false;
                    grdMISDetail.Columns[7].Visible = false;
                    grdMISDetail.Columns[8].Visible = false;
                    grdMISDetail.Columns[9].Visible = false;
                }
                int j = 4;
                int first = 0;
                int Last = 0;
                for (j = 4; j < grdMISDetail.Columns.Count; j++)
                {
                    if (grdMISDetail.Columns[j].Visible == true)
                    {
                        first = j;
                        break;
                    }
                }
                int i = 0;
                for (j = grdMISDetail.Columns.Count - 6; j > 4; j--)
                {
                    if (grdMISDetail.Columns[j].Visible == true)
                    {
                        Last = j;
                        break;
                    }
                }
                for (i = 0; i < grdMISDetail.Rows.Count; i++)
                {
                    grdMISDetail.Rows[i].Cells[12].Text = string.Empty;
                    if (grdMISDetail.Rows[i].Cells[Last].Text != "" && grdMISDetail.Rows[i].Cells[Last].Text != "&nbsp;")
                    {
                        //TimeSpan objTimeSpan = Convert.ToDateTime(grdMISDetail.Rows[i].Cells[Last].Text) - Convert.ToDateTime(grdMISDetail.Rows[i].Cells[first].Text);

                        DateTime start = DateTime.ParseExact(grdMISDetail.Rows[i].Cells[first].Text, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                        DateTime end = DateTime.ParseExact(grdMISDetail.Rows[i].Cells[Last].Text, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);                        
                        TimeSpan objTimeSpan = end - start;
                        grdMISDetail.Rows[i].Cells[12].Text = String.Format("{0:00}.{1:00}", objTimeSpan.TotalHours, objTimeSpan.Minutes);
                    }
                }
                countTimeSlot();
                DisplayAvg();
                DisplayTimewise();
            }
        }
        private void DisplayAvg()
        {

            int NoofRecords = 0;
            double Timehrs = 0;
            Double SumofHours = 0;
            //string hrs = "";
            for (int i = 0; i < grdMISDetail.Rows.Count; i++)
            {
                if (grdMISDetail.Rows[i].Cells[12].Text != "" && grdMISDetail.Rows[i].Cells[12].Text != "&nbsp;")
                {
                    NoofRecords = NoofRecords + 1;
                    // TimeSpan ts = new TimeSpan(int.Parse(grdMISDetail.Rows[i].Cells[12].Text.Split('.')[0]), int.Parse(grdMISDetail.Rows[i].Cells[12].Text.Split('.')[1]), 0);
                    //SumofHours += (ts.TotalHours);
                    //  var sumhrs = String.Format("{0:00}.{1:00}", ts.TotalHours, ts.Minutes);
                    //  SumofHours += Convert.ToDouble( sumhrs);

                    if (double.TryParse(grdMISDetail.Rows[i].Cells[12].Text, out Timehrs))
                    {
                        SumofHours += Convert.ToDouble(grdMISDetail.Rows[i].Cells[12].Text);
                    }


                    //hours = ts.Hours;
                    //minutes = ts.Minutes;
                    //hoursCpt += hours;
                    //minutesCpt += minutes;




                    //  lbl_Avg.Text = Convert.ToString(hoursCpt + ((minutesCpt - minutesCpt % 60) / 60));
                    //long totalMinutes = 0;
                    //string[] split = grdMISDetail.Rows[i].Cells[12].Text.Split('.');
                    //totalMinutes += (Int64.Parse(split[0]) * 60) + (Int64.Parse(split[1]));
                    //hrs = (String.Format("Total Less Hours = {0:00}:{1:00} (hours/minutes)", totalMinutes / 60, (totalMinutes % 60)));
                    //string[] split = grdMISDetail.Rows[i].Cells[12].Text.Split('.');
                    //    dtHrs.Rows.Add(Int64.Parse(split[0]));
                    //    dtMnts.Rows.Add(Int64.Parse(split[1]));
                    //Int64 Hrs = (Int64)dtHrs.Compute("Sum(HOURS)", string.Empty);
                    //Int64 HrsIntoSeconds = Hrs * 3600;
                    //Int64 Mnts = (Int64)dtMnts.Compute("Sum(MINUTES)", string.Empty);
                    //Int64 MntsIntoSeconds = Mnts * 60;
                    //Int64 TotalSeconds = HrsIntoSeconds + MntsIntoSeconds;
                    //lblTotalLessHrs.Text = String.Format("{0:00}:{1:00}", TotalSeconds / 3600, TotalSeconds / 60 % 60);

                }
            }
            if (NoofRecords > 0)
            {
                // lbl_Avg.Text = "Average time required (HH:MM) : " + ((hoursCpt + ((minutesCpt - minutesCpt % 60) / 60)) / NoofRecords).ToString("00.00");

                lbl_Avg.Text = "Average time required (HH:MM) : ";
                lblAvgres.Text = Convert.ToDouble(SumofHours / NoofRecords).ToString("00.00");
                //  string Diff = String.Format("{0:00}.{1:00}", objTimeSpan.TotalHours, objTimeSpan.Minutes);
                // var ghgf = (String.Format("Total Less Hours = {0:00}:{1:00} (hours/minutes)", lbl_Avg.Text));
            }
            else
            {
                lblAvgres.Text = "";
                lbl_Avg.Text = "Average time required (HH:MM) : ";
            }
        }
        protected void grdMISDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (grdMISDetail.Columns[14].Visible == true)
                {
                    for (int i = 1; i < 17; i++)
                    {
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Black;
                    }
                    e.Row.Cells[1].BackColor = System.Drawing.Color.PapayaWhip;
                    e.Row.Cells[2].BackColor = System.Drawing.Color.PapayaWhip;
                    e.Row.Cells[3].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[4].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[5].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[6].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[7].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[8].BackColor = System.Drawing.Color.MistyRose;
                    e.Row.Cells[9].BackColor = System.Drawing.Color.MistyRose;
                    e.Row.Cells[10].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[11].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[12].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[13].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[14].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[15].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[16].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[2].Text = "Testing Types";
                    e.Row.Cells[3].Text = "Total Enq.";
                    e.Row.Cells[4].Text = "Pending";
                    e.Row.Cells[5].Text = "Approved";
                    e.Row.Cells[6].Text = "Collected";
                    e.Row.Cells[7].Text = "Closed";
                    e.Row.Cells[8].Text = "Inwards";
                    e.Row.Cells[9].Text = "App. Inwards";
                    e.Row.Cells[10].Text = "Total Reports";
                    e.Row.Cells[11].Text = "Entered";
                    e.Row.Cells[12].Text = "Checked";
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (grdMISDetail.Columns[14].Visible == true)
                {
                    e.Row.Cells[2].Width = 400;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Black;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.PapayaWhip;
                    e.Row.Cells[2].BackColor = System.Drawing.Color.PapayaWhip;
                    e.Row.Cells[3].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[4].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[5].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[6].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[7].BackColor = System.Drawing.Color.NavajoWhite;
                    e.Row.Cells[8].BackColor = System.Drawing.Color.MistyRose;
                    e.Row.Cells[9].BackColor = System.Drawing.Color.MistyRose;
                    e.Row.Cells[10].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[11].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[12].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[13].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[14].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[15].BackColor = System.Drawing.Color.Silver;
                    e.Row.Cells[16].BackColor = System.Drawing.Color.Silver;
                }
                else if (e.Row.Cells[11].Text != "" && e.Row.Cells[11].Text != "&nbsp;")
                {
                    DateTime start = DateTime.ParseExact(e.Row.Cells[5].Text, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(e.Row.Cells[11].Text, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    //TimeSpan objTimeSpan = Convert.ToDateTime(e.Row.Cells[11].Text) - Convert.ToDateTime(e.Row.Cells[5].Text);
                    TimeSpan objTimeSpan = end - start;
                    e.Row.Cells[12].Text = String.Format("{0:00}.{1:00}", objTimeSpan.TotalHours, objTimeSpan.Minutes);// Convert.ToInt32(objTimeSpan.TotalHours).ToString(); 
                   
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (grdMISDetail.Columns[14].Visible == true)
                {
                    e.Row.Cells[2].Text = "Total";
                    for (int i = 2; i < 17; i++)
                    {
                        e.Row.Cells[i].BackColor = System.Drawing.Color.OrangeRed;
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }


        protected void lnk_Summary_Click(object sender, EventArgs e)
        {
            grdMISDetail.Visible = true;
            grdMISDetail.Columns[0].Visible = false;
            grdMISDetail.Columns[3].Visible = true;
            grdMISDetail.Columns[13].Visible = true;
            grdMISDetail.Columns[14].Visible = true;
            grdMISDetail.Columns[15].Visible = true;
            grdMISDetail.Columns[16].Visible = true;
            grdMISDetail.Columns[4].Visible = true;
            grdMISDetail.Columns[5].Visible = true;
            grdMISDetail.Columns[6].Visible = true;
            grdMISDetail.Columns[7].Visible = true;
            grdMISDetail.Columns[8].Visible = true;
            grdMISDetail.Columns[9].Visible = true;
            grdMISDetail.Columns[10].Visible = true;
            grdMISDetail.Columns[11].Visible = true;
            lbl_Avg.Visible = false;
            lblAvgres.Visible = false;
            txtEnq.Visible = true;
            lblenq.Visible = true;
            lblInwd.Visible = true;
            txtInwd.Visible = true;
            txtReport.Visible = true;
            lblRpt.Visible = true;
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            grdMISDetail.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drow = null;
            var data = dc.Material_View("", "");
            dtTable.Columns.Add(new DataColumn("MISRecordNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISRecType", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISRefNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISTestType", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISCollectionDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISRecievedDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISEnteredDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISCheckedDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISApprovedDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISPrintedDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MISOutwardDt", typeof(string)));
            foreach (var d in data)
            {
                drow = dtTable.NewRow();
                drow["MISRecordNo"] = "";
                if (Convert.ToString(d.MATERIAL_RecordType_var) == "MF")
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (j == 0)
                        {
                            drow["MISRecType"] = "MDL";
                        }
                        else
                        {
                            drow["MISRecType"] = "Final";
                        }
                        drow["MISRefNo"] = Convert.ToString(d.MATERIAL_Name_var);
                        dtTable.Rows.Add(drow);
                        dtTable.AcceptChanges();
                        drow = dtTable.NewRow();
                        rowIndex++;
                    }
                }
                else
                {
                    drow["MISRecType"] = Convert.ToString(d.MATERIAL_RecordType_var);
                    drow["MISRefNo"] = Convert.ToString(d.MATERIAL_Name_var);
                    dtTable.Rows.Add(drow);
                    dtTable.AcceptChanges();
                    rowIndex++;
                }
            }
            grdMISDetail.DataSource = dtTable;
            grdMISDetail.DataBind();
            lblTotalRecord.Text = "";
            lblTimereqd.Text = "";
            int TotalPending = 0;
            int TotalEnqApprv = 0;
            int TotalCollected = 0;
            int TotalClosed = 0;
            int TotalInwards = 0;
            int TotalInwdAprv = 0;
            int TotalInwdReports = 0;
            int TotalEntered = 0;
            int TotalChecked = 0;
            int TotalApprv = 0;
            int TotalPrint = 0;
            int TotalOutward = 0;
            int TotalPhyOutward = 0;
            int TotalEnq = 0;
            grdMISDetail.FooterRow.Visible = true;
            string MISTestType = string.Empty;
            string MISRecType = string.Empty;
            for (int i = 0; i < grdMISDetail.Rows.Count; i++)
            {
                int ApprvEnq = 0;
                if (grdMISDetail.Rows[i].Cells[1].Text == "MDL" || grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    MISTestType = grdMISDetail.Rows[i].Cells[1].Text;
                    MISRecType = "MF";
                }
                else
                {
                    MISRecType = grdMISDetail.Rows[i].Cells[1].Text;
                    MISTestType = string.Empty;
                }
                if (grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    grdMISDetail.Rows[i].Cells[3].Text = "";
                }
                else
                {
                    var summ = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, true, false, false, false, false, false, false, false, false, false, false, false, false);
                    foreach (var enq in summ)
                    {
                        grdMISDetail.Rows[i].Cells[3].Text = Convert.ToString(enq.Column1);
                    }
                }
                if (grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    grdMISDetail.Rows[i].Cells[4].Text = "";
                }
                else
                {
                    var sum = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, true, false, false, false, false, false, false, false, false, false, false, false);
                    foreach (var enq in sum)
                    {
                        grdMISDetail.Rows[i].Cells[4].Text = Convert.ToString(enq.Column1);
                        ApprvEnq += Convert.ToInt32(grdMISDetail.Rows[i].Cells[4].Text);
                    }
                }
                if (grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    grdMISDetail.Rows[i].Cells[6].Text = "";
                }
                else
                {
                    var s = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, true, false, false, false, false, false, false, false, false, false, false);
                    foreach (var enq in s)
                    {
                        grdMISDetail.Rows[i].Cells[6].Text = Convert.ToString(enq.Column1);
                    }
                }
                if (grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    grdMISDetail.Rows[i].Cells[7].Text = "";
                }
                else
                {
                    var d = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, true, false, false, false, false, false, false, false, false, false);
                    foreach (var enq in d)
                    {
                        grdMISDetail.Rows[i].Cells[7].Text = Convert.ToString(enq.Column1);
                        ApprvEnq += Convert.ToInt32(grdMISDetail.Rows[i].Cells[7].Text);
                    }
                }
                if (grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    grdMISDetail.Rows[i].Cells[8].Text = "";
                }
                else
                {
                    var ds = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, true, false, false, false, false, false, false, false, false);
                    foreach (var enq in ds)
                    {
                        grdMISDetail.Rows[i].Cells[8].Text = Convert.ToString(enq.Column1);
                    }
                }
                if (grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    grdMISDetail.Rows[i].Cells[9].Text = "";
                }
                else
                {
                    var cs = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, true, false, false, false, false, false, false, false);
                    foreach (var enq in cs)
                    {
                        grdMISDetail.Rows[i].Cells[9].Text = Convert.ToString(enq.Column1);
                    }
                }
                if (grdMISDetail.Rows[i].Cells[1].Text == "Final")
                {
                    grdMISDetail.Rows[i].Cells[10].Text = "";
                }
                else
                {
                    var a = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, false, true, false, false, false, false, false, false);
                    foreach (var enq in a)
                    {
                        if (grdMISDetail.Rows[i].Cells[1].Text == "MDL")
                        {
                            grdMISDetail.Rows[i].Cells[10].Text = Convert.ToString(enq.Column1 / 2);
                        }
                        else
                        {
                            grdMISDetail.Rows[i].Cells[10].Text = Convert.ToString(enq.Column1);
                        }
                    }
                }
                var b = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, false, false, true, false, false, false, false, false);
                foreach (var enq in b)
                {
                    grdMISDetail.Rows[i].Cells[11].Text = Convert.ToString(enq.Column1);
                }
                var da = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, false, false, false, true, false, false, false, false);
                foreach (var enq in da)
                {
                    grdMISDetail.Rows[i].Cells[12].Text = Convert.ToString(enq.Column1);
                }
                var b1 = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, false, false, false, false, true, false, false, false);
                foreach (var enq in b1)
                {
                    grdMISDetail.Rows[i].Cells[13].Text = Convert.ToString(enq.Column1);
                }
                var c1 = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, false, false, false, false, false, true, false, false);
                foreach (var enq in c1)
                {
                    grdMISDetail.Rows[i].Cells[14].Text = Convert.ToString(enq.Column1);
                }
                var e1 = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, false, false, false, false, false, false, true, false);
                foreach (var enq in e1)
                {
                    grdMISDetail.Rows[i].Cells[15].Text = Convert.ToString(enq.Column1);
                }
                var g1 = dc.MISSummaryView(Fromdate, Todate, MISRecType, MISTestType, false, false, false, false, false, false, false, false, false, false, false, false, true);
                foreach (var enq in g1)
                {
                    grdMISDetail.Rows[i].Cells[16].Text = Convert.ToString(enq.Column1);
                }
                if (grdMISDetail.Rows[i].Cells[3].Text != "" && grdMISDetail.Rows[i].Cells[3].Text != "&nbsp;")
                {
                    if (Convert.ToInt32(grdMISDetail.Rows[i].Cells[3].Text) > 0)
                    {
                        grdMISDetail.Rows[i].Cells[5].Text = (Convert.ToInt32(grdMISDetail.Rows[i].Cells[3].Text) - (ApprvEnq)).ToString();
                    }
                }
                if (grdMISDetail.Rows[i].Cells[3].Text != "" && grdMISDetail.Rows[i].Cells[3].Text != "&nbsp;")
                {
                    TotalEnq += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[3].Text));
                    grdMISDetail.FooterRow.Cells[3].Text = TotalEnq.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[4].Text != "" && grdMISDetail.Rows[i].Cells[4].Text != "&nbsp;")
                {
                    TotalPending += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[4].Text));
                    grdMISDetail.FooterRow.Cells[4].Text = TotalPending.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[5].Text != "" && grdMISDetail.Rows[i].Cells[5].Text != "&nbsp;")
                {
                    TotalEnqApprv += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[5].Text));
                    grdMISDetail.FooterRow.Cells[5].Text = TotalEnqApprv.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[6].Text != "" && grdMISDetail.Rows[i].Cells[6].Text != "&nbsp;")
                {
                    TotalCollected += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[6].Text));
                    grdMISDetail.FooterRow.Cells[6].Text = TotalCollected.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[7].Text != "" && grdMISDetail.Rows[i].Cells[7].Text != "&nbsp;")
                {
                    TotalClosed += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[7].Text));
                    grdMISDetail.FooterRow.Cells[7].Text = TotalClosed.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[8].Text != "" && grdMISDetail.Rows[i].Cells[8].Text != "&nbsp;")
                {
                    TotalInwards += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[8].Text));
                    grdMISDetail.FooterRow.Cells[8].Text = TotalInwards.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[9].Text != "" && grdMISDetail.Rows[i].Cells[9].Text != "&nbsp;")
                {
                    TotalInwdAprv += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[9].Text));
                    grdMISDetail.FooterRow.Cells[9].Text = TotalInwdAprv.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[10].Text != "" && grdMISDetail.Rows[i].Cells[10].Text != "&nbsp;")
                {
                    TotalInwdReports += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[10].Text));
                    grdMISDetail.FooterRow.Cells[10].Text = TotalInwdReports.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[11].Text != "" && grdMISDetail.Rows[i].Cells[11].Text != "&nbsp;")
                {
                    TotalEntered += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[11].Text));
                    grdMISDetail.FooterRow.Cells[11].Text = TotalEntered.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[12].Text != "" && grdMISDetail.Rows[i].Cells[12].Text != "&nbsp;")
                {
                    TotalChecked += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[12].Text));
                    grdMISDetail.FooterRow.Cells[12].Text = TotalChecked.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[13].Text != "" && grdMISDetail.Rows[i].Cells[13].Text != "&nbsp;")
                {
                    TotalApprv += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[13].Text));
                    grdMISDetail.FooterRow.Cells[13].Text = TotalApprv.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[14].Text != "" && grdMISDetail.Rows[i].Cells[14].Text != "&nbsp;")
                {
                    TotalPrint += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[14].Text));
                    grdMISDetail.FooterRow.Cells[14].Text = TotalPrint.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[15].Text != "" && grdMISDetail.Rows[i].Cells[15].Text != "&nbsp;")
                {
                    TotalOutward += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[15].Text));
                    grdMISDetail.FooterRow.Cells[15].Text = TotalOutward.ToString();
                }
                if (grdMISDetail.Rows[i].Cells[16].Text != "" && grdMISDetail.Rows[i].Cells[16].Text != "&nbsp;")
                {
                    TotalPhyOutward += (Convert.ToInt32(grdMISDetail.Rows[i].Cells[16].Text));
                    grdMISDetail.FooterRow.Cells[16].Text = TotalPhyOutward.ToString();
                }
            }
        }

        private void BindToFilter()
        {
            ddl_To.Items.Clear();
            ddl_To.Items.Insert(0, "---Select---");
            ddl_To.Items.Insert(1, "Collection Date");
            ddl_To.Items.Insert(2, "Recieved Date");
            ddl_To.Items.Insert(3, "Entered Date");
            ddl_To.Items.Insert(4, "Checked Date");
            ddl_To.Items.Insert(5, "Approved Date");
            ddl_To.Items.Insert(6, "Print Date");
            ddl_To.Items.Insert(7, "Outward");
        }
        protected void ddl_StageFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindToFilter();
            if (ddl_StageFrom.SelectedValue != "0")
            {
                for (int i = Convert.ToInt32(ddl_StageFrom.SelectedValue) - 1; i > 0; i--)
                {
                    ddl_To.Items.RemoveAt(i);
                }
            }
        }
        protected void lnk_Print_Click(object sender, EventArgs e)
        {
            if (grdMISDetail.Rows.Count > 0 && grdMISDetail.Visible == true)
            {
                //string reportStr = "";
                //reportStr = RptMISDetails();
                //PrintHTMLReport rptHtml = new PrintHTMLReport();
                //rptHtml.DownloadHtmlReport("MISDetails", reportStr);

                string strTitle = "";
                if (grdMISDetail.Columns[15].Visible != true)
                    strTitle = "MIS Details ";
                else
                    strTitle = "MIS Summary ";
                strTitle += DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy");

                PrintGrid.PrintGridView(grdMISDetail, strTitle, "MISDetails");
            }
            
        }
        protected string RptMISDetails()
        {
            string reportStr = "", mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            if (grdMISDetail.Columns[15].Visible != true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> MIS Details </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> MIS Summary </b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            if (grdMISDetail.Columns[15].Visible != true)
            {
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b>  From Date  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Stage From  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + ddl_StageFrom.SelectedItem.Text + " - " + ddl_To.SelectedItem.Text + "</font></td>" +
               "</tr>";
            }
            else
            {
                mySql += "<tr>" +
                "<td width='15%' align=left valign=top height=19><font size=2><b>  From Date  </b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
                "</tr>";
            }
            if (grdMISDetail.Columns[15].Visible != true)
            {
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Inward Type  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + ddl_InwardType.SelectedItem.Text + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Total No of Records  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + grdMISDetail.Rows.Count + "</font></td>" +
               "</tr>";
            }
            else
            {
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19></td>" +
               "<td width='2%' height=19><font size=2></font></td>" +
               "<td width='40%' height=19><font size=2></font></td>" +
               "</tr>";
            }

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            if (grdMISDetail.Columns[0].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Record No </b></font></td>";
            }
            if (grdMISDetail.Columns[1].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>" + grdMISDetail.HeaderRow.Cells[1].Text + " </b></font></td>";
            }
            if (grdMISDetail.Columns[2].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[2].Text + " </b></font></td>";
            }
            if (grdMISDetail.Columns[3].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> " + grdMISDetail.HeaderRow.Cells[3].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[4].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> " + grdMISDetail.HeaderRow.Cells[4].Text + " </b></font></td>";
            }
            if (grdMISDetail.Columns[5].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> " + grdMISDetail.HeaderRow.Cells[5].Text + " </b></font></td>";
            }
            if (grdMISDetail.Columns[6].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> " + grdMISDetail.HeaderRow.Cells[6].Text + " </b></font></td>";
            }
            if (grdMISDetail.Columns[7].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> " + grdMISDetail.HeaderRow.Cells[7].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[8].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> " + grdMISDetail.HeaderRow.Cells[8].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[9].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[9].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[10].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[10].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[11].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[11].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[12].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[12].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[13].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[13].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[14].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[14].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[15].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[15].Text + "  </b></font></td>";
            }
            if (grdMISDetail.Columns[16].Visible == true)
            {
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  " + grdMISDetail.HeaderRow.Cells[16].Text + "  </b></font></td>";
            }

            for (int i = 0; i < grdMISDetail.Rows.Count; i++)
            {
                mySql += "<tr>";
                if (grdMISDetail.Columns[0].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=middle height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[0].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[1].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=middle height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[1].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[2].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=middle height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[2].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[3].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[3].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[4].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[4].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[5].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[5].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[6].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[6].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[7].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[7].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[8].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[8].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[9].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[9].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[10].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[10].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[11].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[11].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[12].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[12].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[13].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[13].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[14].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[14].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[15].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[15].Text + "</font></td>";
                }
                if (grdMISDetail.Columns[16].Visible == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.Rows[i].Cells[16].Text + "</font></td>";
                }
                mySql += "</tr>";
            }
            if (grdMISDetail.FooterRow.Visible == true)
            {
                mySql += "<tr>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[2].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[3].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[4].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[5].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[6].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[7].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[8].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[9].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[10].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[11].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[12].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[13].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[14].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[15].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdMISDetail.FooterRow.Cells[16].Text + "</font></td>";
                mySql += "</tr>";
            }
            else
            {
                mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2>&nbsp;</font></td>";
                mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> " + lbl_Avg.Text + lblAvgres.Text + " </b></font></td>";
                mySql += "</tr>";
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }


        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }



    }
}