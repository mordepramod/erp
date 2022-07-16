using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class BalanceTransfer : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                bool userRight = false;
                var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Account_right_bit == true)
                        userRight = true;
                }
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (userRight == true)
                {
                    lblheading.Text = "Balance Trasfer";
                    LoadTransferNoteList();
                    txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }
                else
                {
                    pnlContent.Visible = false;
                    lblheading.Text = "Balance Trasfer";
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }
        
        private void LoadTransferNoteList()
        {
            ddlReceiptNo.DataSource = null;
            ddlReceiptNo.DataBind();
            var note = dc.CashDetail_View_TrfNote("");
            ddlReceiptNo.DataTextField = "CashDetail_NoteNo_var";
            ddlReceiptNo.DataSource = note;
            ddlReceiptNo.DataBind();
            ddlReceiptNo.Items.Insert(0, "---New---");
        }

        private void ClearAllControls()
        {
            txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlStatus.SelectedValue = "0";
            txt_Client.Text = "";
            lblClientId.Text = "0";
            txtBillNo.Text = "";
            txtCrDrAmount.Text = "";
            grdCrDrEntry.DataSource = null;
            grdCrDrEntry.DataBind();
        }

        protected void ddlReceiptNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            if (ddlReceiptNo.SelectedIndex > 0)
            {
                int rowNo = 0;
                var cashDetail = dc.CashDetail_View_TrfNote(ddlReceiptNo.SelectedValue);
                foreach (var cashd in cashDetail)
                {
                    if (rowNo == 0)
                    {
                        txtDate.Text = Convert.ToDateTime(cashd.CashDetail_Date_date).ToString("dd/MM/yyyy");                        
                    }
                    AddRowCrDrEntry();
                    
                    Label ClientId = (Label)grdCrDrEntry.Rows[rowNo].FindControl("ClientId");
                    ClientId.Text = cashd.CashDetail_ClientId.ToString();
                    grdCrDrEntry.Rows[rowNo].Cells[2].Text = cashd.CL_Name_var;
                    grdCrDrEntry.Rows[rowNo].Cells[3].Text = cashd.CashDetail_BillNo_int.ToString();
                    if (cashd.CashDetail_Amount_money > 0)
                        grdCrDrEntry.Rows[rowNo].Cells[4].Text = Convert.ToDecimal(cashd.CashDetail_Amount_money).ToString("0.00");
                    else
                        grdCrDrEntry.Rows[rowNo].Cells[5].Text = Convert.ToDecimal((-1) * cashd.CashDetail_Amount_money).ToString("0.00");

                    rowNo++;
                }
            }
        }

        protected void imgBtnEditRow_Click(object sender, CommandEventArgs e)
        {
            if (grdCrDrEntry.Rows.Count > 0)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                Label ClientId = (Label)grdCrDrEntry.Rows[gvr.RowIndex].FindControl("ClientId");
                lblClientId.Text = ClientId.Text;
                txt_Client.Text = grdCrDrEntry.Rows[gvr.RowIndex].Cells[2].Text;
                txtBillNo.Text = grdCrDrEntry.Rows[gvr.RowIndex].Cells[3].Text;
                if (grdCrDrEntry.Rows[gvr.RowIndex].Cells[4].Text != "")
                {
                    txtCrDrAmount.Text = grdCrDrEntry.Rows[gvr.RowIndex].Cells[4].Text;
                    ddlCrDrAmount.SelectedValue = "Debit Amount"; 
                }
                else
                {
                    txtCrDrAmount.Text = grdCrDrEntry.Rows[gvr.RowIndex].Cells[5].Text;
                    ddlCrDrAmount.SelectedValue = "Credit Amount";
                }

                DeleteRowCrDrEntry(gvr.RowIndex);
            }
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdCrDrEntry.Rows.Count > 0)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;                
                DeleteRowCrDrEntry(gvr.RowIndex);
            }
        }
        protected void DeleteRowCrDrEntry(int rowIndex)
        {
            GetCurrentDataCrDrEntry();
            DataTable dt = ViewState["CrDrEntryTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CrDrEntryTable"] = dt;
            grdCrDrEntry.DataSource = dt;
            grdCrDrEntry.DataBind();
            SetPreviousDataCrDrEntry();
        }
        protected void AddRowCrDrEntry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CrDrEntryTable"] != null)
            {
                GetCurrentDataCrDrEntry();
                dt = (DataTable)ViewState["CrDrEntryTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ClientId", typeof(string)));
                dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
                dt.Columns.Add(new DataColumn("DebitAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("CreditAmount", typeof(string)));
            }
            dr = dt.NewRow();
            dr["ClientId"] = string.Empty; 
            dr["ClientName"] = string.Empty;
            dr["BillNo"] = string.Empty;
            dr["DebitAmount"] = string.Empty;
            dr["CreditAmount"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["CrDrEntryTable"] = dt;
            grdCrDrEntry.DataSource = dt;
            grdCrDrEntry.DataBind();
            SetPreviousDataCrDrEntry();
        }
        protected void GetCurrentDataCrDrEntry()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("ClientId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DebitAmount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CreditAmount", typeof(string)));

            for (int i = 0; i < grdCrDrEntry.Rows.Count; i++)
            {
                Label ClientId = (Label)grdCrDrEntry.Rows[i].FindControl("ClientId");
                drRow = dtTable.NewRow();
                drRow["ClientId"] = ClientId.Text;
                drRow["ClientName"] = grdCrDrEntry.Rows[i].Cells[2].Text;
                drRow["BillNo"] = grdCrDrEntry.Rows[i].Cells[3].Text;
                drRow["DebitAmount"] = grdCrDrEntry.Rows[i].Cells[4].Text;
                drRow["CreditAmount"] = grdCrDrEntry.Rows[i].Cells[5].Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CrDrEntryTable"] = dtTable;

        }
        protected void SetPreviousDataCrDrEntry()
        {
            DataTable dt = (DataTable)ViewState["CrDrEntryTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label ClientId = (Label)grdCrDrEntry.Rows[i].FindControl("ClientId");
                ClientId.Text = dt.Rows[i]["ClientId"].ToString();
                grdCrDrEntry.Rows[i].Cells[2].Text = dt.Rows[i]["ClientName"].ToString();
                grdCrDrEntry.Rows[i].Cells[3].Text = dt.Rows[i]["BillNo"].ToString();
                grdCrDrEntry.Rows[i].Cells[4].Text = dt.Rows[i]["DebitAmount"].ToString();
                grdCrDrEntry.Rows[i].Cells[5].Text = dt.Rows[i]["CreditAmount"].ToString();
            }
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            if (lblClientId.Text == "" || lblClientId.Text == "0")
            {
                lblMsg.Text = "Select Client..";
                lblMsg.Visible = true;
                txt_Client.Focus();
            }
            else if (txtCrDrAmount.Text == "")
            {
                lblMsg.Text = "Enter Amount..";
                lblMsg.Visible = true;
                txtCrDrAmount.Focus();
            }
            else if (Convert.ToDecimal(txtCrDrAmount.Text) == 0)
            {
                lblMsg.Text = "Enter Amount greater than zero..";
                lblMsg.Visible = true;
                txtCrDrAmount.Focus();
            }
            else
            {
                bool addFlag = true;
                for(int i=0; i<grdCrDrEntry.Rows.Count-1; i++)
                {
                    Label ClientIdTemp = (Label)grdCrDrEntry.Rows[i].FindControl("ClientId");
                    if (lblClientId.Text == ClientIdTemp.Text)
                    {
                        addFlag = false;
                        lblMsg.Text = "Client already added..";
                        break;
                    }
                }
                if (addFlag == true)
                {
                    AddRowCrDrEntry();
                    int rowNo = grdCrDrEntry.Rows.Count - 1;
                    Label ClientId = (Label)grdCrDrEntry.Rows[rowNo].FindControl("ClientId");
                    ClientId.Text = lblClientId.Text;
                    grdCrDrEntry.Rows[rowNo].Cells[2].Text = txt_Client.Text;
                    grdCrDrEntry.Rows[rowNo].Cells[3].Text = txtBillNo.Text;
                    if (ddlCrDrAmount.SelectedValue == "Debit Amount")
                        grdCrDrEntry.Rows[rowNo].Cells[4].Text = txtCrDrAmount.Text;
                    else
                        grdCrDrEntry.Rows[rowNo].Cells[5].Text = txtCrDrAmount.Text;
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            decimal totalAmt = 0;
            bool addFlag = true ;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            for (int i = 0; i < grdCrDrEntry.Rows.Count ; i++)
            {
                if (grdCrDrEntry.Rows[i].Cells[4].Text != "")
                {
                    totalAmt += Convert.ToDecimal(grdCrDrEntry.Rows[i].Cells[4].Text);
                }
                else if (grdCrDrEntry.Rows[i].Cells[5].Text != "")
                {
                    totalAmt -= Convert.ToDecimal(grdCrDrEntry.Rows[i].Cells[5].Text);
                }
            }
            if (grdCrDrEntry.Rows.Count == 0)
            {
                addFlag = false;
                lblMsg.Text = "Enter Credit and Debit amount..";
                lblMsg.Visible = true;
            }
            //else if (totalAmt != 0)
            //{
            //    addFlag = false;
            //    lblMsg.Text = "Credit amount total and debit amount total should be same..";
            //    lblMsg.Visible = true;
            //}
            else if (addFlag == true)
            {
                string NewrecNo = "";
                if (ddlReceiptNo.SelectedIndex == 0)
                {
                    clsData clsObj = new clsData();
                    NewrecNo = "TRF/" + clsObj.GetnUpdateRecordNo("TRF").ToString() ;
                }
                else
                {
                    NewrecNo = ddlReceiptNo.SelectedValue;
                    dc.CashDetail_Update(0, "", null, 0, "", 0, false, false, 0, NewrecNo, true, false);
                }
                for (int i = 0; i < grdCrDrEntry.Rows.Count ; i++)
                {
                    Label ClientId = (Label)grdCrDrEntry.Rows[i].FindControl("ClientId");
                    decimal amount = 0;                    
                    if (grdCrDrEntry.Rows[i].Cells[4].Text != "")
                        amount = Convert.ToDecimal(grdCrDrEntry.Rows[i].Cells[4].Text);
                    if (grdCrDrEntry.Rows[i].Cells[5].Text != "")
                        amount = (-1) * Convert.ToDecimal(grdCrDrEntry.Rows[i].Cells[5].Text);

                    dc.CashDetail_Update_TrfNote(NewrecNo, Convert.ToInt32(ClientId.Text), grdCrDrEntry.Rows[i].Cells[3].Text, DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null), amount, "On A/c", false);
                }
                ClearAllControls();
                LoadTransferNoteList(); 
                lblMsg.Text = "Saved successfully..";
            }

        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                else
                {
                    lblClientId.Text = "0";
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

    }
}