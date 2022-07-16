using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class VendorCashPayment : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Cash Payment";
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
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
                    lblCashPaymentId.Text = arrIndMsg[1].ToString().Trim();

                    LoadCashPaymentDetails();
                }
                else
                {
                    grdDetails.DataSource = null;
                    grdDetails.DataBind();
                    AddRowforDetailGrid();
                    lblCashPaymentId.Text = "0";
                    txtVoucherDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    optCash.Checked = true;
                }
                
            }
        }
        private void LoadCashPaymentDetails()
        {
            int i = 0;
            //var cashPayment = dc.CashPayment_View(Convert.ToInt32(lblCashPaymentId.Text), "", null, null, "");
            var cashPayment = dc.CashPayment_View(Convert.ToInt32(lblCashPaymentId.Text), "", null, null);
            foreach (var cp in cashPayment)
            {
                txtVoucherNo.Text = cp.CASHPAY_VoucherNo_var;
                txtVoucherDate.Text = Convert.ToDateTime(cp.CASHPAY_VoucherDate_dt).ToString("dd/MM/yyyy");
                //if (cp.CASHPAY_PaymentStatus_var == "Cash")
                //{
                //    optCash.Checked = true;
                //}
                //else
                //{
                    optBankVoucherType.Checked = true;
                    lblBankVoucherType.Visible = true;
                    ddlBankVoucherType.Visible = true;
                    //ddlBankVoucherType.SelectedValue = cp.CASHPAY_BankVoucherType_var;
                //}
                txtNarration.Text = cp.CASHPAY_Narration_var;
                lblTotalAmount.Text = Convert.ToDecimal(cp.CASHPAY_TotalAmount_num).ToString("0.00");
                txtVendor.Text = cp.VEND_FirmName_var;
                lblVendorId.Text = cp.CASHPAY_VEND_Id.ToString(); 

                var cashpaymentDetail = dc.CashPaymentDetail_View(txtVoucherNo.Text);
                foreach (var cpd in cashpaymentDetail)
                {
                    AddRowforDetailGrid();
                    TextBox txtInvoiceNo = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceNo");
                    TextBox txtInvoiceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceAmt");
                    TextBox txtBalanceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtBalanceAmt");
                    TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                    DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                    DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                    TextBox txtDebitAmount = (TextBox)grdDetails.Rows[i].FindControl("txtDebitAmount");
                    TextBox txtCreditAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCreditAmount");

                    txtInvoiceNo.Text = cpd.CASHPAYDETAIL_InvoiceNo_var;
                    txtInvoiceAmt.Text = Convert.ToDecimal(cpd.BILLBOOK_NetPayableAmount_num).ToString("0.00");
                    txtBalanceAmt.Text = Convert.ToDecimal(cpd.BalanceAmount).ToString("0.00");
                    txtLedgerName.Text = cpd.CASHPAYDETAIL_LedgerName_var;
                    ddlCostCenter.SelectedItem.Text = cpd.CASHPAYDETAILL_CostCenter_var;
                    ddlCategory.SelectedItem.Text = cpd.CASHPAYDETAIL_Category_var;
                    if (cpd.CASHPAYDETAIL_DebitAmount_num > 0 )
                        txtDebitAmount.Text = Convert.ToDecimal(cpd.CASHPAYDETAIL_DebitAmount_num).ToString("0.00");
                    if (cpd.CASHPAYDETAIL_CreditAmount_num > 0)
                        txtCreditAmount.Text = Convert.ToDecimal(cpd.CASHPAYDETAIL_CreditAmount_num).ToString("0.00");
                    i++;
                }
            }
            LoadInvoiceDetails();
            CalculateTotal();
        }

        public void LoadInvoiceDetails()
        {
            int i = grdDetails.Rows.Count;
            
            //list of all pending bills of selected vendor
            if (Convert.ToInt32(lblVendorId.Text) > 0)
            {
                var pendingBill = dc.BillBooking_View_PendingInvoice(Convert.ToInt32(lblVendorId.Text));
                foreach (var bill in pendingBill)
                {
                    bool addFlag = true;
                    foreach (GridViewRow row in grdDetails.Rows)
                    {
                        TextBox vbillno = (TextBox)row.FindControl("txtInvoiceNo");
                        if (vbillno.Text == bill.BILLBOOK_SupplierInvoiceNo_var)
                        {
                            addFlag = false;
                            break;
                        }
                    }
                    if (addFlag == true)
                    {
                        AddRowforDetailGrid();
                        TextBox txtInvoiceNo = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceNo");
                        TextBox txtInvoiceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceAmt");
                        TextBox txtBalanceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtBalanceAmt");
                        TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                        DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                        DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                        TextBox txtDebitAmount = (TextBox)grdDetails.Rows[i].FindControl("txtDebitAmount");
                        TextBox txtCreditAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCreditAmount");

                        txtInvoiceNo.Text = bill.BILLBOOK_SupplierInvoiceNo_var;
                        txtInvoiceAmt.Text = Convert.ToDecimal(bill.BILLBOOK_NetPayableAmount_num).ToString("0.00");
                        if (bill.BalanceAmount != null)
                        {
                            txtBalanceAmt.Text = Convert.ToDecimal(bill.BalanceAmount).ToString("0.00");
                        }
                        else
                        {
                            txtBalanceAmt.Text = Convert.ToDecimal(bill.BILLBOOK_NetPayableAmount_num).ToString("0.00");
                        }
                        i++;
                    }
                }
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

            lblMessage.Visible = false;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;
            lblCashPaymentId.Text = "0";

            txtVendor.Text = "";
            lblVendorId.Text = "0"; 
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

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
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
            else if (txtVendor.Text == "" || lblVendorId.Text == "0" || lblVendorId.Text == "")
            {
                lblMsg.Text = "Select Vendor";
                txtVendor.Focus();
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
                    TextBox txtInvoiceNo = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceNo");
                    TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                    TextBox txtDebitAmount = (TextBox)grdDetails.Rows[i].FindControl("txtDebitAmount");
                    TextBox txtCreditAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCreditAmount");

                    if (txtInvoiceNo.Text == "")
                    {
                        lblMsg.Text = "Select Invoice No. for row no. " + (i + 1);
                        txtInvoiceNo.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtLedgerName.Text == "")
                    {
                        lblMsg.Text = "Select Ledger for row no. " + (i + 1);
                        txtLedgerName.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDebitAmount.Text == "" && txtCreditAmount.Text == "")
                    {
                        lblMsg.Text = "Enter Debit or Credit Amount for row no. " + (i + 1);
                        txtDebitAmount.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDebitAmount.Text != "" && txtCreditAmount.Text != "")
                    {
                        lblMsg.Text = "Enter any one of Debit or Credit Amount for row no. " + (i + 1);
                        txtDebitAmount.Focus();
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
                //var cashpay = dc.CashPayment_View(0, txtVoucherNo.Text, null, null,"").ToList();
                var cashpay = dc.CashPayment_View(0, txtVoucherNo.Text, null, null).ToList();
                if (cashpay.Count() > 0 &&
                    (lblCashPaymentId.Text == "0" || (lblCashPaymentId.Text != "0" && lblCashPaymentId.Text != cashpay.FirstOrDefault().CASHPAY_Id.ToString())))
                {                    
                    lblMessage.Text = "Duplicate Voucher No.";
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    DateTime voucherDate = DateTime.ParseExact(txtVoucherDate.Text, "dd/MM/yyyy", null);
                    decimal debitAmount = 0, creditAmount = 0;
                    string strPaymentStatus = "", strBankVoucherType = "";
                    if (optCash.Checked == true)
                        strPaymentStatus = "Cash";
                    else if (optBankVoucherType.Checked == true)
                    {
                        strPaymentStatus = "Bank Voucher Type";
                        strBankVoucherType = ddlBankVoucherType.SelectedValue;
                    }
                    //dc.CashPayment_Update(Convert.ToInt32(lblCashPaymentId.Text), txtVoucherNo.Text, voucherDate, strPaymentStatus, strBankVoucherType, Convert.ToDecimal(lblTotalAmount.Text), txtNarration.Text, Convert.ToInt32(lblVendorId.Text) );
                    dc.CashPayment_Update(Convert.ToInt32(lblCashPaymentId.Text), txtVoucherNo.Text, voucherDate, Convert.ToDecimal(lblTotalAmount.Text), txtNarration.Text, Convert.ToInt32(lblVendorId.Text));
                    dc.CashPaymentDetail_Update(txtVoucherNo.Text, "", "", "", "", 0, 0, 0, true);
                    for (int i = 0; i < grdDetails.Rows.Count; i++)
                    {
                        TextBox txtInvoiceNo = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceNo");
                        TextBox txtInvoiceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceAmt");
                        TextBox txtBalanceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtBalanceAmt");
                        TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                        DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                        DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                        TextBox txtDebitAmount = (TextBox)grdDetails.Rows[i].FindControl("txtDebitAmount");
                        TextBox txtCreditAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCreditAmount");

                        debitAmount = 0; creditAmount = 0;
                        if (txtDebitAmount.Text != "")
                            debitAmount = Convert.ToDecimal(txtDebitAmount.Text);
                        if (txtCreditAmount.Text != "")
                            creditAmount = Convert.ToDecimal(txtCreditAmount.Text);

                        dc.CashPaymentDetail_Update(txtVoucherNo.Text, txtInvoiceNo.Text, txtLedgerName.Text, ddlCostCenter.SelectedItem.Text, ddlCategory.SelectedItem.Text, debitAmount, creditAmount, Convert.ToInt32(lblVendorId.Text), false);
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
            decimal totalDebitAmount = 0, totalCreditAmount = 0, totalAmount = 0;
            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox txtDebitAmount = (TextBox)grdDetails.Rows[i].FindControl("txtDebitAmount");
                TextBox txtCreditAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCreditAmount");

                if (txtDebitAmount.Text != "")
                {
                    totalDebitAmount += Convert.ToDecimal(txtDebitAmount.Text);
                    totalAmount += Convert.ToDecimal(txtDebitAmount.Text);
                }
                if (txtCreditAmount.Text != "")
                {
                    totalCreditAmount += Convert.ToDecimal(txtCreditAmount.Text);
                    totalAmount -= Convert.ToDecimal(txtCreditAmount.Text);
                }
            }
            grdDetails.ShowFooter = true;
            grdDetails.FooterRow.Cells[6].Text = totalDebitAmount.ToString("0.00");
            grdDetails.FooterRow.Cells[6].Font.Bold = true;
            grdDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;

            grdDetails.FooterRow.Cells[7].Text = totalCreditAmount.ToString("0.00");
            grdDetails.FooterRow.Cells[7].Font.Bold = true;
            grdDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                        
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
                dt.Columns.Add(new DataColumn("txtInvoiceNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtInvoiceAmt", typeof(string)));
                dt.Columns.Add(new DataColumn("txtBalanceAmt", typeof(string)));
                dt.Columns.Add(new DataColumn("LedgerName", typeof(string)));
                dt.Columns.Add(new DataColumn("CostCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("Category", typeof(string)));
                dt.Columns.Add(new DataColumn("DebitAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("CreditAmount", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtInvoiceNo"] = string.Empty;
            dr["txtInvoiceAmt"] = string.Empty;
            dr["txtBalanceAmt"] = string.Empty;
            dr["LedgerName"] = string.Empty;
            dr["CostCenter"] = "---Select---";
            dr["Category"] = "---Select---";
            dr["DebitAmount"] = string.Empty;
            dr["CreditAmount"] = string.Empty;

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
            dtTable.Columns.Add(new DataColumn("txtInvoiceNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtInvoiceAmt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtBalanceAmt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("LedgerName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CostCenter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Category", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DebitAmount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CreditAmount", typeof(string)));

            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox txtInvoiceNo = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceNo");
                TextBox txtInvoiceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceAmt");
                TextBox txtBalanceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtBalanceAmt");
                TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                TextBox txtDebitAmount = (TextBox)grdDetails.Rows[i].FindControl("txtDebitAmount");
                TextBox txtCreditAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCreditAmount");
                
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtInvoiceNo"] = txtInvoiceNo.Text;
                drRow["txtInvoiceAmt"] = txtInvoiceAmt.Text;
                drRow["txtBalanceAmt"] = txtBalanceAmt.Text;
                drRow["LedgerName"] = txtLedgerName.Text;
                drRow["CostCenter"] = ddlCostCenter.SelectedItem.Text;
                drRow["Category"] = ddlCategory.SelectedItem.Text;
                drRow["DebitAmount"] = txtDebitAmount.Text;
                drRow["CreditAmount"] = txtCreditAmount.Text;
                
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
                TextBox txtInvoiceNo = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceNo");
                TextBox txtInvoiceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtInvoiceAmt");
                TextBox txtBalanceAmt = (TextBox)grdDetails.Rows[i].FindControl("txtBalanceAmt");
                TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                TextBox txtDebitAmount = (TextBox)grdDetails.Rows[i].FindControl("txtDebitAmount");
                TextBox txtCreditAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCreditAmount");

                txtInvoiceNo.Text = dt.Rows[i]["txtInvoiceNo"].ToString();
                txtInvoiceAmt.Text = dt.Rows[i]["txtInvoiceAmt"].ToString();
                txtBalanceAmt.Text = dt.Rows[i]["txtBalanceAmt"].ToString();
                txtLedgerName.Text = dt.Rows[i]["LedgerName"].ToString();
                ddlCostCenter.SelectedItem.Text = dt.Rows[i]["CostCenter"].ToString();
                ddlCategory.SelectedItem.Text = dt.Rows[i]["Category"].ToString();
                txtDebitAmount.Text = dt.Rows[i]["DebitAmount"].ToString();
                txtCreditAmount.Text = dt.Rows[i]["CreditAmount"].ToString();
            }

        }

        #endregion

        protected void txtVendor_TextChanged(object sender, EventArgs e)
        {
            if (ChkVendorName(txtVendor.Text) == true)
            {
                if (txtVendor.Text != "")
                {
                    lblVendorId.Text = Request.Form[hfVendorId.UniqueID];
                }
                else
                {
                    lblVendorId.Text = "0";
                }
            }
            grdDetails.DataSource = null;
            grdDetails.DataBind();
            LoadInvoiceDetails();
        }
        
        protected Boolean ChkVendorName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txtVendor.Text;
            Boolean valid = false;
            var query = dc.Vendor_View(0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txtVendor.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Vendor is not in the list";
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
        public static List<string> GetVendorName(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Vendor_View(0, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("VEND_FirmName_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.VEND_FirmName_var, rowObj.VEND_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Vendor_View(0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.VEND_FirmName_var, rowObj.VEND_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> VEND_FirmName_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VEND_FirmName_var.Add(dt.Rows[i][0].ToString());
            }
            return VEND_FirmName_var;

        }
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