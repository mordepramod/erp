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
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;

namespace DESPLWEB
{
    public partial class SiteAssignment : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Site Assignment Updation";
                }
                txt_Client.Focus();
                bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_Admin_right_bit == true || u.USER_CS_right_bit == true)
                        {
                            userRight = true;
                        }
                        if (u.USER_SuperAdmin_right_bit == true || u.USER_Name_var.ToLower().Contains("sagvekar"))
                        {
                            lblSuperAdminRight.Text = "true";
                        }
                    }
                }
                if (userRight == false)
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }

        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, 0, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            lnkSave.Enabled = true;
            DisplaySiteDtl();
            lblRec.Text = "Total No. of Records : " + grdSiteAssignment.Rows.Count;
        }

        private void DisplaySiteDtl()
        {
            grdSiteAssignment.DataSource = null;
            grdSiteAssignment.DataBind();
            try
            {
                DataTable dt = new DataTable();
                int rowIndex = 0;
                DataRow drow = null;
                dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
                dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
                dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("SITE_CL_Id", typeof(string)));
                var data = dc.Client_View(0, 0, txt_Client.Text + "%", "");
                foreach (var s in data)
                {
                    var site = dc.Site_View(0, Convert.ToInt32(s.CL_Id), -1, "");
                    foreach (var cs in site)
                    {
                        drow = dt.NewRow();
                        drow["CL_Name_var"] = Convert.ToString(s.CL_Name_var);
                        drow["SITE_Name_var"] = Convert.ToString(cs.SITE_Name_var);
                        drow["SITE_Id"] = Convert.ToString(cs.SITE_Id);
                        drow["SITE_CL_Id"] = Convert.ToString(cs.SITE_CL_Id);
                        dt.Rows.Add(drow);
                        dt.AcceptChanges();
                        rowIndex++;
                    }
                }
                grdSiteAssignment.DataSource = dt;
                grdSiteAssignment.DataBind();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                for (int i = 0; i < grdSiteAssignment.Rows.Count; i++)
                {
                    Label lblSiteId = (Label)grdSiteAssignment.Rows[i].Cells[3].FindControl("lblSiteId");
                    DropDownList ddlRegion = (DropDownList)grdSiteAssignment.Rows[i].Cells[5].FindControl("ddlRegion");
                    DropDownList ddl_ME = (DropDownList)grdSiteAssignment.Rows[i].Cells[6].FindControl("ddl_ME");
                    DropDownList ddl_CE = (DropDownList)grdSiteAssignment.Rows[i].Cells[7].FindControl("ddl_CE");
                    CheckBox chk_MnthBilling = (CheckBox)grdSiteAssignment.Rows[i].Cells[8].FindControl("chk_MnthBilling");
                    CheckBox chk_RABill = (CheckBox)grdSiteAssignment.Rows[i].Cells[9].FindControl("chk_RABill");
                    CheckBox chk_Tassco = (CheckBox)grdSiteAssignment.Rows[i].Cells[10].FindControl("chk_Tassco");

                    if (lblSiteId.Text == "")
                    {
                        lblSiteId.Text = "0";
                    }
                    if (Convert.ToInt32(lblSiteId.Text) > 0)
                    {
                        var site = dc.Site_View(Convert.ToInt32(lblSiteId.Text), 0, -1, "");
                        foreach (var cs in site)
                        {
                            ddlRegion.SelectedValue = Convert.ToString(cs.SITE_REGION_Id_int);
                            ddl_ME.SelectedValue = Convert.ToString(cs.SITE_MEID_int);
                            ddl_CE.SelectedValue = Convert.ToString(cs.SITE_CollectionId_int);
                            if (cs.SITE_MonthlyBillingStatus_bit == true)
                            {
                                chk_MnthBilling.Checked = true;
                            }
                            if (cs.SITE_RABillStatus_bit == true)
                            {
                                chk_RABill.Checked = true;
                            }
                            if (cs.SITE_TasscoStatus_bit == true)
                            {
                                chk_Tassco.Checked = true;
                            }
                        }
                    }
                    if (lblSuperAdminRight.Text != "true")
                    {
                        chk_MnthBilling.Enabled = false;
                    }
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdSiteAssignment.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdSiteAssignment.Rows[i].Cells[0].FindControl("cbxSelect");
                Label lblClientName = (Label)grdSiteAssignment.Rows[i].Cells[1].FindControl("lblClientName");
                Label lblSiteName = (Label)grdSiteAssignment.Rows[i].Cells[2].FindControl("lblSiteName");
                Label lblSiteId = (Label)grdSiteAssignment.Rows[i].Cells[3].FindControl("lblSiteId");
                Label lblClId = (Label)grdSiteAssignment.Rows[i].Cells[4].FindControl("lblClId");
                DropDownList ddlRegion = (DropDownList)grdSiteAssignment.Rows[i].Cells[5].FindControl("ddlRegion");
                DropDownList ddl_ME = (DropDownList)grdSiteAssignment.Rows[i].Cells[6].FindControl("ddl_ME");
                DropDownList ddl_CE = (DropDownList)grdSiteAssignment.Rows[i].Cells[7].FindControl("ddl_CE");
                CheckBox chk_MnthBilling = (CheckBox)grdSiteAssignment.Rows[i].Cells[8].FindControl("chk_MnthBilling");
                CheckBox chk_RABill = (CheckBox)grdSiteAssignment.Rows[i].Cells[9].FindControl("chk_RABill");
                CheckBox chk_Tassco = (CheckBox)grdSiteAssignment.Rows[i].Cells[10].FindControl("chk_Tassco");
                if (cbxSelect.Checked)
                {
                    dc.SiteAssignment_Update(Convert.ToInt32(lblSiteId.Text), Convert.ToInt32(lblClId.Text), Convert.ToInt32(ddlRegion.SelectedValue), Convert.ToInt32(ddl_ME.SelectedValue), Convert.ToInt32(ddl_CE.SelectedValue), Convert.ToBoolean(chk_MnthBilling.Checked), Convert.ToBoolean(chk_RABill.Checked), Convert.ToBoolean(chk_Tassco.Checked));
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Records updated Successfully";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lnkSave.Enabled = false;
                }
            }
        }


        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Binding DDL CE
                DropDownList ddl_CE = (DropDownList)e.Row.FindControl("ddl_CE");
                var CE = dc.User_View(0, -1, "", "Collection", "");
                ddl_CE.DataSource = CE;
                ddl_CE.DataTextField = "USER_Name_var";
                ddl_CE.DataValueField = "USER_Id";
                ddl_CE.DataBind();
                ddl_CE.Items.Insert(0, new ListItem("---Select---", "0"));

                //Binding DDL ME
                DropDownList ddl_ME = (DropDownList)e.Row.FindControl("ddl_ME");
                var data = dc.User_View(0, -1, "", "Marketing", "");
                ddl_ME.DataSource = data;
                ddl_ME.DataTextField = "USER_Name_var";
                ddl_ME.DataValueField = "USER_Id";
                ddl_ME.DataBind();
                ddl_ME.Items.Insert(0, new ListItem("---Select---", "0"));

                //Binding DDL Region
                DropDownList ddlRegion = (DropDownList)e.Row.FindControl("ddlRegion");
                var region = dc.getRegion();
                ddlRegion.DataSource = region;
                ddlRegion.DataTextField = "REGION_Name_var";
                ddlRegion.DataValueField = "REGION_Id";
                ddlRegion.DataBind();
                ddlRegion.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
        protected void grdSiteAssignment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox cbxSelect = (CheckBox)e.Row.Cells[2].FindControl("cbxSelect");
                CheckBox cbxSelectAll = (CheckBox)this.grdSiteAssignment.HeaderRow.FindControl("cbxSelectAll");
                cbxSelect.Attributes["onclick"] = string.Format
                                                       (
                                                          "javascript:ChildClick(this,'{0}');",
                                                          cbxSelectAll.ClientID
                                                       );
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
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdSiteAssignment.Rows.Count > 0)
            {
                //string reportPath;
                string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                reportStr = RptSiteAssignment();
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                PrintHTMLReport rpt = new PrintHTMLReport();
                rpt.DownloadHtmlReport("SiteAssignment", reportStr);
            }
        }

        protected string RptSiteAssignment()
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
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Site Assignment </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            if (txt_Client.Text != "")
            {
                mySql += "<tr>" +
                   "<td width='15%' align=left valign=top height=19><font size=2><b>Client Name</b></font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='40%' height=19><font size=2>" + txt_Client.Text + "</font></td>" +
                   "<td width='15%' height=19><font size=2><b>Total No of Records</b></font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='10%' height=19><font size=2>" + grdSiteAssignment.Rows.Count + "</font></td>" +
                   "</tr>";
            }
            else
            {
                mySql += "<tr>" +
                   "<td width='15%' align=left valign=top height=19><font size=2><b>Client Name</b></font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='40%' height=19><font size=2>" + " All  " + "</font></td>" +
                   "<td width='15%' height=19><font size=2><b>Total No of Records</b></font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='10%' height=19><font size=2>" + grdSiteAssignment.Rows.Count + "</font></td>" +
                   "</tr>";
            }

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";

            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Client  </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Site Name</b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Region</b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>ME </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>CE </b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Monthly Billing </b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>RA Bill </b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Tassco </b></font></td>";

            for (int i = 0; i < grdSiteAssignment.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdSiteAssignment.Rows[i].Cells[0].FindControl("cbxSelect");
                Label lblClientName = (Label)grdSiteAssignment.Rows[i].Cells[1].FindControl("lblClientName");
                Label lblSiteName = (Label)grdSiteAssignment.Rows[i].Cells[2].FindControl("lblSiteName");
                Label lblSiteId = (Label)grdSiteAssignment.Rows[i].Cells[3].FindControl("lblSiteId");
                Label lblClId = (Label)grdSiteAssignment.Rows[i].Cells[4].FindControl("lblClId");
                DropDownList ddlRegion = (DropDownList)grdSiteAssignment.Rows[i].Cells[5].FindControl("ddlRegion");
                DropDownList ddl_ME = (DropDownList)grdSiteAssignment.Rows[i].Cells[6].FindControl("ddl_ME");
                DropDownList ddl_CE = (DropDownList)grdSiteAssignment.Rows[i].Cells[7].FindControl("ddl_CE");
                CheckBox chk_MnthBilling = (CheckBox)grdSiteAssignment.Rows[i].Cells[8].FindControl("chk_MnthBilling");
                CheckBox chk_RABill = (CheckBox)grdSiteAssignment.Rows[i].Cells[9].FindControl("chk_RABill");
                CheckBox chk_Tassco = (CheckBox)grdSiteAssignment.Rows[i].Cells[10].FindControl("chk_Tassco");

                string Chk = string.Empty;
                mySql += "<tr>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + lblClientName.Text + "</font></td>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + lblSiteName.Text + "</font></td>";
                mySql += "<td width =10% align=center valign=top height=19 ><font size=2>&nbsp;" + ddlRegion.SelectedItem.Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + ddl_ME.SelectedItem.Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + ddl_CE.SelectedItem.Text + "</font></td>";
                if (chk_MnthBilling.Checked)
                {
                    Chk = "True";
                }
                else
                {
                    Chk = "False";
                }
                mySql += "<td width=2% align=center valign=top height=19 ><font size=2>&nbsp;" + Chk + "</font></td>";

                Chk = string.Empty;
                if (chk_RABill.Checked)
                {
                    Chk = "True";
                }
                else
                {
                    Chk = "False";
                }
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Chk + "</font></td>";

                Chk = string.Empty;
                if (chk_Tassco.Checked)
                {
                    Chk = "True";
                }
                else
                {
                    Chk = "False";
                }
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Chk + "</font></td>";

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
