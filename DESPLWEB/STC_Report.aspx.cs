using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class STC_Report : System.Web.UI.Page
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
                    txt_RecordType.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEntry.Text = arrIndMsg[1].ToString().Trim();
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = " Steel Chemical - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_RecordType.Text != "")
                {
                    getCurrentTestingDate();
                    DisplaySTCDetails();
                    DisplaySTCGrid();
                    DisplayRemark();
                    LoadReferenceNoList();
                    if (lblEntry.Text == "Check")
                    {
                        lbl_TestedBy.Text = "Approve By";
                        lblheading.Text = "Steel Chemical - Report Check";
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
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

            var reportList = dc.ReferenceNo_View_StatusWise("STC", reportStatus, 0);
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
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Steel Chemical Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "STCINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "STCINWD_ReferenceNo_var";
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
            var testinguser = dc.ReportStatus_View("Steel Chemical Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "STCINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "STCINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        public void DeleteRow()
        {
            if (grdSTCRemark.Rows.Count > 1)
            {
                DeleteRowSTCRemark(grdSTCRemark.Rows.Count - 1);
            }
        }
        protected void DeleteRowSTCRemark(int rowIndex)
        {
            GetCurrentDataSTCRemark();
            DataTable dt = ViewState["STCRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["STCRemarkTable"] = dt;
            grdSTCRemark.DataSource = dt;
            grdSTCRemark.DataBind();
            SetPreviousDataSTCRemark();
        }
        public void getCurrentTestingDate()
        {
            txt_DateOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        public void DisplaySTCDetails()
        {
            var InwardSTC = dc.ReportStatus_View("Steel Chemical Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text.ToString(), 0, 0, 0);
            foreach (var STC in InwardSTC)
            {
                txt_ReferenceNo.Text = STC.STCINWD_ReferenceNo_var.ToString();
                if (STC.STCINWD_TestedDate_dt != null)
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(STC.STCINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_Description.Text = STC.STCINWD_Description_var.ToString();
                txt_Supplier.Text = STC.STCINWD_SupplierName_var.ToString();
                txt_typeOfSteel.Text = STC.STCINWD_SteelType_var.ToString();
                txt_GradeOfSteel.Text = STC.STCINWD_Grade_var.ToString();
                txt_ReportNo.Text = STC.STCINWD_SetOfRecord_var.ToString();
                txt_RecordType.Text = STC.STCINWD_RecordType_var.ToString();
                txt_Qty.Text = STC.STCINWD_Quantity_tint.ToString();
                if (ddl_NablScope.Items.FindByValue(STC.STCINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(STC.STCINWD_NablScope_var);
                }
                if (Convert.ToString(STC.STCINWD_NablLocation_int) != null && Convert.ToString(STC.STCINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(STC.STCINWD_NablLocation_int);
                }

            }


        }
        public void DisplayIdMark()
        {
            var InwardSTC = dc.ReportStatus_View("Steel Chemical Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text.ToString(), 0, 0, 0);
            foreach (var stc in InwardSTC)
            {
                for (int i = 0; i < grdSTCEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_DiaOfBar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[1].FindControl("txt_DiaOfBar");
                    TextBox txt_IdMARK = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[2].FindControl("txt_IdMARK");
                    txt_IdMARK.Text = stc.STCINWD_IdMark_var.ToString();
                    txt_DiaOfBar.Text = stc.STCINWD_Daimeter_tint.ToString();
                }
            }
        }
        public void DisplayGridRow()
        {
            if (Convert.ToInt32(txt_Qty.Text) > 0)
            {
                if (Convert.ToInt32(txt_Qty.Text) > grdSTCEntryRptInward.Rows.Count)
                {
                    for (int i = grdSTCEntryRptInward.Rows.Count + 1; i <= Convert.ToInt32(txt_Qty.Text); i++)
                    {
                        AddRowEnterReportSTCInward();
                    }
                }
            }
        }
        protected void txt_QtyOnTextChanged(object sender, EventArgs e)
        {
            int qty = 0;
            if (int.TryParse(txt_Qty.Text, out qty))
            {
                if (Convert.ToInt32(txt_Qty.Text) > 0)
                {
                    if (Convert.ToInt32(txt_Qty.Text) < grdSTCEntryRptInward.Rows.Count)
                    {
                        for (int i = grdSTCEntryRptInward.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                        {
                            if (grdSTCEntryRptInward.Rows.Count > 1)
                            {
                                DeleteDataReportSTCInward(i - 1);
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
        public void DisplaySTCGrid()
        {
            int i = 0;
            var sInwd = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "STC");
            foreach (var STC in sInwd)
            {
                AddRowEnterReportSTCInward();
                TextBox txt_DiaOfBar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[1].FindControl("txt_DiaOfBar");
                TextBox txt_IdMARK = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[2].FindControl("txt_IdMARK");
                TextBox txt_Carbon = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[3].FindControl("txt_Carbon");
              //  TextBox txt_Manganese = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[4].FindControl("txt_Manganese");
                TextBox txt_Sulphar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[5].FindControl("txt_Sulphar");
                TextBox txt_Phosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[6].FindControl("txt_Phosphorous");
                TextBox txt_SulpharPhosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[7].FindControl("txt_SulpharPhosphorous");
                txt_DiaOfBar.Text = STC.STCTEST_Diameter_tint.ToString();
                txt_IdMARK.Text = STC.STCTEST_IdMark_var.ToString();
                txt_Carbon.Text = STC.STCTEST_Carbon_var.ToString();
               // txt_Manganese.Text = STC.STCTEST_Manganese_var.ToString();
                txt_Sulphar.Text = STC.STCTEST_Sulphur_var.ToString();
                txt_Phosphorous.Text = STC.STCTEST_Phosphorous_var.ToString();
                i++;
            }
            if (grdSTCEntryRptInward.Rows.Count <= 0)
            {
                DisplayGridRow();
                DisplayIdMark();
            }
            else
            {
                Calculation();
            }
        }
        protected void DeleteDataReportSTCInward(int rowIndex)
        {
            GetCurrentDataSTCInward();
            DataTable dt = ViewState["STCTestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["STCTestTable"] = dt;
            grdSTCEntryRptInward.DataSource = dt;
            grdSTCEntryRptInward.DataBind();
            SetPreviousDataSTCInward();
        }
        protected void AddRowEnterReportSTCInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["STCTestTable"] != null)
            {
                GetCurrentDataSTCInward();
                dt = (DataTable)ViewState["STCTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_DiaOfBar", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IdMARK", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Carbon", typeof(string)));
               // dt.Columns.Add(new DataColumn("txt_Manganese", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Sulphar", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Phosphorous", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_SulpharPhosphorous", typeof(string)));

            }
            dr = dt.NewRow();
            dr["txt_DiaOfBar"] = string.Empty;
            dr["txt_IdMARK"] = string.Empty;
            dr["txt_Carbon"] = string.Empty;
           // dr["txt_Manganese"] = string.Empty;
            dr["txt_Sulphar"] = string.Empty;
            dr["txt_Phosphorous"] = string.Empty;
            dr["txt_SulpharPhosphorous"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["STCTestTable"] = dt;
            grdSTCEntryRptInward.DataSource = dt;
            grdSTCEntryRptInward.DataBind();
            SetPreviousDataSTCInward();
        }
        protected void GetCurrentDataSTCInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_DiaOfBar", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IdMARK", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Carbon", typeof(string)));
           // dtTable.Columns.Add(new DataColumn("txt_Manganese", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Sulphar", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Phosphorous", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_SulpharPhosphorous", typeof(string)));

            for (int i = 0; i < grdSTCEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_DiaOfBar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[1].FindControl("txt_DiaOfBar");
                TextBox txt_IdMARK = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[2].FindControl("txt_IdMARK");
                TextBox txt_Carbon = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[3].FindControl("txt_Carbon");
               // TextBox txt_Manganese = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[4].FindControl("txt_Manganese");
                TextBox txt_Sulphar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[5].FindControl("txt_Sulphar");
                TextBox txt_Phosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[6].FindControl("txt_Phosphorous");
                TextBox txt_SulpharPhosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[7].FindControl("txt_SulpharPhosphorous");

                drRow = dtTable.NewRow();
                drRow["txt_DiaOfBar"] = txt_DiaOfBar.Text;
                drRow["txt_IdMARK"] = txt_IdMARK.Text;
                drRow["txt_Carbon"] = txt_Carbon.Text;
               // drRow["txt_Manganese"] = txt_Manganese.Text;
                drRow["txt_Sulphar"] = txt_Sulphar.Text;
                drRow["txt_Phosphorous"] = txt_Phosphorous.Text;
                drRow["txt_SulpharPhosphorous"] = txt_SulpharPhosphorous.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["STCTestTable"] = dtTable;

        }
        protected void SetPreviousDataSTCInward()
        {
            DataTable dt = (DataTable)ViewState["STCTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_DiaOfBar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[1].FindControl("txt_DiaOfBar");
                TextBox txt_IdMARK = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[2].FindControl("txt_IdMARK");
                TextBox txt_Carbon = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[3].FindControl("txt_Carbon");
               // TextBox txt_Manganese = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[4].FindControl("txt_Manganese");
                TextBox txt_Sulphar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[5].FindControl("txt_Sulphar");
                TextBox txt_Phosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[6].FindControl("txt_Phosphorous");
                TextBox txt_SulpharPhosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[7].FindControl("txt_SulpharPhosphorous");

                txt_DiaOfBar.Text = dt.Rows[i]["txt_DiaOfBar"].ToString();
                txt_IdMARK.Text = dt.Rows[i]["txt_IdMARK"].ToString();
                txt_Carbon.Text = dt.Rows[i]["txt_Carbon"].ToString();
               // txt_Manganese.Text = dt.Rows[i]["txt_Manganese"].ToString();
                txt_Sulphar.Text = dt.Rows[i]["txt_Sulphar"].ToString();
                txt_Phosphorous.Text = dt.Rows[i]["txt_Phosphorous"].ToString();
                txt_SulpharPhosphorous.Text = dt.Rows[i]["txt_SulpharPhosphorous"].ToString();

            }

        }

        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowSTCRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdSTCRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdSTCRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowPileRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowPileRemark(int rowIndex)
        {
            GetCurrentDataSTCRemark();
            DataTable dt = ViewState["STCRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["STCRemarkTable"] = dt;
            grdSTCRemark.DataSource = dt;
            grdSTCRemark.DataBind();
            SetPreviousDataSTCRemark();
        }

        protected void AddRowSTCRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["STCRemarkTable"] != null)
            {
                GetCurrentDataSTCRemark();
                dt = (DataTable)ViewState["STCRemarkTable"];
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

            ViewState["STCRemarkTable"] = dt;
            grdSTCRemark.DataSource = dt;
            grdSTCRemark.DataBind();
            SetPreviousDataSTCRemark();
        }

        protected void GetCurrentDataSTCRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdSTCRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdSTCRemark.Rows[i].Cells[1].FindControl("txt_REMARK");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["STCRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataSTCRemark()
        {
            DataTable dt = (DataTable)ViewState["STCRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdSTCRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdSTCRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {

                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);

                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 2, 0, "", 0, "", "", 0, 0, Convert.ToByte(txt_Qty.Text), "STC");
                    dc.ReportDetails_Update("STC", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "STC", txt_ReferenceNo.Text, "STC", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text,txt_Supplier.Text , 3, 0, "", 0, "", "", 0, 0, Convert.ToByte(txt_Qty.Text), "STC");
                    dc.ReportDetails_Update("STC", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "STC", txt_ReferenceNo.Text, "STC", null, false, true, false, false, false, false, false);
                }

                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "STC", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                dc.SteelChemicalTest_Update(txt_ReferenceNo.Text, "", "", "", "", "", 0, true);
                for (int i = 0; i < grdSTCEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_DiaOfBar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[1].FindControl("txt_DiaOfBar");
                    TextBox txt_IdMARK = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[2].FindControl("txt_IdMARK");
                    TextBox txt_Carbon = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[3].FindControl("txt_Carbon");
                    //TextBox txt_Manganese = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[4].FindControl("txt_Manganese");
                    TextBox txt_Sulphar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[5].FindControl("txt_Sulphar");
                    TextBox txt_Phosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[6].FindControl("txt_Phosphorous");
                    TextBox txt_SulpharPhosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[7].FindControl("txt_SulpharPhosphorous");
                    dc.SteelChemicalTest_Update(txt_ReferenceNo.Text, txt_Carbon.Text, "", txt_Sulphar.Text, txt_Phosphorous.Text, txt_IdMARK.Text, Convert.ToByte(txt_DiaOfBar.Text), false);
                }

                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "STC", true);
                for (int i = 0; i < grdSTCRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdSTCRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "STC");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.STC_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "STC");
                            foreach (var c in chkId)
                            {
                                if (c.STCDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "STC", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "STC");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "STC");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.STC_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "STC", false);
                            }
                        }
                    }
                }
                #endregion
                                
                if (lbl_TestedBy.Text == "Approve By")
                {
                    //Approve on check
                    string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                    if (cnStr.ToLower().Contains("veenalive") == false)
                    {
                    
                        ApproveReports("STC", txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text)); //, txtEmailId.Text, lblContactNo.Text);
                                                                                                        //update MISCRLApprStatus to 1 if SITE_CRBypass_bit is 1
                        int siteCRbypssBit = 0; //clsData cd = new clsData();
                        siteCRbypssBit = cd.getClientCrlBypassBit("STC", Convert.ToInt32(lblRecordNo.Text));
                        if (siteCRbypssBit == 1)
                            dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, "STC");

                        //SMS
                        var res = dc.SMSDetailsForReportApproval_View(Convert.ToInt32(lblRecordNo.Text), "STC").ToList();
                        if (res.Count > 0)
                        {
                            cd.sendInwardReportMsg(Convert.ToInt32(res.FirstOrDefault().ENQ_Id), Convert.ToString(res.FirstOrDefault().ENQ_Date_dt), Convert.ToInt32(res.FirstOrDefault().SITE_MonthlyBillingStatus_bit).ToString(), res.FirstOrDefault().crLimitExceededStatus.ToString(), res.FirstOrDefault().BILL_NetAmt_num.ToString(), txt_ReferenceNo.Text, Convert.ToString(res.FirstOrDefault().INWD_ContactNo_var), "Report");
                        }
                        //old dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text), "STC");

                        // CRLimitExceedEmail(txt_ReferenceNo.Text, lblRecordNo.Text.ToString(), "STC", "Steel Chemical Testing");
                    }
                    
                    //update ULR No.
                    dc.Inward_Update_ULRNo(Convert.ToInt32(lblRecordNo.Text), "STC", txt_ReferenceNo.Text, ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text), "STC");
                    var InwardSTC = dc.ReportStatus_View("Steel Chemical Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text.ToString(), 0, 0, 0);
                    foreach (var STC in InwardSTC)
                    {
                        lblULRNo.Text = "ULR No : " + STC.STCINWD_ULRNo_var;
                        lblULRNo.Visible = true;
                    }
                    //
                }
                lnkPrint.Visible = true;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        public void ApproveReports(string Recordtype, string ReferenceNo, int RecordNo) //, string EmailId, string ContactNo)
        {
            string tempRecType = Recordtype;
            string testType = Recordtype;

            #region Bill Generation
            bool approveRptFlag = true;
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
            //if (generateBillFlag == true && Recordtype == "CT")
            //{
            //    var ctinwd = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, RecordNo, "", 0, Recordtype);
            //    foreach (var ct in ctinwd)
            //    {
            //        if (ct.CTINWD_CouponNo_var != null && ct.CTINWD_CouponNo_var != "")
            //        {
            //            generateBillFlag = false;
            //            break;
            //        }
            //    }
            //}
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
                //var gstbillCount = dc.Bill_View("0", 0, 0, "", 0, false, false, DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                if (gstbillCount.Count() != NewrecNo - 1)
                {
                    generateBillFlag = false;
                    approveRptFlag = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill No. mismatch. Can not approve report.');", true);
                }
            }
            if (approveRptFlag == true)
            {
                //Generate bill
                if (generateBillFlag == true)
                {
                    BillUpdation bill = new BillUpdation();
                    BillNo = bill.UpdateBill(Recordtype, RecordNo, BillNo);
                }
                //
                dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, Convert.ToByte(ddl_TestedBy.SelectedValue), true, "Approved By");
                dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, true, false, false, false, false);
            }
            #endregion
            #region email report
            //mail report
            //bool sendMail = true;
            //if (EmailId.Trim() == "" || EmailId.Trim().ToLower() == "na@unknown.com" ||
            //    EmailId.Trim().ToLower() == "na" || EmailId.Trim().ToLower().Contains("na@") == true ||
            //    EmailId.Trim().ToLower().Contains("@") == false || EmailId.Trim().ToLower().Contains(".") == false)
            //{
            //    sendMail = false;
            //}
            //if (IsValidEmailAddress(EmailId.Trim()) == false)
            //{
            //    sendMail = false;
            //}
            //////----------------- shift on print screen
            //////ContactNo = "9011085721";
            ////if (ContactNo.Length == 10 || ContactNo.Length == 12)
            ////{
            ////    string strMsg = "Dear Customer your " + ddl_InwardTestType.SelectedItem.Text + " Sample Ref.No. " + RefNo + " is ready for despatch. Plz revert back to us if you don't receive the report within 24 hrs.";
            ////    clsData objcls = new clsData();
            ////    clsSendMail objmail = new clsSendMail();
            ////    objmail.sendSMS(ContactNo, strMsg, "DUROCR");
            ////}
            //// //------------------
            //sendMail = false;
            //if (sendMail == true)
            //{
            //    BindAppoveRpt(Rectype,RefNo,"Email");
            //    string reportPath = "C:/temp/Veena/" + Rectype + "_" + RefNo.Replace('/', '_') + ".pdf";
            //    //string reportPath = System.Web.HttpContext.Current.Server.MapPath("~") + "Reports/" + Rectype + "_" + RefNo.Replace('/', '_') + ".pdf";
            //    if (File.Exists(@reportPath))
            //    {
            //        clsSendMail objMail = new clsSendMail();
            //        string mTo = "", mCC = "", mSubject = "", mbody = "", mReplyTo = "", TestType = "";                    
            //        mTo = EmailId.Trim();                    
            //        mCC = "duroreports.pune@gmail.com";
            //        mSubject = "";
            //        TestType = "";
            //        if (Rectype == "CT")
            //        {
            //            mSubject = "Concrete Cube Compression test.";
            //            TestType = "Concrete Cube test";
            //        }
            //        else if (Rectype == "ST")
            //            mSubject = "Physical test report for reinforcement steel";
            //        else if (Rectype == "AGGT")
            //            mSubject = "Aggregate test report";
            //        else if (Rectype == "CEMT")
            //            mSubject = "Cement Test Report";
            //        else if (Rectype == "FLYASH")
            //            mSubject = "FLYASH Test Report";
            //        else if (Rectype == "PILE")
            //            mSubject = "PILE Test Report";
            //        else if (Rectype == "TILE")
            //            mSubject = "TILE Test Report";
            //        else if (Rectype == "PT")
            //            mSubject = "Pavement Test Report";
            //        else if (Rectype == "SOLID")
            //            mSubject = "SOLID Test Report";
            //        else if (Rectype == "BT-")
            //            mSubject = "BRICK Test Report";
            //        else if (Rectype == "MF")
            //            mSubject = "Mix Design Test Report";
            //        else if (Rectype == "STC")
            //            mSubject = "Steel Chemical Test Report";
            //        else if (Rectype == "CCH")
            //            mSubject = "Cement Chemical Test Report";
            //        else if (Rectype == "WT")
            //            mSubject = "Mix Design Test Report";
            //        else if (Rectype == "AAC")
            //            mSubject = "Autoclaved Aerated Cellular Test Report";
            //        else
            //            mSubject = TestType + " Test Report";

            //        if (TestType == "")
            //            TestType = mSubject.Replace("Report", "");

            //        mbody = "Dear Sir/Madam,<br><br>";
            //        mbody = mbody + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please find attached " + mSubject + " <br>";
            //        mbody = mbody + "Please feel free to contact in case of any queries." + " <br><br><br>";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "Thanks & Best Regards.";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "Manager Technical";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.(PUNE)";
            //        objMail.SendMail(mTo, mCC, mSubject, mbody, reportPath, mReplyTo);
            //    }
            //}

            //
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "myalert", "alert('Record has been successfully Approved !');", true);
            //Main.Visible = false;
            #endregion
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Report Approved Successfully.";
            lblMsg.Visible = true;
        }
        public void CRLimitExceedEmail(string referenceNo, string recordNo, string recordType, string inwardType)
        {
            bool addMailId = true; //, crBypassLmt = false;
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
                    tollFree = "9850500013";
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
                //mSubject = "Report Confirmation";

                ////  mbody = "Dear Customer,<br><br>";


                //if (crLimtFlag == 1)//Credit Limit exceeded Client
                //    mbody = mbody + "Dear Customer,<br><br>We have tested material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ". The report is ready with us. Your total outstanding dues are Rs " + strOutstndingAmt + ".<br>Please arrange to make the payment for the testing to access the report on our Durocrete Mobile App. The copy of the report will be emailed to you on your registered email id once payment is made. For any assistance please contact on our tollfree no. " + tollFree + "<br><br><br>";
                //else
                //    mbody = mbody + "Dear Customer,<br><br>We have tested material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ". The report is ready with us, soft copy of the report has been emailed to you on your registered email id. You can view this report  with help of Durocrete APP on your mobile phone.You can also download the report from our website www.durocrete.in. For any assistance please contact on our tollfree no. " + tollFree + "<br><br><br>";

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
            else if (ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                if (lblEntry.Text == "Check")
                {
                    lblMsg.Text = "Please Select the Approve By";
                    valid = false;
                }
                else
                {
                    lblMsg.Text = "Please Select the Tested By";
                    valid = false;
                }
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdSTCEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_DiaOfBar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[1].FindControl("txt_DiaOfBar");
                    TextBox txt_IdMARK = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[2].FindControl("txt_IdMARK");
                    TextBox txt_Carbon = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[3].FindControl("txt_Carbon");
                  //  TextBox txt_Manganese = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[4].FindControl("txt_Manganese");
                    TextBox txt_Sulphar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[5].FindControl("txt_Sulphar");
                    TextBox txt_Phosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[6].FindControl("txt_Phosphorous");

                    if (txt_DiaOfBar.Text == "")
                    {
                        lblMsg.Text = "Please Enter Dia Of Bar for Sr No. " + (i + 1) + ".";
                        txt_DiaOfBar.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_IdMARK.Text == "")
                    {
                        lblMsg.Text = "Please Enter Id Mark for Sr No. " + (i + 1) + ".";
                        txt_IdMARK.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Carbon.Text == "")
                    {
                        lblMsg.Text = "Please Enter Carbon(%) for Sr No. " + (i + 1) + ".";
                        txt_Carbon.Focus();
                        valid = false;
                        break;
                    }
                    //if (txt_Manganese.Text == "")
                    //{
                    //    lblMsg.Text = "Please Enter Manganese(%) for Sr No. " + (i + 1) + ".";
                    //    txt_Manganese.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    if (txt_Sulphar.Text == "")
                    {
                        lblMsg.Text = "Please Enter Sulphar(%) for Sr No. " + (i + 1) + ".";
                        txt_Sulphar.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Phosphorous.Text == "")
                    {
                        lblMsg.Text = "Please Enter Phosphorous(%) for Sr No. " + (i + 1) + ".";
                        txt_Phosphorous.Focus();
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
                Calculation();
            }
            return valid;
        }
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            FetchRefNo();
        }
        public void FetchRefNo()
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---")
            {
                lnkSave.Enabled = true;
                lnkPrint.Visible = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                DisplaySTCDetails();
                //LoadOtherPendingCheckRpt();
                LoadReferenceNoList();
                ViewWitnessBy();
                LoadApproveBy();
                grdSTCEntryRptInward.DataSource = null;
                grdSTCEntryRptInward.DataBind();
                DisplaySTCGrid();
                if (grdSTCRemark.Rows.Count > 0)
                {
                    grdSTCRemark.DataSource = null;
                    grdSTCRemark.DataBind();
                }
                DisplayRemark();
            }
        }
        protected void Lnk_Calculate_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
            }
        }
        public void Calculation()
        {

            decimal TotalSP = 0;
            for (int i = 0; i < grdSTCEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_Carbon = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[3].FindControl("txt_Carbon");
                TextBox txt_Sulphar = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[5].FindControl("txt_Sulphar");
                TextBox txt_Phosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[6].FindControl("txt_Phosphorous");
                TextBox txt_SulpharPhosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].Cells[7].FindControl("txt_SulpharPhosphorous");

                string[] line = txt_Sulphar.Text.Split('*');
                foreach (string line1 in line)
                {
                    if (line1 != "")
                    {
                        txt_Sulphar.Text = line1.ToString();
                    }
                }
                string[] phos = txt_Phosphorous.Text.Split('*');
                foreach (string line2 in phos)
                {
                    if (line2 != "")
                    {
                        txt_Phosphorous.Text = line2.ToString();
                    }
                }
                string[] carbon = txt_Carbon.Text.Split('*');
                foreach (string line3 in carbon)
                {
                    if (line3 != "")
                    {
                        txt_Carbon.Text = line3.ToString();
                    }
                }
                TotalSP = Convert.ToDecimal(txt_Sulphar.Text) + Convert.ToDecimal(txt_Phosphorous.Text);

                decimal SpecifiedLmt = 0;
                decimal result = 0;
                decimal variation = 0;

                var gInwd = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, txt_GradeOfSteel.Text, 0, "STC");
                foreach (var grd in gInwd)
                {
                    if (grd.Constituents.ToString() == "Carbon")
                    {
                        SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
                        var variat = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
                        foreach (var vat in variat)
                        {
                            if (vat.Constituents.ToString() == "Carbon")
                            {
                                variation = Convert.ToDecimal(vat.SpecifiedLimit);
                                result = variation + SpecifiedLmt;
                                if (Convert.ToDecimal(txt_Carbon.Text) > result)
                                {
                                    txt_Carbon.Text = "*" + txt_Carbon.Text;
                                }
                                else
                                {
                                    txt_Carbon.Text = txt_Carbon.Text;
                                }
                            }
                        }
                    }
                    if (grd.Constituents.ToString() == "Sulphur")
                    {
                        SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
                        var variat = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
                        foreach (var vat in variat)
                        {
                            if (vat.Constituents.ToString() == "Sulphur")
                            {
                                variation = Convert.ToDecimal(vat.SpecifiedLimit);
                                result = variation + SpecifiedLmt;
                                if (Convert.ToDecimal(txt_Sulphar.Text) > result)
                                {
                                    txt_Sulphar.Text = "*" + txt_Sulphar.Text;
                                }
                                else
                                {
                                    txt_Sulphar.Text = txt_Sulphar.Text;
                                }
                            }
                        }
                    }
                    if (grd.Constituents.ToString() == "Phosphorous")
                    {
                        SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
                        var variat = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
                        foreach (var vat in variat)
                        {
                            if (vat.Constituents.ToString() == "Phosphorous")
                            {
                                variation = Convert.ToDecimal(vat.SpecifiedLimit);
                                result = variation + SpecifiedLmt;
                                if (Convert.ToDecimal(txt_Phosphorous.Text) > result)
                                {
                                    txt_Phosphorous.Text = "*" + txt_Phosphorous.Text;
                                }
                                else
                                {
                                    txt_Phosphorous.Text = txt_Phosphorous.Text;
                                }
                            }
                        }
                    }
                    if (grd.Constituents.ToString() == "Sulphur + Phosphorous")
                    {
                        SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
                        var variat = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
                        foreach (var vat in variat)
                        {
                            if (vat.Constituents.ToString() == "Sulphur + Phosphorous")
                            {
                                variation = Convert.ToDecimal(vat.SpecifiedLimit);
                                result = variation + SpecifiedLmt;
                                //if (Convert.ToDecimal(TotalSP) > variation)
                                if (Convert.ToDecimal(TotalSP) > result)
                                {
                                    txt_SulpharPhosphorous.Text = "*" + TotalSP.ToString();
                                }
                                else
                                {
                                    txt_SulpharPhosphorous.Text = TotalSP.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", txt_ReferenceNo.Text.ToString(), 0, "STC");
            foreach (var r in re)
            {
                AddRowSTCRemark();
                TextBox txt_REMARK = (TextBox)grdSTCRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.STCDetail_RemarkId_int), "STC");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.STC_Remark_var.ToString();
                    i++;
                }
            }
            if (grdSTCRemark.Rows.Count <= 0)
            {
                AddRowSTCRemark();
            }
        }
        public void ViewWitnessBy()
        {
            txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ct = dc.ReportStatus_View("Steel Chemical Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text.ToString(), 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.STCINWD_WitnessBy_var.ToString() != null && c.STCINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.STCINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }

        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.STC_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecordType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            // RptSTCReport();
        }
        //public void RptSTCReport()
        //{
        //    string reportPath;
        //    string reportStr = "";
        //    StreamWriter sw;            
        //    PrintHTMLReport rptHtml = new PrintHTMLReport();
        //    reportPath = Server.MapPath("~") + "\\report.html";
        //    sw = File.CreateText(reportPath);
        //  //  reportStr = rptHtml.getDetailReportSTC();

        //    sw.WriteLine(reportStr);
        //    sw.Close();
        //    NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

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

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            #region TS
            mySql = @"select ref.reference_number,x.*,head.[description],head.[grade_of_concrete],head.[date_of_testing] "+
                     " ,head.[image],head.[witness_by],head.[date_of_casting],head.[id_mark],   " +
                     " stc.dia_of_bar,stc.id_mark,stc.carbon,stc.sulphar,stc.phosphorous ,stc.quantity_no " +
                     " from [new_gt_app_db].[dbo].[reference_number] ref, [new_gt_app_db].[dbo].[category_refno_wise_test] x, [new_gt_app_db].[dbo].[category_header] head, "+
                     " [new_gt_app_db].[dbo].steel_chemical stc "+
                     " where ref.reference_number ='" + txt_ReferenceNo.Text + "' "+
                     " and ref.pk_id = x.reference_number_fk_id and x.category_fk_id = head.category_fk_id "+
                     " and x.category_wise_test_fk_id = head.test_fk_id and x.reference_number_fk_id = head.reference_no  "+
                     " and x.category_fk_id = 7 and x.category_wise_test_fk_id = 35"+
                     " and head.pk_id = stc.category_header_fk_id  ";
            
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
                txt_Qty.Text = dt.Rows.Count.ToString();
                if (Convert.ToInt32(txt_Qty.Text) > 0)
                {
                    if (Convert.ToInt32(txt_Qty.Text) < grdSTCEntryRptInward.Rows.Count)
                    {
                        for (int i = grdSTCEntryRptInward.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                        {
                            if (grdSTCEntryRptInward.Rows.Count > 1)
                            {   
                                DeleteDataReportSTCInward(i - 1); 
                            }
                        }
                    }
                    else
                    {
                        DisplayGridRow();
                    }
                }

                //                DisplayGridRow();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                   TextBox txt_DiaOfBar = (TextBox)grdSTCEntryRptInward.Rows[i].FindControl("txt_DiaOfBar");
                    TextBox txt_IdMARK = (TextBox)grdSTCEntryRptInward.Rows[i].FindControl("txt_IdMARK");

                    TextBox txt_Carbon = (TextBox)grdSTCEntryRptInward.Rows[i].FindControl("txt_Carbon");
                    //TextBox txt_Manganese = (TextBox)grdSTCEntryRptInward.Rows[i].FindControl("txt_Manganese");
                    TextBox txt_Sulphar = (TextBox)grdSTCEntryRptInward.Rows[i].FindControl("txt_Sulphar");
                    TextBox txt_Phosphorous = (TextBox)grdSTCEntryRptInward.Rows[i].FindControl("txt_Phosphorous");
                    //,stc.,stc.carbon,stc.sulphar,stc.phosphorous ,stc.quantity_no
                    txt_DiaOfBar.Text= dt.Rows[i]["dia_of_bar"].ToString();
                    txt_IdMARK.Text = dt.Rows[i]["id_mark"].ToString();
                    txt_Carbon.Text = dt.Rows[i]["carbon"].ToString();
                    //txt_Manganese.Text = dt.Rows[i]["sulphar"].ToString();
                    txt_Sulphar.Text = dt.Rows[i]["sulphar"].ToString();
                    txt_Phosphorous.Text = dt.Rows[i]["phosphorous"].ToString();

                }

            }
            dt.Dispose();
            #endregion

            objcls = null;
        }
    }
}
