using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
namespace DESPLWEB
{

    public partial class Enquiry_Status : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        PrintHTMLReport rpt = new PrintHTMLReport();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentDate();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry Status";
            }
        }

        public void CurrentDate()
        {
            txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            LabDataDataContext db = new LabDataDataContext();
            List<string> CL_Name_var = new List<string>();
            var query = db.Client_View(0, 0, prefixText + "%", "");
            foreach (var d in query)
            {
                CL_Name_var.Add(d.CL_Name_var);
            }
            return CL_Name_var;
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSitename(string prefixText)
        {
            LabDataDataContext db = new LabDataDataContext();
            List<string> SITE_Name = new List<string>();
            var query = db.Site_View(0, 0, -1, prefixText + "%");
            foreach (var d in query)
            {
                SITE_Name.Add(d.SITE_Name_var);
            }
            return SITE_Name;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetTestType(string prefixText)
        {
            LabDataDataContext db = new LabDataDataContext();
            List<string> TestName = new List<string>();
            var query = db.Material_View("", prefixText + "%");
            foreach (var d in query)
            {
                TestName.Add(d.MATERIAL_Name_var);
            }
            return TestName;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetEnqStatus(string prefixText)
        {
            LabDataDataContext db = new LabDataDataContext();
            List<string> TestName = new List<string>();
            string[] test = { "Pending", "Closed" };
            foreach (var d in test)
            {
                TestName.Add(d);
            }
            return TestName;
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdEnquiry.DataSource = null;
            grdEnquiry.DataBind();
            LoadEnquiryList();
            lblnoofRecrds.Text = "Total No of Records :  " + grdEnquiry.Rows.Count.ToString();
        }
        private void LoadEnquiryList()
        {
            grdEnquiry.Visible = true;
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
         
            if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Client Name")
            {
                var client = dc.Enquiry_View_Status(txt_Filter.Text.Trim(), "", 0, null, "", Fromdate, Todate, -2);
                grdEnquiry.DataSource = client;
                grdEnquiry.DataBind();
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Site Name")
            {
                var site = dc.Enquiry_View_Status("", txt_Site.Text.Trim(), 0, null, "", Fromdate, Todate, -2);
                grdEnquiry.DataSource = site;
                grdEnquiry.DataBind();
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Test Type")
            {
                var testtype = dc.Enquiry_View_Status("", "", 0, null, txt_TestType.Text.Trim(), Fromdate, Todate, -2);
                grdEnquiry.DataSource = testtype;
                grdEnquiry.DataBind();
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Enquiry Status")
            {
                if (txt_EnqStatus.Text.Equals("Pending", StringComparison.InvariantCultureIgnoreCase) || txt_EnqStatus.Text == "0")
                {
                    var EnquiryStatus = dc.Enquiry_View_Status("", "", 0, null, "", Fromdate, Todate, 0);
                    grdEnquiry.DataSource = EnquiryStatus;
                    grdEnquiry.DataBind();
                }
                if (txt_EnqStatus.Text.Equals("Closed", StringComparison.InvariantCultureIgnoreCase) || txt_EnqStatus.Text == "1")
                {
                    var EnquiryStatus = dc.Enquiry_View_Status("", "", 0, null, "", Fromdate, Todate, 1);
                    grdEnquiry.DataSource = EnquiryStatus;
                    grdEnquiry.DataBind();
                }
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Route Not Assigned")
            {
                var testtype = dc.Enquiry_View_Status("", "", 0, null, "RouteNotAssigned", Fromdate, Todate, -2);
                grdEnquiry.DataSource = testtype;
                grdEnquiry.DataBind();
            }
            else
            {
                var cl = dc.Enquiry_View_Status("", "", 0, null, "", Fromdate, Todate, -1);
                grdEnquiry.DataSource = cl;
                grdEnquiry.DataBind();
            }
        }
        protected void ddl_EnquiryGridColumnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdEnquiry.Visible = false;
            txt_Filter.Enabled = true;
            if (ddl_EnquiryGridColumnList.SelectedItem.Text == "All")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = false;
                txt_Filter.Enabled = false;
                txt_EnqStatus.Visible = false;
                txt_Filter.Visible = true;
                txt_Site.Text = string.Empty;
                txt_Filter.Text = string.Empty;
                txt_TestType.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Site Name")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = true;
                txt_EnqStatus.Visible = false;
                txt_Filter.Visible = false;
                txt_Filter.Text = string.Empty;
                txt_TestType.Text = string.Empty;
                txt_EnqStatus.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Client Name")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = false;
                txt_Filter.Visible = true;
                txt_EnqStatus.Visible = false;
                txt_Site.Text = string.Empty;
                txt_TestType.Text = string.Empty;
                txt_EnqStatus.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Test Type")
            {
                txt_Site.Visible = false;
                txt_Filter.Visible = false;
                txt_EnqStatus.Visible = false;
                txt_TestType.Visible = true;
                txt_Site.Text = string.Empty;
                txt_Filter.Text = string.Empty;
                txt_EnqStatus.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Enquiry Status")
            {
                txt_Site.Visible = false;
                txt_Filter.Visible = false;
                txt_TestType.Visible = false;
                txt_EnqStatus.Visible = true;
                txt_Site.Text = string.Empty;
                txt_Filter.Text = string.Empty;
                txt_TestType.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Route Not Assigned")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = false;
                txt_Filter.Enabled = false;
                txt_EnqStatus.Visible = false;
                txt_Filter.Visible = true;
                txt_Site.Text = string.Empty;
                txt_Filter.Text = string.Empty;
                txt_TestType.Text = string.Empty;
            }
        }

        protected void grdEnquiry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //7- Enquiry status
                //8 - approved date
                //9 - collected on
                //15-17 - bill no
                //16-18 - bill net amount
                //17-19 - outstanding
                if (e.Row.Cells[8].Text != "" && e.Row.Cells[8].Text != "&nbsp;")
                {   
                    if (Convert.ToInt32(e.Row.Cells[8].Text) == 2)
                    {
                        e.Row.Cells[8].Text = "Closed";
                    }
                    else if (((e.Row.Cells[24].Text!="0" && e.Row.Cells[24].Text != "&nbsp;")
                        || (e.Row.Cells[9].Text != "" && e.Row.Cells[9].Text != "&nbsp;")) 
                        && (Convert.ToInt32(e.Row.Cells[8].Text) == 0 || Convert.ToInt32(e.Row.Cells[8].Text) == 1))
                    {
                        e.Row.Cells[8].Text = "Approved";
                    }
                    else if (e.Row.Cells[10].Text != "" && e.Row.Cells[10].Text != "&nbsp;"
                        && (Convert.ToInt32(e.Row.Cells[8].Text) == 1))
                    {
                        e.Row.Cells[8].Text = "Collected";
                    }
                    else //if (Convert.ToInt32(e.Row.Cells[7].Text) < 2)
                    {
                        e.Row.Cells[8].Text = "Pending";
                    }
                }

                if (e.Row.Cells[18].Text != "" && e.Row.Cells[18].Text != "0" && e.Row.Cells[18].Text != "&nbsp;")
                {
                    var billd = dc.CashDetail_View_bill(e.Row.Cells[18].Text);
                    foreach (var bill in billd)
                    {
                        e.Row.Cells[20].Text = bill.receivedAmt.ToString();
                        break;
                    }
                    if (e.Row.Cells[19].Text != "" && e.Row.Cells[20].Text != "" && e.Row.Cells[19].Text != "&nbsp;" && e.Row.Cells[20].Text != "&nbsp;")
                    {
                        e.Row.Cells[20].Text = (Convert.ToDecimal(e.Row.Cells[19].Text) + Convert.ToDecimal(e.Row.Cells[20].Text)).ToString("0.00");
                    }
                    else
                    {
                        e.Row.Cells[20].Text = e.Row.Cells[19].Text;
                    }
                }

                if(e.Row.Cells[24].Text != "0" && e.Row.Cells[24].Text != "")
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#FFCC99");
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdEnquiry.Rows.Count > 0)
            {
                if (chkHtmlPrint.Checked == true)
                {
                    string reportStr = "";
                    reportStr = RptEnquiryStatus();
                    rpt.DownloadHtmlReport("EnquiryStatus", reportStr);
                }
                else
                {
                    string strName = "";
                    if (txt_Filter.Text != "")
                    {
                        strName = txt_Filter.Text;
                    }
                    else if (txt_Site.Text != "")
                    {
                        strName = txt_Site.Text;
                    }
                    else if (txt_TestType.Text != "")
                    {
                        strName = txt_TestType.Text;
                    }
                    else if (txt_EnqStatus.Text != "")
                    {
                        strName = txt_EnqStatus.Text;
                    }
                    string Subheader = "From Date" + " : " + txt_FromDate.Text + "~" + "To Date" + " : " + txt_ToDate.Text + "~" +
                        "Total No of Records" + " : " + grdEnquiry.Rows.Count;
                    if (ddl_EnquiryGridColumnList.SelectedValue != "All")
                    {
                        Subheader += "~" + ddl_EnquiryGridColumnList.SelectedValue + " : " + strName;
                    }
                    PrintGrid.PrintGridView(grdEnquiry, Subheader, "Enquiry_Status");
                }
            }
        }
        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdEnquiry.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdEnquiry.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }

                if (row.Cells[24].Text != "0" && row.Cells[24].Text != "")
                    row.BackColor = System.Drawing.Color.FromName("#FFCC99");
            }
        }
        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = false;
            CheckBox ChkBoxHeader = (CheckBox)grdEnquiry.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdEnquiry.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxRows.Checked == true)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                    break;
                }
               
            }
            foreach (GridViewRow row in grdEnquiry.Rows)
            {
                if (row.Cells[24].Text != "0" && row.Cells[24].Text != "")
                    row.BackColor = System.Drawing.Color.FromName("#FFCC99");
            }
                if (flag)
                ChkBoxHeader.Checked = true;
            else
                ChkBoxHeader.Checked = false;


        }
        protected void lnkView_Oncommand(object sender, CommandEventArgs e)
        {
            if (Convert.ToString(e.CommandArgument) != "")
            {
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
              
                //reportStr = rpt.EnquiryView_Html(Convert.ToInt32(e.CommandArgument));
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                rpt.EnquiryView_Html(Convert.ToInt32(e.CommandArgument));
            }
        }
       
        protected string RptEnquiryStatus()
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
            mySql += "<tr><td width='99%' colspan=8 align=center valign=top height=19><font size=4><b> Enquiry Status </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=8>&nbsp;</td></tr>";

            string txtname = "";
            if (txt_Filter.Text != "")
            {
                txtname = " = " + txt_Filter.Text;
            }
            else if (txt_Site.Text != "")
            {
                txtname = " = " + txt_Site.Text;
            }
            else if (txt_TestType.Text != "")
            {
                txtname = " = " + txt_TestType.Text;
            }
            else if (txt_EnqStatus.Text != "")
            {
                txtname = " = " + txt_EnqStatus.Text;
            }

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> From Date   </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Total No of Records  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + grdEnquiry.Rows.Count + "</font></td>" +
               "</tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Filter For  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + ddl_EnquiryGridColumnList.SelectedItem.Text + txtname + "</font></td>" +
               "</tr>";

            mySql += "<tr><td width='99%' colspan=8>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            
            //mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Client Name </b></font></td>";
            //mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Site Name  </b></font></td>";
            //mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Enquiry No </b></font></td>";
            //mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Enquiry Date </b></font></td>";
            //mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Test Type </b></font></td>";
            //mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Enquiry Type </b></font></td>";
            //mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Enquiry Status  </b></font></td>";
            //mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Entered by  </b></font></td>";

            for (int i = 1; i < grdEnquiry.Columns.Count; i++)
            {
                mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>  " + grdEnquiry.HeaderRow.Cells[i].Text + " </b></font></td>";
            }
            mySql += "</tr>";
            for (int i = 0; i < grdEnquiry.Rows.Count; i++)
            {
                //Label lblClientName = (Label)grdEnquiry.Rows[i].Cells[1].FindControl("lblClientName");
                //Label lblSiteName = (Label)grdEnquiry.Rows[i].Cells[2].FindControl("lblSiteName");
                //Label lblEnquiryId = (Label)grdEnquiry.Rows[i].Cells[3].FindControl("lblEnquiryId");
                //Label lblEnquiryDate = (Label)grdEnquiry.Rows[i].Cells[4].FindControl("lblEnquiryDate");
                //Label lblMaterialName = (Label)grdEnquiry.Rows[i].Cells[5].FindControl("lblMaterialName");
                //Label lblOpenEnquiryStatus_var = (Label)grdEnquiry.Rows[i].Cells[6].FindControl("lblOpenEnquiryStatus_var");

                //mySql += "<tr>";
                //mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + lblClientName.Text + "</font></td>";
                //mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + lblSiteName.Text + "</font></td>";
                //mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + lblEnquiryId.Text + "</font></td>";
                //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + lblEnquiryDate.Text + "</font></td>";
                //mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + lblMaterialName.Text + "</font></td>";
                //mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + lblOpenEnquiryStatus_var.Text + "</font></td>";
                //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdEnquiry.Rows[i].Cells[7].Text + "</font></td>";
                //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdEnquiry.Rows[i].Cells[8].Text + "</font></td>";
                //mySql += "</tr>";
                mySql += "<tr>";
                for (int j = 1; j < grdEnquiry.Columns.Count; j++)
                {
                    mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + grdEnquiry.Rows[i].Cells[j].Text + "</font></td>";
                }
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

        protected void lnkPrintTestRequest_Click(object sender, EventArgs e)
        {
            if(grdEnquiry.Rows.Count>0)
            {
                List<string> lstEnqNo = new List<string>();
                PrintPDFReport rpt = new PrintPDFReport();
                string mobEnqNo = ""; //enqNo = "",
                //bool valid = false;
                for (int i = 0; i < grdEnquiry.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdEnquiry.Rows[i].Cells[0].FindControl("chkSelect");
                    mobEnqNo = grdEnquiry.Rows[i].Cells[24].Text.ToString();
                    if (chkSelect.Checked && chkSelect.Enabled == true && mobEnqNo != "" && mobEnqNo != "0")
                    {
                        lstEnqNo.Add(mobEnqNo);
                    }

                }

                //for (int i = 0; i < grdEnquiry.Rows.Count; i++)
                //{
                //    CheckBox cbxSelect = (CheckBox)grdEnquiry.Rows[i].Cells[0].FindControl("cbxSelect");
                //    if (cbxSelect.Checked)
                //        cbxSelect.Checked = false;
                //}

                if(lstEnqNo.Count>0)
                 rpt.AppEnqTestRequestFormPrint(lstEnqNo.Distinct().ToList());

          

            }

        }
    }
}