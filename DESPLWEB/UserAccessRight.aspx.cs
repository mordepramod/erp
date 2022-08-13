using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace DESPLWEB
{
    public partial class UserAccessRight : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            rdn_ActiveUser.Checked = true;
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enable/Disable Access Rights";
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
        private void FirstGrd()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("USER_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_Designation_var", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_EmailId_var", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_RemoteId_var", typeof(string)));
            dr = dt.NewRow();
            dr["USER_Id"] = string.Empty;
            dr["USER_Name_var"] = string.Empty;
            dr["USER_Designation_var"] = string.Empty;
            dr["USER_EmailId_var"] = string.Empty;
            dr["USER_RemoteId_var"] = string.Empty;
            dt.Rows.Add(dr);
            grdUserRight.DataSource = dt;
            grdUserRight.DataBind();

        }
        protected void lnkSaveUser_Click(object sender, EventArgs e)
        {

            if (lblUserId.Text == "")
            {
                lblUserId.Text = "0";
            }
            bool ActiveStatus = false;
            if (!chk_Active.Checked)
            {
                ActiveStatus = true;
            }


            dc.User_Update(Convert.ToInt32(lblUserId.Text), txt_UserName.Text, txt_EmailId.Text, txt_RemoteId.Text, txt_UserDesignation.Text, ddl_Section.SelectedItem.Text, Convert.ToBoolean(Chk_Entry.Checked),
                 Convert.ToBoolean(Chk_Check.Checked), Convert.ToBoolean(Chk_Recheck.Checked), Convert.ToBoolean(Chk_Print.Checked), Convert.ToBoolean(Chk_Inward.Checked), Convert.ToBoolean(Chk_Bill.Checked), Convert.ToBoolean(Chk_View.Checked), Convert.ToBoolean(Chk_Approve.Checked),
                 Convert.ToBoolean(Chk_Admin.Checked), Convert.ToBoolean(Chk_User.Checked), Convert.ToBoolean(Chk_SuperAdmin.Checked), Convert.ToBoolean(chk_Rptapprv.Checked), Convert.ToBoolean(Chk_Enquiry.Checked), Convert.ToBoolean(Chk_InwdApprv.Checked),
                 Convert.ToBoolean(Chk_MatCollec.Checked), Convert.ToBoolean(Chk_RecptApprv.Checked), Convert.ToBoolean(Chk_Account.Checked), Convert.ToBoolean(Chk_CRLmtApprv.Checked), Convert.ToBoolean(Chk_Mkt.Checked), Convert.ToBoolean(Chk_MatTest.Checked), Convert.ToBoolean(Chk_MatRecvd.Checked), Convert.ToBoolean(Chk_RecptLock.Checked), Convert.ToBoolean(Chk_Proposal.Checked), Convert.ToBoolean(chk_CS.Checked),
                 Convert.ToBoolean(Chk_Clientapprv.Checked), Convert.ToBoolean(Chk_BillPrint.Checked), Convert.ToBoolean(Chk_AllEnqApprv.Checked),Convert.ToBoolean(Chk_DiscountModify.Checked), ActiveStatus, "");


            lblpmsg.Text = "Data Saved Successfully";
            lblpmsg.Visible = true;
            lnkSaveUser.Enabled = false;
            DisplayUserDetails();
        }
        protected void lnkCancelUser_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        protected void imgInsertUser_Click(object sender, CommandEventArgs e)
        {
            lblUserId.Text = "0";
            ModalPopupExtender1.Show();
            lblpmsg.Visible = false;
            lblAddUser.Text = " Add New User ";
            ChkBoxesDisable();
        }
        private void ChkBoxesDisable()
        {
            chk_SelectAll.Checked = false;
            chk_Active.Checked = false;
            Chk_Entry.Checked = false;
            Chk_Check.Checked = false;
            Chk_Recheck.Checked = false;
            Chk_Print.Checked = false;
            Chk_View.Checked = false;
            Chk_Approve.Checked = false;
            Chk_Inward.Checked = false;
            Chk_Bill.Checked = false;
            Chk_Admin.Checked = false;
            Chk_Enquiry.Checked = false;
            Chk_User.Checked = false;
            Chk_SuperAdmin.Checked = false;
            chk_Rptapprv.Checked = false;
            Chk_Inward.Checked = false;
            Chk_InwdApprv.Checked = false;
            Chk_MatCollec.Checked = false;
            Chk_RecptApprv.Checked = false;
            Chk_Account.Checked = false;
            Chk_CRLmtApprv.Checked = false;
            Chk_Mkt.Checked = false;
            Chk_MatTest.Checked = false;
            Chk_MatRecvd.Checked = false;
            Chk_RecptLock.Checked = false;
            Chk_Proposal.Checked = false;
            chk_CS.Checked = false;
            Chk_Clientapprv.Checked = false;
            Chk_BillPrint.Checked = false;
            Chk_AllEnqApprv.Checked = false;
            Chk_DiscountModify.Checked = false;
            lblpmsg.Visible = false;
            txt_UserName.Text = "";
            txt_UserDesignation.Text = "";
            txt_EmailId.Text = "";
            txt_RemoteId.Text = "";
            lnkSaveUser.Enabled = true;
        }
        protected void imgCloseUserPopup_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        protected void imgEditUser_Click(object sender, CommandEventArgs e)
        {
            ChkBoxesDisable();
            lblUserId.Text = Convert.ToString(e.CommandArgument);
            ModalPopupExtender1.Show();
            lblAddUser.Text = "Define User Rights";

            var allUser = dc.User_View(Convert.ToInt32(lblUserId.Text), -1, "", "", "");
            foreach (var grd in allUser)
            {
                txt_UserName.Text = Convert.ToString(grd.USER_Name_var);
                txt_UserDesignation.Text = Convert.ToString(grd.USER_Designation_var);
                txt_EmailId.Text = Convert.ToString(grd.USER_EmailId_var);
                txt_RemoteId.Text = Convert.ToString(grd.USER_RemoteId_var);
                ddl_Section.ClearSelection();
                if (grd.USER_Section != null && grd.USER_Section != "")
                {
                    ddl_Section.Items.FindByText(grd.USER_Section).Selected = true;
                }
                if (grd.USER_Status_bit == false)//1
                {
                    chk_Active.Checked = true;
                }
                if (grd.USER_Entry_right_bit == true)//1
                {
                    Chk_Entry.Checked = true;
                }
                if (grd.USER_Check_right_bit == true)//2
                {
                    Chk_Check.Checked = true;
                }
                if (grd.USER_Recheck_right_bit == true)//2
                {
                    Chk_Recheck.Checked = true;
                }
                if (grd.USER_Print_right_bit == true)//3
                {
                    Chk_Print.Checked = true;
                }
                if (grd.USER_View_right_bit == true)//4
                {
                    Chk_View.Checked = true;
                }
                if (grd.USER_Approve_right_bit == true)//5
                {
                    Chk_Approve.Checked = true;
                }
                if (grd.USER_Inward_right_bit == true)//6
                {
                    Chk_Inward.Checked = true;
                }
                if (grd.USER_Bill_right_bit == true)//7
                {
                    Chk_Bill.Checked = true;
                }
                if (grd.USER_Admin_right_bit == true)//8
                {
                    Chk_Admin.Checked = true;
                }
                if (grd.USER_EnqApprove_right_bit == true)//9
                {
                    Chk_Enquiry.Checked = true;
                }
                if (grd.USER_User_right_bit == true)//10
                {
                    Chk_User.Checked = true;
                }
                if (grd.USER_SuperAdmin_right_bit == true)//11
                {
                    Chk_SuperAdmin.Checked = true;
                }
                if (grd.USER_RptApproval_right_bit == true)//12
                {
                    chk_Rptapprv.Checked = true;
                }
                if (grd.USER_InwardApprove_right_bit == true)//13
                {
                    Chk_InwdApprv.Checked = true;
                }
                if (grd.USER_CollectedStatus_bit == true)//14
                {
                    Chk_MatCollec.Checked = true;
                }
                if (grd.USER_ReceiptApprove_bit == true)
                {
                    Chk_RecptApprv.Checked = true;//15
                }
                if (grd.USER_Account_right_bit == true)//16
                {
                    Chk_Account.Checked = true;
                }
                if (grd.USER_CRLimitApprove_right_bit == true)//17
                {
                    Chk_CRLmtApprv.Checked = true;
                }
                if (grd.USER_Mkt_right_bit == true)//18
                {
                    Chk_Mkt.Checked = true;
                }
                if (grd.USER_Testing_right_bit == true)//19
                {
                    Chk_MatTest.Checked = true;
                }
                if (grd.USER_Receiving_right_bit == true)//20
                {
                    Chk_MatRecvd.Checked = true;
                }
                if (grd.USER_RcptLock_right_bit == true)//21
                {
                    Chk_RecptLock.Checked = true;
                }
                if (grd.USER_Proposal_right_bit == true)//22
                {
                    Chk_Proposal.Checked = true;
                }
                if (grd.USER_CS_right_bit == true)//23
                {
                    chk_CS.Checked = true;
                }
                if (grd.USER_ClientApproval_right_bit == true)//24
                {
                    Chk_Clientapprv.Checked = true;
                }
                if (grd.USER_BillPrint_right_bit == true)//25
                {
                    Chk_BillPrint.Checked = true;
                }
                if (grd.USER_AllEnqApproval_right_bit == true)//26
                {
                    Chk_AllEnqApprv.Checked = true;
                }
                if (grd.User_DiscountModify_right_bit == true)//27
                {
                    Chk_DiscountModify.Checked = true;
                }
            }
        }
        protected void grdUserRight_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].CssClass = "locked";
                e.Row.Cells[1].CssClass = "locked";
            }
        }

        protected void grdUserRight_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (e.CommandName == "ResetPwd")
            {
                int UserId = Convert.ToInt32(e.CommandArgument);
                dc.UserRight_Update(UserId, -1, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, "", "", "", "test123");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Password has been reset to test123');", true);
                //lblMsg.Text = "Password has been reset to test123";
                //lblMsg.Visible = true;
                //lblMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMsg.Visible = false;
            }
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindUserGrid();
            if (grdUserRight.Rows.Count <= 0)
            {
                FirstGrd();
                grdUserRight.Columns[1].Visible = false;
                grdUserRight.Columns[2].Visible = false;
            }
        }
        public void BindUserGrid()
        {
            DisplayUserDetails();
        }
        public void DisplayUserDetails()
        {
            grdUserRight.Columns[1].Visible = true;
            grdUserRight.Columns[2].Visible = true;
            int Status = 0;
            if (rdn_AllUser.Checked)
            {
                Status = -1;
            }
            string searchtextUser = "";
            if (txt_UserSearch.Text != "")
                searchtextUser = txt_UserSearch.Text + "%";
            var User = dc.User_View(0, Status, searchtextUser, "", "");
            grdUserRight.DataSource = User;
            grdUserRight.DataBind();

            if (grdUserRight.Rows.Count > 0)
            {
                int i = 0;
                if (rdn_ActiveUser.Checked)
                {
                    var activeuser = dc.User_View(0, Status, searchtextUser, "", "");
                    foreach (var grd in activeuser)
                    {
                        CheckBox chkUSER_Status_bit_Right = (CheckBox)grdUserRight.Rows[i].Cells[7].FindControl("chkUSER_Status_bit_Right");
                        CheckBox chkEntryRight = (CheckBox)grdUserRight.Rows[i].Cells[8].FindControl("chkEntryRight");
                        CheckBox chkCheckRight = (CheckBox)grdUserRight.Rows[i].Cells[9].FindControl("chkCheckRight");
                        CheckBox chKRecheckRight = (CheckBox)grdUserRight.Rows[i].Cells[10].FindControl("chKRecheckRight");
                        CheckBox chkPrintRight = (CheckBox)grdUserRight.Rows[i].Cells[11].FindControl("chkPrintRight");
                        CheckBox chkViewRight = (CheckBox)grdUserRight.Rows[i].Cells[12].FindControl("chkViewRight");
                        CheckBox chkApproveRight = (CheckBox)grdUserRight.Rows[i].Cells[13].FindControl("chkApproveRight");
                        CheckBox chkInwardRight = (CheckBox)grdUserRight.Rows[i].Cells[14].FindControl("chkInwardRight");
                        CheckBox chkBillRight = (CheckBox)grdUserRight.Rows[i].Cells[15].FindControl("chkBillRight");
                        CheckBox chkAdminRight = (CheckBox)grdUserRight.Rows[i].Cells[16].FindControl("chkAdminRight");
                        CheckBox chkEnquiryRight = (CheckBox)grdUserRight.Rows[i].Cells[17].FindControl("chkEnquiryRight");
                        CheckBox chkUserRight = (CheckBox)grdUserRight.Rows[i].Cells[18].FindControl("chkUserRight");
                        CheckBox chkSuperAdminRight = (CheckBox)grdUserRight.Rows[i].Cells[19].FindControl("chkSuperAdminRight");
                        CheckBox chkRptApprovalRight = (CheckBox)grdUserRight.Rows[i].Cells[20].FindControl("chkRptApprovalRight");
                        CheckBox chkInwardApproveRight = (CheckBox)grdUserRight.Rows[i].Cells[21].FindControl("chkInwardApproveRight");
                        CheckBox chkCollectedStatusRight = (CheckBox)grdUserRight.Rows[i].Cells[22].FindControl("chkCollectedStatusRight");
                        CheckBox chkReceiptApproveRight = (CheckBox)grdUserRight.Rows[i].Cells[23].FindControl("chkReceiptApproveRight");
                        CheckBox chkAccountRight = (CheckBox)grdUserRight.Rows[i].Cells[24].FindControl("chkAccountRight");
                        CheckBox chkCRlimitApproveRight = (CheckBox)grdUserRight.Rows[i].Cells[25].FindControl("chkCRlimitApproveRight");
                        CheckBox chkMktRight = (CheckBox)grdUserRight.Rows[i].Cells[26].FindControl("chkMktRight");
                        CheckBox chkTestingRight = (CheckBox)grdUserRight.Rows[i].Cells[27].FindControl("chkTestingRight");
                        CheckBox chkReceivingRight = (CheckBox)grdUserRight.Rows[i].Cells[28].FindControl("chkReceivingRight");
                        CheckBox chkRcptLockRight = (CheckBox)grdUserRight.Rows[i].Cells[29].FindControl("chkRcptLockRight");
                        CheckBox chkProposalRight = (CheckBox)grdUserRight.Rows[i].Cells[30].FindControl("chkProposalRight");
                        CheckBox chkCSRight = (CheckBox)grdUserRight.Rows[i].Cells[31].FindControl("chkCSRight");
                        CheckBox chkClientApprovalRight = (CheckBox)grdUserRight.Rows[i].Cells[32].FindControl("chkClientApprovalRight");
                        CheckBox chkBillPrintRight = (CheckBox)grdUserRight.Rows[i].Cells[33].FindControl("chkBillPrintRight");
                        CheckBox chkAllEnqApprovalRight = (CheckBox)grdUserRight.Rows[i].Cells[34].FindControl("chkAllEnqApprovalRight");
                        CheckBox chkDiscountModifyRight = (CheckBox)grdUserRight.Rows[i].Cells[35].FindControl("chkDiscountModifyRight");

                        if (grd.USER_Status_bit == false)//1
                        {
                            chkUSER_Status_bit_Right.Checked = true;
                        }
                        if (grd.USER_Entry_right_bit == true)//1
                        {
                            chkEntryRight.Checked = true;
                        }
                        if (grd.USER_Check_right_bit == true)//2
                        {
                            chkCheckRight.Checked = true;
                        }
                        if (grd.USER_Recheck_right_bit == true)//2
                        {
                            chKRecheckRight.Checked = true;
                        }
                        if (grd.USER_Print_right_bit == true)//3
                        {
                            chkPrintRight.Checked = true;
                        }
                        if (grd.USER_View_right_bit == true)//4
                        {
                            chkViewRight.Checked = true;
                        }
                        if (grd.USER_Approve_right_bit == true)//5
                        {
                            chkApproveRight.Checked = true;
                        }
                        if (grd.USER_Inward_right_bit == true)//6
                        {
                            chkInwardRight.Checked = true;
                        }
                        if (grd.USER_Bill_right_bit == true)//7
                        {
                            chkBillRight.Checked = true;
                        }
                        if (grd.USER_Admin_right_bit == true)//8
                        {
                            chkAdminRight.Checked = true;
                        }
                        if (grd.USER_EnqApprove_right_bit == true)//9
                        {
                            chkEnquiryRight.Checked = true;
                        }
                        if (grd.USER_User_right_bit == true)//10
                        {
                            chkUserRight.Checked = true;
                        }
                        if (grd.USER_SuperAdmin_right_bit == true)//11
                        {
                            chkSuperAdminRight.Checked = true;
                        }
                        if (grd.USER_RptApproval_right_bit == true)//12
                        {
                            chkRptApprovalRight.Checked = true;
                        }
                        if (grd.USER_InwardApprove_right_bit == true)//13
                        {
                            chkInwardApproveRight.Checked = true;
                        }
                        if (grd.USER_CollectedStatus_bit == true)//14
                        {
                            chkCollectedStatusRight.Checked = true;
                        }
                        if (grd.USER_ReceiptApprove_bit == true)
                        {
                            chkReceiptApproveRight.Checked = true;//15
                        }
                        if (grd.USER_Account_right_bit == true)//16
                        {
                            chkAccountRight.Checked = true;
                        }
                        if (grd.USER_CRLimitApprove_right_bit == true)//17
                        {
                            chkCRlimitApproveRight.Checked = true;
                        }
                        if (grd.USER_Mkt_right_bit == true)//18
                        {
                            chkMktRight.Checked = true;
                        }
                        if (grd.USER_Testing_right_bit == true)//19
                        {
                            chkTestingRight.Checked = true;
                        }
                        if (grd.USER_Receiving_right_bit == true)//20
                        {
                            chkReceivingRight.Checked = true;
                        }
                        if (grd.USER_RcptLock_right_bit == true)//21
                        {
                            chkRcptLockRight.Checked = true;
                        }
                        if (grd.USER_Proposal_right_bit == true)//22
                        {
                            chkProposalRight.Checked = true;
                        }
                        if (grd.USER_CS_right_bit == true)//23
                        {
                            chkCSRight.Checked = true;
                        }
                        if (grd.USER_ClientApproval_right_bit == true)//24
                        {
                            chkClientApprovalRight.Checked = true;
                        }
                        if (grd.USER_BillPrint_right_bit == true)//25
                        {
                            chkBillPrintRight.Checked = true;
                        }
                        if (grd.USER_AllEnqApproval_right_bit == true)//26
                        {
                            chkAllEnqApprovalRight.Checked = true;
                        }
                        if (grd.User_DiscountModify_right_bit == true)//27
                        {
                            chkDiscountModifyRight.Checked = true;
                        }
                        i++;
                    }
                }
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
    }
}
