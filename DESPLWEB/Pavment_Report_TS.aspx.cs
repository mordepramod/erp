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
    public partial class Pavement_Report_TS : System.Web.UI.Page
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
                lblheading.Text = "Pavement Splitting Tensile Strength - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_RecType.Text != "")
                {
                    DisplayPavement_TS_Details();
                    DisplayPavment_TSGrid();
                    DisplayRemark();
                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "Pavement Splitting Tensile Strength - Report Check";
                        //lblEntry.Text = Session["Check"].ToString();//
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
                        ViewWitnessBy();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
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

            var reportList = dc.ReferenceNo_View_StatusWise("PT", reportStatus, Convert.ToInt32(txt_TestId.Text));
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
            var testinguser = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, Convert.ToInt32(txt_TestId.Text));
            ddl_OtherPendingRpt.DataTextField = "PTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "PTINWD_ReferenceNo_var";
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
                var testinguser = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, Convert.ToInt32(txt_TestId.Text));
                ddl_OtherPendingRpt.DataTextField = "PTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "PTINWD_ReferenceNo_var";
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
              txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ct = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var p in ct)
                {
                    if (Convert.ToString(p.PTINWD_WitnessBy_var) != string.Empty)
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = p.PTINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }
        }
        public void DisplayPavement_TS_Details()
        {
            var Inwardc = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var p in Inwardc)
            {
                txt_ReferenceNo.Text = p.PTINWD_ReferenceNo_var.ToString();

                if (p.PTINWD_CastingDate_dt != "NA")
                {
                    txt_DtOfCasting.Text = p.PTINWD_CastingDate_dt.ToString();
                }
                else
                {
                    txt_DtOfCasting.Text = "NA";
                }
                if (Convert.ToString(p.PTINWD_TestingDate_dt) != string.Empty)
                {
                    txt_TestingDt.Text = Convert.ToDateTime(p.PTINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_TestingDt.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_TestingDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (Convert.ToString(p.INWD_CollectionDate_dt) != string.Empty)
                {
                    txt_CollectionDt.Text = Convert.ToDateTime(p.INWD_CollectionDate_dt).ToString("dd/MM/yyyy");
                }
                else
                {
                    txt_CollectionDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (ddl_NablScope.Items.FindByValue(p.PTINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(p.PTINWD_NablScope_var);
                }

                if (Convert.ToString(p.PTINWD_NablLocation_int) != null && Convert.ToString(p.PTINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(p.PTINWD_NablLocation_int);
                }

                txt_RecType.Text = p.PTINWD_RecordType_var.ToString();
                txt_NatureofWork.Text = p.PTINWD_WorkingNature_var.ToString();
                txt_SupplierName.Text = p.PTINWD_SupplierName_var.ToString();
                txt_GradeOfConcrete.Text = p.PTINWD_Grade_var.ToString();
                txt_ReportNo.Text = p.PTINWD_SetOfRecord_var.ToString();
                txt_Description.Text = p.PTINWD_Description_var.ToString();
                txt_Qty.Text = p.PTINWD_Quantity_tint.ToString();
                txt_TestId.Text = p.PTINWD_TEST_Id.ToString();
               //  txt_TestId.Text  = p.PTINWD_TEST_Id.ToString();

            }
        }
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.Pavement_Test_Remark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, Convert.ToInt32(txt_TestId.Text));
            foreach (var r in re)
            {
                AddRowPT_CS_Remark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32(txt_TestId.Text));
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.Remark_var.ToString();
                    i++;
                }
            }
            if (grdRemark.Rows.Count <= 0)
            {
                AddRowPT_CS_Remark();
            }

        }
        public void DisplayPavment_TSGrid()
        {
            int i = 0;
            string AvgTensileStr = "";
            var PT_TS = dc.Pavement_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32(txt_TestId.Text), "TS");
            foreach (var pt in PT_TS)
            {
                AddRowEnterReportPT_TSInward();
                TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].Cells[2].FindControl("ddl_BlockType");
                DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].Cells[3].FindControl("ddl_Thickness");
                Button FailureThickness = (Button)grdPT_TSInward.Rows[i].Cells[4].FindControl("FailureThickness");
                TextBox txt_FailureLoad = (TextBox)grdPT_TSInward.Rows[i].Cells[5].FindControl("txt_FailureLoad");
                Button FailureLength = (Button)grdPT_TSInward.Rows[i].Cells[6].FindControl("FailureLength");
                TextBox txt_CorectionFactor = (TextBox)grdPT_TSInward.Rows[i].Cells[7].FindControl("txt_CorectionFactor");
                TextBox txt_Age = (TextBox)grdPT_TSInward.Rows[i].Cells[8].FindControl("txt_Age");
                TextBox txt_AreaOfFailure = (TextBox)grdPT_TSInward.Rows[i].Cells[9].FindControl("txt_AreaOfFailure");
                TextBox txt_FaliureLoadPerLen = (TextBox)grdPT_TSInward.Rows[i].Cells[10].FindControl("txt_FaliureLoadPerLen");
                TextBox txt_TensileStrength = (TextBox)grdPT_TSInward.Rows[i].Cells[11].FindControl("txt_TensileStrength");
                TextBox txt_ActualresultDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[13].FindControl("txt_ActualresultDtls");
                TextBox txt_FaliureLenDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[14].FindControl("txt_FaliureLenDtls");

                txt_IdMark.Text = pt.IdMark_var.ToString();
                ddl_BlockType.SelectedItem.Text = Convert.ToString(pt.BlockType_var);
                ddl_Thickness.SelectedItem.Text = Convert.ToString(pt.Thickness_var);
                FailureThickness.Text = Convert.ToString(pt.FailureThickness_int);
                txt_FailureLoad.Text = Convert.ToString(pt.FailureLoad_num);
                FailureLength.Text = Convert.ToString(pt.FailureLength_num);
                txt_CorectionFactor.Text = Convert.ToString(pt.CorrectionFactor_dec);
                txt_Age.Text = Convert.ToString(pt.Age_var);
                txt_AreaOfFailure.Text = Convert.ToString(pt.AreaOfFailure_num);
                txt_FaliureLoadPerLen.Text = Convert.ToString(pt.FailureLoadPerUnitLen_num);
                txt_TensileStrength.Text = Convert.ToString(pt.TensileStrength_dec);
                txt_ActualresultDtls.Text = Convert.ToString(pt.AcutalresultDtl_var);
                txt_FaliureLenDtls.Text = Convert.ToString(pt.FailueLengthDtls_var);
                txt_TestId.Text = Convert.ToString(pt.TestId_int);
                if (i == 0)
                {
                    AvgTensileStr = Convert.ToString(pt.PTINWD_AvgStr_var);
                }
                i++;
            }

            if (grdPT_TSInward.Rows.Count > 0)
            {
                int NoOfrows = grdPT_TSInward.Rows.Count / 2;
                TextBox txt_AvgStr = (TextBox)grdPT_TSInward.Rows[NoOfrows].Cells[12].FindControl("txt_AvgStr");
                txt_AvgStr.Text = Convert.ToString(AvgTensileStr);
            }
            if (grdPT_TSInward.Rows.Count <= 0)
            {
                DisplayGridRow();
                DisplayIdMark();
            }

        }
        public void DisplayIdMark()
        {
            var ct = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 2, 0);
            foreach (var pt in ct)
            {
                for (int i = 0; i < grdPT_TSInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].Cells[2].FindControl("ddl_BlockType");
                    DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].Cells[3].FindControl("ddl_Thickness");
                    txt_IdMark.Text = pt.PTINWD_Id_Mark_var.ToString();
                    ddl_BlockType.ClearSelection();
                    ddl_BlockType.Items.FindByText(pt.PTINWD_BlockType_var).Selected = true;
                    ddl_Thickness.ClearSelection();
                    ddl_Thickness.Items.FindByText(pt.PTINWD_Thickness_var).Selected = true;

                }
            }
        }
        public void DisplayGridRow()
        {
            if (Convert.ToInt32(txt_Qty.Text) > 0)
            {
                if (Convert.ToInt32(txt_Qty.Text) > grdPT_TSInward.Rows.Count)
                {
                    for (int i = grdPT_TSInward.Rows.Count + 1; i <= Convert.ToInt32(txt_Qty.Text); i++)
                    {
                        AddRowEnterReportPT_TSInward();
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
                    if (Convert.ToInt32(txt_Qty.Text) < grdPT_TSInward.Rows.Count)
                    {
                        for (int i = grdPT_TSInward.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                        {
                            if (grdPT_TSInward.Rows.Count > 1)
                            {
                                DeleteDataPT_TSInward(i - 1);
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

        protected void AddRowEnterReportPT_TSInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PT_TS_DetailTable"] != null)
            {
                GetCurrentDataPT_TSInward();
                dt = (DataTable)ViewState["PT_TS_DetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_BlockType", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Thickness", typeof(string)));
                dt.Columns.Add(new DataColumn("FailureThickness", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FailureLoad", typeof(string)));
                dt.Columns.Add(new DataColumn("FailureLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CorectionFactor", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Age", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AreaOfFailure", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FaliureLoadPerLen", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TensileStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AvgStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_ActualresultDtls", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FaliureLenDtls", typeof(string)));

            }
            dr = dt.NewRow();
            dr["txt_IdMark"] = string.Empty;
            dr["ddl_BlockType"] = string.Empty;
            dr["ddl_Thickness"] = string.Empty;
            dr["FailureThickness"] = string.Empty;
            dr["txt_FailureLoad"] = string.Empty;
            dr["FailureLength"] = string.Empty;
            dr["txt_CorectionFactor"] = string.Empty;
            dr["txt_Age"] = string.Empty;
            dr["txt_AreaOfFailure"] = string.Empty;
            dr["txt_FaliureLoadPerLen"] = string.Empty;
            dr["txt_TensileStrength"] = string.Empty;
            dr["txt_AvgStr"] = string.Empty;
            dr["txt_ActualresultDtls"] = string.Empty;
            dr["txt_FaliureLenDtls"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["PT_TS_DetailTable"] = dt;
            grdPT_TSInward.DataSource = dt;
            grdPT_TSInward.DataBind();
            SetPreviousDataPT_TSInward();
        }
        protected void GetCurrentDataPT_TSInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_BlockType", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Thickness", typeof(string)));
            dtTable.Columns.Add(new DataColumn("FailureThickness", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FailureLoad", typeof(string)));
            dtTable.Columns.Add(new DataColumn("FailureLength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CorectionFactor", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Age", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AreaOfFailure", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FaliureLoadPerLen", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TensileStrength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_ActualresultDtls", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FaliureLenDtls", typeof(string)));

            for (int i = 0; i < grdPT_TSInward.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].Cells[2].FindControl("ddl_BlockType");
                DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].Cells[3].FindControl("ddl_Thickness");
                Button FailureThickness = (Button)grdPT_TSInward.Rows[i].Cells[4].FindControl("FailureThickness");
                TextBox txt_FailureLoad = (TextBox)grdPT_TSInward.Rows[i].Cells[5].FindControl("txt_FailureLoad");
                Button FailureLength = (Button)grdPT_TSInward.Rows[i].Cells[6].FindControl("FailureLength");
                TextBox txt_CorectionFactor = (TextBox)grdPT_TSInward.Rows[i].Cells[7].FindControl("txt_CorectionFactor");
                TextBox txt_Age = (TextBox)grdPT_TSInward.Rows[i].Cells[8].FindControl("txt_Age");
                TextBox txt_AreaOfFailure = (TextBox)grdPT_TSInward.Rows[i].Cells[9].FindControl("txt_AreaOfFailure");
                TextBox txt_FaliureLoadPerLen = (TextBox)grdPT_TSInward.Rows[i].Cells[10].FindControl("txt_FaliureLoadPerLen");
                TextBox txt_TensileStrength = (TextBox)grdPT_TSInward.Rows[i].Cells[11].FindControl("txt_TensileStrength");
                TextBox txt_AvgStr = (TextBox)grdPT_TSInward.Rows[i].Cells[12].FindControl("txt_AvgStr");
                TextBox txt_ActualresultDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[13].FindControl("txt_ActualresultDtls");
                TextBox txt_FaliureLenDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[14].FindControl("txt_FaliureLenDtls");

                drRow = dtTable.NewRow();
                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["ddl_BlockType"] = ddl_BlockType.Text;
                drRow["ddl_Thickness"] = ddl_Thickness.Text;
                drRow["FailureThickness"] = FailureThickness.Text;
                drRow["txt_FailureLoad"] = txt_FailureLoad.Text;
                drRow["FailureLength"] = FailureLength.Text;
                drRow["txt_CorectionFactor"] = txt_CorectionFactor.Text;
                drRow["txt_Age"] = txt_Age.Text;
                drRow["txt_AreaOfFailure"] = txt_AreaOfFailure.Text;
                drRow["txt_FaliureLoadPerLen"] = txt_FaliureLoadPerLen.Text;
                drRow["txt_TensileStrength"] = txt_TensileStrength.Text;
                drRow["txt_AvgStr"] = txt_AvgStr.Text;
                drRow["txt_ActualresultDtls"] = txt_ActualresultDtls.Text;
                drRow["txt_FaliureLenDtls"] = txt_FaliureLenDtls.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["PT_TS_DetailTable"] = dtTable;

        }
        protected void SetPreviousDataPT_TSInward()
        {
            DataTable dt = (DataTable)ViewState["PT_TS_DetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].Cells[2].FindControl("ddl_BlockType");
                DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].Cells[3].FindControl("ddl_Thickness");
                Button FailureThickness = (Button)grdPT_TSInward.Rows[i].Cells[4].FindControl("FailureThickness");
                TextBox txt_FailureLoad = (TextBox)grdPT_TSInward.Rows[i].Cells[5].FindControl("txt_FailureLoad");
                Button FailureLength = (Button)grdPT_TSInward.Rows[i].Cells[6].FindControl("FailureLength");
                TextBox txt_CorectionFactor = (TextBox)grdPT_TSInward.Rows[i].Cells[7].FindControl("txt_CorectionFactor");
                TextBox txt_Age = (TextBox)grdPT_TSInward.Rows[i].Cells[8].FindControl("txt_Age");
                TextBox txt_AreaOfFailure = (TextBox)grdPT_TSInward.Rows[i].Cells[9].FindControl("txt_AreaOfFailure");
                TextBox txt_FaliureLoadPerLen = (TextBox)grdPT_TSInward.Rows[i].Cells[10].FindControl("txt_FaliureLoadPerLen");
                TextBox txt_TensileStrength = (TextBox)grdPT_TSInward.Rows[i].Cells[11].FindControl("txt_TensileStrength");
                TextBox txt_AvgStr = (TextBox)grdPT_TSInward.Rows[i].Cells[12].FindControl("txt_AvgStr");
                TextBox txt_ActualresultDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[13].FindControl("txt_ActualresultDtls");
                TextBox txt_FaliureLenDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[14].FindControl("txt_FaliureLenDtls");

                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                ddl_BlockType.Text = dt.Rows[i]["ddl_BlockType"].ToString();
                ddl_Thickness.Text = dt.Rows[i]["ddl_Thickness"].ToString();
                FailureThickness.Text = dt.Rows[i]["FailureThickness"].ToString();
                txt_FailureLoad.Text = dt.Rows[i]["txt_FailureLoad"].ToString();
                FailureLength.Text = dt.Rows[i]["FailureLength"].ToString();
                txt_CorectionFactor.Text = dt.Rows[i]["txt_CorectionFactor"].ToString();
                txt_Age.Text = dt.Rows[i]["txt_Age"].ToString();
                txt_AreaOfFailure.Text = dt.Rows[i]["txt_AreaOfFailure"].ToString();
                txt_FaliureLoadPerLen.Text = dt.Rows[i]["txt_FaliureLoadPerLen"].ToString();
                txt_TensileStrength.Text = dt.Rows[i]["txt_TensileStrength"].ToString();
                txt_AvgStr.Text = dt.Rows[i]["txt_AvgStr"].ToString();
                txt_ActualresultDtls.Text = dt.Rows[i]["txt_ActualresultDtls"].ToString();
                txt_FaliureLenDtls.Text = dt.Rows[i]["txt_FaliureLenDtls"].ToString();
            }

        }
        protected void DeleteDataPT_TSInward(int rowIndex)
        {
            GetCurrentDataPT_TSInward();
            DataTable dt = ViewState["PT_TS_DetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PT_TS_DetailTable"] = dt;
            grdPT_TSInward.DataSource = dt;
            grdPT_TSInward.DataBind();
            SetPreviousDataPT_TSInward();
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
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowPT_CS_Remark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowCubeRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowCubeRemark(int rowIndex)
        {
            GetCurrentDataPT_CS_Remark();
            DataTable dt = ViewState["PavementRemark_FS_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PavementRemark_FS_Table"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataPT_CS_Remark();
        }
        protected void AddRowPT_CS_Remark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PavementRemark_FS_Table"] != null)
            {
                GetCurrentDataPT_CS_Remark();
                dt = (DataTable)ViewState["PavementRemark_FS_Table"];
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

            ViewState["PavementRemark_FS_Table"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataPT_CS_Remark();
        }

        protected void GetCurrentDataPT_CS_Remark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["PavementRemark_FS_Table"] = dtTable;

        }
        protected void SetPreviousDataPT_CS_Remark()
        {
            DataTable dt = (DataTable)ViewState["PavementRemark_FS_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                txt_DtOfCasting.Text = txt_DtOfCasting.Text.ToUpper();
                DateTime TestingDt = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
                DateTime CollectionDt = DateTime.ParseExact(txt_CollectionDt.Text, "dd/MM/yyyy", null);
                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.PavmentTestInward_Update("PT", txt_ReferenceNo.Text, 2, TestingDt, CollectionDt, txt_DtOfCasting.Text, txt_Description.Text, txt_SupplierName.Text, txt_witnessBy.Text, txt_GradeOfConcrete.Text, txt_NatureofWork.Text, "", Convert.ToInt32(txt_Qty.Text));
                    dc.ReportDetails_Update("PT", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "PT", txt_ReferenceNo.Text, "PT", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.PavmentTestInward_Update("PT", txt_ReferenceNo.Text, 3, TestingDt, CollectionDt, txt_DtOfCasting.Text, txt_Description.Text, txt_SupplierName.Text, txt_witnessBy.Text, txt_GradeOfConcrete.Text, txt_NatureofWork.Text, "", Convert.ToInt32(txt_Qty.Text));
                    dc.ReportDetails_Update("PT", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "PT", txt_ReferenceNo.Text, "PT", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "PT", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                dc.PavmentTS_Detail_Update(txt_ReferenceNo.Text, "", "", "", 0, 0, "", 0, 0, "", 0, 0, 0, "", Convert.ToInt32(txt_TestId.Text), true);

                for (int i = 0; i < grdPT_TSInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].Cells[2].FindControl("ddl_BlockType");
                    DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].Cells[3].FindControl("ddl_Thickness");
                    Button FailureThickness = (Button)grdPT_TSInward.Rows[i].Cells[4].FindControl("FailureThickness");
                    TextBox txt_FailureLoad = (TextBox)grdPT_TSInward.Rows[i].Cells[5].FindControl("txt_FailureLoad");
                    Button FailureLength = (Button)grdPT_TSInward.Rows[i].Cells[6].FindControl("FailureLength");
                    TextBox txt_CorectionFactor = (TextBox)grdPT_TSInward.Rows[i].Cells[7].FindControl("txt_CorectionFactor");
                    TextBox txt_Age = (TextBox)grdPT_TSInward.Rows[i].Cells[8].FindControl("txt_Age");
                    TextBox txt_AreaOfFailure = (TextBox)grdPT_TSInward.Rows[i].Cells[9].FindControl("txt_AreaOfFailure");
                    TextBox txt_FaliureLoadPerLen = (TextBox)grdPT_TSInward.Rows[i].Cells[10].FindControl("txt_FaliureLoadPerLen");
                    TextBox txt_TensileStrength = (TextBox)grdPT_TSInward.Rows[i].Cells[11].FindControl("txt_TensileStrength");
                    TextBox txt_AvgStr = (TextBox)grdPT_TSInward.Rows[i].Cells[12].FindControl("txt_AvgStr");
                    TextBox txt_ActualresultDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[13].FindControl("txt_ActualresultDtls");
                    TextBox txt_FaliureLenDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[14].FindControl("txt_FaliureLenDtls");

                    dc.PavmentTS_Detail_Update(txt_ReferenceNo.Text, txt_IdMark.Text, ddl_BlockType.SelectedItem.Text, ddl_Thickness.SelectedItem.Text, Convert.ToInt32(FailureThickness.Text), Convert.ToDecimal(txt_FailureLoad.Text),
                        txt_Age.Text, Convert.ToDecimal(FailureLength.Text), Convert.ToDecimal(txt_TensileStrength.Text), txt_FaliureLenDtls.Text, Convert.ToDecimal(txt_FaliureLoadPerLen.Text), Convert.ToDecimal(txt_AreaOfFailure.Text),
                         Convert.ToDecimal(txt_CorectionFactor.Text), txt_ActualresultDtls.Text, Convert.ToInt32(txt_TestId.Text), false);


                    if (txt_AvgStr.Text != "")
                    {
                        dc.PavmentTestInward_Update("PT", txt_ReferenceNo.Text, 0, TestingDt, CollectionDt, txt_DtOfCasting.Text, txt_Description.Text, txt_SupplierName.Text, txt_witnessBy.Text, txt_GradeOfConcrete.Text, txt_NatureofWork.Text, Convert.ToString(txt_AvgStr.Text), 0);
                    }
                }

                //Remark 

                int RemarkId = 0;
                dc.Pavement_Test_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(txt_TestId.Text), true);
                for (int i = 0; i < grdRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.Pavement_Test_Remark_View(txt_REMARK.Text, "", 0, Convert.ToInt32(txt_TestId.Text));
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.Pavement_Test_Remark_View("", txt_ReferenceNo.Text, 0, Convert.ToInt32(txt_TestId.Text));
                            foreach (var c in chkId)
                            {
                                if (c.RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.Pavement_Test_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(txt_TestId.Text), false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.Pavement_Test_Remark_Update(0, txt_REMARK.Text);
                            var chc = dc.Pavement_Test_Remark_View(txt_REMARK.Text, "", 0, Convert.ToInt32(txt_TestId.Text));
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.RemarkId_int);
                                dc.Pavement_Test_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(txt_TestId.Text), false);
                            }
                        }
                    }
                }

                lnkPrint.Visible = true;
                //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record Saved Successfully');", true);
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        protected void Lnk_Calculate_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
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
                for (int i = 0; i < grdPT_TSInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].Cells[2].FindControl("ddl_BlockType");
                    DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].Cells[3].FindControl("ddl_Thickness");
                    Button FailureThickness = (Button)grdPT_TSInward.Rows[i].Cells[4].FindControl("FailureThickness");
                    TextBox txt_FailureLoad = (TextBox)grdPT_TSInward.Rows[i].Cells[5].FindControl("txt_FailureLoad");
                    Button FailureLength = (Button)grdPT_TSInward.Rows[i].Cells[6].FindControl("FailureLength");

                    if (txt_IdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No. " + (i + 1) + ".";
                        txt_IdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddl_BlockType.Text == "Select")
                    {
                        lblMsg.Text = "Please Select the Block Type for Sr No. " + (i + 1) + ".";
                        ddl_BlockType.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddl_Thickness.Text == "Select")
                    {
                        lblMsg.Text = "Please Select the Thickness for Sr No. " + (i + 1) + ".";
                        ddl_Thickness.Focus();
                        valid = false;
                        break;
                    }
                    else if (FailureThickness.Text == "")
                    {
                        lblMsg.Text = "Enter Actual Thickness for Sr No. " + (i + 1) + ".";
                        FailureThickness.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_FailureLoad.Text == "")
                    {
                        lblMsg.Text = "Enter Failure Load for Sr No. " + (i + 1) + ".";
                        txt_FailureLoad.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToDecimal(txt_FailureLoad.Text) <= 0 && Convert.ToString(txt_FailureLoad.Text) != "")
                    {
                        lblMsg.Text = "Failure Load should be greater than 0 for Sr No. " + (i + 1) + ".";
                        txt_FailureLoad.Focus();
                        valid = false;
                        break;
                    }
                    else if (FailureLength.Text == "")
                    {
                        lblMsg.Text = "Enter Failure Length for Sr No. " + (i + 1) + ".";
                        FailureLength.Focus();
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
                Calculation();
            }
            return valid;


        }
        public void Calculation()
        {
            int NoOfRowsQty = 0;
            int Thickness = 0;
            decimal SumOfTensileStr = 0;
            int NoOfrows = grdPT_TSInward.Rows.Count / 2;
            for (int i = 0; i < grdPT_TSInward.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].Cells[2].FindControl("ddl_BlockType");
                DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].Cells[3].FindControl("ddl_Thickness");
                Button FailureThickness = (Button)grdPT_TSInward.Rows[i].Cells[4].FindControl("FailureThickness");
                TextBox txt_FailureLoad = (TextBox)grdPT_TSInward.Rows[i].Cells[5].FindControl("txt_FailureLoad");
                Button FailureLength = (Button)grdPT_TSInward.Rows[i].Cells[6].FindControl("FailureLength");
                TextBox txt_CorectionFactor = (TextBox)grdPT_TSInward.Rows[i].Cells[7].FindControl("txt_CorectionFactor");
                TextBox txt_Age = (TextBox)grdPT_TSInward.Rows[i].Cells[8].FindControl("txt_Age");
                TextBox txt_AreaOfFailure = (TextBox)grdPT_TSInward.Rows[i].Cells[9].FindControl("txt_AreaOfFailure");
                TextBox txt_FaliureLoadPerLen = (TextBox)grdPT_TSInward.Rows[i].Cells[10].FindControl("txt_FaliureLoadPerLen");
                TextBox txt_TensileStrength = (TextBox)grdPT_TSInward.Rows[i].Cells[11].FindControl("txt_TensileStrength");
                TextBox txt_AvgStre = (TextBox)grdPT_TSInward.Rows[i].Cells[12].FindControl("txt_AvgStr");
                TextBox txt_AvgStr = (TextBox)grdPT_TSInward.Rows[NoOfrows].Cells[12].FindControl("txt_AvgStr");
                TextBox txt_ActualresultDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[13].FindControl("txt_ActualresultDtls");
                TextBox txt_FaliureLenDtls = (TextBox)grdPT_TSInward.Rows[i].Cells[14].FindControl("txt_FaliureLenDtls");

                txt_AvgStre.Text = string.Empty;
                if (txt_DtOfCasting.Text != "NA")
                {
                    DateTime Testdt = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
                    DateTime Castdt = DateTime.ParseExact(txt_DtOfCasting.Text, "dd/MM/yyyy", null);
                    // DateTime Castdt = Convert.ToDateTime(txt_DtOfCasting.Text);
                    txt_Age.Text = Convert.ToInt32((Testdt - Castdt).TotalDays).ToString();
                }
                else
                {
                    txt_Age.Text = "NA";
                }

                string[] thicknes = Convert.ToString(ddl_Thickness.Text).Split(' ');
                foreach (var thick in thicknes)
                {
                    if (thick != "")
                    {
                        Thickness = Convert.ToInt32(thick);
                        break;
                    }
                }

                switch (Thickness)
                {
                    case 40:
                        txt_CorectionFactor.Text = "0.71";
                        break;
                    case 50:
                        txt_CorectionFactor.Text = "0.79";
                        break;
                    case 60:
                        txt_CorectionFactor.Text = "0.87";
                        break;
                    case 70:
                        txt_CorectionFactor.Text = "0.94";
                        break;
                    case 80:
                        txt_CorectionFactor.Text = "1.00";
                        break;
                    case 90:
                        txt_CorectionFactor.Text = "1.06";
                        break;
                    case 100:
                        txt_CorectionFactor.Text = "1.11";
                        break;
                    case 110:
                        txt_CorectionFactor.Text = "1.15";
                        break;
                    case 120:
                        txt_CorectionFactor.Text = "1.19";
                        break;
                    case 130:
                        txt_CorectionFactor.Text = "1.23";
                        break;
                    case 140:
                        txt_CorectionFactor.Text = "1.25";
                        break;
                    case 150:
                        txt_CorectionFactor.Text = "1.27";
                        break;
                    case 160:
                        txt_CorectionFactor.Text = "1.29";
                        break;
                    case 170:
                        txt_CorectionFactor.Text = "1.3";
                        break;
                    case 180:
                        txt_CorectionFactor.Text = "1.3";
                        break;
                }

                txt_AreaOfFailure.Text = (Convert.ToDecimal(FailureLength.Text) * Convert.ToDecimal(FailureThickness.Text)).ToString();

                txt_FaliureLoadPerLen.Text = (Convert.ToDouble(txt_FailureLoad.Text) / Convert.ToDouble(FailureLength.Text)).ToString("#");

                txt_TensileStrength.Text = (Convert.ToDouble(0.637) * Convert.ToDouble(txt_CorectionFactor.Text) * (Convert.ToDouble(txt_FailureLoad.Text) / Convert.ToDouble(txt_AreaOfFailure.Text))).ToString("0.0");

                if (txt_FaliureLoadPerLen.Text != "")
                {
                    double Remainder = Convert.ToDouble(txt_FaliureLoadPerLen.Text) % 10;
                    if (Remainder <= 5)
                    {
                        txt_FaliureLoadPerLen.Text = (Convert.ToDouble(txt_FaliureLoadPerLen.Text) - (Remainder)).ToString("#");
                    }
                    else if (Remainder <= 9)
                    {
                        txt_FaliureLoadPerLen.Text = (Convert.ToDouble(txt_FaliureLoadPerLen.Text) + (10 - Remainder)).ToString("#");
                    }
                }

                if (txt_FaliureLoadPerLen.Text == "")
                {
                    txt_FaliureLoadPerLen.Text = "0";
                }
                SumOfTensileStr += Convert.ToDecimal(txt_TensileStrength.Text);
                NoOfRowsQty++;
                if (i == grdPT_TSInward.Rows.Count - 1)
                {
                    bool flgAdd = false, flgAvailable = false;
                    int RowNo = 0;
                    if (NoOfRowsQty < 8)
                    {
                        txt_AvgStr.Text = "***";
                        //for (int j = 0; j < grdRemark.Rows.Count; j++)
                        //{
                        //    TextBox txt_REMARK = (TextBox)grdRemark.Rows[j].Cells[1].FindControl("txt_REMARK");
                        //    txt_REMARK.Text = "*** Sample constitutes of minimum eight specimen for compressive strength.";
                        //    break;
                        //}
                        flgAdd = true;
                    }
                    else
                    {
                        txt_AvgStr.Text = (SumOfTensileStr / Convert.ToInt32(NoOfRowsQty)).ToString("0.0");
                    }

                    for (int j = 0; j < grdRemark.Rows.Count; j++)
                    {
                        TextBox txtRemark = (TextBox)grdRemark.Rows[j].Cells[1].FindControl("txt_REMARK");
                        if (txtRemark.Text.Contains("Sample constitutes") == true)
                        {
                            flgAvailable = true;
                            RowNo = j;
                            break;
                        }
                    }
                    if (flgAdd == true && flgAvailable == false)//add remark
                    {
                        TextBox txtRemark = (TextBox)grdRemark.Rows[0].FindControl("txt_REMARK");
                        if (txtRemark.Text != "")
                        {
                            AddRowPT_CS_Remark();
                            txtRemark = (TextBox)grdRemark.Rows[grdRemark.Rows.Count - 1].FindControl("txt_REMARK");
                        }
                        txtRemark.Text = "*** Sample constitutes of minimum eight specimen for compressive strength.";
                    }
                    else if (flgAdd == false && flgAvailable == true)//delete remark
                    {
                        if (RowNo == 0)
                        {
                            TextBox txtRemark = (TextBox)grdRemark.Rows[0].FindControl("txt_REMARK");
                            txtRemark.Text = "";
                        }
                        else
                        {
                            DeleteRowCubeRemark(RowNo);
                        }
                    }

                }
            }
        }

        protected void FailureThickness_Click(object sender, CommandEventArgs e)
        {
            lbl_Msg.Visible = false;
            ModalPopupExtender1.Show();
            divFailureThickness.Visible = true;
            divLen.Visible = false;
            txt_T1.Focus();
            txt_T1.Text = string.Empty;
            txt_T2.Text = string.Empty;
            txt_T3.Text = string.Empty;
            Button lnk = (Button)sender;
            GridViewRow row = (GridViewRow)lnk.Parent.Parent;
            int rowindex = row.RowIndex;
            Button ActualThickneess = (Button)grdPT_TSInward.Rows[rowindex].Cells[4].FindControl("Btn_FailureThickness");
            TextBox txt_ActualresultDtls = (TextBox)grdPT_TSInward.Rows[rowindex].Cells[13].FindControl("txt_ActualresultDtls");
            rowindx = Convert.ToInt32(rowindex);
            if (txt_ActualresultDtls.Text != string.Empty)
            {
                string[] Tresult = Convert.ToString(txt_ActualresultDtls.Text).Split('|');
                foreach (var TR in Tresult)
                {
                    if (TR != "")
                    {
                        if (txt_T1.Text == string.Empty)
                        {
                            txt_T1.Text = TR.ToString();
                        }
                        else if (txt_T2.Text == string.Empty)
                        {
                            txt_T2.Text = TR.ToString();
                        }
                        else if (txt_T3.Text == string.Empty)
                        {
                            txt_T3.Text = TR.ToString();
                        }

                    }
                }

            }
        }

        protected void FailureLength_Click(object sender, CommandEventArgs e)
        {
            lbl_Msg.Visible = false;
            ModalPopupExtender1.Show();
            divLen.Visible = true;
            divFailureThickness.Visible = false;
            txt_L1.Focus();
            txt_L1.Text = string.Empty;
            txt_L2.Text = string.Empty;
            txt_L3.Text = string.Empty;
            Button lnk = (Button)sender;
            GridViewRow row = (GridViewRow)lnk.Parent.Parent;
            int rowindex = row.RowIndex;
            Button FailureLength = (Button)grdPT_TSInward.Rows[rowindex].Cells[6].FindControl("FailureLength");
            TextBox txt_FaliureLenDtls = (TextBox)grdPT_TSInward.Rows[rowindex].Cells[14].FindControl("txt_FaliureLenDtls");
           rowindx= Convert.ToInt32(rowindex);
            if (txt_FaliureLenDtls.Text != string.Empty)
            {
                string[] Tresult = Convert.ToString(txt_FaliureLenDtls.Text).Split('|');
                foreach (var LR in Tresult)
                {
                    if (LR != "")
                    {
                        if (txt_L1.Text == string.Empty)
                        {
                            txt_L1.Text = LR.ToString();
                        }
                        else if (txt_L2.Text == string.Empty)
                        {
                            txt_L2.Text = LR.ToString();
                        }
                        else if (txt_L3.Text == string.Empty)
                        {
                            txt_L3.Text = LR.ToString();
                        }
                    }
                }
            }
        }

        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            if (divFailureThickness.Visible == true)
            {
                if (ValidateThickness() == true)
                {
                    TextBox txt_ActualresultDtls = (TextBox)grdPT_TSInward.Rows[Convert.ToInt32(rowindx)].Cells[13].FindControl("txt_ActualresultDtls");
                    txt_ActualresultDtls.Text = string.Empty;
                    txt_ActualresultDtls.Text = txt_ActualresultDtls.Text + txt_T1.Text + "|" + txt_T2.Text + "|" + txt_T3.Text;
                    Button ActualThickneess = (Button)grdPT_TSInward.Rows[Convert.ToInt32(rowindx)].Cells[4].FindControl("FailureThickness");
                    ActualThickneess.Text = ((Convert.ToInt32(txt_T1.Text) + Convert.ToInt32(txt_T2.Text) + Convert.ToInt32(txt_T3.Text)) / 3).ToString();
                    ModalPopupExtender1.Hide();
                }
            }
            else if (divLen.Visible == true)
            {
                if (ValidateFaliureLen() == true)
                {
                    TextBox txt_FaliureLenDtls = (TextBox)grdPT_TSInward.Rows[Convert.ToInt32(rowindx)].Cells[14].FindControl("txt_FaliureLenDtls");
                    txt_FaliureLenDtls.Text = string.Empty;
                    txt_FaliureLenDtls.Text = txt_FaliureLenDtls.Text + txt_L1.Text + "|" + txt_L2.Text + "|" + txt_L3.Text;
                    Button FailureLength = (Button)grdPT_TSInward.Rows[Convert.ToInt32(rowindx)].Cells[6].FindControl("FailureLength");
                    FailureLength.Text = ((Convert.ToInt32(txt_L1.Text) + Convert.ToInt32(txt_L2.Text) + Convert.ToInt32(txt_L3.Text)) / 3).ToString();
                    ModalPopupExtender1.Hide();
                }
            }
        }
        protected Boolean ValidateFaliureLen()
        {
            //  string dispalyMsg = "";
            Boolean valid = true;
            if (txt_L1.Text == string.Empty)
            {
                lbl_Msg.Text = "Enter L1 value";
                txt_L1.Focus();
                valid = false;
            }
            else if (txt_L2.Text == string.Empty)
            {
                lbl_Msg.Text = "Enter L2 value";
                txt_L2.Focus();
                valid = false;
            }
            else if (txt_L3.Text == string.Empty)
            {
                lbl_Msg.Text = "Enter L3 value";
                txt_L3.Focus();
                valid = false;
            }
            if (valid == false)
            {
                lbl_Msg.Visible = true;
                //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lbl_Msg.Visible = false;
            }
            return valid;
        }
        protected Boolean ValidateThickness()
        {
            //   string dispalyMsg = "";
            Boolean valid = true;
            if (txt_T1.Text == string.Empty)
            {
                lbl_Msg.Text = "Enter T1 value";
                txt_T1.Focus();
                valid = false;
            }
            else if (txt_T2.Text == string.Empty)
            {
                lbl_Msg.Text = "Enter T2 value";
                txt_T2.Focus();
                valid = false;
            }
            else if (txt_T3.Text == string.Empty)
            {
                lbl_Msg.Text = "Enter T3 value";
                txt_T3.Focus();
                valid = false;
            }

            if (valid == false)
            {
                lbl_Msg.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lbl_Msg.Visible = false;
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
                DisplayPavement_TS_Details();
                //LoadOtherPendingCheckRpt();
                LoadReferenceNoList();
                LoadApproveBy();
                ViewWitnessBy();
                if (grdPT_TSInward.Rows.Count > 0)
                {
                    grdPT_TSInward.DataSource = null;
                    grdPT_TSInward.DataBind();
                }
                //DisplayGridRow();
                DisplayPavment_TSGrid();
                DisplayIdMark();
                if (grdRemark.Rows.Count > 0)
                {
                    grdRemark.DataSource = null;
                    grdRemark.DataBind();
                }
                DisplayRemark();                
            }

        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.Pavement_TS_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            // RptPavement_TS_Testing();
        }
        public void RptPavement_TS_Testing()
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = getDetailReportPT_TS();
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.Pavement_TS_Report_Html(txt_ReferenceNo.Text, Convert.ToInt32(txt_TestId.Text), lblEntry.Text);
        }

        //protected string getDetailReportPT_TS()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Splitting Tensile Strength </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


        //    var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);

        //    foreach (var pavmt in pt)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
        //           "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
        //           "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table cell=9>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<th width= 2%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></th>";
        //    mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></th>";
        //    mySql += "<th width= 5%  rowspan=2   align=center valign=top height=19 ><font size=2><b>Age </br></br> </br> (Days)</b></font></th>";
        //    mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Thickness </br></br></br> (mm) </b></font></th>";
        //    mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Failure Load </br></br></br> (N) </b></font></th>";
        //    mySql += "<th width= 10% align=center  colspan=2  valign=top height=19 ><font size=2><b>Mean Failure </br></br> </b></font></th>";
        //    mySql += "<th width= 10% align=center valign=top  rowspan=2  height=19 ><font size=2><b>Failure Load per Unit Length </br> (N/mm)</b></font></th>";
        //    mySql += "<th width= 10% align=center valign=top rowspan=2  height=19 ><font size=2><b>Splitting Tensile Strength (N/mm <sup>2</sup>)</b></font></th>";
        //    mySql += "<th width= 10% align=center valign=top rowspan=2  height=19 ><font size=2><b>Average </br></br> </br>(N/mm <sup>2</sup>)</b></font></t>";
        //    mySql += "</tr>";
        //    mySql += "<th width= 10%   align=center valign=top height=19 ><font size=2><b>Length </br> (mm)</b></font></th>";
        //    mySql += "<th width= 10%   align=center valign=top height=19 ><font size=2><b>Thickness </br> (mm)</b></font></th>";

        //    int SrNo = 0;
        //    int i = 0;
        //    var PT_TS = dc.Pavement_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32( txt_TestId.Text ), "TS");
        //    var count = PT_TS.Count();
        //    var PT_TensileStr = dc.Pavement_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32( txt_TestId.Text ), "TS");
        //    foreach (var ptts in PT_TensileStr)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.IdMark_var) + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.Age_var) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.Thickness_var).Replace("mm", "") + "</font></td>";
        //        mySql += "<td width= 10%  align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLoad_num) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLength_num) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureThickness_int) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLoadPerUnitLen_num) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.TensileStrength_dec) + "</font></td>";

        //        if (i == 0)
        //        {
        //            mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.PTINWD_AvgStr_var) + "</font></td>";
        //        }
        //        i++;
        //        mySql += "</tr>";
        //    }

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("PT", "");
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
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }

        //    }

        //    SrNo = 0;
        //    var re = dc.Pavement_Test_Remark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, Convert.ToInt32( txt_TestId.Text ));
        //    foreach (var r in re)
        //    {
        //        var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32( txt_TestId.Text ));
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
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
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
        //        var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
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
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
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

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            #region TS
            mySql = @"select ref.reference_number,x.*,head.[description],head.[grade_of_concrete],head.[date_of_testing] "+
                        " ,head.[image],head.[witness_by],head.[date_of_casting],head.[id_mark], "+
                        " ts.id_mark ,ts.block_type,ts.thickness,ts.failure_thickness,ts.failure_length ,ts.failure_load " +
                        " from [new_gt_app_db].[dbo].[reference_number] ref, [new_gt_app_db].[dbo].[category_refno_wise_test] x, [new_gt_app_db].[dbo].[category_header] head, " +
                        " [new_gt_app_db].[dbo].pt_splitting_tesile ts " +
                        " where ref.reference_number ='" + txt_ReferenceNo.Text + "'" +
                        " and ref.pk_id = x.reference_number_fk_id and x.category_fk_id = head.category_fk_id " +
                        " and x.category_wise_test_fk_id = head.test_fk_id and x.reference_number_fk_id = head.reference_no  "+
                        " and x.category_fk_id = 4 and x.category_wise_test_fk_id = 9 "+
                        " and head.pk_id = ts.category_header_fk_id  ";

            dt = objcls.getGeneralData(mySql);
            if (dt.Rows.Count > 0)
            {
                txt_TestingDt.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                txt_Description.Text = dt.Rows[0]["description"].ToString();
                txt_DtOfCasting.Text = dt.Rows[0]["date_of_casting"].ToString();

                if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                {
                    txt_witnessBy.Text = dt.Rows[0]["witness_by"].ToString();
                    chk_WitnessBy.Checked = true;
                    txt_witnessBy.Visible = true;
                }
                txt_Qty.Text = dt.Rows.Count.ToString();
                if (Convert.ToInt32(txt_Qty.Text) > 0)
                {
                    if (Convert.ToInt32(txt_Qty.Text) < grdPT_TSInward.Rows.Count)
                    {
                        for (int i = grdPT_TSInward.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                        {
                            if (grdPT_TSInward.Rows.Count > 1)
                            {
                                DeleteDataPT_TSInward(i - 1);
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
                    TextBox txt_IdMark = (TextBox)grdPT_TSInward.Rows[i].FindControl("txt_IdMark");
                    DropDownList ddl_BlockType = (DropDownList)grdPT_TSInward.Rows[i].FindControl("ddl_BlockType");
                    DropDownList ddl_Thickness = (DropDownList)grdPT_TSInward.Rows[i].FindControl("ddl_Thickness");
                    Button FailureThickness = (Button)grdPT_TSInward.Rows[i].FindControl("FailureThickness");
                    Button FailureLength = (Button)grdPT_TSInward.Rows[i].FindControl("FailureLength");
                    TextBox txt_FailureLoad = (TextBox)grdPT_TSInward.Rows[i].FindControl("txt_FailureLoad");
                    

                    txt_IdMark.Text = dt.Rows[i]["Id_mark"].ToString();
                    ddl_BlockType.Text = dt.Rows[i]["block_type"].ToString();
                    ddl_Thickness.Text = dt.Rows[i]["thickness"].ToString();
                    FailureThickness.Text = dt.Rows[i]["failure_thickness"].ToString();
                    FailureLength.Text = dt.Rows[i]["failure_length"].ToString();                    
                    txt_FailureLoad.Text = dt.Rows[i]["failure_load"].ToString();

                    
                }

            }
            dt.Dispose();
            #endregion

            objcls = null;
        }
    }
}