using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class PrintReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                bool printRight = false;
                var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Print_right_bit == true)
                        printRight = true;
                }
                if (printRight == true)
                {
                    Label lblheading = (Label)Master.FindControl("lblheading");
                    lblheading.Text = "Print Report";
                    LoadInwardType();
                    txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    optPending.Checked = true;
                    LoadApprovedBy();
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void LoadOtherTestList()
        {
            var test = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            ddl_Category.DataSource = test;
            ddl_Category.DataTextField = "TEST_Name_var";
            ddl_Category.DataValueField = "TEST_Id";
            ddl_Category.DataBind();
            ddl_Category.Items.Insert(0, new ListItem("---Select All---", "0"));
        }


        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select---");
        }
        private void LoadApprovedBy()
        {
            ddlApprovedBy.DataTextField = "USER_Name_var";
            ddlApprovedBy.DataValueField = "USER_Id";
            var apprUser = dc.ReportStatus_View("", null, null, 0, 0, 1, "", 0, 0, 0);
            ddlApprovedBy.DataSource = apprUser;
            ddlApprovedBy.DataBind();
            ddlApprovedBy.Items.Insert(0, "---Select---");
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdReports.Visible = true;
            DisplayReports();
        }

        public void DisplayReports()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            byte rptStatus = 4;
            if (optPending.Checked == true)
                rptStatus = 4;
            else if (optPrinted.Checked == true)
                rptStatus = 5;

            string finalStatus = "";
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                finalStatus = " " + ddlMF.SelectedValue;
            }
                        
            int categoryId = 0;
            if (ddl_InwardTestType.SelectedItem.Text == "Other Testing")
            {
                categoryId = Convert.ToInt32(ddl_Category.SelectedValue);
            }
            var Inward = dc.ReportPrintStatus_View(ddl_InwardTestType.SelectedItem.Text + finalStatus, Fromdate, Todate, rptStatus, ClientId, categoryId).ToList();
            grdReports.DataSource = Inward;
            grdReports.DataBind();

            if (optPending.Checked == true)
            {
                grdReports.Columns[11].Visible = true;
                grdReports.Columns[12].Visible = true;
            }
            else
            {
                grdReports.Columns[11].Visible = false;
                grdReports.Columns[12].Visible = false;
            }

            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "Trial")
            {
                grdReports.Columns[8].Visible = true;
                grdReports.Columns[9].Visible = false;
            }
            else if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "Cube Strength")
            {
                grdReports.Columns[8].Visible = true;
                grdReports.Columns[9].Visible = true;
            }
            else
            {
                grdReports.Columns[8].Visible = false;
                grdReports.Columns[9].Visible = false;
            }
            if (ddl_InwardTestType.SelectedItem.Text == "Non Destructive Testing")
            {
                lblPageBreak.Visible = true;
                txtPageBreak.Visible = true;
            }
            if (ddl_InwardTestType.SelectedItem.Text == "AAC Block Testing" || ddl_InwardTestType.SelectedItem.Text == "Aggregate Testing"
                || ddl_InwardTestType.SelectedItem.Text == "Soil Testing")
            {
                chkSplitReport.Visible = true;
            }
            lbl_RecordsNo.Text = "Total Records   :  " + grdReports.Rows.Count;
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
                {
                    Label lblNABLScope = (Label)grdReports.Rows[i].FindControl("lblNABLScope");
                    Label lblNABLLocation = (Label)grdReports.Rows[i].FindControl("lblNABLLocation");
                    if (ddlMF.SelectedValue == "Sieve Analysis")
                    {
                        lblNABLScope.Text = "F";
                        lblNABLLocation.Text = "0";
                    }
                    else
                    {
                        lblNABLScope.Text = "NA";
                        lblNABLLocation.Text = "0";
                    }
                }
                if (grdReports.Rows[i].Cells[1].Text != "")
                {
                    for (int j = i + 1; j < grdReports.Rows.Count; j++)
                    {
                        if (grdReports.Rows[i].Cells[0].Text == grdReports.Rows[j].Cells[0].Text &&
                            grdReports.Rows[i].Cells[1].Text == grdReports.Rows[j].Cells[1].Text)
                        {
                            grdReports.Rows[j].Cells[0].Text = "";
                            grdReports.Rows[j].Cells[1].Text = "";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (optPending.Checked == true)
                {
                    Label lblBalanceStatus = (Label)grdReports.Rows[i].FindControl("lblBalanceStatus");
                    if (lblBalanceStatus.Text == "red")
                    {
                        for (int j = 0; j < grdReports.Columns.Count; j++)
                        {
                            grdReports.Rows[i].Cells[j].BackColor = System.Drawing.Color.Tomato;
                        }
                    }
                    else if (lblBalanceStatus.Text == "green")
                    {
                        for (int j = 0; j < grdReports.Columns.Count; j++)
                        {
                            grdReports.Rows[i].Cells[j].BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                }


                //if (ddl_InwardTestType.SelectedIndex > 0)
                //{
                //    string ulrLocation = "0", ulrScope = "NA";
                //    string recordNo = grdReports.Rows[i].Cells[2].Text;
                //    string[] arr = recordNo.Split('/');
                //    string refNo = grdReports.Rows[i].Cells[3].Text;
                //    List<string> strNabl = new List<string>();
                //    List<int> strNablLoctn = new List<int>();
                //    clsData cd = new clsData();
                //    string recordType = cd.getMaterialTypeValue(Convert.ToString(ddl_InwardTestType.SelectedValue));
                //    var result = dc.Inward_Test_View(recordType, Convert.ToInt32(arr[0]), refNo).ToList();

                //    if (result.Count > 0)
                //    {
                //        foreach (var item in result)
                //        {
                //            if (Convert.ToString(item.TEST_NablScope_var) != "" && Convert.ToString(item.TEST_NablScope_var) != null)
                //                strNabl.Add(item.TEST_NablScope_var);

                //            strNablLoctn.Add(Convert.ToInt32(item.TEST_NablLocation_int));
                //        }
                //    }
                //    if (strNabl.Count > 0)
                //    {
                //        if (strNabl.All(x => x.ToString() == "NA"))
                //            ulrScope = "NA";
                //        else if (strNabl.All(x => x.ToString() == "F"))
                //            ulrScope = "F";
                //        else
                //            ulrScope = "P";
                //    }
                //    if (strNablLoctn.Count > 0)
                //        ulrLocation = strNablLoctn[0].ToString();
                    
                //    DropDownList ddlNABLScope = (DropDownList)grdReports.Rows[i].FindControl("ddlNABLScope");
                //    DropDownList ddlNABLLocation = (DropDownList)grdReports.Rows[i].FindControl("ddlNABLLocation");

                //    if (ulrScope != "")
                //    {
                //        if (ddlNABLScope.SelectedIndex == 3 && ddlNABLScope.Enabled == true)
                //            ddlNABLScope.Items.FindByValue(ulrScope).Selected = true;
                //    }
                //    else if (ddlNABLScope.Enabled == true)
                //        ddlNABLScope.SelectedIndex = 0;

                //    if (ulrLocation != "" && ulrLocation != "0" && ulrLocation != "1" && ddlNABLLocation.Enabled == true)
                //        ddlNABLLocation.Items.FindByValue(ulrLocation).Selected = true;
                //    else if (ddlNABLLocation.Enabled == true)
                //        ddlNABLLocation.SelectedIndex = 0;

                //}

            }
        }

        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[3];
            arg = strReportDetails.Split(';');            
            string Recordtype = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            string ReferenceNo = Convert.ToString(arg[2]);

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            PrintPDFReport rpt = new PrintPDFReport();
            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = grdrow.RowIndex;
            Label lblTrialId = (Label)grdReports.Rows[rowindex].FindControl("lblTrialId");
            Label lblCubeDays = (Label)grdReports.Rows[rowindex].FindControl("lblCubeDays");
            Label lblContactNo = (Label)grdReports.Rows[rowindex].FindControl("lblContactNo");
            //DropDownList ddlNABLScope = (DropDownList)grdReports.Rows[rowindex].FindControl("ddlNABLScope");
            //DropDownList ddlNABLLocation = (DropDownList)grdReports.Rows[rowindex].FindControl("ddlNABLLocation");
            Label lblNABLScope = (Label)grdReports.Rows[rowindex].FindControl("lblNABLScope");
            Label lblNABLLocation = (Label)grdReports.Rows[rowindex].FindControl("lblNABLLocation");

            string mfType = "";
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "Final")
            {
                mfType = "Final";
            }
            string[] strRefNo = ReferenceNo.Split('/');
            var chkBalance = dc.Inward_View_ClientBalance(ReferenceNo, Recordtype + mfType);
            if (chkBalance.Count() > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client is blocked as per credit policy, can not print report.');", true);
            }
            else
            {
                if (e.CommandName == "PrintReport")
                {
                    //string nablScope = "";
                    //int nablLoctn = 0;
                    //lblMsg.Visible = false;
                    //lblMsg.Text = "";
                    //bool flag = false, MFFlag = false;
                    //if (optPending.Checked)
                    //{
                    //    if (ddlMF.Visible == true)
                    //    {
                    //        if (ddlMF.SelectedItem.Text == "Trial" || ddlMF.SelectedItem.Text == "Blank Trial")
                    //        {
                    //            MFFlag = true;
                    //        }
                    //    }
                    //    if (!MFFlag)
                    //    {
                    //        if (ddlNABLScope.SelectedItem.Text == "--Select--" && ddlNABLScope.Enabled == true)
                    //        {
                    //            lblMsg.Visible = true;
                    //            lblMsg.Text = "Set NABL Scope to Test";
                    //            ddlNABLScope.Focus();
                    //            flag = true;
                    //        }
                    //        else
                    //            nablScope = ddlNABLScope.SelectedItem.Text;

                    //        if (ddlNABLLocation.SelectedItem.Text == "--Select--" && ddlNABLLocation.Enabled == true)
                    //        {
                    //            lblMsg.Visible = true;
                    //            lblMsg.Text = "Set NABL Location to Test";
                    //            ddlNABLLocation.Focus();
                    //            flag = true;
                    //        }
                    //        else
                    //            nablLoctn = Convert.ToInt32(ddlNABLLocation.SelectedItem.Text);
                    //    }
                    //}
                    ////

                    //if (flag)
                    //    return;

                    if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" &&
                        (ddlMF.SelectedValue == "Cover Sheet" || ddlMF.SelectedValue == "Moisture Correction"
                        || ddlMF.SelectedValue == "Sieve Analysis" || ddlMF.SelectedValue == "Blank Trial"))
                    {
                        rpt.PrintSelectedReport(Recordtype, ReferenceNo, "Print", lblTrialId.Text, lblCubeDays.Text, ddlMF.SelectedValue, "", "", "", "");
                    }
                    else
                    {
                        #region Bill Generation
                        bool printRptFlag = true;
                        bool generateBillFlag = true;
                        string BillNo = "0";
                        if (DateTime.Now.Day >= 26)
                        {
                            generateBillFlag = false;
                        }
                        if (generateBillFlag == true)
                        {
                            var inward = dc.Inward_View(0, RecordNo, Recordtype, null, null).ToList();
                            foreach (var inwd in inward)
                            {
                                if (inwd.INWD_BILL_Id != null && inwd.INWD_BILL_Id != "0")
                                {
                                    BillNo = inwd.INWD_BILL_Id;
                                    generateBillFlag = false;
                                }
                                if (inwd.SITE_MonthlyBillingStatus_bit != null && inwd.SITE_MonthlyBillingStatus_bit == true)
                                {
                                    generateBillFlag = false;
                                }
                                if (inwd.INWD_MonthlyBill_bit == true)
                                {
                                    generateBillFlag = false;
                                }
                            }
                        }
                        if (generateBillFlag == true && Recordtype == "CT")
                        {
                            var ctinwd = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, RecordNo, "", 0, Recordtype);
                            foreach (var ct in ctinwd)
                            {
                                if (ct.CTINWD_CouponNo_var != null && ct.CTINWD_CouponNo_var != "")
                                {
                                    generateBillFlag = false;
                                    break;
                                }
                            }
                        }
                        if (generateBillFlag == true)
                        {
                            var withoutbill = dc.WithoutBill_View(RecordNo, Recordtype);
                            if (withoutbill.Count() > 0)
                            {
                                generateBillFlag = false;
                            }
                        }
                        if (generateBillFlag == true)
                        {
                            int NewrecNo = 0;
                            clsData clsObj = new clsData();
                            NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                            var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                            if (gstbillCount.Count() != NewrecNo - 1)
                            {
                                generateBillFlag = false;
                                printRptFlag = false;
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill No. mismatch. Can not print report.');", true);
                            }
                        }

                        if (printRptFlag == true && generateBillFlag == true)
                        {
                            //Generate bill
                            BillUpdation bill = new BillUpdation();
                            BillNo = bill.UpdateBill(Recordtype, RecordNo, BillNo);
                            var chkBalance1 = dc.Inward_View_ClientBalance(ReferenceNo, Recordtype + mfType);
                            if (chkBalance1.Count() > 0)
                            {
                                printRptFlag = false;
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client is blocked as per credit policy, can not print report.');", true);
                            }
                            //
                        }
                        #endregion

                        if (printRptFlag == true)
                        {
                            #region update issuedate/ ULR No.
                            //UpdateIssuDt(RecordNo, Recordtype, ReferenceNo, nablScope, nablLoctn);
                            UpdateIssuDt(RecordNo, Recordtype, ReferenceNo, lblNABLScope.Text,Convert.ToInt32(lblNABLLocation.Text));
                            #endregion
                            string pageBrk = "";
                            if (chkSplitReport.Visible == true)
                            {
                                pageBrk = chkSplitReport.Checked.ToString();
                            }
                            else
                            {
                                pageBrk = txtPageBreak.Text;
                            }
                            rpt.PrintSelectedReport(Recordtype, ReferenceNo, "Print", lblTrialId.Text, lblCubeDays.Text, ddlMF.SelectedValue, "", "", "", pageBrk);
                            //if (optPending.Checked == true)
                            //{
                            //    //lblContactNo.Text = "9011085721";
                            //    if (lblContactNo.Text.Length == 10 || lblContactNo.Text.Length == 12)
                            //    {
                            //        //    string strMsg = "Dear Customer your " + ddl_InwardTestType.SelectedItem.Text + " Sample Ref.No. " + ReferenceNo + " is ready for despatch. Plz revert back to us if you don't receive the report within 24 hrs.";
                            //        //clsData objcls = new clsData();
                            //        //clsSendMail objmail = new clsSendMail();
                            //        //objmail.sendSMS(lblContactNo.Text, strMsg, "DUROCR");
                            //    }
                            //}
                        }


                    }

                }
                else if (e.CommandName == "ViewReport")
                {
                    string pageBrk = "";
                    if (chkSplitReport.Visible == true)
                    {
                        pageBrk = chkSplitReport.Checked.ToString();
                    }
                    else
                    {
                        pageBrk = txtPageBreak.Text;
                    }
                    //PrintSelectedReport(Recordtype, ReferenceNo, "View", lblTrialId.Text, lblCubeDays.Text);
                    rpt.PrintSelectedReport(Recordtype, ReferenceNo, "View", lblTrialId.Text, lblCubeDays.Text, ddlMF.SelectedValue, "", "", "", pageBrk);
                }
            }
        }
        
        public void UpdateIssuDt(int RecordNo, string Rectype, string RefNo, string nablScope, int nablLctn)
        {
            byte apprBy = 0;
            if (ddlApprovedBy.SelectedIndex > 0)
                apprBy = Convert.ToByte(ddlApprovedBy.SelectedValue);

            string tempRecType = Rectype;
            string testType = Rectype;
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                if (ddlMF.SelectedValue == "Final")
                {
                    tempRecType = tempRecType + " " + ddlMF.SelectedValue;
                    testType = "Final";
                }
                else
                {
                    testType = "MDL";
                }
            }
            string nablStatus = "";

            if (optPending.Checked == true)
            {
                dc.ReportDetails_Update(tempRecType, RefNo, 0, 0, 0, apprBy, false, "Issued By");
                dc.MISDetail_Update_IssueDate(0, Rectype, RefNo, testType);
                if (dc.Connection.Database.ToLower() == "veenalive")
                {
                    dc.ReportDetails_Update_NABLDetails(Rectype, RefNo, nablLctn, nablScope, testType);//if nablLctn, nablScope is null then it will update
                    dc.Inward_Update_ULRNo(RecordNo, Rectype, RefNo, nablScope, nablLctn, testType);
                }
                else
                {
                    dc.ReportDetails_Update_NABLDetails(Rectype, RefNo, nablLctn, nablScope, testType);
                    dc.Inward_Update_ULRNo(RecordNo, Rectype, RefNo, nablScope, nablLctn, testType);
                }

                if (Rectype != "OT" && Rectype != "NDT"
                    && Rectype != "CT" && Rectype != "CR"
                    && Rectype != "CEMT" && Rectype != "CCH" && Rectype != "TILE"
                    && Rectype != "ST" && Rectype != "STC" && Rectype != "WT" && Rectype != "AAC"
                    && Rectype != "GGBS" && Rectype != "GGBSCH")
                {
                    if (nablScope == "NA")
                        nablStatus = "NON-NABL";
                    else
                        nablStatus = "NABL";
                    dc.Inward_Update_NablStatus(RefNo, Rectype, nablStatus);
                }
            }
            else
            {
                dc.ReportDetails_Update(tempRecType, RefNo, 0, 0, 0, apprBy, false, "Printed By");
                dc.MISDetail_Update(0, Rectype, RefNo, testType, null, false, false, false, true, false, false, false);
            }
        }

        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
            if (ddl_InwardTestType.SelectedItem.Text == "Cube Testing" && optPending.Checked == true)
            {
                lnkPrintAllCubeRpt.Visible = true;
            }
            else
            {
                lnkPrintAllCubeRpt.Visible = false;
            }
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                lblMF.Visible = true;
                ddlMF.Visible = true;
            }
            else
            {
                lblMF.Visible = false;
                ddlMF.Visible = false;
            }
            if (ddl_InwardTestType.SelectedItem.Text == "Non Destructive Testing")
            {
                lblPageBreak.Visible = true;
                txtPageBreak.Visible = true;
            }
            else
            {
                lblPageBreak.Visible = false;
                txtPageBreak.Visible = false;
            }
            if (ddl_InwardTestType.SelectedItem.Text == "AAC Block Testing" || ddl_InwardTestType.SelectedItem.Text == "Soil Testing")
            {
                chkSplitReport.Visible = true;
            }
            else
            {
                chkSplitReport.Visible = false;
            }
            if (ddl_InwardTestType.SelectedItem.Text == "Other Testing")
            {
                LoadOtherTestList();
                lblCategory.Visible = true;
                ddl_Category.Visible = true;
            }
            else
            {
                lblCategory.Visible = false;
                ddl_Category.Visible = false;
            }

        }
        private void ClearReportList()
        {
            grdReports.Visible = false;
            lbl_RecordsNo.Text = "";
            grdReports.DataSource = null;
            grdReports.DataBind();
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

        protected void optPending_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
            lblIssueDate.Visible = false;
            txt_FromDate.Visible = false;
            txt_ToDate.Visible = false;
            if (ddl_InwardTestType.SelectedItem.Text == "Cube Testing" && optPending.Checked == true)
            {
                lnkPrintAllCubeRpt.Visible = true;
            }
            else
            {
                lnkPrintAllCubeRpt.Visible = false;
            }
        }

        protected void optPrinted_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
            lblIssueDate.Visible = true;
            txt_FromDate.Visible = true;
            txt_ToDate.Visible = true;            
            lnkPrintAllCubeRpt.Visible = false;
        }

        protected void ddlMF_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
        }        

        protected void lnkPrintAllCubeRpt_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            lblMsg.Text = "";

            string strRefNoList = "";
            byte rptStatus = 4;
            var Inward = dc.ReportPrintStatus_View(ddl_InwardTestType.SelectedItem.Text, null, null, rptStatus, 0, 0).ToList();
            foreach (var inwd in Inward)
            {
                if (inwd.balanceStatus != "red")
                {
                    // update issuedate/ ULR No.
                    UpdateIssuDt(Convert.ToInt32(inwd.RecordNo), "CT", inwd.ReferenceNo, inwd.NablScope, Convert.ToInt32(inwd.NablLocation));

                    if (strRefNoList == "")
                        strRefNoList = inwd.ReferenceNo;
                    else
                        strRefNoList += "|" + inwd.ReferenceNo;

                }
            }
            if (strRefNoList != "")
            {
                rpt.Cube_PDFReportMultiple(strRefNoList, "Print");
                //foreach (var inwd in Inward)
                //{
                //    if (inwd.balanceStatus != "red")
                //    {
                //        if (inwd.ContactNo.ToString().Length == 10 || inwd.ContactNo.ToString().Length == 12)
                //        {
                //            //string strMsg = "Dear Customer your " + ddl_InwardTestType.SelectedItem.Text + " Sample Ref.No. " + inwd.ReferenceNo + " is ready for despatch. Plz revert back to us if you don't receive the report within 24 hrs.";
                //            //clsData objcls = new clsData();
                //            //clsSendMail objmail = new clsSendMail();
                //            //objmail.sendSMS(inwd.ContactNo.ToString(), strMsg, "DUROCR");
                //        }
                //    }
                //}
            }
        }

        //private int getNablLogoStatus(string RecType)
        //{
        //    int status = 0;

        //    if (RecType != "")
        //    {
        //        var reslt = dc.Material_View(RecType, "");
        //        status = Convert.ToInt32(reslt.FirstOrDefault().MATERIAL_NablPrn_bit);
        //    }

        //    return status;
        //}

        //protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DropDownList ddlNABLScope = (DropDownList)e.Row.FindControl("ddlNABLScope");
        //        DropDownList ddlNABLLocation = (DropDownList)e.Row.FindControl("ddlNABLLocation");
        //        if (ddl_InwardTestType.SelectedItem.Text == "Other Testing" || ddl_InwardTestType.SelectedItem.Text == "AAC Block Testing" ||
        //            ddl_InwardTestType.SelectedItem.Text == "Water Testing" || ddl_InwardTestType.SelectedItem.Text == "Steel Testing" ||
        //            ddl_InwardTestType.SelectedItem.Text == "Tile Testing" || ddl_InwardTestType.SelectedItem.Text == "Steel Chemical Testing" ||
        //            ddl_InwardTestType.SelectedItem.Text == "Non Destructive Testing" || ddl_InwardTestType.SelectedItem.Text == "Cube Testing" ||
        //            ddl_InwardTestType.SelectedItem.Text == "Core Testing" || ddl_InwardTestType.SelectedItem.Text == "Cement Testing" || ddl_InwardTestType.SelectedItem.Text == "Cement Chemical Testing")
        //        {
        //            string strNABLLocation = (e.Row.FindControl("lblNABLLocation") as Label).Text;
        //            if (strNABLLocation != "" && strNABLLocation != null)
        //            {
        //                ddlNABLLocation.Items.FindByValue(strNABLLocation).Selected = true;
        //                ddlNABLLocation.Enabled = false;
        //            }
        //            string strNABLScope = (e.Row.FindControl("lblNABLScope") as Label).Text;
        //            if (strNABLScope != "" && strNABLScope != null)
        //            {
        //                ddlNABLScope.Items.FindByValue(strNABLScope).Selected = true;
        //                ddlNABLScope.Enabled = false;
        //            }
        //        }
        //    }
        //}

        //public void PrintSelectedReport(string Rectype, string RefNo, string Print, string strTrialId, string strCubeDays)
        //{
        //    PrintPDFReport rpt = new PrintPDFReport();
        //    switch (Rectype)
        //    {
        //        case "SO":
        //            var smp = dc.SoilSampleTest_View(RefNo, "");
        //            foreach (var so in smp)
        //            {
        //                rpt.Soil_PDFReport(RefNo, Convert.ToString(so.SOSMPLTEST_SampleName_var), Print);
        //                break;
        //            }
        //            break;
        //        case "TILE":
        //            rpt.Tile_PDFReport(RefNo, Print);
        //            break;
        //        case "BT-":
        //            rpt.Brick_PDFReport(RefNo, Print);
        //            break;
        //        case "FLYASH":
        //            rpt.FlyAsh_PDFReport(RefNo, Print);
        //            break;
        //        case "CEMT":
        //            rpt.Cement_PDFReport(RefNo, Print);
        //            break;
        //        case "CCH":
        //            rpt.CCH_PDFReport(RefNo, Print);
        //            break;
        //        case "CT":
        //            rpt.Cube_PDFReport(RefNo, 0, Rectype, "", Print, "", "");
        //            break;
        //        case "PILE":
        //            rpt.Pile_PDFReport(RefNo, Print);
        //            break;
        //        case "STC":
        //            rpt.STC_PDFReport(RefNo, Print);
        //            break;
        //        case "ST":
        //            rpt.ST_PDFReport(RefNo, Print);
        //            break;
        //        case "WT":
        //            rpt.WT_PDFReport(RefNo, Print);
        //            break;
        //        case "AGGT":
        //            rpt.Aggregate_PDFReport(RefNo, Rectype, "", 0, Print);
        //            break;
        //        case "SOLID":
        //            var details = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "SOLID");
        //            foreach (var solid in details)
        //            {
        //                if (Convert.ToString(solid.TEST_Sr_No) == "1")//(solid.SOLIDINWD_TEST_Id) == "66")
        //                {
        //                    rpt.SOLID_CS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(solid.TEST_Sr_No) == "2")
        //                {
        //                    rpt.SOLID_WA_PDFReport(RefNo, Print);
        //                }
        //            }
        //            break;
        //        case "OT":
        //            rpt.OT_PDFReport(RefNo, Print);
        //            break;
        //        case "CR":
        //            rpt.Core_PDFReport(RefNo, Print);
        //            break;
        //        case "NDT":
        //            rpt.NDT_PDFReport(RefNo, Print, txtPageBreak.Text);
        //            break;
        //        case "PT":
        //            var PTdetails = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "PT");
        //            foreach (var PTWA in PTdetails)
        //            {
        //                if (Convert.ToString(PTWA.TEST_Sr_No) == "1")//1
        //                {
        //                    rpt.Pavement_CS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "2")//2 //(Convert.ToString(PTWA.PTINWD_TEST_Id) == "63")
        //                {
        //                    rpt.Pavement_WA_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "3")//3
        //                {
        //                    rpt.Pavement_TS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "4")//4
        //                {
        //                    rpt.Pavement_FS_PDFReport(RefNo, Print);
        //                }
        //            }
        //            break;
        //        case "MF":
        //            PrintHTMLReport rptHtml = new PrintHTMLReport();
        //            int trailId = 0;
        //            var mixd = dc.Trial_View(RefNo, true);
        //            foreach (var mf in mixd)
        //            {
        //                trailId = mf.Trial_Id;
        //            }
        //            if (ddlMF.SelectedValue == "Sieve Analysis")
        //                //rptHtml.TrialSieveAnalysis_Html(RefNo, "MF");
        //                rpt.MFSieveAnalysis_PDF(RefNo, "MF", Print);
        //            else if (ddlMF.SelectedValue == "Moisture Correction")
        //                rpt.MoistureCorrection_PDF(RefNo, trailId, Print);
        //            else if (ddlMF.SelectedValue == "Cover Sheet")
        //                rpt.MDLCoverSheet_PDF(RefNo, trailId, Print);
        //            else if (ddlMF.SelectedValue == "Blank Trial")
        //                rptHtml.TrialProportion_Html(RefNo, 0);
        //            else if (ddlMF.SelectedValue == "Trial")
        //                rptHtml.TrialInformation_Html(RefNo, Convert.ToInt32(strTrialId));
        //            else if (ddlMF.SelectedValue == "Cube Strength")
        //                rpt.Cube_PDFReport(RefNo, Convert.ToInt32(strCubeDays), "MF", "MF", Print, "TrialCubeCompStr", strTrialId);
        //            else //MDL and Final
        //                //rptHtml.TrialMDLetter_Html(RefNo, trailId, "MF", ddlMF.SelectedValue, Print);
        //                rpt.MF_MDLetter_PDFReport(RefNo, trailId, "MF", ddlMF.SelectedValue, Print);
        //            break;
        //        case "AAC":
        //            var detailss = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AAC");
        //            foreach (var aac in detailss)
        //            {
        //                if (Convert.ToString(aac.TEST_Sr_No) == "1")
        //                {
        //                    rpt.AAC_CS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "2")
        //                {
        //                    rpt.AAC_DS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "3")
        //                {
        //                    rpt.AAC_DM_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "4")
        //                {
        //                    rpt.AAC_SN_PDFReport(RefNo, Print);
        //                }
        //            }
        //            break;
        //    }
        //}
        //public void BindAppoveRpt(string Rectype, string RefNo, string strAction)
        //{
        //    PrintPDFReport rpt = new PrintPDFReport();

        //    switch (Rectype)
        //    {
        //        case "SO":
        //            var smp = dc.SoilSampleTest_View(RefNo, "");
        //            foreach (var so in smp)
        //            {
        //                rpt.Soil_PDFReport(RefNo, Convert.ToString(so.SOSMPLTEST_SampleName_var), strAction);
        //                break;
        //            }
        //            break;
        //        case "TILE":
        //            rpt.Tile_PDFReport(RefNo, strAction);
        //            break;
        //        case "BT-":
        //            rpt.Brick_PDFReport(RefNo, strAction);
        //            break;
        //        case "CCH":
        //            rpt.CCH_PDFReport(RefNo, strAction);
        //            break;
        //        case "CEMT":
        //            rpt.Cement_PDFReport(RefNo, strAction);
        //            break;
        //        case "FLYASH":
        //            rpt.FlyAsh_PDFReport(RefNo, strAction);
        //            break;
        //        case "CT":
        //            rpt.Cube_PDFReport(RefNo, 0, Rectype, "", strAction, "", "");
        //            break;
        //        case "PILE":
        //            rpt.Pile_PDFReport(RefNo, strAction);
        //            break;
        //        case "STC":
        //            rpt.STC_PDFReport(RefNo, strAction);
        //            break;
        //        case "ST":
        //            rpt.ST_PDFReport(RefNo, strAction);
        //            break;
        //        case "WT":
        //            rpt.WT_PDFReport(RefNo, strAction);
        //            break;
        //        case "SOLID":
        //            var details = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "SOLID");
        //            foreach (var solid in details)
        //            {
        //                if (Convert.ToString(solid.TEST_Sr_No) == "1")
        //                {
        //                    rpt.SOLID_CS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(solid.TEST_Sr_No) == "2")
        //                {
        //                    rpt.SOLID_WA_PDFReport(RefNo, strAction);
        //                }
        //            }
        //            break;
        //        case "OT":
        //            rpt.OT_PDFReport(RefNo, strAction);
        //            break;
        //        case "CR":
        //            rpt.Core_PDFReport(RefNo, strAction);
        //            break;
        //        case "NDT":
        //            rpt.NDT_PDFReport(RefNo, strAction, "");
        //            break;
        //        case "AGGT":
        //            rpt.Aggregate_PDFReport(RefNo, Rectype, "", 0, strAction);
        //            break;
        //        case "PT":
        //            var PTdetails = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "PT");
        //            foreach (var PTWA in PTdetails)
        //            {
        //                if (Convert.ToString(PTWA.TEST_Sr_No) == "1")//1
        //                {
        //                    rpt.Pavement_CS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "2")//2 //(Convert.ToString(PTWA.PTINWD_TEST_Id) == "63")
        //                {
        //                    rpt.Pavement_WA_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "3")//3
        //                {
        //                    rpt.Pavement_TS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "4")//4
        //                {
        //                    rpt.Pavement_FS_PDFReport(RefNo, strAction);
        //                }
        //            }
        //            break;//
        //        case "AAC":
        //            var detailss = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AAC");
        //            foreach (var aac in detailss)
        //            {
        //                if (Convert.ToString(aac.TEST_Sr_No) == "1")
        //                {
        //                    rpt.AAC_CS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "2")
        //                {
        //                    rpt.AAC_DS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "3")
        //                {
        //                    rpt.AAC_DM_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "4")
        //                {
        //                    rpt.AAC_SN_PDFReport(RefNo, strAction);
        //                }
        //            }
        //            break;
        //    }
        //}

        //protected void grdReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        //DropDownList ddlNABL = (e.Row.FindControl("ddlNABL") as DropDownList);
        //string strNABL = (e.Row.FindControl("lblNABL") as Label).Text;
        //if (strNABL == "")
        //    ddlNABL.SelectedIndex = 0;
        //else
        //    ddlNABL.Items.FindByValue(strNABL).Selected = true;

        //        DropDownList ddlNABLLocation = (e.Row.FindControl("ddlNABLLocation") as DropDownList);
        //        string strNABLLocation = (e.Row.FindControl("lblNABLLocation") as Label).Text;
        //        if (strNABLLocation == "")
        //            ddlNABLLocation.SelectedIndex = 0;
        //        else
        //            ddlNABLLocation.Items.FindByValue(strNABLLocation).Selected = true;

        //        DropDownList ddlNABLScope = (e.Row.FindControl("ddlNABLScope") as DropDownList);
        //        string strNABLScope = (e.Row.FindControl("lblNABLScope") as Label).Text;
        //        if (strNABLScope == "")
        //            ddlNABLScope.SelectedIndex = 0;
        //        else
        //            ddlNABLScope.Items.FindByValue(strNABLScope).Selected = true;

        //    }
        //}

        //protected void chkLocationAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkLocationAll.Checked)
        //    {
        //        Label lblMsg = (Label)Master.FindControl("lblMsg");
        //        lblMsg.Visible = false;

        //        if (ddlNABLLocationAll.SelectedItem.Text == "--Select--")
        //        {
        //            lblMsg.Visible = true;
        //            lblMsg.Text = "Select Location To Apply All Records.";
        //        }
        //        else
        //        {
        //            updateLocationToAllRows();
        //            lblMsg.Text = "";
        //        }
        //    }
        //}

        //protected void ddlNABLLocationAll_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (chkLocationAll.Checked)
        //    {
        //        updateLocationToAllRows();
        //    }
        //}

        //private void updateLocationToAllRows()
        //{
        //    Label lblMsg = (Label)Master.FindControl("lblMsg");
        //    lblMsg.Visible = false; lblMsg.Text = "";
        //    if (ddlNABLLocationAll.SelectedItem.Text != "--Select--")
        //    {
        //        string val = ddlNABLLocationAll.SelectedItem.Text;
        //        if (grdReports.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < grdReports.Rows.Count; i++)
        //            {
        //                DropDownList ddlNABLLocation = (DropDownList)grdReports.Rows[i].FindControl("ddlNABLLocation");
        //                ddlNABLLocation.ClearSelection();
        //                ddlNABLLocation.Items.FindByValue(val).Selected = true;
        //            }
        //        }
        //    }
        //}

    }

}

