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
using System.Collections.Generic;
using System.Web.Services;

namespace DESPLWEB
{
    public partial class AdvanceReciptEntry : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        //public static int Cl_Id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblClientId.Text = "0";
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Advance Receipt Entry";
                    DisplayDate();
                    BindColletedUser();
                    BindLedgerName();
                    BindLedger();
                    BindBankList();
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
                        txt_receivedNo.Text = arrIndMsg[1].ToString().Trim();
                    }
                    ////
                    //if (Session["AdvReceiptNo"] != null)
                    if (txt_receivedNo.Text != "" && txt_receivedNo.Text != "Create New...")
                    {
                        lblheading.Text = "Advance Receipt Modify";
                        DisplayAdvDetail();
                    }
                    else
                    {
                        LoadClientLedgerMappingList();
                        LoadClientLedgerMappingList1();
                    }
                }
            }
        }
        public void BindBankList()
        {
            //var bank = dc.Bank_View();
            //CmbBankname.DataTextField = "Cash_BankName_var";
            //CmbBankname.DataSource = bank;
            //CmbBankname.DataBind();
            //CmbBankname.Items.Insert(0, "---Select---");

            var BL = dc.Bank_View("");
            ddl_Bank_Name.DataSource = BL;
            ddl_Bank_Name.DataTextField = "BANK_Name_var";
            ddl_Bank_Name.DataValueField = "BANK_Name_var";
            ddl_Bank_Name.DataBind();
            ddl_Bank_Name.Items.Insert(0, "---Select---");
        }
        public void DisplayDate()
        {
            txt_date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_chquedate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        private void BindColletedUser()
        {
            var CollectUser = dc.User_View(0, -1, "", "Collection", "");
            ddlUserCollected.DataSource = CollectUser;
            ddlUserCollected.DataTextField = "USER_Name_var";
            ddlUserCollected.DataValueField = "USER_Id";
            ddlUserCollected.DataBind();
            ddlUserCollected.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void BindLedgerName()
        {
            //var Ledg = dc.Ledger_View(0, 0, true);
            //ddlLedgerName.DataSource = Ledg;
            //ddlLedgerName.DataTextField = "LedgerName_Description";
            //ddlLedgerName.DataValueField = "LedgerName_Id";
            //ddlLedgerName.DataBind();
            //ddlLedgerName.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void BindLedger()
        {
            //var Ledg = dc.Ledger_View(0, 0, false);
            //ddlLedger.DataSource = Ledg;
            //ddlLedger.DataTextField = "LedgerName_Description";
            //ddlLedger.DataValueField = "LedgerName_Id";
            //ddlLedger.DataBind();
            ddlLedger.Items.Insert(0, "On A/c");
        }

        private void DisplayAdvDetail()
        {
            var data = dc.Advance_View(null, null, Convert.ToInt32(txt_receivedNo.Text), "", true);
            foreach (var adv in data)
            {
                txt_receivedNo.Text = Convert.ToString(adv.ReceiptNo);
                txt_date.Text = Convert.ToDateTime(adv.ReceiptDate).ToString("dd/MM/yyyy");
                if (adv.ReceiptDate <= DateTime.ParseExact("31/03/2016", "dd/MM/yyyy", null))
                {
                    txt_date.Enabled = false;
                }
                if (adv.ADV_Status == true)
                {
                    ddl_status.Items.FindByText("Hold").Selected = true;
                }
                //ddlLedgerName.SelectedValue = adv.LedgerId.ToString();
                txt_Client.Text = adv.LedgerName_Description;
                lblClientId.Text = adv.LedgerId.ToString() ;
                LoadClientLedgerMappingList();
                txt_Client1.Text = adv.clientName;
                lblClientId1.Text = adv.ClientId.ToString();
                LoadClientLedgerMappingList1();
                //for (int i = 0; i < ddlClient.Items.Count; i++)
                //{
                //    if (ddlClient.Items[i].Value == adv.ClientId.ToString())
                //    {
                //        ddlClient.SelectedValue = adv.ClientId.ToString();
                //        break;
                //    }
                //}
                
                if (adv.PaymentMode == true)
                {
                    rdn_cheque.Checked = true;
                    string[] CheqDetail = new string[3];
                    CheqDetail = Convert.ToString(adv.ChqDetail).Split('|');
                    txt_cheque_no.Text = Convert.ToString(CheqDetail[0]);
                    //CmbBankname.Items.Insert(0, Convert.ToString(CheqDetail[1]));
                    //CmbBankname.SelectedValue = Convert.ToString(CheqDetail[1]);
                    ddl_Bank_Name.SelectedValue = Convert.ToString(CheqDetail[1]);
                    //txt_chquedate.Text =Convert.ToDateTime(Convert.ToString(CheqDetail[2])).ToString("dd/MM/yyyy");
                    //DateTime.ParseExact(CheqDetail[2], "dd/MM/yyyy", null)
                    txt_chquedate.Text =CheqDetail[2].ToString();
                    
                    txt_Branch.Text = Convert.ToString(CheqDetail[3]);
                    txt_cheque_no.Enabled = true;
                    rdn_cash.Checked = false;
                    txt_chquedate.Enabled = true;
                    txt_Branch.Enabled = true;
                    ddl_Bank_Name.Enabled = true;
                }
                else
                {
                    rdn_cash.Checked = true;
                }
                txt_AmtReceived.Text = Convert.ToDecimal(adv.ActualAmt).ToString("0.00");
                txt_TDS.Text = Convert.ToDecimal(adv.xTds).ToString("0.00");
                txt_totalAmt.Text = Convert.ToDecimal(adv.ReceiptAmount).ToString("0.00");
                ddlLedger.SelectedItem.Text = Convert.ToString(adv.Settlement);
                txt_Note.Text = Convert.ToString(adv.Note);
                if (Convert.ToString(adv.CollectionDetail) != "")
                {
                    string[] CollectionDetail = new string[1];
                    CollectionDetail = Convert.ToString(adv.CollectionDetail).Split('|');
                    ddlUserCollected.SelectedValue = Convert.ToString(CollectionDetail[0]);
                    txt_userip.Text = Convert.ToString(CollectionDetail[1]);
                }
                else
                {
                    chkColleted.Checked = false;
                    ddlUserCollected.Enabled = false;
                    txt_userip.Enabled = false;
                }
                break;
            }
        }

        protected void rdn_cheque_CheckedChanged(object sender, EventArgs e)
        {
            clearChequeBox();
            ddl_Bank_Name.Enabled = true;
            txt_cheque_no.Enabled = true;
            txt_chquedate.Enabled = true;
            txt_Branch.Enabled = true;
            txt_date.Enabled = true;
        }
        public void clearChequeBox()
        {
            txt_cheque_no.Text = string.Empty;
            txt_chquedate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_Branch.Text = string.Empty;
        }
        protected void rdn_cash_CheckedChanged(object sender, EventArgs e)
        {
            if (rdn_cash.Checked)
            {
                clearChequeBox();
                ddl_Bank_Name.Enabled = false;
                txt_cheque_no.Enabled = false;
                txt_chquedate.Enabled = false;
                txt_Branch.Enabled = false;
                txt_date.Enabled = true;
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
                    CalTotal();
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
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_TDS.Text, "^[0-9]+(.[0-9]{1,2})?$"))
            {
                CalTotal();
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
        private void CalTotal()
        {
            if (txt_AmtReceived.Text != "" && txt_TDS.Text != "")
            {
                txt_totalAmt.Text = (Convert.ToDecimal(txt_AmtReceived.Text) + Convert.ToDecimal(txt_TDS.Text)).ToString();
            }
        }
        protected void LnkBtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                if (lblClientId.Text == "" || lblClientId.Text == "0")
                {
                    var ledger = dc.Ledger_View(0, 0, true, txt_Client.Text);
                    foreach (var ledg in ledger)
                    {
                        lblClientId.Text = ledg.LedgerName_Id.ToString();
                    }
                    if (lblClientId.Text == "" || lblClientId.Text == "0")
                    {
                        lblClientId.Text = dc.Ledger_Update(0, 0, 0, true, txt_Client.Text).ToString();
                    }
                }


                if (lblClientId1.Text == "" || lblClientId1.Text == "0")
                {
                    var ledger = dc.Client_View(0, 0, txt_Client1.Text, "");
                    foreach (var ledg in ledger)
                    {
                        lblClientId1.Text = ledg.CL_Id.ToString();
                    }
                    if (lblClientId1.Text == "" || lblClientId1.Text == "0")
                    {
                        lblClientId1.Text = dc.Client_View(0, 0, txt_Client1.Text, "").ToString();
                    }
                }
                DateTime recvddt = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                DateTime ChqDt = DateTime.ParseExact(txt_chquedate.Text, "dd/MM/yyyy", null);
                bool Paymentmode = false;

                if (rdn_cash.Checked)
                {
                    Paymentmode = false;
                }
                else if (rdn_cheque.Checked)
                {
                    Paymentmode = true;
                }
                string CollectionDetail = "";
                if (chkColleted.Checked)
                {
                    CollectionDetail = ddlUserCollected.SelectedValue + "|" + txt_userip.Text;
                }
                string CheqDetail = "";
                if (rdn_cheque.Checked)
                {
                    //CheqDetail = txt_cheque_no.Text + "|" + ddl_Bank_Name.SelectedValue + "|" + ChqDt + "|" + txt_Branch.Text;
                    CheqDetail = txt_cheque_no.Text + "|" + ddl_Bank_Name.SelectedValue + "|" + txt_chquedate.Text + "|" + txt_Branch.Text;
                }
                if (txt_receivedNo.Text == "Create New...")
                {
                    //var recp = dc.Advance_View(null, null, 0, true);
                    int Recpt = dc.getAdvanceMaxNo(true ,null);
                    txt_receivedNo.Text = Recpt.ToString();
                    //foreach (var rcp in recp)
                    //{
                        //if (r.ReceiptNo == null)
                        //{
                        //    Recpt = Recpt + 1;
                        //}
                        //else
                        //{
                        //    Recpt = Convert.ToInt32(r.ReceiptNo) + 1;
                        //}
                        //txt_receivedNo.Text = Recpt.ToString();                        
                    //}
                }

                dc.Advance_Update(Convert.ToInt32(txt_receivedNo.Text), "", null, 0, 0, 0, "", "", 0, 0, false, false, false, "", false, false, 0, null, false, 0, true, false);
                //dc.Advance_Update(Convert.ToInt32(txt_receivedNo.Text), "", recvddt, Convert.ToDecimal(txt_totalAmt.Text), Convert.ToDecimal(txt_TDS.Text), 0, CheqDetail, txt_Note.Text, Convert.ToInt32(Session["LoginID"]), Convert.ToInt32(ddlLedgerName.SelectedValue), Convert.ToBoolean(ddl_status.SelectedValue), false, Paymentmode, CollectionDetail, false, false, Convert.ToDecimal(txt_AmtReceived.Text), recvddt, true, 0, false, false);
                dc.Advance_Update(Convert.ToInt32(txt_receivedNo.Text), "", recvddt, Convert.ToDecimal(txt_totalAmt.Text), Convert.ToDecimal(txt_TDS.Text), 0, CheqDetail, txt_Note.Text, Convert.ToInt32(Session["LoginID"]), Convert.ToInt32(lblClientId.Text), Convert.ToBoolean(ddl_status.SelectedValue), false, Paymentmode, CollectionDetail, false, false, Convert.ToDecimal(txt_AmtReceived.Text), recvddt, true, Convert.ToInt32(lblClientId1.Text) , false, false);
                dc.AdvanceDetail_Update(Convert.ToInt32(txt_receivedNo.Text), "0", null,0, "", 0, "", 0, false, true);
                //dc.AdvanceDetail_Update(Convert.ToInt32(txt_receivedNo.Text), 0, recvddt, (-1) * Convert.ToDecimal(txt_totalAmt.Text), ddlLedger.SelectedItem.Text, Convert.ToDecimal(txt_TDS.Text), "", Convert.ToInt32(ddlLedgerName.SelectedValue), Convert.ToBoolean(ddl_status.SelectedValue), false);
                dc.AdvanceDetail_Update(Convert.ToInt32(txt_receivedNo.Text), "0", recvddt, (-1) * Convert.ToDecimal(txt_totalAmt.Text), ddlLedger.SelectedItem.Text, Convert.ToDecimal(txt_TDS.Text), "", Convert.ToInt32(lblClientId.Text), Convert.ToBoolean(ddl_status.SelectedValue), false);
                dc.Client_Update_BalanceAmt(Convert.ToInt32(lblClientId.Text));

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Records Saved Sucessfully";
                LnkBtnSave.Enabled = false;
                lnkPrint.Visible = true;
                lnkNext.Visible = true;
                //Session["AdvReceiptNo"] = null;
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            //if (ddlLedgerName.SelectedItem.Text == "---Select---")
            //if (txt_Client.Text == "" || Convert.ToInt32(lblClientId.Text) == 0)
            if (txt_Client.Text == "")
            {
                lblMsg.Text = "Select Ledger Name";
                txt_Client.Focus();
                valid = false;
            }
            else if (txt_Client1.Text == "")
            {
                lblMsg.Text = "Select Client Name";
                txt_Client1.Focus();
                valid = false;
            }
            else if (txt_date.Text == "")
            {
                lblMsg.Text = "Enter Date ";
                txt_date.Focus();
                valid = false;
            }
            else if (rdn_cheque.Checked == false && rdn_cash.Checked ==false)
            {
                lblMsg.Text = "Select anyone of these radio button";
                valid = false;
            }
            else if (rdn_cheque.Checked == true && txt_cheque_no.Text == "")
            {
                lblMsg.Text = "Enter Check No";
                txt_cheque_no.Focus();
                valid = false;
            }
            else if (rdn_cheque.Checked == true && txt_chquedate.Text == "")
            {
                lblMsg.Text = "Enter Check Date";
                txt_chquedate.Focus();
                valid = false;
            }
            else if (rdn_cheque.Checked == true && txt_Branch.Text == "")
            {
                lblMsg.Text = "Enter Branch ";
                txt_Branch.Focus();
                valid = false;
            }
            else if (rdn_cheque.Checked == true && ddl_Bank_Name.SelectedValue == "---Select---" )
            {
                lblMsg.Text = "Select Bank Name ";
                ddl_Bank_Name.Focus();
                valid = false;
            }
            else if (txt_AmtReceived.Text == "")
            {
                lblMsg.Text = " Enter Receipt Amount ";
                txt_AmtReceived.Focus();
                valid = false;
            }
            else if (txt_TDS.Text == "")
            {
                lblMsg.Text = " Enter TDS Amount ";
                txt_TDS.Focus();
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
            else if (txt_Note.Text == "")
            {
                lblMsg.Text = " Enter Note ";
                txt_Note.Focus();
                valid = false;
            }
            else if (txt_receivedNo.Text != "Create New...")
            {
                DateTime recvddt = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                if (recvddt > DateTime.ParseExact("31/03/2016", "dd/MM/yyyy", null))
                {
                    var jrnl = dc.AdvanceDetail_View_CheckJrnlEntry(Convert.ToInt32(txt_receivedNo.Text));
                    if (jrnl.Count() > 0)
                    {
                        lblMsg.Text = " Can not modify adjusted advance.";
                        valid = false;
                    }
                }
            }
            //if (valid == true && ddlClient.SelectedValue != "0")
            //{
            //    clsData clsDt = new clsData();
            //    if (clsDt.checkGSTInfoUpdated(ddlClient.SelectedValue, "", "") == false)
            //    {
            //        valid = false;
            //        lblMsg.Text = "Please update client GST information. Can not add/modify receipt.";
            //    }
            //}
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

        protected void LnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Advance_Modify.aspx");
        }
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdvanceReciptEntry.aspx");
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            //PrintPDFReport rpt = new PrintPDFReport();
            //rpt.AdvanceReceipt_PDF(txt_receivedNo.Text);

            DateTime BillDate = DateTime.Now;
            var AdvReceipt = dc.Advance_View(null, null, Convert.ToInt32(txt_receivedNo.Text), "", true);
            foreach (var adv in AdvReceipt)
            {
                BillDate = Convert.ToDateTime(adv.ReceiptDate);
                break;
            }

            PrintPDFReport rpt = new PrintPDFReport();
            if (CheckGSTFlag(BillDate) == false)
                rpt.AdvanceReceipt_PDF(txt_receivedNo.Text);//service tax old format
            else
                rpt.AdvanceReceipt_PDF_GST(txt_receivedNo.Text);//GST new format
        }
        public bool CheckGSTFlag(DateTime BillDate)
        {
            bool gstFlag = false;
            var master = dc.GST_View(1, BillDate);
            if (master.Count() > 0)
            {
                gstFlag = true;
            }
            else
            {
                gstFlag = false;
            }
            return gstFlag;
        }
  
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
          //  ddlClient.Items.Clear();
            LoadClientLedgerMappingList();
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                   lblClientId.Text = Request.Form[hfClientId.UniqueID].ToString();
                   LoadClientLedgerMappingList();
                }
            }
        }

        protected void txt_Client_TextChanged1(object sender, EventArgs e)
        {
            lblClientId1.Text = "0";
            //  ddlClient.Items.Clear();
            LoadClientLedgerMappingList();
            if (ChkClientName1(txt_Client1.Text) == true)
            {
                if (txt_Client1.Text != "")
                {
                    lblClientId1.Text = Request.Form[hfClientId1.UniqueID].ToString();
                    LoadClientLedgerMappingList();
                }
            }
        }
        public void LoadClientLedgerMappingList()
        {
            if (lblClientId.Text != "" && lblClientId.Text != "0")
            {
                var mapping = dc.ClientLedger_View(0, Convert.ToInt32(lblClientId.Text), 0, false);

                //ddlClient.DataSource = mapping;
                //ddlClient.DataTextField = "CL_Name_var";
                //ddlClient.DataValueField = "CLLedger_CL_Id";
                //ddlClient.DataBind();
            }
            
        }


        public void LoadClientLedgerMappingList1()
        {
            if (lblClientId1.Text != "" && lblClientId1.Text != "0")
            {
                var mapping = dc.ClientLedger_View(0, Convert.ToInt32(lblClientId1.Text), 0, false);

                //ddlClient.DataSource = mapping;
                //ddlClient.DataTextField = "CL_Name_var";
                //ddlClient.DataValueField = "CLLedger_CL_Id";
                //ddlClient.DataBind();
            }

        }

        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            //var query = dc.Client_View(0, 0, searchHead);
            var query = dc.Ledger_View(0, 0, true, searchHead);
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This ledger name is not in the list";
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
        protected Boolean ChkClientName1(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client1.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead,"");
           // var query = dc.Ledger_View(0, 0, true, searchHead);
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client1.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This ledger name is not in the list";
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
            //var query = db.Client_View(0, 0, searchHead);
            var query = db.Ledger_View(0, 0, true, searchHead);

            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("LedgerName_Description");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.LedgerName_Description, rowObj.LedgerName_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                //var clnm = db.Client_View(0, 0, "");
                var clnm = db.Ledger_View(0, 0, true, "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.LedgerName_Description, rowObj.LedgerName_Id.ToString());
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]

        public static List<string> GetClientname1(string prefixText)//new
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, 0, searchHead,"");
         //  var query = db.Ledger_View(0, 0, true, searchHead);
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("cL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }

            if (row == null)
            {
                //var clnm = db.Client_View(0, 0, "");
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