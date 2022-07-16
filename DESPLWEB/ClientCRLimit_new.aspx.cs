using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ClientCRLimit_new : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Update Client Credit Limit";

                bool userRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                lnk1daybyPass.Visible = false ;
                foreach (var u in user)
                {
                    if (u.USER_Level_tint == 3 || u.USER_OneDayBypassCrL_right_bit == true)
                    {
                        userRight = true;
                        chkByPassCreditLimit.Enabled = true;
                        if (u.USER_OneDayBypassCrL_right_bit == true)
                        {
                            lblRight.Text = "onedaybypasscrlimit";
                            lblReason.Visible = true;
                            txtReason.Visible = true;
                        }
                        else
                        {

                            lnk1daybyPass.Visible = true;
                        }
                    }
                    if (u.USER_Level_tint == 3)
                    {
                        btnBlockClient.Visible = true;
                    }
                }
                if (userRight == false)
                {
                    chkByPassCreditLimit.Enabled = false;
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        protected void lnk1daybyPass_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = false;
            bool flag = false;
            if (lblClientId.Text == "" || lblClientId.Text == "0")
            {
                lblMsg.Text = "Select Client from list.";
                txt_Client.Focus();
                lblMsg.Visible = true;
            }            
            else
            {
                if (chkByPassCreditLimit.Checked == true)
                {
                    dc.OneDayUnblockClientHistory_Update(Convert.ToInt32(lblClientId.Text), txt_Client.Text, Session["LoginUserName"].ToString(), txtReason.Text.Trim());
                    flag = true;
                }
                
                if (flag == true)
                {
                    lblMsg.Text = "Updated successfully.";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = false;
            bool flag = false;
            if (lblClientId.Text == "" || lblClientId.Text == "0")
            {
                lblMsg.Text = "Select Client from list.";
                txt_Client.Focus();
                lblMsg.Visible = true;
            }
            else if (lblRight.Text == "onedaybypasscrlimit" && chkByPassCreditLimit.Checked == true && txtReason.Text.Trim() == "")
            {
                lblMsg.Text = "Input Reason.";
                txtReason.Focus();
                lblMsg.Visible = true;
            }
            else
            {
                if (lblRight.Text == "onedaybypasscrlimit")
                {
                    if (chkByPassCreditLimit.Checked == true)
                    {
                        dc.OneDayUnblockClientHistory_Update(Convert.ToInt32(lblClientId.Text), txt_Client.Text, Session["LoginUserName"].ToString(), txtReason.Text.Trim());
                        flag = true;
                    }
                }
                else
                {
                    dc.Client_Update_CRLimit(Convert.ToInt32(lblClientId.Text), chkByPassCreditLimit.Checked);
                    flag = true;
                }
                if (flag == true)
                {
                    lblMsg.Text = "Updated successfully.";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                }
            }
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
            btnBlockClient.Text = "Block Client";
            if (txt_Client.Text != "" && ChkClientName(txt_Client.Text) == true)
            {
                lblClientId.Text = Request.Form[hfClientId.UniqueID];
            }
            txtReason.Text = "";
        }


        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            chkByPassCreditLimit.Checked = false;
            btnBlockClient.Text = "Block Client";
            foreach (var obj in query)
            {
                lblCrLimit.Text = "Limit : " + Convert.ToInt32(obj.CL_Limit_mny).ToString();
                lblBalance.Text = "Balance : " + Convert.ToDecimal(obj.CL_BalanceAmt_mny).ToString("0.00");
                if (Convert.ToBoolean(obj.CL_ByPassCRLimitChecking_bit) == true)
                {
                    chkByPassCreditLimit.Checked = true;
                }
                if (Convert.ToBoolean(obj.CL_BlockForPaymentStatus_bit) == true)
                {
                    btnBlockClient.Text = "Unblock Client";
                }
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "Please select client from list.";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
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

        protected void btnBlockClient_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = false;
            bool flag = false;
            if (lblClientId.Text == "" || lblClientId.Text == "0")
            {
                lblMsg.Text = "Select Client from list.";
                txt_Client.Focus();
                lblMsg.Visible = true;
            }
            else
            {
                if (btnBlockClient.Text == "Block Client")                                   
                    flag = true;
                else
                    flag = false;

                dc.Client_Update_BlockForPayment(Convert.ToInt32(lblClientId.Text), flag);

                if (btnBlockClient.Text == "Block Client")
                    btnBlockClient.Text = "Unblock Client";
                else
                    btnBlockClient.Text = "Block Client";

                lblMsg.Text = "Updated successfully.";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Visible = true;
                
            }
        }
    }
}