using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;

namespace DESPLWEB
{
    public partial class RecoveryAging : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Recovery Aging Report";
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

            }
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("PendingDays", typeof(string)));
            dt.Columns.Add(new DataColumn("SubmittedNos", typeof(string)));
            dt.Columns.Add(new DataColumn("SubmittedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("NotReceivedAtSiteNos", typeof(string)));
            dt.Columns.Add(new DataColumn("NotReceivedAtSiteAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedAtSiteNos", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedAtSiteAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("NotPostedInAccountsNos", typeof(string)));
            dt.Columns.Add(new DataColumn("NotPostedInAccountsAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PostedInAccountsNos", typeof(string)));
            dt.Columns.Add(new DataColumn("PostedInAccountsAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("AckNotAvailableNos", typeof(string)));
            dt.Columns.Add(new DataColumn("AckNotAvailableAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("UnderReconciliationNos", typeof(string)));
            dt.Columns.Add(new DataColumn("UnderReconciliationAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("BlanksNos", typeof(string)));
            dt.Columns.Add(new DataColumn("BlanksAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalNos", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));


            decimal grandTotal_Submitted = 0, grandTotal_NotReceivedAtSite = 0, grandTotal_ReceivedAtSite = 0, grandTotal_NotPostedInAccounts = 0, grandTotal_PostedInAcc = 0, grandTotal_AckNotAvailable = 0, grandTotal_UnderRcn = 0, grandTotal_Blanks = 0, grandTotal_Total = 0;
            int noGrandTotal_Submitted = 0, noGrandTotal_NotReceivedAtSite = 0, noGrandTotal_ReceivedAtSite = 0, noGrandTotal_NotPostedInAccounts = 0, noGrandTotal_PostedInAcc = 0, noGrandTotal_AckNotAvailable = 0, noGrandTotal_UnderRcn = 0, noGrandTotal_Blanks = 0, noGrandTotal_Total = 0;
            
            decimal  grandTotal_Submitted180 = 0, grandTotal_NotReceivedAtSite180 = 0, grandTotal_ReceivedAtSite180 = 0, grandTotal_NotPostedInAccounts180 = 0, grandTotal_PostedInAcc180 = 0, grandTotal_AckNotAvailable180 = 0, grandTotal_UnderRcn180 = 0, grandTotal_Blanks180 = 0, grandTotal_Total180 = 0;
            int  noGrandTotal_Submitted180 = 0, noGrandTotal_NotReceivedAtSite180 = 0, noGrandTotal_ReceivedAtSite180 = 0, noGrandTotal_NotPostedInAccounts180 = 0, noGrandTotal_PostedInAcc180 = 0, noGrandTotal_AckNotAvailable180 = 0, noGrandTotal_UnderRcn180 = 0, noGrandTotal_Blanks180 = 0, noGrandTotal_Total180 = 0;

            decimal total_0_30_Submitted = 0, total_31_60_Submitted = 0, total_61_90_Submitted = 0, total_91_120_Submitted = 0, total_121_180_Submitted = 0,
                    total_181_270_Submitted = 0, total_271_365_Submitted = 0, total_366_730_Submitted = 0, total_731_Above_Submitted = 0, total_180_Less_Submitted = 0, total_180_Greater_Submitted = 0;

            decimal total_0_30_NotReceivedAtSite = 0, total_31_60_NotReceivedAtSite = 0, total_61_90_NotReceivedAtSite = 0, total_91_120_NotReceivedAtSite = 0, total_121_180_NotReceivedAtSite = 0,
                    total_181_270_NotReceivedAtSite = 0, total_271_365_NotReceivedAtSite = 0, total_366_730_NotReceivedAtSite = 0, total_731_Above_NotReceivedAtSite = 0, total_180_Less_NotReceivedAtSite = 0, total_180_Greater_NotReceivedAtSite = 0;

            decimal total_0_30_ReceivedAtSite = 0, total_31_60_ReceivedAtSite = 0, total_61_90_ReceivedAtSite = 0, total_91_120_ReceivedAtSite = 0, total_121_180_ReceivedAtSite = 0,
                    total_181_270_ReceivedAtSite = 0, total_271_365_ReceivedAtSite = 0, total_366_730_ReceivedAtSite = 0, total_731_Above_ReceivedAtSite = 0, total_180_Less_ReceivedAtSite = 0, total_180_Greater_ReceivedAtSite = 0;

            decimal total_0_30_NotPostedInAccounts = 0, total_31_60_NotPostedInAccounts = 0, total_61_90_NotPostedInAccounts = 0, total_91_120_NotPostedInAccounts = 0, total_121_180_NotPostedInAccounts = 0,
                    total_181_270_NotPostedInAccounts = 0, total_271_365_NotPostedInAccounts = 0, total_366_730_NotPostedInAccounts = 0, total_731_Above_NotPostedInAccounts = 0, total_180_Less_NotPostedInAccounts = 0, total_180_Greater_NotPostedInAccounts = 0;

            decimal total_0_30_PostedInAcc = 0, total_31_60_PostedInAcc = 0, total_61_90_PostedInAcc = 0, total_91_120_PostedInAcc = 0, total_121_180_PostedInAcc = 0,
                    total_181_270_PostedInAcc = 0, total_271_365_PostedInAcc = 0, total_366_730_PostedInAcc = 0, total_731_Above_PostedInAcc = 0, total_180_Less_PostedInAcc = 0, total_180_Greater_PostedInAcc = 0;

            decimal total_0_30_AckNotAvailable = 0, total_31_60_AckNotAvailable = 0, total_61_90_AckNotAvailable = 0, total_91_120_AckNotAvailable = 0, total_121_180_AckNotAvailable = 0,
                    total_181_270_AckNotAvailable = 0, total_271_365_AckNotAvailable = 0, total_366_730_AckNotAvailable = 0, total_731_Above_AckNotAvailable = 0, total_180_Less_AckNotAvailable = 0, total_180_Greater_AckNotAvailable = 0;

            decimal total_0_30_UnderRcn = 0, total_31_60_UnderRcn = 0, total_61_90_UnderRcn = 0, total_91_120_UnderRcn = 0, total_121_180_UnderRcn = 0,
                    total_181_270_UnderRcn = 0, total_271_365_UnderRcn = 0, total_366_730_UnderRcn = 0, total_731_Above_UnderRcn = 0, total_180_Less_UnderRcn = 0, total_180_Greater_UnderRcn = 0;

            decimal total_0_30_Blanks = 0, total_31_60_Blanks = 0, total_61_90_Blanks = 0, total_91_120_Blanks = 0, total_121_180_Blanks = 0,
                    total_181_270_Blanks = 0, total_271_365_Blanks = 0, total_366_730_Blanks = 0, total_731_Above_Blanks = 0, total_180_Less_Blanks = 0, total_180_Greater_Blanks = 0;

            decimal total_0_30_Total = 0, total_31_60_Total = 0, total_61_90_Total = 0, total_91_120_Total = 0, total_121_180_Total = 0,
                    total_181_270_Total = 0, total_271_365_Total = 0, total_366_730_Total = 0, total_731_Above_Total = 0, total_180_Less_Total = 0, total_180_Greater_Total = 0;

            int no_0_30_Submitted = 0, no_31_60_Submitted = 0, no_61_90_Submitted = 0, no_91_120_Submitted = 0, no_121_180_Submitted = 0,
                    no_181_270_Submitted = 0, no_271_365_Submitted = 0, no_366_730_Submitted = 0, no_731_Above_Submitted = 0, no_180_Less_Submitted = 0, no_180_Greater_Submitted = 0;

            int no_0_30_NotReceivedAtSite = 0, no_31_60_NotReceivedAtSite = 0, no_61_90_NotReceivedAtSite = 0, no_91_120_NotReceivedAtSite = 0, no_121_180_NotReceivedAtSite = 0,
                    no_181_270_NotReceivedAtSite = 0, no_271_365_NotReceivedAtSite = 0, no_366_730_NotReceivedAtSite = 0, no_731_Above_NotReceivedAtSite = 0, no_180_Less_NotReceivedAtSite = 0, no_180_Greater_NotReceivedAtSite = 0;

            int no_0_30_ReceivedAtSite = 0, no_31_60_ReceivedAtSite = 0, no_61_90_ReceivedAtSite = 0, no_91_120_ReceivedAtSite = 0, no_121_180_ReceivedAtSite = 0,
                    no_181_270_ReceivedAtSite = 0, no_271_365_ReceivedAtSite = 0, no_366_730_ReceivedAtSite = 0, no_731_Above_ReceivedAtSite = 0, no_180_Less_ReceivedAtSite = 0, no_180_Greater_ReceivedAtSite = 0;
            
            int no_0_30_NotPostedInAccounts = 0, no_31_60_NotPostedInAccounts = 0, no_61_90_NotPostedInAccounts = 0, no_91_120_NotPostedInAccounts = 0, no_121_180_NotPostedInAccounts = 0,
                    no_181_270_NotPostedInAccounts = 0, no_271_365_NotPostedInAccounts = 0, no_366_730_NotPostedInAccounts = 0, no_731_Above_NotPostedInAccounts = 0, no_180_Less_NotPostedInAccounts = 0, no_180_Greater_NotPostedInAccounts = 0;

            int no_0_30_PostedInAcc = 0, no_31_60_PostedInAcc = 0, no_61_90_PostedInAcc = 0, no_91_120_PostedInAcc = 0, no_121_180_PostedInAcc = 0,
                    no_181_270_PostedInAcc = 0, no_271_365_PostedInAcc = 0, no_366_730_PostedInAcc = 0, no_731_Above_PostedInAcc = 0, no_180_Less_PostedInAcc = 0, no_180_Greater_PostedInAcc = 0;

            int no_0_30_AckNotAvailable = 0, no_31_60_AckNotAvailable = 0, no_61_90_AckNotAvailable = 0, no_91_120_AckNotAvailable = 0, no_121_180_AckNotAvailable = 0,
                    no_181_270_AckNotAvailable = 0, no_271_365_AckNotAvailable = 0, no_366_730_AckNotAvailable = 0, no_731_Above_AckNotAvailable = 0, no_180_Less_AckNotAvailable = 0, no_180_Greater_AckNotAvailable = 0;

            int no_0_30_UnderRcn = 0, no_31_60_UnderRcn = 0, no_61_90_UnderRcn = 0, no_91_120_UnderRcn = 0, no_121_180_UnderRcn = 0,
                    no_181_270_UnderRcn = 0, no_271_365_UnderRcn = 0, no_366_730_UnderRcn = 0, no_731_Above_UnderRcn = 0, no_180_Less_UnderRcn = 0, no_180_Greater_UnderRcn = 0;

            int no_0_30_Blanks = 0, no_31_60_Blanks = 0, no_61_90_Blanks = 0, no_91_120_Blanks = 0, no_121_180_Blanks = 0,
                    no_181_270_Blanks = 0, no_271_365_Blanks = 0, no_366_730_Blanks = 0, no_731_Above_Blanks = 0, no_180_Less_Blanks = 0, no_180_Greater_Blanks = 0;

            int no_0_30_Total = 0, no_31_60_Total = 0, no_61_90_Total = 0, no_91_120_Total = 0, no_121_180_Total = 0,
                    no_181_270_Total = 0, no_271_365_Total = 0, no_366_730_Total = 0, no_731_Above_Total = 0, no_180_Less_Total = 0, no_180_Greater_Total = 0;

            string[] strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];

            //outstanding
            decimal totalAmount = 0, totalOutstanding = 0, clientOutstanding = 0, tempOutstanding = 0, billOutstanding = 0, PendingAmount = 0;
            clsData obj = new clsData();
            DataTable dtClient = obj.getClientOutstanding(0, strToDate, strToDate, 1);
            for (int cl = 0; cl < dtClient.Rows.Count; cl++)
            {
                clientOutstanding = Convert.ToDecimal(dtClient.Rows[cl]["billAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmount"]);
                //bills
                billOutstanding = 0;
                var billList = dc.Bill_View_Outstanding(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, ToDate, ToDate, true).ToList();
                var dbList = dc.Journal_View_OutstandingDbNote(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, ToDate, ToDate, true).ToList();
                foreach (var bill in billList)
                {
                    PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                    billOutstanding += PendingAmount;
                }
                foreach (var db in dbList)
                {
                    PendingAmount = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmt);
                    billOutstanding += PendingAmount;
                }
                tempOutstanding = billOutstanding - clientOutstanding;
                if (tempOutstanding < 0)
                    tempOutstanding = 0;
                foreach (var bill in billList)
                {
                    PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                    if (tempOutstanding == 0 || (tempOutstanding > 0 && tempOutstanding < PendingAmount))
                    {   
                        PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                        if (tempOutstanding < PendingAmount && tempOutstanding > 0)
                        {
                            PendingAmount = PendingAmount - tempOutstanding;
                            tempOutstanding = 0;
                        }
                        clientOutstanding = clientOutstanding - PendingAmount;
                        totalAmount += Convert.ToDecimal(bill.BILL_NetAmt_num);
                        totalOutstanding += PendingAmount;
                        int daysPending = Convert.ToInt32((DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null) - Convert.ToDateTime(bill.BILL_Date_dt)).Days);
                        #region 0-30 day
                        if (daysPending <= 30)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_0_30_Submitted += PendingAmount;
                                no_0_30_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_0_30_NotReceivedAtSite += PendingAmount;
                                no_0_30_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_0_30_ReceivedAtSite += PendingAmount;
                                no_0_30_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_0_30_NotPostedInAccounts += PendingAmount;
                                no_0_30_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_0_30_PostedInAcc += PendingAmount; //Convert.ToDecimal(bill.RECV_BillBookedAmt_num);
                                no_0_30_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_0_30_AckNotAvailable += PendingAmount;
                                no_0_30_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_0_30_UnderRcn += PendingAmount;
                                no_0_30_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_0_30_Blanks += PendingAmount;
                                no_0_30_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }
                            total_0_30_Total += PendingAmount;
                            no_0_30_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;
                            
                        }
                        #endregion
                        #region 31-60 day
                        else if (daysPending >= 31 && daysPending <= 60)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_31_60_Submitted += PendingAmount;
                                no_31_60_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_31_60_NotReceivedAtSite += PendingAmount;
                                no_31_60_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_31_60_ReceivedAtSite += PendingAmount;
                                no_31_60_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }                           
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_31_60_NotPostedInAccounts += PendingAmount;
                                no_31_60_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_31_60_PostedInAcc += PendingAmount;
                                no_31_60_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_31_60_AckNotAvailable += PendingAmount;
                                no_31_60_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_31_60_UnderRcn += PendingAmount;
                                no_31_60_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_31_60_Blanks += PendingAmount;
                                no_31_60_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }
                            total_31_60_Total += PendingAmount;
                            no_31_60_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;                            
                        }
                        #endregion
                        #region 61-90 day
                        else if (daysPending >= 61 && daysPending <= 90)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_61_90_Submitted += PendingAmount;
                                no_61_90_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_61_90_NotReceivedAtSite += PendingAmount;
                                no_61_90_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_61_90_ReceivedAtSite += PendingAmount;
                                no_61_90_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }                            
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_61_90_NotPostedInAccounts += PendingAmount;
                                no_61_90_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_61_90_PostedInAcc += PendingAmount;
                                no_61_90_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_61_90_AckNotAvailable += PendingAmount;
                                no_61_90_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_61_90_UnderRcn += PendingAmount;
                                no_61_90_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_61_90_Blanks += PendingAmount;
                                no_61_90_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }
                            total_61_90_Total += PendingAmount;
                            no_61_90_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;
                        }
                        #endregion
                        #region 91-120 day
                        else if (daysPending >= 91 && daysPending <= 120)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_91_120_Submitted += PendingAmount;
                                no_91_120_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_91_120_NotReceivedAtSite += PendingAmount;
                                no_91_120_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_91_120_ReceivedAtSite += PendingAmount;
                                no_91_120_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_91_120_NotPostedInAccounts += PendingAmount;
                                no_91_120_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_91_120_PostedInAcc += PendingAmount;
                                no_91_120_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_91_120_AckNotAvailable += PendingAmount;
                                no_91_120_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_91_120_UnderRcn += PendingAmount;
                                no_91_120_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_91_120_Blanks += PendingAmount;
                                no_91_120_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }
                            total_91_120_Total += PendingAmount;
                            no_91_120_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;
                        }
                        #endregion
                        #region 121-180 day
                        else if (daysPending >= 121 && daysPending <= 180)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_121_180_Submitted += PendingAmount;
                                no_121_180_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_121_180_NotReceivedAtSite += PendingAmount;
                                no_121_180_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_121_180_ReceivedAtSite += PendingAmount;
                                no_121_180_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_121_180_NotPostedInAccounts += PendingAmount;
                                no_121_180_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_121_180_PostedInAcc += PendingAmount;
                                no_121_180_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_121_180_AckNotAvailable += PendingAmount;
                                no_121_180_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_121_180_UnderRcn += PendingAmount;
                                no_121_180_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_121_180_Blanks += PendingAmount;
                                no_121_180_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }
                            total_121_180_Total += PendingAmount;
                            no_121_180_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;
                        }
                        #endregion
                        #region 181-270 day
                        else if (daysPending >= 181 && daysPending <= 270)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_181_270_Submitted += PendingAmount;
                                no_181_270_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_181_270_NotReceivedAtSite += PendingAmount;
                                no_181_270_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_181_270_ReceivedAtSite += PendingAmount;
                                no_181_270_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }                            
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_181_270_NotPostedInAccounts += PendingAmount;
                                no_181_270_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_181_270_PostedInAcc += PendingAmount;
                                no_181_270_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_181_270_AckNotAvailable += PendingAmount;
                                no_181_270_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_181_270_UnderRcn += PendingAmount;
                                no_181_270_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_181_270_Blanks += PendingAmount;
                                no_181_270_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }                            
                            total_181_270_Total += PendingAmount;
                            no_181_270_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;                            
                        }
                        #endregion
                        #region 271-365 day
                        else if (daysPending >= 271 && daysPending <= 365)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_271_365_Submitted += PendingAmount;
                                no_271_365_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_271_365_NotReceivedAtSite += PendingAmount;
                                no_271_365_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_271_365_ReceivedAtSite += PendingAmount;
                                no_271_365_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }                            
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_271_365_NotPostedInAccounts += PendingAmount;
                                no_271_365_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_271_365_PostedInAcc += PendingAmount;
                                no_271_365_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_271_365_AckNotAvailable += PendingAmount;
                                no_271_365_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_271_365_UnderRcn += PendingAmount;
                                no_271_365_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_271_365_Blanks += PendingAmount;
                                no_271_365_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }                            
                            total_271_365_Total += PendingAmount;
                            no_271_365_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;                            
                        }
                        #endregion
                        #region 366-730 day
                        else if (daysPending >= 366 && daysPending <= 730)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_366_730_Submitted += PendingAmount;
                                no_366_730_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_366_730_NotReceivedAtSite += PendingAmount;
                                no_366_730_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_366_730_ReceivedAtSite += PendingAmount;
                                no_366_730_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_366_730_NotPostedInAccounts += PendingAmount;
                                no_366_730_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_366_730_PostedInAcc += PendingAmount;
                                no_366_730_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_366_730_AckNotAvailable += PendingAmount;
                                no_366_730_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_366_730_UnderRcn += PendingAmount;
                                no_366_730_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_366_730_Blanks += PendingAmount;
                                no_366_730_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }
                            total_366_730_Total += PendingAmount;
                            no_366_730_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;                           
                        }
                        #endregion
                        #region More than 730 day
                        else if (daysPending >= 731)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_731_Above_Submitted += PendingAmount;
                                no_731_Above_Submitted++;
                                noGrandTotal_Submitted++;
                                grandTotal_Submitted += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_731_Above_NotReceivedAtSite += PendingAmount;
                                no_731_Above_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite++;
                                grandTotal_NotReceivedAtSite += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_731_Above_ReceivedAtSite += PendingAmount;
                                no_731_Above_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }                            
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_731_Above_NotPostedInAccounts += PendingAmount;
                                no_731_Above_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts++;
                                grandTotal_NotPostedInAccounts += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_731_Above_PostedInAcc += PendingAmount;
                                no_731_Above_PostedInAcc++;
                                noGrandTotal_PostedInAcc++;
                                grandTotal_PostedInAcc += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_731_Above_AckNotAvailable += PendingAmount;
                                no_731_Above_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable++;
                                grandTotal_AckNotAvailable += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_731_Above_UnderRcn += PendingAmount;
                                no_731_Above_UnderRcn++;
                                noGrandTotal_UnderRcn++;
                                grandTotal_UnderRcn += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_731_Above_Blanks += PendingAmount;
                                no_731_Above_Blanks++;
                                noGrandTotal_Blanks++;
                                grandTotal_Blanks += PendingAmount;
                            }                            
                            total_731_Above_Total += PendingAmount;
                            no_731_Above_Total++;
                            noGrandTotal_Total++;
                            grandTotal_Total += PendingAmount;                            
                        }
                        #endregion

                        #region less than 180 day
                        if (daysPending <= 180)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_180_Less_Submitted += PendingAmount;
                                no_180_Less_Submitted++;
                                noGrandTotal_Submitted180++;
                                grandTotal_Submitted180 += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_180_Less_NotReceivedAtSite += PendingAmount;
                                no_180_Less_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite180++;
                                grandTotal_NotReceivedAtSite180 += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_180_Less_ReceivedAtSite += PendingAmount;
                                no_180_Less_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }                            
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_180_Less_NotPostedInAccounts += PendingAmount;
                                no_180_Less_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts180++;
                                grandTotal_NotPostedInAccounts180 += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_180_Less_PostedInAcc += PendingAmount;
                                no_180_Less_PostedInAcc++;
                                noGrandTotal_PostedInAcc180++;
                                grandTotal_PostedInAcc180 += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_180_Less_AckNotAvailable += PendingAmount;
                                no_180_Less_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable180++;
                                grandTotal_AckNotAvailable180 += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_180_Less_UnderRcn += PendingAmount;
                                no_180_Less_UnderRcn++;
                                noGrandTotal_UnderRcn180++;
                                grandTotal_UnderRcn180 += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_180_Less_Blanks += PendingAmount;
                                no_180_Less_Blanks++;
                                noGrandTotal_Blanks180++;
                                grandTotal_Blanks180 += PendingAmount;
                            }                            
                            total_180_Less_Total += PendingAmount;
                            no_180_Less_Total++;
                            noGrandTotal_Total180++;
                            grandTotal_Total180 += PendingAmount;                            
                        }
                        #endregion
                        #region less than 180 day
                        if (daysPending > 180)
                        {
                            string recvStatus = bill.RECV_Status_var;
                            if (recvStatus == "Submitted")
                            {
                                total_180_Greater_Submitted += PendingAmount;
                                no_180_Greater_Submitted++;
                                noGrandTotal_Submitted180++;
                                grandTotal_Submitted180 += PendingAmount;
                            }
                            else if (recvStatus == "Not received at site")
                            {
                                total_180_Greater_NotReceivedAtSite += PendingAmount;
                                no_180_Greater_NotReceivedAtSite++;
                                noGrandTotal_NotReceivedAtSite180++;
                                grandTotal_NotReceivedAtSite180 += PendingAmount;
                            }
                            else if (recvStatus == "Received at site")
                            {
                                total_180_Greater_ReceivedAtSite += PendingAmount;
                                no_180_Greater_ReceivedAtSite++;
                                noGrandTotal_ReceivedAtSite++;
                                grandTotal_ReceivedAtSite += PendingAmount;
                            }                            
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_180_Greater_NotPostedInAccounts += PendingAmount;
                                no_180_Greater_NotPostedInAccounts++;
                                noGrandTotal_NotPostedInAccounts180++;
                                grandTotal_NotPostedInAccounts180 += PendingAmount;
                            }
                            else if (recvStatus == "Posted in accounts")
                            {
                                total_180_Greater_PostedInAcc += PendingAmount;
                                no_180_Greater_PostedInAcc++;
                                noGrandTotal_PostedInAcc180++;
                                grandTotal_PostedInAcc180 += PendingAmount;
                            }
                            else if (recvStatus == "Not posted in accounts")
                            {
                                total_180_Greater_AckNotAvailable += PendingAmount;
                                no_180_Greater_AckNotAvailable++;
                                noGrandTotal_AckNotAvailable180++;
                                grandTotal_AckNotAvailable180 += PendingAmount;
                            }
                            else if ((recvStatus == null || recvStatus == "")
                                    && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                            {
                                total_180_Greater_UnderRcn += PendingAmount;
                                no_180_Greater_UnderRcn++;
                                noGrandTotal_UnderRcn180++;
                                grandTotal_UnderRcn180 += PendingAmount;
                            }
                            else if (recvStatus == null || recvStatus == "")
                            {
                                total_180_Greater_Blanks += PendingAmount;
                                no_180_Greater_Blanks++;
                                noGrandTotal_Blanks180++;
                                grandTotal_Blanks180 += PendingAmount;
                            }                            
                            total_180_Greater_Total += PendingAmount;
                            no_180_Greater_Total++;
                            noGrandTotal_Total180++;
                            grandTotal_Total180 += PendingAmount;                            
                        }
                        #endregion
                    }
                    else
                    {
                        tempOutstanding = tempOutstanding - PendingAmount;
                    }
                }
            }

            ////
            #region aging grid 1
            dr1 = dt.NewRow();
            dr1["PendingDays"] = "0 - 30 day"; 
            dr1["SubmittedNos"] = no_0_30_Submitted.ToString();
            dr1["SubmittedAmount"] = total_0_30_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_0_30_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_0_30_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_0_30_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_0_30_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_0_30_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_0_30_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_0_30_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_0_30_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_0_30_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_0_30_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_0_30_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_0_30_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_0_30_Blanks.ToString();
            dr1["BlanksAmount"] = total_0_30_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_0_30_Total.ToString();
            dr1["TotalAmount"] = total_0_30_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "31 - 60 day"; 
            dr1["SubmittedNos"] = no_31_60_Submitted.ToString();
            dr1["SubmittedAmount"] = total_31_60_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_31_60_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_31_60_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_31_60_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_31_60_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_31_60_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_31_60_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_31_60_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_31_60_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_31_60_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_31_60_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_31_60_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_31_60_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_31_60_Blanks.ToString();
            dr1["BlanksAmount"] = total_31_60_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_31_60_Total.ToString();
            dr1["TotalAmount"] = total_31_60_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "61 - 90 day"; 
            dr1["SubmittedNos"] = no_61_90_Submitted.ToString();
            dr1["SubmittedAmount"] = total_61_90_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_61_90_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_61_90_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_61_90_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_61_90_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_61_90_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_61_90_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_61_90_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_61_90_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_61_90_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_61_90_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_61_90_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_61_90_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_61_90_Blanks.ToString();
            dr1["BlanksAmount"] = total_61_90_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_61_90_Total.ToString();
            dr1["TotalAmount"] = total_61_90_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "91 - 120 day";
            dr1["SubmittedNos"] = no_91_120_Submitted.ToString();
            dr1["SubmittedAmount"] = total_91_120_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_91_120_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_91_120_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_91_120_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_91_120_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_91_120_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_91_120_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_91_120_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_91_120_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_91_120_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_91_120_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_91_120_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_91_120_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_91_120_Blanks.ToString();
            dr1["BlanksAmount"] = total_91_120_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_91_120_Total.ToString();
            dr1["TotalAmount"] = total_91_120_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "121 - 180 day";
            dr1["SubmittedNos"] = no_121_180_Submitted.ToString();
            dr1["SubmittedAmount"] = total_121_180_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_121_180_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_121_180_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_121_180_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_121_180_ReceivedAtSite.ToString("0.00");            
            dr1["NotPostedInAccountsNos"] = no_121_180_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_121_180_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_121_180_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_121_180_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_121_180_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_121_180_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_121_180_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_121_180_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_121_180_Blanks.ToString();
            dr1["BlanksAmount"] = total_121_180_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_121_180_Total.ToString();
            dr1["TotalAmount"] = total_121_180_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "181 - 270 day";
            dr1["SubmittedNos"] = no_181_270_Submitted.ToString();
            dr1["SubmittedAmount"] = total_181_270_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_181_270_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_181_270_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_181_270_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_181_270_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_181_270_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_181_270_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_181_270_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_181_270_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_181_270_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_181_270_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_181_270_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_181_270_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_181_270_Blanks.ToString();
            dr1["BlanksAmount"] = total_181_270_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_181_270_Total.ToString();
            dr1["TotalAmount"] = total_181_270_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "271 - 365 day";
            dr1["SubmittedNos"] = no_271_365_Submitted.ToString();
            dr1["SubmittedAmount"] = total_271_365_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_271_365_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_271_365_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_271_365_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_271_365_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_271_365_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_271_365_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_271_365_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_271_365_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_271_365_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_271_365_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_271_365_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_271_365_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_271_365_Blanks.ToString();
            dr1["BlanksAmount"] = total_271_365_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_271_365_Total.ToString();
            dr1["TotalAmount"] = total_271_365_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "366 - 730 day";
            dr1["SubmittedNos"] = no_366_730_Submitted.ToString();
            dr1["SubmittedAmount"] = total_366_730_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_366_730_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_366_730_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_366_730_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_366_730_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_366_730_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_366_730_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_366_730_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_366_730_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_366_730_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_366_730_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_366_730_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_366_730_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_366_730_Blanks.ToString();
            dr1["BlanksAmount"] = total_366_730_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_366_730_Total.ToString();
            dr1["TotalAmount"] = total_366_730_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "More than 731 day";
            dr1["SubmittedNos"] = no_731_Above_Submitted.ToString();
            dr1["SubmittedAmount"] = total_731_Above_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_731_Above_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_731_Above_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_731_Above_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_731_Above_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_731_Above_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_731_Above_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_731_Above_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_731_Above_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_731_Above_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_731_Above_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_731_Above_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_731_Above_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_731_Above_Blanks.ToString();
            dr1["BlanksAmount"] = total_731_Above_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_731_Above_Total.ToString();
            dr1["TotalAmount"] = total_731_Above_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            grdRecovery.DataSource = dt;
            grdRecovery.DataBind();

            if (grdRecovery.Rows.Count > 0)
            {
                grdRecovery.FooterRow.BackColor = System.Drawing.Color.White;
                grdRecovery.FooterStyle.BackColor = System.Drawing.Color.White;
                for (int j = 0; j < grdRecovery.Columns.Count; j++)
                {
                    grdRecovery.FooterRow.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                    grdRecovery.FooterRow.Cells[j].Font.Bold = true;
                }

                grdRecovery.FooterRow.Cells[0].Text = "Grand Total";
                grdRecovery.FooterRow.Cells[1].Text = noGrandTotal_Submitted.ToString();
                grdRecovery.FooterRow.Cells[2].Text = grandTotal_Submitted.ToString("0.00");
                grdRecovery.FooterRow.Cells[3].Text = noGrandTotal_NotReceivedAtSite.ToString();
                grdRecovery.FooterRow.Cells[4].Text = grandTotal_NotReceivedAtSite.ToString("0.00");
                grdRecovery.FooterRow.Cells[5].Text = noGrandTotal_ReceivedAtSite.ToString();
                grdRecovery.FooterRow.Cells[6].Text = grandTotal_ReceivedAtSite.ToString("0.00");
                grdRecovery.FooterRow.Cells[7].Text = noGrandTotal_NotPostedInAccounts.ToString();
                grdRecovery.FooterRow.Cells[8].Text = grandTotal_NotPostedInAccounts.ToString("0.00");
                grdRecovery.FooterRow.Cells[9].Text = noGrandTotal_PostedInAcc.ToString();
                grdRecovery.FooterRow.Cells[10].Text = grandTotal_PostedInAcc.ToString("0.00");
                grdRecovery.FooterRow.Cells[11].Text = noGrandTotal_AckNotAvailable.ToString();
                grdRecovery.FooterRow.Cells[12].Text = grandTotal_AckNotAvailable.ToString("0.00");
                grdRecovery.FooterRow.Cells[13].Text = noGrandTotal_UnderRcn.ToString();
                grdRecovery.FooterRow.Cells[14].Text = grandTotal_UnderRcn.ToString("0.00");
                grdRecovery.FooterRow.Cells[15].Text = noGrandTotal_Blanks.ToString();
                grdRecovery.FooterRow.Cells[16].Text = grandTotal_Blanks.ToString("0.00");
                grdRecovery.FooterRow.Cells[17].Text = noGrandTotal_Total.ToString();
                grdRecovery.FooterRow.Cells[18].Text = grandTotal_Total.ToString("0.00");
            }
            #endregion

            #region aging grid 2
            dt.Clear();
            dr1 = dt.NewRow();
            dr1["PendingDays"] = "Less than 180 days";
            dr1["SubmittedNos"] = no_180_Less_Submitted.ToString();
            dr1["SubmittedAmount"] = total_180_Less_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_180_Less_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_180_Less_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_180_Less_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_180_Less_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_180_Less_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_180_Less_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_180_Less_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_180_Less_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_180_Less_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_180_Less_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_180_Less_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_180_Less_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_180_Less_Blanks.ToString();
            dr1["BlanksAmount"] = total_180_Less_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_180_Less_Total.ToString();
            dr1["TotalAmount"] = total_180_Less_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "More than 180 days";
            dr1["SubmittedNos"] = no_180_Greater_Submitted.ToString();
            dr1["SubmittedAmount"] = total_180_Greater_Submitted.ToString("0.00");
            dr1["NotReceivedAtSiteNos"] = no_180_Greater_NotReceivedAtSite.ToString();
            dr1["NotReceivedAtSiteAmount"] = total_180_Greater_NotReceivedAtSite.ToString("0.00");
            dr1["ReceivedAtSiteNos"] = no_180_Greater_ReceivedAtSite.ToString();
            dr1["ReceivedAtSiteAmount"] = total_180_Greater_ReceivedAtSite.ToString("0.00");
            dr1["NotPostedInAccountsNos"] = no_180_Greater_NotPostedInAccounts.ToString();
            dr1["NotPostedInAccountsAmount"] = total_180_Greater_NotPostedInAccounts.ToString("0.00");
            dr1["PostedInAccountsNos"] = no_180_Greater_PostedInAcc.ToString();
            dr1["PostedInAccountsAmount"] = total_180_Greater_PostedInAcc.ToString("0.00");
            dr1["AckNotAvailableNos"] = no_180_Greater_AckNotAvailable.ToString();
            dr1["AckNotAvailableAmount"] = total_180_Greater_AckNotAvailable.ToString("0.00");
            dr1["UnderReconciliationNos"] = no_180_Greater_UnderRcn.ToString();
            dr1["UnderReconciliationAmount"] = total_180_Greater_UnderRcn.ToString("0.00");
            dr1["BlanksNos"] = no_180_Greater_Blanks.ToString();
            dr1["BlanksAmount"] = total_180_Greater_Blanks.ToString("0.00");
            dr1["TotalNos"] = no_180_Greater_Total.ToString();
            dr1["TotalAmount"] = total_180_Greater_Total.ToString("0.00");
            dt.Rows.Add(dr1);

            grdRecovery2.DataSource = dt;
            grdRecovery2.DataBind();

            if (grdRecovery2.Rows.Count > 0)
            {
                grdRecovery2.FooterRow.BackColor = System.Drawing.Color.White;
                grdRecovery2.FooterStyle.BackColor = System.Drawing.Color.White;
                for (int j = 0; j < grdRecovery2.Columns.Count; j++)
                {
                    grdRecovery2.FooterRow.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                    grdRecovery2.FooterRow.Cells[j].Font.Bold = true;
                }

                grdRecovery2.FooterRow.Cells[0].Text = "Grand Total";
                grdRecovery2.FooterRow.Cells[1].Text = noGrandTotal_Submitted180.ToString();
                grdRecovery2.FooterRow.Cells[2].Text = grandTotal_Submitted180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[3].Text = noGrandTotal_NotReceivedAtSite180.ToString();
                grdRecovery2.FooterRow.Cells[4].Text = grandTotal_NotReceivedAtSite180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[5].Text = noGrandTotal_ReceivedAtSite180.ToString();
                grdRecovery2.FooterRow.Cells[6].Text = grandTotal_ReceivedAtSite180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[7].Text = noGrandTotal_NotPostedInAccounts180.ToString();
                grdRecovery2.FooterRow.Cells[8].Text = grandTotal_NotPostedInAccounts180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[9].Text = noGrandTotal_PostedInAcc180.ToString();
                grdRecovery2.FooterRow.Cells[10].Text = grandTotal_PostedInAcc180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[11].Text = noGrandTotal_AckNotAvailable180.ToString();
                grdRecovery2.FooterRow.Cells[12].Text = grandTotal_AckNotAvailable180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[13].Text = noGrandTotal_UnderRcn180.ToString();
                grdRecovery2.FooterRow.Cells[14].Text = grandTotal_UnderRcn180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[15].Text = noGrandTotal_Blanks180.ToString();
                grdRecovery2.FooterRow.Cells[16].Text = grandTotal_Blanks180.ToString("0.00");
                grdRecovery2.FooterRow.Cells[17].Text = noGrandTotal_Total180.ToString();
                grdRecovery2.FooterRow.Cells[18].Text = grandTotal_Total180.ToString("0.00");
            }
            #endregion

            #region aging grid 3
            dt.Clear();
            dr1 = dt.NewRow();
            dr1["PendingDays"] = "Less than 180 days";
            if (noGrandTotal_Submitted180 > 0)
                dr1["SubmittedNos"] = Math.Round((Convert.ToDecimal(no_180_Less_Submitted) * 100) / Convert.ToDecimal(noGrandTotal_Submitted180)).ToString() + "%";
            if (grandTotal_Submitted180 > 0)
                dr1["SubmittedAmount"] = Math.Round((total_180_Less_Submitted * 100) / grandTotal_Submitted180).ToString() + "%";
            if (noGrandTotal_NotReceivedAtSite180 > 0)
                dr1["NotReceivedAtSiteNos"] = Math.Round((Convert.ToDecimal(no_180_Less_NotReceivedAtSite) * 100) / Convert.ToDecimal(noGrandTotal_NotReceivedAtSite180)).ToString() + "%";
            if (grandTotal_NotReceivedAtSite180 > 0)
                dr1["NotReceivedAtSiteAmount"] = Math.Round((total_180_Less_NotReceivedAtSite * 100) / grandTotal_NotReceivedAtSite180).ToString() + "%";
            if (noGrandTotal_ReceivedAtSite180 > 0)
                dr1["ReceivedAtSiteNos"] = Math.Round((Convert.ToDecimal(no_180_Less_ReceivedAtSite) * 100) / Convert.ToDecimal(noGrandTotal_ReceivedAtSite180)).ToString() + "%";
            if (grandTotal_ReceivedAtSite180 > 0)
                dr1["ReceivedAtSiteAmount"] = Math.Round((total_180_Less_ReceivedAtSite * 100) / grandTotal_ReceivedAtSite180).ToString() + "%";
            if (noGrandTotal_NotPostedInAccounts180 > 0)
                dr1["NotPostedInAccountsNos"] = Math.Round((Convert.ToDecimal(no_180_Less_NotPostedInAccounts) * 100) / Convert.ToDecimal(noGrandTotal_NotPostedInAccounts180)).ToString() + "%";
            if (grandTotal_NotPostedInAccounts180 > 0)
                dr1["NotPostedInAccountsAmount"] = Math.Round((total_180_Less_NotPostedInAccounts * 100) / grandTotal_NotPostedInAccounts180).ToString() + "%";
            if (noGrandTotal_PostedInAcc180 > 0)
                dr1["PostedInAccountsNos"] = Math.Round((Convert.ToDecimal(no_180_Less_PostedInAcc) * 100) / Convert.ToDecimal(noGrandTotal_PostedInAcc180)).ToString() + "%";
            if (grandTotal_PostedInAcc180 > 0)
                dr1["PostedInAccountsAmount"] = Math.Round((total_180_Less_PostedInAcc * 100) / grandTotal_PostedInAcc180).ToString() + "%";
            if (noGrandTotal_AckNotAvailable180 > 0)
                dr1["AckNotAvailableNos"] = Math.Round((Convert.ToDecimal(no_180_Less_AckNotAvailable) * 100) / Convert.ToDecimal(noGrandTotal_AckNotAvailable180)).ToString() + "%";
            if (grandTotal_AckNotAvailable180 > 0)
                dr1["AckNotAvailableAmount"] = Math.Round((total_180_Less_AckNotAvailable * 100) / grandTotal_AckNotAvailable180).ToString() + "%";
            if (noGrandTotal_UnderRcn180 > 0)
                dr1["UnderReconciliationNos"] = Math.Round((Convert.ToDecimal(no_180_Less_UnderRcn) * 100) / Convert.ToDecimal(noGrandTotal_UnderRcn180)).ToString() + "%";
            if (grandTotal_UnderRcn180 > 0)
                dr1["UnderReconciliationAmount"] = Math.Round((total_180_Less_UnderRcn * 100) / grandTotal_UnderRcn180).ToString() + "%";
            if (noGrandTotal_Blanks180 > 0)
                dr1["BlanksNos"] = Math.Round((Convert.ToDecimal(no_180_Less_Blanks) * 100) / Convert.ToDecimal(noGrandTotal_Blanks180)).ToString() + "%";
            if (grandTotal_Blanks180 > 0)
                dr1["BlanksAmount"] = Math.Round((total_180_Less_Blanks * 100) / grandTotal_Blanks180).ToString() + "%";
            if (noGrandTotal_Total180 > 0)
                dr1["TotalNos"] = Math.Round((Convert.ToDecimal(no_180_Less_Total) * 100) / Convert.ToDecimal(noGrandTotal_Total180)).ToString() + "%";
            if (grandTotal_Total180 > 0)
                dr1["TotalAmount"] = Math.Round((total_180_Less_Total * 100) / grandTotal_Total180).ToString() + "%";
            dt.Rows.Add(dr1);

            dr1 = dt.NewRow();
            dr1["PendingDays"] = "More than 180 days";
            if (noGrandTotal_Submitted180 > 0)
                dr1["SubmittedNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_Submitted) * 100) / Convert.ToDecimal(noGrandTotal_Submitted180)).ToString() + "%";
            if (grandTotal_Submitted180 > 0)
                dr1["SubmittedAmount"] = Math.Round((total_180_Greater_Submitted * 100) / grandTotal_Submitted180).ToString() + "%";
            if (noGrandTotal_NotReceivedAtSite180 > 0)
                dr1["NotReceivedAtSiteNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_NotReceivedAtSite) * 100) / Convert.ToDecimal(noGrandTotal_NotReceivedAtSite180)).ToString() + "%";
            if (grandTotal_NotReceivedAtSite180 > 0)
                dr1["NotReceivedAtSiteAmount"] = Math.Round((total_180_Greater_NotReceivedAtSite * 100) / grandTotal_NotReceivedAtSite180).ToString() + "%";
            if (noGrandTotal_ReceivedAtSite180 > 0)
                dr1["ReceivedAtSiteNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_ReceivedAtSite) * 100) / Convert.ToDecimal(noGrandTotal_ReceivedAtSite180)).ToString() + "%";
            if (grandTotal_ReceivedAtSite180 > 0)
                dr1["ReceivedAtSiteAmount"] = Math.Round((total_180_Greater_ReceivedAtSite * 100) / grandTotal_ReceivedAtSite180).ToString() + "%";
            if (noGrandTotal_NotPostedInAccounts180 > 0)
                dr1["NotPostedInAccountsNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_NotPostedInAccounts) * 100) / Convert.ToDecimal(noGrandTotal_NotPostedInAccounts180)).ToString() + "%";
            if (grandTotal_NotPostedInAccounts180 > 0)
                dr1["NotPostedInAccountsAmount"] = Math.Round((total_180_Greater_NotPostedInAccounts * 100) / grandTotal_NotPostedInAccounts180).ToString() + "%";
            if (noGrandTotal_PostedInAcc180 > 0)
                dr1["PostedInAccountsNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_PostedInAcc) * 100) / Convert.ToDecimal(noGrandTotal_PostedInAcc180)).ToString() + "%";
            if (grandTotal_PostedInAcc180 > 0)
                dr1["PostedInAccountsAmount"] = Math.Round((total_180_Greater_PostedInAcc * 100) / grandTotal_PostedInAcc180).ToString() + "%";
            if (noGrandTotal_AckNotAvailable180 > 0)
                dr1["AckNotAvailableNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_AckNotAvailable) * 100) / Convert.ToDecimal(noGrandTotal_AckNotAvailable180)).ToString() + "%";
            if (grandTotal_AckNotAvailable180 > 0)
                dr1["AckNotAvailableAmount"] = Math.Round((total_180_Greater_AckNotAvailable * 100) / grandTotal_AckNotAvailable180).ToString() + "%";
            if (noGrandTotal_UnderRcn180 > 0)
                dr1["UnderReconciliationNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_UnderRcn) * 100) / Convert.ToDecimal(noGrandTotal_UnderRcn180)).ToString() + "%";
            if (grandTotal_UnderRcn180 > 0)
                dr1["UnderReconciliationAmount"] = Math.Round((total_180_Greater_UnderRcn * 100) / grandTotal_UnderRcn180).ToString() + "%";
            if (noGrandTotal_Blanks180 > 0)
                dr1["BlanksNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_Blanks) * 100) / Convert.ToDecimal(noGrandTotal_Blanks180)).ToString() + "%";
            if (grandTotal_Blanks180 > 0)
                dr1["BlanksAmount"] = Math.Round((total_180_Greater_Blanks * 100) / grandTotal_Blanks180).ToString() + "%";
            if (noGrandTotal_Total180 > 0)
                dr1["TotalNos"] = Math.Round((Convert.ToDecimal(no_180_Greater_Total) * 100) / Convert.ToDecimal(noGrandTotal_Total180)).ToString() + "%";
            if (grandTotal_Total180 > 0)
                dr1["TotalAmount"] = Math.Round((total_180_Greater_Total * 100) / grandTotal_Total180).ToString() + "%";
            dt.Rows.Add(dr1);

            grdRecovery3.DataSource = dt;
            grdRecovery3.DataBind();
            
            #endregion
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdRecovery.Rows.Count > 0 && grdRecovery.Visible == true)
            {
                string Subheader = "", Subheader2 = "";
                Subheader = "Status Report of outstanding invoices as on " + txtToDate.Text;
                Subheader2 = "Summary Report of lab less than and more than 180 days status as on " + txtToDate.Text;
                PrintGrid.PrintGridViewRecoveryAging(grdRecovery, grdRecovery2, grdRecovery3, Subheader, Subheader2, "Recovery_Aging_Report");

            }
        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

    }

}