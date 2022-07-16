using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Cement_Report : System.Web.UI.Page
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
                    txt_RecType.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEntry.Text = arrIndMsg[1].ToString().Trim();
                    arrIndMsg = arrMsgs[4].Split('=');
                    lblCubeCompStr.Text = arrIndMsg[1].ToString().Trim();
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Cement  - Report Entry ";

                if (txt_RecType.Text != "")
                {
                    //BindGrade();
                    getCurrentTestingDate();
                    DisplayCementDetails();
                    BindDetailsGrid();
                    CompStrength();
                    displayrowheight();
                    if (lblEntry.Text == "Check")
                    {
                        // lblEntry.Text = Session["Check"].ToString();                        
                        lblheading.Text = "Cement - Report Check ";
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
                        ViewWitnessBy();
                        DisplayRemark();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
                        AddRowCemtChemicalRemark();
                        //LoadOtherPendingRpt();
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

            var reportList = dc.ReferenceNo_View_StatusWise("CEMT", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Cement Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "CEMTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "CEMTINWD_ReferenceNo_var";
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
            var testinguser = dc.ReportStatus_View("Cement Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "CEMTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "CEMTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(txt_ReferenceNo.Text);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }

        public void CompStrength()
        {

            //if ( lblCubeCompStr.Text == "CubeCompStrength" || lblCubeCompStr.Text == "CementStrength")
            //{
            int Days = 0;
            for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                TextBox txt_Status = (TextBox)grdCementEntryRptInward.Rows[i].Cells[3].FindControl("txt_Status");
                string[] CompressiveTest = txt_NameOfTest.Text.Split('(', ')', ' ');
                foreach (var Comp in CompressiveTest)
                {
                    if (Comp != "")
                    {
                        if (int.TryParse(Comp, out Days))
                        {
                            Days = Convert.ToInt32(Comp.ToString());
                            break;
                        }
                    }
                }
                //if (Convert.ToInt32(txt_Status.Text) >= 2)
                //{
                var CompStr = dc.OtherCubeTestView(txt_ReferenceNo.Text, "CEMT", Convert.ToByte(Days), 0, "CEMT", false, true);
                foreach (var cms in CompStr)
                {
                    txt_result.Text = Convert.ToString(cms.Avg_var);
                }
                //}
            }

            //}
            // //Session["CubeCompStrength"] = null;
            // //Session["CementStrength"] = null;
            lblCubeCompStr.Text = "";
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
                var ct = dc.ReportStatus_View("Cement Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.CEMTINWD_WitnessBy_var.ToString() != null && c.CEMTINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.CEMTINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }

        }
        public void DeleteRow()
        {
            if (grdCementRemark.Rows.Count > 1)
            {
                DeleteRowCemtRemark(grdCementRemark.Rows.Count - 1);
            }
        }

        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "CEMT");
            foreach (var r in re)
            {
                AddRowCemtChemicalRemark();
                TextBox txt_REMARK = (TextBox)grdCementRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CEMTDetail_RemarkId_int), "CEMT");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.CEMT_Remark_var.ToString();
                    i++;
                }
            }
            if (grdCementRemark.Rows.Count <= 0)
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
            if (grdCementRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdCementRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowCemtRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowCemtRemark(int rowIndex)
        {
            GetCurrentDataCemtChemicalRemark();
            DataTable dt = ViewState["CEMTRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CEMTRemarkTable"] = dt;
            grdCementRemark.DataSource = dt;
            grdCementRemark.DataBind();
            SetPreviousDataCemtChemicalRemark();
        }

        protected void AddRowCemtChemicalRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CEMTRemarkTable"] != null)
            {
                GetCurrentDataCemtChemicalRemark();
                dt = (DataTable)ViewState["CEMTRemarkTable"];
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

            ViewState["CEMTRemarkTable"] = dt;
            grdCementRemark.DataSource = dt;
            grdCementRemark.DataBind();
            SetPreviousDataCemtChemicalRemark();
        }
        protected void GetCurrentDataCemtChemicalRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdCementRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdCementRemark.Rows[i].Cells[1].FindControl("txt_REMARK");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CEMTRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataCemtChemicalRemark()
        {
            DataTable dt = (DataTable)ViewState["CEMTRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdCementRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdCementRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        public void DisplayCementDetails()
        {
            var wInwd = dc.ReportStatus_View("Cement Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var w in wInwd)
            {
                txt_ReferenceNo.Text = w.CEMTINWD_ReferenceNo_var.ToString();
                txt_RecType.Text = w.CEMTINWD_RecordType_var.ToString();
                txt_ReportNo.Text = w.CEMTINWD_SetOfRecord_var.ToString();
                if (w.CEMTINWD_TestedDate_dt.ToString() != null && w.CEMTINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(w.CEMTINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_Description.Text = w.CEMTINWD_Description_var.ToString();
                txt_Supplier.Text = w.CEMTINWD_SupplierName_var.ToString();
                txt_CementUsed.Text = w.CEMTINWD_CementName_var.ToString();
                if (w.CEMTINWD_ReceivedDate_dt != null && w.CEMTINWD_ReceivedDate_dt.ToString() != "")
                {
                    txt_ReceivedDate.Text = w.CEMTINWD_ReceivedDate_dt.ToString();
                }
                else
                {
                    this.txt_ReceivedDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }
                if (w.CEMTINWD_Grade_var != null && w.CEMTINWD_Grade_var != "")
                {
                        ddl_Grade.SelectedValue = w.CEMTINWD_Grade_var;
                    //ddl_Grade.ClearSelection();
                    //ddl_Grade.Items.FindByText(wt.CEMTINWD_Grade_var).Selected = true;
                }
                if (ddl_NablScope.Items.FindByValue(w.CEMTINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(w.CEMTINWD_NablScope_var);
                }
                if (Convert.ToString(w.CEMTINWD_NablLocation_int) != null && Convert.ToString(w.CEMTINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(w.CEMTINWD_NablLocation_int);
                }


            }
        }
        public void BindGrade()
        {
            var matid = dc.Material_View("CEMT", "");
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

        public void BindDetailsGrid()
        {
            int i = 0;
            var wttest = dc.AllInward_View("CEMT", 0, txt_ReferenceNo.Text);
            foreach (var wt in wttest)
            {
                var c = dc.Test_View(0, Convert.ToInt32(wt.CEMTTEST_TEST_Id), "", 0, 0, 0);
                foreach (var n in c)
                {
                    AddRowEnterReportCementInward();
                    TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                    TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                    Button Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                    TextBox txt_Status = (TextBox)grdCementEntryRptInward.Rows[i].Cells[3].FindControl("txt_Status");
                    TextBox txt_TestDetails = (TextBox)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("txt_TestDetails");
                    LinkButton Lnk_Strength = (LinkButton)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("Lnk_Strength");//
                    Label lbl_TestId = (Label)grdCementEntryRptInward.Rows[i].Cells[4].FindControl("lbl_TestId");
                    //if (i == 0)
                    //{
                    //    if (wt.CEMTINWD_Grade_var != null && wt.CEMTINWD_Grade_var != "")
                    //    {
                    //        ddl_Grade.ClearSelection();
                    //        ddl_Grade.Items.FindByText(wt.CEMTINWD_Grade_var).Selected = true;
                    //    }
                    //}
                    if (Convert.ToString(wt.CEMTTEST_Result_var) != "" && Convert.ToString(wt.CEMTTEST_Result_var) != null)
                    {
                        txt_result.Text = Convert.ToString(wt.CEMTTEST_Result_var);
                    }
                    if (Convert.ToString(wt.CEMTTEST_TestStatus_tint) != null && Convert.ToString(wt.CEMTTEST_TestStatus_tint) != "")
                    {
                        txt_Status.Text = Convert.ToString(wt.CEMTTEST_TestStatus_tint);
                    }
                    txt_TestDetails.Text = Convert.ToString(wt.CEMTTEST_Details_var);
                    lbl_TestId.Text = Convert.ToString(wt.CEMTTEST_TEST_Id);

                    if (n.TEST_Name_var.ToString() == "Compressive Strength")
                    {
                        if (wt.CEMTTEST_Days_tint.ToString() != "" && wt.CEMTTEST_Days_tint.ToString() != null && wt.CEMTTEST_Days_tint.ToString() != "0")
                        {
                            txt_NameOfTest.Text = " " + "(" + "" + wt.CEMTTEST_Days_tint.ToString() + " " + "Days" + " " + ")" + " " + n.TEST_Name_var.ToString();
                            Lnk_Strength.Visible = true;
                            Lnk_Strength.Text = "Strength";
                            var inwd = dc.OtherCubeTestView(txt_ReferenceNo.Text, "CEMT", 0, 0, "", true, false);
                            foreach (var cn in inwd)
                            {
                                if (Convert.ToString(cn.CEMTINWD_CubeCastingStatus_tint) == null || Convert.ToInt32(cn.CEMTINWD_CubeCastingStatus_tint) <= 0)
                                {
                                    txt_result.Text = "Awaited";
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        txt_NameOfTest.Text = " " + n.TEST_Name_var.ToString();
                        Lnk_Strength.Visible = false;
                    }
                }
                i++;
            }
            ShowStrength();
        }

        public void ShowStrength()
        {
            var cmInward = dc.ReportStatus_View("Cement Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text, 0, 0, 0);
            foreach (var c in cmInward)
            {
                for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
                {
                    LinkButton Lnk_Strength = (LinkButton)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("Lnk_Strength");
                    if (Lnk_Strength.Visible == true)
                    {
                        if (Convert.ToInt32(c.CEMTINWD_CubeCastingStatus_tint) <= 0 || Convert.ToString(c.CEMTINWD_CubeCastingStatus_tint) == "")
                        {
                            Lnk_Strength.Enabled = false;
                        }
                        else
                        {
                            Lnk_Strength.Enabled = true;
                        }
                    }
                }
            }
        }
        public void EnterStatusShow()
        {
            for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                Button Btn_Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                TextBox txt_TestDetails = (TextBox)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("txt_TestDetails");
                TextBox txt_Status = (TextBox)grdCementEntryRptInward.Rows[i].Cells[3].FindControl("txt_Status");

                if (Convert.ToString(txt_Status.Text) == string.Empty)
                {
                    txt_Status.Text = Convert.ToString(0);
                }

                if (lblEntry.Text != "Check")
                {
                    if (Convert.ToInt32(txt_Status.Text) < 2)
                    {
                        if (txt_result.Visible == true)
                        {
                            txt_result.ReadOnly = false;
                        }
                    }
                    else
                    {
                        if (txt_result.Visible == true && txt_result.Text != string.Empty)
                        {
                            txt_result.ReadOnly = true;
                        }
                    }
                    if (txt_result.Visible == true && txt_result.Text == "Awaited")
                    {
                        txt_result.ReadOnly = false;
                    }
                }
                else if (lblEntry.Text == "Check")
                {
                    if (Convert.ToInt32(txt_Status.Text) == 2)
                    {
                        if (txt_result.Visible == true && txt_result.Text != string.Empty)
                        {
                            txt_result.ReadOnly = false;
                        }
                        if (txt_result.Visible == true && txt_result.Text == "Awaited")
                        {
                            txt_result.ReadOnly = true;
                        }
                    }
                    else
                    {
                        if (txt_result.Visible == true && txt_result.Text != string.Empty)
                        {
                            txt_result.ReadOnly = true;
                        }
                        if (txt_result.Visible == true && txt_result.Text == "Awaited")
                        {
                            txt_result.ReadOnly = true;
                        }
                    }
                }
                string TestName = txt_NameOfTest.Text.Substring(txt_NameOfTest.Text.LastIndexOf(')') + 1);
                if (TestName.Trim() == "Compressive Strength")
                {
                    txt_result.ReadOnly = true;
                }
            }

        }
        protected void AddRowEnterReportCementInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CementTestTable"] != null)
            {
                GetCurrentDataReportCementInward();
                dt = (DataTable)ViewState["CementTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_NameOfTest", typeof(string)));//
                dt.Columns.Add(new DataColumn("Lnk_Strength", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_result", typeof(string)));
                dt.Columns.Add(new DataColumn("Btn_Result", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TestDetails", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Status", typeof(string)));//
                dt.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_NameOfTest"] = string.Empty;
            dr["Lnk_Strength"] = string.Empty;
            dr["txt_result"] = string.Empty;
            dr["Btn_Result"] = string.Empty;
            dr["txt_TestDetails"] = string.Empty;
            dr["txt_Status"] = string.Empty;
            dr["lbl_TestId"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CementTestTable"] = dt;
            grdCementEntryRptInward.DataSource = dt;
            grdCementEntryRptInward.DataBind();
            SetPreviousDataReportCementInward();
        }
        protected void GetCurrentDataReportCementInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_NameOfTest", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Lnk_Strength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_result", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Btn_Result", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TestDetails", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Status", typeof(string)));//
            dtTable.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));

            for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                LinkButton Lnk_Strength = (LinkButton)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("Lnk_Strength");
                TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                Button Btn_Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                TextBox txt_TestDetails = (TextBox)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("txt_TestDetails");
                TextBox txt_Status = (TextBox)grdCementEntryRptInward.Rows[i].Cells[3].FindControl("txt_Status");
                Label lbl_TestId = (Label)grdCementEntryRptInward.Rows[i].Cells[4].FindControl("lbl_TestId");

                drRow = dtTable.NewRow();
                drRow["txt_NameOfTest"] = txt_NameOfTest.Text;
                drRow["Lnk_Strength"] = Lnk_Strength.Text;
                drRow["txt_result"] = txt_result.Text;
                drRow["Btn_Result"] = Btn_Result.Text;
                drRow["txt_TestDetails"] = txt_TestDetails.Text;
                drRow["txt_Status"] = txt_Status.Text;
                drRow["lbl_TestId"] = lbl_TestId.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CementTestTable"] = dtTable;

        }
        protected void SetPreviousDataReportCementInward()
        {
            DataTable dt = (DataTable)ViewState["CementTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                LinkButton Lnk_Strength = (LinkButton)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("Lnk_Strength");
                TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                Button Btn_Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                TextBox txt_TestDetails = (TextBox)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("txt_TestDetails");
                TextBox txt_Status = (TextBox)grdCementEntryRptInward.Rows[i].Cells[3].FindControl("txt_Status");
                Label lbl_TestId = (Label)grdCementEntryRptInward.Rows[i].Cells[4].FindControl("lbl_TestId");

                grdCementEntryRptInward.Rows[i].Cells[1].BackColor = System.Drawing.Color.White;
                grdCementEntryRptInward.Rows[i].Cells[2].BackColor = System.Drawing.Color.White;
                txt_NameOfTest.Text = dt.Rows[i]["txt_NameOfTest"].ToString();
                Lnk_Strength.Text = dt.Rows[i]["Lnk_Strength"].ToString();
                txt_result.Text = dt.Rows[i]["txt_result"].ToString();
                Btn_Result.Text = dt.Rows[i]["Btn_Result"].ToString();
                txt_TestDetails.Text = dt.Rows[i]["txt_TestDetails"].ToString();
                txt_Status.Text = dt.Rows[i]["txt_Status"].ToString();
                lbl_TestId.Text = dt.Rows[i]["lbl_TestId"].ToString();

            }

        }


        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);

                //ll be added after in report Status
                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 0, 0, "", 0, ddl_Grade.SelectedValue, "", 0, 0, 0, "CEMT");
                    dc.ReportDetails_Update("CEMT", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "CEMT", txt_ReferenceNo.Text, "CEMT", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 0, 0, "", 0, ddl_Grade.SelectedValue, "", 0, 0, 0, "CEMT");
                    dc.ReportDetails_Update("CEMT", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "CEMT", txt_ReferenceNo.Text, "CEMT", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "CEMT", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                #region SaveData
                //int TestId = 0;
                bool chkTestStatus = false;
                for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
                {
                    Button Btn_Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                    TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                    TextBox txt_TestDetails = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_TestDetails");
                    TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                    TextBox txt_Status = (TextBox)grdCementEntryRptInward.Rows[i].Cells[3].FindControl("txt_Status");
                    Label lbl_TestId = (Label)grdCementEntryRptInward.Rows[i].Cells[4].FindControl("lbl_TestId");

                    string line = txt_NameOfTest.Text.Substring(txt_NameOfTest.Text.LastIndexOf(')') + 1);
                    byte Days = 0;
                    string TestName = txt_NameOfTest.Text;
                    if (line.Trim() == "Compressive Strength")
                    {
                        string[] days = txt_NameOfTest.Text.Split('(', ' ', ')');
                        foreach (string cdays in days)
                        {
                            if (cdays != "" && cdays != " ")
                            {
                                Days = Convert.ToByte(cdays.ToString());
                                break;
                            }
                        }
                        if (txt_result.Text != "Awaited")
                        {
                            if (lbl_TestedBy.Text == "Tested By")
                            {
                                dc.OtherCubeTest_Update(txt_ReferenceNo.Text, "CEMT", Convert.ToByte(Days), 0, 2, "", 0,"", true, false);
                            }
                            else if (lbl_TestedBy.Text == "Approve By")
                            {
                                dc.OtherCubeTest_Update(txt_ReferenceNo.Text, "CEMT", Convert.ToByte(Days), 0, 3, "", 0,"", true, false);
                            }
                        }
                        TestName = line.ToString();
                    }

                    //var cemtytest = dc.Test(0, "", 0, "CEMT", TestName.Trim());
                    //foreach (var cemt in cemtytest)
                    //{
                    //    TestId = Convert.ToInt32(cemt.TEST_Id);
                    //}                    
                    //TestId = Convert.ToInt32(lbl_TestId.Text);
                    string Result = string.Empty;
                    if (Btn_Result.Text.Trim() != "Awaited" && Btn_Result.Visible == true)
                    {
                        Result = Btn_Result.Text;
                        if (lbl_TestedBy.Text == "Tested By")
                        {
                            if (Convert.ToByte(txt_Status.Text) < 2)
                            {
                                txt_Status.Text = "2";
                            }
                        }
                        else if (lbl_TestedBy.Text == "Approve By")
                        {
                            if (Convert.ToByte(txt_Status.Text) == 2)
                            {
                                txt_Status.Text = "3";
                            }
                        }
                    }
                    if (txt_result.Text.Trim() != "Awaited" && txt_result.Visible == true)
                    {
                        Result = txt_result.Text;
                        if (lbl_TestedBy.Text == "Tested By")
                        {
                            if (Convert.ToByte(txt_Status.Text) < 2)
                            {
                                txt_Status.Text = "2";
                            }
                        }
                        else if (lbl_TestedBy.Text == "Approve By")
                        {
                            if (Convert.ToByte(txt_Status.Text) == 2)
                            {
                                txt_Status.Text = "3";
                            }
                        }
                    }

                    if (Btn_Result.Text.Trim() == "Awaited" && Btn_Result.Visible == true)
                    {
                        Result = "Awaited";
                    }
                    else if (txt_result.Text.Trim() == "Awaited" && txt_result.Visible == true)
                    {
                        Result = "Awaited";
                    }

                    if (Days <= 0)
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, "", 0, null, "", "", 0, Convert.ToInt32(lbl_TestId.Text), Convert.ToString(Result), 0, "", Convert.ToString(txt_TestDetails.Text), Convert.ToByte(txt_Status.Text), 0, 0, "CEMT");
                    }
                    else
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, "", 0, null, "", "", 0, Convert.ToInt32(lbl_TestId.Text), Convert.ToString(Result), 0, "", Convert.ToString(txt_TestDetails.Text), Convert.ToByte(txt_Status.Text), Days, 0, "CEMT");
                    }

                    //Chcek Test- Status
                    //var cemttest = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, Convert.ToInt32(TestId), "", 0, 0, 0, 0, 0, "", Days, "CEMT");
                    //foreach (var cm in cemttest)
                    //{
                    //    if (cm.CEMTTEST_TestStatus_tint < 2 || cm.CEMTTEST_TestStatus_tint == null)
                    //    {
                    //        chkTestStatus = true;
                    //    }
                    //}
                    if (Convert.ToString(txt_Status.Text) == string.Empty)
                    {
                        txt_Status.Text = Convert.ToString(0);
                    }

                    if (Convert.ToInt32(txt_Status.Text) < 2 || Convert.ToString(txt_Status.Text) == string.Empty || Convert.ToInt32(txt_Status.Text) > 3)
                    {
                        chkTestStatus = true;
                    }

                }
                if (chkTestStatus == false)
                {
                    //Update Report StATUS
                    if (lbl_TestedBy.Text == "Tested By")
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 2, 0, "", 0, "", "", 0, 0, 0, "CEMT");
                    }
                    else if (lbl_TestedBy.Text == "Approve By")
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 3, 0, "", 0, "", "", 0, 0, 0, "CEMT");
                    }
                }
                #endregion

                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CEMT", true);
                for (int i = 0; i < grdCementRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdCementRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CEMT");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.CEMT_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "CEMT");
                            foreach (var c in chkId)
                            {
                                if (c.CEMTDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CEMT", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "CEMT");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CEMT");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.CEMT_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CEMT", false);
                            }
                        }
                    }
                }
                #endregion
                lnkPrint.Visible = true;
                //
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;

                //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record Saved Successfully');", true);
                lnkSave.Enabled = false;
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            //string dispalyMsg = "";
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
                int j = 0;
                for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                    Button Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                    if (Result.Text == "Awaited" || txt_result.Text == "Awaited")
                    {
                        j++;
                    }
                    if (txt_result.Visible == true && txt_result.Text == "")
                    {
                        lblMsg.Text = "Please Enter result in (%) for Sr No. " + (i + 1) + ".";
                        txt_result.Focus();
                        valid = false;
                        break;
                    }
                    else if (j == grdCementEntryRptInward.Rows.Count)
                    {
                        lblMsg.Text = "It requires at least one result";
                        valid = false;
                        break;
                    }
                    else if (txt_result.Visible == true && txt_result.Text != "Awaited" && txt_result.Text != "*" && txt_result.Text.ToUpper() != "NIL" && Convert.ToDouble(txt_result.Text) <= 0)
                    {
                        lblMsg.Text = "Result should be greater than 0 for Sr No. " + (i + 1) + ".";
                        txt_result.Focus();
                        valid = false;
                        break;
                    }
                    else if (Result.Visible == true && Result.Text != "Awaited" && Result.Text != "*" && Result.Text.ToUpper() != "NIL" && Convert.ToDouble(Result.Text) <= 0)
                    {
                        lblMsg.Text = "Result should be greater than 0 for Sr No. " + (i + 1) + ".";
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        protected void LnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
            //object refUrl = ViewState["RefUrl"];
            //if (refUrl != null)
            //{
            //    Response.Redirect((string)refUrl);
            //}
        }
        //protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        //{

        //object refUrl = ViewState["RefUrl"];
        //if (refUrl != null)
        //{
        //    Response.Redirect((string)refUrl);
        //}
        //  }

        protected void AddRowGrdIRFR()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["GrdIRFRTable"] != null)
            {
                GetCurrentDataGrdIRFR();
                dt = (DataTable)ViewState["GrdIRFRTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IRgrid", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FRgrid", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_IRgrid"] = string.Empty;
            dr["txt_FRgrid"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["GrdIRFRTable"] = dt;
            GrdIRFR.DataSource = dt;
            GrdIRFR.DataBind();
            SetPreviousDataGrdIRFR();
        }

        protected void GetCurrentDataGrdIRFR()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IRgrid", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FRgrid", typeof(string)));

            for (int i = 0; i < GrdIRFR.Rows.Count; i++)
            {
                TextBox txt_IRgrid = (TextBox)GrdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                TextBox txt_FRgrid = (TextBox)GrdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");

                drRow = dtTable.NewRow();
                drRow["txt_IRgrid"] = txt_IRgrid.Text;
                drRow["txt_FRgrid"] = txt_FRgrid.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["GrdIRFRTable"] = dtTable;

        }
        protected void SetPreviousDataGrdIRFR()
        {
            DataTable dt = (DataTable)ViewState["GrdIRFRTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IRgrid = (TextBox)GrdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                TextBox txt_FRgrid = (TextBox)GrdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");

                txt_IRgrid.Text = dt.Rows[i]["txt_IRgrid"].ToString();
                txt_FRgrid.Text = dt.Rows[i]["txt_FRgrid"].ToString();
            }
        }

        public void DisplayGridIRFRRow()
        {
            GrdIRFR.DataSource = null;
            GrdIRFR.DataBind();
            for (int i = 0; i < 2; i++)
            {
                AddRowGrdIRFR();
            }
        }

        public void displayrowheight()
        {

            for (int j = 0; j < grdCementEntryRptInward.Rows.Count; j++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[j].Cells[0].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[j].Cells[1].FindControl("txt_result");
                Button Result = (Button)grdCementEntryRptInward.Rows[j].Cells[1].FindControl("Btn_Result");

                //if ( lblEntry.Text != "Check" )
                //{
                //    // grdCementEntryRptInward.RowStyle.Height = 30;
                //    if (txt_NameOfTest.Text == " Density" || txt_NameOfTest.Text == " Soundness by Autoclave"
                //        || txt_NameOfTest.Text == " Soundness By Le-Chateliers Apparatus" || txt_NameOfTest.Text == " Initial Setting" || txt_NameOfTest.Text == " Final Setting")
                //    {
                //        Result.Visible = true;
                //        txt_result.Visible = false;
                //        Result.Text = "Awaited";
                //        Result.ForeColor = System.Drawing.Color.Black;
                //    }
                //    else
                //    {
                //        Result.Visible = false;
                //        txt_result.Visible = true;
                //        txt_result.Text = "Awaited";
                //    }
                //    if (txt_NameOfTest.Text == " Standard Consistency")
                //    {
                //        Result.Visible = false;
                //        txt_result.Visible = true;
                //        txt_result.Text = string.Empty;
                //    }
                //}
                //else
                //{
                if (txt_NameOfTest.Text == " Density" || txt_NameOfTest.Text == " Soundness by Autoclave"
              || txt_NameOfTest.Text == " Soundness By Le-Chateliers Apparatus" || txt_NameOfTest.Text == " Initial Setting Time" || txt_NameOfTest.Text == " Final Setting Time")
                {
                    Result.Visible = true;
                    txt_result.Visible = false;
                    if (txt_result.Text != "")
                    {
                        Result.Text = txt_result.Text;
                    }
                    else
                    {
                        Result.Text = "Awaited";
                    }
                    Result.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    Result.Visible = false;
                    txt_result.Visible = true;
                    if (txt_result.Text != string.Empty)
                    {
                        txt_result.Text = txt_result.Text;
                    }
                    else
                    {
                        txt_result.Text = "Awaited";
                    }
                }
                if (txt_NameOfTest.Text == " Standard Consistency")
                {
                    Result.Visible = false;
                    txt_result.Visible = true;
                    if (txt_result.Text != "")
                    {
                        txt_result.Text = txt_result.Text;
                    }
                    if (txt_result.Text == "Awaited")
                    {
                        txt_result.Text = string.Empty;
                    }
                }
                //}
            }
            EnterStatusShow();
            ShoWGrid();
        }





        protected void Btn_Result_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
            TextBox txt_NameOfTest = (TextBox)clickedRow.FindControl("txt_NameOfTest");
            TextBox txt_TestDetails = (TextBox)clickedRow.FindControl("txt_TestDetails");
            Button Btn_Result = (Button)clickedRow.FindControl("Btn_Result");
            TextBox txt_Status = (TextBox)clickedRow.FindControl("txt_Status");
            if (txt_NameOfTest.Text.Trim() == "Density")
            {
                divDensity.Visible = true;
                divSndnesautoclave.Visible = false;
                divSndnsByLe_Chateliers.Visible = false;
                divIni_finSet.Visible = false;
                txt_Weight.Focus();
            }
            if (txt_NameOfTest.Text.Trim() == "Soundness by Autoclave")
            {
                divSndnesautoclave.Visible = true;
                divDensity.Visible = false;
                divSndnsByLe_Chateliers.Visible = false;
                divIni_finSet.Visible = false;
                txt_IR.Focus();
            }
            if (txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus")
            {
                divSndnsByLe_Chateliers.Visible = true;
                divDensity.Visible = false;
                divSndnesautoclave.Visible = false;
                divIni_finSet.Visible = false;
                DisplayGridIRFRRow();
            }
            if (txt_NameOfTest.Text.Trim() == "Initial Setting Time" || txt_NameOfTest.Text.Trim() == "Final Setting Time")
            {
                divIni_finSet.Visible = true;
                divSndnesautoclave.Visible = false;
                divDensity.Visible = false;
                divSndnsByLe_Chateliers.Visible = false;
                txt_WaterAdded.Focus();
            }
            //if (Session["Check"] != null)
            //{
            //    displayTestDetails();
            //}
            //else
            //{
            int Testid = 0;
            int j = 0;
            txt_Weight.Text = string.Empty;
            txt_Volume.Text = string.Empty;
            txt_IR.Text = string.Empty;
            txt_FR.Text = string.Empty;
            txt_WaterAdded.Text = string.Empty;
            txt_IST.Text = string.Empty;
            txt_FST.Text = string.Empty;
            if (lblEntry.Text == "Check")
            {
                if (Convert.ToInt32(txt_Status.Text) > 2 || Convert.ToInt32(txt_Status.Text) < 2 || Convert.ToInt32(txt_Status.Text) != 2)
                {
                    chk_Awaited.Enabled = false;
                    txt_Weight.ReadOnly = true;
                    txt_Volume.ReadOnly = true;
                    txt_IR.ReadOnly = true;
                    txt_FR.ReadOnly = true;
                    txt_WaterAdded.ReadOnly = true;
                    txt_IST.ReadOnly = true;
                    txt_FST.ReadOnly = true;
                }
                else
                {
                    if (Btn_Result.Text != "Awaited")
                    {
                        chk_Awaited.Enabled = false;
                    }
                    else
                    {
                        chk_Awaited.Enabled = true;
                    }
                    txt_Weight.ReadOnly = false;
                    txt_Volume.ReadOnly = false;
                    txt_IR.ReadOnly = false;
                    txt_FR.ReadOnly = false;
                    txt_WaterAdded.ReadOnly = false;
                    txt_IST.ReadOnly = false;
                    txt_FST.ReadOnly = false;
                }
            }
            else
            {
                if (Convert.ToInt32(txt_Status.Text) >= 2)
                {
                    chk_Awaited.Enabled = false;
                    txt_Weight.ReadOnly = true;
                    txt_Volume.ReadOnly = true;
                    txt_IR.ReadOnly = true;
                    txt_FR.ReadOnly = true;
                    txt_WaterAdded.ReadOnly = true;
                    txt_IST.ReadOnly = true;
                    txt_FST.ReadOnly = true;
                }
                else
                {
                    chk_Awaited.Enabled = true;
                    txt_Weight.ReadOnly = false;
                    txt_Volume.ReadOnly = false;
                    txt_IR.ReadOnly = false;
                    txt_FR.ReadOnly = false;
                    txt_WaterAdded.ReadOnly = false;
                    txt_IST.ReadOnly = false;
                    txt_FST.ReadOnly = false;
                }
            }

            var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
            foreach (var cemt in cemtytest)
            {
                Testid = Convert.ToInt32(cemt.TEST_Id);
            }
            if (txt_TestDetails.Text != string.Empty)
            {
                if (Btn_Result.Text.Trim() != "Awaited")
                {
                    string line = txt_TestDetails.Text.Substring(txt_TestDetails.Text.LastIndexOf('|') + 1);
                    string[] line2 = line.Split('~');
                    foreach (string line3 in line2)
                    {
                        j = 0;
                        if (divDensity.Visible == true && txt_NameOfTest.Text.Trim() == "Density")
                        {
                            chk_Awaited.Checked = false;
                            if (line3 != "" && txt_Weight.Text == "")
                            {
                                txt_Weight.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0)
                            {
                                txt_Volume.Text = line3.ToString();
                            }
                        }
                        if (divSndnesautoclave.Visible == true && txt_NameOfTest.Text.Trim() == "Soundness by Autoclave")
                        {
                            chk_Awaited.Checked = false;
                            if (line3 != "" && txt_IR.Text == "")
                            {
                                txt_IR.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0 && txt_FR.Text == "")
                            {
                                txt_FR.Text = line3.ToString();
                            }
                        }
                        if (divSndnsByLe_Chateliers.Visible == true && txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus")
                        {
                            chk_Awaited.Checked = false;
                            GrdIRFR.DataSource = null;
                            GrdIRFR.DataBind();
                            int g = 0;
                            string[] line4 = txt_TestDetails.Text.Split('$');
                            foreach (var line6 in line4)
                            {
                                if (line6 != "")
                                {
                                    AddRowGrdIRFR();
                                    TextBox txt_IRgrid = (TextBox)GrdIRFR.Rows[g].Cells[0].FindControl("txt_IRgrid");
                                    TextBox txt_FRgrid = (TextBox)GrdIRFR.Rows[g].Cells[1].FindControl("txt_FRgrid");
                                    string lineS = line6.Substring(line6.LastIndexOf('|') + 1);
                                    string[] resultline = lineS.Split('~');
                                    foreach (string rline in resultline)
                                    {
                                        if (rline != "")
                                        {
                                            j = 0;
                                            if (txt_IRgrid.Text == "")
                                            {
                                                txt_IRgrid.Text = rline.ToString();
                                                j++;
                                            }
                                            if (txt_FRgrid.Text == "" && j == 0)
                                            {
                                                txt_FRgrid.Text = rline.ToString();
                                                g++;
                                            }
                                        }
                                    }
                                    if (g == 2)
                                    {
                                        if (lblEntry.Text == "Check")
                                        {
                                            if (Convert.ToInt32(txt_Status.Text) != 2)
                                            {
                                                DisplayGr();
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(txt_Status.Text) >= 2)
                                            {
                                                DisplayGr();
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        if (divIni_finSet.Visible == true && txt_NameOfTest.Text.Trim() == "Initial Setting Time" || txt_NameOfTest.Text.Trim() == "Final Setting Time")
                        {
                            j = 0;
                            chk_Awaited.Checked = false;
                            if (line3 != "" && txt_WaterAdded.Text == "")
                            {
                                txt_WaterAdded.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0 && txt_IST.Text == "")
                            {
                                txt_IST.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0 && txt_FST.Text == "")
                            {
                                txt_FST.Text = line3.ToString();
                            }
                        }
                    }
                }
                else
                {
                    Btn_Result.Text = "Awaited";
                    txt_TestDetails.Text = string.Empty;
                }
            }
            if (Btn_Result.Text == "Awaited")
            {
                if (lblEntry.Text == "Check")
                {
                    if (Convert.ToInt32(txt_Status.Text) != 2)
                    {
                        DisplayGr();
                    }
                }
                else
                {
                    if (Convert.ToInt32(txt_Status.Text) >= 2)
                    {
                        DisplayGr();
                    }
                }
            }
        }

        protected void ImgExit_OnClick(object sender, ImageClickEventArgs e)
        {

        }
        protected void grdCementEntryRptInward_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }
        public void ShoWGrid()
        {
            for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_result");
                Button Btn_Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                TextBox txt_Status = (TextBox)grdCementEntryRptInward.Rows[i].Cells[3].FindControl("txt_Status");
                if (Convert.ToInt32(txt_Status.Text) == 2)
                {
                    if (txt_result.Text != "Awaited" && txt_result.Visible == true)
                    {
                        grdCementEntryRptInward.Rows[i].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightYellow;
                        txt_result.BackColor = System.Drawing.Color.LightYellow;
                        grdCementEntryRptInward.Rows[i].Cells[1].BackColor = System.Drawing.Color.LightYellow;
                        grdCementEntryRptInward.Rows[i].Cells[2].BackColor = System.Drawing.Color.LightYellow;
                    }
                    if (Btn_Result.Text != "Awaited" && Btn_Result.Visible == true)
                    {
                        grdCementEntryRptInward.Rows[i].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightYellow;
                        Btn_Result.BackColor = System.Drawing.Color.LightYellow;
                        grdCementEntryRptInward.Rows[i].Cells[1].BackColor = System.Drawing.Color.LightYellow;
                        grdCementEntryRptInward.Rows[i].Cells[2].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
                else if (Convert.ToInt32(txt_Status.Text) > 2)
                {
                    if (txt_result.Text != "Awaited" && txt_result.Visible == true)
                    {
                        grdCementEntryRptInward.Rows[i].Cells[0].BackColor = System.Drawing.Color.LightPink;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightPink;
                        txt_result.BackColor = System.Drawing.Color.LightPink;
                        grdCementEntryRptInward.Rows[i].Cells[1].BackColor = System.Drawing.Color.LightPink;
                        grdCementEntryRptInward.Rows[i].Cells[2].BackColor = System.Drawing.Color.LightPink;
                    }
                    if (Btn_Result.Text != "Awaited" && Btn_Result.Visible == true)
                    {
                        grdCementEntryRptInward.Rows[i].Cells[0].BackColor = System.Drawing.Color.LightPink;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightPink;
                        Btn_Result.BackColor = System.Drawing.Color.LightPink;
                        grdCementEntryRptInward.Rows[i].Cells[1].BackColor = System.Drawing.Color.LightPink;
                        grdCementEntryRptInward.Rows[i].Cells[2].BackColor = System.Drawing.Color.LightPink;
                    }
                }

            }
        }
        public void DisplayGr()
        {

            for (int i = 0; i < GrdIRFR.Rows.Count; i++)
            {
                TextBox txt_IRgrid = (TextBox)GrdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                TextBox txt_FRgrid = (TextBox)GrdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");
                txt_IRgrid.ReadOnly = true;
                txt_FRgrid.ReadOnly = true;
            }
        }

        protected void Lnk_Strength_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            LinkButton Lnk_Strength = (LinkButton)clickedRow.FindControl("Lnk_Strength");
            TextBox txt_NameOfTest = (TextBox)clickedRow.FindControl("txt_NameOfTest");
            TextBox txt_Status = (TextBox)clickedRow.FindControl("txt_Status");//
            TextBox txt_result = (TextBox)clickedRow.FindControl("txt_result");

            //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            //string strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}"
            //                                                                         , "CEMT", lblRecordNo.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "CubeCompStrength", txt_result.Text, txt_NameOfTest.Text, txt_Status.Text));
            int Days = 0;
            string[] test = txt_NameOfTest.Text.Split('(', ')', ' ');
            foreach (var Comp in test)
            {
                if (Comp != "")
                {
                    if (int.TryParse(Comp, out Days))
                    {
                        Days = Convert.ToInt32(Comp.ToString());
                        break;
                    }
                }
            }
            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            string strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}&Days={10}"
                                                                                     , "CEMT", lblRecordNo.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "CubeCompStrength", txt_result.Text, txt_NameOfTest.Text, txt_Status.Text, Days));
            Response.Redirect(strURLWithData);


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
                    DisplayCementDetails();
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
                    grdCementEntryRptInward.DataSource = null;
                    grdCementEntryRptInward.DataBind();
                    BindDetailsGrid();
                    CompStrength();
                    displayrowheight();
                    grdCementRemark.DataSource = null;
                    grdCementRemark.DataBind();
                    DisplayRemark();
                }
            }
        }

        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateCalculateData() == true)
            {
                ModalPopupExtender1.Hide();
                int Testid = 0;
                for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                    Button Btn_Result = (Button)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("Btn_Result");
                    TextBox txt_TestDetails = (TextBox)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("txt_TestDetails");

                    if (divDensity.Visible == true)
                    {
                        if (txt_NameOfTest.Text.Trim() == "Density")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }
                                if (Convert.ToDecimal(txt_Weight.Text) > 0 && Convert.ToDecimal(txt_Volume.Text) > 0)
                                {
                                    Btn_Result.Text = (Convert.ToDecimal(txt_Weight.Text) / Convert.ToDecimal(txt_Volume.Text)).ToString("0.00");

                                    if (Testid > 0 && Btn_Result.Text != "Awaited")
                                    {
                                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_Weight.Text + "~" + txt_Volume.Text;
                                    }
                                }
                            }
                            else
                            {
                                txt_TestDetails.Text = string.Empty;
                                Btn_Result.Text = "Awaited";
                            }
                        }
                    }
                    if (divIni_finSet.Visible == true)
                    {
                        if (txt_NameOfTest.Text.Trim() == "Initial Setting Time")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }

                                if (txt_WaterAdded.Text != "" && txt_IST.Text != "")
                                {
                                    if (txt_NameOfTest.Text.Trim() == "Initial Setting Time")
                                    {
                                        DateTime WaterAdded = new DateTime();
                                        WaterAdded = Convert.ToDateTime(txt_WaterAdded.Text);
                                        DateTime IST = new DateTime();
                                        IST = Convert.ToDateTime(txt_IST.Text);
                                        TimeSpan ts = WaterAdded.Subtract(IST);
                                        Btn_Result.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));
                                        if (Testid > 0 && Btn_Result.Text != "Awaited")
                                        {
                                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_WaterAdded.Text + "~" + txt_IST.Text + "~" + txt_FST.Text;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Btn_Result.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }
                        }
                        if (txt_NameOfTest.Text.Trim() == "Final Setting Time")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                if (txt_WaterAdded.Text != "" && txt_FST.Text != "")
                                {
                                    chk_Awaited.Checked = false;
                                    DateTime WaterAdded = new DateTime();
                                    WaterAdded = Convert.ToDateTime(txt_WaterAdded.Text);
                                    DateTime FST = new DateTime();
                                    FST = Convert.ToDateTime(txt_FST.Text);
                                    TimeSpan ts = WaterAdded.Subtract(FST);
                                    Btn_Result.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));

                                    if (Testid > 0 && Btn_Result.Text != "Awaited")
                                    {
                                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_WaterAdded.Text + "~" + txt_IST.Text + "~" + txt_FST.Text;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                Btn_Result.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }

                        }
                    }
                    if (divSndnsByLe_Chateliers.Visible == true)
                    {
                        decimal Diffrnce = 0;
                        if (txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }
                                txt_TestDetails.Text = string.Empty;
                                for (int j = 0; j < GrdIRFR.Rows.Count; j++)
                                {
                                    TextBox txt_IRgrid = (TextBox)GrdIRFR.Rows[j].Cells[0].FindControl("txt_IRgrid");
                                    TextBox txt_FRgrid = (TextBox)GrdIRFR.Rows[j].Cells[1].FindControl("txt_FRgrid");
                                    if (Convert.ToDecimal(txt_FRgrid.Text) > Convert.ToDecimal(txt_IRgrid.Text))
                                    {
                                        Btn_Result.Text = Convert.ToString(Convert.ToDecimal(txt_FRgrid.Text) - Convert.ToDecimal(txt_IRgrid.Text));
                                        if (j == 0)
                                        {
                                            Diffrnce = Convert.ToDecimal(Btn_Result.Text);
                                        }
                                        if (Testid > 0 && Btn_Result.Text != "Awaited")
                                        {
                                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_IRgrid.Text + "~" + txt_FRgrid.Text + "$";
                                        }
                                    }
                                }
                                if (Diffrnce > 0 && Convert.ToDecimal(Btn_Result.Text) > 0)
                                {
                                    Btn_Result.Text = Convert.ToString(Convert.ToDecimal(Diffrnce + Convert.ToDecimal(Btn_Result.Text)) / 2);
                                    if (Btn_Result.Text == Convert.ToString(Math.Round(Convert.ToDecimal(Btn_Result.Text))))
                                    {
                                        Btn_Result.Text = Convert.ToDecimal(Btn_Result.Text).ToString("00.00");
                                    }
                                    else if (Convert.ToDouble(Btn_Result.Text) != Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 && Convert.ToDouble(Btn_Result.Text) < Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 && (Convert.ToDouble(Btn_Result.Text) != Math.Floor(Convert.ToDouble(Btn_Result.Text))))
                                    {
                                        Btn_Result.Text = Convert.ToString(Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5);
                                    }
                                    else
                                    {
                                        Btn_Result.Text = Convert.ToString(Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 + 0.5);
                                    }

                                    Btn_Result.Text = Convert.ToDecimal(Btn_Result.Text).ToString("00.00");
                                }
                            }
                            else
                            {
                                Btn_Result.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }
                        }
                    }

                    if (divSndnesautoclave.Visible == true)
                    {
                        if (txt_NameOfTest.Text.Trim() == "Soundness by Autoclave")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }
                                if (Convert.ToDecimal(txt_FR.Text) > Convert.ToDecimal(txt_IR.Text))
                                {
                                    Btn_Result.Text = Convert.ToDecimal(Convert.ToDecimal(txt_FR.Text) - Convert.ToDecimal(txt_IR.Text)).ToString();
                                    if (Btn_Result.Text == Convert.ToString(Math.Round(Convert.ToDecimal(Btn_Result.Text))))
                                    {
                                        Btn_Result.Text = Convert.ToDecimal(Btn_Result.Text).ToString("00.00");
                                    }
                                    else if (Convert.ToDouble(Btn_Result.Text) != Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 && Convert.ToDouble(Btn_Result.Text) < Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 && (Convert.ToDouble(Btn_Result.Text) != Math.Floor(Convert.ToDouble(Btn_Result.Text))))
                                    {
                                        Btn_Result.Text = Convert.ToString(Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5);
                                    }
                                    else
                                    {
                                        Btn_Result.Text = Convert.ToString(Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 + 0.5);
                                    }
                                    Btn_Result.Text = Convert.ToDecimal(Btn_Result.Text).ToString("00.00");
                                    if (Testid > 0 && Btn_Result.Text != "Awaited")
                                    {
                                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_IR.Text + "~" + txt_FR.Text;
                                    }
                                }
                            }
                            else
                            {
                                Btn_Result.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }
                        }

                    }
                }
                ModalPopupExtender1.Hide();
            }
        }
        protected Boolean ValidateCalculateData()
        {
            bool valid = true;

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (chk_Awaited.Checked == false && divDensity.Visible == true && txt_Weight.Visible == true && txt_Weight.Text == "")
            {
                lblMsg.Text = "Please Enter Weight";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divDensity.Visible == true && txt_Volume.Visible == true && txt_Volume.Text == "")
            {
                lblMsg.Text = "Please Enter Volume";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divSndnesautoclave.Visible == true && txt_IR.Visible == true && txt_IR.Text == "")
            {
                lblMsg.Text = "Please Enter IR";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divSndnesautoclave.Visible == true && txt_FR.Visible == true && txt_FR.Text == "")
            {
                lblMsg.Text = "Please Enter FR";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divIni_finSet.Visible == true && txt_WaterAdded.Visible == true && txt_WaterAdded.Text == "")
            {
                lblMsg.Text = "Please Enter Water Added ";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divIni_finSet.Visible == true && txt_IST.Visible == true && txt_IST.Text == "")
            {
                lblMsg.Text = "Please Enter IST";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divIni_finSet.Visible == true && txt_FST.Visible == true && txt_FST.Text == "")
            {
                lblMsg.Text = "Please Enter FST";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divSndnesautoclave.Visible == true && valid == true)
            {
                if (Convert.ToDecimal(txt_FR.Text) < Convert.ToDecimal(txt_IR.Text))
                {
                    lblMsg.Text = "FR value should be greater than the IR value";
                    valid = false;
                }
            }
            else if (chk_Awaited.Checked == false && divSndnsByLe_Chateliers.Visible == true && valid == true)
            {
                if (GrdIRFR.Visible == true)
                {
                    for (int i = 0; i < GrdIRFR.Rows.Count; i++)
                    {
                        TextBox txt_IRgrid = (TextBox)GrdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                        TextBox txt_FRgrid = (TextBox)GrdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");

                        if (txt_IRgrid.Text == "")
                        {
                            lblMsg.Text = "Please Enter  IR  of Grid for Sr No. " + (i + 1) + ".";
                            valid = false;
                        }
                        else if (txt_FRgrid.Text == "")
                        {
                            lblMsg.Text = "Please Enter  FR  of Grid for Sr No. " + (i + 1) + ".";
                            valid = false;
                        }
                        else if (Convert.ToDecimal(txt_FRgrid.Text) < Convert.ToDecimal(txt_IRgrid.Text))
                        {
                            lblMsg.Text = "FR value should be greater than the IR value for Sr No. " + (i + 1) + ".";
                            valid = false;
                        }
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + lblMsg.Text + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            string strRefNo = txt_ReferenceNo.Text;
            #region Flyash
            for (int i = 0; i < grdCementEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdCementEntryRptInward.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("txt_result");
                Button Btn_Result = (Button)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("Btn_Result");
                TextBox txt_TestDetails = (TextBox)grdCementEntryRptInward.Rows[i].Cells[2].FindControl("txt_TestDetails");

                DataTable dt;
                if (txt_NameOfTest.Text.Trim() == "Standard Consistency")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, fcon.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_consistency as fcon on chead.pk_id = fcon.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 17 order by fcon.quantity_no";
                    dt = objcls.getGeneralData(mySql);
                    if (dt.Rows.Count > 0)
                        txt_result.Text = dt.Rows[0]["consistency"].ToString();

                    if (i == 0 && dt.Rows.Count > 0)
                    {
                        txt_DateOfTesting.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                        txt_Description.Text = dt.Rows[0]["description"].ToString();

                        if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                        {
                            txt_witnessBy.Text = dt.Rows[0]["witness_by"].ToString();
                            chk_WitnessBy.Checked = true;
                            txt_witnessBy.Visible = true;
                        }
                    }
                    dt.Dispose();
                }
                else if (txt_NameOfTest.Text.Trim() == "Density")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, spegrv.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_density as spegrv on chead.pk_id = spegrv.category_header_fk_id 
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 18 order by spegrv.quantity_no";
                    dt = objcls.getGeneralData(mySql);
                    if (dt.Rows.Count > 0)
                    {
                        int Testid = 0;
                        var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                        foreach (var cemt in cemtytest)
                        {
                            Testid = Convert.ToInt32(cemt.TEST_Id);
                        }
                        if (Convert.ToDecimal(dt.Rows[0]["weight"].ToString()) > 0 && Convert.ToDecimal(dt.Rows[0]["volume"].ToString()) > 0)
                        {
                            Btn_Result.Text = (Convert.ToDecimal(dt.Rows[0]["weight"].ToString()) / Convert.ToDecimal(dt.Rows[0]["volume"].ToString())).ToString("0.00");
                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[0]["weight"].ToString() + "~" + dt.Rows[0]["volume"].ToString();
                        }
                    }
                    dt.Dispose();
                }
                else if (txt_NameOfTest.Text.Trim() == "Initial Setting Time")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, istfst.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_ist_fst istfst on chead.pk_id = istfst.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 19 order by istfst.quantity_no";
                    dt = objcls.getGeneralData(mySql);

                    if (dt.Rows.Count > 0)
                    {
                        int Testid = 0;
                        var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                        foreach (var cemt in cemtytest)
                        {
                            Testid = Convert.ToInt32(cemt.TEST_Id);
                        }

                        DateTime WaterAdded = new DateTime();
                        WaterAdded = Convert.ToDateTime(dt.Rows[0]["water_added"].ToString());
                        DateTime IST = new DateTime();
                        IST = Convert.ToDateTime(dt.Rows[0]["initial_setting_time"].ToString());
                        TimeSpan ts = WaterAdded.Subtract(IST);
                        Btn_Result.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));
                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[0]["water_added"].ToString() + "~" + dt.Rows[0]["initial_setting_time"].ToString() + "~" + dt.Rows[0]["final_setting_time"].ToString();
                    }
                    dt.Dispose();
                }
                else if (txt_NameOfTest.Text.Trim() == "Final Setting Time")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, istfst.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_ist_fst istfst on chead.pk_id = istfst.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 19 order by istfst.quantity_no";
                    dt = objcls.getGeneralData(mySql);
                    if (dt.Rows.Count > 0)
                    {
                        int Testid = 0;
                        var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                        foreach (var cemt in cemtytest)
                        {
                            Testid = Convert.ToInt32(cemt.TEST_Id);
                        }

                        DateTime WaterAdded = new DateTime();
                        WaterAdded = Convert.ToDateTime(dt.Rows[0]["water_added"].ToString());
                        DateTime FST = new DateTime();
                        FST = Convert.ToDateTime(dt.Rows[0]["final_setting_time"].ToString());
                        TimeSpan ts = WaterAdded.Subtract(FST);
                        Btn_Result.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));
                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[0]["water_added"].ToString() + "~" + dt.Rows[0]["initial_setting_time"].ToString() + "~" + dt.Rows[0]["final_setting_time"].ToString();
                    }
                    dt.Dispose();
                }
                else if (txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus" ||
                    txt_NameOfTest.Text.Trim() == "Soundness by Autoclave")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, sound.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_soundness sound  on chead.pk_id = sound.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 20 order by sound.quantity_no";
                    dt = objcls.getGeneralData(mySql);

                    if (dt.Rows.Count > 0)
                    {
                        int Testid = 0;
                        var cemtytest = dc.Test(0, "", 0, "CEMT", txt_NameOfTest.Text.Trim(), 0);
                        foreach (var cemt in cemtytest)
                        {
                            Testid = Convert.ToInt32(cemt.TEST_Id);
                        }

                        decimal Diffrnce = 0;
                        if (Convert.ToDecimal(dt.Rows[0]["fr_1"].ToString()) > Convert.ToDecimal(dt.Rows[0]["ir_1"].ToString()))
                        {
                            Btn_Result.Text = Convert.ToString(Convert.ToDecimal(dt.Rows[0]["fr_1"].ToString()) - Convert.ToDecimal(dt.Rows[0]["ir_1"].ToString()));

                            Diffrnce = Convert.ToDecimal(Btn_Result.Text);

                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[0]["ir_1"].ToString() + "~" + dt.Rows[0]["fr_1"].ToString() + "$";
                        }
                        if (Convert.ToDecimal(dt.Rows[0]["fr_2"].ToString()) > Convert.ToDecimal(dt.Rows[0]["ir_2"].ToString()))
                        {
                            Btn_Result.Text = Convert.ToString(Convert.ToDecimal(dt.Rows[0]["fr_2"].ToString()) - Convert.ToDecimal(dt.Rows[0]["ir_2"].ToString()));
                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[0]["ir_2"].ToString() + "~" + dt.Rows[0]["fr_2"].ToString() + "$";
                        }
                        if (Diffrnce > 0 && Convert.ToDecimal(Btn_Result.Text) > 0)
                        {
                            Btn_Result.Text = Convert.ToString(Convert.ToDecimal(Diffrnce + Convert.ToDecimal(Btn_Result.Text)) / 2);
                            if (Btn_Result.Text == Convert.ToString(Math.Round(Convert.ToDecimal(Btn_Result.Text))))
                            {
                                Btn_Result.Text = Convert.ToDecimal(Btn_Result.Text).ToString("00.00");
                            }
                            else if (Convert.ToDouble(Btn_Result.Text) != Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 && Convert.ToDouble(Btn_Result.Text) < Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 && (Convert.ToDouble(Btn_Result.Text) != Math.Floor(Convert.ToDouble(Btn_Result.Text))))
                            {
                                Btn_Result.Text = Convert.ToString(Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5);
                            }
                            else
                            {
                                Btn_Result.Text = Convert.ToString(Math.Floor(Convert.ToDouble(Btn_Result.Text)) + 0.5 + 0.5);
                            }

                            Btn_Result.Text = Convert.ToDecimal(Btn_Result.Text).ToString("00.00");
                        }
                    }
                    dt.Dispose();
                }
                else if (txt_NameOfTest.Text.Trim() == "Fineness By Blain`s apparatus" ||
                    txt_NameOfTest.Text.Trim() == "Fineness By Dry Sieving")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, fine.* 
                     from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_fineness fine on chead.pk_id = fine.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 21 order by fine.quantity_no";
                    dt = objcls.getGeneralData(mySql);
                    if (dt.Rows.Count > 0)
                    {
                        txt_result.Text = dt.Rows[0]["fineness"].ToString();
                    }
                    dt.Dispose();
                }

            }
            #endregion
            objcls = null;
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.Cement_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            // RptCementReport();
        }
        public void RptCementReport()
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = getDetailReportCement();
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.CementReport_Html(txt_ReferenceNo.Text, ddl_Grade.SelectedItem.Text, lblEntry.Text);
        }

        //protected string getDetailReportCement()
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
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Testing</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var Cement = dc.ReportStatus_View("Cement Testing", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);
        //    foreach (var w in Cement)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +// DateTime.Now.ToString("dd/MM/yy")
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CEMT" + "-" + w.CEMTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CEMT" + "-" + w.CEMTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CEMT" + "-" + "" + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.CEMTINWD_CementName_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + w.CEMTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(w.CEMTINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + w.CEMTINWD_SupplierName_var.ToString() + "</font></td>" +
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
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Unit </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Result </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Compliance </b></font></td>";
        //    mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Method Of Testing </b></font></td>";
        //    mySql += "</tr>";

        //    int SrNo = 0;

        //    var details = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CEMT");
        //    foreach (var CEMT in details)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        bool valid = false;
        //        string TEST_Name_var = "";

        //        if (CEMT.TEST_Name_var.ToString() == "Compressive Strength")
        //        {
        //            if (CEMT.CEMTTEST_Days_tint.ToString() != "" && CEMT.CEMTTEST_Days_tint.ToString() != null && CEMT.CEMTTEST_Days_tint.ToString() != "0")
        //            {
        //                TEST_Name_var = " " + "(" + "" + CEMT.CEMTTEST_Days_tint.ToString() + " " + "Days" + " " + ")" + " " + CEMT.TEST_Name_var.ToString();
        //                mySql += "<td width=20% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + "</font></td>";
        //            }
        //        }
        //        else
        //        {
        //            mySql += "<td width=20% align=left valign=top height=19 ><font size=2>&nbsp;" + CEMT.TEST_Name_var.ToString() + "</font></td>";
        //        }
        //        var Id = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, CEMT.TEST_Id, "", 0, 0, 0, 0, 0, "", 0, "CEMT");
        //        foreach (var testid in Id)
        //        {
        //            valid = true;
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_Unit_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(CEMT.CEMTTEST_Result_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_SpecifiedLimit_var) + "</font></td>";
        //            int SpecifiedLmtRes = 0;
        //            bool validmax = false;
        //            string res = "";

        //            string[] SpceifiedLmt = Convert.ToString(testid.splmt_SpecifiedLimit_var).Split(' ', ',');
        //            foreach (var Comp in SpceifiedLmt)
        //            {
        //                if (Comp != "")
        //                {
        //                    if (Comp.Trim() == "Maximum")
        //                    {
        //                        validmax = true;
        //                    }
        //                    if (Comp.Trim() == "PCC" || Comp.Trim() == "RCC")
        //                    {
        //                        res = res + " " + "-" + " " + Comp + "</br>";
        //                    }
        //                    if (int.TryParse(Comp, out SpecifiedLmtRes))
        //                    {
        //                        SpecifiedLmtRes = Convert.ToInt32(Comp.ToString());
        //                        if (validmax == true)
        //                        {
        //                            if (Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "Awaited" && Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "*" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
        //                            {
        //                                if ((Convert.ToDouble(CEMT.CEMTTEST_Result_var)) < Convert.ToInt32(SpecifiedLmtRes))
        //                                {
        //                                    res = res + "Pass ";
        //                                    //  mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Pass" + "</font></td>";
        //                                }
        //                                else
        //                                {
        //                                    res = res + "Fail ";
        //                                    //  mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Fail" + "</font></td>";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                res = "---";
        //                                //mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "---" + "</font></td>";
        //                            }

        //                        }
        //                        else
        //                        {
        //                            if (Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "Awaited" && Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "*" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
        //                            {
        //                                if ((Convert.ToDouble(CEMT.CEMTTEST_Result_var)) > Convert.ToInt32(SpecifiedLmtRes))
        //                                {
        //                                    res = res + "Pass ";
        //                                    // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Pass" + "</font></td>";
        //                                }
        //                                else
        //                                {
        //                                    res = res + "Fail ";
        //                                    // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Fail" + "</font></td>";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                res = "---";
        //                                // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "---" + "</font></td>";
        //                            }

        //                        }
        //                    }

        //                }
        //            }
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + res + "</font></td>";
        //            mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>" + Convert.ToString(testid.splmt_testingMethod_var) + "</font></td>";
        //            break;
        //        }
        //        if (valid == false)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(CEMT.CEMTTEST_Result_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //        }
        //        mySql += "</tr>";
        //    }

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    SrNo = 0;
        //    var matid = dc.Material_View("CEMT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, ddl_Grade.SelectedItem.Text, 0, "CEMT");
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
        //    var re = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "CEMT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CEMTDetail_RemarkId_int), "CEMT");
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
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CEMT_Remark_var.ToString() + "</font></td>";
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
        //        var RecNo = dc.ReportStatus_View("Chemical Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.CEMTINWD_ApprovedBy_tint != null && r.CEMTINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.CEMTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.CEMTINWD_CheckedBy_tint, -1, "", "", "");
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

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
        }
        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
        }


    }
}