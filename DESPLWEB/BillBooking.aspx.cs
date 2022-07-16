using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class BillBooking : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Bill Booking";

                //bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                //else
                //{
                //    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                //    foreach (var u in user)
                //    {
                //        if (u.USER_Account_right_bit == true)
                //        {
                //            userRight = true;
                //        }
                //    }
                //}
                //userRight = true;
                //if (userRight == false)
                //{
                //    pnlContent.Visible = false;
                //    lblAccess.Visible = true;
                //    lblAccess.Text = "Access is Denied.. ";
                //}

                //Query string decrypt
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
                    txtBookingNo.Text = arrIndMsg[1].ToString().Trim();
                    lblBookingId.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblVendorId.Text = arrIndMsg[1].ToString().Trim();

                    LoadBillBookingDetails();
                }
                else
                {
                    getCurrentBookingId();
                    grdDetails.DataSource = null;
                    grdDetails.DataBind();
                    AddRowforDetailGrid();
                    lblBookingId.Text = "0";
                    //LoadCostCenterList();
                    //LoadCategoryList();
                    txtBookDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    txtInvoiceDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }               

            }
        }

        private void LoadBillBookingDetails()
        {
            string BillNumber = txtBookingNo.Text;
            int i = 0;

            var bill = dc.BillBooking_View(Convert.ToInt32(BillNumber), null, null, "", false);
            foreach (var b in bill)
            {
                txtVendor.Text = b.VEND_FirmName_var.ToString();
                txtNarration.Text = b.BILLBOOK_Narration_var.ToString();
                txtSupplierInvoiceNo.Text = b.BILLBOOK_SupplierInvoiceNo_var.ToString();
                txtComment.Text = b.BILLBOOK_Comment_var.ToString();
                txtBookDate.Text = Convert.ToDateTime(b.BILLBOOK_Date_dt).ToString("dd/MM/yyyy");
                txtInvoiceDate.Text = Convert.ToDateTime(b.BILLBOOK_InvoiceDate_dt).ToString("dd/MM/yyyy");
                if (b.BILLBOOK_CompTaxPayer_bit == true)
                    ddlCompTaxPayer.SelectedValue = "Yes";
                else
                    ddlCompTaxPayer.SelectedValue = "No";
                ddlBookingType.SelectedValue = b.BILLBOOK_Type_var.ToString();
                ddlStatus.SelectedValue = Convert.ToInt32(b.BILLBOOK_Status_bit).ToString();

                var billDetail = dc.BillBookingDetail_View(Convert.ToInt32(BillNumber));
                foreach (var bd in billDetail)
                {
                    AddRowforDetailGrid();
                    TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                    DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                    DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                    TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                    TextBox txtHSNcode = (TextBox)grdDetails.Rows[i].FindControl("txtHSNcode");
                    TextBox txtSACcode = (TextBox)grdDetails.Rows[i].FindControl("txtSACcode");
                    DropDownList ddlType = (DropDownList)grdDetails.Rows[i].FindControl("ddlType");
                    DropDownList ddlCgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlCgstPercent");
                    DropDownList ddlSgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlSgstPercent");
                    DropDownList ddlIgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlIgstPercent");
                    TextBox txtCgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCgstAmount");
                    TextBox txtSgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtSgstAmount");
                    TextBox txtIgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtIgstAmount");
                    DropDownList ddlDrCr = (DropDownList)grdDetails.Rows[i].FindControl("ddlDrCr");
                    TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                    DropDownList ddlUqc = (DropDownList)grdDetails.Rows[i].FindControl("ddlUqc");
                    TextBox txtQty = (TextBox)grdDetails.Rows[i].FindControl("txtQty");

                    txtLedgerName.Text = bd.BOOKDETAIL_LedgerName_var.ToString();
                    ddlCostCenter.SelectedItem.Text = bd.BOOKDETAIL_CostCenter_var.ToString();
                    ddlCategory.SelectedItem.Text = bd.BOOKDETAIL_Category_var.ToString();
                    txtHSNcode.Text = bd.BOOKDETAIL_HSNcode_var.ToString();
                    txtSACcode.Text = bd.BOOKDETAIL_SACcode_var.ToString();
                    ddlType.Text = bd.BOOKDETAIL_Type_var.ToString();
                    if (bd.BOOKDETAIL_CgstPercent_num != null)
                        ddlCgstPercent.SelectedValue = Convert.ToDecimal(bd.BOOKDETAIL_CgstPercent_num).ToString("0.##");
                    if (bd.BOOKDETAIL_SgstPercent_num != null)
                        ddlSgstPercent.SelectedValue = Convert.ToDecimal(bd.BOOKDETAIL_SgstPercent_num).ToString("0.##");
                    if (bd.BOOKDETAIL_IgstPercent_num != null)
                        ddlIgstPercent.SelectedValue = Convert.ToDecimal(bd.BOOKDETAIL_IgstPercent_num).ToString("0.##");

                    if (bd.BOOKDETAIL_CgstAmount_num != null)
                        txtCgstAmount.Text = Convert.ToDecimal(bd.BOOKDETAIL_CgstAmount_num).ToString();
                    if (bd.BOOKDETAIL_SgstAmount_num != null)
                        txtSgstAmount.Text = Convert.ToDecimal(bd.BOOKDETAIL_SgstAmount_num).ToString();
                    if (bd.BOOKDETAIL_IgstAmount_num != null)
                        txtIgstAmount.Text = Convert.ToDecimal(bd.BOOKDETAIL_IgstAmount_num).ToString();

                    if (bd.BOOKDETAIL_Qty_num != null)
                        txtQty.Text = Convert.ToInt32(bd.BOOKDETAIL_Qty_num).ToString();
                    txtAmount.Text = Convert.ToDecimal(bd.BOOKDETAIL_Amount_num).ToString("0.00");
                    txtDescription.Text = bd.BOOKDETAIL_Description_var.ToString();

                    if (bd.BOOKDETAIL_DrCr_var != null)
                        ddlDrCr.SelectedValue = bd.BOOKDETAIL_DrCr_var.ToString();
                    if (bd.BOOKDETAIL_UQC_var != null)
                        ddlUqc.SelectedValue = bd.BOOKDETAIL_UQC_var.ToString();

                    i++;
                }
                CalculateTotal();
            }
        }

        private void getCurrentBookingId()
        {
            var billb = dc.BillBooking_View(0, null, null, "", false).ToList();
            if (billb.Count == 0)
            {
                txtBookingNo.Text = "1";
            }
            else
            {
                txtBookingNo.Text = billb.FirstOrDefault().BILLBOOK_Id.ToString();
                txtBookingNo.Text = (Convert.ToInt32(txtBookingNo.Text) + 1).ToString();
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

            txtBookDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtSupplierInvoiceNo.Text = "";
            txtInvoiceDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlBookingType.SelectedValue = "---Select---";
            lblVendorId.Text = "0";
            txtVendor.Text = "";
            lblTotalNetPaybleAmount.Text = "0.00";
            txtNarration.Text = "";
            txtComment.Text = "";
            ddlCompTaxPayer.SelectedIndex = 0;
            grdDetails.DataSource = null;
            grdDetails.DataBind();
            ViewState["DetailTable"] = null;
            AddRowforDetailGrid();

            lblMessage.Visible = false;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;

            getCurrentBookingId();
        }

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

        protected void txtLedgerName_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;

            TextBox txtLedgerName = (TextBox)grdDetails.Rows[rowindex].FindControl("txtLedgerName");
            Label lblLedgerId = (Label)grdDetails.Rows[rowindex].FindControl("lblLedgerId");
            //HiddenField hfLedgerId = (HiddenField)grdDetails.Rows[rowindex].FindControl("hfLedgerId");

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

            if (txtBookingNo.Text == "")
            {
                lblMsg.Text = "Enter Booking No.";
                txtBookingNo.Focus();
                valid = false;
            }
            else if (txtBookDate.Text == "")
            {
                lblMsg.Text = "Enter Book Date";
                txtBookDate.Focus();
                valid = false;
            }
            else if (txtSupplierInvoiceNo.Text == "")
            {
                lblMsg.Text = "Enter Supplier Invoice No.";
                txtSupplierInvoiceNo.Focus();
                valid = false;
            }
            else if (txtInvoiceDate.Text == "")
            {
                lblMsg.Text = "Enter Invoice Date";
                txtInvoiceDate.Focus();
                valid = false;
            }
            else if (txtVendor.Text == "" || lblVendorId.Text == "0" || lblVendorId.Text == "")
            {
                lblMsg.Text = "Select Vendor";
                txtVendor.Focus();
                valid = false;
            }
            else if (ddlBookingType.SelectedValue == "---Select---")
            {
                lblMsg.Text = "Select Booking Type";
                ddlBookingType.Focus();
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
            else if (txtComment.Text == "")
            {
                lblMsg.Text = "Enter Comment";
                txtComment.Focus();
                valid = false;
            }
            else
            {
                for (int i = 0; i < grdDetails.Rows.Count; i++)
                {
                    TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                    TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                    DropDownList ddlCgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlCgstPercent");
                    DropDownList ddlSgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlSgstPercent");
                    DropDownList ddlIgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlIgstPercent");
                    TextBox txtCgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCgstAmount");
                    TextBox txtSgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtSgstAmount");
                    TextBox txtIgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtIgstAmount");
                    DropDownList ddlDrCr = (DropDownList)grdDetails.Rows[i].FindControl("ddlDrCr");
                    //TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                    //DropDownList ddlUqc = (DropDownList)grdDetails.Rows[i].FindControl("ddlUqc");
                    //TextBox txtQty = (TextBox)grdDetails.Rows[i].FindControl("txtQty");

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
                    //else if ((ddlCgstPercent.SelectedItem.Text != "---Select---" && ddlSgstPercent.SelectedItem.Text == "---Select---")
                    //    || (ddlCgstPercent.SelectedItem.Text == "---Select---" && ddlSgstPercent.SelectedItem.Text != "---Select---")
                    //    || (ddlCgstPercent.SelectedItem.Text != "---Select---" && ddlIgstPercent.SelectedItem.Text != "---Select---")
                    //    || (ddlSgstPercent.SelectedItem.Text != "---Select---" && ddlIgstPercent.SelectedItem.Text != "---Select---"))
                    //{
                    //    lblMsg.Text = "Enter CGST % And SGST %  or IGST % for row no. " + (i + 1);
                    //    ddlCgstPercent.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    //else if ((txtCgstAmount.Text != "" && txtSgstAmount.Text == "")
                    //    || (txtCgstAmount.Text == "" && txtSgstAmount.Text != "")
                    //    || (txtCgstAmount.Text != "" && txtIgstAmount.Text != "")
                    //    || (txtSgstAmount.Text != "" && txtIgstAmount.Text != ""))
                    //{
                    //    lblMsg.Text = "Enter CGST Amount And SGST Amount  or IGST Amount for row no. " + (i + 1);
                    //    txtCgstAmount.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    else if (ddlDrCr.SelectedValue == "---Select---")
                    {
                        lblMsg.Text = "Select Dr/Cr for row no. " + (i + 1);
                        ddlDrCr.Focus();
                        valid = false;
                        break;
                    }
                    //else if (txtDescription.Text == "")
                    //{
                    //    lblMsg.Text = "Enter Description for row no. " + (i + 1);
                    //    txtDescription.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    //else if (ddlUqc.SelectedValue == "---Select---")
                    //{
                    //    lblMsg.Text = "Select UQC unit. " + (i + 1);
                    //    ddlUqc.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    //else if (txtQty.Text == "")
                    //{
                    //    lblMsg.Text = "Enter Quantity for row no. " + (i + 1);
                    //    txtQty.Focus();
                    //    valid = false;
                    //    break;
                    //}

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
                int bookingId = 0, bookUpdatedId = 0;
                if (lblBookingId.Text != "0" && lblBookingId.Text != "")
                    bookUpdatedId = Convert.ToInt32(lblBookingId.Text);
                lblMessage.Visible = false;
                DateTime BookDate = DateTime.ParseExact(txtBookDate.Text, "dd/MM/yyyy", null);
                DateTime InvoiceDate = DateTime.ParseExact(txtInvoiceDate.Text, "dd/MM/yyyy", null);
                string strType = "";
                decimal CgstPercent = 0, SgstPercent = 0, IgstPercent = 0, CgstAmount = 0, SgstAmount = 0, IgstAmount = 0, Qty = 0;

                //delete billbooking details
                dc.BillBookingDetail_Update(bookUpdatedId, "", "", "", 0, "", "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, true);

                bookingId = dc.BillBooking_Update(BookDate, txtSupplierInvoiceNo.Text, InvoiceDate, ddlBookingType.SelectedValue, Convert.ToInt32(lblVendorId.Text), Convert.ToDecimal(lblTotalNetPaybleAmount.Text), txtNarration.Text, txtComment.Text, Convert.ToBoolean(Convert.ToInt32(ddlCompTaxPayer.SelectedValue)), bookUpdatedId, Convert.ToBoolean(Convert.ToByte(ddlStatus.SelectedValue)));
                if (bookingId == 0)
                    bookingId = bookUpdatedId;
                for (int i = 0; i < grdDetails.Rows.Count; i++)
                {
                    TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                    DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                    DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                    TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                    TextBox txtHSNcode = (TextBox)grdDetails.Rows[i].FindControl("txtHSNcode");
                    TextBox txtSACcode = (TextBox)grdDetails.Rows[i].FindControl("txtSACcode");
                    DropDownList ddlType = (DropDownList)grdDetails.Rows[i].FindControl("ddlType");
                    DropDownList ddlCgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlCgstPercent");
                    DropDownList ddlSgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlSgstPercent");
                    DropDownList ddlIgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlIgstPercent");
                    TextBox txtCgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCgstAmount");
                    TextBox txtSgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtSgstAmount");
                    TextBox txtIgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtIgstAmount");
                    DropDownList ddlDrCr = (DropDownList)grdDetails.Rows[i].FindControl("ddlDrCr");
                    TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                    DropDownList ddlUqc = (DropDownList)grdDetails.Rows[i].FindControl("ddlUqc");
                    TextBox txtQty = (TextBox)grdDetails.Rows[i].FindControl("txtQty");

                    strType = "";
                    CgstPercent = 0; SgstPercent = 0; IgstPercent = 0; CgstAmount = 0; SgstAmount = 0; IgstAmount = 0; Qty = 0;
                    if (ddlType.SelectedIndex > 0)
                        strType = ddlType.SelectedValue;
                    if (ddlCgstPercent.SelectedIndex > 0)
                        CgstPercent = Convert.ToDecimal(ddlCgstPercent.SelectedValue);
                    if (ddlSgstPercent.SelectedIndex > 0)
                        SgstPercent = Convert.ToDecimal(ddlSgstPercent.SelectedValue);
                    if (ddlIgstPercent.SelectedIndex > 0)
                        IgstPercent = Convert.ToDecimal(ddlIgstPercent.SelectedValue);
                    if (txtCgstAmount.Text != "")
                        CgstAmount = Convert.ToDecimal(txtCgstAmount.Text);
                    if (txtSgstAmount.Text != "")
                        SgstAmount = Convert.ToDecimal(txtSgstAmount.Text);
                    if (txtIgstAmount.Text != "")
                        IgstAmount = Convert.ToDecimal(txtIgstAmount.Text);
                    if (txtQty.Text != "")
                        Qty = Convert.ToDecimal(txtQty.Text);

                    txtLedgerName.Text = txtLedgerName.Text.Trim();
                    dc.TallyLedger_Update(txtLedgerName.Text);
                    dc.BillBookingDetail_Update(bookingId, txtLedgerName.Text.Trim(), ddlCostCenter.SelectedItem.Text, ddlCategory.SelectedItem.Text, Convert.ToDecimal(txtAmount.Text), txtHSNcode.Text, txtSACcode.Text, strType, CgstPercent, SgstPercent, IgstPercent, CgstAmount, SgstAmount, IgstAmount, ddlDrCr.SelectedValue, txtDescription.Text, ddlUqc.SelectedValue, Qty, false);
                }

                lblMessage.Text = "Updated Successfully";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Visible = true;
                lnkSave.Enabled = false;
                lnkCalculate.Enabled = false;
            }
        }

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        protected void CalculateTotal()
        {
            decimal totalAmount = 0, totalCgstAmount = 0, totalSgstAmount = 0, totalIgstAmount = 0, totalNetPayableAmount = 0;
            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                TextBox txtCgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCgstAmount");
                TextBox txtSgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtSgstAmount");
                TextBox txtIgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtIgstAmount");
                DropDownList ddlDrCr = (DropDownList)grdDetails.Rows[i].FindControl("ddlDrCr");

                if (txtAmount.Text != "")
                {
                    totalAmount += Convert.ToDecimal(txtAmount.Text);
                    if (txtCgstAmount.Text != "")
                        totalCgstAmount += Convert.ToDecimal(txtCgstAmount.Text);
                    if (txtSgstAmount.Text != "")
                        totalSgstAmount += Convert.ToDecimal(txtSgstAmount.Text);
                    if (txtIgstAmount.Text != "")
                        totalIgstAmount += Convert.ToDecimal(txtIgstAmount.Text);

                    if (ddlDrCr.SelectedValue == "Debit")
                        totalNetPayableAmount += Convert.ToDecimal(txtAmount.Text);
                    else
                        totalNetPayableAmount -= Convert.ToDecimal(txtAmount.Text);
                }
            }
            totalNetPayableAmount += totalCgstAmount + totalSgstAmount + totalIgstAmount;
            grdDetails.ShowFooter = true;
            //grdDetails.FooterRow.BackColor = System.Drawing.Color.White;
            //grdDetails.FooterStyle.BackColor = System.Drawing.Color.White;

            grdDetails.FooterRow.Cells[6].Text = totalAmount.ToString("0.00");
            grdDetails.FooterRow.Cells[6].Font.Bold = true;
            grdDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;

            grdDetails.FooterRow.Cells[13].Text = totalCgstAmount.ToString("0.00");
            grdDetails.FooterRow.Cells[13].Font.Bold = true;
            grdDetails.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Center;

            grdDetails.FooterRow.Cells[14].Text = totalSgstAmount.ToString("0.00");
            grdDetails.FooterRow.Cells[14].Font.Bold = true;
            grdDetails.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Center;

            grdDetails.FooterRow.Cells[15].Text = totalIgstAmount.ToString("0.00");
            grdDetails.FooterRow.Cells[15].Font.Bold = true;
            grdDetails.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Center;

            lblTotalNetPaybleAmount.Text = totalNetPayableAmount.ToString("0.00");

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
                dt.Columns.Add(new DataColumn("LedgerName", typeof(string)));
                dt.Columns.Add(new DataColumn("CostCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("Category", typeof(string)));
                dt.Columns.Add(new DataColumn("Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("HSNcode", typeof(string)));
                dt.Columns.Add(new DataColumn("SACcode", typeof(string)));
                dt.Columns.Add(new DataColumn("Type", typeof(string)));
                dt.Columns.Add(new DataColumn("CgstPercent", typeof(string)));
                dt.Columns.Add(new DataColumn("SgstPercent", typeof(string)));
                dt.Columns.Add(new DataColumn("IgstPercent", typeof(string)));
                dt.Columns.Add(new DataColumn("CgstAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("SgstAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("IgstAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlDrCr", typeof(string)));
                dt.Columns.Add(new DataColumn("Description", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlUqc", typeof(string)));
                dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["LedgerName"] = string.Empty;
            dr["CostCenter"] = "---Select---";
            dr["Category"] = "---Select---";
            dr["Amount"] = string.Empty;
            dr["HSNcode"] = string.Empty;
            dr["SACcode"] = string.Empty;
            dr["Type"] = string.Empty;
            dr["CgstPercent"] = string.Empty;
            dr["SgstPercent"] = string.Empty;
            dr["IgstPercent"] = string.Empty;
            dr["CgstAmount"] = string.Empty;
            dr["SgstAmount"] = string.Empty;
            dr["IgstAmount"] = string.Empty;
            dr["ddlDrCr"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["ddlUqc"] = string.Empty;
            dr["Qty"] = string.Empty;

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
            dtTable.Columns.Add(new DataColumn("LedgerName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CostCenter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Category", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("HSNcode", typeof(string)));
            dtTable.Columns.Add(new DataColumn("SACcode", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Type", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CgstPercent", typeof(string)));
            dtTable.Columns.Add(new DataColumn("SgstPercent", typeof(string)));
            dtTable.Columns.Add(new DataColumn("IgstPercent", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CgstAmount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("SgstAmount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("IgstAmount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlDrCr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Description", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlUqc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Qty", typeof(string)));

            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                TextBox txtHSNcode = (TextBox)grdDetails.Rows[i].FindControl("txtHSNcode");
                TextBox txtSACcode = (TextBox)grdDetails.Rows[i].FindControl("txtSACcode");
                DropDownList ddlType = (DropDownList)grdDetails.Rows[i].FindControl("ddlType");
                DropDownList ddlCgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlCgstPercent");
                DropDownList ddlSgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlSgstPercent");
                DropDownList ddlIgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlIgstPercent");
                TextBox txtCgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCgstAmount");
                TextBox txtSgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtSgstAmount");
                TextBox txtIgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtIgstAmount");
                DropDownList ddlDrCr = (DropDownList)grdDetails.Rows[i].FindControl("ddlDrCr");
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                DropDownList ddlUqc = (DropDownList)grdDetails.Rows[i].FindControl("ddlUqc");
                TextBox txtQty = (TextBox)grdDetails.Rows[i].FindControl("txtQty");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["LedgerName"] = txtLedgerName.Text;
                drRow["CostCenter"] = ddlCostCenter.SelectedItem.Text;
                drRow["Category"] = ddlCategory.SelectedItem.Text;
                drRow["Amount"] = txtAmount.Text;
                drRow["HSNcode"] = txtHSNcode.Text;
                drRow["SACcode"] = txtSACcode.Text;
                drRow["Type"] = ddlType.SelectedItem.Text;
                drRow["CgstPercent"] = ddlCgstPercent.SelectedItem.Text;
                drRow["SgstPercent"] = ddlSgstPercent.SelectedItem.Text;
                drRow["IgstPercent"] = ddlIgstPercent.SelectedItem.Text;
                drRow["CgstAmount"] = txtCgstAmount.Text;
                drRow["SgstAmount"] = txtSgstAmount.Text;
                drRow["IgstAmount"] = txtIgstAmount.Text;
                drRow["ddlDrCr"] = ddlDrCr.SelectedItem.Text;
                drRow["Description"] = txtDescription.Text;
                drRow["ddlUqc"] = ddlUqc.SelectedItem.Text;
                drRow["Qty"] = txtQty.Text;
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
                TextBox txtLedgerName = (TextBox)grdDetails.Rows[i].FindControl("txtLedgerName");
                DropDownList ddlCostCenter = (DropDownList)grdDetails.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdDetails.Rows[i].FindControl("ddlCategory");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                TextBox txtHSNcode = (TextBox)grdDetails.Rows[i].FindControl("txtHSNcode");
                TextBox txtSACcode = (TextBox)grdDetails.Rows[i].FindControl("txtSACcode");
                DropDownList ddlType = (DropDownList)grdDetails.Rows[i].FindControl("ddlType");
                DropDownList ddlCgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlCgstPercent");
                DropDownList ddlSgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlSgstPercent");
                DropDownList ddlIgstPercent = (DropDownList)grdDetails.Rows[i].FindControl("ddlIgstPercent");
                TextBox txtCgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtCgstAmount");
                TextBox txtSgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtSgstAmount");
                TextBox txtIgstAmount = (TextBox)grdDetails.Rows[i].FindControl("txtIgstAmount");
                DropDownList ddlDrCr = (DropDownList)grdDetails.Rows[i].FindControl("ddlDrCr");
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                DropDownList ddlUqc = (DropDownList)grdDetails.Rows[i].FindControl("ddlUqc");
                TextBox txtQty = (TextBox)grdDetails.Rows[i].FindControl("txtQty");

                txtLedgerName.Text = dt.Rows[i]["LedgerName"].ToString();
                ddlCostCenter.SelectedItem.Text = dt.Rows[i]["CostCenter"].ToString();
                ddlCategory.SelectedItem.Text = dt.Rows[i]["Category"].ToString();
                txtAmount.Text = dt.Rows[i]["Amount"].ToString();
                txtHSNcode.Text = dt.Rows[i]["HSNcode"].ToString();
                txtSACcode.Text = dt.Rows[i]["SACcode"].ToString();
                ddlType.SelectedValue = dt.Rows[i]["Type"].ToString();
                ddlCgstPercent.SelectedValue = dt.Rows[i]["CgstPercent"].ToString();
                ddlSgstPercent.SelectedValue = dt.Rows[i]["SgstPercent"].ToString();
                ddlIgstPercent.SelectedValue = dt.Rows[i]["IgstPercent"].ToString();
                txtCgstAmount.Text = dt.Rows[i]["CgstAmount"].ToString();
                txtSgstAmount.Text = dt.Rows[i]["SgstAmount"].ToString();
                txtIgstAmount.Text = dt.Rows[i]["IgstAmount"].ToString();
                ddlDrCr.SelectedValue = dt.Rows[i]["ddlDrCr"].ToString();
                txtDescription.Text = dt.Rows[i]["Description"].ToString();
                ddlUqc.SelectedValue = dt.Rows[i]["ddlUqc"].ToString();
                txtQty.Text = dt.Rows[i]["Qty"].ToString();

            }

        }

        #endregion
    }
}