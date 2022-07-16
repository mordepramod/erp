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
    public partial class JournalEntry : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {   
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strReq = "";
                strReq = Request.RawUrl;
                if (strReq.Contains("?"))
                {
                    strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                    if (!strReq.Equals(""))
                    {
                        strReq = obj.Decrypt(strReq);
                        if (!strReq.Contains("=") == false)
                        {
                            string[] arrMsgs = strReq.Split('&');
                            string[] arrIndMsg;

                            arrIndMsg = arrMsgs[0].Split('=');
                            lblJournalNoteNo.Text = arrIndMsg[1].ToString().Trim();
                        }
                    }
                }
                else
                {
                    lblJournalNoteNo.Text = "Create New...";
                    lnkNext.Visible = false;
                    //txt_DBNoteNo.Text = "Create New...";
                }
                lblClientId.Text = "0";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Journal Entry";
                BindClient();
                getCurrentDate();

                if (lblJournalNoteNo.Text != "" && lblJournalNoteNo.Text != "Create New...")
                {
                    DisplayJournalDtl();
                    if (Rdn_Credit.Checked == true)
                    {
                        lblheading.Text = "Journal Modify - Credit Note";
                    }
                    else if (Rdn_Debit.Checked == true)
                    {
                        lblheading.Text = "Journal Modify - Debit Note";
                    }
                    lblJournalNoteNo.Text = "";
                    txt_Client.ReadOnly = true;
                }
                else
                {
                    //Rdn_Debit.Checked = true;
                    Rdn_Credit.Checked = true;
                }
                DisplayjournalGrid();
            }
        }
        public void getCurrentDate()
        {
            //this.txt_Date.Text = Convert.ToDateTime("31/03/2016").ToString("dd/MM/yyyy"); //DateTime.Today.ToString("dd/MM/yyyy");
            //this.txt_Date.Text = DateTime.ParseExact("31/03/2016", "dd/MM/yyyy", null).ToString("dd/MM/yyyy");
            txt_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddl_SiteName.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_ListOfBillNo.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void BindClient()
        {
            //var client = dc.Client_View(0, 0, "");
            //ddl_ClientName.DataSource = client;
            //ddl_ClientName.DataTextField = "CL_Name_var";
            //ddl_ClientName.DataValueField = "CL_Id";
            //ddl_ClientName.DataBind();
            //ddl_ClientName.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadSiteList()
        {
            ddl_SiteName.DataTextField = "SITE_Name_var";
            ddl_SiteName.DataValueField = "SITE_Id";
            //var site = dc.Site_View(0, Convert.ToInt32(ddl_ClientName.SelectedValue), 0, "");
            var site = dc.Site_View(0, Convert.ToInt32(lblClientId.Text), 0, "");
            ddl_SiteName.DataSource = site;
            ddl_SiteName.DataBind();
            ddl_SiteName.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadBillList()
        {
            //if (Convert.ToInt32(ddl_ClientName.SelectedValue) != 0 && Convert.ToInt32(ddl_SiteName.SelectedValue) != 0)
            if (Convert.ToInt32(lblClientId.Text) != 0 && Convert.ToInt32(ddl_SiteName.SelectedValue) != 0)
            {
                var client = dc.BillView(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(ddl_SiteName.SelectedValue), 0, "", false);
                ddl_ListOfBillNo.DataSource = client;
                ddl_ListOfBillNo.DataTextField = "BILL_Id";
                ddl_ListOfBillNo.DataValueField = "BILL_Id";
                ddl_ListOfBillNo.DataBind();
                ddl_ListOfBillNo.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
        protected void grdJournalEntry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_LedgerName = (DropDownList)e.Row.FindControl("ddl_LedgerName");
                var Ledger = dc.BillView(0, 0, 0, "", false);
                ddl_LedgerName.DataSource = Ledger;
                ddl_LedgerName.DataTextField = "LedgerName_Description";
                ddl_LedgerName.DataValueField = "LedgerName_Id";
                ddl_LedgerName.DataBind();
                ddl_LedgerName.Items.Insert(0, new ListItem("---Select---", "0"));
                if (ddl_LedgerName != null)
                {
                    ddl_LedgerName.SelectedIndexChanged += new EventHandler(ddl_LedgerName_SelectedIndexChanged);
                }
            }
        }
        protected void ddl_LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;

            DropDownList ddl_LedgerName = (DropDownList)grdJournalEntry.Rows[idx].Cells[0].FindControl("ddl_LedgerName");
            TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[idx].Cells[5].FindControl("txt_CostCenter");
            TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[idx].Cells[6].FindControl("txt_CostCatagory");
            TextBox txt_Id = (TextBox)grdJournalEntry.Rows[idx].Cells[10].FindControl("txt_Id");

            if (Convert.ToInt32(ddl_LedgerName.SelectedValue) != 0)
            {
                var data = dc.BillView(0, 0, Convert.ToInt32(ddl_LedgerName.SelectedValue), "", false);
                foreach (var r in data)
                {
                    txt_CostCenter.Text = r.CostCenter_Description;
                    txt_CostCatagory.Text = r.CostCatagory_Description;
                    txt_Id.Text = r.CostCenter_Id + "|" + r.CostCatagory_Id;
                }
            }
        }
        public void DisplayJournalDtl()
        {
            int i = 0;
            var result = dc.JournalDetail_View(lblJournalNoteNo.Text, "", 0);
            foreach (var r in result)
            {
                if (i == 0)
                {
                    Rdn_Debit.Enabled = false;
                    Rdn_Credit.Enabled = false;
                    txt_DBNoteNo.Text = Convert.ToString(r.Journal_NoteNo_var);
                    txt_Date.Text = Convert.ToDateTime(r.Journal_Date_dt).ToString("dd/MM/yyyy");
                    //ddl_ClientName.SelectedValue = r.CL_Id.ToString();
                    txt_Client.Text = r.CL_Name_var;
                    //lblClientId.Text = r.CL_Id.ToString() ;
                    lblClientId.Text = r.Journal_ClientId_int.ToString();
                    txt_Note.Text = r.Journal_Note_var.ToString();
                    if (Convert.ToInt32(r.Journal_SiteId_int) > 0)
                    {
                        Rdn_Debit.Checked = true;
                        LoadSiteList();
                        ddl_SiteName.SelectedValue = r.Journal_SiteId_int.ToString();
                        LoadBillList();
                        ddl_ListOfBillNo.SelectedValue = r.JournalDetail_BillNo_var.ToString();
                    }
                    if (r.Journal_SiteId_int == 0)
                    {
                        Rdn_Credit.Checked = true;
                        txt_ServiceTax.Text = r.Journal_ServiceTax_num.ToString();
                        if (r.Journal_Status_bit == true)
                        {
                            ddl_Status.ClearSelection();
                            ddl_Status.Items.FindByText("Hold").Selected = true;
                        }
                        if (r.Journal_Status_bit == false)
                        {
                            ddl_Status.ClearSelection();
                            ddl_Status.Items.FindByText("Ok").Selected = true;
                        }
                    }
                }
                if (Rdn_Debit.Checked == true)
                {
                    AddRowJournalEntry();
                    DropDownList ddl_LedgerName = (DropDownList)grdJournalEntry.Rows[i].Cells[0].FindControl("ddl_LedgerName");
                    TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[i].Cells[5].FindControl("txt_CostCenter");
                    TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[i].Cells[6].FindControl("txt_CostCatagory");
                    TextBox txt_Amount = (TextBox)grdJournalEntry.Rows[i].Cells[7].FindControl("txt_Amount");
                    TextBox txt_Id = (TextBox)grdJournalEntry.Rows[i].Cells[10].FindControl("txt_Id");

                    ddl_LedgerName.SelectedValue = r.JournalDetail_LedgerId_int.ToString();
                    txt_CostCenter.Text = Convert.ToString(r.CostCenter_Description);
                    txt_CostCatagory.Text = Convert.ToString(r.CostCatagory_Description);
                    txt_Amount.Text = Convert.ToString(r.JournalDetail_Amount_dec);
                    //txt_Id.Text = r.CostCenter_Id + "|" + r.CostCatagory_Id;
                    txt_Id.Text = r.JournalDetail_CostCenterId_int + "|" + r.JournalDetail_CatagoryId_int;
                    i++;
                }
                else
                {
                    //Bind_BillDetails();
                    if (i == 0)
                    {
                        LoadCreditNoteDetails();
                        i++;
                    }
                }

            }
        }
        public void LoadCreditNoteDetails()
        {
            grdJournalEntry.DataSource = null;
            grdJournalEntry.DataBind();

            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("txt_BillLedger", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_BillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_BillAmt", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_BalAmt", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_CostCenter", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_CostCatagory", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_Debit", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_Credit", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_Id", typeof(string)));
            ViewState["CurrentTable"] = null;

            int rowIndex = 0;
            if (Convert.ToInt32(lblClientId.Text) > 0)
            {
                if (lblJournalNoteNo.Text != "Create New..." && lblJournalNoteNo.Text != "")
                {
                    var journal = dc.CashDetail_View_Receipt(0, lblJournalNoteNo.Text, Convert.ToInt32(lblClientId.Text));
                    foreach (var jrnl in journal)
                    {
                        //AddRowJournalEntry();
                        //TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                        //TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                        //TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                        //TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                        //TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                        //TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                        //TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                        //TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                        //TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");

                        drow = dt.NewRow();

                        drow["txt_BillLedger"] = jrnl.CashDetail_BillNo_int.ToString();
                        if (jrnl.CashDetail_BillNo_int.Contains("DB/") == true)
                        {
                            drow["txt_BillDate"] = Convert.ToDateTime(jrnl.Journal_Date_dt).ToString("dd/MM/yyyy");
                            drow["txt_BillAmt"] = Convert.ToDecimal(jrnl.Journal_Amount_dec).ToString("0.00");
                            if (jrnl.billJrnlpendingAmt != null)
                            {
                                drow["txt_BalAmt"] = Convert.ToDecimal(jrnl.Journal_Amount_dec + jrnl.billJrnlpendingAmt).ToString("0.00");
                            }
                            else
                            {
                                drow["txt_BalAmt"] = Convert.ToDecimal(jrnl.Journal_Amount_dec).ToString("0.00");
                            }
                        }
                        else
                        {
                            drow["txt_BillDate"] = Convert.ToDateTime(jrnl.BILL_Date_dt).ToString("dd/MM/yyyy");
                            drow["txt_BillAmt"] = Convert.ToDecimal(jrnl.BILL_NetAmt_num).ToString("0.00");
                            if (jrnl.billJrnlpendingAmt != null)
                            {
                                drow["txt_BalAmt"] = Convert.ToDecimal(jrnl.BILL_NetAmt_num + jrnl.billJrnlpendingAmt).ToString("0.00");
                            }
                            else
                            {
                                drow["txt_BalAmt"] = Convert.ToDecimal(jrnl.BILL_NetAmt_num).ToString("0.00");
                            }
                        }
                        drow["txt_Credit"] = Math.Abs(Convert.ToDecimal(jrnl.CashDetail_Amount_money)).ToString("0.00");
                        drow["txt_CostCenter"] = "---";
                        drow["txt_CostCatagory"] = "---";
                        dt.Rows.Add(drow);
                        dt.AcceptChanges();
                        rowIndex++;
                    }
                    ViewState["CurrentTable"] = dt;
                    grdJournalEntry.DataSource = dt;
                    grdJournalEntry.DataBind();
                }
                var pendingBillJrnl = dc.CashDetail_View_Receipt(0, "", Convert.ToInt32(lblClientId.Text));
                foreach (var billJrnl in pendingBillJrnl)
                {
                    bool addFlag = true;
                    foreach (GridViewRow row in grdJournalEntry.Rows)
                    {
                        TextBox vbillno = (TextBox)row.FindControl("txt_BillLedger");
                        if (vbillno.Text == Convert.ToString(billJrnl.BILL_Id))
                        {
                            addFlag = false;
                            break;
                        }
                    }
                    if (addFlag == true)
                    {
                        //AddRowJournalEntry();
                        //TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                        //TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                        //TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                        //TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                        //TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                        //TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                        //TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                        //TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                        //TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");
                        drow = dt.NewRow();

                        drow["txt_BillLedger"] = billJrnl.BILL_Id;
                        drow["txt_BillDate"] = Convert.ToDateTime(billJrnl.BILL_Date_dt).ToString("dd/MM/yyyy");
                        drow["txt_BillAmt"] = Convert.ToDecimal(billJrnl.BILL_NetAmt_num).ToString("0.00");
                        if (billJrnl.billJrnlpendingAmt != null)
                        {
                            drow["txt_BalAmt"] = Convert.ToDecimal(billJrnl.billJrnlpendingAmt).ToString("0.00");
                        }
                        else
                        {
                            drow["txt_BalAmt"] = Convert.ToDecimal(billJrnl.BILL_NetAmt_num).ToString("0.00");
                        }
                        drow["txt_CostCenter"] = "---";
                        drow["txt_CostCatagory"] = "---";
                        dt.Rows.Add(drow);
                        dt.AcceptChanges();
                        rowIndex++;
                    }
                }
                ViewState["CurrentTable"] = dt;
                grdJournalEntry.DataSource = dt;
                grdJournalEntry.DataBind();

                var ledger = dc.Ledger_View(0, 0, false, "");
                foreach (var ledg in ledger)
                {
                    //AddRowJournalEntry();
                    //TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                    //TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                    //TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                    //TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                    //TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                    //TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                    //TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                    //TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                    //TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");
                    
                    drow = dt.NewRow();
                    drow["txt_BillLedger"] = ledg.LedgerName_Description.ToString();
                    drow["txt_CostCenter"] = ledg.CostCenter_Description;
                    drow["txt_CostCatagory"] = ledg.CostCatagory_Description;
                    drow["txt_Id"] = ledg.LedgerName_CostCenter_Id + "|" + ledg.LedgerName_Catagory_Id + "|" + ledg.LedgerName_Id;
                    
                    drow["txt_BillDate"] = "---";
                    drow["txt_BillAmt"] = "---";
                    drow["txt_BalAmt"] = "---";
                    dt.Rows.Add(drow);
                    dt.AcceptChanges();
                    rowIndex++;
                }
                ViewState["CurrentTable"] = dt;
                grdJournalEntry.DataSource = dt;
                grdJournalEntry.DataBind();

                if (lblJournalNoteNo.Text != "")
                {
                    var jrnlDetails = dc.JournalDetail_View(lblJournalNoteNo.Text, "", 0);
                    foreach (var JrnlDtl in jrnlDetails)
                    {
                        foreach (GridViewRow row in grdJournalEntry.Rows)
                        {
                            TextBox txt_Id = (TextBox)row.FindControl("txt_Id");
                            TextBox txt_Debit = (TextBox)row.FindControl("txt_Debit");
                            if (txt_Id.Text != "")
                            {
                                string[] strId = txt_Id.Text.Split('|');
                                if (strId[2] == Convert.ToString(JrnlDtl.JournalDetail_LedgerId_int))
                                {
                                    txt_Debit.Text = Convert.ToString(JrnlDtl.JournalDetail_Amount_dec);
                                    break;
                                }
                            }
                        }
                    }
                }
                SumOfTotalCredit();
                DisplayChkServiceTax();
            }
        }
        public void LoadCreditNoteDetailsOld()
        {
            grdJournalEntry.DataSource = null;
            grdJournalEntry.DataBind();
            int rowIndex = 0;
            if (Convert.ToInt32(lblClientId.Text) > 0)
            {
                if (lblJournalNoteNo.Text != "Create New..." && lblJournalNoteNo.Text != "")
                {
                    var journal = dc.CashDetail_View_Receipt(0, lblJournalNoteNo.Text, Convert.ToInt32(lblClientId.Text));
                    foreach (var jrnl in journal)
                    {
                        AddRowJournalEntry();
                        TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                        TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                        TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                        TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                        TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                        TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                        TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                        TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                        TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");

                        txt_BillLedger.Text = jrnl.CashDetail_BillNo_int.ToString();
                        if (jrnl.CashDetail_BillNo_int.Contains("DB/") == true)
                        {
                            txt_BillDate.Text = Convert.ToDateTime(jrnl.Journal_Date_dt).ToString("dd/MM/yyyy");
                            txt_BillAmt.Text = Convert.ToDecimal(jrnl.Journal_Amount_dec).ToString("0.00");
                            if (jrnl.billJrnlpendingAmt != null)
                            {
                                txt_BalAmt.Text = Convert.ToDecimal(jrnl.Journal_Amount_dec + jrnl.billJrnlpendingAmt).ToString("0.00");
                            }
                            else
                            {
                                txt_BalAmt.Text = Convert.ToDecimal(jrnl.Journal_Amount_dec).ToString("0.00");
                            }
                        }
                        else
                        {
                            txt_BillDate.Text = Convert.ToDateTime(jrnl.BILL_Date_dt).ToString("dd/MM/yyyy");
                            txt_BillAmt.Text = Convert.ToDecimal(jrnl.BILL_NetAmt_num).ToString("0.00");
                            if (jrnl.billJrnlpendingAmt != null)
                            {
                                txt_BalAmt.Text = Convert.ToDecimal(jrnl.BILL_NetAmt_num + jrnl.billJrnlpendingAmt).ToString("0.00");
                            }
                            else
                            {
                                txt_BalAmt.Text = Convert.ToDecimal(jrnl.BILL_NetAmt_num).ToString("0.00");
                            }
                        }
                        txt_Credit.Text = Math.Abs(Convert.ToDecimal(jrnl.CashDetail_Amount_money)).ToString("0.00");
                        txt_CostCenter.Text = "---";
                        txt_CostCatagory.Text = "---";
                        rowIndex++;
                    }
                }
                var pendingBillJrnl = dc.CashDetail_View_Receipt(0, "", Convert.ToInt32(lblClientId.Text));
                foreach (var billJrnl in pendingBillJrnl)
                {
                    bool addFlag = true;
                    foreach (GridViewRow row in grdJournalEntry.Rows)
                    {
                        TextBox vbillno = (TextBox)row.FindControl("txt_BillLedger");
                        if (vbillno.Text == Convert.ToString(billJrnl.BILL_Id))
                        {
                            addFlag = false;
                            break;
                        }
                    }
                    if (addFlag == true)
                    {
                        AddRowJournalEntry();
                        TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                        TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                        TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                        TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                        TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                        TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                        TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                        TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                        TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");

                        txt_BillLedger.Text = billJrnl.BILL_Id;
                        txt_BillDate.Text = Convert.ToDateTime(billJrnl.BILL_Date_dt).ToString("dd/MM/yyyy");
                        txt_BillAmt.Text = Convert.ToDecimal(billJrnl.BILL_NetAmt_num).ToString("0.00");
                        if (billJrnl.billJrnlpendingAmt != null)
                        {
                            txt_BalAmt.Text = Convert.ToDecimal(billJrnl.billJrnlpendingAmt).ToString("0.00");
                        }
                        else
                        {
                            txt_BalAmt .Text = Convert.ToDecimal(billJrnl.BILL_NetAmt_num).ToString("0.00");
                        }
                        txt_CostCenter.Text = "---";
                        txt_CostCatagory.Text = "---";
                        rowIndex++;
                    }
                }
                var ledger = dc.Ledger_View(0, 0, false, "");
                foreach (var ledg in ledger)
                {
                    AddRowJournalEntry();
                    TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                    TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                    TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                    TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                    TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                    TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                    TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                    TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                    TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");

                    txt_BillLedger.Text = ledg.LedgerName_Description.ToString();
                    txt_CostCenter.Text = ledg.CostCenter_Description;
                    txt_CostCatagory.Text = ledg.CostCatagory_Description;
                    txt_Id.Text = ledg.LedgerName_CostCenter_Id + "|" + ledg.LedgerName_Catagory_Id + "|" + ledg.LedgerName_Id;
                    //var Cost_ = dc.BillView(0, 0, Convert.ToInt32(r.LedgerName_Id), "", false);
                    //foreach (var cost in Cost_)
                    //{
                    //    txt_CostCenter.Text = cost.CostCenter_Description;
                    //    txt_CostCatagory.Text = cost.CostCatagory_Description;
                    //    txt_Id.Text = cost.CostCenter_Id + "|" + cost.CostCatagory_Id + "|" + cost.LedgerName_Id;
                    //}

                    //txt_Id.Text = ledg.LedgerName_Id.ToString();
                    txt_BillDate.Text = "---";
                    txt_BillAmt.Text = "---";
                    txt_BalAmt.Text = "---";
                    //txt_CostCenter.Text = "---";
                    //txt_CostCatagory.Text = "---";
                    rowIndex++;
                }
                if (lblJournalNoteNo.Text != "")
                {
                    var jrnlDetails = dc.JournalDetail_View(lblJournalNoteNo.Text, "", 0);
                    foreach (var JrnlDtl in jrnlDetails)
                    {   
                        foreach (GridViewRow row in grdJournalEntry.Rows)
                        {
                            TextBox txt_Id = (TextBox)row.FindControl("txt_Id");
                            TextBox txt_Debit = (TextBox)row.FindControl("txt_Debit");
                            if (txt_Id.Text != "")
                            {
                                string[] strId = txt_Id.Text.Split('|');
                                if (strId[2] == Convert.ToString(JrnlDtl.JournalDetail_LedgerId_int))
                                {
                                    txt_Debit.Text = Convert.ToString(JrnlDtl.JournalDetail_Amount_dec);
                                    break;
                                }
                            }
                        }
                    }
                }
                SumOfTotalCredit();
                DisplayChkServiceTax();
            }
        }
        public void Bind_BillDetails()
        {
            int rowIndex = 0;

            //if (Convert.ToInt32(ddl_ClientName.SelectedValue) != 0)
            if (Convert.ToInt32(lblClientId.Text) > 0)
            {
                grdJournalEntry.DataSource = null;
                grdJournalEntry.DataBind();
                //var details = dc.BillView(Convert.ToInt32(ddl_ClientName.SelectedValue), 0, 0, "", false);
                var details = dc.BillView(Convert.ToInt32(lblClientId.Text), 0, 0, "", false);
                foreach (var d in details)
                {
                    AddRowJournalEntry();
                    TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                    TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                    TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                    TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                    TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                    TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                    TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                    TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                    TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");

                    txt_BillLedger.Text = d.BILL_Id.ToString();
                    txt_BillDate.Text = Convert.ToDateTime(d.BILL_Date_dt).ToString("dd/MM/yyyy");
                    txt_BillAmt.Text = d.BILL_NetAmt_num.ToString();

                     if (lblJournalNoteNo.Text != "")
                    {
                        var res = dc.JournalDetail_View(lblJournalNoteNo.Text, txt_BillLedger.Text, 0);
                        foreach (var cd in res)
                        {
                            txt_Credit.Text = Math.Abs(Convert.ToDecimal(cd.JournalDetail_Amount_dec)).ToString();
                        }
                    }
                    decimal CashDetailAmt = 0;

                    var BalanceAmt = dc.BillView(0, 0, 0, txt_BillLedger.Text, false);
                    foreach (var cash in BalanceAmt)
                    {
                        CashDetailAmt = (Convert.ToDecimal(d.BILL_NetAmt_num) + Convert.ToDecimal(cash.CashDetail_Amount_money));
                    }
                    if (CashDetailAmt != 0)
                    {
                        txt_BalAmt.Text = Convert.ToDecimal(CashDetailAmt).ToString("0.00");
                    }
                    txt_CostCenter.Text = "---";
                    txt_CostCatagory.Text = "---";
                    rowIndex++;
                }
                var data = dc.BillView(0, 0, 0, "", false);
                foreach (var r in data)
                {
                    AddRowJournalEntry();
                    TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[1].FindControl("txt_BillLedger");
                    TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[2].FindControl("txt_BillDate");
                    TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[3].FindControl("txt_BillAmt");
                    TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[4].FindControl("txt_BalAmt");
                    TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[5].FindControl("txt_CostCenter");
                    TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[6].FindControl("txt_CostCatagory");
                    TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[8].FindControl("txt_Debit");
                    TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[9].FindControl("txt_Credit");
                    TextBox txt_Id = (TextBox)grdJournalEntry.Rows[rowIndex].Cells[10].FindControl("txt_Id");

                    txt_BillLedger.Text = r.LedgerName_Description.ToString();

                    if (lblJournalNoteNo.Text != "")
                    {
                        var res = dc.JournalDetail_View(lblJournalNoteNo.Text, "", Convert.ToInt32(r.LedgerName_Id));
                        foreach (var cd in res)
                        {
                            txt_Debit.Text = Convert.ToString(cd.JournalDetail_Amount_dec);
                        }
                    }
                    var Cost_ = dc.BillView(0, 0, Convert.ToInt32(r.LedgerName_Id), "", false);
                    foreach (var cost in Cost_)
                    {
                        txt_CostCenter.Text = cost.CostCenter_Description;
                        txt_CostCatagory.Text = cost.CostCatagory_Description;
                        txt_Id.Text = cost.CostCenter_Id + "|" + cost.CostCatagory_Id + "|" + cost.LedgerName_Id;
                    }
                    if (txt_Id.Text == "")
                    {
                        txt_Id.Text = r.LedgerName_Id.ToString();
                    }
                    if (txt_BillDate.Text == "")
                    {
                        txt_BillDate.Text = "---";
                    }
                    if (txt_BillAmt.Text == "")
                    {
                        txt_BillAmt.Text = "---";
                    }
                    if (txt_BalAmt.Text == "")
                    {
                        txt_BalAmt.Text = "---";
                    }
                    if (txt_CostCenter.Text == "")
                    {
                        txt_CostCenter.Text = "---";
                    }
                    if (txt_CostCatagory.Text == "")
                    {
                        txt_CostCatagory.Text = "---";
                    }
                    rowIndex++;
                }
                SumOfTotalCredit();
                DisplayChkServiceTax();
            }
        }
        public void SumofTotalDebit()
        {
            GridViewRow rowfooter = grdJournalEntry.FooterRow;
            TextBox txt_TotalAmt = ((TextBox)rowfooter.FindControl("txt_TotalAmt"));

            decimal total = 0;
            decimal number;
            foreach (GridViewRow r in grdJournalEntry.Rows)
            {
                var txt_Amount = r.FindControl("txt_Amount") as TextBox;
                if (decimal.TryParse(txt_Amount.Text, out number))
                {
                    total += number;
                }
                txt_TotalAmt.Text = total.ToString("0.00");
            }
        }
        public void SumOfTotalCredit()
        {
            GridViewRow rowfooter = grdJournalEntry.FooterRow;
            TextBox txt_TotalBalAmt = ((TextBox)rowfooter.FindControl("txt_TotalBalAmt"));
            TextBox txt_TotalDebitAmt = ((TextBox)rowfooter.FindControl("txt_TotalDebitAmt"));
            TextBox txt_TotalCreditAmt = ((TextBox)rowfooter.FindControl("txt_TotalCreditAmt"));
            decimal total = 0;
            decimal number;
            decimal ServiceTax = 0;
            foreach (GridViewRow r in grdJournalEntry.Rows)
            {
                var txt_BillLedger = r.FindControl("txt_BillLedger") as TextBox;
                var txt_BillDate = r.FindControl("txt_BillDate") as TextBox;
                var txt_Credit = r.FindControl("txt_Credit") as TextBox;
                var txt_Debit = r.FindControl("txt_Debit") as TextBox;
                if (txt_BillLedger.Text == "On A/c")
                {
                    txt_Debit.ReadOnly = false;
                    txt_Credit.ReadOnly = false;
                }
                else if (txt_BillDate.Text != "---")
                {
                    txt_Debit.ReadOnly = true;
                    txt_Credit.ReadOnly = false;
                }
                else
                {
                    txt_Debit.ReadOnly = false;
                    txt_Credit.ReadOnly = true;
                }
            }
            foreach (GridViewRow r in grdJournalEntry.Rows)
            {
                var txt_BalAmt = r.FindControl("txt_BalAmt") as TextBox;
                if (decimal.TryParse(txt_BalAmt.Text, out number))
                {
                    total += number;
                }
                txt_TotalBalAmt.Text = total.ToString("0.00");

            }
            total = 0;
            number = 0;
            foreach (GridViewRow r in grdJournalEntry.Rows)
            {
                var txt_Debit = r.FindControl("txt_Debit") as TextBox;
                if (decimal.TryParse(txt_Debit.Text, out number))
                {
                    total += number;
                }
                txt_TotalDebitAmt.Text = total.ToString("0.00");

            }
            total = 0;
            number = 0;
            foreach (GridViewRow r in grdJournalEntry.Rows)
            {
                var txt_Credit = r.FindControl("txt_Credit") as TextBox;
                if (decimal.TryParse(txt_Credit.Text, out number))
                {
                    total += number;
                    ServiceTax += Convert.ToDecimal(txt_Credit.Text) / (100 + 12) * 12;
                }
                txt_TotalCreditAmt.Text = total.ToString("0.00");
            }
            txt_Amtallocated.Text = (Convert.ToDecimal(txt_TotalDebitAmt.Text) - Convert.ToDecimal(txt_TotalCreditAmt.Text)).ToString("0.00");
            txt_ServiceTax.Text = Convert.ToInt32(ServiceTax).ToString("0.00");
        }
        protected void LnkBtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime JournalDt = DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", null);

                string DBNoteNo = "";                
                int SrNo = 0;

                int Year = Convert.ToInt32(JournalDt.Year.ToString().Substring(2, 2));
                int Account = 0;
                string AccountYear = "";
                if (txt_DBNoteNo.Text == "")
                {
                    if (JournalDt.Month > 3)
                    {
                        Account = Year + 1;
                        AccountYear = Year + "-" + Account;
                    }
                    else if (JournalDt.Month <= 3)
                    {
                        Account = (Year - 1);
                        AccountYear = Account + "-" + Year;
                    }
                    if (Rdn_Debit.Checked)
                    {
                        DBNoteNo = "DB" + "/" + AccountYear;
                    }
                    else if (Rdn_Credit.Checked)
                    {
                        DBNoteNo = "CR" + "/" + AccountYear;
                    }
                    SrNo = 0;
                    var cn = dc.Journal_View(DBNoteNo + "%", true, false, false);
                    foreach (var c in cn)
                    {
                        string[] strVal=c.Journal_NoteNo_var.ToString().Split('/');
                        if (strVal[strVal.GetUpperBound(0)] != "")
                        {
                            if (Convert.ToInt32(strVal[strVal.GetUpperBound(0)]) > SrNo)
                            {
                                SrNo = Convert.ToInt32(strVal[strVal.GetUpperBound(0)]);
                            }
                        }                         
                    }
                    SrNo = SrNo + 1;
                    if (Rdn_Debit.Checked)
                    {
                        DBNoteNo = "DB" + "/" + AccountYear + "/" + SrNo.ToString();
                    }
                    else if (Rdn_Credit.Checked)
                    {
                        DBNoteNo = "CR" + "/" + AccountYear + "/" + SrNo.ToString();
                        ddl_ListOfBillNo.SelectedItem.Text = "";
                    }
                    txt_DBNoteNo.Text = DBNoteNo;
                }
                else
                {
                    DBNoteNo = txt_DBNoteNo.Text;
                }

               // dc.Debit_Update(DBNoteNo, "", JournalDt, 0, 0, false, false, "", 0, true);
                //dc.JournalDetail_Update(DBNoteNo, 0, 0, 0, 0, "", true);
                //dc.Journal_Update(DBNoteNo, null, false, 0, 0, "", 0, 0, 0, true);

                TextBox txt_TotalBalAmt = (TextBox)grdJournalEntry.FooterRow.Cells[4].FindControl("txt_TotalBalAmt");
                TextBox txt_TotalCreditAmt = (TextBox)grdJournalEntry.FooterRow.Cells[9].FindControl("txt_TotalCreditAmt");//
                int j=0;

                for (int i = 0; i < grdJournalEntry.Rows.Count; i++)
                {
                    string Settlement = "";
                    DropDownList ddl_LedgerName = (DropDownList)grdJournalEntry.Rows[i].Cells[0].FindControl("ddl_LedgerName");
                    TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[i].Cells[1].FindControl("txt_BillLedger");
                    TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[i].Cells[2].FindControl("txt_BillDate");
                    TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[i].Cells[3].FindControl("txt_BillAmt");
                    TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[i].Cells[4].FindControl("txt_BalAmt");
                    TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[i].Cells[5].FindControl("txt_CostCenter");
                    TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[i].Cells[6].FindControl("txt_CostCatagory");
                    TextBox txt_Amount = (TextBox)grdJournalEntry.Rows[i].Cells[7].FindControl("txt_Amount");
                    TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[i].Cells[8].FindControl("txt_Debit");
                    TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[i].Cells[9].FindControl("txt_Credit");
                    TextBox txt_Id = (TextBox)grdJournalEntry.Rows[i].Cells[10].FindControl("txt_Id");

                    if (ddl_LedgerName.SelectedItem.Text == "On A/c")
                    {
                        Settlement = ddl_LedgerName.SelectedItem.Text;
                        ddl_ListOfBillNo.SelectedItem.Text = "";
                    }
                    if (ddl_ListOfBillNo.SelectedItem.Text == "---Select---")
                    {
                        ddl_ListOfBillNo.SelectedItem.Text = "";
                    }

                    decimal Amount = 0;
                    if (Rdn_Credit.Checked)
                    {
                        ddl_SiteName.SelectedValue = "0";
                        ddl_LedgerName.SelectedValue = "0";
                        if (txt_Credit.Text != "")
                        {
                            if (Convert.ToDecimal(txt_Credit.Text) > 0)
                            {
                                Amount = -(Convert.ToDecimal(txt_Credit.Text));
                            }
                        }
                        if (txt_Debit.Text != "")
                        {
                            if (Convert.ToDecimal(txt_Debit.Text) > 0)
                            {
                                Amount = (Convert.ToDecimal(txt_Debit.Text));
                            }
                        }
                    }
                    else if (Rdn_Debit.Checked)
                    {
                        Amount = Convert.ToDecimal(txt_Amount.Text);
                    }
                    int CostCenterId = 0;
                    int CostCatagoryId = 0;
                    int LedgerId = 0;

                    if (txt_Id.Text != "")
                    {
                        string[] Id = txt_Id.Text.Split('|');
                        foreach (string IdValue in Id)
                        {
                            if (IdValue != "")
                            {
                                if (CostCatagoryId != 0)
                                {
                                    LedgerId = Convert.ToInt32(IdValue);
                                }
                                else if (CostCenterId != 0)
                                {
                                    CostCatagoryId = Convert.ToInt32(IdValue);
                                }
                                else
                                {
                                    CostCenterId = Convert.ToInt32(IdValue);
                                }
                            }
                        }
                        if (CostCatagoryId == 0)
                        {
                            LedgerId = CostCenterId;
                        }
                    }
                    if (Rdn_Credit.Checked)
                    {
                        if (txt_Credit.Text != "" || txt_Debit.Text != "")
                        {
                            if (j == 0)
                            {
                                dc.Journal_Update(DBNoteNo, JournalDt, Convert.ToBoolean(ddl_Status.SelectedValue), Convert.ToInt32(lblClientId.Text), Convert.ToDecimal(txt_ServiceTax.Text), txt_Note.Text, Convert.ToInt32(ddl_SiteName.SelectedValue), Convert.ToDecimal(txt_TotalCreditAmt.Text), Convert.ToInt32(Session["LoginID"]), false);
                                j++;
                            }
                            if (txt_Credit.Text != "")
                            {
                                if (Convert.ToDecimal(txt_Credit.Text) > 0)
                                {
                                    dc.Debit_Update(DBNoteNo, txt_BillLedger.Text, JournalDt, Amount, 0, Convert.ToBoolean(ddl_Status.SelectedValue), false, Settlement, Convert.ToInt32(lblClientId.Text), false);
                                    dc.JournalDetail_Update(DBNoteNo, Convert.ToInt32(ddl_LedgerName.SelectedValue), CostCenterId, CostCatagoryId, Amount, txt_BillLedger.Text, false);
                                }
                            }
                            if (txt_Debit.Text != "")
                            {
                                if (Convert.ToDecimal(txt_Debit.Text) > 0)
                                {
                                    dc.JournalDetail_Update(DBNoteNo, LedgerId, CostCenterId, CostCatagoryId, Amount, "", false);
                                }
                            }
                            
                        }
                    }
                    else if (Rdn_Debit.Checked)
                    {   
                        if (j == 0)
                        {
                            dc.Journal_Update(DBNoteNo, JournalDt, Convert.ToBoolean(ddl_Status.SelectedValue), Convert.ToInt32(lblClientId.Text), 0, txt_Note.Text, Convert.ToInt32(ddl_SiteName.SelectedValue), Convert.ToDecimal(txt_Amount.Text), Convert.ToInt32(Session["LoginID"]), false);
                            j++;
                        }
                        dc.JournalDetail_Update(DBNoteNo, Convert.ToInt32(ddl_LedgerName.SelectedValue), CostCenterId, CostCatagoryId, Convert.ToDecimal(txt_Amount.Text), ddl_ListOfBillNo.SelectedItem.Text, false);
                    }
                    
                }
                
                //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record is updated sucessfully');", true);
                Label lblMsg = (Label)Master.FindControl("lblMsg");                
                lblMsg.Text = "Record updated sucessfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                LnkBtnSave.Enabled = false;
                Rdn_Debit.Enabled = false  ;
                Rdn_Credit.Enabled = false ;
                lnkNext.Visible = true;
                if (Rdn_Credit.Checked)
                {
                    lnkPrint.Visible = true;
                }
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (Rdn_Credit.Checked)
            {
                DateTime BillDate = DateTime.Now;
                if (txt_DBNoteNo.Text != "")
                {
                    // Session["DBNoteNo"] = txt_DBNoteNo.Text;
                }
                var JOurnal = dc.Journal_View(txt_DBNoteNo.Text, false, true, false).ToList();
                foreach (var Credit in JOurnal)
                {
                    BillDate = Convert.ToDateTime(Credit.BILL_Date_dt);
                    break;
                }

                PrintPDFReport rpt = new PrintPDFReport();
                if (CheckGSTFlag(BillDate) == false)
                    rpt.Journal_PDF(txt_DBNoteNo.Text);//service tax old format
                else
                    rpt.Journal_PDF_GST(txt_DBNoteNo.Text);//GST new print
            }
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
    
        protected void txt_Amount_TextChanged(object sender, EventArgs eventArgs)
        {
            SumofTotalDebit();
        }
        protected void txt_Debit_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowindex].Cells[8].FindControl("txt_Debit");
            TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowindex].Cells[8].FindControl("txt_Credit");            
            if (txt_Debit.Text != "")
                txt_Credit.Text = "";
            SumOfTotalCredit();
            txt_Debit.Focus();

        }
        protected void txt_Credit_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[rowindex].Cells[9].FindControl("txt_Credit");
            TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[rowindex].Cells[8].FindControl("txt_Debit");
            if (txt_Credit.Text != "")
                txt_Debit.Text = "";
            SumOfTotalCredit();
            txt_Credit.Focus();

        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            //string dispalyMsg = "";
            Boolean valid = true;
            GridViewRow rowfooter = grdJournalEntry.FooterRow;
            TextBox txt_TotalDebitAmt = ((TextBox)rowfooter.FindControl("txt_TotalDebitAmt"));
            TextBox txt_TotalCreditAmt = ((TextBox)rowfooter.FindControl("txt_TotalCreditAmt"));
            if (Rdn_Debit.Checked == true)
            {
                SumofTotalDebit();
            }
            else
            {
                for (int i = 0; i < grdJournalEntry.Rows.Count; i++)
                {
                    TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[i].Cells[9].FindControl("txt_Credit");
                    TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[i].Cells[8].FindControl("txt_Debit");
                    if (txt_Credit.Text != "" && txt_Debit.Text != "")
                    {
                        txt_Debit.Text = "";
                    }
                }
                SumOfTotalCredit();
            }
            if (txt_Date.Text == "")
            {
                lblMsg.Text = "Please Enter Date ";
                valid = false;
            }
            //else if (Convert.ToInt32(ddl_ClientName.SelectedValue) == 0)
            else if (txt_Client.Text == "" || Convert.ToInt32(lblClientId.Text) == 0)
            {
                lblMsg.Text = "Please Select Client Name ";
                valid = false;
            }
            else if (ddl_SiteName.SelectedItem.Text == "---Select---" && ddl_SiteName.Visible == true)
            {
                lblMsg.Text = "Please Select Site Name ";
                valid = false;
            }
            //else if (ddl_ListOfBillNo.SelectedItem.Text == "---Select---" && ddl_ListOfBillNo.Visible == true)
            //{
            //    lblMsg.Text = "Please Select Bill No ";
            //    valid = false;
            //}
            else if (valid == true && Rdn_Debit.Checked)
            {
                for (int i = 0; i < grdJournalEntry.Rows.Count; i++)
                {
                    DropDownList ddl_LedgerName = (DropDownList)grdJournalEntry.Rows[i].Cells[0].FindControl("ddl_LedgerName");
                    TextBox txt_Amount = (TextBox)grdJournalEntry.Rows[i].Cells[7].FindControl("txt_Amount");

                    if (Convert.ToInt32(ddl_LedgerName.SelectedValue) == 0)
                    {
                        lblMsg.Text = "Please Select Ledger Name";
                        ddl_LedgerName.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Amount.Text == "")
                    {
                        lblMsg.Text = "Please Enter Amount ";
                        txt_Amount.Focus();
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == true && Rdn_Credit.Checked)
            {
                if (Convert.ToDecimal(txt_TotalDebitAmt.Text) != Convert.ToDecimal(txt_TotalCreditAmt.Text) || Convert.ToDecimal(txt_TotalDebitAmt.Text) <= 0 || Convert.ToDecimal(txt_TotalCreditAmt.Text) <= 0)
                {
                    lblMsg.Text = "Total Debit Amount does not match to the Total Credit Amount ";
                    valid = false;
                }

            }
            if (valid == true)
            {
                if (txt_Note.Text == "")
                {
                    lblMsg.Text = "Please Enter Note ";
                    txt_Note.Focus();
                    valid = false;
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        public void DisplayjournalGrid()
        {
            if (Rdn_Credit.Checked)
            {
                ddl_SiteName.Visible = false;
                lbl_SiteName.Visible = false;
                lbl_BillNo.Visible = false;
                chk_ServiceTax.Visible = true;
                ddl_ListOfBillNo.Visible = false;
                lbl_Amtallocated.Visible = true;
                txt_Amtallocated.Visible = true;
                lbl_ServiceTax.Visible = true;
                txt_ServiceTax.Visible = true;
                grdJournalEntry.Columns[1].Visible = true;
                grdJournalEntry.Columns[2].Visible = true;
                grdJournalEntry.Columns[3].Visible = true;
                grdJournalEntry.Columns[4].Visible = true;
                grdJournalEntry.Columns[5].Visible = true;
                grdJournalEntry.Columns[6].Visible = true;
                grdJournalEntry.Columns[8].Visible = true;
                grdJournalEntry.Columns[9].Visible = true;
                grdJournalEntry.Columns[0].Visible = false;
                grdJournalEntry.Columns[7].Visible = false;

            }
            else if (Rdn_Debit.Checked)
            {
                if (grdJournalEntry.Rows.Count <= 0)
                {
                    grdJournalEntry.DataSource = null;
                    grdJournalEntry.DataBind();
                    AddRowJournalEntry();
                }
                chk_ServiceTax.Visible = false;
                ddl_SiteName.Visible = true;
                lbl_SiteName.Visible = true;
                lbl_BillNo.Visible = true;
                ddl_ListOfBillNo.Visible = true;
                lbl_Amtallocated.Visible = false;
                txt_Amtallocated.Visible = false;
                lbl_ServiceTax.Visible = false;
                txt_ServiceTax.Visible = false;
                grdJournalEntry.Columns[1].Visible = false;
                grdJournalEntry.Columns[2].Visible = false;
                grdJournalEntry.Columns[3].Visible = false;
                grdJournalEntry.Columns[4].Visible = false;
                grdJournalEntry.Columns[8].Visible = false;
                grdJournalEntry.Columns[9].Visible = false;
                grdJournalEntry.Columns[0].Visible = true;
                grdJournalEntry.Columns[5].Visible = true;
                grdJournalEntry.Columns[6].Visible = true;
                grdJournalEntry.Columns[7].Visible = true;
                SumofTotalDebit();
            }
            setGrid();
        }
        public void setGrid()
        {

            for (int i = 0; i < grdJournalEntry.Rows.Count; i++)
            {
                DropDownList ddl_LedgerName = (DropDownList)grdJournalEntry.Rows[i].Cells[0].FindControl("ddl_LedgerName");
                TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[i].Cells[5].FindControl("txt_CostCenter");
                TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[i].Cells[6].FindControl("txt_CostCatagory");
                TextBox txt_Amount = (TextBox)grdJournalEntry.Rows[i].Cells[7].FindControl("txt_Amount");
                GridViewRow rowfooter = grdJournalEntry.FooterRow;
                TextBox txt_TotalAmt = ((TextBox)rowfooter.FindControl("txt_TotalAmt"));
                if (Rdn_Debit.Checked)
                {
                    ddl_LedgerName.Width = 300;
                    txt_CostCenter.Width = 200;
                    txt_CostCatagory.Width = 200;
                    txt_Amount.Width = 200;
                    txt_TotalAmt.Width = 200;
                }
                else
                {
                    ddl_LedgerName.Width = 100;
                    txt_CostCenter.Width = 100;
                    txt_CostCatagory.Width = 100;
                    txt_Amount.Width = 100;
                    txt_TotalAmt.Width = 100;
                }
            }
        }
        protected void AddRowJournalEntry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["JournalEntryTable"] != null)
            {
                GetCurrentDataJournalEntry();
                dt = (DataTable)ViewState["JournalEntryTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddl_LedgerName", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_BillLedger", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_BillDate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_BillAmt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_BalAmt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CostCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CostCatagory", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Debit", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Credit", typeof(string)));//
                dt.Columns.Add(new DataColumn("txt_Id", typeof(string)));
            }
            dr = dt.NewRow();

            dr["ddl_LedgerName"] = string.Empty;
            dr["txt_BillLedger"] = string.Empty;
            dr["txt_BillDate"] = string.Empty;
            dr["txt_BillAmt"] = string.Empty;
            dr["txt_BalAmt"] = string.Empty;
            dr["txt_CostCenter"] = string.Empty;
            dr["txt_CostCatagory"] = string.Empty;
            dr["txt_Amount"] = string.Empty;
            dr["txt_Debit"] = string.Empty;
            dr["txt_Credit"] = string.Empty;
            dr["txt_Id"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["JournalEntryTable"] = dt;
            grdJournalEntry.DataSource = dt;
            grdJournalEntry.DataBind();
            SetPreviousDataJournalEntry();
        }
        protected void SetPreviousDataJournalEntry()
        {
            DataTable dt = (DataTable)ViewState["JournalEntryTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddl_LedgerName = (DropDownList)grdJournalEntry.Rows[i].Cells[0].FindControl("ddl_LedgerName");
                TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[i].Cells[1].FindControl("txt_BillLedger");
                TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[i].Cells[2].FindControl("txt_BillDate");
                TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[i].Cells[3].FindControl("txt_BillAmt");
                TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[i].Cells[4].FindControl("txt_BalAmt");
                TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[i].Cells[5].FindControl("txt_CostCenter");
                TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[i].Cells[6].FindControl("txt_CostCatagory");
                TextBox txt_Amount = (TextBox)grdJournalEntry.Rows[i].Cells[7].FindControl("txt_Amount");
                TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[i].Cells[8].FindControl("txt_Debit");
                TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[i].Cells[9].FindControl("txt_Credit");
                TextBox txt_Id = (TextBox)grdJournalEntry.Rows[i].Cells[10].FindControl("txt_Id");

                ddl_LedgerName.Text = dt.Rows[i]["ddl_LedgerName"].ToString();
                txt_BillLedger.Text = dt.Rows[i]["txt_BillLedger"].ToString();
                txt_BillDate.Text = dt.Rows[i]["txt_BillDate"].ToString();
                txt_BillAmt.Text = dt.Rows[i]["txt_BillAmt"].ToString();
                txt_BalAmt.Text = dt.Rows[i]["txt_BalAmt"].ToString();
                txt_CostCenter.Text = dt.Rows[i]["txt_CostCenter"].ToString();
                txt_CostCatagory.Text = dt.Rows[i]["txt_CostCatagory"].ToString();
                txt_Amount.Text = dt.Rows[i]["txt_Amount"].ToString();
                txt_Debit.Text = dt.Rows[i]["txt_Debit"].ToString();
                txt_Credit.Text = dt.Rows[i]["txt_Credit"].ToString();
                txt_Id.Text = dt.Rows[i]["txt_Id"].ToString();
            }
        }
        protected void GetCurrentDataJournalEntry()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("ddl_LedgerName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_BillLedger", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_BillDate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_BillAmt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_BalAmt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CostCenter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CostCatagory", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Debit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Credit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Id", typeof(string)));

            for (int i = 0; i < grdJournalEntry.Rows.Count; i++)
            {
                DropDownList ddl_LedgerName = (DropDownList)grdJournalEntry.Rows[i].Cells[0].FindControl("ddl_LedgerName");
                TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[i].Cells[1].FindControl("txt_BillLedger");
                TextBox txt_BillDate = (TextBox)grdJournalEntry.Rows[i].Cells[2].FindControl("txt_BillDate");
                TextBox txt_BillAmt = (TextBox)grdJournalEntry.Rows[i].Cells[3].FindControl("txt_BillAmt");
                TextBox txt_BalAmt = (TextBox)grdJournalEntry.Rows[i].Cells[4].FindControl("txt_BalAmt");
                TextBox txt_CostCenter = (TextBox)grdJournalEntry.Rows[i].Cells[5].FindControl("txt_CostCenter");
                TextBox txt_CostCatagory = (TextBox)grdJournalEntry.Rows[i].Cells[6].FindControl("txt_CostCatagory");
                TextBox txt_Amount = (TextBox)grdJournalEntry.Rows[i].Cells[7].FindControl("txt_Amount");
                TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[i].Cells[8].FindControl("txt_Debit");
                TextBox txt_Credit = (TextBox)grdJournalEntry.Rows[i].Cells[9].FindControl("txt_Credit");
                TextBox txt_Id = (TextBox)grdJournalEntry.Rows[i].Cells[10].FindControl("txt_Id");

                drRow = dtTable.NewRow();
                drRow["ddl_LedgerName"] = ddl_LedgerName.Text;
                drRow["txt_BillLedger"] = txt_BillLedger.Text;
                drRow["txt_BillDate"] = txt_BillDate.Text;
                drRow["txt_BillAmt"] = txt_BillAmt.Text;
                drRow["txt_BalAmt"] = txt_BalAmt.Text;
                drRow["txt_CostCenter"] = txt_CostCenter.Text;
                drRow["txt_CostCatagory"] = txt_CostCatagory.Text;
                drRow["txt_Amount"] = txt_Amount.Text;
                drRow["txt_Debit"] = txt_Debit.Text;
                drRow["txt_Credit"] = txt_Credit.Text;
                drRow["txt_Id"] = txt_Id.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["JournalEntryTable"] = dtTable;
        }
        protected void Rdn_Debit_CheckedChanged(object sender, EventArgs e)
        {
            grdJournalEntry.DataSource = null;
            grdJournalEntry.DataBind();
            DisplayjournalGrid();
            SumofTotalDebit();
            cleartxt();
        }
        protected void Rdn_Credit_CheckedChanged(object sender, EventArgs e)
        {
            LoadCreditNoteDetails();
            DisplayjournalGrid();
            cleartxt();
        }
        private void cleartxt()
        {
            txt_DBNoteNo.Text = "";
            
        }
        protected void ddl_ClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdn_Debit.Checked)
            {
                LoadSiteList();
                DisplayjournalGrid();
            }
            else
            {
                //Bind_BillDetails();
                LoadCreditNoteDetails();
            }
        }
        protected void ddl_SiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBillList();
        }
        protected void chk_ServiceTax_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdJournalEntry.Rows.Count; i++)
            {
                TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[i].Cells[1].FindControl("txt_BillLedger");
                TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[i].Cells[8].FindControl("txt_Debit");
                if (chk_ServiceTax.Checked)
                {
                    if (txt_BillLedger.Text.Trim() == "Service Tax 10.30%")
                    {
                        txt_Debit.Text = txt_ServiceTax.Text;
                        break;
                    }
                }
                else
                {
                    if (txt_BillLedger.Text.Trim() == "Service Tax 10.30%")
                    {
                        txt_Debit.Text = "";
                    }
                }
            }
            SumOfTotalCredit();
        }
        public void DisplayChkServiceTax()
        {
             if (lblJournalNoteNo.Text != "")
            {
                for (int i = 0; i < grdJournalEntry.Rows.Count; i++)
                {
                    TextBox txt_BillLedger = (TextBox)grdJournalEntry.Rows[i].Cells[1].FindControl("txt_BillLedger");
                    TextBox txt_Debit = (TextBox)grdJournalEntry.Rows[i].Cells[8].FindControl("txt_Debit");

                    if (txt_BillLedger.Text.Trim() == "Service Tax 10.30%")
                    {
                        if (txt_Debit.Text != "")
                        {
                            if (Convert.ToDecimal(txt_Debit.Text) == Convert.ToDecimal(txt_ServiceTax.Text))
                            {
                                chk_ServiceTax.Checked = true;
                                break;
                            }
                            else
                            {
                                chk_ServiceTax.Checked = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
        //protected void lnk_Exit_Click(object sender, EventArgs e)
        //{
        //    object refUrl = ViewState["RefUrl"];
        //    if (refUrl != null)
        //    {
        //        Response.Redirect((string)refUrl);
        //    }
        //}
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
            if (Rdn_Debit.Checked)
            {
                LoadSiteList();
                DisplayjournalGrid();
            }
            else
            {
                //Bind_BillDetails();
                LoadCreditNoteDetails();
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

        protected void LnkNext_Click(object sender, EventArgs e)
        {
            txt_DBNoteNo.Text = "";
            lblJournalNoteNo.Text = "Create New...";
            LnkBtnSave.Enabled = true;
            Rdn_Debit.Enabled = true;
            Rdn_Credit.Enabled = true;
            Response.Redirect("JournalEntry.aspx");
         
            
        }

    }
}