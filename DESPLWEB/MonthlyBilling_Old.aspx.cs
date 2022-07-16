using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace DESPLWEB
{
    public partial class MonthlyBilling_Old : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Monthly Billing";
                txtFromDate.Text = DateTime.Today.AddDays(- DateTime.Today.Day + 1).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day).ToString("dd/MM/yyyy");
                optAll.Checked = true;
                LoadClientList();
                bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_BillPrint_right_bit == true)
                            userRight = true;
                    }
                    if (userRight == false)
                    {
                        pnlContent.Visible = false;
                        lblAccess.Visible = true;
                        lblAccess.Text = "Access is Denied.. ";
                    }
                }
            }
        }

        private void LoadClientList()
        {
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            //var cl = dc.Client_View_MonthlyBilling("", FromDate, ToDate);
            //ddlClient.DataSource = cl;
            //ddlClient.DataTextField = "CL_Name_var";
            //ddlClient.DataValueField = "CL_Id";
            //ddlClient.DataBind();
            //ddlClient.Items.Insert(0, new ListItem("-----Select----", "0"));
        }

        private void LoadSiteList()
        {
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            //var site = dc.Site_View_MonthlyBilling("", Convert.ToInt32(ddlClient.SelectedValue), FromDate, ToDate);
            //ddlSite.DataSource = site;
            //ddlSite.DataTextField = "SITE_Name_var";
            //ddlSite.DataValueField = "SITE_Id";
            //ddlSite.DataBind();
            //ddlSite.Items.Insert(0, new ListItem("-----Select----", "0"));
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSite.Items.Clear();
            if (ddlClient.SelectedIndex > 0)
            {
                LoadSiteList();
            }
            clearData();
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("BillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TestingType", typeof(string)));
            dt.Columns.Add(new DataColumn("Particular", typeof(string)));
            dt.Columns.Add(new DataColumn("ReportNo", typeof(string)));
            dt.Columns.Add(new DataColumn("BillAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PrintStatus", typeof(string)));

            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            int ClientId = 0, SiteId = 0;
            if (chkClientSpecific.Checked == true)
            {
                ClientId = Convert.ToInt32(ddlClient.SelectedValue );
                SiteId = Convert.ToInt32(ddlSite.SelectedValue);
            }
            int srNo = 1;
            string prevBillNo = "0";
            var bill = dc.Bill_View_ClientSiteWise(ClientId, SiteId, FromDate, ToDate).ToList();
            foreach (var bl in bill)
            {
                if (prevBillNo == bl.BILL_Id)
                {
                    dr1["Particular"] = dr1["Particular"] + " , " + bl.BILLD_TEST_Name_var;
                }
                else
                {
                    if (srNo == 1)
                    {
                        dr1 = dt.NewRow();
                    }
                    else
                    {
                        dt.Rows.Add(dr1);
                        dr1 = dt.NewRow();
                    }
                    dr1["SrNo"] = srNo;
                    dr1["BillNo"] = bl.BILL_Id;
                    dr1["BillDate"] = Convert.ToDateTime(bl.BILL_Date_dt).ToString("dd/MM/yyyy");
                    dr1["Particular"] = bl.BILLD_TEST_Name_var;
                    if (bl.BILL_RecordType_var == "---")
                    {
                        dr1["TestingType"] = "Coupon Sale - Cube Testing";
                    }
                    else
                    {
                        dr1["TestingType"] = bl.testType;
                        var refNo = dc.Report_View_MothlyBilling(bl.BILL_RecordType_var, bl.BILL_RecordNo_int);
                        foreach (var rptNo in refNo)
                        {
                            if (dr1["ReportNo"].ToString() == "" )
                                dr1["ReportNo"] = rptNo.ReferenceNo_var;
                            else
                                dr1["ReportNo"] = dr1["ReportNo"].ToString() + "," + rptNo.ReferenceNo_var;
                        }
                    }
                    dr1["BillAmount"] = bl.BILL_NetAmt_num;
                    if (bl.BILL_PrintLock_bit == true)
                        dr1["PrintStatus"] = "Pending";
                    else
                        dr1["PrintStatus"] = "Printed";
                    srNo++;
                }
                prevBillNo = bl.BILL_Id;
            }
            if (bill.Count() > 0)
            {
                dt.Rows.Add(dr1);
            }
            grdBill.DataSource = dt;
            grdBill.DataBind();
            SortByPrintStatus();
            grdBill.Visible = true;
        }
        protected void SortByPrintStatus()
        {
            int SrNo = 1;
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                grdBill.Rows[i].Visible = false;
                if (optAll.Checked == true || 
                    (optPending.Checked == true && grdBill.Rows[i].Cells[7].Text == "Pending") ||
                    (optPrinted.Checked == true && grdBill.Rows[i].Cells[7].Text == "Printed"))
                {
                    grdBill.Rows[i].Visible = true ;
                    grdBill.Rows[i].Cells[0].Text = SrNo.ToString();
                    SrNo++;
                }
            }
        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdBill.Rows.Count > 0 && grdBill.Visible == true)
            {
                BillingSummary_Html();
            }
        }

        protected void optAll_CheckedChanged(object sender, EventArgs e)
        {
            SortByPrintStatus();
        }

        protected void optPrinted_CheckedChanged(object sender, EventArgs e)
        {
            SortByPrintStatus();
        }

        protected void optPending_CheckedChanged(object sender, EventArgs e)
        {
            SortByPrintStatus();
        }

        protected void lnkPrintBill_Click(object sender, EventArgs e)
        {
            if (grdBill.Rows.Count > 0 && grdBill.Visible == true)
            {
                string mySql = "";
                mySql += "<html>";
                mySql += "<head>";
                mySql += "<style type='text/css'>";
                mySql += "body {margin-left:2em margin-right:2em}";
                mySql += "</style>";
                mySql += "</head>";
                int billCount = 0;
                BillUpdation bill = new BillUpdation();
                for (int i = 0; i < grdBill.Rows.Count; i++)
                {
                    if (grdBill.Rows[i].Visible == true)
                    {
                        if (billCount > 0)
                        {
                            mySql += "<DIV style='width:0px;height:0px;page-break-BEFORE:always;'\\></DIV>";
                        }
                        DateTime billDate = DateTime.ParseExact(grdBill.Rows[i].Cells[2].Text, "dd/MM/yyyy", null);
                        if (bill.CheckGSTFlag(billDate) == false)
                        {
                            mySql += bill.getBillString(grdBill.Rows[i].Cells[1].Text, false);
                        }
                        else
                        {
                            mySql += bill.getBillStringWithGST(grdBill.Rows[i].Cells[1].Text, false);
                        }                        
                        billCount++;
                    }
                }
                
                mySql += "</html>";

                for (int i = 0; i < grdBill.Rows.Count; i++)
                {
                    if (grdBill.Rows[i].Visible == true)
                    {
                        dc.Bill_Update_PrintLock(grdBill.Rows[i].Cells[1].Text, false);
                    }
                }

                string reportPath;
                string reportStr = mySql;
                StreamWriter sw;

                string strFileName = "MonthlyBill_" + System.Web.HttpContext.Current.Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".html";
                reportPath = @"C:/temp/" + strFileName;
                sw = File.CreateText(reportPath);
                sw.WriteLine(reportStr);
                sw.Close();
                System.Web.HttpContext.Current.Response.ContentType = "text/HTML";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName);
                System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
                System.Web.HttpContext.Current.Response.End();
            }
        }

        public void BillingSummary_Html()
        {
            string mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> " + "Billing Summary From " + txtFromDate.Text + " To " + txtToDate.Text + " </b></font></td></tr>";
            if (chkClientSpecific.Checked == true)
            {
                if (ddlClient.Items.Count > 0 && ddlClient.SelectedIndex > 0)
                {
                    mySql += "<tr><td width='99%' colspan=7 align=left valign=top height=19><font size=2> " + "Client Name : " + ddlClient.SelectedItem.Text + " </font></td></tr>";
                }
                if (ddlSite.Items.Count > 0 && ddlSite.SelectedIndex > 0)
                {
                    mySql += "<tr><td width='99%' colspan=7 align=left valign=top height=19><font size=2> " + " Site Name : " + ddlSite.SelectedItem.Text + " </font></td></tr>";
                }   
            }
            
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            
            mySql += "<tr>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Sr.No </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Bill No. </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Bill Date </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Test Type </b></font></td>";
            mySql += "<td width= 30% align=center valign=medium height=19 ><font size=2><b> Particular </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Report No </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Bill Amount </b></font></td>";
            mySql += "</tr>";
            int Srno = 1;
            decimal totalAmt = 0;
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                if (grdBill.Rows[i].Visible == true)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2> " + Srno + " </font></td>";
                    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2> " + grdBill.Rows[i].Cells[1].Text + " </font></td>";
                    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2> " + grdBill.Rows[i].Cells[2].Text + " </font></td>";
                    mySql += "<td width= 10% align=left valign=medium height=19 ><font size=2> " + grdBill.Rows[i].Cells[3].Text + " </font></td>";
                    mySql += "<td width= 30% align=left valign=medium height=19 ><font size=2> " + grdBill.Rows[i].Cells[4].Text + " </font></td>";
                    mySql += "<td width= 10% align=left valign=medium height=19 ><font size=2> " + grdBill.Rows[i].Cells[5].Text + " </font></td>";
                    mySql += "<td width= 10% align=right valign=medium height=19 ><font size=2> " + grdBill.Rows[i].Cells[6].Text + " </font></td>";
                    mySql += "</tr>";
                    totalAmt += Convert.ToDecimal(grdBill.Rows[i].Cells[6].Text);
                    Srno++;
                }
            }
            mySql += "<tr>";
            mySql += "<td  colspan='6' align=right valign=medium height=19 ><font size=2> " + "Total Bill Amount" + " </font></td>";
            mySql += "<td  align=right valign=medium height=19 ><font size=2> " + totalAmt.ToString("0.00") + " </font></td>";
            mySql += "</tr>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            
            mySql += "</table>";
            mySql += "</html>";
            PrintHTMLReport rpt = new PrintHTMLReport();
            rpt.DownloadHtmlReport("BillingSummary", mySql);
        }
        
        private void clearData()
        {
            grdBill.DataSource = null;
            grdBill.DataBind();
            grdBill.Visible = false;
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            clearData();
            LoadClientList();
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            clearData();
            LoadClientList();
        }

        protected void chkClientSpecific_CheckedChanged(object sender, EventArgs e)
        {
            clearData();
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearData();
        }

       
    }
}