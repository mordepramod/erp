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
    public partial class Enquiry : System.Web.UI.Page
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
                        if (arrIndMsg[1].ToString() == "VerifyClient")
                        {
                            lblEnqType.Text = "VerifyClient";
                            lblTempEnqId.Text = lblEnqId.Text;
                        }
                        else if (arrIndMsg[1].ToString() == "VerifyClientApp")
                        {
                            lblEnqType.Text = "VerifyClientApp";
                            lblTempEnqId.Text = lblEnqId.Text;

                            arrIndMsg = arrMsgs[2].Split('=');
                            Session["CL_ID"] = Convert.ToInt32(arrIndMsg[1].ToString().Trim());

                            arrIndMsg = arrMsgs[3].Split('=');
                            Session["SITE_ID"] = Convert.ToInt32(arrIndMsg[1].ToString().Trim());

                        }
                        else
                        {
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
                }
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Add Enquiry";
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
                        if (u.USER_SuperAdmin_right_bit == true)
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
                else
                {
                    LoadInwardList();
                    LoadLocation();
                    LoadDriverName();
                    txt_Client.Focus();
                    Tab_To_be_Collected.CssClass = "Clicked";
                    MainView_EnquiryStatus.ActiveViewIndex = 0;
                    if (lblEnqId.Text != "")
                    {
                        if (Convert.ToInt32(lblEnqId.Text) > 0)
                        {
                            EditEnquiry();
                        }
                    }
                }
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
        private void LoadRoutename()
        {
            ddlRouteName.DataTextField = "ROUTE_Name_var";
            ddlRouteName.DataValueField = "ROUTE_Id";
            var LocationRoute = dc.Route_View(0, "", "False", Convert.ToInt32(ddl_Location.SelectedValue));
            ddlRouteName.DataSource = LocationRoute;
            ddlRouteName.DataBind();
            ddlRouteName.Items.Insert(0, "----------------------------Select----------------------------");
        }

        protected void lnkSaveEnquiry_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                int clientId = 0, siteId = 0, contactPerId = 0;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                bool valid = false;
                if (chkNewClient.Checked == true)
                {
                    valid = true;
                    lblBalanceAmt.Text = "0.00";
                    lblBelow90BalanceAmt.Text = "0.00";
                    lblAbove90BalanceAmt.Text = "0.00";
                    lblLimitAmt.Text = "0.00";
                }
                else if (ChkClientName(txt_Client.Text) == true && ChkSiteName(txt_Site.Text) == true) // && ChkContactName(txt_ContactPerson.Text) == true)
                {
                    valid = true;
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
                    //if previous enquiry available then dont add new enquiry for selected record type
                    //if (lblEnqId.Text == "")
                    //{
                    //    var chkEnq = dc.Enquiry_View_Previous(clientId, siteId, Convert.ToInt32(ddl_InwardType.SelectedValue));
                    //    if (chkEnq.Count() > 0)
                    //    {
                    //        valid = false;
                    //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Enquiry for selected client, site and Test Type already recorded, can not add new enquiry." + "');", true);
                    //    }
                    //}
                }

                if (valid == true && ValidateEnqStatus() == true)
                {

                    DateTime ENQ_Date_dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    string OpenEnQuiryStatus = string.Empty; // , mailIdCC = "", mailIdTo = ""; ;
                    string ENQ_MaterialSendingOnDate_dt = string.Empty;
                    string ENQ_TestingDate_dt = string.Empty;
                    string ENQ_CollectionDate_dt = string.Empty;
                    bool ENQ_UrgentStatus_bit = false;
                    string comment = string.Empty;
                    string ENQ_ClientExpectedDate_dt = string.Empty;
                    string ENQ_CollectedAt_var = string.Empty;
                    byte ENQ_CRLimitApprStatus_tint = 0, ENQ_OrderStatus_bit = 0;
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
                                if (lblCouponCount.Text == "" || Convert.ToInt32(lblCouponCount.Text) < 3)
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
                        //OpenEnQuiryStatus = Tab_To_be_Collected.Text;
                        ENQ_OrderStatus_bit = 1;
                        OpenEnQuiryStatus = "To be Collected";
                        ENQ_CollectionDate_dt = txt_CollectionDate.Text;//need to change
                        CollectionDate = DateTime.ParseExact(txt_CollectionDate.Text, "dd/MM/yyyy", null);
                    }
                    else if (MainView_EnquiryStatus.ActiveViewIndex == 1)
                    {
                        //OpenEnQuiryStatus = Tab_Already_Collected.Text;
                        ENQ_OrderStatus_bit = 1;
                        OpenEnQuiryStatus = "Already Collected";
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
                        //OpenEnQuiryStatus = Tab_Others.Text;
                        OpenEnQuiryStatus = "Others";
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
                            ENQ_OrderStatus_bit = 2;
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
                            ENQ_OrderStatus_bit = 1;
                            OpenEnQuiryStatus = Label29.Text;
                        }
                        else if (Rdn_MaterialSendingOn_Date.Checked == true)
                        {
                            ENQ_OrderStatus_bit = 1;
                            OpenEnQuiryStatus = Label30.Text;
                        }
                    }
                    if (Rdn_DecisionPending.Checked == true)
                    {
                        ENQ_OrderStatus_bit = 2;
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
                        ENQ_OrderStatus_bit = 1;
                        ENQ_TestingDate_dt = txt_OnsiteTesting_Date.Text;
                        TestingDate = DateTime.ParseExact(txt_OnsiteTesting_Date.Text, "dd/MM/yyyy", null);//need to change
                    }
                    else if (Rdn_MaterialSendingOn_Date.Checked)
                    {
                        ENQ_OrderStatus_bit = 1;
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
                        if (clientId == 0)
                        {
                            int EnqId = 0;
                            EnQ_No = dc.EnquiryNewClient_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, Convert.ToInt32(ddl_InwardType.SelectedValue), Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, Convert.ToDecimal(txt_Quantity.Text), Convert.ToInt32(Session["LoginId"]), txt_Client.Text.Trim(), txt_Site.Text.Trim(), txt_Site_address.Text.Trim(), txt_Site_LandMark.Text.Trim(), txt_ContactPerson.Text.Trim(), txt_ContactNo.Text.Trim(), "", "", false, false, Convert.ToInt32(ENQ_OrderStatus_bit), "", Convert.ToString(txtClientAddress.Text));
                            lblEnqId.Text = EnQ_No.ToString();
                            lblMsg.Text = "Record is Saved Successfully. " + " " + " Your Enquiry No is " + " " + (lblEnqId.Text).ToString();
                            lnkSaveEnquiry.Visible = false;
                            lnk_Inward.Visible = false;
                            lblSelectedInward.Text = ddl_InwardType.SelectedItem.Text;
                            Cleartextbox();
                        }
                        else
                        {
                            //dc.Contact_Update(Convert.ToInt32(Session["CONTPersonID"]), Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), txt_ContactPerson.Text, txt_ContactNo.Text, "", false);
                            dc.Contact_Update(contactPerId, clientId, siteId, txt_ContactPerson.Text, txt_ContactNo.Text, "", false);
                            //ShowCONT_Id();
                            //var c = dc.Contact_View(0, Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CL_ID"]), txt_ContactPerson.Text);
                            if (contactPerId == 0)
                            {
                                var c = dc.Contact_View(0, siteId, clientId, txt_ContactPerson.Text);
                                foreach (var cn in c)
                                {
                                    txt_ContactNo.Text = Convert.ToString(cn.CONT_ContactNo_var);
                                    Session["CONTPersonID"] = cn.CONT_Id;
                                    contactPerId = cn.CONT_Id;
                                }
                            }
                            int EnqId = 0, tempEnqId = 0; string strEnqId = "";
                            if (lblEnqId.Text == "")
                            {
                                EnqId = 0;
                            }
                            else
                            {
                                EnqId = Convert.ToInt32(lblEnqId.Text);
                            }
                            //Approve auto enquiry - if cr limit
                            //DateTime? EditedCollectionDate = null;
                            DateTime? ApproveDate = null;
                            //byte enqStatus = 0;
                            ApproveDate = DateTime.Now;
                            //////if (OpenEnQuiryStatus == "To be Collected")
                            //////    enqStatus = 0;
                            //////else
                            //////    enqStatus = 1;

                            //dc.Enquiry_Update_Approve(Convert.ToInt32(grdEnquiry.Rows[i].Cells[12].Text), enqStatus, txtReasonForApprove.Text, EditedCollectionDate, ApproveDate, Convert.ToInt32(Session["LoginId"]));
                            //

                            if (lblEnqType.Text == "VerifyClientApp")
                            {
                                for (int i = 0; i < grdInwardType.Rows.Count; i++)
                                {
                                    Label lblMaterialId = (Label)grdInwardType.Rows[i].FindControl("lblMaterialId");
                                    Label lblMaterialName = (Label)grdInwardType.Rows[i].FindControl("lblMaterialName");
                                    Label lblMaterialQuantity = (Label)grdInwardType.Rows[i].FindControl("lblMaterialQuantity");

                                    if (ENQ_CRLimitApprStatus_tint != 3)
                                    {
                                        ENQ_CRLimitApprStatus_tint = 0;
                                        if (Convert.ToDecimal(lblBalanceAmt.Text) >= Convert.ToDecimal(lblLimitAmt.Text)) //&& MainView_EnquiryStatus.ActiveViewIndex != 1)
                                        {
                                            if (lblMaterialName.Text == "Cube Testing")
                                            {
                                                if (Convert.ToInt32(lblCouponCount.Text) < 3)
                                                    ENQ_CRLimitApprStatus_tint = 1;
                                            }
                                            else if (lblMaterialName.Text == "Soil Investigation")
                                            {
                                                ENQ_CRLimitApprStatus_tint = 0;
                                            }
                                            else
                                            {
                                                ENQ_CRLimitApprStatus_tint = 1;
                                            }
                                        }
                                    }

                                    tempEnqId = dc.Enquiry_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, Convert.ToInt32(lblMaterialId.Text), clientId, siteId, contactPerId, Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, ENQ_CRLimitApprStatus_tint, Convert.ToDecimal(lblMaterialQuantity.Text), Convert.ToInt32(Session["LoginId"]), "", "", false, false, Convert.ToInt32(ENQ_OrderStatus_bit), 0);
                                    //mail after enquiry registered
                                    //if (Convert.ToDecimal(lblBalanceAmt.Text) <= Convert.ToDecimal(lblLimitAmt.Text))
                                    //{
                                    //    SendMailAfterEnquiryRegistered(Convert.ToString(tempEnqId), lblMaterialName.Text, mailIdTo, mailIdCC, ENQ_Date_dt);
                                    //}

                                    //if enq is to be collected then auto approved it
                                    //if (enqStatus == 0)
                                    //    AutoApprovalOfEnquiry(Convert.ToInt32(tempEnqId), enqStatus, ApproveDate, EditedCollectionDate);


                                    if (strEnqId == "")
                                        strEnqId = ",";
                                    strEnqId += tempEnqId.ToString() + ",";
                                }
                            }
                            else
                            {
                                tempEnqId = dc.Enquiry_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, Convert.ToInt32(ddl_InwardType.SelectedValue), clientId, siteId, contactPerId, Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, ENQ_CRLimitApprStatus_tint, Convert.ToDecimal(txt_Quantity.Text), Convert.ToInt32(Session["LoginId"]), "", "", false, false, Convert.ToInt32(ENQ_OrderStatus_bit), 0);
                                //if (ENQ_CRLimitApprStatus_tint != 0)
                                //{
                                //    dc.Enquiry_Update_Approve(tempEnqId, enqStatus, "---", EditedCollectionDate, ApproveDate, Convert.ToInt32(Session["LoginId"]));
                                //}
                            }
                            dc.Site_Update(siteId, clientId, txt_Site.Text, 0, txt_Site_address.Text, "", 0, 0, txt_Site_LandMark.Text, Convert.ToInt32(ddl_Location.SelectedValue), false, true, "", 0, "", "", "", null, "", false, false, "", "", "", 0);
                            if (lblEnqType.Text == "VerifyClient")
                            {
                                dc.EnquiryNewClient_Update_Status(Convert.ToInt32(lblTempEnqId.Text), true, tempEnqId.ToString());
                            }
                            else if (lblEnqType.Text == "VerifyClientApp")
                            {
                                dc.EnquiryApp_Update_Status(Convert.ToInt32(lblTempEnqId.Text), 1, strEnqId);
                            }
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

                                // sms sending
                                //clsSendMail objcls = new clsSendMail();
                                //if (strEnqId != "")
                                //{
                                //    if (strEnqId.Contains(",") == true)
                                //        comment = "Dear customer your enquiry has been registered with us. your enquiry numbers are " + strEnqId + " Thank you.";
                                //    else
                                //        comment = "Dear customer your enquiry has been registered with us. your enquiry number is " + strEnqId + " Thank you.";
                                //}
                                //else
                                //{
                                //    strEnqId = EnQ_No.ToString();
                                //    comment = "Dear customer your enquiry for " + ddl_InwardType.SelectedItem.Text + " has been registered with us. your enquiry number is " + EnQ_No.ToString() + " Thank you.";
                                //}
                               // objcls.sendSMS(txt_ContactNo.Text, comment, "DUROCR","");

                                //if enq is to be collected then auto approved it
                                //if (!strEnqId.Contains(','))
                                //{
                                //    //sms for RedTagClient
                                //    //redTagCL = chkRedTagClient(Convert.ToInt32(strEnqId));
                                //    //if(redTagCL == 1)
                                //    //    objcls.sendSMS(txt_Contact_No.Text, "Dear Customer, Thank you for your enquiry. Please clear your outstanding for better service response.", "DUROCR");

                                //    AutoApprovalOfEnquiry(Convert.ToInt32(strEnqId), enqStatus, ApproveDate, EditedCollectionDate);
                                //    //mail after enquiry registered
                                //    if (Convert.ToDecimal(lblBalanceAmt.Text) <= Convert.ToDecimal(lblLimitAmt.Text))
                                //    {
                                //        SendMailAfterEnquiryRegistered(strEnqId, ddl_InwardType.SelectedItem.Text, mailIdTo, mailIdCC, ENQ_Date_dt);
                                //    }

                                //}
                                //credit limit exceeding email
                                //if (Convert.ToDecimal(lblBalanceAmt.Text) > 200)
                                //{
                                if (Convert.ToDecimal(lblBalanceAmt.Text) >= Convert.ToDecimal(lblLimitAmt.Text))
                                    {
                                        if (lblEnqType.Text == "VerifyClientApp")
                                        {
                                            string[] creditEnq = strEnqId.Split(',');
                                            for (int i = 0; i < grdInwardType.Rows.Count; i++)
                                            {
                                                Label lblMaterialName = (Label)grdInwardType.Rows[i].FindControl("lblMaterialName");
                                                if (lblMaterialName.Text != "Cube Testing" ||
                                                    (lblMaterialName.Text == "Cube Testing" && Convert.ToInt32(lblCouponCount.Text) == 0))
                                                {
                                                    CRLimitExceedEmail(creditEnq[i + 1], lblMaterialName.Text);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ddl_InwardType.SelectedItem.Text != "Cube Testing" ||
                                            (ddl_InwardType.SelectedItem.Text == "Cube Testing" && Convert.ToInt32(lblCouponCount.Text) == 0))
                                            {
                                                CRLimitExceedEmail(strEnqId, ddl_InwardType.SelectedItem.Text);
                                            }
                                        }
                                    }
                                //}

                                lblMsg.Text = "Record is Saved Successfully. " + " " + " Your Enquiry No is " + " " + strEnqId;
                                //

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

                            if (MainView_EnquiryStatus.ActiveViewIndex == 1) // && ENQ_CRLimitApprStatus_tint == 0)
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
                    }
                    catch (Exception ex)
                    {
                        string ms = ex.Message;
                    }

                }
            }
        }

        public void CRLimitExceedEmail(string enquiryId, string inwardType)
        {
            bool addMailId = true;
            string mailIdTo = "", mailIdCc = "", tempMailId = "", strAllMailId = "", strMEName = "", strMEContactNo = "";
            string strOutstndingAmt = "0", strEnqAmount = "0", strEnqNo = "", strEnqDate = "";
            int siteRouteId = 0, siteMeId = 0;
            var enquiry = dc.MailIdForCRLExceedEnquiry_View(Convert.ToInt32(enquiryId));
            foreach (var enq in enquiry)
            {
                strMEName = enq.MEName;
                strMEContactNo = enq.MEContactNo;
                if (enq.MEContactNo == null || enq.MEContactNo == "")
                    strMEContactNo = enq.MEMailId;
                strEnqDate = Convert.ToDateTime(enq.ENQ_Date_dt).ToString("dd/MM/yyyy");
                if (Convert.ToString(enq.CL_BalanceAmt_mny) != "" && Convert.ToString(enq.CL_BalanceAmt_mny) != null)
                    strOutstndingAmt = Convert.ToString(enq.CL_BalanceAmt_mny);
                strEnqAmount = Convert.ToString(enq.Proposal_NetAmount);
                strEnqNo = Convert.ToString(enq.ENQ_Id);
                tempMailId = enq.CL_AccEmailId_var.Trim();
                if (Convert.ToString(enq.SITE_Route_Id) != "" && Convert.ToString(enq.SITE_Route_Id) != null)
                    siteRouteId = Convert.ToInt32(enq.SITE_Route_Id);
                if (Convert.ToString(enq.MEId) != "" && Convert.ToString(enq.MEId) != null)
                    siteMeId = Convert.ToInt32(enq.MEId);

                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.CL_DirectorEmailID_var.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.CL_EmailID_var.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.SITE_EmailID_var.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.EnteredByMailId.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdCc != "")
                        mailIdCc += ",";
                    mailIdCc += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.MEMailId.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdCc != "")
                        mailIdCc += ",";
                    mailIdCc += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }
            }

            if (mailIdTo != "")
            {

                clsSendMail objMail = new clsSendMail();
                string mSubject = "", mbody = "", mReplyTo = "";
                //mailIdTo = "shital.bandal@gmail.com";
                //mailIdCc = "";
                mSubject = "Enquiry Confirmation";
                mbody = "Dear Customer,<br><br>";
                mbody = mbody + "We thank you for your enquiry (no. " + strEnqNo + ") on date " + strEnqDate + ". Your outstanding amount is " + strOutstndingAmt + ". We can arrange material pickup once we recieve the order confirmation along with payment against outstanding dues. <br><br>";
                mbody = mbody + "Please ignore this if payment is already made. <br><br>";

                mbody = mbody + "Please call " + strMEName + " on " + strMEContactNo + " for more details.";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>";
                mbody = mbody + "Best Regards,";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>";
                mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";
            
                try
                {
                    objMail.SendMail(mailIdTo, mailIdCc, mSubject, mbody, "", mReplyTo);
                    //update enq-outstanding mail count in route table of that ME
                    if (siteRouteId != 0 && siteMeId != 0)
                        dc.Route_Update_OutsandingMailCount(siteRouteId, siteMeId, 0);

                }
                catch { }
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

        private Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;

            if (MainView_EnquiryStatus.ActiveViewIndex == 3)
            {
                MainView_EnquiryStatus.ActiveViewIndex = Convert.ToInt32(Session["ActiveTabIndex"]);
                Tab_To_be_Collected.CssClass = "Initial";
                Tab_Already_Collected.CssClass = "Initial";
                Tab_Others.CssClass = "Initial";
                Tab_TestType.CssClass = "Initial";
                if (MainView_EnquiryStatus.ActiveViewIndex == 0)
                {
                    Tab_To_be_Collected.CssClass = "Clicked";
                }
                else if (MainView_EnquiryStatus.ActiveViewIndex == 1)
                {
                    Tab_Already_Collected.CssClass = "Clicked";
                }
                else if (MainView_EnquiryStatus.ActiveViewIndex == 2)
                {
                    Tab_Others.CssClass = "Clicked";
                }
            }
            if (txt_Client.Text == "" || (Convert.ToInt32(Session["CL_ID"]) == 0 && lblEnqType.Text == "VerifyClient"))
            {
                lblMsg.Text = "Please Select the Client from list";
                txt_Client.Text = "";
                txt_Client.Focus();
                valid = false;
            }
            else if (txt_Site.Text == "" || (Convert.ToInt32(Session["SITE_ID"]) == 0 && lblEnqType.Text == "VerifyClient"))
            {
                lblMsg.Text = "Please Select the Site from list";
                txt_Site.Text = "";
                txt_Site.Focus();
                valid = false;
            }
            else if (txt_Site_address.Text == "")
            {
                lblMsg.Text = "Please enter the site address";
                valid = false;
            }
            else if (txt_Site_LandMark.Text == "")
            {
                lblMsg.Text = "Please enter the site landmark";
                valid = false;
            }
            else if (txt_ContactPerson.Text == "")
            {
                lblMsg.Text = "Please Select the Contact Person";
                valid = false;
            }
            else if (txt_ContactNo.Text == "")
            {
                lblMsg.Text = "Please Select the Contact Number";
                valid = false;
            }
            else if (ddl_InwardType.SelectedValue == "----------------------------Select----------------------------" && lblEnqType.Text != "VerifyClientApp")
            {
                lblMsg.Text = "Please Select the Test Type";
                valid = false;
            }
            else if (txt_Quantity.Text == "" && lblEnqType.Text != "VerifyClientApp")
            {
                lblMsg.Text = "Please enter quantity";
                valid = false;
            }
            else if (MainView_EnquiryStatus.ActiveViewIndex == 0)
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
            else if (MainView_EnquiryStatus.ActiveViewIndex == 1 && valid == true)
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
            else if (MainView_EnquiryStatus.ActiveViewIndex == 2 && valid == true)
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

        protected void ddl_Location_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddlRouteName.Enabled = true;
            Tab_To_be_Collected.CssClass = "Clicked";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            Tab_TestType.CssClass = "Initial";
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

        protected void ddlRouteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayDateRouteWise();
        }

        private void ddl_LocationSelectChanged()
        {
            Tab_To_be_Collected.CssClass = "Clicked";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            Tab_TestType.CssClass = "Initial";
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

        private void DisplayDateRouteWise()
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

        private void EditEnquiry()
        {
            if (Convert.ToInt32(lblEnqId.Text) > 0)
            {
                if (lblEnqType.Text == "VerifyClientApp")
                {
                    Tab_TestType.Visible = true;
                    chkNewClient.Visible = false;
                    lblEnqId.Text = "";
                    var viewEnquiry = dc.EnquiryApp_View(Convert.ToInt32(lblTempEnqId.Text),-1);
                    foreach (var Enqry in viewEnquiry)
                    {
                        Session["CL_ID"] = Convert.ToInt32(Enqry.ENQ_CL_Id);
                        Session["SITE_ID"] = Convert.ToInt32(Enqry.ENQ_SITE_Id);
                        Session["CONTPersonID"] = Convert.ToInt32(Enqry.ENQ_CONT_Id);

                        bool enqApprRight = false;
                        var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                        foreach (var u in user)
                        {
                            if (u.USER_EnqApprove_right_bit == true)
                            {
                                enqApprRight = true;
                            }
                        }

                        if (enqApprRight == false)
                        {
                            txt_Client.ReadOnly = true;
                            txt_Site.ReadOnly = true;
                            //txt_ContactPerson.ReadOnly = true;
                            txt_Site_address.ReadOnly = true;
                            txt_Site_LandMark.ReadOnly = true;
                            //txt_ContactNo.ReadOnly = true;
                            AutoCompleteExtender1.Enabled = false;
                            AutoCompleteExtender2.Enabled = false;
                            //AutoCompleteExtender3.Enabled = false;
                        }

                        txt_Client.Text = Enqry.CL_Name_var.ToString();
                        txtClientAddress.Text = Enqry.CL_OfficeAddress_var;
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
                        //txt_ContactPerson.Text = Enqry.CONT_Name_var.ToString();
                        //txt_ContactNo.Text = Enqry.CONT_ContactNo_var.ToString();
                        //ddl_InwardType.SelectedValue = Enqry.MATERIAL_Id.ToString();
                        //txt_Quantity.Text = Enqry.ENQ_Quantity.ToString();
                        //txt_Note.Text = Enqry.ENQ_Note_var.ToString();
                        //txt_Reference.Text = Enqry.ENQ_Reference_var.ToString();
                        //if (Enqry.ENQ_OpenEnquiryStatus_var == "To be Collected")
                        //{
                        //    Tab_To_be_Collected.CssClass = "Clicked";
                        //    Tab_Already_Collected.CssClass = "Initial";
                        //    Tab_Others.CssClass = "Initial";
                        //    MainView_EnquiryStatus.ActiveViewIndex = 0;

                        //    ddl_Location.SelectedValue = Enqry.ENQ_LOCATION_Id.ToString();
                        //    LoadRoutename();
                        //    ddlRouteName.SelectedValue = Enqry.ENQ_ROUTE_Id.ToString();
                        //    ddlRouteName.Enabled = true;
                        //    txt_CollectionDate.Text = Convert.ToDateTime(Enqry.ENQ_CollectionDate_dt).ToString("dd/MM/yyyy");

                        //    if (Enqry.ENQ_UrgentStatus_bit == true)
                        //    {
                        //        chkbox_Urgent.Checked = true;
                        //        txt_ClientExpected_Date.Text = Convert.ToDateTime(Enqry.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                        //        Lbl_Client_Expected_Date.Visible = true;
                        //        txt_ClientExpected_Date.Visible = true;
                        //    }
                        //}

                        //else if (Enqry.ENQ_OpenEnquiryStatus_var == "Already Collected")
                        //{
                        //    Tab_To_be_Collected.CssClass = "Initial";
                        //    Tab_Already_Collected.CssClass = "Clicked";
                        //    Tab_Others.CssClass = "Initial";
                        //    MainView_EnquiryStatus.ActiveViewIndex = 1;

                        //    if (Enqry.ENQ_CollectedAt_var == "By Driver")
                        //    {
                        //        Rdn_ByDriver.Checked = true;
                        //        ddl_DriverName.SelectedValue = Enqry.ENQ_DriverId.ToString();
                        //        ddl_DriverName.Enabled = true;
                        //    }
                        //    else if (Enqry.ENQ_CollectedAt_var == "At Lab")
                        //    {
                        //        Rdn_AtLab.Checked = true;
                        //        ddl_DriverName.Enabled = false;
                        //    }
                        //}
                        //else if (Enqry.ENQ_OpenEnquiryStatus_var == "Decision Pending")
                        //{
                        //    Tab_To_be_Collected.CssClass = "Initial";
                        //    Tab_Already_Collected.CssClass = "Initial";
                        //    Tab_Others.CssClass = "Clicked";
                        //    MainView_EnquiryStatus.ActiveViewIndex = 2;
                        //    Rdn_DecisionPending.Checked = true;
                        //    txt_CommentDecisionPending.Text = Enqry.ENQ_Comment_var.ToString();
                        //    txt_CommentDecisionPending.Enabled = true;
                        //    txt_CommentDeclinebyUs.Enabled = false;
                        //    txt_CommentRejectedByClient.Enabled = false;
                        //    chkRateDiiference.Enabled = false;
                        //    txt_OnsiteTesting_Date.Enabled = false;
                        //    txt_MaterialSendingOnDate.Enabled = false;
                        //}
                        //else if (Enqry.ENQ_OpenEnquiryStatus_var == "On site Testing")
                        //{
                        //    Tab_To_be_Collected.CssClass = "Initial";
                        //    Tab_Already_Collected.CssClass = "Initial";
                        //    Tab_Others.CssClass = "Clicked";
                        //    MainView_EnquiryStatus.ActiveViewIndex = 2;
                        //    Rdn_OnsiteTesting.Checked = true;
                        //    txt_OnsiteTesting_Date.Text = Convert.ToDateTime(Enqry.ENQ_TestingDate_dt).ToString("dd/MM/yyyy");
                        //    txt_OnsiteTesting_Date.Enabled = true;
                        //    txt_CommentDecisionPending.Enabled = false;
                        //    txt_CommentDeclinebyUs.Enabled = false;
                        //    txt_CommentRejectedByClient.Enabled = false;
                        //    chkRateDiiference.Enabled = false;
                        //    txt_MaterialSendingOnDate.Enabled = false;
                        //}
                        //else if (Enqry.ENQ_OpenEnquiryStatus_var == "Declined by us")
                        //{
                        //    Tab_To_be_Collected.CssClass = "Initial";
                        //    Tab_Already_Collected.CssClass = "Initial";
                        //    Tab_Others.CssClass = "Clicked";
                        //    MainView_EnquiryStatus.ActiveViewIndex = 2;
                        //    Rdn_DeclienedbyUs.Checked = true;
                        //    txt_CommentDeclinebyUs.Text = Enqry.ENQ_Comment_var.ToString();
                        //    txt_CommentDeclinebyUs.Enabled = true;
                        //    txt_CommentDecisionPending.Enabled = false;
                        //    txt_CommentRejectedByClient.Enabled = false;
                        //    chkRateDiiference.Enabled = false;
                        //    txt_OnsiteTesting_Date.Enabled = false;
                        //    txt_MaterialSendingOnDate.Enabled = false;
                        //}
                        //else if (Enqry.ENQ_OpenEnquiryStatus_var == "Rejected By Client" || Enqry.ENQ_OpenEnquiryStatus_var == "Declined By Client")
                        //{
                        //    Tab_To_be_Collected.CssClass = "Initial";
                        //    Tab_Already_Collected.CssClass = "Initial";
                        //    Tab_Others.CssClass = "Clicked";
                        //    MainView_EnquiryStatus.ActiveViewIndex = 2;
                        //    Rdn_RejectedByClient.Checked = true;
                        //    txt_CommentRejectedByClient.Text = Enqry.ENQ_Comment_var.ToString();
                        //    txt_CommentRejectedByClient.Enabled = true;
                        //    chkRateDiiference.Enabled = true;
                        //    if (txt_CommentRejectedByClient.Text.ToLower().Contains("rate difference") == true)
                        //        chkRateDiiference.Checked = true;
                        //    txt_CommentDecisionPending.Enabled = false;
                        //    txt_CommentDeclinebyUs.Enabled = false;
                        //    txt_OnsiteTesting_Date.Enabled = false;
                        //    txt_MaterialSendingOnDate.Enabled = false;
                        //}
                        //else if (Enqry.ENQ_OpenEnquiryStatus_var == "Material Sending On Date")
                        //{
                        //    Tab_To_be_Collected.CssClass = "Initial";
                        //    Tab_Already_Collected.CssClass = "Initial";
                        //    Tab_Others.CssClass = "Clicked";
                        //    MainView_EnquiryStatus.ActiveViewIndex = 2;
                        //    Rdn_MaterialSendingOn_Date.Checked = true;
                        //    txt_MaterialSendingOnDate.Text = Convert.ToDateTime(Enqry.ENQ_MaterialSendingOnDate_dt).ToString("dd/MM/yyyy");
                        //    txt_MaterialSendingOnDate.Enabled = true;
                        //    txt_CommentDecisionPending.Enabled = false;
                        //    txt_CommentDeclinebyUs.Enabled = false;
                        //    txt_CommentRejectedByClient.Enabled = false;
                        //    chkRateDiiference.Enabled = false;
                        //    txt_OnsiteTesting_Date.Enabled = false;

                        //}
                    }
                    var material = dc.EnquiryApp_Material_View(Convert.ToInt32(lblTempEnqId.Text));
                    grdInwardType.DataSource = material;
                    grdInwardType.DataBind();

                    DisplayBalLmt();
                    DisplayDiscount();
                }
                else if (lblEnqType.Text == "VerifyClient")
                {
                    chkNewClient.Visible = false;
                    lblEnqId.Text = "";
                    var viewEnquiry = dc.EnquiryNewClient_View(Convert.ToInt32(lblTempEnqId.Text), false, 0);
                    foreach (var Enqry in viewEnquiry)
                    {
                        Session["CL_ID"] = 0;
                        Session["SITE_ID"] = 0;
                        Session["CONTPersonID"] = 0;

                        txt_Client.Text = Enqry.ENQNEW_ClientName_var;
                        //txtClientAddress.Text = Enqry.CL_OfficeAddress_var;
                        txt_Site.Text = Enqry.ENQNEW_SiteName_var;
                        txt_Site_address.Text = Enqry.ENQNEW_SiteAddress_var;
                        txt_Site_LandMark.Text = Enqry.ENQNEW_SiteLandmark_var;
                        txt_ContactPerson.Text = Enqry.ENQNEW_ContactPersonName_var;
                        txt_ContactNo.Text = Enqry.ENQNEW_ContactNo_var;
                        ddl_InwardType.SelectedValue = Enqry.MATERIAL_Id.ToString();
                        txt_Quantity.Text = Enqry.ENQNEW_Quantity.ToString();
                        txt_Note.Text = Enqry.ENQNEW_Note_var;
                        txt_Reference.Text = Enqry.ENQNEW_Reference_var;
                        if (Enqry.ENQNEW_OpenEnquiryStatus_var == "To be Collected")
                        {
                            Tab_To_be_Collected.CssClass = "Clicked";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Initial";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 0;

                            ddl_Location.SelectedValue = Enqry.ENQNEW_LOCATION_Id.ToString();
                            LoadRoutename();
                            ddlRouteName.SelectedValue = Enqry.ENQNEW_ROUTE_Id.ToString();
                            ddlRouteName.Enabled = true;
                            txt_CollectionDate.Text = Convert.ToDateTime(Enqry.ENQNEW_CollectionDate_dt).ToString("dd/MM/yyyy");

                            if (Enqry.ENQNEW_UrgentStatus_bit == true)
                            {
                                chkbox_Urgent.Checked = true;
                                txt_ClientExpected_Date.Text = Convert.ToDateTime(Enqry.ENQNEW_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                                Lbl_Client_Expected_Date.Visible = true;
                                txt_ClientExpected_Date.Visible = true;
                            }
                        }

                        else if (Enqry.ENQNEW_OpenEnquiryStatus_var == "Already Collected")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Clicked";
                            Tab_Others.CssClass = "Initial";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 1;

                            if (Enqry.ENQNEW_CollectedAt_var == "By Driver")
                            {
                                Rdn_ByDriver.Checked = true;
                                ddl_DriverName.SelectedValue = Enqry.ENQNEW_DriverId.ToString();
                                ddl_DriverName.Enabled = true;
                            }
                            else if (Enqry.ENQNEW_CollectedAt_var == "At Lab")
                            {
                                Rdn_AtLab.Checked = true;
                                ddl_DriverName.Enabled = false;
                            }
                        }
                        else if (Enqry.ENQNEW_OpenEnquiryStatus_var == "Decision Pending")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_DecisionPending.Checked = true;
                            txt_CommentDecisionPending.Text = Enqry.ENQNEW_Comment_var.ToString();
                            txt_CommentDecisionPending.Enabled = true;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQNEW_OpenEnquiryStatus_var == "On site Testing")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_OnsiteTesting.Checked = true;
                            txt_OnsiteTesting_Date.Text = Convert.ToDateTime(Enqry.ENQNEW_TestingDate_dt).ToString("dd/MM/yyyy");
                            txt_OnsiteTesting_Date.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQNEW_OpenEnquiryStatus_var == "Declined by us")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_DeclienedbyUs.Checked = true;
                            txt_CommentDeclinebyUs.Text = Enqry.ENQNEW_Comment_var.ToString();
                            txt_CommentDeclinebyUs.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQNEW_OpenEnquiryStatus_var == "Rejected By Client" || Enqry.ENQNEW_OpenEnquiryStatus_var == "Declined By Client")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_RejectedByClient.Checked = true;
                            txt_CommentRejectedByClient.Text = Enqry.ENQNEW_Comment_var.ToString();
                            txt_CommentRejectedByClient.Enabled = true;
                            chkRateDiiference.Enabled = true;
                            if (txt_CommentRejectedByClient.Text.ToLower().Contains("rate difference") == true)
                                chkRateDiiference.Checked = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQNEW_OpenEnquiryStatus_var == "Material Sending On Date" || Enqry.ENQNEW_OpenEnquiryStatus_var == "Delivered by Client")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_MaterialSendingOn_Date.Checked = true;
                            txt_MaterialSendingOnDate.Text = Convert.ToDateTime(Enqry.ENQNEW_MaterialSendingOnDate_dt).ToString("dd/MM/yyyy");
                            txt_MaterialSendingOnDate.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;

                        }
                    }
                    lblBalanceAmt.Text = "0.00";
                    lblBelow90BalanceAmt.Text = "0.00";
                    lblAbove90BalanceAmt.Text = "0.00";
                    lblLimitAmt.Text = "0.00";
                }
                else
                {
                    var viewEnquiry = dc.Enquiry_View(Convert.ToInt32(lblEnqId.Text), 1, 0);  // ENQ_Status_tint, "");
                    foreach (var Enqry in viewEnquiry)
                    {
                        Session["CL_ID"] = Convert.ToInt32(Enqry.ENQ_CL_Id);
                        Session["SITE_ID"] = Convert.ToInt32(Enqry.ENQ_SITE_Id);
                        Session["CONTPersonID"] = Convert.ToInt32(Enqry.ENQ_CONT_Id);

                        bool enqApprRight = false;
                        var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                        foreach (var u in user)
                        {
                            if (u.USER_EnqApprove_right_bit == true)
                            {
                                enqApprRight = true;
                            }
                        }

                        if (enqApprRight == false)
                        {
                            txt_Client.ReadOnly = true;
                            txt_Site.ReadOnly = true;
                            txt_ContactPerson.ReadOnly = true;
                            txt_Site_address.ReadOnly = true;
                            txt_Site_LandMark.ReadOnly = true;
                            txt_ContactNo.ReadOnly = true;
                            AutoCompleteExtender1.Enabled = false;
                            AutoCompleteExtender2.Enabled = false;
                            AutoCompleteExtender3.Enabled = false;
                        }

                        txt_Client.Text = Enqry.CL_Name_var.ToString();
                        txtClientAddress.Text = Enqry.CL_OfficeAddress_var;
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
                            Tab_TestType.CssClass = "Initial";
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
                            Tab_TestType.CssClass = "Initial";
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
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_DecisionPending.Checked = true;
                            txt_CommentDecisionPending.Text = Enqry.ENQ_Comment_var.ToString();
                            txt_CommentDecisionPending.Enabled = true;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQ_OpenEnquiryStatus_var == "On site Testing")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_OnsiteTesting.Checked = true;
                            txt_OnsiteTesting_Date.Text = Convert.ToDateTime(Enqry.ENQ_TestingDate_dt).ToString("dd/MM/yyyy");
                            txt_OnsiteTesting_Date.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQ_OpenEnquiryStatus_var == "Declined by us")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_DeclienedbyUs.Checked = true;
                            txt_CommentDeclinebyUs.Text = Enqry.ENQ_Comment_var.ToString();
                            txt_CommentDeclinebyUs.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQ_OpenEnquiryStatus_var == "Rejected By Client" || Enqry.ENQ_OpenEnquiryStatus_var == "Declined By Client")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_RejectedByClient.Checked = true;
                            txt_CommentRejectedByClient.Text = Enqry.ENQ_Comment_var.ToString();
                            txt_CommentRejectedByClient.Enabled = true;
                            chkRateDiiference.Enabled = true;
                            if (txt_CommentRejectedByClient.Text.ToLower().Contains("rate difference") == true)
                                chkRateDiiference.Checked = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQ_OpenEnquiryStatus_var == "Material Sending On Date" || Enqry.ENQ_OpenEnquiryStatus_var == "Delivered by Client")
                        {
                            Tab_To_be_Collected.CssClass = "Initial";
                            Tab_Already_Collected.CssClass = "Initial";
                            Tab_Others.CssClass = "Clicked";
                            Tab_TestType.CssClass = "Initial";
                            MainView_EnquiryStatus.ActiveViewIndex = 2;
                            Rdn_MaterialSendingOn_Date.Checked = true;
                            txt_MaterialSendingOnDate.Text = Convert.ToDateTime(Enqry.ENQ_MaterialSendingOnDate_dt).ToString("dd/MM/yyyy");
                            txt_MaterialSendingOnDate.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;

                        }
                    }
                    DisplayBalLmt();
                    DisplayDiscount();
                }
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
                        if (c.CL_BalanceAmt_mny != null)
                        {
                            lblBalanceAmt.Text = Convert.ToDecimal(c.CL_BalanceAmt_mny).ToString("0.00");
                        }
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
        private void DisplayDiscount()
        {
            int Cl_Id = 0;
            if (int.TryParse(Session["CL_ID"].ToString(), out Cl_Id))
            {
                if (Convert.ToInt32(Session["CL_ID"]) > 0)
                {
                    var clDisc = dc.DiscountSetting_View(Cl_Id, "");//view all discount of that client
                    foreach (var item in clDisc)
                    {
                        //lblDiscount.Text = "<b>Discount</b> - ";
                        //lblDiscount.Text += "<b>Introductory</b> : " + item.DISCOUNT_Introductory_num.ToString() + "%";
                        //lblDiscount.Text += ", <b>Volumn</b> : " + item.DISCOUNT_Volume_num.ToString() + "%";
                        //lblDiscount.Text += ", <b>Timely Payment</b> : " + item.DISCOUNT_TimelyPayment_num.ToString() + "%";
                        //lblDiscount.Text += ", <b>Advance</b> : " + item.DISCOUNT_Advance_num.ToString() + "%";
                        //lblDiscount.Text += ", <b>Loyalty</b> : " + item.DISCOUNT_Loyalty_num.ToString() + "%";
                        //lblDiscount.Text += ", <b>Proposal</b> : " + item.DISCOUNT_Proposal_num.ToString() + "%";
                        //lblDiscount.Text += ", <b>App</b> : " + item.DISCOUNT_App_num.ToString() + "%";
                        //lblDiscount.Text += ", <b>Applicable</b> : " + item.Applicable.ToString() + "%";
                        lblDiscountPer.Text = item.Applicable.ToString() + " %";
                        break;
                    }
                }
            }
        }
        private void VisibleOthersComment()
        {
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            chkRateDiiference.Enabled = false;
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
                chkRateDiiference.Enabled = true;
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

        private void Cleartextbox()
        {
            txt_Client.Text = string.Empty;
            txtClientAddress.Text = string.Empty;
            lblClientAddress.Visible = false;
            txtClientAddress.Visible = false;
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
            Tab_TestType.CssClass = "Initial";
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
            chkRateDiiference.Checked = false;
            Rdn_OnsiteTesting.Checked = false;
            txt_OnsiteTesting_Date.Text = string.Empty;
            Rdn_MaterialSendingOn_Date.Checked = false;
            txt_MaterialSendingOnDate.Text = string.Empty;
            Lbl_Client_Expected_Date.Visible = false;
            txt_ClientExpected_Date.Visible = false;
        }

        protected void Tab_To_be_Collected_Click(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Clicked";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            Tab_TestType.CssClass = "Initial";
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
            Tab_TestType.CssClass = "Initial";
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
            Tab_TestType.CssClass = "Initial";
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

        protected void Tab_TestType_Click(object sender, EventArgs e)
        {
            Session["ActiveTabIndex"] = MainView_EnquiryStatus.ActiveViewIndex;
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Initial";
            Tab_TestType.CssClass = "Clicked";
            MainView_EnquiryStatus.ActiveViewIndex = 3;
        }

        protected void Rdn_AtLab_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Clicked";
            Tab_Others.CssClass = "Initial";
            Tab_TestType.CssClass = "Initial";
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
            Tab_TestType.CssClass = "Initial";
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
            Tab_TestType.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_CommentDecisionPending.Enabled = true;
            txt_CommentDecisionPending.Focus();
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            chkRateDiiference.Enabled = false;
            chkRateDiiference.Checked = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            txt_CommentDecisionPending.Text = "";
            txt_CommentDeclinebyUs.Text = "";
            txt_CommentRejectedByClient.Text = "";
        }

        protected void Rdn_DeclienedbyUs_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            Tab_TestType.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_CommentDeclinebyUs.Enabled = true;
            txt_CommentDeclinebyUs.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            chkRateDiiference.Enabled = false;
            chkRateDiiference.Checked = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            txt_CommentDecisionPending.Text = "";
            txt_CommentDeclinebyUs.Text = "";
            txt_CommentRejectedByClient.Text = "";
        }

        protected void Rdn_RejectedByClient_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            Tab_TestType.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_CommentRejectedByClient.Enabled = true;
            chkRateDiiference.Enabled = true;
            chkRateDiiference.Checked = false;
            txt_CommentRejectedByClient.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_MaterialSendingOnDate.Enabled = false;
            txt_CommentDecisionPending.Text = "";
            txt_CommentDeclinebyUs.Text = "";
            txt_CommentRejectedByClient.Text = "";
        }

        protected void Rdn_OnsiteTesting_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            Tab_TestType.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_OnsiteTesting_Date.Enabled = true;
            this.txt_OnsiteTesting_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_OnsiteTesting_Date.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            chkRateDiiference.Enabled = false;
            chkRateDiiference.Checked = false;
            txt_MaterialSendingOnDate.Enabled = false;
            txt_CommentDecisionPending.Text = "";
            txt_CommentDeclinebyUs.Text = "";
            txt_CommentRejectedByClient.Text = "";
        }

        protected void Rdn_MaterialSendingOn_Date_CheckedChanged(object sender, EventArgs e)
        {
            Tab_To_be_Collected.CssClass = "Initial";
            Tab_Already_Collected.CssClass = "Initial";
            Tab_Others.CssClass = "Clicked";
            Tab_TestType.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 2;
            txt_MaterialSendingOnDate.Enabled = true;
            this.txt_MaterialSendingOnDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_MaterialSendingOnDate.Focus();
            txt_CommentDecisionPending.Enabled = false;
            txt_CommentDeclinebyUs.Enabled = false;
            txt_CommentRejectedByClient.Enabled = false;
            chkRateDiiference.Enabled = false;
            chkRateDiiference.Checked = false;
            txt_OnsiteTesting_Date.Enabled = false;
            txt_CommentDecisionPending.Text = "";
            txt_CommentDeclinebyUs.Text = "";
            txt_CommentRejectedByClient.Text = "";
        }

        private void EnableLocation()
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
        private void EnableDriverName()
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

        private void TickCheckkBox()
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
            Tab_TestType.CssClass = "Initial";
            MainView_EnquiryStatus.ActiveViewIndex = 0;
            if (chkbox_Urgent.Checked == true)
            {
                Lbl_Client_Expected_Date.Visible = true;
                txt_ClientExpected_Date.Visible = true;
                txt_ClientExpected_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
            else
            {
                Lbl_Client_Expected_Date.Visible = false;
                txt_ClientExpected_Date.Visible = false;
            }
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblLogin.Visible = false;
            lnkSaveEnquiry.Visible = true;
            lnk_Inward.Visible = false;
            txtClientAddress.Text = string.Empty;
            txt_Site.Text = string.Empty;
            txt_Site_address.Text = string.Empty;
            txt_Site_LandMark.Text = string.Empty;
            if (lblEnqType.Text != "VerifyClient")
            {
                txt_ContactPerson.Text = string.Empty;
                txt_ContactNo.Text = string.Empty;
            }
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "" && chkNewClient.Checked == false)
                {
                    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                    lblLogin.Visible = true;
                    string loginId = "", loginPwd = "";
                    var result = dc.Client_View(Convert.ToInt32(Session["CL_ID"]), 0, "", "");
                    foreach (var item in result)
                    {
                        loginId = item.CL_LoginId_var;
                        loginPwd = item.CL_Password_var;
                        break;
                    }

                    lblLogin.Text = "Login Id=" + loginId + " & Password=" + loginPwd + "";
                }
                else
                {
                    Session["CL_ID"] = 0;
                }
                if (txt_Client.Text == "" || txt_Site.Text == "")
                {
                    if (lblEnqType.Text != "VerifyClient")
                        txt_ContactPerson.Text = "";
                }
                DisplayBalLmt();
                DisplayDiscount();
                txt_Site.Focus();
                chkNewClient.Checked = false;
            }
            if (lblSelectedInward.Text != "")
            {
                lblEnqId.Text = "";
            }
        }
        protected void txt_Site_TextChanged(object sender, EventArgs e)
        {
            txt_Site_address.Text = string.Empty;
            txt_Site_LandMark.Text = string.Empty;
            if (lblEnqType.Text != "VerifyClient")
            {
                txt_ContactPerson.Text = string.Empty;
                txt_ContactNo.Text = string.Empty;
            }
            int cl_Id = 0;
            if (Convert.ToInt32(Session["CL_ID"]) > 0)
            {
                if (int.TryParse(Session["CL_ID"].ToString(), out cl_Id))
                {
                    if (ChkSiteName(txt_Site.Text) == true)
                    {
                        int SiteId = 0;
                        if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                        {
                            Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                        }
                        else
                        {
                            Session["SITE_ID"] = 0;
                        }
                        txt_ContactPerson.Focus();
                        if (txt_Client.Text == "" || txt_Site.Text == "")
                        {
                            if (lblEnqType.Text != "VerifyClient")
                                txt_ContactPerson.Text = "";
                        }
                        if (Convert.ToInt32(Session["SITE_ID"]) > 0)
                        {
                            var s = dc.Site_View(Convert.ToInt32(Session["SITE_ID"]), 0, 0, "");
                            foreach (var site in s)
                            {
                                txt_Site_address.Text = site.SITE_Address_var;
                                txt_Site_LandMark.Text = site.SITE_Landmark_var;
                                if (site.SITE_LocationId_int != null && site.SITE_LocationId_int > 0)
                                {
                                    ddl_Location.SelectedValue = site.SITE_LocationId_int.ToString();
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
                    }
                }
            }
        }
        protected void txt_ContactPerson_TextChanged(object sender, EventArgs e)
        {
            txt_ContactNo.Text = "";
            var c = dc.Contact_View(0, Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CL_ID"]), txt_ContactPerson.Text);
            foreach (var cn in c)
            {
                txt_ContactNo.Text = Convert.ToString(cn.CONT_ContactNo_var);
                Session["CONTPersonID"] = Convert.ToInt32(cn.CONT_Id);
            }
            txt_ContactNo.Focus();
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
                if (searchHead != "")
                {
                    txtClientAddress.Text = obj.CL_OfficeAddress_var;
                }
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
                Session["CL_ID"] = 0;
                Session["SITE_ID"] = 0;
                Session["CONTPersonID"] = 0;
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
            var res = dc.Site_View(0, Convert.ToInt32(Session["CL_ID"]), 0, searchHead);
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

        protected void lnkViewClientAddress_Click(object sender, EventArgs e)
        {
            if (lblClientAddress.Visible == false)
            {
                lblClientAddress.Visible = true;
                txtClientAddress.Visible = true;
            }
            else
            {
                lblClientAddress.Visible = false;
                txtClientAddress.Visible = false;
            }
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

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            if (lblEnqType.Text == "VerifyClient" || lblEnqType.Text == "VerifyClientApp")
                Response.Redirect("EnquiryVerifyClient.aspx");
            else
                Response.Redirect("Enquiry_List.aspx");
        }

        protected void chkRateDiiference_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRateDiiference.Checked == true)
            {
                if (txt_CommentRejectedByClient.Text.ToLower().Contains("rate difference") == false)
                    txt_CommentRejectedByClient.Text = "Rate Difference";
            }
            else
            {
                if (txt_CommentRejectedByClient.Text.ToLower().Contains("rate difference") == true)
                    txt_CommentRejectedByClient.Text = "";
            }

        }

        protected void lnl_ViewPrevEnq_Click(object sender, EventArgs e)
        {
            if (Session["CL_ID"] != null && Convert.ToInt32(Session["CL_ID"]) > 0
                && Session["SITE_ID"] != null && Convert.ToInt32(Session["SITE_ID"]) > 0)
            {
                ModalPopupExtender1.Show();
                var EnquiryStatus = dc.Enquiry_View_Previous(Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0);
                grdEnqList.DataSource = EnquiryStatus;
                grdEnqList.DataBind();
            }
        }
        protected void grdEnqList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //7- Enquiry status 4
                //8 - approved date 5
                //9 - collected on 6

                if (e.Row.Cells[4].Text != "" && e.Row.Cells[4].Text != "&nbsp;")
                {
                    if (Convert.ToInt32(e.Row.Cells[4].Text) == 2)
                    {
                        e.Row.Cells[4].Text = "Closed";
                    }
                    else if (e.Row.Cells[5].Text != "" && e.Row.Cells[5].Text != "&nbsp;"
                        && (Convert.ToInt32(e.Row.Cells[4].Text) == 0 || Convert.ToInt32(e.Row.Cells[4].Text) == 1))
                    {
                        e.Row.Cells[4].Text = "Approved";
                    }
                    else if (e.Row.Cells[6].Text != "" && e.Row.Cells[6].Text != "&nbsp;"
                        && (Convert.ToInt32(e.Row.Cells[4].Text) == 1))
                    {
                        e.Row.Cells[4].Text = "Collected";
                    }
                    else //if (Convert.ToInt32(e.Row.Cells[4].Text) < 2)
                    {
                        e.Row.Cells[4].Text = "Pending";
                    }
                }
            }
        }
        protected void imgCloseEnqList_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }

    }
}
