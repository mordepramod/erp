using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Ionic.Zip;

namespace DESPLWEB
{
    public partial class OutstandingReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Outstanding Bills";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadInwardType();
            }
        }
        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddlInwardTestType.DataTextField = "MATERIAL_Name_var";
            ddlInwardTestType.DataValueField = "MATERIAL_RecordType_var";
            ddlInwardTestType.DataSource = inwd;
            ddlInwardTestType.DataBind();
            ddlInwardTestType.Items.Insert(0, "---All---");
        }
        protected void chkAsOnDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAsOnDate.Checked == true)
            {
                lblFromDate.Visible = false;
                txtFromDate.Visible = false;
                lblToDate.Text = "Till Date";
            }
            else
            {
                lblFromDate.Visible = true;
                txtFromDate.Visible = true;
                lblToDate.Text = "To Date";
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            for (int col = 3; col <= 21; col++)
            {
                if (col <= 14 || col == 21)
                    grdOutstanding.Columns[col].Visible = true;
            }
            
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
            dt.Columns.Add(new DataColumn("DaySpending", typeof(string)));
            dt.Columns.Add(new DataColumn("MktUser", typeof(string)));
            dt.Columns.Add(new DataColumn("RecoveryUser", typeof(string)));
            dt.Columns.Add(new DataColumn("Route", typeof(string)));
            dt.Columns.Add(new DataColumn("Limit", typeof(string)));
            dt.Columns.Add(new DataColumn("BillWiseOutstanding", typeof(string)));
            dt.Columns.Add(new DataColumn("OnAcBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("ActualOutstanding", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTW_AckDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTW_BookedDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTW_AckFileName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Status_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_BillBookedAmt_num", typeof(string)));
            //dt.Columns.Add(new DataColumn("RECV_ClearedFromSite_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Remark_var", typeof(string)));

            DateTime tempDate;
            int i = 0;
            decimal totalAmount = 0, totalOutstanding = 0, actualOutstanding = 0, totalOnAcBalance = 0, billOutstanding = 0, PendingAmount = 0, below90Outstanding = 0, above90Outstanding = 0, totalBillWiseOutstanding = 0;
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strFromDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            string strClient = "|", strMktUser = "|", strRecoveryUser = "|";
            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            clsData obj = new clsData();
            
            DataTable dtClientList = obj.getClientOutstanding2(ClientId, strFromDate, strToDate, Convert.ToInt32(chkAsOnDate.Checked));
            for (int cllist = 0; cllist < dtClientList.Rows.Count; cllist++)
            {
                
                DataTable dtClient = obj.getClientOutstanding3(Convert.ToInt32(dtClientList.Rows[cllist]["BILL_CL_Id"]), strFromDate, strToDate, Convert.ToInt32(chkAsOnDate.Checked));
                for (int cl = 0; cl < dtClient.Rows.Count; cl++)
                {
                    actualOutstanding = Convert.ToDecimal(dtClient.Rows[cl]["billAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmount"]);
                    if (actualOutstanding > 0)
                        totalOutstanding += actualOutstanding;
                    totalOnAcBalance += Convert.ToDecimal(dtClient.Rows[cl]["OnAccAmount"]);
                    int rowNo = 0;
                    //bills
                    billOutstanding = 0;
                    var billList = dc.Bill_View_Outstanding(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, FromDate, ToDate, chkAsOnDate.Checked).ToList();
                    var dbList = dc.Journal_View_OutstandingDbNote(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, FromDate, ToDate, chkAsOnDate.Checked).ToList();
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
                    foreach (var bill in billList)
                    {


                        i++;
                        tempDate = Convert.ToDateTime(bill.BILL_Date_dt);
                        dr1 = dt.NewRow();
                        dr1["SrNo"] = i;

                        if (rowNo == 0)
                        {
                            if (dtClient.Rows[cl]["CL_Limit_mny"].ToString() != "")
                                dr1["Limit"] = Convert.ToDouble(dtClient.Rows[cl]["CL_Limit_mny"]).ToString("0.00");
                            dr1["BillWiseOutstanding"] = billOutstanding.ToString("0.00");
                            totalBillWiseOutstanding += billOutstanding;
                            //if (Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]) < 0)
                            dr1["OnAcBalance"] = (-1 * Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"])).ToString("0.00");
                            //else
                            //    dr1["OnAcBalance"] = Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]).ToString("0.00");
                            if (actualOutstanding > 0)
                                dr1["ActualOutstanding"] = actualOutstanding.ToString("0.00");

                        }
                        rowNo++;

                        dr1["ClientName"] = bill.CL_Name_var;
                        dr1["SiteName"] = bill.SITE_Name_var;
                        dr1["BillNo"] = bill.BILL_Id;
                        dr1["BillDate"] = tempDate.ToString("dd/MM/yyyy");
                        if (bill.BILL_RecordType_var == "---")
                            dr1["TestingType"] = "Coupon";
                        else if (bill.BILL_RecordType_var == "Monthly")
                            dr1["TestingType"] = "Monthly";
                        else
                            dr1["TestingType"] = bill.testtype;
                        dr1["BillAmount"] = bill.BILL_NetAmt_num;

                        PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                        //actualOutstanding = actualOutstanding - PendingAmount;
                        totalAmount += Convert.ToDecimal(bill.BILL_NetAmt_num);

                        dr1["PendingAmount"] = PendingAmount.ToString("0.00");
                        dr1["DaySpending"] = (DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null) - Convert.ToDateTime(bill.BILL_Date_dt)).Days;
                        if (Convert.ToInt32(dr1["DaySpending"]) <= 30)
                        {
                            below90Outstanding += PendingAmount;
                        }
                        else if (Convert.ToInt32(dr1["DaySpending"]) > 30)
                        {
                            above90Outstanding += PendingAmount;
                        }
                        if (bill.mktUser != null && bill.mktUser != "")
                            dr1["MktUser"] = bill.mktUser;
                        else
                            dr1["MktUser"] = "NA";
                        if (bill.routeName != null && bill.routeName != "")
                            dr1["Route"] = bill.routeName;
                        else
                            dr1["Route"] = "NA";
                        if (bill.recoveryUser != null && bill.recoveryUser != "")
                            dr1["RecoveryUser"] = bill.recoveryUser;
                        else
                            dr1["RecoveryUser"] = "NA";
                        if (strClient.Contains("|" + dr1["ClientName"] + "|") == false && dr1["ClientName"].ToString() != "")
                            strClient += dr1["ClientName"] + "|";

                        if (strMktUser.Contains("|" + dr1["MktUser"] + "|") == false && dr1["MktUser"].ToString() != "")
                            strMktUser += dr1["MktUser"] + "|";

                        if (strRecoveryUser.Contains("|" + dr1["RecoveryUser"] + "|") == false && dr1["RecoveryUser"].ToString() != "")
                            strRecoveryUser += dr1["RecoveryUser"] + "|";

                        if (bill.OUTW_AckDate_dt != null)
                        {
                            tempDate = Convert.ToDateTime(bill.OUTW_AckDate_dt);
                            dr1["OUTW_AckDate_dt"] = tempDate.ToString("dd/MM/yyyy");
                        }
                        if (bill.OUTW_BookedDate_dt != null)
                        {
                            tempDate = Convert.ToDateTime(bill.OUTW_BookedDate_dt);
                            dr1["OUTW_BookedDate_dt"] = tempDate.ToString("dd/MM/yyyy");
                        }
                        if (bill.OUTW_AckFileName_var != null && bill.OUTW_AckFileName_var != "")
                            dr1["OUTW_AckFileName_var"] = "Complete";
                        else
                            dr1["OUTW_AckFileName_var"] = "Pending";
                        dr1["RECV_Status_var"] = bill.RECV_Status_var;
                        dr1["RECV_BillBookedAmt_num"] = bill.RECV_BillBookedAmt_num;
                        //dr1["RECV_ClearedFromSite_var"] = bill.RECV_ClearedFromSite_var;
                        dr1["RECV_Remark_var"] = bill.RECV_Remark_var;
                        if ((bill.RECV_Status_var == null || bill.RECV_Status_var == "")
                            && dtClient.Rows[cl]["CL_UnderReconciliation_bit"].ToString() == "True")
                        {
                            dr1["RECV_Status_var"] = "Under Reconciliation";
                        }
                        dt.Rows.Add(dr1);

                    }
                    //db Note
                    //var dbList = dc.Journal_View_OutstandingDbNote(client.CL_Id, 0, FromDate, ToDate, chkAsOnDate.Checked).ToList();
                    foreach (var db in dbList)
                    {


                        i++;
                        tempDate = Convert.ToDateTime(db.Journal_Date_dt);
                        dr1 = dt.NewRow();
                        dr1["SrNo"] = i;

                        if (rowNo == 0)
                        {
                            if (dtClient.Rows[cl]["CL_Limit_mny"].ToString() != "")
                                dr1["Limit"] = Convert.ToDouble(dtClient.Rows[cl]["CL_Limit_mny"]).ToString("0.00");
                            dr1["BillWiseOutstanding"] = billOutstanding.ToString("0.00");
                            totalBillWiseOutstanding += billOutstanding;
                            //if (Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]) < 0)
                            dr1["OnAcBalance"] = (-1 * Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"])).ToString("0.00");
                            //else
                            //    dr1["OnAcBalance"] = Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]).ToString("0.00");
                            if (actualOutstanding > 0)
                                dr1["ActualOutstanding"] = actualOutstanding.ToString("0.00");
                        }
                        rowNo++;
                        dr1["ClientName"] = db.CL_Name_var;
                        dr1["SiteName"] = db.SITE_Name_var;
                        dr1["BillNo"] = db.Journal_NoteNo_var;
                        dr1["BillDate"] = tempDate.ToString("dd/MM/yyyy");
                        dr1["TestingType"] = "";
                        dr1["BillAmount"] = db.Journal_Amount_dec;

                        PendingAmount = Convert.ToDecimal(db.Journal_Amount_dec + db.ReceivedAmt);
                        totalAmount += Convert.ToDecimal(db.Journal_Amount_dec);

                        dr1["PendingAmount"] = PendingAmount.ToString("0.00");
                        dr1["DaySpending"] = (DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null) - Convert.ToDateTime(db.Journal_Date_dt)).Days;
                        if (Convert.ToInt32(dr1["DaySpending"]) <= 30)
                        {
                            below90Outstanding += PendingAmount;
                        }
                        else if (Convert.ToInt32(dr1["DaySpending"]) > 30)
                        {
                            above90Outstanding += PendingAmount;
                        }
                        dr1["MktUser"] = "";
                        dr1["Route"] = "";
                        dr1["RecoveryUser"] = "";
                        if (strClient.Contains("|" + dr1["ClientName"] + "|") == false && dr1["ClientName"].ToString() != "")
                            strClient += dr1["ClientName"] + "|";
                        
                        dt.Rows.Add(dr1);

                    }
                }
            }
            grdOutstanding.DataSource = dt;
            grdOutstanding.DataBind();
            Session["OutstandingDetails"] = dt;
            ddlClientSort.Items.Clear();
            ddlClientSort.Items.Add("---All---");
            ddlMktUserSort.Items.Clear();
            ddlMktUserSort.Items.Add("---All---");
            //ddlRecoveryUserSort.Items.Clear();
            //ddlRecoveryUserSort.Items.Add("---All---");
            string[] strSearch = strClient.Split('|');
            for (int j = 1; j < strSearch.Count() - 1; j++)
            {
                ddlClientSort.Items.Add(strSearch[j]);
            }
            strSearch = strMktUser.Split('|');
            for (int j = 1; j < strSearch.Count() - 1; j++)
            {
                ddlMktUserSort.Items.Add(strSearch[j]);
            }
            //strSearch = strRecoveryUser.Split('|');
            //for (int j = 1; j < strSearch.Count() - 1; j++)
            //{
            //    ddlRecoveryUserSort.Items.Add(strSearch[j]);
            //}
            if (totalOnAcBalance < 0)
                totalOnAcBalance = -1 * totalOnAcBalance;
            lblTotal.Text = "Bill Total : " + totalAmount.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Total Pending : " + totalBillWiseOutstanding.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "On A/c Balance : " + totalOnAcBalance.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Actual Pending : " + totalOutstanding.ToString("0.00");

            if (ClientId > 0)
            {
                lblOutstanding.Text = ClientId + "|" + totalOutstanding + "|" + below90Outstanding + "|" + above90Outstanding;
            }
            
        }
        protected void grdOutstanding_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAckFileName = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "ViewAckFile")
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                string filePath = "D:/AckFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";
                if (strAckFileName.Contains(',') == false)
                {
                    filePath += strAckFileName;
                    if (File.Exists(@filePath))
                    {
                        HttpResponse res = HttpContext.Current.Response;
                        res.Clear();
                        if (filePath.Contains(".pdf") == true)
                            res.ContentType = "application/pdf";
                        else
                            res.ContentType = "application/octet-stream";
                        res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                        res.WriteFile(filePath);
                        res.Flush();
                        res.End();
                    }
                }
                else
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        zip.AddDirectoryByName("Files");
                        string[] strFiles = strAckFileName.Split(',');

                        foreach (string filename in strFiles)
                        {
                            zip.AddFile(filePath + filename.Trim(), "Files");
                        }

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.BufferOutput = false;
                        string zipName = String.Format("BillAck_{0}.zip", DateTime.Now.ToString("dd-MM-yyyy-HHmmss"));
                        HttpContext.Current.Response.ContentType = "application/zip";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(HttpContext.Current.Response.OutputStream);
                        //foreach (string filePath in filePaths)
                        //{
                        //    File.Delete(filePath);
                        //}
                        HttpContext.Current.Response.End();
                    }
                }
            }

        }
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtFromDate.Text = "";
            //txtToDate.Text = "";
            filterOutstandingData();
        }

        protected void filterOutstandingData()
        {
            DataTable dt = (DataTable)Session["OutstandingDetails"];
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            if (dt != null)
            {
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("ClientName", typeof(string)));
                dt1.Columns.Add(new DataColumn("SiteName", typeof(string)));
                dt1.Columns.Add(new DataColumn("BillNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("BillDate", typeof(string)));
                dt1.Columns.Add(new DataColumn("TestingType", typeof(string)));
                dt1.Columns.Add(new DataColumn("BillAmount", typeof(string)));
                dt1.Columns.Add(new DataColumn("PendingAmount", typeof(string)));
                dt1.Columns.Add(new DataColumn("DaySpending", typeof(string)));
                dt1.Columns.Add(new DataColumn("MktUser", typeof(string)));
                dt1.Columns.Add(new DataColumn("RecoveryUser", typeof(string)));
                dt1.Columns.Add(new DataColumn("Route", typeof(string)));
                dt1.Columns.Add(new DataColumn("Limit", typeof(string)));
                dt1.Columns.Add(new DataColumn("BillWiseOutstanding", typeof(string)));
                dt1.Columns.Add(new DataColumn("OnAcBalance", typeof(string)));
                dt1.Columns.Add(new DataColumn("ActualOutstanding", typeof(string)));
                dt1.Columns.Add(new DataColumn("OUTW_AckDate_dt", typeof(string)));
                dt1.Columns.Add(new DataColumn("OUTW_BookedDate_dt", typeof(string)));
                dt1.Columns.Add(new DataColumn("OUTW_AckFileName_var", typeof(string)));
                dt1.Columns.Add(new DataColumn("RECV_Status_var", typeof(string)));
                dt1.Columns.Add(new DataColumn("RECV_BillBookedAmt_num", typeof(string)));
                //dt1.Columns.Add(new DataColumn("RECV_ClearedFromSite_var", typeof(string)));
                dt1.Columns.Add(new DataColumn("RECV_Remark_var", typeof(string)));

                int rowNo = 1;
                decimal totalAmount = 0, totalOutstanding = 0, totalBillWiseOutstanding = 0, totalOnAcBalance = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((dt.Rows[i]["TestingType"].ToString() == ddlInwardTestType.SelectedItem.Text || ddlInwardTestType.SelectedItem.Text == "---All---") &&
                        (dt.Rows[i]["ClientName"].ToString() == ddlClientSort.SelectedItem.Text || ddlClientSort.SelectedItem.Text == "---All---") &&
                        (dt.Rows[i]["MktUser"].ToString() == ddlMktUserSort.SelectedItem.Text || ddlMktUserSort.SelectedItem.Text == "---All---") &&
                        //(dt.Rows[i]["RecoveryUser"].ToString() == ddlRecoveryUserSort.SelectedItem.Text || ddlRecoveryUserSort.SelectedItem.Text == "---All---") &&
                        (txtDaysFrom.Text == "" || txtDaysTo.Text == "" || (Convert.ToInt32(dt.Rows[i]["DaySpending"].ToString()) >= Convert.ToInt32(txtDaysFrom.Text) && Convert.ToInt32(dt.Rows[i]["DaySpending"].ToString()) <= Convert.ToInt32(txtDaysTo.Text)))
                        )
                    {
                        dr1 = dt1.NewRow();
                        dr1["SrNo"] = rowNo;
                        dr1["ClientName"] = dt.Rows[i]["ClientName"];
                        dr1["SiteName"] = dt.Rows[i]["SiteName"];
                        dr1["BillNo"] = dt.Rows[i]["BillNo"];
                        dr1["BillDate"] = dt.Rows[i]["BillDate"];
                        dr1["TestingType"] = dt.Rows[i]["TestingType"];
                        dr1["BillAmount"] = dt.Rows[i]["BillAmount"];
                        dr1["PendingAmount"] = dt.Rows[i]["PendingAmount"];
                        dr1["DaySpending"] = dt.Rows[i]["DaySpending"];
                        dr1["MktUser"] = dt.Rows[i]["MktUser"];
                        dr1["RecoveryUser"] = dt.Rows[i]["RecoveryUser"];
                        dr1["Route"] = dt.Rows[i]["Route"];
                        dr1["Limit"] = dt.Rows[i]["Limit"];
                        dr1["BillWiseOutstanding"] = dt.Rows[i]["BillWiseOutstanding"];
                        dr1["OnAcBalance"] = dt.Rows[i]["OnAcBalance"];
                        dr1["ActualOutstanding"] = dt.Rows[i]["ActualOutstanding"];
                        if (dt.Rows[i]["BillAmount"].ToString() != "")
                            totalAmount += Convert.ToDecimal(dt.Rows[i]["BillAmount"].ToString());                        
                        if (dt.Rows[i]["PendingAmount"].ToString() != "")
                            totalOutstanding += Convert.ToDecimal(dt.Rows[i]["PendingAmount"].ToString());
                        if (dt.Rows[i]["BillWiseOutstanding"].ToString() != "")
                            totalBillWiseOutstanding += Convert.ToDecimal(dt.Rows[i]["BillWiseOutstanding"].ToString());
                        if (dt.Rows[i]["OnAcBalance"].ToString() != "")
                            totalOnAcBalance += Convert.ToDecimal(dt.Rows[i]["OnAcBalance"].ToString());
                        dr1["OUTW_AckDate_dt"] = dt.Rows[i]["OUTW_AckDate_dt"];
                        dr1["OUTW_BookedDate_dt"] = dt.Rows[i]["OUTW_BookedDate_dt"];
                        dr1["OUTW_AckFileName_var"] = dt.Rows[i]["OUTW_AckFileName_var"];
                        dr1["RECV_Status_var"] = dt.Rows[i]["RECV_Status_var"];
                        dr1["RECV_BillBookedAmt_num"] = dt.Rows[i]["RECV_BillBookedAmt_num"];
                        //dr1["RECV_ClearedFromSite_var"] = dt.Rows[i]["RECV_ClearedFromSite_var"];
                        dr1["RECV_Remark_var"] = dt.Rows[i]["RECV_Remark_var"];
                        dt1.Rows.Add(dr1);
                        rowNo++;
                    }
                }
                grdOutstanding.DataSource = dt1;
                grdOutstanding.DataBind();
                
                if (grdOutstanding.Columns[7].Visible == true)
                    lblTotal.Text = "Total : " + totalAmount.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Pending : " + totalOutstanding.ToString("0.00");
                    //lblTotal.Text = "Bill Total : " + totalAmount.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Total Pending : " + totalBillWiseOutstanding.ToString("0.00");
                else
                    lblTotal.Text = "Pending : " + totalBillWiseOutstanding.ToString("0.00");
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

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdOutstanding.Rows.Count > 0 && grdOutstanding.Visible == true)
            {
                string Subheader = "";
                if (lblFromDate.Visible == true && txtFromDate.Visible == true)
                {
                    //Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + lblClient.Text + "|" + ddlClient.SelectedItem.Text;
                    Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text;
                    if (chkClientSpecific.Checked == true)
                        Subheader += "|" + "Client" + "|" + txt_Client.Text;
                    else
                        Subheader += "|" + "" + "|" + "";
                }
                else
                {
                    //Subheader = "" + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + lblClient.Text + "|" + ddlClient.SelectedItem.Text;
                    Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text;
                    if (chkClientSpecific.Checked == true)
                        Subheader += "|" + "Client" + "|" + txt_Client.Text;
                    else
                        Subheader += "|" + "" + "|" + "";
                }

                PrintGrid.PrintGridView(grdOutstanding, Subheader, "Outstanding_Bills");
            }
        }

        protected void lnkPrintBalanceCertificate_Click(object sender, EventArgs e)
        {

        }

        protected void lnkApply_Click(object sender, EventArgs e)
        {
            ddlClientSort.SelectedValue = "---All---";
            ddlMktUserSort.SelectedValue = "---All---";
            //ddlRecoveryUserSort.SelectedValue = "---All---";
            filterOutstandingData();
        }

        protected void lnkUpdateClientOutstandng_Click(object sender, EventArgs e)
        {
            if (lblOutstanding.Text != "")
            {
                string[] strBal = lblOutstanding.Text.Split('|');
                dc.Client_Update(Convert.ToInt32(strBal[0]), "", 0, "", "", "", "", "", "", "", "", "", "", "", "", "", strBal[1] + "|" + strBal[2] + "|" + strBal[3], "", false, "", "", "", "", "", 0, "", null, false,"",0);
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

        protected void lnkDisplaySiteWise_Click(object sender, EventArgs e)
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
            dt.Columns.Add(new DataColumn("DaySpending", typeof(string)));
            dt.Columns.Add(new DataColumn("MktUser", typeof(string)));
            dt.Columns.Add(new DataColumn("RecoveryUser", typeof(string)));
            dt.Columns.Add(new DataColumn("Route", typeof(string)));
            dt.Columns.Add(new DataColumn("Limit", typeof(string)));
            dt.Columns.Add(new DataColumn("BillWiseOutstanding", typeof(string)));
            dt.Columns.Add(new DataColumn("OnAcBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("ActualOutstanding", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTW_AckDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTW_BookedDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTW_AckFileName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Status_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_BillBookedAmt_num", typeof(string)));
            //dt.Columns.Add(new DataColumn("RECV_ClearedFromSite_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Remark_var", typeof(string)));

            int i = 0;
            decimal totalAmount = 0, totalOutstanding = 0, actualOutstanding = 0, totalOnAcBalance = 0, billOutstanding = 0, totalBillWiseOutstanding = 0; //below90Outstanding = 0, above90Outstanding = 0,
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strFromDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            string strClient = "|", strMktUser = "|", strRecoveryUser = "|";
            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            clsData obj = new clsData();
            int prevClientId = 0;
            DataTable dtClientList = obj.getClientSiteOutstanding2(ClientId, strFromDate, strToDate, Convert.ToInt32(chkAsOnDate.Checked));
            for (int cllist = 0; cllist < dtClientList.Rows.Count; cllist++)
            {
                DataTable dtClient = obj.getClientSiteOutstanding3(Convert.ToInt32(dtClientList.Rows[cllist]["BILL_CL_Id"]), Convert.ToInt32(dtClientList.Rows[cllist]["BILL_SITE_Id"]), strFromDate, strToDate, Convert.ToInt32(chkAsOnDate.Checked));
                for (int cl = 0; cl < dtClient.Rows.Count; cl++)
                {
                    
                    billOutstanding = Convert.ToDecimal(dtClient.Rows[cl]["billAmountSite"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmountSite"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmountBill"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmountJournal"]);
                    totalBillWiseOutstanding += billOutstanding;

                    i++;
                    dr1 = dt.NewRow();
                    dr1["SrNo"] = i;
                    if (prevClientId != Convert.ToInt32(dtClientList.Rows[cllist]["BILL_CL_Id"]))
                    {
                        actualOutstanding = Convert.ToDecimal(dtClient.Rows[cl]["billAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmount"]);
                        totalOutstanding += actualOutstanding;
                        totalOnAcBalance += Convert.ToDecimal(dtClient.Rows[cl]["OnAccAmount"]);                        
                        if (dtClient.Rows[cl]["CL_Limit_mny"].ToString() != "")
                            dr1["Limit"] = Convert.ToDouble(dtClient.Rows[cl]["CL_Limit_mny"]).ToString("0.00");
                        if (Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]) < 0)
                            dr1["OnAcBalance"] = (-1 * Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"])).ToString("0.00");
                        else
                            dr1["OnAcBalance"] = Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]).ToString("0.00");
                        dr1["ActualOutstanding"] = actualOutstanding.ToString("0.00");
                        
                    }
                    dr1["ClientName"] = dtClient.Rows[cl]["CL_Name_var"].ToString();
                    dr1["SiteName"] = dtClient.Rows[cl]["SITE_Name_var"].ToString();
                    dr1["BillWiseOutstanding"] = billOutstanding.ToString("0.00");

                    if (dtClient.Rows[cl]["mktUser"].ToString() != null && dtClient.Rows[cl]["mktUser"].ToString() != "")
                        dr1["MktUser"] = dtClient.Rows[cl]["mktUser"].ToString();
                    else
                        dr1["MktUser"] = "NA";
                    if (dtClient.Rows[cl]["routeName"].ToString() != null && dtClient.Rows[cl]["routeName"].ToString() != "")
                        dr1["Route"] = dtClient.Rows[cl]["routeName"].ToString();
                    else
                        dr1["Route"] = "NA";
                    //if (dtClient.recoveryUser != null && dtClient.recoveryUser != "")
                    //    dr1["RecoveryUser"] = dtClient.recoveryUser;
                    //else
                    //    dr1["RecoveryUser"] = "NA";                    
                    
                    if (strClient.Contains("|" + dr1["ClientName"] + "|") == false && dr1["ClientName"].ToString() != "")
                        strClient += dr1["ClientName"] + "|";
                    if (strMktUser.Contains("|" + dr1["MktUser"] + "|") == false && dr1["MktUser"].ToString() != "")
                        strMktUser += dr1["MktUser"] + "|";
                    //if (strRecoveryUser.Contains("|" + dr1["RecoveryUser"] + "|") == false && dr1["RecoveryUser"].ToString() != "")
                    //    strRecoveryUser += dr1["RecoveryUser"] + "|";

                    totalAmount += Convert.ToDecimal(dtClient.Rows[cl]["billAmountSite"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmountSite"]);
                    dt.Rows.Add(dr1);
                }
                prevClientId = Convert.ToInt32(dtClientList.Rows[cllist]["BILL_CL_Id"]);
            }
            grdOutstanding.DataSource = dt;
            grdOutstanding.DataBind();
            Session["OutstandingDetails"] = dt;
            ddlClientSort.Items.Clear();
            ddlClientSort.Items.Add("---All---");
            ddlMktUserSort.Items.Clear();
            ddlMktUserSort.Items.Add("---All---");
            //ddlRecoveryUserSort.Items.Clear();
            //ddlRecoveryUserSort.Items.Add("---All---");
            string[] strSearch = strClient.Split('|');
            for (int j = 1; j < strSearch.Count() - 1; j++)
            {
                ddlClientSort.Items.Add(strSearch[j]);
            }
            strSearch = strMktUser.Split('|');
            for (int j = 1; j < strSearch.Count() - 1; j++)
            {
                ddlMktUserSort.Items.Add(strSearch[j]);
            }
            //strSearch = strRecoveryUser.Split('|');
            //for (int j = 1; j < strSearch.Count() - 1; j++)
            //{
            //    ddlRecoveryUserSort.Items.Add(strSearch[j]);
            //}
            if (totalOnAcBalance < 0)
                totalOnAcBalance = -1 * totalOnAcBalance;
            lblTotal.Text = "Total Pending : " + totalBillWiseOutstanding.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "On A/c Balance : " + totalOnAcBalance.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Actual Pending : " + totalOutstanding.ToString("0.00");

            if (ClientId > 0)
            {
                lblOutstanding.Text = ClientId + "|" + totalOutstanding;
            }
            for (int col = 3; col <= 21; col++)
            {
                if (col <= 14 || col == 21)
                    grdOutstanding.Columns[col].Visible = false;
            }

        }

        protected void lnkPrintBillAckPendingList_Click(object sender, EventArgs e)
        {
            if (grdOutstanding.Rows.Count > 0)
                PrintGrid.PrintGridView_BillAckPendingList(grdOutstanding, "Bill acknowledgement pending list", "BillAckPending");
        }
    }
}