using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;

using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;
using System.Globalization;

namespace DESPLWEB
{
    public partial class CashReceipt_Entry : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        //public static int Cl_Id = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
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
                        txt_receivedNo.Text = arrIndMsg[1].ToString().Trim();
                        arrIndMsg = arrMsgs[1].Split('=');
                        lblBillNo.Text = arrIndMsg[1].ToString().Trim();
                        arrIndMsg = arrMsgs[2].Split('=');
                        lblStatus.Text = arrIndMsg[1].ToString().Trim();
                    }
                }
                lblClientId.Text = "0";
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "New Receipt";
                    DisplayAboveDetails();
                    if (txt_receivedNo.Text != "Create New...")
                    {
                        if (Convert.ToInt32(txt_receivedNo.Text) > 0)
                        {   
                            if (lblStatus.Text == "Approve")
                            {
                                lblheading.Text = "Apporve Receipt";
                                LnkBtnSave.Text = "Approve";
                            }
                            else
                            {
                                lblheading.Text = "Modify Receipt";
                            }
                            DisplayCashDetail();
                        }
                    }
                }
            }
        }

        public void DisplayAboveDetails()
        {
            txt_date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_chquedate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_tdsallocatedamt.Text = "0.00";
            txt_acallocatedamt.Text = "0.00";
            rdn_cash.Checked = true;
            if (rdn_cash.Checked)
            {
                txt_AmtReceived.Enabled = true;
                txt_TDS.Enabled = true;
                txt_totalAmt.Enabled = true;
            }
            LnkBtnSave.Visible = true;
            txt_cheque_no.Enabled = false;
            ddl_Bank_Name.Enabled = false;
            txt_chquedate.Enabled = false;
            txt_Branch.Enabled = false;
            LoadBankList();
            LoadColletionUserList();
            SumAdjstAmt();
            SumTds();
        }

        private void LoadColletionUserList()
        {
            var CollectUser = dc.User_View(0, -1, "", "Collection", "");
            ddlUserCollected.DataSource = CollectUser;
            ddlUserCollected.DataTextField = "USER_Name_var";
            ddlUserCollected.DataValueField = "USER_Id";
            ddlUserCollected.DataBind();
            ddlUserCollected.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        public void LoadBankList()
        {
            var BL = dc.Bank_View("");
            ddl_Bank_Name.DataSource = BL;
            ddl_Bank_Name.DataTextField = "BANK_Name_var";
            ddl_Bank_Name.DataValueField = "BANK_Name_var";
            ddl_Bank_Name.DataBind();
            ddl_Bank_Name.Items.Insert(0, "---Select---");
        }

        public void EnableControls()
        {
            txt_cheque_no.Enabled = true;
            ddl_Bank_Name.Enabled = true;
            txt_AmtReceived.Enabled = true;
            txt_TDS.Enabled = true;
            txt_totalAmt.Enabled = true;
            txt_chquedate.Enabled = true;
            txt_Branch.Enabled = true;
            txt_tdsallocatedamt.Enabled = true;
            txt_acallocatedamt.Enabled = true;
            txtTotalTDSAmtGV.Enabled = true;
            txtTotalAmtGV.Enabled = true;
        }

        //protected void lnkAddNew_Click(object sender, CommandEventArgs e)
        //{
        //    ModalPopupExtender2.Show();
        //    PnlPopup.Visible = true;
        //    pnlCsh.Visible = true;
        //    txt_EnterBankName.Text = string.Empty;
        //    lblpMsg.ForeColor = System.Drawing.Color.Red;
        //    txt_EnterBankName.Focus();
        //    lnkSaveBankNmae.Enabled = true;
        //    lblpMsg.Text = "";
        //}

        //protected void lnkSaveBankNmae_Click(object sender, EventArgs e)
        //{
        //    if (txt_EnterBankName.Text != "")
        //    {
        //        if (ddl_Bank_Name.Items.FindByText(txt_EnterBankName.Text) == null)
        //        {
        //            ddl_Bank_Name.Items.Add(new ListItem(txt_EnterBankName.Text));
        //            lblpMsg.Visible = true;
        //            lblpMsg.ForeColor = System.Drawing.Color.Green;
        //            lblpMsg.Text = "Bank Name is Added Sucessfully";
        //            lnkSaveBankNmae.Enabled = false;
        //            ddl_Bank_Name.SelectedValue = txt_EnterBankName.Text;
        //        }
        //        else
        //        {
        //            txt_EnterBankName.Text = string.Empty;
        //        }
        //    }
        //    else
        //    {
        //        lblpMsg.Text = " Please Enter Bank Name ";
        //        lblpMsg.Visible = true;
        //        txt_EnterBankName.Focus();
        //    }
        //}

        //protected void lnkCancelCash_Click(object sender, EventArgs e)
        //{   
        //    ModalPopupExtender2.Hide();
        //}

        protected void rdn_cash_CheckedChanged(object sender, EventArgs e)
        {
            if (rdn_cash.Checked)
            {
                ClearChequeDetails();
                //enableText
                txt_AmtReceived.Enabled = true;
                txt_TDS.Enabled = true;
                txt_totalAmt.Enabled = true;
                txt_tdsallocatedamt.Enabled = true;
                txt_acallocatedamt.Enabled = true;
                //
                txt_cheque_no.Enabled = false;
                ddl_Bank_Name.Enabled = false;
                txt_chquedate.Enabled = false;
                txt_Branch.Enabled = false;
            }
        }

        protected void rdn_cheque_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
            LoadBankList();
            ClearChequeDetails();
        }

        public void ClearChequeDetails()
        {
            txt_cheque_no.Text = string.Empty;
            txt_chquedate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_Branch.Text = string.Empty;
            ddl_Bank_Name.SelectedValue = "---Select---";
        }

        public void DisplayCashDetail()
        {
            EnableControls();
            var res = dc.Cash_View(Convert.ToInt32(txt_receivedNo.Text), 0,false);
            foreach (var cas in res)
            {
                txt_date.Text = Convert.ToDateTime(cas.Cash_Date_date).ToString("dd/MM/yyyy");
                txt_receivedNo.Text = Convert.ToInt32(cas.Cash_ReceiptNo).ToString();
                //ddl_Client.SelectedValue = cas.Cash_ClientId_int.ToString();
                lblClientId.Text = cas.Cash_ClientId_int.ToString();
                txt_Client.Text = cas.CL_Name_var;
                //if (cas.CashDetail_Status_bit == true)
                if (cas.Cash_Status_bit == true)
                {
                    ddl_status.ClearSelection();
                    ddl_status.Items.FindByText("Hold").Selected = true;
                }
                txt_TDS.Text = Convert.ToDecimal(cas.Cash_TDS_num).ToString("0.00");
                txt_totalAmt.Text = Convert.ToDecimal(cas.Cash_Amount_num).ToString("0.00");
                txt_AmtReceived.Text = (Convert.ToDecimal(txt_totalAmt.Text) - Convert.ToDecimal(txt_TDS.Text)).ToString("0.00");

                if (Convert.ToString(cas.Cash_PaymentType_bit) == "True")
                {
                    rdn_cheque.Checked = true;
                    rdn_cash.Checked = false;
                    txt_cheque_no.Text = cas.Cash_ChequeNo_int.ToString();
                    txt_chquedate.Text = Convert.ToDateTime(cas.Cash_ChqDate_date).ToString("dd/MM/yyyy");
                    txt_Branch.Text = cas.Cash_Branch_var.ToString();
                    ddl_Bank_Name.SelectedValue = cas.Cash_BankName_var.ToString();
                    txt_chquedate.Enabled = true;
                    txt_cheque_no.Enabled = true;
                    txt_Branch.Enabled = true;
                    ddl_Bank_Name.Enabled = true;
                }
                else if (Convert.ToString(cas.Cash_PaymentType_bit) == "False")
                {
                    rdn_cash.Checked = true;
                    rdn_cheque.Checked = false;
                    txt_chquedate.Enabled = false;
                    txt_cheque_no.Enabled = false;
                    txt_Branch.Enabled = false;
                    ddl_Bank_Name.Enabled = false;
                }
                if (Convert.ToString(cas.Cash_CollectionDetail) != "")
                {
                    string[] CollectionDetail = new string[1];
                    CollectionDetail = Convert.ToString(cas.Cash_CollectionDetail).Split('|');
                    ddlUserCollected.SelectedValue = Convert.ToString(CollectionDetail[0]);
                    txt_userip.Text = Convert.ToString(CollectionDetail[1]);
                }
                else
                {
                    chkColleted.Checked = false;
                    ddlUserCollected.Enabled = false;
                    txt_userip.Enabled = false;
                }
                txt_Note.Text = cas.Cash_Note_var.ToString();
                txt_totalAmt.Text = (Convert.ToDecimal(txt_AmtReceived.Text) + Convert.ToDecimal(txt_TDS.Text)).ToString("0.00");
                txt_tdsallocatedamt.Text = (Convert.ToDecimal(txt_TDS.Text) - Convert.ToDecimal(txtTotalTDSAmtGV.Text)).ToString("0.00");
                txt_acallocatedamt.Text = (Convert.ToDecimal(txt_totalAmt.Text) - Convert.ToDecimal(txtTotalAmtGV.Text)).ToString("0.00");
            }
            //BindMainGrid();
            LoadReceiptDetails();
        }

        public void LoadReceiptDetails()
        {   
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("BILL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("BILL_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("BILL_NetAmt_num", typeof(string)));
            dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("CashDetail_Settlement_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CashDetail_TDSAmount_money", typeof(string)));
            dt.Columns.Add(new DataColumn("CashDetail_Amount_money", typeof(string)));

            ViewState["CurrentTable"] = null;
            int rowIndex = 0;
            decimal onAccAmt = 0, onAccTdsAmt = 0;
            //list of selected receipt
            if (txt_receivedNo.Text != "Create New..." && Convert.ToInt32(lblClientId.Text) > 0)
            {
                var receipt = dc.CashDetail_View_Receipt(Convert.ToInt32(txt_receivedNo.Text), "",Convert.ToInt32(lblClientId.Text));
                foreach (var rcpt in receipt)
                {

                    if (rcpt.CashDetail_BillNo_int != "")
                    {
                        drow = dt.NewRow();
                        drow["BILL_Id"] = rcpt.CashDetail_BillNo_int;
                        if (rcpt.CashDetail_BillNo_int.Contains("DB/") == true)
                        {
                            drow["BILL_Date_dt"] = Convert.ToDateTime(rcpt.Journal_Date_dt).ToString("dd/MM/yyyy");
                            drow["BILL_NetAmt_num"] = Convert.ToDecimal(rcpt.Journal_Amount_dec).ToString("0.00");
                            if (rcpt.billJrnlpendingAmt != null)
                            {
                                drow["BalanceAmount"] = Convert.ToDecimal(rcpt.Journal_Amount_dec + rcpt.billJrnlpendingAmt).ToString("0.00");
                            }
                            else
                            {
                                drow["BalanceAmount"] = Convert.ToDecimal(rcpt.Journal_Amount_dec).ToString("0.00");
                            }
                        }
                        else
                        {
                            drow["BILL_Date_dt"] = Convert.ToDateTime(rcpt.BILL_Date_dt).ToString("dd/MM/yyyy");
                            drow["BILL_NetAmt_num"] = Convert.ToDecimal(rcpt.BILL_NetAmt_num).ToString("0.00");
                            if (rcpt.billJrnlpendingAmt != null)
                            {
                                drow["BalanceAmount"] = Convert.ToDecimal(rcpt.BILL_NetAmt_num + rcpt.billJrnlpendingAmt).ToString("0.00");
                            }
                            else
                            {
                                drow["BalanceAmount"] = Convert.ToDecimal(rcpt.BILL_NetAmt_num).ToString("0.00");
                            }
                        }

                        drow["CashDetail_Settlement_var"] = rcpt.CashDetail_Settlement_var;
                        drow["CashDetail_TDSAmount_money"] = Math.Abs(Convert.ToDecimal(rcpt.CashDetail_TDSAmount_money)).ToString("0.00");
                        drow["CashDetail_Amount_money"] = Math.Abs(Convert.ToDecimal(rcpt.CashDetail_Amount_money)).ToString("0.00");
                        dt.Rows.Add(drow);
                        dt.AcceptChanges();
                        rowIndex++;
                    }
                    else if (rcpt.CashDetail_BillNo_int == "") //on acc
                    {
                        onAccTdsAmt = Convert.ToDecimal(rcpt.CashDetail_TDSAmount_money);
                        onAccAmt = Convert.ToDecimal(rcpt.CashDetail_Amount_money);
                        onAccTdsAmt = Math.Abs(onAccTdsAmt);
                        onAccAmt = Math.Abs(onAccAmt);
                    }
                }
                ViewState["CurrentTable"] = dt;
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
            }
            
            //list of all pending bills and journal notes of selected client
            if (Convert.ToInt32(lblClientId.Text) > 0)
            {
                var pendingBillJrnl = dc.CashDetail_View_Receipt(0, "",Convert.ToInt32(lblClientId.Text));
                foreach (var billJrnl in pendingBillJrnl)
                {
                    bool addFlag = true;
                    foreach (GridViewRow row in gvDetailsView.Rows)
                    {
                        TextBox vbillno = (TextBox)row.FindControl("txtBillNo");
                        if (vbillno.Text == Convert.ToString(billJrnl.BILL_Id))
                        {
                            addFlag = false;
                            break;
                        }
                    }
                    if (addFlag == true)
                    {
                        drow = dt.NewRow();
                        drow["BILL_Id"] = billJrnl.BILL_Id;
                        drow["BILL_Date_dt"] = Convert.ToDateTime(billJrnl.BILL_Date_dt).ToString("dd/MM/yyyy");
                        drow["BILL_NetAmt_num"] = Convert.ToDecimal(billJrnl.BILL_NetAmt_num).ToString("0.00");
                        if (billJrnl.billJrnlpendingAmt != null)
                        {
                            drow["BalanceAmount"] = Convert.ToDecimal(billJrnl.billJrnlpendingAmt).ToString("0.00");
                        }
                        else
                        {
                            drow["BalanceAmount"] = Convert.ToDecimal(billJrnl.BILL_NetAmt_num).ToString("0.00");
                        }

                        drow["CashDetail_Settlement_var"] = 0;
                        drow["CashDetail_TDSAmount_money"] = "0.00";
                        drow["CashDetail_Amount_money"] = 0;
                        dt.Rows.Add(drow);
                        dt.AcceptChanges();
                        rowIndex++;
                    }
                }
                ViewState["CurrentTable"] = dt;
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
            }
            //
            if (gvDetailsView.Rows.Count > 0)
            {
                if (onAccAmt > 0 || onAccTdsAmt > 0)
                {
                    TextBox txtTdsAmtFt = (TextBox)gvDetailsView.FooterRow.Cells[5].FindControl("txtTdsAmtFt");
                    TextBox txtAdjustAmtFt = (TextBox)gvDetailsView.FooterRow.Cells[6].FindControl("txtAdjustAmtFt");

                    txtTdsAmtFt.Text = onAccTdsAmt.ToString("0.00");
                    txtAdjustAmtFt.Text = onAccAmt.ToString("0.00");
                    txtAdjustAmtFt.Text = Convert.ToDecimal(Math.Abs(Convert.ToDecimal(txtAdjustAmtFt.Text))).ToString("0.00");
                    txtTotalAmtGV.Text = Convert.ToDecimal(Math.Abs(Convert.ToDecimal(txtAdjustAmtFt.Text))).ToString("0.00");
                    txtTotalTDSAmtGV.Text = Convert.ToDecimal(txtTdsAmtFt.Text).ToString("0.00");
                }
                if (txt_receivedNo.Text != "Create New...")
                {
                    //var st = dc.CashReportView(Convert.ToInt32(txt_receivedNo.Text), 0);
                    //foreach (var s in st)
                    //{
                    //    if (Convert.ToBoolean(s.BILL_Status_bit) == false)
                    //    {
                    //        if (ddl_status.SelectedItem.Text == "Ok" || ddl_status.SelectedItem.Text == "Hold")
                    //        {
                    //            SumOfBalanceAmt();
                    //            break;
                    //        }
                    //    }
                    //    else if (Convert.ToBoolean(s.BILL_Status_bit) == true)
                    //    {
                    //        if (ddl_status.SelectedItem.Text != "Hold")
                    //        {
                    //            SumOfBalanceAmt();
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            TextBox vBalanceamtv = (TextBox)gvDetailsView.Rows[0].Cells[3].FindControl("txtBalAmt");
                    //            if (vBalanceamtv.Text == "0.00")
                    //            {
                    //                LnkBtnSave.Visible = true;
                    //            }
                    //            else
                    //            {
                    //                LnkBtnSave.Visible = false;
                    //            }
                    //        }
                    //    }
                    //}
                    SumOfAdjst();
                    SumOfTds();
                }
            }
            else if (gvDetailsView.Rows.Count == 0)
            {
                drow = dt.NewRow();
                drow["CashDetail_Settlement_var"] = "0";
                dt.Rows.Add(drow);
                dt.AcceptChanges();
                rowIndex++;
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
                ByDefaultOnAc();
                //DisplayFooter();
            }
        }
        public void BindMainGrid()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("BILL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("BILL_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("BILL_NetAmt_num", typeof(string)));
            dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("CashDetail_Settlement_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CashDetail_TDSAmount_money", typeof(string)));
            dt.Columns.Add(new DataColumn("CashDetail_Amount_money", typeof(string)));

            if (lblBillNo.Text == "")
            {
                lblBillNo.Text = "0";
            }
            //list of all pending bills of selected client
            //var result = dc.CashDetail_View(Convert.ToInt32(txt_receivedNo.Text), Convert.ToInt32(lblClientId.Text), false, false, Convert.ToInt32(lblBillNo.Text));
            var result = dc.CashDetail_View(Convert.ToInt32(txt_receivedNo.Text), Convert.ToInt32(lblClientId.Text), false, false, 0);
            foreach (var display in result)
            {
                drow = dt.NewRow();
                drow["BILL_Id"] = display.BILL_Id.ToString();
                drow["BILL_Date_dt"] = Convert.ToDateTime(display.BILL_Date_dt).ToString("dd/MM/yyyy");//2
                drow["BILL_NetAmt_num"] = Convert.ToDecimal(display.BILL_NetAmt_num).ToString("0.00");
                if (display.CashDetail_Amount_money != null)
                {
                    drow["BalanceAmount"] = Convert.ToDecimal(display.CashDetail_Amount_money).ToString("0.00");
                }
                else
                {
                    drow["BalanceAmount"] = Convert.ToDecimal(display.BILL_NetAmt_num).ToString("0.00");
                }
                drow["CashDetail_Settlement_var"] = 0;
                drow["CashDetail_TDSAmount_money"] = "0.00";
                drow["CashDetail_Amount_money"] = Convert.ToDecimal("0.00");
                dt.Rows.Add(drow);
                dt.AcceptChanges();
                rowIndex++;
                ViewState["CurrentTable"] = dt;
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
            }
            bool valid = false;
            //list of bills not equal to selected receipt no
            var res = dc.CashDetail_View(Convert.ToInt32(txt_receivedNo.Text), Convert.ToInt32(lblClientId.Text), true, false, 0);
            foreach (var display in res)
            {
                foreach (GridViewRow row in gvDetailsView.Rows)
                {
                    TextBox vbillno = (TextBox)row.FindControl("txtBillNo");
                    if (vbillno.Text == Convert.ToString(display.BILL_Id))
                    {
                        valid = true;
                        break;
                    }
                }
                if (valid == false)
                {
                    drow = dt.NewRow();
                    drow["BILL_Id"] = display.BILL_Id.ToString();
                    drow["BILL_Date_dt"] = Convert.ToDateTime(display.BILL_Date_dt).ToString("dd/MM/yyyy");//2
                    drow["BILL_NetAmt_num"] = Convert.ToDecimal(display.BILL_NetAmt_num).ToString("0.00");
                    if (display.CashDetail_Amount_money != null)
                    {
                        drow["BalanceAmount"] = Convert.ToDecimal(display.CashDetail_Amount_money).ToString("0.00");
                    }
                    drow["CashDetail_Settlement_var"] = 0;
                    drow["CashDetail_TDSAmount_money"] = "0.00";
                    drow["CashDetail_Amount_money"] = 0;
                    dt.Rows.Add(drow);
                    dt.AcceptChanges();
                    ViewState["CurrentTable"] = dt;
                    rowIndex++;
                }
            }
            gvDetailsView.DataSource = dt;
            gvDetailsView.DataBind();
            //
            if (gvDetailsView.Rows.Count > 0)
            {
                DisplayGrdview();
                DisplayFooter();
                if (txt_receivedNo.Text != "Create New...")
                {
                    var st = dc.CashReportView(Convert.ToInt32(txt_receivedNo.Text), 0);
                    foreach (var s in st)
                    {
                        if (Convert.ToBoolean(s.BILL_Status_bit) == false)
                        {
                            if (ddl_status.SelectedItem.Text == "Ok" || ddl_status.SelectedItem.Text == "Hold")
                            {
                                SumOfBalanceAmt();
                                break;
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(s.BILL_Status_bit) == true)
                            {
                                if (ddl_status.SelectedItem.Text != "Hold")
                                {
                                    SumOfBalanceAmt();
                                    break;
                                }
                                else
                                {
                                    TextBox vBalanceamtv = (TextBox)gvDetailsView.Rows[0].Cells[3].FindControl("txtBalAmt");
                                    if (vBalanceamtv.Text == "0.00")
                                    {
                                        LnkBtnSave.Visible = true;
                                    }
                                    else
                                    {
                                        LnkBtnSave.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    SumOfAdjst();
                    SumOfTds();
                }
            }
            else if (gvDetailsView.Rows.Count == 0)
            {
                drow = dt.NewRow();
                drow["CashDetail_Settlement_var"] = "0";
                dt.Rows.Add(drow);
                dt.AcceptChanges();
                rowIndex++;
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
                DisplayFooter();
            }
        }

        public void DisplayGrdview()
        {
            int rowIndex = 0;
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            //list of bills of selected client not adjustead one time
            var result = dc.CashDetail_View(0, Convert.ToInt32(lblClientId.Text), false, true, 0);
            int i = 0;
            foreach (var chkBlw in result)
            {
                TextBox vBillno = (TextBox)gvDetailsView.Rows[i].Cells[0].FindControl("txtBillNo");
                TextBox vDte = (TextBox)gvDetailsView.Rows[i].Cells[1].FindControl("txtBillDate");
                TextBox vBillamt = (TextBox)gvDetailsView.Rows[i].Cells[2].FindControl("txtBillAmt");
                TextBox vBalanceamt = (TextBox)gvDetailsView.Rows[i].Cells[3].FindControl("txtBalAmt");
                DropDownList ddl = (DropDownList)gvDetailsView.Rows[i].Cells[4].FindControl("ddlSettlement");
                TextBox Vtds = (TextBox)gvDetailsView.Rows[i].Cells[5].FindControl("txtTdsAmt");
                TextBox Vadjustamout = (TextBox)gvDetailsView.Rows[i].Cells[6].FindControl("txtAdjustAmt");
                
                vBillno.Text = chkBlw.BILL_Id.ToString();
                vDte.Text = Convert.ToDateTime(chkBlw.BILL_Date_dt).ToString("dd/MM/yyyy");
                vBillamt.Text = Convert.ToDecimal(chkBlw.BILL_NetAmt_num).ToString("0.00");
                Vtds.Text = Convert.ToDecimal("0.00").ToString();
                ddl.SelectedValue = "0";
                Vadjustamout.Text = Convert.ToDecimal("0.00").ToString();
                vBalanceamt.Text = Convert.ToDecimal(chkBlw.BILL_NetAmt_num).ToString();

                DataRow NewRow = dt.NewRow();
                NewRow[0] = chkBlw.BILL_Id.ToString();
                NewRow[1] = Convert.ToDateTime(chkBlw.BILL_Date_dt).ToString("dd/MM/yyyy");
                NewRow[2] = Convert.ToDecimal(chkBlw.BILL_NetAmt_num).ToString("0.00");
                NewRow[3] = Convert.ToDecimal(vBalanceamt.Text).ToString("0.00");
                NewRow[4] = "0";
                NewRow[5] = Convert.ToDecimal("0.00").ToString();
                NewRow[6] = Convert.ToDecimal("0.00").ToString();

                dt.Rows.Add(NewRow);
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
            }
            rowIndex++;
            //
            if (txt_receivedNo.Text != "Create New...")
            {
                if (Convert.ToInt32(txt_receivedNo.Text) > 0)
                {   
                    var reslt = dc.CashDetail_ViewGrD(Convert.ToInt32(txt_receivedNo.Text));
                    int ii = 0;
                    foreach (var datarow in reslt)
                    {
                        for (int j = 0; j < gvDetailsView.Rows.Count; j++)
                        {
                            TextBox txtBillNo = (TextBox)gvDetailsView.Rows[j].FindControl("txtBillNo");
                            if (datarow.CashDetail_BillNo_int == txtBillNo.Text)
                            {
                                DropDownList vS1 = (DropDownList)gvDetailsView.Rows[j].Cells[4].FindControl("ddlSettlement");
                                TextBox vT1 = (TextBox)gvDetailsView.Rows[j].Cells[5].FindControl("txtTdsAmt");
                                TextBox vA1 = (TextBox)gvDetailsView.Rows[j].Cells[6].FindControl("txtAdjustAmt");

                                vS1.Text = datarow.CashDetail_Settlement_var.ToString();
                                vT1.Text = Convert.ToDecimal(datarow.CashDetail_TDSAmount_money).ToString("0.00");
                                vA1.Text = Convert.ToDecimal(datarow.CashDetail_Amount_money).ToString("0.00");
                                vA1.Text = Convert.ToDecimal(Math.Abs(Convert.ToDecimal((datarow.CashDetail_Amount_money)))).ToString("0.00");
                                break;
                            }
                        }
                        ii++;
                    }
                }
            }
        }

        protected void txt_AmtReceived_TextChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            string oldText = string.Empty;
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_AmtReceived.Text, "^[0-9]+(.[0-9]{1,2})?$"))
            {
                if (txt_AmtReceived.Text != "")
                {
                    if (txt_TDS.Text == "")
                    {
                        txt_TDS.Text = Convert.ToDecimal(0.00).ToString();
                        txt_TDS.Focus();
                    }
                    txt_totalAmt.Text = Convert.ToString(Convert.ToDecimal(txt_AmtReceived.Text) + Convert.ToDecimal(txt_TDS.Text));
                    txt_acallocatedamt.Text = (Convert.ToDecimal(txt_totalAmt.Text) - Convert.ToDecimal(txtTotalAmtGV.Text)).ToString("0.00");
                }
            }
            else
            {
                txt_AmtReceived.Text = txt_AmtReceived.Text.Remove(txt_AmtReceived.Text.Length - 1);
                lblMsg.Text = "Please enter only Numbers";
                lblMsg.Visible = true;
                txt_AmtReceived.Attributes.Add("onfocus", "this.select();");
                txt_AmtReceived.Text = string.Empty;
                txt_AmtReceived.Focus();
                txt_TDS.Text = string.Empty;
                txt_totalAmt.Text = string.Empty;
            }

        }
        
        protected void txt_TDS_TextChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            txt_tdsallocatedamt.Text = Convert.ToDecimal("0.00").ToString();
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_TDS.Text, "^[0-9]+(.[0-9]{1,2})?$"))
            {
                if (txt_AmtReceived.Text != "" && txt_TDS.Text != "")
                {
                    txt_totalAmt.Text = Convert.ToString(Convert.ToDecimal(txt_AmtReceived.Text) + Convert.ToDecimal(txt_TDS.Text));
                    txt_tdsallocatedamt.Text = (Convert.ToDecimal(txt_TDS.Text) - Convert.ToDecimal(txtTotalTDSAmtGV.Text)).ToString("0.00");
                }
                if (txt_TDS.Text == "" || Convert.ToDouble(txt_TDS.Text) == 0)
                {
                    txtTotalTDSAmtGV.Text = "0.00";
                    TextBox txtTdsAmtFt = (TextBox)gvDetailsView.FooterRow.FindControl("txtTdsAmtFt");
                    txtTdsAmtFt.Text = "0.00";
                    for (int i = 0; i < gvDetailsView.Rows.Count; i++)
                    {
                        TextBox txtTdsAmt = (TextBox)gvDetailsView.Rows[i].FindControl("txtTdsAmt");
                        txtTdsAmt.Text = "0.00";
                    }
                }
            }
            else
            {
                lblMsg.Text = "Please enter only Numbers";
                lblMsg.Visible = true;
                txt_TDS.Attributes.Add("onfocus", "this.select();");
                txt_TDS.Text = string.Empty;
                txt_TDS.Focus();
                txt_totalAmt.Text = string.Empty;
            }
        }
        
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (txt_Client.Text == "" || lblClientId.Text == "0")
            {
                lblMsg.Text = "Please Select the Client Name ";
                txt_Client.Focus();
                valid = false;
            }
            else if (txt_date.Text == "")
            {
                lblMsg.Text = "Please Enter Date ";
                txt_date.Focus();
                valid = false;
            }
            else if (rdn_cheque.Checked == false && rdn_cash.Checked == false)
            {
                lblMsg.Text = "Please select anyone of these radio button";
                valid = false;
            }
            else if (rdn_cheque.Checked == true )
            {
                DateTime dt;
                string format = "ddd dd MMM h:mm tt yyyy";
                if (txt_cheque_no.Text == "")
                {
                    lblMsg.Text = "Please Enter Check No ";
                    txt_cheque_no.Focus();
                    valid = false;
                }
                //else if (DateTime.TryParse(txt_date.Text, out dt) == false)
                else if (DateTime.TryParseExact(txt_date.Text, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dt))
                {
                    lblMsg.Text = "Please enter valid receipt date ";
                    txt_date.Focus();
                    valid = false;
                }
                //else if (DateTime.TryParse(txt_chquedate.Text, out dt) == false)
                else if (DateTime.TryParseExact(txt_chquedate.Text, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dt))
                {
                    lblMsg.Text = "Please enter valid cheque date ";
                    txt_chquedate.Focus();
                    valid = false;
                }

                if (valid == true)
                {   
                    //DateTime receiptDate = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                    //DateTime chqDate = DateTime.ParseExact(txt_chquedate.Text, "dd/MM/yyyy", null);
                    DateTime currentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    DateTime receiptDate = new DateTime();
                    DateTime chqDate = new DateTime();
                    //DateTime currentDate = new DateTime();
                    string[] strDate = new string[3];

                    strDate = txt_date.Text.Split('/');
                    receiptDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 0, 0, 0);
                    strDate = txt_chquedate.Text.Split('/');
                    chqDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 0, 0, 0);

                    if (chqDate > receiptDate)
                    {
                        lblMsg.Text = "Please Enter cheque date less than equal to receipt date ";
                        txt_chquedate.Focus();
                        valid = false;
                    }
                    else if (chqDate > currentDate)
                    {
                        lblMsg.Text = "Please Enter cheque date less than equal to current date ";
                        txt_chquedate.Focus();
                        valid = false;
                    }
                }
                if (valid == true)
                {
                    if (txt_Branch.Text == "")
                    {
                        lblMsg.Text = "Please Enter Branch Name ";
                        txt_Branch.Focus();
                        valid = false;
                    }
                    else if (ddl_Bank_Name.SelectedValue == "---Select---")
                    {
                        lblMsg.Text = "Please Select Bank Name ";
                        ddl_Bank_Name.Focus();
                        valid = false;
                    }
                }
            }
            if (valid == true)
            {
                if (txt_AmtReceived.Text == "")
                {
                    lblMsg.Text = "Please Enter Receipt Amount ";
                    txt_AmtReceived.Focus();
                    valid = false;
                }
                else if (txt_TDS.Text == "")
                {
                    lblMsg.Text = "Please Enter TDS Amount ";
                    txt_TDS.Focus();
                    valid = false;
                }
                else if (txtTotalAmtGV.Text == "")
                {
                    lblMsg.Text = "Please Enter some Adjust Amount";
                    txtTotalAmtGV.Focus();
                    valid = false;
                }
            }
            if (valid == true)
            {
                for (int i = 0; i < gvDetailsView.Rows.Count; i++)
                {
                    TextBox txtBalAmt = (TextBox)gvDetailsView.Rows[i].FindControl("txtBalAmt");
                    TextBox txtTdsAmt = (TextBox)gvDetailsView.Rows[i].FindControl("txtTdsAmt");
                    TextBox txtAdjustAmt = (TextBox)gvDetailsView.Rows[i].FindControl("txtAdjustAmt");
                    DropDownList ddlSettlement = (DropDownList)gvDetailsView.Rows[i].FindControl("ddlSettlement");

                    TextBox txtTdsAmtFt = (TextBox)gvDetailsView.FooterRow.Cells[5].FindControl("txtTdsAmtFt");
                    TextBox txtAdjustAmtFt = (TextBox)gvDetailsView.FooterRow.Cells[6].FindControl("txtAdjustAmtFt");
                    if (txtTdsAmt.Text != "0.00" || txtAdjustAmt.Text != "0.00")
                    {
                        if (txtBalAmt.Text == "")
                        {
                            txtBalAmt.Text = "0.00";
                            txtAdjustAmt.Text = "0.00";
                        }
                        if (ddlSettlement.SelectedIndex == 0)
                        {
                            lblMsg.Text = "Please select Settlement";
                            ddlSettlement.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtTdsAmt.Text == "")
                        {
                            lblMsg.Text = "Please enter TDS Amount";
                            txtTdsAmt.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtAdjustAmt.Text == "")
                        {
                            lblMsg.Text = "Please enter Adjust Amount";
                            txtAdjustAmt.Focus();
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDecimal(txtAdjustAmt.Text) > Convert.ToDecimal(txtBalAmt.Text))
                        {
                            lblMsg.Text = "Adjust Amount should not be greater than  Balance Amount Sr No  " + (i + 1) + ".";
                            txtAdjustAmt.Attributes.Add("onfocus", "this.select();");
                            txtAdjustAmt.Focus();
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDecimal(txtAdjustAmt.Text) <= 0 && Convert.ToDecimal(txtTdsAmt.Text) > 0)
                        {
                            lblMsg.Text = "Please enter adjust amount for row no. " + (i + 1) + ".";
                            txtTdsAmt.Attributes.Add("onfocus", "this.select();");
                            txtTdsAmt.Focus();
                            valid = false;
                            break;
                        }
                        //else if (Convert.ToDecimal(txtAdjustAmtFt.Text) > 0 && Convert.ToDecimal(txtTdsAmtFt.Text) <= 0)
                        //{
                        //    lblMsg.Text = "Please enter TDS Amount of On A/C row  " + (i + 1) + ".";
                        //    txtTdsAmtFt.Attributes.Add("onfocus", "this.select();");
                        //    txtTdsAmtFt.Focus();
                        //    valid = false;
                        //    break;
                        //}
                    }
                }
            }
            if (valid == true)
            {
                if (Convert.ToDecimal(txt_totalAmt.Text) != Convert.ToDecimal(txtTotalAmtGV.Text))
                {
                    lblMsg.Text = "Total Adjusted Amount doesnot match to the total Receipt Amount ";
                    txtTotalAmtGV.Attributes.Add("onfocus", "this.select();");
                    valid = false;
                }
                else if (Convert.ToDecimal(txt_TDS.Text) != Convert.ToDecimal(txtTotalTDSAmtGV.Text)
                    && Convert.ToDecimal(txtTotalTDSAmtGV.Text) != 0)
                {
                    lblMsg.Text = "Total Adjusted TDS Amount doesnot match to the total Receipt TDS Amount ";
                    txt_TDS.Attributes.Add("onfocus", "this.select();");
                    valid = false;
                }
                else if (txtTotalAmtGV.Text == "")
                {
                    lblMsg.Text = "Enter some Adjust Amount";
                    txt_AmtReceived.Focus();
                    valid = false;
                }
                else if (txt_Note.Text == "")
                {
                    lblMsg.Text = " Enter some Note";
                    txt_Note.Focus();
                    valid = false;
                }
                else if (chkColleted.Checked && ddlUserCollected.SelectedItem.Text == "---Select---")
                {
                    lblMsg.Text = " Select collected from ";
                    ddlUserCollected.Focus();
                    valid = false;
                }
                else if (chkColleted.Checked && txt_userip.Text == "")
                {
                    lblMsg.Text = " fill the colleted from ";
                    txt_userip.Focus();
                    valid = false;
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

        protected void LnkBtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                //if (Convert.ToInt32(ddl_Client.SelectedValue) != 0)
                //{
                //    Cl_Id = Convert.ToInt32(ddl_Client.SelectedValue);
                //}
                if (txt_Client.Text != "")
                {
                    string ClientId = Request.Form[hfClientId.UniqueID];
                    if (ClientId != "")
                    {
                        lblClientId.Text = ClientId;
                    }
                }

                bool chequeStatus = false;
                if (rdn_cheque.Checked)
                    chequeStatus = true;
                else if (rdn_cash.Checked)
                    chequeStatus = false;
                
                int ReceiptNo = 0;
                if (txt_receivedNo.Text != "Create New...")
                    ReceiptNo = Convert.ToInt32(txt_receivedNo.Text);
                else
                    txt_receivedNo.Text = "Create New...";
                
                string CollectionDetail = "";
                if (chkColleted.Checked)
                    CollectionDetail = ddlUserCollected.SelectedValue + "|" + txt_userip.Text;
                //delete cash detail records 
                dc.CashDetail_Update(ReceiptNo, "", null, 0, "", 0, false, false, Convert.ToInt32(lblClientId.Text), "", true, false);
                //add cash detail records
                foreach (GridViewRow row in gvDetailsView.Rows)
                {
                    TextBox vbillno = (TextBox)row.FindControl("txtBillNo");
                    TextBox vbillnetamt = (TextBox)row.FindControl("txtBillAmt");
                    DropDownList vSettlement = (DropDownList)row.FindControl("ddlSettlement");
                    TextBox vTds = (TextBox)row.FindControl("txtTdsAmt");
                    TextBox vAdjstamt = (TextBox)row.FindControl("txtAdjustAmt");

                    DateTime checkDate = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                    if (Convert.ToDecimal(vAdjstamt.Text) > 0)
                    {
                        dc.CashDetail_Update(ReceiptNo, Convert.ToString(vbillno.Text), checkDate, Convert.ToDecimal(-(Convert.ToDecimal(vAdjstamt.Text))), vSettlement.SelectedItem.Text, Convert.ToDecimal(vTds.Text), Convert.ToBoolean(ddl_status.SelectedValue), false, Convert.ToInt32(lblClientId.Text), "", false, false);//Convert.ToDecimal(vAdjstamt.Text)
                    }
                }
                //Footer row - update on a/c
                Label FBillNo = (Label)gvDetailsView.FooterRow.Cells[0].FindControl("lblAcF");
                TextBox Fdate = (TextBox)gvDetailsView.FooterRow.Cells[1].FindControl("txtBillDateFt");
                TextBox Fnetamt = (TextBox)gvDetailsView.FooterRow.Cells[2].FindControl("txtBillAmtFt");//
                TextBox Fbalanceamt = (TextBox)gvDetailsView.FooterRow.Cells[3].FindControl("txtBalAmtFt");
                TextBox FSettlemt = (TextBox)gvDetailsView.FooterRow.Cells[4].FindControl("txtSettlementFt");
                TextBox Ftds = (TextBox)gvDetailsView.FooterRow.Cells[5].FindControl("txtTdsAmtFt");
                TextBox Fadjst = (TextBox)gvDetailsView.FooterRow.Cells[6].FindControl("txtAdjustAmtFt");
                DateTime FcheckDate = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                if (Ftds.Text != "0.00" || Fadjst.Text != "0.00")
                {
                    FBillNo.Text = null;
                    FSettlemt.Text = "On A/c";
                    dc.CashDetail_Update(ReceiptNo, FBillNo.Text, FcheckDate, Convert.ToDecimal(-(Convert.ToDecimal(Fadjst.Text))), FSettlemt.Text, Convert.ToDecimal(Ftds.Text), Convert.ToBoolean(ddl_status.SelectedValue), false, Convert.ToInt32(lblClientId.Text), "", false, false);
                    FBillNo.Text = "On A/c";
                    FSettlemt.Text = "-";
                }
                //
                DateTime receiptDate = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                Nullable<DateTime> chqDate = null;
                Nullable<int> chqNo = null;
                string strBranch = "";
                if (rdn_cheque.Checked)
                {
                    chqDate = DateTime.ParseExact(txt_chquedate.Text, "dd/MM/yyyy", null);
                    chqNo = Convert.ToInt32(txt_cheque_no.Text);
                    strBranch = txt_Branch.Text;
                }
                txt_receivedNo.Text = dc.Cash_Update(ReceiptNo, receiptDate, Convert.ToDecimal(txt_totalAmt.Text), Convert.ToDecimal(txt_TDS.Text), Convert.ToInt32(lblClientId.Text), ddl_Bank_Name.SelectedValue, chqNo, strBranch, chqDate, chequeStatus, txt_Note.Text, CollectionDetail, Convert.ToInt32(Session["LoginID"]), Convert.ToBoolean(ddl_status.SelectedValue), false).ToString();
                //if (txt_receivedNo.Text == "Create New...")
                //{
                //    var recp = dc.CashReportView(0, 0);
                //    foreach (var c in recp)
                //    {
                //        txt_receivedNo.Text = c.Cash_ReceiptNo.ToString();
                //    }
                //}

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;

                if (lblStatus.Text == "Approve")
                {
                    dc.CashDetail_Update(Convert.ToInt32(txt_receivedNo.Text), "", null, 0, "", 0, false, true, 0, "", false, true);
                    lblMsg.Text = "Records has been successfully approved";
                    lblStatus.Text = "";
                }
                else
                {
                    lblMsg.Text = "Record Saved Successfully";
                }
                LnkBtnSave.Enabled = false;
                lnkPrint.Visible = true;
                lnkNext.Visible = true;
            }
        }
        
        public void SumAdjstAmt()
        {
            decimal total = 0;
            if (gvDetailsView.Rows.Count == 0)
            {
                txtTotalAmtGV.Text = total.ToString("0.00");
            }
            else
            {
                foreach (GridViewRow r in gvDetailsView.Rows)
                {
                    var textT = r.FindControl("txtAdjustAmt") as TextBox;
                    decimal number;
                    if (decimal.TryParse(textT.Text, out number))
                    {
                        total += number;
                    }
                    txtTotalAmtGV.Text = total.ToString("0.00");
                }
            }
        }
        
        public void SumTds()
        {
            decimal total = 0;
            if (gvDetailsView.Rows.Count == 0)
            {
                txtTotalTDSAmtGV.Text = total.ToString("0.00");
            }
            else
            {
                foreach (GridViewRow r in gvDetailsView.Rows)
                {
                    var textT = r.FindControl("txtTdsAmt") as TextBox;
                    decimal number;
                    if (decimal.TryParse(textT.Text, out number))
                    {
                        total += number;
                    }
                    txtTotalTDSAmtGV.Text = total.ToString("0.00");
                }
            }

        }

        public void SumOfAdjst()
        {
            GridViewRow rowfooter = gvDetailsView.FooterRow;
            TextBox adjustmentfooter = ((TextBox)rowfooter.FindControl("txtAdjustAmtFt"));
            decimal total = 0;
            foreach (GridViewRow r in gvDetailsView.Rows)
            {
                var textT = r.FindControl("txtAdjustAmt") as TextBox;
                decimal number;
                if (decimal.TryParse(textT.Text, out number))
                {
                    total += number;
                }

                txtTotalAmtGV.Text = total.ToString("0.00");
                txt_acallocatedamt.Text = (Convert.ToDecimal(txt_totalAmt.Text) - Convert.ToDecimal(txtTotalAmtGV.Text)).ToString("0.00");

            }
            total += Convert.ToDecimal(adjustmentfooter.Text);
            txtTotalAmtGV.Text = total.ToString("0.00");
        }

        public void SumOfTds()
        {
            GridViewRow rowfooter = gvDetailsView.FooterRow;
            TextBox TDSfooter = ((TextBox)rowfooter.FindControl("txtTdsAmtFt"));
            decimal total = 0;
            foreach (GridViewRow r in gvDetailsView.Rows)
            {
                var textT = r.FindControl("txtTdsAmt") as TextBox;
                decimal number;
                if (decimal.TryParse(textT.Text, out number))
                {
                    total += number;
                }
                txtTotalTDSAmtGV.Text = total.ToString("0.00");
                txt_tdsallocatedamt.Text = (Convert.ToDecimal(txt_TDS.Text) - Convert.ToDecimal(txtTotalTDSAmtGV.Text)).ToString("0.00");
            }
            total += Convert.ToDecimal(TDSfooter.Text);
            txtTotalTDSAmtGV.Text = total.ToString("0.00");
        }
        
        protected void txtTdsAmt_TextChanged(object sender, EventArgs eventArgs)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox TDSamount = (TextBox)gvDetailsView.Rows[rowindex].Cells[5].FindControl("txtTdsAmt");
            GridViewRow rowfooter = gvDetailsView.FooterRow;
            TextBox TDSfooter = ((TextBox)rowfooter.FindControl("txtTdsAmtFt"));
            if (System.Text.RegularExpressions.Regex.IsMatch(TDSamount.Text, "^[0-9]+(.[0-9]{1,2})?$"))
            {
                if (txt_AmtReceived.Text == string.Empty || txt_TDS.Text == string.Empty)
                {
                    lblMsg.Text = "Please enter above Receipt Amount and TDS Amount";
                    lblMsg.Visible = true;
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Please enter above Receipt Amount and TDS Amount')", true);
                    txt_AmtReceived.Focus();
                    txt_TDS.Focus();
                    TDSamount.Text = Convert.ToDecimal("0.00").ToString();
                }
                else
                {
                    decimal total = 0;
                    foreach (GridViewRow r in gvDetailsView.Rows)
                    {
                        var textT = r.FindControl("txtTdsAmt") as TextBox;
                        decimal number;
                        if (decimal.TryParse(textT.Text, out number))
                        {
                            total += number;
                        }
                    }
                    txtTotalTDSAmtGV.Text = total.ToString("0.00");
                    total += Convert.ToDecimal(TDSfooter.Text);
                    txtTotalTDSAmtGV.Text = total.ToString("0.00");
                    TDSamount.Focus();
                    txt_tdsallocatedamt.Text = (Convert.ToDecimal(txt_TDS.Text) - Convert.ToDecimal(txtTotalTDSAmtGV.Text)).ToString("0.00");
                }
            }
            else
            {
                lblMsg.Text = "Please enter only Numbers";
                lblMsg.Visible = true;
                SumTds();
                TDSamount.Attributes.Add("onfocus", "this.select();");
                TDSamount.Text = Convert.ToDecimal("0.00").ToString();
                TDSamount.Focus();
            }
        }

        protected void gvDetailsView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region rowdatabound
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox DataAdjstAmt = (TextBox)e.Row.FindControl("txtAdjustAmt");
                if (DataAdjstAmt.Text == "0")
                {
                    DataAdjstAmt.Text = "0.00";
                }
                DropDownList ddlSettlement = e.Row.FindControl("ddlSettlement") as DropDownList;

                if (ddlSettlement != null)
                {
                    ddlSettlement.SelectedIndexChanged += new EventHandler(ddlSettlement_SelectedIndexChanged);
                }
                TextBox dbillno = (TextBox)e.Row.FindControl("txtBillNo");
                TextBox ddate = (TextBox)e.Row.FindControl("txtBillDate");
                TextBox dbilamt = (TextBox)e.Row.FindControl("txtBillAmt");
                TextBox dbalamt = (TextBox)e.Row.FindControl("txtBalAmt");
                TextBox dtds = (TextBox)e.Row.FindControl("txtTdsAmt");

                if (DataAdjstAmt.Text == string.Empty && dbillno.Text == string.Empty && ddate.Text == string.Empty && dbilamt.Text == string.Empty)
                {
                    DataAdjstAmt.Text = "0.00";
                    dtds.Text = "0.00";
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;

                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox FooterTdsAmt = (TextBox)e.Row.FindControl("txtTdsAmtFt");
                TextBox FooterAdjstAmt = (TextBox)e.Row.FindControl("txtAdjustAmtFt");

                if (FooterTdsAmt.Text == string.Empty && FooterAdjstAmt.Text == string.Empty)
                {
                    FooterTdsAmt.Text = "0.00";
                    FooterAdjstAmt.Text = "0.00";
                }

            }
            #endregion
        }

        protected void ddlSettlement_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;
            TextBox Balnceamount = (TextBox)gvDetailsView.Rows[idx].Cells[3].FindControl("txtBalAmt");
            TextBox Adjustamount = (TextBox)gvDetailsView.Rows[idx].Cells[6].FindControl("txtAdjustAmt");
            DropDownList ddlsettlement = (DropDownList)gvDetailsView.Rows[idx].Cells[4].FindControl("ddlSettlement");
            if (ddlsettlement.SelectedItem.Text == "Full")
            {
                for (int i = 0; i <= gvDetailsView.Rows.Count - 1; i++)
                {
                    Adjustamount.Text = Convert.ToDecimal(Convert.ToDecimal(Balnceamount.Text)).ToString("0.00");
                }
                SumAdjstAmt();
            }
            else if (ddlsettlement.SelectedItem.Text == "Part")
            {
                if (Adjustamount.Text == Balnceamount.Text)
                {
                    Adjustamount.Text = Convert.ToDecimal("0.00").ToString();
                    SumAdjstAmt();
                }
                else
                {
                    SumAdjstAmt();
                }
            }
        }

        protected void txtAdjustAmt_TextChanged(object sender, EventArgs eventArgs)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            DropDownList ddlsettlement = (DropDownList)gvDetailsView.Rows[rowindex].Cells[4].FindControl("ddlSettlement");
            TextBox Adjustamount = (TextBox)gvDetailsView.Rows[rowindex].Cells[6].FindControl("txtAdjustAmt");
            TextBox Balanceamt = (TextBox)gvDetailsView.Rows[rowindex].Cells[3].FindControl("txtBalAmt");
            GridViewRow rowfooter = gvDetailsView.FooterRow;
            TextBox adjustmentfooter = ((TextBox)rowfooter.FindControl("txtAdjustAmtFt"));
            if (System.Text.RegularExpressions.Regex.IsMatch(Adjustamount.Text, "^[0-9]+(.[0-9]{1,2})?$"))
            {

                if (txt_AmtReceived.Text == string.Empty || txt_TDS.Text == string.Empty)
                {
                    lblMsg.Text = "Please Enter above Receipt Amount and TDS Amount";
                    lblMsg.Visible = true;
                    txt_AmtReceived.Focus();
                    Adjustamount.Text = Convert.ToDecimal("0.00").ToString();
                }
                else
                {
                    if (Convert.ToDecimal(Adjustamount.Text) < Convert.ToDecimal(Balanceamt.Text))
                    {
                        ddlsettlement.ClearSelection();
                        ddlsettlement.Items.FindByText("Part").Selected = true;
                        SumAdjstAmt();
                    }
                    else if (Convert.ToDecimal(Adjustamount.Text) == Convert.ToDecimal(Balanceamt.Text))
                    {
                        ddlsettlement.ClearSelection();
                        ddlsettlement.Items.FindByText("Full").Selected = true;
                        SumAdjstAmt();
                    }
                    decimal total = 0;
                    foreach (GridViewRow r in gvDetailsView.Rows)
                    {
                        var textT = r.FindControl("txtAdjustAmt") as TextBox;
                        decimal number;
                        if (decimal.TryParse(textT.Text, out number))
                        {
                            total += number;
                        }
                    }
                    txtTotalAmtGV.Text = total.ToString("0.00");
                    total += Convert.ToDecimal(adjustmentfooter.Text);
                    txtTotalAmtGV.Text = total.ToString("0.00");
                    Adjustamount.Focus();
                    txt_acallocatedamt.Text = (Convert.ToDecimal(txt_totalAmt.Text) - Convert.ToDecimal(txtTotalAmtGV.Text)).ToString("0.00");
                }
            }
            else
            {
                lblMsg.Text = "Please enter only Numbers";
                lblMsg.Visible = true;
                SumAdjstAmt();
                Adjustamount.Attributes.Add("onfocus", "this.select();");
                Adjustamount.Text = Convert.ToDecimal("0.00").ToString();
                Adjustamount.Focus();
            }

        }
        
        protected void txtTdsAmtFt_TextChanged(object sender, EventArgs eventArgs)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            GridViewRow rowfooter = gvDetailsView.FooterRow;
            TextBox TDSfooter = ((TextBox)rowfooter.FindControl("txtTdsAmtFt"));
            if (System.Text.RegularExpressions.Regex.IsMatch(TDSfooter.Text, "^[0-9]+(.[0-9]{1,2})?$"))
            {
                if (txt_AmtReceived.Text == string.Empty || txt_TDS.Text == string.Empty)
                {
                    lblMsg.Text = "Please enter above Received Amount and TDS Amount !";
                    lblMsg.Visible = true;
                    txt_AmtReceived.Focus();
                    txt_TDS.Focus();
                    TDSfooter.Text = Convert.ToDecimal("0.00").ToString();
                }
                else
                {

                    decimal total = 0;
                    foreach (GridViewRow r in gvDetailsView.Rows)
                    {
                        var textT = r.FindControl("txtTdsAmt") as TextBox;
                        decimal number;
                        if (decimal.TryParse(textT.Text, out number))
                        {
                            total += number;
                        }
                    }
                    total += Convert.ToDecimal(TDSfooter.Text);
                    txtTotalTDSAmtGV.Text = total.ToString("0.00");
                    TDSfooter.Focus();
                    txt_tdsallocatedamt.Text = (Convert.ToDecimal(txt_TDS.Text) - Convert.ToDecimal(txtTotalTDSAmtGV.Text)).ToString("0.00");
                }
            }
            else
            {
                lblMsg.Text = "Please enter only Numbers";
                lblMsg.Visible = true;
                TDSfooter.Attributes.Add("onfocus", "this.select();");
                TDSfooter.Text = Convert.ToDecimal("0.00").ToString();
                TDSfooter.Focus();
            }
        }

        protected void txtAdjustAmtFt_TextChanged(object sender, EventArgs eventArgs)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            GridViewRow rowfooter = gvDetailsView.FooterRow;
            TextBox adjustmentfooter = ((TextBox)rowfooter.FindControl("txtAdjustAmtFt"));
            if (System.Text.RegularExpressions.Regex.IsMatch(adjustmentfooter.Text, "^[0-9]+(.[0-9]{1,2})?$"))
            {
                if (txt_AmtReceived.Text == string.Empty || txt_TDS.Text == string.Empty)
                {
                    lblMsg.Text = " Please enter above Received Amount and TDS Amount ";
                    lblMsg.Visible = true;
                    txt_AmtReceived.Focus();
                    adjustmentfooter.Text = Convert.ToDecimal("0.00").ToString();
                }
                else
                {
                    decimal total = 0;
                    foreach (GridViewRow r in gvDetailsView.Rows)
                    {
                        var textT = r.FindControl("txtAdjustAmt") as TextBox;
                        decimal number;
                        if (decimal.TryParse(textT.Text, out number))
                        {
                            total += number;
                        }
                    }
                    total += Convert.ToDecimal(adjustmentfooter.Text);
                    txtTotalAmtGV.Text = total.ToString("0.00");
                    adjustmentfooter.Focus();
                    txt_acallocatedamt.Text = (Convert.ToDecimal(txt_totalAmt.Text) - Convert.ToDecimal(txtTotalAmtGV.Text)).ToString("0.00");
                }

            }
            else
            {
                lblMsg.Text = " Please enter only Numbers ";
                lblMsg.Visible = true;
                adjustmentfooter.Attributes.Add("onfocus", "this.select();");
                adjustmentfooter.Text = Convert.ToDecimal("0.00").ToString();
                adjustmentfooter.Focus();
            }
        }
        
        protected void txtTotalAmtGV_TextChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (Convert.ToDecimal(txtTotalAmtGV.Text) > Convert.ToDecimal(txt_totalAmt.Text))
            {
                lblMsg.Text = " It should be always less than Total Amount ";
                lblMsg.Visible = true;
            }
            else if (Convert.ToDecimal(txt_totalAmt.Text) != Convert.ToDecimal(txtTotalAmtGV.Text))
            {
                lblMsg.Text = " Both Total amount should be equal ";
                lblMsg.Visible = true;
            }
        }

        public void DisplayFooter()
        {
            if (Convert.ToInt32(txt_receivedNo.Text) > 0)
            {
                var result = dc.Cash_View(0, Convert.ToInt32(txt_receivedNo.Text),false);
                foreach (var footerow in result)
                {
                    TextBox Ftds = (TextBox)gvDetailsView.FooterRow.Cells[5].FindControl("txtTdsAmtFt");
                    TextBox Fadjst = (TextBox)gvDetailsView.FooterRow.Cells[6].FindControl("txtAdjustAmtFt");

                    Ftds.Text = Convert.ToDecimal(footerow.CashDetail_TDSAmount_money).ToString("0.00");
                    Fadjst.Text = footerow.CashDetail_Amount_money.ToString();
                    Fadjst.Text = Convert.ToDecimal(Math.Abs(Convert.ToDecimal(Fadjst.Text))).ToString("0.00");
                    txtTotalAmtGV.Text = Convert.ToDecimal(Math.Abs(Convert.ToDecimal(Fadjst.Text))).ToString("0.00");
                    txtTotalTDSAmtGV.Text = Convert.ToDecimal(Ftds.Text).ToString("0.00");
                }
            }
        }
        
        public void SumOfBalanceAmt()
        {
            for (int i = 0; i <= gvDetailsView.Rows.Count - 1; i++)
            {
                decimal a = 0, b = 0, c = 0, d = 0;
                
                TextBox vB1 = (TextBox)gvDetailsView.Rows[i].Cells[3].FindControl("txtBalAmt");
                TextBox va1 = (TextBox)gvDetailsView.Rows[i].Cells[6].FindControl("txtAdjustAmt");
                
                if (Convert.ToString(vB1.Text) == "")
                {
                    vB1.Text = "0.00";
                }
                a = Convert.ToDecimal(vB1.Text);
                b = Convert.ToDecimal(va1.Text);
                d += a;
                c += b + d;
                vB1.Text = Convert.ToDecimal(c).ToString();
            }

        }
        
        public void ByDefaultOnAc()
        {
            for (int i = 0; i <= gvDetailsView.Rows.Count - 1; i++)
            {
                TextBox abillno = (TextBox)gvDetailsView.Rows[i].Cells[0].FindControl("txtBillNo");
                TextBox adate = (TextBox)gvDetailsView.Rows[i].Cells[1].FindControl("txtBillDate");
                TextBox abilamt = (TextBox)gvDetailsView.Rows[i].Cells[2].FindControl("txtBillAmt");
                TextBox abalamt = (TextBox)gvDetailsView.Rows[i].Cells[3].FindControl("txtBalAmt");
                TextBox atds = (TextBox)gvDetailsView.Rows[i].Cells[4].FindControl("txtTdsAmt");
                DropDownList ddlSettlement = (DropDownList)gvDetailsView.Rows[i].Cells[5].FindControl("ddlSettlement");
                TextBox aadjstmt = (TextBox)gvDetailsView.Rows[i].Cells[6].FindControl("txtAdjustAmt");
                
                if (abillno.Text == "" && adate.Text == "" && abilamt.Text == "" && atds.Text == "")
                {
                    abilamt.Text = "0.00";
                    aadjstmt.Text = "0.00";
                    atds.Text = "0.00";
                    abillno.Visible = false;
                    adate.Visible = false;
                    abilamt.Visible = false;
                    abalamt.Visible = false;
                    atds.Visible = false;
                    ddlSettlement.Visible = false;
                    aadjstmt.Visible = false;
                    gvDetailsView.Rows[0].Height = 0;
                }
            }
        }
        
        protected void chkColleted_CheckedChanged(object sender, EventArgs e)
        {
            if (chkColleted.Checked)
            {
                lblColectfrm.Enabled = true;
                ddlUserCollected.Enabled = true;
                txt_userip.Enabled = true;
                txt_userip.Text = "";
            }
            else
            {
                lblColectfrm.Enabled = false;
                ddlUserCollected.Enabled = false;
                txt_userip.Enabled = false;
                txt_userip.Text = "";
            }
        }
        
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            rpt.CashReceipt_PDF(Convert.ToInt32(txt_receivedNo.Text));
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID].ToString();
                }
            }
            //LoadBillDetailsGridOfClient();
            LoadReceiptDetails();
            txt_AmtReceived.Focus();
        }

        public void LoadBillDetailsGridOfClient()
        {
            try
            {
                ViewState["CurrentTable"] = null;
                int rowIndex = 0;

                DataTable dtTable = new DataTable();
                DataRow drow = null;
                dtTable.Columns.Add(new DataColumn("BILL_Id", typeof(string)));
                dtTable.Columns.Add(new DataColumn("BILL_Date_dt", typeof(string)));
                dtTable.Columns.Add(new DataColumn("BILL_NetAmt_num", typeof(double)));
                dtTable.Columns.Add(new DataColumn("BalanceAmount", typeof(double)));
                dtTable.Columns.Add(new DataColumn("CashDetail_Settlement_var", typeof(string)));
                dtTable.Columns.Add(new DataColumn("CashDetail_TDSAmount_money", typeof(string)));
                dtTable.Columns.Add(new DataColumn("CashDetail_Amount_money", typeof(string)));
                if (Convert.ToInt32(lblClientId.Text) > 0)
                {
                    var result = dc.CashDetail_View(0, Convert.ToInt32(lblClientId.Text), false, false, 0);
                    foreach (var recipt in result)
                    {
                        drow = dtTable.NewRow();
                        drow["BILL_Id"] = recipt.BILL_Id.ToString();
                        drow["BILL_Date_dt"] = Convert.ToDateTime(recipt.BILL_Date_dt).ToString("dd/MM/yyyy");
                        drow["BILL_NetAmt_num"] = Convert.ToDecimal(recipt.BILL_NetAmt_num);
                        if (recipt.CashDetail_Amount_money != null)
                        {
                            drow["BalanceAmount"] = Convert.ToDecimal(recipt.CashDetail_Amount_money);
                        }
                        else
                        {
                            drow["BalanceAmount"] = Convert.ToDecimal(recipt.BILL_NetAmt_num);
                        }
                        drow["CashDetail_Settlement_var"] = 0;
                        drow["CashDetail_TDSAmount_money"] = "0.00";
                        drow["CashDetail_Amount_money"] = Convert.ToDecimal("0.00");
                        dtTable.Rows.Add(drow);
                        dtTable.AcceptChanges();
                        gvDetailsView.DataSource = dtTable;
                        gvDetailsView.DataBind();
                        gvDetailsView.Visible = true;
                        ViewState["CurrentTable"] = dtTable;
                        rowIndex++;
                    }

                    var res = dc.CashDetail_View(0, Convert.ToInt32(lblClientId.Text), false, true, 0);
                    foreach (var display in res)
                    {
                        drow = dtTable.NewRow();
                        drow["BILL_Id"] = display.BILL_Id.ToString();
                        drow["BILL_Date_dt"] = Convert.ToDateTime(display.BILL_Date_dt).ToString("dd/MM/yyyy");//2
                        drow["BILL_NetAmt_num"] = Convert.ToDecimal(display.BILL_NetAmt_num).ToString("0.00");
                        drow["BalanceAmount"] = Convert.ToDecimal(display.BILL_NetAmt_num).ToString("0.00");
                        drow["CashDetail_Settlement_var"] = 0;
                        drow["CashDetail_TDSAmount_money"] = "0.00";
                        drow["CashDetail_Amount_money"] = 0;
                        dtTable.Rows.Add(drow);
                        dtTable.AcceptChanges();
                        ViewState["CurrentTable"] = dtTable;
                        rowIndex++;
                    }
                }
                gvDetailsView.DataSource = dtTable;
                gvDetailsView.DataBind();

                if (ViewState["CurrentTable"] == null)
                {
                    if (drow == null)
                    {
                        drow = dtTable.NewRow();
                        drow["CashDetail_Settlement_var"] = "0";
                        dtTable.Rows.Add(drow);
                        dtTable.AcceptChanges();
                        rowIndex++;
                        gvDetailsView.DataSource = dtTable;
                        gvDetailsView.DataBind();
                        ByDefaultOnAc();
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, -1, searchHead, "");
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

        protected void LnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("CashReceipt_Modify.aspx");
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            Response.Redirect("CashReceipt_Entry.aspx");
        }
        
    }
}