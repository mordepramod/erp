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
    public partial class CashBook : System.Web.UI.Page
    {        
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Cash Book";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                optDebtorList.Checked = true;
                txtBillLockDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                var master = dc.MasterSetting_View(0);
                foreach (var mst in master)
                {
                    if (mst.MASTER_BillLockDate_dt != null)
                        txtBillLockDate.Text = Convert.ToDateTime(mst.MASTER_BillLockDate_dt).ToString("dd/MM/yyyy");
                }
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAccount_right_bit == true)
                    {
                        lnkUpdateBillLockDate.Enabled = true;
                    }
                }
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
            grdCashBook.ShowFooter = false;
            pnlCashBook.Height = 200;
            if (optDebtorList.Checked == true)
            {
                #region Debtor List
                for (int j = 0; j < grdCashBook.Columns.Count - 1; j++)
                {
                    if (j >= 0 && j <= 4)
                        grdCashBook.Columns[j].Visible = true;
                    else
                        grdCashBook.Columns[j].Visible = false;
                }
                grdCashBook.Columns[grdCashBook.Columns.Count - 1].Visible = true;
                dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ClientId", typeof(string)));
                dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                dt.Columns.Add(new DataColumn("DebitBalance", typeof(string)));
                dt.Columns.Add(new DataColumn("CreditBalance", typeof(string)));
                int SrNo = 1;
                decimal totalAmt = 0, drTot = 0, crTot=0;
                var debtorList = dc.CashDetail_View_DebtorList(0, ToDate);
                foreach (var debtor in debtorList)
                {
                    dr1 = dt.NewRow();
                    dr1["SrNo"] = SrNo;
                    dr1["ClientId"] = debtor.CL_Id;
                    dr1["ClientName"] = debtor.CL_Name_var;
                    tmpAmt = Convert.ToDecimal(debtor.billAmount + debtor.journalAmount + debtor.receivedAmount) ;
                    if (tmpAmt > 0)
                    {
                        dr1["DebitBalance"] = tmpAmt.ToString("0.00");
                        dr1["CreditBalance"] = "0.00";
                        totalAmt += tmpAmt;
                        drTot += tmpAmt;
                    }
                    else
                    {
                        tmpAmt = tmpAmt * (-1);
                        dr1["DebitBalance"] = "0.00";
                        dr1["CreditBalance"] = tmpAmt.ToString("0.00");
                        crTot += tmpAmt;
                    }                    
                    dt.Rows.Add(dr1);
                    SrNo++;
                }
                grdCashBook.ShowFooter = true;
                grdCashBook.DataSource = dt;
                grdCashBook.DataBind();
                grdCashBook.FooterRow.Cells[2].Text = "Total";
                grdCashBook.FooterRow.Cells[3].Text = drTot.ToString("0.00");
                grdCashBook.FooterRow.Cells[4].Text = crTot.ToString("0.00");
                grdCashBook.FooterRow.HorizontalAlign = HorizontalAlign.Center;
                #endregion
            }
            else if (optSummary.Checked == true)
            {
                #region Summary
                for (int j = 0; j < grdCashBook.Columns.Count - 1; j++)
                {
                    if (j >= 5 && j <= 8)
                        grdCashBook.Columns[j].Visible = true;
                    else
                        grdCashBook.Columns[j].Visible = false;
                }
                grdCashBook.Columns[grdCashBook.Columns.Count - 1].Visible = true;
                dt.Columns.Add(new DataColumn("Date", typeof(string)));
                dt.Columns.Add(new DataColumn("DebitAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("CreditAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));

                decimal balAmt = 0;
                var cashDebtorAmt = dc.CashDetail_View_DebtorList(0, Convert.ToDateTime(FromDate).AddDays(-1));
                foreach (var cash in cashDebtorAmt)
                {
                    if ((cash.receivedAmount + cash.billAmount + cash.journalAmount) > 0)
                    {
                        balAmt += cash.receivedAmount + cash.billAmount + cash.journalAmount;
                    }
                }
                dr1 = dt.NewRow();
                dr1["Date"] = "Opening";
                //if (balAmt < 0)
                //    balAmt = 0;
                if (balAmt < 0)
                    dr1["BalanceAmount"] = (balAmt * (-1)).ToString("0.00") + "  Cr";
                else
                    dr1["BalanceAmount"] = (balAmt).ToString("0.00") + "  Dr";
                dt.Rows.Add(dr1);

                var cashDetails = dc.CashDetail_View_DateWise(FromDate, ToDate, false);
                foreach (var cash in cashDetails)
                {
                    dr1 = dt.NewRow();
                    dr1["Date"] = Convert.ToDateTime(cash.cashDate).ToString("dd/MM/yyyy");
                    tmpAmt = Convert.ToDecimal(cash.DrAmount);
                    dr1["DebitAmount"] = tmpAmt.ToString("0.00");
                    tmpAmt = Convert.ToDecimal(cash.CrAmount) * (-1);
                    dr1["CreditAmount"] = tmpAmt.ToString("0.00");
                    balAmt = 0;
                    var cashDebtorAmt1 = dc.CashDetail_View_DebtorList(0, cash.cashDate);
                    foreach (var cash1 in cashDebtorAmt1)
                    {
                        if ((cash1.receivedAmount + cash1.billAmount + cash1.journalAmount) > 0)
                        {
                            balAmt += cash1.receivedAmount + cash1.billAmount + cash1.journalAmount;
                        }
                    }
                    if (balAmt < 0)
                        balAmt = 0;
                    if (balAmt < 0)
                        dr1["BalanceAmount"] = (balAmt * (-1)).ToString("0.00") + "  Cr";
                    else
                        dr1["BalanceAmount"] = (balAmt).ToString("0.00") + "  Dr";
                    dt.Rows.Add(dr1);
                }
                grdCashBook.ShowFooter = false;
                grdCashBook.DataSource = dt;
                grdCashBook.DataBind();
                if (grdCashBook.Rows.Count > 0)
                {
                    if (grdCashBook.Rows[0].Cells[5].Text.Contains("Opening") == true)
                    {
                        LinkButton lnkViewReport = (LinkButton)grdCashBook.Rows[0].FindControl("lnkViewReport");
                        lnkViewReport.Visible = false;
                    }
                }
                #endregion
            }
            else if (optCashBook.Checked == true)
            {
                #region client Cash Book
                for (int j = 0; j < grdCashBook.Columns.Count - 1; j++)
                {
                    if (j >= 9 && j <= 14)
                        grdCashBook.Columns[j].Visible = true;
                    else
                        grdCashBook.Columns[j].Visible = false;
                }
                grdCashBook.Columns[grdCashBook.Columns.Count - 1].Visible = true;
                dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                dt.Columns.Add(new DataColumn("ClientId", typeof(string)));
                dt.Columns.Add(new DataColumn("OpeningBalance", typeof(string)));
                dt.Columns.Add(new DataColumn("DebitAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("CreditAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("ClosingBalance", typeof(string)));

                var CashBook = dc.CashDetail_View_ClientWiseAmount(FromDate ,ToDate);
                foreach (var cash in CashBook)
                {
                    //if (cash.payment != 0 || cash.receipt != 0)
                    //{
                        dr1 = dt.NewRow();
                        dr1["ClientName"] = cash.CL_Name_var;
                        dr1["ClientId"] = cash.CL_Id;

                        tmpAmt = Convert.ToDecimal(cash.opening);
                        if (tmpAmt < 0)
                        {
                            dr1["OpeningBalance"] = (tmpAmt * (-1)).ToString("0.00") + "  Cr";
                        }
                        else
                        {
                            dr1["OpeningBalance"] = (tmpAmt).ToString("0.00") + "  Dr";
                        }
                        tmpAmt = Convert.ToDecimal(cash.payment);
                        dr1["DebitAmount"] = (tmpAmt).ToString("0.00");

                        tmpAmt = Convert.ToDecimal(cash.receipt);
                        dr1["CreditAmount"] = (tmpAmt * (-1)).ToString("0.00");

                        tmpAmt = Convert.ToDecimal(cash.closing);
                        if (tmpAmt < 0)
                        {
                            dr1["ClosingBalance"] = (tmpAmt * (-1)).ToString("0.00") + "  Cr";
                        }
                        else
                        {
                            dr1["ClosingBalance"] = (tmpAmt).ToString("0.00") + "  Dr";
                        }
                        dt.Rows.Add(dr1);
                    //}
                }
                grdCashBook.ShowFooter = false;
                grdCashBook.DataSource = dt;
                grdCashBook.DataBind();
                for (int j = 0; j < grdCashBook.Rows.Count ; j++)
                {
                    if (grdCashBook.Rows[j].Cells[14].Text.Contains("Cr") == true)
                    {
                        foreach (TableCell cell in grdCashBook.Rows[j].Cells)
                        {
                            cell.BackColor = System.Drawing.Color.Orange;
                        }
                    } 
                }
                #endregion
            }
            else if (optSalesRegister.Checked == true)
            {
                #region Sales Register
                pnlCashBook.Height = 400;
                for (int j = 0; j < grdCashBook.Columns.Count - 1; j++)
                {
                    if (j >= 15 && j <= 42)
                        grdCashBook.Columns[j].Visible = true;
                    else
                        grdCashBook.Columns[j].Visible = false;
                }
                grdCashBook.Columns[grdCashBook.Columns.Count - 1].Visible = false;
                dt.Columns.Add(new DataColumn("ClientId", typeof(string)));
                dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                dt.Columns.Add(new DataColumn("Limit", typeof(string)));
                dt.Columns.Add(new DataColumn("Balance", typeof(string)));
                dt.Columns.Add(new DataColumn("Total", typeof(string)));

                dt.Columns.Add(new DataColumn("Coupon", typeof(string)));
                dt.Columns.Add(new DataColumn("AAC", typeof(string)));
                dt.Columns.Add(new DataColumn("AGGT", typeof(string)));
                dt.Columns.Add(new DataColumn("BT-", typeof(string)));
                dt.Columns.Add(new DataColumn("CCH", typeof(string)));
                dt.Columns.Add(new DataColumn("CEMT", typeof(string)));
                dt.Columns.Add(new DataColumn("CORECUT", typeof(string)));
                dt.Columns.Add(new DataColumn("CR", typeof(string)));
                dt.Columns.Add(new DataColumn("CT", typeof(string)));
                dt.Columns.Add(new DataColumn("FLYASH", typeof(string)));
                dt.Columns.Add(new DataColumn("GT", typeof(string)));
                dt.Columns.Add(new DataColumn("MF", typeof(string)));
                dt.Columns.Add(new DataColumn("NDT", typeof(string)));
                dt.Columns.Add(new DataColumn("OT", typeof(string)));
                dt.Columns.Add(new DataColumn("PILE", typeof(string)));
                dt.Columns.Add(new DataColumn("PT", typeof(string)));
                dt.Columns.Add(new DataColumn("RWH", typeof(string)));
                dt.Columns.Add(new DataColumn("SO", typeof(string)));
                dt.Columns.Add(new DataColumn("SOLID", typeof(string)));
                dt.Columns.Add(new DataColumn("ST", typeof(string)));
                dt.Columns.Add(new DataColumn("STC", typeof(string)));
                dt.Columns.Add(new DataColumn("TILE", typeof(string)));
                dt.Columns.Add(new DataColumn("WT", typeof(string)));

                decimal totalAmt = 0;
                var clients = dc.Client_SalesRegister(FromDate ,ToDate);
                foreach (var client in clients)
                {
                    tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCoupon), 0) + Math.Round(Convert.ToDecimal(client.SaleAAC), 0) + Math.Round(Convert.ToDecimal(client.SaleAGGT), 0) + Math.Round(Convert.ToDecimal(client.SaleBT), 0) + Math.Round(Convert.ToDecimal(client.SaleCCH), 0) + Math.Round(Convert.ToDecimal(client.SaleCEMT), 0) + Math.Round(Convert.ToDecimal(client.SaleCORECUT), 0) + Math.Round(Convert.ToDecimal(client.SaleCR), 0) + Math.Round(Convert.ToDecimal(client.SaleCT), 0) +
                        Math.Round(Convert.ToDecimal(client.SaleFLYASH), 0) + Math.Round(Convert.ToDecimal(client.SaleGT), 0) + Math.Round(Convert.ToDecimal(client.SaleMF), 0) + Math.Round(Convert.ToDecimal(client.SaleNDT), 0) + Math.Round(Convert.ToDecimal(client.SaleOT), 0) + Math.Round(Convert.ToDecimal(client.SalePT), 0) +
                        Math.Round(Convert.ToDecimal(client.SalePILE), 0) + Math.Round(Convert.ToDecimal(client.SaleRWH), 0) + Math.Round(Convert.ToDecimal(client.SaleSO), 0) + Math.Round(Convert.ToDecimal(client.SaleSOLID), 0) + Math.Round(Convert.ToDecimal(client.SaleST), 0) + Math.Round(Convert.ToDecimal(client.SaleSTC), 0) + Math.Round(Convert.ToDecimal(client.SaleTILE), 0) + Math.Round(Convert.ToDecimal(client.SaleWT), 0));
                    if (tmpAmt > 0)
                    {
                        dr1 = dt.NewRow();
                        dr1["ClientId"] = client.CL_Id;
                        dr1["ClientName"] = client.CL_Name_var;
                        tmpAmt = Convert.ToDecimal(client.CL_Limit_mny);
                        dr1["Limit"] = tmpAmt.ToString("0.00");
                        tmpAmt = 0;
                        if (client.CL_BalanceAmt_mny > 0)
                            tmpAmt = Convert.ToDecimal(client.CL_BalanceAmt_mny);
                        dr1["Balance"] = tmpAmt.ToString("0.00");
                        tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCoupon), 0) + Math.Round(Convert.ToDecimal(client.SaleAAC), 0) + Math.Round(Convert.ToDecimal(client.SaleAGGT), 0) + Math.Round(Convert.ToDecimal(client.SaleBT), 0) + Math.Round(Convert.ToDecimal(client.SaleCCH), 0) + Math.Round(Convert.ToDecimal(client.SaleCEMT), 0) + Math.Round(Convert.ToDecimal(client.SaleCORECUT), 0) + Math.Round(Convert.ToDecimal(client.SaleCR), 0) + Math.Round(Convert.ToDecimal(client.SaleCT), 0) +
                            Math.Round(Convert.ToDecimal(client.SaleFLYASH), 0) + Math.Round(Convert.ToDecimal(client.SaleGT), 0) + Math.Round(Convert.ToDecimal(client.SaleMF), 0) + Math.Round(Convert.ToDecimal(client.SaleNDT), 0) + Math.Round(Convert.ToDecimal(client.SaleOT), 0) + Math.Round(Convert.ToDecimal(client.SalePT), 0) +
                            Math.Round(Convert.ToDecimal(client.SalePILE), 0) + Math.Round(Convert.ToDecimal(client.SaleRWH), 0) + Math.Round(Convert.ToDecimal(client.SaleSO), 0) + Math.Round(Convert.ToDecimal(client.SaleSOLID), 0) + Math.Round(Convert.ToDecimal(client.SaleST), 0) + Math.Round(Convert.ToDecimal(client.SaleSTC), 0) + Math.Round(Convert.ToDecimal(client.SaleTILE), 0) + Math.Round(Convert.ToDecimal(client.SaleWT), 0));
                        dr1["Total"] = tmpAmt.ToString("0.00");
                        totalAmt += tmpAmt;
                        if (tmpAmt > 0)
                        {
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCoupon), 0));
                            if (tmpAmt > 0)
                                dr1["Coupon"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleAAC), 0));
                            if (tmpAmt > 0)
                                dr1["AAC"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleAGGT), 0));
                            if (tmpAmt > 0)
                                dr1["AGGT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleBT), 0));
                            if (tmpAmt > 0)
                                dr1["BT-"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCCH), 0));
                            if (tmpAmt > 0)
                                dr1["CCH"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCEMT), 0));
                            if (tmpAmt > 0)
                                dr1["CEMT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCORECUT), 0));
                            if (tmpAmt > 0)
                                dr1["CORECUT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCR), 0));
                            if (tmpAmt > 0)
                                dr1["CR"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleCT), 0));
                            if (tmpAmt > 0)
                                dr1["CT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleFLYASH), 0));
                            if (tmpAmt > 0)
                                dr1["FLYASH"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleGT), 0));
                            if (tmpAmt > 0)
                                dr1["GT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleMF), 0));
                            if (tmpAmt > 0)
                                dr1["MF"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleNDT), 0));
                            if (tmpAmt > 0)
                                dr1["NDT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleOT), 0));
                            if (tmpAmt > 0)
                                dr1["OT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SalePT), 0));
                            if (tmpAmt > 0)
                                dr1["PT"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SalePILE), 0));
                            if (tmpAmt > 0)
                                dr1["PILE"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleRWH), 0));
                            if (tmpAmt > 0)
                                dr1["RWH"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleSO), 0));
                            if (tmpAmt > 0)
                                dr1["SO"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleSOLID), 0));
                            if (tmpAmt > 0)
                                dr1["SOLID"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleST), 0));
                            if (tmpAmt > 0)
                                dr1["ST"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleSTC), 0));
                            if (tmpAmt > 0)
                                dr1["STC"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleTILE), 0));
                            if (tmpAmt > 0)
                                dr1["TILE"] = tmpAmt.ToString("0.00");
                            tmpAmt = Convert.ToDecimal(Math.Round(Convert.ToDecimal(client.SaleWT), 0));
                            if (tmpAmt > 0)
                                dr1["WT"] = tmpAmt.ToString("0.00");
                        }
                        dt.Rows.Add(dr1);
                    }             
                }
                grdCashBook.ShowFooter = true;
                grdCashBook.DataSource = dt;
                grdCashBook.DataBind();
                grdCashBook.FooterRow.Cells[18].Text = "Total Sale";
                grdCashBook.FooterRow.Cells[19].Text = totalAmt.ToString("0.00");                
                grdCashBook.FooterRow.HorizontalAlign = HorizontalAlign.Center;
                #endregion
            }
            else if (optBRMovement.Checked == true)
            {
                #region BR Movement
                for (int j = 0; j < grdCashBook.Columns.Count - 1; j++)
                {
                    if (j >= 43 && j <= 53)
                        grdCashBook.Columns[j].Visible = true;
                    else
                        grdCashBook.Columns[j].Visible = false;
                }
                grdCashBook.Columns[grdCashBook.Columns.Count - 1].Visible = true;
                dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
                dt.Columns.Add(new DataColumn("ClientId", typeof(string)));
                dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
                dt.Columns.Add(new DataColumn("BillingDate", typeof(string)));
                dt.Columns.Add(new DataColumn("CollectionDate", typeof(string)));
                dt.Columns.Add(new DataColumn("TestingType", typeof(string)));
                dt.Columns.Add(new DataColumn("CollectedBy", typeof(string)));
                dt.Columns.Add(new DataColumn("Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("Region", typeof(string)));
                dt.Columns.Add(new DataColumn("MktUser", typeof(string)));
                dt.Columns.Add(new DataColumn("Advance", typeof(string)));

                //var collection = dc.CashDetail_View_CollectionDetail(FromDate, ToDate);
                var collection = dc.CashDetail_View_BRMovement(FromDate, ToDate);
                foreach (var coll in collection)
                {
                    dr1 = dt.NewRow();
                    dr1["ClientName"] = coll.clientName;
                    dr1["ClientId"] = coll.CashDetail_ClientId;

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
                        var user = dc.User_View(Convert.ToInt32(strColl[0]), 0, "", "", "").ToList();
                        if (user.Count() > 0)
                            dr1["CollectedBy"] = user.FirstOrDefault().USER_Name_var;
                    }
                    else
                    {
                        dr1["CollectedBy"] = "Lab";
                    }

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
                    if (coll.CashDetail_NoteNo_var.Contains("Adv/") == true)
                    {
                        dr1["Advance"] = "Advance";
                    }
                    
                    dt.Rows.Add(dr1);
                }
                grdCashBook.ShowFooter = false;
                grdCashBook.DataSource = dt;
                grdCashBook.DataBind();
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

            #region A/c statement
            string clientId = "", clientName = "";
            if (optDebtorList.Checked == true)
            {
                clientId = grdCashBook.Rows[index].Cells[1].Text;
                clientName = grdCashBook.Rows[index].Cells[2].Text;
                grdCashDetails.Columns[7].Visible = true;
            }
            else if (optSummary.Checked == true)
            {
                clientId = "0";
                clientName = grdCashBook.Rows[index].Cells[5].Text;
                strDate = clientName.Split('/');
                ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                grdCashDetails.Columns[7].Visible = false;
            }
            else if (optCashBook.Checked == true)
            {
                clientId = grdCashBook.Rows[index].Cells[10].Text;
                clientName = grdCashBook.Rows[index].Cells[9].Text;
                grdCashDetails.Columns[7].Visible = true;
            }
            else if (optBRMovement.Checked == true)
            {
                clientId = grdCashBook.Rows[index].Cells[44].Text;
                clientName = grdCashBook.Rows[index].Cells[43].Text;
                grdCashDetails.Columns[7].Visible = true;
            }
            lblDetailHeading.Text = "A/c Statement [ " + clientName + " ] ";
            pnlCashDetails.Visible = true; pnlCashDetails1.Visible = true;
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceiptNo", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("NoteNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Settlement", typeof(string)));
            dt.Columns.Add(new DataColumn("Debit", typeof(string)));
            dt.Columns.Add(new DataColumn("Credit", typeof(string)));
            dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
            decimal drTotal = 0, crTotal = 0, balAmt = 0;
            int i = 0;
            bool addRow = false;
                
            var cashDetails = dc.CashDetail_View_ClientWise(Convert.ToInt32(clientId), ToDate);            
            foreach (var cashd in cashDetails)
            {
                tmpAmt = Convert.ToDecimal(cashd.Amount);
                balAmt += tmpAmt;
                if (cashd.cashDate < FromDate)
                {
                    if (i == 0)
                        dr1 = dt.NewRow();
                    dr1["Date"] = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
                    dr1["NoteNo"] = "Opening";
                    addRow = true;
                }
                else
                {
                    if (addRow == true)
                    {
                        if (balAmt > 0)
                            dr1["BalanceAmount"] = balAmt.ToString("0.00") + "  Dr";
                        else
                            dr1["BalanceAmount"] = (balAmt * (-1)).ToString("0.00") + "  Cr";
                        dt.Rows.Add(dr1);
                        addRow = false;
                    }
                    dr1 = dt.NewRow();
                    dr1["Date"] = Convert.ToDateTime(cashd.cashDate).ToString("dd/MM/yyyy");
                    if (cashd.ReceiptNo > 0)
                        dr1["ReceiptNo"] = cashd.ReceiptNo;
                    dr1["BillNo"] = cashd.BillNo;
                    dr1["NoteNo"] = cashd.NoteNo;
                    dr1["Settlement"] = cashd.Settlement;
                    tmpAmt = Convert.ToDecimal(cashd.Amount);
                    if (tmpAmt > 0)
                    {
                        dr1["Debit"] = tmpAmt.ToString("0.00");
                        drTotal += tmpAmt;
                    }
                    else
                    {
                        tmpAmt = tmpAmt * (-1);
                        dr1["Credit"] = tmpAmt.ToString("0.00");
                        crTotal += tmpAmt;
                    }
                    if (balAmt > 0)
                        dr1["BalanceAmount"] = balAmt.ToString("0.00") + "  Dr";
                    else
                        dr1["BalanceAmount"] = (balAmt * (-1)).ToString("0.00") + "  Cr";
                    dt.Rows.Add(dr1);
                }
                i++;
            }
            if (addRow == true)
            {
                if (balAmt > 0)
                    dr1["BalanceAmount"] = balAmt.ToString("0.00") + "  Dr";
                else
                    dr1["BalanceAmount"] = (balAmt * (-1)).ToString("0.00") + "  Cr";
                dt.Rows.Add(dr1);
            }
            grdCashDetails.ShowFooter = true;
            grdCashDetails.DataSource = dt;
            grdCashDetails.DataBind();
            if (grdCashDetails.Rows.Count > 0)
            {
                grdCashDetails.FooterRow.Font.Bold = true;
                grdCashDetails.FooterRow.HorizontalAlign = HorizontalAlign.Center;
                grdCashDetails.FooterRow.Cells[0].Text = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
                grdCashDetails.FooterRow.Cells[3].Text = "Closing";
                grdCashDetails.FooterRow.Cells[4].Text = "Total";
                grdCashDetails.FooterRow.Cells[5].Text = drTotal.ToString("0.00");
                grdCashDetails.FooterRow.Cells[6].Text = crTotal.ToString("0.00");
                if (balAmt > 0)
                    grdCashDetails.FooterRow.Cells[7].Text = balAmt.ToString("0.00") + "  Dr";
                else
                    grdCashDetails.FooterRow.Cells[7].Text = (balAmt * (-1)).ToString("0.00") + "  Cr";
            }
            #endregion
            
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdCashBook.Rows.Count > 0 && grdCashBook.Visible == true)
            {
                string Subheader = "";
                string collec = "";

                if (optDebtorList.Checked)
                {
                    collec = optDebtorList.Text;
                }
                else if (optCashBook.Checked)
                {
                    collec = optCashBook.Text;
                }
                else if (optSummary.Checked)
                {
                    collec = optSummary.Text;
                }
                else if (optSalesRegister.Checked)
                {
                    collec = optSalesRegister.Text;
                }
                else if (optBRMovement.Checked)
                {
                    collec = optBRMovement.Text;
                }
                Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + collec;
                PrintGrid.PrintGridView(grdCashBook, Subheader, "Cash_Book_Detail");
            }

        }
        
        protected void lnkgrdPrint_Click(object sender, EventArgs e)
        {
            if (grdCashDetails.Rows.Count > 0 && grdCashDetails.Visible == true)
            {
                string Subheader = "";
                string collec = "";

                if (optDebtorList.Checked)
                {
                    collec = optDebtorList.Text;
                }
                else if (optCashBook.Checked)
                {
                    collec = optCashBook.Text;
                }
                else if (optSummary.Checked)
                {
                    collec = optSummary.Text;
                }
                else if (optSalesRegister.Checked)
                {
                    collec = optSalesRegister.Text;
                }
                else if (optBRMovement.Checked)
                {
                    collec = optBRMovement.Text;
                }
                Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + collec;

                PrintGrid.PrintGridView(grdCashDetails, Subheader, "Cash_Detail_Report");
                //PrintHTMLReport rpt = new PrintHTMLReport();              
                //rpt.RptHTMLgrid("Cash Detail Report", Subheader, grdview);
            }
        }
        
        protected void opt_CheckedChanged(object sender, EventArgs e)
        {
            grdCashBook.DataSource = null;
            grdCashBook.DataBind();
            pnlCashDetails.Visible = false; pnlCashDetails1.Visible = false;
            grdCashDetails.DataSource = null;
            grdCashDetails.DataBind();
            grdCashDetails.ShowFooter = false;
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkUpdateBillLockDate_Click(object sender, EventArgs e)
        {
            if (txtBillLockDate.Text != "")
            {
                DateTime d1 = DateTime.ParseExact(txtBillLockDate.Text, "dd/MM/yyyy", null);
                dc.MasterSetting_Update(d1);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "alert('Date Sucessfully Updated ')", true);
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Date Sucessfully Updated');", true);
            }
        }
        
        
    }
}