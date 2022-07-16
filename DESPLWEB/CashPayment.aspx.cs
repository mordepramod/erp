using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class CashPayment : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Cash / Bank Payment";

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
                        if (u.USER_Account_right_bit == true)
                        {
                            userRight = true;
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

                string strReq = "", strReq1 = "";
                strReq1 = Request.RawUrl;
                strReq = strReq1.Substring(strReq1.IndexOf('?') + 1);
                if (!strReq.Equals("") && strReq1.IndexOf('?') >= 0)
                {
                    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                    strReq = obj.Decrypt(strReq);
                    if (strReq.Contains("=") == false)
                    {
                        Session.Abandon();
                        Response.Redirect("Login.aspx");
                    }

                    string[] arrMsgs = strReq.Split('&');
                    string[] arrIndMsg;

                    arrIndMsg = arrMsgs[0].Split('=');
                    lblCashBankPaymentId.Text = arrIndMsg[1].ToString().Trim();
                    
                    LoadCashBankPaymentDetails();
                } 
                else
                {
                    grdDetails.DataSource = null;
                    grdDetails.DataBind();
                    AddRowforDetailGrid();
                    lblCashBankPaymentId.Text = "0";
                    txtVoucherDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    optCash.Checked = true;
                }
            }
        }
        private void LoadCashBankPaymentDetails()
        {
            int i = 0;

            var cashbank = dc.CashBankPayment_View(Convert.ToInt32(lblCashBankPaymentId.Text), "", null, null);
            foreach (var cb in cashbank)
            {
                txtVoucherNo.Text = cb.CASHBANKPAY_VoucherNo_var;
                txtVoucherDate.Text = Convert.ToDateTime(cb.CASHBANKPAY_VoucherDate_dt).ToString("dd/MM/yyyy"); 
                if (cb.CASHBANKPAY_PaymentStatus_var =="Cash")
                {
                    optCash.Checked = true;
                }
                else
                {
                    optBankVoucherType.Checked = true;
                    lblBankVoucherType.Visible = true;
                    ddlBankVoucherType.Visible = true;
                    ddlBankVoucherType.SelectedValue = cb.CASHBANKPAY_BankVoucherType_var; 
                }
                txtNarration.Text = cb.CASHBANKPAY_Narration_var;
                lblTotalAmount.Text = Convert.ToDecimal(cb.CASHBANKPAY_TotalAmount_num).ToString("0.00");  
                
                var cashbankpaymentDetail = dc.CashBankPaymentDetail_View(txtVoucherNo.Text);
                foreach (var cbd in cashbankpaymentDetail)
                {
                    AddRowforDetailGrid();
                    TextBox txtItemName = (TextBox)grdDetails.Rows[i].FindControl("txtItemName");
                    Label lblItemId = (Label)grdDetails.Rows[i].FindControl("lblItemId");
                    TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");                    
                    DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                    DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                    TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");

                    txtItemName.Text = cbd.CASHBANKPAYDETAIL_ItemName_var;
                    lblItemId.Text = "0";
                    txtLedgerName.Text = cbd.CASHBANKPAYDETAIL_LedgerName_var;                    
                    ddlCostCenter.SelectedItem.Text = cbd.CASHBANKPAYDETAILL_CostCenter_var;
                    ddlCategory.SelectedItem.Text = cbd.CASHBANKPAYDETAIL_Category_var;
                    txtAmount.Text = Convert.ToDecimal(cbd.CASHBANKPAYDETAIL_Amount_num).ToString("0.00");
                    
                    i++;
                }
                CalculateTotal();
            }
        }
        protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCostCenter = (DropDownList)e.Row.FindControl("ddlCostCenter");
                var costcenter = dc.TallyCostCenter_View();
                ddlCostCenter.DataSource = costcenter;
                ddlCostCenter.DataTextField = "TallyCostCenter_Description_var";
                ddlCostCenter.DataValueField = "TallyCostCenter_Id";
                ddlCostCenter.DataBind();
                ddlCostCenter.Items.Insert(0, new ListItem("---Select---", "0"));

                DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                var category = dc.TallyCategory_View();
                ddlCategory.DataSource = category;
                ddlCategory.DataTextField = "TallyCategory_Description_var";
                ddlCategory.DataValueField = "TallyCategory_Id";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }

        private void ClearAllControls()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;

            txtVoucherNo.Text = "";
            txtVoucherDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            grdDetails.DataSource = null;
            grdDetails.DataBind();
            txtNarration.Text = "";
            ViewState["DetailTable"] = null;
            AddRowforDetailGrid();
            lblTotalAmount.Text = "0.00";

            lblBankVoucherType.Visible = false;
            ddlBankVoucherType.Visible = false;
            ddlBankVoucherType.SelectedIndex = 0;

            lblMessage.Visible = false;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;

            lblCashBankPaymentId.Text = "0";

        }

        protected void txtLedgerName_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;

            TextBox txtLedgerName = (TextBox)grdDetails.Rows[rowindex].FindControl("txtLedgerName");
            Label lblLedgerId = (Label)grdDetails.Rows[rowindex].FindControl("lblLedgerId");

            if (ChkLedgerName(txtLedgerName.Text) == true)
            {
                if (txtLedgerName.Text != "")
                {
                    lblLedgerId.Text = Request.Form[hfLedgerId.UniqueID];
                }
                else
                {
                    lblLedgerId.Text = "0";
                }
            }
        }
        
        protected Boolean ChkLedgerName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            //searchHead = txtLedgerName.Text;
            Boolean valid = false;
            var query = dc.TallyLedger_View(searchHead);
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                //txtLedgerName.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Ledger is not in the list";
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
        public static List<string> GetLedgerName(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.TallyLedger_View(searchHead);
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("TallyLedger_Description_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.TallyLedger_Description_var, rowObj.TallyLedger_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.TallyLedger_View("");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.TallyLedger_Description_var, rowObj.TallyLedger_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> TallyLedger_Description_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TallyLedger_Description_var.Add(dt.Rows[i][0].ToString());
            }
            return TallyLedger_Description_var;

        }

        protected void txtItemName_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;

            TextBox txtItemName = (TextBox)grdDetails.Rows[rowindex].FindControl("txtItemName");
            Label lblItemId = (Label)grdDetails.Rows[rowindex].FindControl("lblItemId");

            if (ChkItemName(txtItemName.Text) == true)
            {
                if (txtItemName.Text != "")
                {
                    lblItemId.Text = Request.Form[hfItemId.UniqueID];
                }
                else
                {
                    lblItemId.Text = "";
                }
            }
        }

        protected Boolean ChkItemName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            //searchHead = txtItemName.Text;
            Boolean valid = false;
            var query = dc.Item_View(searchHead);
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                //txtItemName.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Item is not in the list";
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
        public static List<string> GetItemName(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Item_View(searchHead);
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("Item_Description_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.Item_Description_var, rowObj.Item_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Item_View("");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.Item_Description_var, rowObj.Item_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> Item_Description_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Item_Description_var.Add(dt.Rows[i][0].ToString());
            }
            return Item_Description_var;

        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
        }
        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("home.aspx");
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (txtVoucherNo.Text == "")
            {
                lblMsg.Text = "Enter Voucher No.";
                txtVoucherNo.Focus();
                valid = false;
            }
            else if (txtVoucherDate.Text == "")
            {
                lblMsg.Text = "Enter Voucher Date";
                txtVoucherDate.Focus();
                valid = false;
            }
            else if (optCash.Checked == false && optBankVoucherType.Checked == false)
            {
                lblMsg.Text = "Select Payment Status.";
                optCash.Focus();
                valid = false;
            }
            else if (optBankVoucherType.Checked == true && ddlBankVoucherType.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Bank Voucher Type.";
                optBankVoucherType.Focus();
                valid = false;
            }
            else if (grdDetails.Rows.Count == 0)
            {
                lblMsg.Text = "Details not available.";
                grdDetails.Focus();
                valid = false;
            }
            else if (txtNarration.Text == "")
            {
                lblMsg.Text = "Enter Narration";
                txtNarration.Focus();
                valid = false;
            }
            else
            {
                for (int i = 0; i < grdDetails.Rows.Count; i++)
                {
                    TextBox txtItemName = (TextBox)grdDetails.Rows[i].FindControl("txtItemName");
                    TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                    TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");

                    //if (txtItemName.Text == "")
                    //{
                    //    lblMsg.Text = "Select Item for row no. " + (i + 1);
                    //    txtItemName.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    if (txtLedgerName.Text == "")
                    {
                        lblMsg.Text = "Select Ledger for row no. " + (i + 1);
                        txtLedgerName.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtAmount.Text == "")
                    {
                        lblMsg.Text = "Enter Amount for row no. " + (i + 1);
                        txtAmount.Focus();
                        valid = false;
                        break;
                    }
                }
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

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                CalculateTotal();
                lblMessage.Visible = false;
                //check for duplicate voucher no.
                var cashpay = dc.CashBankPayment_View(0 , txtVoucherNo.Text, null, null).ToList();
                if (cashpay.Count() > 0 &&
                     (lblCashBankPaymentId.Text == "0" || (lblCashBankPaymentId.Text != "0" && lblCashBankPaymentId.Text != cashpay.FirstOrDefault().CASHBANKPAY_Id.ToString())))
                {
                    lblMessage.Text = "Duplicate Voucher No.";
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    DateTime voucherDate = DateTime.ParseExact(txtVoucherDate.Text, "dd/MM/yyyy", null);
                    string strPaymentStatus = "", strBankVoucherType = "";
                    if (optCash.Checked == true)
                        strPaymentStatus = "Cash";
                    else if (optBankVoucherType.Checked == true)
                    {
                        strPaymentStatus = "Bank Voucher Type";
                        strBankVoucherType = ddlBankVoucherType.SelectedValue;
                    }
                    dc.CashBankPayment_Update(Convert.ToInt32(lblCashBankPaymentId.Text), txtVoucherNo.Text, voucherDate, strPaymentStatus, strBankVoucherType, Convert.ToDecimal(lblTotalAmount.Text), txtNarration.Text);
                    dc.CashBankPaymentDetail_Update(txtVoucherNo.Text,"", "", "", "", 0, true);
                    for (int i = 0; i < grdDetails.Rows.Count; i++)
                    {
                        TextBox txtItemName = (TextBox)grdDetails.Rows[i].FindControl("txtItemName");
                        Label lblItemId = (Label)grdDetails.Rows[i].FindControl("lblItemId");
                        TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                        DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                        DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                        TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                        dc.CashBankPaymentDetail_Update(txtVoucherNo.Text, txtItemName.Text.Trim(), txtLedgerName.Text, ddlCostCenter.SelectedItem.Text, ddlCategory.SelectedItem.Text, Convert.ToDecimal(txtAmount.Text), false);
                        //if (lblItemId.Text == "")
                        //    dc.Item_Update(txtItemName.Text.Trim());
                    }
                    lblMessage.Text = "Updated Successfully";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;
                    lnkSave.Enabled = false;
                    lnkCalculate.Enabled = false;
                }
            }
        }

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        protected void CalculateTotal()
        {
            decimal totalAmount = 0;
            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                if (txtAmount.Text != "")
                {
                    totalAmount += Convert.ToDecimal(txtAmount.Text);
                }
            }
            lblTotalAmount.Text = totalAmount.ToString("0.00");
        }

        #region detail Grid
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowforDetailGrid();
        }

        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdDetails.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowforDetailGrid(gvr.RowIndex);
            }
        }
        protected void DeleteRowforDetailGrid(int rowIndex)
        {
            GetCurrentDataforDetailGrid();
            DataTable dt = ViewState["DetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["DetailTable"] = dt;
            grdDetails.DataSource = dt;
            grdDetails.DataBind();
            SetPreviousDataforDetailGrid();
        }

        protected void AddRowforDetailGrid()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["DetailTable"] != null)
            {
                GetCurrentDataforDetailGrid();
                dt = (DataTable)ViewState["DetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemId", typeof(string)));
                dt.Columns.Add(new DataColumn("LedgerName", typeof(string)));
                dt.Columns.Add(new DataColumn("CostCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("Category", typeof(string)));
                dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["ItemName"] = string.Empty;
            dr["ItemId"] = string.Empty;
            dr["LedgerName"] = string.Empty;
            dr["CostCenter"] = "---Select---";
            dr["Category"] = "---Select---";
            dr["Amount"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["DetailTable"] = dt;
            grdDetails.DataSource = dt;
            grdDetails.DataBind();
            SetPreviousDataforDetailGrid();
        }

        protected void GetCurrentDataforDetailGrid()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ItemName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ItemId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("LedgerName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CostCenter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Category", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Amount", typeof(string)));

            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox txtItemName = (TextBox)grdDetails.Rows[i].FindControl("txtItemName");
                Label lblItemId = (Label)grdDetails.Rows[i].FindControl("lblItemId");
                TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["ItemName"] = txtItemName.Text;
                drRow["ItemId"] = lblItemId.Text;
                drRow["LedgerName"] = txtLedgerName.Text;
                drRow["CostCenter"] = ddlCostCenter.SelectedItem.Text;
                drRow["Category"] = ddlCategory.SelectedItem.Text;
                drRow["Amount"] = txtAmount.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["DetailTable"] = dtTable;
        }
        protected void SetPreviousDataforDetailGrid()
        {
            DataTable dt = (DataTable)ViewState["DetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtItemName = (TextBox)grdDetails.Rows[i].FindControl("txtItemName");
                Label lblItemId = (Label)grdDetails.Rows[i].FindControl("lblItemId");
                TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");

                txtItemName.Text = dt.Rows[i]["ItemName"].ToString();
                lblItemId.Text = dt.Rows[i]["ItemId"].ToString();
                txtLedgerName.Text = dt.Rows[i]["LedgerName"].ToString();
                ddlCostCenter.SelectedItem.Text = dt.Rows[i]["CostCenter"].ToString();
                ddlCategory.SelectedItem.Text = dt.Rows[i]["Category"].ToString();
                txtAmount.Text = dt.Rows[i]["Amount"].ToString();
            }
        }

        #endregion

        protected void optBankVoucherType_CheckedChanged(object sender, EventArgs e)
        {
            ddlBankVoucherType.SelectedIndex = 0;
            if (optBankVoucherType.Checked == true)
            {
                lblBankVoucherType.Visible = true;
                ddlBankVoucherType.Visible = true;
            }
            else
            {
                lblBankVoucherType.Visible = false;
                ddlBankVoucherType.Visible = false;
            }
        }
        
    }
}