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
using System.Collections.Generic;
using System.Web.Services;

namespace DESPLWEB
{
    public partial class EnquirySiteWise : System.Web.UI.Page
    {
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            if (!IsPostBack)
            {
                Session["CL_ID"] = 0;
                Session["SITE_ID"] = 0;
                Session["CONTPersonID"] = 0;
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                    if (!strReq.Contains("=") == false)
                    {
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        lblEnqId.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[1].Split('=');
                        if (arrIndMsg[1].ToString() != "")
                        {
                            Session["CL_ID"] = Convert.ToInt32(arrIndMsg[1].ToString().Trim());
                        }
                        if (arrIndMsg[1].ToString() != "")
                        {
                            arrIndMsg = arrMsgs[2].Split('=');
                            Session["SITE_ID"] = Convert.ToInt32(arrIndMsg[1].ToString().Trim());
                        }
                        if (arrIndMsg[1].ToString() != "")
                        {
                            arrIndMsg = arrMsgs[3].Split('=');
                            Session["CONTPersonID"] = Convert.ToInt32(arrIndMsg[1].ToString().Trim());
                        }
                    }
                }
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Add Enquiry";
                //bool userRight = false;
                //if (Session["LoginId"] == null)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                //    Response.Redirect("Login.aspx");
                //}
                //else
                //{
                //    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                //    foreach (var u in user)
                //    {
                //        if (u.USER_Admin_right_bit == true)
                //        {
                //            userRight = true;
                //        }
                //    }
                //}
                //if (userRight == false)
                //{
                //    pnlContent.Visible = false;
                //    lblAccess.Visible = true;
                //    lblAccess.Text = "Access is Denied.. ";
                //}
                //else
                //{
                LoadInwardList();
                LoadLocation();
                LoadDriverName();
                txt_Site.Focus();
                Tab_To_be_Collected.CssClass = "Clicked";
                MainView_EnquiryStatus.ActiveViewIndex = 0;
                if (lblEnqId.Text != "")
                {
                    if (Convert.ToInt32(lblEnqId.Text) > 0)
                    {
                        EditEnquiry();
                    }
                }
                //}
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
            DataRow row = null;
            DataTable dt = new DataTable();
            if (Convert.ToInt32(HttpContext.Current.Session["SITE_ID"]) > 0)
            {
                int ClientId = 0;
                if (int.TryParse(HttpContext.Current.Session["SITE_ID"].ToString(), out ClientId))
                {
                    var query = db.Client_View(0, 0, searchHead, HttpContext.Current.Session["SITE_Name"].ToString());
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
            //if (Convert.ToInt32(HttpContext.Current.Session["CL_ID"]) > 0)
            //{
            //    int ClientId = 0;
            //    if (int.TryParse(HttpContext.Current.Session["CL_ID"].ToString(), out ClientId))
            //    {

            var res = db.Site_View(0, 0, 0, searchHead);
            dt.Columns.Add("SITE_Name_var");
            foreach (var obj in res)
            {
                row = dt.NewRow();
                string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                dt.Rows.Add(listitem);
            }
            if (row == null)
            {
                var resclnm = db.Site_View(0, 0, 0, "");
                foreach (var obj in resclnm)
                {
                    row = dt.NewRow();
                    string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                    dt.Rows.Add(listitem);
                }
            }
            //    }
            //}
            List<string> SITE_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SITE_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return SITE_Name_var;
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetContactname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            DataRow row = null;
            DataTable dt = new DataTable();
            if (Convert.ToInt32(HttpContext.Current.Session["SITE_ID"]) > 0 && Convert.ToInt32(HttpContext.Current.Session["CL_ID"]) > 0)
            {
                var contactperson = db.Contact_View(0, Convert.ToInt32(HttpContext.Current.Session["SITE_ID"]), Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), searchHead);
                dt.Columns.Add("CONT_Name_var");
                foreach (var obj in contactperson)
                {
                    row = dt.NewRow();
                    string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.CONT_Name_var, obj.CONT_Id.ToString());
                    dt.Rows.Add(listitem);
                }
                if (row == null)
                {

                    var contPerson = db.Contact_View(0, Convert.ToInt32(HttpContext.Current.Session["SITE_ID"]), Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), "");
                    foreach (var obj in contPerson)
                    {
                        row = dt.NewRow();
                        string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.CONT_Name_var, obj.CONT_Id.ToString());
                        dt.Rows.Add(listitem);
                    }
                }
            }
            List<string> CONT_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CONT_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CONT_Name_var;
        }
        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }
        protected Boolean ChkSiteName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Site.Text;
            Boolean valid = false;
            //var res = dc.Site_View(0, Convert.ToInt32(Session["CL_ID"]), 0, searchHead);
            var res = dc.Site_View(0, 0, 0, searchHead);
            foreach (var obj in res)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Site.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Site Name is not in the list ";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }
        protected Boolean ChkContactName(string searchHead)
        {
            searchHead = txt_ContactPerson.Text;
            Boolean valid = false;
            var contactperson = dc.Contact_View(0, Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CL_ID"]), searchHead);
            foreach (var obj in contactperson)
            {
                valid = true;
            }
            if (valid == false)
            {
                Session["CONTPersonID"] = 0;
            }
            return valid;
        }

        protected void Tab_To_be_Collected_Click(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Clicked";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 0;
            //*

            //ddlRouteName.Enabled = false;
            //ddlRouteName.ClearSelection();
            //txt_CollectionDate.Enabled = false;
            //chkbox_Urgent.Checked = false;
            //Lbl_Client_Expected_Date.Visible = false;
            //txt_ClientExpected_Date.Visible = false;
            //LoadLocation();


            //Clear data on click event
            if (ddl_Location.SelectedIndex > 0 || chkbox_Urgent.Checked || txt_CollectionDate.Text != string.Empty)
            {
                Rdn_AtLab.Checked = false;
                Rdn_ByDriver.Checked = false;
                Rdn_DecisionPending.Checked = false;
                Rdn_DeclienedbyUs.Checked = false;
                Rdn_MaterialSendingOn_Date.Checked = false;
                Rdn_RejectedByClient.Checked = false;
                Rdn_OnsiteTesting.Checked = false;
            }

        }

        protected void Tab_Already_Collected_Click(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Clicked";
            Tab_Others.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 1;
            //*
            //Rdn_AtLab.Checked = false;
            //Rdn_ByDriver.Checked = false;
            //LoadDriverName();
            //ddl_DriverName.Enabled = false;

            //Clear data on click event
            if (Rdn_AtLab.Checked || Rdn_ByDriver.Checked)
            {
                Rdn_DecisionPending.Checked = false;
                Rdn_DeclienedbyUs.Checked = false;
                Rdn_MaterialSendingOn_Date.Checked = false;
                Rdn_RejectedByClient.Checked = false;
                Rdn_OnsiteTesting.Checked = false;
                ddl_Location.SelectedIndex = 0;
                txt_CollectionDate.Text = string.Empty;
                ddlRouteName.ClearSelection();
            }
        }

        protected void Tab_Others_Click(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            //*
            //Rdn_DecisionPending.Checked = false;
            //Rdn_DeclienedbyUs.Checked = false;
            //Rdn_MaterialSendingOn_Date.Checked = false;
            //Rdn_RejectedByClient.Checked = false;
            //Rdn_OnsiteTesting.Checked = false;
            //txt_CommentDecisionPending.Text = string.Empty;
            //txt_CommentDeclinebyUs.Text = string.Empty;
            //txt_CommentRejectedByClient.Text = string.Empty;
            //txt_MaterialSendingOnDate.Text = string.Empty;
            //txt_OnsiteTesting_Date.Text = string.Empty;

            //txt_CommentDecisionPending.Enabled = false;
            //txt_CommentDeclinebyUs.Enabled = false;
            //txt_CommentRejectedByClient.Enabled = false;
            //txt_MaterialSendingOnDate.Enabled = false;
            //txt_OnsiteTesting_Date.Enabled = false;
            //Clear data on click event
            if (Rdn_DecisionPending.Checked || Rdn_DeclienedbyUs.Checked || Rdn_MaterialSendingOn_Date.Checked || Rdn_RejectedByClient.Checked || Rdn_OnsiteTesting.Checked)
            {
                Rdn_AtLab.Checked = false;
                Rdn_ByDriver.Checked = false;
                ddl_Location.SelectedIndex = 0;
                txt_CollectionDate.Text = string.Empty;
                ddlRouteName.ClearSelection();
                ddl_DriverName.Enabled = false;
            }
        }

        private void LoadInwardList()
        {
            ddl_InwardType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardType.DataValueField = "MATERIAL_Id";
            var inwd = dc.Material_View("", "");
            ddl_InwardType.DataSource = inwd;
            ddl_InwardType.DataBind();
            ddl_InwardType.Items.Insert(0, "----------------------------Select----------------------------");
        }
        private void LoadLocation()
        {
            ddl_Location.DataTextField = "LOCATION_Name_var";
            ddl_Location.DataValueField = "LOCATION_Id";
            var Loct = dc.Location_View(0, "", 0);
            ddl_Location.DataSource = Loct;
            ddl_Location.DataBind();
            ddl_Location.Items.Insert(0, new ListItem("----------------------------Select----------------------------", "0"));
        }

        private void LoadDriverName()
        {
            ddl_DriverName.DataTextField = "USER_Name_var";
            ddl_DriverName.DataValueField = "USER_Id";
            var Driver = dc.Driver_View(false);
            ddl_DriverName.DataSource = Driver;
            ddl_DriverName.DataBind();
            ddl_DriverName.Items.Insert(0, "----------------------------Select----------------------------");
        }

        public void EnableLocation()
        {
            ddlRouteName.Enabled = false;

            if (ddl_Location.SelectedIndex > 0)
            {
                ddlRouteName.Enabled = true;
            }
            else
            {
                ddlRouteName.Enabled = false;
            }
        }
        public void EnableDriverName()
        {
            ddl_DriverName.Enabled = false;
            if (Rdn_ByDriver.Checked)
            {
                ddl_DriverName.Enabled = true;
            }
            else
            {
                ddl_DriverName.Enabled = false;
            }
        }
        public void TickCheckkBox()
        {
            if (chkbox_Urgent.Checked == true)
            {
                Lbl_Client_Expected_Date.Visible = true;
                txt_ClientExpected_Date.Visible = true;
            }
            else
            {
                Lbl_Client_Expected_Date.Visible = false;
                txt_ClientExpected_Date.Visible = false;
            }
        }
        protected void chkbox_Urgent_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Clicked";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 0;
            if (chkbox_Urgent.Checked == true)
            {
                Lbl_Client_Expected_Date.Visible = true;
                txt_ClientExpected_Date.Visible = true;
                this.txt_ClientExpected_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
            else
            {
                Lbl_Client_Expected_Date.Visible = false;
                txt_ClientExpected_Date.Visible = false;
            }
        }

        protected Boolean ValidateEnqStatus()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = false;
            int i = 0;
            if (ddl_Location.SelectedIndex == 0)
            {
                i++;
            }
            if (txt_CollectionDate.Text == string.Empty)
            {
                i++;
            }
            if (ddlRouteName.SelectedIndex == -1 || ddlRouteName.SelectedIndex == 0)
            {
                i++;
            }
            if (chkbox_Urgent.Checked != true)
            {
                i++;
            }
            if (Rdn_AtLab.Checked != true)
            {
                i++;
            }
            if (Rdn_ByDriver.Checked != true)
            {
                i++;
            }
            if (ddl_DriverName.SelectedIndex == 0)
            {
                i++;
            }
            if (Rdn_DecisionPending.Checked != true)
            {
                i++;
            }
            if (Rdn_DeclienedbyUs.Checked != true)
            {
                i++;
            }
            if (Rdn_RejectedByClient.Checked != true)
            {
                i++;
            }
            if (Rdn_OnsiteTesting.Checked != true)
            {
                i++;
            }
            if (Rdn_MaterialSendingOn_Date.Checked != true)
            {
                i++;
            }
            if (i >= 12)
            {
                lblMsg.Text = "Please Select Enquiry Status";
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }


        protected void lnkSaveEnquiry_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                int clientId = 0, siteId = 0, contactPerId = 0;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                if (ChkClientName(txt_Client.Text) == true && ChkSiteName(txt_Site.Text) == true) // && ChkContactName(txt_ContactPerson.Text) == true)
                {
                    ChkContactName(txt_ContactPerson.Text); // To check Contact Person

                    if (txt_Client.Text != "")
                    {
                        string ClientId = Request.Form[hfClientId.UniqueID];
                        if (ClientId != "")
                        {
                            Session["CL_ID"] = Convert.ToInt32(ClientId);
                        }
                        var client = dc.Client_View(0, 0, txt_Client.Text, "");
                        foreach (var cl in client)
                        {
                            clientId = cl.CL_Id;
                        }
                    }
                    if (txt_Site.Text != "")
                    {
                        string SiteId = Request.Form[hfSiteId.UniqueID];
                        if (SiteId != "")
                        {
                            Session["SITE_ID"] = Convert.ToInt32(SiteId);
                        }
                        var site = dc.Site_View(0, clientId, 0, txt_Site.Text);
                        foreach (var st in site)
                        {
                            siteId = st.SITE_Id;
                        }
                    }
                    if (txt_ContactNo.Text != "")
                    {
                        string ContactId = Request.Form[hfContactId.UniqueID];
                        if (ContactId != "")
                        {
                            Session["CONTPersonID"] = Convert.ToInt32(ContactId);
                        }
                        var contactper = dc.Contact_View(0, siteId, clientId, txt_ContactPerson.Text);
                        foreach (var cont in contactper)
                        {
                            contactPerId = cont.CONT_Id;
                        }
                    }
                    if (ValidateEnqStatus() == true)
                    {
                        DateTime ENQ_Date_dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        string OpenEnQuiryStatus = string.Empty;
                        string ENQ_MaterialSendingOnDate_dt = string.Empty;
                        string ENQ_TestingDate_dt = string.Empty;
                        string ENQ_CollectionDate_dt = string.Empty;
                        bool ENQ_UrgentStatus_bit = false;
                        string comment = string.Empty;
                        string ENQ_ClientExpectedDate_dt = string.Empty;
                        string ENQ_CollectedAt_var = string.Empty;
                        byte ENQ_CRLimitApprStatus_tint = 0;
                        int ENQ_LOCATION_Id = 0;
                        int ENQ_ROUTE_Id = 0;
                        byte ENQ_Status_tint = 0;
                        int ENQ_DriverId = 0;
                        DateTime? ClentExpectedDate = null;
                        DateTime? CollectionDate = null;
                        DateTime? MaterialSendingOnDate = null;
                        DateTime? TestingDate = null;
                        if (ddl_Location.SelectedIndex > 0)
                        {
                            ENQ_LOCATION_Id = Convert.ToInt32(ddl_Location.SelectedValue);
                        }
                        else
                        {
                            ENQ_LOCATION_Id = 0;
                        }
                        if (MainView_EnquiryStatus.ActiveViewIndex == 0) //to be collected
                        {
                            if (ddlRouteName.Items.Count > 0)//(ddlRouteName.SelectedIndex > 0)
                            {
                                if (ddlRouteName.SelectedValue != "")
                                {
                                    ENQ_ROUTE_Id = Convert.ToInt32(ddlRouteName.SelectedValue);
                                }
                            }
                            else
                            {
                                ENQ_ROUTE_Id = 0;
                            }
                            ENQ_Status_tint = 5;
                        }
                        //if (MainView_EnquiryStatus.ActiveViewIndex != 0)//Not To be Collected 
                        else
                        {
                            ENQ_Status_tint = 1;
                        }
                        //else
                        //{     
                        //ENQ_Status_tint = 0;
                        if (lblBalanceAmt.Text != "0.00" && lblBalanceAmt.Text != "")
                        {
                            decimal BalPer = (Convert.ToDecimal(lblAbove90BalanceAmt.Text) * 100) / Convert.ToDecimal(lblBalanceAmt.Text);
                            if (Convert.ToDecimal(lblBalanceAmt.Text) >= Convert.ToDecimal(lblLimitAmt.Text)
                                || BalPer >= 25 || Convert.ToDecimal(lblAbove90BalanceAmt.Text) > 300)
                            {
                                ENQ_CRLimitApprStatus_tint = 3; //3 in vb
                            }
                        }
                        if (ENQ_CRLimitApprStatus_tint != 3)
                        {
                            ENQ_CRLimitApprStatus_tint = 0;
                            if (Convert.ToDecimal(lblBalanceAmt.Text) >= Convert.ToDecimal(lblLimitAmt.Text)) //&& MainView_EnquiryStatus.ActiveViewIndex != 1)
                            {
                                if (ddl_InwardType.SelectedItem.Text == "Cube Testing")
                                {
                                    if (Convert.ToInt32(lblCouponCount.Text) < 3)
                                        ENQ_CRLimitApprStatus_tint = 1;
                                }
                                else if (ddl_InwardType.SelectedItem.Text == "Soil Investigation")
                                {
                                    ENQ_CRLimitApprStatus_tint = 0;
                                }
                                else
                                {
                                    ENQ_CRLimitApprStatus_tint = 1;
                                }
                            }
                        }
                        //}
                        if (MainView_EnquiryStatus.ActiveViewIndex == 0)
                        {
                            OpenEnQuiryStatus = Tab_To_be_Collected.Text;
                            ENQ_CollectionDate_dt = txt_CollectionDate.Text;//need to change
                            CollectionDate = DateTime.ParseExact(txt_CollectionDate.Text, "dd/MM/yyyy", null);
                        }
                        else if (MainView_EnquiryStatus.ActiveViewIndex == 1)
                        {
                            OpenEnQuiryStatus = Tab_Already_Collected.Text;
                            if (Rdn_AtLab.Checked == true)
                            {
                                ENQ_CollectedAt_var = Label24.Text;
                            }
                            else if (Rdn_ByDriver.Checked == true)
                            {
                                ENQ_CollectedAt_var = Label25.Text;
                                ENQ_DriverId = Convert.ToInt32(ddl_DriverName.SelectedValue.ToString());
                            }
                        }
                        else if (MainView_EnquiryStatus.ActiveViewIndex == 2)
                        {
                            OpenEnQuiryStatus = Tab_Others.Text;
                            if (Rdn_DeclienedbyUs.Checked == true || Rdn_RejectedByClient.Checked == true)
                            {
                                ENQ_Status_tint = 2;
                            }
                            //else
                            //{
                            //    ENQ_Status_tint = 0;
                            //}
                            if (Rdn_DecisionPending.Checked == true)
                            {
                                OpenEnQuiryStatus = Label26.Text;
                            }
                            else if (Rdn_DeclienedbyUs.Checked == true)
                            {
                                OpenEnQuiryStatus = Label27.Text;
                            }
                            else if (Rdn_RejectedByClient.Checked == true)
                            {
                                OpenEnQuiryStatus = Label28.Text;
                            }
                            else if (Rdn_OnsiteTesting.Checked == true)
                            {
                                OpenEnQuiryStatus = Label29.Text;
                            }
                            else if (Rdn_MaterialSendingOn_Date.Checked == true)
                            {
                                OpenEnQuiryStatus = Label30.Text;
                            }
                        }
                        if (Rdn_DecisionPending.Checked == true)
                        {
                            comment = txt_CommentDecisionPending.Text;
                        }
                        else if (Rdn_DeclienedbyUs.Checked)
                        {
                            comment = txt_CommentDeclinebyUs.Text;
                        }
                        else if (Rdn_RejectedByClient.Checked)
                        {
                            comment = txt_CommentRejectedByClient.Text;
                        }
                        if (Rdn_OnsiteTesting.Checked)
                        {
                            ENQ_TestingDate_dt = txt_OnsiteTesting_Date.Text;
                            TestingDate = DateTime.ParseExact(txt_OnsiteTesting_Date.Text, "dd/MM/yyyy", null);//need to change
                        }
                        else if (Rdn_MaterialSendingOn_Date.Checked)
                        {
                            ENQ_MaterialSendingOnDate_dt = txt_MaterialSendingOnDate.Text;
                            MaterialSendingOnDate = DateTime.ParseExact(txt_MaterialSendingOnDate.Text, "dd/MM/yyyy", null);//need to change
                        }
                        if (chkbox_Urgent.Checked == true)
                        {
                            ENQ_UrgentStatus_bit = true;
                            ENQ_ClientExpectedDate_dt = txt_ClientExpected_Date.Text;
                            ClentExpectedDate = DateTime.ParseExact(txt_ClientExpected_Date.Text, "dd/MM/yyyy", null);//need to change
                        }
                        else
                        {
                            ENQ_ClientExpectedDate_dt = null;
                            ClentExpectedDate = null;//need to change
                        }
                        int EnQ_No;
                        try
                        {
                            lblMsg.Visible = true;
                            lblMsg.ForeColor = System.Drawing.Color.Green;

                            //dc.Contact_Update(Convert.ToInt32(Session["CONTPersonID"]), Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), txt_ContactPerson.Text, txt_ContactNo.Text, "", false);
                            dc.Contact_Update(contactPerId, clientId, siteId, txt_ContactPerson.Text, txt_ContactNo.Text, "", false);
                            //ShowCONT_Id();
                            if (contactPerId == 0)
                            {
                                //var c = dc.Contact_View(0, Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CL_ID"]), txt_ContactPerson.Text);
                                var c = dc.Contact_View(0, siteId, clientId, txt_ContactPerson.Text);
                                foreach (var cn in c)
                                {
                                    txt_ContactNo.Text = Convert.ToString(cn.CONT_ContactNo_var);
                                    Session["CONTPersonID"] = cn.CONT_Id;
                                    contactPerId = cn.CONT_Id;
                                }
                            }
                            int EnqId = 0;
                            if (lblEnqId.Text == "")
                            {
                                EnqId = 0;
                            }
                            else
                            {
                                EnqId = Convert.ToInt32(lblEnqId.Text);
                            }
                            //dc.Enquiry_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, Convert.ToInt32(ddl_InwardType.SelectedValue), Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CONTPersonID"]), Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, ENQ_CRLimitApprStatus_tint, Convert.ToDecimal(txt_Quantity.Text), Convert.ToInt32(Session["LoginId"]));
                            dc.Enquiry_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, Convert.ToInt32(ddl_InwardType.SelectedValue), clientId, siteId, contactPerId, Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, ENQ_CRLimitApprStatus_tint, Convert.ToDecimal(txt_Quantity.Text), Convert.ToInt32(Session["LoginId"]), "", "", false, false, 0, 0);
                            //dc.Site_Update(Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CL_ID"]), txt_Site.Text, 0, txt_Site_address.Text, "", 0, 0, txt_Site_LandMark.Text, Convert.ToInt32(ddl_Location.SelectedValue), true);
                            dc.Site_Update(siteId, clientId, txt_Site.Text, 0, txt_Site_address.Text, "", 0, 0, txt_Site_LandMark.Text, Convert.ToInt32(ddl_Location.SelectedValue), false, true, "", 0, "", "", "", null, "", false, false, "", "", "", 0);

                            if (EnqId > 0)
                            {
                                lblMsg.Text = "Record is Saved Successfully. " + " " + " Your Enquiry No is " + " " + (lblEnqId.Text).ToString();
                            }
                            else
                            {
                                var enquiry = dc.Enquiry_View_Id();
                                foreach (var enq in enquiry)
                                {
                                    EnQ_No = Convert.ToInt32(enq.Enqiry_Id);
                                    lblEnqId.Text = EnQ_No.ToString();
                                }
                                EnQ_No = Convert.ToInt32(lblEnqId.Text);
                                lblMsg.Text = "Record is Saved Successfully. " + " " + " Your Enquiry No is " + " " + EnQ_No.ToString();
                            }

                            lnkSaveEnquiry.Visible = false;
                            lnk_Inward.Visible = false;

                            lblSelectedInward.Text = ddl_InwardType.SelectedItem.Text;
                            //if (MainView_EnquiryStatus.ActiveViewIndex == 1 || MainView_EnquiryStatus.ActiveViewIndex == 2)
                            //{
                            //    lnk_Inward.Visible = false;
                            //    if (MainView_EnquiryStatus.ActiveViewIndex == 2)
                            //    {
                            //        if (Rdn_DecisionPending.Checked == true || Rdn_OnsiteTesting.Checked == true || Rdn_MaterialSendingOn_Date.Checked == true)
                            //        {
                            //            lnk_Inward.Visible = true;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        lnk_Inward.Visible = true;
                            //    }
                            //}
                            //else
                            //{
                            //    lnk_Inward.Visible = false;
                            //}

                            if (MainView_EnquiryStatus.ActiveViewIndex == 1 && ENQ_CRLimitApprStatus_tint == 0)
                                lnk_Inward.Visible = true;
                            if (lnk_Inward.Visible == true)
                            {
                                lnk_Inward.Enabled = false;
                                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                                foreach (var u in user)
                                {
                                    if (u.USER_Inward_right_bit == true)
                                        lnk_Inward.Enabled = true;
                                }
                            }
                            Cleartextbox();
                        }
                        catch (Exception ex)
                        {
                            string ms = ex.Message;
                        }
                    }
                }
            }
        }

        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;

            if (MainView_EnquiryStatus.ActiveViewIndex == 0)
            {
                if (ddl_Location.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select the Location";
                    valid = false;
                }
                else if (txt_CollectionDate.Text == string.Empty)
                {
                    lblMsg.Text = "Please Select the collection Date";
                    valid = false;
                }
            }

            else
                if (MainView_EnquiryStatus.ActiveViewIndex == 1 && valid == true)
            {
                if (Rdn_AtLab.Checked == false && Rdn_ByDriver.Checked == false)
                {
                    lblMsg.Text = "Please select anyone of these radio button";
                    valid = false;
                }

                else
                {
                    if (Rdn_ByDriver.Checked == true)
                    {
                        if (ddl_DriverName.SelectedIndex == 0)
                        {
                            lblMsg.Text = "Please Select the Driver Name";
                            valid = false;
                        }
                    }
                }
            }
            else
                    if (MainView_EnquiryStatus.ActiveViewIndex == 2 && valid == true)
            {
                if (Rdn_DecisionPending.Checked == false &&
                    Rdn_DeclienedbyUs.Checked == false &&
                    Rdn_RejectedByClient.Checked == false &&
                    Rdn_OnsiteTesting.Checked == false &&
                    Rdn_MaterialSendingOn_Date.Checked == false)
                {

                    lblMsg.Text = "Please select anyone of these radio button";
                    valid = false;
                }
                else
                {
                    if (Rdn_DecisionPending.Checked)
                    {
                        if (txt_CommentDecisionPending.Text == string.Empty)
                        {
                            lblMsg.Text = "Please write some Comment";
                            txt_CommentDecisionPending.Focus();
                            valid = false;
                        }

                    }
                    else if (Rdn_DeclienedbyUs.Checked)
                    {
                        if (txt_CommentDeclinebyUs.Text == string.Empty)
                        {
                            lblMsg.Text = "Please write some Comment";
                            txt_CommentDeclinebyUs.Focus();
                            valid = false;
                        }
                    }
                    else if (Rdn_RejectedByClient.Checked)
                    {
                        if (txt_CommentRejectedByClient.Text == string.Empty)
                        {
                            lblMsg.Text = "Please write some Comment";
                            txt_CommentRejectedByClient.Focus();
                            valid = false;
                        }

                    }
                    else if (Rdn_OnsiteTesting.Checked)
                    {
                        if (txt_OnsiteTesting_Date.Text == string.Empty)
                        {
                            lblMsg.Text = "Please select onsite Testing Date";
                            txt_OnsiteTesting_Date.Focus();
                            valid = false;
                        }

                    }
                    else if (Rdn_MaterialSendingOn_Date.Checked)
                    {
                        if (txt_MaterialSendingOnDate.Text == string.Empty)
                        {
                            lblMsg.Text = "Please select Material Sending Date";
                            txt_MaterialSendingOnDate.Focus();
                            valid = false;
                        }
                    }
                }
            }
            //if (valid == true)
            //{
            //    if (txt_Note.Text == "")
            //    {
            //        lblMsg.Text = "Please Enter Note";
            //        txt_Note.Focus();
            //        valid = false;
            //    }
            //    else if (txt_Reference.Text == "")
            //    {
            //        lblMsg.Text = "Please Enter Some Reference";
            //        txt_Reference.Focus();
            //        valid = false;
            //    }
            //}
            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }

        public void LoadRoutename()
        {
            ddlRouteName.DataTextField = "ROUTE_Name_var";
            ddlRouteName.DataValueField = "ROUTE_Id";
            var LocationRoute = dc.Route_View(0, "", "False", Convert.ToInt32(ddl_Location.SelectedValue));
            ddlRouteName.DataSource = LocationRoute;
            ddlRouteName.DataBind();
            ddlRouteName.Items.Insert(0, "----------------------------Select----------------------------");
        }

        protected void ddl_Location_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddlRouteName.Enabled = true;
            Tab_To_be_Collected.CssClass = "Clicked";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 0;
            if (ddl_Location.SelectedItem.Text != "----------------------------Select----------------------------")
            {
                ddlRouteName.ClearSelection();
                ddlRouteName.DataTextField = "ROUTE_Name_var";
                ddlRouteName.DataValueField = "ROUTE_Id";
                var LocationRoute = dc.Route_View(0, "", "False", Convert.ToInt32(ddl_Location.SelectedValue));
                ddlRouteName.DataSource = LocationRoute;
                ddlRouteName.DataBind();
                DisplayDateRouteWise();
            }
            else
            {
                ddlRouteName.Items.Clear();
                txt_CollectionDate.Text = string.Empty;
            }
        }
        public void ddl_LocationSelectChanged()
        {
            MainView_EnquiryStatus.ActiveViewIndex = 0;
            if (ddl_Location.SelectedItem.Text != "----------------------------Select----------------------------")
            {
                ddlRouteName.DataTextField = "ROUTE_Name_var";
                ddlRouteName.DataValueField = "ROUTE_Id";
                var LocationRoute = dc.Route_View(0, "", "False", Convert.ToInt32(ddl_Location.SelectedValue));
                ddlRouteName.DataSource = LocationRoute;
                ddlRouteName.DataBind();

                DisplayDateRouteWise();
            }
            else
            {
                ddlRouteName.Items.Clear();
            }
        }
        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Enquiry_List.aspx");
        }
        public void DisplayDateRouteWise()
        {
            if (ddlRouteName.SelectedValue != "")
            {
                string SerchDay;
                var da = dc.RouteDate_View(Convert.ToInt32(ddlRouteName.SelectedValue));
                SerchDay = "";
                foreach (var c in da)
                {

                    SerchDay = SerchDay + c.Coll_Day_var.ToString() + "|";

                }
                DateTime RouteDate;
                RouteDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                for (int i = 1; i < 8; i++)
                {
                    RouteDate = RouteDate.AddDays(1);
                    string Startday = RouteDate.DayOfWeek.ToString() + "|";

                    if (SerchDay.Contains(Startday))
                    {
                        txt_CollectionDate.Text = RouteDate.ToString("dd/MM/yyyy");
                        txt_CollectionDate.Enabled = true;
                        break;
                    }
                }
            }
            else
            {
                ddlRouteName.Items.Insert(0, "---Select---");
                txt_CollectionDate.Text = string.Empty;
                txt_CollectionDate.Enabled = false;
            }

        }

        protected void ddlRouteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayDateRouteWise();
        }

        public void EditEnquiry()
        {
            //add
            //byte ENQ_Status_tint = 0;
            //if (MainView_EnquiryStatus.ActiveViewIndex == 2)
            //{

            //    if (Rdn_DeclienedbyUs.Checked == true || Rdn_RejectedByClient.Checked == true)
            //    {
            //        ENQ_Status_tint = 1;
            //    }
            //    else
            //    {
            //        ENQ_Status_tint = 0;
            //    }
            //}
            if (Convert.ToInt32(lblEnqId.Text) > 0)
            {

                var viewEnquiry = dc.Enquiry_View(Convert.ToInt32(lblEnqId.Text), 1, 0);  // ENQ_Status_tint, "");
                foreach (var Enqry in viewEnquiry)
                {
                    Session["CL_ID"] = Convert.ToInt32(Enqry.ENQ_CL_Id);
                    Session["SITE_ID"] = Convert.ToInt32(Enqry.ENQ_SITE_Id);
                    Session["CONTPersonID"] = Convert.ToInt32(Enqry.ENQ_CONT_Id);

                    txt_Client.Text = Enqry.CL_Name_var.ToString();
                    txt_Site.Text = Enqry.SITE_Name_var.ToString();
                    txt_Site_address.Text = Enqry.SITE_Address_var.ToString();
                    if (Enqry.SITE_Landmark_var != null)
                    {
                        txt_Site_LandMark.Text = Enqry.SITE_Landmark_var.ToString();
                    }
                    else
                    {
                        txt_Site_LandMark.Text = string.Empty;
                    }
                    txt_ContactPerson.Text = Enqry.CONT_Name_var.ToString();
                    txt_ContactNo.Text = Enqry.CONT_ContactNo_var.ToString();
                    ddl_InwardType.SelectedValue = Enqry.MATERIAL_Id.ToString();
                    txt_Quantity.Text = Enqry.ENQ_Quantity.ToString();
                    txt_Note.Text = Enqry.ENQ_Note_var.ToString();
                    txt_Reference.Text = Enqry.ENQ_Reference_var.ToString();
                    if (Enqry.ENQ_OpenEnquiryStatus_var == "To be Collected")
                    {
                        Tab_To_be_Collected.CssClass = "Clicked";
                        Tab_Already_Collected.CssClass = "Initial";
                        Tab_Others.CssClass = "Initial";
                        MainView_EnquiryStatus.ActiveViewIndex = 0;

                        ddl_Location.SelectedValue = Enqry.ENQ_LOCATION_Id.ToString();
                        LoadRoutename();
                        ddlRouteName.SelectedValue = Enqry.ENQ_ROUTE_Id.ToString();
                        ddlRouteName.Enabled = true;
                        txt_CollectionDate.Text = Convert.ToDateTime(Enqry.ENQ_CollectionDate_dt).ToString("dd/MM/yyyy");

                        if (Enqry.ENQ_UrgentStatus_bit == true)
                        {
                            chkbox_Urgent.Checked = true;
                            txt_ClientExpected_Date.Text = Convert.ToDateTime(Enqry.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                            Lbl_Client_Expected_Date.Visible = true;
                            txt_ClientExpected_Date.Visible = true;
                        }
                    }

                    else if (Enqry.ENQ_OpenEnquiryStatus_var == "Already Collected")
                    {
                        Tab_To_be_Collected.CssClass = "Initial";
                        Tab_Already_Collected.CssClass = "Clicked";
                        Tab_Others.CssClass = "Initial";
                        MainView_EnquiryStatus.ActiveViewIndex = 1;

                        if (Enqry.ENQ_CollectedAt_var == "By Driver")
                        {
                            Rdn_ByDriver.Checked = true;
                            ddl_DriverName.SelectedValue = Enqry.ENQ_DriverId.ToString();
                            ddl_DriverName.Enabled = true;
                        }
                        else if (Enqry.ENQ_CollectedAt_var == "At Lab")
                        {
                            Rdn_AtLab.Checked = true;
                            ddl_DriverName.Enabled = false;
                        }
                    }
                    else if (Enqry.ENQ_OpenEnquiryStatus_var == "Decision Pending")
                    {
                        Tab_To_be_Collected.CssClass = "Initial";
                        Tab_Already_Collected.CssClass = "Initial";
                        Tab_Others.CssClass = "Clicked";
                        MainView_EnquiryStatus.ActiveViewIndex = 2;
                        Rdn_DecisionPending.Checked = true;
                        txt_CommentDecisionPending.Text = Enqry.ENQ_Comment_var.ToString();
                        txt_CommentDecisionPending.Enabled = true;
                        txt_CommentDeclinebyUs.Enabled = false;
                        txt_CommentRejectedByClient.Enabled = false;
                        txt_OnsiteTesting_Date.Enabled = false;
                        txt_MaterialSendingOnDate.Enabled = false;
                    }
                    else if (Enqry.ENQ_OpenEnquiryStatus_var == "On site Testing")
                    {
                        Tab_To_be_Collected.CssClass = "Initial";
                        Tab_Already_Collected.CssClass = "Initial";
                        Tab_Others.CssClass = "Clicked";
                        MainView_EnquiryStatus.ActiveViewIndex = 2;
                        Rdn_OnsiteTesting.Checked = true;
                        txt_OnsiteTesting_Date.Text = Convert.ToDateTime(Enqry.ENQ_TestingDate_dt).ToString("dd/MM/yyyy");
                        txt_OnsiteTesting_Date.Enabled = true;
                        txt_CommentDecisionPending.Enabled = false;
                        txt_CommentDeclinebyUs.Enabled = false;
                        txt_CommentRejectedByClient.Enabled = false;
                        txt_MaterialSendingOnDate.Enabled = false;
                    }
                    else if (Enqry.ENQ_OpenEnquiryStatus_var == "Declined by us")
                    {
                        Tab_To_be_Collected.CssClass = "Initial";
                        Tab_Already_Collected.CssClass = "Initial";
                        Tab_Others.CssClass = "Clicked";
                        MainView_EnquiryStatus.ActiveViewIndex = 2;
                        Rdn_DeclienedbyUs.Checked = true;
                        txt_CommentDeclinebyUs.Text = Enqry.ENQ_Comment_var.ToString();
                        txt_CommentDeclinebyUs.Enabled = true;
                        txt_CommentDecisionPending.Enabled = false;
                        txt_CommentRejectedByClient.Enabled = false;
                        txt_OnsiteTesting_Date.Enabled = false;
                        txt_MaterialSendingOnDate.Enabled = false;
                    }
                    else if (Enqry.ENQ_OpenEnquiryStatus_var == "Rejected By Client")
                    {
                        Tab_To_be_Collected.CssClass = "Initial";
                        Tab_Already_Collected.CssClass = "Initial";
                        Tab_Others.CssClass = "Clicked";
                        MainView_EnquiryStatus.ActiveViewIndex = 2;
                        Rdn_RejectedByClient.Checked = true;
                        txt_CommentRejectedByClient.Text = Enqry.ENQ_Comment_var.ToString();
                        txt_CommentRejectedByClient.Enabled = true;
                        txt_CommentDecisionPending.Enabled = false;
                        txt_CommentDeclinebyUs.Enabled = false;
                        txt_OnsiteTesting_Date.Enabled = false;
                        txt_MaterialSendingOnDate.Enabled = false;
                    }
                    else if (Enqry.ENQ_OpenEnquiryStatus_var == "Material Sending On Date")
                    {
                        Tab_To_be_Collected.CssClass = "Initial";
                        Tab_Already_Collected.CssClass = "Initial";
                        Tab_Others.CssClass = "Clicked";
                        MainView_EnquiryStatus.ActiveViewIndex = 2;
                        Rdn_MaterialSendingOn_Date.Checked = true;
                        txt_MaterialSendingOnDate.Text = Convert.ToDateTime(Enqry.ENQ_MaterialSendingOnDate_dt).ToString("dd/MM/yyyy");
                        txt_MaterialSendingOnDate.Enabled = true;
                        txt_CommentDecisionPending.Enabled = false;
                        txt_CommentDeclinebyUs.Enabled = false;
                        txt_CommentRejectedByClient.Enabled = false;
                        txt_OnsiteTesting_Date.Enabled = false;

                    }
                }
                DisplayBalLmt();
            }

        }

        protected void Rdn_AtLab_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Clicked";
            Tab_Others.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 1;
            if (Rdn_AtLab.Checked == true)
            {
                ddl_DriverName.Enabled = false;
            }
        }

        protected void Rdn_ByDriver_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Clicked";
            Tab_Others.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 1;
            if (Rdn_ByDriver.Checked == true)
            {
                ddl_DriverName.Enabled = true;
            }
            else
            {
                ddl_DriverName.Enabled = false;
            }
        }

        protected void Rdn_DecisionPending_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_CommentDecisionPending.Enabled = true;
            txt_CommentDecisionPending.Focus();
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            empty();
        }
        private void empty()
        {
            txt_CommentDecisionPending.Text = "";
            txt_CommentDeclinebyUs.Text = "";
            txt_CommentRejectedByClient.Text = "";

        }
        public void VisibleOthersComment()
        {
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            if (Rdn_DecisionPending.Checked)
            {
                txt_CommentDecisionPending.Enabled = true;
            }
            else if (Rdn_DeclienedbyUs.Checked)
            {
                txt_CommentDeclinebyUs.Enabled = true;
            }
            else if (Rdn_RejectedByClient.Checked)
            {
                txt_CommentRejectedByClient.Enabled = true;
            }
            else if (Rdn_OnsiteTesting.Checked)
            {
                txt_OnsiteTesting_Date.Enabled = true;
                this.txt_OnsiteTesting_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
            else if (Rdn_MaterialSendingOn_Date.Checked)
            {
                txt_MaterialSendingOnDate.Enabled = true;
                this.txt_MaterialSendingOnDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        protected void Rdn_DeclienedbyUs_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_CommentDeclinebyUs.Enabled = true;
            txt_CommentDeclinebyUs.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            empty();
        }

        protected void Rdn_RejectedByClient_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_CommentRejectedByClient.Enabled = true;
            txt_CommentRejectedByClient.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            empty();
        }

        protected void Rdn_OnsiteTesting_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_OnsiteTesting_Date.Enabled = true;
            this.txt_OnsiteTesting_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_OnsiteTesting_Date.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            empty();
        }

        protected void Rdn_MaterialSendingOn_Date_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_MaterialSendingOnDate.Enabled = true;
            this.txt_MaterialSendingOnDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_MaterialSendingOnDate.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            txt_OnsiteTesting_Date.Enabled = false;
            empty();
        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lnkSaveEnquiry.Visible = true;
            lnk_Inward.Visible = false;
            //txt_Site.Text = string.Empty;
            txt_Site_address.Text = string.Empty;
            txt_Site_LandMark.Text = string.Empty;
            txt_ContactPerson.Text = string.Empty;
            txt_ContactNo.Text = string.Empty;
            //if (ChkClientName(txt_Client.Text) == true)
            if (ChkClientName(txt_Site.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                }
                else
                {
                    Session["CL_ID"] = 0;
                }
                if (txt_Client.Text == "" || txt_Site.Text == "")
                {
                    txt_ContactPerson.Text = "";
                }
                DisplayBalLmt();
                txt_ContactPerson.Focus();

                var site = dc.Site_View(0, Convert.ToInt32(Session["CL_ID"]), 0, txt_Site.Text);
                foreach (var st in site)
                {
                    Session["SITE_Id"] = st.SITE_Id;
                    txt_Site_address.Text = st.SITE_Address_var;
                    txt_Site_LandMark.Text = st.SITE_Landmark_var;
                    if (st.SITE_LocationId_int != null && st.SITE_LocationId_int > 0)
                    {
                        ddl_Location.SelectedValue = st.SITE_LocationId_int.ToString();
                        ddl_LocationSelectChanged();
                    }
                }
                var couponsitespec = dc.Coupon_View_Sitewise(Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0, DateTime.Now).ToList();
                lblCouponCount.Text = couponsitespec.Count().ToString();
                if (couponsitespec.Count() == 0)
                {
                    var coupon = dc.Coupon_View("", 0, 0, Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0, DateTime.Now).ToList();
                    lblCouponCount.Text = coupon.Count().ToString();
                }
            }
            if (lblSelectedInward.Text != "")
            {
                lblEnqId.Text = "";
            }
        }
        private void DisplayBalLmt()
        {
            int Cl_Id = 0;
            if (int.TryParse(Session["CL_ID"].ToString(), out Cl_Id))
            {
                if (Convert.ToInt32(Session["CL_ID"]) > 0)
                {
                    var data = dc.Client_View(Convert.ToInt32(Session["CL_ID"]), -1, "", "");
                    foreach (var c in data)
                    {
                        lblBalanceAmt.Text = "0.00";
                        lblBelow90BalanceAmt.Text = "0.00";
                        lblAbove90BalanceAmt.Text = "0.00";
                        lblLimitAmt.Text = "0.00";
                        //if (c.CL_BalanceAmt_mny != null)
                        //{
                        //    lblBalanceAmt.Text = Convert.ToDecimal(c.CL_BalanceAmt_mny).ToString("0.00");
                        //}
                        if (c.CL_OutstandingAmt_var != null)
                        {
                            string[] strAmt = c.CL_OutstandingAmt_var.Split('|');
                            lblBalanceAmt.Text = Convert.ToDecimal(strAmt[0]).ToString("0.00");
                            lblBelow90BalanceAmt.Text = Convert.ToDecimal(strAmt[1]).ToString("0.00");
                            lblAbove90BalanceAmt.Text = Convert.ToDecimal(strAmt[2]).ToString("0.00");
                        }
                        if (c.CL_Limit_mny != null)
                        {
                            lblLimitAmt.Text = Convert.ToDecimal(c.CL_Limit_mny).ToString("0.00");
                        }
                    }
                }
            }
        }
        protected void txt_ContactPerson_TextChanged(object sender, EventArgs e)
        {
            txt_ContactNo.Text = "";

            ShowCONT_Id();
            txt_ContactNo.Focus();

        }
        private void ShowCONT_Id()
        {
            var c = dc.Contact_View(0, Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CL_ID"]), txt_ContactPerson.Text);
            foreach (var cn in c)
            {
                txt_ContactNo.Text = Convert.ToString(cn.CONT_ContactNo_var);
                Session["CONTPersonID"] = Convert.ToInt32(cn.CONT_Id);
            }
        }
        protected void txt_Site_TextChanged(object sender, EventArgs e)
        {
            txt_Site_address.Text = string.Empty;
            txt_Site_LandMark.Text = string.Empty;
            txt_ContactPerson.Text = string.Empty;
            txt_ContactNo.Text = string.Empty;
            Session["SITE_ID"] = 0;

            //int cl_Id = 0;
            //if (Convert.ToInt32(Session["CL_ID"]) > 0)
            //{
            //    if (int.TryParse(Session["CL_ID"].ToString(), out cl_Id))
            //    {
            //if (ChkSiteName(txt_Client.Text) == true)
            if (ChkSiteName(txt_Site.Text) == true)
            {
                int SiteId = 0;
                Session["SITE_Name"] = txt_Site.Text;
                if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                {
                    Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                }
                else
                {
                    Session["SITE_ID"] = 0;
                }
                txt_Client.Focus();
                if (txt_Client.Text == "" || txt_Site.Text == "")
                {
                    txt_ContactPerson.Text = "";
                }
                //if (Convert.ToInt32(Session["SITE_ID"]) > 0)
                //{
                //    var s = dc.Site_View(Convert.ToInt32(Session["SITE_ID"]), 0, 0, "");
                //    foreach (var site in s)
                //    {
                //        txt_Site_address.Text = site.SITE_Address_var;
                //        txt_Site_LandMark.Text = site.SITE_Landmark_var;
                //        if (site.SITE_LocationId_int != null && site.SITE_LocationId_int > 0)
                //        {
                //            ddl_Location.SelectedValue = site.SITE_LocationId_int.ToString();
                //            ddl_LocationSelectChanged();
                //        }
                //    }
                //    var coupon = dc.Coupon_View(0, 0, 0, Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0, DateTime.Now).ToList();
                //    lblCouponCount.Text = coupon.Count().ToString();
                //}
            }
            //    }
            //}
        }

        private void Cleartextbox()
        {
            txt_Client.Text = string.Empty;
            txt_Site.Text = string.Empty;
            txt_Site_address.Text = string.Empty;
            txt_Site_LandMark.Text = string.Empty;
            txt_ContactPerson.Text = string.Empty;
            txt_ContactNo.Text = string.Empty;
            ddl_InwardType.ClearSelection();
            txt_Quantity.Text = string.Empty;
            txt_Note.Text = string.Empty;
            txt_Reference.Text = string.Empty;
            Tab_To_be_Collected.CssClass = "Clicked";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 0;
            ddl_Location.ClearSelection();
            ddlRouteName.Items.Clear();
            txt_CollectionDate.Text = string.Empty;
            chkbox_Urgent.Checked = false;
            Rdn_AtLab.Checked = false;
            Rdn_ByDriver.Checked = false;
            ddl_DriverName.ClearSelection();
            Rdn_DecisionPending.Checked = false;
            txt_CommentDecisionPending.Text = string.Empty;
            Rdn_DeclienedbyUs.Checked = false;
            txt_CommentDeclinebyUs.Text = string.Empty;
            Rdn_RejectedByClient.Checked = false;
            txt_CommentRejectedByClient.Text = string.Empty;
            Rdn_OnsiteTesting.Checked = false;
            txt_OnsiteTesting_Date.Text = string.Empty;
            Rdn_MaterialSendingOn_Date.Checked = false;
            txt_MaterialSendingOnDate.Text = string.Empty;
            Lbl_Client_Expected_Date.Visible = false;
            txt_ClientExpected_Date.Visible = false;
        }
        protected void lnk_Inward_Click(object sender, EventArgs e)
        {
            clsData clsDt = new clsData();
            var enq = dc.Enquiry_View(Convert.ToInt32(lblEnqId.Text), -1, 0).ToList();
            if (clsDt.checkGSTInfoUpdated(enq.FirstOrDefault().ENQ_CL_Id.ToString(), enq.FirstOrDefault().ENQ_SITE_Id.ToString(), enq.FirstOrDefault().MATERIAL_RecordType_var) == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Please update client & site GST details. Can not add inward." + "');", true);
            }
            else if (lblSelectedInward.Text == "Cube Testing")
            {
                string strURLWithData = "Cube_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Steel Testing")
            {
                string strURLWithData = "Steel_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Aggregate Testing")
            {
                string strURLWithData = "Aggregate_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Other Testing")
            {
                string strURLWithData = "Other_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Pile Testing")
            {
                string strURLWithData = "Pile_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Pavement Block Testing")
            {
                string strURLWithData = "Pavement_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Tile Testing")
            {
                string strURLWithData = "Tile_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Steel Chemical Testing")
            {
                string strURLWithData = "STC_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Core Testing")
            {
                string strURLWithData = "Core_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Non Destructive Testing")
            {
                string strURLWithData = "NDT_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Brick Testing")
            {
                string strURLWithData = "Brick_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Water Testing")
            {
                string strURLWithData = "Water_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Cement Chemical Testing")
            {
                string strURLWithData = "CementChemical_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Cement Testing")
            {
                string strURLWithData = "Cement_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Cube Testing")
            {
                string strURLWithData = "Cube_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Fly Ash Testing")
            {
                string strURLWithData = "Flyash_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Masonary Block Testing")
            {
                string strURLWithData = "Solid_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Soil Testing")
            {
                string strURLWithData = "Soil_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Soil Investigation")
            {
                string strURLWithData = "SoilInvestigation_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Rain Water Harvesting")
            {
                string strURLWithData = "RWH_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Core Cutting")
            {
                string strURLWithData = "CoreCutting_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }
            else if (lblSelectedInward.Text == "Mix Design")
            {
                string strURLWithData = "MixDesign_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", lblEnqId.Text, ""));
                Response.Redirect(strURLWithData);
            }

        }

    }
}
