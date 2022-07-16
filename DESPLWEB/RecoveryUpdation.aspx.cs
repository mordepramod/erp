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
    public partial class RecoveryUpdation : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Recovery Updation";
                txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                chkAsOnDate.Checked = true;
                lblFromDate.Visible = false;
                txt_FromDate.Visible = false;
                lblToDate.Text = "Till Date";
                Session["RecoveryUserId"] = "0";
                LoadRecoveryUserList();
            }
        }
        private void LoadRecoveryUserList()
        {
            ddlRecoveryUser.DataTextField = "USER_Name_var";
            ddlRecoveryUser.DataValueField = "USER_Id";
            var users = dc.User_View_ClientRecoveryWise();
            ddlRecoveryUser.DataSource = users;
            ddlRecoveryUser.DataBind();
            ddlRecoveryUser.Items.Insert(0, new ListItem("---All---", "0"));
        }

        protected void chkAsOnDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAsOnDate.Checked == true)
            {
                lblFromDate.Visible = false;
                txt_FromDate.Visible = false;
                lblToDate.Text = "Till Date";
            }
            else
            {
                lblFromDate.Visible = true;
                txt_FromDate.Visible = true;
                lblToDate.Text = "To Date";
            }
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            DisplayBills();
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        public void DisplayBills()
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("BILL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("BILL_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("BILL_NetAmt_num", typeof(string)));
            dt.Columns.Add(new DataColumn("BILL_PendingAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("MEName", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("INWD_ContactNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("OUTW_AckFileName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Status_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_BillBookedAmt_num", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Reason_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_Remark_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_ExpectedPaymentDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("RECV_ExpectedAmt_num", typeof(string)));
            dt.Columns.Add(new DataColumn("PendingAmount", typeof(decimal)));

            DateTime tempDate;            
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            decimal totalAmount = 0, totalOutstanding = 0, clientOutstanding = 0, tempOutstanding = 0, billOutstanding = 0, PendingAmount = 0; //, below90Outstanding = 0, above90Outstanding = 0;
            string[] strDate = txt_FromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strFromDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            strDate = txt_ToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];

            int clId = 0, siteId = 0;
            if (lblClientId.Text != "0" && lblClientId.Text != "")
                clId = Convert.ToInt32(lblClientId.Text);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select client from list..');", true);

            //if (ddlSite.Items.Count > 0 && ddlSite.SelectedIndex > 0)
            //    siteId = Convert.ToInt32(ddlSite.SelectedValue);
            if (clId > 0)
            {
                var billList = dc.BillRecovery_View(clId, siteId, Fromdate, Todate, chkAsOnDate.Checked).ToList();
                //grdBills.DataSource = Inward;
                //grdBills.DataBind();
                clsData obj = new clsData();
                DataTable dtClient = obj.getClientOutstanding(clId, strFromDate, strToDate, Convert.ToInt32(chkAsOnDate.Checked));

                if (dtClient.Rows.Count > 0) 
                    clientOutstanding = Convert.ToDecimal(dtClient.Rows[0]["billAmount"]) + Convert.ToDecimal(dtClient.Rows[0]["journalDBAmount"]) + Convert.ToDecimal(dtClient.Rows[0]["receivedAmount"]);
                billOutstanding = 0;
                //var billList = dc.Bill_View_Outstanding(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, FromDate, ToDate, chkAsOnDate.Checked).ToList();
                foreach (var bill in billList)
                {
                    PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
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
                        tempDate = Convert.ToDateTime(bill.BILL_Date_dt);
                        dr1 = dt.NewRow();
                        
                        dr1["BILL_Id"] =bill.BILL_RecordType_var +" - " + bill.BILL_Id;
                        dr1["BILL_Date_dt"] = tempDate.ToString("dd/MM/yyyy");
                        dr1["BILL_NetAmt_num"] = bill.BILL_NetAmt_num;

                        PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                        if (tempOutstanding < PendingAmount && tempOutstanding > 0)
                        {
                            PendingAmount = PendingAmount - tempOutstanding;
                            tempOutstanding = 0;
                        }
                        clientOutstanding = clientOutstanding - PendingAmount;
                        totalAmount += Convert.ToDecimal(bill.BILL_NetAmt_num);
                        totalOutstanding += PendingAmount;

                        dr1["BILL_PendingAmount"] = PendingAmount.ToString("0.00");
                        dr1["PendingAmount"] = PendingAmount;
                        dr1["SITE_Name_var"] = bill.SITE_Name_var;
                        dr1["MEName"] = bill.MEName;
                        dr1["CONT_Name_var"] = bill.CONT_Name_var;
                        dr1["INWD_ContactNo_var"] = bill.INWD_ContactNo_var;
                        dr1["OUTW_AckFileName_var"] = bill.OUTW_AckFileName_var;
                        dr1["RECV_Id"] = bill.RECV_Id;
                        dr1["RECV_Status_var"] = bill.RECV_Status_var;
                        dr1["RECV_BillBookedAmt_num"] = bill.RECV_BillBookedAmt_num;
                        dr1["RECV_Reason_var"] = bill.RECV_Reason_var;
                        dr1["RECV_Remark_var"] = bill.RECV_Remark_var;
                        if (bill.RECV_ExpectedPaymentDate_dt != null)
                            dr1["RECV_ExpectedPaymentDate_dt"] = Convert.ToDateTime(bill.RECV_ExpectedPaymentDate_dt).ToString("dd/MM/yyyy") ;
                        if (bill.RECV_ExpectedAmt_num != null && bill.RECV_ExpectedAmt_num > 0)
                            dr1["RECV_ExpectedAmt_num"] = bill.RECV_ExpectedAmt_num;
                        dt.Rows.Add(dr1);
                    }
                    else
                    {
                        tempOutstanding = tempOutstanding - PendingAmount;
                    }
                }
                //if (dt != null)
                //{
                //    DataView dataView = new DataView(dt);
                //    dataView.Sort = "PendingAmount" + " " + "DESC";
                //    grdBills.DataSource = dataView;
                //    grdBills.DataBind();
                //}
                //else
                //{
                    grdBills.DataSource = dt;
                    grdBills.DataBind();
                //}
                lblRecords.Text = "Total Records   :  " + grdBills.Rows.Count;
                decimal totalBillPendingAmt = 0, totalBillBookedAmt = 0;
                for (int i = 0; i < grdBills.Rows.Count ; i++)
                {
                    TextBox txtBillBookedAmt = (TextBox)grdBills.Rows[i].FindControl("txtBillBookedAmt");
                    if (txtBillBookedAmt.Text != "")
                    {
                        totalBillBookedAmt += Convert.ToDecimal(txtBillBookedAmt.Text);
                    }
                    totalBillPendingAmt += Convert.ToDecimal(grdBills.Rows[i].Cells[4].Text);
                }
                lblTotal.Text = "Total pending amount : " + totalBillPendingAmt.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Total booked amount : " + totalBillBookedAmt.ToString("0.00"); ;
            }
            else
            {
                grdBills.DataSource = null;
                grdBills.DataBind();
                lblRecords.Text = "Total Records   :  " + "0";
                lblTotal.Text = "";
            }
            ddlStatusFilter.SelectedIndex = 0; 
        }
        protected void grdBills_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblReason = (Label)e.Row.FindControl("lblReason");
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                DropDownList ddlReason = (DropDownList)e.Row.FindControl("ddlReason");

                if (lblStatus.Text != "")
                    ddlStatus.SelectedValue = lblStatus.Text;
                if (lblReason.Text != "")
                    ddlReason.SelectedValue = lblReason.Text;
                
            }
        }
        protected void grdBills_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strBillNo = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "ViewBill")
            {
                PrintPDFReport obj = new PrintPDFReport();
                obj.Bill_PDFPrint(strBillNo, false, "Print");
            }
            else if (e.CommandName == "ViewAckFile")
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
                if (strBillNo.Contains(',') == false)
                {
                    filePath += strBillNo;
                    if (File.Exists(@filePath))
                    {
                        HttpResponse res = HttpContext.Current.Response;
                        res.Clear();
                        res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                        res.ContentType = "application/octet-stream";
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
                        string[] strFiles = strBillNo.Split(',');

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
            else if (e.CommandName == "UpdateBill")
            {
                GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowindex = grdrow.RowIndex;
                DropDownList ddlStatus = (DropDownList)grdBills.Rows[rowindex].FindControl("ddlStatus");
                TextBox txtBillBookedAmt = (TextBox)grdBills.Rows[rowindex].FindControl("txtBillBookedAmt");
                DropDownList ddlReason = (DropDownList)grdBills.Rows[rowindex].FindControl("ddlReason");
                TextBox txtRemark = (TextBox)grdBills.Rows[rowindex].FindControl("txtRemark");
                Label lblRecvId = (Label)grdBills.Rows[rowindex].FindControl("lblRecvId");
                TextBox txtExpectedPaymentDate = (TextBox)grdBills.Rows[rowindex].FindControl("txtExpectedPaymentDate");
                TextBox txtExpectedAmt = (TextBox)grdBills.Rows[rowindex].FindControl("txtExpectedAmt");

                bool updateFlag = true;
                if (ddlStatus.Text.Trim() == "---Select---")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Status..');", true);
                    ddlStatus.Focus();
                    updateFlag = false;
                }
                //else if (txtBillBookedAmt.Text.Trim() == "")
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Bill booked amount..');", true);
                //    txtBillBookedAmt.Focus();
                //    updateFlag = false;
                //}
                //else if (ddlReason.Text.Trim() == "---Select---" && ddlReason.Enabled == true)
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Reason..');", true);
                //    ddlReason.Focus();
                //    updateFlag = false;
                //}
                else if (txtRemark.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Remark..');", true);
                    txtRemark.Focus();
                    updateFlag = false;
                }

                if (updateFlag == true)
                {
                    int recvId = 0;
                    if (lblRecvId.Text != "")
                        recvId = Convert.ToInt32(lblRecvId.Text);
                    DateTime? expPaymentDate = null;
                    decimal expAmount = 0;
                    if (txtExpectedPaymentDate.Text.Trim() != "")
                    {
                        expPaymentDate = DateTime.ParseExact(txtExpectedPaymentDate.Text, "dd/MM/yyyy", null);
                    }
                    if (txtExpectedAmt.Text.Trim() != "")
                    {
                        expAmount = Convert.ToDecimal(txtExpectedAmt.Text);
                    }
                    string reason = "";
                    if (ddlReason.SelectedValue == "---Select---")
                    {
                        reason = "";
                    }
                    else
                    {
                        reason = ddlReason.SelectedValue;
                    }
                    //strBillNo

                    if (txtBillBookedAmt.Text.Trim() == "")
                    {
                        txtBillBookedAmt.Text = "0";
                    }

                    if (strBillNo.Contains('-'))
                    { 
                        string[] strval1 = strBillNo.Split('-');
                        strBillNo= strval1[ strval1.GetLength(0)-1 ].ToString().TrimStart();
                    }
                    dc.BillRecovery_Update(recvId, strBillNo, ddlStatus.SelectedValue, Convert.ToDecimal(txtBillBookedAmt.Text), reason, txtRemark.Text, expPaymentDate, expAmount);
                    DisplayBills();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Updated Successfully..');", true);
                }
            }
            else if (e.CommandName == "AddFollowup")
            {
                lblFollowupBillNo.Text = "Bill No. : " + strBillNo;
                txtDescription.Text = "";
                LoadBillFollowup(strBillNo); 
                ModalPopupExtender1.Show();
            }
        }
        protected void lnkUpdateAllSel_Click(object sender, EventArgs e)
        {
            bool selectedFlag = false;
            bool updateFlag = true;
            if (grdBills.Rows.Count > 0)
            {
                for (int i = 0; i < grdBills.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdBills.Rows[i].FindControl("chkSelect");
                    DropDownList ddlStatus = (DropDownList)grdBills.Rows[i].FindControl("ddlStatus");
                    TextBox txtBillBookedAmt = (TextBox)grdBills.Rows[i].FindControl("txtBillBookedAmt");
                    DropDownList ddlReason = (DropDownList)grdBills.Rows[i].FindControl("ddlReason");
                    TextBox txtRemark = (TextBox)grdBills.Rows[i].FindControl("txtRemark");
                    
                    if (chkSelect.Checked == true)
                    {
                        selectedFlag = true;
                        if (ddlStatus.Text.Trim() == "---Select---")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Status..');", true);
                            ddlStatus.Focus();
                            updateFlag = false;
                            break;
                        }
                        //else if (txtBillBookedAmt.Text.Trim() == "")
                        //{
                        //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Bill booked amount..');", true);
                        //    txtBillBookedAmt.Focus();
                        //    updateFlag = false;
                        //    break;
                        //}
                        //else if (ddlReason.Text.Trim() == "---Select---")
                        //{
                        //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Reason..');", true);
                        //    ddlReason.Focus();
                        //    updateFlag = false;
                        //    break;
                        //}
                        else if (txtRemark.Text.Trim() == "")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Remark..');", true);
                            txtRemark.Focus();
                            updateFlag = false;
                            break;
                        }
                    }
                }
            }
            if (selectedFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one row');", true);
                updateFlag = false;
            }            
            if (updateFlag == true)
            {
                for (int i = 0; i < grdBills.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdBills.Rows[i].FindControl("chkSelect");
                    DropDownList ddlStatus = (DropDownList)grdBills.Rows[i].FindControl("ddlStatus");
                    TextBox txtBillBookedAmt = (TextBox)grdBills.Rows[i].FindControl("txtBillBookedAmt");
                    DropDownList ddlReason = (DropDownList)grdBills.Rows[i].FindControl("ddlReason");
                    TextBox txtRemark = (TextBox)grdBills.Rows[i].FindControl("txtRemark");
                    Label lblRecvId = (Label)grdBills.Rows[i].FindControl("lblRecvId");
                    TextBox txtExpectedPaymentDate = (TextBox)grdBills.Rows[i].FindControl("txtExpectedPaymentDate");
                    TextBox txtExpectedAmt = (TextBox)grdBills.Rows[i].FindControl("txtExpectedAmt");
                    if (chkSelect.Checked == true)
                    {
                        string strBillNo = grdBills.Rows[i].Cells[1].Text;
                        int recvId = 0;
                        if (lblRecvId.Text != "")
                            recvId = Convert.ToInt32(lblRecvId.Text);
                        DateTime? expPaymentDate = null;
                        decimal expAmount = 0;
                        if (txtExpectedPaymentDate.Text.Trim() != "")
                        {
                            expPaymentDate = DateTime.ParseExact(txtExpectedPaymentDate.Text, "dd/MM/yyyy", null);
                        }
                        if (txtExpectedAmt.Text.Trim() != "")
                        {
                            expAmount = Convert.ToDecimal(txtExpectedAmt.Text);
                        }

                        string reason = "";
                        if (ddlReason.SelectedValue == "---Select---")
                        {
                            reason = "";
                        }
                        else
                        {
                            reason = ddlReason.SelectedValue;
                        }
                        //strBillNo

                        if (txtBillBookedAmt.Text.Trim() == "")
                        {
                            txtBillBookedAmt.Text = "0";
                        }

                        if (strBillNo.Contains('-'))
                        {
                            string[] strval1 = strBillNo.Split('-');
                            //strBillNo = strval1[1].ToString().TrimStart();
                            strBillNo = strval1[strval1.GetLength(0) - 1].ToString().TrimStart();
                        }


                        dc.BillRecovery_Update(recvId, strBillNo, ddlStatus.SelectedValue, Convert.ToDecimal(txtBillBookedAmt.Text), reason, txtRemark.Text, expPaymentDate, expAmount);
                    }
                }
                DisplayBills();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Updated Successfully..');", true);
            }
        }

        private void ClearBillList()
        {
            lblRecords.Text = "";
            lblTotal.Text = "";
            //grdBills.Visible = false;
            grdBills.DataSource = null;
            grdBills.DataBind();
        }
        
        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdBills.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdBills.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdBills.Rows.Count > 0)
            {
                PrintGrid.PrintGridViewBillRecovery(grdBills, "Recovery Updation", "BillRecovery");
            }
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
            txtClientAddress.Text = "";
            txtClientContactPerson.Text = "";
            txtClientPhoneNo.Text = "";
            txtClientEmailId.Text = "";
            chkClientUnderReconciliation.Checked = false;
            txtSiteAddress.Text = "";
            txtSiteContactPerson.Text = "";
            txtSitePhoneNo.Text = "";
            txtSiteEmailId.Text = "";
            ddlSite.DataSource = null;
            ddlSite.DataBind();
            
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                    LoadSiteList();
                    var client = dc.Client_View(Convert.ToInt32(lblClientId.Text), -1, "", "");
                    foreach (var cl in client)
                    {
                        txtClientAddress.Text = cl.CL_OfficeAddress_var;
                        txtClientContactPerson.Text = cl.CL_AccContactName_var;
                        txtClientPhoneNo.Text = cl.CL_AccContactNo_var;
                        txtClientEmailId.Text = cl.CL_AccEmailId_var;
                        chkClientUnderReconciliation.Checked = Convert.ToBoolean(cl.CL_UnderReconciliation_bit);
                    }
                }
            }
        }

        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            if (ddlRecoveryUser.SelectedIndex == 0)
            {
                var query = dc.Client_View(0, 0, searchHead, "");
                foreach (var obj in query)
                {
                    valid = true;
                }
            }
            else
            {
                var query = dc.Client_View_RecoveryUserWise(searchHead, Convert.ToInt32(ddlRecoveryUser.SelectedValue));
                foreach (var obj in query)
                {
                    valid = true;
                }
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
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            if (HttpContext.Current.Session["RecoveryUserId"].ToString() == "0")
            {
                var query = db.Client_View(0, -1, searchHead, "");                
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
            }
            else
            {
                var query = db.Client_View_RecoveryUserWise(searchHead, Convert.ToInt32(HttpContext.Current.Session["RecoveryUserId"].ToString()));
                foreach (var rowObj in query)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
                if (row == null)
                {
                    var clnm = db.Client_View_RecoveryUserWise("", Convert.ToInt32(HttpContext.Current.Session["RecoveryUserId"].ToString()));
                    foreach (var rowObj in clnm)
                    {
                        row = dt.NewRow();
                        string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                        dt.Rows.Add(item);
                    }
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }

        private void LoadSiteList()
        {
            var site = dc.Site_View(0, Convert.ToInt32(lblClientId.Text), 0, "");
            ddlSite.DataSource = site;
            ddlSite.DataTextField = "Site_Name_var";
            ddlSite.DataValueField = "Site_Id";
            ddlSite.DataBind();
            ddlSite.Items.Insert(0, new ListItem("----All----", "0"));
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSiteAddress.Text = "";
            txtSiteContactPerson.Text = "";
            txtSitePhoneNo.Text = "";
            txtSiteEmailId.Text = "";
            if (ddlSite.SelectedIndex > 0)
            {
                var site = dc.Site_View(Convert.ToInt32(ddlSite.SelectedValue), 0, 0, "");
                foreach (var st in site)
                {
                    txtSiteAddress.Text = st.SITE_Address_var;
                    txtSiteContactPerson.Text = st.SITE_Incharge_var;
                    txtSitePhoneNo.Text = st.SITE_IncMobNo_var;
                    txtSiteEmailId.Text = st.SITE_EmailID_var;
                }
            }
       
        }

        protected void lnkUpdateClAsUnderRecon_Click(object sender, EventArgs e)
        {
            int clId = 0;
            if (lblClientId.Text != "0" && lblClientId.Text != "")
            {
                clId = Convert.ToInt32(lblClientId.Text);
                dc.Client_Update_UnderReconciliation(clId, chkClientUnderReconciliation.Checked);
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client Under Reconciliation status updated successfully..');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select client from list..');", true);
            }
        }
        protected void lnkUpdateClientContactDetails_Click(object sender, EventArgs e)
        {
            if (lblClientId.Text != "0" && lblClientId.Text != "")
            {
                dc.Client_Update_ContactDetails(Convert.ToInt32(lblClientId.Text), txtClientAddress.Text, txtClientContactPerson.Text, txtClientPhoneNo.Text, txtClientEmailId.Text);
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Contact details updated successfully..');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select client from list..');", true);
            }
        }
        protected void lnkUpdateSiteContactDetails_Click(object sender, EventArgs e)
        {
            if (ddlSite.Items.Count > 0 && ddlSite.SelectedIndex > 0)
            {
                dc.Site_Update_ContactDetails(Convert.ToInt32(ddlSite.SelectedValue), txtSiteAddress.Text, txtSiteContactPerson.Text, txtSitePhoneNo.Text, txtSiteEmailId.Text);
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Contact details updated successfully..');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select site from list..');", true);
            }
        }

        protected void ddlRecoveryUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearBillList();
            txt_Client.Text = "";
            lblClientId.Text = "0";
            txtClientAddress.Text = "";
            txtClientContactPerson.Text = "";
            txtClientPhoneNo.Text = "";
            txtClientEmailId.Text = "";
            chkClientUnderReconciliation.Checked = false;
            txtSiteAddress.Text = "";
            txtSiteContactPerson.Text = "";
            txtSitePhoneNo.Text = "";
            txtSiteEmailId.Text = "";
            ddlSite.Items.Clear();
            ddlSite.DataSource = null;
            ddlSite.DataBind();

            if (ddlRecoveryUser.SelectedIndex >= 0)
            {
                Session["RecoveryUserId"] = ddlRecoveryUser.SelectedValue;
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdBills.Rows.Count; i++)
            {
                Label lblStatus = (Label)grdBills.Rows[i].FindControl("lblStatus");                
                if (ddlStatusFilter.SelectedIndex == 0 || ddlStatusFilter.SelectedValue == lblStatus.Text)
                {
                    grdBills.Rows[i].Visible = true;
                }
                else
                {
                    grdBills.Rows[i].Visible = false;
                }
            }
        }
        private void LoadBillFollowup(string BillNo)
        {
            var followup = dc.RecoveryFollowup_View(BillNo);
            grdFollowup.DataSource = followup;
            grdFollowup.DataBind();
        }
        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            if (txtDescription.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", "alert('Enter description. ')", true);
                txtDescription.Focus();
            }
            else 
            {
                dc.RecoveryFollowup_Update(lblFollowupBillNo.Text.Replace("Bill No. : " ,""), txtDescription.Text.Trim(), Convert.ToInt32(Session["LoginId"]));
                LoadBillFollowup(lblFollowupBillNo.Text.Replace("Bill No. : ", ""));
                ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alert", "alert('Added Successfully. ')", true);
            }
        }

        protected void imgCloseDetails_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
    }

}
