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
    public partial class ReportApproveCRLimit : System.Web.UI.Page
    {   
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Approve CR Limit For Report";
                bool superAccountRight = false;
                var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAccount_right_bit == true || u.USER_OneDayBypassCrL_right_bit == true)
                        superAccountRight = true;
                }
                if (superAccountRight == true)
                {   
                    LoadInwardType();
                    LoadReasonList();
                    LoadMEList();
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        protected void LoadMEList()
        {
            var MEList = dc.User_View_ME(-1);
            ddlMESel.DataSource = MEList;
            ddlMESel.DataTextField = "USER_Name_var";
            ddlMESel.DataValueField = "USER_Id";
            ddlMESel.DataBind();
            ddlMESel.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, new ListItem("---All---", "0"));
        }

        private void LoadReasonList()
        {
            var reason = dc.CreditReason_View("");
            ddlReasonSel.DataTextField = "CrR_Reason_var";
            ddlReasonSel.DataValueField = "CrR_Id";
            ddlReasonSel.DataSource = reason;
            ddlReasonSel.DataBind();
            ddlReasonSel.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdReports.Visible = true;
            DisplayReports();
        }

        public void DisplayReports()
        {
            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            
            string finalStatus = "";
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                finalStatus = " " + ddlMF.SelectedValue;
            }

            if (ddl_InwardTestType.SelectedItem.Text == "---All---")
            {
                var Inward = dc.ReportStatus_View_CRLimitExceedAll(ClientId).ToList();
                grdReports.DataSource = Inward;
                grdReports.DataBind();
            }
            else
            {
                var Inward = dc.ReportStatus_View_CRLimitExceed(ddl_InwardTestType.SelectedItem.Text + finalStatus, ClientId).ToList();
                grdReports.DataSource = Inward;
                grdReports.DataBind();
            }
            lbl_RecordsNo.Text = "Total Records   :  " + grdReports.Rows.Count;
            
            for (int i = 0; i < grdReports.Rows.Count - 1; i++)
            {
                if (grdReports.Rows[i].Cells[1].Text != "")
                {
                    for (int j = i + 1; j < grdReports.Rows.Count; j++)
                    {
                        if (grdReports.Rows[i].Cells[1].Text == grdReports.Rows[j].Cells[1].Text &&
                            grdReports.Rows[i].Cells[2].Text == grdReports.Rows[j].Cells[2].Text)
                        {
                            grdReports.Rows[j].Cells[1].Text = "";
                            grdReports.Rows[j].Cells[2].Text = "";
                            grdReports.Rows[j].Cells[3].Text = "";
                            LinkButton lnkBalanceAmt = (LinkButton)grdReports.Rows[j].FindControl("lnkBalanceAmt");
                            lnkBalanceAmt.Visible = false;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlReason = (e.Row.FindControl("ddlReason") as DropDownList);
                var reason = dc.CreditReason_View("");
                ddlReason.DataTextField = "CrR_Reason_var";
                ddlReason.DataValueField = "CrR_Id";
                ddlReason.DataSource = reason;
                ddlReason.DataBind();
                ddlReason.Items.Insert(0, new ListItem("---Select---", "0"));

                DropDownList ddlME = (e.Row.FindControl("ddlME") as DropDownList);
                var MEList = dc.User_View_ME(-1);
                ddlME.DataSource = MEList;
                ddlME.DataTextField = "USER_Name_var";
                ddlME.DataValueField = "USER_Id";
                ddlME.DataBind();
                ddlME.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
        
        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[3];
            arg = strReportDetails.Split(';');

            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = grdrow.RowIndex;
            CheckBox cbxSelect = (CheckBox)grdReports.Rows[rowindex].FindControl("cbxSelect");
            DropDownList ddlReason = (DropDownList)grdReports.Rows[rowindex].FindControl("ddlReason");
            TextBox txtDate = (TextBox)grdReports.Rows[rowindex].FindControl("txtDate");
            TextBox txtAmount = (TextBox)grdReports.Rows[rowindex].FindControl("txtAmount");
            DropDownList ddlME = (DropDownList)grdReports.Rows[rowindex].FindControl("ddlME");
            Label lblClientId = (Label)grdReports.Rows[rowindex].FindControl("lblClientId");
            Label lblSiteId = (Label)grdReports.Rows[rowindex].FindControl("lblSiteId");

            if (e.CommandName == "ApproveCRLimit")
            {
                string Recordtype = Convert.ToString(arg[0]);
                int RecordNo = Convert.ToInt32(arg[1]);
                string ReferenceNo = Convert.ToString(arg[2]);
                string testType = Convert.ToString(arg[3]);
                if (testType == "&nbsp;")
                {
                    testType = "";
                }
                decimal expectedAmount = 0;
                DateTime? expectedDate = null;

                if (ddlReason.SelectedIndex > 0)
                {
                    expectedAmount = 0;
                    expectedDate = null;
                    if (txtAmount.Text != "")
                    {
                        expectedAmount = Convert.ToDecimal(txtAmount.Text);
                    }
                    if (txtDate.Text != "")
                    {
                        expectedDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);
                    }                    
                    //dc.MISDetail_Update_CRLDetail(RecordNo, Recordtype, ReferenceNo, testType, txtReason.Text, true);
                    dc.MISDetail_Update_CRLDetail(Recordtype, ReferenceNo, testType, true ,Convert.ToInt32(lblClientId.Text));
                    dc.CreditFollowup_Update(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), ReferenceNo, ddlReason.SelectedItem.Text, expectedDate, expectedAmount, DateTime.Now, Convert.ToInt32(ddlME.SelectedItem.Value));
                    for (int i = 0; i < grdReports.Rows.Count; i++)
                    {
                        string[] refNo1 = ReferenceNo.Split('/');
                        string[] refNo2 = grdReports.Rows[i].Cells[7].Text.Split('/');
                        if (refNo1[0] == refNo2[0] && ReferenceNo != grdReports.Rows[i].Cells[7].Text)
                        {
                            string tempTestType = grdReports.Rows[i].Cells[10].Text;
                            if (tempTestType == "&nbsp;")
                            {
                                tempTestType = "";
                            }
                            dc.MISDetail_Update_CRLDetail(grdReports.Rows[i].Cells[5].Text, grdReports.Rows[i].Cells[7].Text, tempTestType, true,Convert.ToInt32(lblClientId.Text));
                        }
                    }
                    DisplayReports();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Approved successfully.');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please input reason.');", true);
                }
                    
            }
            else if (e.CommandName == "ViewReason")
            {
                int ClientId = Convert.ToInt32(arg[0]);
                var misDetail = dc.MISDetail_View_CRLDetail(ClientId);
                grdReason.DataSource = misDetail;
                grdReason.DataBind();
                ModalPopupExtender1.Show();
            }
            else if (e.CommandName == "ViewOutstanding")
            {
                int ClientId = Convert.ToInt32(arg[0]);
                LoadOutstandingBillList(ClientId);
                ModalPopupExtender3.Show();
            }
        }

        protected void LoadOutstandingBillList(int ClientId)
        {
            for (int col = 3; col <= 8; col++)
            {
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
            dt.Columns.Add(new DataColumn("Route", typeof(string)));
            dt.Columns.Add(new DataColumn("Limit", typeof(string)));
            dt.Columns.Add(new DataColumn("BillWiseOutstanding", typeof(string)));
            dt.Columns.Add(new DataColumn("OnAcBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("ActualOutstanding", typeof(string)));

            DateTime tempDate;
            int i = 0;
            decimal totalAmount = 0, totalOutstanding = 0, actualOutstanding = 0, totalOnAcBalance = 0, billOutstanding = 0, PendingAmount = 0, below90Outstanding = 0, above90Outstanding = 0, totalBillWiseOutstanding = 0;
            string[] strDate = DateTime.Now.ToString("dd/MM/yyyy").Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            
            clsData obj = new clsData();
            DataTable dtClientList = obj.getClientOutstanding2(ClientId, strToDate, strToDate, 1);
            for (int cllist = 0; cllist < dtClientList.Rows.Count; cllist++)
            {

                DataTable dtClient = obj.getClientOutstanding3(Convert.ToInt32(dtClientList.Rows[cllist]["BILL_CL_Id"]), strToDate, strToDate, 1);
                for (int cl = 0; cl < dtClient.Rows.Count; cl++)
                {
                    actualOutstanding = Convert.ToDecimal(dtClient.Rows[cl]["billAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["journalDBAmount"]) + Convert.ToDecimal(dtClient.Rows[cl]["receivedAmount"]);
                    totalOutstanding += actualOutstanding;
                    totalOnAcBalance += Convert.ToDecimal(dtClient.Rows[cl]["OnAccAmount"]);
                    int rowNo = 0;
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
                            if (Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]) < 0)
                                dr1["OnAcBalance"] = (-1 * Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"])).ToString("0.00");
                            else
                                dr1["OnAcBalance"] = Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]).ToString("0.00");
                            dr1["ActualOutstanding"] = actualOutstanding.ToString("0.00");

                        }
                        rowNo++;

                        dr1["ClientName"] = bill.CL_Name_var;
                        dr1["SiteName"] = bill.SITE_Name_var;
                        dr1["BillNo"] = bill.BILL_Id;
                        dr1["BillDate"] = tempDate.ToString("dd/MM/yyyy");
                        dr1["TestingType"] = bill.testtype;
                        dr1["BillAmount"] = bill.BILL_NetAmt_num;

                        PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                        //actualOutstanding = actualOutstanding - PendingAmount;
                        totalAmount += Convert.ToDecimal(bill.BILL_NetAmt_num);

                        dr1["PendingAmount"] = PendingAmount.ToString("0.00");
                        dr1["DaySpending"] = (DateTime.ParseExact(strDate[0] + "/" + strDate[1] + "/" + strDate[2], "dd/MM/yyyy", null) - Convert.ToDateTime(bill.BILL_Date_dt)).Days;
                        
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
                            if (Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]) < 0)
                                dr1["OnAcBalance"] = (-1 * Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"])).ToString("0.00");
                            else
                                dr1["OnAcBalance"] = Convert.ToDouble(dtClient.Rows[cl]["OnAccAmount"]).ToString("0.00");
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
                        dr1["DaySpending"] = (DateTime.ParseExact(strDate[0] + "/" + strDate[1] + "/" + strDate[2], "dd/MM/yyyy", null) - Convert.ToDateTime(db.Journal_Date_dt)).Days;
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
                        
                        dt.Rows.Add(dr1);

                    }
                }
            }
            grdOutstanding.DataSource = dt;
            grdOutstanding.DataBind();
            if (totalOnAcBalance < 0)
                totalOnAcBalance = -1 * totalOnAcBalance;
            lblTotal.Text = "Bill Total : " + totalAmount.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Total Pending : " + totalBillWiseOutstanding.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "On A/c Balance : " + totalOnAcBalance.ToString("0.00") + "&nbsp;&nbsp;&nbsp;" + "Actual Pending : " + totalOutstanding.ToString("0.00");

        }
        protected void imgCloseDetails_Click(object sender, ImageClickEventArgs e)
        {
            grdReason.DataSource = null;
            grdReason.DataBind();
            ModalPopupExtender1.Hide();
        }

        protected void imgCloseReason_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender2.Hide();
        }

        protected void imgCloseOutstandingList_Click(object sender, ImageClickEventArgs e)
        {
            grdOutstanding.DataSource = null;
            grdOutstanding.DataBind();
            ModalPopupExtender3.Hide();
        }

        private void ClearReportList()
        {
            grdReports.Visible = false;
            lbl_RecordsNo.Text = "";
            grdReports.DataSource = null;
            grdReports.DataBind();
        }

        
        protected void ddlMF_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
        }

        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                ddlMF.Visible = true;
            }
            else
            {
                ddlMF.Visible = false;
            }
        }

        protected void lnkAddReason_Click(object sender, EventArgs e)
        {
            txtReason.Text = "";
            ModalPopupExtender2.Show();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            txtReason.Text = txtReason.Text.Trim();
            bool addStatus = true;
            if (txtReason.Text == "")
            {
                addStatus = false;
                txtReason.Focus();
                ScriptManager.RegisterStartupScript(UpdatePanel3, UpdatePanel3.GetType(), "alert", "alert('Enter reason. ')", true);
            }
            else
            {
                //check for duplicate credit reason
                var reason = dc.CreditReason_View(txtReason.Text);
                if (reason.Count() > 0)
                {
                    addStatus = false;
                    txtReason.Focus();
                    ScriptManager.RegisterStartupScript(UpdatePanel3, UpdatePanel3.GetType(), "alert", "alert('Reason already available. ')", true);
                }
            }
            if (addStatus == true)
            {
                dc.CreditReason_Update(txtReason.Text);
                LoadReasonList();
                for (int i = 0; i < grdReports.Rows.Count -1 ; i++)
                {
                    DropDownList ddlReason = (DropDownList)grdReports.Rows[i].FindControl("ddlReason") ;
                    ddlReason.Items.Add(new ListItem(txtReason.Text, ""));
                }
                txtReason.Text = "";
                //ClientScript.RegisterStartupScript(UpdatePanel3.GetType(), "myalert", "alert('Reason added successfully.');", true);
                ScriptManager.RegisterStartupScript(UpdatePanel3, UpdatePanel3.GetType(), "alert", "alert('Reason added successfully. ')", true);
            }
            
        }

        protected void lnkApproveSelReports_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            //if (ddlReasonSel.SelectedIndex <= 0)
            //{
            //    ddlReasonSel.Focus();
            //    strMsg = "Please input reason.";
            //}
            //else if (txtDateSel.Text == "")
            //{
            //    txtDateSel.Focus();
            //    strMsg = "Please input expected date of payment.";
            //}
            //else
            //{
                bool selFlag = false;
                for (int i = 0; i < grdReports.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdReports.Rows[i].FindControl("cbxSelect");
                    if (cbxSelect.Checked == true)
                    {
                        DropDownList ddlReason = (DropDownList)grdReports.Rows[i].FindControl("ddlReason");
                        TextBox txtDate = (TextBox)grdReports.Rows[i].FindControl("txtDate");
                        TextBox txtAmount = (TextBox)grdReports.Rows[i].FindControl("txtAmount");
                        DropDownList ddlME = (DropDownList)grdReports.Rows[i].FindControl("ddlME");
                        Label lblClientId = (Label)grdReports.Rows[i].FindControl("lblClientId");
                        Label lblSiteId = (Label)grdReports.Rows[i].FindControl("lblSiteId");

                        string Recordtype = grdReports.Rows[i].Cells[5].Text;
                        string ReferenceNo = grdReports.Rows[i].Cells[7].Text;
                        string testType = grdReports.Rows[i].Cells[10].Text;
                        if (testType == "&nbsp;")
                            testType = "";
                        decimal expectedAmount = 0;
                        DateTime? expectedDate = null;
                        string reason = "";
                        int MEId = 0;

                        if (txtAmountSel.Text != "")
                        {
                            expectedAmount = Convert.ToDecimal(txtAmountSel.Text);
                        }
                        if (txtDateSel.Text != "")
                        {
                            expectedDate = DateTime.ParseExact(txtDateSel.Text, "dd/MM/yyyy", null);
                        }
                        if (ddlReasonSel.SelectedIndex > 0)
                        {
                            reason = ddlReasonSel.SelectedItem.Text;
                        }
                        if (ddlMESel.SelectedIndex > 0)
                        {
                            MEId = Convert.ToInt32(ddlMESel.SelectedValue);
                        }
                        if (ddlReason.SelectedIndex > 0)
                        {
                            expectedAmount = 0;
                            expectedDate = null;
                            reason = "";
                            if (txtAmount.Text != "")
                            {
                                expectedAmount = Convert.ToDecimal(txtAmount.Text);
                            }
                            if (txtDate.Text != "")
                            {
                                expectedDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);
                            }
                            reason = ddlReason.SelectedItem.Text;
                            if (ddlME.SelectedIndex > 0)
                            {
                                MEId = Convert.ToInt32(ddlME.SelectedValue);
                            }
                        }
                        if (reason != "")
                        {
                            selFlag = true;
                            dc.MISDetail_Update_CRLDetail(Recordtype, ReferenceNo, testType, true, Convert.ToInt32(lblClientId.Text));
                            dc.CreditFollowup_Update(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), ReferenceNo, reason, expectedDate, expectedAmount, DateTime.Now, MEId);
                            for (int ii = 0; ii < grdReports.Rows.Count; ii++)
                            {
                                string[] refNo1 = ReferenceNo.Split('/');
                                string[] refNo2 = grdReports.Rows[ii].Cells[7].Text.Split('/');
                                CheckBox cbxSelect_ii = (CheckBox)grdReports.Rows[ii].FindControl("cbxSelect");
                                if (refNo1[0] == refNo2[0] && ReferenceNo != grdReports.Rows[ii].Cells[7].Text && cbxSelect_ii.Checked == false)
                                {
                                    string tempTestType = grdReports.Rows[ii].Cells[10].Text;
                                    if (tempTestType == "&nbsp;")
                                    {
                                        tempTestType = "";
                                    }
                                    dc.MISDetail_Update_CRLDetail(grdReports.Rows[ii].Cells[5].Text, grdReports.Rows[ii].Cells[7].Text, tempTestType, true, Convert.ToInt32(lblClientId.Text));
                                }
                            }
                        }
                    }
                }
                if (selFlag == true)
                {
                    strMsg = "Approved successfully";
                    DisplayReports();
                }
                else
                {
                    strMsg = "Select at least one report for approval.";
                }
            //}
            if (strMsg != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
        }

        protected void grdReports_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox cbxSelect = (CheckBox)e.Row.FindControl("cbxSelect");
                CheckBox cbxSelectAll = (CheckBox)this.grdReports.HeaderRow.FindControl("cbxSelectAll");
                cbxSelect.Attributes["onclick"] = string.Format
                                                       (
                                                          "javascript:ChildClick(this,'{0}');",
                                                          cbxSelectAll.ClientID
                                                       );
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

        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            var mis = dc.MISDetail_View_CRLDetail_temp();
            foreach (var m in mis)
            {
                dc.CreditFollowup_Update(m.INWD_CL_Id, m.INWD_SITE_Id, m.MISRefNo, m.MISCRLApprReason, null, 0, m.MISIssueDt, 0);
            }
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('done');", true);
        }

    }

}
