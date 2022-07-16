using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class TallyTransfer : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        //static string cnStr = System.Configuration.ConfigurationSettings.AppSettings["conStr"].ToString();
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Tally Transfer";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                optBill.Checked = true;
                lblFromDate.Text = "Approved From date : ";

                var banklist = dc.Bank_View("");
                ddlBankname.DataSource = banklist;
                ddlBankname.DataTextField = "BANK_Name_var";
                ddlBankname.DataBind();
                ddlBankname.Items.Insert(0, "---Select---");
            }
        }
        protected string getTestType(string recordType, string narration)
        { 
            string testType ="";
            if (recordType == "Monthly")
                testType = "Monthly";
            else if (recordType == "---")
                testType = "Coupon Sale - Cube Testing";
            else if (recordType == "AAC")
                testType = "AAC Block Testing";
            else if (recordType == "CT" )
                testType = "Cube Testing Fees";
            else if (recordType == "BT" || recordType == "WA" || recordType == "BT-"  )
                testType = "Brick Testing";
            else if (recordType == "PT" )
                testType = "Paving Blocks";
            else if (recordType == "MF" )
                testType = "Mix Design";
            else if (recordType == "ST" )
                testType = "Steel Testing";
            else if (recordType == "SO" )
                testType = "Soil Testing";
            else if (recordType == "NDT" )
                testType = "NDT";
            else if (recordType == "SOLID" )
                testType = "Masonry Blocks";
            else if (recordType == "CEMT" )
                testType = "Cement Testing";
            else if (recordType == "CORECUT" )
                testType = "Core Cutting";
            else if (recordType == "CR" )
                testType = "Core Testing";
            else if (recordType == "FLYASH" )
                testType = "Fly Ash";
            else if (recordType == "TILE" )
                testType = "Tile Testing";
            else if (recordType == "PILE" )
                testType = "Pile Testing";
            else if (recordType == "GT" )
                testType = "Soil Investigation";
            else if (recordType == "STC" )
                testType = "Steel Chemical Testing";
            else if (recordType == "CCH" )
                testType = "Cement Chemical Testing";
            else if (recordType == "WT" )
                testType = "Water Testing";
            else if (narration.Contains("Cube Testing Coupons")==true) //vsfBill.TextMatrix(1, 1)
                testType = "Coupon Sale - Cube Testing";
            else if (narration.Contains("core cutting and testing") == true)
                testType = "Core Testing";
            else if (narration.Contains("Core Cutting") == true)
                testType = "Core Cutting";
            else if (narration.Contains("Site Visit") == true)
                testType = "Site Visit";
            else if (narration.Contains("Water Testing") == true)
                testType = "Water Testing";
            else if (narration.Contains("pile") == true) //NarrationStr                
                testType = "Pile Testing";
            else 
                testType = "Testing  fees";
            return testType;
        }
        
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadList();
        }
        protected void LoadList()
        {
            grdBill.DataSource = null;
            grdBill.DataBind();
            #region columns
            if (optReceipt.Checked == true || optAdvanceReceipt.Checked == true)
            {
                ddlBankname.Visible = true;
                lblBankName.Visible = true;
                ddlBankname.SelectedValue = "---Select---";
            }
            else
            {
                ddlBankname.Visible = false;
                lblBankName.Visible = false;
            }
            for (int i = 5; i < grdBill.Columns.Count; i++)
            {
                if (optBill.Checked == true)
                {
                    grdBill.Columns[i].Visible = true;
                }
                else
                {
                    grdBill.Columns[i].Visible = false;
                }
            }
            if (optBill.Checked == true)
            {
                grdBill.Columns[6].Visible = false;
            }
            if (optDebitNote.Checked == true && optAdjustAdvanceNote.Checked == true)
            {
                grdBill.Columns[5].Visible = true;
            }

            if (optBill.Checked == true)
            {
                grdBill.Columns[1].HeaderText = "Bill No.";
                grdBill.Columns[2].HeaderText = "Bill Date";
                grdBill.Columns[3].HeaderText = "Bill Amount";
                grdBill.Columns[4].HeaderText = "Client Name";
                grdBill.Columns[5].HeaderText = "Site Name";
            }
            else if (optReceipt.Checked == true)
            {
                grdBill.Columns[1].HeaderText = "Receipt No.";
                grdBill.Columns[2].HeaderText = "Receipt Date";
                grdBill.Columns[3].HeaderText = "Receipt Amount";
                grdBill.Columns[4].HeaderText = "Client Name";
            }
            else if (optCreditNote.Checked == true)
            {
                grdBill.Columns[1].HeaderText = "Note No.";
                grdBill.Columns[2].HeaderText = "Date";
                grdBill.Columns[3].HeaderText = "Amount";
                grdBill.Columns[4].HeaderText = "Client Name";
            }
            else if (optDebitNote.Checked == true)
            {
                grdBill.Columns[1].HeaderText = "Note No.";
                grdBill.Columns[2].HeaderText = "Date";
                grdBill.Columns[3].HeaderText = "Amount";
                grdBill.Columns[4].HeaderText = "Client Name";
                grdBill.Columns[5].HeaderText = "Site Name";
            }
            else if (optAdvanceReceipt.Checked == true)
            {
                grdBill.Columns[1].HeaderText = "Receipt No.";
                grdBill.Columns[2].HeaderText = "Receipt Date";
                grdBill.Columns[3].HeaderText = "Receipt Amount";
                grdBill.Columns[4].HeaderText = "Ledger Name";
            }
            else if (optAdjustAdvanceNote.Checked == true)
            {
                grdBill.Columns[1].HeaderText = "Note No.";
                grdBill.Columns[2].HeaderText = "Date";
                grdBill.Columns[3].HeaderText = "Amount";                
                grdBill.Columns[4].HeaderText = "Client Name";
                grdBill.Columns[5].HeaderText = "Ledger Name";
            }
            else if (optCashPayment.Checked == true)
            {
                grdBill.Columns[1].HeaderText = "Voucher No.";
                grdBill.Columns[2].HeaderText = "Voucher Date";
                grdBill.Columns[3].HeaderText = "Total Amount";
                grdBill.Columns[3].HeaderText = "Narration";
            }
            #endregion

            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            if (optBill.Checked == true)
            {
                #region display bill
                DataTable dt = new DataTable();
                //all bills should display (ok cancel) exported bill should not be displayed
                var billd = dc.Bill_View_ApprovedDateWise(Fromdate, Todate).ToList();
                for (int i = 0; i < billd.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblReceiptNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSiteName", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblNarration", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblAmt", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblServiceTaxAmt", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblTestType", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblServiceTax", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillStatus", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblTravelling", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblDiscountAmt", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblMktUser", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblRoundingOff", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblMobilization", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSwachhBharatCess", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSwachhBharatCessAmt", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblKisanKrishiCess", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblKisanKrishiCessAmt", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblCgst", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblCgstAmt", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSgst", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSgstAmt", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblIgst", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblIgstAmt", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBill.DataSource = dt;
                    grdBill.DataBind();
                }
                int rowNo = 0;
                foreach (var bill in billd)
                {
                    Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[rowNo].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[rowNo].FindControl("lblSiteName");
                    Label lblNarration = (Label)grdBill.Rows[rowNo].FindControl("lblNarration");
                    Label lblAmt = (Label)grdBill.Rows[rowNo].FindControl("lblAmt");
                    Label lblServiceTaxAmt = (Label)grdBill.Rows[rowNo].FindControl("lblServiceTaxAmt");
                    Label lblTestType = (Label)grdBill.Rows[rowNo].FindControl("lblTestType");
                    Label lblServiceTax = (Label)grdBill.Rows[rowNo].FindControl("lblServiceTax");
                    Label lblBillStatus = (Label)grdBill.Rows[rowNo].FindControl("lblBillStatus");
                    Label lblTravelling = (Label)grdBill.Rows[rowNo].FindControl("lblTravelling");
                    Label lblDiscountAmt = (Label)grdBill.Rows[rowNo].FindControl("lblDiscountAmt");
                    Label lblMktUser = (Label)grdBill.Rows[rowNo].FindControl("lblMktUser");
                    Label lblRoundingOff = (Label)grdBill.Rows[rowNo].FindControl("lblRoundingOff");
                    Label lblMobilization = (Label)grdBill.Rows[rowNo].FindControl("lblMobilization");
                    Label lblSwachhBharatCess = (Label)grdBill.Rows[rowNo].FindControl("lblSwachhBharatCess");
                    Label lblSwachhBharatCessAmt = (Label)grdBill.Rows[rowNo].FindControl("lblSwachhBharatCessAmt");
                    Label lblKisanKrishiCess = (Label)grdBill.Rows[rowNo].FindControl("lblKisanKrishiCess");
                    Label lblKisanKrishiCessAmt = (Label)grdBill.Rows[rowNo].FindControl("lblKisanKrishiCessAmt");
                    Label lblCgst = (Label)grdBill.Rows[rowNo].FindControl("lblCgst");
                    Label lblCgstAmt = (Label)grdBill.Rows[rowNo].FindControl("lblCgstAmt");
                    Label lblSgst = (Label)grdBill.Rows[rowNo].FindControl("lblSgst");
                    Label lblSgstAmt = (Label)grdBill.Rows[rowNo].FindControl("lblSgstAmt");
                    Label lblIgst = (Label)grdBill.Rows[rowNo].FindControl("lblIgst");
                    Label lblIgstAmt = (Label)grdBill.Rows[rowNo].FindControl("lblIgstAmt");

                    lblBillNo.Text = bill.BILL_Id.ToString();
                    lblBillDate.Text = Convert.ToDateTime(bill.BILL_Date_dt.ToString()).ToString("dd/MM/yyyy");
                    lblBillAmount.Text = bill.BILL_NetAmt_num.ToString();
                    lblClientName.Text = bill.CL_Name_var;
                    lblSiteName.Text = bill.SITE_Name_var;
                    lblNarration.Text = bill.BILL_TallyNarration_var;

                    double amt = 0, tAmt = 0, mobilizaiton = 0;
                    var billdetail = dc.BillDetail_View(bill.BILL_Id);
                    foreach (var bd in billdetail)
                    {
                        if (bd.BILLD_TEST_Name_var.Contains("Travelling") == true)
                            tAmt = tAmt + Convert.ToDouble(bd.BILLD_Amt_num);
                        else if (bd.BILLD_TEST_Name_var.Contains("Mobilization") == true || bd.BILLD_TEST_Name_var.Contains("Mobilisation") == true)
                            mobilizaiton = mobilizaiton + Convert.ToDouble(bd.BILLD_Amt_num);
                        else
                            amt = amt + Convert.ToDouble(bd.BILLD_Amt_num);
                    }
                    lblAmt.Text = amt.ToString();
                    lblServiceTaxAmt.Text = bill.BILL_SerTaxAmt_num.ToString();
                    lblTestType.Text = getTestType(bill.BILL_RecordType_var, bill.BILL_TallyNarration_var);
                    lblServiceTax.Text = bill.BILL_SerTax_num.ToString();
                    //status should be created /updated 
                    if (bill.BILL_Status_bit == false)
                        lblBillStatus.Text = "Ok";
                    else
                        lblBillStatus.Text = "Cancel";
                    lblTravelling.Text = tAmt.ToString();
                    lblMobilization.Text = mobilizaiton.ToString();
                    lblDiscountAmt.Text = bill.BILL_DiscountAmt_num.ToString();
                    lblMktUser.Text = ""; //assinged user name to be displayed
                    lblRoundingOff.Text = bill.BILL_RoundOffAmt_num.ToString();
                    lblSwachhBharatCess.Text = bill.BILL_SwachhBharatTax_num.ToString();
                    lblSwachhBharatCessAmt.Text = bill.BILL_SwachhBharatTaxAmt_num.ToString();
                    lblKisanKrishiCess.Text = bill.BILL_KisanKrishiTax_num.ToString();
                    lblKisanKrishiCessAmt.Text = bill.BILL_KisanKrishiTaxAmt_num.ToString();
                    lblCgst.Text = bill.BILL_CGST_num.ToString();
                    lblCgstAmt.Text = bill.BILL_CGSTAmt_num.ToString();
                    lblSgst.Text = bill.BILL_SGST_num.ToString();
                    lblSgstAmt.Text = bill.BILL_SGSTAmt_num.ToString();
                    lblIgst.Text = bill.BILL_IGST_num.ToString();
                    lblIgstAmt.Text = bill.BILL_IGSTAmt_num.ToString();

                    //after tally transfer tally status bit to be update
                    rowNo++;
                }
                #endregion
            }
            else if (optReceipt.Checked == true)
            {
                #region display receipt
                DataTable dt = new DataTable();               
                var receipt = dc.Cash_View_Receipt(0,Fromdate, Todate).ToList();
                for (int i = 0; i < receipt.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBillNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBill.DataSource = dt;
                    grdBill.DataBind();
                }
                int rowNo = 0;
                foreach (var rcpt in receipt)
                {
                    Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[rowNo].FindControl("lblClientName");
                    
                    lblBillNo.Text = rcpt.Cash_ReceiptNo.ToString();
                    lblBillDate.Text = Convert.ToDateTime(rcpt.Cash_Date_date.ToString()).ToString("dd/MM/yyyy");
                    lblBillAmount.Text = rcpt.Cash_Amount_num.ToString();
                    lblClientName.Text = rcpt.CL_Name_var;                    
                    rowNo++;
                }
                 #endregion
            }
            else if (optCreditNote.Checked == true)
            {                
                #region display credit notes
                DataTable dt = new DataTable();
                var crnote = dc.JournalModify_View("", "CR%", Fromdate, Todate, false, true).ToList();
                for (int i = 0; i < crnote.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBillNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSiteName", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBill.DataSource = dt;
                    grdBill.DataBind();
                }
                int rowNo = 0;
                foreach (var cr in crnote)
                {
                    Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[rowNo].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[rowNo].FindControl("lblSiteName");

                    lblBillNo.Text = cr.Journal_NoteNo_var;
                    lblBillDate.Text = Convert.ToDateTime(cr.Journal_Date_dt.ToString()).ToString("dd/MM/yyyy");
                    lblBillAmount.Text = cr.Journal_Amount_dec.ToString();
                    lblClientName.Text = cr.CL_Name_var;
                    lblSiteName.Text = cr.SITE_Name_var;
                    rowNo++;
                }
                #endregion
            }
            else if (optDebitNote.Checked == true)
            {
                #region display debit notes
                DataTable dt = new DataTable();
                var crnote = dc.JournalModify_View("", "DB%", Fromdate, Todate, false,true).ToList();
                for (int i = 0; i < crnote.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBillNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBill.DataSource = dt;
                    grdBill.DataBind();
                }
                int rowNo = 0;
                foreach (var cr in crnote)
                {
                    Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[rowNo].FindControl("lblClientName");

                    lblBillNo.Text = cr.Journal_NoteNo_var;
                    lblBillDate.Text = Convert.ToDateTime(cr.Journal_Date_dt.ToString()).ToString("dd/MM/yyyy");
                    lblBillAmount.Text = cr.Journal_Amount_dec.ToString();
                    lblClientName.Text = cr.CL_Name_var;
                    rowNo++;
                }
                #endregion
            }
            else if (optAdvanceReceipt.Checked == true)
            {
                #region display advance receipt
                DataTable dt = new DataTable();
                var advRcpt = dc.Advance_View(Fromdate, Todate, 0, "", true).ToList();
                for (int i = 0; i < advRcpt.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBillNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBill.DataSource = dt;
                    grdBill.DataBind();
                }
                int rowNo = 0;
                foreach (var adv in advRcpt)
                {
                    Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[rowNo].FindControl("lblClientName");

                    lblBillNo.Text = adv.ReceiptNo.ToString();
                    lblBillDate.Text = Convert.ToDateTime(adv.ReceiptDate.ToString()).ToString("dd/MM/yyyy");
                    lblBillAmount.Text = (adv.Amount * -1).ToString();
                    lblClientName.Text = adv.LedgerName_Description;
                    rowNo++;
                }
                #endregion
            }
            else if (optAdjustAdvanceNote.Checked == true)
            {
                #region display adjusted advance note
                DataTable dt = new DataTable();
                var advRcpt = dc.Advance_View(Fromdate, Todate, 0,"", false).ToList();
                for (int i = 0; i < advRcpt.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBillNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSiteName", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBill.DataSource = dt;
                    grdBill.DataBind();
                }
                int rowNo = 0;
                foreach (var adv in advRcpt)
                {
                    Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[rowNo].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[rowNo].FindControl("lblSiteName");

                    lblBillNo.Text = adv.NoteNo;
                    lblBillDate.Text = Convert.ToDateTime(adv.ReceiptDate.ToString()).ToString("dd/MM/yyyy");
                    lblBillAmount.Text = adv.ReceiptAmount.ToString();
                    lblClientName.Text = adv.clientName;
                    lblSiteName.Text = adv.LedgerName_Description; 
                    rowNo++;
                }
                #endregion
            }
            else if (optCashPayment.Checked == true)
            {
                #region display cash payment
                DataTable dt = new DataTable();
                var receipt = dc.CashBankPayment_View(0, "", Fromdate, Todate).ToList();
                for (int i = 0; i < receipt.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBillNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBill.DataSource = dt;
                    grdBill.DataBind();
                }
                int rowNo = 0;
                foreach (var rcpt in receipt)
                {
                    Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[rowNo].FindControl("lblClientName");

                    lblBillNo.Text = rcpt.CASHBANKPAY_VoucherNo_var;
                    lblBillDate.Text = Convert.ToDateTime(rcpt.CASHBANKPAY_VoucherDate_dt).ToString("dd/MM/yyyy");
                    lblBillAmount.Text = rcpt.CASHBANKPAY_TotalAmount_num.ToString();
                    lblClientName.Text = rcpt.CASHBANKPAY_Narration_var;
                    rowNo++;
                }
                #endregion
            }
        }

        protected void lnkTransfer_Click(object sender, EventArgs e)
        {
            bool selectedFlag = false;
            if (grdBill.Rows.Count > 0)
            {
                for (int i = 0; i < grdBill.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        selectedFlag = true;                        
                        break;
                    }
                }
            }
            if (selectedFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one row');", true);
            }
            else
            {
                if (optReceipt.Checked == true || optAdvanceReceipt.Checked == true)
                {
                    if (ddlBankname.SelectedIndex <= 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select bank account');", true);
                        selectedFlag = false;
                    }
                }
            }
            if (selectedFlag == true )
            {
                if (optBill.Checked == true)
                {
                    BillTallyTransfer();
                }
                else if (optReceipt.Checked == true)
                {
                    ReceiptTallyTransfer();
                }
                else if (optDebitNote.Checked == true)
                {
                    DebitNoteTallyTransfer();
                }
                else if (optCreditNote.Checked == true)
                {
                    CreditNoteTallyTransfer();
                }
                else if (optAdvanceReceipt.Checked == true)
                {
                    AdvanceReceiptTallyTransfer();
                }
                else if (optAdjustAdvanceNote.Checked == true)
                {
                    AdjustAdvanceNoteTallyTransfer();
                }
                else if (optCashPayment.Checked == true)
                {
                    CashPaymentTallyTransfer();
                }
            }
        }
        private void CashPaymentTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2015";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region cash payment
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    //Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");

                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    
                    var cashpayment = dc.CashBankPayment_View(0,lblBillNo.Text,null,null).ToList();
                    foreach (var cp in cashpayment)
                    {

                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "Payment");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        NARRATION.InnerText = lblClientName.Text;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        VOUCHERTYPENAME.InnerText = "Payment";
                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        VOUCHERNUMBER.InnerText = lblBillNo.Text;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            PARTYLEDGERNAME.InnerText = "Cash";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            PARTYLEDGERNAME.InnerText = "Cash";
                        else
                            PARTYLEDGERNAME.InnerText = "Petty Cash - Sinhgad Road";                        
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);
                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
                        ISMSTFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(ISMSTFROMSYNC);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
                        USEFOREXCISE.InnerText = "No";
                        VOUCHER.AppendChild(USEFOREXCISE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);

                        XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
                        USEFORSERVICETAX.InnerText = "No";
                        VOUCHER.AppendChild(USEFORSERVICETAX);
                        
                        XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
                        ISEXCISEVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEVOUCHER);

                        XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
                        EXCISETAXOVERRIDE.InnerText = "No";
                        VOUCHER.AppendChild(EXCISETAXOVERRIDE);

                        XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
                        USEFORTAXUNITTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

                        XmlElement IGNOREPOSVALIDATION = xmldoc.CreateElement("IGNOREPOSVALIDATION");
                        IGNOREPOSVALIDATION.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREPOSVALIDATION);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
                        ISTDSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSOVERRIDDEN);

                        XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
                        ISTCSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTCSOVERRIDDEN);

                        XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
                        ISTDSTCSCASHVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSTCSCASHVCH);

                        XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
                        INCLUDEADVPYMTVCH.InnerText = "No";
                        VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

                        XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
                        ISSUBWORKSCONTRACT.InnerText = "No";
                        VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

                        XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
                        ISVATOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISVATOVERRIDDEN);

                        XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
                        IGNOREORIGVCHDATE.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREORIGVCHDATE);

                        XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
                        ISVATPAIDATCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

                        XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
                        ISDECLAREDTOCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

                        XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
                        ISSERVICETAXOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

                        XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
                        ISISDVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISISDVOUCHER);

                        XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
                        ISEXCISEOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

                        XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
                        ISEXCISESUPPLYVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

                        XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
                        ISGSTOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISGSTOVERRIDDEN);


                        XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
                        GSTNOTEEXPORTED.InnerText = "No";
                        VOUCHER.AppendChild(GSTNOTEEXPORTED);

                        XmlElement IGNOREGSTINVALIDATION = xmldoc.CreateElement("IGNOREGSTINVALIDATION");
                        IGNOREGSTINVALIDATION.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREGSTINVALIDATION);

                        XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
                        ISVATPRINCIPALACCOUNT.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

                        XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
                        ISBOENOTAPPLICABLE.InnerText = "No";
                        VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

                        XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
                        ISSHIPPINGWITHINSTATE.InnerText = "No";
                        VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

                        XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
                        ISOVERSEASTOURISTTRANS.InnerText = "No";
                        VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

                        XmlElement ISDESIGNATEDZONEPARTY = xmldoc.CreateElement("ISDESIGNATEDZONEPARTY");
                        ISDESIGNATEDZONEPARTY.InnerText = "No";
                        VOUCHER.AppendChild(ISDESIGNATEDZONEPARTY);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "No";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
                        ISVOID.InnerText = "No";
                        VOUCHER.AppendChild(ISVOID);

                        XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
                        ISONHOLD.InnerText = "No";
                        VOUCHER.AppendChild(ISONHOLD);

                        XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
                        ORDERLINESTATUS.InnerText = "No";
                        VOUCHER.AppendChild(ORDERLINESTATUS);

                        XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
                        VATISAGNSTCANCSALES.InnerText = "No";
                        VOUCHER.AppendChild(VATISAGNSTCANCSALES);

                        XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
                        VATISPURCEXEMPTED.InnerText = "No";
                        VOUCHER.AppendChild(VATISPURCEXEMPTED);

                        XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
                        ISVATRESTAXINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISVATRESTAXINVOICE);

                        XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
                        VATISASSESABLECALCVCH.InnerText = "No";
                        VOUCHER.AppendChild(VATISASSESABLECALCVCH);

                        XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
                        ISVATDUTYPAID.InnerText = "Yes";
                        VOUCHER.AppendChild(ISVATDUTYPAID);

                        XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
                        ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
                        VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

                        XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
                        ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
                        VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        //XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        //ASORIGINAL.InnerText = "No";
                        //VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
                        CHANGEVCHMODE.InnerText = "No";
                        VOUCHER.AppendChild(CHANGEVCHMODE);

                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement EXCLUDEDTAXATION = xmldoc.CreateElement("EXCLUDEDTAXATION.LIST");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
                        XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                        XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
                        XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(EXCLUDEDTAXATION);
                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(DUTYHEADDETAILS);
                        VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
                        VOUCHER.AppendChild(EWAYBILLDETAILS);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);
                        VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                        VOUCHER.AppendChild(INVOICEEXPORTLIST);

                        var cashpaymentdetail = dc.CashBankPaymentDetail_View(lblBillNo.Text).ToList();
                        foreach (var cpd in cashpaymentdetail)
                        {
                            XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                            XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                            OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                            XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                            OLDAUDITENTRYIDSs.InnerText = "-1";
                            OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                            ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                            XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                            LEDGERNAMEe.InnerText = cpd.CASHBANKPAYDETAIL_LedgerName_var;
                            ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                            XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                            ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                            XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                            ISDEEMEDPOSITIVEa.InnerText = "Yes";
                            ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                            XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                            LEDGERFROMITEMa.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                            XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                            REMOVEZEROENTRIESq.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                            XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                            ISPARTYLEDGERw.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                            XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                            //ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                            ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                            ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                            XmlElement ISCAPVATTAXALTEREDr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                            ISCAPVATTAXALTEREDr.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDr);

                            XmlElement ISCAPVATNOTCLAIMEDdr = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                            ISCAPVATNOTCLAIMEDdr.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(ISCAPVATNOTCLAIMEDdr);

                            XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                            AMOUNTav.InnerText = "-" + cpd.CASHBANKPAYDETAIL_Amount_num.ToString();
                            ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                            XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
                            VATEXPAMOUNT.InnerText = "-" + cpd.CASHBANKPAYDETAIL_Amount_num.ToString();
                            ALLLEDGERENTRIESM.AppendChild(VATEXPAMOUNT);

                            XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                            ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILS);

                            if (cpd.CASHBANKPAYDETAIL_Category_var != "---Select---" && cpd.CASHBANKPAYDETAIL_Category_var != "NA" && cpd.CASHBANKPAYDETAIL_Category_var != null)
                            {
                                XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                                XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                                CATEGORY.InnerText = cpd.CASHBANKPAYDETAIL_Category_var;
                                CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                                XmlElement ISDEEMEDPOSITIVEc = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                ISDEEMEDPOSITIVEc.InnerText = "Yes";
                                CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEc);

                                XmlElement COSTCENTREALLOCATIONS = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                                XmlElement NAMEc = xmldoc.CreateElement("NAME");
                                NAMEc.InnerText = cpd.CASHBANKPAYDETAILL_CostCenter_var;
                                COSTCENTREALLOCATIONS.AppendChild(NAMEc);

                                XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                                AMOUNTc.InnerText = "-" + cpd.CASHBANKPAYDETAIL_Amount_num.ToString();
                                COSTCENTREALLOCATIONS.AppendChild(AMOUNTc);

                                CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONS);
                                ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);
                            }
                            XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                            XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                            XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                            XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                            XmlElement INPUTCRALLOCSs = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                            XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                            XmlElement EXCISEDUTYHEADDETAILSs = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                            XmlElement RATEDETAILSs = xmldoc.CreateElement("RATEDETAILS.LIST");
                            XmlElement SUMMARYALLOCSs = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                            XmlElement STPYMTDETAILSs = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                            XmlElement EXCISEPAYMENTALLOCATIONSs = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                            XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                            XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                            XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                            XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                            XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                            XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                            XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                            XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
                            XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                            ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
                            ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                            ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                            ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                            ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                            ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                            ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSs);
                            ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
                            ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSs);
                            ALLLEDGERENTRIESM.AppendChild(RATEDETAILSs);
                            ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSs);
                            ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSs);
                            ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSs);
                            ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                            ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                            ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                            ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                            ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                            ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
                            ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
                            ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
                            ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
                            VOUCHER.AppendChild(ALLLEDGERENTRIESM);
                        }

                        //**
                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            LEDGERNAME.InnerText = "Cash";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            LEDGERNAME.InnerText = "Cash";
                        else
                            LEDGERNAME.InnerText = "Petty Cash - Sinhgad Road";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                        XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTERED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);

                        XmlElement ISCAPVATNOTCLAIMED = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                        ISCAPVATNOTCLAIMED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATNOTCLAIMED);

                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

                        XmlElement VATEXPAMOUNTs = xmldoc.CreateElement("VATEXPAMOUNT");
                        VATEXPAMOUNTs.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNTs);
                        XmlElement SERVICETAXDETAILSs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILSs);
                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                        //**

                        XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
                        VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
                        XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
                        XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
                        VOUCHER.AppendChild(TEMPGSTRATEDETAILS);

                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        //dc.CashBankPayment_Update_TallyStatus(Convert.ToInt32(lblBillNo.Text), true);
                        break;
                    }
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "CashPayment_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "CashPayment_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "CashPayment_Metro";
            else
                fileName = "CashPayment_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }

        //private void CashPaymentTallyTransfer()
        //{
        //    XmlDocument xmldoc = new XmlDocument();
        //    XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
        //    xmldoc.AppendChild(RootNode);

        //    XmlElement HEADER = xmldoc.CreateElement("HEADER");
        //    XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
        //    TALLYREQUEST.InnerText = "Import Data";
        //    HEADER.AppendChild(TALLYREQUEST);
        //    RootNode.AppendChild(HEADER);

        //    XmlElement BODY = xmldoc.CreateElement("BODY");
        //    XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
        //    XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
        //    XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
        //    REPORTNAME.InnerText = "Vouchers";
        //    REQUESTDESC.AppendChild(REPORTNAME);

        //    XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
        //    XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
        //    //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
        //    //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
        //    //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
        //    if (cnStr.ToLower().Contains("mumbai") == true)
        //        SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
        //    else if (cnStr.ToLower().Contains("nashik") == true)
        //        SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
        //    else
        //        SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
        //    STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
        //    REQUESTDESC.AppendChild(STATICVARIABLES);
        //    XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
        //    #region receipt
        //    for (int i = 0; i < grdBill.Rows.Count; i++)
        //    {
        //        CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
        //        if (chkSelect.Checked == true)
        //        {
        //            Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
        //            Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
        //            Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
        //            Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
        //            //Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");

        //            var cashpayment = dc.CashBankPayment_View(0, lblBillNo.Text, null, null);
        //            foreach (var rcptd in cashpayment)
        //            {
        //                XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
        //                TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

        //                XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
        //                VOUCHER.SetAttribute("REMOTEID", "");
        //                VOUCHER.SetAttribute("VCHKEY", "");
        //                VOUCHER.SetAttribute("VCHTYPE", "Payment");
        //                VOUCHER.SetAttribute("ACTION", "Create");
        //                VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

        //                XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
        //                XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                OLDAUDITENTRYIDSf.InnerText = "-1";
        //                OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
        //                VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

        //                XmlElement GUID = xmldoc.CreateElement("GUID");
        //                XmlElement DATE = xmldoc.CreateElement("DATE");
        //                DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
        //                DATE.InnerText = date.ToString("yyyyMMdd");
        //                VOUCHER.AppendChild(DATE);
        //                GUID.InnerText = "";
        //                VOUCHER.AppendChild(GUID);

        //                XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
        //                //string NarrationStr = "";
        //                //if (rcptd.Cash_)
        //                //{
        //                //    NarrationStr = NarrationStr + "AS PER CASH BOOK ";
        //                //}
        //                //else
        //                //{
        //                //    NarrationStr = "Ch. No. " + Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
        //                //    NarrationStr += " DT " + Convert.ToDateTime(rcptd.Cash_ChqDate_date).ToString("dd/MM/yyyy");
        //                //    NarrationStr += " " + rcptd.Cash_BankName_var;
        //                //    NarrationStr += " " + rcptd.Cash_Branch_var;
        //                //}
        //                //NarrationStr += rcptd.Cash_Note_var;
        //                //if (rcptd.Cash_CollectionDetail != null && rcptd.Cash_CollectionDetail != "")
        //                //{
        //                //    string[] CollectionDetail = new string[1];
        //                //    CollectionDetail = Convert.ToString(rcptd.Cash_CollectionDetail).Split('|');
        //                //    NarrationStr += " " + Convert.ToString(CollectionDetail[0]);
        //                //    NarrationStr += " - " + Convert.ToString(CollectionDetail[1]);
        //                //}
        //                //NARRATION.InnerText = NarrationStr;
        //                NARRATION.InnerText = lblClientName.Text;
        //                VOUCHER.AppendChild(NARRATION);

        //                XmlElement TAXUNITNAME = xmldoc.CreateElement("TAXUNITNAME");
        //                TAXUNITNAME.InnerText = "Default Tax Unit";
        //                VOUCHER.AppendChild(TAXUNITNAME);

        //                XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
        //                //if (lblType.Text == "Cash")
        //                    VOUCHERTYPENAME.InnerText = "PAYMENT";
        //                //else
        //                //    VOUCHERTYPENAME.InnerText = lblComment.Text;

        //                VOUCHER.AppendChild(VOUCHERTYPENAME);

        //                XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
        //                //if (DateTime.Now.Month <= 3)
        //                //    NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
        //                //else
        //                //    NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

        //                //NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
        //                VOUCHERNUMBER.InnerText = lblBillNo.Text;
        //                VOUCHER.AppendChild(VOUCHERNUMBER);

        //                XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
        //                XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
        //                XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
        //                PARTYLEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
        //                VOUCHER.AppendChild(PARTYLEDGERNAME);
        //                VOUCHER.AppendChild(CSTFORMISSUETYPE);
        //                VOUCHER.AppendChild(CSTFORMRECVTYPE);

        //                XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
        //                FBTPAYMENTTYPE.InnerText = "Default";
        //                VOUCHER.AppendChild(FBTPAYMENTTYPE);

        //                XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
        //                XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
        //                PERSISTEDVIEW.InnerText = "Accounting Voucher View";
        //                VOUCHER.AppendChild(PERSISTEDVIEW);
        //                VOUCHER.AppendChild(VCHGSTCLASS);

        //                XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
        //                ENTEREDBY.InnerText = "DESPL";
        //                VOUCHER.AppendChild(ENTEREDBY);

        //                //if (rcptd.CASHPAY_PaymentStatus_var == "Cash")
        //                //{
        //                //    XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
        //                //    VOUCHERTYPEORIGNAME.InnerText = "Receipt";
        //                //    VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);
        //                //}

        //                XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
        //                DIFFACTUALQTY.InnerText = "No";
        //                VOUCHER.AppendChild(DIFFACTUALQTY);

        //                XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
        //                ISMSTFROMSYNC.InnerText = "No";
        //                VOUCHER.AppendChild(ISMSTFROMSYNC);

        //                XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
        //                ASORIGINAL.InnerText = "No";
        //                VOUCHER.AppendChild(ASORIGINAL);

        //                XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
        //                AUDITED.InnerText = "No";
        //                VOUCHER.AppendChild(AUDITED);

        //                XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
        //                FORJOBCOSTING.InnerText = "No";
        //                VOUCHER.AppendChild(FORJOBCOSTING);

        //                XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
        //                ISOPTIONAL.InnerText = "No";
        //                VOUCHER.AppendChild(ISOPTIONAL);

        //                XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
        //                DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
        //                EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
        //                VOUCHER.AppendChild(EFFECTIVEDATE);

        //                XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
        //                USEFOREXCISE.InnerText = "No";
        //                VOUCHER.AppendChild(USEFOREXCISE);

        //                XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
        //                ISFORJOBWORKIN.InnerText = "No";
        //                VOUCHER.AppendChild(ISFORJOBWORKIN);

        //                XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
        //                ALLOWCONSUMPTION.InnerText = "No";
        //                VOUCHER.AppendChild(ALLOWCONSUMPTION);

        //                XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
        //                USEFORINTEREST.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORINTEREST);

        //                XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
        //                USEFORGAINLOSS.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORGAINLOSS);

        //                XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
        //                USEFORGODOWNTRANSFER.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

        //                XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
        //                USEFORCOMPOUND.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORCOMPOUND);


        //                XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
        //                USEFORSERVICETAX.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORSERVICETAX);

        //                XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
        //                ISEXCISEVOUCHER.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISEVOUCHER);

        //                XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
        //                EXCISETAXOVERRIDE.InnerText = "No";
        //                VOUCHER.AppendChild(EXCISETAXOVERRIDE);

        //                XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
        //                USEFORTAXUNITTRANSFER.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

        //                XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
        //                EXCISEOPENING.InnerText = "No";
        //                VOUCHER.AppendChild(EXCISEOPENING);

        //                XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
        //                USEFORFINALPRODUCTION.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORFINALPRODUCTION);

        //                XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
        //                ISTDSOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISTDSOVERRIDDEN);

        //                XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
        //                ISTCSOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISTCSOVERRIDDEN);

        //                XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
        //                ISTDSTCSCASHVCH.InnerText = "No";
        //                VOUCHER.AppendChild(ISTDSTCSCASHVCH);

        //                XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
        //                INCLUDEADVPYMTVCH.InnerText = "No";
        //                VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

        //                XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
        //                ISSUBWORKSCONTRACT.InnerText = "No";
        //                VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

        //                XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
        //                ISVATOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATOVERRIDDEN);

        //                XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
        //                IGNOREORIGVCHDATE.InnerText = "No";
        //                VOUCHER.AppendChild(IGNOREORIGVCHDATE);

        //                XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
        //                ISVATPAIDATCUSTOMS.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

        //                XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
        //                ISDECLAREDTOCUSTOMS.InnerText = "No";
        //                VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

        //                XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
        //                ISSERVICETAXOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

        //                XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
        //                ISISDVOUCHER.InnerText = "No";
        //                VOUCHER.AppendChild(ISISDVOUCHER);

        //                XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
        //                ISEXCISEOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

        //                XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
        //                ISEXCISESUPPLYVCH.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

        //                XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
        //                ISGSTOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISGSTOVERRIDDEN);


        //                XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
        //                GSTNOTEEXPORTED.InnerText = "No";
        //                VOUCHER.AppendChild(GSTNOTEEXPORTED);

        //                XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
        //                ISVATPRINCIPALACCOUNT.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

        //                XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
        //                ISBOENOTAPPLICABLE.InnerText = "No";
        //                VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

        //                XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
        //                ISSHIPPINGWITHINSTATE.InnerText = "No";
        //                VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

        //                XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
        //                ISOVERSEASTOURISTTRANS.InnerText = "No";
        //                VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

        //                XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
        //                ISCANCELLED.InnerText = "No";
        //                VOUCHER.AppendChild(ISCANCELLED);

        //                XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
        //                HASCASHFLOW.InnerText = "Yes";
        //                VOUCHER.AppendChild(HASCASHFLOW);

        //                XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
        //                ISPOSTDATED.InnerText = "No";
        //                VOUCHER.AppendChild(ISPOSTDATED);

        //                XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
        //                USETRACKINGNUMBER.InnerText = "No";
        //                VOUCHER.AppendChild(USETRACKINGNUMBER);

        //                XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
        //                ISINVOICE.InnerText = "No";
        //                VOUCHER.AppendChild(ISINVOICE);

        //                XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
        //                MFGJOURNAL.InnerText = "No";
        //                VOUCHER.AppendChild(MFGJOURNAL);

        //                XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
        //                HASDISCOUNTS.InnerText = "No";
        //                VOUCHER.AppendChild(HASDISCOUNTS);

        //                XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
        //                ASPAYSLIP.InnerText = "No";
        //                VOUCHER.AppendChild(ASPAYSLIP);

        //                XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
        //                ISCOSTCENTRE.InnerText = "No";
        //                VOUCHER.AppendChild(ISCOSTCENTRE);

        //                XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
        //                ISSTXNONREALIZEDVCH.InnerText = "No";
        //                VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

        //                XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
        //                ISEXCISEMANUFACTURERON.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

        //                XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
        //                ISBLANKCHEQUE.InnerText = "No";
        //                VOUCHER.AppendChild(ISBLANKCHEQUE);

        //                XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
        //                ISVOID.InnerText = "No";
        //                VOUCHER.AppendChild(ISVOID);

        //                XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
        //                ISONHOLD.InnerText = "No";
        //                VOUCHER.AppendChild(ISONHOLD);

        //                XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
        //                ORDERLINESTATUS.InnerText = "No";
        //                VOUCHER.AppendChild(ORDERLINESTATUS);

        //                XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
        //                VATISAGNSTCANCSALES.InnerText = "No";
        //                VOUCHER.AppendChild(VATISAGNSTCANCSALES);

        //                XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
        //                VATISPURCEXEMPTED.InnerText = "No";
        //                VOUCHER.AppendChild(VATISPURCEXEMPTED);

        //                XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
        //                ISVATRESTAXINVOICE.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATRESTAXINVOICE);

        //                XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
        //                VATISASSESABLECALCVCH.InnerText = "No";
        //                VOUCHER.AppendChild(VATISASSESABLECALCVCH);

        //                XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
        //                ISVATDUTYPAID.InnerText = "Yes";
        //                VOUCHER.AppendChild(ISVATDUTYPAID);

        //                XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
        //                ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
        //                VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

        //                XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
        //                ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
        //                VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

        //                XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
        //                ISDELETED.InnerText = "No";
        //                VOUCHER.AppendChild(ISDELETED);

        //                XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
        //                CHANGEVCHMODE.InnerText = "No";
        //                VOUCHER.AppendChild(CHANGEVCHMODE);

        //                //XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
        //                //VCHISFROMSYNC.InnerText = "No";
        //                //VOUCHER.AppendChild(VCHISFROMSYNC);

        //                //XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
        //                //USEFORCOMPOUND.InnerText = "No";
        //                //VOUCHER.AppendChild(USEFORCOMPOUND);


        //                XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
        //                XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
        //                XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
        //                XmlElement EXCLUDEDTAXATIONS = xmldoc.CreateElement("EXCLUDEDTAXATIONS.LIST");
        //                XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
        //                XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
        //                XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
        //                XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
        //                XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
        //                XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
        //                XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
        //                XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
        //                XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
        //                XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
        //                XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
        //                XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

        //                ALTERID.InnerText = "";
        //                VOUCHER.AppendChild(ALTERID);

        //                MASTERID.InnerText = "";
        //                VOUCHER.AppendChild(MASTERID);

        //                VOUCHERKEY.InnerText = "";
        //                VOUCHER.AppendChild(VOUCHERKEY);

        //                VOUCHER.AppendChild(EXCLUDEDTAXATIONS);
        //                VOUCHER.AppendChild(OLDAUDITENTRIES);
        //                VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
        //                VOUCHER.AppendChild(AUDITENTRIES);
        //                VOUCHER.AppendChild(DUTYHEADDETAILS);
        //                VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
        //                VOUCHER.AppendChild(EWAYBILLDETAILS);
        //                VOUCHER.AppendChild(INVOICEDELNOTES);
        //                VOUCHER.AppendChild(INVOICEORDERLIST);
        //                VOUCHER.AppendChild(INVOICEINDENTLIST);
        //                VOUCHER.AppendChild(ATTENDANCEENTRIES);
        //                VOUCHER.AppendChild(ORIGINVOICEDETAILS);
        //                VOUCHER.AppendChild(INVOICEEXPORTLIST);

        //                XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
        //                XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
        //                XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                OLDAUDITENTRYIDSm.InnerText = "-1";
        //                OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
        //                ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

        //                XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
        //                XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
        //                //LEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
        //                ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
        //                ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

        //                XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                ISDEEMEDPOSITIVE.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

        //                XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
        //                LEDGERFROMITEM.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

        //                XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
        //                REMOVEZEROENTRIES.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

        //                XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
        //                ISPARTYLEDGER.InnerText = "Yes";
        //                ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

        //                XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
        //                ISLASTDEEMEDPOSITIVE.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);


        //                XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
        //                ISCAPVATTAXALTERED.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);


        //                XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
        //                XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
        //                AMOUNT.InnerText = lblBillAmount.Text;
        //                ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

        //                XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
        //                VATEXPAMOUNT.InnerText = lblBillAmount.Text;
        //                ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNT);

        //                XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
        //                ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILS);
        //                ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
        //                //
        //                int bno = 0;
        //                var rcpt = dc.CashBankPaymentDetail_View(lblBillNo.Text);
        //                foreach (var rcptdt in rcpt)
        //                {

        //                    XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
        //                    XmlElement NAME = xmldoc.CreateElement("NAME");
        //                    XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
        //                    //if (rcptdt.CashDetail_Settlement_var == "On A/c")
        //                    //{
        //                    //    BILLTYPE.InnerText = "On Account";
        //                    //    BILLALLOCATIONS.AppendChild(BILLTYPE);
        //                    //}
        //                    //else if (int.TryParse(rcptdt.CashDetail_BillNo_int, out bno) == true && Convert.ToInt32(rcptdt.CashDetail_BillNo_int) != 0)
        //                    //{
        //                    //    if (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) < 0)
        //                    //        NAME.InnerText = "DT - " + (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) * (-1));
        //                    //    else
        //                    //        NAME.InnerText = "DT - " + rcptdt.CashDetail_BillNo_int;

        //                    //    BILLALLOCATIONS.AppendChild(NAME);
        //                    //    BILLTYPE.InnerText = "Agst Ref";
        //                    //    BILLALLOCATIONS.AppendChild(BILLTYPE);
        //                    //}
        //                    if (rcptdt.CASHbankPAYDETAIL_InvoiceNo_var != "" && rcptdt.CASHPAYDETAIL_InvoiceNo_var != "0")
        //                    {
        //                        NAME.InnerText = rcptdt.CASHPAYDETAIL_InvoiceNo_var;
        //                        BILLALLOCATIONS.AppendChild(NAME);

        //                        BILLTYPE.InnerText = "Agst Ref";
        //                        BILLALLOCATIONS.AppendChild(BILLTYPE);
        //                    }
        //                    //else if (rcptdt.CashDetail_NoteNo_var != "")
        //                    //{
        //                    //    NAME.InnerText = rcptdt.CashDetail_NoteNo_var;
        //                    //    BILLALLOCATIONS.AppendChild(NAME);

        //                    //    BILLTYPE.InnerText = "Agst Ref";
        //                    //    BILLALLOCATIONS.AppendChild(BILLTYPE);
        //                    //}
        //                    XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
        //                    TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
        //                    BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

        //                    XmlElement AMT = xmldoc.CreateElement("AMOUNT");
        //                    AMT.InnerText = (rcptdt.CASHPAYDETAIL_CreditAmount_num * -1).ToString();
        //                    BILLALLOCATIONS.AppendChild(AMT);

        //                    XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
        //                    BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
        //                    ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);

        //                    XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
        //                    BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
        //                    ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
        //                }

        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
        //                //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ESCISEDUTYHEADDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
        //                VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

        //                XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
        //                XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
        //                XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                OLDAUDITENTRYIDSs.InnerText = "-1";
        //                OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
        //                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

        //                XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
        //                if (rcptd.CASHPAY_PaymentStatus_var == "Cash")
        //                {
        //                    if (cnStr.ToLower().Contains("mumbai") == true)
        //                        LEDGERNAMEe.InnerText = "Cash";
        //                    else if (cnStr.ToLower().Contains("nashik") == true)
        //                        LEDGERNAMEe.InnerText = "Cash";
        //                    else
        //                        LEDGERNAMEe.InnerText = "Petty Cash - Sinhgad Road";
        //                }
        //                else
        //                {
        //                    LEDGERNAMEe.InnerText = lblType.Text;
        //                }
        //                ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
        //                XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
        //                ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

        //                XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                ISDEEMEDPOSITIVEa.InnerText = "Yes";
        //                ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

        //                XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
        //                LEDGERFROMITEMa.InnerText = "No";
        //                ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

        //                XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
        //                REMOVEZEROENTRIESq.InnerText = "No";
        //                ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

        //                XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
        //                ISPARTYLEDGERw.InnerText = "Yes";
        //                ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

        //                XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
        //                ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
        //                ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

        //                XmlElement ISCAPVATTAXALTEREDEr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
        //                ISCAPVATTAXALTEREDEr.InnerText = "No";
        //                ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDEr);

        //                XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
        //                //if (rcptd.Cash_TDS_num > 0)
        //                //{
        //                //    AMOUNTav.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
        //                //}
        //                //else
        //                //{
        //                AMOUNTav.InnerText = (rcptd.CASHPAY_TotalAmount_num * -1).ToString();
        //                //}
        //                ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

        //                XmlElement AMOUNTVAT = xmldoc.CreateElement("VATEXPAMOUNT");
        //                //if (rcptd.Cash_TDS_num > 0)
        //                //{
        //                //    AMOUNTVAT.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
        //                //}
        //                //else
        //                //{
        //                AMOUNTVAT.InnerText = (rcptd.CASHPAY_TotalAmount_num * -1).ToString();
        //                //}
        //                ALLLEDGERENTRIESM.AppendChild(AMOUNTVAT);

        //                XmlElement SERVICETAXDETAILSS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
        //                ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILSS);

        //                XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
        //                //
        //                ////if (rcptd.Cash_PaymentType_bit == true)
        //                ////{
        //                ////    XmlElement TEMPXmlElemt = xmldoc.CreateElement("DATE");
        //                ////    date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
        //                ////    TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTDATE");
        //                ////    date = Convert.ToDateTime(rcptd.Cash_ChqDate_date);
        //                ////    TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    //TEMPXmlElemt = xmldoc.CreateElement("BANKERSDATE");
        //                ////    //date = Convert.ToDateTime(rcptd.Cash_ChqDate_date);
        //                ////    //TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
        //                ////    //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("NAME");
        //                ////    //TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
        //                ////    //TEMPXmlElemt.InnerText = rcptd.Cash_BankName_var;
        //                ////    if (DateTime.Now.Month <= 3)
        //                ////        NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
        //                ////    else
        //                ////        NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

        //                ////    NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
        //                ////    TEMPXmlElemt.InnerText = NarrationStr;
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("TRANSACTIONTYPE");
        //                ////    TEMPXmlElemt.InnerText = "Cheque/DD";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("BANKNAME");
        //                ////    TEMPXmlElemt.InnerText = rcptd.Cash_BankName_var;
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("BANKBRANCHNAME");
        //                ////    TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("PAYMENTFAVOURING");
        //                ////    TEMPXmlElemt.InnerText = lblClientName.Text;
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("BANKID");
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTNUMBER");
        //                ////    TEMPXmlElemt.InnerText = Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("UNIQUEREFERENCENUMBER");
        //                ////    TEMPXmlElemt.InnerText = "";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("STATUS");
        //                ////    TEMPXmlElemt.InnerText = "No";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("PAYMENTMODE");
        //                ////    TEMPXmlElemt.InnerText = "Transacted";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    //TEMPXmlElemt = xmldoc.CreateElement("SECONDARYSTATUS");
        //                ////    //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("BANKPARTYNAME");
        //                ////    TEMPXmlElemt.InnerText = lblClientName.Text.Replace("&", "AND"); ;
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("ISCONNECTEDPAYMENT");
        //                ////    TEMPXmlElemt.InnerText = "No";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("ISSPLIT");
        //                ////    TEMPXmlElemt.InnerText = "No";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("ISCONTRACTUSED");
        //                ////    TEMPXmlElemt.InnerText = "No";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("ISACCEPTEDWITHWARNING");
        //                ////    TEMPXmlElemt.InnerText = "No";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("ISTRANSFORCED");
        //                ////    TEMPXmlElemt.InnerText = "No";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


        //                ////    TEMPXmlElemt = xmldoc.CreateElement("CHEQUEPRINTED");
        //                ////    TEMPXmlElemt.InnerText = "1";
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("AMOUNT");
        //                ////    if (rcptd.Cash_TDS_num > 0)
        //                ////    {
        //                ////        TEMPXmlElemt.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
        //                ////    }
        //                ////    else
        //                ////    {
        //                ////        TEMPXmlElemt.InnerText = "-" + rcptd.Cash_Amount_num;
        //                ////    }
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("CONTRACTDETAILS.LIST");
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

        //                ////    TEMPXmlElemt = xmldoc.CreateElement("BANKSTATUSINFO.LIST");
        //                ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


        //                ////}
        //                //
        //                ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);

        //                XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
        //                XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
        //                XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
        //                XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
        //                XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
        //                XmlElement INPUTCRALLOCSS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
        //                XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
        //                XmlElement EXCISEDUTYHEADDETAILSS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
        //                XmlElement RATEDETAILSS = xmldoc.CreateElement("RATEDETAILS.LIST");
        //                XmlElement SUMMARYALLOCSS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
        //                XmlElement STPYMTDETAILSS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
        //                XmlElement EXCISEPAYMENTALLOCATIONSS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
        //                XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
        //                XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
        //                XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
        //                XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
        //                XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
        //                XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
        //                XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
        //                XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
        //                XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
        //                ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
        //                ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
        //                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
        //                ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
        //                ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
        //                ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSS);
        //                ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
        //                ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSS);
        //                ALLLEDGERENTRIESM.AppendChild(RATEDETAILSS);
        //                ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSS);
        //                ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSS);
        //                ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSS);
        //                ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
        //                ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
        //                ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
        //                ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
        //                ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
        //                ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
        //                ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
        //                ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
        //                ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
        //                VOUCHER.AppendChild(ALLLEDGERENTRIESM);

        //                //tds
        //                //XmlElement ALLLEDGERENTRIESMt = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
        //                //XmlElement OLDAUDITENTRYIDSzt = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                //OLDAUDITENTRYIDSzt.SetAttribute("TYPE", "Number");
        //                //XmlElement OLDAUDITENTRYIDSst = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                //OLDAUDITENTRYIDSst.InnerText = "-1";
        //                //OLDAUDITENTRYIDSzt.AppendChild(OLDAUDITENTRYIDSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRYIDSzt);

        //                //XmlElement LEDGERNAMEet = xmldoc.CreateElement("LEDGERNAME");
        //                //DateTime tempDate = Convert.ToDateTime(rcptd.CASHPAY_VoucherDate_dt);
        //                //if (tempDate.Month > 3)
        //                //    LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year) + "-" + (tempDate.Year + 1);
        //                //else
        //                //    LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year - 1) + "-" + (tempDate.Year);

        //                //ALLLEDGERENTRIESMt.AppendChild(LEDGERNAMEet);
        //                //XmlElement GSTCLASSst = xmldoc.CreateElement("GSTCLASS");
        //                //ALLLEDGERENTRIESMt.AppendChild(GSTCLASSst);

        //                ////XmlElement ISDEEMEDPOSITIVEat = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                ////ISDEEMEDPOSITIVEat.InnerText = "Yes";
        //                ////ALLLEDGERENTRIESMt.AppendChild(ISDEEMEDPOSITIVEat);

        //                //XmlElement LEDGERFROMITEMat = xmldoc.CreateElement("LEDGERFROMITEM");
        //                //LEDGERFROMITEMat.InnerText = "No";
        //                //ALLLEDGERENTRIESMt.AppendChild(LEDGERFROMITEMat);

        //                //XmlElement REMOVEZEROENTRIESqt = xmldoc.CreateElement("REMOVEZEROENTRIES");
        //                //REMOVEZEROENTRIESqt.InnerText = "No";
        //                //ALLLEDGERENTRIESMt.AppendChild(REMOVEZEROENTRIESqt);

        //                //XmlElement ISPARTYLEDGERwt = xmldoc.CreateElement("ISPARTYLEDGER");
        //                //ISPARTYLEDGERwt.InnerText = "Yes";
        //                //ALLLEDGERENTRIESMt.AppendChild(ISPARTYLEDGERwt);

        //                //XmlElement ISLASTDEEMEDPOSITIVErt = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
        //                //ISLASTDEEMEDPOSITIVErt.InnerText = "Yes";
        //                //ALLLEDGERENTRIESMt.AppendChild(ISLASTDEEMEDPOSITIVErt);

        //                //XmlElement ISCAPVATTAXALTEREd = xmldoc.CreateElement("ISCAPVATTAXALTERED");
        //                //ISCAPVATTAXALTEREd.InnerText = "No";
        //                //ALLLEDGERENTRIESMt.AppendChild(ISCAPVATTAXALTEREd);

        //                //XmlElement AMOUNTavt = xmldoc.CreateElement("AMOUNT");
        //                //AMOUNTavt.InnerText = "-" + rcptd.Cash_TDS_num;
        //                //ALLLEDGERENTRIESMt.AppendChild(AMOUNTavt);

        //                //XmlElement VATEXPAYMENT = xmldoc.CreateElement("VATEXPAYMENT");
        //                //VATEXPAYMENT.InnerText = "-" + rcptd.Cash_TDS_num;
        //                //ALLLEDGERENTRIESMt.AppendChild(VATEXPAYMENT);

        //                //XmlElement SERVICETAXDETAILs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
        //                //ALLLEDGERENTRIESMt.AppendChild(SERVICETAXDETAILs);

        //                //XmlElement BANKALLOCATIONSst = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
        //                //ALLLEDGERENTRIESMt.AppendChild(BANKALLOCATIONSst);

        //                //XmlElement BILLALLOCATIONSst = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
        //                //XmlElement INTERESTCOLLECTIONst = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
        //                //XmlElement OLDAUDITENTRIESst = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
        //                //XmlElement ACCOUNTAUDITENTRIESSst = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
        //                //XmlElement AUDITENTRIESswt = xmldoc.CreateElement("AUDITENTRIES.LIST");
        //                //XmlElement INPUTCRALLOCS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
        //                //XmlElement DUTYHEADDETAILs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
        //                //XmlElement EXCISEDUTYHEADDETAILS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
        //                //XmlElement RATEDETAILS = xmldoc.CreateElement("RATEDETAILS.LIST");
        //                //XmlElement SUMMARYALLOCS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
        //                //XmlElement STPYMTDETAILS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
        //                //XmlElement EXCISEPAYMENTALLOCATIONS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
        //                //XmlElement TAXBILLALLOCATIONSst = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
        //                //XmlElement TAXOBJECTALLOCATIONSst = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
        //                //XmlElement TDSEXPENSEALLOCATIONSst = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
        //                //XmlElement VATSTATUTORYDETAILSSst = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
        //                //XmlElement COSTTRACKALLOCATIONSst = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
        //                //XmlElement REFVOUCHERDETAILS = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
        //                //XmlElement INVOICEWISEDETAILS = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
        //                //XmlElement VATITCDETAILS = xmldoc.CreateElement("VATITCDETAILS.LIST");
        //                //XmlElement ADVANCETAXDETAILS = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
        //                //ALLLEDGERENTRIESMt.AppendChild(BILLALLOCATIONSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(INTERESTCOLLECTIONst);
        //                //ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRIESst);
        //                //ALLLEDGERENTRIESMt.AppendChild(ACCOUNTAUDITENTRIESSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(AUDITENTRIESswt);
        //                //ALLLEDGERENTRIESMt.AppendChild(INPUTCRALLOCS);
        //                //ALLLEDGERENTRIESMt.AppendChild(DUTYHEADDETAILs);
        //                //ALLLEDGERENTRIESMt.AppendChild(EXCISEDUTYHEADDETAILS);
        //                //ALLLEDGERENTRIESMt.AppendChild(RATEDETAILS);
        //                //ALLLEDGERENTRIESMt.AppendChild(SUMMARYALLOCS);
        //                //ALLLEDGERENTRIESMt.AppendChild(STPYMTDETAILS);
        //                //ALLLEDGERENTRIESMt.AppendChild(EXCISEPAYMENTALLOCATIONS);
        //                //ALLLEDGERENTRIESMt.AppendChild(TAXBILLALLOCATIONSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(TAXOBJECTALLOCATIONSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(TDSEXPENSEALLOCATIONSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(VATSTATUTORYDETAILSSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(COSTTRACKALLOCATIONSst);
        //                //ALLLEDGERENTRIESMt.AppendChild(REFVOUCHERDETAILS);
        //                //ALLLEDGERENTRIESMt.AppendChild(INVOICEWISEDETAILS);
        //                //ALLLEDGERENTRIESMt.AppendChild(VATITCDETAILS);
        //                //ALLLEDGERENTRIESMt.AppendChild(ADVANCETAXDETAILS);
        //                //VOUCHER.AppendChild(ALLLEDGERENTRIESMt);
        //                //

        //                XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
        //                VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
        //                XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
        //                VOUCHER.AppendChild(ATTDRECORDS);
        //                XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
        //                VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
        //                XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
        //                VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
        //                XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
        //                VOUCHER.AppendChild(TEMPGSTRATEDETAILS);
        //                TALLYMESSAGE.AppendChild(VOUCHER);
        //                REQUESTDATA.AppendChild(TALLYMESSAGE);
        //                dc.CashPayment_Update_TallyStatus(Convert.ToInt32(lblBookingNo.Text), true);
        //            }

        //        }
        //    }
        //    #endregion
        //    RootNode.AppendChild(HEADER);
        //    IMPORTDATA.AppendChild(REQUESTDESC);
        //    IMPORTDATA.AppendChild(REQUESTDATA);
        //    BODY.AppendChild(IMPORTDATA);
        //    RootNode.AppendChild(BODY);

        //    if (!Directory.Exists(@"d:\xml"))
        //        Directory.CreateDirectory(@"d:\xml");
        //    string fileName = "";
        //    if (cnStr.ToLower().Contains("mumbai") == true)
        //        fileName = "VendorCashPayment_Mumbai";
        //    else if (cnStr.ToLower().Contains("nashik") == true)
        //        fileName = "VendorCashPayment_Nashik";
        //    else
        //        fileName = "VendorCashPayment_Pune";
        //    xmldoc.Save(@"d:\xml\" + fileName + ".xml");
        //    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        //    response.ClearContent();
        //    response.Clear();
        //    //response.ContentType = "text/plain";
        //    response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
        //    response.TransmitFile("d:\\xml\\" + fileName + ".xml");
        //    response.Flush();
        //    response.End();
        //}
        private void BillTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region bill
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");
                    Label lblNarration = (Label)grdBill.Rows[i].FindControl("lblNarration");
                    Label lblAmt = (Label)grdBill.Rows[i].FindControl("lblAmt");
                    Label lblServiceTaxAmt = (Label)grdBill.Rows[i].FindControl("lblServiceTaxAmt");
                    Label lblTestType = (Label)grdBill.Rows[i].FindControl("lblTestType");
                    Label lblServiceTax = (Label)grdBill.Rows[i].FindControl("lblServiceTax");
                    Label lblBillStatus = (Label)grdBill.Rows[i].FindControl("lblBillStatus");
                    Label lblTravelling = (Label)grdBill.Rows[i].FindControl("lblTravelling");
                    Label lblDiscountAmt = (Label)grdBill.Rows[i].FindControl("lblDiscountAmt");
                    Label lblMktUser = (Label)grdBill.Rows[i].FindControl("lblMktUser");
                    Label lblRoundingOff = (Label)grdBill.Rows[i].FindControl("lblRoundingOff");
                    Label lblMobilization = (Label)grdBill.Rows[i].FindControl("lblMobilization");
                    Label lblSwachhBharatCess = (Label)grdBill.Rows[i].FindControl("lblSwachhBharatCess");
                    Label lblSwachhBharatCessAmt = (Label)grdBill.Rows[i].FindControl("lblSwachhBharatCessAmt");
                    Label lblKisanKrishiCess = (Label)grdBill.Rows[i].FindControl("lblKisanKrishiCess");
                    Label lblKisanKrishiCessAmt = (Label)grdBill.Rows[i].FindControl("lblKisanKrishiCessAmt");
                    Label lblCgst = (Label)grdBill.Rows[i].FindControl("lblCgst");
                    Label lblCgstAmt = (Label)grdBill.Rows[i].FindControl("lblCgstAmt");
                    Label lblSgst = (Label)grdBill.Rows[i].FindControl("lblSgst");
                    Label lblSgstAmt = (Label)grdBill.Rows[i].FindControl("lblSgstAmt");
                    Label lblIgst = (Label)grdBill.Rows[i].FindControl("lblIgst");
                    Label lblIgstAmt = (Label)grdBill.Rows[i].FindControl("lblIgstAmt");

                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    VOUCHER.SetAttribute("REMOTEID", "");
                    VOUCHER.SetAttribute("VCHKEY", "");
                    VOUCHER.SetAttribute("VCHTYPE", "Sales");
                    VOUCHER.SetAttribute("ACTION", "Create");
                    VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                    XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                    OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                    XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                    OLDAUDITENTRYIDSf.InnerText = "-1";
                    OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                    VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                    XmlElement GUID = xmldoc.CreateElement("GUID");
                    XmlElement DATE = xmldoc.CreateElement("DATE");
                    DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                    DATE.InnerText = date.ToString("yyyyMMdd");  //"20150727"
                    VOUCHER.AppendChild(DATE);
                    GUID.InnerText = "";
                    VOUCHER.AppendChild(GUID);

                    XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                    string strNarration = "";
                    strNarration = lblNarration.Text + " AT SITE " + lblSiteName.Text + lblMktUser.Text;
                    strNarration = strNarration.Replace("&", "AND");

                    NARRATION.InnerText = strNarration;
                    VOUCHER.AppendChild(NARRATION);

                    XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                    VOUCHERTYPENAME.InnerText = "Sales";//
                    VOUCHER.AppendChild(VOUCHERTYPENAME);

                    XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                    if (lblBillNo.Text.Contains("-") == true)
                        VOUCHERNUMBER.InnerText = "DT - " + lblBillNo.Text.Replace("-", "");
                    else
                    {
                        int bl = 0;
                        if (Int32.TryParse(lblBillNo.Text, out bl) == true)
                            VOUCHERNUMBER.InnerText = "DT - " + lblBillNo.Text;
                        else
                            VOUCHERNUMBER.InnerText = lblBillNo.Text;
                    }
                    VOUCHER.AppendChild(VOUCHERNUMBER);

                    XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                    XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                    XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                    PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                    VOUCHER.AppendChild(PARTYLEDGERNAME);
                    VOUCHER.AppendChild(CSTFORMISSUETYPE);
                    VOUCHER.AppendChild(CSTFORMRECVTYPE);

                    XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                    FBTPAYMENTTYPE.InnerText = "Default";
                    VOUCHER.AppendChild(FBTPAYMENTTYPE);

                    XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                    XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                    PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                    VOUCHER.AppendChild(PERSISTEDVIEW);
                    VOUCHER.AppendChild(VCHGSTCLASS);

                    XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                    ENTEREDBY.InnerText = "DESPL";
                    VOUCHER.AppendChild(ENTEREDBY);

                    XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                    DIFFACTUALQTY.InnerText = "No";
                    VOUCHER.AppendChild(DIFFACTUALQTY);

                    XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                    AUDITED.InnerText = "No";
                    VOUCHER.AppendChild(AUDITED);

                    XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                    FORJOBCOSTING.InnerText = "No";
                    VOUCHER.AppendChild(FORJOBCOSTING);

                    XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                    ISOPTIONAL.InnerText = "No";
                    VOUCHER.AppendChild(ISOPTIONAL);

                    XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                    DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                    EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                    VOUCHER.AppendChild(EFFECTIVEDATE);

                    XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                    ISFORJOBWORKIN.InnerText = "No";
                    VOUCHER.AppendChild(ISFORJOBWORKIN);

                    XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                    ALLOWCONSUMPTION.InnerText = "No";
                    VOUCHER.AppendChild(ALLOWCONSUMPTION);

                    XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                    USEFORINTEREST.InnerText = "No";
                    VOUCHER.AppendChild(USEFORINTEREST);

                    XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                    USEFORGAINLOSS.InnerText = "No";
                    VOUCHER.AppendChild(USEFORGAINLOSS);

                    XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                    USEFORGODOWNTRANSFER.InnerText = "No";
                    VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                    XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                    XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                    USEFORCOMPOUND.InnerText = "No";
                    VOUCHER.AppendChild(USEFORCOMPOUND);
                    ALTERID.InnerText = "";
                    VOUCHER.AppendChild(ALTERID);

                    XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                    EXCISEOPENING.InnerText = "No";
                    VOUCHER.AppendChild(EXCISEOPENING);

                    XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                    USEFORFINALPRODUCTION.InnerText = "No";
                    VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                    XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                    ISCANCELLED.InnerText = "No";
                    VOUCHER.AppendChild(ISCANCELLED);

                    XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                    HASCASHFLOW.InnerText = "No";
                    VOUCHER.AppendChild(HASCASHFLOW);

                    XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                    ISPOSTDATED.InnerText = "No";
                    VOUCHER.AppendChild(ISPOSTDATED);

                    XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                    USETRACKINGNUMBER.InnerText = "No";
                    VOUCHER.AppendChild(USETRACKINGNUMBER);

                    XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                    ISINVOICE.InnerText = "No";
                    VOUCHER.AppendChild(ISINVOICE);

                    XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                    MFGJOURNAL.InnerText = "No";
                    VOUCHER.AppendChild(MFGJOURNAL);

                    XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                    HASDISCOUNTS.InnerText = "No";
                    VOUCHER.AppendChild(HASDISCOUNTS);

                    XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                    ASPAYSLIP.InnerText = "No";
                    VOUCHER.AppendChild(ASPAYSLIP);

                    XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                    ISCOSTCENTRE.InnerText = "No";
                    VOUCHER.AppendChild(ISCOSTCENTRE);

                    XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                    ISSTXNONREALIZEDVCH.InnerText = "No";
                    VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                    XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                    ISEXCISEMANUFACTURERON.InnerText = "No";
                    VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                    XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                    ISBLANKCHEQUE.InnerText = "No";
                    VOUCHER.AppendChild(ISBLANKCHEQUE);

                    XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                    ISDELETED.InnerText = "No";
                    VOUCHER.AppendChild(ISDELETED);

                    XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                    ASORIGINAL.InnerText = "No";
                    VOUCHER.AppendChild(ASORIGINAL);

                    XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                    VCHISFROMSYNC.InnerText = "No";
                    VOUCHER.AppendChild(VCHISFROMSYNC);

                    XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                    XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");

                    XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                    XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                    XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                    XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                    XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                    XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                    XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");

                    MASTERID.InnerText = "";
                    VOUCHER.AppendChild(MASTERID);

                    VOUCHERKEY.InnerText = "";
                    VOUCHER.AppendChild(VOUCHERKEY);

                    VOUCHER.AppendChild(OLDAUDITENTRIES);
                    VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                    VOUCHER.AppendChild(AUDITENTRIES);
                    VOUCHER.AppendChild(INVOICEDELNOTES);
                    VOUCHER.AppendChild(INVOICEORDERLIST);
                    VOUCHER.AppendChild(INVOICEINDENTLIST);
                    VOUCHER.AppendChild(ATTENDANCEENTRIES);
                    //VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                    //VOUCHER.AppendChild(INVOICEEXPORTLIST);

                    #region bill no with client name
                    XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                    XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                    OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                    XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                    OLDAUDITENTRYIDSm.InnerText = "-1";
                    OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                    ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                    XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                    XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                    LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                    ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                    ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                    XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                    ISDEEMEDPOSITIVE.InnerText = "Yes";
                    ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                    XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                    LEDGERFROMITEM.InnerText = "No";
                    ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                    XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                    REMOVEZEROENTRIES.InnerText = "No";
                    ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                    XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                    ISPARTYLEDGER.InnerText = "Yes";
                    ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                    XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                    ISLASTDEEMEDPOSITIVE.InnerText = "Yes";
                    ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                    XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                    XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                    AMOUNT.InnerText = "-" + lblBillAmount.Text;
                    ALLLEDGERENTRIESmain.AppendChild(AMOUNT);
                    ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);

                    XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                    XmlElement NAME = xmldoc.CreateElement("NAME");
                    if (lblBillNo.Text.Contains("-") == true)
                        NAME.InnerText = "DT - " + lblBillNo.Text.Replace("-", "");
                    else
                    {
                        int bl = 0;
                        if (Int32.TryParse(lblBillNo.Text, out bl) == true)
                            NAME.InnerText = "DT - " + lblBillNo.Text;
                        else
                            NAME.InnerText = lblBillNo.Text;

                    }
                    BILLALLOCATIONS.AppendChild(NAME);

                    XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                    BILLTYPE.InnerText = "New Ref";
                    BILLALLOCATIONS.AppendChild(BILLTYPE);

                    XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                    TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                    BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                    XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                    AMT.InnerText = "-" + lblBillAmount.Text;
                    BILLALLOCATIONS.AppendChild(AMT);

                    XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                    BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);//changes done
                    XmlElement INTERESTCOLLECTIONLm = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                    XmlElement OLDAUDITENTRIESS = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                    XmlElement ACCOUNTAUDITENTRIESs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                    XmlElement AUDITENTRIESs = xmldoc.CreateElement("AUDITENTRIES.LIST");
                    XmlElement TAXBILLALLOCATIONS = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                    XmlElement TAXOBJECTALLOCATIONS = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                    XmlElement TDSEXPENSEALLOCATIONS = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                    XmlElement VATSTATUTORYDETAILS = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                    XmlElement COSTTRACKALLOCATIONS = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                    ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(INTERESTCOLLECTIONLm);
                    ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRIESS);
                    ALLLEDGERENTRIESmain.AppendChild(ACCOUNTAUDITENTRIESs);
                    ALLLEDGERENTRIESmain.AppendChild(AUDITENTRIESs);
                    ALLLEDGERENTRIESmain.AppendChild(TAXBILLALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(TAXOBJECTALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(TDSEXPENSEALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(VATSTATUTORYDETAILS);
                    ALLLEDGERENTRIESmain.AppendChild(COSTTRACKALLOCATIONS);
                    VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                    #endregion

                    if (lblTestType.Text == "Monthly")
                    {
                        var billd = dc.BillDetail_View(lblBillNo.Text);
                        foreach (var bd in billd)
                        {
                            #region bill amount with test type
                            XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                            XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                            OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                            XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                            OLDAUDITENTRYIDSs.InnerText = "-1";
                            OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                            ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                            XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                            XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                            string tmpTestType = getTestType(bd.RecordType, "");
                            if (tmpTestType == "Testing - Material")
                                LEDGERNAMEe.InnerText = "Testing Fees";
                            else
                                LEDGERNAMEe.InnerText = tmpTestType;
                            ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                            ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                            XmlElement ISDEEMEDPOSITIVEe = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                            ISDEEMEDPOSITIVEe.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEe);

                            XmlElement LEDGERFROMITEMe = xmldoc.CreateElement("LEDGERFROMITEM");
                            LEDGERFROMITEMe.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMe);

                            XmlElement REMOVEZEROENTRIESs = xmldoc.CreateElement("REMOVEZEROENTRIES");
                            REMOVEZEROENTRIESs.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESs);

                            XmlElement ISPARTYLEDGERr = xmldoc.CreateElement("ISPARTYLEDGER");
                            ISPARTYLEDGERr.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERr);

                            XmlElement ISLASTDEEMEDPOSITIVEe = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                            ISLASTDEEMEDPOSITIVEe.InnerText = "No";
                            ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEe);

                            XmlElement AMOUNTa = xmldoc.CreateElement("AMOUNT");
                            //if (lblDiscountAmt.Text != "")
                            //    AMOUNTa.InnerText = (Convert.ToDouble(lblAmt.Text) - Convert.ToDouble(lblDiscountAmt.Text)).ToString();
                            //else
                            //    AMOUNTa.InnerText = lblAmt.Text;
                            AMOUNTa.InnerText = bd.BILLD_Rate_num.ToString();
                            ALLLEDGERENTRIESM.AppendChild(AMOUNTa);

                            XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                            XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                            if (cnStr.ToLower().Contains("mumbai") == true)
                                CATEGORY.InnerText = "Mumbai";
                            else if (cnStr.ToLower().Contains("nashik") == true)
                                CATEGORY.InnerText = "Nashik Lab";
                            else
                                CATEGORY.InnerText = "S R LAB"; //S R LAB, Mumbai, Nashik Lab

                            CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                            ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);

                            XmlElement COSTCENTREALLOCATIONSn = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                            XmlElement NAMEE = xmldoc.CreateElement("NAME");
                            if (tmpTestType.Contains("Cube") == true)
                                NAMEE.InnerText = ("Cube Testing").ToUpper();
                            else if (tmpTestType.Contains("Pile Testing") == true)
                                NAMEE.InnerText = tmpTestType;
                            else
                                NAMEE.InnerText = tmpTestType.ToUpper();
                            COSTCENTREALLOCATIONSn.AppendChild(NAMEE);

                            XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                            //if (lblDiscountAmt.Text != "")
                            //    AMOUNTc.InnerText = (Convert.ToDouble(lblAmt.Text) - Convert.ToDouble(lblDiscountAmt.Text)).ToString();
                            //else
                            //    AMOUNTc.InnerText = lblAmt.Text;
                            AMOUNTc.InnerText = bd.BILLD_Rate_num.ToString();
                            COSTCENTREALLOCATIONSn.AppendChild(AMOUNTc);
                            CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONSn);
                            ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);

                            XmlElement BANKALLOCATIONS3rd = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                            XmlElement BILLALLOCATIONS3rd = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement INTERESTCOLLECTION3rd = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            XmlElement OLDAUDITENTRIES3rd = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                            XmlElement ACCOUNTAUDITENTRIES3rd = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                            XmlElement AUDITENTRIES3rd = xmldoc.CreateElement("AUDITENTRIES.LIST");
                            XmlElement TAXBILLALLOCATIONS3rd = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                            XmlElement TAXOBJECTALLOCATIONS3rd = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                            XmlElement TDSEXPENSEALLOCATIONS3rd = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                            XmlElement VATSTATUTORYDETAILS3rd = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                            XmlElement COSTTRACKALLOCATIONS3rd = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                            ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONS3rd);
                            ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONS3rd);
                            ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTION3rd);
                            ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIES3rd);
                            ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIES3rd);
                            ALLLEDGERENTRIESM.AppendChild(AUDITENTRIES3rd);
                            ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONS3rd);
                            ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONS3rd);
                            ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONS3rd);
                            ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILS3rd);
                            ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONS3rd);
                            VOUCHER.AppendChild(ALLLEDGERENTRIESM);
                            //
                            #endregion
                        }
                    }
                    else
                    {
                        #region bill amount with test type
                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                        if (lblTestType.Text == "Testing - Material")
                            LEDGERNAMEe.InnerText = "Testing Fees";
                        else
                            LEDGERNAMEe.InnerText = lblTestType.Text;
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEe = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEe.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEe);

                        XmlElement LEDGERFROMITEMe = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMe.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMe);

                        XmlElement REMOVEZEROENTRIESs = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESs.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESs);

                        XmlElement ISPARTYLEDGERr = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERr.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERr);

                        XmlElement ISLASTDEEMEDPOSITIVEe = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEe.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEe);

                        XmlElement AMOUNTa = xmldoc.CreateElement("AMOUNT");
                        if (lblDiscountAmt.Text != "")
                            AMOUNTa.InnerText = (Convert.ToDouble(lblAmt.Text) - Convert.ToDouble(lblDiscountAmt.Text)).ToString();
                        else
                            AMOUNTa.InnerText = lblAmt.Text;
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTa);

                        XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                        XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            CATEGORY.InnerText = "Mumbai";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            CATEGORY.InnerText = "Nashik Lab";
                        else
                            CATEGORY.InnerText = "S R LAB"; //S R LAB, Mumbai, Nashik Lab

                        CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                        //XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        //ISDEEMEDPOSITIVEa.InnerText = "No";
                        //CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEa);
                        ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);

                        XmlElement COSTCENTREALLOCATIONSn = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                        XmlElement NAMEE = xmldoc.CreateElement("NAME");
                        if (lblTestType.Text.Contains("Cube") == true)
                            NAMEE.InnerText = ("Cube Testing").ToUpper();
                        else if (lblTestType.Text.Contains("Pile Testing") == true)
                            NAMEE.InnerText = lblTestType.Text;
                        else
                            NAMEE.InnerText = lblTestType.Text.ToUpper();
                        COSTCENTREALLOCATIONSn.AppendChild(NAMEE);

                        XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                        if (lblDiscountAmt.Text != "")
                            AMOUNTc.InnerText = (Convert.ToDouble(lblAmt.Text) - Convert.ToDouble(lblDiscountAmt.Text)).ToString();
                        else
                            AMOUNTc.InnerText = lblAmt.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(AMOUNTc);
                        CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONSn);
                        ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);

                        XmlElement BANKALLOCATIONS3rd = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement BILLALLOCATIONS3rd = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTION3rd = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIES3rd = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES3rd = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES3rd = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement TAXBILLALLOCATIONS3rd = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONS3rd = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONS3rd = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILS3rd = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONS3rd = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONS3rd);
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONS3rd);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTION3rd);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIES3rd);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIES3rd);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIES3rd);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONS3rd);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONS3rd);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONS3rd);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILS3rd);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONS3rd);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);
                        //
                        #endregion
                    }
                    XmlElement ALLLEDGERENTRIESMn;
                    XmlElement OLDAUDITENTRYIDSL;
                    XmlElement OLDAUDITENTRYIDSy;
                    XmlElement GSTCLASSq;
                    XmlElement LEDGERNAMEee;
                    XmlElement ISDEEMEDPOSITIVEq;
                    XmlElement LEDGERFROMITEMa;
                    XmlElement REMOVEZEROENTRIESq;
                    XmlElement ISPARTYLEDGERw;
                    XmlElement ISLASTDEEMEDPOSITIVEr;
                    XmlElement AMOUNTav;
                    XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                    XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                    XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                    XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                    XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                    XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                    XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                    XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                    XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                    XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                    XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                    #region travelling
                    //Travelling
                    if (lblTravelling.Text != "" && lblTravelling.Text != "0")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Travelling Charges";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblTravelling.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                        XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            CATEGORY.InnerText = "Mumbai";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            CATEGORY.InnerText = "Nashik Lab";
                        else
                            CATEGORY.InnerText = "S R LAB"; //S R LAB, Mumbai, Nashik Lab
                        CATEGORYALLOCATIONS.AppendChild(CATEGORY);
                        ALLLEDGERENTRIESMn.AppendChild(CATEGORYALLOCATIONS);

                        XmlElement COSTCENTREALLOCATIONSn = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                        XmlElement NAMEE = xmldoc.CreateElement("NAME");
                        NAMEE.InnerText = lblTestType.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(NAMEE);

                        XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                        AMOUNTc.InnerText = lblTravelling.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(AMOUNTc);
                        CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONSn);

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region Mobilization
                    //Mobilization
                    if (lblMobilization.Text != "" && lblMobilization.Text != "0")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Transport Reimbursement";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblMobilization.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                        XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            CATEGORY.InnerText = "Mumbai";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            CATEGORY.InnerText = "Nashik Lab";
                        else
                            CATEGORY.InnerText = "S R LAB"; //S R LAB, Mumbai, Nashik Lab
                        CATEGORYALLOCATIONS.AppendChild(CATEGORY);
                        ALLLEDGERENTRIESMn.AppendChild(CATEGORYALLOCATIONS);

                        XmlElement COSTCENTREALLOCATIONSn = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                        XmlElement NAMEE = xmldoc.CreateElement("NAME");
                        NAMEE.InnerText = lblTestType.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(NAMEE);

                        XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                        AMOUNTc.InnerText = lblMobilization.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(AMOUNTc);
                        CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONSn);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region serv tax
                    //Service tax
                    if (lblServiceTaxAmt.Text != "" && lblServiceTaxAmt.Text != "0"
                        && lblServiceTaxAmt.Text != "0.00" && lblServiceTaxAmt.Text != "0.000")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Service Tax " + lblServiceTax.Text + " %";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblServiceTaxAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region Swachh Bharat Cess
                    //Swachh Bharat Cess
                    if (lblSwachhBharatCessAmt.Text != "" && lblSwachhBharatCessAmt.Text != "0" && lblSwachhBharatCessAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Swachh Bharat Cess " + lblSwachhBharatCess.Text + " %";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblSwachhBharatCessAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region Kisan Krishi Cess
                    //Kisan Krishi Cess
                    if (lblKisanKrishiCessAmt.Text != "" && lblKisanKrishiCessAmt.Text != "0" && lblKisanKrishiCessAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Kisan Krishi Cess " + lblKisanKrishiCess.Text + " %";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblKisanKrishiCessAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region CGST
                    //CGST
                    if (lblCgstAmt.Text != "" && lblCgstAmt.Text != "0" && lblCgstAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        //LEDGERNAMEee.InnerText = "CGST " + Convert.ToInt32(lblCgst.Text) + "%";
                        LEDGERNAMEee.InnerText = "CGST " + Convert.ToDecimal(lblCgst.Text).ToString("0.##") + "%";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblCgstAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region SGST
                    //SGST
                    if (lblSgstAmt.Text != "" && lblSgstAmt.Text != "0" && lblSgstAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        //LEDGERNAMEee.InnerText = "SGST " + Convert.ToInt32(lblSgst.Text) + "%";
                        LEDGERNAMEee.InnerText = "SGST " + Convert.ToDecimal(lblSgst.Text).ToString("0.##") + "%";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblSgstAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region IGST
                    //IGST
                    if (lblIgstAmt.Text != "" && lblIgstAmt.Text != "0" && lblIgstAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        //LEDGERNAMEee.InnerText = "IGST " + Convert.ToInt32(lblIgst.Text) + "%";
                        LEDGERNAMEee.InnerText = "IGST " + Convert.ToDecimal(lblIgst.Text).ToString("0.##") + "%";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblIgstAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region round off
                    //Rouning off
                    if (lblRoundingOff.Text != "" && lblRoundingOff.Text != "0" && lblRoundingOff.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Round Off";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        if (Convert.ToDouble(lblRoundingOff.Text) < 0)
                            ISDEEMEDPOSITIVEq.InnerText = "Yes";
                        else
                            ISDEEMEDPOSITIVEq.InnerText = "No";

                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");

                        if (Convert.ToDouble(lblRoundingOff.Text) < 0)
                            ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        else
                            ISLASTDEEMEDPOSITIVEr.InnerText = "No";

                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblRoundingOff.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("BANKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("BILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    //

                    XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                    VOUCHER.AppendChild(ATTDRECORDS);
                    TALLYMESSAGE.AppendChild(VOUCHER);
                    REQUESTDATA.AppendChild(TALLYMESSAGE);

                    XmlElement TALLYMESSAGE2 = xmldoc.CreateElement("TALLYMESSAGE");
                    TALLYMESSAGE2.SetAttribute("xmlns:UDF", "TallyUDF");
                    dc.Bill_Update_TallyStatus(lblBillNo.Text, true);
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);
            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "SalesVchr_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "SalesVchr_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "SalesVchr_Metro";
            else
                fileName = "SalesVchr_Pune";

            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
            LoadList();
        }
        
        private void BillTallyTransfer_Old()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");            
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region bill
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");
                    Label lblNarration = (Label)grdBill.Rows[i].FindControl("lblNarration");
                    Label lblAmt = (Label)grdBill.Rows[i].FindControl("lblAmt");
                    Label lblServiceTaxAmt = (Label)grdBill.Rows[i].FindControl("lblServiceTaxAmt");
                    Label lblTestType = (Label)grdBill.Rows[i].FindControl("lblTestType");
                    Label lblServiceTax = (Label)grdBill.Rows[i].FindControl("lblServiceTax");
                    Label lblBillStatus = (Label)grdBill.Rows[i].FindControl("lblBillStatus");
                    Label lblTravelling = (Label)grdBill.Rows[i].FindControl("lblTravelling");
                    Label lblDiscountAmt = (Label)grdBill.Rows[i].FindControl("lblDiscountAmt");
                    Label lblMktUser = (Label)grdBill.Rows[i].FindControl("lblMktUser");
                    Label lblRoundingOff = (Label)grdBill.Rows[i].FindControl("lblRoundingOff");
                    Label lblMobilization = (Label)grdBill.Rows[i].FindControl("lblMobilization");
                    Label lblSwachhBharatCess = (Label)grdBill.Rows[i].FindControl("lblSwachhBharatCess");
                    Label lblSwachhBharatCessAmt = (Label)grdBill.Rows[i].FindControl("lblSwachhBharatCessAmt");
                    Label lblKisanKrishiCess = (Label)grdBill.Rows[i].FindControl("lblKisanKrishiCess");
                    Label lblKisanKrishiCessAmt = (Label)grdBill.Rows[i].FindControl("lblKisanKrishiCessAmt");
                    Label lblCgst = (Label)grdBill.Rows[i].FindControl("lblCgst");
                    Label lblCgstAmt = (Label)grdBill.Rows[i].FindControl("lblCgstAmt");
                    Label lblSgst = (Label)grdBill.Rows[i].FindControl("lblSgst");
                    Label lblSgstAmt = (Label)grdBill.Rows[i].FindControl("lblSgstAmt");
                    Label lblIgst = (Label)grdBill.Rows[i].FindControl("lblIgst");
                    Label lblIgstAmt = (Label)grdBill.Rows[i].FindControl("lblIgstAmt");
                                        
                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    VOUCHER.SetAttribute("REMOTEID", "");
                    VOUCHER.SetAttribute("VCHKEY", "");
                    VOUCHER.SetAttribute("VCHTYPE", "Sales");
                    VOUCHER.SetAttribute("ACTION", "Create");
                    VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                    XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                    OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                    XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                    OLDAUDITENTRYIDSf.InnerText = "-1";
                    OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                    VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                    XmlElement GUID = xmldoc.CreateElement("GUID");
                    XmlElement DATE = xmldoc.CreateElement("DATE");
                    DateTime date = DateTime.ParseExact(lblBillDate.Text , "dd/MM/yyyy", null);
                    DATE.InnerText = date.ToString("yyyyMMdd");  //"20150727"
                    VOUCHER.AppendChild(DATE);                    
                    GUID.InnerText = "";
                    VOUCHER.AppendChild(GUID);

                    XmlElement NARRATION = xmldoc.CreateElement("NARRATION");                    
                    string strNarration = "";
                    strNarration = lblNarration.Text + " AT SITE " + lblSiteName.Text + lblMktUser.Text;
                    strNarration = strNarration.Replace("&", "AND");
                        
                    NARRATION.InnerText = strNarration;
                    VOUCHER.AppendChild(NARRATION);

                    XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                    VOUCHERTYPENAME.InnerText = "Sales";//
                    VOUCHER.AppendChild(VOUCHERTYPENAME);

                    XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                    if (lblBillNo.Text.Contains("-") == true)
                        VOUCHERNUMBER.InnerText = "DT - " + lblBillNo.Text.Replace("-", "");
                    else
                    {
                        int bl = 0;
                        if (Int32.TryParse(lblBillNo.Text, out bl) == true)
                            VOUCHERNUMBER.InnerText = "DT - " + lblBillNo.Text;
                        else
                            VOUCHERNUMBER.InnerText = lblBillNo.Text;
                    }
                    VOUCHER.AppendChild(VOUCHERNUMBER);

                    XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                    XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                    XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                    PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&","AND") ;
                    VOUCHER.AppendChild(PARTYLEDGERNAME);
                    VOUCHER.AppendChild(CSTFORMISSUETYPE);
                    VOUCHER.AppendChild(CSTFORMRECVTYPE);

                    XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                    FBTPAYMENTTYPE.InnerText = "Default";
                    VOUCHER.AppendChild(FBTPAYMENTTYPE);

                    XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                    XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                    PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                    VOUCHER.AppendChild(PERSISTEDVIEW);
                    VOUCHER.AppendChild(VCHGSTCLASS);

                    XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                    ENTEREDBY.InnerText = "DESPL";
                    VOUCHER.AppendChild(ENTEREDBY);

                    XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                    DIFFACTUALQTY.InnerText = "No";
                    VOUCHER.AppendChild(DIFFACTUALQTY);

                    XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                    AUDITED.InnerText = "No";
                    VOUCHER.AppendChild(AUDITED);

                    XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                    FORJOBCOSTING.InnerText = "No";
                    VOUCHER.AppendChild(FORJOBCOSTING);

                    XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                    ISOPTIONAL.InnerText = "No";
                    VOUCHER.AppendChild(ISOPTIONAL);

                    XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                    DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                    EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd"); 
                    VOUCHER.AppendChild(EFFECTIVEDATE);

                    XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                    ISFORJOBWORKIN.InnerText = "No";
                    VOUCHER.AppendChild(ISFORJOBWORKIN);

                    XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                    ALLOWCONSUMPTION.InnerText = "No";
                    VOUCHER.AppendChild(ALLOWCONSUMPTION);

                    XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                    USEFORINTEREST.InnerText = "No";
                    VOUCHER.AppendChild(USEFORINTEREST);

                    XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                    USEFORGAINLOSS.InnerText = "No";
                    VOUCHER.AppendChild(USEFORGAINLOSS);

                    XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                    USEFORGODOWNTRANSFER.InnerText = "No";
                    VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                    XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                    XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                    USEFORCOMPOUND.InnerText = "No";
                    VOUCHER.AppendChild(USEFORCOMPOUND);
                    ALTERID.InnerText = "";
                    VOUCHER.AppendChild(ALTERID);

                    XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                    EXCISEOPENING.InnerText = "No";
                    VOUCHER.AppendChild(EXCISEOPENING);

                    XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                    USEFORFINALPRODUCTION.InnerText = "No";
                    VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                    XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                    ISCANCELLED.InnerText = "No";
                    VOUCHER.AppendChild(ISCANCELLED);

                    XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                    HASCASHFLOW.InnerText = "No";
                    VOUCHER.AppendChild(HASCASHFLOW);

                    XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                    ISPOSTDATED.InnerText = "No";
                    VOUCHER.AppendChild(ISPOSTDATED);

                    XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                    USETRACKINGNUMBER.InnerText = "No";
                    VOUCHER.AppendChild(USETRACKINGNUMBER);

                    XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                    ISINVOICE.InnerText = "No";
                    VOUCHER.AppendChild(ISINVOICE);

                    XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                    MFGJOURNAL.InnerText = "No";
                    VOUCHER.AppendChild(MFGJOURNAL);

                    XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                    HASDISCOUNTS.InnerText = "No";
                    VOUCHER.AppendChild(HASDISCOUNTS);

                    XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                    ASPAYSLIP.InnerText = "No";
                    VOUCHER.AppendChild(ASPAYSLIP);

                    XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                    ISCOSTCENTRE.InnerText = "No";
                    VOUCHER.AppendChild(ISCOSTCENTRE);

                    XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                    ISSTXNONREALIZEDVCH.InnerText = "No";
                    VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                    XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                    ISEXCISEMANUFACTURERON.InnerText = "No";
                    VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                    XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                    ISBLANKCHEQUE.InnerText = "No";
                    VOUCHER.AppendChild(ISBLANKCHEQUE);

                    XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                    ISDELETED.InnerText = "No";
                    VOUCHER.AppendChild(ISDELETED);

                    XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                    ASORIGINAL.InnerText = "No";
                    VOUCHER.AppendChild(ASORIGINAL);

                    XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                    VCHISFROMSYNC.InnerText = "No";
                    VOUCHER.AppendChild(VCHISFROMSYNC);

                    XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                    XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");

                    XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                    XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                    XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                    XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                    XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                    XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                    XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                    
                    MASTERID.InnerText = "";
                    VOUCHER.AppendChild(MASTERID);

                    VOUCHERKEY.InnerText = "";
                    VOUCHER.AppendChild(VOUCHERKEY);

                    VOUCHER.AppendChild(OLDAUDITENTRIES);
                    VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                    VOUCHER.AppendChild(AUDITENTRIES);
                    VOUCHER.AppendChild(INVOICEDELNOTES);
                    VOUCHER.AppendChild(INVOICEORDERLIST);
                    VOUCHER.AppendChild(INVOICEINDENTLIST);
                    VOUCHER.AppendChild(ATTENDANCEENTRIES);
                    //VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                    //VOUCHER.AppendChild(INVOICEEXPORTLIST);

                    #region bill no with client name
                    XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                    XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                    OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                    XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                    OLDAUDITENTRYIDSm.InnerText = "-1";
                    OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                    ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                    XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                    XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                    LEDGERNAME.InnerText = lblClientName.Text.Replace("&","AND");
                    ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                    ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                    XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                    ISDEEMEDPOSITIVE.InnerText = "Yes";
                    ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                    XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                    LEDGERFROMITEM.InnerText = "No";
                    ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                    XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                    REMOVEZEROENTRIES.InnerText = "No";
                    ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                    XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                    ISPARTYLEDGER.InnerText = "Yes";
                    ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                    XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                    ISLASTDEEMEDPOSITIVE.InnerText = "Yes";
                    ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                    XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                    XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                    AMOUNT.InnerText = "-" + lblBillAmount.Text;
                    ALLLEDGERENTRIESmain.AppendChild(AMOUNT);
                    ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);

                    XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                    XmlElement NAME = xmldoc.CreateElement("NAME");
                    if (lblBillNo.Text.Contains("-") == true)
                        NAME.InnerText = "DT - " + lblBillNo.Text.Replace("-", "");
                    else
                    {
                        int bl = 0;
                        if (Int32.TryParse(lblBillNo.Text, out bl) == true)
                            NAME.InnerText = "DT - " + lblBillNo.Text;
                        else
                            NAME.InnerText = lblBillNo.Text;
                        
                    }
                    BILLALLOCATIONS.AppendChild(NAME);

                    XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                    BILLTYPE.InnerText = "New Ref";
                    BILLALLOCATIONS.AppendChild(BILLTYPE);

                    XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                    TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                    BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                    XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                    AMT.InnerText = "-" + lblBillAmount.Text;
                    BILLALLOCATIONS.AppendChild(AMT);

                    XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                    BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);//changes done
                    XmlElement INTERESTCOLLECTIONLm = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                    XmlElement OLDAUDITENTRIESS = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                    XmlElement ACCOUNTAUDITENTRIESs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                    XmlElement AUDITENTRIESs = xmldoc.CreateElement("AUDITENTRIES.LIST");
                    XmlElement TAXBILLALLOCATIONS = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                    XmlElement TAXOBJECTALLOCATIONS = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                    XmlElement TDSEXPENSEALLOCATIONS = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                    XmlElement VATSTATUTORYDETAILS = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                    XmlElement COSTTRACKALLOCATIONS = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                    ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(INTERESTCOLLECTIONLm);
                    ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRIESS);
                    ALLLEDGERENTRIESmain.AppendChild(ACCOUNTAUDITENTRIESs);
                    ALLLEDGERENTRIESmain.AppendChild(AUDITENTRIESs);
                    ALLLEDGERENTRIESmain.AppendChild(TAXBILLALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(TAXOBJECTALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(TDSEXPENSEALLOCATIONS);
                    ALLLEDGERENTRIESmain.AppendChild(VATSTATUTORYDETAILS);
                    ALLLEDGERENTRIESmain.AppendChild(COSTTRACKALLOCATIONS);
                    VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                    #endregion

                    #region bill amount with test type
                    XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                    XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                    OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                    XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                    OLDAUDITENTRYIDSs.InnerText = "-1";
                    OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                    ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                    XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                    XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                    if (lblTestType.Text == "Testing - Material")
                        LEDGERNAMEe.InnerText = "Testing Fees";
                    else
                        LEDGERNAMEe.InnerText = lblTestType.Text;
                    ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                    ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                    XmlElement ISDEEMEDPOSITIVEe = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                    ISDEEMEDPOSITIVEe.InnerText = "No";
                    ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEe);

                    XmlElement LEDGERFROMITEMe = xmldoc.CreateElement("LEDGERFROMITEM");
                    LEDGERFROMITEMe.InnerText = "No";
                    ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMe);

                    XmlElement REMOVEZEROENTRIESs = xmldoc.CreateElement("REMOVEZEROENTRIES");
                    REMOVEZEROENTRIESs.InnerText = "No";
                    ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESs);

                    XmlElement ISPARTYLEDGERr = xmldoc.CreateElement("ISPARTYLEDGER");
                    ISPARTYLEDGERr.InnerText = "No";
                    ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERr);

                    XmlElement ISLASTDEEMEDPOSITIVEe = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                    ISLASTDEEMEDPOSITIVEe.InnerText = "No";
                    ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEe);

                    XmlElement AMOUNTa = xmldoc.CreateElement("AMOUNT");
                    if (lblDiscountAmt.Text != "")
                        AMOUNTa.InnerText = (Convert.ToDouble(lblAmt.Text) - Convert.ToDouble(lblDiscountAmt.Text)).ToString();
                    else
                        AMOUNTa.InnerText = lblAmt.Text;
                    ALLLEDGERENTRIESM.AppendChild(AMOUNTa);

                    XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                    XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                    if (cnStr.ToLower().Contains("mumbai") == true)
                        CATEGORY.InnerText = "Mumbai";
                    else if (cnStr.ToLower().Contains("nashik") == true)
                        CATEGORY.InnerText = "Nashik Lab"; 
                    else
                        CATEGORY.InnerText = "S R LAB"; //S R LAB, Mumbai, Nashik Lab

                    CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                    //XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                    //ISDEEMEDPOSITIVEa.InnerText = "No";
                    //CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEa);
                    ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);

                    XmlElement COSTCENTREALLOCATIONSn = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                    XmlElement NAMEE = xmldoc.CreateElement("NAME");
                    if(lblTestType.Text.Contains("Cube") == true)
                        NAMEE.InnerText = ("Cube Testing").ToUpper();
                    else if (lblTestType.Text.Contains("Pile Testing") == true)
                        NAMEE.InnerText = lblTestType.Text;
                    else
                        NAMEE.InnerText = lblTestType.Text.ToUpper();
                    COSTCENTREALLOCATIONSn.AppendChild(NAMEE);

                    XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                    if (lblDiscountAmt.Text != "")
                        AMOUNTc.InnerText = (Convert.ToDouble(lblAmt.Text) - Convert.ToDouble(lblDiscountAmt.Text)).ToString();
                    else
                        AMOUNTc.InnerText = lblAmt.Text;                    
                    COSTCENTREALLOCATIONSn.AppendChild(AMOUNTc);
                    CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONSn);
                    ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);

                    XmlElement BANKALLOCATIONS3rd = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                    XmlElement BILLALLOCATIONS3rd = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                    XmlElement INTERESTCOLLECTION3rd = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                    XmlElement OLDAUDITENTRIES3rd = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                    XmlElement ACCOUNTAUDITENTRIES3rd = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                    XmlElement AUDITENTRIES3rd = xmldoc.CreateElement("AUDITENTRIES.LIST");
                    XmlElement TAXBILLALLOCATIONS3rd = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                    XmlElement TAXOBJECTALLOCATIONS3rd = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                    XmlElement TDSEXPENSEALLOCATIONS3rd = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                    XmlElement VATSTATUTORYDETAILS3rd = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                    XmlElement COSTTRACKALLOCATIONS3rd = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                    ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONS3rd);
                    ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONS3rd);
                    ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTION3rd);
                    ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIES3rd);
                    ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIES3rd);
                    ALLLEDGERENTRIESM.AppendChild(AUDITENTRIES3rd);
                    ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONS3rd);
                    ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONS3rd);
                    ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONS3rd);
                    ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILS3rd);
                    ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONS3rd);
                    VOUCHER.AppendChild(ALLLEDGERENTRIESM);
                    //
                    #endregion

                    XmlElement ALLLEDGERENTRIESMn;
                    XmlElement OLDAUDITENTRYIDSL;
                    XmlElement OLDAUDITENTRYIDSy;
                    XmlElement GSTCLASSq;
                    XmlElement LEDGERNAMEee;
                    XmlElement ISDEEMEDPOSITIVEq;
                    XmlElement LEDGERFROMITEMa;
                    XmlElement REMOVEZEROENTRIESq;
                    XmlElement ISPARTYLEDGERw;
                    XmlElement ISLASTDEEMEDPOSITIVEr;
                    XmlElement AMOUNTav;
                    XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                    XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                    XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                    XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                    XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                    XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                    XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                    XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                    XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                    XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                    XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                    #region travelling
                    //Travelling
                    if (lblTravelling.Text != "" && lblTravelling.Text != "0")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Travelling Charges";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");                        
                        AMOUNTav.InnerText = lblTravelling.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                        CATEGORY = xmldoc.CreateElement("CATEGORY");
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            CATEGORY.InnerText = "Mumbai";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            CATEGORY.InnerText = "Nashik Lab";
                        else
                            CATEGORY.InnerText = "S R LAB"; //S R LAB, Mumbai, Nashik Lab
                        CATEGORYALLOCATIONS.AppendChild(CATEGORY);
                        ALLLEDGERENTRIESMn.AppendChild(CATEGORYALLOCATIONS);

                        COSTCENTREALLOCATIONSn = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                        NAMEE = xmldoc.CreateElement("NAME");
                        NAMEE.InnerText = lblTestType.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(NAMEE);

                        AMOUNTc = xmldoc.CreateElement("AMOUNT");                        
                        AMOUNTc.InnerText = lblTravelling.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(AMOUNTc);
                        CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONSn);
                        
                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region Mobilization
                    //Mobilization
                    if (lblMobilization.Text != "" && lblMobilization.Text != "0")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Transport Reimbursement";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblMobilization.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                        CATEGORY = xmldoc.CreateElement("CATEGORY");
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            CATEGORY.InnerText = "Mumbai";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            CATEGORY.InnerText = "Nashik Lab";
                        else
                            CATEGORY.InnerText = "S R LAB"; //S R LAB, Mumbai, Nashik Lab
                        CATEGORYALLOCATIONS.AppendChild(CATEGORY);
                        ALLLEDGERENTRIESMn.AppendChild(CATEGORYALLOCATIONS);

                        COSTCENTREALLOCATIONSn = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                        NAMEE = xmldoc.CreateElement("NAME");
                        NAMEE.InnerText = lblTestType.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(NAMEE);

                        AMOUNTc = xmldoc.CreateElement("AMOUNT");
                        AMOUNTc.InnerText = lblMobilization.Text;
                        COSTCENTREALLOCATIONSn.AppendChild(AMOUNTc);
                        CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONSn);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region serv tax
                    //Service tax
                    if (lblServiceTaxAmt.Text != "" && lblServiceTaxAmt.Text != "0" 
                        && lblServiceTaxAmt.Text != "0.00" && lblServiceTaxAmt.Text != "0.000")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Service Tax " + lblServiceTax.Text + " %";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblServiceTaxAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region Swachh Bharat Cess
                    //Swachh Bharat Cess
                    if (lblSwachhBharatCessAmt.Text != "" && lblSwachhBharatCessAmt.Text != "0" && lblSwachhBharatCessAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Swachh Bharat Cess " + lblSwachhBharatCess.Text + " %";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblSwachhBharatCessAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region Kisan Krishi Cess
                    //Kisan Krishi Cess
                    if (lblKisanKrishiCessAmt.Text != "" && lblKisanKrishiCessAmt.Text != "0" && lblKisanKrishiCessAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Kisan Krishi Cess " + lblKisanKrishiCess.Text + " %";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblKisanKrishiCessAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region CGST
                    //CGST
                    if (lblCgstAmt.Text != "" && lblCgstAmt.Text != "0" && lblCgstAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        //LEDGERNAMEee.InnerText = "CGST " + Convert.ToInt32(lblCgst.Text) + "%";
                        LEDGERNAMEee.InnerText = "CGST " + Convert.ToDecimal(lblCgst.Text).ToString("0.##") + "%";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblCgstAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region SGST
                    //SGST
                    if (lblSgstAmt.Text != "" && lblSgstAmt.Text != "0" && lblSgstAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        //LEDGERNAMEee.InnerText = "SGST " + Convert.ToInt32(lblSgst.Text) + "%";
                        LEDGERNAMEee.InnerText = "SGST " + Convert.ToDecimal(lblSgst.Text).ToString("0.##") + "%";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblSgstAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region IGST
                    //IGST
                    if (lblIgstAmt.Text != "" && lblIgstAmt.Text != "0" && lblIgstAmt.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        //LEDGERNAMEee.InnerText = "IGST " + Convert.ToInt32(lblIgst.Text) + "%";
                        LEDGERNAMEee.InnerText = "IGST " + Convert.ToDecimal(lblIgst.Text).ToString("0.##") + "%";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblIgstAmt.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESMn.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESMn.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESMn.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESMn.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESMn.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    #region round off
                    //Rouning off
                    if (lblRoundingOff.Text != "" && lblRoundingOff.Text != "0" && lblRoundingOff.Text != "0.00")
                    {
                        ALLLEDGERENTRIESMn = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        OLDAUDITENTRYIDSL = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSL.SetAttribute("TYPE", "Number");
                        OLDAUDITENTRYIDSy = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSy.InnerText = "-1";
                        OLDAUDITENTRYIDSL.AppendChild(OLDAUDITENTRYIDSy);
                        ALLLEDGERENTRIESMn.AppendChild(OLDAUDITENTRYIDSL);

                        GSTCLASSq = xmldoc.CreateElement("GSTCLASS");
                        LEDGERNAMEee = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEee.InnerText = "Round Off";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERNAMEee);
                        ALLLEDGERENTRIESMn.AppendChild(GSTCLASSq);

                        ISDEEMEDPOSITIVEq = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        if (Convert.ToDouble(lblRoundingOff.Text) < 0)
                            ISDEEMEDPOSITIVEq.InnerText = "Yes";
                        else
                            ISDEEMEDPOSITIVEq.InnerText = "No";

                        ALLLEDGERENTRIESMn.AppendChild(ISDEEMEDPOSITIVEq);

                        LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(LEDGERFROMITEMa);

                        REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(REMOVEZEROENTRIESq);

                        ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESMn.AppendChild(ISPARTYLEDGERw);

                        ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");

                        if (Convert.ToDouble(lblRoundingOff.Text) < 0)
                            ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        else
                            ISLASTDEEMEDPOSITIVEr.InnerText = "No";

                        ALLLEDGERENTRIESMn.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = lblRoundingOff.Text;
                        ALLLEDGERENTRIESMn.AppendChild(AMOUNTav);

                        BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");

                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("BANKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("BILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESMn.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMn);
                    }
                    #endregion
                    //
                    
                    XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                    VOUCHER.AppendChild(ATTDRECORDS);
                    TALLYMESSAGE.AppendChild(VOUCHER);
                    REQUESTDATA.AppendChild(TALLYMESSAGE);
                    
                    XmlElement TALLYMESSAGE2 = xmldoc.CreateElement("TALLYMESSAGE");
                    TALLYMESSAGE2.SetAttribute("xmlns:UDF", "TallyUDF");
                    dc.Bill_Update_TallyStatus(lblBillNo.Text, true); 
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);            
            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            xmldoc.Save(@"d:\xml\SalesVchr.xml");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + "SalesVchr.xml");
            response.TransmitFile("d:\\xml\\SalesVchr.xml");
            response.Flush();            
            response.End();
            LoadList();
        }

        private void ReceiptTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region receipt
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");

                    var cashrcpt = dc.Cash_View_Receipt(Convert.ToInt32(lblBillNo.Text), null, null);
                    foreach (var rcptd in cashrcpt)
                    {
                        XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

                        XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "RECEIPT");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        string NarrationStr = "";
                        if (rcptd.Cash_PaymentType_bit == false)
                        {
                            NarrationStr = NarrationStr + "AS PER CASH BOOK ";
                        }
                        else
                        {
                            NarrationStr = "Ch. No. " + Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
                            NarrationStr += " DT " + Convert.ToDateTime(rcptd.Cash_ChqDate_date).ToString("dd/MM/yyyy");
                            NarrationStr += " " + rcptd.Cash_BankName_var;
                            NarrationStr += " " + rcptd.Cash_Branch_var;
                        }
                        NarrationStr += rcptd.Cash_Note_var;
                        if (rcptd.Cash_CollectionDetail != null && rcptd.Cash_CollectionDetail != "")
                        {
                            string[] CollectionDetail = new string[1];
                            CollectionDetail = Convert.ToString(rcptd.Cash_CollectionDetail).Split('|');
                            NarrationStr += " " + Convert.ToString(CollectionDetail[0]);
                            NarrationStr += " - " + Convert.ToString(CollectionDetail[1]);
                        }
                        NARRATION.InnerText = NarrationStr;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement TAXUNITNAME = xmldoc.CreateElement("TAXUNITNAME");
                        TAXUNITNAME.InnerText = "Default Tax Unit";
                        VOUCHER.AppendChild(TAXUNITNAME);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (cnStr.ToLower().Contains("nashik") == true)
                            VOUCHERTYPENAME.InnerText = "Receipt A";
                        else
                            VOUCHERTYPENAME.InnerText = "Receipt"; // nashik- Receipt A

                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        if (DateTime.Now.Month <= 3)
                            NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                        else
                            NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                        //NarrationStr += "/" + lblBillNo.Text.Replace("-","");
                        NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
                        VOUCHERNUMBER.InnerText = NarrationStr;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);

                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        if (rcptd.Cash_PaymentType_bit == false)
                        {
                            XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
                            VOUCHERTYPEORIGNAME.InnerText = "Receipt";
                            VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);
                        }

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
                        ISMSTFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(ISMSTFROMSYNC);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
                        USEFOREXCISE.InnerText = "No";
                        VOUCHER.AppendChild(USEFOREXCISE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);


                        XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
                        USEFORSERVICETAX.InnerText = "No";
                        VOUCHER.AppendChild(USEFORSERVICETAX);

                        XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
                        ISEXCISEVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEVOUCHER);

                        XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
                        EXCISETAXOVERRIDE.InnerText = "No";
                        VOUCHER.AppendChild(EXCISETAXOVERRIDE);

                        XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
                        USEFORTAXUNITTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
                        ISTDSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSOVERRIDDEN);

                        XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
                        ISTCSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTCSOVERRIDDEN);

                        XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
                        ISTDSTCSCASHVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSTCSCASHVCH);

                        XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
                        INCLUDEADVPYMTVCH.InnerText = "No";
                        VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

                        XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
                        ISSUBWORKSCONTRACT.InnerText = "No";
                        VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

                        XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
                        ISVATOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISVATOVERRIDDEN);

                        XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
                        IGNOREORIGVCHDATE.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREORIGVCHDATE);

                        XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
                        ISVATPAIDATCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

                        XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
                        ISDECLAREDTOCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

                        XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
                        ISSERVICETAXOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

                        XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
                        ISISDVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISISDVOUCHER);

                        XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
                        ISEXCISEOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

                        XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
                        ISEXCISESUPPLYVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

                        XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
                        ISGSTOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISGSTOVERRIDDEN);


                        XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
                        GSTNOTEEXPORTED.InnerText = "No";
                        VOUCHER.AppendChild(GSTNOTEEXPORTED);

                        XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
                        ISVATPRINCIPALACCOUNT.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

                        XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
                        ISBOENOTAPPLICABLE.InnerText = "No";
                        VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

                        XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
                        ISSHIPPINGWITHINSTATE.InnerText = "No";
                        VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

                        XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
                        ISOVERSEASTOURISTTRANS.InnerText = "No";
                        VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "Yes";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
                        ISVOID.InnerText = "No";
                        VOUCHER.AppendChild(ISVOID);

                        XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
                        ISONHOLD.InnerText = "No";
                        VOUCHER.AppendChild(ISONHOLD);

                        XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
                        ORDERLINESTATUS.InnerText = "No";
                        VOUCHER.AppendChild(ORDERLINESTATUS);

                        XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
                        VATISAGNSTCANCSALES.InnerText = "No";
                        VOUCHER.AppendChild(VATISAGNSTCANCSALES);

                        XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
                        VATISPURCEXEMPTED.InnerText = "No";
                        VOUCHER.AppendChild(VATISPURCEXEMPTED);

                        XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
                        ISVATRESTAXINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISVATRESTAXINVOICE);

                        XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
                        VATISASSESABLECALCVCH.InnerText = "No";
                        VOUCHER.AppendChild(VATISASSESABLECALCVCH);

                        XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
                        ISVATDUTYPAID.InnerText = "Yes";
                        VOUCHER.AppendChild(ISVATDUTYPAID);

                        XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
                        ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
                        VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

                        XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
                        ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
                        VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
                        CHANGEVCHMODE.InnerText = "No";
                        VOUCHER.AppendChild(CHANGEVCHMODE);

                        //XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                        //VCHISFROMSYNC.InnerText = "No";
                        //VOUCHER.AppendChild(VCHISFROMSYNC);

                        //XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        //USEFORCOMPOUND.InnerText = "No";
                        //VOUCHER.AppendChild(USEFORCOMPOUND);


                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement EXCLUDEDTAXATIONS = xmldoc.CreateElement("EXCLUDEDTAXATIONS.LIST");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
                        XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                        XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
                        XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(EXCLUDEDTAXATIONS);
                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(DUTYHEADDETAILS);
                        VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
                        VOUCHER.AppendChild(EWAYBILLDETAILS);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);
                        VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                        VOUCHER.AppendChild(INVOICEEXPORTLIST);

                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);


                        XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTERED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);


                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

                        XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
                        VATEXPAMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNT);

                        XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILS);
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        //
                        int bno = 0;
                        var rcpt = dc.Cash_View(Convert.ToInt32(lblBillNo.Text), 0, true);
                        foreach (var rcptdt in rcpt)
                        {

                            XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement NAME = xmldoc.CreateElement("NAME");
                            XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                            if (rcptdt.CashDetail_Settlement_var == "On A/c")
                            {
                                BILLTYPE.InnerText = "On Account";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            //else if (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) != 0)
                            else if (int.TryParse(rcptdt.CashDetail_BillNo_int, out bno) == true && Convert.ToInt32(rcptdt.CashDetail_BillNo_int) != 0)
                            {
                                if (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) < 0)
                                    NAME.InnerText = "DT - " + (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) * (-1));
                                else
                                    NAME.InnerText = "DT - " + rcptdt.CashDetail_BillNo_int;

                                BILLALLOCATIONS.AppendChild(NAME);
                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.CashDetail_BillNo_int != "" && rcptdt.CashDetail_BillNo_int != "0")
                            {
                                NAME.InnerText = rcptdt.CashDetail_BillNo_int;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.CashDetail_NoteNo_var != "")
                            {
                                NAME.InnerText = rcptdt.CashDetail_NoteNo_var;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                            TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                            BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                            XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                            AMT.InnerText = (rcptdt.CashDetail_Amount_money * -1).ToString();
                            BILLALLOCATIONS.AppendChild(AMT);

                            XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);

                            XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
                            BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                        }

                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ESCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                        if (rcptd.Cash_PaymentType_bit == false)
                        {
                            if (cnStr.ToLower().Contains("mumbai") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else if (cnStr.ToLower().Contains("nashik") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else
                                LEDGERNAMEe.InnerText = "Petty Cash - Sinhgad Road"; //nashik, mumbai - Cash
                        }
                        else
                        {
                            LEDGERNAMEe.InnerText = ddlBankname.SelectedItem.Text;
                        }
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        XmlElement ISCAPVATTAXALTEREDEr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTEREDEr.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDEr);

                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        if (rcptd.Cash_TDS_num > 0)
                        {
                            AMOUNTav.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                        }
                        else
                        {
                            AMOUNTav.InnerText = "-" + rcptd.Cash_Amount_num;
                        }
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                        XmlElement AMOUNTVAT = xmldoc.CreateElement("VATEXPAMOUNT");
                        if (rcptd.Cash_TDS_num > 0)
                        {
                            AMOUNTVAT.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                        }
                        else
                        {
                            AMOUNTVAT.InnerText = "-" + rcptd.Cash_Amount_num;
                        }
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTVAT);

                        XmlElement SERVICETAXDETAILSS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILSS);

                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        //
                        if (rcptd.Cash_PaymentType_bit == true)
                        {
                            XmlElement TEMPXmlElemt = xmldoc.CreateElement("DATE");
                            date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTDATE");
                            date = Convert.ToDateTime(rcptd.Cash_ChqDate_date);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            //TEMPXmlElemt = xmldoc.CreateElement("BANKERSDATE");
                            //date = Convert.ToDateTime(rcptd.Cash_ChqDate_date);
                            //TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("NAME");
                            //TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
                            //TEMPXmlElemt.InnerText = rcptd.Cash_BankName_var;
                            if (DateTime.Now.Month <= 3)
                                NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                            else
                                NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                            NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
                            TEMPXmlElemt.InnerText = NarrationStr;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("TRANSACTIONTYPE");
                            TEMPXmlElemt.InnerText = "Cheque/DD";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKNAME");
                            TEMPXmlElemt.InnerText = rcptd.Cash_BankName_var;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKBRANCHNAME");
                            TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("PAYMENTFAVOURING");
                            TEMPXmlElemt.InnerText = lblClientName.Text;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKID");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTNUMBER");
                            TEMPXmlElemt.InnerText = Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("UNIQUEREFERENCENUMBER");
                            TEMPXmlElemt.InnerText = "";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("STATUS");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("PAYMENTMODE");
                            TEMPXmlElemt.InnerText = "Transacted";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            //TEMPXmlElemt = xmldoc.CreateElement("SECONDARYSTATUS");
                            //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKPARTYNAME");
                            TEMPXmlElemt.InnerText = lblClientName.Text.Replace("&", "AND"); ;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISCONNECTEDPAYMENT");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISSPLIT");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISCONTRACTUSED");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISACCEPTEDWITHWARNING");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISTRANSFORCED");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


                            TEMPXmlElemt = xmldoc.CreateElement("CHEQUEPRINTED");
                            TEMPXmlElemt.InnerText = "1";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("AMOUNT");
                            if (rcptd.Cash_TDS_num > 0)
                            {
                                TEMPXmlElemt.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                            }
                            else
                            {
                                TEMPXmlElemt.InnerText = "-" + rcptd.Cash_Amount_num;
                            }
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("CONTRACTDETAILS.LIST");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKSTATUSINFO.LIST");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


                        }
                        //
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);

                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INPUTCRALLOCSS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                        XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement EXCISEDUTYHEADDETAILSS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                        XmlElement RATEDETAILSS = xmldoc.CreateElement("RATEDETAILS.LIST");
                        XmlElement SUMMARYALLOCSS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                        XmlElement STPYMTDETAILSS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                        XmlElement EXCISEPAYMENTALLOCATIONSS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                        XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                        XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
                        XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSS);
                        ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
                        ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(RATEDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSS);
                        ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSS);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                        //tds
                        XmlElement ALLLEDGERENTRIESMt = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSzt = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSzt.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSst = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSst.InnerText = "-1";
                        OLDAUDITENTRYIDSzt.AppendChild(OLDAUDITENTRYIDSst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRYIDSzt);

                        XmlElement LEDGERNAMEet = xmldoc.CreateElement("LEDGERNAME");
                        DateTime tempDate = Convert.ToDateTime(rcptd.Cash_Date_date);
                        if (tempDate.Month > 3)
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year) + "-" + (tempDate.Year + 1);
                        else
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year - 1) + "-" + (tempDate.Year);

                        ALLLEDGERENTRIESMt.AppendChild(LEDGERNAMEet);
                        XmlElement GSTCLASSst = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESMt.AppendChild(GSTCLASSst);

                        //XmlElement ISDEEMEDPOSITIVEat = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        //ISDEEMEDPOSITIVEat.InnerText = "Yes";
                        //ALLLEDGERENTRIESMt.AppendChild(ISDEEMEDPOSITIVEat);

                        XmlElement LEDGERFROMITEMat = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMat.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(LEDGERFROMITEMat);

                        XmlElement REMOVEZEROENTRIESqt = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESqt.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(REMOVEZEROENTRIESqt);

                        XmlElement ISPARTYLEDGERwt = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERwt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISPARTYLEDGERwt);

                        XmlElement ISLASTDEEMEDPOSITIVErt = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVErt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISLASTDEEMEDPOSITIVErt);

                        XmlElement ISCAPVATTAXALTEREd = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTEREd.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(ISCAPVATTAXALTEREd);

                        XmlElement AMOUNTavt = xmldoc.CreateElement("AMOUNT");
                        AMOUNTavt.InnerText = "-" + rcptd.Cash_TDS_num;
                        ALLLEDGERENTRIESMt.AppendChild(AMOUNTavt);

                        XmlElement VATEXPAYMENT = xmldoc.CreateElement("VATEXPAYMENT");
                        VATEXPAYMENT.InnerText = "-" + rcptd.Cash_TDS_num;
                        ALLLEDGERENTRIESMt.AppendChild(VATEXPAYMENT);

                        XmlElement SERVICETAXDETAILs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(SERVICETAXDETAILs);

                        XmlElement BANKALLOCATIONSst = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BANKALLOCATIONSst);

                        XmlElement BILLALLOCATIONSst = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONst = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESst = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSst = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESswt = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INPUTCRALLOCS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                        XmlElement DUTYHEADDETAILs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement EXCISEDUTYHEADDETAILS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                        XmlElement RATEDETAILS = xmldoc.CreateElement("RATEDETAILS.LIST");
                        XmlElement SUMMARYALLOCS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                        XmlElement STPYMTDETAILS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                        XmlElement EXCISEPAYMENTALLOCATIONS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                        XmlElement TAXBILLALLOCATIONSst = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSst = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSst = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSst = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSst = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        XmlElement REFVOUCHERDETAILS = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                        XmlElement INVOICEWISEDETAILS = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                        XmlElement VATITCDETAILS = xmldoc.CreateElement("VATITCDETAILS.LIST");
                        XmlElement ADVANCETAXDETAILS = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(INTERESTCOLLECTIONst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRIESst);
                        ALLLEDGERENTRIESMt.AppendChild(ACCOUNTAUDITENTRIESSst);
                        ALLLEDGERENTRIESMt.AppendChild(AUDITENTRIESswt);
                        ALLLEDGERENTRIESMt.AppendChild(INPUTCRALLOCS);
                        ALLLEDGERENTRIESMt.AppendChild(DUTYHEADDETAILs);
                        ALLLEDGERENTRIESMt.AppendChild(EXCISEDUTYHEADDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(RATEDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(SUMMARYALLOCS);
                        ALLLEDGERENTRIESMt.AppendChild(STPYMTDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(EXCISEPAYMENTALLOCATIONS);
                        ALLLEDGERENTRIESMt.AppendChild(TAXBILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TAXOBJECTALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TDSEXPENSEALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(VATSTATUTORYDETAILSSst);
                        ALLLEDGERENTRIESMt.AppendChild(COSTTRACKALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(REFVOUCHERDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(INVOICEWISEDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(VATITCDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(ADVANCETAXDETAILS);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMt);
                        //

                        XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
                        VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
                        XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
                        XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
                        VOUCHER.AppendChild(TEMPGSTRATEDETAILS);
                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.Cash_Update_TallyStatus(Convert.ToInt32(lblBillNo.Text), true);
                    }

                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "RECEIPT_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "RECEIPT_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "RECEIPT_Metro";
            else
                fileName = "RECEIPT_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }

        private void ReceiptTallyTransferOld23052018()
        {            
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");            
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region receipt
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {                
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {                    
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");
                                      
                    var cashrcpt = dc.Cash_View_Receipt(Convert.ToInt32(lblBillNo.Text),null,null);
                    foreach (var rcptd in cashrcpt)
                    {
                        XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

                        XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "RECEIPT");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        string NarrationStr = "";
                        if (rcptd.Cash_PaymentType_bit == false)
                        {
                            NarrationStr = NarrationStr + "AS PER CASH BOOK ";
                        }
                        else
                        {
                            NarrationStr = "Ch. No. " + Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
                            NarrationStr += " DT " + Convert.ToDateTime(rcptd.Cash_ChqDate_date).ToString("dd/MM/yyyy");
                            NarrationStr += " " + rcptd.Cash_BankName_var;
                            NarrationStr += " " + rcptd.Cash_Branch_var;
                        }
                        NarrationStr += rcptd.Cash_Note_var;
                        if (rcptd.Cash_CollectionDetail != null && rcptd.Cash_CollectionDetail != "")
                        {
                            string[] CollectionDetail = new string[1];
                            CollectionDetail = Convert.ToString(rcptd.Cash_CollectionDetail).Split('|');
                            NarrationStr += " " + Convert.ToString(CollectionDetail[0]);
                            NarrationStr += " - " + Convert.ToString(CollectionDetail[1]);
                        }
                        NARRATION.InnerText = NarrationStr;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (cnStr.ToLower().Contains("nashik") == true)
                            VOUCHERTYPENAME.InnerText = "Receipt A";
                        else
                            VOUCHERTYPENAME.InnerText = "Receipt"; // nashik- Receipt A
                        
                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        if (DateTime.Now.Month <= 3)
                            NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                        else
                            NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                        //NarrationStr += "/" + lblBillNo.Text.Replace("-","");
                        NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
                        VOUCHERNUMBER.InnerText = NarrationStr;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);

                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);
                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "Yes";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                        VCHISFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(VCHISFROMSYNC);

                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);

                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        //
                        int bno = 0;
                        var rcpt = dc.Cash_View(Convert.ToInt32(lblBillNo.Text),0,true);
                        foreach (var rcptdt in rcpt)
                        {

                            XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement NAME = xmldoc.CreateElement("NAME");
                            XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                            if (rcptdt.CashDetail_Settlement_var == "On A/c")
                            {
                                BILLTYPE.InnerText = "On Account";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            //else if (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) != 0)
                            else if (int.TryParse(rcptdt.CashDetail_BillNo_int, out bno) == true && Convert.ToInt32(rcptdt.CashDetail_BillNo_int) != 0)
                            {
                                if (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) < 0)
                                    NAME.InnerText = "DT - " + (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) * (-1));
                                else
                                    NAME.InnerText = "DT - " + rcptdt.CashDetail_BillNo_int;
                                
                                BILLALLOCATIONS.AppendChild(NAME);
                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.CashDetail_BillNo_int != "" && rcptdt.CashDetail_BillNo_int != "0")
                            {
                                NAME.InnerText = rcptdt.CashDetail_BillNo_int;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.CashDetail_NoteNo_var != "")
                            {
                                NAME.InnerText = rcptdt.CashDetail_NoteNo_var;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                            TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                            BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                            XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                            AMT.InnerText = (rcptdt.CashDetail_Amount_money * -1).ToString();
                            BILLALLOCATIONS.AppendChild(AMT);

                            XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                        }

                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                        if (rcptd.Cash_PaymentType_bit == false)
                        {
                            if (cnStr.ToLower().Contains("mumbai") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else if (cnStr.ToLower().Contains("nashik") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else
                                LEDGERNAMEe.InnerText = "Petty Cash - Sinhgad Road"; //nashik, mumbai - Cash
                        }
                        else
                        {
                            LEDGERNAMEe.InnerText = ddlBankname.SelectedItem.Text;
                        }
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);
                        
                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        if (rcptd.Cash_TDS_num > 0)
                        {
                            AMOUNTav.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                        }
                        else
                        {
                            AMOUNTav.InnerText = "-" + rcptd.Cash_Amount_num;
                        }
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        //
                        if (rcptd.Cash_PaymentType_bit == true)
                        {
                            XmlElement TEMPXmlElemt = xmldoc.CreateElement("DATE");
                            date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTDATE");
                            date = Convert.ToDateTime(rcptd.Cash_ChqDate_date);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKBRANCHNAME");
                            TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);
                            
                            TEMPXmlElemt = xmldoc.CreateElement("TRANSACTIONTYPE");
                            TEMPXmlElemt.InnerText = "Cheque/DD";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);
                            
                            TEMPXmlElemt = xmldoc.CreateElement("BANKNAME");
                            TEMPXmlElemt.InnerText = rcptd.Cash_BankName_var;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("PAYMENTFAVOURING");
                            TEMPXmlElemt.InnerText = lblClientName.Text;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTNUMBER");
                            TEMPXmlElemt.InnerText = Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("UNIQUEREFERENCENUMBER");
                            TEMPXmlElemt.InnerText = "";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("STATUS");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("CHEQUEPRINTED");
                            TEMPXmlElemt.InnerText = "1";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("AMOUNT");
                            if (rcptd.Cash_TDS_num > 0)
                            {
                                TEMPXmlElemt.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                            }
                            else
                            {
                                TEMPXmlElemt.InnerText = "-" + rcptd.Cash_Amount_num;
                            }                            
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);
                        }
                        //
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);

                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                        //tds
                        XmlElement ALLLEDGERENTRIESMt = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSzt = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSzt.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSst = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSst.InnerText = "-1";
                        OLDAUDITENTRYIDSzt.AppendChild(OLDAUDITENTRYIDSst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRYIDSzt);

                        XmlElement LEDGERNAMEet = xmldoc.CreateElement("LEDGERNAME");
                        DateTime tempDate = Convert.ToDateTime(rcptd.Cash_Date_date);
                        if (tempDate.Month > 3)
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year) + "-" + (tempDate.Year + 1);
                        else
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year - 1) + "-" + (tempDate.Year);
                                                
                        ALLLEDGERENTRIESMt.AppendChild(LEDGERNAMEet);
                        XmlElement GSTCLASSst = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESMt.AppendChild(GSTCLASSst);

                        XmlElement ISDEEMEDPOSITIVEat = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEat.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISDEEMEDPOSITIVEat);

                        XmlElement LEDGERFROMITEMat = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMat.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(LEDGERFROMITEMat);

                        XmlElement REMOVEZEROENTRIESqt = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESqt.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(REMOVEZEROENTRIESqt);

                        XmlElement ISPARTYLEDGERwt = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERwt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISPARTYLEDGERwt);

                        XmlElement ISLASTDEEMEDPOSITIVErt = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVErt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISLASTDEEMEDPOSITIVErt);

                        XmlElement AMOUNTavt = xmldoc.CreateElement("AMOUNT");                        
                        AMOUNTavt.InnerText = "-" + rcptd.Cash_TDS_num;                        
                        ALLLEDGERENTRIESMt.AppendChild(AMOUNTavt);

                        XmlElement BANKALLOCATIONSst = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BANKALLOCATIONSst);

                        XmlElement BILLALLOCATIONSst = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONst = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESst = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSst = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESswt = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement TAXBILLALLOCATIONSst = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSst = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSst = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSst = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSst = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(INTERESTCOLLECTIONst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRIESst);
                        ALLLEDGERENTRIESMt.AppendChild(ACCOUNTAUDITENTRIESSst);
                        ALLLEDGERENTRIESMt.AppendChild(AUDITENTRIESswt);
                        ALLLEDGERENTRIESMt.AppendChild(TAXBILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TAXOBJECTALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TDSEXPENSEALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(VATSTATUTORYDETAILSSst);
                        ALLLEDGERENTRIESMt.AppendChild(COSTTRACKALLOCATIONSst);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMt);
                        //
                        
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.Cash_Update_TallyStatus(Convert.ToInt32(lblBillNo.Text), true); 
                    }

                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "RECEIPT_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "RECEIPT_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "RECEIPT_Metro";
            else
                fileName = "RECEIPT_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }

        private void DebitNoteTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");            
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region debit note
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");
                    int j = 0;
                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    var jounal = dc.JournalDetail_View_tr(lblBillNo.Text);
                    foreach (var jrnl in jounal)
                    {
                        if (j == 0)
                        {                            
                            TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");                            
                            VOUCHER.SetAttribute("REMOTEID", "");
                            VOUCHER.SetAttribute("VCHKEY", "");
                            VOUCHER.SetAttribute("VCHTYPE", "Journal 1");
                            VOUCHER.SetAttribute("ACTION", "Create");
                            VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                            XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                            OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                            XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                            OLDAUDITENTRYIDSf.InnerText = "-1";
                            OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                            VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                            XmlElement DATE = xmldoc.CreateElement("DATE");
                            DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                            DATE.InnerText = date.ToString("yyyyMMdd");
                            VOUCHER.AppendChild(DATE);

                            XmlElement GUID = xmldoc.CreateElement("GUID");
                            GUID.InnerText = "";
                            VOUCHER.AppendChild(GUID);

                            XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                            NARRATION.InnerText = jrnl.Journal_Note_var;
                            VOUCHER.AppendChild(NARRATION);

                            XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                            if (cnStr.ToLower().Contains("nashik") == true)
                                VOUCHERTYPENAME.InnerText = "Journal A";
                            else
                                VOUCHERTYPENAME.InnerText = "Journal 1"; // nashik- Journal A
                            VOUCHER.AppendChild(VOUCHERTYPENAME);

                            XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                            if (lblBillNo.Text.Contains("-") == true)
                                VOUCHERNUMBER.InnerText = lblBillNo.Text.Replace("-", "");
                            else
                                VOUCHERNUMBER.InnerText = lblBillNo.Text;
                            
                            VOUCHER.AppendChild(VOUCHERNUMBER);

                            XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                            PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                            VOUCHER.AppendChild(PARTYLEDGERNAME);
                            XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                            VOUCHER.AppendChild(CSTFORMISSUETYPE);
                            XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                            VOUCHER.AppendChild(CSTFORMRECVTYPE);
                            XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                            FBTPAYMENTTYPE.InnerText = "Default";
                            VOUCHER.AppendChild(FBTPAYMENTTYPE);

                            XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                            PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                            VOUCHER.AppendChild(PERSISTEDVIEW);
                            XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                            VOUCHER.AppendChild(VCHGSTCLASS);

                            XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                            ENTEREDBY.InnerText = "DESPL";
                            VOUCHER.AppendChild(ENTEREDBY);

                            XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                            DIFFACTUALQTY.InnerText = "No";
                            VOUCHER.AppendChild(DIFFACTUALQTY);

                            XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                            AUDITED.InnerText = "No";
                            VOUCHER.AppendChild(AUDITED);

                            XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                            FORJOBCOSTING.InnerText = "No";
                            VOUCHER.AppendChild(FORJOBCOSTING);

                            XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                            ISOPTIONAL.InnerText = "No";
                            VOUCHER.AppendChild(ISOPTIONAL);

                            XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                            DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                            EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                            VOUCHER.AppendChild(EFFECTIVEDATE);

                            XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                            ISFORJOBWORKIN.InnerText = "No";
                            VOUCHER.AppendChild(ISFORJOBWORKIN);

                            XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                            ALLOWCONSUMPTION.InnerText = "No";
                            VOUCHER.AppendChild(ALLOWCONSUMPTION);

                            XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                            USEFORINTEREST.InnerText = "No";
                            VOUCHER.AppendChild(USEFORINTEREST);

                            XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                            USEFORGAINLOSS.InnerText = "No";
                            VOUCHER.AppendChild(USEFORGAINLOSS);

                            XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                            USEFORGODOWNTRANSFER.InnerText = "No";
                            VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                            XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                            USEFORCOMPOUND.InnerText = "No";
                            VOUCHER.AppendChild(USEFORCOMPOUND);
                            XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                            ALTERID.InnerText = "";
                            VOUCHER.AppendChild(ALTERID);

                            XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                            EXCISEOPENING.InnerText = "No";
                            VOUCHER.AppendChild(EXCISEOPENING);

                            XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                            USEFORFINALPRODUCTION.InnerText = "No";
                            VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                            XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                            ISCANCELLED.InnerText = "No";
                            VOUCHER.AppendChild(ISCANCELLED);

                            XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                            HASCASHFLOW.InnerText = "Yes";
                            VOUCHER.AppendChild(HASCASHFLOW);

                            XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                            ISPOSTDATED.InnerText = "No";
                            VOUCHER.AppendChild(ISPOSTDATED);

                            XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                            USETRACKINGNUMBER.InnerText = "No";
                            VOUCHER.AppendChild(USETRACKINGNUMBER);

                            XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                            ISINVOICE.InnerText = "No";
                            VOUCHER.AppendChild(ISINVOICE);

                            XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                            MFGJOURNAL.InnerText = "No";
                            VOUCHER.AppendChild(MFGJOURNAL);

                            XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                            HASDISCOUNTS.InnerText = "No";
                            VOUCHER.AppendChild(HASDISCOUNTS);

                            XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                            ASPAYSLIP.InnerText = "No";
                            VOUCHER.AppendChild(ASPAYSLIP);

                            XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                            ISCOSTCENTRE.InnerText = "No";
                            VOUCHER.AppendChild(ISCOSTCENTRE);

                            XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                            ISSTXNONREALIZEDVCH.InnerText = "No";
                            VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                            XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                            ISEXCISEMANUFACTURERON.InnerText = "No";
                            VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                            XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                            ISBLANKCHEQUE.InnerText = "No";
                            VOUCHER.AppendChild(ISBLANKCHEQUE);

                            XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                            ISDELETED.InnerText = "No";
                            VOUCHER.AppendChild(ISDELETED);

                            XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                            ASORIGINAL.InnerText = "No";
                            VOUCHER.AppendChild(ASORIGINAL);

                            XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                            VCHISFROMSYNC.InnerText = "No";
                            VOUCHER.AppendChild(VCHISFROMSYNC);

                            XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                            XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                            XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                            XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                            XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                            XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                            XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                            XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                            XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");

                            MASTERID.InnerText = "";
                            VOUCHER.AppendChild(MASTERID);

                            VOUCHERKEY.InnerText = "";
                            VOUCHER.AppendChild(VOUCHERKEY);

                            VOUCHER.AppendChild(OLDAUDITENTRIES);
                            VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                            VOUCHER.AppendChild(AUDITENTRIES);
                            VOUCHER.AppendChild(INVOICEDELNOTES);
                            VOUCHER.AppendChild(INVOICEORDERLIST);
                            VOUCHER.AppendChild(INVOICEINDENTLIST);
                            VOUCHER.AppendChild(ATTENDANCEENTRIES);

                            XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                            XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                            OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                            XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                            OLDAUDITENTRYIDSm.InnerText = "-1";
                            OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                            ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                            XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                            XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                            LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                            ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                            ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                            XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                            ISDEEMEDPOSITIVE.InnerText = "No";
                            ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                            XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                            LEDGERFROMITEM.InnerText = "No";
                            ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                            XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                            REMOVEZEROENTRIES.InnerText = "No";
                            ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                            XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                            ISPARTYLEDGER.InnerText = "Yes";
                            ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                            XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                            ISLASTDEEMEDPOSITIVE.InnerText = "No";
                            ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                            XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                            XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                            AMOUNT.InnerText = "-" + lblBillAmount.Text;
                            ALLLEDGERENTRIESmain.AppendChild(AMOUNT);
                            ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                            //
                            XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement NAME = xmldoc.CreateElement("NAME");
                            XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");

                            NAME.InnerText = jrnl.Journal_NoteNo_var;
                            BILLALLOCATIONS.AppendChild(NAME);

                            BILLTYPE.InnerText = "Agst Ref";
                            BILLALLOCATIONS.AppendChild(BILLTYPE);

                            XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                            TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                            BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                            XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                            AMT.InnerText = "-" + lblBillAmount.Text;
                            BILLALLOCATIONS.AppendChild(AMT);

                            XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);

                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                            ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                            VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                        }
                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");                        
                        LEDGERNAMEe.InnerText = jrnl.ledgerName;                        
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = jrnl.JournalDetail_Amount_dec.ToString();
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                        if (jrnl.costCenterName != "NA")
                        {
                            XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                            XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                            CATEGORY.InnerText = jrnl.categoryName;
                            CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                            XmlElement ISDEEMEDPOSITIVEc = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                            ISDEEMEDPOSITIVEc.InnerText = "No";
                            CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEc);

                            XmlElement COSTCENTREALLOCATIONS = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                            XmlElement NAMEc = xmldoc.CreateElement("NAME");
                            NAMEc.InnerText = jrnl.costCenterName;
                            COSTCENTREALLOCATIONS.AppendChild(NAMEc);

                            XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                            AMOUNTc.InnerText = jrnl.JournalDetail_Amount_dec.ToString();
                            COSTCENTREALLOCATIONS.AppendChild(AMOUNTc);

                            CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONS);
                            ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);
                        }
                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        j++;
                        dc.Journal_Update_TallyStatus(lblBillNo.Text, true); 
                    }
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "DEBITNOTE_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "DEBITNOTE_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "DEBITNOTE_Metro";
            else
                fileName = "DEBITNOTE_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }
        bool IsDigitsOnly(string str)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (!r.IsMatch(str))
            {
                return false;
            }

            return true;
        }
        private void CreditNoteTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region credit note
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");
                    //int j = 0;
                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    double CreditAmt = 0;
                    var jounal = dc.JournalDetail_View_tr(lblBillNo.Text).ToList();
                    foreach (var jrnl in jounal)
                    {

                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "Journal");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        NARRATION.InnerText = jrnl.Journal_Note_var;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (cnStr.ToLower().Contains("nashik") == true)
                            VOUCHERTYPENAME.InnerText = "Journal A";
                        else
                            VOUCHERTYPENAME.InnerText = "Journal"; // nashik- Journal A
                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        if (lblBillNo.Text.Contains("-") == true)
                            VOUCHERNUMBER.InnerText = lblBillNo.Text.Replace("-", "");
                        else
                            VOUCHERNUMBER.InnerText = lblBillNo.Text;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);
                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
                        VOUCHERTYPEORIGNAME.InnerText = "Journal";
                        VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
                        ISMSTFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(ISMSTFROMSYNC);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
                        USEFOREXCISE.InnerText = "No";
                        VOUCHER.AppendChild(USEFOREXCISE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);

                        XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
                        USEFORSERVICETAX.InnerText = "No";
                        VOUCHER.AppendChild(USEFORSERVICETAX);


                        XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
                        ISEXCISEVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEVOUCHER);


                        XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
                        EXCISETAXOVERRIDE.InnerText = "No";
                        VOUCHER.AppendChild(EXCISETAXOVERRIDE);

                        XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
                        USEFORTAXUNITTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);


                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
                        ISTDSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSOVERRIDDEN);

                        XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
                        ISTCSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTCSOVERRIDDEN);

                        XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
                        ISTDSTCSCASHVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSTCSCASHVCH);

                        XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
                        INCLUDEADVPYMTVCH.InnerText = "No";
                        VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

                        XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
                        ISSUBWORKSCONTRACT.InnerText = "No";
                        VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

                        XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
                        ISVATOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISVATOVERRIDDEN);

                        XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
                        IGNOREORIGVCHDATE.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREORIGVCHDATE);

                        XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
                        ISVATPAIDATCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

                        XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
                        ISDECLAREDTOCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

                        XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
                        ISSERVICETAXOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

                        XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
                        ISISDVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISISDVOUCHER);

                        XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
                        ISEXCISEOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

                        XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
                        ISEXCISESUPPLYVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

                        XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
                        ISGSTOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISGSTOVERRIDDEN);


                        XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
                        GSTNOTEEXPORTED.InnerText = "No";
                        VOUCHER.AppendChild(GSTNOTEEXPORTED);

                        XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
                        ISVATPRINCIPALACCOUNT.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

                        XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
                        ISBOENOTAPPLICABLE.InnerText = "No";
                        VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

                        XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
                        ISSHIPPINGWITHINSTATE.InnerText = "No";
                        VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

                        XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
                        ISOVERSEASTOURISTTRANS.InnerText = "No";
                        VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "No";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
                        ISVOID.InnerText = "No";
                        VOUCHER.AppendChild(ISVOID);

                        XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
                        ISONHOLD.InnerText = "No";
                        VOUCHER.AppendChild(ISONHOLD);

                        XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
                        ORDERLINESTATUS.InnerText = "No";
                        VOUCHER.AppendChild(ORDERLINESTATUS);

                        XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
                        VATISAGNSTCANCSALES.InnerText = "No";
                        VOUCHER.AppendChild(VATISAGNSTCANCSALES);

                        XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
                        VATISPURCEXEMPTED.InnerText = "No";
                        VOUCHER.AppendChild(VATISPURCEXEMPTED);

                        XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
                        ISVATRESTAXINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISVATRESTAXINVOICE);

                        XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
                        VATISASSESABLECALCVCH.InnerText = "No";
                        VOUCHER.AppendChild(VATISASSESABLECALCVCH);

                        XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
                        ISVATDUTYPAID.InnerText = "Yes";
                        VOUCHER.AppendChild(ISVATDUTYPAID);

                        XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
                        ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
                        VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

                        XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
                        ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
                        VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        //XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        //ASORIGINAL.InnerText = "No";
                        //VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
                        CHANGEVCHMODE.InnerText = "No";
                        VOUCHER.AppendChild(CHANGEVCHMODE);



                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement EXCLUDEDTAXATION = xmldoc.CreateElement("EXCLUDEDTAXATION.LIST");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
                        XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                        XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
                        XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(EXCLUDEDTAXATION);
                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(DUTYHEADDETAILS);
                        VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
                        VOUCHER.AppendChild(EWAYBILLDETAILS);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);
                        VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                        VOUCHER.AppendChild(INVOICEEXPORTLIST);

                        foreach (var jrnl1 in jounal)
                        {
                            if (jrnl1.JournalDetail_Amount_dec > 0)
                            {
                                XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                                XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                OLDAUDITENTRYIDSs.InnerText = "-1";
                                OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                                XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                                LEDGERNAMEe.InnerText = jrnl1.ledgerName;
                                ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                                XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                                ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                                XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                ISDEEMEDPOSITIVEa.InnerText = "Yes";
                                ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                                XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                                LEDGERFROMITEMa.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                                XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                REMOVEZEROENTRIESq.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                                XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                                ISPARTYLEDGERw.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                                XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                                XmlElement ISCAPVATTAXALTEREDr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                ISCAPVATTAXALTEREDr.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDr);

                                XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                                AMOUNTav.InnerText = "-" + jrnl1.JournalDetail_Amount_dec.ToString();
                                ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                                XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
                                VATEXPAMOUNT.InnerText = lblBillAmount.Text;
                                ALLLEDGERENTRIESM.AppendChild(VATEXPAMOUNT);

                                XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILS);

                                if (jrnl1.categoryName != "NA" && jrnl1.categoryName != null)
                                {
                                    XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                                    XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                                    CATEGORY.InnerText = jrnl1.categoryName;
                                    CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                                    XmlElement ISDEEMEDPOSITIVEc = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEc.InnerText = "Yes";
                                    CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEc);

                                    XmlElement COSTCENTREALLOCATIONS = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                                    XmlElement NAMEc = xmldoc.CreateElement("NAME");
                                    NAMEc.InnerText = jrnl1.costCenterName;
                                    COSTCENTREALLOCATIONS.AppendChild(NAMEc);

                                    XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTc.InnerText = "-" + jrnl1.JournalDetail_Amount_dec.ToString();
                                    COSTCENTREALLOCATIONS.AppendChild(AMOUNTc);

                                    CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONS);
                                    ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);
                                }
                                XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                XmlElement INPUTCRALLOCSs = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                XmlElement EXCISEDUTYHEADDETAILSs = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                XmlElement RATEDETAILSs = xmldoc.CreateElement("RATEDETAILS.LIST");
                                XmlElement SUMMARYALLOCSs = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                XmlElement STPYMTDETAILSs = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                XmlElement EXCISEPAYMENTALLOCATIONSs = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                                ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                                ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                                ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSs);
                                ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(RATEDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSs);
                                ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                                ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
                                VOUCHER.AppendChild(ALLLEDGERENTRIESM);
                            }
                            else
                            {
                                CreditAmt += Convert.ToDouble(jrnl1.JournalDetail_Amount_dec * (-1));
                            }
                        }
                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                        XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTERED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);

                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = CreditAmt.ToString();
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

                        XmlElement VATEXPAMOUNTs = xmldoc.CreateElement("VATEXPAMOUNT");
                        VATEXPAMOUNTs.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNTs);

                        XmlElement SERVICETAXDETAILSs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILSs);




                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        ////
                        foreach (var jrnl2 in jounal)
                        {
                            if (jrnl2.JournalDetail_Amount_dec < 0)
                            {
                                XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement NAME = xmldoc.CreateElement("NAME");
                                XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                                if (jrnl2.JournalDetail_BillNo_var != null && jrnl2.JournalDetail_BillNo_var != "")
                                {
                                    if (!IsDigitsOnly(jrnl2.JournalDetail_BillNo_var.ToString()))
                                        NAME.InnerText = "DT - " + jrnl2.JournalDetail_BillNo_var;
                                    else
                                        NAME.InnerText = jrnl2.JournalDetail_BillNo_var;
                                }
                                else
                                {
                                    NAME.InnerText = jrnl2.ledgerName;
                                }
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);

                                XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                                TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                                BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                                XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                                AMT.InnerText = (jrnl2.JournalDetail_Amount_dec * (-1)).ToString();
                                BILLALLOCATIONS.AppendChild(AMT);

                                XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                                XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
                                BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
                                ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                            }
                        }
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                        XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
                        VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
                        XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
                        XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
                        VOUCHER.AppendChild(TEMPGSTRATEDETAILS);

                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.Journal_Update_TallyStatus(lblBillNo.Text, true);
                        break;
                    }
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "CREDITNOTE_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "CREDITNOTE_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "CREDITNOTE_Metro";
            else
                fileName = "CREDITNOTE_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }
   
        //private void CreditNoteTallyTransfer()
        //{
        //    XmlDocument xmldoc = new XmlDocument();
        //    XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
        //    xmldoc.AppendChild(RootNode);

        //    XmlElement HEADER = xmldoc.CreateElement("HEADER");
        //    XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
        //    TALLYREQUEST.InnerText = "Import Data";
        //    HEADER.AppendChild(TALLYREQUEST);
        //    RootNode.AppendChild(HEADER);

        //    XmlElement BODY = xmldoc.CreateElement("BODY");
        //    XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
        //    XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
        //    XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
        //    REPORTNAME.InnerText = "Vouchers";
        //    REQUESTDESC.AppendChild(REPORTNAME);

        //    XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
        //    XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
        //    //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
        //    //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
        //    //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
        //    if (cnStr.ToLower().Contains("mumbai") == true)
        //        SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
        //    else if (cnStr.ToLower().Contains("nashik") == true)
        //        SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
        //    else
        //        SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
        //    STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
        //    REQUESTDESC.AppendChild(STATICVARIABLES);
        //    XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
        //    #region credit note
        //    for (int i = 0; i < grdBill.Rows.Count; i++)
        //    {
        //        CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
        //        if (chkSelect.Checked == true)
        //        {
        //            Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
        //            Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
        //            Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
        //            Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
        //            Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");
        //            //int j = 0;
        //            XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
        //            XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
        //            double CreditAmt = 0;
        //            var jounal = dc.JournalDetail_View_tr(lblBillNo.Text).ToList();
        //            foreach (var jrnl in jounal)
        //            {
                        
        //                TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");
        //                VOUCHER.SetAttribute("REMOTEID", "");
        //                VOUCHER.SetAttribute("VCHKEY", "");
        //                VOUCHER.SetAttribute("VCHTYPE", "Journal");
        //                VOUCHER.SetAttribute("ACTION", "Create");
        //                VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

        //                XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
        //                XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                OLDAUDITENTRYIDSf.InnerText = "-1";
        //                OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
        //                VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

        //                XmlElement DATE = xmldoc.CreateElement("DATE");
        //                DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
        //                DATE.InnerText = date.ToString("yyyyMMdd");
        //                VOUCHER.AppendChild(DATE);

        //                XmlElement GUID = xmldoc.CreateElement("GUID");
        //                GUID.InnerText = "";
        //                VOUCHER.AppendChild(GUID);

        //                XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
        //                NARRATION.InnerText = jrnl.Journal_Note_var;
        //                VOUCHER.AppendChild(NARRATION);

        //                XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
        //                if (cnStr.ToLower().Contains("nashik") == true)
        //                    VOUCHERTYPENAME.InnerText = "Journal A";
        //                else
        //                    VOUCHERTYPENAME.InnerText = "Journal"; // nashik- Journal A
        //                VOUCHER.AppendChild(VOUCHERTYPENAME);

        //                XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
        //                if (lblBillNo.Text.Contains("-") == true)
        //                    VOUCHERNUMBER.InnerText = lblBillNo.Text.Replace("-", "");
        //                else
        //                    VOUCHERNUMBER.InnerText = lblBillNo.Text;
        //                VOUCHER.AppendChild(VOUCHERNUMBER);

        //                XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
        //                PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
        //                VOUCHER.AppendChild(PARTYLEDGERNAME);
        //                XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
        //                VOUCHER.AppendChild(CSTFORMISSUETYPE);
        //                XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
        //                VOUCHER.AppendChild(CSTFORMRECVTYPE);
        //                XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
        //                FBTPAYMENTTYPE.InnerText = "Default";
        //                VOUCHER.AppendChild(FBTPAYMENTTYPE);

        //                XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
        //                PERSISTEDVIEW.InnerText = "Accounting Voucher View";
        //                VOUCHER.AppendChild(PERSISTEDVIEW);
        //                XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
        //                VOUCHER.AppendChild(VCHGSTCLASS);

        //                XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
        //                ENTEREDBY.InnerText = "DESPL";
        //                VOUCHER.AppendChild(ENTEREDBY);

        //                XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
        //                DIFFACTUALQTY.InnerText = "No";
        //                VOUCHER.AppendChild(DIFFACTUALQTY);

        //                XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
        //                AUDITED.InnerText = "No";
        //                VOUCHER.AppendChild(AUDITED);

        //                XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
        //                FORJOBCOSTING.InnerText = "No";
        //                VOUCHER.AppendChild(FORJOBCOSTING);

        //                XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
        //                ISOPTIONAL.InnerText = "No";
        //                VOUCHER.AppendChild(ISOPTIONAL);

        //                XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
        //                DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
        //                EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
        //                VOUCHER.AppendChild(EFFECTIVEDATE);

        //                XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
        //                ISFORJOBWORKIN.InnerText = "No";
        //                VOUCHER.AppendChild(ISFORJOBWORKIN);

        //                XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
        //                ALLOWCONSUMPTION.InnerText = "No";
        //                VOUCHER.AppendChild(ALLOWCONSUMPTION);

        //                XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
        //                USEFORINTEREST.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORINTEREST);

        //                XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
        //                USEFORGAINLOSS.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORGAINLOSS);

        //                XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
        //                USEFORGODOWNTRANSFER.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

        //                XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
        //                USEFORCOMPOUND.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORCOMPOUND);
        //                XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
        //                ALTERID.InnerText = "";
        //                VOUCHER.AppendChild(ALTERID);

        //                XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
        //                EXCISEOPENING.InnerText = "No";
        //                VOUCHER.AppendChild(EXCISEOPENING);

        //                XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
        //                USEFORFINALPRODUCTION.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORFINALPRODUCTION);

        //                XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
        //                ISCANCELLED.InnerText = "No";
        //                VOUCHER.AppendChild(ISCANCELLED);

        //                XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
        //                HASCASHFLOW.InnerText = "No";
        //                VOUCHER.AppendChild(HASCASHFLOW);

        //                XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
        //                ISPOSTDATED.InnerText = "No";
        //                VOUCHER.AppendChild(ISPOSTDATED);

        //                XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
        //                USETRACKINGNUMBER.InnerText = "No";
        //                VOUCHER.AppendChild(USETRACKINGNUMBER);

        //                XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
        //                ISINVOICE.InnerText = "No";
        //                VOUCHER.AppendChild(ISINVOICE);

        //                XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
        //                MFGJOURNAL.InnerText = "No";
        //                VOUCHER.AppendChild(MFGJOURNAL);

        //                XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
        //                HASDISCOUNTS.InnerText = "No";
        //                VOUCHER.AppendChild(HASDISCOUNTS);

        //                XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
        //                ASPAYSLIP.InnerText = "No";
        //                VOUCHER.AppendChild(ASPAYSLIP);

        //                XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
        //                ISCOSTCENTRE.InnerText = "No";
        //                VOUCHER.AppendChild(ISCOSTCENTRE);

        //                XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
        //                ISSTXNONREALIZEDVCH.InnerText = "No";
        //                VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

        //                XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
        //                ISEXCISEMANUFACTURERON.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

        //                XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
        //                ISBLANKCHEQUE.InnerText = "No";
        //                VOUCHER.AppendChild(ISBLANKCHEQUE);

        //                XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
        //                ISDELETED.InnerText = "No";
        //                VOUCHER.AppendChild(ISDELETED);

        //                XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
        //                ASORIGINAL.InnerText = "No";
        //                VOUCHER.AppendChild(ASORIGINAL);

        //                XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
        //                VCHISFROMSYNC.InnerText = "No";
        //                VOUCHER.AppendChild(VCHISFROMSYNC);

        //                XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
        //                XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
        //                XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
        //                XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
        //                XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
        //                XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
        //                XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
        //                XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
        //                XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");

        //                MASTERID.InnerText = "";
        //                VOUCHER.AppendChild(MASTERID);

        //                VOUCHERKEY.InnerText = "";
        //                VOUCHER.AppendChild(VOUCHERKEY);

        //                VOUCHER.AppendChild(OLDAUDITENTRIES);
        //                VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
        //                VOUCHER.AppendChild(AUDITENTRIES);
        //                VOUCHER.AppendChild(INVOICEDELNOTES);
        //                VOUCHER.AppendChild(INVOICEORDERLIST);
        //                VOUCHER.AppendChild(INVOICEINDENTLIST);
        //                VOUCHER.AppendChild(ATTENDANCEENTRIES);

        //                foreach (var jrnl1 in jounal)
        //                {
        //                    if (jrnl1.JournalDetail_Amount_dec > 0)
        //                    {
        //                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
        //                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
        //                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                        OLDAUDITENTRYIDSs.InnerText = "-1";
        //                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
        //                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

        //                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
        //                        LEDGERNAMEe.InnerText = jrnl1.ledgerName;
        //                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
        //                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
        //                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

        //                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
        //                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

        //                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
        //                        LEDGERFROMITEMa.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

        //                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
        //                        REMOVEZEROENTRIESq.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

        //                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
        //                        ISPARTYLEDGERw.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

        //                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
        //                        ISLASTDEEMEDPOSITIVEr.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

        //                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
        //                        AMOUNTav.InnerText = "-" + jrnl1.JournalDetail_Amount_dec.ToString();
        //                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

        //                        if (jrnl1.categoryName != "NA" && jrnl1.categoryName != null)
        //                        {
        //                            XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
        //                            XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
        //                            CATEGORY.InnerText = jrnl1.categoryName;
        //                            CATEGORYALLOCATIONS.AppendChild(CATEGORY);

        //                            XmlElement ISDEEMEDPOSITIVEc = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                            ISDEEMEDPOSITIVEc.InnerText = "Yes";
        //                            CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEc);

        //                            XmlElement COSTCENTREALLOCATIONS = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
        //                            XmlElement NAMEc = xmldoc.CreateElement("NAME");
        //                            NAMEc.InnerText = jrnl1.costCenterName;
        //                            COSTCENTREALLOCATIONS.AppendChild(NAMEc);

        //                            XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
        //                            AMOUNTc.InnerText = "-" + jrnl1.JournalDetail_Amount_dec.ToString();
        //                            COSTCENTREALLOCATIONS.AppendChild(AMOUNTc);

        //                            CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONS);
        //                            ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);
        //                        }
        //                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
        //                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
        //                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
        //                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
        //                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
        //                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
        //                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
        //                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
        //                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
        //                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
        //                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
        //                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
        //                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
        //                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
        //                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
        //                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
        //                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
        //                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);
        //                    }
        //                    else
        //                    {
        //                        CreditAmt += Convert.ToDouble(jrnl1.JournalDetail_Amount_dec * (-1));
        //                    }
        //                }
        //                XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
        //                XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
        //                XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                OLDAUDITENTRYIDSm.InnerText = "-1";
        //                OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
        //                ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);
                                                
        //                XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
        //                LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
        //                ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
        //                XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
        //                ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

        //                XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                ISDEEMEDPOSITIVE.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

        //                XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
        //                LEDGERFROMITEM.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

        //                XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
        //                REMOVEZEROENTRIES.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

        //                XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
        //                ISPARTYLEDGER.InnerText = "Yes";
        //                ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

        //                XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
        //                ISLASTDEEMEDPOSITIVE.InnerText = "Yes";
        //                ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);
                                                
        //                XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
        //                AMOUNT.InnerText = CreditAmt.ToString();
        //                ALLLEDGERENTRIESmain.AppendChild(AMOUNT);
        //                XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
        //                ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
        //                ////
        //                foreach (var jrnl2 in jounal)
        //                {
        //                    if (jrnl2.JournalDetail_Amount_dec < 0)
        //                    {
        //                        XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
        //                        XmlElement NAME = xmldoc.CreateElement("NAME");
        //                        XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
        //                        if (jrnl2.JournalDetail_BillNo_var != null && jrnl2.JournalDetail_BillNo_var != "")
        //                        {
        //                            NAME.InnerText = jrnl2.JournalDetail_BillNo_var;
        //                        }
        //                        else
        //                        {
        //                            NAME.InnerText = jrnl2.ledgerName;
        //                        }
        //                        BILLALLOCATIONS.AppendChild(NAME);

        //                        BILLTYPE.InnerText = "Agst Ref";
        //                        BILLALLOCATIONS.AppendChild(BILLTYPE);

        //                        XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
        //                        TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
        //                        BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

        //                        XmlElement AMT = xmldoc.CreateElement("AMOUNT");
        //                        AMT.InnerText = (jrnl2.JournalDetail_Amount_dec * (-1)).ToString();
        //                        BILLALLOCATIONS.AppendChild(AMT);

        //                        XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
        //                        BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
        //                        ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
        //                    }
        //                }
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
        //                VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

        //                XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
        //                VOUCHER.AppendChild(ATTDRECORDS);
        //                TALLYMESSAGE.AppendChild(VOUCHER);
        //                REQUESTDATA.AppendChild(TALLYMESSAGE);
        //                dc.Journal_Update_TallyStatus(lblBillNo.Text, true); 
        //                break;
        //            }
        //        }
        //    }
        //    #endregion
        //    RootNode.AppendChild(HEADER);
        //    IMPORTDATA.AppendChild(REQUESTDESC);
        //    IMPORTDATA.AppendChild(REQUESTDATA);
        //    BODY.AppendChild(IMPORTDATA);
        //    RootNode.AppendChild(BODY);

        //    if (!Directory.Exists(@"d:\xml"))
        //        Directory.CreateDirectory(@"d:\xml");
        //    string fileName = "";
        //    if (cnStr.ToLower().Contains("mumbai") == true)
        //        fileName = "CREDITNOTE_Mumbai";
        //    else if (cnStr.ToLower().Contains("nashik") == true)
        //        fileName = "CREDITNOTE_Nashik";
        //    else
        //        fileName = "CREDITNOTE_Pune";
        //    xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            
        //    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        //    response.ClearContent();
        //    response.Clear();
        //    //response.ContentType = "text/plain";
        //    response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
        //    response.TransmitFile("d:\\xml\\" + fileName + ".xml");
        //    response.Flush();
        //    response.End();
        //}
        private void AdvanceReceiptTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region advance receipt
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");

                    var cashrcpt = dc.Advance_View(null, null, Convert.ToInt32(lblBillNo.Text), "", true).ToList();
                    foreach (var rcptd in cashrcpt)
                    {
                        XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

                        XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "RECEIPT");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        string NarrationStr = "";
                        if (rcptd.PaymentMode == false)
                        {
                            NarrationStr = NarrationStr + "AS PER CASH BOOK ";
                        }
                        else
                        {
                            string[] strChq = rcptd.ChqDetail.Split('|');
                            NarrationStr = "Ch. No. " + strChq[0];
                            NarrationStr += " DT " + strChq[2];
                            NarrationStr += " " + strChq[1];
                            NarrationStr += " " + strChq[3];
                        }
                        NarrationStr += rcptd.Note;
                        if (rcptd.CollectionDetail != null && rcptd.CollectionDetail != "")
                        {
                            string[] CollectionDetail = new string[1];
                            CollectionDetail = Convert.ToString(rcptd.CollectionDetail).Split('|');
                            NarrationStr += " " + Convert.ToString(CollectionDetail[0]);
                            NarrationStr += " - " + Convert.ToString(CollectionDetail[1]);
                        }
                        NARRATION.InnerText = NarrationStr;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement TAXUNITNAME = xmldoc.CreateElement("TAXUNITNAME");
                        TAXUNITNAME.InnerText = "Default Tax Unit";
                        VOUCHER.AppendChild(TAXUNITNAME);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (cnStr.ToLower().Contains("nashik") == true)
                            VOUCHERTYPENAME.InnerText = "Receipt A";
                        else
                            VOUCHERTYPENAME.InnerText = "Receipt"; // nashik- Receipt A

                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        if (DateTime.Now.Month <= 3)
                            NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                        else
                            NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                        NarrationStr += "/" + lblBillNo.Text.Replace("-", "");
                        VOUCHERNUMBER.InnerText = NarrationStr;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);

                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        if (rcptd.PaymentMode == false)
                        {
                            XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
                            VOUCHERTYPEORIGNAME.InnerText = "Receipt";
                            VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);
                        }

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
                        ISMSTFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(ISMSTFROMSYNC);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
                        USEFOREXCISE.InnerText = "No";
                        VOUCHER.AppendChild(USEFOREXCISE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);


                        XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
                        USEFORSERVICETAX.InnerText = "No";
                        VOUCHER.AppendChild(USEFORSERVICETAX);

                        XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
                        ISEXCISEVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEVOUCHER);

                        XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
                        EXCISETAXOVERRIDE.InnerText = "No";
                        VOUCHER.AppendChild(EXCISETAXOVERRIDE);

                        XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
                        USEFORTAXUNITTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
                        ISTDSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSOVERRIDDEN);

                        XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
                        ISTCSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTCSOVERRIDDEN);

                        XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
                        ISTDSTCSCASHVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSTCSCASHVCH);

                        XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
                        INCLUDEADVPYMTVCH.InnerText = "No";
                        VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

                        XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
                        ISSUBWORKSCONTRACT.InnerText = "No";
                        VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

                        XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
                        ISVATOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISVATOVERRIDDEN);

                        XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
                        IGNOREORIGVCHDATE.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREORIGVCHDATE);

                        XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
                        ISVATPAIDATCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

                        XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
                        ISDECLAREDTOCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

                        XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
                        ISSERVICETAXOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

                        XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
                        ISISDVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISISDVOUCHER);

                        XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
                        ISEXCISEOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

                        XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
                        ISEXCISESUPPLYVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

                        XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
                        ISGSTOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISGSTOVERRIDDEN);


                        XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
                        GSTNOTEEXPORTED.InnerText = "No";
                        VOUCHER.AppendChild(GSTNOTEEXPORTED);

                        XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
                        ISVATPRINCIPALACCOUNT.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

                        XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
                        ISBOENOTAPPLICABLE.InnerText = "No";
                        VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

                        XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
                        ISSHIPPINGWITHINSTATE.InnerText = "No";
                        VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

                        XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
                        ISOVERSEASTOURISTTRANS.InnerText = "No";
                        VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "Yes";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
                        ISVOID.InnerText = "No";
                        VOUCHER.AppendChild(ISVOID);

                        XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
                        ISONHOLD.InnerText = "No";
                        VOUCHER.AppendChild(ISONHOLD);

                        XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
                        ORDERLINESTATUS.InnerText = "No";
                        VOUCHER.AppendChild(ORDERLINESTATUS);

                        XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
                        VATISAGNSTCANCSALES.InnerText = "No";
                        VOUCHER.AppendChild(VATISAGNSTCANCSALES);

                        XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
                        VATISPURCEXEMPTED.InnerText = "No";
                        VOUCHER.AppendChild(VATISPURCEXEMPTED);

                        XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
                        ISVATRESTAXINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISVATRESTAXINVOICE);

                        XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
                        VATISASSESABLECALCVCH.InnerText = "No";
                        VOUCHER.AppendChild(VATISASSESABLECALCVCH);

                        XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
                        ISVATDUTYPAID.InnerText = "Yes";
                        VOUCHER.AppendChild(ISVATDUTYPAID);

                        XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
                        ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
                        VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

                        XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
                        ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
                        VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
                        CHANGEVCHMODE.InnerText = "No";
                        VOUCHER.AppendChild(CHANGEVCHMODE);

                        //XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                        //VCHISFROMSYNC.InnerText = "No";
                        //VOUCHER.AppendChild(VCHISFROMSYNC);

                        //XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        //USEFORCOMPOUND.InnerText = "No";
                        //VOUCHER.AppendChild(USEFORCOMPOUND);


                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement EXCLUDEDTAXATIONS = xmldoc.CreateElement("EXCLUDEDTAXATIONS.LIST");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
                        XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                        XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
                        XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(EXCLUDEDTAXATIONS);
                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(DUTYHEADDETAILS);
                        VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
                        VOUCHER.AppendChild(EWAYBILLDETAILS);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);
                        VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                        VOUCHER.AppendChild(INVOICEEXPORTLIST);

                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);


                        XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTERED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);


                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

                        XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
                        VATEXPAMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNT);

                        XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILS);
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        //
                        //var rcpt = dc.Cash_View(Convert.ToInt32(lblBillNo.Text), 0);
                        foreach (var rcptdt in cashrcpt)
                        {

                            XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement NAME = xmldoc.CreateElement("NAME");
                            XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                            int bl = 0;
                            if (rcptdt.Settlement == "On A/c")
                            {
                                BILLTYPE.InnerText = "On Account";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            //else if (Convert.ToInt32(rcptdt.BillNo) > 0)
                            else if (Int32.TryParse(rcptdt.BillNo.ToString(), out bl) == true && Convert.ToInt32(rcptdt.BillNo) > 0)
                            {
                                NAME.InnerText = "DT - " + rcptdt.BillNo;

                                BILLALLOCATIONS.AppendChild(NAME);
                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.BillNo != "0" && rcptdt.BillNo != "")
                            {
                                NAME.InnerText = rcptdt.BillNo;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.NoteNo != "")
                            {
                                NAME.InnerText = rcptdt.NoteNo;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                            TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                            BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                            XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                            AMT.InnerText = (rcptdt.Amount * -1).ToString();
                            BILLALLOCATIONS.AppendChild(AMT);

                            XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);

                            XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
                            BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                        }

                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ESCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                        if (rcptd.PaymentMode == false)
                        {
                            if (cnStr.ToLower().Contains("mumbai") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else if (cnStr.ToLower().Contains("nashik") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else
                                LEDGERNAMEe.InnerText = "Petty Cash - Sinhgad Road"; //nashik, mumbai - Cash
                        }
                        else
                        {
                            LEDGERNAMEe.InnerText = ddlBankname.SelectedItem.Text;
                        }
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        XmlElement ISCAPVATTAXALTEREDEr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTEREDEr.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDEr);

                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        if (rcptd.TDSAmount > 0)
                        {
                            AMOUNTav.InnerText = "-" + (rcptd.ReceiptAmount - rcptd.TDSAmount);
                        }
                        else
                        {
                            AMOUNTav.InnerText = "-" + rcptd.ReceiptAmount;
                        }
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                        XmlElement AMOUNTVAT = xmldoc.CreateElement("VATEXPAMOUNT");
                        if (rcptd.TDSAmount > 0)
                        {
                            AMOUNTVAT.InnerText = "-" + (rcptd.ReceiptAmount - rcptd.TDSAmount);
                        }
                        else
                        {
                            AMOUNTVAT.InnerText = "-" + rcptd.ReceiptAmount;
                        }
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTVAT);

                        XmlElement SERVICETAXDETAILSS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILSS);

                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        //
                        if (rcptd.PaymentMode == true)
                        {
                            XmlElement TEMPXmlElemt = xmldoc.CreateElement("DATE");
                            date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            string[] strChq = rcptd.ChqDetail.Split('|');
                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTDATE");
                            date = DateTime.ParseExact(strChq[2].ToString(), "dd/MM/yyyy", null);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            //TEMPXmlElemt = xmldoc.CreateElement("BANKERSDATE");
                            //date = DateTime.ParseExact(strChq[2].ToString(), "dd/MM/yyyy", null);
                            //TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("NAME");
                            //TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
                            //TEMPXmlElemt.InnerText = strChq[1];
                            if (DateTime.Now.Month <= 3)
                                NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                            else
                                NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                            NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
                            TEMPXmlElemt.InnerText = NarrationStr;

                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("TRANSACTIONTYPE");
                            TEMPXmlElemt.InnerText = "Cheque/DD";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKNAME");
                            TEMPXmlElemt.InnerText = strChq[1];
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKBRANCHNAME");
                            TEMPXmlElemt.InnerText = strChq[3];
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("PAYMENTFAVOURING");
                            TEMPXmlElemt.InnerText = lblClientName.Text;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKID");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTNUMBER");
                            TEMPXmlElemt.InnerText = strChq[0].ToString();
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("UNIQUEREFERENCENUMBER");
                            TEMPXmlElemt.InnerText = "";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("STATUS");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("PAYMENTMODE");
                            TEMPXmlElemt.InnerText = "Transacted";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            //TEMPXmlElemt = xmldoc.CreateElement("SECONDARYSTATUS");
                            //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKPARTYNAME");
                            TEMPXmlElemt.InnerText = lblClientName.Text.Replace("&", "AND"); ;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISCONNECTEDPAYMENT");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISSPLIT");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISCONTRACTUSED");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISACCEPTEDWITHWARNING");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("ISTRANSFORCED");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


                            TEMPXmlElemt = xmldoc.CreateElement("CHEQUEPRINTED");
                            TEMPXmlElemt.InnerText = "1";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("AMOUNT");
                            if (rcptd.TDSAmount > 0)
                            {
                                TEMPXmlElemt.InnerText = "-" + (rcptd.ReceiptAmount - rcptd.TDSAmount);
                            }
                            else
                            {
                                TEMPXmlElemt.InnerText = "-" + rcptd.ReceiptAmount;
                            }
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("CONTRACTDETAILS.LIST");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKSTATUSINFO.LIST");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


                        }
                        //
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);

                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INPUTCRALLOCSS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                        XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement EXCISEDUTYHEADDETAILSS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                        XmlElement RATEDETAILSS = xmldoc.CreateElement("RATEDETAILS.LIST");
                        XmlElement SUMMARYALLOCSS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                        XmlElement STPYMTDETAILSS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                        XmlElement EXCISEPAYMENTALLOCATIONSS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                        XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                        XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
                        XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSS);
                        ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
                        ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(RATEDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSS);
                        ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSS);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                        //tds
                        XmlElement ALLLEDGERENTRIESMt = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSzt = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSzt.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSst = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSst.InnerText = "-1";
                        OLDAUDITENTRYIDSzt.AppendChild(OLDAUDITENTRYIDSst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRYIDSzt);

                        XmlElement LEDGERNAMEet = xmldoc.CreateElement("LEDGERNAME");
                        DateTime tempDate = Convert.ToDateTime(rcptd.Date);
                        if (tempDate.Month > 3)
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year) + "-" + (tempDate.Year + 1);
                        else
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year - 1) + "-" + (tempDate.Year);

                        ALLLEDGERENTRIESMt.AppendChild(LEDGERNAMEet);
                        XmlElement GSTCLASSst = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESMt.AppendChild(GSTCLASSst);

                        //XmlElement ISDEEMEDPOSITIVEat = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        //ISDEEMEDPOSITIVEat.InnerText = "Yes";
                        //ALLLEDGERENTRIESMt.AppendChild(ISDEEMEDPOSITIVEat);

                        XmlElement LEDGERFROMITEMat = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMat.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(LEDGERFROMITEMat);

                        XmlElement REMOVEZEROENTRIESqt = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESqt.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(REMOVEZEROENTRIESqt);

                        XmlElement ISPARTYLEDGERwt = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERwt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISPARTYLEDGERwt);

                        XmlElement ISLASTDEEMEDPOSITIVErt = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVErt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISLASTDEEMEDPOSITIVErt);

                        XmlElement ISCAPVATTAXALTEREd = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTEREd.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(ISCAPVATTAXALTEREd);

                        XmlElement AMOUNTavt = xmldoc.CreateElement("AMOUNT");
                        AMOUNTavt.InnerText = "-" + rcptd.TDSAmount;
                        ALLLEDGERENTRIESMt.AppendChild(AMOUNTavt);

                        XmlElement VATEXPAYMENT = xmldoc.CreateElement("VATEXPAYMENT");
                        VATEXPAYMENT.InnerText = "-" + rcptd.TDSAmount;
                        ALLLEDGERENTRIESMt.AppendChild(VATEXPAYMENT);

                        XmlElement SERVICETAXDETAILs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(SERVICETAXDETAILs);

                        XmlElement BANKALLOCATIONSst = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BANKALLOCATIONSst);

                        XmlElement BILLALLOCATIONSst = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONst = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESst = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSst = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESswt = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INPUTCRALLOCS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                        XmlElement DUTYHEADDETAILs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement EXCISEDUTYHEADDETAILS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                        XmlElement RATEDETAILS = xmldoc.CreateElement("RATEDETAILS.LIST");
                        XmlElement SUMMARYALLOCS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                        XmlElement STPYMTDETAILS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                        XmlElement EXCISEPAYMENTALLOCATIONS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                        XmlElement TAXBILLALLOCATIONSst = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSst = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSst = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSst = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSst = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        XmlElement REFVOUCHERDETAILS = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                        XmlElement INVOICEWISEDETAILS = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                        XmlElement VATITCDETAILS = xmldoc.CreateElement("VATITCDETAILS.LIST");
                        XmlElement ADVANCETAXDETAILS = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(INTERESTCOLLECTIONst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRIESst);
                        ALLLEDGERENTRIESMt.AppendChild(ACCOUNTAUDITENTRIESSst);
                        ALLLEDGERENTRIESMt.AppendChild(AUDITENTRIESswt);
                        ALLLEDGERENTRIESMt.AppendChild(INPUTCRALLOCS);
                        ALLLEDGERENTRIESMt.AppendChild(DUTYHEADDETAILs);
                        ALLLEDGERENTRIESMt.AppendChild(EXCISEDUTYHEADDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(RATEDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(SUMMARYALLOCS);
                        ALLLEDGERENTRIESMt.AppendChild(STPYMTDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(EXCISEPAYMENTALLOCATIONS);
                        ALLLEDGERENTRIESMt.AppendChild(TAXBILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TAXOBJECTALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TDSEXPENSEALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(VATSTATUTORYDETAILSSst);
                        ALLLEDGERENTRIESMt.AppendChild(COSTTRACKALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(REFVOUCHERDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(INVOICEWISEDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(VATITCDETAILS);
                        ALLLEDGERENTRIESMt.AppendChild(ADVANCETAXDETAILS);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMt);
                        //

                        XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
                        VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
                        XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
                        XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
                        VOUCHER.AppendChild(TEMPGSTRATEDETAILS);
                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.Advance_Update_TallyStatus(Convert.ToInt32(lblBillNo.Text), "", true);
                    }

                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "ADVANCERECEIPT_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "ADVANCERECEIPT_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "ADVANCERECEIPT_Metro";
            else
                fileName = "ADVANCERECEIPT_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }
        private void AdvanceReceiptTallyTransferOld25052018()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region advance receipt
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");

                    var cashrcpt = dc.Advance_View(null,null,Convert.ToInt32(lblBillNo.Text),"",true).ToList();
                    foreach (var rcptd in cashrcpt)
                    {
                        XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

                        XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "RECEIPT");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        string NarrationStr = "";
                        if (rcptd.PaymentMode == false)
                        {
                            NarrationStr = NarrationStr + "AS PER CASH BOOK ";
                        }
                        else
                        {
                            string[] strChq = rcptd.ChqDetail.Split('|');
                            NarrationStr = "Ch. No. " + strChq[0];
                            NarrationStr += " DT " + strChq[2];
                            NarrationStr += " " + strChq[1];
                            NarrationStr += " " + strChq[3];
                        }
                        NarrationStr += rcptd.Note;
                        if (rcptd.CollectionDetail != null && rcptd.CollectionDetail != "")
                        {
                            string[] CollectionDetail = new string[1];
                            CollectionDetail = Convert.ToString(rcptd.CollectionDetail).Split('|');
                            NarrationStr += " " + Convert.ToString(CollectionDetail[0]);
                            NarrationStr += " - " + Convert.ToString(CollectionDetail[1]);
                        }
                        NARRATION.InnerText = NarrationStr;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (cnStr.ToLower().Contains("nashik") == true)
                            VOUCHERTYPENAME.InnerText = "Receipt A";
                        else
                            VOUCHERTYPENAME.InnerText = "Receipt"; // nashik- Receipt A
                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        if (DateTime.Now.Month <= 3)
                            NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                        else
                            NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                        NarrationStr += "/" + lblBillNo.Text.Replace("-","");
                        VOUCHERNUMBER.InnerText = NarrationStr;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);

                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);
                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "Yes";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                        VCHISFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(VCHISFROMSYNC);

                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);

                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        //
                        //var rcpt = dc.Cash_View(Convert.ToInt32(lblBillNo.Text), 0);
                        foreach (var rcptdt in cashrcpt)
                        {
                            XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement NAME = xmldoc.CreateElement("NAME");
                            XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                            int bl = 0;
                            if (rcptdt.Settlement == "On A/c")
                            {
                                BILLTYPE.InnerText = "On Account";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            //else if (Convert.ToInt32(rcptdt.BillNo) > 0)
                            else if (Int32.TryParse(rcptdt.BillNo.ToString(), out bl) == true && Convert.ToInt32(rcptdt.BillNo) > 0)
                            {
                                NAME.InnerText = "DT - " + rcptdt.BillNo;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.BillNo != "0" && rcptdt.BillNo != "")
                            {
                                NAME.InnerText = rcptdt.BillNo;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            else if (rcptdt.NoteNo != "")
                            {
                                NAME.InnerText = rcptdt.NoteNo;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                            TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                            BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                            XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                            AMT.InnerText = (rcptdt.Amount * -1).ToString();
                            BILLALLOCATIONS.AppendChild(AMT);

                            XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                        }

                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                        if (rcptd.PaymentMode == false)
                        {
                            if (cnStr.ToLower().Contains("mumbai") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else if (cnStr.ToLower().Contains("nashik") == true)
                                LEDGERNAMEe.InnerText = "Cash";
                            else
                                LEDGERNAMEe.InnerText = "Petty Cash - Sinhgad Road"; //nashik, mumbai - Cash
                        }
                        else
                        {
                            LEDGERNAMEe.InnerText = ddlBankname.SelectedItem.Text;
                        }
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        if (rcptd.TDSAmount > 0)
                        {
                            AMOUNTav.InnerText = "-" + (rcptd.ReceiptAmount - rcptd.TDSAmount);
                        }
                        else
                        {
                            AMOUNTav.InnerText = "-" + rcptd.ReceiptAmount;
                        }
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        //
                        if (rcptd.PaymentMode == true)
                        {
                            XmlElement TEMPXmlElemt = xmldoc.CreateElement("DATE");
                            date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            string[] strChq = rcptd.ChqDetail.Split('|');

                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTDATE");
                            date = DateTime.ParseExact(strChq[2].ToString(),"dd/MM/yyyy",null);
                            TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKBRANCHNAME");
                            TEMPXmlElemt.InnerText = strChq[3];
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("TRANSACTIONTYPE");
                            TEMPXmlElemt.InnerText = "Cheque/DD";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("BANKNAME");
                            TEMPXmlElemt.InnerText = strChq[1];
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("PAYMENTFAVOURING");
                            TEMPXmlElemt.InnerText = lblClientName.Text;
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTNUMBER");
                            TEMPXmlElemt.InnerText = strChq[0].ToString();
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("UNIQUEREFERENCENUMBER");
                            TEMPXmlElemt.InnerText = "";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("STATUS");
                            TEMPXmlElemt.InnerText = "No";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("CHEQUEPRINTED");
                            TEMPXmlElemt.InnerText = "1";
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                            TEMPXmlElemt = xmldoc.CreateElement("AMOUNT");
                            if (rcptd.TDSAmount > 0)
                            {
                                TEMPXmlElemt.InnerText = "-" + (rcptd.ReceiptAmount - rcptd.TDSAmount);
                            }
                            {
                                TEMPXmlElemt.InnerText = "-" + rcptd.ReceiptAmount;
                            }
                            BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);
                        }
                        //
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);

                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                        //tds
                        XmlElement ALLLEDGERENTRIESMt = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSzt = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSzt.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSst = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSst.InnerText = "-1";
                        OLDAUDITENTRYIDSzt.AppendChild(OLDAUDITENTRYIDSst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRYIDSzt);

                        XmlElement LEDGERNAMEet = xmldoc.CreateElement("LEDGERNAME");
                        DateTime tempDate = Convert.ToDateTime(rcptd.Date);
                        if (tempDate.Month > 3)
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year) + "-" + (tempDate.Year + 1);
                        else
                            LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year - 1) + "-" + (tempDate.Year);

                        ALLLEDGERENTRIESMt.AppendChild(LEDGERNAMEet);
                        XmlElement GSTCLASSst = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESMt.AppendChild(GSTCLASSst);

                        XmlElement ISDEEMEDPOSITIVEat = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEat.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISDEEMEDPOSITIVEat);

                        XmlElement LEDGERFROMITEMat = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMat.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(LEDGERFROMITEMat);

                        XmlElement REMOVEZEROENTRIESqt = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESqt.InnerText = "No";
                        ALLLEDGERENTRIESMt.AppendChild(REMOVEZEROENTRIESqt);

                        XmlElement ISPARTYLEDGERwt = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERwt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISPARTYLEDGERwt);

                        XmlElement ISLASTDEEMEDPOSITIVErt = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVErt.InnerText = "Yes";
                        ALLLEDGERENTRIESMt.AppendChild(ISLASTDEEMEDPOSITIVErt);

                        XmlElement AMOUNTavt = xmldoc.CreateElement("AMOUNT");
                        AMOUNTavt.InnerText = "-" + rcptd.TDSAmount;
                        ALLLEDGERENTRIESMt.AppendChild(AMOUNTavt);

                        XmlElement BANKALLOCATIONSst = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BANKALLOCATIONSst);

                        XmlElement BILLALLOCATIONSst = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONst = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESst = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSst = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESswt = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement TAXBILLALLOCATIONSst = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSst = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSst = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSst = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSst = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESMt.AppendChild(BILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(INTERESTCOLLECTIONst);
                        ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRIESst);
                        ALLLEDGERENTRIESMt.AppendChild(ACCOUNTAUDITENTRIESSst);
                        ALLLEDGERENTRIESMt.AppendChild(AUDITENTRIESswt);
                        ALLLEDGERENTRIESMt.AppendChild(TAXBILLALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TAXOBJECTALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(TDSEXPENSEALLOCATIONSst);
                        ALLLEDGERENTRIESMt.AppendChild(VATSTATUTORYDETAILSSst);
                        ALLLEDGERENTRIESMt.AppendChild(COSTTRACKALLOCATIONSst);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESMt);
                        //

                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.Advance_Update_TallyStatus(Convert.ToInt32(lblBillNo.Text),"", true); 
                        break;
                    }
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "ADVANCERECEIPT_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "ADVANCERECEIPT_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "ADVANCERECEIPT_Metro";
            else
                fileName = "ADVANCERECEIPT_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }
        
        private void AdjustAdvanceNoteTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region credit note
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                    Label lblBillDate = (Label)grdBill.Rows[i].FindControl("lblBillDate");
                    Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                    Label lblClientName = (Label)grdBill.Rows[i].FindControl("lblClientName");
                    Label lblSiteName = (Label)grdBill.Rows[i].FindControl("lblSiteName");
                    //int j = 0;
                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    //double CreditAmt = 0;
                    var advNote = dc.Advance_View(null, null, 0, lblBillNo.Text, false).ToList();
                    foreach (var AdvAdj in advNote)
                    {

                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "Journal");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        NARRATION.InnerText = AdvAdj.Note;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (cnStr.ToLower().Contains("nashik") == true)
                            VOUCHERTYPENAME.InnerText = "Journal A";
                        else
                            VOUCHERTYPENAME.InnerText = "Journal"; // nashik- Journal A
                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        //if (lblBillNo.Text.Contains("-") == true)
                        //    VOUCHERNUMBER.InnerText = lblBillNo.Text.Replace("-", "");
                        //else
                            VOUCHERNUMBER.InnerText = lblBillNo.Text;

                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);
                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);
                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "No";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                        VCHISFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(VCHISFROMSYNC);

                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);

                        
                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAMEe.InnerText = lblSiteName.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        AMOUNTav.InnerText = "-" + lblBillAmount.Text;
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);
                                                        
                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);
                        
                        //second effect
                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblClientName.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblBillAmount.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);
                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        //
                        //on acc
                        var onAcc = dc.CashDetail_View_BillAdvNote(lblBillNo.Text, "0", false, true);
                        foreach (var onAc in onAcc)
                        {
                            if (onAc.BILL_NetAmt_num < 0)
                            {
                                XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement NAME = xmldoc.CreateElement("NAME");
                                XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                                
                                NAME.InnerText = "On A/c";
                                
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);

                                XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                                TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                                BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                                XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                                double tmpAmt = Convert.ToDouble(onAc.BILL_NetAmt_num * (-1));
                                AMT.InnerText = tmpAmt.ToString();                                
                                BILLALLOCATIONS.AppendChild(AMT);

                                XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                                ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                            }
                        }
                        //bill nos
                        var bills = dc.CashDetail_View_BillAdvNote(lblBillNo.Text, "0", false, false);
                        foreach (var bill in bills)
                        {
                            if (bill.billNoteAmt < 0)
                            {
                                XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement NAME = xmldoc.CreateElement("NAME");
                                XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                                int bl = 0;
                                if (Int32.TryParse(bill.BILL_Id.ToString() , out bl) == true)
                                    NAME.InnerText ="DT - " + bill.BILL_Id.ToString();
                                else
                                    NAME.InnerText = bill.BILL_Id.ToString();

                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);

                                XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                                TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                                BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                                XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                                double tmpAmt = Convert.ToDouble(bill.billNoteAmt * (-1));
                                AMT.InnerText = tmpAmt.ToString();
                                BILLALLOCATIONS.AppendChild(AMT);

                                XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                                ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                            }
                        }
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.Advance_Update_TallyStatus(0, lblBillNo.Text, true);
                        break;
                    }
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "ADVANCENOTE_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "ADVANCENOTE_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "ADVANCENOTE_Metro";
            else
                fileName = "ADVANCENOTE_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }
        
        private void createNode(string pID, string pName, string pPrice, XmlTextWriter writer)
        {
            writer.WriteStartElement("ENVELOPE");
            writer.WriteStartElement("Product_id");
            writer.WriteString(pID);
            writer.WriteEndElement();
            writer.WriteStartElement("Product_name");
            writer.WriteString(pName);
            writer.WriteEndElement();
            writer.WriteStartElement("Product_price");
            writer.WriteString(pPrice);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void optBill_CheckedChanged(object sender, EventArgs e)
        {
            grdBill.DataSource = null;
            grdBill.DataBind();
            if (optBill.Checked == true)
                lblFromDate.Text = "Approved From date : ";
            else
                lblFromDate.Text = "From date : ";
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdBill.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdBill.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
    }
}