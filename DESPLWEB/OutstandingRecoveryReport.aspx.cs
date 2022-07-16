using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class OutstandingRecoveryReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Outstanding Report";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                
                bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_Account_right_bit == true)
                            userRight = true;
                    }
                    if (userRight == false)
                    {
                        pnlContent.Visible = false;
                        lblAccess.Visible = true;
                        lblAccess.Text = "Access is Denied.. ";
                    }
                }
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {   
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("BillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TestingType", typeof(string)));
            dt.Columns.Add(new DataColumn("BillAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PendingAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PendingAmountTillDate", typeof(string)));
            //dt.Columns.Add(new DataColumn("DaySpending", typeof(string)));
            //dt.Columns.Add(new DataColumn("MktUser", typeof(string)));
            //dt.Columns.Add(new DataColumn("Region", typeof(string)));
            //dt.Columns.Add(new DataColumn("Limit", typeof(string)));
            //dt.Columns.Add(new DataColumn("TotalOutstading", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedAmount", typeof(string)));
            //dt.Columns.Add(new DataColumn("OnAccount", typeof(string)));
            dt.Columns.Add(new DataColumn("SalesReversal", typeof(string)));
            dt.Columns.Add(new DataColumn("Discount", typeof(string)));
            dt.Columns.Add(new DataColumn("BadDebts", typeof(string)));
            //dt.Columns.Add(new DataColumn("CGST", typeof(string)));
            //dt.Columns.Add(new DataColumn("SGST", typeof(string)));
            dt.Columns.Add(new DataColumn("TDS", typeof(string)));
            //dt.Columns.Add(new DataColumn("ServiceTax", typeof(string)));
            dt.Columns.Add(new DataColumn("Other", typeof(string)));

            DateTime tempDate;
            int i = 0;
            decimal totalAmount = 0, totalOutstanding = 0, clientOutstanding = 0, clientOutstandingTillDate = 0, tempOutstanding = 0, tempOutstandingTillDate = 0, billOutstanding = 0, billOutstandingTillDate = 0, PendingAmount = 0, PendingAmountTillDate = 0;

            string[] strDate = txtFromDate.Text.Split('/');             
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strFromDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            
            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            clsData obj = new clsData();
            DataTable dtClient = obj.getClientOutstanding_Recovery(ClientId, strFromDate, strToDate);
            for (int cl = 0; cl < dtClient.Rows.Count; cl++)
            {
                clientOutstanding = Convert.ToDecimal(dtClient.Rows[cl]["billAmountAsOn"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmountAsOn"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmountAsOn"]);
                clientOutstandingTillDate = Convert.ToDecimal(dtClient.Rows[cl]["billAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmount"]);
                
                //bills
                billOutstanding = 0;
                var billList = dc.Bill_View_OutstandingRecovery(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, FromDate, ToDate).ToList();
                var dbList = dc.Journal_View_OutstandingDbNoteRecovery(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, FromDate, ToDate).ToList();
                foreach (var bill in billList)
                {
                    PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                    billOutstanding += PendingAmount;

                    PendingAmountTillDate = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmtTillDate);
                    billOutstandingTillDate += PendingAmountTillDate;
                }
                foreach (var db in dbList)
                {
                    PendingAmount = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmt);
                    billOutstanding += PendingAmount;

                    PendingAmountTillDate = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmtTillDate);
                    billOutstandingTillDate += PendingAmountTillDate;
                }

                tempOutstanding = billOutstanding - clientOutstanding;
                tempOutstandingTillDate = billOutstandingTillDate - clientOutstandingTillDate;

                if (tempOutstanding < 0)
                    tempOutstanding = 0;
                if (tempOutstandingTillDate < 0)
                    tempOutstandingTillDate = 0;
                foreach (var bill in billList)
                {
                    PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                    PendingAmountTillDate = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmtTillDate);
                    if (tempOutstanding == 0 || (tempOutstanding > 0 && tempOutstanding < PendingAmount))
                    {
                        i++;
                        tempDate = Convert.ToDateTime(bill.BILL_Date_dt);
                        dr1 = dt.NewRow();
                        dr1["SrNo"] = i;
                        dr1["ClientName"] = bill.CL_Name_var;
                        dr1["SiteName"] = bill.SITE_Name_var;
                        dr1["BillNo"] = bill.BILL_Id;
                        dr1["BillDate"] = tempDate.ToString("dd/MM/yyyy");
                        dr1["TestingType"] = bill.testtype;
                        dr1["BillAmount"] = bill.BILL_NetAmt_num;

                        PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                        if (tempOutstanding < PendingAmount && tempOutstanding > 0)
                        {
                            PendingAmount = PendingAmount - tempOutstanding;
                            tempOutstanding = 0;
                        }
                        totalAmount += Convert.ToDecimal(bill.BILL_NetAmt_num);
                        totalOutstanding += PendingAmount;
                        dr1["PendingAmount"] = PendingAmount.ToString("0.00");

                        //if (tempOutstandingTillDate == 0 || (tempOutstandingTillDate > 0 && tempOutstandingTillDate < PendingAmountTillDate))
                        //{
                        //    PendingAmountTillDate = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmtTillDate);
                        //    if (tempOutstandingTillDate < PendingAmountTillDate && tempOutstandingTillDate > 0)
                        //    {
                        //        PendingAmountTillDate = PendingAmountTillDate - tempOutstandingTillDate;
                        //        tempOutstandingTillDate = 0;
                        //    }
                        //    dr1["PendingAmountTillDate"] = PendingAmountTillDate.ToString("0.00");
                        //}
                        //else
                        //{
                        //    dr1["PendingAmountTillDate"] = "0.00";
                        //    tempOutstandingTillDate = tempOutstandingTillDate - PendingAmountTillDate;
                        //}
                        PendingAmountTillDate = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmtTillDate);
                        dr1["PendingAmountTillDate"] = PendingAmountTillDate.ToString("0.00");
                        //credit note details
                        decimal ReceivedAmount = 0, SalesReversal = 0, Discount = 0, BadDebts = 0, TDS = 0, Other = 0;
                        //CGST = 0, SGST = 0,  ServiceTax = 0, OnAccount = 0,
                        
                        var cashDetails = dc.CashDetail_View_bill(bill.BILL_Id);
                        foreach (var cashd in cashDetails)
                        {
                            DateTime cashDate = Convert.ToDateTime(cashd.CashDetail_Date_date);
                            if (cashDate >= FromDate && cashDate <= ToDate)
                            {
                                if (cashd.CashDetail_NoteNo_var.Contains("CR/") == true)
                                {
                                    bool ledgerFound = false;
                                    var jrnld = dc.JournalDetail_View(cashd.CashDetail_NoteNo_var, "", 0);
                                    foreach (var jd in jrnld)
                                    {
                                        if (jd.JournalDetail_LedgerId_int > 0)
                                        {
                                            if (jd.LedgerName_Description == "Bad Debts")
                                            {
                                                BadDebts += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }
                                            else if (jd.LedgerName_Description == "Brick Testing" || jd.LedgerName_Description == "Cement Testing"
                                                || jd.LedgerName_Description == "Core Testing" || jd.LedgerName_Description == "Coupon Sale - Cube Testing"
                                                || jd.LedgerName_Description == "Cube Testing Fees" || jd.LedgerName_Description == "Flyash"
                                                || jd.LedgerName_Description == "Masonry Blocks" || jd.LedgerName_Description == "Mix Design"
                                                || jd.LedgerName_Description == "NDT" || jd.LedgerName_Description == "Paving Blocks"
                                                || jd.LedgerName_Description == "Pile Testing" || jd.LedgerName_Description == "RAIN WATER HARVESTING"
                                                || jd.LedgerName_Description == "Soil Investigation" || jd.LedgerName_Description == "Steel Testing"
                                                || jd.LedgerName_Description == "Testing Fees" || jd.LedgerName_Description == "Tile Testing"
                                                || jd.LedgerName_Description == "WATER TESTING")
                                            {
                                                SalesReversal += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }

                                            //else if (jd.LedgerName_Description.Contains("CGST") == true)
                                            //{
                                            //    CGST += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                            //}
                                            //else if (jd.LedgerName_Description.Contains("SGST") == true)
                                            //{
                                            //    SGST += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                            //}
                                            //else if (jd.LedgerName_Description.Contains("On A/c") == true)
                                            //{
                                            //    OnAccount += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                            //}
                                            else if (jd.LedgerName_Description == "Discount")
                                            {
                                                Discount += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }
                                            //else if (jd.LedgerName_Description.Contains("Kisan Krishi Cess") == true || jd.LedgerName_Description.Contains("Service Tax") == true
                                            //    || jd.LedgerName_Description.Contains("Swachh Bharat Cess") == true)
                                            //{
                                            //    ServiceTax += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                            //}
                                            else if (jd.LedgerName_Description.Contains("TDS") == true)
                                            {
                                                TDS += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }
                                            //else
                                            //{
                                            //    Other += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                            //}

                                        }
                                    }
                                    if (ledgerFound == false)
                                    {
                                        Other += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                    }
                                }
                                else
                                {
                                    ReceivedAmount += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                }
                            }
                        }
                        if (ReceivedAmount != 0)
                            dr1["ReceivedAmount"] = (ReceivedAmount * -1).ToString("0.00");
                        //if (OnAccount != 0)
                        //    dr1["OnAccount"] = OnAccount.ToString("0.00");
                        if (SalesReversal != 0)
                            dr1["SalesReversal"] = (SalesReversal * -1).ToString("0.00");
                        if (Discount != 0)
                            dr1["Discount"] = (Discount * -1).ToString("0.00");
                        if (BadDebts != 0)
                            dr1["BadDebts"] = (BadDebts * -1).ToString("0.00");
                        //if (CGST != 0)
                        //    dr1["CGST"] = CGST.ToString("0.00");
                        //if (SGST != 0)
                        //    dr1["SGST"] = SGST.ToString("0.00");
                        if (TDS != 0)
                            dr1["TDS"] = (TDS * -1).ToString("0.00");
                        //if (ServiceTax != 0)
                        //    dr1["ServiceTax"] = ServiceTax.ToString("0.00");
                        if (Other != 0)
                            dr1["Other"] = (Other * -1).ToString("0.00");
                        //if (ReceivedAmount == 0 && SalesReversal == 0 && Discount == 0 && BadDebts == 0 && TDS == 0 && Other == 0)
                        //{
                        //    if (Convert.ToDecimal(dr1["PendingAmountTillDate"].ToString()) < Convert.ToDecimal(dr1["PendingAmount"].ToString()))
                        //        dr1["OnAccount"] = (Convert.ToDecimal(dr1["PendingAmount"].ToString()) - Convert.ToDecimal(dr1["PendingAmountTillDate"].ToString())).ToString("0.00");
                        //}
                        //
                        dt.Rows.Add(dr1);
                    }
                    else
                    {
                        tempOutstanding = tempOutstanding - PendingAmount;
                        tempOutstandingTillDate = tempOutstandingTillDate - PendingAmountTillDate;
                    }
                }
                //db Note
                foreach (var db in dbList)
                {
                    PendingAmount = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmt);
                    PendingAmountTillDate = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmtTillDate);
                    if (tempOutstanding == 0 || (tempOutstanding > 0 && tempOutstanding < PendingAmount))
                    {
                        i++;
                        tempDate = Convert.ToDateTime(db.Journal_Date_dt);
                        dr1 = dt.NewRow();
                        dr1["SrNo"] = i;
                        dr1["ClientName"] = db.CL_Name_var;
                        dr1["SiteName"] = db.SITE_Name_var;
                        dr1["BillNo"] = db.Journal_NoteNo_var;
                        dr1["BillDate"] = tempDate.ToString("dd/MM/yyyy");
                        dr1["TestingType"] = "";
                        dr1["BillAmount"] = db.Journal_Amount_dec;
                        
                        PendingAmount = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmt);
                        if (tempOutstanding < PendingAmount && tempOutstanding > 0)
                        {
                            PendingAmount = PendingAmount - tempOutstanding;
                            tempOutstanding = 0;
                        }
                        clientOutstanding = clientOutstanding - PendingAmount;

                        totalAmount += Convert.ToDecimal(db.Journal_Amount_dec);
                        totalOutstanding += PendingAmount;
                        dr1["PendingAmount"] = PendingAmount.ToString("0.00");

                        //if (tempOutstandingTillDate == 0 || (tempOutstandingTillDate > 0 && tempOutstandingTillDate < PendingAmountTillDate))
                        //{
                        //    PendingAmountTillDate = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmtTillDate);
                        //    if (tempOutstandingTillDate < PendingAmountTillDate && tempOutstandingTillDate > 0)
                        //    {
                        //        PendingAmountTillDate = PendingAmountTillDate - tempOutstandingTillDate;
                        //        tempOutstandingTillDate = 0;
                        //    }
                        //    clientOutstandingTillDate = clientOutstandingTillDate - PendingAmountTillDate;

                        //    dr1["PendingAmountTillDate"] = PendingAmountTillDate.ToString("0.00");
                        //}
                        //else
                        //{
                        //    dr1["PendingAmountTillDate"] = "0.00";
                        //    tempOutstandingTillDate = tempOutstandingTillDate - PendingAmountTillDate;
                        //}
                        PendingAmountTillDate = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmtTillDate);
                        dr1["PendingAmountTillDate"] = PendingAmountTillDate.ToString("0.00");
                        //credit note details
                        decimal ReceivedAmount = 0, SalesReversal = 0, Discount = 0, BadDebts = 0, TDS = 0, Other = 0;

                        var cashDetails = dc.CashDetail_View_bill(db.Journal_NoteNo_var);
                        foreach (var cashd in cashDetails)
                        {
                            DateTime cashDate = Convert.ToDateTime(cashd.CashDetail_Date_date);
                            if (cashDate >= FromDate && cashDate <= ToDate)
                            {
                                if (cashd.CashDetail_NoteNo_var.Contains("CR/") == true)
                                {
                                    bool ledgerFound = false;
                                    var jrnld = dc.JournalDetail_View(cashd.CashDetail_NoteNo_var, "", 0);
                                    foreach (var jd in jrnld)
                                    {
                                        if (jd.JournalDetail_LedgerId_int > 0)
                                        {
                                            if (jd.LedgerName_Description == "Bad Debts")
                                            {
                                                BadDebts += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }
                                            else if (jd.LedgerName_Description == "Brick Testing" || jd.LedgerName_Description == "Cement Testing"
                                                || jd.LedgerName_Description == "Core Testing" || jd.LedgerName_Description == "Coupon Sale - Cube Testing"
                                                || jd.LedgerName_Description == "Cube Testing Fees" || jd.LedgerName_Description == "Flyash"
                                                || jd.LedgerName_Description == "Masonry Blocks" || jd.LedgerName_Description == "Mix Design"
                                                || jd.LedgerName_Description == "NDT" || jd.LedgerName_Description == "Paving Blocks"
                                                || jd.LedgerName_Description == "Pile Testing" || jd.LedgerName_Description == "RAIN WATER HARVESTING"
                                                || jd.LedgerName_Description == "Soil Investigation" || jd.LedgerName_Description == "Steel Testing"
                                                || jd.LedgerName_Description == "Testing Fees" || jd.LedgerName_Description == "Tile Testing"
                                                || jd.LedgerName_Description == "WATER TESTING")
                                            {
                                                SalesReversal += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }
                                            else if (jd.LedgerName_Description == "Discount")
                                            {
                                                Discount += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }
                                            else if (jd.LedgerName_Description.Contains("TDS") == true)
                                            {
                                                TDS += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                                ledgerFound = true;
                                                break;
                                            }

                                        }
                                    }
                                    if (ledgerFound == false)
                                    {
                                        Other += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                    }
                                }
                                else
                                {
                                    ReceivedAmount += Convert.ToDecimal(cashd.CashDetail_Amount_money);
                                }
                            }
                        }
                        if (ReceivedAmount != 0)
                            dr1["ReceivedAmount"] = (ReceivedAmount * -1).ToString("0.00");                        
                        if (SalesReversal != 0)
                            dr1["SalesReversal"] = (SalesReversal * -1).ToString("0.00");
                        if (Discount != 0)
                            dr1["Discount"] = (Discount * -1).ToString("0.00");
                        if (BadDebts != 0)
                            dr1["BadDebts"] = (BadDebts * -1).ToString("0.00");                        
                        if (TDS != 0)
                            dr1["TDS"] = (TDS * -1).ToString("0.00");                        
                        if (Other != 0)
                            dr1["Other"] = (Other * -1).ToString("0.00");
                        //if (ReceivedAmount == 0 && SalesReversal == 0 && Discount == 0 && BadDebts == 0 && TDS == 0 && Other == 0)
                        //{
                        //    if (Convert.ToDecimal(dr1["PendingAmountTillDate"].ToString()) < Convert.ToDecimal(dr1["PendingAmount"].ToString()))
                        //        dr1["OnAccount"] = (Convert.ToDecimal(dr1["PendingAmount"].ToString()) - Convert.ToDecimal(dr1["PendingAmountTillDate"].ToString())).ToString("0.00");
                        //}
                        //
                        dt.Rows.Add(dr1);
                    }
                    else
                    {
                        tempOutstanding = tempOutstanding - PendingAmount;
                        tempOutstandingTillDate = tempOutstandingTillDate - PendingAmountTillDate;
                    }
                }
            }
            grdOutstanding.DataSource = dt;
            grdOutstanding.DataBind();
            Session["OutstandingDetails"] = dt;
            
            lblTotal.Text = "Total : " + totalAmount.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Pending As On Date : " + totalOutstanding.ToString("0.00");
            
        }
        
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdOutstanding.Rows.Count > 0 && grdOutstanding.Visible == true)
            {
                string Subheader = "";
                if (lblFromDate.Visible == true && txtFromDate.Visible == true)
                {
                    Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text;
                    if (chkClientSpecific.Checked == true)
                        Subheader += "|" + "Client" + "|" + txt_Client.Text;
                    else
                        Subheader += "|" + "" + "|" + "";
                }
                else
                {
                    Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text ;
                    if (chkClientSpecific.Checked == true)
                        Subheader += "|" + "Client" + "|" + txt_Client.Text;
                    else
                        Subheader += "|" + "" + "|" + "";
                }               
               
                PrintGrid.PrintGridView(grdOutstanding, Subheader, "Outstanding_Bills");
            }
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