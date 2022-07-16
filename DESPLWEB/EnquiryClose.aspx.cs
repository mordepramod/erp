using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using AjaxControlToolkit;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace DESPLWEB
{
    public partial class EnquiryClose : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        PrintHTMLReport rpt = new PrintHTMLReport();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentDate();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry Close";
            }
        }
        public void CurrentDate()
        {
            this.txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            this.txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
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
       
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdEnquiryClose.DataSource = null;
            grdEnquiryClose.DataBind();
            LoadEnquiryList();
            lblnoofRecrds.Text = "Total No of Records :  " + grdEnquiryClose.Rows.Count.ToString();
            bool adminRight = false;
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                if (u.USER_EnqApprove_right_bit  == true)
                    adminRight = true;
            }
            if (grdEnquiryClose.Rows.Count > 0)
            {
                for (int i = 0; i < grdEnquiryClose.Rows.Count; i++)
                {
                    LinkButton lnkEnqClose = (LinkButton)grdEnquiryClose.Rows[i].FindControl("lnkEnqClose");
                    lnkEnqClose.Enabled = false;
                    if (adminRight == true)
                    {
                        lnkEnqClose.Enabled = true;
                    }
                }
            }
        }
        protected void lnkEnqClose_Oncommand(object sender, CommandEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = false;
            if (Convert.ToString(e.CommandArgument) != "")
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                TextBox txt_Comment = (TextBox)clickedRow.FindControl("txt_Comment");

                if (txt_Comment.Text != "")
                {
                    int EnqId = Convert.ToInt32(e.CommandArgument);
                    dc.Enquiry_Update_Close(EnqId, 2, txt_Comment.Text);
                    LoadEnquiryList();
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please Enter Comment";
                    txt_Comment.Focus();
                }
            }
        }
        private void LoadEnquiryList()
        {
            grdEnquiryClose.Visible = true;
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Client Name")
            {
                var client = dc.Enquiry_View_Status(txt_Filter.Text.Trim(), "", 0, null, "", Fromdate, Todate, -2);
                grdEnquiryClose.DataSource = client;
                grdEnquiryClose.DataBind();
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Site Name")
            {
                var site = dc.Enquiry_View_Status("", txt_Site.Text.Trim(), 0, null, "", Fromdate, Todate, -2);
                grdEnquiryClose.DataSource = site;
                grdEnquiryClose.DataBind();
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Test Type")
            {
                var testtype = dc.Enquiry_View_Status("", "", 0, null, txt_TestType.Text.Trim(), Fromdate, Todate, -2);
                grdEnquiryClose.DataSource = testtype;
                grdEnquiryClose.DataBind();
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Pending")
            {
                var EnquiryStatus = dc.Enquiry_View_Status("", "", 0, null, "", Fromdate, Todate, 0);
                grdEnquiryClose.DataSource = EnquiryStatus;
                grdEnquiryClose.DataBind();
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Closed")
            {
                var EnquiryStatus = dc.Enquiry_View_Status("", "", 0, null, "", Fromdate, Todate, 1);
                grdEnquiryClose.DataSource = EnquiryStatus;
                grdEnquiryClose.DataBind();
            }
            else
            {
                var cl = dc.Enquiry_View_Status("", "", 0, null, "", Fromdate, Todate, -1);
                grdEnquiryClose.DataSource = cl;
                grdEnquiryClose.DataBind();
            }
        }
        protected void ddl_EnquiryGridColumnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdEnquiryClose.Visible = false;
            txt_Filter.Enabled = true;
            if (ddl_EnquiryGridColumnList.SelectedItem.Text == "All")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = false;
                txt_Filter.Enabled = false;
                txt_Filter.Visible = true;
                txt_Site.Text = string.Empty;
                txt_Filter.Text = string.Empty;
                txt_TestType.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Site Name")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = true;
                txt_Filter.Visible = false;
                txt_Filter.Text = string.Empty;
                txt_TestType.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Client Name")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = false;
                txt_Filter.Visible = true;
                txt_Site.Text = string.Empty;
                txt_TestType.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Test Type")
            {
                txt_Site.Visible = false;
                txt_Filter.Visible = false;
                txt_TestType.Visible = true;
                txt_Site.Text = string.Empty;
                txt_Filter.Text = string.Empty;
            }
            else if (ddl_EnquiryGridColumnList.SelectedItem.Text == "Pending" || ddl_EnquiryGridColumnList.SelectedItem.Text == "Closed")
            {
                txt_TestType.Visible = false;
                txt_Site.Visible = false;
                txt_Filter.Enabled = false;
                txt_Filter.Visible = true;
                txt_Site.Text = string.Empty;
                txt_Filter.Text = string.Empty;
                txt_TestType.Text = string.Empty;
            }
        }
        protected void grdEnquiryClose_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[8].Text != "" && e.Row.Cells[8].Text != "&nbsp;")
                {
                    if (Convert.ToInt32(e.Row.Cells[8].Text) < 2)
                    {
                        e.Row.Cells[8].Text = "Pending";
                    }
                    else if (Convert.ToInt32(e.Row.Cells[8].Text) == 2)
                    {
                        e.Row.Cells[8].Text = "Closed";
                    }
                }
            }
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
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdEnquiryClose.Rows.Count > 0)
            {
                //string reportPath;
                string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                reportStr = RptEnquiryStatus();
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                rpt.DownloadHtmlReport("EnquiryStatus", reportStr);
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
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Enquiry Details </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

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
            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> From Date   </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Total No of Records  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + grdEnquiryClose.Rows.Count + "</font></td>" +
               "</tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Filter For  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + ddl_EnquiryGridColumnList.SelectedItem.Text + txtname + "</font></td>" +
               "</tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";

            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Client Name </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Site Name  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Enquiry No </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Enquiry Date </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Test Type </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Enquiry Type </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Enquiry Status  </b></font></td>";

            for (int i = 0; i < grdEnquiryClose.Rows.Count; i++)
            {
                Label lblClientName = (Label)grdEnquiryClose.Rows[i].Cells[2].FindControl("lblClientName");
                Label lblSiteName = (Label)grdEnquiryClose.Rows[i].Cells[3].FindControl("lblSiteName");
                Label lblEnquiryId = (Label)grdEnquiryClose.Rows[i].Cells[4].FindControl("lblEnquiryId");
                Label lblEnquiryDate = (Label)grdEnquiryClose.Rows[i].Cells[5].FindControl("lblEnquiryDate");
                Label lblMaterialName = (Label)grdEnquiryClose.Rows[i].Cells[6].FindControl("lblMaterialName");
                Label lblOpenEnquiryStatus_var = (Label)grdEnquiryClose.Rows[i].Cells[7].FindControl("lblOpenEnquiryStatus_var");

                mySql += "<tr>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + lblClientName.Text + "</font></td>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + lblSiteName.Text + "</font></td>";
                mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + lblEnquiryId.Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + lblEnquiryDate.Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + lblMaterialName.Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + lblOpenEnquiryStatus_var.Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdEnquiryClose.Rows[i].Cells[8].Text + "</font></td>";
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
    }
}