using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class AAC_Report_SN : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        public static int rowindx = 0;
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
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "AAC Shrinkage Test Inward - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_RecType.Text != "")
                {
                    DisplayAAC_SN_Details();
                    DisplayGridRow();
                    DisplayRemark();
                    DisplayIdMark();
                    txt_RecType.Text = "AAC";

                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "AAC Shrinkage Test Inward - Report Check";
                        LoadApproveBy();
                        DisplayAAC_SNGrid();
                        ViewWitnessBy();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
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

            var reportList = dc.ReferenceNo_View_StatusWise("AAC", reportStatus, Convert.ToInt32(txt_TestId.Text));
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
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
        private void LoadOtherPendingRpt()
        {
            string Refno = txt_ReferenceNo.Text;
            var testinguser = dc.ReportStatus_View("AAC Block Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, Convert.ToInt32(txt_TestId.Text));
            ddl_OtherPendingRpt.DataTextField = "AACINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "AACINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("AAC Block Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, Convert.ToInt32(txt_TestId.Text));
                ddl_OtherPendingRpt.DataTextField = "AACINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "AACINWD_ReferenceNo_var";
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
        public void ViewWitnessBy()
        {
            var ct = dc.ReportStatus_View("AAC Block Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var p in ct)
            {
                if (Convert.ToString(p.AACINWD_WitnessBy_var) != string.Empty)
                {
                    txt_witnessBy.Visible = true;
                    txt_witnessBy.Text = p.AACINWD_WitnessBy_var.ToString();
                    chk_WitnessBy.Checked = true;
                }
                else
                {
                    txt_witnessBy.Visible = false;
                    chk_WitnessBy.Checked = false;
                }
            }

        }

        public void DisplayAAC_SN_Details()
        {
            var Inwardc = dc.ReportStatus_View("AAC Block Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var p in Inwardc)
            {
                txt_ReferenceNo.Text = p.AACINWD_ReferenceNo_var.ToString();


                if (Convert.ToString(p.AACINWD_TestingDate_dt) != null)
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(p.AACINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (Convert.ToString(p.INWD_CollectionDate_dt) != string.Empty)
                {
                    txt_CollectionDt.Text = Convert.ToDateTime(p.INWD_CollectionDate_dt).ToString("dd/MM/yyyy");
                }
                else
                {
                    txt_CollectionDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_RecType.Text = p.AACINWD_RecordType_var.ToString();
                txt_Supplier.Text = p.AACINWD_SupplierName_var.ToString();
                txt_ReportNo.Text = p.AACINWD_SetOfRecord_var.ToString();
                txt_Description.Text = p.AACINWD_Description_var.ToString();
                txt_Qty.Text = p.AACINWD_Quantity_tint.ToString();
                txt_TestId.Text = p.AACINWD_TEST_Id.ToString();
                txtGaugeLength.Text = p.AACINWD_GaugeLength_dec.ToString();
                txtLeastCount.Text = p.AACINWD_LeastCount_dec.ToString();
                if (ddl_NablScope.Items.FindByValue(p.AACINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(p.AACINWD_NablScope_var);
                }

                if (Convert.ToString(p.AACINWD_NablLocation_int) != null && Convert.ToString(p.AACINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(p.AACINWD_NablLocation_int);
                }

            }
        }

        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AAC_Remark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, Convert.ToInt32(txt_TestId.Text));
            foreach (var r in re)
            {
                AddRowAAC_SN_Remark();
                TextBox txt_REMARK = (TextBox)grdAACRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AAC_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32(txt_TestId.Text));
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.Remark_var.ToString();
                    i++;
                }
            }
            if (grdAACRemark.Rows.Count <= 0)
            {
                AddRowAAC_SN_Remark();
            }

        }
        public void DisplayAAC_SNGrid()
        {
            grdAAC.DataSource = null;
            grdAAC.DataBind();
            int i = 0;
            var AAC_SN = dc.AAC_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32(txt_TestId.Text), "SN");
            foreach (var aac in AAC_SN)
            {
                AddRowEnterReportAAC_SNInward();
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_IniWetMesurement = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_IniWetMesurement");
                TextBox txt_FinalDryMesurement = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_FinalDryMesurement");
                TextBox txt_WetMeasurement = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_WetMeasurement");
                TextBox txt_DryMeasurement = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_DryMeasurement");
                TextBox txt_DryShrinkage = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_DryShrinkage");
                TextBox txt_AvgStr = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_AvgStr");

                txt_IdMark.Text = aac.AACTEST_IdMark_var.ToString();
                txt_IniWetMesurement.Text = aac.AACTEST_IniWetMeasuremnt_dec.ToString();
                txt_FinalDryMesurement.Text = aac.AACTEST_FinalDryMeasuremnt_dec.ToString();
                txt_WetMeasurement.Text = aac.AACTEST_WetMeasuremnt_dec.ToString();
                txt_DryMeasurement.Text = aac.AACTEST_DryMeasuremnt_dec.ToString();
                txt_DryShrinkage.Text = aac.AACTEST_DryShrinkage_dec.ToString();
                i++;
            }
            var avg = dc.ReportStatus_View("AAC Block Testing", null, null, 0, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var c in avg)
            {
                if (grdAAC.Rows.Count > 0)
                {
                    int NoOfrows = grdAAC.Rows.Count / 2;
                    TextBox txt_AvgStr = (TextBox)grdAAC.Rows[NoOfrows].Cells[7].FindControl("txt_AvgStr");
                    txt_AvgStr.Text = c.AACINWD_AvgStr_var.ToString();
                    break;
                }
            }

            if (grdAAC.Rows.Count <= 0)
            {
                DisplayGridRow();
            }


        }

        public void DisplayGridRow()
        {
            if (Convert.ToInt32(txt_Qty.Text) > 0)
            {
                if (Convert.ToInt32(txt_Qty.Text) > grdAAC.Rows.Count)
                {
                    for (int i = grdAAC.Rows.Count + 1; i <= Convert.ToInt32(txt_Qty.Text); i++)
                    {
                        AddRowEnterReportAAC_SNInward();
                    }
                }
            }
        }
        protected void AddRowAAC_SN_Remark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AACRemark_SN_Table"] != null)
            {
                GetCurrentDataAAC_SN_Remark();
                dt = (DataTable)ViewState["AACRemark_SN_Table"];
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

            ViewState["AACRemark_SN_Table"] = dt;
            grdAACRemark.DataSource = dt;
            grdAACRemark.DataBind();
            SetPreviousDataAAC_SN_Remark();
        }
        protected void GetCurrentDataAAC_SN_Remark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdAACRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdAACRemark.Rows[i].Cells[1].FindControl("txt_REMARK");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["AACRemark_SN_Table"] = dtTable;

        }
        protected void SetPreviousDataAAC_SN_Remark()
        {
            DataTable dt = (DataTable)ViewState["AACRemark_SN_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdAACRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdAACRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }

        protected void AddRowEnterReportAAC_SNInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AAC_SN_DetailTable"] != null)
            {
                GetCurrentDataAAC_SNInward();
                dt = (DataTable)ViewState["AAC_SN_DetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IniWetMesurement", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FinalDryMesurement", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WetMeasurement", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DryMeasurement", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DryShrinkage", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AvgStr", typeof(string)));

            }
            dr = dt.NewRow();
            dr["txt_IdMark"] = string.Empty;
            dr["txt_IniWetMesurement"] = string.Empty;
            dr["txt_FinalDryMesurement"] = string.Empty;
            dr["txt_WetMeasurement"] = string.Empty;
            dr["txt_DryMeasurement"] = string.Empty;
            dr["txt_DryShrinkage"] = string.Empty;
            dr["txt_AvgStr"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["AAC_SN_DetailTable"] = dt;
            grdAAC.DataSource = dt;
            grdAAC.DataBind();
            SetPreviousDataAAC_SNInward();
        }
        protected void GetCurrentDataAAC_SNInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IniWetMesurement", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FinalDryMesurement", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WetMeasurement", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DryMeasurement", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DryShrinkage", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgStr", typeof(string)));

            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_IniWetMesurement = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_IniWetMesurement");
                TextBox txt_FinalDryMesurement = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_FinalDryMesurement");
                TextBox txt_WetMeasurement = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_WetMeasurement");
                TextBox txt_DryMeasurement = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_DryMeasurement");
                TextBox txt_DryShrinkage = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_DryShrinkage");
                TextBox txt_AvgStr = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_AvgStr");
                drRow = dtTable.NewRow();

                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txt_IniWetMesurement"] = txt_IniWetMesurement.Text;
                drRow["txt_FinalDryMesurement"] = txt_FinalDryMesurement.Text;
                drRow["txt_WetMeasurement"] = txt_WetMeasurement.Text;
                drRow["txt_DryMeasurement"] = txt_DryMeasurement.Text;
                drRow["txt_DryShrinkage"] = txt_DryShrinkage.Text;
                drRow["txt_AvgStr"] = txt_AvgStr.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["AAC_SN_DetailTable"] = dtTable;

        }
        protected void SetPreviousDataAAC_SNInward()
        {
            DataTable dt = (DataTable)ViewState["AAC_SN_DetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_IniWetMesurement = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_IniWetMesurement");
                TextBox txt_FinalDryMesurement = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_FinalDryMesurement");
                TextBox txt_WetMeasurement = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_WetMeasurement");
                TextBox txt_DryMeasurement = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_DryMeasurement");
                TextBox txt_DryShrinkage = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_DryShrinkage");
                TextBox txt_AvgStr = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_AvgStr");

                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txt_IniWetMesurement.Text = dt.Rows[i]["txt_IniWetMesurement"].ToString();
                txt_FinalDryMesurement.Text = dt.Rows[i]["txt_FinalDryMesurement"].ToString();
                txt_WetMeasurement.Text = dt.Rows[i]["txt_WetMeasurement"].ToString();
                txt_DryMeasurement.Text = dt.Rows[i]["txt_DryMeasurement"].ToString();
                txt_DryShrinkage.Text = dt.Rows[i]["txt_DryShrinkage"].ToString();
                txt_AvgStr.Text = dt.Rows[i]["txt_AvgStr"].ToString();

            }

        }
        protected void DeleteDataPT_SNInward(int rowIndex)
        {
            GetCurrentDataAAC_SNInward();
            DataTable dt = ViewState["AAC_SN_DetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AAC_SN_DetailTable"] = dt;
            grdAAC.DataSource = dt;
            grdAAC.DataBind();
            SetPreviousDataAAC_SNInward();
        }
        protected void txt_QtyOnTextChanged(object sender, EventArgs e)
        {
            SNRowsChanged();
        }
        private void SNRowsChanged()
        {
            int qty = 0;
            if (int.TryParse(txt_Qty.Text, out qty))
            {
                if (Convert.ToInt32(txt_Qty.Text) > 0)
                {
                    if (Convert.ToInt32(txt_Qty.Text) < grdAAC.Rows.Count)
                    {
                        for (int i = grdAAC.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                        {
                            if (grdAAC.Rows.Count > 1)
                            {
                                DeleteDataPT_SNInward(i - 1);
                            }
                        }
                    }
                    else
                    {
                        DisplayGridRow();
                    }
                }
            }
            else
            {
                txt_Qty.Text = string.Empty;
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
        protected void lnkCal_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                int SrNo = 1;
                DateTime CollectionDt = DateTime.ParseExact(txt_CollectionDt.Text, "dd/MM/yyyy", null);
                dc.AacSN_Test_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, 0, 0, "", "", null, 0, "", true, 0);//delete  Test entry
                DateTime DateofTesting = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
                txt_Qty.Text = grdAAC.Rows.Count.ToString();
                if (chk_WitnessBy.Checked == false)
                {
                    txt_witnessBy.Text = "";
                }

                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AACTestInward_Update("AAC", txt_ReferenceNo.Text, 2, DateofTesting, CollectionDt, txt_Description.Text, txt_Supplier.Text, txt_witnessBy.Text, "", Convert.ToInt32(txt_Qty.Text), Convert.ToDecimal(txtGaugeLength.Text), Convert.ToDecimal(txtLeastCount.Text));
                     dc.ReportDetails_Update("AAC", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                     dc.MISDetail_Update(0, "AAC", txt_ReferenceNo.Text, "AAC", null, true, false, false, false, false, false, false);
                 
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AACTestInward_Update("AAC", txt_ReferenceNo.Text, 3, DateofTesting, CollectionDt, txt_Description.Text, txt_Supplier.Text, txt_witnessBy.Text, "", Convert.ToInt32(txt_Qty.Text), Convert.ToDecimal(txtGaugeLength.Text), Convert.ToDecimal(txtLeastCount.Text));
                    dc.ReportDetails_Update("AAC", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "AAC", txt_ReferenceNo.Text, "AAC", null, false, true, false, false, false, false, false);
                }

                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "AAC", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                for (int i = 0; i < grdAAC.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_IniWetMesurement = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_IniWetMesurement");
                    TextBox txt_FinalDryMesurement = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_FinalDryMesurement");
                    TextBox txt_WetMeasurement = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_WetMeasurement");
                    TextBox txt_DryMeasurement = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_DryMeasurement");
                    TextBox txt_DryShrinkage = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_DryShrinkage");
                    TextBox txt_AvgStr = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_AvgStr");


                    if (txt_AvgStr.Text != "")
                    {
                        dc.AACTestInward_Update("AAC", txt_ReferenceNo.Text, 0, DateofTesting, CollectionDt, txt_Description.Text, txt_Supplier.Text, txt_witnessBy.Text, Convert.ToString(txt_AvgStr.Text), 0, Convert.ToDecimal(txtGaugeLength.Text), Convert.ToDecimal(txtLeastCount.Text));
                    }
                    dc.AacSN_Test_Update(txt_ReferenceNo.Text, txt_IdMark.Text, Convert.ToDecimal(txt_IniWetMesurement.Text), Convert.ToDecimal(txt_FinalDryMesurement.Text), Convert.ToDecimal(txt_WetMeasurement.Text),
                                           Convert.ToDecimal(txt_DryMeasurement.Text),
                                             Convert.ToDecimal(txt_DryShrinkage.Text), 0, "", "", null, 0, "", false, SrNo);
                    SrNo++;
                }




                //Remark 
                #region Remark Gridview
                int RemarkId = 0;
                dc.AAC_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(txt_TestId.Text), true);
                for (int i = 0; i < grdAACRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdAACRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AAC_Remark_View(txt_REMARK.Text, "", 0, Convert.ToInt32(txt_TestId.Text));
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AAC_Remark_View("", txt_ReferenceNo.Text, 0, Convert.ToInt32(txt_TestId.Text));
                            foreach (var c in chkId)
                            {
                                if (c.RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AAC_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(txt_TestId.Text), false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AAC_Remark_Update(0, txt_REMARK.Text);
                            var chc = dc.AAC_Remark_View(txt_REMARK.Text, "", 0, Convert.ToInt32(txt_TestId.Text));
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.RemarkId_int);
                                dc.AAC_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(txt_TestId.Text), false);
                            }
                        }
                    }
                }

                #endregion



                lnkPrint.Visible = true;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record Saved Successfully');", true);
                lnkSave.Enabled = false;
            }


        }

        protected Boolean ValidateData()
        {

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
            else if (chk_WitnessBy.Checked && txt_witnessBy.Text == "" && txt_witnessBy.Enabled == true)
            {
                lblMsg.Text = "Please Enter Witness By";
                txt_witnessBy.Focus();
                valid = false;
            }
            else if (txt_Qty.Text == string.Empty)
            {
                lblMsg.Text = "Please Enter Qty";
                txt_Qty.Focus();
                valid = false;
            }
            else if (Convert.ToInt32(txt_Qty.Text) <= 0)
            {
                lblMsg.Text = "Qty should be greater than 0";
                txt_Qty.Focus();
                valid = false;
            }
            else if (txtGaugeLength.Text == string.Empty)
            {
                lblMsg.Text = "Please Enter Gauge Length";
                txtGaugeLength.Focus();
                valid = false;
            }
            else if (Convert.ToInt32(txtGaugeLength.Text) <= 0)
            {
                lblMsg.Text = "Gauge Length should be greater than 0";
                txtGaugeLength.Focus();
                valid = false;
            }
            else if (txtLeastCount.Text == string.Empty)
            {
                lblMsg.Text = "Please Enter Least Count of dial gauge";
                txtLeastCount.Focus();
                valid = false;
            }
            else if (Convert.ToDecimal(txtLeastCount.Text) <= 0)
            {
                lblMsg.Text = "Least Count of dial gauge should be greater than 0";
                txtLeastCount.Focus();
                valid = false;
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdAAC.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_IniWetMesurement = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_IniWetMesurement");
                    TextBox txt_FinalDryMesurement = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_FinalDryMesurement");

                    if (txt_IdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No. " + (i + 1) + ".";
                        txt_IdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_IniWetMesurement.Text == "")
                    {
                        lblMsg.Text = "Enter Initial Wet Measurement for Sr No. " + (i + 1) + ".";
                        txt_IniWetMesurement.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_FinalDryMesurement.Text == "")
                    {
                        lblMsg.Text = "Enter Final Dry Mesurement for Sr No. " + (i + 1) + ".";
                        txt_FinalDryMesurement.Focus();
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
                Calculation();
            }
            return valid;
        }
        public void Calculation()
        {
            decimal WetDryM = 0,DryS=0, gaugeLength = 0, leastCountGauge = 0, FinalDryM=0;
            decimal SumOfDryShrkng = 0;
            int NoOfrows = grdAAC.Rows.Count / 2;
            gaugeLength = Convert.ToDecimal(txtGaugeLength.Text);
            leastCountGauge = Convert.ToDecimal(txtLeastCount.Text);


            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_IniWetMesurement = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_IniWetMesurement");
                TextBox txt_FinalDryMesurement = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_FinalDryMesurement");
                TextBox txt_WetMeasurement = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_WetMeasurement");
                TextBox txt_DryMeasurement = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_DryMeasurement");
                TextBox txt_DryShrinkage = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_DryShrinkage");
                TextBox txt_AvgStr = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_AvgStr");

                txt_AvgStr.Text = string.Empty;
                if (Convert.ToDecimal(txt_IniWetMesurement.Text) > 0 && gaugeLength > 0 && leastCountGauge > 0)
                {
                    WetDryM =(  gaugeLength + leastCountGauge * Convert.ToDecimal(txt_IniWetMesurement.Text));
                }
                if (Convert.ToDecimal(txt_FinalDryMesurement.Text) > 0 && gaugeLength > 0 && leastCountGauge > 0)
                {
                    FinalDryM = (gaugeLength + leastCountGauge * Convert.ToDecimal(txt_FinalDryMesurement.Text));
                }

                txt_WetMeasurement.Text = WetDryM.ToString("0.00");
                txt_DryMeasurement.Text = FinalDryM.ToString("0.00");

                if (WetDryM > 0 && FinalDryM > 0)
                     DryS = (WetDryM - FinalDryM) / 153 * 100;

                txt_DryShrinkage.Text = DryS.ToString("0.00");
            }

            int Count = 0;
            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                TextBox txt_DryShrinkage = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_DryShrinkage");
                if (txt_DryShrinkage.Text != "" && txt_DryShrinkage.Text != "---")
                {
                    SumOfDryShrkng += Convert.ToDecimal(txt_DryShrinkage.Text);
                }
                else
                {
                    SumOfDryShrkng += Convert.ToDecimal(0);
                }
                if (txt_DryShrinkage.Text != "" && Convert.ToDecimal(txt_DryShrinkage.Text) > 0)
                {
                    Count++;
                }
            }

            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                int NoOfrow = grdAAC.Rows.Count / 2;
                TextBox txt_AvgStr = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_AvgStr");

                if (i == grdAAC.Rows.Count - 1)
                {
                    if (Count > 0 && SumOfDryShrkng > 0)
                        txt_AvgStr.Text = Convert.ToDecimal(SumOfDryShrkng / Count).ToString("0.00");
                    else
                        txt_AvgStr.Text = "*";

                }

            }

        }

        public void FetchRefNo()
        {
            try
            {
                lnkSave.Enabled = true;
                lnkPrint.Visible = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                DisplayAAC_SN_Details();
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
                grdAAC.DataSource = null;
                grdAAC.DataBind();
                DisplayAAC_SNGrid();
                DisplayIdMark();
                grdAACRemark.DataSource = null;
                grdAACRemark.DataBind();
                DisplayRemark();
            }
        }

        public void DisplayIdMark()
        {
            var ct = dc.ReportStatus_View("AAC Block Testing", null, null, 0, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 2, 0);
            foreach (var cu in ct)
            {
                for (int i = 0; i < grdAAC.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                    txt_IdMark.Text = cu.AACINWD_Id_Mark_var.ToString();
                }
            }
        }

        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            if (ddl_OtherPendingRpt.SelectedItem.Text != "---Select---")
            {

                FetchRefNo();

            }
        }

        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowAAC_SN_Remark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdAACRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdAACRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowAACRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowAACRemark(int rowIndex)
        {
            GetCurrentDataAAC_SN_Remark();
            DataTable dt = ViewState["AACRemark_SN_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AACRemark_SN_Table"] = dt;
            grdAACRemark.DataSource = dt;
            grdAACRemark.DataBind();
            SetPreviousDataAAC_SN_Remark();
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.AAC_SN_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            //rpt.AAC_PDFReport(txt_ReferenceNo.Text, Days, txt.Text, lblcubetype.Text, lblEntry.Text, lblCubecompstr.Text, lblComprTest.Text);
            // RptCubeTesting();
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

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            #region CS
            mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.gauge_length, chead.count_of_gauge, sn.* 
                     from new_gt_app_db.dbo.reference_number ref, new_gt_app_db.dbo.category_header chead, 
                     new_gt_app_db.dbo.aac_shrinkage sn
                     where ref.pk_id = chead.reference_no and chead.pk_id = sn.category_header_fk_id
                     and ref.reference_number = '" + txt_ReferenceNo.Text + "' order by sn.quantity_no";

            dt = objcls.getGeneralData(mySql);
            if (dt.Rows.Count > 0)
            {
                txt_DateOfTesting.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                txtGaugeLength.Text = dt.Rows[0]["gauge_length"].ToString();
                txtLeastCount.Text = dt.Rows[0]["count_of_gauge"].ToString();
                if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                {
                    txt_witnessBy.Text = dt.Rows[0]["witness_by"].ToString();
                    chk_WitnessBy.Checked = true;                    
                }
                txt_Qty.Text = dt.Rows.Count.ToString();
                SNRowsChanged();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_IniWetMesurement = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_IniWetMesurement");
                    TextBox txt_FinalDryMesurement = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_FinalDryMesurement");
                    //TextBox txt_WetMeasurement = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_WetMeasurement");
                    //TextBox txt_DryMeasurement = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_DryMeasurement");
                    //TextBox txt_DryShrinkage = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_DryShrinkage");
                    //TextBox txt_AvgStr = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_AvgStr");

                    txt_IdMark.Text = dt.Rows[i]["id_mark"].ToString();
                    txt_IniWetMesurement.Text = dt.Rows[i]["initial_wet_measurement"].ToString();
                    txt_FinalDryMesurement.Text = dt.Rows[i]["final_dry_measurement"].ToString();
                    //txt_WetMeasurement.Text = dt.Rows[i]["txt_WetMeasurement"].ToString();
                    //txt_DryMeasurement.Text = dt.Rows[i]["txt_DryMeasurement"].ToString();
                    //txt_DryShrinkage.Text = dt.Rows[i]["txt_DryShrinkage"].ToString();
                    //txt_AvgStr.Text = dt.Rows[i]["txt_AvgStr"].ToString();
                }
            }
            dt.Dispose();
            #endregion

            objcls = null;
        }

    }
}