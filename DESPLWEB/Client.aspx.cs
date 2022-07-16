using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class Client : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        //public static int Cl_Id = 0; 
        //public static int Site_Id = 0;
        //public static int ContactPersonId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client Updation";

                txtSearch.Attributes.Add("onkeypress", "return clickButton(event,'" + ImgBtnSearch.ClientID + "')");
                //HtmlGenericControl body = (HtmlGenericControl)this.Page.Master.FindControl("myBody");
                //body.Attributes.Add("onkeypress", "catchEsc(event)");
                Session["Cl_Id"] = 0;
                Session["Site_Id"] = 0;
                Session["ContactPersonId"] = 0;

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
                        if (u.USER_Admin_right_bit == true)
                        {
                            userRight = true;
                            chkCouponSetting.Enabled = true;
                        }
                        if (u.USER_ClientApproval_right_bit == true)
                        {
                            lblClientApprRight.Text = "true";
                        }
                        if (u.USER_SuperAdmin_right_bit == false)
                        {
                            chkClientStatus.Enabled = false;
                            // chkSiteStatus.Enabled = false;
                        }
                        if (u.USER_Account_right_bit == false)
                        {
                            // chkServiceTax.Enabled = false;
                        }
                        if (u.USER_ClientApproval_right_bit == true ||
                            u.USER_SuperAdmin_right_bit == true ||
                            u.USER_CRLimitApprove_right_bit == true ||
                            u.USER_SuperAccount_right_bit == true)
                        {
                            txtCrLimitMod.ReadOnly = false;
                            txtCrPeriodMod.ReadOnly = false;
                            lnkUpdateCreditDetails.Enabled = true;
                        }
                    }
                }
                userRight = true;
                if (userRight == false)
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
                FirstGridViewRowOfClient();
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
                string[] item = rowObj.CL_Name_var.Split(' ');
                // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                for (int i = 0; i < item.Length; i++)
                {
                    dt.Rows.Add(item[i]);
                }

            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    string[] item = rowObj.CL_Name_var.Split(' ');
                    // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    for (int i = 0; i < item.Length; i++)
                    {
                        dt.Rows.Add(item[i]);
                    }
                    //  dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();

            DataTable dt1 = new DataTable();
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "CL_Name_var");
            if (distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").Length != 0)
                dt1 = distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").CopyToDataTable();
            //else
            //    dt1 = distinctValues.Copy();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                CL_Name_var.Add(dt1.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }
        //public static List<string> GetClientname(string prefixText)
        //{
        //    string searchHead = "";
        //    LabDataDataContext db = new LabDataDataContext();
        //    if (prefixText != "")
        //        searchHead = prefixText + "%";
        //    var query = db.Client_View(0, -1, searchHead, "");
        //    DataRow row = null;
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("CL_Name_var");
        //    foreach (var rowObj in query)
        //    {
        //        row = dt.NewRow();
        //        ////string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
        //        //string item = "";
        //        //if (rowObj.CL_Status_bit == 0)
        //        //    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var + "-Continue", rowObj.CL_Id.ToString());
        //        //else if (rowObj.CL_Status_bit == 1)
        //        //    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var + "-Discontinue", rowObj.CL_Id.ToString());
        //        //else if (rowObj.CL_Status_bit == 2)
        //        //    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var + "-Not Approved", rowObj.CL_Id.ToString());
        //        //dt.Rows.Add(item);
        //        string[] item = rowObj.CL_Name_var.Split(' ');
        //        for (int i = 0; i < item.Length; i++)
        //        {
        //            dt.Rows.Add(item[i]);
        //        }
        //    }
        //    if (row == null)
        //    {
        //        var clnm = db.Client_View(0, -1, "", "");
        //        foreach (var rowObj in clnm)
        //        {
        //            row = dt.NewRow();
        //            //string item = "";
        //            //if (rowObj.CL_Status_bit == 0)
        //            //    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var + "-Continue", rowObj.CL_Id.ToString());
        //            //else if (rowObj.CL_Status_bit == 1)
        //            //    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var + "-Discontinue", rowObj.CL_Id.ToString());
        //            //else if (rowObj.CL_Status_bit == 2)
        //            //    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var + "-Not Approved", rowObj.CL_Id.ToString());
        //            //dt.Rows.Add(item);
        //            string[] item = rowObj.CL_Name_var.Split(' ');
        //            for (int i = 0; i < item.Length; i++)
        //            {
        //                dt.Rows.Add(item[i]);
        //            }                    
        //        }
        //    }
        //    List<string> CL_Name_var = new List<string>();
        //    //for (int i = 0; i < dt.Rows.Count; i++)
        //    //{
        //    //    CL_Name_var.Add(dt.Rows[i][0].ToString());
        //    //}
        //    DataTable dt1 = new DataTable();
        //    DataView view = new DataView(dt);
        //    DataTable distinctValues = view.ToTable(true, "CL_Name_var");
        //    if (distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").Length != 0)
        //        dt1 = distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").CopyToDataTable();
        //    for (int i = 0; i < dt1.Rows.Count; i++)
        //    {
        //        CL_Name_var.Add(dt1.Rows[i][0].ToString());
        //    }
        //    return CL_Name_var;

        //}
        
        protected void lnkSaveCity_Click(object sender, EventArgs e)
        {
            if (ddlState.SelectedIndex != -1 && ddlState.SelectedIndex != 0)
            {
                if (txtCity.Text != "")
                {
                    ListItem item = ddlCity.Items.FindByText(txtCity.Text);
                    if (item == null && txtCity.Text != "")
                    {
                        ddlCity.Items.Add(txtCity.Text);
                    }

                    if (ddlCity.Items.FindByText(txtCity.Text) != null)
                    {
                        ddlCity.ClearSelection();
                        ddlCity.Items.FindByText(txtCity.Text).Selected = true;
                    }

                    ddlCity.Visible = true;
                    txtCity.Visible = false; txtCity.Text = "";
                    lnkAddCity.Text = "New City";
                    lnkSaveCity.Visible = false;
                }
            }
            else
            {
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true;
                lblClientMessage.Text = "Please Select State";
            }
        }
        protected void lnkAddCity_Click(object sender, EventArgs e)
        {
            if (lnkAddCity.Text == "New City")
            {
                ddlCity.Visible = false;
                lnkAddCity.Text = "Old City"; txtCity.Text = "";
                txtCity.Visible = true; lnkSaveCity.Visible = true;
            }
            else
            {
                ddlCity.Visible = true;
                lnkAddCity.Text = "New City";
                txtCity.Visible = false; lnkSaveCity.Visible = false;
                // ddlCity.Items.Add(txtClntCity.Text);
                //  ddlCity.Items.FindByText(txtClntCity.Text).Selected = true;

            }

        }


        protected void imgInsertClient_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            lblAddClient.Text = "Add New Client";
            Session["Cl_Id"] = 0;
            lblClId.Text = "";
            chkClientStatus.Checked = true;
            LoadStateList();
            BindCity();
            rbSiteUnReg.Checked = false; valName1.Visible = false;
            valName2.Visible = false;
            rbSiteReg.Checked = false;
            txtGstNo.Enabled = false;
            txtGstDate.Enabled = false;
            ddlCity.Visible = true;
            txtCity.Visible = false; txtCity.Text = "";
            lnkAddCity.Text = "New City";
            lnkSaveCity.Visible = false;
            //grdSite.Visible = false;
            //lblEditedClient.Visible = false;
            //lblSite.Visible = false;
            //grdContact.Visible = false;
            //lblEditedSite.Visible = false;
            //lblContPer.Visible = false;
            txtClientName.ReadOnly = false;
            lnkUpdateDirectorDetail.Visible = false;
            lnkUpdateAuthPersonDetail.Visible = false;
        }
        protected void imgEditClient_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            lnkUpdateDirectorDetail.Visible = true;
            lnkUpdateAuthPersonDetail.Visible = true;
            rbSiteUnReg.Checked = false; valName1.Visible = false;
            valName2.Visible = false;
            rbSiteReg.Checked = false; txtGstNo.Enabled = false; txtGstDate.Enabled = false;
            ddlCity.Visible = true;
            txtCity.Visible = false; txtCity.Text = "";
            lnkAddCity.Text = "New City";
            lnkSaveCity.Visible = false;
            LoadClientGroupList(); LoadStateList();
            lnkSaveClient.Enabled = true;
            lblClientMessage.Visible = false;
            //  ClearAllControls();
            lblAddClient.Text = "Edit Client";
            if (lblClientApprRight.Text != "true")
                txtClientName.ReadOnly = true;
            else
                txtClientName.ReadOnly = false;

            Session["Cl_Id"] = Convert.ToInt32(e.CommandArgument);
            lblClId.Text = e.CommandArgument.ToString();
            var cl = dc.Client_View(Convert.ToInt32(lblClId.Text ), -1, "", "");
            foreach (var client in cl)
            {
                lblCurrentClientName.Text = client.CL_Name_var;
                lblCurrentClientId.Text = "Client Id : " + client.CL_Id;
                txtClientName.Text = client.CL_Name_var;
                txtClientEmail.Text = client.CL_EmailID_var;
                txtDirector.Text = client.CL_DirectorName_var;
                txtDirectorEmail.Text = client.CL_DirectorEmailID_var;
                txtLedgerName.Text = client.CL_LedgerName_var;
                txtOffAddress.Text = client.CL_OfficeAddress_var;
                txtOffTelNo.Text = client.CL_OfficeTelNo_var;
                txtPanNo.Text = client.CL_PANNo_var;
                txtTanNo.Text = client.CL_TANNo_var;
                txtOffFaxNo.Text = client.CL_OfficeFaxNo_var;
                txtAuthPerson.Text = client.CL_AuthorisedPerson_var;
                txtAccContact.Text = client.CL_AccContactName_var;
                txtAccContactNo.Text = client.CL_AccContactNo_var;
                txtAccEmail.Text = client.CL_AccEmailId_var;
                txtPincode.Text = client.CL_Pin_int.ToString();
                txtLoginName.Text = client.CL_LoginId_var;
                txtPassword.Text = client.CL_Password_var;
                txtCrLimitStd.Text = client.CL_Limit_mny.ToString();
                txtCrLimitMod.Text = client.CL_CreditLimitModified_dec.ToString();
                txtCrPeriodStd.Text = client.CL_CreditPeriod_int.ToString();
                txtCrPeriodMod.Text = client.CL_CreditPeriodModified_int.ToString();
                txtAvegBusiness.Text = client.CL_AvgBusiness_dec.ToString();
                //  txtCity.Text = client.CL_City_var;
                txtNatureBusiness.Text = client.CL_NatureOfBusn_var;
                if (client.CL_DirectorDetailUpdate_date != null)
                    lblDirectorDetailUpdateDate.Text = "Last updated on : " + Convert.ToDateTime(client.CL_DirectorDetailUpdate_date).ToString("dd/MM/yyyy hh:mm tt");


                if (client.CL_GST_bit == true)
                {
                    rbSiteReg.Checked = true; rbSiteUnReg.Checked = false;
                    txtGstNo.Enabled = true;
                    txtGstDate.Enabled = true;
                    if (client.CL_GstDate_date != null)
                    {
                        txtGstDate.Text = Convert.ToDateTime(client.CL_GstDate_date).ToString();
                        System.Web.UI.WebControls.Calendar cll = new System.Web.UI.WebControls.Calendar();
                        cll.SelectedDate = Convert.ToDateTime(txtGstDate.Text);
                        txtGstDate.Text = cll.SelectedDate.ToString("dd/MM/yyyy");
                    }


                    txtGstNo.Text = client.CL_GstNo_var;

                }
                else if (client.CL_GST_bit == false)
                {
                    rbSiteUnReg.Checked = true;
                    rbSiteReg.Checked = false;
                    txtGstNo.Enabled = false;
                    txtGstDate.Enabled = false;
                    txtGstDate.Text = "";
                    txtGstNo.Text = "";
                }
                chkPriorityClient.Checked = false; ;
                if (client.CL_Priority_bit == true)
                    chkPriorityClient.Checked = true;


                if (ddlState.Items.FindByText(client.CL_State_var) != null)
                    ddlState.Items.FindByText(client.CL_State_var).Selected = true;
                else if (client.CL_State_var != null && client.CL_State_var != "")
                    ddlState.SelectedItem.Text = client.CL_State_var;


                if (client.CL_Group_var != null && client.CL_Group_var != "")
                    ddlClientGroup.SelectedValue = client.CL_Group_var;

                //if (client.CL_State_var != null && client.CL_State_var != "")
                //    ddlState.SelectedValue = client.CL_State_var;

                if (client.CL_NatureOfFirm_var != null && client.CL_NatureOfFirm_var != "")
                    ddlNatureOfFirm.SelectedValue = client.CL_NatureOfFirm_var;


                if (ddlState.SelectedValue != "")
                    BindCity();
                if (ddlCity.Items.FindByText(client.CL_City_var) != null)
                {
                    ddlCity.Items.FindByText(client.CL_City_var).Selected = true;
                }
                else if (client.CL_City_var != null && client.CL_City_var != "")
                {
                    ddlCity.SelectedItem.Text = client.CL_City_var;
                }

                //if (client.CL_Status_bit == false)
                if (client.CL_Status_bit == 2)
                {
                    chkClientStatus.Checked = true;
                    lnkSaveClient.Text = "Approve";
                    lnkViewAllClient.Visible = true;
                }
                else if (client.CL_Status_bit == 0)
                {
                    chkClientStatus.Checked = true;
                    lnkSaveClient.Text = "Save";
                    txtClientName.ReadOnly = true;
                    lnkViewAllClient.Visible = false;
                }
                else
                {
                    chkClientStatus.Checked = false;
                    lnkSaveClient.Text = "Save";
                    txtClientName.ReadOnly = true;
                    lnkViewAllClient.Visible = false;
                }

                if (client.CL_SiteSpecificCoupon_bit == true)
                    chkCouponSetting.Checked = true;
                else
                    chkCouponSetting.Checked = false;

                break;
            }


        }
        private void LoadClientList()
        {
            string searchHead = "";
            if (txtSearch.Text != "" || chkApprPending.Checked == true)
                searchHead = txtSearch.Text + "%";
            //searchHead = "%" + txtSearch.Text + "%";
            int clientStatus = 0;
            if (chkApprPending.Checked == true)
                clientStatus = 2;
            var cl = dc.Client_View(0, clientStatus, searchHead, "");
            grdClient.DataSource = cl;
            grdClient.DataBind();
            lblClient.Visible = true;
            if (grdClient.Rows.Count <= 0)
                FirstGridViewRowOfClient();


        }
        private void LoadClientGroupList()
        {
            var clgrp = dc.Client_View_GroupList();
            ddlClientGroup.DataSource = clgrp;
            ddlClientGroup.DataTextField = "CL_Group_var";
            ddlClientGroup.DataBind();
            ddlClientGroup.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void LoadStateList()
        {
            var stateGrp = dc.State_View();
            ddlState.DataSource = stateGrp;
            ddlState.DataTextField = "State_Name_var";
            ddlState.DataValueField = "State_Id";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void ClearAllControls()
        {
            chkCouponSetting.Checked = false;
            lblCurrentClientName.Text = "";
            lblCurrentClientId.Text = "";
            txtAccContact.Text = "";
            txtAccContactNo.Text = "";
            txtAccEmail.Text = "";
            txtAuthPerson.Text = "";
            txtClientEmail.Text = "";
            txtClientName.Text = "";
            ddlNatureOfFirm.SelectedIndex = 0;
            txtNatureBusiness.Text = "";
            ddlState.SelectedValue = "0";
            txtPincode.Text = "";
            txtDirector.Text = "";
            txtDirectorEmail.Text = "";
            txtLedgerName.Text = "";
            txtOffAddress.Text = "";
            txtOffFaxNo.Text = "";
            txtOffTelNo.Text = "";
            lblDirectorDetailUpdateDate.Text = "";
            txtPanNo.Text = "";
            txtTanNo.Text = "";
            txtLoginName.Text = "";
            txtPassword.Text = "";
            txtGstDate.Text = "";
            txtGstNo.Text = "";
            txtClientGroup.Text = "";
            ddlClientGroup.SelectedValue = "0";
            txtCity.Text = "";
            txtCrLimitStd.Text = "";
            txtCrLimitMod.Text = "";
            txtCrPeriodStd.Text = "";
            txtCrPeriodMod.Text = "";
            txtAvegBusiness.Text = "";
            lnkNewGroup.Text = "New";
            txtClientGroup.Visible = false;
            ddlClientGroup.Visible = true;
            RequiredFieldValidator26.Visible = false;
            RequiredFieldValidator14.Visible = true;
            lblClientMessage.Visible = false;
            //lblSiteMessage.Visible = false;
            //lblContMessage.Visible = false;
            lnkSaveClient.Enabled = true;
            //lnkSaveSite.Enabled = true;
            //lnkSaveContact.Enabled = true;
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            lblClientMessage.Visible = false; bool flag = true; 
            txtClientName.Text = txtClientName.Text.Replace("\t", "");
            txtLedgerName.Text = txtLedgerName.Text.Replace("\t", "");
            valName1.Visible = false;
            valName2.Visible = false;
            clsSendMail objcls = new clsSendMail();
            if(txtPincode.Text!="" && txtPincode.Text.Length<6)
            {
                flag = false;
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true;
                lblClientMessage.Text = "Invalid PinCode";
            }
            else if (ddlCity.SelectedItem.Text == "--Select--" || ddlCity.SelectedItem.Text == "")
            {
                flag = false;
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true;
                lblClientMessage.Text = "Please Select City";
            }
            else if (txtCity.Visible == true)
            {
                flag = false;
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true;
                lblClientMessage.Text = "Please Select City";
            }
            else if (rbSiteReg.Checked == false && rbSiteUnReg.Checked == false)
            {
                lblClientMessage.Text = "Input GST.";
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true; flag = false;
            }
            else if (rbSiteReg.Checked)
            {
                if (txtGstNo.Text == "")
                {
                    valName1.Visible = true; flag = false;
                }
                else
                {
                    if (txtGstNo.Text.Length < 15)
                    {
                        lblClientMessage.Text = "Invalid GST No.";
                        lblClientMessage.ForeColor = System.Drawing.Color.Red;
                        lblClientMessage.Visible = true; flag = false;
                    }
                    else
                    {
                        var cl = dc.GST_DuplicateView(txtGstNo.Text, Convert.ToInt32(Session["Cl_Id"])).ToList();
                        if (cl.Count > 0)
                        {
                            lblClientMessage.Text = "GST No already updated to client- ";
                            foreach (var objcl in cl)
                            {
                                lblClientMessage.Text = lblClientMessage.Text + objcl.CL_Name_var.ToString();
                                break;
                            }
                            lblClientMessage.ForeColor = System.Drawing.Color.Red;
                            lblClientMessage.Visible = true; flag = false;
                        }
                    }
                }

                //if (txtGstDate.Text == "")
                //{
                //    valName2.Visible = true; flag = false;
                //}

            }

            if (Page.IsValid)
            {
                if (flag)
                {
                    byte clientStatus = 0;
                    if (lblClId.Text == "")
                    {
                        lblClId.Text = "0";
                    }

                    //check for duplicate client
                    var cl = dc.Client_View(0, 0, txtClientName.Text, "");
                    foreach (var client in cl)
                    {
                        if (Convert.ToInt32(lblClId.Text) == 0)
                        {
                            clientStatus = 1;
                            lblClientMessage.Text = "Duplicate Client Name..";
                            lblClientMessage.Visible = true;
                            break;
                        }
                        else if (Convert.ToInt32(lblClId.Text) > 0)
                        {
                            if (client.CL_Id != Convert.ToInt32(lblClId.Text))
                            {
                                clientStatus = 1;
                                lblClientMessage.Text = "Duplicate Client Name..";
                                lblClientMessage.Visible = true;
                                break;
                            }
                        }
                    }
                    //
                    //check for duplicate client login name
                    //var clogin = dc.Client_View_Login(Convert.ToInt32(Session["Cl_Id"]), txtLoginName.Text, "");
                    //foreach (var clientl in clogin)
                    //{   
                    //    clientStatus = 1;
                    //    lblClientMessage.Text = "Duplicate Client Login Name..";
                    //    lblClientMessage.Visible = true;
                    //    break;
                    //}
                    //
                    if (clientStatus == 0)
                    {
                        bool gstBit = false, flagPriority = false;

                        if (rbSiteReg.Checked)
                            gstBit = true;

                        if (chkClientStatus.Checked == true)
                            clientStatus = 0;
                        else
                            clientStatus = 1;


                        if (chkPriorityClient.Checked)
                            flagPriority = true;
                     
                        string strClientGroup = "";
                        if (txtClientGroup.Visible == true)
                            strClientGroup = txtClientGroup.Text;
                        else if (ddlClientGroup.Visible == true)
                            strClientGroup = ddlClientGroup.SelectedValue;

                        string cl_City;

                        //  int clientID = dc.Client_Update(Convert.ToInt32(Session["Cl_Id"]), txtClientName.Text, clientStatus, txtOffAddress.Text, txtOffTelNo.Text, txtOffFaxNo.Text, txtClientEmail.Text, txtDirector.Text, txtDirectorEmail.Text, txtPanNo.Text, txtTanNo.Text, txtAuthPerson.Text, txtAccContact.Text, txtAccContactNo.Text, txtAccEmail.Text, txtLedgerName.Text, "", strClientGroup, chkCouponSetting.Checked, txtLoginName.Text, txtPassword.Text);

                        Nullable<DateTime> dt = null;

                        if (txtGstDate.Text != "")
                            dt = DateTime.ParseExact(txtGstDate.Text, "dd/MM/yyyy", null);

                        string ledgerNm = "";
                        if (txtLedgerName.Text == "")
                            ledgerNm = txtClientName.Text;
                        else
                            ledgerNm = txtLedgerName.Text;


                        if (ddlCity.Visible == true)
                            cl_City = ddlCity.SelectedItem.Text;
                        else
                            cl_City = txtCity.Text;
                        int clientID = 0;
                        if (lblClId.Text == "")
                            lblClId.Text = "0";
                        else
                            clientID = Convert.ToInt32(lblClId.Text);
                        clientID = dc.Client_Update(clientID, txtClientName.Text, clientStatus, txtOffAddress.Text, txtOffTelNo.Text, txtOffFaxNo.Text, txtClientEmail.Text, txtDirector.Text, txtDirectorEmail.Text, txtPanNo.Text, txtTanNo.Text, txtAuthPerson.Text, txtAccContact.Text, txtAccContactNo.Text, txtAccEmail.Text, ledgerNm, "", strClientGroup, chkCouponSetting.Checked, txtLoginName.Text, txtPassword.Text, ddlNatureOfFirm.SelectedItem.Text, txtNatureBusiness.Text, ddlState.SelectedItem.Text, Convert.ToInt32(txtPincode.Text), txtGstNo.Text, dt, gstBit, cl_City, Convert.ToInt32(Session["LoginId"]));
                        dc.Client_Update_Priority(Convert.ToInt32(lblClId.Text), flagPriority);
                        var cityDetails = dc.City_Update(cl_City, Convert.ToInt32(ddlState.SelectedValue), 0).ToList();

                        if (cityDetails.Count() == 0)
                            dc.City_Update(cl_City, Convert.ToInt32(ddlState.SelectedValue), 1);
                        string cnStr = ConfigurationManager.AppSettings["conStr"].ToString();

                        if (Convert.ToInt32(lblClId.Text) == 0)
                        {
                            if (cnStr.ToLower().Contains("mumbai") == true)
                            {
                                dc.Client_Update_Login(clientID, "22m" + (clientID + 22).ToString(), "test" + (clientID).ToString());
                            }
                            else if (cnStr.ToLower().Contains("nashik") == true)
                            {
                                dc.Client_Update_Login(clientID, "25n" + (clientID + 25).ToString(), "test" + (clientID).ToString());
                            }
                            else
                            {
                                dc.Client_Update_Login(clientID, "20p" + (clientID + 20).ToString(), "test" + (clientID).ToString());
                            }
                        }
                        if (lnkSaveClient.Text == "Approve")
                        {
                            //sendmSg
                            string smsContent = "";
                            if (txtOffTelNo.Text.Trim().Length == 10)
                            {
                                smsContent = @"You are added as registered user on our web portal www.durocrete.in. Your Login id  is " + txtLoginName.Text + " , password is " + txtPassword.Text + ". You can view reports on our web portal.";
                                
                                objcls.sendSMS(txtOffTelNo.Text, smsContent, "DUROCR", "1007235754611039213");                                
                            }
                            //sendMail
                            SendMailToAprroveClient(txtClientEmail.Text, txtLoginName.Text, txtPassword.Text);


                            //dc.DiscountSetting_Update_Introductory(Convert.ToInt32(Session["Cl_Id"]), 10);
                        }
                        lblClId.Text = clientID.ToString();
                        lblClientMessage.Text = "Updated Successfully";
                        lblClientMessage.ForeColor = System.Drawing.Color.Green;

                        lblClientMessage.Visible = true;
                        if (txtSearch.Text != "")
                            LoadClientList();

                        //  ModalPopupExtender1.Hide();
                        lnkSaveClient.Enabled = false;
                    }
                    // ClearAllControls();
                }
            }
        }
        protected void lnkmailLogin_Click(object sender, EventArgs e)
        {
            if (txtLoginName.Text.Trim() != "" && txtPassword.Text.Trim() != "")
            {
                SendMailToAprroveClient(txtClientEmail.Text, txtLoginName.Text, txtPassword.Text);
                lblClientMessage.Text = "Email Sent..";
                lblClientMessage.ForeColor = System.Drawing.Color.Green;
                lblClientMessage.Visible = true;
            }
        }
        private void SendMailToAprroveClient(string mailIdTo, string loginId, string passwrd)
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
            }

            string cnStr = ConfigurationManager.AppSettings["conStr"].ToString();

            if (sendMail == true)
            {
                clsSendMail objMail = new clsSendMail();
                string mSubject = "", mbody = "", mReplyTo = "", tollFree = "";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    tollFree = "9850500013";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    tollFree = "7720006754";
                else
                    tollFree = "18001206465";
                List<string> siteNameList = new List<string>();
              
                mSubject = "Login Details";

                mbody = "Dear Customer,<br><br>";
                mbody = mbody + @"Welcome to Durocrete Portal. You are now a registered user on our web site www.durocrete.in. You can now view/download reports from our web site. <br>You can also add registered mobile users from the web portal. The registered mobile users can download our mobile App from Play Store and place enquiries, view reports on their mobile phones. 
                                <br>Your credentials for our web portal are as follows : ";
                mbody = mbody + "<br>Login Id : " + loginId + " <br>Password : " + passwrd + ".";
                mbody = mbody + "<br>You are requested to change your password periodically.";
                mbody = mbody + "<br>For any assistance, please contact on our Toll free no." + tollFree;
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>";
                mbody = mbody + "<br>";
                mbody = mbody + "<br>";
                mbody = mbody + "<br>";
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
        private void FirstGridViewRowOfClient()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Status_bit", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_OfficeAddress_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_OfficeTelNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_EmailId_var", typeof(string)));
            dr = dt.NewRow();
            dr["CL_Id"] = 0;
            dr["CL_Name_var"] = string.Empty;
            dr["CL_Status_bit"] = string.Empty;
            dr["CL_OfficeAddress_var"] = string.Empty;
            dr["CL_OfficeTelNo_var"] = string.Empty;
            dr["CL_EmailId_var"] = string.Empty;
            dt.Rows.Add(dr);

            grdClient.DataSource = dt;
            grdClient.DataBind();

        }
        protected void grdClient_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblClientId = ((Label)e.Row.FindControl("lblClientId"));
                if (lblClientId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgEditClient")).Visible = false;
                    //((LinkButton)e.Row.FindControl("btnSites")).Visible = false;
                }
                if (lblClientApprRight.Text != "true")
                {
                    ((ImageButton)e.Row.FindControl("imgEditClient")).Visible = false;
                }
            }
        }

        protected void lnkNewGroup_Click(object sender, EventArgs e)
        {
            if (ddlClientGroup.Visible == true)
            {
                txtClientGroup.Visible = true;
                ddlClientGroup.Visible = false;
                lnkNewGroup.Text = "Existing";
                RequiredFieldValidator26.Visible = true;
                RequiredFieldValidator14.Visible = false;
            }
            else
            {
                txtClientGroup.Visible = false;
                ddlClientGroup.Visible = true;
                lnkNewGroup.Text = "New";
                txtClientGroup.Text = "";
                RequiredFieldValidator26.Visible = false;
                RequiredFieldValidator14.Visible = true;
            }
        }
        protected void lnkCancelClient_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender1.Hide();
        }

        protected void lnkCancelSite_Click(object sender, EventArgs e)
        {
            ClearAllControls();

        }

        protected void lnkCancelContact_Click(object sender, EventArgs e)
        {
            ClearAllControls();

        }

        protected void imgCloseClientPopup_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender1.Hide();
        }

        protected void imgCloseSitePopup_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();

        }

        protected void imgCloseContactPopup_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();

        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;

            if (txtSearch.Text != "" || chkApprPending.Checked == true)
            {
                LoadClientList();
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Enter the Client Name";
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please Enter the Client Name');", true);
            }

        }
        protected void rbSiteUnReg_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSiteUnReg.Checked)
            {
                txtGstNo.Enabled = false;
                txtGstDate.Enabled = false;
                txtGstNo.Text = ""; txtGstDate.Text = ""; valName1.Visible = false;
                valName2.Visible = false;
            }

        }
        protected void rbSiteReg_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSiteReg.Checked)
            {
                txtGstNo.Enabled = true;
                txtGstDate.Enabled = true;

            }

        }

        protected void chkApprPending_CheckedChanged(object sender, EventArgs e)
        {
            pnlClient.Visible = false;
            LoadClientList();
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlState.SelectedIndex > 0)
            {
                BindCity(); lblClientMessage.Visible = false;
            }
        }
        private void BindCity()
        {
            var reg = dc.City_View(Convert.ToInt32(ddlState.SelectedValue));
            ddlCity.DataSource = reg;
            ddlCity.DataTextField = "City_Name_var";
            ddlCity.DataValueField = "City_Name_var";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        protected void lnkViewAllClient_Click(object sender, EventArgs e)
        {
            pnlClientSearch.Visible = true;
            pnlClient1.Visible = false; 
            if (txtClientName.Text != "")
            {
                string searchHead = "";
                if (txtClientName.Text != "" )
                    searchHead = "%" + txtClientName.Text + "%";
                
                var cl = dc.Client_View(0, -1, searchHead, "");
                grdClientSearch.DataSource = cl;
                grdClientSearch.DataBind();
                
            }
        }

        protected void imgCloseClientSearch_Click(object sender, ImageClickEventArgs e)
        {
            pnlClientSearch.Visible = false;
            pnlClient1.Visible = true;
            grdClientSearch.DataSource = null;
            grdClientSearch.DataBind();
        }
        protected void grdClientSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblClientStatus = ((Label)e.Row.FindControl("lblClientStatus"));
                if (lblClientStatus.Text == "0")
                {
                    lblClientStatus.Text = "Active";
                }
                else if (lblClientStatus.Text == "1")
                {
                    lblClientStatus.Text = "Deactive";
                }
                else if (lblClientStatus.Text == "2")
                {
                    lblClientStatus.Text = "Approval Pending";
                }
            }
        }
        protected void lnkOkClientSearch_Click(object sender, EventArgs e)
        {   
            pnlClientSearch.Visible = false;
            pnlClient1.Visible = true;
            grdClientSearch.DataSource = null;
            grdClientSearch.DataBind();
        }

        protected void lnkUpdateDirectorDetail_Click(object sender, EventArgs e)
        {
            dc.Client_Update_DirectorDetails(Convert.ToInt32(lblClId.Text), txtDirector.Text.Trim(), txtDirectorEmail.Text.Trim(), txtOffTelNo.Text.Trim());
            lblClientMessage.Text = "Director details updated successfully";
            lblClientMessage.ForeColor = System.Drawing.Color.Green;
            lblClientMessage.Visible = true;
        }

        protected void lnkUpdateAuthPersonDetail_Click(object sender, EventArgs e)
        {
            dc.Client_Update_AuthPersonDetails(Convert.ToInt32(lblClId.Text), txtAuthPerson.Text.Trim(), txtClientEmail.Text.Trim());
            lblClientMessage.Text = "Authorised person details updated successfully";
            lblClientMessage.ForeColor = System.Drawing.Color.Green;
            lblClientMessage.Visible = true;
        }
        protected void lnkUpdateCreditDetails_Click(object sender, EventArgs e)
        {
            if (lblClId.Text == "" || lblClId.Text == "0")
            {
                lblClientMessage.Text = "First add client, then update credit details";
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true;
            }
            else if (txtCrLimitMod.Text == "")
            {
                lblClientMessage.Text = "Input Credit Limit - Modified";
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true;
            }
            else if (txtCrPeriodMod.Text == "")
            {
                lblClientMessage.Text = "Input Credit Period - Modified";
                lblClientMessage.ForeColor = System.Drawing.Color.Red;
                lblClientMessage.Visible = true;
            }
            else
            {
                dc.Client_Update_CreditDetails(Convert.ToInt32(lblClId.Text),Convert.ToInt32(txtCrPeriodMod.Text), Convert.ToDecimal(txtCrLimitMod.Text));
                lblClientMessage.Text = "Credit details updated successfully";
                lblClientMessage.ForeColor = System.Drawing.Color.Green;
                lblClientMessage.Visible = true;
            }
        }
        //protected void lnkLoadAllClient_Click(object sender, EventArgs e)
        //{
        //    txtSearch.Text = "";
        //    LoadClientList();
        //}

    }
}
