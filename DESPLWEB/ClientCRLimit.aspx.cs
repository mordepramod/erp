using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ClientCRLimit : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                //lblheading.Text = "Client Credit Limit Setting";
                lblheading.Text = "Site Setting";

                bool userRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    ////  if (u.USER_CRLimitApprove_right_bit == true)
                    if (u.USER_SuperAccount_right_bit == true)
                    {
                        userRight = true;
                        lblRight.Text = "bypasscrlimit";
                        optBypassCreditLimit.Enabled = true;
                    }
                    // if (u.USER_SuperAdmin_right_bit == true || u.USER_Name_var.ToLower().Contains("sagvekar"))
                    if (u.User_sitewise_rate_bit == true )
                    {
                        userRight = true;
                        lblRight.Text += "monthlybilling";
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
                    if (lblRight.Text.Contains("bypasscrlimit") == true)
                        optBypassCreditLimit.Enabled = true;
                    else
                        optBypassCreditLimit.Enabled = false;
                    if (lblRight.Text.Contains("monthlybilling") == true)
                        optMonthlyBilling.Enabled = true;
                    else
                        optMonthlyBilling.Enabled = false;
                    if (optBypassCreditLimit.Enabled == true)
                        optBypassCreditLimit.Checked = true;
                    else if (optMonthlyBilling.Enabled == true)
                        optMonthlyBilling.Checked = true;
                }
            }
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
            if (txt_Client.Text != "" && ChkClientName(txt_Client.Text) == true)
            {
                lblClientId.Text = Request.Form[hfClientId.UniqueID];
            }

            if (lblClientId.Text != "0")
            {
                LoadSiteList(lblClientId.Text);
            }
            else
            {
                grdCredit.DataSource = null;
                grdCredit.DataBind();
            }
        }

        private void LoadSiteList(string CL_Id)
        {
            var site = dc.Site_View(0, Convert.ToInt32(CL_Id), 0, "").ToList();
            grdCredit.DataSource = site;
            grdCredit.DataBind();
            int i = 0;
            foreach (var cs in site)
            {
                CheckBox chkbox = (CheckBox)grdCredit.Rows[i].Cells[0].FindControl("cbxSelect");
                if (optBypassCreditLimit.Checked == true && cs.SITE_CRBypass_bit == true)
                {
                    chkbox.Checked = true;
                }
                else if (optMonthlyBilling.Checked == true && cs.SITE_MonthlyBillingStatus_bit == true)
                {
                    chkbox.Checked = true;
                }
                else if (OptMetro.Checked == true && cs.SITE_MetroStatus_bit  == true)
                {
                    chkbox.Checked = true;
                }
                i++;
            }
            int cnt = 0;
            for (int j = 0; j < grdCredit.Rows.Count; j++)
            {
                CheckBox chkbox = (CheckBox)grdCredit.Rows[j].Cells[0].FindControl("cbxSelect");
                if (chkbox.Checked)
                    cnt++;
            }

            if (grdCredit.Rows.Count > 0)
            {
                CheckBox ChkBoxHeader = (CheckBox)grdCredit.HeaderRow.FindControl("cbxSelectAll");

                if (cnt == grdCredit.Rows.Count)
                    ChkBoxHeader.Checked = true;
                else
                    ChkBoxHeader.Checked = false;
            }
        }

        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                txtCrLimit.Text = Convert.ToInt32(obj.CL_Limit_mny).ToString();
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

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = false; bool flag = false;
            if (lblClientId.Text == "" || lblClientId.Text == "0")
            {
                lblMsg.Text = "Select Client from list.";
                txt_Client.Focus();
            }
            else if (optBypassCreditLimit.Checked == true && lblCrLimit.Text == "")
            {
                lblMsg.Text = "Input Credit Limit";
                txtCrLimit.Focus();
            }
            else
            {
                if (optBypassCreditLimit.Checked == true)
                {
                //    dc.Client_Update_CRLimit(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(txtCrLimit.Text));
                }
                for (int i = 0; i < grdCredit.Rows.Count; i++)
                {
                    CheckBox chkbox = (CheckBox)grdCredit.Rows[i].Cells[0].FindControl("cbxSelect");
                    Label lblSiteId = (Label)grdCredit.Rows[i].Cells[1].FindControl("lblSiteId");

                    //if (optBypassCreditLimit.Checked == true)
                    //{
                    //    if (chkbox.Checked == true)
                    //    {
                    //        dc.Site_Update_CRLimit(Convert.ToInt32(lblSiteId.Text), true); flag = true;
                    //    }
                    //    else
                    //    { dc.Site_Update_CRLimit(Convert.ToInt32(lblSiteId.Text), false); flag = true; }
                    //}
                    if (optMonthlyBilling.Checked == true)
                    {
                        if (chkbox.Checked == true)
                        { dc.Site_Update_MonthlyBillingStatus(Convert.ToInt32(lblSiteId.Text), true); flag = true; }
                        else
                        { dc.Site_Update_MonthlyBillingStatus(Convert.ToInt32(lblSiteId.Text), false); flag = true; }
                    }
                    if (OptMetro.Checked == true)
                    {
                        if (chkbox.Checked == true)
                        { dc.Site_Update_MetroStatus(Convert.ToInt32(lblSiteId.Text), true); flag = true; }
                        else
                        { dc.Site_Update_MetroStatus(Convert.ToInt32(lblSiteId.Text), false); flag = true; }
                    }
                }
                if (flag)
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

        protected void grdCredit_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox cbxSelect = (CheckBox)e.Row.Cells[2].FindControl("cbxSelect");
                CheckBox cbxSelectAll = (CheckBox)this.grdCredit.HeaderRow.FindControl("cbxSelectAll");
                cbxSelect.Attributes["onclick"] = string.Format
                                                       (
                                                          "javascript:ChildClick(this,'{0}');",
                                                          cbxSelectAll.ClientID
                                                       );
            }
        }

        protected void optBypassCreditLimit_CheckedChanged(object sender, EventArgs e)
        {
            txt_Client.Text = "";
            lblClientId.Text = "0";
            txtCrLimit.Text = "";
            lblCrLimit.Visible = true;
            txtCrLimit.Visible = true;
            grdCredit.DataSource = null;
            grdCredit.DataBind();
            optMonthlyBilling.Checked = false;
            OptMetro.Checked = false;
        }

        protected void optMonthlyBilling_CheckedChanged(object sender, EventArgs e)
        {
            txt_Client.Text = "";
            lblClientId.Text = "0";
            txtCrLimit.Text = "";
            lblCrLimit.Visible = false;
            txtCrLimit.Visible = false;
            grdCredit.DataSource = null;
            grdCredit.DataBind();
            optBypassCreditLimit.Checked = false;
            OptMetro.Checked = false;
        }
        protected void optMetro_CheckedChanged(object sender, EventArgs e)
        {
            txt_Client.Text = "";
            lblClientId.Text = "0";
            txtCrLimit.Text = "";
            lblCrLimit.Visible = false;
            txtCrLimit.Visible = false;
            grdCredit.DataSource = null;
            grdCredit.DataBind();
            optBypassCreditLimit.Checked = false;
            optMonthlyBilling.Checked = false;
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
    }
}