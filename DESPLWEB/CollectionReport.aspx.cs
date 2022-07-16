using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace DESPLWEB
{
    public partial class CollectionReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Collection Report";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                optCollection.Checked = true;
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {                        
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            decimal tmpAmt = 0;
            if (optCollection.Checked == true)
            {                
                #region Collection report
                for (int j = 0; j < grdCollection.Columns.Count ; j++)
                {
                    if (j <= 1)
                        grdCollection.Columns[j].Visible = true;
                    else
                        grdCollection.Columns[j].Visible = false;
                }
                grdCollection.Columns[23].Visible = true;
                dt.Columns.Add(new DataColumn("RowName", typeof(string)));
                dt.Columns.Add(new DataColumn("Amount", typeof(string)));

                decimal totalCollection=0,totalAdvance=0,totalTDS=0;
                var collection = dc.CashDetail_View_Collection(FromDate, ToDate);
                totalCollection = Convert.ToDecimal(collection.FirstOrDefault().totalCollection) * (-1);
                var openAdv = dc.Advance_View_OpenAdvance(FromDate, ToDate,false);
                totalAdvance = Convert.ToDecimal(openAdv.FirstOrDefault().totalAdvance) * (-1);
                var tds = dc.Cash_Advance_View_TDS(FromDate, ToDate);
                totalTDS = Convert.ToDecimal(tds.FirstOrDefault().totalTDS);

                dr1 = dt.NewRow();
                dr1["RowName"] = "Total Collection";
                dr1["Amount"] = totalCollection.ToString("0.00");
                dt.Rows.Add(dr1);

                
                dr1 = dt.NewRow();
                dr1["RowName"] = "Open Advance in Selected Period";
                dr1["Amount"] = totalAdvance.ToString("0.00");
                dt.Rows.Add(dr1);

                
                dr1 = dt.NewRow();
                dr1["RowName"] = "TDS";
                dr1["Amount"] = totalTDS.ToString("0.00");
                dt.Rows.Add(dr1);

                dr1 = dt.NewRow();
                dr1["RowName"] = "Net Collection";
                dr1["Amount"] =  (totalCollection + totalAdvance - totalTDS).ToString("0.00");
                dt.Rows.Add(dr1);

                grdCollection.DataSource = dt;
                grdCollection.DataBind();
                if (grdCollection.Rows.Count >= 4)
                {
                    LinkButton lnkViewReport = (LinkButton)grdCollection.Rows[3].FindControl("lnkViewReport");
                    lnkViewReport.Visible = false;
                }
                #endregion
            }
            else if (optAsOnOpenAdvance.Checked == true)
            {
                #region As On Open Advance
                for (int j = 0; j < grdCollection.Columns.Count ; j++)
                {
                    if (j >= 2 && j <= 5)
                        grdCollection.Columns[j].Visible = true;
                    else
                        grdCollection.Columns[j].Visible = false;
                }
                grdCollection.Columns[23].Visible = true;
               
                dt.Columns.Add(new DataColumn("LedgerId", typeof(string)));
                dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("LedgerName", typeof(string)));
                dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
               
                int SrNo = 1;
                decimal totalBalanceAmt = 0;
                var openAdvance = dc.Advance_View_ClientWise(ToDate);
                foreach (var adv in openAdvance)
                {
                    dr1 = dt.NewRow();
                    dr1["LedgerId"] = adv.LedgerId;
                    dr1["SrNo"] = SrNo;
                    dr1["LedgerName"] = adv.LedgerName_Description;                    
                    tmpAmt = Convert.ToDecimal(adv.BalanceAmount) * (-1);
                    totalBalanceAmt = totalBalanceAmt + tmpAmt;
                    dr1["BalanceAmount"] = tmpAmt.ToString("0.00");
                   // dr1["Note"] = adv ;
                    dt.Rows.Add(dr1);
                    SrNo++;
                }
                dr1 = dt.NewRow();
                dt.Rows.Add(dr1);
                dr1 = dt.NewRow();
                dr1["LedgerName"] = "Total";
                dr1["BalanceAmount"] = totalBalanceAmt.ToString("0.00");
                dt.Rows.Add(dr1);
                grdCollection.DataSource = dt;
                grdCollection.DataBind();
                LinkButton lnkViewReport = (LinkButton)grdCollection.Rows[grdCollection.Rows.Count - 1].FindControl("lnkViewReport");
                lnkViewReport.Visible = false;
                lnkViewReport = (LinkButton)grdCollection.Rows[grdCollection.Rows.Count - 2].FindControl("lnkViewReport");
                lnkViewReport.Visible = false;
                #endregion
            }
            else if (optPrevAdjAdvance.Checked == true)
            {
                #region Previous Advance Adjusted
                for (int j = 0; j < grdCollection.Columns.Count; j++)
                {
                    if (j >= 7 && j<=15)
                        grdCollection.Columns[j].Visible = true;
                    else
                        grdCollection.Columns[j].Visible = false;
                }
                dt.Columns.Add(new DataColumn("AdvanceReceivedFrom", typeof(string)));
                dt.Columns.Add(new DataColumn("AdvanceDate", typeof(string)));
                dt.Columns.Add(new DataColumn("ReceiptNo", typeof(string)));
                dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
                dt.Columns.Add(new DataColumn("BillDate", typeof(string)));
                dt.Columns.Add(new DataColumn("BillAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                dt.Columns.Add(new DataColumn("NoteNo", typeof(string)));
                dt.Columns.Add(new DataColumn("AdjustedDate", typeof(string)));
                dt.Columns.Add(new DataColumn("AdjustedAmount", typeof(string)));

                var PrevAdjAdvance = dc.CashDetail_View_AdjAdvRcptDetails(0, FromDate, ToDate).ToList();
                foreach (var AdjAdv in PrevAdjAdvance)
                {
                    dr1 = dt.NewRow();
                    dr1["AdvanceReceivedFrom"] = AdjAdv.LedgerName_Description;
                    dr1["AdvanceDate"] = Convert.ToDateTime(AdjAdv.ReceiptDate).ToString("dd/MM/yyyy");
                    dr1["ReceiptNo"] = AdjAdv.ReceiptNo;
                    
                    if (AdjAdv.BILL_Date_dt != null)
                    {
                        dr1["BillNo"] = AdjAdv.CashDetail_BillNo_int;
                        dr1["BillDate"] = Convert.ToDateTime(AdjAdv.BILL_Date_dt).ToString("dd/MM/yyyy");
                        tmpAmt = Convert.ToDecimal(AdjAdv.BILL_NetAmt_num) ;
                        dr1["BillAmount"] = tmpAmt.ToString("0.00");
                    }
                    else if (AdjAdv.Journal_Date_dt != null)
                    {
                        dr1["BillNo"] = AdjAdv.CashDetail_BillNo_int;
                        dr1["BillDate"] = Convert.ToDateTime(AdjAdv.Journal_Date_dt).ToString("dd/MM/yyyy");
                        tmpAmt = Convert.ToDecimal(AdjAdv.Journal_Amount_dec);
                        dr1["BillAmount"] = tmpAmt.ToString("0.00");
                    }
                    else
                    {
                        dr1["BillNo"] = AdjAdv.CashDetail_Settlement_var;
                    }
                    dr1["ClientName"] = AdjAdv.CL_Name_var;
                    dr1["NoteNo"] = AdjAdv.CashDetail_NoteNo_var;
                    dr1["AdjustedDate"] = Convert.ToDateTime(AdjAdv.CashDetail_Date_date).ToString("dd/MM/yyyy");
                    tmpAmt = Convert.ToDecimal(AdjAdv.CashDetail_Amount_money) * (-1);
                    dr1["AdjustedAmount"] = tmpAmt.ToString("0.00");
                    dt.Rows.Add(dr1);                    
                }
                grdCollection.DataSource = dt;
                grdCollection.DataBind();
                #endregion
            }
            else if (optOpenAdvanceAging.Checked == true)
            {
                #region Open Advance aging
                for (int j = 0; j < grdCollection.Columns.Count; j++)
                {
                    if (j >= 16 && j <= 22)
                        grdCollection.Columns[j].Visible = true;
                    else
                        grdCollection.Columns[j].Visible = false;
                }
                grdCollection.Columns[grdCollection.Columns.Count - 1].Visible = true;

                dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("LedgerName", typeof(string)));
                dt.Columns.Add(new DataColumn("ReceiptNo", typeof(string))); 
                dt.Columns.Add(new DataColumn("ReceiptDate", typeof(string)));  
                dt.Columns.Add(new DataColumn("ReceiptAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("Age", typeof(string)));         
                dt.Columns.Add(new DataColumn("Note", typeof(string)));
                int SrNo = 1;
                decimal totalBalanceAmt = 0, totalReceiptAmt = 0;
                var openAdv = dc.Advance_View_OpenAdvanceDetails(FromDate, ToDate, true, 0);
                foreach (var adv in openAdv)
                {
                    dr1 = dt.NewRow();
                    dr1["SrNo"] = SrNo;
                    dr1["LedgerName"] = adv.LedgerName_Description;
                    dr1["ReceiptNo"] = adv.ReceiptNo;
                    dr1["ReceiptDate"] = Convert.ToDateTime(adv.ReceiptDate).ToString("dd/MM/yyyy");
                    tmpAmt = Convert.ToDecimal(adv.ReceiptAmount);
                    totalReceiptAmt = totalReceiptAmt + tmpAmt;
                    dr1["ReceiptAmount"] = tmpAmt.ToString("0.00");
                    tmpAmt = Convert.ToDecimal(adv.BalanceAmount) * (-1);
                    totalBalanceAmt = totalBalanceAmt + tmpAmt;
                    dr1["BalanceAmount"] = tmpAmt.ToString("0.00");
                    dr1["Age"] = (ToDate - Convert.ToDateTime(adv.ReceiptDate)).TotalDays;
                    if (adv.Note != null)
                    {
                        dr1["Note"] = adv.Note;
                    }
                    dt.Rows.Add(dr1);
                    SrNo++;
                }
                dr1 = dt.NewRow();
                dt.Rows.Add(dr1);
                dr1 = dt.NewRow();
                dr1["LedgerName"] = "Total";
                dr1["ReceiptAmount"] = totalReceiptAmt.ToString("0.00");
                dr1["BalanceAmount"] = totalBalanceAmt.ToString("0.00");
                dt.Rows.Add(dr1);
                grdCollection.DataSource = dt;
                grdCollection.DataBind();
                #endregion
            }
            else if (optOpenAdvanceAgingNew.Checked == true)
            {
                #region Open Advance aging new
                for (int j = 0; j < grdCollection.Columns.Count; j++)
                {
                    if (j >= 24 && j <= 35)
                        grdCollection.Columns[j].Visible = true;
                    else
                        grdCollection.Columns[j].Visible = false;
                }

                dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("LedgerNameAdvance", typeof(string)));
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

                int SrNo = 1;
                decimal totalAdvBalance = 0, total_0_30_Balance = 0, total_31_60_Balance = 0, total_61_90_Balance = 0;
                decimal total_91_120_Balance = 0, total_121_180_Balance = 0, total_181_270_Balance = 0, total_271_365_Balance = 0, total_366_730_Balance = 0;
                decimal total_731_Above_Balance = 0, total_TotalBalance = 0; 
                var openAdv = dc.OpenAdvanceAsOn_View_Aging(ToDate);
                foreach (var adv in openAdv)
                {
                    dr1 = dt.NewRow();
                    dr1["SrNo"] = SrNo;
                    dr1["LedgerNameAdvance"] = adv.LedgerName_Description;
                    if (adv._0_30_Balance != 0)
                    {
                        dr1["0to30"] = Convert.ToDecimal(adv._0_30_Balance * -1).ToString("0.00");
                        total_0_30_Balance += Convert.ToDecimal(adv._0_30_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._0_30_Balance * -1);
                    }
                    if (adv._31_60_Balance != 0)
                    {
                        dr1["31to60"] = Convert.ToDecimal(adv._31_60_Balance * -1).ToString("0.00");
                        total_31_60_Balance += Convert.ToDecimal(adv._31_60_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._31_60_Balance * -1);
                    }
                    if (adv._61_90_Balance != 0)
                    {
                        dr1["61to90"] = Convert.ToDecimal(adv._61_90_Balance * -1).ToString("0.00");
                        total_61_90_Balance += Convert.ToDecimal(adv._61_90_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._61_90_Balance * -1);
                    }
                    if (adv._91_120_Balance != 0)
                    {
                        dr1["91to120"] = Convert.ToDecimal(adv._91_120_Balance * -1).ToString("0.00");
                        total_91_120_Balance += Convert.ToDecimal(adv._91_120_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._91_120_Balance * -1);
                    }
                    if (adv._121_180_Balance != 0)
                    {
                        dr1["121to180"] = Convert.ToDecimal(adv._121_180_Balance * -1).ToString("0.00");
                        total_121_180_Balance += Convert.ToDecimal(adv._121_180_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._121_180_Balance * -1);
                    }
                    if (adv._181_270_Balance != 0)
                    {
                        dr1["181to270"] = Convert.ToDecimal(adv._181_270_Balance * -1).ToString("0.00");
                        total_181_270_Balance += Convert.ToDecimal(adv._181_270_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._181_270_Balance * -1);
                    }
                    if (adv._271_365_Balance != 0)
                    {
                        dr1["271to365"] = Convert.ToDecimal(adv._271_365_Balance * -1).ToString("0.00");
                        total_271_365_Balance += Convert.ToDecimal(adv._271_365_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._271_365_Balance * -1);
                    }
                    if (adv._366_730_Balance != 0)
                    {
                        dr1["366to730"] = Convert.ToDecimal(adv._366_730_Balance * -1).ToString("0.00");
                        total_366_730_Balance += Convert.ToDecimal(adv._366_730_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._366_730_Balance * -1);
                    }
                    if (adv._731_Above_Balance != 0)
                    {
                        dr1["MoreThan731"] = Convert.ToDecimal(adv._731_Above_Balance * -1).ToString("0.00");
                        total_731_Above_Balance += Convert.ToDecimal(adv._731_Above_Balance * -1);
                        totalAdvBalance += Convert.ToDecimal(adv._731_Above_Balance * -1);
                    }
                    if (adv.TotalBalance != 0)
                    {
                        dr1["GrandTotal"] = Convert.ToDecimal(adv.TotalBalance * -1).ToString("0.00");
                    }
                    total_TotalBalance += Convert.ToDecimal(adv.TotalBalance * -1);

                    dt.Rows.Add(dr1);
                    SrNo++;
                }
                dr1 = dt.NewRow();
                dt.Rows.Add(dr1);
                dr1 = dt.NewRow();
                dr1["LedgerNameAdvance"] = "Total";
                dr1["0to30"] = total_0_30_Balance.ToString("0.00");
                dr1["31to60"] = total_31_60_Balance.ToString("0.00");
                dr1["61to90"] = total_61_90_Balance.ToString("0.00");
                dr1["91to120"] = total_91_120_Balance.ToString("0.00");
                dr1["121to180"] = total_121_180_Balance.ToString("0.00");
                dr1["181to270"] = total_181_270_Balance.ToString("0.00");
                dr1["271to365"] = total_271_365_Balance.ToString("0.00");
                dr1["366to730"] = total_366_730_Balance.ToString("0.00");
                dr1["MoreThan731"] = total_731_Above_Balance.ToString("0.00");
                dr1["GrandTotal"] = totalAdvBalance.ToString("0.00");
                dt.Rows.Add(dr1);
                grdCollection.DataSource = dt;
                grdCollection.DataBind();
                #endregion
            }
        }
        protected void lnkViewReport(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int index = Convert.ToInt32(row.RowIndex);
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            decimal tmpAmt = 0;
            if (optCollection.Checked == true)
            {
                if (index == 0)
                {
                    #region Total Collection Amount Details
                    string strClient = "|", strTestType = "|", strCollectedBy = "|", strRegion = "|";
                    lblDetailHeading.Text = "Total Collection Amount Details";
                    pnlCollectionDetails.Visible = true; div1.Visible = true;
                    pnlSort.Visible = true;
                    for (int j = 0; j < grdCollDetails.Columns.Count; j++)
                    {
                        if (j <= 8)
                            grdCollDetails.Columns[j].Visible = true;
                        else
                            grdCollDetails.Columns[j].Visible = false;
                    }
                    dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                    dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
                    dt.Columns.Add(new DataColumn("BillingDate", typeof(string)));
                    dt.Columns.Add(new DataColumn("CollectionDate", typeof(string)));
                    dt.Columns.Add(new DataColumn("TestingType", typeof(string)));
                    dt.Columns.Add(new DataColumn("CollectedBy", typeof(string)));
                    dt.Columns.Add(new DataColumn("Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("Region", typeof(string)));
                    dt.Columns.Add(new DataColumn("MktUser", typeof(string)));

                    var collection = dc.CashDetail_View_CollectionDetail(FromDate, ToDate);
                    foreach (var coll in collection)
                    {
                        dr1 = dt.NewRow();
                        dr1["ClientName"] = coll.clientName;

                        if (coll.CashDetail_BillNo_int != null && coll.CashDetail_BillNo_int != "")
                            dr1["BillNo"] = coll.CashDetail_BillNo_int;
                        else if (coll.CashDetail_Settlement_var != null && coll.CashDetail_Settlement_var != "")
                            dr1["BillNo"] = coll.CashDetail_Settlement_var;

                        if (coll.BILL_Date_dt != null)
                            dr1["BillingDate"] = Convert.ToDateTime(coll.BILL_Date_dt).ToString("dd/MM/yyyy");
                        //else if (coll.Journal_Date_dt != null)
                        //    dr1["BillingDate"] = coll.Journal_Date_dt;
                        else
                            dr1["BillingDate"] = Convert.ToDateTime(coll.CashDetail_Date_date).ToString("dd/MM/yyyy");

                        dr1["CollectionDate"] = Convert.ToDateTime(coll.CashDetail_Date_date).ToString("dd/MM/yyyy");
                        dr1["TestingType"] = coll.testtype;

                        if (coll.collectiondetail != null && coll.collectiondetail != "")
                        {
                            string[] strColl = coll.collectiondetail.Split('|');
                            var user = dc.User_View(Convert.ToInt32(strColl[0]), 0, "", "", "");
                            //dr1["CollectedBy"] = user.FirstOrDefault().USER_Name_var;
                            foreach (var item in user)
	                        {
                                dr1["CollectedBy"] = item.USER_Name_var.ToString();
	                        }
                            
                        }
                        else
                        {
                            dr1["CollectedBy"] = "Lab";
                        }

                        //dr1["Amount"] = coll.CashDetail_Amount_money * (-1);
                        tmpAmt = Convert.ToDecimal(coll.CashDetail_Amount_money) * (-1);
                        dr1["Amount"] = tmpAmt.ToString("0.00");
                        if (coll.regionName != null && coll.regionName != "")
                            dr1["Region"] = coll.regionName;
                        else
                            dr1["Region"] = "Pune";

                        if (coll.mktUser != null && coll.mktUser != "")
                            dr1["MktUser"] = coll.mktUser;
                        else
                            dr1["MktUser"] = "NA";
                        if (strClient.Contains("|" + dr1["ClientName"] + "|") == false && dr1["ClientName"].ToString() != "")
                            strClient += dr1["ClientName"] + "|";

                        if (strTestType.Contains("|" + dr1["TestingType"] + "|") == false && dr1["TestingType"].ToString() != "")
                            strTestType += dr1["TestingType"] + "|";

                        if (strCollectedBy.Contains("|" + dr1["CollectedBy"] + "|") == false && dr1["CollectedBy"].ToString() != "")
                            strCollectedBy += dr1["CollectedBy"] + "|";

                        if (strRegion.Contains("|" + dr1["Region"] + "|") == false && dr1["Region"].ToString() != "")
                            strRegion += dr1["Region"] + "|";
                        dt.Rows.Add(dr1);
                    }
                    grdCollDetails.DataSource = dt;
                    grdCollDetails.DataBind();
                    ViewState["CollectionDetails"] = dt;
                    ddlClient.Items.Clear();
                    ddlClient.Items.Add("---All---");
                    ddlTestType.Items.Clear();
                    ddlTestType.Items.Add("---All---");
                    ddlCollectedBy.Items.Clear();
                    ddlCollectedBy.Items.Add("---All---");
                    ddlRegion.Items.Clear();
                    ddlRegion.Items.Add("---All---");

                    string[] strSearch = strClient.Split('|');
                    for (int j = 1; j < strSearch.Count()-1; j++)
                    {
                        ddlClient.Items.Add(strSearch[j]);
                    }
                    strSearch = strTestType.Split('|');
                    for (int j = 1; j < strSearch.Count()-1; j++)
                    {
                        ddlTestType.Items.Add(strSearch[j]);
                    }
                    strSearch = strCollectedBy.Split('|');
                    for (int j = 1; j < strSearch.Count()-1; j++)
                    {
                        ddlCollectedBy.Items.Add(strSearch[j]);
                    }
                    strSearch = strRegion.Split('|');
                    for (int j = 1; j < strSearch.Count()-1; j++)
                    {
                        ddlRegion.Items.Add(strSearch[j]);
                    }
                    #endregion
                }
                else if (index == 1)
                {
                    #region Open Advance Details
                    lblDetailHeading.Text = "Open Advance Details";
                    pnlCollectionDetails.Visible = true;
                    pnlSort.Visible = false; div1.Visible = true;
                    for (int j = 0; j < grdCollDetails.Columns.Count; j++)
                    {
                        if (j >= 9 && j <= 14)
                            grdCollDetails.Columns[j].Visible = true;
                        else
                            grdCollDetails.Columns[j].Visible = false;
                    }
                    dt.Columns.Add(new DataColumn("ReceiptDate", typeof(string)));
                    dt.Columns.Add(new DataColumn("AdvReceiptNo", typeof(string)));
                    dt.Columns.Add(new DataColumn("LedgerName", typeof(string)));
                    dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
                    dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
                    dt.Columns.Add(new DataColumn("Note", typeof(string)));
                  
                    var openAdv = dc.Advance_View_OpenAdvanceDetails(FromDate, ToDate, false,0);
                    foreach (var adv in openAdv)
                    {
                        dr1 = dt.NewRow();
                        dr1["ReceiptDate"] = Convert.ToDateTime(adv.ReceiptDate).ToString("dd/MM/yyyy");
                        dr1["AdvReceiptNo"] = adv.ReceiptNo;
                        dr1["LedgerName"] = adv.LedgerName_Description;
                        tmpAmt = Convert.ToDecimal(adv.ReceiptAmount);
                        dr1["TotalAmount"] = tmpAmt.ToString("0.00");
                        tmpAmt = Convert.ToDecimal(adv.BalanceAmount) * (-1);
                        dr1["BalanceAmount"] = tmpAmt.ToString("0.00");
                        dr1["Note"] = Convert.ToString(adv.Note);
                        dt.Rows.Add(dr1);
                    }
                    
                    grdCollDetails.DataSource = dt;
                    grdCollDetails.DataBind();
                    #endregion
                }
                else if (index == 2)
                {
                    #region Total TDS Details
                    lblDetailHeading.Text = "Total TDS Details";
                    pnlCollectionDetails.Visible = true;
                    pnlSort.Visible = false; div1.Visible = true;
                    for (int j = 0; j < grdCollDetails.Columns.Count; j++)
                    {
                        if (j >= 15 && j<= 19)
                            grdCollDetails.Columns[j].Visible = true;
                        else
                            grdCollDetails.Columns[j].Visible = false;
                    }
                    dt.Columns.Add(new DataColumn("ReceiptDate", typeof(string)));
                    dt.Columns.Add(new DataColumn("ReceiptNo", typeof(string)));
                    dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                    dt.Columns.Add(new DataColumn("ReceiptAmount", typeof(string)));
                    dt.Columns.Add(new DataColumn("TDSAmount", typeof(string)));

                    var TDSDetails = dc.Cash_Advance_View_TDSDetails(FromDate, ToDate);
                    foreach (var tds in TDSDetails)
                    {
                        dr1 = dt.NewRow();
                        dr1["ReceiptDate"] = Convert.ToDateTime(tds.ReceiptDate).ToString("dd/MM/yyyy");
                        dr1["ReceiptNo"] = tds.ReceiptNo;
                        dr1["ClientName"] = tds.ClientName;
                        tmpAmt = Convert.ToDecimal(tds.ReceiptAmount);
                        dr1["ReceiptAmount"] = tmpAmt.ToString("0.00");
                        tmpAmt = Convert.ToDecimal(tds.TDSAmount);
                        dr1["TDSAmount"] = tmpAmt.ToString("0.00");
                        dt.Rows.Add(dr1);
                    }
                    grdCollDetails.DataSource = dt;
                    grdCollDetails.DataBind();
                    #endregion
                }
                else if (index == 3)
                {
                    pnlCollectionDetails.Visible = false;
                }
            }
            else if (optAsOnOpenAdvance.Checked == true)
            {
                #region As on Open Advance Details
                lblDetailHeading.Text = grdCollection.Rows[index].Cells[4].Text + " - " + grdCollection.Rows[index].Cells[5].Text;
                pnlCollectionDetails.Visible = true;
                pnlSort.Visible = false; div1.Visible = true;
                grdCollDetails.ShowFooter = true;
                for (int j = 0; j < grdCollDetails.Columns.Count; j++)
                {
                    if (j >= 20 && j <= 28)
                        grdCollDetails.Columns[j].Visible = true;
                    else
                        grdCollDetails.Columns[j].Visible = false;
                }
                dt.Columns.Add(new DataColumn("ReceiptDate", typeof(string)));
                dt.Columns.Add(new DataColumn("ReceiptNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ReceiptAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("AdjustedDate", typeof(string)));
                dt.Columns.Add(new DataColumn("NoteNo", typeof(string)));
                dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
                dt.Columns.Add(new DataColumn("AdjustedAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("Note", typeof(string)));

                decimal Total = 0, drTotal =0, crTotal =0;
                var openAdv = dc.Advance_View_OpenAdvanceDetails(FromDate, ToDate, true, Convert.ToInt32(grdCollection.Rows[index].Cells[2].Text));
                foreach (var adv in openAdv)
                {
                    dr1 = dt.NewRow();
                    dr1["ReceiptDate"] = Convert.ToDateTime(adv.ReceiptDate).ToString("dd/MM/yyyy");
                    dr1["ReceiptNo"] = adv.ReceiptNo;                    
                    tmpAmt = Convert.ToDecimal(adv.ReceiptAmount);
                    dr1["ReceiptAmount"] = tmpAmt.ToString("0.00");
                    Total += tmpAmt;
                    tmpAmt = Convert.ToDecimal(adv.BalanceAmount) * (-1);
                    dr1["BalanceAmount"] = tmpAmt.ToString("0.00");
                    drTotal += tmpAmt;
                    dr1["Note"] = Convert.ToString(adv.Note);
                      
                    bool newRow = false;
                    var AdjAdvance = dc.CashDetail_View_AdjAdvRcptDetails(adv.ReceiptNo,FromDate, ToDate).ToList();
                    foreach (var adjAdv in AdjAdvance)
                    {
                        if (newRow==true)
                            dr1 = dt.NewRow();
                        dr1["AdjustedDate"] = Convert.ToDateTime(adjAdv.CashDetail_Date_date).ToString("dd/MM/yyyy");
                        dr1["NoteNo"] = adjAdv.CashDetail_NoteNo_var;
                        dr1["BillNo"] = adjAdv.CashDetail_BillNo_int;
                        tmpAmt = Convert.ToDecimal(adjAdv.CashDetail_Amount_money) * (-1);
                        dr1["AdjustedAmount"] = tmpAmt.ToString("0.00");
                        crTotal += tmpAmt;
                        dt.Rows.Add(dr1);
                        newRow = true;
                    }
                    if (AdjAdvance.Count == 0)
                        dt.Rows.Add(dr1);
                }
                grdCollDetails.DataSource = dt;
                grdCollDetails.DataBind();                
                grdCollDetails.FooterRow.Cells[20].Text = "Total";
                grdCollDetails.FooterRow.Cells[22].Text = Total.ToString("0.00");
                grdCollDetails.FooterRow.Cells[23].Text = drTotal.ToString("0.00");
                grdCollDetails.FooterRow.Cells[27].Text = crTotal.ToString("0.00");
                grdCollDetails.FooterRow.HorizontalAlign = HorizontalAlign.Center;
                #endregion
            }
        }

        protected void filterCollectionData()
        {
            DataTable dt = (DataTable)ViewState["CollectionDetails"];
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt1.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("BillingDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("CollectionDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("TestingType", typeof(string)));
            dt1.Columns.Add(new DataColumn("CollectedBy", typeof(string)));
            dt1.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt1.Columns.Add(new DataColumn("Region", typeof(string)));
            dt1.Columns.Add(new DataColumn("MktUser", typeof(string)));
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((dt.Rows[i]["ClientName"].ToString() == ddlClient.SelectedItem.Text || ddlClient.SelectedItem.Text == "---All---") &&
                    (dt.Rows[i]["TestingType"].ToString() == ddlTestType.SelectedItem.Text || ddlTestType.SelectedItem.Text == "---All---") &&
                    (dt.Rows[i]["CollectedBy"].ToString() == ddlCollectedBy.SelectedItem.Text || ddlCollectedBy.SelectedItem.Text == "---All---") &&
                    (dt.Rows[i]["Region"].ToString() == ddlRegion.SelectedItem.Text || ddlRegion.SelectedItem.Text == "---All---"))
                {
                    dr1 = dt1.NewRow();
                    dr1["ClientName"] = dt.Rows[i]["ClientName"];
                    dr1["BillNo"] = dt.Rows[i]["BillNo"];
                    dr1["BillingDate"] = dt.Rows[i]["BillingDate"];
                    dr1["CollectionDate"] = dt.Rows[i]["CollectionDate"];
                    dr1["TestingType"] = dt.Rows[i]["TestingType"];
                    dr1["CollectedBy"] = dt.Rows[i]["CollectedBy"];
                    dr1["Amount"] = dt.Rows[i]["Amount"];
                    dr1["Region"] = dt.Rows[i]["Region"];
                    dr1["MktUser"] = dt.Rows[i]["MktUser"];
                    dt1.Rows.Add(dr1);
                }
            }
            grdCollDetails.DataSource = dt1;
            grdCollDetails.DataBind();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdCollection.Rows.Count > 0 && grdCollection.Visible == true)
            {
                string Subheader = "";
                string collec = "";

                if (optCollection.Checked)
                {
                    collec = optCollection.Text;
                }
                else if (optAsOnOpenAdvance.Checked)
                {
                    collec = optAsOnOpenAdvance.Text;
                }
                else if (optPrevAdjAdvance.Checked)
                {
                    collec = optPrevAdjAdvance.Text;
                }
                else if (optOpenAdvanceAging.Checked)
                {
                    collec = optOpenAdvanceAging.Text;
                }
                else if (optOpenAdvanceAgingNew.Checked)
                {
                    collec = optOpenAdvanceAgingNew.Text;
                }
                Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + collec;
                PrintHTMLReport rpt = new PrintHTMLReport();
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                string grddata = "";
                string grdColumn = "";
                string grdview = "";

                for (int j = 0; j < grdCollection.Columns.Count; j++)
                {
                    if (grdCollection.Columns[j].Visible == true)
                    {
                        grdColumn += grdCollection.Columns[j].HeaderText + "|";
                    }
                }
                for (int i = 0; i < grdCollection.Rows.Count; i++)
                {
                    grddata += "$";
                    for (int c = 0; c < grdCollection.Rows[i].Cells.Count; c++)
                    {
                        grddata += grdCollection.Rows[i].Cells[c].Text + "~";
                    }
                }
                grdview = grdColumn + grddata;
                //reportStr = rpt.RptHTMLgrid("Collection Report", Subheader, grdview);
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                //rpt.RptHTMLgrid("Collection Report", Subheader, grdview);
                PrintGrid.PrintGridView(grdCollection, Subheader, "Collection_Detail_Report");
            }

        }
        protected void lnkgrdPrint_Click(object sender, EventArgs e)
        {
            if (grdCollDetails.Rows.Count > 0 && grdCollDetails.Visible == true)
            {
                string Subheader = "";
                string collec = "";

                if (optCollection.Checked)
                {
                    collec = optCollection.Text;
                }
                else if (optAsOnOpenAdvance.Checked)
                {
                    collec = optAsOnOpenAdvance.Text;
                }
                else if (optPrevAdjAdvance.Checked)
                {
                    collec = optPrevAdjAdvance.Text;
                }
                Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + collec;
                PrintHTMLReport rpt = new PrintHTMLReport();
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                for (int j = 0; j < grdCollDetails.Columns.Count; j++)
                {
                    if (grdCollDetails.Columns[j].Visible == true)
                    {
                        grdColumn += grdCollDetails.Columns[j].HeaderText + "|";
                    }
                }
                for (int i = 0; i < grdCollDetails.Rows.Count; i++)
                {
                    grddata += "$";
                    for (int c = 0; c < grdCollDetails.Rows[i].Cells.Count; c++)
                    {
                        grddata += grdCollDetails.Rows[i].Cells[c].Text + "~";
                    }
                }
                grdview = grdColumn + grddata;
                //reportStr = rpt.RptHTMLgrid("Collection Detail Report", Subheader, grdview);
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                //rpt.RptHTMLgrid("Collection Detail Report", Subheader, grdview);
                PrintGrid.PrintGridView(grdCollDetails, Subheader, "Collection_Detail_Report");
            }
        }


        protected void opt_CheckedChanged(object sender, EventArgs e)
        {
            grdCollection.DataSource = null;
            grdCollection.DataBind();
            pnlCollectionDetails.Visible = false;
            lblDetailHeading.Visible = false;
            pnlSort.Visible = false;
            div1.Visible = false;
            grdCollDetails.DataSource = null;
            grdCollDetails.DataBind();
            grdCollDetails.ShowFooter = false;

            if (optCollection.Checked == true)
            {
                pnlCollection.Height = 100;
                pnlCollectionDetails.Height = 450;
            }
            else if (optAsOnOpenAdvance.Checked == true )
            {
                pnlCollection.Height = 185;
                pnlCollectionDetails.Height = 180;
            }
            else if (optPrevAdjAdvance.Checked == true)
            {
                pnlCollection.Height = 300;
                pnlCollectionDetails.Height = 100;
            }
            else if (optOpenAdvanceAging.Checked == true || optOpenAdvanceAgingNew.Checked == true)
            {
                pnlCollection.Height = 400;
                pnlCollectionDetails.Height = 0;
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

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterCollectionData();
        }
        
    }
}