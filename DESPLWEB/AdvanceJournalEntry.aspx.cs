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
    public partial class AdvanceJournalEntry : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        //public static int Cl_Id = 0;
        //public static int Ledger_Id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                lblClientId.Text = "0";
                lblLedgerId.Text = "0";
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Adjust Advance Note Entry";
                    txt_date.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    BindLedgerName();
                    BindClient();
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
                        txt_NoteNo.Text = arrIndMsg[1].ToString().Trim();                        
                    }
                    ////
                    //if (Session["AdvNoteNo"] != null)
                    if (txt_NoteNo.Text != "" && txt_NoteNo.Text != "Create New...")
                    {
                        lblheading.Text = "Modify Adjusted Advance Note";
                        DisplayAdvAdjNoteDetail();
                    }
                }
            }
        }
        private void DisplayAdvAdjNoteDetail()
        {
            var data = dc.Advance_View(null, null,0, txt_NoteNo.Text, false);
            foreach (var adv in data)
            {
                txt_NoteNo.Text = Convert.ToString(adv.NoteNo);
                txt_date.Text = Convert.ToDateTime(adv.ReceiptDate).ToString("dd/MM/yyyy");
                if (adv.Status == true)
                {
                    ddl_status.Items.FindByText("Hold").Selected = true;
                }
                //ddlLedgerName.SelectedValue = adv.LedgerId.ToString();
                //ddlLedgerName.Enabled = false;
                txt_Ledger.Text = adv.LedgerName_Description;
                lblLedgerId.Text  = adv.LedgerId.ToString();

                txt_Note.Text = Convert.ToString(adv.Note);
                //ddlClient.SelectedValue = adv.ClientId.ToString();
                //ddlClient.Enabled = false;
                txt_Client.Text = adv.clientName;
                lblClientId.Text  = adv.ClientId.ToString();

                SelectGridviewfromClient();
                BindTotalAdjstamt();
                LoadReceipts();
                break;
            }
        }
        private void BindClient()
        {
            //var client = dc.Client_View(0, 0, "");
            //ddlClient.DataSource = client;
            //ddlClient.DataTextField = "CL_Name_var";
            //ddlClient.DataValueField = "CL_Id";
            //ddlClient.DataBind();
            //ddlClient.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void BindLedgerName()
        {
            //var Ledg = dc.Ledger_View(0, 0, true,"");
            //ddlLedgerName.DataSource = Ledg;
            //ddlLedgerName.DataTextField = "LedgerName_Description";
            //ddlLedgerName.DataValueField = "LedgerName_Id";
            //ddlLedgerName.DataBind();
            //ddlLedgerName.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        public void LoadReceipts()
        {  
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drow = null;
            dtTable.Columns.Add(new DataColumn("ReceiptNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ReceiptDate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
                        
            //var result = dc.AdvanceDetail_View(Convert.ToInt32(ddlLedgerName.SelectedValue), false, true,txt_NoteNo.Text );
            var result = dc.AdvanceDetail_View(Convert.ToInt32(lblLedgerId.Text), false, true, txt_NoteNo.Text);
            foreach (var recipt in result)            
            {
                drow = dtTable.NewRow();
                drow["ReceiptNo"] = recipt.ReceiptNo.ToString();
                drow["ReceiptDate"] = Convert.ToDateTime(recipt.ReceiptDate).ToString("dd/MM/yyyy");
                drow["Amount"] = recipt.ReceiptAmount;
                drow["BalanceAmount"] = recipt.Amount;

                dtTable.Rows.Add(drow);
                dtTable.AcceptChanges();
                rowIndex++;
            }
            grdAdjstdtl.DataSource = dtTable;
            grdAdjstdtl.DataBind();
            rowIndex = 0;
        }

        public void SelectGridviewfromClient()
        {
            //try
            //{
                txt_Amtallocated.Text = "0";
                //var ad = dc.AdvanceDetail_View(Convert.ToInt32(ddlLedgerName.SelectedValue), false, false,"");
                var ad = dc.AdvanceDetail_View(Convert.ToInt32(lblLedgerId.Text), false, false, "");
                foreach (var a in ad)
                {
                    if (a.Amount != null)
                    {
                        txt_Amtallocated.Text = Convert.ToDecimal(a.Amount * -1).ToString("0.00");
                    }
                }
                if (txt_NoteNo.Text != "Create New...")
                {
                    var data = dc.Advance_View(null, null, 0, txt_NoteNo.Text, false);
                    foreach (var adv in data)
                    {
                        txt_Amtallocated.Text = (Convert.ToDecimal(txt_Amtallocated.Text) + Convert.ToDecimal(adv.ReceiptAmount)).ToString();
                        break;
                    }
                }
                DataTable dtTable = new DataTable();
                DataRow drow = null;
                int rowIndex = 0;
                //if (Convert.ToInt32(ddlClient.SelectedValue) > 0)
                if (Convert.ToInt32(lblClientId.Text) > 0)
                {
                    string billList = "", strRecptDetail="", prevSaveData = "";
                    gvAdvanceJournal.Visible = true;                    
                    dtTable.Columns.Add(new DataColumn("BILL_Id", typeof(string)));
                    dtTable.Columns.Add(new DataColumn("BILL_Date_dt", typeof(string)));
                    dtTable.Columns.Add(new DataColumn("BILL_NetAmt_num", typeof(string)));
                    dtTable.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
                    dtTable.Columns.Add(new DataColumn("lblrecpdtls", typeof(string)));
                    
                    drow = dtTable.NewRow();
                    //On A/c Entry
                    drow["BILL_Id"] = "On A/c";
                    drow["BILL_Date_dt"] = "---";
                    drow["BILL_NetAmt_num"] = "---";
                    drow["BalanceAmount"] = "---";
                    drow["lblrecpdtls"] = "";
                    if (txt_NoteNo.Text != "Create New...")
                    {
                        strRecptDetail = "";
                        var OnAcc = dc.CashDetail_View_BillAdvNote(txt_NoteNo.Text, "0", false, true);
                        foreach (var OnA in OnAcc)
                        {
                            //strRecptDetail = "";
                            double tmpAmt = 0;
                            tmpAmt = Convert.ToDouble(OnA.BILL_NetAmt_num * (-1));
                            strRecptDetail = strRecptDetail + OnA.CashDetail_ReceiptNo.ToString() + "=" + tmpAmt.ToString() + "|";
                            
                            //drow["lblrecpdtls"] = strRecptDetail;
                            //prevSaveData += "On A/c" + "#" + strRecptDetail + "~";
                        }
                        drow["lblrecpdtls"] = strRecptDetail;
                        prevSaveData += "On A/c" + "#" + strRecptDetail + "~";
                    }
                    dtTable.Rows.Add(drow);
                    dtTable.AcceptChanges();
                    rowIndex++;
                    #region bills
                    //Existing updated bills
                    if (txt_NoteNo.Text != "Create New...")
                    {
                        var bill = dc.CashDetail_View_BillAdvNote(txt_NoteNo.Text, "0", false,false);
                        foreach (var bl in bill)
                        {
                            if (bl.BalanceAmount == null || (bl.BalanceAmount + (bl.billNoteAmt * -1)) > 0)
                            {
                                drow = dtTable.NewRow();
                                drow["BILL_Id"] = bl.BILL_Id.ToString();
                                drow["BILL_Date_dt"] = Convert.ToDateTime(bl.BILL_Date_dt).ToString("dd/MM/yyyy");
                                drow["BILL_NetAmt_num"] = Convert.ToDecimal(bl.BILL_NetAmt_num);
                                if (bl.BalanceAmount != null)
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(bl.BalanceAmount + (bl.billNoteAmt * -1)).ToString("0.00");
                                }
                                else
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(bl.BILL_NetAmt_num);
                                }
                                strRecptDetail = "";
                                double tmpAmt = 0;
                                var rcpt = dc.CashDetail_View_BillAdvNote(txt_NoteNo.Text, bl.BILL_Id, true, false);
                                foreach (var rc in rcpt)
                                {
                                    tmpAmt = Convert.ToDouble(rc.billNoteAmt * (-1));
                                    strRecptDetail = strRecptDetail + rc.CashDetail_ReceiptNo.ToString() + "=" + tmpAmt.ToString() + "|";
                                }
                                drow["lblrecpdtls"] = strRecptDetail;
                                prevSaveData += bl.BILL_Id.ToString() + "#" + strRecptDetail + "~";
                                dtTable.Rows.Add(drow);
                                dtTable.AcceptChanges();
                                rowIndex++;
                                billList += "|" + bl.BILL_Id.ToString() + "|";
                            }
                        }
                    }
                    //New outstanding bills partly adjusted
                    //var res = dc.CashDetail_View(0, Convert.ToInt32(ddlClient.SelectedValue), false, true, 0);
                    //var res = dc.CashDetail_View_OutstandingBills(Convert.ToInt32(ddlClient.SelectedValue), 0, DateTime.Now, null, true);
                    var res = dc.CashDetail_View_OutstandingBills(Convert.ToInt32(lblClientId.Text), 0, DateTime.Now, null, true);
                    foreach (var d in res)
                    {
                        if (billList.Contains("|" + d.BILL_Id.ToString() + "|") == false)
                        {
                            if (d.BalanceAmount == null || (d.BILL_NetAmt_num + d.BalanceAmount) > 0)
                            {
                                drow = dtTable.NewRow();
                                drow["BILL_Id"] = d.BILL_Id.ToString();
                                drow["BILL_Date_dt"] = Convert.ToDateTime(d.BILL_Date_dt).ToString("dd/MM/yyyy");
                                drow["BILL_NetAmt_num"] = Convert.ToDecimal(d.BILL_NetAmt_num);
                                if (d.BalanceAmount != null)
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(d.BILL_NetAmt_num + d.BalanceAmount).ToString("0.00");
                                }
                                else
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(d.BILL_NetAmt_num);
                                }
                                dtTable.Rows.Add(drow);
                                dtTable.AcceptChanges();
                                rowIndex++;
                            }
                        }
                    }
                    //New outstanding bills not adjusted
                    //var res1 = dc.CashDetail_View(0, Convert.ToInt32(ddlClient.SelectedValue), false, true, 0);
                    var res1 = dc.CashDetail_View(0, Convert.ToInt32(lblClientId.Text), false, true, 0);
                    foreach (var d1 in res1)
                    {
                        if (billList.Contains("|" + d1.BILL_Id.ToString() + "|") == false)
                        {
                            if (d1.CashDetail_Amount_money == null || d1.CashDetail_Amount_money > 0)
                            {
                                drow = dtTable.NewRow();
                                drow["BILL_Id"] = d1.BILL_Id.ToString();
                                drow["BILL_Date_dt"] = Convert.ToDateTime(d1.BILL_Date_dt).ToString("dd/MM/yyyy");
                                drow["BILL_NetAmt_num"] = Convert.ToDecimal(d1.BILL_NetAmt_num);
                                if (d1.CashDetail_Amount_money != null)
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(d1.CashDetail_Amount_money).ToString("0.00");
                                }
                                else
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(d1.BILL_NetAmt_num);
                                }
                                dtTable.Rows.Add(drow);
                                dtTable.AcceptChanges();
                                rowIndex++;
                            }
                        }
                    }
                    #endregion

                    #region debit notes
                    //Existing updated debit notes
                    if (txt_NoteNo.Text != "Create New...")
                    {
                        var dbNotes = dc.CashDetail_View_DBNoteAdvNote(txt_NoteNo.Text, "", false);
                        foreach (var dbNote in dbNotes)
                        {
                            if (dbNote.BalanceAmount == null || (dbNote.BalanceAmount + (dbNote.billNoteAmt * -1)) > 0)
                            {
                                drow = dtTable.NewRow();
                                drow["BILL_Id"] = dbNote.Journal_NoteNo_var;
                                drow["BILL_Date_dt"] = Convert.ToDateTime(dbNote.Journal_Date_dt).ToString("dd/MM/yyyy");
                                drow["BILL_NetAmt_num"] = Convert.ToDecimal(dbNote.Journal_Amount_dec);
                                if (dbNote.BalanceAmount != 0 && dbNote.BalanceAmount != null)
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(dbNote.BalanceAmount + (dbNote.billNoteAmt * -1)).ToString("0.00");
                                }
                                else
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(dbNote.Journal_Amount_dec);
                                }
                                strRecptDetail = "";
                                double tmpAmt = 0;

                                var rcpt = dc.CashDetail_View_DBNoteAdvNote(txt_NoteNo.Text, dbNote.Journal_NoteNo_var, true);
                                foreach (var rc in rcpt)
                                {
                                    tmpAmt = Convert.ToDouble(rc.billNoteAmt * (-1));
                                    strRecptDetail = strRecptDetail + rc.CashDetail_ReceiptNo.ToString() + "=" + tmpAmt.ToString() + "|";
                                }
                                drow["lblrecpdtls"] = strRecptDetail;
                                prevSaveData += dbNote.Journal_NoteNo_var + "#" + strRecptDetail + "~";
                                dtTable.Rows.Add(drow);
                                dtTable.AcceptChanges();
                                rowIndex++;
                                billList += "|" + dbNote.Journal_NoteNo_var + "|";
                            }
                        }
                    }
                    //New outstanding debit notes                   
                    //var dbNotes1 = dc.Journal_View_Outstanding(Convert.ToInt32(ddlClient.SelectedValue));
                    var dbNotes1 = dc.Journal_View_Outstanding(Convert.ToInt32(lblClientId.Text));
                    foreach (var dbNote1 in dbNotes1)
                    {
                        if (billList.Contains("|" + dbNote1.Journal_NoteNo_var + "|") == false)
                        {
                            if (dbNote1.BalanceAmt == null || (dbNote1.Journal_Amount_dec + dbNote1.BalanceAmt) > 0)
                            {
                                drow = dtTable.NewRow();
                                drow["BILL_Id"] = dbNote1.Journal_NoteNo_var;
                                drow["BILL_Date_dt"] = Convert.ToDateTime(dbNote1.Journal_Date_dt).ToString("dd/MM/yyyy");
                                drow["BILL_NetAmt_num"] = Convert.ToDecimal(dbNote1.Journal_Amount_dec);
                                if (dbNote1.BalanceAmt != null)
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(dbNote1.Journal_Amount_dec + dbNote1.BalanceAmt).ToString("0.00");
                                }
                                else
                                {
                                    drow["BalanceAmount"] = Convert.ToDecimal(dbNote1.Journal_Amount_dec);
                                }
                                dtTable.Rows.Add(drow);
                                dtTable.AcceptChanges();
                                rowIndex++;
                            }
                        }
                    }                    
                    #endregion

                    gvAdvanceJournal.DataSource = dtTable;
                    gvAdvanceJournal.DataBind();
                    if (txt_NoteNo.Text != "Create New...")
                    {
                        //prevSaveData += bl.BILL_Id.ToString() + "#" + strRecptDetail + "~";
                        string[] strPrev = prevSaveData.Split('~');
                        for (int pr = 0; pr < strPrev.Count() - 1; pr++)
                        {
                            string[] strPrev1 = strPrev[pr].Split('#');
                            for (int i = 0; i < gvAdvanceJournal.Rows.Count; i++)
                            {                                
                                Label lblBillNo = (Label)gvAdvanceJournal.Rows[i].FindControl("lblBillNo");
                                if (lblBillNo.Text == strPrev1[0])
                                {
                                    Label lblrecpdtls = (Label)gvAdvanceJournal.Rows[i].FindControl("lblrecpdtls");
                                    Button txt_Adjustamount = (Button)gvAdvanceJournal.Rows[i].FindControl("txt_Adjustamount");
                                    lblrecpdtls.Text = strPrev1[1];
                                    double Amt = 0;
                                    if (lblrecpdtls.Text != "")
                                    {
                                        string[] strArg = lblrecpdtls.Text.Split('|');
                                        for (int j = 0; j < strArg.Count() - 1; j++)
                                        {
                                            string[] strArg1 = strArg[j].Split('=');
                                            Amt += Convert.ToDouble(strArg1[1]);
                                        }
                                    }
                                    if (txt_Adjustamount.Text != "")
                                        txt_Adjustamount.Text = (Convert.ToDouble(txt_Adjustamount.Text) + Amt).ToString("0.00");
                                    else
                                        txt_Adjustamount.Text = Amt.ToString("0.00");
                                }
                            }
                        }
                    }
                }
              
            //}
            //catch (Exception ex)
            //{
            //    string s = ex.Message;
            //}
        }
   
        protected void LnkBtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime CurrentDt = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                string ADVNoteNo = "";
                
                int Year = Convert.ToInt32(CurrentDt.Year.ToString().Substring(2, 2));
                int tempYear = Convert.ToInt32(CurrentDt.Year);
                int Account = 0;
                string AccountYear = "";
                if (txt_NoteNo.Text == "Create New...")
                {
                    DateTime tempDate = new DateTime();
                    if (CurrentDt.Month > 3)
                    {
                        Account = Year + 1;
                        AccountYear = Year + "-" + Account;
                        tempYear = tempYear + 1;
                        tempDate = DateTime.ParseExact(("31/03/" + tempYear.ToString()), "dd/MM/yyyy", null);
                    }
                    else if (CurrentDt.Month <= 3)
                    {
                        Account = (Year - 1);
                        AccountYear = Account + "-" + Year;
                        tempDate = DateTime.ParseExact(("31/03/" + tempYear.ToString()), "dd/MM/yyyy", null);
                    }
                    //ADVNoteNo = "ADV" + "/" + AccountYear + "/" + dc.getAdvanceMaxNo(false, tempDate);
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("ADVNOTE");
                    //ADVNoteNo = "ADV" + "/" + AccountYear + "/" + NewrecNo;
                    ADVNoteNo = "ADV" + "/" + AccountYear;
                    var cn = dc.Journal_View(ADVNoteNo + "%", true, false, false);
                    foreach (var c in cn)
                    {
                        string[] strVal = c.NoteNo.Split('/');
                        if (strVal[strVal.GetUpperBound(0)] != "")
                        {
                            if (Convert.ToInt32(strVal[strVal.GetUpperBound(0)]) > NewrecNo)
                            {
                                NewrecNo = Convert.ToInt32(strVal[strVal.GetUpperBound(0)]);
                            }
                        }
                    }
                    NewrecNo = NewrecNo + 1;
                    ADVNoteNo = "ADV" + "/" + AccountYear + "/" + NewrecNo;
                    txt_NoteNo.Text = ADVNoteNo;
                }
                else
                {
                    ADVNoteNo = txt_NoteNo.Text;
                }

                string TotalAdjst =  gvAdvanceJournal.FooterRow.Cells[5].Text ;
                string TotalTDS = gvAdvanceJournal.FooterRow.Cells[4].Text;
                //delete previous records
                dc.AdvanceDetail_Update(0, "0", null, 0, "", 0, txt_NoteNo.Text, 0, false, true);
                dc.CashDetail_Update(0, "", null, 0, "", 0, false, false, Convert.ToInt32(lblClientId.Text), txt_NoteNo.Text, true, false);
                dc.Advance_Update(0, txt_NoteNo.Text, null, 0, 0, 0, "", "", 0, 0, false, false, false, "", false, false, 0, null, false, 0, true, false);
                //add new 
                //dc.Advance_Update(0, txt_NoteNo.Text, CurrentDt, Convert.ToDecimal(TotalAdjst), Convert.ToDecimal(TotalTDS), 0, "", txt_Note.Text, Convert.ToInt32(Session["LoginID"]), Convert.ToInt32(ddlLedgerName.SelectedValue), Convert.ToBoolean(ddl_status.SelectedValue), false, false, "", false, false, Convert.ToDecimal(Convert.ToDouble(TotalAdjst) - Convert.ToDouble(TotalTDS)), CurrentDt, false, Convert.ToInt32(ddlClient.SelectedValue), false, false);
                dc.Advance_Update(0, txt_NoteNo.Text, CurrentDt, Convert.ToDecimal(TotalAdjst), Convert.ToDecimal(TotalTDS), 0, "", txt_Note.Text, Convert.ToInt32(Session["LoginID"]), Convert.ToInt32(lblLedgerId.Text), Convert.ToBoolean(ddl_status.SelectedValue), false, false, "", false, false, Convert.ToDecimal(Convert.ToDouble(TotalAdjst) - Convert.ToDouble(TotalTDS)), CurrentDt, false, Convert.ToInt32(lblClientId.Text), false, false);
                for (int k = 0; k < grdAdjstdtl.Rows.Count; k++)
                {
                    double adjAmount = 0;
                    Label lblAdvRecNo = (Label)grdAdjstdtl.Rows[k].Cells[0].FindControl("lblAdvRecNo");
                    for (int i = 0; i < gvAdvanceJournal.Rows.Count; i++)
                    {                        
                        Label lblrecpdtls = (Label)gvAdvanceJournal.Rows[i].Cells[6].FindControl("lblrecpdtls");
                        
                        if (lblrecpdtls.Text != "")
                        {
                            string[] strArg = lblrecpdtls.Text.Split('|');
                            for (int j = 0; j < strArg.Count() - 1; j++)
                            {
                                string[] strArg1 = strArg[j].Split('=');
                                if (lblAdvRecNo.Text == strArg1[0])
                                {
                                    //if (Convert.ToInt32(strArg1[1]) > 0)
                                    if (Convert.ToDecimal(strArg1[1]) > 0)
                                    {
                                        adjAmount = adjAmount + Convert.ToDouble(strArg1[1]);

                                        Label lblBillNo = (Label)gvAdvanceJournal.Rows[i].Cells[0].FindControl("lblBillNo");
                                        TextBox txt_TDSamount = (TextBox)gvAdvanceJournal.Rows[i].Cells[4].FindControl("txt_TDSamount");
                                        
                                        string Settlement = "";
                                        string BillNo = "";
                                        decimal TDS = 0;
                                        if (lblBillNo.Text == "On A/c")
                                        {
                                            Settlement = "On A/c";
                                            BillNo = "";
                                        }
                                        else
                                        {
                                            BillNo = lblBillNo.Text;
                                        }
                                        if (txt_TDSamount.Text == "")
                                        {
                                            TDS = 0;
                                        }
                                        //dc.CashDetail_Update(Convert.ToInt32(lblAdvRecNo.Text), BillNo, CurrentDt, -(Convert.ToDecimal(strArg1[1])), Settlement, TDS, Convert.ToBoolean(ddl_status.SelectedValue), false, Convert.ToInt32(ddlClient.SelectedValue), txt_NoteNo.Text, false, false);
                                        dc.CashDetail_Update(Convert.ToInt32(lblAdvRecNo.Text), BillNo, CurrentDt, -(Convert.ToDecimal(strArg1[1])), Settlement, TDS, Convert.ToBoolean(ddl_status.SelectedValue), false, Convert.ToInt32(lblClientId.Text), txt_NoteNo.Text, false, false);
                                        
                                    }
                                }
                            }
                        }
                    }
                    if (adjAmount > 0)
                    {
                        //dc.AdvanceDetail_Update(Convert.ToInt32(lblAdvRecNo.Text), 0, CurrentDt, Convert.ToDecimal(adjAmount), "", 0, txt_NoteNo.Text, Convert.ToInt32(ddlLedgerName.SelectedValue), Convert.ToBoolean(ddl_status.SelectedValue), false);
                        dc.AdvanceDetail_Update(Convert.ToInt32(lblAdvRecNo.Text), "0", CurrentDt, Convert.ToDecimal(adjAmount), "", 0, txt_NoteNo.Text, Convert.ToInt32(lblLedgerId.Text), Convert.ToBoolean(ddl_status.SelectedValue), false);
                    }
                }
                dc.Client_Update_BalanceAmt(Convert.ToInt32(lblClientId.Text));
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                LnkBtnSave.Enabled = false;
                //lnkPrint.Visible = true;
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            
            if (txt_date.Text == "")
            {
                lblMsg.Text = "Enter Date ";
                txt_date.Focus();
                valid = false;
            }
            //else if (ddlLedgerName.SelectedItem.Text == "---Select---")
            else if (Convert.ToInt32(lblLedgerId.Text) == 0 || txt_Ledger.Text == "")
            {
                lblMsg.Text = "Select Ledger Name";
                txt_Ledger.Focus();
                valid = false;
            }
            //else if (ddlClient.Text == "")
            else if (Convert.ToInt32(lblClientId.Text) == 0 || txt_Client.Text == "")
            {
                lblMsg.Text = "Select Client Name ";
                txt_Client.Focus();
                valid = false;
            }
            else
            {
                valid = false;

                decimal Total = 0;
                for (int i = 0; i < gvAdvanceJournal.Rows.Count; i++)
                {
                    Button txt_Adjustamount = (Button)gvAdvanceJournal.Rows[i].Cells[5].FindControl("txt_Adjustamount");
                    if (txt_Adjustamount.Text != "" && Convert.ToDecimal(txt_Adjustamount.Text) > 0)
                    {
                        Total += Convert.ToDecimal(txt_Adjustamount.Text);
                        valid = true;
                    }
                }
                if (valid == false)
                {
                    lblMsg.Text = "Enter at least one Adjust Amount  ";                    
                }
                else if (valid == true)
                {
                    for (int i = 0; i < gvAdvanceJournal.Rows.Count; i++)
                    {
                        Label lblBillNo = (Label)gvAdvanceJournal.Rows[i].Cells[0].FindControl("lblBillNo");
                        TextBox txt_TDSamount = (TextBox)gvAdvanceJournal.Rows[i].Cells[4].FindControl("txt_TDSamount");
                        Button txt_Adjustamount = (Button)gvAdvanceJournal.Rows[i].Cells[5].FindControl("txt_Adjustamount");

                        if (txt_TDSamount.Text != "" && txt_Adjustamount.Text == "")
                        {
                            lblMsg.Text = "Enter Adjust Amount for Bill No.  " + lblBillNo.Text;
                            valid = false;
                            break;
                        }
                        else if (txt_TDSamount.Text != "" && txt_Adjustamount.Text != "")
                        {
                            if (Convert.ToDecimal(txt_TDSamount.Text) > Convert.ToDecimal(txt_Adjustamount.Text))
                            {
                                lblMsg.Text = "TDS amount should not be greater than the Adjust Amount for Bill No.  " + lblBillNo.Text;
                                valid = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (valid == true)
            {
                if (txt_Note.Text == "")
                {
                    lblMsg.Text = "Enter Note ";
                    txt_Note.Focus();
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
                CalTDS();
                BindTotalAdjstamt();
            }
            return valid;
        }
        protected Boolean ValidateAdjustdata()
        {
            Boolean valid = true;
            double TotalAdjstamt = 0;
            for (int j = 0; j < grdAdjstdtl.Rows.Count; j++)
            {
                TextBox txt_Adjstamount = (TextBox)grdAdjstdtl.Rows[j].Cells[4].FindControl("txt_Adjstamount");
                if (txt_Adjstamount.Text != "")
                {
                    TotalAdjstamt += Convert.ToDouble(txt_Adjstamount.Text);
                }
            }
            if (valid == true)
            {
                for (int j = 0; j < grdAdjstdtl.Rows.Count; j++)
                {
                    Label lblAdvRecNo = (Label)grdAdjstdtl.Rows[j].Cells[0].FindControl("lblAdvRecNo");
                    Label lblbalAmount = (Label)grdAdjstdtl.Rows[j].Cells[3].FindControl("lblbalAmount");
                    TextBox txt_Adjstamount = (TextBox)grdAdjstdtl.Rows[j].Cells[4].FindControl("txt_Adjstamount");
                    if (txt_Adjstamount.Text != "")
                    {
                        if (Convert.ToDecimal(txt_Adjstamount.Text) > Convert.ToDecimal(lblbalAmount.Text))
                        {
                            lbl_ErrMsg.Text = "Adjust Amount should be less than or equal to the Balance Amount for Receipt No.  " + lblAdvRecNo.Text;
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (valid == true)
            {
                if (lblAmtAdjusted.Text != "---")
                {
                    if (TotalAdjstamt > Convert.ToDouble(lblAmtAdjusted.Text))
                    {
                        lbl_ErrMsg.Text = "Sum of adjust Amount should be less than or equal to the Amount to be adjusted ";
                        valid = false;
                    }
                    else
                    {
                        lbl_ErrMsg.Text = "";
                    }
                }
            }
            if (valid == false)
            {
                lbl_ErrMsg.Visible = true;
            }
            else
            {
                lbl_ErrMsg.Visible = false;
            }
            return valid;
        }
        protected void txt_TDSamount_TextChanged(object sender, EventArgs e)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txt_TDSamount = (TextBox)gvAdvanceJournal.Rows[rowindex].Cells[4].FindControl("txt_TDSamount");
            CalTDS();
        }
        private void CalTDS()
        {
            decimal total = 0;
            foreach (GridViewRow r in gvAdvanceJournal.Rows)
            {
                var TDS = r.FindControl("txt_TDSamount") as TextBox;
                decimal number;
                if (decimal.TryParse(TDS.Text, out number))
                {
                    total += number;
                }
            }
            gvAdvanceJournal.FooterRow.Cells[4].Text = total.ToString("0.00");
            gvAdvanceJournal.FooterRow.Cells[4].Font.Bold = true;
            gvAdvanceJournal.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
        }
        private void BindTotalAdjstamt()
        {
            double TotalAdjst = 0;
            double TotalTDS = 0;
            double TotalBal = 0;
            for (int i = 0; i < gvAdvanceJournal.Rows.Count; i++)
            {
                Button txt_Adjustamount = (Button)gvAdvanceJournal.Rows[i].Cells[5].FindControl("txt_Adjustamount");
                Label lblBalAmount = (Label)gvAdvanceJournal.Rows[i].Cells[3].FindControl("lblBalAmount");
                TextBox txt_TDSamount = (TextBox)gvAdvanceJournal.Rows[i].Cells[4].FindControl("txt_TDSamount");
                gvAdvanceJournal.ShowFooter = true;
                if (txt_Adjustamount.Text != "---" && txt_Adjustamount.Text != "")
                {
                    TotalAdjst += Convert.ToDouble(txt_Adjustamount.Text);
                    gvAdvanceJournal.FooterRow.Cells[5].Text = Convert.ToDouble(TotalAdjst).ToString("0.00");
                    gvAdvanceJournal.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    gvAdvanceJournal.FooterRow.Cells[5].Font.Bold = true;
                }
                if (txt_TDSamount.Text != "---" && txt_TDSamount.Text != "")
                {
                    TotalTDS += Convert.ToDouble(txt_TDSamount.Text);
                    gvAdvanceJournal.FooterRow.Cells[4].Text = Convert.ToDouble(TotalTDS).ToString("0.00");
                    gvAdvanceJournal.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    gvAdvanceJournal.FooterRow.Cells[4].Font.Bold = true;
                }
                if (lblBalAmount.Text != "---" && lblBalAmount.Text != "")
                {
                    TotalBal += Convert.ToDouble(lblBalAmount.Text);
                    gvAdvanceJournal.FooterRow.Cells[3].Text = Convert.ToDouble(TotalBal).ToString("0.00");
                    gvAdvanceJournal.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    gvAdvanceJournal.FooterRow.Cells[3].Font.Bold = true;
                }
            }
        }
        
        protected void txt_Adjustamount_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            lbl_ErrMsg.Text = "";
            Button lnk = (Button)sender;
            GridViewRow row = (GridViewRow)lnk.Parent.Parent;
            int rowindex = row.RowIndex;
            Label lblBillNo = (Label)gvAdvanceJournal.Rows[rowindex].Cells[0].FindControl("lblBillNo");
            Button txt_Adjustamount = (Button)gvAdvanceJournal.Rows[rowindex].Cells[5].FindControl("txt_Adjustamount");
            Label lblBalAmount = (Label)gvAdvanceJournal.Rows[rowindex].Cells[3].FindControl("lblBalAmount");
            Label lblrecpdtls = (Label)gvAdvanceJournal.Rows[rowindex].Cells[6].FindControl("lblrecpdtls");

            //if (lblBillNo.Text == "On A/c")
            //{
            //    lblAmtAdjusted.Text = txt_Amtallocated.Text.ToString();
            //}
            lblAmtAdjusted.Text = lblBalAmount.Text.ToString();
            lblBILLNo.Text =  lblBillNo.Text.ToString();
            
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drow = null;
            dtTable.Columns.Add(new DataColumn("ReceiptNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ReceiptDate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
                        
            //var result = dc.AdvanceDetail_View(Convert.ToInt32(ddlLedgerName.SelectedValue), false, true,txt_NoteNo.Text );
            var result = dc.AdvanceDetail_View(Convert.ToInt32(lblLedgerId.Text), false, true, txt_NoteNo.Text);
            foreach (var recipt in result)            
            {
                drow = dtTable.NewRow();
                drow["ReceiptNo"] = recipt.ReceiptNo.ToString();
                drow["ReceiptDate"] = Convert.ToDateTime(recipt.ReceiptDate).ToString("dd/MM/yyyy");
                drow["Amount"] = recipt.ReceiptAmount;
                drow["BalanceAmount"] = recipt.Amount;

                double tempBalAmt = Convert.ToDouble(recipt.Amount) * (-1);
                for (int i = 0; i < gvAdvanceJournal.Rows.Count; i++)
                {
                    if (i != rowindex)
                    {
                        Label lblrecpdtls1 = (Label)gvAdvanceJournal.Rows[i].Cells[6].FindControl("lblrecpdtls");
                        if (lblrecpdtls1.Text != "")
                        {
                            string[] strArg = lblrecpdtls1.Text.Split('|');
                            for (int k = 0; k < strArg.Count()-1; k++)
                            {
                                string[] strArg1 = strArg[k].Split('=');
                                if (Convert.ToInt32(strArg1[0]) == recipt.ReceiptNo)
                                {
                                    tempBalAmt = tempBalAmt - Convert.ToDouble(strArg1[1]);
                                }
                            }
                        }
                    }
                }
                drow["BalanceAmount"] = tempBalAmt;
                    
                dtTable.Rows.Add(drow);
                dtTable.AcceptChanges();
                rowIndex++;
            }
            grdAdjstdtl.DataSource = dtTable;
            grdAdjstdtl.DataBind();
            rowIndex = 0; 
            
            if (lblrecpdtls.Text != "")
            {
                string[] recpdtls = lblrecpdtls.Text.Split('|');
                for (int k=0; k< recpdtls.Count()-1; k++)
                {
                    string[] strArg1= recpdtls[k].Split('=');
                    
                    for (int j = 0; j < grdAdjstdtl.Rows.Count; j++)
                    {
                        Label lblAdvRecNo = (Label)grdAdjstdtl.Rows[j].Cells[4].FindControl("lblAdvRecNo");
                        TextBox txt_Adjstamount = (TextBox)grdAdjstdtl.Rows[j].Cells[4].FindControl("txt_Adjstamount");
                        if (lblAdvRecNo.Text == strArg1[0])
                        {                            
                            txt_Adjstamount.Text = strArg1[1];
                        }
                    }
                }
            }
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateAdjustdata() == true)
            {
                ModalPopupExtender1.Hide();
                double TotalAdjstamt = 0;
                for (int i = 0; i < gvAdvanceJournal.Rows.Count; i++)
                {
                    Label lblBillNo = (Label)gvAdvanceJournal.Rows[i].Cells[0].FindControl("lblBillNo");
                    Button txt_Adjustamount = (Button)gvAdvanceJournal.Rows[i].Cells[5].FindControl("txt_Adjustamount");
                    Label lblrecpdtls = (Label)gvAdvanceJournal.Rows[i].Cells[6].FindControl("lblrecpdtls");
                    if (lblBILLNo.Text == lblBillNo.Text.Trim())
                    {
                        lblrecpdtls.Text = "";
                        for (int j = 0; j < grdAdjstdtl.Rows.Count; j++)
                        {
                            Label lblAdvRecNo = (Label)grdAdjstdtl.Rows[j].Cells[0].FindControl("lblAdvRecNo");
                            TextBox txt_Adjstamount = (TextBox)grdAdjstdtl.Rows[j].Cells[4].FindControl("txt_Adjstamount");
                            //lblrecpdtls.Text += "|" + lblAdvRecNo.Text + "=" + txt_Adjstamount.Text; 
                            if (txt_Adjstamount.Text != "")
                            {
                                lblrecpdtls.Text += lblAdvRecNo.Text + "=" + txt_Adjstamount.Text + "|"; 
                                TotalAdjstamt += Convert.ToDouble(txt_Adjstamount.Text);
                            }
                        }
                        txt_Adjustamount.Text = TotalAdjstamt.ToString("0.00");
                        break;
                    }
                }
                BindTotalAdjstamt();
            }
        }

        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            //object refUrl = ViewState["RefUrl"];
            //if (refUrl != null)
            //{
            //    Response.Redirect((string)refUrl);
            //}
            Response.Redirect("Advance_Modify.aspx");
        }

        protected void ddlLedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectGridviewfromClient();
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectGridviewfromClient();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
            gvAdvanceJournal.DataSource = null;
            gvAdvanceJournal.DataBind();
            grdAdjstdtl.DataSource = null;
            grdAdjstdtl.DataBind();
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID].ToString();
                    SelectGridviewfromClient();
                }
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

        protected void txt_Ledger_TextChanged(object sender, EventArgs e)
        {
            lblLedgerId.Text  = "0";
            gvAdvanceJournal.DataSource = null;
            gvAdvanceJournal.DataBind();
            grdAdjstdtl.DataSource = null;
            grdAdjstdtl.DataBind();
            if (ChkLedgerName(txt_Ledger.Text) == true)
            {
                if (txt_Ledger.Text != "")
                {
                    lblLedgerId.Text = Request.Form[hfLedgerId.UniqueID].ToString();
                    SelectGridviewfromClient();
                }
            }
        }

        protected Boolean ChkLedgerName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Ledger.Text;
            Boolean valid = false;
            var query = dc.Ledger_View(0, 0, true, searchHead);
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Ledger.Focus();
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
        public static List<string> GetLedgerName(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
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
                var clnm = db.Ledger_View(0, 0, true, "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.LedgerName_Description, rowObj.LedgerName_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> Ledger_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Ledger_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return Ledger_Name_var;

        }

    }
}