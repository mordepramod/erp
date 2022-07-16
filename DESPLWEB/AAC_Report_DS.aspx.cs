using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class AAC_Report_DS : System.Web.UI.Page
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
                lblheading.Text = "AAC Density Test Inward - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_RecType.Text != "")
                {
                    DisplayAAC_DS_Details();
                    DisplayGridRow();
                    DisplayRemark();
                    DisplayIdMark();
                    txt_RecType.Text = "AAC";

                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "AAC Density Test Inward - Report Check";
                        LoadApproveBy();
                        DisplayAAC_DSGrid();
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

        public void DisplayAAC_DS_Details()
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
                AddRowAAC_DS_Remark();
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
                AddRowAAC_DS_Remark();
            }

        }
        public void DisplayAAC_DSGrid()
        {
            grdAAC.DataSource = null;
            grdAAC.DataBind();
            int i = 0;
            var AAC_DS = dc.AAC_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32(txt_TestId.Text), "DS");
            foreach (var aac in AAC_DS)
            {
                AddRowEnterReportAAC_DSInward();
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Volume = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_Volume");
                TextBox txt_Density = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_Density");
                TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[i].Cells[8].FindControl("txt_AvgDensity");

                txt_IdMark.Text = aac.AACTEST_IdMark_var.ToString();
                txt_Length.Text = aac.AACTEST_Length_dec.ToString();
                txt_Breadth.Text = aac.AACTEST_Breadth_dec.ToString();
                txt_Height.Text = aac.AACTEST_Height_dec.ToString();
                txt_Weight.Text = aac.AACTEST_Weight_dec.ToString();
                txt_Volume.Text = aac.AACTEST_Volume_dec.ToString();
                txt_Density.Text = aac.AACTEST_Density_dec.ToString();
                i++;
            }
            var avg = dc.ReportStatus_View("AAC Block Testing", null, null, 0, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var c in avg)
            {
                if (grdAAC.Rows.Count > 0)
                {
                    int NoOfrows = grdAAC.Rows.Count / 2;
                    TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[NoOfrows].Cells[8].FindControl("txt_AvgDensity");
                    txt_AvgDensity.Text = c.AACINWD_AvgStr_var.ToString();
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
                        AddRowEnterReportAAC_DSInward();
                    }
                }
            }
        }
        protected void AddRowAAC_DS_Remark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AACRemark_DS_Table"] != null)
            {
                GetCurrentDataAAC_DS_Remark();
                dt = (DataTable)ViewState["AACRemark_DS_Table"];
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

            ViewState["AACRemark_DS_Table"] = dt;
            grdAACRemark.DataSource = dt;
            grdAACRemark.DataBind();
            SetPreviousDataAAC_DS_Remark();
        }
        protected void GetCurrentDataAAC_DS_Remark()
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
            ViewState["AACRemark_DS_Table"] = dtTable;

        }
        protected void SetPreviousDataAAC_DS_Remark()
        {
            DataTable dt = (DataTable)ViewState["AACRemark_DS_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdAACRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdAACRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
      
        protected void AddRowEnterReportAAC_DSInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AAC_DS_DetailTable"] != null)
            {
                GetCurrentDataAAC_DSInward();
                dt = (DataTable)ViewState["AAC_DS_DetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Length", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Breadth", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Height", typeof(string)));
                 dt.Columns.Add(new DataColumn("txt_Weight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Volume", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Density", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AvgDensity", typeof(string)));

            }
            dr = dt.NewRow();
            dr["txt_IdMark"] = string.Empty;
            dr["txt_Length"] = string.Empty;
            dr["txt_Breadth"] = string.Empty;
            dr["txt_Height"] = string.Empty;
            dr["txt_Weight"] = string.Empty;
            dr["txt_Volume"] = string.Empty;
            dr["txt_Density"] = string.Empty;
            dr["txt_AvgDensity"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["AAC_DS_DetailTable"] = dt;
            grdAAC.DataSource = dt;
            grdAAC.DataBind();
            SetPreviousDataAAC_DSInward();
        }
        protected void GetCurrentDataAAC_DSInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Length", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Breadth", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Height", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Weight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Volume", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Density", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgDensity", typeof(string)));

            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Volume = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_Volume");
                TextBox txt_Density = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_Density");
                TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[i].Cells[8].FindControl("txt_AvgDensity");
                drRow = dtTable.NewRow();

                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txt_Length"] = txt_Length.Text;
                drRow["txt_Breadth"] = txt_Breadth.Text;
                drRow["txt_Height"] = txt_Height.Text;
                drRow["txt_Weight"] = txt_Weight.Text;
                drRow["txt_Volume"] = txt_Volume.Text;
                drRow["txt_Density"] = txt_Density.Text;
                drRow["txt_AvgDensity"] = txt_AvgDensity.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["AAC_DS_DetailTable"] = dtTable;

        }
        protected void SetPreviousDataAAC_DSInward()
        {
            DataTable dt = (DataTable)ViewState["AAC_DS_DetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Volume = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_Volume");
                TextBox txt_Density = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_Density");
                TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[i].Cells[8].FindControl("txt_AvgDensity");

                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txt_Length.Text = dt.Rows[i]["txt_Length"].ToString();
                txt_Breadth.Text = dt.Rows[i]["txt_Breadth"].ToString();
                txt_Height.Text = dt.Rows[i]["txt_Height"].ToString();
                txt_Weight.Text = dt.Rows[i]["txt_Weight"].ToString();
                txt_Volume.Text = dt.Rows[i]["txt_Volume"].ToString();
                txt_Density.Text = dt.Rows[i]["txt_Density"].ToString();
                txt_AvgDensity.Text = dt.Rows[i]["txt_AvgDensity"].ToString();

            }

        }
        protected void DeleteDataPT_DSInward(int rowIndex)
        {
            GetCurrentDataAAC_DSInward();
            DataTable dt = ViewState["AAC_DS_DetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AAC_DS_DetailTable"] = dt;
            grdAAC.DataSource = dt;
            grdAAC.DataBind();
            SetPreviousDataAAC_DSInward();
        }
        protected void txt_QtyOnTextChanged(object sender, EventArgs e)
        {
            DSRowsChanged();
        }
        private void DSRowsChanged()
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
                                DeleteDataPT_DSInward(i - 1);
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
                DateTime DateofTesting = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
                DateTime CollectionDt = DateTime.ParseExact(txt_CollectionDt.Text, "dd/MM/yyyy", null);
                txt_Qty.Text = grdAAC.Rows.Count.ToString();
                    if (chk_WitnessBy.Checked == false)
                    {
                        txt_witnessBy.Text = "";
                    }
                   
                    if (lbl_TestedBy.Text == "Tested By")
                    {
                        dc.AACTestInward_Update("AAC", txt_ReferenceNo.Text, 2, DateofTesting, CollectionDt, txt_Description.Text, txt_Supplier.Text, txt_witnessBy.Text, "", Convert.ToInt32(txt_Qty.Text),0,0);
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

                dc.AacDS_Test_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, 0, 0, 0, "", "", null, 0, "", true, 0);//delete  Test entry
                  
                     for (int i = 0; i < grdAAC.Rows.Count; i++)
                    {
                        TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                        TextBox txt_Length = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_Length");
                        TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_Breadth");
                        TextBox txt_Height = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_Height");
                        TextBox txt_Weight = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_Weight");
                        TextBox txt_Volume = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_Volume");
                        TextBox txt_Density = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_Density");
                        TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[i].Cells[8].FindControl("txt_AvgDensity");

                        if (txt_Density.Text == "---")
                        {
                            txt_Density.Text = "---";
                        }
                        if (txt_AvgDensity.Text != "")
                        {
                            dc.AACTestInward_Update("AAC", txt_ReferenceNo.Text, 0, DateofTesting, CollectionDt, txt_Description.Text, txt_Supplier.Text, txt_witnessBy.Text, Convert.ToString(txt_AvgDensity.Text), 0,0,0);
                     }
                        dc.AacDS_Test_Update(txt_ReferenceNo.Text, txt_IdMark.Text, Convert.ToDecimal(txt_Length.Text), Convert.ToDecimal(txt_Breadth.Text), Convert.ToDecimal(txt_Height.Text),
                                               Convert.ToDecimal(txt_Weight.Text),
                                                 Convert.ToDecimal(txt_Volume.Text),  Convert.ToDecimal(txt_Density.Text), 0, "", "", null,  0, "", false,SrNo);
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
            else if (valid == true)
            {
                for (int i = 0; i < grdAAC.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Length = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_Length");
                    TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_Breadth");
                    TextBox txt_Height = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_Height");
                    TextBox txt_Weight = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_Weight");
                   
                    if (txt_IdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No. " + (i + 1) + ".";
                        txt_IdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Length.Text == "")
                    {
                        lblMsg.Text = "Enter Length for Sr No. " + (i + 1) + ".";
                        txt_Length.Focus();
                        valid = false;
                        break;
                    }
                    //else if (Convert.ToDecimal(txt_Length.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Length.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                    //{
                    //    lblMsg.Text = "Invalid Length for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                    //    txt_Length.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    else if (txt_Breadth.Text == "")
                    {
                        lblMsg.Text = "Enter Breadth for Sr No. " + (i + 1) + ".";
                        txt_Breadth.Focus();
                        valid = false;
                        break;
                    }
                    //else if (Convert.ToDecimal(txt_Breadth.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Breadth.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                    //{
                    //    lblMsg.Text = "Invalid Breadth for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                    //    txt_Breadth.Focus();
                    //    valid = false;
                    //    break;

                    //}
                    else if (txt_Height.Text == "")
                    {
                        lblMsg.Text = "Enter Height for Sr No. " + (i + 1) + ".";
                        txt_Height.Focus();
                        valid = false;
                        break;
                    }
                    //else if (Convert.ToDecimal(txt_Height.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Height.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                    //{
                    //    lblMsg.Text = "Invalid Height for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                    //    txt_Height.Focus();
                    //    valid = false;
                    //    break;

                    //}
                    else if (txt_Weight.Text == "")
                    {
                        lblMsg.Text = "Enter Weight for Sr No. " + (i + 1) + ".";
                        txt_Weight.Focus();
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
           decimal vol = 0;
            decimal SumOfCompStr = 0;
            int NoOfrows = grdAAC.Rows.Count / 2;
            for (int i = 0; i < grdAAC.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Volume = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_Volume");
                TextBox txt_Density = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_Density");
                TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[i].Cells[8].FindControl("txt_AvgDensity");

                txt_AvgDensity.Text = string.Empty;
                if (Convert.ToDecimal(txt_Length.Text) > 0 && Convert.ToDecimal(txt_Breadth.Text) > 0 && Convert.ToDecimal(txt_Height.Text) > 0)
                {
                    vol = (Convert.ToDecimal(txt_Length.Text) * Convert.ToDecimal(txt_Breadth.Text) * Convert.ToDecimal(txt_Height.Text) / 1000 / 1000 / 1000);
                }
                txt_Volume.Text = vol.ToString("0.000");

                if ( vol > 0 && Convert.ToDecimal(txt_Weight.Text) > 0)
                {
                    txt_Density.Text = (Convert.ToDecimal(txt_Weight.Text) / vol).ToString("0.0000");
                }
                else
                    txt_Density.Text = "0.0000";
            }

                int Count = 0;
                for (int i = 0; i < grdAAC.Rows.Count; i++)
                {
                    TextBox txt_Density = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_Density");
                    if (txt_Density.Text != "" && txt_Density.Text != "---")
                    {
                        SumOfCompStr += Convert.ToDecimal(txt_Density.Text);
                    }
                    else
                    {
                        SumOfCompStr += Convert.ToDecimal(0);
                    }
                    if (txt_Density.Text != "" && Convert.ToDecimal(txt_Density.Text) > 0)
                    {
                        Count++;
                    }
                }

                for (int i = 0; i < grdAAC.Rows.Count; i++)
                {
                    int NoOfrow = grdAAC.Rows.Count / 2;
                    TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[i].Cells[8].FindControl("txt_AvgDensity");

                    if (i == grdAAC.Rows.Count - 1)
                    {
                        if (Count > 0 && SumOfCompStr > 0)
                            txt_AvgDensity.Text = Math.Round(Convert.ToDecimal(SumOfCompStr / Count)).ToString();
                        else
                            txt_AvgDensity.Text = "*";
                        
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
                 DisplayAAC_DS_Details();
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
                DisplayAAC_DSGrid();
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
            AddRowAAC_DS_Remark();
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
            GetCurrentDataAAC_DS_Remark();
            DataTable dt = ViewState["AACRemark_DS_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AACRemark_DS_Table"] = dt;
            grdAACRemark.DataSource = dt;
            grdAACRemark.DataBind();
            SetPreviousDataAAC_DS_Remark();
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.AAC_DS_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
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
            mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, ds.* 
                     from new_gt_app_db.dbo.reference_number ref, new_gt_app_db.dbo.category_header chead, 
                     new_gt_app_db.dbo.aac_density ds
                     where ref.pk_id = chead.reference_no and chead.pk_id = ds.category_header_fk_id
                     and ref.reference_number = '" + txt_ReferenceNo.Text + "' order by ds.quantity_no";

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
                DSRowsChanged();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdAAC.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Length = (TextBox)grdAAC.Rows[i].Cells[2].FindControl("txt_Length");
                    TextBox txt_Breadth = (TextBox)grdAAC.Rows[i].Cells[3].FindControl("txt_Breadth");
                    TextBox txt_Height = (TextBox)grdAAC.Rows[i].Cells[4].FindControl("txt_Height");
                    TextBox txt_Weight = (TextBox)grdAAC.Rows[i].Cells[5].FindControl("txt_Weight");
                    //TextBox txt_Volume = (TextBox)grdAAC.Rows[i].Cells[6].FindControl("txt_Volume");
                    //TextBox txt_Density = (TextBox)grdAAC.Rows[i].Cells[7].FindControl("txt_Density");
                    //TextBox txt_AvgDensity = (TextBox)grdAAC.Rows[i].Cells[8].FindControl("txt_AvgDensity");

                    txt_IdMark.Text = dt.Rows[i]["id_mark"].ToString();
                    txt_Length.Text = dt.Rows[i]["length"].ToString();
                    txt_Breadth.Text = dt.Rows[i]["breadth"].ToString();
                    txt_Height.Text = dt.Rows[i]["height"].ToString();
                    txt_Weight.Text = dt.Rows[i]["weight"].ToString();
                    //txt_Volume.Text = dt.Rows[i]["txt_Volume"].ToString();
                    //txt_Density.Text = dt.Rows[i]["txt_Density"].ToString();
                    //txt_AvgDensity.Text = dt.Rows[i]["txt_AvgDensity"].ToString();
                }
            }
            dt.Dispose();
            #endregion

            objcls = null;
        }
    }
}