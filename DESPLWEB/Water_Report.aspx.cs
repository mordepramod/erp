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
    public partial class Water_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                    txt_RecordType.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEntry.Text = arrIndMsg[1].ToString().Trim();
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Water - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                if (txt_RecordType.Text != "")
                {
                    getCurrentTestingDate();
                    DisplayRemark();
                    LoadTestedBy();
                    DisplayWTDetails();
                    BindDetailsGrid();
                    LoadReferenceNoList();
                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "Water - Report Check";
                        lbl_TestedBy.Text = "Approve By";
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
                        BindResultGrid();
                        ViewWitnessBy();
                    }
                    else
                    {
                        //LoadOtherPendingRpt();
                        LoadTestedBy();
                    }
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

            var reportList = dc.ReferenceNo_View_StatusWise("WT", reportStatus,0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        public void getCurrentTestingDate()
        {
            txt_DateOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Water Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "WTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "WTINWD_ReferenceNo_var";
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
            var testinguser = dc.ReportStatus_View("Water Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "WTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "WTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        protected void AddRowEnterReportWTInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["WTTestTable"] != null)
            {
                GetCurrentDataWTTestInward();
                dt = (DataTable)ViewState["WTTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("TEST_Name_var", typeof(string)));
                dt.Columns.Add(new DataColumn("WTTEST_TEST_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("splmt_Unit_var", typeof(string)));
                dt.Columns.Add(new DataColumn("splmt_SpecifiedLimit_var", typeof(string)));
                dt.Columns.Add(new DataColumn("splmt_testingMethod_var", typeof(string)));
            }
            dr = dt.NewRow();
            dr["TEST_Name_var"] = string.Empty;
            dr["WTTEST_TEST_Id"] = string.Empty;
            dr["splmt_Unit_var"] = string.Empty;
            dr["splmt_SpecifiedLimit_var"] = string.Empty;
            dr["splmt_testingMethod_var"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["WTTestTable"] = dt;
            grdWaterEntryRptInward.DataSource = dt;
            grdWaterEntryRptInward.DataBind();
            SetPreviousDataWTTestInward();
        }
        protected void GetCurrentDataWTTestInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("TEST_Name_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("WTTEST_TEST_Id", typeof(string)));
            dtTable.Columns.Add(new DataColumn("splmt_Unit_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("splmt_SpecifiedLimit_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("splmt_testingMethod_var", typeof(string)));

            for (int i = 0; i < grdWaterEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                TextBox txt_Unit = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[3].FindControl("txt_Unit");
                TextBox txt_SpecifiedLimits = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[4].FindControl("txt_SpecifiedLimits");
                TextBox txt_MetodOfTsting = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[5].FindControl("txt_MetodOfTsting");

                drRow = dtTable.NewRow();
                drRow["TEST_Name_var"] = txt_NameOfTest.Text;
                drRow["WTTEST_TEST_Id"] = txt_result.Text;
                drRow["splmt_Unit_var"] = txt_Unit.Text;
                drRow["splmt_SpecifiedLimit_var"] = txt_SpecifiedLimits.Text;
                drRow["splmt_testingMethod_var"] = txt_MetodOfTsting.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["WTTestTable"] = dtTable;

        }
        protected void SetPreviousDataWTTestInward()
        {
            DataTable dt = (DataTable)ViewState["WTTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                TextBox txt_Unit = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[3].FindControl("txt_Unit");
                TextBox txt_SpecifiedLimits = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[4].FindControl("txt_SpecifiedLimits");
                TextBox txt_MetodOfTsting = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[5].FindControl("txt_MetodOfTsting");

                txt_NameOfTest.Text = dt.Rows[i]["TEST_Name_var"].ToString();
                txt_result.Text = dt.Rows[i]["WTTEST_TEST_Id"].ToString();
                txt_Unit.Text = dt.Rows[i]["splmt_Unit_var"].ToString();
                txt_SpecifiedLimits.Text = dt.Rows[i]["splmt_SpecifiedLimit_var"].ToString();
                txt_MetodOfTsting.Text = dt.Rows[i]["splmt_testingMethod_var"].ToString();

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
                    DisplayWTDetails();
                    //LoadOtherPendingCheckRpt();
                    LoadReferenceNoList();
                    ViewWitnessBy();
                    LoadApproveBy();
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                finally
                {
                    grdWaterEntryRptInward.DataSource = null;
                    grdWaterEntryRptInward.DataBind();
                    BindDetailsGrid();
                    if(lblEntry.Text == "Check")
                    {
                        BindResultGrid();
                    }
                    grdWaterRemark.DataSource = null;
                    grdWaterRemark.DataBind();
                    DisplayRemark();
                }
            }
        }

        public void DisplayWTDetails()
        {
            var wInwd = dc.ReportStatus_View("Water Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var w in wInwd)
            {
                txt_ReferenceNo.Text = w.WTINWD_ReferenceNo_var.ToString();
                txt_ReportNo.Text = w.WTINWD_SetOfRecord_var.ToString();
                if (w.WTINWD_TestedDate_dt != null && w.WTINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(w.WTINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_Description.Text = w.WTINWD_Description_var.ToString();
                txt_Supplier.Text = w.WTINWD_SupplierName_var.ToString();
                txt_ReportNo.Text = w.WTINWD_SetOfRecord_var.ToString();
                txt_RecordType.Text = w.WTINWD_RecordType_var.ToString();
                if (ddl_NablScope.Items.FindByValue(w.WTINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(w.WTINWD_NablScope_var);
                }
                if (Convert.ToString(w.WTINWD_NablLocation_int) != null && Convert.ToString(w.WTINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(w.WTINWD_NablLocation_int);
                }

            }
        }
        public void BindDetailsGrid()
        {
            //int i = 0;
            var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", 0, "WT");
            grdWaterEntryRptInward.DataSource = details;
            grdWaterEntryRptInward.DataBind();
            //var deta1ils = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", "WT");
            //foreach (var w in deta1ils)
            //{
            //    AddRowEnterReportWTInward();
            //    TextBox txt_NameOfTest = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
            //    TextBox txt_result = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
            //    TextBox txt_Unit = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[3].FindControl("txt_Unit");
            //    TextBox txt_SpecifiedLimits = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[4].FindControl("txt_SpecifiedLimits");
            //    TextBox txt_MetodOfTsting = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[5].FindControl("txt_MetodOfTsting");

            //    txt_NameOfTest.Text = w.TEST_Name_var.ToString();
            //    txt_Unit.Text = w.splmt_Unit_var.ToString();
            //    txt_SpecifiedLimits.Text = w.splmt_SpecifiedLimit_var.ToString();
            //    txt_MetodOfTsting.Text = w.splmt_testingMethod_var.ToString();
            //    i++;
            //}
            if (grdWaterEntryRptInward.Rows.Count <= 0)
            {
                AddRowEnterReportWTInward();
            }
        }
        public void BindResultGrid()
        {
            int TestId = 0;
            for (int i = 0; i < grdWaterEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_result = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                TestId = Convert.ToInt32((txt_result.CssClass));
                var wttest = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, TestId, "", 0, 0, 0, 0, 0, "", 0, "WT");
                foreach (var w in wttest)
                {
                    txt_result.Text = w.WTTEST_Result_var.ToString();
                }
            }
        }
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "WT");
            foreach (var r in re)
            {
                AddRowWaterRemark();
                TextBox txt_REMARK = (TextBox)grdWaterRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.WTDetail_RemarkId_int), "WT");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.WT_Remark_var.ToString();
                    i++;
                }
            }
            if (grdWaterRemark.Rows.Count <= 0)
            {
                AddRowWaterRemark();
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
                var ct = dc.ReportStatus_View("Water Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.WTINWD_WitnessBy_var.ToString() != null && c.WTINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.WTINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }

        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {

                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);


                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, "", 2, 0, "", 0, "", "", 0, 0, 0, "WT");
                    dc.ReportDetails_Update("WT", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "WT", txt_ReferenceNo.Text, "WT", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, "", 3, 0, "", 0, "", "", 0, 0, 0, "WT");
                    dc.ReportDetails_Update("WT", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "WT", txt_ReferenceNo.Text, "WT", null, false, true, false, false, false, false, false);
                }

                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "WT", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                int TestId = 0;
                for (int i = 0; i < grdWaterEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_result = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                    TestId = Convert.ToInt32((txt_result.CssClass));

                    dc.AllInwd_Update(txt_ReferenceNo.Text, "", 0, null, "", "", 0, TestId, txt_result.Text, 0, "", "", 0, 0, 0, "WT");
                }


                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "WT", true);
                for (int i = 0; i < grdWaterRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdWaterRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "WT");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.WT_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "WT");
                            foreach (var c in chkId)
                            {
                                if (c.WTDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "WT", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "WT");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "WT");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.WT_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "WT", false);
                            }
                        }
                    }
                }
                #endregion

                lnkPrint.Visible = true;
                //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record Saved Successfully');", true);
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        protected Boolean ValidateData()
        {
            //   string dispalyMsg = "";
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
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
                for (int i = 0; i < grdWaterEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_NameOfTest = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                    TextBox txt_result = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                    TextBox txt_Unit = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[3].FindControl("txt_Unit");
                    TextBox txt_SpecifiedLimits = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[4].FindControl("txt_SpecifiedLimits");
                    TextBox txt_MetodOfTsting = (TextBox)grdWaterEntryRptInward.Rows[i].Cells[5].FindControl("txt_MetodOfTsting");
                    if (txt_NameOfTest.Text == "")
                    {
                        lblMsg.Text = "Please Enter Name of the Test in (%) for Sr No. " + (i + 1) + ".";
                        txt_NameOfTest.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_result.Text == "")
                    {
                        lblMsg.Text = "Please Enter result in (%) for Sr No. " + (i + 1) + ".";
                        txt_result.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Unit.Text == "")
                    {
                        lblMsg.Text = "Please Enter Unit for Sr No. " + (i + 1) + ".";
                        txt_Unit.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_SpecifiedLimits.Text == "")
                    {
                        lblMsg.Text = "Please Enter Specified Limits for Sr No. " + (i + 1) + ".";
                        txt_SpecifiedLimits.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_MetodOfTsting.Text == "")
                    {
                        lblMsg.Text = "Please Enter Method of Testing for Sr No. " + (i + 1) + ".";
                        txt_MetodOfTsting.Focus();
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
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

        protected void DeleteRowWaterRemark(int rowIndex)
        {
            GetCurrentDataWaterRemark();
            DataTable dt = ViewState["WaterRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["WaterRemarkTable"] = dt;
            grdWaterRemark.DataSource = dt;
            grdWaterRemark.DataBind();
            SetPreviousDataWaterRemark();
        }
        protected void AddRowWaterRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["WaterRemarkTable"] != null)
            {
                GetCurrentDataWaterRemark();
                dt = (DataTable)ViewState["WaterRemarkTable"];
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

            ViewState["WaterRemarkTable"] = dt;
            grdWaterRemark.DataSource = dt;
            grdWaterRemark.DataBind();
            SetPreviousDataWaterRemark();
        }

        protected void GetCurrentDataWaterRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdWaterRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdWaterRemark.Rows[i].Cells[1].FindControl("txt_REMARK");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["WaterRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataWaterRemark()
        {
            DataTable dt = (DataTable)ViewState["WaterRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdWaterRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdWaterRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }

        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowWaterRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdWaterRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdWaterRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowWTRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowWTRemark(int rowIndex)
        {
            GetCurrentDataWaterRemark();
            DataTable dt = ViewState["WaterRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["WaterRemarkTable"] = dt;
            grdWaterRemark.DataSource = dt;
            grdWaterRemark.DataBind();
            SetPreviousDataWaterRemark();
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.WT_PDFReport(txt_ReferenceNo.Text,lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecordType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            //RptWaterReport();
        }
        //public void RptWaterReport()
        //{
        //    string reportPath;
        //    string reportStr = "";
        //    StreamWriter sw;
        //    reportPath = Server.MapPath("~") + "\\report.html";
        //    sw = File.CreateText(reportPath);
        //    PrintHTMLReport rptHtml = new PrintHTMLReport();
        //  //  reportStr = rptHtml.getDetailReportWT();
        //    sw.WriteLine(reportStr);
        //    sw.Close();
        //    NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

        //}
        //protected string getDetailReportWT()
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
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Water For Construction Purpose</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var water = dc.ReportStatus_View("Water Testing", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);

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

        //             "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //             "<td width='2%' height=19><font size=2></font></td>" +
        //             "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "WT" + "-" + w.WTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "WT" + "-" + w.WTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "WT" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";



        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td height=19><font size=2></font></td>" +
        //              "<td height=19><font size=2>" + w.WTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(w.WTINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

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
        //    mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Test Parameters</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Observations </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Compliance </b></font></td>";

        //    mySql += "<td>";
        //    mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Permissible Limit IS:456-2000 </b></font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Mixing and Curing </br> Water Clause 5:4 Table 1 </b></font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table >";
        //    mySql += "</td>";

        //    mySql += "<td width= 20% align=center valign=medium height=19 ><font size=2><b>Test Specification Used </b></font></td>";
        //    mySql += "</tr>";

        //    int i = 0;
        //    int SrNo = 0;
        //    // int TestId=0;

        //    var wInwd = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", 0, "WT");
        //    foreach (var wt in wInwd)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.TEST_Name_var + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.splmt_Unit_var.ToString() + "</font></td>";
        //        //TestId=wt.TEST_Id;
        //        //var wttest = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false,TestId, "",0,0,0,0,0,"","WT");
        //        //foreach (var w1 in wttest)
        //        //{
        //        mySql += "<td width=2% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.WTTEST_Result_var.ToString() + "</font></td>";
        //        //}

        //        //Complaince 
        //        string SpecifiedLmt = "";
        //        double Obsns = 0;
        //        int result = 0;
        //        string PRC = "";
        //        bool valid = false;
        //        string Compliance = "";
        //        string Observations = wt.WTTEST_Result_var;

        //        if (double.TryParse(Observations, out Obsns))
        //        {
        //            Obsns = Convert.ToDouble(Observations);
        //        }
        //        SpecifiedLmt = wt.splmt_SpecifiedLimit_var.ToString();
        //        string[] line = SpecifiedLmt.Split(' ', ',', '-');
        //        foreach (string line1 in line)
        //        {
        //            if (line1 != " ")
        //            {
        //                if (line1 == "PCC" || line1 == "RCC")
        //                {
        //                    PRC = line1.ToString();
        //                    if (Convert.ToInt32(ViewState["res"]) > 0)
        //                    {
        //                        //Maximum 2000 - PCC,Maximum 500 - RCC
        //                        if (Obsns < Convert.ToInt32(ViewState["res"]))
        //                        {
        //                            Compliance = Compliance + "Pass " + " " + "-" + " " + PRC + "," + "<br />";
        //                            valid = true;
        //                        }
        //                        else
        //                        {
        //                            Compliance = Compliance + "Fail " + " " + "-" + " " + PRC + "," + "<br />";
        //                            valid = true;
        //                        }
        //                    }
        //                }
        //                if (int.TryParse(line1, out result))
        //                {
        //                    result = Convert.ToInt32(line1);
        //                    Session["res"] = result.ToString();
        //                    ViewState["res"] = Session["res"].ToString();
        //                }
        //            }
        //        }
        //        if (valid == true)
        //        {
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Compliance + " </font></td>";
        //        }
        //        if (valid == false)
        //        {
        //            if (wt.WTTEST_Result_var == "NIL" || SpecifiedLmt == "---")
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            }
        //            else if (Obsns < result)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Pass" + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Fail" + "</font></td>";

        //            }
        //        }

        //        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>" + wt.splmt_SpecifiedLimit_var.ToString() + "</font></td>";
        //        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>" + wt.splmt_testingMethod_var.ToString() + "</font></td>";


        //        mySql += "</tr>";
        //        i++;
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("WT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
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
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "(Fourth Revision)" + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }

        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "WT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.WTDetail_RemarkId_int), "WT");
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
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.WT_Remark_var.ToString() + "</font></td>";
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
        //        var RecNo = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, Convert.ToInt32(lblRecordNo.Text), "", 0, "WT");
        //        foreach (var r in RecNo)
        //        {
        //            var Auth = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, r.WTINWD_ApprovedBy_tint, 0, 0, "", 0, "WT");

        //            foreach (var Approve in Auth)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Approve.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + Approve.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                mySql += "</tr>";

        //            }
        //            var lgin = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, r.WTINWD_CheckedBy_tint, 0, "", 0, "WT");

        //            foreach (var loginusr in lgin)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
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
    }
}
