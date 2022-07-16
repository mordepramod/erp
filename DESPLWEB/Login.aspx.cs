using System;

namespace DESPLWEB
{
    public partial class Login : System.Web.UI.Page
    {
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(txtCurrentPassword.Text.Trim())))
            {
                txtCurrentPassword.Attributes["value"] = txtCurrentPassword.Text;
            }

            if (!IsPostBack)
            {
                //lblheader.Text = "Employee Login - Pune";
                //if (cnStr.ToLower().Contains("mumbai") == true)
                //    lblheader.Text = "Employee Login - Mumbai";
                //else if (cnStr.ToLower().Contains("nashik") == true)
                //    lblheader.Text = "Employee Login - Nashik";

                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                //Response.Cache.SetNoStore();

                LoadLoginUser();
            }
        }
        private void LoadLoginUser()
        {
            ddlUserName.DataTextField = "USER_Name_var";
            ddlUserName.DataValueField = "USER_Id";
            var user = dc.User_View(0, 0, "", "", "");
            ddlUserName.DataSource = user;
            ddlUserName.DataBind();
            ddlUserName.Items.Insert(0, "---Select---");
        }
        protected void singIn_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                lbl_Error.Visible = false;
                int UserId = 0;
                var user = dc.User_View(0, 0, ddlUserName.SelectedItem.Text, "", password.Text);
                foreach (var u in user)
                {
                    UserId = u.USER_Id;
                    Session["LoginId"] = UserId;
                    Session["LoginUserName"] = ddlUserName.SelectedItem.Text;
                    Session["Superadmin"] = u.USER_SuperAdmin_right_bit;
                    //Session["InwardApproveRight"] = u.USER_InwardApprove_right_bit;
                    //Session["WithoutBillRight"] = u.USER_WithoutBill_right_bit;
                    //commented on 21-09-2020
                    //bool delayFlag = false;
                    //if (Session["LoginUserName"].ToString().ToLower().Contains("vishwas") ||
                    //    Session["LoginUserName"].ToString().ToLower().Contains("ujwal") ||
                    //    Session["LoginUserName"].ToString().ToLower().Contains("sampada") )
                    //{
                    //    delayFlag = true;
                    //    Response.Redirect("ReportDelayList.aspx");
                    //}
                    //else
                    //{
                    //    var report = dc.AlertDelayReportList_View(UserId).ToList();
                    //    if (report.Count() > 0)
                    //    {
                    //        delayFlag = true;
                    //        Response.Redirect("ReportDelayList.aspx");
                    //    }
                    //}
                    //if (delayFlag == false)
                    //{
                    //    if (u.USER_SuperAdmin_right_bit == true)
                    //    {
                    //        Response.Redirect("EnquiryPendingForME.aspx");
                    //    }
                    //    else
                    //    {
                    //        var enq = dc.Enquiry_View_OpenMEWise(UserId);
                    //        if (enq.Count() > 0)
                    //        {
                    //            Response.Redirect("EnquiryPendingForME.aspx");
                    //        }
                    //        else
                    //        {
                    //            var report = dc.ReportPending_View_CRLimitExceed(UserId).ToList();
                    //            if (report.Count() > 0)
                    //            {
                    //                Response.Redirect("EnquiryPendingForME.aspx");
                    //            }
                    //            else
                    //            {
                    //                Response.Redirect("Home.aspx");
                    //            }
                    //        }
                    //    }
                    //}
                    Response.Redirect("Home.aspx");
                }
                if (UserId == 0)
                {
                    lbl_Error.Text = "Invalid Login Id /Password...";
                    lbl_Error.Visible = true;
                }
            }
        }
        protected Boolean ValidateData()
        {
            Boolean valid = true;
            if (ddlUserName.SelectedItem.Text == "---Select---")
            {
                lbl_Error.Text = "Select Login Id  ";
                ddlUserName.Focus();
                valid = false;
            }
            else if (password.Text == "")
            {
                lbl_Error.Text = "Enter Password ";
                password.Focus();
                valid = false;
            }
            else if (password.Text.ToLower().Trim() == "test123")
            {
                lbl_Error.Text = "Please change your Password ";
                password.Focus();
                valid = false;
            }
            if (valid == false)
            {
                lbl_Error.Visible = true;
            }
            else
            {
                lbl_Error.Visible = false;
            }
            return valid;
        }
        protected void LnkChangePassword_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            txtUserName.Text = "";
            if (ddlUserName.SelectedItem.Text != "---Select---")
            {
                txtUserName.Text = ddlUserName.SelectedItem.Text;
                txtCurrentPassword.Focus();
            }
            PnlChangePassowrd.Visible = true;
            lblMessage.Visible = false;
            txtCurrentPassword.Attributes["value"] = "";
            txtNewPassword.Text = "";
            txtConfirmNewPassword.Text = "";
            lnkChangePasswordButton.Enabled = true;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        protected void lnkExit_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
            PnlChangePassowrd.Visible = false;
        }
        protected void lnkChangePasswordButton_Click(object sender, EventArgs e)
        {
            if (ValidatePwd() == true)
            {
                bool valid= false;
                int UserId = 0;
                var user = dc.User_View(0, 0, txtUserName.Text,"", "");
                foreach (var u in user)
                {
                    UserId = u.USER_Id;
                }
                var us = dc.User_View(0, 0, txtUserName.Text,txtCurrentPassword.Text, "");
                foreach (var u in us)
                {
                    if (u.USER_Password_var.ToString().Equals(txtCurrentPassword.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        valid = true;
                    }
                }
                if (valid == true)
                {
                    if (UserId > 0)
                    {
                        dc.User_Update(UserId, "", "", "", "", "", false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,false, txtNewPassword.Text);
                        lblMessage.Text = "Password has been changed Sucessfully";
                        txtCurrentPassword.Attributes["value"] = "";
                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lnkChangePasswordButton.Enabled = false;
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid Login Id /Password...";
                    lblMessage.Visible = true;
                }
            }
        }
        protected Boolean ValidatePwd( )
        {
            Boolean valid = true;
            if (txtUserName.Text == string.Empty)
            {
                lblMessage.Text = "Enter Login Id ";
                txtUserName.Focus();
                valid = false;
            }
            else if (txtCurrentPassword.Text == string.Empty)
            {
                lblMessage.Text = "Enter Current Password";
                txtCurrentPassword.Focus();
                valid = false;
            }
            else if (txtNewPassword.Text == string.Empty)
            {
                lblMessage.Text = "Enter new Password";
                txtNewPassword.Focus();
                valid = false;
            }
            else if (txtConfirmNewPassword.Text == string.Empty)
            {
                lblMessage.Text = "Enter Confirm Password";
                txtConfirmNewPassword.Focus();
                valid = false;
            }
            else if (txtConfirmNewPassword.Text != txtNewPassword.Text)
            {
                lblMessage.Text = "New/Confirm Password mismatch";
                txtConfirmNewPassword.Focus();
                valid = false;
            }
            if (valid == false)
            {
                lblMessage.Visible = true;
            }
            else
            {
                lblMessage.Visible = false;
            }
            return valid;
        }

        protected void lnkdefault_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
        
    }
}
