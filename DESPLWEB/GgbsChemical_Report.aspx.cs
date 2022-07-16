using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class GgbsChemical_Report : System.Web.UI.Page
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
                lblheading.Text = "GGBS Chemical - Report Entry ";
                if (txt_RecType.Text != "")
                {
                    txt_DateOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    DisplayGGBSCHDetails();
                    if (lblEntry.Text == "Check")
                    {
                        lbl_TestedBy.Text = "Approve By";
                        lblheading.Text = "GGBS Chemical - Report Check ";                        
                        DisplayGGBSCHGrid();
                        DisplayRemark();
                    }
                    else
                    {
                        AddRowGgbsChemicalRemark();
                    }
                    LoadTestedOrApproveBy();
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

            var reportList = dc.ReferenceNo_View_StatusWise("GGBSCH", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        private void LoadTestedOrApproveBy()
        {
            byte testingBit = 0;
            byte approveBit = 0;            
            if (lblEntry.Text == "Check")            
                approveBit = 1;            
            else            
                testingBit = 1;                           
            var testinguser = dc.ReportStatus_View("", null, null, 0, testingBit, approveBit, "", 0, 0, 0);
            ddl_TestedBy.DataTextField = "USER_Name_var";
            ddl_TestedBy.DataValueField = "USER_Id";
            ddl_TestedBy.DataSource = testinguser;
            ddl_TestedBy.DataBind();
            ddl_TestedBy.Items.Insert(0, "---Select---");
        }
        public void DisplayGGBSCHDetails()
        {
            txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            var wInwd = dc.ReportStatus_View("GGBS Chemical Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var w in wInwd)
            {
                txt_ReferenceNo.Text = w.GGBSCHINWD_ReferenceNo_var.ToString();
                if (w.GGBSCHINWD_TestedDate_dt.ToString() != null && w.GGBSCHINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(w.GGBSCHINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_Description.Text = w.GGBSCHINWD_Description_var.ToString();
                txt_Supplier.Text = w.GGBSCHINWD_SupplierName_var.ToString();
                txt_GgbsUsed.Text = w.GGBSCHINWD_GgbsName_var.ToString();
                txt_ReportNo.Text = w.GGBSCHINWD_SetOfRecord_var.ToString();
                txt_RecType.Text = w.GGBSCHINWD_RecordType_var.ToString();
               
                if (ddl_NablScope.Items.FindByValue(w.GGBSCHINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(w.GGBSCHINWD_NablScope_var);
                }
                if (Convert.ToString(w.GGBSCHINWD_NablLocation_int) != null && Convert.ToString(w.GGBSCHINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(w.GGBSCHINWD_NablLocation_int);
                }
                if (w.GGBSCHINWD_WitnessBy_var.ToString() != null && w.GGBSCHINWD_WitnessBy_var.ToString() != "")
                {
                    txt_witnessBy.Visible = true;
                    txt_witnessBy.Text = w.GGBSCHINWD_WitnessBy_var.ToString();
                    chk_WitnessBy.Checked = true;
                }
                if (w.GGBSCHINWD_ReportDetails_var != null && w.GGBSCHINWD_ReportDetails_var.ToString() != "")
                {
                    string[] strVal = w.GGBSCHINWD_ReportDetails_var.Split('~');
                    string[] strVal1 = strVal[0].Split('|');
                    chkCal1.Checked = Convert.ToBoolean(strVal1[0]);
                    lblCal1.Text = strVal1[1];

                    strVal1 = strVal[1].Split('|');
                    chkCal2.Checked = Convert.ToBoolean(strVal1[0]);
                    lblCal2.Text = strVal1[1];

                    strVal1 = strVal[0].Split('|');
                    chkCal3.Checked = Convert.ToBoolean(strVal1[0]);
                    lblCal3.Text = strVal1[1];
                }
            }
            var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "GGBSCH");
            grdGGBSCHEntryRptInward.DataSource = details;
            grdGGBSCHEntryRptInward.DataBind();
        }
        public void DisplayGGBSCHGrid()
        {
            int i = 0;
            var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "GGBSCH");
            foreach (var ggbsch in details)
            {
                TextBox txt_result = (TextBox)grdGGBSCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                if (ggbsch.GGBSCHTEST_Result_dec.ToString() != "" && ggbsch.GGBSCHTEST_Result_dec != null)
                {
                    txt_result.Text = ggbsch.GGBSCHTEST_Result_dec.ToString();
                }
                i++;
            }
        }
        public void DisplayRemark()
        {
            grdGGBSCHRemark.DataSource = null;
            grdGGBSCHRemark.DataBind();
            int i = 0;
            var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "GGBSCH");
            foreach (var r in re)
            {
                AddRowGgbsChemicalRemark();
                TextBox txt_REMARK = (TextBox)grdGGBSCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.GGBSCHDetail_RemarkId_int), "GGBSCH");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.GGBSCH_Remark_var.ToString();
                    i++;
                }
            }
            if (grdGGBSCHRemark.Rows.Count <= 0)
            {
                AddRowGgbsChemicalRemark();
            }
        }
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---")
            {
                try
                {
                    lnkSave.Enabled = true;
                    lnkPrint.Visible = false;
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Visible = false;
                    ddl_TestedBy.SelectedIndex = 0;
                    chkCal1.Checked = false;
                    chkCal2.Checked = false;
                    chkCal3.Checked = false;
                    lblCal1.Text = "";
                    lblCal2.Text = "";
                    lblCal3.Text = "";
                    grdGGBSCHEntryRptInward.DataSource = null;
                    grdGGBSCHEntryRptInward.DataBind();
                    grdGGBSCHRemark.DataSource = null;
                    grdGGBSCHRemark.DataBind();

                    txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                    LoadReferenceNoList();
                    DisplayGGBSCHDetails();                    
                    DisplayGGBSCHGrid();
                    DisplayRemark();
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculate();
                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
                string strCalculate = "";                
                strCalculate += chkCal1.Checked.ToString() + "|" + lblCal1.Text + "~";
                strCalculate += chkCal2.Checked.ToString() + "|" + lblCal2.Text + "~";
                strCalculate += chkCal3.Checked.ToString() + "|" + lblCal3.Text + "~";
                
                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, "", 2, 0, "", 0, "", strCalculate, 0, 0, 0, "GGBSCH");
                    dc.ReportDetails_Update("GGBSCH", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "GGBSCH", txt_ReferenceNo.Text, "GGBSCH", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, "", 3, 0, "", 0,"", strCalculate, 0, 0, 0, "GGBSCH");
                    dc.ReportDetails_Update("GGBSCH", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "GGBSCH", txt_ReferenceNo.Text, "GGBSCH", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "GGBSCH", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                int TestId = 0;
                for (int i = 0; i < grdGGBSCHEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_result = (TextBox)grdGGBSCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                    TestId = Convert.ToInt32((txt_result.CssClass));

                    dc.AllInwd_Update(txt_ReferenceNo.Text, "", 0, null, "", "", 0, TestId, "", Convert.ToDecimal(txt_result.Text), "", "", 0, 0, 0, "GGBSCH");
                }

                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "GGBSCH", true);
                for (int i = 0; i < grdGGBSCHRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdGGBSCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "GGBSCH");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.GGBSCH_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "GGBSCH");
                            foreach (var c in chkId)
                            {
                                if (c.GGBSCHDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "GGBSCH", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "GGBSCH");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "GGBSCH");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.GGBSCH_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "GGBSCH", false);
                            }
                        }
                    }
                }
                #endregion

                //Approve on check
                if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.ReportDetails_Update("GGBSCH", txt_ReferenceNo.Text, 0, 0, 0, Convert.ToByte(ddl_TestedBy.SelectedValue), true, "Approved By");
                    dc.MISDetail_Update(0, "GGBSCH", txt_ReferenceNo.Text, "GGBSCH", null, false, false, true, false, false, false, false);
                    int siteCRbypssBit = 0;
                    siteCRbypssBit = cd.getSITECRBypassBit("GGBSCH", Convert.ToInt32(lblRecordNo.Text));
                    if (siteCRbypssBit == 1)
                        dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, "GGBSCH");
                    //SMS
                    var res = dc.SMSDetailsForReportApproval_View(Convert.ToInt32(lblRecordNo.Text), "GGBSCH").ToList();
                    if (res.Count > 0)
                    {
                        cd.sendInwardReportMsg(Convert.ToInt32(res.FirstOrDefault().ENQ_Id), Convert.ToString(res.FirstOrDefault().ENQ_Date_dt), Convert.ToInt32(res.FirstOrDefault().SITE_MonthlyBillingStatus_bit).ToString(), res.FirstOrDefault().crLimitExceededStatus.ToString(), res.FirstOrDefault().BILL_NetAmt_num.ToString(), txt_ReferenceNo.Text, Convert.ToString(res.FirstOrDefault().INWD_ContactNo_var), "Report");
                    }
                }
                lnkPrint.Visible = true;

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            #region GGBSCH
            mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, gc.* 
                     from new_gt_app_db.dbo.reference_number ref, new_gt_app_db.dbo.category_header chead, 
                     new_gt_app_db.dbo.ggbs_chemical gc
                     where ref.pk_id = chead.reference_no and chead.pk_id = gc.category_header_fk_id
                     and ref.reference_number = '" + txt_ReferenceNo.Text + "' order by gc.quantity_no  ";

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

                for (int i = 0; i < grdGGBSCHEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_NameOfTest = (TextBox)grdGGBSCHEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                    TextBox txt_result = (TextBox)grdGGBSCHEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");

                    if (txt_NameOfTest.Text == "Calcium Oxide (CaO)")
                    {
                        txt_result.Text = dt.Rows[0]["calcium_oxide"].ToString();
                    }
                    else if (txt_NameOfTest.Text == "Silica (SiO2)")
                    {
                        txt_result.Text = dt.Rows[0]["silica"].ToString();
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
                    else if (txt_NameOfTest.Text == "Sulphate (SO3)")
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
                }

            }
            dt.Dispose();
            #endregion
            objcls = null;
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
                for (int i = 0; i < grdGGBSCHEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_result = (TextBox)grdGGBSCHEntryRptInward.Rows[i].FindControl("txt_result");
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
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        #region Remark grid
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowGgbsChemicalRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdGGBSCHRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdGGBSCHRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowGgbsChemicalRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowGgbsChemicalRemark(int rowIndex)
        {
            GetCurrentDataGgbsChemicalRemark();
            DataTable dt = ViewState["GGBSCHRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["GGBSCHRemarkTable"] = dt;
            grdGGBSCHRemark.DataSource = dt;
            grdGGBSCHRemark.DataBind();
            SetPreviousDataGgbsChemicalRemark();
        }
        protected void AddRowGgbsChemicalRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["GGBSCHRemarkTable"] != null)
            {
                GetCurrentDataGgbsChemicalRemark();
                dt = (DataTable)ViewState["GGBSCHRemarkTable"];
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

            ViewState["GGBSCHRemarkTable"] = dt;
            grdGGBSCHRemark.DataSource = dt;
            grdGGBSCHRemark.DataBind();
            SetPreviousDataGgbsChemicalRemark();
        }
        protected void GetCurrentDataGgbsChemicalRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdGGBSCHRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdGGBSCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["GGBSCHRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataGgbsChemicalRemark()
        {
            DataTable dt = (DataTable)ViewState["GGBSCHRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdGGBSCHRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdGGBSCHRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        #endregion
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
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
        }
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

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;
            lblMsg.Visible = false;
            for (int i = 0; i < grdGGBSCHEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_result = (TextBox)grdGGBSCHEntryRptInward.Rows[i].FindControl("txt_result");
                if (txt_result.Text == "")
                {
                    lblMsg.Text = "Please Enter result in (%) for Sr No. " + (i + 1) + ".";
                    txt_result.Focus();
                    valid = false;
                    lblMsg.Visible = true;
                    break;
                }
            }
            if (valid == true)
            {
                Calculate();
            }
            else
            {
                lblCal1.Text = "0.00";
                lblCal2.Text = "0.00";
                lblCal3.Text = "0.00";
            }
        }
        protected void Calculate()
        {
            decimal CaO = 0, MgO = 0, Al2O3 = 0, SiO2 = 0, CaS = 0, MnO = 0, cal1 = 0, cal2 = 0, cal3 = 0, temp = 0;
            //CaO + MgO + Al2O3 / SiO2
            //CaO + MgO + 1/3 Al2O3 / SiO2 + 2/3 Al2O3
            //CaO + CaS + 1/2 MgO + Al2O3 / SiO2 + MnO (For granulated slag with > 2.5 Percent MnO)
            for (int i = 0; i < grdGGBSCHEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdGGBSCHEntryRptInward.Rows[i].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdGGBSCHEntryRptInward.Rows[i].FindControl("txt_result");
                if (txt_result.Text != "")
                {
                    if (txt_NameOfTest.Text.Contains("CaO") == true)
                    {
                        CaO = Convert.ToDecimal(txt_result.Text);
                    }
                    else if (txt_NameOfTest.Text.Contains("SiO2") == true)
                    {
                        SiO2 = Convert.ToDecimal(txt_result.Text);
                    }
                    else if (txt_NameOfTest.Text.Contains("Al2O3") == true)
                    {
                        Al2O3 = Convert.ToDecimal(txt_result.Text);
                    }
                    else if (txt_NameOfTest.Text.Contains("MgO") == true)
                    {
                        MgO = Convert.ToDecimal(txt_result.Text);
                    }
                    else if (txt_NameOfTest.Text.Contains("MnO") == true)
                    {
                        MnO = Convert.ToDecimal(txt_result.Text);
                    }
                }
            }
            if (SiO2 != 0)
            {
                cal1 = (CaO + MgO + Al2O3) / SiO2;
            }
            temp = SiO2 + ((Convert.ToDecimal(2) / Convert.ToDecimal(3)) * Al2O3);
            if (temp != 0)
            {
                cal2 = (CaO + MgO + ((Convert.ToDecimal(1) / Convert.ToDecimal(3)) * Al2O3)) / temp;
            }
            temp = SiO2 + MnO;
            if (temp != 0)
            {
                cal3 = (CaO + CaS + ((Convert.ToDecimal(1) / Convert.ToDecimal(3)) * MgO) + Al2O3) / temp;
            }
            lblCal1.Text = cal1.ToString("0.00");
            lblCal2.Text = cal2.ToString("0.00");
            lblCal3.Text = cal3.ToString("0.00");
        }
    }
}
