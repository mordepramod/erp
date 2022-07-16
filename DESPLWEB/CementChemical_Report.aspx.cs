using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace DESPLWEB
{
    public partial class CementChemical_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                    if (strReq.Contains("=") == false)
                    {
                        Session.Abandon();
                        Response.Redirect("Login.aspx");
                    }
                    string[] arrMsgs = strReq.Split('&');
                    string[] arrIndMsg;

                    arrIndMsg = arrMsgs[0].Split('=');
                    txt_RecType.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEntry.Text = arrIndMsg[1].ToString().Trim();
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Cement Chemical - Report Entry ";
                if (txt_RecType.Text != "")
                {
                    //BindGrade();
                    getCurrentTestingDate();
                    DisplayCCHDetails();
                    BindDetailsGrid();

                    if (lblEntry.Text == "Check")
                    {
                        lbl_TestedBy.Text = "Approve By";
                      //  lblEntry.Text = Session["Check"].ToString();//
                        lblheading.Text = "Cement Chemical - Report Check ";
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
                        ViewWitnessBy();
                        DisplayCCHGrid();
                        DisplayRemark();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
                        //LoadOtherPendingRpt();
                        AddRowCemtChemicalRemark();
                        LoadTestedBy();
                    }
                    LoadReferenceNoList();
                }
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEntry.Text == "Enter")
                reportStatus = 1;
            else if (lblEntry.Text == "Check")
                reportStatus = 2;

            var reportList = dc.ReferenceNo_View_StatusWise("CCH", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        public void BindGrade()
        {
            var matid = dc.Material_View("CCH", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                ddl_Grade.DataSource = iscd;
                ddl_Grade.DataTextField = "Isc_Criteria_var";
                ddl_Grade.DataValueField = "Isc_Criteria_var";
                ddl_Grade.DataBind();
                ddl_Grade.Items.Insert(0, "---Select---");
            }
        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Cement Chemical Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "CCHINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "CCHINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataSource = testinguser;
                ddl_OtherPendingRpt.DataBind();
                ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
                ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
                if (itemToRemove != null)
                {
                    ddl_OtherPendingRpt.Items.Remove(itemToRemove);
                }
                lbl_TestedBy.Text = "Approve By";
            }
            else
            {
                LoadOtherPendingRpt();
            }
        }
        private void LoadOtherPendingRpt()
        {
            string Refno = txt_ReferenceNo.Text;
            var testinguser = dc.ReportStatus_View("Cement Chemical Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "CCHINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "CCHINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        public void getCurrentTestingDate()
        {
            txt_DateOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        private void LoadTestedBy()
        {
            ddl_TestedBy.DataTextField = "USER_Name_var";
            ddl_TestedBy.DataValueField = "USER_Id";
            var testinguser = dc.ReportStatus_View("", null, null, 0, 1, 0, "", 0, 0, 0);
            ddl_TestedBy.DataSource = testinguser;
            ddl_TestedBy.DataBind();
            ddl_TestedBy.Items.Insert(0, "---Select---");
        }
        private void LoadApproveBy()
        {
            if (lblEntry.Text == "Check")
            {
                ddl_TestedBy.DataTextField = "USER_Name_var";
                ddl_TestedBy.DataValueField = "USER_Id";
                var testinguser = dc.ReportStatus_View("", null, null, 0, 0, 1, "", 0, 0, 0);
                ddl_TestedBy.DataSource = testinguser;
                ddl_TestedBy.DataBind();
                ddl_TestedBy.Items.Insert(0, "---Select---");
            }
            else
            {
                LoadTestedBy();
            }
        }
        protected void chk_WitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txt_witnessBy.Text = string.Empty;
            if (chk_WitnessBy.Checked)
            {
                txt_witnessBy.Visible = true;
                txt_witnessBy.Focus();
            }
            else
            {
                txt_witnessBy.Visible = false;
            }
        }
        public void ViewWitnessBy()
        {
            txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ct = dc.ReportStatus_View("Cement Chemical Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.CCHINWD_WitnessBy_var.ToString() != null && c.CCHINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.CCHINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }
        }

        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "CCH");
            foreach (var r in re)
            {
                AddRowCemtChemicalRemark();
                TextBox txt_REMARK = (TextBox)grdCCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CCHDetail_RemarkId_int), "CCH");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.CCH_Remark_var.ToString();
                    i++;
                }
            }
            if (grdCCHRemark.Rows.Count <= 0)
            {
                AddRowCemtChemicalRemark();
            }

        }
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowCemtChemicalRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdCCHRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdCCHRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowCemtChemicalRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowCemtChemicalRemark(int rowIndex)
        {
            GetCurrentDataCemtChemicalRemark();
            DataTable dt = ViewState["CCHRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CCHRemarkTable"] = dt;
            grdCCHRemark.DataSource = dt;
            grdCCHRemark.DataBind();
            SetPreviousDataCemtChemicalRemark();
        }
        protected void AddRowCemtChemicalRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CCHRemarkTable"] != null)
            {
                GetCurrentDataCemtChemicalRemark();
                dt = (DataTable)ViewState["CCHRemarkTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_REMARK"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["CCHRemarkTable"] = dt;
            grdCCHRemark.DataSource = dt;
            grdCCHRemark.DataBind();
            SetPreviousDataCemtChemicalRemark();
        }
        protected void GetCurrentDataCemtChemicalRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdCCHRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdCCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CCHRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataCemtChemicalRemark()
        {
            DataTable dt = (DataTable)ViewState["CCHRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdCCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdCCHRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        public void DisplayCCHDetails()
        {
            var wInwd = dc.ReportStatus_View("Cement Chemical Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var w in wInwd)
            {
                txt_ReferenceNo.Text = w.CCHINWD_ReferenceNo_var.ToString();
                if (w.CCHINWD_TestedDate_dt.ToString() != null && w.CCHINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(w.CCHINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_Description.Text = w.CCHINWD_Description_var.ToString();
                txt_Supplier.Text = w.CCHINWD_SupplierName_var.ToString();
                txt_CementUsed.Text = w.CCHINWD_CementName_var.ToString();
                txt_ReportNo.Text = w.CCHINWD_SetOfRecord_var.ToString();
                txt_RecType.Text = w.CCHINWD_RecordType_var.ToString();
                if (w.CCHINWD_Grade_var != null && w.CCHINWD_Grade_var != "")
                {
                    ddl_Grade.ClearSelection();
                    ddl_Grade.Items.FindByText(w.CCHINWD_Grade_var).Selected = true;
                }                
                if (ddl_NablScope.Items.FindByValue(w.CCHINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(w.CCHINWD_NablScope_var);
                }
                if (Convert.ToString(w.CCHINWD_NablLocation_int) != null && Convert.ToString(w.CCHINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(w.CCHINWD_NablLocation_int);
                }


            }
        }
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            FetchRefNo();
        }
        public void FetchRefNo()
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---")
            {
                try
                {
                    lnkSave.Enabled = true;
                    lnkPrint.Visible = false;
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Visible = false;
                    txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                    txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                    DisplayCCHDetails();
                    //LoadOtherPendingCheckRpt();
                    LoadReferenceNoList();
                    LoadApproveBy();
                    ViewWitnessBy();
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                finally
                {
                    grdCCHEntryRptInward.DataSource = null;
                    grdCCHEntryRptInward.DataBind();
                    BindDetailsGrid();
                    DisplayCCHGrid();
                    grdCCHRemark.DataSource = null;
                    grdCCHRemark.DataBind();
                    DisplayRemark();
                }
            }
        }
        public void BindDetailsGrid()
        {
            var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CCH");
            grdCCHEntryRptInward.DataSource = details;
            grdCCHEntryRptInward.DataBind();
            //if (grdCCHEntryRptInward.Rows.Count <= 0)
            //{
            //    AddRowEnterReportCementChemicalInward();
            //}
        }
        protected void AddRowEnterReportCementChemicalInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CementChemicalTestTable"] != null)
            {
                GetCurrentDataCementChemicalInward();
                dt = (DataTable)ViewState["CementChemicalTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_NameOfTest", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_result", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_SpecifiedLimits", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_NameOfTest"] = string.Empty;
            dr["txt_result"] = string.Empty;
            dr["txt_SpecifiedLimits"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CementChemicalTestTable"] = dt;
            grdCCHEntryRptInward.DataSource = dt;
            grdCCHEntryRptInward.DataBind();
            SetPreviousDataCementChemicalInward();
        }
        protected void GetCurrentDataCementChemicalInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_NameOfTest", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_result", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_SpecifiedLimits", typeof(string)));

            for (int i = 0; i < grdCCHEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                TextBox txt_SpecifiedLimits = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[3].FindControl("txt_SpecifiedLimits");

                drRow = dtTable.NewRow();
                drRow["txt_NameOfTest"] = txt_NameOfTest.Text;
                drRow["txt_result"] = txt_result.Text;
                drRow["txt_SpecifiedLimits"] = txt_SpecifiedLimits.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CementChemicalTestTable"] = dtTable;

        }
        protected void SetPreviousDataCementChemicalInward()
        {
            DataTable dt = (DataTable)ViewState["CementChemicalTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                TextBox txt_SpecifiedLimits = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[3].FindControl("txt_SpecifiedLimits");

                txt_NameOfTest.Text = dt.Rows[i]["txt_NameOfTest"].ToString();
                txt_result.Text = dt.Rows[i]["txt_result"].ToString();
                txt_SpecifiedLimits.Text = dt.Rows[i]["txt_SpecifiedLimits"].ToString();

            }

        }
        public void DisplayCCHGrid()
        {
            int i = 0;
            var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CCH");
            foreach (var cch in details)
            {
                TextBox txt_result = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                if (cch.CCHTEST_Result_dec.ToString() != "" && cch.CCHTEST_Result_dec != null)
                {
                    txt_result.Text = cch.CCHTEST_Result_dec.ToString();
                }
                i++;
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);

                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, "", 2, 0, "", 0, ddl_Grade.SelectedItem.Text, "", 0, 0, 0, "CCH");
                    dc.ReportDetails_Update("CCH", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "CCH", txt_ReferenceNo.Text, "CCH", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, "", 3, 0, "", 0, ddl_Grade.SelectedItem.Text, "", 0, 0, 0, "CCH");
                    dc.ReportDetails_Update("CCH", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "CCH", txt_ReferenceNo.Text, "CCH", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "CCH", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                int TestId = 0;
                for (int i = 0; i < grdCCHEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_result = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                    TestId = Convert.ToInt32((txt_result.CssClass));

                    dc.AllInwd_Update(txt_ReferenceNo.Text, "", 0, null, "", "", 0, TestId, "", Convert.ToDecimal(txt_result.Text), "", "", 0, 0, 0, "CCH");
                }

                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CCH", true);
                for (int i = 0; i < grdCCHRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdCCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CCH");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.CCH_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "CCH");
                            foreach (var c in chkId)
                            {
                                if (c.CCHDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CCH", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "CCH");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CCH");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.CCH_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CCH", false);
                            }
                        }
                    }
                }
                #endregion

                //Approve on check
                if (lbl_TestedBy.Text == "Approve By")
                {
                    ApproveReports("CCH", txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text)); //, txtEmailId.Text, lblContactNo.Text);
                    //update MISCRLApprStatus to 1 if SITE_CRBypass_bit is 1
                    int siteCRbypssBit = 0; //clsData cd = new clsData();
                    siteCRbypssBit = cd.getSITECRBypassBit("CCH", Convert.ToInt32(lblRecordNo.Text));
                    if (siteCRbypssBit == 1)
                        dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, "CCH");
                    //SMS
                    var res = dc.SMSDetailsForReportApproval_View(Convert.ToInt32(lblRecordNo.Text), "CCH").ToList();
                    if (res.Count > 0)
                    {
                        cd.sendInwardReportMsg(Convert.ToInt32(res.FirstOrDefault().ENQ_Id), Convert.ToString(res.FirstOrDefault().ENQ_Date_dt), Convert.ToInt32(res.FirstOrDefault().SITE_MonthlyBillingStatus_bit).ToString(), res.FirstOrDefault().crLimitExceededStatus.ToString(), res.FirstOrDefault().BILL_NetAmt_num.ToString(), txt_ReferenceNo.Text, Convert.ToString(res.FirstOrDefault().INWD_ContactNo_var), "Report");
                    }


                    //old dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text), "CCH");
                    CRLimitExceedEmail(txt_ReferenceNo.Text, lblRecordNo.Text.ToString(), "CCH", "Cement Chemical Testing");

                }

                lnkPrint.Visible = true;

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;

                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record Saved Successfully');", true);
                lnkSave.Enabled = false;
            }
        }
        public void ApproveReports(string Recordtype, string ReferenceNo, int RecordNo) //, string EmailId, string ContactNo)
        {
            string tempRecType = Recordtype;
            string testType = Recordtype;

            #region Bill Generation
            bool approveRptFlag = true;
            if (approveRptFlag == true)
            {
                dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, Convert.ToByte(ddl_TestedBy.SelectedValue), true, "Approved By");
                dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, true, false, false, false, false);
            }
            #endregion
            
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Report Approved Successfully.";
            lblMsg.Visible = true;
        }
        public void CRLimitExceedEmail(string referenceNo, string recordNo, string recordType, string inwardType)
        {
            bool addMailId = true;
            string mailIdTo = "", mailIdCc = "", tempMailId = "", strAllMailId = "", strMEName = "", strMEContactNo = "", strEnqDate = "";
            string strOutstndingAmt = "0", strEnqAmount = "0", strEnqNo = "", tollFree = "";
            int siteRouteId = 0, siteMeId = 0, crLimtFlag = 0;

            var enquiry = dc.MailIdForCRLExceed_View(0, 0, Convert.ToInt32(recordNo), recordType, "ReportApproval", false).ToList();//chk  crlimt is exceeded
            if (enquiry.Count == 0)
                enquiry = dc.MailIdForCRLExceed_View(0, 0, Convert.ToInt32(recordNo), recordType, "ReportApproval", true).ToList();//chk  crlimt is not exceeded 

            foreach (var enq in enquiry)
            {
                strMEName = enq.MEName;
                strMEContactNo = enq.MEContactNo;
                if (enq.MEContactNo == null || enq.MEContactNo == "")
                    strMEContactNo = enq.MEMailId;
                strEnqDate = Convert.ToDateTime(enq.ENQ_Date_dt).ToString("dd/MM/yyyy");
                if (Convert.ToString(enq.CL_BalanceAmt_mny) != "" && Convert.ToString(enq.CL_BalanceAmt_mny) != null)
                    strOutstndingAmt = Convert.ToString(enq.CL_BalanceAmt_mny);
                strEnqAmount = Convert.ToString(enq.PROINV_NetAmt_num);
                strEnqNo = Convert.ToString(enq.ENQ_Id);
                tempMailId = enq.INWD_EmailId_var.Trim();
                if (Convert.ToString(enq.SITE_Route_Id) != "" && Convert.ToString(enq.SITE_Route_Id) != null)
                    siteRouteId = Convert.ToInt32(enq.SITE_Route_Id);
                if (Convert.ToString(enq.MEId) != "" && Convert.ToString(enq.MEId) != null)
                    siteMeId = Convert.ToInt32(enq.MEId);
                crLimtFlag = enq.crExceededStatus;

                if (PrintPDFReport.cnStr.ToLower().Contains("mumbai") == true)
                    tollFree = "9850500013"; // "18001214070";
                else if (PrintPDFReport.cnStr.ToLower().Contains("nashik") == true)
                    tollFree = "";
                else
                    tollFree = "18001206465";

                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.CL_AccEmailId_var.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.CL_DirectorEmailID_var.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.CL_EmailID_var.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.SITE_EmailID_var.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdTo != "")
                        mailIdTo += ",";
                    mailIdTo += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.EnteredByMailId.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdCc != "")
                        mailIdCc += ",";
                    mailIdCc += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }

                addMailId = true;
                tempMailId = enq.MEMailId.Trim();
                if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    tempMailId.ToLower().Contains(".") == false)
                {
                    addMailId = false;
                }
                if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                {
                    if (mailIdCc != "")
                        mailIdCc += ",";
                    mailIdCc += tempMailId;
                    strAllMailId += "," + tempMailId + ",";
                }
            }

            if (mailIdTo != "")
            {
              

                //clsSendMail objMail = new clsSendMail();
                //string mSubject = "", mbody = "", mReplyTo = "";
                ////mailIdTo = "shital.bandal@gmail.com";
                ////mailIdCc = "";
                ////  mSubject = "Credit Limit Exceeded";
                //mSubject = "Report Confirmation";

                //mbody = "Dear Customer,<br><br>";

                //if (crLimtFlag == 1)//Credit Limit exceeded Client
                //    mbody = mbody + "We have tested material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ". The report is ready with us. Your total outstanding dues are Rs " + strOutstndingAmt + ".<br>Please arrange to make the payment for the testing to access the report on our Durocrete Mobile App. The copy of the report will be emailed to you on your registered email id once payment is made. For any assistance please contact on our tollfree no. " + tollFree + "<br><br><br>";
                //else
                //    mbody = mbody + "We have tested material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ". The report is ready with us, soft copy of the report has been emailed to you on your registered email id. You can view this report  with help of Durocrete APP on your mobile phone.You can also download the report from our website www.durocrete.in. For any assistance please contact on our tollfree no. " + tollFree + "<br><br><br>";

                //mbody = mbody + "<br>&nbsp;";
                //mbody = mbody + "<br>&nbsp;";
                //mbody = mbody + "<br>";
                //mbody = mbody + "Best Regards,";
                //mbody = mbody + "<br>&nbsp;";
                //mbody = mbody + "<br>";
                //mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";
                try
                {
                    //objMail.SendMail(mailIdTo, mailIdCc, mSubject, mbody, "", mReplyTo);
                    dc.Inward_Update_CRLExceedMailStatus(Convert.ToInt32(recordNo), recordType, true);
                    //update enq-outstanding mail count in route table of that ME
                    if (siteRouteId != 0 && siteMeId != 0)
                        dc.Route_Update_OutsandingMailCount(siteRouteId, siteMeId, 2);

                }
                catch { }
            }
        }
     
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
            DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);
            if (TestingDate > currentDate1)
            {
                lblMsg.Text = "Testing Date should be always less than or equal to the Current Date.";
                valid = false;
            }
            else if (txt_Description.Text == "")
            {
                lblMsg.Text = "Please Enter Description";
                valid = false;
            }
            else if (ddl_Grade.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select Grade";
                valid = false;
            }
            else if (ddl_NablScope.SelectedItem.Text == "--Select--")
            {
                lblMsg.Text = "Select 'NABL Scope'.";
                valid = false;
                ddl_NablScope.Focus();
            }
            else if (ddl_NABLLocation.SelectedItem.Text == "--Select--")
            {
                lblMsg.Text = "Select 'NABL Location'.";
                valid = false;
                ddl_NABLLocation.Focus();
            }
            else if (chk_WitnessBy.Checked && txt_witnessBy.Text == "" && txt_witnessBy.Enabled == true)
            {
                lblMsg.Text = "Please Enter Witness By";
                txt_witnessBy.Focus();
                valid = false;
            }
            else if (lblEntry.Text == "Check" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Approve By";
                valid = false;

            }
            else if (lblEntry.Text == "Enter" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Tested By";
                valid = false;

            }
            else if (valid == true)
            {
                for (int i = 0; i < grdCCHEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_result = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                    if (txt_result.Text == "")
                    {
                        lblMsg.Text = "Please Enter result in (%) for Sr No. " + (i + 1) + ".";
                        txt_result.Focus();
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.CCH_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            //RptCCHReport();
        }
        public void RptCCHReport()
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;

            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = getDetailReportCCH();

            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.CCHReport_Html(txt_ReferenceNo.Text, ddl_Grade.SelectedItem.Text,lblEntry.Text);
        }

        //protected string getDetailReportCCH()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Hydraulic Cement(Chemical)</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var water = dc.ReportStatus_View("Cement Chemical Testing", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);

        //    foreach (var w in water)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CCH" + "-" + w.CCHINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CCH" + "-" + w.CCHINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CCH" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.CCHINWD_CementName_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";



        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + w.CCHINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(w.CCHINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + w.CCHINWD_SupplierName_var.ToString() + "</font></td>" +
        //            "<td align=left valign=top height=19></td>" +
        //            "<td height=19></td>" +
        //            "<td height=19></td>" +
        //            "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";


        //    mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of Test</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Result(%)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits (IS - 12269) </b></font></td>";
        //    mySql += "</tr>";


        //    int i = 0;
        //    int SrNo = 0;

        //    var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CCH");
        //    foreach (var CCH in details)
        //    {

        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width=10% align=center valign=top height=19 ><font size=2>&nbsp;" + CCH.TEST_Name_var + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + CCH.CCHTEST_Result_dec + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + CCH.splmt_SpecifiedLimit_var.ToString() + "</font></td>";
        //        mySql += "</tr>";
        //    }

        //    i++;

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("CCH", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, ddl_Grade.SelectedItem.Text, 0, "CCH");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "CCH");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CCHDetail_RemarkId_int), "CCH");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CCH_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (lblEntry.Text == "Check")
        //    {
        //        var RecNo = dc.ReportStatus_View("Cement Chemical Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.CCHINWD_ApprovedBy_tint != null && r.CCHINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.CCHINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.CCHINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        
        //protected void LnkExit_Click(object sender, EventArgs e)
        //{
        //object refUrl = ViewState["RefUrl"];
        //if (refUrl != null)
        //{
        //    Response.Redirect((string)refUrl);
        //}
        //}
        //protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        //{
        //    //object refUrl = ViewState["RefUrl"];
        //if (refUrl != null)
        //{
        //    Response.Redirect((string)refUrl);
        //}
        //}
        
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void ddl_Grade_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            string strGrade = ddl_Grade.SelectedValue;
            if (strGrade.ToLower().Contains("ppc") == true && strGrade.ToLower().Contains("43") == true)
                strGrade = "PPC 43 Grade";
            else if (strGrade.ToLower().Contains("43") == true)
                strGrade = "43 Grade";
            else if (strGrade.ToLower().Contains("53") == true)
                strGrade = "53 Grade";
            else if (strGrade.ToLower().Contains("ppc") == true)
                strGrade = "PPC Cement";
            else if (strGrade.ToLower().Contains("psc") == true)
                strGrade = "PSC Cement";
            else
                strGrade = "NA";
            var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, strGrade, 0, "CCH");
            foreach (var cch in details)
            {
                TextBox txt_SpecifiedLimits = (TextBox)grdCCHEntryRptInward.Rows[i].FindControl("txt_SpecifiedLimits");
                txt_SpecifiedLimits.Text = cch.splmt_SpecifiedLimit_var;
                i++;
            }
        }

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            #region CCH
            mySql = @"select ref.reference_number,x.[quantity],head.[description],head.[date_of_testing] " +
                    " ,head.[image],head.[witness_by], cch.[calcium_oxide],cch.[silica],cch.[ferric_oxide]  " +
                    " ,cch.[aluminium_oxide],cch.[magnesium_oxide],cch.[sulphuric_anhydride],cch.[loss_on_ignition],cch.[insoluble_residue]  " +
                    " ,cch.[lime_saturation_factor],cch.[ratio_of_alumina]  " +
                    " from [new_gt_app_db].[dbo].[reference_number] ref, [new_gt_app_db].[dbo].[category_refno_wise_test] x, "+
                    " [new_gt_app_db].[dbo].[category_header]  head, [new_gt_app_db].[dbo].cement_chemical cch " +
                    " where ref.reference_number ='"+ txt_ReferenceNo.Text +"'" +
                    " and ref.pk_id = x.reference_number_fk_id and x.category_fk_id = head.category_fk_id  " +
                    " and x.category_wise_test_fk_id = head.test_fk_id and x.reference_number_fk_id = head.reference_no  " +
                    " and x.category_fk_id = 9 and x.category_wise_test_fk_id =  45 " +
                    " and head.pk_id = cch.category_header_fk_id  ";


            dt = objcls.getGeneralData(mySql);
            if (dt.Rows.Count > 0)
            {
                txt_DateOfTesting.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                txt_Description.Text = dt.Rows[0]["description"].ToString();

                if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                {
                    txt_witnessBy.Text = dt.Rows[0]["witness_by"].ToString();
                    chk_WitnessBy.Checked = true;
                    txt_witnessBy.Visible = true;
                }
                
                for (int i = 0; i < grdCCHEntryRptInward.Rows.Count ; i++)
                {
                    TextBox txt_NameOfTest = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                    TextBox txt_result = (TextBox)grdCCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");

                    if (txt_NameOfTest.Text == "Calcium Oxide (CaO)")
                    {
                        txt_result.Text =  dt.Rows[0]["calcium_oxide"].ToString();
                    }
                    else if(txt_NameOfTest.Text == "Silica (SiO2)")
                    {
                        txt_result.Text  = dt.Rows[0]["silica"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Ferric Oxide (Fe2O3)")
                    {
                        txt_result.Text = dt.Rows[0]["ferric_oxide"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Aluminium Oxide (Al2O3)")
                    {
                        txt_result.Text = dt.Rows[0]["aluminium_oxide"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Magnesium Oxide (MgO)")
                    {
                        txt_result.Text = dt.Rows[0]["magnesium_oxide"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Sulphuric Anhydride (SO3)")
                    {
                        txt_result.Text = dt.Rows[0]["sulphuric_anhydride"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Loss on Ignition (LOI)")
                    {
                        txt_result.Text = dt.Rows[0]["loss_on_ignition"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Insoluble Residue (IR)")
                    {
                        txt_result.Text = dt.Rows[0]["insoluble_residue"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Lime Saturation Factor (LSF)")
                    {
                        txt_result.Text = dt.Rows[0]["lime_saturation_factor"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Ratio of Alumina & Iron Oxide (A/F)")
                    {
                        txt_result.Text = dt.Rows[0]["ratio_of_alumina"].ToString();
                    }
                    
                    
                }

            }
            dt.Dispose();
            #endregion
            objcls = null;
        }
    }
}
