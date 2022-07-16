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
using System.Collections.Generic;

namespace DESPLWEB
{
    public partial class SiteVerify : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Verify Site";
                FirstGridViewRowOfSite();
            }
        }

        protected void LoadSiteList()
        {
            var site = dc.Client_View_App(-2);
            grdSite.DataSource = site;
            grdSite.DataBind();

            if (grdSite.Rows.Count <= 0)
            {
                FirstGridViewRowOfSite();
            }
            else
            {
                lblTotalRecords.Text = "Total No of Records : " + grdSite.Rows.Count;
            }
            grdSite.Visible = true;
            lblTotalRecords.Visible = true;
        }

        protected void lnkFetchSiteFromApp_Click(object sender, EventArgs e)
        {
            LoadSiteList();
        }
        protected void lnkDisableSite(object sender, CommandEventArgs e)
        {
            string strMsg = "";
            string Idsplit = Convert.ToString(e.CommandArgument);
            string[] arg = new string[2];
            arg = Idsplit.Split(';');
            if (Convert.ToString(arg[0]) != "")
            {
                dc.Client_Update_DisableStatus(Convert.ToInt32(arg[0]));
                strMsg = "Status Updated Successfully.";
                   
            }
            if (strMsg != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
            LoadSiteList();
        }
        protected void lnkApproveSite(object sender, CommandEventArgs e)
        {
            string Idsplit = Convert.ToString(e.CommandArgument);
            string[] arg = new string[2];
            arg = Idsplit.Split(';');
            if (Convert.ToString(arg[0]) != "")
            {
                LinkButton lnkApproveClient = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)lnkApproveClient.Parent.Parent;
                Label lblClientId = (Label)gvRow.FindControl("lblClientId");
                Label lblSiteId = (Label)gvRow.FindControl("lblSiteId");

                Label lblClientIdNew = (Label)gvRow.FindControl("lblClientIdNew");
                Label lblClientAddress = (Label)gvRow.FindControl("lblClientAddress");
                Label lblClientMobileNo = (Label)gvRow.FindControl("lblClientMobileNo");
                Label lblSiteIdNew = (Label)gvRow.FindControl("lblSiteIdNew");
                Label lblSiteAddress = (Label)gvRow.FindControl("lblSiteAddress");
                Label lblSiteMobileNo = (Label)gvRow.FindControl("lblSiteMobileNo");

                string strMsg = "";
                if (lblClientId.Text == null || lblClientId.Text == "" || lblClientId.Text == "0")
                {
                    strMsg = "Select Client from list.";
                }
                else if (lblSiteId.Text == null || lblSiteId.Text == "" || lblSiteId.Text == "0")
                {
                    strMsg = "Select Site from list.";
                }
                else
                {
                    dc.Client_Update_VerifyApp(Convert.ToInt32(lblClientId.Text), lblClientAddress.Text, lblClientMobileNo.Text, Convert.ToInt32(lblSiteId.Text), lblSiteAddress.Text, lblSiteMobileNo.Text, Convert.ToInt32(lblClientIdNew.Text), Convert.ToInt32(lblSiteIdNew.Text));

                    clsSendMail objcls = new clsSendMail();
                    var cl = dc.Client_View(Convert.ToInt32(lblClientId.Text), 0, "", "");
                    //string strSms = "Dear customer your registration with Durocrete is approved. Your password for mobile number " + lblClientMobileNo.Text + " is " + cl.FirstOrDefault().CL_Password_var;
                    string strSms = "Dear Customer , your password for Durocrete Client App is " + cl.FirstOrDefault().CL_Password_var;
                    objcls.sendSMS(lblClientMobileNo.Text, strSms, "DUROCR","");

                    strMsg = "Approved Successfully.";
                    LoadSiteList();
                }
                if (strMsg != "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
                }
            }
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void FirstGridViewRowOfSite()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_OfficeAddress_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_mobile_No", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Address_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_mobile_No", typeof(string)));
            dr = dt.NewRow();
            dr["CL_Id"] = string.Empty;
            dr["CL_Name_var"] = string.Empty;
            dr["CL_OfficeAddress_var"] = string.Empty;
            dr["CL_mobile_No"] = string.Empty;
            dr["SITE_Id"] = string.Empty;
            dr["SITE_Name_var"] = string.Empty;
            dr["SITE_Address_var"] = string.Empty;
            dr["SITE_mobile_No"] = string.Empty;
            dt.Rows.Add(dr);
            grdSite.DataSource = dt;
            grdSite.DataBind();
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txt_Client = (TextBox)gvRow.FindControl("txt_Client");
            TextBox txt_Site = (TextBox)gvRow.FindControl("txt_Site");
            Label lblClientId = (Label)gvRow.FindControl("lblClientId");
            Label lblSiteId = (Label)gvRow.FindControl("lblSiteId");
            
            txt_Site.Text = string.Empty;
            lblSiteId.Text = string.Empty;
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                    lblClientId.Text = Session["CL_ID"].ToString();
                }
                else
                {
                    Session["CL_ID"] = 0;
                    lblClientId.Text = "0";
                }
                txt_Site.Focus();
              
            }
        }
        protected void txt_Site_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txt_Site = (TextBox)gvRow.FindControl("txt_Site");
            Label lblSiteId = (Label)gvRow.FindControl("lblSiteId");
            Label lblClientId = (Label)gvRow.FindControl("lblClientId");

            int cl_Id = 0;
            //if (Convert.ToInt32(Session["CL_ID"]) > 0)
            if (Convert.ToInt32(lblClientId.Text) > 0)
            {
                if (int.TryParse(lblClientId.Text, out cl_Id))
                {
                    if (ChkSiteName(txt_Site.Text) == true)
                    {
                        int SiteId = 0;
                        if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                        {
                            Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                            lblSiteId.Text = Session["SITE_ID"].ToString();
                        }
                        else
                        {
                            Session["SITE_ID"] = 0;
                            lblSiteId.Text = "0";
                        }
                        
                    }
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
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSitename(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            DataRow row = null;
            DataTable dt = new DataTable();
            if (Convert.ToInt32(HttpContext.Current.Session["CL_ID"]) > 0)
            {
                int ClientId = 0;
                if (int.TryParse(HttpContext.Current.Session["CL_ID"].ToString(), out ClientId))
                {

                    var res = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, searchHead);
                    dt.Columns.Add("SITE_Name_var");
                    foreach (var obj in res)
                    {
                        row = dt.NewRow();
                        string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                        dt.Rows.Add(listitem);
                    }
                    if (row == null)
                    {
                        var resclnm = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, "");
                        foreach (var obj in resclnm)
                        {
                            row = dt.NewRow();
                            string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                            dt.Rows.Add(listitem);
                        }
                    }
                }
            }
            List<string> SITE_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SITE_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return SITE_Name_var;
        }
        protected Boolean ChkClientName(string searchHead)
        {   
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            //searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, -1, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                //txt_Client.Focus();
                //lblMsg.Visible = true;
                //lblMsg.Text = "This Client is not in the list";
                Session["CL_ID"] = 0;
                Session["SITE_ID"] = 0;
            }
            else
            {
                //lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }
        protected Boolean ChkSiteName(string searchHead)
        {
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            //searchHead = txt_Site.Text;
            Boolean valid = false;
            var res = dc.Site_View(0, Convert.ToInt32(Session["CL_ID"]), -1, searchHead);
            foreach (var obj in res)
            {
                valid = true;
            }
            //if (valid == false)
            //{
            //    txt_Site.Focus();
            //    lblMsg.Visible = true;
            //    lblMsg.Text = "This Site Name is not in the list ";
            //}
            //else
            //{
            //    lblMsg.Visible = false;
            //    valid = true;
            //}
            return valid;
        }

        
    }
}
