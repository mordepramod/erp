using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class OutstandingAging : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Outstanding Aging Report";
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

            }
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("0to30", typeof(string)));
            dt.Columns.Add(new DataColumn("31to60", typeof(string)));
            dt.Columns.Add(new DataColumn("61to90", typeof(string)));
            dt.Columns.Add(new DataColumn("91to120", typeof(string)));
            dt.Columns.Add(new DataColumn("121to180", typeof(string)));
            dt.Columns.Add(new DataColumn("181to270", typeof(string)));
            dt.Columns.Add(new DataColumn("271to365", typeof(string)));
            dt.Columns.Add(new DataColumn("366to730", typeof(string)));
            dt.Columns.Add(new DataColumn("MoreThan731", typeof(string)));
            dt.Columns.Add(new DataColumn("GrandTotal", typeof(string)));
            dt.Columns.Add(new DataColumn("Cr/DrBalanceAmt", typeof(string)));
            dt.Columns.Add(new DataColumn("RecoveryUser", typeof(string)));           
            

            dt.Columns.Add(new DataColumn("PostedInAccountsAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedAtHOAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("NotReceivedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ResubmittedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("UnderReconciliationAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("BlanksAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("DebitNoteAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("OnAccAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("Business1920", typeof(string)));

            decimal totalBillBalance = 0, total_0_30_Balance = 0, total_31_60_Balance = 0, total_61_90_Balance = 0,
                 total_91_120_Balance = 0, total_121_180_Balance = 0, total_181_270_Balance = 0, total_271_365_Balance = 0, total_366_730_Balance = 0,
                 total_731_Above_Balance = 0, total_TotalBalance = 0;
            decimal total_PostedInAccounts_Balance = 0, total_ReceivedAtHO_Balance = 0, total_NotReceived_Balance = 0, total_Resubmitted_Balance = 0, total_UnderReconciliation_Balance = 0, total_Blank_Balance = 0, total_DebitNote_Balance = 0, total_OnAcc_Balance = 0;
            string[] strDate = txtToDate.Text.Split('/');            
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            int i = 0;
            var balance = dc.Client_OutstandingAging(ToDate);
            foreach (var bal in balance)
            {                
                i++;
                dr1 = dt.NewRow();
                dr1["SrNo"] = i;
                dr1["ClientName"] = bal.CL_Name_var;
                if (bal.CL_Name_var =="Tricon Infra Buildtech Pvt. Ltd.")
                {
                    i =i+0;
                }
                dr1["RecoveryUser"] = Convert.ToString(bal.RecoveryUser);
                dr1["Business1920"] = Convert.ToString(bal._CurrYearBusiness);
                
                var recovery = dc.Client_OutstandingAging_RecoveryDetails(bal.CL_Id, ToDate);
                foreach (var rec in recovery)
                {
                    dr1["PostedInAccountsAmount"] = rec._PostedInAccounts_Balance;
                    dr1["ReceivedAtHOAmount"] = rec._ReceivedAtHO_Balance;
                    dr1["NotReceivedAmount"] = rec._NotReceived_Balance;
                    dr1["ResubmittedAmount"] = rec._Resubmitted_Balance;
                    dr1["UnderReconciliationAmount"] = rec._UnderReconciliation_Balance;
                    dr1["BlanksAmount"] = rec._Blank_Balance;
                    dr1["DebitNoteAmount"] = rec._DebitNote_Balance;
                    //dr1["OnAccAmount"] = rec._OnAcc_Balance;

                    if (Convert.ToDecimal(rec._OnAcc_Balance) < 0)
                        dr1["OnAccAmount"] = Convert.ToDecimal(rec._OnAcc_Balance) * -1;
                    else
                        dr1["OnAccAmount"] = 0;


                    total_PostedInAccounts_Balance += Convert.ToDecimal(rec._PostedInAccounts_Balance);
                    total_ReceivedAtHO_Balance += Convert.ToDecimal(rec._ReceivedAtHO_Balance);
                    total_NotReceived_Balance += Convert.ToDecimal(rec._NotReceived_Balance);
                    total_Resubmitted_Balance += Convert.ToDecimal(rec._Resubmitted_Balance);
                    total_UnderReconciliation_Balance += Convert.ToDecimal(rec._UnderReconciliation_Balance);
                    total_Blank_Balance += Convert.ToDecimal(rec._Blank_Balance);
                    total_DebitNote_Balance += Convert.ToDecimal(rec._DebitNote_Balance);
                    //total_OnAcc_Balance += Convert.ToDecimal(rec._OnAcc_Balance);
                    if (Convert.ToDecimal(rec._OnAcc_Balance) < 0)
                        total_OnAcc_Balance += Convert.ToDecimal(rec._OnAcc_Balance) * -1;
                }
                totalBillBalance = 0;
                if (bal._0_30_Balance > 0)
                {
                    dr1["0to30"] = bal._0_30_Balance;
                    total_0_30_Balance += Convert.ToDecimal(bal._0_30_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._0_30_Balance); 
                }
                if (bal._31_60_Balance > 0)
                {
                    dr1["31to60"] = bal._31_60_Balance;
                    total_31_60_Balance += Convert.ToDecimal(bal._31_60_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._31_60_Balance); 
                }
                if (bal._61_90_Balance > 0)
                {
                    dr1["61to90"] = bal._61_90_Balance;
                    total_61_90_Balance += Convert.ToDecimal(bal._61_90_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._61_90_Balance); 
                }
                if (bal._91_120_Balance > 0)
                {
                    dr1["91to120"] = bal._91_120_Balance;
                    total_91_120_Balance += Convert.ToDecimal(bal._91_120_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._91_120_Balance); 
                }
                if (bal._121_180_Balance > 0)
                {
                    dr1["121to180"] = bal._121_180_Balance;
                    total_121_180_Balance += Convert.ToDecimal(bal._121_180_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._121_180_Balance); 
                }
                if (bal._181_270_Balance > 0)
                {
                    dr1["181to270"] = bal._181_270_Balance;
                    total_181_270_Balance += Convert.ToDecimal(bal._181_270_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._181_270_Balance); 
                }
                
                if (bal._271_365_Balance > 0)
                {
                    dr1["271to365"] = bal._271_365_Balance;
                    total_271_365_Balance += Convert.ToDecimal(bal._271_365_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._271_365_Balance);
                }
                if (bal._366_730_Balance > 0)
                {
                    dr1["366to730"] = bal._366_730_Balance;
                    total_366_730_Balance += Convert.ToDecimal(bal._366_730_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._366_730_Balance);
                }
                if (bal._731_Above_Balance > 0)
                {
                    dr1["MoreThan731"] = bal._731_Above_Balance;
                    total_731_Above_Balance += Convert.ToDecimal(bal._731_Above_Balance);
                    totalBillBalance += Convert.ToDecimal(bal._731_Above_Balance); 
                }
                
                dr1["GrandTotal"] = bal.TotalBalance;
                total_TotalBalance += Convert.ToDecimal(bal.TotalBalance);
                //totalBillBalance = Convert.ToDecimal(bal._0_30_Balance + bal._31_45_Balance + bal._46_60_Balance + bal._61_90_Balance + bal._91_120_Balance + bal._121_180_Balance + bal._181_270_Balance + bal._271_360_Balance + bal._361_Above_Balance);
                
                if ((bal.TotalBalance - totalBillBalance) != 0 && totalBillBalance > 0)
                {
                    totalBillBalance = totalBillBalance - Convert.ToDecimal(bal.TotalBalance);
                    if (totalBillBalance > 0 && bal._731_Above_Balance > 0)
                    {
                        if (bal._731_Above_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._731_Above_Balance);
                            total_731_Above_Balance = Convert.ToDecimal(total_731_Above_Balance - bal._731_Above_Balance);
                            dr1["MoreThan731"] = "";
                        }
                        else
                        {
                            dr1["MoreThan731"] = bal._731_Above_Balance   - totalBillBalance;
                            total_731_Above_Balance = total_731_Above_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }                        
                    }

                    if (totalBillBalance > 0 && bal._366_730_Balance > 0)
                    {
                        if (bal._366_730_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._366_730_Balance);
                            total_366_730_Balance = Convert.ToDecimal(total_366_730_Balance - bal._366_730_Balance);
                            dr1["366to730"] = "";
                        }
                        else
                        {
                            dr1["366to730"] = bal._366_730_Balance - totalBillBalance;
                            total_366_730_Balance = total_366_730_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                    if (totalBillBalance > 0 && bal._181_270_Balance > 0)
                    {
                        if (bal._181_270_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._181_270_Balance);
                            total_181_270_Balance = Convert.ToDecimal(total_181_270_Balance - bal._181_270_Balance);
                            dr1["181to270"] = "";
                        }
                        else
                        {
                            dr1["181to270"] = bal._181_270_Balance - totalBillBalance;
                            total_181_270_Balance = total_181_270_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                    if (totalBillBalance > 0 && bal._271_365_Balance > 0)
                    {
                        if (bal._271_365_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._271_365_Balance);
                            total_271_365_Balance = Convert.ToDecimal(total_271_365_Balance - bal._271_365_Balance);
                            dr1["271to365"] = "";
                        }
                        else
                        {
                            dr1["271to365"] = bal._271_365_Balance - totalBillBalance;
                            total_271_365_Balance = total_271_365_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                    if (totalBillBalance > 0 && bal._121_180_Balance > 0)
                    {
                        if (bal._121_180_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._121_180_Balance);
                            total_121_180_Balance = Convert.ToDecimal(total_121_180_Balance - bal._121_180_Balance);
                            dr1["121to180"] = "";
                        }
                        else
                        {
                            dr1["121to180"] = bal._121_180_Balance - totalBillBalance;
                            total_121_180_Balance = total_121_180_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                    if (totalBillBalance > 0 && bal._91_120_Balance > 0)
                    {
                        if (bal._91_120_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._91_120_Balance);
                            total_91_120_Balance = Convert.ToDecimal(total_91_120_Balance - bal._91_120_Balance);
                            dr1["91to120"] = "";
                        }
                        else
                        {
                            dr1["91to120"] = bal._91_120_Balance - totalBillBalance;
                            total_91_120_Balance = total_91_120_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                    if (totalBillBalance > 0 && bal._61_90_Balance > 0)
                    {
                        if (bal._61_90_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._61_90_Balance);
                            total_61_90_Balance = Convert.ToDecimal(total_61_90_Balance - bal._61_90_Balance);
                            dr1["61to90"] = "";
                        }
                        else
                        {
                            dr1["61to90"] = bal._61_90_Balance - totalBillBalance;
                            total_61_90_Balance = total_61_90_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                  
                    if (totalBillBalance > 0 && bal._31_60_Balance > 0)
                    {
                        if (bal._31_60_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._31_60_Balance);
                            total_31_60_Balance = Convert.ToDecimal(total_31_60_Balance - bal._31_60_Balance);
                            dr1["31to60"] = "";
                        }
                        else
                        {
                            dr1["31to60"] = bal._31_60_Balance - totalBillBalance;
                            total_31_60_Balance = total_31_60_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                    if (totalBillBalance > 0 && bal._0_30_Balance > 0)
                    {
                        if (bal._0_30_Balance < totalBillBalance)
                        {
                            totalBillBalance = Convert.ToDecimal(totalBillBalance - bal._0_30_Balance);
                            total_0_30_Balance = Convert.ToDecimal(total_0_30_Balance - bal._0_30_Balance);
                            dr1["0to30"] = "";
                        }
                        else
                        {
                            dr1["0to30"] = bal._0_30_Balance - totalBillBalance;
                            total_0_30_Balance = total_0_30_Balance - totalBillBalance;
                            totalBillBalance = 0;
                        }
                    }
                }

                dt.Rows.Add(dr1);
                    
            }

            grdOutstanding.DataSource = dt;
            grdOutstanding.DataBind();

            if (grdOutstanding.Rows.Count > 0)
            {
                grdOutstanding.FooterRow.BackColor = System.Drawing.Color.White;
                grdOutstanding.FooterStyle.BackColor = System.Drawing.Color.White;
                for (int j = 0; j < grdOutstanding.Columns.Count; j++)
                {
                    grdOutstanding.FooterRow.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                    grdOutstanding.FooterRow.Cells[4].Font.Bold = true;
                }

                grdOutstanding.FooterRow.Cells[1].Text = "Grand Total";
                grdOutstanding.FooterRow.Cells[2].Text = total_0_30_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[3].Text = total_31_60_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[4].Text = total_61_90_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[5].Text = total_91_120_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[6].Text = total_121_180_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[7].Text = total_181_270_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[8].Text = total_271_365_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[9].Text = total_366_730_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[10].Text = total_731_Above_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[11].Text = total_TotalBalance.ToString("0.00");
                //grdOutstanding.FooterRow.Cells[12].Text = total_totalBillBalance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[13].Text = total_PostedInAccounts_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[14].Text = total_ReceivedAtHO_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[15].Text = total_NotReceived_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[16].Text = total_Resubmitted_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[17].Text = total_UnderReconciliation_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[18].Text = total_Blank_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[19].Text = total_DebitNote_Balance.ToString("0.00");
                grdOutstanding.FooterRow.Cells[20].Text = total_OnAcc_Balance.ToString("0.00");

            }
            
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdOutstanding.Rows.Count > 0 && grdOutstanding.Visible == true)
            {
                string Subheader = "";
                Subheader = "" + "|" + lblToDate.Text + "|" + txtToDate.Text;              
                 PrintGrid.PrintGridView(grdOutstanding, Subheader, "Outstanding_Aging_Report");

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