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
    public partial class Pavement_Report_WA : System.Web.UI.Page
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
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Pavement Water Absorption - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_RecType.Text != "")
                {
                    DisplayPavementWA_Details();
                    DisplayPavment_WAGrid();
                    DisplayRemark();

                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "Pavement Water Absorption - Report Check";
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
        public void DisplayPavementWA_Details()
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
            var re = dc.Pavement_Test_Remark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, Convert.ToInt32( txt_TestId.Text ));
            foreach (var r in re)
            {
                AddRowPT_WA_Remark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32( txt_TestId.Text ));
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.Remark_var.ToString();
                    i++;
                }
            }
            if (grdRemark.Rows.Count <= 0)
            {
                AddRowPT_WA_Remark();
            }

        }
        public void DisplayPavment_WAGrid()
        {
            int i = 0;
            string AvgWaterAbsorption = "";
            var PT_WA = dc.Pavement_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32( txt_TestId.Text ), "WA");
            foreach (var pt in PT_WA)
            {
                AddRowEnterReportPT_WAInward();
                TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Dryweight = (TextBox)grdPT_WAInward.Rows[i].Cells[2].FindControl("txt_Dryweight");
                TextBox txt_WetWeight = (TextBox)grdPT_WAInward.Rows[i].Cells[3].FindControl("txt_WetWeight");
                TextBox txt_WaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[4].FindControl("txt_WaterAbsorption");

                if (i == 0)
                {
                    AvgWaterAbsorption = Convert.ToString(pt.PTINWD_AvgStr_var);
                }
                txt_IdMark.Text = pt.IdMark_var.ToString();
                txt_Dryweight.Text = pt.DryWeight_int.ToString();
                txt_WetWeight.Text = pt.Wet_Weight_int.ToString();
                txt_WaterAbsorption.Text = pt.WaterAbsorption_dec.ToString();
                i++;
            }

            if (grdPT_WAInward.Rows.Count > 0)
            {
                int NoOfrows = grdPT_WAInward.Rows.Count / 2;
                TextBox txt_AvgWaterAbsorption = (TextBox)grdPT_WAInward.Rows[NoOfrows].Cells[5].FindControl("txt_AvgWaterAbsorption");
                txt_AvgWaterAbsorption.Text = Convert.ToString(AvgWaterAbsorption);
            }
            if (grdPT_WAInward.Rows.Count <= 0)
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
                for (int i = 0; i < grdPT_WAInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    txt_IdMark.Text = pt.PTINWD_Id_Mark_var.ToString();
                }
            }
        }
        public void DisplayGridRow()
        {
            if (Convert.ToInt32(txt_Qty.Text) > 0)
            {
                if (Convert.ToInt32(txt_Qty.Text) > grdPT_WAInward.Rows.Count)
                {
                    for (int i = grdPT_WAInward.Rows.Count + 1; i <= Convert.ToInt32(txt_Qty.Text); i++)
                    {
                        AddRowEnterReportPT_WAInward();
                    }
                }                
            }
        }

        protected void AddRowEnterReportPT_WAInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PT_WA_DetailTable"] != null)
            {
                GetCurrentDataPT_WAInward();
                dt = (DataTable)ViewState["PT_WA_DetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Dryweight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WetWeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WaterAbsorption", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AvgWaterAbsorption", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_IdMark"] = string.Empty;
            dr["txt_Dryweight"] = string.Empty;
            dr["txt_WetWeight"] = string.Empty;
            dr["txt_WaterAbsorption"] = string.Empty;
            dr["txt_AvgWaterAbsorption"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["PT_WA_DetailTable"] = dt;
            grdPT_WAInward.DataSource = dt;
            grdPT_WAInward.DataBind();
            SetPreviousDataPT_WAInward();
        }
        protected void GetCurrentDataPT_WAInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Dryweight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WetWeight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WaterAbsorption", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgWaterAbsorption", typeof(string)));

            for (int i = 0; i < grdPT_WAInward.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Dryweight = (TextBox)grdPT_WAInward.Rows[i].Cells[2].FindControl("txt_Dryweight");
                TextBox txt_WetWeight = (TextBox)grdPT_WAInward.Rows[i].Cells[3].FindControl("txt_WetWeight");
                TextBox txt_WaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[4].FindControl("txt_WaterAbsorption");
                TextBox txt_AvgWaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[5].FindControl("txt_AvgWaterAbsorption");

                drRow = dtTable.NewRow();
                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txt_Dryweight"] = txt_Dryweight.Text;
                drRow["txt_WetWeight"] = txt_WetWeight.Text;
                drRow["txt_WaterAbsorption"] = txt_WaterAbsorption.Text;
                drRow["txt_AvgWaterAbsorption"] = txt_AvgWaterAbsorption.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["PT_WA_DetailTable"] = dtTable;

        }
        protected void SetPreviousDataPT_WAInward()
        {
            DataTable dt = (DataTable)ViewState["PT_WA_DetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Dryweight = (TextBox)grdPT_WAInward.Rows[i].Cells[2].FindControl("txt_Dryweight");
                TextBox txt_WetWeight = (TextBox)grdPT_WAInward.Rows[i].Cells[3].FindControl("txt_WetWeight");
                TextBox txt_WaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[4].FindControl("txt_WaterAbsorption");
                TextBox txt_AvgWaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[5].FindControl("txt_AvgWaterAbsorption");

                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txt_Dryweight.Text = dt.Rows[i]["txt_Dryweight"].ToString();
                txt_WetWeight.Text = dt.Rows[i]["txt_WetWeight"].ToString();
                txt_WaterAbsorption.Text = dt.Rows[i]["txt_WaterAbsorption"].ToString();
                txt_AvgWaterAbsorption.Text = dt.Rows[i]["txt_AvgWaterAbsorption"].ToString();
            }

        }
        protected void DeleteDataPT_WAInward(int rowIndex)
        {
            GetCurrentDataPT_WAInward();
            DataTable dt = ViewState["PT_WA_DetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PT_WA_DetailTable"] = dt;
            grdPT_WAInward.DataSource = dt;
            grdPT_WAInward.DataBind();
            SetPreviousDataPT_WAInward();
        }
        protected void txt_QtyOnTextChanged(object sender, EventArgs e)
        {
            int qty = 0;
            if (int.TryParse(txt_Qty.Text, out qty))
            {
                if (Convert.ToInt32(txt_Qty.Text) > 0)
                {
                    if (Convert.ToInt32(txt_Qty.Text) < grdPT_WAInward.Rows.Count)
                    {
                        for (int i = grdPT_WAInward.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                        {
                            if (grdPT_WAInward.Rows.Count > 1)
                            {
                                DeleteDataPT_WAInward(i - 1);
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
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowPT_WA_Remark();
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
            GetCurrentDataPT_WA_Remark();
            DataTable dt = ViewState["PavementRemark_WA_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PavementRemark_WA_Table"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataPT_WA_Remark();
        }
        protected void AddRowPT_WA_Remark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PavementRemark_WA_Table"] != null)
            {
                GetCurrentDataPT_WA_Remark();
                dt = (DataTable)ViewState["PavementRemark_WA_Table"];
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

            ViewState["PavementRemark_WA_Table"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataPT_WA_Remark();
        }
        protected void GetCurrentDataPT_WA_Remark()
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
            ViewState["PavementRemark_WA_Table"] = dtTable;

        }
        protected void SetPreviousDataPT_WA_Remark()
        {
            DataTable dt = (DataTable)ViewState["PavementRemark_WA_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        protected void Lnk_Calculate_Click(object sender, EventArgs e)
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


                dc.PavmentWA_TestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, Convert.ToInt32(txt_TestId.Text), true);
                for (int i = 0; i < grdPT_WAInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Dryweight = (TextBox)grdPT_WAInward.Rows[i].Cells[2].FindControl("txt_Dryweight");
                    TextBox txt_WetWeight = (TextBox)grdPT_WAInward.Rows[i].Cells[3].FindControl("txt_WetWeight");
                    TextBox txt_WaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[4].FindControl("txt_WaterAbsorption");
                    TextBox txt_AvgWaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[5].FindControl("txt_AvgWaterAbsorption");

                    dc.PavmentWA_TestDetail_Update(txt_ReferenceNo.Text, txt_IdMark.Text, Convert.ToInt32(txt_Dryweight.Text), Convert.ToInt32(txt_WetWeight.Text), Convert.ToDecimal(txt_WaterAbsorption.Text), Convert.ToInt32(txt_TestId.Text), false);

                    if (txt_AvgWaterAbsorption.Text != "")
                    {
                        dc.PavmentTestInward_Update("PT", txt_ReferenceNo.Text, 0, TestingDt, CollectionDt, txt_DtOfCasting.Text, txt_Description.Text, txt_SupplierName.Text, txt_witnessBy.Text, txt_GradeOfConcrete.Text, txt_NatureofWork.Text, Convert.ToString(txt_AvgWaterAbsorption.Text), 0);
                    }
                }
                //Remark 
                int RemarkId = 0;
                dc.Pavement_Test_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32( txt_TestId.Text ), true);
                for (int i = 0; i < grdRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.Pavement_Test_Remark_View(txt_REMARK.Text, "", 0, Convert.ToInt32( txt_TestId.Text ));
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.Pavement_Test_Remark_View("", txt_ReferenceNo.Text, 0, Convert.ToInt32( txt_TestId.Text ));
                            foreach (var c in chkId)
                            {
                                if (c.RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.Pavement_Test_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32( txt_TestId.Text ), false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.Pavement_Test_Remark_Update(0, txt_REMARK.Text);
                            var chc = dc.Pavement_Test_Remark_View(txt_REMARK.Text, "", 0, Convert.ToInt32( txt_TestId.Text ));
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.RemarkId_int);
                                dc.Pavement_Test_Remark_Detail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32( txt_TestId.Text ), false);
                            }
                        }
                    }
                }

                lnkPrint.Visible = true;
                //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record Saved Successfully');", true);
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
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
                for (int i = 0; i < grdPT_WAInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Dryweight = (TextBox)grdPT_WAInward.Rows[i].Cells[2].FindControl("txt_Dryweight");
                    TextBox txt_WetWeight = (TextBox)grdPT_WAInward.Rows[i].Cells[3].FindControl("txt_WetWeight");

                    if (txt_IdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No. " + (i + 1) + ".";
                        txt_IdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Dryweight.Text == "")
                    {
                        lblMsg.Text = "Enter Dry Weight for Sr No. " + (i + 1) + ".";
                        txt_Dryweight.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToDecimal(txt_Dryweight.Text) <= 0 && Convert.ToString(txt_Dryweight.Text) != "")
                    {
                        lblMsg.Text = "Dry Weight should be greater than 0 for Sr No. " + (i + 1) + ".";
                        txt_Dryweight.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_WetWeight.Text == "")
                    {
                        lblMsg.Text = "Enter Wet Weight for Sr No. " + (i + 1) + ".";
                        txt_WetWeight.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToInt32(txt_WetWeight.Text) < Convert.ToInt32(txt_Dryweight.Text))
                    {
                        lblMsg.Text = "Wet Weight should be greater than the Dry Weight for Sr No. " + (i + 1) + ".";
                        txt_WetWeight.Focus();
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
            int NoOfRowsQty = 0;
            decimal AvgWaterAbsorption = 0;
            int NoOfrows = grdPT_WAInward.Rows.Count / 2;
            for (int i = 0; i < grdPT_WAInward.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Dryweight = (TextBox)grdPT_WAInward.Rows[i].Cells[2].FindControl("txt_Dryweight");
                TextBox txt_WetWeight = (TextBox)grdPT_WAInward.Rows[i].Cells[3].FindControl("txt_WetWeight");
                TextBox txt_WaterAbsorption = (TextBox)grdPT_WAInward.Rows[i].Cells[4].FindControl("txt_WaterAbsorption");
                TextBox txt_AvgWaterAbsorption = (TextBox)grdPT_WAInward.Rows[NoOfrows].Cells[5].FindControl("txt_AvgWaterAbsorption");
                TextBox txt_AvgWater = (TextBox)grdPT_WAInward.Rows[i].Cells[5].FindControl("txt_AvgWaterAbsorption");

                txt_AvgWater.Text = string.Empty;
                txt_WaterAbsorption.Text = (((Convert.ToDecimal(txt_WetWeight.Text) - Convert.ToDecimal(txt_Dryweight.Text)) * 100) / Convert.ToDecimal(txt_Dryweight.Text)).ToString("0.00");
                AvgWaterAbsorption += Convert.ToDecimal(txt_WaterAbsorption.Text);
                NoOfRowsQty++;
                if (i == grdPT_WAInward.Rows.Count - 1)
                {
                    txt_AvgWaterAbsorption.Text = (AvgWaterAbsorption / Convert.ToInt32(NoOfRowsQty)).ToString("0.00");
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
                lnkSave.Enabled = true;
                lnkPrint.Visible = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                DisplayPavementWA_Details();
                //LoadOtherPendingCheckRpt();
                LoadReferenceNoList();
                LoadApproveBy();
                ViewWitnessBy();
                if (grdPT_WAInward.Rows.Count > 0)
                {
                    grdPT_WAInward.DataSource = null;
                    grdPT_WAInward.DataBind();
                }
                //DisplayGridRow();
                DisplayPavment_WAGrid();
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
                //rpt.Pavement_WA_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            // RptPavement_WA_Testing();
        }
        public void RptPavement_WA_Testing()
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = getDetailReportPT_WA();
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.Pavement_WA_Report_Html(txt_ReferenceNo.Text, Convert.ToInt32(txt_TestId.Text), lblEntry.Text);

        }
        //protected string getDetailReportPT_WA()
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
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Water Absorption </b></font></td></tr>";

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
        //            "<td height=19><font size=2>" + DateTime.Now.ToString("dd/MMM/yy") + "</font></td>" +
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


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Dry Weight </br> (g)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Wet Weight </br>  (g) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Water Absorption </br>  (%)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Average </br>  (%)</b></font></td>";
        //    mySql += "</tr>";

        //    int SrNo = 0;
        //    int i = 0;
        //    var PT_WA = dc.Pavement_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32( txt_TestId.Text ), "WA");
        //    var count = PT_WA.Count();
        //    var PT_WaterAbsorp = dc.Pavement_Test_View(Convert.ToString(txt_ReferenceNo.Text), Convert.ToInt32( txt_TestId.Text ), "WA");
        //    foreach (var ptwa in PT_WaterAbsorp)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.IdMark_var) + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.DryWeight_int) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.Wet_Weight_int) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.WaterAbsorption_dec) + "</font></td>";
        //        if (i == 0)
        //        {
        //            mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.PTINWD_AvgStr_var) + "</font></td>";
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
            #region WA
            mySql = @"select ref.reference_number,x.*,head.[description],head.[grade_of_concrete],head.[date_of_testing]" +
                ",head.[image],head.[witness_by],head.[date_of_casting],head.[id_mark],ptw.id_mark,ptw.dry_weight,ptw.wet_weight" +
                " from [new_gt_app_db].[dbo].[reference_number] ref,[new_gt_app_db].[dbo].[category_refno_wise_test] x, [new_gt_app_db].[dbo].[category_header] head," +
                " [new_gt_app_db].[dbo].[pt_water] ptw where ref.reference_number = '" + txt_ReferenceNo.Text + "'" +
                " and ref.pk_id = x.reference_number_fk_id and x.category_fk_id = head.category_fk_id " +
                " and x.category_wise_test_fk_id = head.test_fk_id and x.reference_number_fk_id = head.reference_no " +
                " and x.category_fk_id = 4 and x.category_wise_test_fk_id = 10 " +
                " and head.pk_id = ptw.category_header_fk_id ";

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
                    if (Convert.ToInt32(txt_Qty.Text) < grdPT_WAInward.Rows.Count)
                    {
                        for (int i = grdPT_WAInward.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                        {
                            if (grdPT_WAInward.Rows.Count > 1)
                            {
                                DeleteDataPT_WAInward(i - 1);
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
                    TextBox txt_IdMark = (TextBox)grdPT_WAInward.Rows[i].FindControl("txt_IdMark");
                    
                    TextBox txt_Dryweight = (TextBox)grdPT_WAInward.Rows[i].FindControl("txt_Dryweight");
                    TextBox txt_WetWeight = (TextBox)grdPT_WAInward.Rows[i].FindControl("txt_WetWeight");

                    txt_IdMark.Text = dt.Rows[i]["Id_mark"].ToString();
                    //,ptw.
                    txt_Dryweight.Text = dt.Rows[i]["dry_weight"].ToString();
                    txt_WetWeight.Text = dt.Rows[i]["wet_weight"].ToString();
                }

            }
            dt.Dispose();
            #endregion

            objcls = null;
        }


    }
}