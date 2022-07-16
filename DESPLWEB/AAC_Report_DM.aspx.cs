using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class AAC_Report_DM : System.Web.UI.Page
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
                lblheading.Text = "AAC Dimension Test Inward - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_RecType.Text != "")
                {
                    DisplayAAC_DM_Details();
                    DisplayGridRow();
                    DisplayRemark();
                    DisplayIdMark();
                    txt_RecType.Text = "AAC";

                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "AAC Dimension Test Inward - Report Check";
                        LoadApproveBy();
                        DisplayAAC_DMGrid();
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

        public void DisplayAAC_DM_Details()
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
                AddRowAAC_DM_Remark();
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
                AddRowAAC_DM_Remark();
            }

        }
        public void DisplayAAC_DMGrid()
        {
            grdAAC.DataSource = null;
            grdAAC.DataBind();
            int i = 0;
            var AAC_DM = dc.AAC_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32(txt_TestId.Text), "DM");
            foreach (var aac in AAC_DM)
            {
                AddRowEnterReportAAC_DMInward();
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].FindControl("txt_IdMark");
                TextBox txtL1 = (TextBox)grdAAC.Rows[i].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdAAC.Rows[i].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdAAC.Rows[i].FindControl("txtL3");
                TextBox txtL4 = (TextBox)grdAAC.Rows[i].FindControl("txtL4");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].FindControl("txt_Length");
                TextBox txtB1 = (TextBox)grdAAC.Rows[i].FindControl("txtB1");
                TextBox txtB2 = (TextBox)grdAAC.Rows[i].FindControl("txtB2");
                TextBox txtB3 = (TextBox)grdAAC.Rows[i].FindControl("txtB3");
                TextBox txtB4 = (TextBox)grdAAC.Rows[i].FindControl("txtB4");
                TextBox txtB5 = (TextBox)grdAAC.Rows[i].FindControl("txtB5");
                TextBox txtB6 = (TextBox)grdAAC.Rows[i].FindControl("txtB6");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].FindControl("txt_Breadth");
                TextBox txtH1 = (TextBox)grdAAC.Rows[i].FindControl("txtH1");
                TextBox txtH2 = (TextBox)grdAAC.Rows[i].FindControl("txtH2");
                TextBox txtH3 = (TextBox)grdAAC.Rows[i].FindControl("txtH3");
                TextBox txtH4 = (TextBox)grdAAC.Rows[i].FindControl("txtH4");
                TextBox txtH5 = (TextBox)grdAAC.Rows[i].FindControl("txtH5");
                TextBox txtH6 = (TextBox)grdAAC.Rows[i].FindControl("txtH6");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].FindControl("txt_Height");

                txt_IdMark.Text = aac.AACTEST_IdMark_var.ToString();
                txt_Length.Text = aac.AACTEST_Length_dec.ToString();
                txt_Breadth.Text = aac.AACTEST_Breadth_dec.ToString();
                txt_Height.Text = aac.AACTEST_Height_dec.ToString();

                if (aac.AACTEST_LengthValues_var != null)
                {
                    string[] strData = aac.AACTEST_LengthValues_var.Split('|');
                    txtL1.Text = strData[0];
                    txtL2.Text = strData[1];
                    txtL3.Text = strData[2];
                    txtL4.Text = strData[3];
                    strData = aac.AACTEST_BreadthValues_var.Split('|');
                    txtB1.Text = strData[0];
                    txtB2.Text = strData[1];
                    txtB3.Text = strData[2];
                    txtB4.Text = strData[3];
                    txtB5.Text = strData[4];
                    txtB6.Text = strData[5];
                    strData = aac.AACTEST_HeightValues_var.Split('|');
                    txtH1.Text = strData[0];
                    txtH2.Text = strData[1];
                    txtH3.Text = strData[2];
                    txtH4.Text = strData[3];
                    txtH5.Text = strData[4];
                    txtH6.Text = strData[5];
                }
                i++;
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
                        AddRowEnterReportAAC_DMInward();
                    }
                }
            }
        }
        protected void AddRowAAC_DM_Remark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AACRemark_DM_Table"] != null)
            {
                GetCurrentDataAAC_DM_Remark();
                dt = (DataTable)ViewState["AACRemark_DM_Table"];
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

            ViewState["AACRemark_DM_Table"] = dt;
            grdAACRemark.DataSource = dt;
            grdAACRemark.DataBind();
            SetPreviousDataAAC_DM_Remark();
        }
        protected void GetCurrentDataAAC_DM_Remark()
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
            ViewState["AACRemark_DM_Table"] = dtTable;

        }
        protected void SetPreviousDataAAC_DM_Remark()
        {
            DataTable dt = (DataTable)ViewState["AACRemark_DM_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdAACRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdAACRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }

        protected void AddRowEnterReportAAC_DMInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AAC_DM_DetailTable"] != null)
            {
                GetCurrentDataAAC_DMInward();
                dt = (DataTable)ViewState["AAC_DM_DetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtL1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtL2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtL3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtL4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Length", typeof(string)));
                dt.Columns.Add(new DataColumn("txtB1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtB2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtB3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtB4", typeof(string)));
                dt.Columns.Add(new DataColumn("txtB5", typeof(string)));
                dt.Columns.Add(new DataColumn("txtB6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Breadth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH4", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH5", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Height", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_IdMark"] = string.Empty;
            dr["txtL1"] = string.Empty;
            dr["txtL2"] = string.Empty;
            dr["txtL3"] = string.Empty;
            dr["txtL4"] = string.Empty;
            dr["txt_Length"] = string.Empty;
            dr["txtB1"] = string.Empty;
            dr["txtB2"] = string.Empty;
            dr["txtB3"] = string.Empty;
            dr["txtB4"] = string.Empty;
            dr["txtB5"] = string.Empty;
            dr["txtB6"] = string.Empty;
            dr["txt_Breadth"] = string.Empty;
            dr["txtH1"] = string.Empty;
            dr["txtH2"] = string.Empty;
            dr["txtH3"] = string.Empty;
            dr["txtH4"] = string.Empty;
            dr["txtH5"] = string.Empty;
            dr["txtH6"] = string.Empty;
            dr["txt_Height"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["AAC_DM_DetailTable"] = dt;
            grdAAC.DataSource = dt;
            grdAAC.DataBind();
            SetPreviousDataAAC_DMInward();
        }
        protected void GetCurrentDataAAC_DMInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtL1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtL2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtL3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtL4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Length", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtB1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtB2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtB3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtB4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtB5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtB6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Breadth", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Height", typeof(string)));

            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].FindControl("txt_IdMark");
                TextBox txtL1 = (TextBox)grdAAC.Rows[i].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdAAC.Rows[i].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdAAC.Rows[i].FindControl("txtL3");
                TextBox txtL4 = (TextBox)grdAAC.Rows[i].FindControl("txtL4");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].FindControl("txt_Length");
                TextBox txtB1 = (TextBox)grdAAC.Rows[i].FindControl("txtB1");
                TextBox txtB2 = (TextBox)grdAAC.Rows[i].FindControl("txtB2");
                TextBox txtB3 = (TextBox)grdAAC.Rows[i].FindControl("txtB3");
                TextBox txtB4 = (TextBox)grdAAC.Rows[i].FindControl("txtB4");
                TextBox txtB5 = (TextBox)grdAAC.Rows[i].FindControl("txtB5");
                TextBox txtB6 = (TextBox)grdAAC.Rows[i].FindControl("txtB6");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].FindControl("txt_Breadth");
                TextBox txtH1 = (TextBox)grdAAC.Rows[i].FindControl("txtH1");
                TextBox txtH2 = (TextBox)grdAAC.Rows[i].FindControl("txtH2");
                TextBox txtH3 = (TextBox)grdAAC.Rows[i].FindControl("txtH3");
                TextBox txtH4 = (TextBox)grdAAC.Rows[i].FindControl("txtH4");
                TextBox txtH5 = (TextBox)grdAAC.Rows[i].FindControl("txtH5");
                TextBox txtH6 = (TextBox)grdAAC.Rows[i].FindControl("txtH6");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].FindControl("txt_Height");
                drRow = dtTable.NewRow();

                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txtL1"] = txtL1.Text;
                drRow["txtL2"] = txtL2.Text;
                drRow["txtL3"] = txtL3.Text;
                drRow["txtL4"] = txtL4.Text;
                drRow["txt_Length"] = txt_Length.Text;
                drRow["txtB1"] = txtB1.Text;
                drRow["txtB2"] = txtB2.Text;
                drRow["txtB3"] = txtB3.Text;
                drRow["txtB4"] = txtB4.Text;
                drRow["txtB5"] = txtB5.Text;
                drRow["txtB6"] = txtB6.Text;
                drRow["txt_Breadth"] = txt_Breadth.Text;
                drRow["txtH1"] = txtH1.Text;
                drRow["txtH2"] = txtH2.Text;
                drRow["txtH3"] = txtH3.Text;
                drRow["txtH4"] = txtH4.Text;
                drRow["txtH5"] = txtH5.Text;
                drRow["txtH6"] = txtH6.Text;
                drRow["txt_Height"] = txt_Height.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["AAC_DM_DetailTable"] = dtTable;

        }
        protected void SetPreviousDataAAC_DMInward()
        {
            DataTable dt = (DataTable)ViewState["AAC_DM_DetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].FindControl("txt_IdMark");
                TextBox txtL1 = (TextBox)grdAAC.Rows[i].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdAAC.Rows[i].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdAAC.Rows[i].FindControl("txtL3");
                TextBox txtL4 = (TextBox)grdAAC.Rows[i].FindControl("txtL4");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].FindControl("txt_Length");
                TextBox txtB1 = (TextBox)grdAAC.Rows[i].FindControl("txtB1");
                TextBox txtB2 = (TextBox)grdAAC.Rows[i].FindControl("txtB2");
                TextBox txtB3 = (TextBox)grdAAC.Rows[i].FindControl("txtB3");
                TextBox txtB4 = (TextBox)grdAAC.Rows[i].FindControl("txtB4");
                TextBox txtB5 = (TextBox)grdAAC.Rows[i].FindControl("txtB5");
                TextBox txtB6 = (TextBox)grdAAC.Rows[i].FindControl("txtB6");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].FindControl("txt_Breadth");
                TextBox txtH1 = (TextBox)grdAAC.Rows[i].FindControl("txtH1");
                TextBox txtH2 = (TextBox)grdAAC.Rows[i].FindControl("txtH2");
                TextBox txtH3 = (TextBox)grdAAC.Rows[i].FindControl("txtH3");
                TextBox txtH4 = (TextBox)grdAAC.Rows[i].FindControl("txtH4");
                TextBox txtH5 = (TextBox)grdAAC.Rows[i].FindControl("txtH5");
                TextBox txtH6 = (TextBox)grdAAC.Rows[i].FindControl("txtH6");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].FindControl("txt_Height");

                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txtL1.Text = dt.Rows[i]["txtL1"].ToString();
                txtL2.Text = dt.Rows[i]["txtL2"].ToString();
                txtL3.Text = dt.Rows[i]["txtL3"].ToString();
                txtL4.Text = dt.Rows[i]["txtL4"].ToString();
                txt_Length.Text = dt.Rows[i]["txt_Length"].ToString();
                txtB1.Text = dt.Rows[i]["txtB1"].ToString();
                txtB2.Text = dt.Rows[i]["txtB2"].ToString();
                txtB3.Text = dt.Rows[i]["txtB3"].ToString();
                txtB4.Text = dt.Rows[i]["txtB4"].ToString();
                txtB5.Text = dt.Rows[i]["txtB5"].ToString();
                txtB6.Text = dt.Rows[i]["txtB6"].ToString();
                txt_Breadth.Text = dt.Rows[i]["txt_Breadth"].ToString();
                txtH1.Text = dt.Rows[i]["txtH1"].ToString();
                txtH2.Text = dt.Rows[i]["txtH2"].ToString();
                txtH3.Text = dt.Rows[i]["txtH3"].ToString();
                txtH4.Text = dt.Rows[i]["txtH4"].ToString();
                txtH5.Text = dt.Rows[i]["txtH5"].ToString();
                txtH6.Text = dt.Rows[i]["txtH6"].ToString();
                txt_Height.Text = dt.Rows[i]["txt_Height"].ToString();

            }

        }
        protected void DeleteDataPT_DMInward(int rowIndex)
        {
            GetCurrentDataAAC_DMInward();
            DataTable dt = ViewState["AAC_DM_DetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AAC_DM_DetailTable"] = dt;
            grdAAC.DataSource = dt;
            grdAAC.DataBind();
            SetPreviousDataAAC_DMInward();
        }
        protected void txt_QtyOnTextChanged(object sender, EventArgs e)
        {
            DMRowsChanged();
        }
        private void DMRowsChanged()
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
                                DeleteDataPT_DMInward(i - 1);
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

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculate();
                int SrNo = 1;
                DateTime CollectionDt = DateTime.ParseExact(txt_CollectionDt.Text, "dd/MM/yyyy", null);
                dc.AacDM_Test_Update(txt_ReferenceNo.Text, "","",0,"",0,"",0,0, "", "", null, 0, true, 0);//delete  Test entry
                DateTime DateofTesting = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
                txt_Qty.Text = grdAAC.Rows.Count.ToString();
                if (chk_WitnessBy.Checked == false)
                {
                    txt_witnessBy.Text = "";
                }

                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AACTestInward_Update("AAC", txt_ReferenceNo.Text, 2, DateofTesting, CollectionDt, txt_Description.Text, txt_Supplier.Text, txt_witnessBy.Text, "", Convert.ToInt32(txt_Qty.Text), 0, 0);
                    dc.ReportDetails_Update("AAC", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "AAC", txt_ReferenceNo.Text, "AAC", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AACTestInward_Update("AAC", txt_ReferenceNo.Text, 3, DateofTesting, CollectionDt, txt_Description.Text, txt_Supplier.Text, txt_witnessBy.Text, "", Convert.ToInt32(txt_Qty.Text), 0, 0);
                    dc.ReportDetails_Update("AAC", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "AAC", txt_ReferenceNo.Text, "AAC", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "AAC", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                for (int i = 0; i < grdAAC.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].FindControl("txt_IdMark");
                    TextBox txtL1 = (TextBox)grdAAC.Rows[i].FindControl("txtL1");
                    TextBox txtL2 = (TextBox)grdAAC.Rows[i].FindControl("txtL2");
                    TextBox txtL3 = (TextBox)grdAAC.Rows[i].FindControl("txtL3");
                    TextBox txtL4 = (TextBox)grdAAC.Rows[i].FindControl("txtL4");
                    TextBox txt_Length = (TextBox)grdAAC.Rows[i].FindControl("txt_Length");
                    TextBox txtB1 = (TextBox)grdAAC.Rows[i].FindControl("txtB1");
                    TextBox txtB2 = (TextBox)grdAAC.Rows[i].FindControl("txtB2");
                    TextBox txtB3 = (TextBox)grdAAC.Rows[i].FindControl("txtB3");
                    TextBox txtB4 = (TextBox)grdAAC.Rows[i].FindControl("txtB4");
                    TextBox txtB5 = (TextBox)grdAAC.Rows[i].FindControl("txtB5");
                    TextBox txtB6 = (TextBox)grdAAC.Rows[i].FindControl("txtB6");
                    TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].FindControl("txt_Breadth");
                    TextBox txtH1 = (TextBox)grdAAC.Rows[i].FindControl("txtH1");
                    TextBox txtH2 = (TextBox)grdAAC.Rows[i].FindControl("txtH2");
                    TextBox txtH3 = (TextBox)grdAAC.Rows[i].FindControl("txtH3");
                    TextBox txtH4 = (TextBox)grdAAC.Rows[i].FindControl("txtH4");
                    TextBox txtH5 = (TextBox)grdAAC.Rows[i].FindControl("txtH5");
                    TextBox txtH6 = (TextBox)grdAAC.Rows[i].FindControl("txtH6");
                    TextBox txt_Height = (TextBox)grdAAC.Rows[i].FindControl("txt_Height");
                    
                    dc.AacDM_Test_Update(txt_ReferenceNo.Text, txt_IdMark.Text, (txtL1.Text + "|" + txtL2.Text + "|" + txtL3.Text + "|" + txtL4.Text), Convert.ToDecimal(txt_Length.Text), (txtB1.Text + "|" + txtB2.Text + "|" + txtB3.Text + "|" + txtB4.Text + "|" + txtB5.Text + "|" + txtB6.Text), Convert.ToDecimal(txt_Breadth.Text),
                        (txtH1.Text + "|" + txtH2.Text + "|" + txtH3.Text + "|" + txtH4.Text + "|" + txtH5.Text + "|" + txtH6.Text), Convert.ToDecimal(txt_Height.Text), 0, "", "", null, 0, false, SrNo);
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
                valid = false;
            }
            else if (Convert.ToInt32(txt_Qty.Text) <= 0)
            {
                lblMsg.Text = "Qty should be greater than 0";
                valid = false;
            }
            else if (ValidateDA() == false)
            {
                valid = false;
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

        protected Boolean ValidateDA()
        {

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;

            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].FindControl("txt_IdMark");
                TextBox txtL1 = (TextBox)grdAAC.Rows[i].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdAAC.Rows[i].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdAAC.Rows[i].FindControl("txtL3");
                TextBox txtL4 = (TextBox)grdAAC.Rows[i].FindControl("txtL4");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].FindControl("txt_Length");
                TextBox txtB1 = (TextBox)grdAAC.Rows[i].FindControl("txtB1");
                TextBox txtB2 = (TextBox)grdAAC.Rows[i].FindControl("txtB2");
                TextBox txtB3 = (TextBox)grdAAC.Rows[i].FindControl("txtB3");
                TextBox txtB4 = (TextBox)grdAAC.Rows[i].FindControl("txtB4");
                TextBox txtB5 = (TextBox)grdAAC.Rows[i].FindControl("txtB5");
                TextBox txtB6 = (TextBox)grdAAC.Rows[i].FindControl("txtB6");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].FindControl("txt_Breadth");
                TextBox txtH1 = (TextBox)grdAAC.Rows[i].FindControl("txtH1");
                TextBox txtH2 = (TextBox)grdAAC.Rows[i].FindControl("txtH2");
                TextBox txtH3 = (TextBox)grdAAC.Rows[i].FindControl("txtH3");
                TextBox txtH4 = (TextBox)grdAAC.Rows[i].FindControl("txtH4");
                TextBox txtH5 = (TextBox)grdAAC.Rows[i].FindControl("txtH5");
                TextBox txtH6 = (TextBox)grdAAC.Rows[i].FindControl("txtH6");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].FindControl("txt_Height");

                if (txt_IdMark.Text == "")
                {
                    lblMsg.Text = "Enter Id Mark for Sr No. " + (i + 1) + ".";
                    txt_IdMark.Focus();
                    valid = false;
                    break;
                }
                else if (txtL1.Text == "")
                {
                    lblMsg.Text = "Enter L1 for row no " + (i + 1) + ".";
                    valid = false;
                    txtL1.Focus();
                    break;
                }
                else if (txtL2.Text == "")
                {
                    lblMsg.Text = "Enter L2 for row no " + (i + 1) + ".";
                    valid = false;
                    txtL2.Focus();
                    break;
                }
                else if (txtL3.Text == "")
                {
                    lblMsg.Text = "Enter L3 for row no " + (i + 1) + ".";
                    valid = false;
                    txtL3.Focus();
                    break;
                }
                else if (txtL4.Text == "")
                {
                    lblMsg.Text = "Enter L4 for row no " + (i + 1) + ".";
                    valid = false;
                    txtL4.Focus();
                    break;
                }
                //else if (txt_Length.Text == "")
                //{
                //    lblMsg.Text = "Enter Length for Sr No. " + (i + 1) + ".";
                //    txt_Length.Focus();
                //    valid = false;
                //    break;
                //}
                //else if (Convert.ToDecimal(txt_Length.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Length.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                //{
                //    lblMsg.Text = "Invalid Length for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                //    txt_Length.Focus();
                //    valid = false;
                //    break;
                //}
                else if (txtB1.Text == "")
                {
                    lblMsg.Text = "Enter B1 for row no " + (i + 1) + ".";
                    valid = false;
                    txtB1.Focus();
                    break;
                }
                else if (txtB2.Text == "")
                {
                    lblMsg.Text = "Enter B2 for row no " + (i + 1) + ".";
                    valid = false;
                    txtB2.Focus();
                    break;
                }
                else if (txtB3.Text == "")
                {
                    lblMsg.Text = "Enter B3 for row no " + (i + 1) + ".";
                    valid = false;
                    txtB3.Focus();
                    break;
                }
                else if (txtB4.Text == "")
                {
                    lblMsg.Text = "Enter B4 for row no " + (i + 1) + ".";
                    valid = false;
                    txtB4.Focus();
                    break;
                }
                else if (txtB5.Text == "")
                {
                    lblMsg.Text = "Enter B5 for row no " + (i + 1) + ".";
                    valid = false;
                    txtB5.Focus();
                    break;
                }
                else if (txtB6.Text == "")
                {
                    lblMsg.Text = "Enter B6 for row no " + (i + 1) + ".";
                    valid = false;
                    txtB6.Focus();
                    break;
                }
                //else if (txt_Breadth.Text == "")
                //{
                //    lblMsg.Text = "Enter Breadth for Sr No. " + (i + 1) + ".";
                //    txt_Breadth.Focus();
                //    valid = false;
                //    break;
                //}
                //else if (Convert.ToDecimal(txt_Breadth.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Breadth.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                //{
                //    lblMsg.Text = "Invalid Breadth for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                //    txt_Breadth.Focus();
                //    valid = false;
                //    break;

                //}
                else if (txtH1.Text == "")
                {
                    lblMsg.Text = "Enter H1 for row no " + (i + 1) + ".";
                    valid = false;
                    txtH1.Focus();
                    break;
                }
                else if (txtH2.Text == "")
                {
                    lblMsg.Text = "Enter H2 for row no " + (i + 1) + ".";
                    valid = false;
                    txtH2.Focus();
                    break;
                }
                else if (txtH3.Text == "")
                {
                    lblMsg.Text = "Enter H3 for row no " + (i + 1) + ".";
                    valid = false;
                    txtH3.Focus();
                    break;
                }
                else if (txtH4.Text == "")
                {
                    lblMsg.Text = "Enter H4 for row no " + (i + 1) + ".";
                    valid = false;
                    txtH4.Focus();
                    break;
                }
                else if (txtH5.Text == "")
                {
                    lblMsg.Text = "Enter H5 for row no " + (i + 1) + ".";
                    valid = false;
                    txtH5.Focus();
                    break;
                }
                else if (txtH6.Text == "")
                {
                    lblMsg.Text = "Enter H6 for row no " + (i + 1) + ".";
                    valid = false;
                    txtH6.Focus();
                    break;
                }
                //else if (txt_Height.Text == "")
                //{
                //    lblMsg.Text = "Enter Height for Sr No. " + (i + 1) + ".";
                //    txt_Height.Focus();
                //    valid = false;
                //    break;
                //}
                //else if (Convert.ToDecimal(txt_Height.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Height.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                //{
                //    lblMsg.Text = "Invalid Height for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                //    txt_Height.Focus();
                //    valid = false;
                //    break;

                //}

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
                DisplayAAC_DM_Details();
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
                DisplayAAC_DMGrid();
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
            AddRowAAC_DM_Remark();
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
            GetCurrentDataAAC_DM_Remark();
            DataTable dt = ViewState["AACRemark_DM_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AACRemark_DM_Table"] = dt;
            grdAACRemark.DataSource = dt;
            grdAACRemark.DataBind();
            SetPreviousDataAAC_DM_Remark();
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.AAC_DM_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
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
        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        private void Calculate()
        {
            if (ValidateDA() == true)
            {
                try
                {
                    decimal len = 0, wdh = 0, het = 0;
                    decimal totlen = 0, totwdh = 0, tothet = 0;

                    for (int i = 0; i < grdAAC.Rows.Count; i++)
                    {

                        TextBox txtL1 = (TextBox)grdAAC.Rows[i].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdAAC.Rows[i].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdAAC.Rows[i].FindControl("txtL3");
                        TextBox txtL4 = (TextBox)grdAAC.Rows[i].FindControl("txtL4");
                        TextBox txt_Length = (TextBox)grdAAC.Rows[i].FindControl("txt_Length");
                        TextBox txtB1 = (TextBox)grdAAC.Rows[i].FindControl("txtB1");
                        TextBox txtB2 = (TextBox)grdAAC.Rows[i].FindControl("txtB2");
                        TextBox txtB3 = (TextBox)grdAAC.Rows[i].FindControl("txtB3");
                        TextBox txtB4 = (TextBox)grdAAC.Rows[i].FindControl("txtB4");
                        TextBox txtB5 = (TextBox)grdAAC.Rows[i].FindControl("txtB5");
                        TextBox txtB6 = (TextBox)grdAAC.Rows[i].FindControl("txtB6");
                        TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].FindControl("txt_Breadth");
                        TextBox txtH1 = (TextBox)grdAAC.Rows[i].FindControl("txtH1");
                        TextBox txtH2 = (TextBox)grdAAC.Rows[i].FindControl("txtH2");
                        TextBox txtH3 = (TextBox)grdAAC.Rows[i].FindControl("txtH3");
                        TextBox txtH4 = (TextBox)grdAAC.Rows[i].FindControl("txtH4");
                        TextBox txtH5 = (TextBox)grdAAC.Rows[i].FindControl("txtH5");
                        TextBox txtH6 = (TextBox)grdAAC.Rows[i].FindControl("txtH6");
                        TextBox txt_Height = (TextBox)grdAAC.Rows[i].FindControl("txt_Height");

                        len = 0;
                        if (txtL1.Text != "")
                            len = len + Convert.ToDecimal(txtL1.Text);
                        if (txtL2.Text != "")
                            len = len + Convert.ToDecimal(txtL2.Text);
                        if (txtL3.Text != "")
                            len = len + Convert.ToDecimal(txtL3.Text);
                        if (txtL4.Text != "")
                            len = len + Convert.ToDecimal(txtL4.Text);

                        txt_Length.Text = "0.00";
                        if (len > 0)
                        {
                            txt_Length.Text = (len / 4).ToString("0.00");
                        }
                        totlen = totlen + Convert.ToDecimal(txt_Length.Text);

                        //Width
                        wdh = 0;
                        if (txtB1.Text != "")
                            wdh = wdh + Convert.ToDecimal(txtB1.Text);
                        if (txtB2.Text != "")
                            wdh = wdh + Convert.ToDecimal(txtB2.Text);
                        if (txtB3.Text != "")
                            wdh = wdh + Convert.ToDecimal(txtB3.Text);
                        if (txtB4.Text != "")
                            wdh = wdh + Convert.ToDecimal(txtB4.Text);
                        if (txtB5.Text != "")
                            wdh = wdh + Convert.ToDecimal(txtB5.Text);
                        if (txtB6.Text != "")
                            wdh = wdh + Convert.ToDecimal(txtB6.Text);
                        
                        txt_Breadth.Text = "0.00";
                        if (wdh > 0)
                        {
                            txt_Breadth.Text = (wdh / 6).ToString("0.00");
                        }
                        totwdh = totwdh + Convert.ToDecimal(txt_Breadth.Text);

                        //Height
                        het = 0;
                        if (txtH1.Text != "")
                            het = het + Convert.ToDecimal(txtH1.Text);
                        if (txtH2.Text != "")
                            het = het + Convert.ToDecimal(txtH2.Text);
                        if (txtH3.Text != "")
                            het = het + Convert.ToDecimal(txtH3.Text);
                        if (txtH4.Text != "")
                            het = het + Convert.ToDecimal(txtH4.Text);
                        if (txtH5.Text != "")
                            het = het + Convert.ToDecimal(txtH5.Text);
                        if (txtH6.Text != "")
                            het = het + Convert.ToDecimal(txtH6.Text);
                                                
                        txt_Height.Text = "0.00";
                        if (het > 0)
                        {
                            txt_Height.Text = (het / 6).ToString("0.00");
                        }
                        tothet = tothet + Convert.ToDecimal(txt_Height.Text);
                    }
                    
                }
                catch
                {
                }
            }

        }

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            #region DM
            mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, dm.* 
                     from new_gt_app_db.dbo.reference_number ref, new_gt_app_db.dbo.category_header chead, 
                     new_gt_app_db.dbo.aac_diamension dm
                     where ref.pk_id = chead.reference_no and chead.pk_id = dm.category_header_fk_id
                     and ref.reference_number = '" + txt_ReferenceNo.Text + "' order by dm.quantity_no";

            dt = objcls.getGeneralData(mySql);
            if (dt.Rows.Count > 0)
            {
                txt_DateOfTesting.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                {
                    txt_witnessBy.Text = dt.Rows[0]["witness_by"].ToString();
                    chk_WitnessBy.Checked = true;
                }
                txt_Qty.Text = dt.Rows.Count.ToString();
                DMRowsChanged();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].FindControl("txt_IdMark");
                    TextBox txtL1 = (TextBox)grdAAC.Rows[i].FindControl("txtL1");
                    TextBox txtL2 = (TextBox)grdAAC.Rows[i].FindControl("txtL2");
                    TextBox txtL3 = (TextBox)grdAAC.Rows[i].FindControl("txtL3");
                    TextBox txtL4 = (TextBox)grdAAC.Rows[i].FindControl("txtL4");
                    TextBox txt_Length = (TextBox)grdAAC.Rows[i].FindControl("txt_Length");
                    TextBox txtB1 = (TextBox)grdAAC.Rows[i].FindControl("txtB1");
                    TextBox txtB2 = (TextBox)grdAAC.Rows[i].FindControl("txtB2");
                    TextBox txtB3 = (TextBox)grdAAC.Rows[i].FindControl("txtB3");
                    TextBox txtB4 = (TextBox)grdAAC.Rows[i].FindControl("txtB4");
                    TextBox txtB5 = (TextBox)grdAAC.Rows[i].FindControl("txtB5");
                    TextBox txtB6 = (TextBox)grdAAC.Rows[i].FindControl("txtB6");
                    TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].FindControl("txt_Breadth");
                    TextBox txtH1 = (TextBox)grdAAC.Rows[i].FindControl("txtH1");
                    TextBox txtH2 = (TextBox)grdAAC.Rows[i].FindControl("txtH2");
                    TextBox txtH3 = (TextBox)grdAAC.Rows[i].FindControl("txtH3");
                    TextBox txtH4 = (TextBox)grdAAC.Rows[i].FindControl("txtH4");
                    TextBox txtH5 = (TextBox)grdAAC.Rows[i].FindControl("txtH5");
                    TextBox txtH6 = (TextBox)grdAAC.Rows[i].FindControl("txtH6");
                    TextBox txt_Height = (TextBox)grdAAC.Rows[i].FindControl("txt_Height");

                    txt_IdMark.Text = dt.Rows[i]["id_mark"].ToString();
                    txtL1.Text = dt.Rows[i]["l1"].ToString();
                    txtL2.Text = dt.Rows[i]["l2"].ToString();
                    txtL3.Text = dt.Rows[i]["l3"].ToString();
                    txtL4.Text = dt.Rows[i]["l4"].ToString();
                    txt_Length.Text = dt.Rows[i]["length"].ToString();
                    txtB1.Text = dt.Rows[i]["b1"].ToString();
                    txtB2.Text = dt.Rows[i]["b2"].ToString();
                    txtB3.Text = dt.Rows[i]["b3"].ToString();
                    txtB4.Text = dt.Rows[i]["b4"].ToString();
                    txtB5.Text = dt.Rows[i]["b5"].ToString();
                    txtB6.Text = dt.Rows[i]["b6"].ToString();
                    txt_Breadth.Text = dt.Rows[i]["breadth"].ToString();
                    txtH1.Text = dt.Rows[i]["h1"].ToString();
                    txtH2.Text = dt.Rows[i]["h2"].ToString();
                    txtH3.Text = dt.Rows[i]["h3"].ToString();
                    txtH4.Text = dt.Rows[i]["h4"].ToString();
                    txtH5.Text = dt.Rows[i]["h5"].ToString();
                    txtH6.Text = dt.Rows[i]["h6"].ToString();
                    txt_Height.Text = dt.Rows[i]["height"].ToString();
                }
            }
            dt.Dispose();
            #endregion

            objcls = null;
        }
    }
}