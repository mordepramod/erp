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
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class ClientVerify : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "App Login Approval";
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
                        if (u.USER_AllEnqApproval_right_bit == true || u.USER_CS_right_bit == true)
                        {
                            userRight = true;
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

        protected void LoadClientSiteList()
        {

            var client = dc.Client_View_App(-1);
            grdClientSite.DataSource = client;
            grdClientSite.DataBind();
            grdClientSite.Columns[0].Visible = true;
            grdClientSite.Columns[1].Visible = true;
            grdClientSite.Columns[2].Visible = true;
            grdClientSite.Columns[7].Visible = true;
            grdClientSite.Columns[9].Visible = true;
            grdClientSite.Columns[10].Visible = true;
            grdClientSite.Columns[11].Visible = true;
            grdClientSite.Columns[12].Visible = true;
            grdClientSite.Columns[13].Visible = false;
            grdClientSite.Columns[14].Visible = false;

            if (grdClientSite.Rows.Count <= 0)
            {
                FirstGridViewRowOfClientSite();
                lblTotalRecords.Text = "Total No of Records : 0";
            }
            else
            {
                lblTotalRecords.Text = "Total No of Records : " + grdClientSite.Rows.Count;
            }
            grdClientSite.Visible = true;
            lblTotalRecords.Visible = true;


        }
        protected void Loaddefault()
        {

            var client = dc.DeviceLogin_View("-1");
            grdClientSite.DataSource = client;
            grdClientSite.DataBind();

            grdClientSite.Columns[0].Visible = false;
            grdClientSite.Columns[1].Visible = false;
            grdClientSite.Columns[2].Visible = false;
            grdClientSite.Columns[7].Visible = false;
            grdClientSite.Columns[9].Visible = false;
            grdClientSite.Columns[10].Visible = false;
            grdClientSite.Columns[11].Visible = false;
            grdClientSite.Columns[12].Visible = false;
            grdClientSite.Columns[13].Visible = true;
            grdClientSite.Columns[14].Visible = true;


            if (grdClientSite.Rows.Count <= 0)
            {
                FirstGridViewRowOfClientSite();
                lblTotalRecords.Text = "Total No of Records : 0";
            }
            else
            {
                lblTotalRecords.Text = "Total No of Records : " + grdClientSite.Rows.Count;
            }
            grdClientSite.Visible = true;
            lblTotalRecords.Visible = true;

        }

        protected void grdClientSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblClientMobileNo = (Label)e.Row.FindControl("lblClientMobileNo");
                Label lblSiteMobileNo = (Label)e.Row.FindControl("lblSiteMobileNo");
                if (lblClientMobileNo.Text != "")
                {
                    var loginFound = dc.DeviceLogin_View(lblClientMobileNo.Text);
                    if (loginFound.Count() > 0)
                    {
                        e.Row.Cells[5].BackColor = System.Drawing.Color.LightPink;
                    }
                }
                if (lblSiteMobileNo.Text != "")
                {
                    var stloginFound = dc.DeviceLogin_View(lblSiteMobileNo.Text);
                    if (stloginFound.Count() > 0)
                    {
                        e.Row.Cells[10].BackColor = System.Drawing.Color.LightPink;
                    }
                }
            }
        }

        protected void lnkFetchClientFromApp_Click(object sender, EventArgs e)
        {
            LoadClientSiteList();
        }


        protected void lnkDisableClient(object sender, CommandEventArgs e)
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
            LoadClientSiteList();
        }
        protected void lnkFetchdefaultDetails_Click(object sender, EventArgs e)
        {
            Loaddefault();
        }
        protected void lnkApproveClient(object sender, CommandEventArgs e)
        {
            string Idsplit = Convert.ToString(e.CommandArgument);
            string[] arg = new string[2];
            arg = Idsplit.Split(';');
            string cntctPerson = "", conctNo = "", conctEmail = "";
            if (Convert.ToString(arg[0]) != "")
            {
                LinkButton lnkApproveClient = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)lnkApproveClient.Parent.Parent;
                Label lblClientId = (Label)gvRow.FindControl("lblClientId");
                //Label lblSiteId = (Label)gvRow.FindControl("lblSiteId");

                Label lblClientIdNew = (Label)gvRow.FindControl("lblClientIdNew");
                Label lblClientName = (Label)gvRow.FindControl("lblClientName");
                Label lblClientMobileNo = (Label)gvRow.FindControl("lblClientMobileNo");
                Label lblClientEmailId = (Label)gvRow.FindControl("lblClientEmailId");
                //Label lblSiteIdNew = (Label)gvRow.FindControl("lblSiteIdNew");
                //Label lblSiteAddress = (Label)gvRow.FindControl("lblSiteAddress");
                //Label lblSiteMobileNo = (Label)gvRow.FindControl("lblSiteMobileNo");

                string strMsg = "";
                if (lblClientId.Text == null || lblClientId.Text == "" || lblClientId.Text == "0")
                {
                    strMsg = "Select Client from list.";
                }
                //else if (lblSiteId.Text == null || lblSiteId.Text == "" || lblSiteId.Text == "0")
                //{
                //    strMsg = "Select Site from list.";
                //}
                else if (lblClientMobileNo.Text == null || lblClientMobileNo.Text == "")
                {
                    strMsg = "Client Mobile No. not available.";
                }
                else
                {
                    string strPassword = "cl" + (Convert.ToInt32(lblClientId.Text) + 10).ToString();
                    //dc.Client_Update_VerifyApp(Convert.ToInt32(lblClientId.Text), lblClientAddress.Text, lblClientMobileNo.Text, Convert.ToInt32(lblSiteId.Text), lblSiteAddress.Text, lblSiteMobileNo.Text, Convert.ToInt32(lblClientIdNew.Text), Convert.ToInt32(lblSiteIdNew.Text));
                    dc.Client_Update_VerifyApp(Convert.ToInt32(lblClientId.Text), lblClientMobileNo.Text, lblClientEmailId.Text, 0, "", "", Convert.ToInt32(lblClientIdNew.Text), 0);

                    string contctDetails = getConctDetails(lblClientMobileNo.Text);
                    if (contctDetails != "")
                    {
                        string[] arrContct = contctDetails.Split('|');
                        cntctPerson = arrContct[0];
                        conctNo = arrContct[1];
                        conctEmail = arrContct[2];
                    }
                    else
                    {
                        cntctPerson = lblClientName.Text;
                        conctNo = lblClientMobileNo.Text;
                        conctEmail = lblClientEmailId.Text;
                    }
                    dc.DeviceLogin_Update(Convert.ToInt32(lblClientId.Text), 0, lblClientMobileNo.Text, strPassword, false, cntctPerson, conctNo, conctEmail, false, 0, null, false);

                    clsSendMail objcls = new clsSendMail();
                    var cl = dc.Client_View(Convert.ToInt32(lblClientId.Text), 0, "", "");
                    //string strSms = "Dear customer your registration with Durocrete is approved. Your password for mobile number " + lblClientMobileNo.Text + " is " + cl.FirstOrDefault().CL_Password_var;
                    //string strSms = "Dear Customer , your password for Durocrete Client App is " + cl.FirstOrDefault().CL_Password_var;
                    string strSms = "Dear Customer , your password for Durocrete Client App is " + strPassword;
                    objcls.sendSMS(lblClientMobileNo.Text, strSms, "DUROCR","");

                    strMsg = "Approved Successfully.";
                    LoadClientSiteList();
                    //}
                }
                if (strMsg != "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
                }
            }
        }

        protected void lnkApproveSite(object sender, CommandEventArgs e)
        {
            string Idsplit = Convert.ToString(e.CommandArgument);
            string[] arg = new string[2];
            arg = Idsplit.Split(';');
            string cntctPerson = "", conctNo = "", conctEmail = "";
            if (Convert.ToString(arg[0]) != "")
            {
                LinkButton lnkApproveClient = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)lnkApproveClient.Parent.Parent;
                Label lblClientId = (Label)gvRow.FindControl("lblClientId");
                Label lblSiteId = (Label)gvRow.FindControl("lblSiteId");

                Label lblClientIdNew = (Label)gvRow.FindControl("lblClientIdNew");
                Label lblClientMobileNo = (Label)gvRow.FindControl("lblClientMobileNo");
                Label lblClientEmailId = (Label)gvRow.FindControl("lblClientEmailId");
                Label lblSiteIdNew = (Label)gvRow.FindControl("lblSiteIdNew");
                Label lblSiteName = (Label)gvRow.FindControl("lblSiteName");
                Label lblSiteMobileNo = (Label)gvRow.FindControl("lblSiteMobileNo");
                Label lblSiteEmailId = (Label)gvRow.FindControl("lblSiteEmailId");

                string strMsg = "";
                if (lblClientId.Text == null || lblClientId.Text == "" || lblClientId.Text == "0")
                {
                    strMsg = "Select Client from list.";
                }
                else if (lblSiteId.Text == null || lblSiteId.Text == "" || lblSiteId.Text == "0")
                {
                    strMsg = "Select Site from list.";
                }
                else if (lblSiteMobileNo.Text == null || lblSiteMobileNo.Text == "")
                {
                    strMsg = "Site Mobile No. not available.";
                }
                else
                {
                    string strPassword = "st" + (Convert.ToInt32(lblSiteId.Text) + 20).ToString();
                    //dc.Client_Update_VerifyApp(Convert.ToInt32(lblClientId.Text), lblClientAddress.Text, lblClientMobileNo.Text, Convert.ToInt32(lblSiteId.Text), lblSiteAddress.Text, lblSiteMobileNo.Text, Convert.ToInt32(lblClientIdNew.Text), Convert.ToInt32(lblSiteIdNew.Text));
                    dc.Client_Update_VerifyApp(Convert.ToInt32(lblClientId.Text), "", "", Convert.ToInt32(lblSiteId.Text), lblSiteMobileNo.Text, lblSiteEmailId.Text, Convert.ToInt32(lblClientIdNew.Text), Convert.ToInt32(lblSiteIdNew.Text));

                    string contctDetails = getConctDetails(lblSiteMobileNo.Text);
                    if (contctDetails != "")
                    {
                        string[] arrContct = contctDetails.Split('|');
                        cntctPerson = arrContct[0];
                        conctNo = arrContct[1];
                        conctEmail = arrContct[2];
                    }
                    else
                    {
                        cntctPerson = lblSiteName.Text;
                        conctNo = lblSiteMobileNo.Text;
                        conctEmail = lblSiteEmailId.Text;
                    }
                    dc.DeviceLogin_Update(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), lblSiteMobileNo.Text, strPassword, false, cntctPerson, conctNo, conctEmail, false, 0, null, false);


                    //dc.DeviceLogin_Update(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), lblSiteMobileNo.Text, strPassword, false);

                    //var cl = dc.Client_View(Convert.ToInt32(lblClientId.Text), 0, "", "");
                    //string strSms = "Dear customer your registration with Durocrete is approved. Your password for mobile number " + lblClientMobileNo.Text + " is " + cl.FirstOrDefault().CL_Password_var;
                    string strSms = "Dear Customer , your password for Durocrete Client App is " + strPassword;
                    clsSendMail objcls = new clsSendMail();
                    objcls.sendSMS(lblSiteMobileNo.Text, strSms, "DUROCR","");
                    //if(lblClientEmailId.Text != "" && lblClientMobileNo.Text!="")
                    // SendMailToCilentAfterSiteApproval(lblClientEmailId.Text, lblClientMobileNo.Text, lblSiteName.Text);


                    strMsg = "Approved Successfully.";
                    LoadClientSiteList();
                    //}
                }
                if (strMsg != "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
                }
            }
        }
        public string getConctDetails(string mobId)
        {
            clsData obj = new clsData();
            string contctDetails = obj.getContctDetailsFromAppEnquiry(mobId);
            return contctDetails;
        }
        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");

            }
        }

        private void SendMailToCilentAfterSiteApproval(string mailIdTo, string ContactNo, string SiteName)
        {
            bool sendMail = true;
            string mCC = "";
            if (mailIdTo == "" || mailIdTo.Trim().ToLower() == "na@unknown.com" ||
                           mailIdTo.Trim().ToLower() == "na" || mailIdTo.Trim().ToLower().Contains("na@") == true ||
                           mailIdTo.Trim().ToLower().Contains("@") == false || mailIdTo.Trim().ToLower().Contains(".") == false)
            {
                sendMail = false;
            }

            if (IsValidEmailAddress(mailIdTo.Trim()) == false)
            {
                sendMail = false;
                //Label lblMsg = (Label)Master.FindControl("lblMsg");
                //lblMsg.ForeColor = System.Drawing.Color.Red;
                //lblMsg.Text = "Invalid email id.";
                //lblMsg.Visible = true;
            }


            if (sendMail == true)
            {
                clsSendMail objMail = new clsSendMail();
                string mSubject = "", mbody = "", mReplyTo = "";
                //mailIdTo = "shital.bandal@gmail.com";
                //mailIdCc = "";

                mSubject = "Login Request Received";

                mbody = "Dear Customer,<br><br>";
                mbody = mbody + "We have received Login request from " + ContactNo + " and  " + SiteName + ".";
                mbody = mbody + "<br>Henceforth they will be using our Client App for sending enquires and viewing Test Reports for  " + SiteName + ".";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>";
                mbody = mbody + "<br>";
                mbody = mbody + "Best Regards,";
                mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                try
                {
                    objMail.SendMail(mailIdTo, mCC, mSubject, mbody, "", mReplyTo);

                }
                catch { }
            }
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void FirstGridViewRowOfClientSite()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_OfficeAddress_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_mobile_No", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_EmailID_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Address_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_mobile_No", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_EmailID_var", typeof(string)));
            dt.Columns.Add(new DataColumn("DL_LoginId_var", typeof(string)));
            dt.Columns.Add(new DataColumn("DL_Password_var", typeof(string)));
            dr = dt.NewRow();
            dr["CL_Id"] = string.Empty;
            dr["CL_Name_var"] = string.Empty;
            dr["CL_OfficeAddress_var"] = string.Empty;
            dr["CL_mobile_No"] = string.Empty;
            dr["CL_EmailID_var"] = string.Empty;
            dr["SITE_Id"] = string.Empty;
            dr["SITE_Name_var"] = string.Empty;
            dr["SITE_Address_var"] = string.Empty;
            dr["SITE_mobile_No"] = string.Empty;
            dr["SITE_EmailID_var"] = string.Empty;
            dr["DL_LoginId_var"] = string.Empty;
            dr["DL_Password_var"] = string.Empty;
            dt.Rows.Add(dr);
            grdClientSite.DataSource = dt;
            grdClientSite.DataBind();
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
            if (lblClientId.Text != "")
            {
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

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            if (rbApprovalComplete.Checked)
            {
                Loaddefault();
            }
            else if (rbApprovalPending.Checked)
            {
                LoadClientSiteList();
            }
        }

        protected void rbApprovalComplete_CheckedChanged(object sender, EventArgs e)
        {
            grdClientSite.DataSource = null;
            grdClientSite.DataBind();
            lblTotalRecords.Text = "Total No of Records : " + grdClientSite.Rows.Count;
            lblName.Text = "List of Login Approved";
        }

        protected void rbApprovalPending_CheckedChanged(object sender, EventArgs e)
        {
            grdClientSite.DataSource = null;
            grdClientSite.DataBind();
            lblTotalRecords.Text = "Total No of Records : " + grdClientSite.Rows.Count;
            lblName.Text = "List of Login Waiting for Approval";
        }
    }
}
