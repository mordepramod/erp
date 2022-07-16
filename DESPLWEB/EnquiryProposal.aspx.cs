using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace DESPLWEB
{
    public partial class EnquiryProposal : System.Web.UI.Page
    {
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        LabDataDataContext dc = new LabDataDataContext();
        //static double groupA = 0, groupB = 0, groupC = 0, calculatedDisc = 0, maxDiscnt = 0, appliedDisc = 0, totDisc = 0, totDiscA = 0, totDiscB = 0, introDiscA = 0, volDiscB = 0, timelyDiscC = 0, AdvDiscD = 0, loyDiscE = 0, propDiscF = 0, AppDiscG = 0;
        static double calculatedDisc = 0, maxDiscnt = 0, appliedDisc = 0, introDiscA = 0, volDiscB = 0, timelyDiscC = 0, AdvDiscD = 0, loyDiscE = 0, propDiscF = 0, AppDiscG = 0;
        static bool otFlag = false, chkNewClientFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.MaintainScrollPositionOnPostBack = false;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            if (chkNewClient.Checked)
                chkNewClientFlag = true;
            
            if (!IsPostBack)
            {
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    lblUserLevel.Text = u.USER_Level_tint.ToString();
                    break;
                }
                chkNewClientFlag = false;
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
                        //if (arrIndMsg[1].ToString() == "VerifyEnquiryApp")
                        //{
                        //    lblEnqType.Text = "VerifyEnquiryApp";
                        //    lblTempEnqId.Text = lblEnqId.Text;
                        //}
                        //else 
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
                lblheading.Text = "Add Enquiry & Proposal";

                LoadInwardList();
                LoadLocation();
                LoadDriverName();
                txt_Client.Focus();
                LoadUserList();
                lblEnqNo.Visible = false;
                if (lblEnqId.Text != "")
                {
                    if (Convert.ToInt32(lblEnqId.Text) > 0)
                    {
                        EditEnquiry();
                    }
                }

            }
        }
        #region enquiry
        private void LoadInwardList()
        {
            ddl_InwardType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardType.DataValueField = "MATERIAL_RecordType_var";
            var inwd = dc.Material_View("", "");
            ddl_InwardType.DataSource = inwd;
            ddl_InwardType.DataBind();
            //ddl_InwardType.Items.Insert(0, "----------------------------Select----------------------------");
            ddl_InwardType.Items.Insert(0, new ListItem("Coupon", "Coupon"));
           // ddl_InwardType.Items.Insert(0, new ListItem("Coupon-Steel", "STCoupon"));
            ddl_InwardType.Items.Insert(0, "----------------------------Select----------------------------");
        }
        private void LoadLocation()
        {
            ddl_Location.DataTextField = "LOCATION_Name_var";
            ddl_Location.DataValueField = "LOCATION_Id";
            var Loct = dc.Location_View(0, "", 0);
            ddl_Location.DataSource = Loct;
            ddl_Location.DataBind();
            ddl_Location.Items.Insert(0, new ListItem("-----------Select-----------", "0"));
        }
        private void LoadDriverName()
        {
            ddl_DriverName.DataTextField = "USER_Name_var";
            ddl_DriverName.DataValueField = "USER_Id";
            var Driver = dc.Driver_View(false);
            ddl_DriverName.DataSource = Driver;
            ddl_DriverName.DataBind();
            ddl_DriverName.Items.Insert(0, "-----------Select-----------");
        }
        private void LoadRoutename()
        {
            ddlRouteName.DataTextField = "ROUTE_Name_var";
            ddlRouteName.DataValueField = "ROUTE_Id";
            var LocationRoute = dc.Route_View(0, "", "False", Convert.ToInt32(ddl_Location.SelectedValue));
            ddlRouteName.DataSource = LocationRoute;
            ddlRouteName.DataBind();
            //ddlRouteName.Items.Insert(0, "----Select---");
        }
        private void EditEnquiry()
        {
            clsData cd = new clsData();
            if (Convert.ToInt32(lblEnqId.Text) > 0)
            {
                lblEnqNo.Visible = true; string priority = "", status = "Regular";
                lblEnqNo.Text = "Enquiry No : " + lblEnqId.Text;
               
                if (lblEnqType.Text == "VerifyClientApp")
                {
                    chkNewClient.Visible = false;
                    lblEnqId.Text = "";
                    var viewEnquiry = dc.EnquiryApp_View(Convert.ToInt32(lblTempEnqId.Text), -1);
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
                            //txt_Contact_No.ReadOnly = true;
                            AutoCompleteExtender1.Enabled = false;
                            AutoCompleteExtender2.Enabled = false;
                            //AutoCompleteExtender3.Enabled = false;
                        }
                        if (Convert.ToBoolean(Enqry.CL_Priority_bit) == true)
                            priority = "Priority";
                        else
                            priority = "Regular";

                        if (Convert.ToInt32(Enqry.SITE_MonthlyBillingStatus_bit) == 1)
                            status = "Monthly";
                        else
                            status = "Regular";

                        lblBillngStatus.Text = status;
                        lblClientStatus.Text = priority;
                        txt_Client.Text = Convert.ToString(Enqry.CL_Name_var);
                        txtClientAddress.Text = Convert.ToString(Enqry.CL_OfficeAddress_var);
                        txt_Site.Text = Convert.ToString(Enqry.SITE_Name_var);
                        txt_Site_address.Text = Convert.ToString(Enqry.SITE_Address_var);


                        ddl_EnqType.SelectedIndex = 1;
                        ddl_EnqStatus.SelectedIndex = 1;

                        //by default set to tobe collected 
                        rbOrderConfirm.Checked = true;
                        addEnquiryDetails();
                        ddl_MatCollectn.SelectedIndex = 1;
                        MaterialStatusChange();
                        //if (Session["CONTPersonID"].ToString() == "0")
                        //{
                        if (Enqry.ENQ_contact_person != null)
                            txt_ContactPerson.Text = Enqry.ENQ_contact_person.ToString();
                        else
                            txt_ContactPerson.Text = string.Empty;


                        if (Enqry.ENQ_contact_number != null)
                            txt_Contact_No.Text = Enqry.ENQ_contact_number.ToString();
                        else
                            txt_Contact_No.Text = string.Empty;


                        if (Enqry.ENQ_contact_emailid != null)
                            txt_EnqEmail.Text = Enqry.ENQ_contact_emailid.ToString();
                        else
                            txt_EnqEmail.Text = string.Empty;
                        //}
                        //else
                        //{
                        //    var cnt = dc.Contact_View(Convert.ToInt32(Session["CONTPersonID"]), Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), "").ToList();
                        //    if(cnt.Count>0)
                        //    {
                        //        if (cnt.FirstOrDefault().CONT_Name_var != null)
                        //            txt_ContactPerson.Text = cnt.FirstOrDefault().CONT_Name_var;

                        //        if (cnt.FirstOrDefault().CONT_ContactNo_var != null)
                        //            txt_Contact_No.Text = cnt.FirstOrDefault().CONT_ContactNo_var;

                        //        if (cnt.FirstOrDefault().CONT_EmailID_var != null)
                        //            txt_EnqEmail.Text = cnt.FirstOrDefault().CONT_EmailID_var;
                        //        else
                        //            txt_EnqEmail.Text = Enqry.ENQ_contact_emailid.ToString();
                        //    }
                        //}
                        if (Enqry.SITE_Landmark_var != null)
                        {
                            txt_Site_LandMark.Text = Convert.ToString(Enqry.SITE_Landmark_var);
                        }
                        else
                        {
                            txt_Site_LandMark.Text = string.Empty;
                        }
                    }


                    var material = dc.EnquiryApp_Material_View(Convert.ToInt32(lblTempEnqId.Text)).ToList();
                    if (material.Count > 0)
                    {
                        lblMobEnq.Text = "InwardTest List";
                        divEnqPendList.Visible = true;
                    }
                    else
                    {
                        lblMobEnq.Text = "Pending Enquiry List";
                        divEnqPendList.Visible = false;
                    }
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
                        txt_Contact_No.Text = Enqry.ENQNEW_ContactNo_var;
                        txt_EnqEmail.Text =Convert.ToString(Enqry.ENQNEW_Email_var);
                        txt_Client.Text = Enqry.ENQNEW_ClientName_var;
                        //txtClientAddress.Text = Enqry.CL_OfficeAddress_var;
                        txt_Qty.Text = Enqry.ENQNEW_Quantity.ToString();
                        string matRecType = cd.getMaterialTypeValue(Enqry.MATERIAL_Id.ToString());
                        ddl_InwardType.SelectedValue = matRecType;
                        if (Convert.ToString(Enqry.ENQNEW_Type_var) != "" && Convert.ToString(Enqry.ENQNEW_Type_var) != null)
                            ddl_EnqStatus.SelectedValue = Convert.ToString(Enqry.ENQNEW_Type_var);
                        else
                            ddl_EnqStatus.SelectedIndex = 0;

                        if (Convert.ToString(Enqry.ENQNEW_GeneratedStatus_var) != "" && Convert.ToString(Enqry.ENQNEW_GeneratedStatus_var) != null)
                            ddl_EnqType.SelectedValue = Convert.ToString(Enqry.ENQNEW_GeneratedStatus_var);
                        else
                            ddl_EnqType.SelectedIndex = 0;
                        txt_Note.Text = Enqry.ENQNEW_Note_var;
                        txt_Reference.Text = Enqry.ENQNEW_Reference_var;
                        if (Convert.ToString(Enqry.ENQNEW_ConfirmStatus_bit) != "" && Convert.ToString(Enqry.ENQNEW_ConfirmStatus_bit) != null)
                        {
                            if (Convert.ToInt32(Enqry.ENQNEW_ConfirmStatus_bit) == 1)
                                chkConifrmEnq.Checked = true;
                            else if (Convert.ToInt32(Enqry.ENQNEW_ConfirmStatus_bit) == 0)
                                chkConifrmEnq.Checked = false;
                        }
                        else
                            chkConifrmEnq.Checked = true;

                        if (Enqry.ENQNEW_OpenEnquiryStatus_var == "To be Collected" || Enqry.ENQNEW_OpenEnquiryStatus_var == "Already Collected" || Enqry.ENQNEW_OpenEnquiryStatus_var == "On site Testing" || Enqry.ENQNEW_OpenEnquiryStatus_var == "Material Sending On Date" || Enqry.ENQNEW_OpenEnquiryStatus_var == "Delivered by Client")
                            rbOrderConfirm.Checked = true;
                        else if(Enqry.ENQNEW_OpenEnquiryStatus_var == "Decision Pending")
                            rbDecisionPending.Checked = true;
                        else
                            rbOrderLoss.Checked = true;

                        addEnquiryDetails();

                        if (Convert.ToString(Enqry.ENQNEW_WalkingStatus_bit) != "" && Convert.ToString(Enqry.ENQNEW_WalkingStatus_bit) != null)
                        {
                            if (Convert.ToInt32(Enqry.ENQNEW_WalkingStatus_bit) == 1)
                                chkWalkIn.Checked = true;
                            else if (Convert.ToInt32(Enqry.ENQNEW_WalkingStatus_bit) == 0)
                                chkWalkIn.Checked = false;
                        }
                        if (Enqry.ENQNEW_OpenEnquiryStatus_var == "To be Collected")
                        {
                            ddl_MatCollectn.SelectedIndex = 1;
                            rbOrderConfirm.Checked = true;
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
                            ddl_MatCollectn.SelectedIndex = 2;
                            rbOrderConfirm.Checked = true;
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
                            ddl_MatCollectn.SelectedIndex = 1;//3;
                            rbDecisionPending.Checked = true;
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
                            ddl_MatCollectn.SelectedIndex = 3;//6; 
                            rbOrderConfirm.Checked = true;
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
                            ddl_MatCollectn.SelectedIndex = 1;//4;
                            rbOrderLoss.Checked = true;
                            txt_CommentDeclinebyUs.Text = Enqry.ENQNEW_Comment_var.ToString();
                            txt_CommentDeclinebyUs.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQNEW_OpenEnquiryStatus_var == "Rejected By Client" || Enqry.ENQNEW_OpenEnquiryStatus_var == "Declined by Client")
                        {
                            ddl_MatCollectn.SelectedIndex = 2;//5;
                            rbOrderLoss.Checked = true;
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
                            ddl_MatCollectn.SelectedIndex = 4;//7; 
                            rbOrderConfirm.Checked = true;
                            txt_MaterialSendingOnDate.Text = Convert.ToDateTime(Enqry.ENQNEW_MaterialSendingOnDate_dt).ToString("dd/MM/yyyy");
                            txt_MaterialSendingOnDate.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;

                        }
                        MaterialStatusChange(); //work asper order status
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
                            txt_Contact_No.ReadOnly = true;
                            AutoCompleteExtender1.Enabled = false;
                            AutoCompleteExtender2.Enabled = false;
                            AutoCompleteExtender3.Enabled = false;
                        }
                        if (Convert.ToBoolean(Enqry.CL_Priority_bit) == true)
                            priority = "Priority";
                        else
                            priority = "Regular";
                        lblClientStatus.Text = priority;

                        if (Convert.ToInt32(Enqry.SITE_MonthlyBillingStatus_bit) == 1)
                            status = "Monthly";
                        else
                            status = "Regular";

                        lblBillngStatus.Text = status;
                        txt_Client.Text = Enqry.CL_Name_var.ToString();
                        txtClientAddress.Text = Enqry.CL_OfficeAddress_var;
                        txt_EnqEmail.Text = Enqry.CONT_EmailID_var;
                        txt_Site.Text = Enqry.SITE_Name_var.ToString();
                        txt_Site_address.Text = Enqry.SITE_Address_var.ToString();
                        if (Convert.ToString(Enqry.ENQ_Type_var) != "" && Convert.ToString(Enqry.ENQ_Type_var) != null)
                            ddl_EnqStatus.SelectedValue = Convert.ToString(Enqry.ENQ_Type_var);
                        else
                            ddl_EnqStatus.SelectedIndex = 0;

                        if (Convert.ToString(Enqry.ENQ_GeneratedStatus_var) != "" && Convert.ToString(Enqry.ENQ_GeneratedStatus_var) != null)
                            ddl_EnqType.SelectedValue = Convert.ToString(Enqry.ENQ_GeneratedStatus_var);
                        else
                            ddl_EnqType.SelectedIndex = 0;

                        //ddl_EnqStatus.SelectedItem.Text = Convert.ToString(Enqry.ENQ_Type_var);
                        //ddl_EnqType.SelectedItem.Text=Convert.ToString(Enqry.ENQ_GeneratedStatus_var);
                        if (Convert.ToString(Enqry.ENQ_ConfirmStatus_bit) != "" && Convert.ToString(Enqry.ENQ_ConfirmStatus_bit) != null)
                        {
                            if (Convert.ToInt32(Enqry.ENQ_ConfirmStatus_bit) == 1)
                                chkConifrmEnq.Checked = true;
                            else if (Convert.ToInt32(Enqry.ENQ_ConfirmStatus_bit) == 0)
                                chkConifrmEnq.Checked = false;
                        }
                        else
                            chkConifrmEnq.Checked = true;

                        if (Convert.ToString(Enqry.ENQ_OrderStatus_bit) != "" && Convert.ToString(Enqry.ENQ_OrderStatus_bit) != null)
                        {
                            if (Convert.ToInt32(Enqry.ENQ_OrderStatus_bit) == 1)
                                rbOrderConfirm.Checked = true;
                            else if (Convert.ToInt32(Enqry.ENQ_OrderStatus_bit) == 0)
                                rbOrderLoss.Checked = true;
                            else if (Convert.ToInt32(Enqry.ENQ_OrderStatus_bit) == 2)
                                rbDecisionPending.Checked = true;
                        }
                        else
                        {
                            if (Enqry.ENQ_OpenEnquiryStatus_var == "To be Collected" || Enqry.ENQ_OpenEnquiryStatus_var == "Already Collected" || Enqry.ENQ_OpenEnquiryStatus_var == "On site Testing" || Enqry.ENQ_OpenEnquiryStatus_var == "Material Sending On Date" || Enqry.ENQ_OpenEnquiryStatus_var == "Delivered by Client")
                                rbOrderConfirm.Checked = true;
                            else if (Enqry.ENQ_OpenEnquiryStatus_var == "Decision Pending")
                                rbDecisionPending.Checked = true;
                            else
                                rbOrderLoss.Checked = true;
                        }
                        addEnquiryDetails();//work asper order status
                        //else
                        //{
                        //    rbOrderConfirm.Checked = true;
                        //    rbOrderLoss.Checked = true;
                        //}


                        if (Convert.ToString(Enqry.ENQ_WalkingStatus_bit) != "" && Convert.ToString(Enqry.ENQ_WalkingStatus_bit) != null)
                        {
                            if (Convert.ToInt32(Enqry.ENQ_WalkingStatus_bit) == 1)
                                chkWalkIn.Checked = true;
                            else if (Convert.ToInt32(Enqry.ENQ_WalkingStatus_bit) == 0)
                                chkWalkIn.Checked = false;
                        }
                        if (Enqry.SITE_Landmark_var != null)
                        {
                            txt_Site_LandMark.Text = Enqry.SITE_Landmark_var.ToString();
                        }
                        else
                        {
                            txt_Site_LandMark.Text = string.Empty;
                        }
                        txt_ContactPerson.Text = Enqry.CONT_Name_var.ToString();
                        txt_Contact_No.Text = Enqry.CONT_ContactNo_var.ToString();
                        string matRecType = cd.getMaterialTypeValue(Enqry.MATERIAL_Id.ToString());
                        ddl_InwardType.SelectedValue = matRecType;
                        txt_Qty.Text = Enqry.ENQ_Quantity.ToString();
                        //txt_Quantity.Text = Enqry.ENQ_Quantity.ToString();
                        txt_Note.Text = Enqry.ENQ_Note_var.ToString();
                        txt_Reference.Text = Enqry.ENQ_Reference_var.ToString();
                        if (Enqry.ENQ_OpenEnquiryStatus_var == "To be Collected")
                        {
                            ddl_MatCollectn.SelectedIndex = 1;
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
                            ddl_MatCollectn.SelectedIndex = 2;

                            if (Enqry.ENQ_CollectedAt_var == "By Driver")
                            {
                                //lblDriverName.Visible = true;
                                //ddl_DriverName.Visible = true;
                                Rdn_ByDriver.Checked = true;
                                ddl_DriverName.SelectedValue = Enqry.ENQ_DriverId.ToString();
                                ddl_DriverName.Enabled = true;
                            }
                            else if (Enqry.ENQ_CollectedAt_var == "At Lab")
                            {
                                //lblDriverName.Visible = false;
                                //ddl_DriverName.Visible = false;
                                Rdn_AtLab.Checked = true;
                                ddl_DriverName.Enabled = false;
                            }
                        }
                        else if (Enqry.ENQ_OpenEnquiryStatus_var == "Decision Pending")
                        {
                            ddl_MatCollectn.SelectedIndex = 1;//3;
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
                            ddl_MatCollectn.SelectedIndex = 3;//6;
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
                            ddl_MatCollectn.SelectedIndex = 1;//4;
                            txt_CommentDeclinebyUs.Text = Enqry.ENQ_Comment_var.ToString();
                            txt_CommentDeclinebyUs.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;
                            txt_MaterialSendingOnDate.Enabled = false;
                        }
                        else if (Enqry.ENQ_OpenEnquiryStatus_var == "Rejected By Client" || Enqry.ENQ_OpenEnquiryStatus_var == "Declined by Client")
                        {
                            ddl_MatCollectn.SelectedIndex = 2;//5;
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
                            ddl_MatCollectn.SelectedIndex = 4;//7;
                            txt_MaterialSendingOnDate.Text = Convert.ToDateTime(Enqry.ENQ_MaterialSendingOnDate_dt).ToString("dd/MM/yyyy");
                            txt_MaterialSendingOnDate.Enabled = true;
                            txt_CommentDecisionPending.Enabled = false;
                            txt_CommentDeclinebyUs.Enabled = false;
                            txt_CommentRejectedByClient.Enabled = false;
                            chkRateDiiference.Enabled = false;
                            txt_OnsiteTesting_Date.Enabled = false;

                        }
                        MaterialStatusChange();
                    }
                    DisplayBalLmt();
                    DisplayDiscount();
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
                    strOutstndingAmt = Convert.ToDecimal(enq.CL_BalanceAmt_mny).ToString("0.00");
                if (Convert.ToString(enq.Proposal_NetAmount) != "" && Convert.ToString(enq.Proposal_NetAmount) != null)
                    strEnqAmount = Convert.ToString(enq.Proposal_NetAmount);
                strEnqNo = Convert.ToString(enq.ENQ_Id);                
                if (Convert.ToString(enq.SITE_Route_Id) != "" && Convert.ToString(enq.SITE_Route_Id) != null)
                    siteRouteId = Convert.ToInt32(enq.SITE_Route_Id);
                if (Convert.ToString(enq.MEId) != "" && Convert.ToString(enq.MEId) != null)
                    siteMeId = Convert.ToInt32(enq.MEId);

                tempMailId = txt_EnqEmail.Text.Trim();
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

                //tempMailId = enq.CL_AccEmailId_var.Trim();
                //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                //    tempMailId.ToLower().Contains(".") == false)
                //{
                //    addMailId = false;
                //}
                //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                //{
                //    if (mailIdTo != "")
                //        mailIdTo += ",";
                //    mailIdTo += tempMailId;
                //    strAllMailId += "," + tempMailId + ",";
                //}

                //addMailId = true;
                //tempMailId = enq.CL_DirectorEmailID_var.Trim();
                //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                //    tempMailId.ToLower().Contains(".") == false)
                //{
                //    addMailId = false;
                //}
                //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                //{
                //    if (mailIdTo != "")
                //        mailIdTo += ",";
                //    mailIdTo += tempMailId;
                //    strAllMailId += "," + tempMailId + ",";
                //}

                //addMailId = true;
                //tempMailId = enq.CL_EmailID_var.Trim();
                //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                //    tempMailId.ToLower().Contains(".") == false)
                //{
                //    addMailId = false;
                //}
                //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                //{
                //    if (mailIdTo != "")
                //        mailIdTo += ",";
                //    mailIdTo += tempMailId;
                //    strAllMailId += "," + tempMailId + ",";
                //}

                //addMailId = true;
                //tempMailId = enq.SITE_EmailID_var.Trim();
                //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                //    tempMailId.ToLower().Contains(".") == false)
                //{
                //    addMailId = false;
                //}
                //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                //{
                //    if (mailIdTo != "")
                //        mailIdTo += ",";
                //    mailIdTo += tempMailId;
                //    strAllMailId += "," + tempMailId + ",";
                //}

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


                if (strEnqAmount=="0")
                    mbody = mbody + "We thank you for your enquiry (no. " + strEnqNo + ") on date " + strEnqDate + ". Your outstanding amount is " + strOutstndingAmt + ". We can arrange material pickup once we recieve the order confirmation.<br><br>";
                else
                    mbody = mbody + "We thank you for your enquiry (no. " + strEnqNo + ") on date " + strEnqDate + ". Your outstanding amount is " + strOutstndingAmt + ". We can arrange material pickup once we recieve the order confirmation.<br><br>";

                mbody = mbody + "Please ignore this if payment is already made. <br><br> ";

                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>";
                mbody = mbody + "Best Regards,";
                mbody = mbody + "<br>";
                mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                 try
                {
                    objMail.SendMail(mailIdTo, mailIdCc, mSubject, mbody, "", mReplyTo);
                
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
            if (ddl_MatCollectn.SelectedItem.Text.Equals("Decision Pending"))
            {
                i++;
            }
            if (ddl_MatCollectn.SelectedItem.Text.Equals("Declined by us"))
            {
                i++;
            }
            if (ddl_MatCollectn.SelectedItem.Text.Equals("Declined by Client"))
            {
                i++;
            }
            if (ddl_MatCollectn.SelectedItem.Text.Equals("On site Testing"))
            {
                i++;
            }
            if (ddl_MatCollectn.SelectedItem.Text.Equals("Delivered by Client"))
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
                        //if (c.CL_OutstandingAmt_var != null)
                        //{
                        //    string[] strAmt = c.CL_OutstandingAmt_var.Split('|');
                        //    lblBalanceAmt.Text = Convert.ToDecimal(strAmt[0]).ToString("0.00");
                        //    lblBelow90BalanceAmt.Text = Convert.ToDecimal(strAmt[1]).ToString("0.00");
                        //    lblAbove90BalanceAmt.Text = Convert.ToDecimal(strAmt[2]).ToString("0.00");
                        //}
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
                        lblDiscountPer.Text = item.Applicable.ToString() + " %";
                        break;
                    }
                }
            }
        }
        private void Cleartextbox()
        {
            txt_Client.Text = string.Empty;
            txtClientAddress.Text = string.Empty;
            trAdd.Visible = false;
            divEnqPendList.Visible = false;
            lblProposalError.Visible = false;
            grdInwardType.DataSource = null;
            grdInwardType.DataBind();
            lblEnqNo.Visible = false;
            chkWalkIn.Checked = false;
            txt_Qty.Text = string.Empty;
            lblClientStatus.Text = "";
            lblBillngStatus.Text = "";
            chkConifrmEnq.Checked = true;
            txt_EnqEmail.Text = "";
            //lblClientAddress.Visible = false;
            //txtClientAddress.Visible = false;
            txt_Site.Text = string.Empty;
            txt_Site_address.Text = string.Empty;
            txt_Site_LandMark.Text = string.Empty;
            txt_ContactPerson.Text = string.Empty;
            txt_Contact_No.Text = string.Empty;
            ddl_InwardType.ClearSelection();
            txt_Note.Text = string.Empty;
            txt_Reference.Text = string.Empty;
            ddl_Location.ClearSelection();
            ddlRouteName.Items.Clear();
            txt_CollectionDate.Text = string.Empty;
            chkbox_Urgent.Checked = false;
            Rdn_AtLab.Checked = false;
            Rdn_ByDriver.Checked = false;
            ddl_DriverName.ClearSelection();
            txt_CommentDecisionPending.Text = string.Empty;
            txt_CommentDeclinebyUs.Text = string.Empty;
            txt_CommentRejectedByClient.Text = string.Empty;
            chkRateDiiference.Checked = false;
            txt_OnsiteTesting_Date.Text = string.Empty;
            txt_MaterialSendingOnDate.Text = string.Empty;
            Lbl_Client_Expected_Date.Visible = false;
            txt_ClientExpected_Date.Visible = false;
            ddl_MatCollectn.SelectedIndex = 0;
            ddl_EnqStatus.SelectedIndex = 0;
            ddl_EnqType.SelectedIndex = 0;
            txtMailCC.Text = "";
            txtEmail.Text = ""; hideGTFields();
            txt_KindAttention.Text = "";
            txt_Subject.Text = "";
            txt_Description.Text = "";
            grdProposal.DataSource = null;
            grdProposal.DataBind();
            grdGT.DataSource = null;
            grdGT.DataBind();
            Grd_Note.DataSource = null;
            Grd_Note.DataBind();
            txt_NetAmount.Text = "";
            txtMe.Text = "";
            txtMeNum.Text = "";
            ddl_PrposalBy.SelectedIndex = 0;
            txt_ContactNo.Text = "";
            txtPaymentTerm.Text = "";
        }
        private void EnableLocation()
        {
            ddlRouteName.Enabled = false;

            if (ddl_Location.SelectedIndex > 0)
                ddlRouteName.Enabled = true;
            else
                ddlRouteName.Enabled = false;
        }
        private void EnableDriverName()
        {
            ddl_DriverName.Enabled = false;
            if (Rdn_ByDriver.Checked)
                ddl_DriverName.Enabled = true;
            else
                ddl_DriverName.Enabled = false;
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
        protected void priviewPendingEnq()
        {
            if (Session["CL_ID"] != null && Convert.ToInt32(Session["CL_ID"]) > 0
                && Session["SITE_ID"] != null && Convert.ToInt32(Session["SITE_ID"]) > 0)
            {
                divEnqPendList.Visible = true;
                var EnquiryStatus = dc.Enquiry_View_Previous(Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0);
                grdEnqList.DataSource = EnquiryStatus;
                grdEnqList.DataBind();
            }
        }
        protected void Rdn_AtLab_CheckedChanged(object sender, EventArgs e)
        {
            if (Rdn_AtLab.Checked == true)
                trByDriver.Visible = false;
        }
        protected void Rdn_ByDriver_CheckedChanged(object sender, EventArgs e)
        {
            if (Rdn_ByDriver.Checked == true)
            {
                trByDriver.Visible = true;
                ddl_DriverName.Enabled = true;
            }
            else
            {
                trByDriver.Visible = false;
                ddl_DriverName.Enabled = false;

            }
        }
        protected void chkbox_Urgent_CheckedChanged(object sender, EventArgs e)
        {
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
        protected void ddl_MatCollectn_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaterialStatusChange();
        }

        private void MaterialStatusChange()
        {
            if (ddl_MatCollectn.SelectedItem.Text == "----------------------------Select----------------------------")
            {
                disableEnquiryDetails();
                //panelAlredyCollected.Visible = false;
                //panelMatToBeCollected.Visible = false;
                //panelOther.Visible = false;
                //panelMatCollStatus.Visible = false;
                //panelMatCollStatus.Visible = false;
                //tblMatToBeCollected.Visible = false;
                //tblAlredyCollected.Visible = false;
                //tblOther.Visible = false;
                // txt_Qty.Visible = false; lblQty.Visible = false;
                //btn_Cal.Visible = false; btn_Cal1.Visible = false;

            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Material To be Collected")
            {
                //panelMatCollStatus.Visible = true;
                tblMatToBeCollected.Visible = true;
                tblAlredyCollected.Visible = false;
                tblOther.Visible = false;
                //   txt_Qty.Visible = false; lblQty.Visible = false;
                // btn_Cal.Visible = true; btn_Cal1.Visible = true;

            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Already Collected")
            {
                //panelMatCollStatus.Visible = true;
                tblMatToBeCollected.Visible = false;
                tblAlredyCollected.Visible = true;
                tblOther.Visible = false;
                //  txt_Qty.Visible = false;  lblQty.Visible = false;
                // btn_Cal.Visible = true; btn_Cal1.Visible = true;
                if (Rdn_ByDriver.Checked == true)
                    trByDriver.Visible = true;
                else
                    trByDriver.Visible = false;


            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Decision Pending")
            {
                //panelMatCollStatus.Visible = true;
                tblMatToBeCollected.Visible = false;
                tblAlredyCollected.Visible = false;
                tblOther.Visible = true;
                trDecisionPending.Visible = true;
                trDeclinebyUs.Visible = false;
                trDeclinedByClient.Visible = false;
                trOnsiteTesting.Visible = false;
                trDeliveredbyClient.Visible = false;
                //  txt_Qty.Visible = false; lblQty.Visible = false;
                // btn_Cal.Visible = true; btn_Cal1.Visible = true;
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Declined by us")
            {
                //panelMatCollStatus.Visible = true;
                tblMatToBeCollected.Visible = false;
                tblAlredyCollected.Visible = false;
                tblOther.Visible = true;
                trDecisionPending.Visible = false;
                trDeclinebyUs.Visible = true;
                trDeclinedByClient.Visible = false;
                trOnsiteTesting.Visible = false;
                trDeliveredbyClient.Visible = false;
                // txt_Qty.Visible = true; lblQty.Visible = true;
                //btn_Cal.Visible = false; btn_Cal1.Visible = false;
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Declined by Client")
            {
                tblMatToBeCollected.Visible = false;
                tblAlredyCollected.Visible = false;
                tblOther.Visible = true;
                trDecisionPending.Visible = false;
                trDeclinebyUs.Visible = false;
                trDeclinedByClient.Visible = true;
                trOnsiteTesting.Visible = false;
                trDeliveredbyClient.Visible = false;
                //  txt_Qty.Visible = true; lblQty.Visible = true;
                //btn_Cal.Visible = false; btn_Cal1.Visible = false;
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "On site Testing")
            {
                tblMatToBeCollected.Visible = false;
                tblAlredyCollected.Visible = false;
                tblOther.Visible = true;
                trDecisionPending.Visible = false;
                trDeclinebyUs.Visible = false;
                trDeclinedByClient.Visible = false;
                trOnsiteTesting.Visible = true;
                trDeliveredbyClient.Visible = false;
                //    txt_Qty.Visible = false; lblQty.Visible = false;
                //btn_Cal.Visible = true; btn_Cal1.Visible = true;
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Delivered by Client")
            {
                panelMatCollStatus.Visible = true;
                tblMatToBeCollected.Visible = false;
                tblAlredyCollected.Visible = false;
                tblOther.Visible = true;
                trDecisionPending.Visible = false;
                trDeclinebyUs.Visible = false;
                trDeclinedByClient.Visible = false;
                trOnsiteTesting.Visible = false;
                trDeliveredbyClient.Visible = true;
                //  txt_Qty.Visible = false; lblQty.Visible = false;
                // btn_Cal.Visible = true; btn_Cal1.Visible = true;
            }

        }
        protected void ddl_Location_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddlRouteName.Enabled = true;
            if (ddl_Location.SelectedItem.Text != "-----------Select-----------")
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
            if (ddl_Location.SelectedItem.Text != "-----------Select-----------")
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
        protected void lnkViewClientAddress_Click(object sender, EventArgs e)
        {
            //if (lblClientAddress.Visible == false)
            //{
            //    lblClientAddress.Visible = true;
            //    txtClientAddress.Visible = true;
            //}
            //else
            //{
            //    lblClientAddress.Visible = false;
            //    txtClientAddress.Visible = false;
            //}
            if (trAdd.Visible == false)
            {
                trAdd.Visible = true;
            }
            else
            {
                trAdd.Visible = false;
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
        protected void btn_Exit_Click(object sender, EventArgs e)
        {
            if (lblEnqType.Text == "VerifyClient" || lblEnqType.Text == "VerifyClientApp")
                Response.Redirect("EnquiryVerifyClient.aspx");
            else
                Response.Redirect("Enquiry_List.aspx");
        }
        protected void btn_Inward_Click(object sender, EventArgs e)
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
        #endregion

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (!chkNewClientFlag)
            {
                lblLogin.Visible = false;
                //lnkSaveEnquiry.Visible = true;
                //lnk_Inward.Visible = false;
                btn_SaveEnquiryProposal.Visible = true;
                btn_Inward.Visible = false;
                btn_SaveEnquiryProposal1.Visible = true;
                btn_Inward1.Visible = false;
                txtClientAddress.Text = string.Empty;
                txt_Site.Text = string.Empty;
                txt_Site_address.Text = string.Empty;
                txt_Site_LandMark.Text = string.Empty;


                if (lblEnqType.Text != "VerifyClient")
                {
                    txt_ContactPerson.Text = string.Empty;
                    txt_Contact_No.Text = string.Empty;
                }
                if (ChkClientName(txt_Client.Text) == true)
                {
                    if (txt_Client.Text != "" && chkNewClient.Checked == false)
                    {
                        Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                        lblClientId.Text = Convert.ToInt32(Session["CL_ID"]).ToString();
                        lblLogin.Visible = true;
                        string loginId = "", loginPwd = "", priority = "";
                        var result = dc.Client_View(Convert.ToInt32(Session["CL_ID"]), 0, "", "");
                        foreach (var item in result)
                        {
                            loginId = item.CL_LoginId_var;
                            loginPwd = item.CL_Password_var;
                            if (Convert.ToBoolean(item.CL_Priority_bit) == true)
                                priority = "Priority";
                            else
                                priority = "Regular";
                            //lblMonthlyStatus.Text = Convert.ToInt32(item.CL_MonthlyBilling_bit).ToString();
                            break;
                        }
                        lblLogin.Text = "Login Id=" + loginId + " & Password=" + loginPwd + "";
                        lblClientStatus.Text = priority;
                    }
                    else
                    {
                        Session["CL_ID"] = 0;
                        //lblMonthlyStatus.Text = "0";
                        lblClientId.Text = "0";
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
        }
        protected void txt_Site_TextChanged(object sender, EventArgs e)
        {
            if (!chkNewClientFlag)
            {
                txt_Site_address.Text = string.Empty;
                txt_Site_LandMark.Text = string.Empty;

                if (lblEnqType.Text != "VerifyClient")
                {
                    txt_ContactPerson.Text = string.Empty;
                    txt_Contact_No.Text = string.Empty;
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
                                lblSiteId.Text = Convert.ToString(Request.Form[hfSiteId.UniqueID]);
                            }
                            else
                            {
                                Session["SITE_ID"] = 0;
                                lblSiteId.Text = "0";
                            }
                            lblMonthlyStatus.Text = "0"; string status = "Regular";
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
                                    lblRouteId.Text = Convert.ToString(site.SITE_Route_Id);
                                    txtEmail.Text = Convert.ToString(site.SITE_EmailID_var);

                                    if (site.SITE_LocationId_int != null && site.SITE_LocationId_int > 0)
                                    {
                                        ddl_Location.SelectedValue = site.SITE_LocationId_int.ToString();
                                        ddl_LocationSelectChanged();
                                    }
                                    //lblMonthlyStatus.Text = Convert.ToInt32(site.SITE_MonthlyBillingStatus_bit).ToString();

                                    if (Convert.ToInt32(site.SITE_MonthlyBillingStatus_bit) == 1)
                                        status = "Monthly";

                                    lblBillngStatus.Text = status;
                                }

                                var couponsitespec = dc.Coupon_View_Sitewise(Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0, DateTime.Now).ToList();
                                lblCouponCount.Text =couponsitespec.Count().ToString();
                                if (couponsitespec.Count() == 0)
                                {
                                    var coupon = dc.Coupon_View("", 0, 0, Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0, DateTime.Now).ToList();
                                    lblCouponCount.Text =coupon.Count().ToString();
                                }
                                couponsitespec.Clear();
                                // steel coupon
                                var couponsitespec1 = dc.Coupon_View_SitewiseST(Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0, DateTime.Now).ToList();
                                lblSTCoupons.Text = " ST= " + couponsitespec1.Count().ToString();
                                if (couponsitespec1.Count() == 0)
                                {
                                    var coupon1 = dc.Coupon_ViewST("", 0, 0, Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), 0, DateTime.Now).ToList();
                                    lblSTCoupons.Text = " ST= " + coupon1.Count().ToString();
                                }
                                couponsitespec1.Clear();
                                //++
                                priviewPendingEnq();
                            }
                        }
                    }
                }
            }

        }
        protected void txt_ContactPerson_TextChanged(object sender, EventArgs e)
        {
            if (!chkNewClientFlag)
            {
                txt_Contact_No.Text = "";
                var c = dc.Contact_View(0, Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(Session["CL_ID"]), txt_ContactPerson.Text);
                foreach (var cn in c)
                {
                    txt_Contact_No.Text = Convert.ToString(cn.CONT_ContactNo_var);
                    txt_EnqEmail.Text = Convert.ToString(cn.CONT_EmailID_var);
                    Session["CONTPersonID"] = Convert.ToInt32(cn.CONT_Id);
                }
                txt_Contact_No.Focus();
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            if (!chkNewClientFlag)
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
            else
                return null;
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSitename(string prefixText)
        {
            if (!chkNewClientFlag)
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
            else
                return null;
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetContactname(string prefixText)
        {
            if (!chkNewClientFlag)
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
            else
                return null;
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

        #region Proposal
        protected void grdProposal_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddlOtherTest.Visible == true && ddl_InwardType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.SelectedIndex != 0)
                    {
                        if ((ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "Plate Load Testing"))
                        {
                            TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                            txt_DiscRate.ReadOnly = false;
                        }
                    }
                }
                else if (ddl_InwardType.SelectedValue == "RWH")
                {
                    TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                    txt_DiscRate.ReadOnly = false;
                }
                else
                {
                    TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                    txt_DiscRate.ReadOnly = true;
                }
            }


        }
        protected void ddl_InwardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string inwardTypeValue = "";
            lblProposalError.Visible = false;
            if (grdProposal.Rows.Count > 0)
            {
                Label lbl_InwdType = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].FindControl("lbl_InwdType");
                inwardTypeValue = lbl_InwdType.Text;
                if (lbl_InwdType.Text == "OTHER")
                    inwardTypeValue = "OT";
            }

            if (inwardTypeValue == "OT")
            {
                if (!ddl_InwardType.SelectedValue.ToString().Equals(inwardTypeValue))
                {
                    lblProposalError.Visible = true;
                    lblProposalError.Text = "Clear Proposal table to add new Inward Type Test";
                    ddl_InwardType.SelectedValue = ddl_InwardType.Items.FindByValue("OT").Value;

                    return;
                }
            }

            if (ddl_InwardType.SelectedValue == "OT")
            {
                ddlOtherTest.Visible = true; 
                LoadOtherTestList();
            }
            else
            {
                if (ddl_InwardType.SelectedValue == "RWH")
                {
                    Grd_NoteGT.Visible = true; 
                    lblAddChargesGT.Visible = true;
                    
                    grdPayTermsGT.Visible = true;
                    lblPayTermsGT.Visible = true;
                }

                //if (ddl_InwardType.SelectedValue == "NDT" && ddl_InwardType.SelectedValue == "CR" 
                //    && ddl_InwardType.SelectedValue == "CORECUT")
                //{
                    lblClientScope.Visible = true;
                    grdClientScope.Visible = true;
                //}

                ddlOtherTest.Visible = false;
            }
            ddlOtherTest.SelectedIndex = -1;


            if (ddl_InwardType.SelectedItem.Text != "----------------------------Select----------------------------" && ddl_InwardType.SelectedItem.Text != "Core Cutting" && lblMonthlyStatus.Text == "0" && ddl_MatCollectn.SelectedItem.Text != "Declined by us" && ddl_MatCollectn.SelectedItem.Text != "Declined by Client" && chkConifrmEnq.Checked == true)//&& chkNewClient.Checked != true
            {
                if (grdProposal.Rows.Count == 0 && grdProposal.Visible == true)
                    DisplayHeader("");
            }

        }

        public double getDiscount(string recordType)
        {

            int clId = 0, siteId = 0, maxDisc = 0;
            //groupA = 0; groupB = 0; groupC = 0; totDisc = 0; totDiscA = 0; totDiscB = 0;
            calculatedDisc = 0; maxDiscnt = 0; appliedDisc = 0; introDiscA = 0; volDiscB = 0; timelyDiscC = 0; AdvDiscD = 0; loyDiscE = 0; propDiscF = 0; AppDiscG = 0;


            clId = Convert.ToInt32(lblClientId.Text);
            siteId = Convert.ToInt32(lblSiteId.Text);

            maxDisc = getMaxDiscount(recordType);
            //lblMax.Text = "Max (%) = " + maxDisc.ToString();


            if (clId > 0)
            {
                var clDisc = dc.DiscountSetting_View(clId, "");//view all discount of that client
                foreach (var item in clDisc)
                {
                    appliedDisc = Convert.ToDouble(item.Applicable.ToString());
                    break;
                }
            }
            if (appliedDisc > maxDisc)
                appliedDisc = maxDisc;

            //if (dc.Connection.Database.ToLower().ToString() == "veenalive")
            //{
            //    //for ST  if applied dis<30 then set it to bydefault 30
            //    if (recordType == "ST")
            //    {
            //        if (appliedDisc < 30)
            //            appliedDisc = 30;
            //    }
            //}


            return appliedDisc;
        }
        public int getMaxDiscount(string matrialType)
        {
            int maxDisc = 0;
            var maxDiscDetails = dc.Material_View(matrialType, "");
            foreach (var max in maxDiscDetails)
            {
                maxDisc = Convert.ToInt32(max.MATERIAL_MaxDiscount_int);
                break;
            }
            return maxDisc;

        }
        private double chkSiteWiseRate(int testId)
        {
            double siteWiseRate = 0;
            int clId = 0, siteId = 0;
            if (!chkNewClient.Checked)
            {
                clId = Convert.ToInt32(lblClientId.Text);
                siteId = Convert.ToInt32(lblSiteId.Text);

                //check whether sitewise rate is there or not
                var chk = dc.SiteWiseRate_View(clId, siteId, testId, true).ToList();
                if (chk.Count() > 0)//sitewise rate is applicable
                    siteWiseRate = Convert.ToDouble(chk.FirstOrDefault().SITERATE_Test_Rate_int);

            }
            return siteWiseRate;
        }

        protected void lnkAddInwardTestToGrid_Click(object sender, EventArgs e)
        {
            if (ddl_InwardType.SelectedItem.Text != "----------------------------Select----------------------------" && lblEnqNo.Visible == false && lblMonthlyStatus.Text == "0" && ddl_MatCollectn.SelectedItem.Text != "Declined by us" && ddl_MatCollectn.SelectedItem.Text != "Declined by Client" && chkConifrmEnq.Checked == true)//&& chkNewClient.Checked != true
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                lblProposalError.Visible = false;
                // btn_Cal.Visible = true; btn_Cal1.Visible = true;
                bool validFlag = false; string inwardTypeValue = "";

                if (grdProposal.Rows.Count > 0)
                {
                    Label lbl_InwdType = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].FindControl("lbl_InwdType");
                    inwardTypeValue = lbl_InwdType.Text;

                }
                if (grdGT.Rows.Count > 0)
                {
                    Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].FindControl("lbl_InwdType");
                    inwardTypeValue = lbl_InwdType.Text;

                }
                
                clsData obj = new clsData();
                //string inwardTypeValue = obj.getInwardTypeValue(inwdPrevRecType);
                string ddlInwrdType = ddl_InwardType.SelectedItem.Text;//this is random type which is select by us to add new test of other rec type
                string ddlInwrdTypeValue = ddl_InwardType.SelectedValue.ToString();

                if (ddlInwrdTypeValue == "OT" && ddlOtherTest.SelectedItem.Text == "Structural Audit")
                {
                    inwardTypeValue = "OT";
                }
                //for terms and conditions variation
                if (inwardTypeValue == "")//1st time click on link addInwardTestToGrid
                    validFlag = true;
                else if (inwardTypeValue == "GT")
                {
                    if (ddlInwrdTypeValue == "GT")
                        validFlag = true;
                }
                else if (inwardTypeValue == "RWH")
                {
                    if (ddlInwrdTypeValue == "RWH")
                        validFlag = true;
                }
                //else if (inwardTypeValue == "ST")
                //{
                //    if (ddlInwrdTypeValue == "ST")
                //        validFlag = true;
                //}
                else if (inwardTypeValue == "NDT" || inwardTypeValue == "CORECUT" || inwardTypeValue == "CR")
                {
                    if (ddlInwrdTypeValue == "NDT" || ddlInwrdTypeValue == "CORECUT" || ddlInwrdTypeValue == "CR")
                        validFlag = true;
                }
                else if (inwardTypeValue == "AAC" || inwardTypeValue == "AGGT" || inwardTypeValue == "BT-" || inwardTypeValue == "CCH" || inwardTypeValue == "CEMT" || inwardTypeValue == "CT" || inwardTypeValue == "FLYASH" || inwardTypeValue == "SOLID" || inwardTypeValue == "MF" || inwardTypeValue == "PT" || inwardTypeValue == "PILE" || inwardTypeValue == "SO" || inwardTypeValue == "STC" || inwardTypeValue == "TILE" || inwardTypeValue == "WT" || inwardTypeValue == "ST")
                {
                    if (ddlInwrdTypeValue == "AAC" || ddlInwrdTypeValue == "Coupon" || ddlInwrdTypeValue == "AGGT" || ddlInwrdTypeValue == "BT-" || ddlInwrdTypeValue == "CCH" || ddlInwrdTypeValue == "CEMT" || ddlInwrdTypeValue == "CT" || ddlInwrdTypeValue == "FLYASH" || ddlInwrdTypeValue == "SOLID" || ddlInwrdTypeValue == "MF" || ddlInwrdTypeValue == "PT" || ddlInwrdTypeValue == "PILE" || ddlInwrdTypeValue == "SO" || ddlInwrdTypeValue == "STC" || ddlInwrdTypeValue == "TILE" || ddlInwrdTypeValue == "WT" || ddlInwrdTypeValue == "ST")
                        validFlag = true;
                }
                else if (inwardTypeValue == "OT")
                {
                    if (ddlInwrdTypeValue == "OT" && ddlOtherTest.Visible == true)
                    {
                        string otherInwrdTest = ddlOtherTest.SelectedItem.Text;
                        DataTable dt = obj.getOtherSubTest();
                        if (ddlOtherTest.SelectedItem.Text == "Structural Audit")
                        {
                            grdProposal.DataSource = null;
                            grdProposal.DataBind();
                            validFlag = true; otFlag = true;
                            pnlStructAudit.Visible = true;
                            DisplayStructuralAuditDetails();
                        }
                        else if (ddlOtherTest.SelectedItem.Text == "Plate Load Testing")
                        {
                            grdProposal.DataSource = null;
                            grdProposal.DataBind();
                            validFlag = true; otFlag = true;
                        }
                        else if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test")
                        {
                            grdProposal.DataSource = null;
                            grdProposal.DataBind();
                            validFlag = true; otFlag = true;
                        }
                        else if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                        {
                            grdGT.DataSource = null;
                            grdGT.DataBind();
                            validFlag = true; otFlag = true;

                            chkLums.Checked = true;
                            ChkLumpSum();
                            txtFrmRow.Text = 2.ToString();
                            txtToRow.Text = 3.ToString();
                        }
                        else
                        {
                            if (otFlag == true)
                            {
                                otFlag = false;
                                grdProposal.DataSource = null;
                                grdProposal.DataBind();
                            }
                            foreach (DataRow item in dt.Rows)
                            {
                                if (otherInwrdTest == item["Test_name_var"].ToString())
                                    validFlag = true;
                            }
                        }
                        if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                        {
                            chkLums.Checked = true;
                            ChkLumpSum();
                            txtFrmRow.Text = 1.ToString();
                            txtToRow.Text = 3.ToString();
                        }
                    }

                }

                if (validFlag == false)
                {
                    if (grdProposal.Visible == true)
                    {
                        if (grdProposal.Rows.Count > 0)
                            ddl_InwardType.SelectedValue = ddl_InwardType.Items.FindByValue(inwardTypeValue).Value;
                    }
                    else if (ddlInwrdTypeValue == "GT")
                        ddl_InwardType.SelectedValue = ddl_InwardType.Items.FindByValue("GT").Value;
                    else if (ddlInwrdTypeValue == "RWH")
                        ddl_InwardType.SelectedValue = ddl_InwardType.Items.FindByValue("RWH").Value;
                    else if (ddlInwrdTypeValue == "ST")
                        ddl_InwardType.SelectedValue = ddl_InwardType.Items.FindByValue("ST").Value;

                    if (inwardTypeValue != "OT")
                        ddlOtherTest.Visible = false;

                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Terms and conditions are different as per Inward Type.. So you can not add selected Inward Type Test" + "');", true);

                    lblProposalError.Visible = true;
                    lblProposalError.Text = "Terms and conditions are different as per Inward Type.. So you can not add selected Inward Type Test";
                    return;

                }

                //
                if (ddlInwrdTypeValue == "OT" && ddlOtherTest.SelectedValue == "0")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Select Sub test" + "');", true);

                    lblProposalError.Visible = true;
                    lblProposalError.Text = "Terms and conditions are different as per Inward Type.. So you can not add selected Inward Type Test";

                    ddlOtherTest.Focus();
                }
                else
                {
                    chkGTDiscNote.Visible = false;
                    lblGTDiscNote.Visible = false;
                    lnkDepth.Visible = false;
                    if (grdGT.Visible == false)
                    {
                        grdGT.DataSource = null; grdGT.DataBind();
                    }
                    if (grdProposal.Visible == false)
                    {
                        grdProposal.DataSource = null; grdProposal.DataBind();
                    }

                    if (ddlInwrdType == "Soil Investigation" && ddlInwrdTypeValue == "GT")
                    {
                        chkGTDiscNote.Visible = true;
                        lblGTDiscNote.Visible = true;
                        lnkDepth.Visible = true;
                        showGTFields();
                        Grd_Note.DataSource = null;
                        Grd_Note.DataBind();
                        Grd_NoteGT.DataSource = null;
                        Grd_NoteGT.DataBind();
                        grdPayTermsGT.DataSource = null;
                        grdPayTermsGT.DataBind();
                        int mergeFrom = 2; int mergeTo = 8;

                        if (mergeFrom != 0 && mergeTo != 0)
                        {
                            chkLums.Checked = true;
                            ChkLumpSum();
                            txtToRow.Text = mergeTo.ToString();
                            txtFrmRow.Text = mergeFrom.ToString();
                        }
                        AddTermsConditionNotes("Soil Investigation");
                        AddTestToGridGT("GT");
                    }
                    else if (ddlInwrdTypeValue == "OT" && (ddlOtherTest.SelectedItem.Text == "SBC by SPT" || ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose"))
                    {
                        showGTFields();
                        if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                        {
                            Grd_NoteGT.Visible = false;
                            lblAddChargesGT.Visible = false;

                            grdPayTermsGT.Visible = false;
                            lblPayTermsGT.Visible = false;                           
                        }
                        ChkLumpSum();
                        grdProposal.Visible = false;
                        if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                        {
                            ddlInwrdType = "SBC by SPT";
                            Grd_Note.DataSource = null;
                            Grd_Note.DataBind();
                            Grd_NoteGT.DataSource = null;
                            Grd_NoteGT.DataBind();
                            grdPayTermsGT.DataSource = null;
                            grdPayTermsGT.DataBind();
                        }
                        AddTermsConditionNotes(ddlInwrdType);
                        AddTestToGridGT(ddlOtherTest.SelectedItem.Text);

                    }
                    else if (ddlInwrdType != "Soil Investigation" && ddlInwrdTypeValue != "GT")
                    {
                        hideGTFields();
                        ChkLumpSum();
                        Grd_Note.DataSource = null;
                        Grd_Note.DataBind();
                        Grd_NoteGT.DataSource = null;
                        Grd_NoteGT.DataBind();
                        grdPayTermsGT.DataSource = null;
                        grdPayTermsGT.DataBind();
                        //ddlInwrdType = ddl_InwardType.SelectedItem.Text;
                        if (ddlInwrdTypeValue == "RWH")
                            ddlInwrdType = "Rain Water Harvesting";
                        else if (ddlOtherTest.Visible == true)
                        {
                            if (ddlOtherTest.SelectedItem.Text == "Plate Load Testing")
                            {
                                ddlInwrdType = "Plate Load Testing";
                            }
                            else if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test")
                            {
                                ddlInwrdType = "Earth Resistivity Test";
                            }
                        }
                        bool addTest = true;
                        if (ddl_InwardType.SelectedValue == "OT" && ddlOtherTest.SelectedItem.Text == "Structural Audit")
                            addTest = false;
                        if (addTest == true)
                            AddTestToProposalGrid(ddlInwrdTypeValue);
                        
                        AddTermsConditionNotes(ddlInwrdType);
                    }
                    DisplayGenericDiscountDetails();
                }

            }

        }
        private void DisplayStructuralAuditDetails()
        {
            int age = 0, area = 0, category1 = 0, category2 = 0;
            if (txtStructAge.Text != "")
            {
                age = Convert.ToInt32(txtStructAge.Text);
            }
            if (txtStructBuiltupArea.Text != "")
            {
                area = Convert.ToInt32(txtStructBuiltupArea.Text);
            }

            if (age < 15)
                category1 = 1;
            else if (age >= 15 && age <= 25)
                category1 = 2;
            else if (age > 25)
                category1 = 3;

            if (ddlStructLocation.SelectedValue == "Noncoastal")
                category1 = 2;
            if (ddlStructAddLoadExpc.SelectedValue == "Yes")
                category1 = 3;
            if (ddlStructDistressObs.SelectedValue == "Yes")
                category1 = 3;
            //if (ddlStructAddLoadExpc.SelectedValue == "Yes")
            //    category1 = 3;
            if (area < 50000)
                category2 = 1;
            else
                category2 = 2;

            for (int i = 0; i <= 4; i++)
            {
                AddRowProposal();
                TextBox txt_Particular = (TextBox)grdProposal.Rows[i].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdProposal.Rows[i].FindControl("txt_TestMethod");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[i].FindControl("txt_Rate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].FindControl("txt_Quantity");
                TextBox txt_Discount = (TextBox)grdProposal.Rows[i].FindControl("txt_Discount");
                TextBox txt_DisRate = (TextBox)grdProposal.Rows[i].FindControl("txt_DiscRate");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[i].FindControl("txt_Amount");
                Label lbl_TestId = (Label)grdProposal.Rows[i].FindControl("lbl_TestId");

                if (category1 == 1 && category2 == 1)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "10";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "10";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "3";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "3";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "3";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 1 && category2 == 2)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "30";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "30";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 2 && category2 == 1)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "6";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "6";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "6";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 2 && category2 == 2)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "35";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "35";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "12";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "12";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "12";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 3 && category2 == 1)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "20";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "20";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 3 && category2 == 2)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "40";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "40";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
            }
        }
        protected void lnkAddNewRowToProposalGrid_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (ddl_InwardType.SelectedItem.Text != "----------------------------Select----------------------------")
            {
                lblMsg.Visible = false;
                ModalPopupExtender2.Show();
                txt_PerticularName.Text = "";
                txt_UnitRate.Text = "";
                txt_DiscountRate.Text = "";
                lblFlag.Text = "0";
                lblRecType.Text = "";
                if (ddl_InwardType.SelectedValue == "GT")
                    lblRecType.Text = "GT";
                else if (ddl_InwardType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.Visible == true)
                    {
                        if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                            lblRecType.Text = "SPT";
                        else if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                            lblRecType.Text = "WT";
                    }
                }

            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Select Inward Type";
                lblMsg.ForeColor = System.Drawing.Color.Red;

            }
        }
        private void AddTestToProposalGrid(string testType)
        {
            int testId; double siteRate = 0, disc = 0, calDisc = 0, discountedRate = 0;
            int MaterialId = -1, subTestId = 0;
            string subTestOt = "";
            if (ddl_InwardType.SelectedValue == "OT")
            {

                if (ddlOtherTest.SelectedValue.ToString() != "")
                {
                    subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
                    testType = "Other";
                    subTestOt = ddlOtherTest.SelectedItem.Text.ToString();
                }
            }
            if (grdProposal.Visible == true)
            {
                //AddTermsConditionNotes(ddl_InwardType.SelectedItem.Text.ToString());
                var res = dc.Test_View_ForProposal(MaterialId, 0, testType, 0, 0, subTestId);
                int i = 0;
                string testName = "", str = "";
                foreach (var re in res)
                {
                    bool flag = false;

                    for (int j = 0; j < grdProposal.Rows.Count; j++)//check if datagrid have same test  
                    {
                        TextBox txtParticular = (TextBox)grdProposal.Rows[j].Cells[3].FindControl("txt_Particular");

                        if (re.Test_RecType_var == "MF")
                        {
                            int frmNum = Convert.ToInt32(re.TEST_From_num);
                            int toNum = Convert.ToInt32(re.TEST_To_num);

                            if (toNum == 30)
                                str = "Upto M30";
                            else if (frmNum == 31)
                                str = "Above M30";
                            else
                                str = "";

                            testName = re.TEST_Name_var.ToString() + " " + str;
                        }
                        else
                            testName = re.TEST_Name_var.ToString();

                        if (testType == "CEMT" && testName == "Compressive Strength")
                        {
                            testName = "Compressive Strength (3, 7, 28)";
                        }
                        if (testType == "FLYASH" && testName == "Compressive Strength")
                        {
                            testName = "Compressive Strength (7, 28, 90)";
                        }
                        if (txtParticular.Text == testName)//&& txtRate.Text == Convert.ToDecimal(re.TEST_Rate_int).ToString("0.00"))
                        {

                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        testId = Convert.ToInt32(re.TEST_Id);
                        siteRate = chkSiteWiseRate(testId);
                        AddRowProposal();

                        TextBox txt_Particular = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[5].FindControl("txt_Rate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[6].FindControl("txt_Discount");
                        TextBox txt_DiscRate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        TextBox txt_Quantity = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[8].FindControl("txt_Quantity");
                        TextBox txt_Amount = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[9].FindControl("txt_Amount");
                        Label lbl_InwdType = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[10].FindControl("lbl_InwdType");
                        Label lbl_SiteWiseRate = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[11].FindControl("lbl_SiteWiseRate");
                        Label lbl_TestId = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[12].FindControl("lbl_TestId");

                        if (testType == "ST")
                            txt_Particular.Text = re.TEST_Name_var.ToString() + "(" + re.TEST_From_num.ToString() + "mm - " + re.TEST_To_num.ToString() + "mm)";
                        else if (testType == "MF")
                        {
                            int frmNum = Convert.ToInt32(re.TEST_From_num);
                            int toNum = Convert.ToInt32(re.TEST_To_num);
                            if (toNum == 30)
                                str = "Upto M30";
                            else if (frmNum == 31)
                                str = "Above M30";
                            else
                                str = "";

                            txt_Particular.Text = re.TEST_Name_var.ToString() + " " + str;
                        }
                        else
                            txt_Particular.Text = re.TEST_Name_var.ToString();

                        if (Convert.ToString(re.TEST_Method_var) != "" && Convert.ToString(re.TEST_Method_var) != null)
                            txt_TestMethod.Text = re.TEST_Method_var.ToString();
                        else
                            txt_TestMethod.Text = "NA";

                        txt_Rate.Text = re.TEST_Rate_int.ToString();

                        if (testType == "Other")
                            lbl_InwdType.Text = "OTHER";
                        else
                            lbl_InwdType.Text = ddl_InwardType.SelectedItem.Value;

                        //txt_Quantity.Text = "1";

                        lbl_TestId.Text = re.TEST_Id.ToString();
                        if (siteRate != 0)//apply sitewise rate setting
                        {
                            txt_DiscRate.Text = siteRate.ToString("0.00");
                            //calculate rev. discount
                            calDisc = Math.Round(((Convert.ToDouble(re.TEST_Rate_int) - (Convert.ToDouble(siteRate))) * 100) / Convert.ToDouble(re.TEST_Rate_int));
                            //txt_Discount.Text = calDisc.ToString();
                            txt_Discount.Text = "-";
                            lbl_SiteWiseRate.Text = siteRate.ToString();
                        }
                        else //apply discount setting
                        {
                            if (lbl_InwdType.Text == "OTHER")
                                disc = getDiscount("OT");
                            else
                                disc = getDiscount(lbl_InwdType.Text);

                            txt_Discount.Text = disc.ToString();
                            discountedRate = Convert.ToDouble(re.TEST_Rate_int) - (Convert.ToDouble(re.TEST_Rate_int) * (disc / 100));
                            txt_DiscRate.Text = discountedRate.ToString("0.00");
                            lbl_SiteWiseRate.Text = "0";
                        }
                        //if (testType == "CEMT" && txt_Particular.Text == "Compressive Strength")
                        //{
                        //    txt_Particular.Text = "Compressive Strength (3, 7, 28)";
                        //    txt_Rate.Text = (re.TEST_Rate_int * 3).ToString();
                        //    txt_DiscRate.Text = (Convert.ToDecimal(txt_DiscRate.Text) * 3).ToString();
                        //}
                        //if (testType == "FLYASH" && txt_Particular.Text == "Compressive Strength")
                        //{
                        //    txt_Particular.Text = "Compressive Strength (7, 28, 90)";
                        //    txt_Rate.Text = (re.TEST_Rate_int * 3).ToString();
                        //    txt_DiscRate.Text = (Convert.ToDecimal(txt_DiscRate.Text) * 3).ToString();
                        //}
                        i++;
                    }
                }

            }
        }
        protected void AddRowProposal()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ProposalTable"] != null)
            {
                GetCurrentDataProposal();
                dt = (DataTable)ViewState["ProposalTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Rate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Discount", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_SiteWiseRate", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));
            }
            dr = dt.NewRow();

            dr["txt_Particular"] = string.Empty;
            dr["txt_TestMethod"] = string.Empty;
            dr["txt_Rate"] = string.Empty;
            dr["txt_Discount"] = string.Empty;
            dr["txt_DiscRate"] = string.Empty;
            dr["txt_Quantity"] = string.Empty;
            dr["txt_Amount"] = string.Empty;
            if (ddl_InwardType.SelectedIndex != -1)
            {
                if (ddlOtherTest.Visible == true && ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.SelectedIndex != 0)
                    dr["lbl_InwdType"] = "OTHER";
                else
                    dr["lbl_InwdType"] = ddl_InwardType.SelectedItem.Value;
            }
            else
                dr["lbl_InwdType"] = string.Empty;
            dr["lbl_SiteWiseRate"] = string.Empty;
            dr["lbl_TestId"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ProposalTable"] = dt;
            grdProposal.DataSource = dt;
            grdProposal.DataBind();
            SetPreviousDataProposal();
        }
        protected void SetPreviousDataProposal()
        {
            DataTable dt = (DataTable)ViewState["ProposalTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdProposal.Rows[i].Cells[3].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdProposal.Rows[i].Cells[4].FindControl("txt_TestMethod");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[i].Cells[5].FindControl("txt_Rate");
                TextBox txt_Discount = (TextBox)grdProposal.Rows[i].Cells[6].FindControl("txt_Discount");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].Cells[8].FindControl("txt_Quantity");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[i].Cells[9].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[10].FindControl("lbl_InwdType");
                Label lbl_SiteWiseRate = (Label)grdProposal.Rows[i].Cells[11].FindControl("lbl_SiteWiseRate");
                Label lbl_TestId = (Label)grdProposal.Rows[i].Cells[12].FindControl("lbl_TestId");

                txt_Particular.Text = dt.Rows[i]["txt_Particular"].ToString();
                txt_TestMethod.Text = dt.Rows[i]["txt_TestMethod"].ToString();
                txt_Rate.Text = dt.Rows[i]["txt_Rate"].ToString();
                txt_Discount.Text = dt.Rows[i]["txt_Discount"].ToString();
                txt_DiscRate.Text = dt.Rows[i]["txt_DiscRate"].ToString();
                txt_Quantity.Text = dt.Rows[i]["txt_Quantity"].ToString();
                txt_Amount.Text = dt.Rows[i]["txt_Amount"].ToString();
                lbl_InwdType.Text = dt.Rows[i]["lbl_InwdType"].ToString();
                lbl_SiteWiseRate.Text = dt.Rows[i]["lbl_SiteWiseRate"].ToString();
                lbl_TestId.Text = dt.Rows[i]["lbl_TestId"].ToString();
            }
        }
        protected void GetCurrentDataProposal()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Rate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Discount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_SiteWiseRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));

            for (int i = 0; i < grdProposal.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdProposal.Rows[i].Cells[3].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdProposal.Rows[i].Cells[4].FindControl("txt_TestMethod");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[i].Cells[5].FindControl("txt_Rate");
                TextBox txt_Discount = (TextBox)grdProposal.Rows[i].Cells[6].FindControl("txt_Discount");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].Cells[8].FindControl("txt_Quantity");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[i].Cells[9].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[10].FindControl("lbl_InwdType");
                Label lbl_SiteWiseRate = (Label)grdProposal.Rows[i].Cells[11].FindControl("lbl_SiteWiseRate");
                Label lbl_TestId = (Label)grdProposal.Rows[i].Cells[12].FindControl("lbl_TestId");


                drRow = dtTable.NewRow();
                drRow["txt_Particular"] = txt_Particular.Text;
                drRow["txt_TestMethod"] = txt_TestMethod.Text;
                drRow["txt_Rate"] = txt_Rate.Text;
                drRow["txt_Discount"] = txt_Discount.Text;
                drRow["txt_DiscRate"] = txt_DiscRate.Text;
                drRow["txt_Quantity"] = txt_Quantity.Text;
                drRow["txt_Amount"] = txt_Amount.Text;
                drRow["lbl_InwdType"] = lbl_InwdType.Text;
                drRow["lbl_SiteWiseRate"] = lbl_SiteWiseRate.Text;
                drRow["lbl_TestId"] = lbl_TestId.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ProposalTable"] = dtTable;
        }
        protected void imgEdit_Click(object sender, CommandEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            Label lbl_TestId = (Label)grdProposal.Rows[gvr.RowIndex].Cells[11].FindControl("lbl_TestId");
            if (lbl_TestId.Text == "0")
            {
                ModalPopupExtender2.Show();
                TextBox txt_Particular = (TextBox)grdProposal.Rows[gvr.RowIndex].Cells[3].FindControl("txt_Particular");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[gvr.RowIndex].Cells[4].FindControl("txt_Rate");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[gvr.RowIndex].Cells[6].FindControl("txt_DiscRate");

                txt_PerticularName.Text = txt_Particular.Text;
                txt_UnitRate.Text = txt_Rate.Text;
                txt_DiscountRate.Text = txt_DiscRate.Text;
                lblFlag.Text = gvr.RowIndex.ToString();
            }

        }
        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdProposal.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdProposal.Rows[gvr.RowIndex].Cells[1].Text == "")
                {
                    DeleteRowProposal(gvr.RowIndex);
                    GetCurrentDataDiscount();
                }
            }
        }

        protected void lnkAddNewPerticularToGrid_Click(object sender, EventArgs e)
        {
            if (grdProposal.Visible == true)
            {
                if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                {
                    double calDisc = 0;
                    ModalPopupExtender2.Hide();
                    if (lblFlag.Text == "0")//0 indicates new row added by lnkAddNewRow click
                    {
                        AddRowProposal();
                        TextBox txt_Particular = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[5].FindControl("txt_Rate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[6].FindControl("txt_Discount");
                        TextBox txt_DisRate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_TestId = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[12].FindControl("lbl_TestId");



                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_TestMethod.Text = txt_TestMethod.Text;
                        if (txt_TestMethod.Text == "")
                            txt_TestMethod.Text = "NA";
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DisRate.Text = txt_DiscountRate.Text;
                        calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(txt_DisRate.Text))) * 100) / Convert.ToDouble(txt_Rate.Text));
                        txt_Discount.Text = calDisc.ToString();
                        lbl_TestId.Text = "0";
                    }
                    else // edit added row
                    {
                        int rowindex = Convert.ToInt32(lblFlag.Text);
                        TextBox txt_Particular = (TextBox)grdProposal.Rows[rowindex].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[rowindex].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[rowindex].Cells[5].FindControl("txt_Rate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[rowindex].Cells[6].FindControl("txt_Discount");
                        TextBox txt_DisRate = (TextBox)grdProposal.Rows[rowindex].Cells[7].FindControl("txt_DiscRate");
                        calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(txt_DisRate.Text))) * 100) / Convert.ToDouble(txt_Rate.Text));
                        Label lbl_TestId = (Label)grdProposal.Rows[rowindex].Cells[12].FindControl("lbl_TestId");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_TestMethod.Text = txt_TestMethod.Text;
                        if (txt_TestMethod.Text == "")
                            txt_TestMethod.Text = "NA";

                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_Discount.Text = calDisc.ToString();
                        txt_DisRate.Text = txt_DiscountRate.Text;
                        lbl_TestId.Text = "0";

                    }
                }
            }
            else if (grdGT.Visible == true)
            {
                int FromRowNo = 0, ToRowNo = 0;
                if (chkLums.Checked)
                {
                    if (txtFrmRow.Text != "" && txtToRow.Text != "")
                    {
                        FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                        ToRowNo = Convert.ToInt32(txtToRow.Text);


                    }
                }

                if (lblRecType.Text == "GT")
                {
                    if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                    {
                        ModalPopupExtender2.Hide();
                        AddRowProposalGT();
                        TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                        TextBox txt_Rate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DiscRate.Text = txt_DiscountRate.Text;
                        lbl_InwdType.Text = "GT";

                        ShowMerge(FromRowNo, ToRowNo, "GT");
                    }
                }
                else if (lblRecType.Text == "SPT")
                {
                    if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                    {
                        ModalPopupExtender2.Hide();
                        AddRowProposalGT();
                        TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                        TextBox txt_Rate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DiscRate.Text = txt_DiscountRate.Text;
                        lbl_InwdType.Text = "OTHER";

                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    }
                }
                else if (lblRecType.Text == "WT")//Water Test for Drinking/Domestic Purpose
                {
                    if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                    {
                        ModalPopupExtender2.Hide();
                        AddRowProposalGT();
                        TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                        TextBox txt_Rate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DiscRate.Text = txt_DiscountRate.Text;
                        lbl_InwdType.Text = "OTHER";

                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    }
                }
            }

        }
        protected void imgExitFromNewPerticular_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender2.Hide();
        }

        //protected void ddlDepth_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlDepth.SelectedIndex == 0)
        //    {
        //        txtGtRate.ReadOnly = true; txtGtRate.Text = lblGtRate.Text;
        //    }
        //    else
        //        txtGtRate.ReadOnly = false;
        //}
        protected void lnkDepth_Click(object sender, EventArgs e)
        {
            txtGtRate.Text = lblGtRate.Text;
            ddlDepth.SelectedIndex = 0;
            //txtGtRate.ReadOnly = true;
            ModalPopupExtender3.Show();

        }

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
                //string[] eid = s.Split(',');
                //for (int i = 0; i < eid.Length; i++)
                //{
                //    string strRegex = @"/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/";
                //    Regex re = new Regex(strRegex);
                //    if (!(re.IsMatch(eid[i].ToString())))
                //        return false;
                //}
                //return true;


                //var result = s.Split(',');
                //for (var i = 0; i < result.Length; i++)
                //    if (!validateEmail(result[i]))
                //        return false;
                //return true;
            }
        }
        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ViewState["ProposalTable"] = null;
            grdProposal.DataSource = null;
            grdProposal.DataBind();

            ViewState["ProposalTableGT"] = null;
            grdGT.DataSource = null;
            grdGT.DataBind();
            grdProposal.DataSource = null;
            grdProposal.DataBind();

            Grd_Note.DataSource = null;
            Grd_Note.DataBind();
            Grd_NoteGT.DataSource = null;
            Grd_NoteGT.DataBind();
            grdClientScope.DataSource = null;
            grdClientScope.DataBind();
            grdPayTermsGT.DataSource = null;
            grdPayTermsGT.DataBind();

            txtPaymentTerm.Text = "";

            Tab_Discount.CssClass = "Initial";
            Tab_Notes.CssClass = "Clicked";
            Tab_GenericDiscount.CssClass = "Initial";
            MainView_Proposal.ActiveViewIndex = 0;
        }

        #endregion
        public void SendMailAfterEnquiryRegistered(string enquiryId, string inwardType, string mailIdTo, string mailIdCc, DateTime enqDate)
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
                if (mailIdCc != "")
                    mCC = mailIdCc;
                mSubject = "Enquiry Registered";

                mbody = "Dear Customer,<br><br>";
                mbody = mbody + "We thank you for your enquiry (no. " + enquiryId + ") on date " + enqDate.ToString("dd/MM/yyyy") + ".";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "Best Regards,";
                mbody = mbody + "<br>";
                mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";


                try
                {
                    objMail.SendMail(mailIdTo, mCC, mSubject, mbody, "", mReplyTo);

                }
                catch { }
            }

        }

        public int chkRedTagClient(int strEnqId)
        {
            int redTagClient = 0;
            var viewEnquiry = dc.Enquiry_View(Convert.ToInt32(strEnqId), 0, 0);
            foreach (var Enqry in viewEnquiry)
            {
                redTagClient = Convert.ToInt32(Enqry.CL_RedTag_bit);
            }

            return redTagClient;
        }
        private void AutoApprovalOfEnquiry(int strEnqId, byte enqStatus, DateTime? ApproveDate, DateTime? EditedCollectionDate)
        {
            if (enqStatus == 0)
            {
                int redTagClient = 0;
                redTagClient=chkRedTagClient(strEnqId);

                if (redTagClient == 0)//approval for only client who is not redTagged
                {
                    if (dc.Connection.Database.ToLower() == "veenalive")
                    {
                        //assign that enq to driver
                        var enqDetail = dc.Enquiry_View_ForPickup(Convert.ToInt32(strEnqId));
                        foreach (var enq in enqDetail)
                        {
                            //if (enq.CL_Id == 4158 || enq.CL_Id == 14367 || enq.CL_Id == 9739
                            //    || enq.CL_Id == 11420
                            //    || enq.CL_Id == 14067
                            //    || enq.CL_Id == 14569 || enq.CL_Id == 13591 || enq.CL_Id == 13647 || enq.CL_Id == 13754 || enq.CL_Id == 8681
                            //    || enq.CL_Id == 14257 || enq.CL_Id == 12825)
                            //{
                            //    return;
                            //}
                            ApproveDate = DateTime.Now;
                            dc.Enquiry_Update_Approve(Convert.ToInt32(strEnqId), enqStatus, "Auto Approval", EditedCollectionDate, ApproveDate, Convert.ToInt32(Session["LoginId"]));

                            dc.PickUpAllocation_Update(Convert.ToInt32(strEnqId), enq.CL_Id, enq.SITE_Id, enq.LOCATION_Id, enq.ENQ_ROUTE_Id, enq.ENQ_CollectionDate_dt, enq.CONT_Name_var, enq.unUsedCoupon.ToString(), enq.ME_Name, enq.CONT_ContactNo_var, enq.Driver_id, enq.MATERIAL_Name_var, Convert.ToInt32(enq.ENQ_Quantity), enq.ENQ_ContactNoForCollection_var, enq.ENQ_ContactPersonForCollection_var, false);
                        }
                    }
                    else
                    {
                        ApproveDate = DateTime.Now;
                        dc.Enquiry_Update_Approve(Convert.ToInt32(strEnqId), enqStatus, "Auto Approval", EditedCollectionDate, ApproveDate, Convert.ToInt32(Session["LoginId"]));
                        var enqDetail = dc.Enquiry_View_ForPickup(Convert.ToInt32(strEnqId));
                        foreach (var enq in enqDetail)
                        {
                            dc.PickUpAllocation_Update(Convert.ToInt32(strEnqId), enq.CL_Id, enq.SITE_Id, enq.LOCATION_Id, enq.ENQ_ROUTE_Id, enq.ENQ_CollectionDate_dt, enq.CONT_Name_var, enq.unUsedCoupon.ToString(), enq.ME_Name, enq.CONT_ContactNo_var, enq.Driver_id, enq.MATERIAL_Name_var, Convert.ToInt32(enq.ENQ_Quantity), enq.ENQ_ContactNoForCollection_var, enq.ENQ_ContactPersonForCollection_var, false);
                        }
                    }
                }
            }

        }

        public void LoadUserList()
        {
            var user = dc.User_View(0, 0, "", "", "");
            ddl_PrposalBy.DataSource = user;
            ddl_PrposalBy.DataTextField = "USER_Name_var";
            ddl_PrposalBy.DataValueField = "USER_Id";
            ddl_PrposalBy.DataBind();
            ddl_PrposalBy.Items.Insert(0, new ListItem("----------------------------Select----------------------------", "0"));
        }
        protected void LoadOtherTestList()
        {
            ddlOtherTest.Items.Clear();
            var a = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            ddlOtherTest.DataSource = a;
            ddlOtherTest.DataTextField = "TEST_Name_var";
            ddlOtherTest.DataValueField = "TEST_Id";
            ddlOtherTest.DataBind();
            ddlOtherTest.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        public string DisplayHeader(string Heading)
        {
            txt_ProposalDt.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddl_PrposalBy.SelectedValue = Convert.ToString(Session["LoginId"]);
            txt_Description.Text = "This has reference to our discussion for requirement of testing services as mentioned below. It will be a pleasure for us to provide testing services to your project. Please find the proposal below; ";
            if (!chkNewClient.Checked)//existing system client
            {

                Heading = "Proposal " + "(" + Convert.ToString(ddl_InwardType.SelectedItem.Text) + ")";
                txt_KindAttention.Text = Convert.ToString(txt_ContactPerson.Text);
                //lblPrvRecType.Text = Convert.ToString(ddl_InwardType.SelectedItem.Text);
                if (lblRouteId.Text != "")
                {
                    if (Convert.ToInt32(lblRouteId.Text) > 0)
                    {
                        var reslt = dc.User_View_ME(Convert.ToInt32(lblRouteId.Text)).ToList();
                        if (reslt.Count > 0)
                        {
                            txtMe.Text = Convert.ToString(reslt.FirstOrDefault().USER_Name_var);
                            txtMeNum.Text = Convert.ToString(reslt.FirstOrDefault().USER_ContactNo_var);
                            lblMEEmail.Text = Convert.ToString(reslt.FirstOrDefault().USER_EmailId_var);
                        }
                    }
                }
                txt_Subject.Text = "Commercial offer for " + Convert.ToString(ddl_InwardType.SelectedItem.Text) + " requirement for your site " + txt_Site.Text;
            }
            else
            {
                //var res1 = dc.EnquiryNewClient_View_Details(Convert.ToInt32(txt_EnquiryNo.Text), false).ToList();
                //foreach (var e in res1)
                //{
                Heading = "Proposal " + "(" + Convert.ToString(ddl_InwardType.SelectedItem.Text) + ")";
                //txt_EnquiryNo.Text = Convert.ToString(e.ENQNEW_Id);
                txt_KindAttention.Text = Convert.ToString(txt_ContactPerson.Text);
                //lblRType.Text = Convert.ToString(e.MATERIAL_Name_var);
                txtMe.Text = "";
                txtMeNum.Text = "";
                txt_Subject.Text = "Commercial offer for " + Convert.ToString(ddl_InwardType.SelectedItem.Text) + " requirement for your site " + txt_Site.Text;

                //txtProContactNo.ReadOnly = false;
                lnkUpdateIntro.Visible = false;
            }

            string mailCC = "ravindra.j@durocrete.acts-int.com,sandeep.s@durocrete.acts-int.com,billing@durocrete.acts-int.com,marketing@durocrete.acts-int.com,sampada.k@durocrete.acts-int.com";
            if (cnStr.ToLower().Contains("mumbai") == true)
                mailCC += ",neeta.n@durocrete.acts-int.com,mahesh.kamble@durocrete.acts-int.com";
            if (lblMEEmail.Text != "")
            {
                txtMailCC.Text = mailCC + "," + lblMEEmail.Text;
            }
            else
            {
                txtMailCC.Text = mailCC;
            }
            //puran.m@durocrete.acts-int.com,mahesh.kamble@durocrete.acts-int.com,girish.k@durocrete.acts-int.com --mumabi
            return Heading;
        }
        protected void lnkAddNewRowToGrid_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (ddl_InwardType.SelectedItem.Text != "----------------------------Select----------------------------" && lblMonthlyStatus.Text == "0" && ddl_MatCollectn.SelectedItem.Text != "Declined by us" && ddl_MatCollectn.SelectedItem.Text != "Declined by Client" && chkConifrmEnq.Checked == true)//&& chkNewClient.Checked != true
            {
                lblMsg.Visible = false;
                ModalPopupExtender2.Show();
                txt_PerticularName.Text = "";
                txt_UnitRate.Text = "";
                txt_DiscountRate.Text = "";
                lblFlag.Text = "0";
                lblRecType.Text = "";
                if (ddl_InwardType.SelectedValue == "GT")
                    lblRecType.Text = "GT";
                else if (ddl_InwardType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.Visible == true)
                    {
                        if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                            lblRecType.Text = "SPT";
                        else if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                            lblRecType.Text = "WT";
                    }
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Select Inward Type";
                lblMsg.ForeColor = System.Drawing.Color.Red;

            }
        }
        protected void imgExitDepth_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender3.Hide();
        }
        protected void lnkUpdateDepthRate_Click(object sender, EventArgs e)
        {
            ModalPopupExtender3.Hide();
            if (ddlDepth.SelectedIndex != -1 && txtGtRate.Text != "")//means 15m
            {
                int FromRowNo = 2, ToRowNo = 8;
                if (txtFrmRow.Text != "")
                    FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                if (txtToRow.Text != "")
                    ToRowNo = Convert.ToInt32(txtToRow.Text);

                for (int i = 0; i < grdGT.Rows.Count; i++)
                {
                    if (FromRowNo == i)
                    {
                        TextBox txt_Rate = (TextBox)grdGT.Rows[i - 1].Cells[5].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[i - 1].Cells[6].FindControl("txt_DiscRate");
                        if (ddlDepth.SelectedItem.Text == "10 m")
                        {
                            txt_Rate.Text = lbl10mDepthRate.Text;
                            txt_DiscRate.Text = lbl10mDepthRate.Text;
                        }
                        else
                        {
                            txt_Rate.Text = txtGtRate.Text;
                            txt_DiscRate.Text = txtGtRate.Text;
                        }
                        break;
                    }

                }
            }
        }

        protected void Tab_Discount_Click(object sender, EventArgs e)
        {
            Tab_Discount.CssClass = "Clicked";
            Tab_Notes.CssClass = "Initial";
            Tab_GenericDiscount.CssClass = "Initial";
            MainView_Proposal.ActiveViewIndex = 1;

            ViewState["Discount_Table"] = null;
            string testNm = "";
            if (grdProposal.Visible == true)
            {
                for (int i = 0; i < grdProposal.Rows.Count; i++)
                {
                    TextBox txt_Particular = (TextBox)grdProposal.Rows[i].Cells[3].FindControl("txt_Particular");
                    TextBox txt_Rate = (TextBox)grdProposal.Rows[i].Cells[4].FindControl("txt_Rate");
                    TextBox txt_Discount = (TextBox)grdProposal.Rows[i].Cells[5].FindControl("txt_Discount");
                    TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].Cells[6].FindControl("txt_DiscRate");
                    Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[9].FindControl("lbl_InwdType");
                    Label lbl_SiteWiseRate = (Label)grdProposal.Rows[i].Cells[10].FindControl("lbl_SiteWiseRate");


                    AddRowDiscount();
                    Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                    Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                    TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                    TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                    TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");


                    lblMaterialName.Text = lbl_InwdType.Text;
                    if (i == 0)
                        testNm = lbl_InwdType.Text;

                    if (i != 0)
                    {
                        if (lblMaterialName.Text == testNm)
                            lblMaterialName.Text = "";
                        else
                            testNm = lbl_InwdType.Text;
                    }
                    lblTestName.Text = txt_Particular.Text;
                    if (txt_Discount.Text == "-")
                    {
                        double calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(lbl_SiteWiseRate.Text))) * 100) / Convert.ToDouble(txt_Rate.Text));

                        txtSiteWiseDisc.Text = calDisc.ToString();
                        txtAppliedDisc.Text = calDisc.ToString();
                        txtVolDisc.Text = "-";
                    }
                    else if (txt_Discount.Text != "-")
                    {
                        string dis = "0";
                        if (txt_Discount.Text != "")
                            dis = txt_Discount.Text;

                        txtVolDisc.Text = dis;
                        txtAppliedDisc.Text = dis;
                        txtSiteWiseDisc.Text = "-";
                    }

                }

            }
            else
            {
                Label lbl_InwdType = (Label)grdGT.Rows[0].Cells[9].FindControl("lbl_InwdType");

                AddRowDiscountGT();
                Label lblMaterialName = (Label)grdDiscount.Rows[0].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[0].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[0].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[0].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[0].Cells[4].FindControl("txtAppliedDisc");


                lblMaterialName.Text = lbl_InwdType.Text;
                double disc = getDiscount(lbl_InwdType.Text);
                lblTestName.Text = "";
                if (disc > 0)
                {
                    txtVolDisc.Text = disc.ToString();
                    txtAppliedDisc.Text = disc.ToString();
                    txtSiteWiseDisc.Text = "-";


                }
                else
                {

                    txtSiteWiseDisc.Text = "0";
                    txtAppliedDisc.Text = "0";
                    txtVolDisc.Text = "-";
                }

                //}

            }

        }
        protected void Tab_Notes_Click(object sender, EventArgs e)
        {
            Tab_Notes.CssClass = "Clicked";
            Tab_Discount.CssClass = "Initial";
            Tab_GenericDiscount.CssClass = "Initial";
            MainView_Proposal.ActiveViewIndex = 0;
        }
        protected void Tab_GenericDiscount_Click(object sender, EventArgs e)
        {
            Tab_Notes.CssClass = "Initial";
            Tab_Discount.CssClass = "Initial";
            Tab_GenericDiscount.CssClass = "Clicked";
            MainView_Proposal.ActiveViewIndex = 2;
            //DisplayGenericDiscountDetails();
        }

        ////Client Scope NDT CR CORECUT
        protected void AddRowClientScope()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ClientScopeTable"] != null)
            {
                GetCurrentDataClientScope();
                dt = (DataTable)ViewState["ClientScopeTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNoClientScope", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NOTEClientScope", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNoClientScope"] = dt.Rows.Count + 1;
            dr["txt_NOTEClientScope"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ClientScopeTable"] = dt;
            grdClientScope.DataSource = dt;
            grdClientScope.DataBind();
            SetPreviousDataClientScope();
        }
        protected void SetPreviousDataClientScope()
        {
            DataTable dt = (DataTable)ViewState["ClientScopeTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)grdClientScope.Rows[i].Cells[2].FindControl("txt_NOTEClientScope");
                grdClientScope.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_NOTE.Text = dt.Rows[i]["txt_NOTEClientScope"].ToString();

            }
        }
        protected void GetCurrentDataClientScope()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNoClientScope", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NOTEClientScope", typeof(string)));
            for (int i = 0; i < grdClientScope.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)grdClientScope.Rows[i].Cells[2].FindControl("txt_NOTEClientScope");

                drRow = dtTable.NewRow();
                drRow["lblSrNoClientScope"] = i + 1;
                drRow["txt_NOTEClientScope"] = txt_NOTE.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ClientScopeTable"] = dtTable;
        }
        protected void DeleteRowClientScope(int rowIndex)
        {
            GetCurrentDataClientScope();
            DataTable dt = ViewState["ClientScopeTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ClientScopeTable"] = dt;
            grdClientScope.DataSource = dt;
            grdClientScope.DataBind();
            SetPreviousDataClientScope();
        }
        protected void imgBtnAddClientScopeRow_Click(object sender, CommandEventArgs e)
        {
            AddRowClientScope();
        }
        protected void imgBtnDeleteClientScopeRow_Click(object sender, CommandEventArgs e)
        {
            if (grdClientScope.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdClientScope.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowClientScope(gvr.RowIndex);
            }
        }

        //// GT
        protected void ddlOtherTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlStructAudit.Visible == true)
            {
                grdProposal.DataSource = null;
                grdProposal.DataBind();
                pnlStructAudit.Visible = false;
                grdProposal.Columns[3].HeaderText = "Particular";
                grdProposal.Columns[4].HeaderText = "Test Method";
            }
            if (ddl_InwardType.SelectedValue == "OT")
            {
                if (ddlOtherTest.SelectedItem.Text == "Structural Audit")
                {
                    grdProposal.DataSource = null;
                    grdProposal.DataBind();
                    pnlStructAudit.Visible = true;
                    grdProposal.Columns[3].HeaderText = "Members";
                    grdProposal.Columns[4].HeaderText = "Samples";
                }
                if (ddlOtherTest.SelectedItem.Text == "Plate Load Testing" || ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                {
                    Grd_Note.DataSource = null;
                    Grd_Note.DataBind();
                    Grd_NoteGT.DataSource = null;
                    Grd_NoteGT.DataBind();
                    grdPayTermsGT.DataSource = null;
                    grdPayTermsGT.DataBind();
                    AddTermsConditionNotes("Soil Investigation");
                }
                else
                {
                    Grd_NoteGT.Visible = false;
                    lblAddChargesGT.Visible = false;
                    Grd_Note.Visible = true;
                    Grd_Note.DataSource = null;
                    Grd_Note.DataBind();                    
                    grdPayTermsGT.Visible = false;
                    lblPayTermsGT.Visible = false;
                    //AddTermsConditionNotes("OT");
                    AddTermsConditionNotes(ddlOtherTest.SelectedItem.Text);
                }

            }

        }
        public void ChkLumpSum()
        {
            if (chkLums.Checked == true)
            {
                txtFrmRow.Focus();
                txtFrmRow.Visible = true;
                txtToRow.Visible = true;
                btnApplyLumpsum.Visible = true;
                lblFromRow.Visible = true;
                lblToRow.Visible = true;
            }
            else
            {
                txtFrmRow.Text = string.Empty;
                txtFrmRow.Visible = false;
                txtToRow.Visible = false;
                txtToRow.Text = string.Empty;
                btnApplyLumpsum.Visible = false;
                lblFromRow.Visible = false;
                lblToRow.Visible = false;
            }
        }
        private void showGTFields()
        {
            grdGT.Visible = true;
            Grd_NoteGT.Visible = true;
            lblAddChargesGT.Visible = true;
            chkLums.Visible = true;
            lblLumSum.Visible = true;
            grdProposal.Visible = false;            
            grdPayTermsGT.Visible = true;
            lblPayTermsGT.Visible = true;
            lblPaymentTerm.Visible = false;
            txtPaymentTerm.Visible = false;
        }
        private void hideGTFields()
        {
            grdProposal.Visible = true;
            grdGT.Visible = false;
            Grd_NoteGT.Visible = false;
            lblAddChargesGT.Visible = false;
            chkLums.Visible = false; chkLums.Checked = false;
            lblLumSum.Visible = false;
            //lblClientScope.Visible = false;
            //grdClientScope.Visible = false;            
            grdPayTermsGT.Visible = false;
            lblPayTermsGT.Visible = false;
            lblPaymentTerm.Visible = true;
            txtPaymentTerm.Visible = true;
        }
        private void AddTestToGridGT(string testType)
        {
            if (testType == "GT")
            {
                string[] gtTest = new string[] {@"Mobilization of drilling equipments, accessories,personnel, etc. to site, and demobilization of the same after completion of work.",
                    "Setting and shifting of rig at borehole locations",
                    "Drilling exploratory holes of size, up to 100mm dia. in soil of all sorts and collecting disturbed samples (Soil where SPT N<50)",
                    "Drilling ‘NX’ size exploratory bore holes in rock,extracting rock cores, serially marking them as per specification. (Rock where SPT N >50)",
                    "Conducting Standard Penetration Test (SPT) in soil and collecting/preserving SPT soil samples at every 1.5 m vertical interval",
                    "Preservation & handing over of soil and rock samples on site during & after completion of job in wooden core boxes",
                    "Conducting Laboratory Test on Soil Samples- a) Sieve Analysis b) Atterberg’s Limits (LL, PL) c) Sulphate & Chloride d) Free Swell Index",
                    "Conducting Laboratory Test on Rock Core Samples- a) Dry Density b) Specific gravity c) Water absorption d) Porosity e) Saturated crushing Strength/ Point Load",
                    "Submission of geotechnical report including results of all field and laboratory tests, foundation recommendations, and any other relevant geotechnical issues",
                    "Core box Charges"};

                //LnkBtnCal.Visible = false;
                for (int o = 0; o <= 9; o++)
                {
                    AddRowProposalGT();
                    TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                    Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                    txt_Particular.Text = gtTest[o];
                    //lbl_InwdType.Text = "Soil Investigation";//ddl_InwardType.SelectedItem.Text;
                    lbl_InwdType.Text = "GT";
                }

                ShowMerge(2, 8, "GT");
            }
            else if (testType == "SBC by SPT" || testType == "Water Test for Drinking/Domestic Purpose")
            {
                string recType = "";
                int subTestId = 0;
                if (ddl_InwardType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.SelectedValue.ToString() != "")
                    {
                        subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
                        recType = "OTHER";
                    }
                }


                var res = dc.Test_View_ForProposal(-1, 0, recType, 0, 0, subTestId);


                foreach (var re in res)
                {
                    AddRowProposalGT();
                    TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                    TextBox txt_TestMethod = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[3].FindControl("txt_TestMethod");
                    Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                    txt_Particular.Text = re.TEST_Name_var.ToString();
                    if (Convert.ToString(re.TEST_Method_var) == "" || Convert.ToString(re.TEST_Method_var) == null)
                        txt_TestMethod.Text = "NA";
                    else
                        txt_TestMethod.Text = re.TEST_Method_var.ToString();
                    lbl_InwdType.Text = "OTHER";
                }

                //if (testType == "Water Test for Drinking/Domestic Purpose")
                //    grdGT.Columns[3].Visible = false;
                //else
                //    grdGT.Columns[3].Visible = true;

                if (testType == "SBC by SPT")
                    ShowMerge(2, 3, "OTHER");
                else
                    ShowMerge(1, 3, "OTHER");

            }
        }
        protected void AddRowProposalGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ProposalTableGT"] != null)
            {
                GetCurrentDataProposalGT();
                dt = (DataTable)ViewState["ProposalTableGT"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Unit", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_UnitRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));

            }
            dr = dt.NewRow();

            dr["txt_Particular"] = string.Empty;
            dr["txt_TestMethod"] = string.Empty;
            dr["txt_DiscRate"] = string.Empty;
            dr["txt_UnitRate"] = string.Empty;
            dr["txt_Unit"] = string.Empty;
            dr["txt_Quantity"] = string.Empty;
            dr["txt_Amount"] = string.Empty;
            if (ddl_InwardType.SelectedIndex != -1)
            {
                if (ddlOtherTest.Visible == true && ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.SelectedIndex != 0)
                    dr["lbl_InwdType"] = "OTHER";
                else
                    dr["lbl_InwdType"] = ddl_InwardType.SelectedItem.Value;
            }
            else
                dr["lbl_InwdType"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ProposalTableGT"] = dt;
            grdGT.DataSource = dt;
            grdGT.DataBind();
            SetPreviousDataProposalGT();
        }
        protected void SetPreviousDataProposalGT()
        {
            DataTable dt = (DataTable)ViewState["ProposalTableGT"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdGT.Rows[i].Cells[2].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                TextBox txt_Unit = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                TextBox txt_Quantity = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_UnitRate = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Amount = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdGT.Rows[i].Cells[9].FindControl("lbl_InwdType");

                txt_Particular.Text = dt.Rows[i]["txt_Particular"].ToString();
                txt_TestMethod.Text = dt.Rows[i]["txt_TestMethod"].ToString();
                txt_Unit.Text = dt.Rows[i]["txt_Unit"].ToString();
                txt_Quantity.Text = dt.Rows[i]["txt_Quantity"].ToString();
                txt_UnitRate.Text = dt.Rows[i]["txt_UnitRate"].ToString();
                txt_DiscRate.Text = dt.Rows[i]["txt_DiscRate"].ToString();
                txt_Amount.Text = dt.Rows[i]["txt_Amount"].ToString();
                lbl_InwdType.Text = dt.Rows[i]["lbl_InwdType"].ToString();
            }
        }
        protected void GetCurrentDataProposalGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Unit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_UnitRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));

            for (int i = 0; i < grdGT.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdGT.Rows[i].Cells[2].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                TextBox txt_Unit = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                TextBox txt_Quantity = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_UnitRate = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Amount = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdGT.Rows[i].Cells[9].FindControl("lbl_InwdType");


                drRow = dtTable.NewRow();
                drRow["txt_Particular"] = txt_Particular.Text;
                drRow["txt_TestMethod"] = txt_TestMethod.Text;
                drRow["txt_Unit"] = txt_Unit.Text;
                drRow["txt_Quantity"] = txt_Quantity.Text;
                drRow["txt_UnitRate"] = txt_UnitRate.Text;
                drRow["txt_DiscRate"] = txt_DiscRate.Text;
                drRow["txt_Amount"] = txt_Amount.Text;
                drRow["lbl_InwdType"] = lbl_InwdType.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ProposalTableGT"] = dtTable;
        }
        protected void AddRowDiscountGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["Discount_TableGT"] != null)
            {
                GetCurrentDataDiscountGT();
                dt = (DataTable)ViewState["Discount_TableGT"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTestName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblMaterialName"] = string.Empty;
            dr["lblTestName"] = string.Empty;
            dr["txtSiteWiseDisc"] = string.Empty;
            dr["txtVolDisc"] = string.Empty;
            dr["txtAppliedDisc"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Discount_TableGT"] = dt;
            grdDiscount.DataSource = dt;
            grdDiscount.DataBind();
            SetPreviousDataDiscountGT();
        }
        protected void GetCurrentDataDiscountGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTestName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            for (int i = 0; i < grdDiscount.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");
                drRow = dtTable.NewRow();

                drRow["lblMaterialName"] = lblMaterialName.Text;
                drRow["lblTestName"] = lblTestName.Text;
                drRow["txtSiteWiseDisc"] = txtSiteWiseDisc.Text;
                drRow["txtVolDisc"] = txtVolDisc.Text;
                drRow["txtAppliedDisc"] = txtAppliedDisc.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["Discount_TableGT"] = dtTable;

        }
        protected void SetPreviousDataDiscountGT()
        {
            DataTable dt = (DataTable)ViewState["Discount_TableGT"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");

                lblMaterialName.Text = dt.Rows[i]["lblMaterialName"].ToString();
                lblTestName.Text = dt.Rows[i]["lblTestName"].ToString();
                txtSiteWiseDisc.Text = dt.Rows[i]["txtSiteWiseDisc"].ToString();
                txtVolDisc.Text = dt.Rows[i]["txtVolDisc"].ToString();
                txtAppliedDisc.Text = dt.Rows[i]["txtAppliedDisc"].ToString();

            }
        }
        protected void imgDeleteGT_Click(object sender, CommandEventArgs e)
        {
            if (grdGT.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                //if (grdGT.Rows[gvr.RowIndex].Cells[1].Text == "")
                //{
                DeleteRowProposalGT(gvr.RowIndex);
                //GetCurrentDataDiscount();
                // }

                if (ddl_InwardType.SelectedValue == "GT")
                {
                    if (txtFrmRow.Text != "" && txtToRow.Text != "")
                    {
                        if (grdGT.Rows.Count >= Convert.ToInt32(txtToRow.Text))
                            ShowMerge(Convert.ToInt32(txtFrmRow.Text), Convert.ToInt32(txtToRow.Text), "GT");
                    }
                    else if (grdGT.Rows.Count >= 7 || grdGT.Rows.Count >= 8)
                    {
                        ShowMerge(2, 8, "GT");
                    }

                }
                else if (ddl_InwardType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.Visible == true)
                    {
                        if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                        {
                            if (txtFrmRow.Text != "" && txtToRow.Text != "")
                            {
                                if (grdGT.Rows.Count >= Convert.ToInt32(txtToRow.Text))
                                {
                                    ShowMerge(Convert.ToInt32(txtFrmRow.Text), Convert.ToInt32(txtToRow.Text), "OTHER");
                                }
                            }
                            else if (grdGT.Rows.Count >= 3)
                            {
                                ShowMerge(2, 3, "OTHER");
                            }
                        }
                        else if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                        {
                            if (txtFrmRow.Text != "" && txtToRow.Text != "")
                            {
                                if (grdGT.Rows.Count >= Convert.ToInt32(txtToRow.Text))
                                {
                                    ShowMerge(Convert.ToInt32(txtFrmRow.Text), Convert.ToInt32(txtToRow.Text), "OTHER");
                                }
                            }
                            else if (grdGT.Rows.Count >= 3)
                            {
                                ShowMerge(1, 3, "OTHER");
                            }
                        }
                    }
                }
            }
        }
        protected void DeleteRowProposalGT(int rowIndex)
        {
            GetCurrentDataProposalGT();
            DataTable dt = ViewState["ProposalTableGT"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ProposalTableGT"] = dt;
            grdGT.DataSource = dt;
            grdGT.DataBind();
            SetPreviousDataProposalGT();
        }
        public void ShowMerge(int FromRowNo, int ToRowNo, string recType)
        {
            if (FromRowNo.ToString() != "" && ToRowNo.ToString() != "")
            {
                double soilTestRate = 0;
                if (recType == "GT")
                {
                    var res = dc.Test_View_ForProposal(-1, 0, "GT", 0, 0, 0).ToList();
                    if (res.Count > 0)
                    {
                        soilTestRate = Convert.ToDouble(res.FirstOrDefault().TEST_Rate_int);
                    }

                    lblGtRate.Text = soilTestRate.ToString();
                    //int FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                    //int ToRowNo = Convert.ToInt32(txtToRow.Text);
                    if (ToRowNo > FromRowNo)
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                            if (txt_ProposalNo.Text == "Create New..." && (i == 0 || i == 8))
                            {
                                txt_Unit1.Text = "LS";
                            }
                            Boolean foundIt = false;
                            if (i >= FromRowNo && i <= ToRowNo)
                            {
                                for (int j = 0; j < ToRowNo; j++)
                                {
                                    if (j == i)
                                    {
                                        TextBox txt_TestMethod = (TextBox)grdGT.Rows[i - 1].Cells[3].FindControl("txt_TestMethod");
                                        TextBox txt_Unit = (TextBox)grdGT.Rows[i - 1].Cells[4].FindControl("txt_Unit");
                                        TextBox txt_Quantity = (TextBox)grdGT.Rows[i - 1].Cells[5].FindControl("txt_Quantity");
                                        TextBox txt_UnitRate = (TextBox)grdGT.Rows[i - 1].Cells[6].FindControl("txt_UnitRate");
                                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[i - 1].Cells[7].FindControl("txt_DiscRate");
                                        TextBox txt_Amount = (TextBox)grdGT.Rows[i - 1].Cells[8].FindControl("txt_Amount");
                                        txt_TestMethod.BorderStyle = BorderStyle.None;
                                        txt_Unit.BorderStyle = BorderStyle.None;
                                        txt_Quantity.BorderStyle = BorderStyle.None;
                                        txt_UnitRate.BorderStyle = BorderStyle.None;
                                        txt_DiscRate.BorderStyle = BorderStyle.None;
                                        txt_Amount.BorderStyle = BorderStyle.None;
                                        if (txt_ProposalNo.Text == "Create New...")
                                        {
                                            txt_Unit.Text = "Lump sum Up to 10 m Depth";
                                            txt_UnitRate.Text = soilTestRate.ToString();
                                            txtGtRate.Text = soilTestRate.ToString();
                                            double disc = getDiscount("GT");
                                            double discountedRate = Convert.ToDouble(soilTestRate) - (Convert.ToDouble(soilTestRate) * (disc / 100));
                                            if (discountedRate == 0)
                                                txt_DiscRate.Text = soilTestRate.ToString("0.00");
                                            else
                                                txt_DiscRate.Text = discountedRate.ToString("0.00");

                                            lbl10mDepthRate.Text = txt_DiscRate.Text;
                                        }

                                        grdGT.Rows[i].Cells[8].Visible = false;
                                        grdGT.Rows[i].Cells[9].Visible = false;
                                        grdGT.Rows[i].Cells[10].Visible = false;
                                        grdGT.Rows[i].Cells[11].Visible = false;
                                        grdGT.Rows[i].Cells[12].Visible = false;
                                        grdGT.Rows[i].Cells[13].Visible = false;
                                        grdGT.BackColor = System.Drawing.Color.White;
                                        foundIt = true;
                                        break;
                                    }
                                    else
                                    {
                                        txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                        txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                        txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                        txt_Amount1.BorderStyle = BorderStyle.NotSet;
                                        grdGT.Rows[i].Cells[8].Visible = true;
                                        grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[9].Visible = true;
                                        grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[10].Visible = true;
                                        grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[11].Visible = true;
                                        grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[12].Visible = true;
                                        grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[13].Visible = true;
                                        grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;

                                    }
                                }
                            }
                            else if (ToRowNo ==9)
                            {
                                
                                txt_UnitRate.Text = "600";
                                txtGtRate.Text = "600";

                            }
                            if (foundIt == false)
                            {
                                grdGT.Rows[i].Cells[8].Visible = true;
                                grdGT.Rows[i].Cells[9].Visible = true;
                                grdGT.Rows[i].Cells[10].Visible = true;
                                grdGT.Rows[i].Cells[11].Visible = true;
                                grdGT.Rows[i].Cells[12].Visible = true;
                                grdGT.Rows[i].Cells[13].Visible = true;
                                txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");

                            txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                            txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                            txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                            txt_Unit1.BorderStyle = BorderStyle.NotSet;
                            txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                            txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            grdGT.Rows[i].Cells[11].Visible = true;
                            grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[8].Visible = true;
                            grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[9].Visible = true;
                            grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[10].Visible = true;
                            grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[12].Visible = true;
                            grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[13].Visible = true;
                            grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;
                        }
                    }
                }
                else if (recType == "OTHER")
                {
                    int subTestId = 0;
                    if (ddl_InwardType.SelectedValue == "OT")
                    {
                        if (ddlOtherTest.SelectedValue.ToString() != "")
                        {
                            subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
                            recType = "OTHER";
                        }
                    }
                   
                    var res = dc.Test_View_ForProposal(-1, 0, recType, 0, 0, subTestId).ToList();
                    double[] arrTestRate = new double[res.Count+1];
                    if (res.Count > 0)
                    {
                        for (int i = 0; i < res.Count; i++)
                        {
                            arrTestRate[i] = Convert.ToDouble(res[i].TEST_Rate_int);
                        }
                    }
                    double disc = getDiscount("OT");
                    double discountedRate = 0;
                    if (ToRowNo > FromRowNo)
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                            if (ddlOtherTest.SelectedItem.Text == "SBC by SPT" || ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                            {
                                if (i == 0 || i == 3)
                                {
                                    if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                                        txt_Unit1.Text = "LS";
                                }
                                //txt_UnitRate1.Text = arrTestRate[i].ToString();
                                //discountedRate = Convert.ToDouble(arrTestRate[i]) - (Convert.ToDouble(arrTestRate[i]) * (disc / 100));
                                //if (discountedRate == 0)
                                //    txt_DiscRate1.Text = arrTestRate[i].ToString("0.00");
                                //else
                                //    txt_DiscRate1.Text = discountedRate.ToString("0.00");
                            }


                            Boolean foundIt = false;
                            if (i >= FromRowNo && i <= ToRowNo)
                            {
                                for (int j = 0; j < ToRowNo; j++)
                                {
                                    if (j == i)
                                    {
                                        TextBox txt_TestMethod = (TextBox)grdGT.Rows[i - 1].Cells[3].FindControl("txt_TestMethod");
                                        TextBox txt_Unit = (TextBox)grdGT.Rows[i - 1].Cells[4].FindControl("txt_Unit");
                                        TextBox txt_Quantity = (TextBox)grdGT.Rows[i - 1].Cells[5].FindControl("txt_Quantity");
                                        TextBox txt_UnitRate = (TextBox)grdGT.Rows[i - 1].Cells[6].FindControl("txt_UnitRate");
                                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[i - 1].Cells[7].FindControl("txt_DiscRate");
                                        TextBox txt_Amount = (TextBox)grdGT.Rows[i - 1].Cells[8].FindControl("txt_Amount");
                                        //if (ddlOtherTest.SelectedItem.Text != "Water Test for Drinking/Domestic Purpose")
                                        txt_TestMethod.BorderStyle = BorderStyle.None;
                                        txt_Unit.BorderStyle = BorderStyle.None;
                                        txt_Quantity.BorderStyle = BorderStyle.None;
                                        txt_UnitRate.BorderStyle = BorderStyle.None;
                                        txt_DiscRate.BorderStyle = BorderStyle.None;
                                        txt_Amount.BorderStyle = BorderStyle.None;

                                        if (txt_ProposalNo.Text == "Create New...")
                                        {
                                            if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                                            {
                                                txt_Unit.Text = "Nos";
                                            }
                                            txt_UnitRate.Text = arrTestRate[i].ToString();
                                            discountedRate = Convert.ToDouble(arrTestRate[i]) - (Convert.ToDouble(arrTestRate[i]) * (disc / 100));
                                            if (discountedRate == 0)
                                                txt_DiscRate1.Text = arrTestRate[i].ToString("0.00");
                                            else
                                                txt_DiscRate.Text = discountedRate.ToString("0.00");
                                        }


                                        grdGT.Rows[i].Cells[8].Visible = false;
                                        grdGT.Rows[i].Cells[9].Visible = false;
                                        grdGT.Rows[i].Cells[10].Visible = false;
                                        grdGT.Rows[i].Cells[11].Visible = false;
                                        grdGT.Rows[i].Cells[12].Visible = false;
                                        grdGT.Rows[i].Cells[13].Visible = false;
                                        grdGT.BackColor = System.Drawing.Color.White;
                                        foundIt = true;
                                        break;
                                    }
                                    else
                                    {
                                        txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                        txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                        txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                        txt_Amount1.BorderStyle = BorderStyle.NotSet;
                                        grdGT.Rows[i].Cells[11].Visible = true;
                                        grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[8].Visible = true;
                                        grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[9].Visible = true;
                                        grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[10].Visible = true;
                                        grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[12].Visible = true;
                                        grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[13].Visible = true;
                                        grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;
                                    }
                                }
                            }
                            if (foundIt == false)
                            {
                                grdGT.Rows[i].Cells[8].Visible = true;
                                grdGT.Rows[i].Cells[9].Visible = true;
                                grdGT.Rows[i].Cells[10].Visible = true;
                                grdGT.Rows[i].Cells[11].Visible = true;
                                grdGT.Rows[i].Cells[12].Visible = true;
                                grdGT.Rows[i].Cells[13].Visible = true;
                                txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");

                            txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                            txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                            txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                            txt_Unit1.BorderStyle = BorderStyle.NotSet;
                            txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                            txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            grdGT.Rows[i].Cells[11].Visible = true;
                            grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[8].Visible = true;
                            grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[9].Visible = true;
                            grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[10].Visible = true;
                            grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[12].Visible = true;
                            grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[13].Visible = true;
                            grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }
        protected void btnApplyLumpsum_OnClick(object sender, EventArgs e)
        {
            ChkLumpSum();
            //ShowMerge();
            if (txtFrmRow.Text != "" && txtToRow.Text != "")
            {
                int FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                int ToRowNo = Convert.ToInt32(txtToRow.Text);
                if (FromRowNo < ToRowNo)
                {
                    if (ddl_InwardType.SelectedItem.Text == "Soil Investigation")
                        ShowMerge(FromRowNo, ToRowNo, "GT");
                    else if (ddlOtherTest.Visible == true)
                    {
                        if (ddlOtherTest.SelectedItem.Text.Equals("SBC by SPT"))
                            ShowMerge(FromRowNo, ToRowNo, "OTHER");
                        else if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                            ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    }
                }
            }
            txt_NetAmount.Focus();
        }
        protected void chk_Lumshup_OnCheckedChanged(object sender, EventArgs e)
        {
            ChkLumpSum();
            //ShowMerge();
            if (txtFrmRow.Text != "" && txtToRow.Text != "")
            {
                int FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                int ToRowNo = Convert.ToInt32(txtToRow.Text);

                //if (lblRType.Text == "Soil Investigation")
                if (ddl_InwardType.SelectedItem.Text == "Soil Investigation")
                    ShowMerge(FromRowNo, ToRowNo, "GT");
                else if (ddlOtherTest.Visible == true)
                {
                    if (ddlOtherTest.SelectedItem.Text.Equals("SBC by SPT"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    else if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                }
            }
            else
            {
                int FromRowNo = 0;
                int ToRowNo = 0;
                if (ddl_InwardType.SelectedItem.Text == "Soil Investigation")
                    ShowMerge(FromRowNo, ToRowNo, "GT");
                else if (ddlOtherTest.Visible == true
                    )
                {
                    if (ddlOtherTest.SelectedItem.Text.Equals("SBC by SPT"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    else if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                }
            }


        }

        ////Additional Charges GT
        protected void AddRowNoteGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["NoteTableGT"] != null)
            {
                GetCurrentDataNoteGT();
                dt = (DataTable)ViewState["NoteTableGT"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNoGT", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NOTEGT", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNoGT"] = dt.Rows.Count + 1;
            dr["txt_NOTEGT"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["NoteTableGT"] = dt;
            Grd_NoteGT.DataSource = dt;
            Grd_NoteGT.DataBind();
            SetPreviousDataNoteGT();
        }
        protected void SetPreviousDataNoteGT()
        {
            DataTable dt = (DataTable)ViewState["NoteTableGT"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_NoteGT.Rows[i].Cells[2].FindControl("txt_NOTEGT");
                Grd_NoteGT.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_NOTE.Text = dt.Rows[i]["txt_NOTEGT"].ToString();

            }
        }
        protected void GetCurrentDataNoteGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNoGT", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NOTEGT", typeof(string)));
            for (int i = 0; i < Grd_NoteGT.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_NoteGT.Rows[i].Cells[2].FindControl("txt_NOTEGT");

                drRow = dtTable.NewRow();
                drRow["lblSrNoGT"] = i + 1;
                drRow["txt_NOTEGT"] = txt_NOTE.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["NoteTableGT"] = dtTable;
        }
        protected void DeleteRowNoteGT(int rowIndex)
        {
            GetCurrentDataNoteGT();
            DataTable dt = ViewState["NoteTableGT"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["NoteTableGT"] = dt;
            Grd_NoteGT.DataSource = dt;
            Grd_NoteGT.DataBind();
            SetPreviousDataNoteGT();
        }
        protected void imgBtnAddNotesRowGT_Click(object sender, CommandEventArgs e)
        {
            AddRowNoteGT();
        }
        protected void imgBtnDeleteNotesRowGT_Click(object sender, CommandEventArgs e)
        {
            if (Grd_NoteGT.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (Grd_NoteGT.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowNoteGT(gvr.RowIndex);
            }
        }
        protected void DeleteRowProposal(int rowIndex)
        {
            GetCurrentDataProposal();
            DataTable dt = ViewState["ProposalTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ProposalTable"] = dt;
            grdProposal.DataSource = dt;
            grdProposal.DataBind();
            SetPreviousDataProposal();
        }
        //payment terms gt
        protected void AddRowPayTermGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PayTermGTTable"] != null)
            {
                GetCurrentDataPayTermGT();
                dt = (DataTable)ViewState["PayTermGTTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNoPayTermGT", typeof(string)));
                dt.Columns.Add(new DataColumn("txtNotePayTermGT", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNoPayTermGT"] = dt.Rows.Count + 1;
            dr["txtNotePayTermGT"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["PayTermGTTable"] = dt;
            grdPayTermsGT.DataSource = dt;
            grdPayTermsGT.DataBind();
            SetPreviousDataPayTermGT();
        }
        protected void SetPreviousDataPayTermGT()
        {
            DataTable dt = (DataTable)ViewState["PayTermGTTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].Cells[2].FindControl("txtNotePayTermGT");
                grdPayTermsGT.Rows[i].Cells[2].Text = (i + 1).ToString();
                txtNotePayTermGT.Text = dt.Rows[i]["txtNotePayTermGT"].ToString();

            }
        }
        protected void GetCurrentDataPayTermGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNoPayTermGT", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtNotePayTermGT", typeof(string)));
            for (int i = 0; i < grdPayTermsGT.Rows.Count; i++)
            {
                TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].Cells[2].FindControl("txtNotePayTermGT");

                drRow = dtTable.NewRow();
                drRow["lblSrNoPayTermGT"] = i + 1;
                drRow["txtNotePayTermGT"] = txtNotePayTermGT.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["PayTermGTTable"] = dtTable;
        }
        protected void DeleteRowPayTermGT(int rowIndex)
        {
            GetCurrentDataPayTermGT();
            DataTable dt = ViewState["PayTermGTTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PayTermGTTable"] = dt;
            grdPayTermsGT.DataSource = dt;
            grdPayTermsGT.DataBind();
            SetPreviousDataPayTermGT();
        }
        protected void imgBtnAddRowPayTermGT_Click(object sender, CommandEventArgs e)
        {
            AddRowPayTermGT();
        }
        protected void imgBtnDeleteRowPayTermGT_Click(object sender, CommandEventArgs e)
        {
            if (grdPayTermsGT.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdPayTermsGT.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowPayTermGT(gvr.RowIndex);
            }
        }
        //
        
        protected void AddRowDiscount()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["Discount_Table"] != null)
            {
                GetCurrentDataDiscount();
                dt = (DataTable)ViewState["Discount_Table"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTestName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblMaterialName"] = string.Empty;
            dr["lblTestName"] = string.Empty;
            dr["txtSiteWiseDisc"] = string.Empty;
            dr["txtVolDisc"] = string.Empty;
            dr["txtAppliedDisc"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Discount_Table"] = dt;
            grdDiscount.DataSource = dt;
            grdDiscount.DataBind();
            SetPreviousDataDiscount();
        }
        protected void GetCurrentDataDiscount()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTestName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            for (int i = 0; i < grdDiscount.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");
                drRow = dtTable.NewRow();

                drRow["lblMaterialName"] = lblMaterialName.Text;
                drRow["lblTestName"] = lblTestName.Text;
                drRow["txtSiteWiseDisc"] = txtSiteWiseDisc.Text;
                drRow["txtVolDisc"] = txtVolDisc.Text;
                drRow["txtAppliedDisc"] = txtAppliedDisc.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["Discount_Table"] = dtTable;

        }
        protected void SetPreviousDataDiscount()
        {
            DataTable dt = (DataTable)ViewState["Discount_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");

                lblMaterialName.Text = dt.Rows[i]["lblMaterialName"].ToString();
                lblTestName.Text = dt.Rows[i]["lblTestName"].ToString();
                txtSiteWiseDisc.Text = dt.Rows[i]["txtSiteWiseDisc"].ToString();
                txtVolDisc.Text = dt.Rows[i]["txtVolDisc"].ToString();
                txtAppliedDisc.Text = dt.Rows[i]["txtAppliedDisc"].ToString();

            }


        }
        private void DeleteRowDiscount(int rowIndex)
        {
            GetCurrentDataDiscount();
            DataTable dt = ViewState["Discount_Table"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count <= rowIndex)
                {
                    dt.Rows[rowIndex].Delete();
                    ViewState["Discount_Table"] = dt;
                    grdDiscount.DataSource = dt;
                    grdDiscount.DataBind();
                    SetPreviousDataDiscount();
                }
            }
        }

        protected void AddRowNote()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["NoteTable"] != null)
            {
                GetCurrentDataNote();
                dt = (DataTable)ViewState["NoteTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NOTE", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_NOTE"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["NoteTable"] = dt;
            Grd_Note.DataSource = dt;
            Grd_Note.DataBind();
            SetPreviousDataNote();
        }
        protected void SetPreviousDataNote()
        {
            DataTable dt = (DataTable)ViewState["NoteTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].Cells[2].FindControl("txt_NOTE");
                Grd_Note.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_NOTE.Text = dt.Rows[i]["txt_NOTE"].ToString();

            }
        }
        protected void GetCurrentDataNote()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NOTE", typeof(string)));
            for (int i = 0; i < Grd_Note.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].Cells[2].FindControl("txt_NOTE");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_NOTE"] = txt_NOTE.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["NoteTable"] = dtTable;
        }
        protected void DeleteRowNote(int rowIndex)
        {
            GetCurrentDataNote();
            DataTable dt = ViewState["NoteTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["NoteTable"] = dt;
            Grd_Note.DataSource = dt;
            Grd_Note.DataBind();
            SetPreviousDataNote();
        }
        protected void imgBtnAddRowNote_Click(object sender, CommandEventArgs e)
        {
            AddRowNote();
        }
        protected void imgBtnDeleteRowNote_Click(object sender, CommandEventArgs e)
        {
            if (Grd_Note.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (Grd_Note.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowNote(gvr.RowIndex);
            }
        }

        private void AddTermsConditionNotes(string inwdType)
        {

            if (inwdType == "Soil Investigation" || inwdType == "SBC by SPT" || inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")// tems for GT only
            {
                if (Grd_Note.Rows.Count == 0)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        AddRowNote();
                        TextBox txt_NOTE = (TextBox)Grd_Note.Rows[l].Cells[2].FindControl("txt_NOTE");

                        if (l == 0)
                        {
                            txt_NOTE.Text = "You shall issue us a firm work order written or email from company mail ID.";//work order along with 50% advance.";
                        }
                        else if (l == 1)
                        {
                            if (inwdType == "Plate Load Testing")
                                txt_NOTE.Text = "All Local problems like right of way, obstructions of local peoples,etc shall be settled by you before our setup reach site.We shall deploy our machines only after getting confirmation from your end.";
                            else
                                txt_NOTE.Text = "All Local problems like right of way, obstructions of local peoples,etc shall be settled by you before our machines reach site.We shall deploy our machines only after getting confirmation from your end.";
                        }
                        else if (l == 2)
                        {
                            if (inwdType == "Plate Load Testing" || inwdType == "SBC by SPT")
                                txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the locations and reasonable access to the locations,shall be in client scope.";
                            else if (inwdType == "Earth Resistivity Test")
                                txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the locations and the way to the locations,shall be in client scope.";
                            else
                                txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the bore hole locations and the way to the borehole locations,shall be in client scope.";
                        }
                        else if (l == 3)
                        {
                            txt_NOTE.Text = "We shall carry our camping equipment with us to site.You shall provide us with open space at site for our labor camp free of cost.";
                        }
                        else if (l == 4)
                        {
                            if (inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")
                                txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the drilling activity. We shall not be responsible for any damage caused to them.";
                            else if (inwdType == "SBC by SPT")
                                txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the testing. We shall not be responsible for any damage caused to them during drilling operation.";
                            else
                                txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the drilling activity. We shall not be responsible for any damage caused to them during drilling operation.";

                        }
                        else if (l == 5)
                        {
                            if (inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")
                                txt_NOTE.Text = "Locations of all the points shall be marked by your representative at site. The testing shall be carried out by us only at the locations marked by you.";
                            else if (inwdType != "SBC by SPT")
                                txt_NOTE.Text = "Locations of all the boreholes shall be marked by your representative at site. The drilling shall be carried out by us only at the locations marked by you.";

                        }
                        else if (l == 6)
                        {
                            if (inwdType == "Plate Load Testing")
                                txt_NOTE.Text = "In case if required JCB or Poclaine machine shall be provided by you for shifting / setting of PLT setup at location on site.";
                            else if (inwdType != "Earth Resistivity Test" && inwdType != "Rain Water Harvesting" && inwdType != "SBC by SPT")
                                txt_NOTE.Text = "The water required for the drilling activity shall be provided by you at the borehole drilling location free of cost.In case if required JCB or Poclaine machine shall be provided by you for shifting or setting drilling machine at borehole location on site.";

                        }
                        else if (l == 7)
                        {
                            txt_NOTE.Text = "All work executed will be witnessed by your representative jointly so as to avoid any discrepancy between the work executed and your requirement.";
                        }
                        else if (l == 8)
                        {
                            txt_NOTE.Text = "Any other requirement of detail calculations/additional recommendations which are not part of above proposal will be charged extra.";
                        }
                        else if (l == 9)
                        {
                            txt_NOTE.Text = "Draft report will be submitted in soft format for your perusal along with final bill after completion of field work and laboratory tests.";
                        }
                        else if (l == 10)
                        {
                            txt_NOTE.Text = "Final report(Hard copy-One color) will be submitted after receipt of entire payment.";
                        }
                        else if (l == 11)
                        {
                            if (inwdType == "Soil Investigation" || inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing")
                                txt_NOTE.Text = "The validity of proposal is for 30 days from the date of proposal submission.";
                            else
                                DeleteRowNote(Grd_Note.Rows.Count -1);
                        }
                    }

                    if (inwdType == "SBC by SPT")
                    {
                        DeleteRowNote(5);
                        DeleteRowNote(5);
                    }
                    if (inwdType == "Earth Resistivity Test" || inwdType == "Rain Water Harvesting")
                    {
                        DeleteRowNote(6);
                    }                

                }
                if (inwdType == "Soil Investigation" || inwdType == "Rain Water Harvesting" || inwdType == "SBC by SPT")
                {
                    Grd_NoteGT.Visible = true;
                    lblAddChargesGT.Visible = true;                    
                    if (Grd_NoteGT.Rows.Count == 0)
                    {
                        /////Additional Charges GT
                        if (inwdType == "Soil Investigation")
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                AddRowNoteGT();
                                TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[l].Cells[2].FindControl("txt_NOTEGT");

                                if (l == 0)
                                    txt_NOTEGT.Text = "In case total depth of boreholes exceeds 10m, additional charges for drilling will be charged on pro rata basis ie charges per borehole divided by 10, per running meter.";
                                else if (l == 1)
                                    txt_NOTEGT.Text = "If water is not provided by you the same shall arranged by Durocrete at cost of Rs. 1500/boreholes.";
                                else if (l == 2)
                                    txt_NOTEGT.Text = "If site topography has hillocks or if distance between boreholes is more than 30m movement of rigs shall require tracter/JCB and the same shall be charged to client at cost of Rs. 2200/boreholes. ";
                            }
                        }
                        else
                        {
                            AddRowNoteGT();
                            TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[0].Cells[2].FindControl("txt_NOTEGT");

                            if (inwdType == "Rain Water Harvesting")
                                txt_NOTEGT.Text = "For certain test water is required during testing procedure, If water is not provided by you the same shall arranged by Durocrete at cost of Rs. 1500/boreholes.";
                            else
                                txt_NOTEGT.Text = "If site topography has hillocks or if distance between locations is more than 30m movement of rigs shall require tracter/JCB and the same shall be charged to client at cost of Rs. 2200/boreholes. ";

                        }
                    }
                }

                lblPayTermsGT.Visible = true;
                grdPayTermsGT.Visible = true;
                if (grdPayTermsGT.Rows.Count == 0)
                {
                    /////payment terms GT
                    string[] arrPayTerm = {
                    "Mobilization charges and 50% advance to be paid before mobilization of equipment at site.",
                    "25% of bill value to be paid after submission of provisional report.",
                    "Balanced 25% of bill value to be paid within two weeks of submission of final report.",
                    "Visit of our competent Technical Officer after conducting the geotechnical investigation or during site exploration for inspection or verification will be charged extra as below:-",
                    "   a)    PMC & PCMC - Rs. 5000 (Travelling & GST Extra).",
                    "   b)    Out of Pune – Rs. 7500 (Travelling & GST Extra)."};

                    for (int i = 0; i < arrPayTerm.Length; i++)
                    {
                        AddRowPayTermGT();
                        TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].FindControl("txtNotePayTermGT");
                        txtNotePayTermGT.Text = arrPayTerm[i];
                    }

                }
                
            }
            else
            {
                if (Grd_Note.Rows.Count == 0)
                {
                    string[] arrTerm = {
                        "We have well organised door to door collection service for PMC and PCMC Area . We request you to arrange material on time to avoid delay as the vehicle has futher sites to visit.",
                        "Travelling charges will be applicable extra.",
                        "Minimum billing Rs. 20000/- will be applicable if the scope of work is reduced or not carried out due to unavailability of client's support.",
                        "The validity of proposal is for 30 days from the date of proposal submission.",
                        "Required 3 pieces of 1400mm for steel bar dia up to 25mm and 1600mm for 32mm diameter."};

                    for (int i = 0; i < arrTerm.Length; i++)
                    {
                        if (i <= 3 || (i == 4 && inwdType == "Steel Testing"))
                        {
                            AddRowNote();
                            TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].FindControl("txt_NOTE");
                            txt_NOTE.Text = arrTerm[i];
                        }
                    }
                }
                
                grdClientScope.Visible = true;
                lblClientScope.Visible = true;
                if (grdClientScope.Rows.Count == 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        string strNote = "";
                        if (i == 0 && (inwdType == "Core Cutting" || inwdType == "Core Testing"))
                        {
                            strNote = "Client has to provide two labours for help, water, electricity & scaffolding etc.";
                        }
                        else if (i == 1 && inwdType == "Non Destructive Testing")
                        {
                            strNote = "Chiselling, cleaning and rework of plaster to expose the concrete if required.";
                        }
                        else if (i == 2 && inwdType == "Non Destructive Testing")
                        {
                            strNote = "Client has to provide two labours for help & scaffolding etc.";
                        }
                        else if (i == 3)
                        {
                            strNote = "Providing necessary permissions, safe access and safe working conditions for our team at site.";
                        }
                        else if (i == 4 && inwdType == "Pile Testing")
                        {
                            strNote = "Chiselling of piles till sound concrete.";
                        }
                        else if (i == 5 && inwdType == "Pile Dynamic Test")
                        {
                            strNote = "Hydra, nylon rope and hammer as per load required.";
                        }
                        else if (i == 6 && inwdType == "Plate Load Testing")
                        {
                            strNote = "Poclain/Sand bags as reaction load and labours for loading and unloading.";
                        }
                        else if (i == 7 && (inwdType == "Non Destructive Testing" || inwdType == "Core Cutting" || inwdType == "Core Testing"
                            || inwdType == "Soil Testing" || inwdType == "Plate Load Testing" || inwdType == "Slab Load Test"
                            || inwdType.Contains("Pull Out Test") == true))
                        {
                            strNote = "For outstation projects lodging and boarding.";
                        }
                        else if (i == 8 && (inwdType == "Structural Audit" || inwdType == "Retrofiting test"))
                        {
                            strNote = "Architectural & structural drawings in CAD format.";
                        }
                        else if (i == 9 && inwdType == "Slab Deflection test")
                        {
                            strNote = "Aggregate/Sand bags and labours, crain for loading and unloading etc.";
                        }
                        else if (i == 10 && inwdType == "Mix Design")
                        {
                            strNote = "Labours, mix design report, calibration certificate of mixture/batching plant, cube mould, slump cone, weighing balance and tamping rod etc.";
                        }
                        else if (i == 11 && (inwdType == "Pile Testing" || inwdType == "Pile Dynamic Test"))
                        {
                            strNote = "Hydra, nylon rope and hammer as per load required.";
                        }
                        if (strNote != "")
                        {
                            AddRowClientScope();
                            TextBox txt_NOTE = (TextBox)grdClientScope.Rows[grdClientScope.Rows.Count - 1].FindControl("txt_NOTEClientScope");
                            txt_NOTE.Text = strNote;
                        }
                    }                   

                }
                txtPaymentTerm.Text = "Payment- Provide 100% advance along with work order/email confirmation.";
            }
            
        }
        protected void OnTextChanged(object sender, EventArgs e)
        {
            float total = 0;
            foreach (GridViewRow gridViewRow in grdGT.Rows)
            {
                TextBox txt_Amount = (TextBox)gridViewRow.FindControl("txt_Amount");
                float rowValue = 0;
                if (float.TryParse(txt_Amount.Text.Trim(), out rowValue))
                    total += rowValue;
            }
            txt_NetAmount.Text = Math.Round(total).ToString("0.00");

        }
        public void ShowHeader()
        {
            if (grdProposal.Rows.Count <= 0)
            {
                AddRowProposal();
                grdProposal.Rows[0].Visible = false;
                ViewState["ProposalTable"] = null;
            }
        }

        protected void lnkUpdateIntro_Click(object sender, EventArgs e)
        {
            //int introDisc = 0;
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            //lblMsg.Visible = false; clsData obj = new clsData();
            ////update introductory discount
            //if (lblClientId.Text != "" && lblClientId.Text != "0")
            //{
            //    if (txtIntro.Text != "")
            //    {
            //        introDisc = Convert.ToInt32(txtIntro.Text);
            //        if (introDisc <= 10)
            //        {
            //            //string discountType = "Introductory";
            //            //update intro discount
            //            dc.DiscountSetting_Update_Introductory(Convert.ToInt32(lblClId.Text), introDisc);
            //            //apply changed discount to tests
            //            //grdProposal.DataSource = null;
            //            //grdProposal.DataBind();
            //            if (grdProposal.Visible == true)
            //            {
            //                for (int i = 0; i < grdProposal.Rows.Count; i++)
            //                {
            //                    Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[9].FindControl("lbl_InwdType");

            //                    //inwardType = obj.getInwardTypeValue(lbl_InwdType.Text);
            //                    if (lbl_InwdType.Text != "")
            //                    {
            //                        //  addTestToGrid(inwardType);
            //                        UpdateTestToGrid(lbl_InwdType.Text, i);
            //                    }
            //                }
            //            }
            //            else if (grdGT.Visible == true)
            //            {
            //                UpdateTestToGrid("GT", 1);
            //            }
            //            DisplayGenericDiscountDetails();
            //            Calculation();
            //            //discountType = "Applicable";
            //            dc.DiscountSetting_Update_Introductory(Convert.ToInt32(lblClId.Text), Convert.ToDecimal(appliedDisc));
            //        }
            //        else
            //        {
            //            lblMsg.Visible = true;
            //            lblMsg.Text = "Introductory Discount should be less than equal to 10.";
            //        }

            //    }
            //    else
            //    {
            //        lblMsg.Visible = true;
            //        lblMsg.Text = "Input Introductoty Discount";
            //    }
            //}
        }

        private void UpdateTestToGrid(string inwardType, int rowPos)
        {
            int testId = 0; double siteRate = 0, disc = 0, calDisc = 0, discountedRate = 0;
            var res = dc.Test_View_ForProposal(-1, 0, inwardType, 0, 0, 0);
            if (grdProposal.Visible == true)
            {
                TextBox txt_Particular = (TextBox)grdProposal.Rows[rowPos].Cells[3].FindControl("txt_Particular");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[rowPos].Cells[4].FindControl("txt_Rate");
                TextBox txt_Discount = (TextBox)grdProposal.Rows[rowPos].Cells[5].FindControl("txt_Discount");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[rowPos].Cells[6].FindControl("txt_DiscRate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[rowPos].Cells[7].FindControl("txt_Quantity");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[rowPos].Cells[8].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdProposal.Rows[rowPos].Cells[9].FindControl("lbl_InwdType");
                Label lbl_SiteWiseRate = (Label)grdProposal.Rows[rowPos].Cells[10].FindControl("lbl_SiteWiseRate");
                Label lbl_TestId = (Label)grdProposal.Rows[rowPos].Cells[11].FindControl("lbl_TestId");

                if (lbl_TestId.Text != "")
                    testId = Convert.ToInt32(lbl_TestId.Text);

                siteRate = chkSiteWiseRate(testId);

                if (siteRate != 0)//apply sitewise rate setting
                {
                    txt_DiscRate.Text = siteRate.ToString("0.00");
                    //calculate rev. discount
                    calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(siteRate))) * 100) / Convert.ToDouble(txt_Rate.Text));
                    //txt_Discount.Text = calDisc.ToString();
                    txt_Discount.Text = "-";
                    //string inType = ddl_InwardType.Items.FindByValue(re.Test_RecType_var).Text;
                    // discListDetails = discListDetails + txt_Particular.Text + "~" + calDisc + "~" + "-" + "~" + calDisc + "|";
                    lbl_SiteWiseRate.Text = siteRate.ToString();
                }
                else //apply discount setting
                {
                    disc = getDiscount(lbl_InwdType.Text);
                    txt_Discount.Text = disc.ToString();
                    discountedRate = Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(txt_Rate.Text) * (disc / 100));
                    txt_DiscRate.Text = discountedRate.ToString("0.00");
                    //string inType = ddl_InwardType.Items.FindByValue(re.Test_RecType_var).Text;
                    //  discListDetails = discListDetails + txt_Particular.Text + "~" + "-" + "~" + disc + "~" + disc + "|";
                    lbl_SiteWiseRate.Text = "0";
                }
            }
            else if (grdGT.Visible == true)
            {
                TextBox txt_UnitRate = (TextBox)grdGT.Rows[rowPos].Cells[5].FindControl("txt_UnitRate");
                TextBox txt_DiscRate = (TextBox)grdGT.Rows[rowPos].Cells[6].FindControl("txt_DiscRate");

                disc = getDiscount("GT");
                discountedRate = Convert.ToDouble(txt_UnitRate.Text) - (Convert.ToDouble(txt_UnitRate.Text) * (disc / 100));
                txt_DiscRate.Text = discountedRate.ToString("0.00");

            }
        }
        private void DisplayGenericDiscountDetails()
        {
            if (grdProposal.Rows.Count > 0 || grdGT.Rows.Count > 0)
            {
                txtIntro.Text = introDiscA.ToString();
                lblVol.Text = "Volumn (%) = " + volDiscB.ToString();
                lblTime.Text = "Timely Payment (%) = " + timelyDiscC.ToString();
                lblAdv.Text = "Advance (%) = " + AdvDiscD.ToString();
                lblLoy.Text = "Loyalty (%) = " + loyDiscE.ToString();
                lblProp.Text = "Proposal (%) = " + propDiscF.ToString();
                lblApp.Text = "App (%) = " + AppDiscG.ToString();
                lblMax.Text = "Max (%) = " + maxDiscnt.ToString();
                lblDisc.Text = "Applied (%) = " + appliedDisc.ToString();
                lblCalcDisc.Text = "Calculated (%) = " + calculatedDisc.ToString();

            }
        }
        public void Calculation()
        {
            //gross total
            decimal GrossAmount = 0, GstCalculatedAmt = 0, GstAmount = 0;
            if (grdProposal.Visible == true)
            {
                foreach (GridViewRow r in grdProposal.Rows)
                {
                    TextBox txt_Amount = r.FindControl("txt_Amount") as TextBox;
                    TextBox txt_Rate = r.FindControl("txt_Rate") as TextBox;
                    TextBox txt_Quantity = r.FindControl("txt_Quantity") as TextBox;
                    TextBox txt_DiscRate = r.FindControl("txt_DiscRate") as TextBox;

                    if (txt_DiscRate.Text != "" && txt_Quantity.Text != "")
                    {
                        txt_Amount.Text = Convert.ToDecimal(Convert.ToDecimal(txt_Quantity.Text) * Convert.ToDecimal(txt_DiscRate.Text)).ToString("0.00");
                    }
                    decimal number = 0; ;
                    if (decimal.TryParse(txt_Amount.Text, out number))
                    {
                        GrossAmount += number;
                    }
                }
                txt_NetAmount.Text = Math.Round(GrossAmount).ToString("0.00");
            }
            else if (grdGT.Visible == true)
            {
                int count = 0;
                foreach (GridViewRow r in grdGT.Rows)
                {
                    TextBox txt_Amount = r.FindControl("txt_Amount") as TextBox;
                    TextBox txt_UnitRate = r.FindControl("txt_UnitRate") as TextBox;
                    TextBox txt_DiscRate = r.FindControl("txt_DiscRate") as TextBox;
                    TextBox txt_Quantity = r.FindControl("txt_Quantity") as TextBox;

                    if (count != 1)
                    {
                        if (txt_DiscRate.Text != "")
                            txt_UnitRate.Text = txt_DiscRate.Text;
                    }
                    decimal qty;
                    if (decimal.TryParse(txt_Quantity.Text, out qty))
                    {
                        // it's a valid integer => you could use the distance variable here
                        if (txt_DiscRate.Text != "" && txt_Quantity.Text != "")
                        {
                            txt_Amount.Text = Convert.ToDecimal(Convert.ToDecimal(txt_Quantity.Text) * Convert.ToDecimal(txt_DiscRate.Text)).ToString("0.00");
                        }
                    }
                    else
                    {
                        //if (txt_DiscRate.Text != "")
                        txt_Amount.Text = txt_DiscRate.Text;
                        //else if (txt_UnitRate.Text != "")
                        //    txt_Amount.Text = txt_UnitRate.Text;
                    }

                    decimal number = 0;
                    if (txt_Amount.Visible == true)
                    {
                        if (decimal.TryParse(txt_Amount.Text, out number))
                        {
                            GrossAmount += number;
                        }
                    }

                    count++;
                }
                txt_NetAmount.Text = Math.Round(GrossAmount).ToString("0.00");
            }
            txt_NetAmount.Focus();
            GstAmount = GrossAmount * Convert.ToDecimal(0.18);
            GstCalculatedAmt = GrossAmount + GstAmount;
            lblGstAmt.Text = GstAmount.ToString("0.00");
            lblGrandTotal.Text = Math.Round(GstCalculatedAmt).ToString("0.00");
        }
        protected void ddl_PrposalBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_ContactNo.Text = "";
            if (ddl_PrposalBy.SelectedIndex > 0)
            {
                var proposal = dc.Proposal_View_ContactNo(Convert.ToInt32(ddl_PrposalBy.SelectedValue));
                foreach (var prop in proposal)
                {
                    txt_ContactNo.Text = prop.Proposal_ContactNo;
                    break;
                }
            }
        }
        protected void btn_PrintProposal_Click(object sender, EventArgs e)
        {
            if (lblEnqId.Text != "")
            {
                int qtyFlag = 0, proposalId = 0; //status = 0,

                if (chkQty.Checked)
                    qtyFlag = 1;

                var res = dc.Proposal_View(Convert.ToInt32(lblEnqId.Text), Convert.ToBoolean(chkNewClient.Checked), 0, txt_ProposalNo.Text);
                foreach (var reslt in res)
                {
                    proposalId = reslt.Proposal_Id;
                    break;
                }

                PrintPDFReport rpt = new PrintPDFReport();
                rpt.Proposal_PDF(Convert.ToInt32(lblEnqId.Text), qtyFlag, proposalId, Convert.ToBoolean(chkNewClient.Checked), "View", txt_ProposalNo.Text);
            }
        }
        protected void btn_Cal_Click(object sender, EventArgs e)
        {
            //if (ddl_MatCollectn.SelectedItem.Text != "Declined by us" &&  ddl_MatCollectn.SelectedItem.Text != "Declined by Client")
            //{
            //if (ValidateData() == true && ValidatDataMatColl() == true && (ValidateDataProposal() == true && lblMonthlyStatus.Text=="0"))

            lblProposalError.Visible = false;
            if (lblEnqNo.Visible == true || chkConifrmEnq.Checked == false)
                return;

            if (ValidateData() == true && ValidatDataMatColl() == true && (ValidateDataProposal() == true && (grdProposal.Visible == true || grdGT.Visible == true)))
            {
                if (ValidateDataProposal() == true)
                    Calculation();
            }
            // }
        }
        protected Boolean ValidateDataProposal()
        {
            string msg = "";
            Boolean valid = true;
            DateTime? ProposalDate = null;
            if (txt_ProposalDt.Text != "")
                ProposalDate = DateTime.ParseExact(txt_ProposalDt.Text, "dd/MM/yyyy", null);
            DateTime CurrentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);

            if (ProposalDate > CurrentDate)
            {
                msg = "Proposal Date should be always less than or equal to the Current Date.";
                txt_ProposalDt.Focus();
                valid = false;
            }
            else if (txt_KindAttention.Text == "")
            {
                msg = "Enter Kind Attention";
                txt_KindAttention.Focus();
                valid = false;
            }
            else if (txt_Subject.Text == "")
            {
                msg = "Enter Subject";
                txt_Subject.Focus();
                valid = false;
            }
            else if (txt_Description.Text == "")
            {
                msg = "Enter Description";
                txt_Description.Focus();
                valid = false;
            }
            else if (txtEmail.Text == "")
            {
                msg = "Enter Proposal Email Id";
                txtEmail.Focus();
                valid = false;
            }
            else if (grdProposal.Rows.Count == 0 && grdProposal.Visible == true)//&& chk_Proposal.Checked)
            {
                msg = "Please Add Inward Test";
                grdProposal.Focus();
                valid = false;
            }
            else if (grdGT.Rows.Count == 0 && grdGT.Visible == true)//&& chk_Proposal.Checked)
            {
                msg = "Please Add Inward Test";
                valid = false;
            }
            //else if (txtPaymentTerm.Text == "")
            //{
            //    msg = "Enter Payment term";
            //    txtPaymentTerm.Focus();
            //    valid = false;
            //}
            else if (ddl_PrposalBy.SelectedItem.Text == "---Select---")
            {
                msg = "Select Proposal By";
                ddl_PrposalBy.Focus();
                valid = false;
            }
            else if (txt_ContactNo.Text == "")
            {
                msg = "Select the Contact No.";
                txt_ContactNo.Focus();
                valid = false;
            }
            else if (txtMe.Text == "")
            {
                msg = "Enter ME Name";
                txtMe.Focus();
                valid = false;
            }
            else if (txtMeNum.Text == "")
            {
                msg = "Enter Contact No.";
                txtMeNum.Focus();
                valid = false;
            }
            else if (ddl_InwardType.SelectedValue == "OT" && ddlOtherTest.SelectedIndex > 0 && ddlOtherTest.SelectedItem.Text == "Structural Audit")
            {
                if (txtStructNameOfApartSoc.Text == "")
                {
                    msg = "Enter Name of Apartment / Society";
                    txtStructNameOfApartSoc.Focus();
                    valid = false;
                }
                else if (txtStructAddress.Text == "")
                {
                    msg = "Enter Address";
                    txtStructAddress.Focus();
                    valid = false;
                }
                else if (txtStructBuiltupArea.Text == "")
                {
                    msg = "Enter Builtup Area of Society";
                    txtStructBuiltupArea.Focus();
                    valid = false;
                }
                else if (txtStructNoOfBuild.Text == "")
                {
                    msg = "Enter No of buildings in Society";
                    txtStructNoOfBuild.Focus();
                    valid = false;
                }
                else if (txtStructAge.Text == "")
                {
                    msg = "Enter Age of Building";
                    txtStructAge.Focus();
                    valid = false;
                }
                else if (ddlStructConstWithin5Y.SelectedIndex == 0)
                {
                    msg = "Select All buildings constructed with in 5 years range";
                    ddlStructConstWithin5Y.Focus();
                    valid = false;
                }
                else if (ddlStructLocation.SelectedIndex == 0)
                {
                    msg = "Select Location";
                    ddlStructLocation.Focus();
                    valid = false;
                }
                else if (ddlStructAddLoadExpc.SelectedIndex == 0)
                {
                    msg = "Select Any additional loads expected on building";
                    ddlStructAddLoadExpc.Focus();
                    valid = false;
                }
                else if (ddlStructDistressObs.SelectedIndex == 0)
                {
                    msg = "Select Any Distress Observed";
                    ddlStructDistressObs.Focus();
                    valid = false;
                }
            }
            if (valid == true && grdProposal.Visible == true)
            {
                int flag = 0;//to chk discount is blank or not in case of editable distount rate validation 
                for (int j = 0; j < grdProposal.Rows.Count; j++)
                {
                    TextBox txt_Quantity = (TextBox)grdProposal.Rows[j].Cells[5].FindControl("txt_Quantity");
                    TextBox txt_Rate = (TextBox)grdProposal.Rows[j].Cells[4].FindControl("txt_Rate");
                    TextBox txt_DiscRate = (TextBox)grdProposal.Rows[j].Cells[6].FindControl("txt_DiscRate");

                    if (ddlOtherTest.Visible == true && ddl_InwardType.SelectedValue == "OT")
                    {
                        flag = 1;
                        if (ddlOtherTest.SelectedIndex > 0)
                        {
                            if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "Plate Load Testing")
                            {
                                //if (txt_DiscRate.Text != "")
                                if (j != 1)
                                    txt_Rate.Text = txt_DiscRate.Text;
                                //else if (txt_Rate.Text != "")
                                //    txt_DiscRate.Text = txt_Rate.Text;
                            }
                        }
                    }
                    else if (ddl_InwardType.SelectedValue == "RWH")
                    {
                        flag = 1;
                        // if (txt_DiscRate.Text != "")
                        if (j != 1)
                            txt_Rate.Text = txt_DiscRate.Text;
                        //else if (txt_Rate.Text != "")
                        //    txt_DiscRate.Text = txt_Rate.Text;
                    }


                    //if (txt_Rate.Text == "")
                    //{
                    //    msg = "Input Rate for row no " + (j + 1) + ".";
                    //    valid = false;
                    //    break;
                    //}
                    //else 

                    if (flag == 1)
                    {
                        if (txt_DiscRate.Text == "")
                        {
                            msg = "Input Discount Rate for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDecimal(txt_DiscRate.Text) == 0)
                        {
                            msg = "Discount Rate should be greater than 0 for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }

                    }

                    if (txt_Quantity.Text == "")
                    {
                        msg = "Input Quanity for row no " + (j + 1) + ".";
                        valid = false;
                        break;
                    }
                    else if (txt_Rate.Text != "" && txt_DiscRate.Text != "")
                    {
                        if (Convert.ToDecimal(txt_Rate.Text) > 0)
                        {
                            if (Convert.ToDecimal(txt_Rate.Text) < Convert.ToDecimal(txt_DiscRate.Text))
                            {
                                msg = "Discounted Rate should be less than equal to Unit Rate for row no " + (j + 1) + ".";
                                valid = false;
                                break;
                            }
                        }
                    }

                }
                grdProposal.Focus();
            }
            else if (valid == true && grdGT.Visible == true)//29/01/2018
            {

                for (int j = 0; j < grdGT.Rows.Count; j++)
                {
                    TextBox txt_Quantity = (TextBox)grdGT.Rows[j].Cells[4].FindControl("txt_Quantity");
                    TextBox txt_UnitRate = (TextBox)grdGT.Rows[j].Cells[5].FindControl("txt_UnitRate");
                    TextBox txt_DiscRate = (TextBox)grdGT.Rows[j].Cells[6].FindControl("txt_DiscRate");

                    if (txt_UnitRate.Visible == true && txt_DiscRate.Visible == true)
                    {
                        if (j != 1)
                        {
                            if (txt_DiscRate.Text != "")
                                txt_UnitRate.Text = txt_DiscRate.Text;
                        }

                        if (txt_Quantity.Text == "")
                        {
                            msg = "Input Quantity for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        //else if (txt_UnitRate.Text == "")
                        //{
                        //    msg = "Input Unit Rate for row no " + (j + 1) + ".";
                        //    valid = false;
                        //    break;
                        //}
                        else if (txt_DiscRate.Text == "")
                        {
                            msg = "Input Discount Rate for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDecimal(txt_DiscRate.Text) == 0)
                        {
                            msg = "Discount Rate should be greater than 0 for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        else if (txt_UnitRate.Text != "")
                        {
                            if (Convert.ToDecimal(txt_UnitRate.Text) > 0)
                            {
                                if (Convert.ToDecimal(txt_UnitRate.Text) < Convert.ToDecimal(txt_DiscRate.Text))
                                {
                                    msg = "Discounted Rate should be less than equal to Unit Rate for row no " + (j + 1) + ".";
                                    valid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                grdGT.Focus();
            }

            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + msg + "');", true);

            }
            else
            {
                Calculation();
            }
            return valid;
        }
        private Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;

            if (txt_Client.Text == "" || (Convert.ToInt32(Session["CL_ID"]) == 0 && lblEnqType.Text == "VerifyClient"))
            {
                lblMsg.Text = "Please Select The Client From List";
                txt_Client.Text = "";
                txt_Client.Focus();
                valid = false;
            }
            else if (txt_Site.Text == "" || (Convert.ToInt32(Session["SITE_ID"]) == 0 && lblEnqType.Text == "VerifyClient"))
            {
                lblMsg.Text = "Please Select The Site From List";
                txt_Site.Text = "";
                txt_Site.Focus();
                valid = false;
            }
            else if (txt_Site_address.Text == "")
            {
                lblMsg.Text = "Please Enter The Site Address";
                valid = false;
            }
            else if (txt_Site_LandMark.Text == "")
            {
                lblMsg.Text = "Please Enter The Site Landmark";
                valid = false;
            }
            else if (txt_EnqEmail.Text == "")
            {
                lblMsg.Text = "Please Enter Email";
                txt_EnqEmail.Text = "";
                txt_EnqEmail.Focus();
                valid = false;
            }
            else if (txt_Qty.Text == "" && lblEnqType.Text != "VerifyClientApp")
            {
                lblMsg.Text = "Please Enter Quantity";
                txt_Qty.Focus();
                valid = false;
            }
            else if (txt_ContactPerson.Text == "")
            {
                lblMsg.Text = "Please Select the Contact Person";
                valid = false;
            }
            else if (txt_Contact_No.Text == "")
            {
                lblMsg.Text = "Please Select the Contact Number";
                valid = false;
            }

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
        private Boolean ValidatDataMatColl()
        {
            string msg = "";
            Boolean valid = true;
            if (!rbOrderLoss.Checked && !rbOrderConfirm.Checked && !rbDecisionPending.Checked)
            {
                msg = "Please Select Order Status From Enquiry Details";
                rbOrderConfirm.Focus();
                valid = false;
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "----------------------------Select----------------------------")
            {
                msg = "Please Select Material Collection Status";
                ddl_MatCollectn.Focus();
                valid = false;
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Material To be Collected" && valid == true)
            {
                if (ddl_Location.SelectedIndex == 0)
                {
                    msg = "Please Select the Location";
                    valid = false;
                }
                else if (txt_CollectionDate.Text == string.Empty)
                {
                    msg = "Please Select the collection Date";
                    valid = false;
                }
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Already Collected" && valid == true)
            {
                if (Rdn_AtLab.Checked == false && Rdn_ByDriver.Checked == false)
                {
                    msg = "Please Select Anyone of These Radio Button";
                    valid = false;
                }

                else
                {
                    if (Rdn_ByDriver.Checked == true)
                    {
                        if (ddl_DriverName.SelectedIndex == 0)
                        {
                            msg = "Please Select the Driver Name";
                            valid = false;
                        }
                    }
                }
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Decision Pending" && valid == true)
            {

                if (txt_CommentDecisionPending.Text == string.Empty)
                {
                    msg = "Please Write Some Comment for Decision Pending";
                    txt_CommentDecisionPending.Focus();
                    valid = false;
                }

            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Declined by us" && valid == true)
            {
                if (txt_CommentDeclinebyUs.Text == string.Empty)
                {
                    msg = "Please Write Some Comment for Declined by us";
                    txt_CommentDeclinebyUs.Focus();
                    valid = false;
                }
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Declined by Client" && valid == true)
            {
                if (txt_CommentRejectedByClient.Text == string.Empty)
                {
                    msg = "Please Write Some Comment for Declined by Client";
                    txt_CommentRejectedByClient.Focus();
                    valid = false;
                }

            }
            else if (ddl_MatCollectn.SelectedItem.Text == "On site Testing" && valid == true)
            {
                if (txt_OnsiteTesting_Date.Text == string.Empty)
                {
                    msg = "Please Select Onsite Testing Date";
                    txt_OnsiteTesting_Date.Focus();
                    valid = false;
                }

            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Delivered by Client" && valid == true)
            {
                if (txt_MaterialSendingOnDate.Text == string.Empty)
                {
                    msg = "Please Select Material Sending Date";
                    txt_MaterialSendingOnDate.Focus();
                    valid = false;
                }
            }

            //if (txt_Qty.Visible == true && txt_Qty.Text == "" && valid == true)
            //{
            //    //if (txt_Qty.Text == "")
            //    //{
            //        msg = "Please Enter Quantity";
            //        valid = false;
            //    //}

            //} else
            if (ddl_EnqStatus.SelectedItem.Text == "----------------------------Select----------------------------" && valid == true)
            {
                msg = "Please Select Enquiry Status";
                ddl_EnqStatus.Focus();
                valid = false;
            }
            else if (ddl_EnqType.SelectedItem.Text == "-----------Select-----------" && valid == true)
            {
                msg = "Please Select Enquiry Type";
                ddl_EnqType.Focus();
                valid = false;
            }
            else if (ddl_InwardType.SelectedItem.Text == "----------------------------Select----------------------------" && valid == true && lblEnqType.Text != "VerifyClientApp")
            {
                msg = "Please Select Inward Test Type";
                ddl_InwardType.Focus();
                valid = false;
            }

            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + msg + "');", true);

            }

            return valid;
        }

        private static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
        private bool chkSiteMonthlyOrNot(int siteId, int clientId)
        {
            int mothlyBit = 0;
            var site = dc.Site_View(siteId, clientId, 0, "").ToList();
            if (site.Count > 0)
            {
                if (site.FirstOrDefault().SITE_MonthlyBillingStatus_bit.ToString() != "" || site.FirstOrDefault().SITE_MonthlyBillingStatus_bit.ToString() != null)
                    mothlyBit = Convert.ToInt32(site.FirstOrDefault().SITE_MonthlyBillingStatus_bit);
            }

            return Convert.ToBoolean(mothlyBit);
        }
        protected void btn_SaveEnquiryProposal_Click(object sender, EventArgs e)
        {
            lblProposalError.Visible = false;
            bool flagEnqProposal = false, flagProposal = true;
            if (lblEnqNo.Visible == true)
            {
                if (ValidateData() == true && ValidatDataMatColl() == true)
                {
                    flagEnqProposal = true; flagProposal = false;
                }
            }
            else if (ddl_MatCollectn.SelectedItem.Text == "Declined by us" || ddl_MatCollectn.SelectedItem.Text == "Declined by Client" || chkConifrmEnq.Checked == false || chkWalkIn.Checked == true)//|| chkNewClient.Checked == true || lblMonthlyStatus.Text == "1"
            {
                if (ValidateData() == true && ValidatDataMatColl() == true)
                {
                    flagEnqProposal = true;
                }
                flagProposal = false;
            }
            else
            {
                if (ValidateData() == true && ValidatDataMatColl() == true && ValidateDataProposal() == true)
                {
                    flagEnqProposal = true;
                    flagProposal = true;
                }
            }
            #region Enquiry
            if (flagEnqProposal)
            {
                clsData clsObj = new clsData();

                string strEnqId = "", mailIdCC = "", mailIdTo = ""; //, tollFree = "";
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
                    mailIdTo = txt_EnqEmail.Text.Trim();
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
                            //mailIdCC = Convert.ToString(st.SITE_EmailID_var);
                        }
                    }
                    if (txt_Contact_No.Text != "")
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
                   
                    if (txtEmail.Text != "")
                        mailIdCC = txtEmail.Text.Trim();

                }
                if (valid == true && ValidateEnqStatus() == true)
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
                    byte ENQ_CRLimitApprStatus_tint = 0, ENQ_ConfirmStatus_bit = 0, ENQ_WalkingStatus_bit = 0;
                    int ENQ_LOCATION_Id = 0, ENQ_OrderStatus_bit = 0;
                    int ENQ_ROUTE_Id = 0;
                    byte ENQ_Status_tint = 0;
                    int ENQ_DriverId = 0;
                    DateTime? ClentExpectedDate = null;
                    DateTime? CollectionDate = null;
                    DateTime? MaterialSendingOnDate = null;
                    DateTime? TestingDate = null;
                    int qty = 0;
                  


                    if (txt_Qty.Text != "")
                        qty = Convert.ToInt32(txt_Qty.Text);

                    if (ddl_Location.SelectedIndex > 0)
                    {
                        ENQ_LOCATION_Id = Convert.ToInt32(ddl_Location.SelectedValue);
                    }
                    else
                    {
                        ENQ_LOCATION_Id = 0;
                    }
                    if (chkConifrmEnq.Checked)
                        ENQ_ConfirmStatus_bit = 1;

                    if (chkWalkIn.Checked)
                        ENQ_WalkingStatus_bit = 1;

                    if (rbOrderConfirm.Checked)
                        ENQ_OrderStatus_bit = 1;
                    else if(rbDecisionPending.Checked)
                        ENQ_OrderStatus_bit = 2;


                    if (ddl_MatCollectn.SelectedItem.Text.Equals("Material To be Collected")) //to be collected
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
                    else
                    {
                        ENQ_Status_tint = 1;
                    }
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
                            if (ddl_InwardType.SelectedItem.Text == "Cube Testing" || ddl_InwardType.SelectedItem.Text == "Coupon")
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
                    if (ddl_MatCollectn.SelectedItem.Text.Equals("Material To be Collected"))
                    {
                        //OpenEnQuiryStatus = Tab_To_be_Collected.Text;
                        OpenEnQuiryStatus = "To be Collected";
                        ENQ_CollectionDate_dt = txt_CollectionDate.Text;//need to change
                        CollectionDate = DateTime.ParseExact(txt_CollectionDate.Text, "dd/MM/yyyy", null);
                    }
                    else if (ddl_MatCollectn.SelectedItem.Text.Equals("Already Collected"))
                    {
                        //OpenEnQuiryStatus = Tab_Already_Collected.Text;
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
                    else
                    {
                        //OpenEnQuiryStatus = Tab_Others.Text;
                        OpenEnQuiryStatus = "Others";
                        if (ddl_MatCollectn.SelectedItem.Text.Equals("Declined by us") || ddl_MatCollectn.SelectedItem.Text.Equals("Declined by Client"))
                        {
                            ENQ_Status_tint = 2;
                        }
                        if (ddl_MatCollectn.SelectedItem.Text.Equals("Decision Pending"))
                        {
                            OpenEnQuiryStatus = ddl_MatCollectn.SelectedItem.Text;
                        }
                        else if (ddl_MatCollectn.SelectedItem.Text.Equals("Declined by us"))
                        {
                            OpenEnQuiryStatus = ddl_MatCollectn.SelectedItem.Text;
                        }
                        else if (ddl_MatCollectn.SelectedItem.Text.Equals("Declined by Client"))
                        {
                            OpenEnQuiryStatus = ddl_MatCollectn.SelectedItem.Text;
                        }
                        else if (ddl_MatCollectn.SelectedItem.Text.Equals("On site Testing"))
                        {
                            OpenEnQuiryStatus = ddl_MatCollectn.SelectedItem.Text;
                        }
                        else if (ddl_MatCollectn.SelectedItem.Text.Equals("Delivered by Client"))
                        {
                            OpenEnQuiryStatus = ddl_MatCollectn.SelectedItem.Text;
                        }
                    }
                    if (ddl_MatCollectn.SelectedItem.Text.Equals("Decision Pending"))
                    {
                        comment = txt_CommentDecisionPending.Text;
                    }
                    else if (ddl_MatCollectn.SelectedItem.Text.Equals("Declined by us"))
                    {
                        comment = txt_CommentDeclinebyUs.Text;
                    }
                    else if (ddl_MatCollectn.SelectedItem.Text.Equals("Declined by Client"))
                    {
                        comment = txt_CommentRejectedByClient.Text;
                    }
                    if (ddl_MatCollectn.SelectedItem.Text.Equals("On site Testing"))
                    {
                        ENQ_TestingDate_dt = txt_OnsiteTesting_Date.Text;
                        TestingDate = DateTime.ParseExact(txt_OnsiteTesting_Date.Text, "dd/MM/yyyy", null);//need to change
                    }
                    else if (ddl_MatCollectn.SelectedItem.Text.Equals("Delivered by Client"))
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
                        clsData cd = new clsData();
                        clsSendMail objcls = new clsSendMail();
                        int materialId = 0;

                        if (ddl_InwardType.SelectedItem.Text.ToString() == "Coupon")
                            materialId = cd.getMaterialTypeId("CT");
                        else
                            materialId = cd.getMaterialTypeId(ddl_InwardType.SelectedValue.ToString());

                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Green;
                        string enquiryStatus = "", enqType = "";
                        enquiryStatus = ddl_EnqStatus.SelectedItem.Text;
                        enqType = ddl_EnqType.SelectedItem.Text;

                        if (clientId == 0)
                        {
                            int EnqId = 0;//txt_Quantity.Text
                            EnQ_No = dc.EnquiryNewClient_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, materialId, Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, qty, Convert.ToInt32(Session["LoginId"]), txt_Client.Text.Trim(), txt_Site.Text.Trim(), txt_Site_address.Text.Trim(), txt_Site_LandMark.Text.Trim(), txt_ContactPerson.Text.Trim(), txt_Contact_No.Text.Trim(), enquiryStatus, enqType, Convert.ToBoolean(ENQ_ConfirmStatus_bit), Convert.ToBoolean(ENQ_WalkingStatus_bit), Convert.ToInt32(ENQ_OrderStatus_bit),Convert.ToString(txt_EnqEmail.Text),Convert.ToString(txtClientAddress.Text));
                            lblEnqId.Text = EnQ_No.ToString();
                            strEnqId = EnQ_No.ToString();
                            lblMsg.Text = "Record is Saved Successfully. " + " " + " Your Enquiry No is " + " " + (lblEnqId.Text).ToString();
                            ////lnkSaveEnquiry.Visible = false;
                            ////lnk_Inward.Visible = false;
                            //btn_SaveEnquiryProposal.Visible = false;
                            //btn_Inward.Visible = false;
                            //btn_SaveEnquiryProposal1.Visible = false;
                            //btn_Inward1.Visible = false;
                            lblSelectedInward.Text = ddl_InwardType.SelectedItem.Text;
                            //Cleartextbox();
                        }
                        else
                        {
                            dc.Contact_Update(contactPerId, clientId, siteId, txt_ContactPerson.Text, txt_Contact_No.Text, "", false);
                            if (contactPerId == 0)
                            {
                                var c = dc.Contact_View(0, siteId, clientId, txt_ContactPerson.Text);
                                foreach (var cn in c)
                                {
                                    txt_Contact_No.Text = Convert.ToString(cn.CONT_ContactNo_var);
                                    Session["CONTPersonID"] = cn.CONT_Id;
                                    contactPerId = cn.CONT_Id;
                                }
                            }
                            int EnqId = 0, tempEnqId = 0;
                            if (lblEnqId.Text == "")
                            {
                                EnqId = 0;
                            }
                            else
                            {
                                EnqId = Convert.ToInt32(lblEnqId.Text);
                            }
                            //Approve auto enquiry - if cr limit
                            DateTime? EditedCollectionDate = null;
                            DateTime? ApproveDate = null;
                            byte enqStatus = 0; //int redTagCL = 0;
                            ApproveDate = DateTime.Now;
                            if (OpenEnQuiryStatus == "To be Collected")
                                enqStatus = 0;
                            else
                                enqStatus = 1;
                    
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
                                    string[] arr = lblEnqNo.Text.Split(' ');
                                    string enqNo = "";
                                    if (lblEnqNo.Text != "")
                                        enqNo = arr[3];
                                    tempEnqId = dc.Enquiry_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, Convert.ToInt32(lblMaterialId.Text), clientId, siteId, contactPerId, Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, ENQ_CRLimitApprStatus_tint, Convert.ToDecimal(lblMaterialQuantity.Text), Convert.ToInt32(Session["LoginId"]), enquiryStatus, enqType, Convert.ToBoolean(ENQ_ConfirmStatus_bit), Convert.ToBoolean(ENQ_WalkingStatus_bit), Convert.ToInt32(ENQ_OrderStatus_bit), Convert.ToInt32(enqNo));
                                    
                                    //mail after enquiry registered
                                    if (Convert.ToDecimal(lblBalanceAmt.Text) <= Convert.ToDecimal(lblLimitAmt.Text))
                                    {
                                        SendMailAfterEnquiryRegistered(Convert.ToString(tempEnqId), lblMaterialName.Text, mailIdTo, mailIdCC, ENQ_Date_dt);
                                    }
                                   
                                    //if enq is to be collected then auto approved it
                                    if (enqStatus == 0)
                                        AutoApprovalOfEnquiry(Convert.ToInt32(tempEnqId), enqStatus, ApproveDate, EditedCollectionDate);

                                    /*
                                    //sms for RedTagClient
                                    //if (i==0)//multiple record type for one client so multiple enq from mob.. so need to send msg only once to that client 
                                    //{
                                    //    redTagCL = chkRedTagClient(Convert.ToInt32(tempEnqId));
                                    //    if (redTagCL == 1)
                                    //    {
                                    //        objcls.sendSMS(txt_Contact_No.Text, "Dear Customer, Thank you for your enquiry. Please clear your outstanding for better service response.", "DUROCR");
                                    //    }
                                    //}
                                    */

                                    //if (strEnqId == "")
                                    //    strEnqId = ",";
                                    strEnqId += tempEnqId.ToString() + ",";
                                }
                            }
                            else
                            {
                                tempEnqId = dc.Enquiry_Update(EnqId, ENQ_Date_dt, ENQ_Status_tint, materialId, clientId, siteId, contactPerId, Convert.ToInt32(Session["LoginId"]), txt_Note.Text, txt_Reference.Text, OpenEnQuiryStatus, comment, MaterialSendingOnDate, TestingDate, ENQ_LOCATION_Id, ENQ_ROUTE_Id, CollectionDate, ENQ_UrgentStatus_bit, ClentExpectedDate, null, ENQ_CollectedAt_var, "", ENQ_DriverId, ENQ_CRLimitApprStatus_tint, qty, Convert.ToInt32(Session["LoginId"]), enquiryStatus, enqType, Convert.ToBoolean(ENQ_ConfirmStatus_bit), Convert.ToBoolean(ENQ_WalkingStatus_bit), Convert.ToInt32(ENQ_OrderStatus_bit),0);
                                //txt_Quantity.Text
                            }
                            dc.Site_Update(siteId, clientId, txt_Site.Text, 0, txt_Site_address.Text, "", 0, 0, txt_Site_LandMark.Text, Convert.ToInt32(ddl_Location.SelectedValue), false, true, "", 0, "", "", "", null, "", false, false, "", "", "", 0);
                            if (lblEnqType.Text == "VerifyClient")
                            {
                                dc.EnquiryNewClient_Update_Status(Convert.ToInt32(lblTempEnqId.Text), true, tempEnqId.ToString());
                                //update proposal enquiry id for previous proposal
                                if (lblEnqNo.Visible == true)
                                {
                                    dc.Proposal_Update_EnqNo(Convert.ToInt32(lblEnqNo.Text.Replace("Enquiry No : ", "")), tempEnqId);
                                }
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

                              
                                // sms sending new
                               
                                if (strEnqId != "")
                                {
                                    cd.sendEnquiryProposalMsg(strEnqId, ENQ_Date_dt, txt_Contact_No.Text,"Enquiry");
                                }
                                else
                                {
                                    strEnqId = EnQ_No.ToString();
                                    cd.sendEnquiryProposalMsg(strEnqId, ENQ_Date_dt, txt_Contact_No.Text, "Enquiry");
                                }
                         
                                
                                //if enq is to be collected then auto approved it
                                if (!strEnqId.Contains(','))
                                {
                                    //sms for RedTagClient
                                    //redTagCL = chkRedTagClient(Convert.ToInt32(strEnqId));
                                    //if(redTagCL == 1)
                                    //    objcls.sendSMS(txt_Contact_No.Text, "Dear Customer, Thank you for your enquiry. Please clear your outstanding for better service response.", "DUROCR");
                                    
                                    AutoApprovalOfEnquiry(Convert.ToInt32(strEnqId), enqStatus, ApproveDate, EditedCollectionDate);
                                    //mail after enquiry registered
                                    if (Convert.ToDecimal(lblBalanceAmt.Text) <= Convert.ToDecimal(lblLimitAmt.Text))
                                    {
                                        SendMailAfterEnquiryRegistered(strEnqId, ddl_InwardType.SelectedItem.Text, mailIdTo, mailIdCC, ENQ_Date_dt);
                                    }
                                    
                                }



                                //update client contct email id
                                clsObj.updateContactEmail(clientId, siteId, txt_EnqEmail.Text);


                                lblMsg.Text = "Record is Saved Successfully. " + " " + " Your Enquiry No is " + " " + strEnqId;
                                //

                            }

                        }
                        #endregion
                        #region Proposal
                        //proposal save code
                        if (flagProposal)
                        {
                            Tab_Notes.CssClass = "Clicked";
                            Tab_Discount.CssClass = "Initial";
                            Tab_GenericDiscount.CssClass = "Initial";
                            MainView_Proposal.ActiveViewIndex = 0;
                            lblSelectedInward.Text = ddl_InwardType.SelectedItem.Text;
                            DateTime ProposalDt = DateTime.ParseExact(txt_ProposalDt.Text, "dd/MM/yyyy", null);
                            string[] arr;
                            string appliedDisc = "0", ndtClientScope = "", gtDiscNote = "", Note = string.Empty, NoteGT = string.Empty, PayTermGT="", splitString = "", splitNo = "0";
                            int DiscNotesVisibility_bit = 0, SiteWiseRateApplied_bit = 0, propsalNo = 0, proposalStatus = 0, mergeFrom = 0, mergeTo = 0;
                            
                            arr = lblDisc.Text.Split('=');
                            appliedDisc = arr[1];
                            if (appliedDisc != "")
                            {
                                if (Convert.ToInt32(appliedDisc) > 0)
                                    DiscNotesVisibility_bit = 1;
                            }

                            if (txtFrmRow.Text != "")
                                mergeFrom = Convert.ToInt32(txtFrmRow.Text);
                            if (txtToRow.Text != "")
                                mergeTo = Convert.ToInt32(txtToRow.Text);

                            for (int i = 0; i < Grd_Note.Rows.Count; i++)
                            {
                                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].Cells[2].FindControl("txt_NOTE");
                                if (txt_NOTE.Text != "")
                                {
                                    Note = Note + txt_NOTE.Text + "|";
                                }
                            }
                            //additional Charges GT
                            if (Grd_NoteGT.Visible == true)
                            {
                                for (int i = 0; i < Grd_NoteGT.Rows.Count; i++)
                                {
                                    TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[i].FindControl("txt_NOTEGT");
                                    if (txt_NOTEGT.Text != "")
                                    {
                                        NoteGT = NoteGT + txt_NOTEGT.Text + "|";
                                    }
                                }
                            }
                            //payment terms GT
                            if (grdPayTermsGT.Visible == true)
                            {
                                for (int i = 0; i < grdPayTermsGT.Rows.Count; i++)
                                {
                                    TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].FindControl("txtNotePayTermGT");
                                    if (txtNotePayTermGT.Text != "")
                                    {
                                        PayTermGT = PayTermGT + txtNotePayTermGT.Text + "|";
                                    }
                                }
                            }
                            //Client Scope
                            if (grdClientScope.Visible == true)
                            {
                                for (int i = 0; i < grdClientScope.Rows.Count; i++)
                                {
                                    TextBox txt_NOTEClientScope = (TextBox)grdClientScope.Rows[i].FindControl("txt_NOTEClientScope");
                                    if (txt_NOTEClientScope.Text != "")
                                    {
                                        ndtClientScope = ndtClientScope + txt_NOTEClientScope.Text + "|";
                                    }
                                }
                            }
                            if (lblSelectedInward.Text == "Soil Investigation")
                            {
                                if (chkGTDiscNote.Checked == true && chkGTDiscNote.Visible == true)
                                    gtDiscNote = lblGTDiscNote.Text;
                            }

                            if (txt_ProposalNo.Text != "Create New..." && lblModifiedType.Text == "1")//revised
                            {

                                int statusOfNewClient = 0;
                                if (chkNewClient.Checked == true)
                                    statusOfNewClient = 1;

                                string ProNo = "";
                                DataTable dt = clsObj.getProposalNo(Convert.ToInt32(strEnqId), statusOfNewClient);
                                if (dt.Rows.Count > 0)
                                {
                                    ProNo = dt.Rows[0][0].ToString();
                                }
                                splitString = ProNo.Split('/').Last();
                                if (splitString.Contains("R"))
                                    splitNo = GetNumbers(splitString);
                                propsalNo = Convert.ToInt32(splitNo) + 1;
                                int index = txt_ProposalNo.Text.IndexOf("/R");
                                //  int index = ProNo.IndexOf("/R");
                                if (index > 0)
                                {
                                    dc.Proposal_Update(txt_ProposalNo.Text, 0, null, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 1, proposalStatus, 0, 0, false, "", "", "", "", "", false, "", 0, "", false, 0, 0, 0, "","",false,"", false,"");//it update proposalStatus to 0
                                    txt_ProposalNo.Text = txt_ProposalNo.Text.Remove(index);//remove /R from given index from proposal no textbox  
                                }
                                else
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            dc.Proposal_Update(dt.Rows[i][0].ToString(), 0, null, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 1, proposalStatus, 0, 0, false, "", "", "", "", "", false, "", 0, "", false, 0, 0, 0, "","",false,"", false,"");//it update proposalStatus to 0
                                        }
                                    }
                                }
                                txt_ProposalNo.Text = txt_ProposalNo.Text + "/R" + propsalNo;
                                proposalStatus = 2;
                                dc.Proposal_Update_Status(Convert.ToInt32(strEnqId), true, statusOfNewClient);//it update Proposal_ActiveStatus_bit to 1 for all proposal of given enqNo (beacause for new proposal it is defaul 0)


                            }
                            else if (txt_ProposalNo.Text != "Create New..." && lblModifiedType.Text == "2")//modified
                            {
                                dc.Proposal_Update(txt_ProposalNo.Text, 0, null, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 2, proposalStatus, 0, 0, false, "", "", "", "", "", false, "", 0, "", false, 0, 0, 0, "","",false,"", false,"");
                                dc.ProposalDetail_Update(txt_ProposalNo.Text, null, "", "", 0, "", 0, "0", 0, true, "", 0, "");
                                proposalStatus = 1;
                            }
                            else
                            {
                                Int32 NewrecNo = 0;
                                proposalStatus = 1;
                                NewrecNo = clsObj.GetnUpdateRecordNo("ProposalNo");
                                //txt_ProposalNo.Text = "Duro/Pro/" + NewrecNo + "/" + Convert.ToDateTime(txt_ProposalDt.Text).Year;
                                txt_ProposalNo.Text = "Duro/Pro/" + NewrecNo + "/" + DateTime.ParseExact(txt_ProposalDt.Text, "dd/MM/yyyy", null).Year;
                            }
                            if (grdProposal.Visible == true)
                            {
                                for (int j = 0; j < grdProposal.Rows.Count; j++)
                                {
                                    TextBox txt_Particular = (TextBox)grdProposal.Rows[j].Cells[3].FindControl("txt_Particular");
                                    TextBox txt_TestMethod = (TextBox)grdProposal.Rows[j].Cells[4].FindControl("txt_TestMethod");
                                    TextBox txt_Rate = (TextBox)grdProposal.Rows[j].Cells[5].FindControl("txt_Rate");
                                    TextBox txt_Discount = (TextBox)grdProposal.Rows[j].Cells[6].FindControl("txt_Discount");
                                    TextBox txt_DiscRate = (TextBox)grdProposal.Rows[j].Cells[7].FindControl("txt_DiscRate");
                                    TextBox txt_Quantity = (TextBox)grdProposal.Rows[j].Cells[8].FindControl("txt_Quantity");
                                    TextBox txt_Amount = (TextBox)grdProposal.Rows[j].Cells[9].FindControl("txt_Amount");
                                    Label lbl_InwdType = (Label)grdProposal.Rows[j].Cells[10].FindControl("lbl_InwdType");
                                    Label lbl_SiteWiseRate = (Label)grdProposal.Rows[j].Cells[11].FindControl("lbl_SiteWiseRate");
                                    Label lbl_TestId = (Label)grdProposal.Rows[j].Cells[12].FindControl("lbl_TestId");
                                    if (txt_TestMethod.Text == "" || txt_TestMethod.Text == "---")
                                        txt_TestMethod.Text = "NA";

                                    if (lbl_SiteWiseRate.Text != "0")
                                        SiteWiseRateApplied_bit = 1;
                                    dc.ProposalDetail_Update(txt_ProposalNo.Text, ProposalDt, txt_Particular.Text, txt_TestMethod.Text, Convert.ToDecimal(txt_Rate.Text), txt_Discount.Text, Convert.ToDecimal(txt_DiscRate.Text), txt_Quantity.Text, Convert.ToDecimal(txt_Amount.Text), false, lbl_InwdType.Text, Convert.ToInt32(lbl_TestId.Text), "");
                                    if (lblClientId.Text != "" && lblClientId.Text != "0")
                                    {
                                        dc.SiteWiseRate_Update_FromProposal(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), Convert.ToInt32(lbl_TestId.Text), Convert.ToDecimal(txt_DiscRate.Text));
                                    }
                                }
                            }
                            else if (grdGT.Visible == true)
                            {
                                for (int j = 0; j < grdGT.Rows.Count; j++)
                                {
                                    TextBox txt_Particular = (TextBox)grdGT.Rows[j].Cells[2].FindControl("txt_Particular");
                                    TextBox txt_TestMethod = (TextBox)grdGT.Rows[j].Cells[3].FindControl("txt_TestMethod");
                                    TextBox txt_Unit = (TextBox)grdGT.Rows[j].Cells[4].FindControl("txt_Unit");
                                    TextBox txt_Quantity = (TextBox)grdGT.Rows[j].Cells[5].FindControl("txt_Quantity");
                                    TextBox txt_UnitRate = (TextBox)grdGT.Rows[j].Cells[6].FindControl("txt_UnitRate");
                                    TextBox txt_DiscRate = (TextBox)grdGT.Rows[j].Cells[7].FindControl("txt_DiscRate");
                                    TextBox txt_Amount = (TextBox)grdGT.Rows[j].Cells[8].FindControl("txt_Amount");
                                    Label lbl_InwdType = (Label)grdGT.Rows[j].Cells[9].FindControl("lbl_InwdType");

                                    if (grdGT.Rows[j].Cells[8].Visible == false)
                                        txt_TestMethod.Text = "";
                                    if (grdGT.Rows[j].Cells[9].Visible == false)
                                        txt_Unit.Text = "";
                                    if (grdGT.Rows[j].Cells[10].Visible == false)
                                        txt_Quantity.Text = "";
                                    if (grdGT.Rows[j].Cells[11].Visible == false)
                                        txt_UnitRate.Text = "";
                                    if (grdGT.Rows[j].Cells[12].Visible == false)
                                        txt_DiscRate.Text = "";
                                    if (grdGT.Rows[j].Cells[13].Visible == false)
                                        txt_Amount.Text = "";

                                    if (txt_TestMethod.Text == "" || txt_TestMethod.Text == "---")
                                        txt_TestMethod.Text = "NA";
                                    if (txt_UnitRate.Text == "")
                                    {
                                        txt_UnitRate.Text = "0";
                                    }
                                    if (txt_DiscRate.Text == "")
                                    {
                                        txt_DiscRate.Text = "0";
                                    }
                                    if (txt_Amount.Text == "")
                                    {
                                        txt_Amount.Text = "0";
                                    }
                                    dc.ProposalDetail_Update(txt_ProposalNo.Text, ProposalDt, txt_Particular.Text, txt_TestMethod.Text, Convert.ToDecimal(txt_UnitRate.Text), "0", Convert.ToDecimal(txt_DiscRate.Text), txt_Quantity.Text.ToString(), Convert.ToDecimal(txt_Amount.Text), false, lbl_InwdType.Text, 0, txt_Unit.Text);
                                    //dc.SiteWiseRate_Update_FromProposal(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), Convert.ToInt32(lbl_TestId.Text), Convert.ToDecimal(txt_DiscRate.Text)); 
                                }
                            }
                            int subTestId = 0;
                            if (ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.Visible == true)
                                subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue.ToString());

                            string strDiscDetails = "", email = "";
                            strDiscDetails += lblVol.Text + "~" + lblTime.Text + "~" + lblAdv.Text + "~" + lblLoy.Text + "~" + lblProp.Text + "~" + lblApp.Text + "~" + lblCalcDisc.Text + "~" + lblMax.Text + "~" + txtIntro.Text + "~" + lblDisc.Text;
                            //no need cz enq nd proposal screen are merged
                            if (lblClientId.Text == "0" && chkNewClient.Checked == true)
                            {
                                email = txtEmail.Text;
                                //dc.Client_Update_Address(Convert.ToInt32(strEnqId), txt_Site.Text, txt_Contact_No.Text, 0, 1);//here we update site name of client which is not in system
                            }
                            if (txt_Contact_No.Text != "") //update contact no of existing client
                            {
                                dc.Client_Update_Contact(Convert.ToInt32(strEnqId), txt_Contact_No.Text);//here we update contact no of client which is  in system

                            }
                            if (ddlOtherTest.Visible == true && ddl_InwardType.SelectedValue == "OT")
                            {
                                if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "Plate Load Testing" || ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                                    txt_Subject.Text = "Commercial offer for Soil Testing requirement for your site " + txt_Site.Text + ".";
                            }
                            string structAudDetails = "";

                            if (ddl_InwardType.SelectedValue == "OT" && ddlOtherTest.SelectedItem.Text == "Structural Audit")
                            {
                                structAudDetails = txtStructNameOfApartSoc.Text + "~" + txtStructAddress.Text + "~" + txtStructBuiltupArea.Text + "~" +
                                    txtStructNoOfBuild.Text + "~" + txtStructAge.Text + "~" + ddlStructConstWithin5Y.SelectedValue + "~" + ddlStructLocation.SelectedValue + "~" +
                                    ddlStructAddLoadExpc.SelectedValue + "~" + ddlStructDistressObs.SelectedValue;
                            }
                            if (grdPayTermsGT.Visible == false)
                            {
                                PayTermGT = txtPaymentTerm.Text.Trim();
                            }
                            dc.Proposal_Update(txt_ProposalNo.Text, Convert.ToInt32(strEnqId), ProposalDt, txt_KindAttention.Text, txt_Subject.Text, txt_Description.Text, txt_ContactNo.Text, 0, 0, 0, 0, 0, Convert.ToDecimal(txt_NetAmount.Text), Convert.ToInt32(ddl_PrposalBy.SelectedValue), Convert.ToInt32(Session["LoginID"]), 0, proposalStatus, mergeFrom, mergeTo, chkQty.Checked, txtMe.Text, txtMeNum.Text, Note, NoteGT, strDiscDetails, Convert.ToBoolean(chkNewClient.Checked), gtDiscNote, subTestId, email, Convert.ToBoolean(DiscNotesVisibility_bit), Convert.ToDecimal(18), Convert.ToDecimal(lblGstAmt.Text), Convert.ToDecimal(lblGrandTotal.Text), ndtClientScope, txtEmail.Text, Convert.ToBoolean(SiteWiseRateApplied_bit), structAudDetails, chkMOUWorkOrder.Checked, PayTermGT);
                            if (chkPropSendForAppr.Checked == true)
                            {
                                dc.Proposal_Update_ApprovalPendingStatus(txt_ProposalNo.Text, true);
                            }
                            //sms
                            cd.sendEnquiryProposalMsg(txt_ProposalNo.Text, ENQ_Date_dt, txt_Contact_No.Text, "Proposal");

                            lblMsg.Text = "Record Saved Successfully";
                            lblMsg.Visible = true;
                            lblMsg.ForeColor = System.Drawing.Color.Green;

                            #region proposal mail
                            if (chkPropSendForAppr.Checked == false)
                            {
                                int proposalId = 0;
                                var rslt = dc.Proposal_View(Convert.ToInt32(strEnqId), false, 0, txt_ProposalNo.Text).ToList();
                                if (rslt.Count > 0)
                                {
                                    proposalId = Convert.ToInt32(rslt.FirstOrDefault().Proposal_Id);
                                }
                                PrintPDFReport rpt = new PrintPDFReport();
                                rpt.Proposal_PDF(Convert.ToInt32(strEnqId), 0, proposalId, Convert.ToBoolean(chkNewClient.Checked), "Email", txt_ProposalNo.Text);

                                string reportPath = "C:/temp/Veena/" + "Proposal_" + txt_ProposalNo.Text.Replace("/", "_") + ".pdf";
                                if (File.Exists(reportPath))
                                {
                                    string mTo = "", mCC = "", mSubject = "", mbody = "", mReplyTo = ""; 
                                    if (txtEmail.Text.Trim() != txt_EnqEmail.Text.Trim())
                                        mTo = txtEmail.Text.Trim() + "," + txt_EnqEmail.Text.Trim();
                                    else
                                        mTo = txtEmail.Text.Trim();
                                    mSubject = txt_Subject.Text;
                                    if (txtMailCC.Text != "")
                                        mCC = txtMailCC.Text;
                                    //mTo = "shital.bandal@gmail.com";
                                    //mCC = "";
                                    mSubject = "Proposal for " + ddl_InwardType.SelectedItem.Text;
                                    mSubject = mSubject + "  : Proposal No. " + txt_ProposalNo.Text;
                                    mbody = "Dear Customer,<br><br>";
                                    //mbody = mbody + "Please find attached proposal no. " + txt_ProposalNo.Text;
                                    mbody = mbody + "We are pleased to submit our proposal (" + txt_ProposalNo.Text + ") based on our discussion. <br/>You are requested to approve the proposal by work order/ confirmatory email from your <br/>registered company email id for further action.";
                                    if (chkNewClient.Checked)
                                    {
                                        mbody = mbody + "<br/><br/>Also find attached KYC data sheet for your project. <br/>We request you to fill the same and send us by email along with confirmatory mail.";
                                        string kycSheet = "";
                                        if (cnStr.ToLower().Contains("mumbai") == true)
                                            kycSheet = "DurocreteKYCDetailsSheet_Mumbai.xls";
                                        else if (cnStr.ToLower().Contains("nashik") == true)
                                            kycSheet = "DurocreteKYCDetailsSheet_Nashik.xls";
                                        else
                                            kycSheet = "DurocreteKYCDetailsSheet_Pune.xls"; 
                                        reportPath += "," + System.Web.HttpContext.Current.Server.MapPath(".") + "/Images/" + kycSheet;
                                    }
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "Following are commercial details ";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + getProposalDetailHtml();
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "Detail terms and conditions has attached within a proposal.";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "<br/>";
                                    //mbody = mbody + "Please approve the proposal for further action.<br/>Please send approval email to us from your registered email Id." + " <br><br><br>";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "Best Regards,";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "<br/>";
                                    mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";
                                    clsSendMail objMail = new clsSendMail();
                                    objMail.SendMailProposal(mTo, mCC, mSubject, mbody, reportPath, mReplyTo);
                                    if (chkNewClient.Checked)
                                        dc.Proposal_Update_Email(proposalId, mTo, mCC, false);
                                }
                            }
                            #endregion
                            //proposal save code end
                        }
                        #endregion


                        //credit limit exceeding email
                        if (Convert.ToDecimal(lblBalanceAmt.Text) >= Convert.ToDecimal(lblLimitAmt.Text))
                        {
                            if (lblEnqType.Text == "VerifyClientApp")
                            {
                                string enqNo = "";
                                string[] creditEnq = strEnqId.Split(',');
                                for (int i = 0; i < grdInwardType.Rows.Count; i++)
                                {
                                    Label lblMaterialName = (Label)grdInwardType.Rows[i].FindControl("lblMaterialName");
                                    if (lblMaterialName.Text != "Cube Testing" ||
                                        (lblMaterialName.Text == "Cube Testing" && Convert.ToInt32(lblCouponCount.Text) == 0))
                                    {
                                        enqNo = creditEnq[i];
                                        if (enqNo != "")
                                            CRLimitExceedEmail(enqNo, lblMaterialName.Text);
                                        //CRLimitExceedEmail(creditEnq[i + 1], lblMaterialName.Text);old by 15-01-2019 error for 58394,
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

                        btn_SaveEnquiryProposal.Visible = false;
                        btn_Inward.Visible = false;
                        btn_Cal.Visible = false;
                        btn_Cal1.Visible = false;
                        btn_SaveEnquiryProposal1.Visible = false;
                        btn_Inward1.Visible = false;
                        txt_Client.Focus();
                        lblSelectedInward.Text = ddl_InwardType.SelectedItem.Text;
                        if (ddl_MatCollectn.SelectedItem.Text.Equals("Material To be Collected")) // && ENQ_CRLimitApprStatus_tint == 0)
                        {
                            btn_Inward.Visible = true; btn_Inward1.Visible = true;
                        }
                        //lnk_Inward.Visible = true;
                        if (btn_Inward.Visible == true)
                        {
                            btn_Inward.Enabled = false;
                            btn_Inward1.Enabled = false;
                            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                            foreach (var u in user)
                            {
                                if (u.USER_Inward_right_bit == true)
                                {
                                    btn_Inward.Enabled = true;
                                    btn_Inward1.Enabled = true;
                                }
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

        protected void chkConifrmEnq_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkConifrmEnq.Checked)
            {
                txtMailCC.Text = "";
                txtEmail.Text = "";
                txt_KindAttention.Text = "";
                txt_Subject.Text = "";
                txt_Description.Text = "";
                grdGT.Visible = false;
                grdProposal.DataSource = null;
                grdProposal.DataBind();
                grdGT.DataSource = null;
                grdGT.DataBind();
                Grd_Note.DataSource = null;
                Grd_Note.DataBind();
                txt_NetAmount.Text = "";
                txtMe.Text = "";
                txtMeNum.Text = "";
                ddl_PrposalBy.SelectedIndex = 0;
                txt_ContactNo.Text = "";
                txtPaymentTerm.Text = "";
            }
        }

        protected void chkNewClient_CheckedChanged(object sender, EventArgs e)
        {
            chkNewClientFlag = false;

            if (chkNewClient.Checked)
                chkNewClientFlag = true;

        }

        protected void rbOrderConfirm_CheckedChanged(object sender, EventArgs e)
        {
            addEnquiryDetails();
        }

        protected void rbOrderLoss_CheckedChanged(object sender, EventArgs e)
        {
            addEnquiryDetails();
        }

        protected void rbDecisionPending_CheckedChanged(object sender, EventArgs e)
        {
            addEnquiryDetails();
        }
        private void disableEnquiryDetails()
        {
            tblMatToBeCollected.Visible = false;
            tblAlredyCollected.Visible = false;
            tblOther.Visible = false;
            //btn_Cal.Visible = false; btn_Cal1.Visible = false;
        }

        private void addEnquiryDetails()
        {
            disableEnquiryDetails();
            ddl_MatCollectn.Items.Clear();
            ddl_MatCollectn.Items.Insert(0, new ListItem("----------------------------Select----------------------------", "0"));

            if (rbOrderConfirm.Checked)
            {
                ddl_MatCollectn.Items.Insert(1, new ListItem("Material To be Collected", "1"));
                ddl_MatCollectn.Items.Insert(2, new ListItem("Already Collected", "2"));
                ddl_MatCollectn.Items.Insert(3, new ListItem("On site Testing", "3"));//6
                ddl_MatCollectn.Items.Insert(4, new ListItem("Delivered by Client", "4"));//7
            }
            else if (rbOrderLoss.Checked)
            {
                //ddl_MatCollectn.Items.Insert(1, new ListItem("Decision Pending", "1"));
                ddl_MatCollectn.Items.Insert(1, new ListItem("Declined by us", "1"));
                ddl_MatCollectn.Items.Insert(2, new ListItem("Declined by Client", "2"));
            }
            else if (rbDecisionPending.Checked)
            {
                ddl_MatCollectn.Items.Insert(1, new ListItem("Decision Pending", "1"));
                ddl_MatCollectn.SelectedIndex = 1;
                MaterialStatusChange();
            }
        }
        protected void lnkDiscApplyToAll_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool valid = true;
            if (txtDiscApplyToAll.Text == "")
            {
                msg = "Please input discount";
                valid = false;
            }
            else if (Convert.ToDecimal(txtDiscApplyToAll.Text) >= 100)
            {
                msg = "Please input discount less than 100.";
                valid = false;
            }
            else
            {
                int maxUserDiscount = 0;
                var material = dc.Material_View(ddl_InwardType.SelectedValue, ddl_InwardType.SelectedItem.Text);
                foreach (var mat in material)
                {
                    if (lblUserLevel.Text == "1")
                        maxUserDiscount = Convert.ToInt32(mat.MATERIAL_Level1Discount_int);
                    else if (lblUserLevel.Text == "2")
                        maxUserDiscount = Convert.ToInt32(mat.MATERIAL_Level2Discount_int);
                    else if (lblUserLevel.Text == "3")
                        maxUserDiscount = Convert.ToInt32(mat.MATERIAL_Level3Discount_int);
                }
                if (Convert.ToDecimal(txtDiscApplyToAll.Text) > maxUserDiscount)
                {
                    msg = "Can not apply discount greater than " + maxUserDiscount + ".";
                    valid = false;
                }
                if (valid == true)
                {
                    if (grdProposal.Visible == true)
                    {
                        for (int i = 0; i < grdProposal.Rows.Count; i++)
                        {
                            TextBox txt_Rate = (TextBox)grdProposal.Rows[i].FindControl("txt_Rate");
                            TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].FindControl("txt_DiscRate");
                            TextBox txt_Discount = (TextBox)grdProposal.Rows[i].FindControl("txt_Discount");
                            TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].FindControl("txt_Quantity");
                            TextBox txt_Amount = (TextBox)grdProposal.Rows[i].FindControl("txt_Amount");
                            Label lbl_InwdType = (Label)grdProposal.Rows[i].FindControl("lbl_InwdType");
                            decimal maxTempDiscount = 0;
                            if (lbl_InwdType.Text == ddl_InwardType.SelectedValue || (lbl_InwdType.Text == "OTHER" && ddl_InwardType.SelectedValue == "OT"))
                            {
                                maxTempDiscount = Convert.ToDecimal(txtDiscApplyToAll.Text);
                            }
                            else
                            {
                                var material2 = dc.Material_View(lbl_InwdType.Text, "");
                                foreach (var mat in material2)
                                {
                                    if (lblUserLevel.Text == "1")
                                        maxTempDiscount = Convert.ToInt32(mat.MATERIAL_Level1Discount_int);
                                    else if (lblUserLevel.Text == "2")
                                        maxTempDiscount = Convert.ToInt32(mat.MATERIAL_Level2Discount_int);
                                    else if (lblUserLevel.Text == "3")
                                        maxTempDiscount = Convert.ToInt32(mat.MATERIAL_Level3Discount_int);
                                }
                                if (maxTempDiscount > Convert.ToDecimal(txtDiscApplyToAll.Text))
                                    maxTempDiscount = Convert.ToDecimal(txtDiscApplyToAll.Text);
                            }
                            if (chkDiscApplyToAll.Checked == true || txt_Rate.Text == txt_DiscRate.Text || txt_Discount.Text == "" || Convert.ToDecimal(txt_Discount.Text) == 0)
                            {
                                //txt_Discount.Text = txtDiscApplyToAll.Text;
                                txt_Discount.Text = maxTempDiscount.ToString();
                                txt_DiscRate.Text = Convert.ToDecimal(Convert.ToDecimal(txt_Rate.Text) - (Convert.ToDecimal(txt_Rate.Text) * (Convert.ToDecimal(txt_Discount.Text) / 100))).ToString("0.00");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_UnitRate = (TextBox)grdGT.Rows[i].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].FindControl("txt_DiscRate");
                            TextBox txt_Quantity = (TextBox)grdGT.Rows[i].FindControl("txt_Quantity");
                            TextBox txt_Amount = (TextBox)grdGT.Rows[i].FindControl("txt_Amount");
                            if (chkDiscApplyToAll.Checked == true || txt_UnitRate.Text == txt_DiscRate.Text)
                            {
                                txt_DiscRate.Text = Convert.ToDecimal(Convert.ToDecimal(txt_UnitRate.Text) - (Convert.ToDecimal(txt_UnitRate.Text) * (Convert.ToDecimal(txtDiscApplyToAll.Text) / 100))).ToString("0.00");
                            }
                        }
                    }
                    Calculation();
                }
            }
            if (msg != "")
            {
                //ClientScript.RegisterStartupScript(UpdatePanel1.GetType(), "myalert", "alert('" + msg + "');", true);
                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", msg, true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "Showalert", "alert('" + msg + "');", true);
            }
        }
        private string getProposalDetailHtml()
        {
            int srNo = 1, srNoRecType = 1;
            string strDetail = "";
            string strPrevRecType = "";
            decimal inwardTotal = 0, netTotal = 0;

            if (grdProposal.Visible == true)
            {
                for (int i = 0; i < grdProposal.Rows.Count; i++)
                {
                    TextBox txt_Particular = (TextBox)grdProposal.Rows[i].FindControl("txt_Particular");
                    TextBox txt_TestMethod = (TextBox)grdProposal.Rows[i].FindControl("txt_TestMethod");
                    TextBox txt_Rate = (TextBox)grdProposal.Rows[i].FindControl("txt_Rate");
                    TextBox txt_Discount = (TextBox)grdProposal.Rows[i].FindControl("txt_Discount");
                    TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].FindControl("txt_DiscRate");
                    TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].FindControl("txt_Quantity");
                    TextBox txt_Amount = (TextBox)grdProposal.Rows[i].FindControl("txt_Amount");
                    Label lbl_InwdType = (Label)grdProposal.Rows[i].FindControl("lbl_InwdType");

                    if (strPrevRecType != lbl_InwdType.Text)
                    {
                        if (strPrevRecType != "")
                        {
                            strDetail += "<tr>";
                            strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "Total" + "</b></font></td>";
                            strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + inwardTotal.ToString("0.00") + "</font></td>";
                            strDetail += "</tr>";

                            strDetail += "</table>";
                        }

                        string inwardTypeName = "";
                        if (lbl_InwdType.Text == "Coupon")
                        {
                            inwardTypeName = "Cube Testing";
                        }
                        else
                        {
                            var material = dc.Material_View(lbl_InwdType.Text, "");
                            foreach (var mat in material)
                            {
                                inwardTypeName = mat.MATERIAL_Name_var;
                            }
                        }

                        strDetail += "<table>";
                        strDetail += "<tr>";
                        strDetail += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + srNoRecType.ToString() + ") " + inwardTypeName + "</b></font></td>";
                        strDetail += "</tr>";
                        strDetail += "</table>";
                        srNoRecType++;

                        strDetail += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                        strDetail += "<tr>";
                        strDetail += "<td width= 5% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Sr.No" + "</b></font></td>";
                        strDetail += "<td width= 30% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Particular" + "</b></font></td>";
                        strDetail += "<td width= 20% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Test Method" + "</b></font></td>"; //Unit
                        strDetail += "<td width= 10% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Unit Rate" + "</b></font></td>";
                        strDetail += "<td width= 12% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Discounted Rate" + "</b></font></td>";
                        strDetail += "<td width= 6% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Qty" + "</b></font></td>";
                        strDetail += "<td width= 10% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Amount" + "</b></font></td>";
                        strDetail += "</tr>";

                        srNo = 1;
                        inwardTotal = 0;
                    }
                    strPrevRecType = lbl_InwdType.Text;
                    int qty = 0;
                    if (txt_Quantity.Visible == true && txt_Quantity.Text != "")
                    {
                        qty = Convert.ToInt32(txt_Quantity.Text);
                    }
                    strDetail += "<tr>";
                    strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + srNo.ToString() + "</font></td>";
                    strDetail += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + txt_Particular.Text + "</font></td>";
                    strDetail += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + txt_TestMethod.Text + "</font></td>"; //Test Method
                    strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_Rate.Text + "</font></td>";
                    strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_DiscRate.Text + "</font></td>";
                    strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_Quantity.Text + "</font></td>";
                    strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_Amount.Text + "</font></td>";
                    strDetail += "</tr>";
                    srNo++;
                    inwardTotal += Convert.ToDecimal(txt_Amount.Text);
                    netTotal += Convert.ToDecimal(txt_Amount.Text);
                }
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "Total" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + inwardTotal.ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "Net Total" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + netTotal.ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                decimal gstAmount = (netTotal * 18) / 100;
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "GST (18 %)" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + gstAmount.ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "Grand Total" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + (netTotal + gstAmount).ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                strDetail += "</table>";
            }
            else if (grdGT.Visible == true)
            {
                int mergeFrom = 1, mergeTo = 1, rowSpan = 1;
                for (int i = 0; i < grdGT.Rows.Count; i++)
                {
                    TextBox txt_TestMethod = (TextBox)grdGT.Rows[i].FindControl("txt_TestMethod");
                    if (txt_TestMethod.BorderStyle == BorderStyle.None)
                    {
                        mergeTo++;
                        rowSpan++;
                    }
                }
                for (int i = 0; i < grdGT.Rows.Count; i++)
                {
                    TextBox txt_Particular = (TextBox)grdGT.Rows[i].FindControl("txt_Particular");
                    TextBox txt_TestMethod = (TextBox)grdGT.Rows[i].FindControl("txt_TestMethod");
                    TextBox txt_Unit = (TextBox)grdGT.Rows[i].FindControl("txt_Unit");
                    TextBox txt_Quantity = (TextBox)grdGT.Rows[i].FindControl("txt_Quantity");
                    TextBox txt_UnitRate = (TextBox)grdGT.Rows[i].FindControl("txt_UnitRate");
                    TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].FindControl("txt_DiscRate");
                    TextBox txt_Amount = (TextBox)grdGT.Rows[i].FindControl("txt_Amount");
                    Label lbl_InwdType = (Label)grdGT.Rows[i].FindControl("lbl_InwdType");

                    if (i == 0)
                    {
                        string inwardTypeName = "";
                        if (lbl_InwdType.Text == "GT")
                        {
                            inwardTypeName = "Soil Investigation";
                        }
                        else if (lbl_InwdType.Text == "OTHER")
                        {
                            inwardTypeName = "Other Testing";
                        }

                        strDetail += "<table>";
                        strDetail += "<tr>";
                        strDetail += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + srNoRecType.ToString() + ") " + inwardTypeName + "</b></font></td>";
                        strDetail += "</tr>";
                        strDetail += "</table>";
                        srNoRecType++;

                        strDetail += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                        strDetail += "<tr>";
                        strDetail += "<td width= 5% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Sr.No" + "</b></font></td>";
                        strDetail += "<td width= 35% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Particular" + "</b></font></td>";
                        strDetail += "<td width= 15% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Test Method" + "</b></font></td>"; //Unit
                        strDetail += "<td width= 10% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Unit Rate" + "</b></font></td>";
                        strDetail += "<td width= 12% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Discounted Rate" + "</b></font></td>";
                        strDetail += "<td width= 6% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Qty" + "</b></font></td>";
                        strDetail += "<td width= 10% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Amount" + "</b></font></td>";
                        strDetail += "</tr>";

                        srNo = 1;
                        inwardTotal = 0;
                    }
                    int qty = 0;
                    if (txt_Quantity.Visible == true && txt_Quantity.Text != "")
                    {
                        qty = Convert.ToInt32(txt_Quantity.Text);
                    }

                    strDetail += "<tr>";
                    strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + srNo.ToString() + "</font></td>";
                    strDetail += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + txt_Particular.Text + "</font></td>";
                    if (i == mergeFrom)
                    {
                        strDetail += "<td align=left valign=top height=19 rowspan=" + rowSpan + "><font size=2>&nbsp;" + txt_TestMethod.Text + "</font></td>"; //Test Method
                        strDetail += "<td align=center valign=top height=19  rowspan=" + rowSpan + "><font size=2>&nbsp;" + txt_UnitRate.Text + "</font></td>";
                        strDetail += "<td align=center valign=top height=19  rowspan=" + rowSpan + "><font size=2>&nbsp;" + txt_DiscRate.Text + "</font></td>";
                        strDetail += "<td align=center valign=top height=19  rowspan=" + rowSpan + "><font size=2>&nbsp;" + txt_Quantity.Text + "</font></td>";
                        strDetail += "<td align=center valign=top height=19  rowspan=" + rowSpan + "><font size=2>&nbsp;" + txt_Amount.Text + "</font></td>";
                    }
                    else if (i < mergeFrom || i > mergeTo)
                    {
                        strDetail += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + txt_TestMethod.Text + "</font></td>"; //Test Method
                        strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_UnitRate.Text + "</font></td>";
                        strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_DiscRate.Text + "</font></td>";
                        strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_Quantity.Text + "</font></td>";
                        strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + txt_Amount.Text + "</font></td>";
                    }
                    strDetail += "</tr>";
                    srNo++;
                    inwardTotal += Convert.ToDecimal(txt_Amount.Text);
                    netTotal += Convert.ToDecimal(txt_Amount.Text);
                }
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "Total" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + inwardTotal.ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "Net Total" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + netTotal.ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                decimal gstAmount = (netTotal * 18) / 100;
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "GST (18 %)" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + gstAmount.ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                strDetail += "<tr>";
                strDetail += "<td align=right valign=top height=19 colspan=6><font size=2><b>&nbsp;" + "Grand Total" + "</b></font></td>";
                strDetail += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + (netTotal + gstAmount).ToString("0.00") + "</font></td>";
                strDetail += "</tr>";
                strDetail += "</table>";
            }

            return strDetail;
        }
    }

}