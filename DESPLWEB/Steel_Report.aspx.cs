using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Steel_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        static int Qty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                #region decrypt parametres
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
                    txt_ST.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEntry.Text = arrIndMsg[1].ToString().Trim();
                }
                #endregion
                
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblEntry.Text == "Check")
                {
                    lblheading.Text = "Steel - Report Check";
                    lbl_TestedBy.Text = "Approve By";
                }
                else
                {
                    lblheading.Text = "Steel - Report Entry";
                }
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_ST.Text != "")
                {
                    txt_DateOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    Rdn_LoadKg.Checked = true;
                    DisplaySteelDetails();
                    LoadReferenceNoList();
                    LoadApprovedBy();
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

            var reportList = dc.ReferenceNo_View_StatusWise("ST", reportStatus,0);
            ddl_SubReports.DataTextField = "ReferenceNo";
            ddl_SubReports.DataSource = reportList;
            ddl_SubReports.DataBind();
            ddl_SubReports.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_SubReports.Items.Remove(txt_ReferenceNo.Text);
        }

        private void LoadApprovedBy()
        {
            byte testBit = 0;
            byte apprBit = 0;
            if (lblEntry.Text == "Enter")
            {
                testBit = 1;
                apprBit = 0;
            }
            else if (lblEntry.Text == "Check")
            {
                testBit = 0;
                apprBit = 1;
            }

            ddl_TestedBy.DataTextField = "USER_Name_var";
            ddl_TestedBy.DataValueField = "USER_Id";
            var apprUser = dc.ReportStatus_View("", null, null, 0, testBit, apprBit, "", 0, 0, 0);
            ddl_TestedBy.DataSource = apprUser;
            ddl_TestedBy.DataBind();
            ddl_TestedBy.Items.Insert(0, "---Select---");
        }

        public void DisplaySteelDetails()
        {
            ddl_ComplainceNote.ClearSelection();
          //  ddl_ComplainceNote.SelectedValue = "2";
            var stInwd = dc.ReportStatus_View("Steel Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text.ToString(), 0, 0, 0);
            foreach (var st in stInwd)
            {
                lblRecordNo.Text = st.INWD_RecordNo_int.ToString(); 
                txt_ReferenceNo.Text = st.STINWD_ReferenceNo_var.ToString();
                txt_ReportNo.Text = st.STINWD_SetOfRecord_var.ToString();
                if (st.STINWD_TestedDate_dt != null && st.STINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(st.STINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_typeofSteel.Text = st.STINWD_SteelType_var.ToString();
                txt_Description.Text = st.STINWD_Description_var.ToString();
                txt_Supplier.Text = st.STINWD_SupplierName_var.ToString();
                //txt_GradeSteel.Text = "Fe " + "" + st.STINWD_Grade_var.ToString();
                txt_GradeSteel.Text = st.STINWD_Grade_var.ToString();
                
                if (ddl_NablScope.Items.FindByValue(st.STINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(st.STINWD_NablScope_var);
                }
                if (Convert.ToString(st.STINWD_NablLocation_int) != null && Convert.ToString(st.STINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(st.STINWD_NablLocation_int);
                }

                Qty = Convert.ToInt32(st.STINWD_Quantity_tint);                
                if (st.STINWD_ComplianceNote_var != null && st.STINWD_ComplianceNote_var != "")
                {
                    ddl_ComplainceNote.ClearSelection();
                    ddl_ComplainceNote.Items.FindByText(st.STINWD_ComplianceNote_var).Selected = true;
                }
                if (st.STINWD_LoadKgKn_var != null && st.STINWD_LoadKgKn_var.ToString() != "")
                {
                    if (st.STINWD_LoadKgKn_var == "Kg")
                    {
                        Rdn_LoadKg.Checked = true;
                    }
                    if (st.STINWD_LoadKgKn_var == "Kn")
                    {
                        Rdn_LoadKn.Checked = true;
                    }
                }

            }
            if (Qty > 0)
            {
                if (Qty > grdSteelEntry.Rows.Count)
                {
                    for (int i = grdSteelEntry.Rows.Count + 1; i <= Qty; i++)
                    {
                        AddRowSteelEntry();
                        AddRowSteelCalculated();
                        DisplayChkTestName();
                        DisplayCalCulatedGrid();
                    }
                }
            }
            var InwardST = dc.ReportStatus_View("Steel Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text.ToString(), 0, 0, 0);
            foreach (var st in InwardST)
            {
                for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
                {
                    TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                    TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                    txt_IdMark.Text = st.STINWD_IdMark_var.ToString();
                    txt_Dia.Text = st.STINWD_Diameter_tint.ToString();
                }
            }
            if (lblEntry.Text == "Check")
            {
                DisplayDetailSteelGrid();
            }
            //load witness by
            txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ST = dc.ReportStatus_View("Steel Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text.ToString(), 0, 0, 0);
                foreach (var c in ST)
                {
                    if (c.STINWD_WitnessBy_var.ToString() != null && c.STINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.STINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }
            //
            DisplayRemark();
        }

        public void DisplayDetailSteelGrid()
        {
            int i = 0;
            int j = 0;
            grdSteelEntry.DataSource = null;
            grdSteelEntry.DataBind();
            grdSteelCalulated.DataSource = null;
            grdSteelCalulated.DataBind();
            decimal AvgWtMeter = 0;
            var details = dc.SteelDetailInward_Update(txt_ReferenceNo.Text, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, "","", 0, 0, "", "", "", "", "", "", false, true, false);
            foreach (var st in details)
            {
                AddRowSteelEntry();
                TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txt_Weight = (TextBox)grdSteelEntry.Rows[i].Cells[3].FindControl("txt_Weight");
                TextBox txt_Length = (TextBox)grdSteelEntry.Rows[i].Cells[4].FindControl("txt_Length");
                TextBox txt_FinalLength = (TextBox)grdSteelEntry.Rows[i].Cells[5].FindControl("txt_FinalLength");
                TextBox txt_YieldLoad = (TextBox)grdSteelEntry.Rows[i].Cells[6].FindControl("txt_YieldLoad");
                TextBox txt_UltiMateLoad = (TextBox)grdSteelEntry.Rows[i].Cells[7].FindControl("txt_UltiMateLoad");
                DropDownList ddl_Rebend = (DropDownList)grdSteelEntry.Rows[i].Cells[8].FindControl("ddl_Rebend");
                DropDownList ddl_Bend = (DropDownList)grdSteelEntry.Rows[i].Cells[9].FindControl("ddl_Bend");

                Label lblGaugeLength = (Label)grdSteelEntry.Rows[i].FindControl("lblGaugeLength");
                Label lblWtMeterImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblWtMeterImage1");
                Label lblTensileImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage1");
                Label lblTensileImage2 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage2");
                Label lblTensileImage3 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage3");
                Label lblBendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblBendImage1");
                Label lblRebendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblRebendImage1");

                AddRowSteelCalculated();
                TextBox txt_IdMark2 = (TextBox)grdSteelCalulated.Rows[j].Cells[1].FindControl("txt_IdMark");
                TextBox txt_CsArea = (TextBox)grdSteelCalulated.Rows[j].Cells[2].FindControl("txt_CsArea");
                TextBox txt_WtMeter = (TextBox)grdSteelCalulated.Rows[j].Cells[3].FindControl("txt_WtMeter");

                TextBox txt_Elongation = (TextBox)grdSteelCalulated.Rows[j].Cells[5].FindControl("txt_Elongation");
                TextBox txt_YieldStress = (TextBox)grdSteelCalulated.Rows[j].Cells[6].FindControl("txt_YieldStress");
                TextBox txt_UltimateStress = (TextBox)grdSteelCalulated.Rows[j].Cells[7].FindControl("txt_UltimateStress");

                txt_Dia.Text = st.STINWD_Diameter_tint.ToString();
                //txt_IdMark.Text = st.STINWD_IdMark_var.ToString();

                txt_IdMark.Text = st.STDETAIL_IdMark_var.ToString();

                if (st.STDETAIL_weight_dec != null && st.STDETAIL_weight_dec.ToString() != "")
                {
                    txt_Weight.Text = st.STDETAIL_weight_dec.ToString();
                }
                if (st.STDETAIL_Length_dec != null && st.STDETAIL_Length_dec.ToString() != "")
                {
                    txt_Length.Text = st.STDETAIL_Length_dec.ToString();
                }
                if (st.STDETAIL_FinalLen_dec != null && st.STDETAIL_FinalLen_dec.ToString() != "")
                {
                    txt_FinalLength.Text = st.STDETAIL_FinalLen_dec.ToString();
                }
                if (st.STDETAIL_YieldLoad_dec != null && st.STDETAIL_YieldLoad_dec.ToString() != "")
                {
                    txt_YieldLoad.Text = st.STDETAIL_YieldLoad_dec.ToString();
                }
                if (st.STDETAIL_UltimateLoad_dec != null && st.STDETAIL_UltimateLoad_dec.ToString() != "")
                {
                    txt_UltiMateLoad.Text = st.STDETAIL_UltimateLoad_dec.ToString();
                }
                if (st.STDETAIL_Bend_var != null && st.STDETAIL_Bend_var.ToString() != "")
                {
                    ddl_Bend.SelectedItem.Text = st.STDETAIL_Bend_var.ToString();
                }
                if (st.STDETAIL_Rebend_var != null && st.STDETAIL_Rebend_var.ToString() != "")
                {
                    ddl_Rebend.SelectedItem.Text = st.STDETAIL_Rebend_var.ToString();
                }
                if (st.STDETAIL_IdMark_var != null && st.STDETAIL_IdMark_var.ToString() != "")
                {
                    txt_IdMark2.Text = st.STDETAIL_IdMark_var.ToString();
                }
                if (st.STDETAIL_CSArea_dec != null && st.STDETAIL_CSArea_dec.ToString() != "")
                {
                    txt_CsArea.Text = st.STDETAIL_CSArea_dec.ToString();
                }
                if (st.STDETAIL_WtMeter_dec != null && st.STDETAIL_WtMeter_dec.ToString() != "")
                {
                    txt_WtMeter.Text = st.STDETAIL_WtMeter_dec.ToString();
                }
                if (st.STINWD_AvgWtMeter_dec != null && st.STINWD_AvgWtMeter_dec.ToString() != "")
                {
                    AvgWtMeter = Convert.ToDecimal(st.STINWD_AvgWtMeter_dec);
                }
                if (st.STDETAIL_Elongation_dec != null && st.STDETAIL_Elongation_dec.ToString() != "")
                {
                    txt_Elongation.Text = st.STDETAIL_Elongation_dec.ToString();
                }
                if (st.STDETAIL_YieldStress_dec != null && st.STDETAIL_YieldStress_dec.ToString() != "")
                {
                    txt_YieldStress.Text = st.STDETAIL_YieldStress_dec.ToString();
                }
                if (st.STDETAIL_UltimateStress_dec != null && st.STDETAIL_UltimateStress_dec.ToString() != "")
                {
                    txt_UltimateStress.Text = st.STDETAIL_UltimateStress_dec.ToString();
                }
                if (st.STDETAIL_GaugeLength_dec != null && st.STDETAIL_GaugeLength_dec.ToString() != "")
                {
                    lblGaugeLength.Text = st.STDETAIL_GaugeLength_dec.ToString();
                }
                if (st.STDETAIL_WtMeterImage1_var != null && st.STDETAIL_WtMeterImage1_var.ToString() != "" && st.STDETAIL_WtMeterImage1_var.ToString() != "null")
                {
                    lblWtMeterImage1.Text = st.STDETAIL_WtMeterImage1_var.ToString();
                }
                if (st.STDETAIL_TensileImage1_var != null && st.STDETAIL_TensileImage1_var.ToString() != "" && st.STDETAIL_TensileImage1_var.ToString() != "null")
                {
                    lblTensileImage1.Text = st.STDETAIL_TensileImage1_var.ToString();
                }
                if (st.STDETAIL_TensileImage2_var != null && st.STDETAIL_TensileImage2_var.ToString() != "" && st.STDETAIL_TensileImage2_var.ToString() != "null")
                {
                    lblTensileImage2.Text = st.STDETAIL_TensileImage2_var.ToString();
                }
                if (st.STDETAIL_TensileImage3_var != null && st.STDETAIL_TensileImage3_var.ToString() != "" && st.STDETAIL_TensileImage3_var.ToString() != "null")
                {
                    lblTensileImage3.Text = st.STDETAIL_TensileImage3_var.ToString();
                }
                if (st.STDETAIL_BendImage1_var != null && st.STDETAIL_BendImage1_var.ToString() != "" && st.STDETAIL_BendImage1_var.ToString() != "null")
                {
                    lblBendImage1.Text = st.STDETAIL_BendImage1_var.ToString();
                }
                if (st.STDETAIL_RebendImage1_var != null && st.STDETAIL_RebendImage1_var.ToString() != "" && st.STDETAIL_RebendImage1_var.ToString() != "null")
                {
                    lblRebendImage1.Text = st.STDETAIL_RebendImage1_var.ToString();
                }
                i++;
                j++;
            }
            int NoOfrows = grdSteelCalulated.Rows.Count / 2;
            TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[NoOfrows].Cells[4].FindControl("txt_AvgWtMeter");
            // TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[grdSteelCalulated.Rows.Count - 1].Cells[4].FindControl("txt_AvgWtMeter");
            txt_AvgWtMeter.Text = AvgWtMeter.ToString();
            DisplayChkTestName();
            DisplayCalCulatedGrid();

        }

        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", txt_ReferenceNo.Text.ToString(), 0, "ST");
            foreach (var r in re)
            {
                AddRowSteelRemark();
                TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.STDetail_RemarkId_int), "ST");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.ST_Remark_var.ToString();
                    i++;
                }
            }
            if (grdSteelRemark.Rows.Count <= 0)
            {
                AddRowSteelRemark();
            }

        }

        #region Steel entry
        protected void AddRowSteelEntry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SteelTestTable"] != null)
            {
                GetCurrentDataSteelEntry();
                dt = (DataTable)ViewState["SteelTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_Dia", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Weight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Length", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FinalLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_YieldLoad", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_UltiMateLoad", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Rebend", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Bend", typeof(string)));

                dt.Columns.Add(new DataColumn("lblGaugeLength", typeof(string)));
                dt.Columns.Add(new DataColumn("lblWtMeterImage1", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTensileImage1", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTensileImage2", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTensileImage3", typeof(string)));
                dt.Columns.Add(new DataColumn("lblBendImage1", typeof(string)));
                dt.Columns.Add(new DataColumn("lblRebendImage1", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_Dia"] = string.Empty;
            dr["txt_IdMark"] = string.Empty;
            dr["txt_Weight"] = string.Empty;
            dr["txt_Length"] = string.Empty;
            dr["txt_FinalLength"] = string.Empty;
            dr["txt_YieldLoad"] = string.Empty;
            dr["txt_UltiMateLoad"] = string.Empty;
            dr["ddl_Rebend"] = string.Empty;
            dr["ddl_Bend"] = string.Empty;

            dr["lblGaugeLength"] = string.Empty;
            dr["lblWtMeterImage1"] = string.Empty;
            dr["lblTensileImage1"] = string.Empty;
            dr["lblTensileImage2"] = string.Empty;
            dr["lblTensileImage3"] = string.Empty;
            dr["lblBendImage1"] = string.Empty;
            dr["lblRebendImage1"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["SteelTestTable"] = dt;
            grdSteelEntry.DataSource = dt;
            grdSteelEntry.DataBind();
            SetPreviousDataSteelEntry();
        }
        protected void GetCurrentDataSteelEntry()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_Dia", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Weight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Length", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FinalLength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_YieldLoad", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_UltiMateLoad", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Rebend", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Bend", typeof(string)));

            dtTable.Columns.Add(new DataColumn("lblGaugeLength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblWtMeterImage1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTensileImage1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTensileImage2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTensileImage3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblBendImage1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblRebendImage1", typeof(string)));

            for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
            {
                TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txt_Weight = (TextBox)grdSteelEntry.Rows[i].Cells[3].FindControl("txt_Weight");
                TextBox txt_Length = (TextBox)grdSteelEntry.Rows[i].Cells[4].FindControl("txt_Length");
                TextBox txt_FinalLength = (TextBox)grdSteelEntry.Rows[i].Cells[5].FindControl("txt_FinalLength");
                TextBox txt_YieldLoad = (TextBox)grdSteelEntry.Rows[i].Cells[6].FindControl("txt_YieldLoad");
                TextBox txt_UltiMateLoad = (TextBox)grdSteelEntry.Rows[i].Cells[7].FindControl("txt_UltiMateLoad");
                DropDownList ddl_Rebend = (DropDownList)grdSteelEntry.Rows[i].Cells[8].FindControl("ddl_Rebend");
                DropDownList ddl_Bend = (DropDownList)grdSteelEntry.Rows[i].Cells[9].FindControl("ddl_Bend");

                Label lblGaugeLength = (Label)grdSteelEntry.Rows[i].FindControl("lblGaugeLength");
                Label lblWtMeterImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblWtMeterImage1");
                Label lblTensileImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage1");
                Label lblTensileImage2 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage2");
                Label lblTensileImage3 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage3");
                Label lblBendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblBendImage1");
                Label lblRebendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblRebendImage1");

                drRow = dtTable.NewRow();
                drRow["txt_Dia"] = txt_Dia.Text;
                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txt_Weight"] = txt_Weight.Text;
                drRow["txt_Length"] = txt_Length.Text;
                drRow["txt_FinalLength"] = txt_FinalLength.Text;
                drRow["txt_YieldLoad"] = txt_YieldLoad.Text;
                drRow["txt_UltiMateLoad"] = txt_UltiMateLoad.Text;
                drRow["ddl_Rebend"] = ddl_Rebend.SelectedValue;
                drRow["ddl_Bend"] = ddl_Bend.SelectedValue;

                drRow["lblGaugeLength"] = lblGaugeLength.Text;
                drRow["lblWtMeterImage1"] = lblWtMeterImage1.Text;
                drRow["lblTensileImage1"] = lblTensileImage1.Text;
                drRow["lblTensileImage2"] = lblTensileImage2.Text;
                drRow["lblTensileImage3"] = lblTensileImage3.Text;
                drRow["lblBendImage1"] = lblBendImage1.Text;
                drRow["lblRebendImage1"] = lblRebendImage1.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SteelTestTable"] = dtTable;

        }
        protected void SetPreviousDataSteelEntry()
        {
            DataTable dt = (DataTable)ViewState["SteelTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txt_Weight = (TextBox)grdSteelEntry.Rows[i].Cells[3].FindControl("txt_Weight");
                TextBox txt_Length = (TextBox)grdSteelEntry.Rows[i].Cells[4].FindControl("txt_Length");
                TextBox txt_FinalLength = (TextBox)grdSteelEntry.Rows[i].Cells[5].FindControl("txt_FinalLength");
                TextBox txt_YieldLoad = (TextBox)grdSteelEntry.Rows[i].Cells[6].FindControl("txt_YieldLoad");
                TextBox txt_UltiMateLoad = (TextBox)grdSteelEntry.Rows[i].Cells[7].FindControl("txt_UltiMateLoad");
                DropDownList ddl_Rebend = (DropDownList)grdSteelEntry.Rows[i].Cells[8].FindControl("ddl_Rebend");
                DropDownList ddl_Bend = (DropDownList)grdSteelEntry.Rows[i].Cells[9].FindControl("ddl_Bend");

                Label lblGaugeLength = (Label)grdSteelEntry.Rows[i].FindControl("lblGaugeLength");
                Label lblWtMeterImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblWtMeterImage1");
                Label lblTensileImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage1");
                Label lblTensileImage2 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage2");
                Label lblTensileImage3 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage3");
                Label lblBendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblBendImage1");
                Label lblRebendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblRebendImage1");

                txt_Dia.Text = dt.Rows[i]["txt_Dia"].ToString();
                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txt_Weight.Text = dt.Rows[i]["txt_Weight"].ToString();
                txt_Length.Text = dt.Rows[i]["txt_Length"].ToString();
                txt_FinalLength.Text = dt.Rows[i]["txt_FinalLength"].ToString();
                txt_YieldLoad.Text = dt.Rows[i]["txt_YieldLoad"].ToString();
                txt_UltiMateLoad.Text = dt.Rows[i]["txt_UltiMateLoad"].ToString();
                ddl_Rebend.SelectedValue = dt.Rows[i]["ddl_Rebend"].ToString();
                ddl_Bend.SelectedValue = dt.Rows[i]["ddl_Bend"].ToString();

                lblGaugeLength.Text = dt.Rows[i]["lblGaugeLength"].ToString();
                lblWtMeterImage1.Text = dt.Rows[i]["lblWtMeterImage1"].ToString();
                lblTensileImage1.Text = dt.Rows[i]["lblTensileImage1"].ToString();
                lblTensileImage2.Text = dt.Rows[i]["lblTensileImage2"].ToString();
                lblTensileImage3.Text = dt.Rows[i]["lblTensileImage3"].ToString();
                lblBendImage1.Text = dt.Rows[i]["lblBendImage1"].ToString();
                lblRebendImage1.Text = dt.Rows[i]["lblRebendImage1"].ToString();
            }

        }
        #endregion

        #region Steel calculated
        protected void AddRowSteelCalculated()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SteelCalulatedTable"] != null)
            {
                GetCurrentDataSteelCalculated();
                dt = (DataTable)ViewState["SteelCalulatedTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CsArea", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtMeter", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AvgWtMeter", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Elongation", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_YieldStress", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_UltimateStress", typeof(string)));

            }
            dr = dt.NewRow();
            dr["txt_IdMark"] = string.Empty;
            dr["txt_CsArea"] = string.Empty;
            dr["txt_WtMeter"] = string.Empty;
            dr["txt_AvgWtMeter"] = string.Empty;
            dr["txt_Elongation"] = string.Empty;
            dr["txt_YieldStress"] = string.Empty;
            dr["txt_UltimateStress"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["SteelCalulatedTable"] = dt;
            grdSteelCalulated.DataSource = dt;
            grdSteelCalulated.DataBind();
            SetPreviousDataSteelCalculated();
        }
        protected void GetCurrentDataSteelCalculated()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CsArea", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtMeter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgWtMeter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Elongation", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_YieldStress", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_UltimateStress", typeof(string)));


            for (int i = 0; i < grdSteelCalulated.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdSteelCalulated.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_CsArea = (TextBox)grdSteelCalulated.Rows[i].Cells[2].FindControl("txt_CsArea");
                TextBox txt_WtMeter = (TextBox)grdSteelCalulated.Rows[i].Cells[3].FindControl("txt_WtMeter");
                TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[i].Cells[4].FindControl("txt_AvgWtMeter");
                TextBox txt_Elongation = (TextBox)grdSteelCalulated.Rows[i].Cells[5].FindControl("txt_Elongation");
                TextBox txt_YieldStress = (TextBox)grdSteelCalulated.Rows[i].Cells[6].FindControl("txt_YieldStress");
                TextBox txt_UltimateStress = (TextBox)grdSteelCalulated.Rows[i].Cells[7].FindControl("txt_UltimateStress");

                drRow = dtTable.NewRow();
                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txt_CsArea"] = txt_CsArea.Text;
                drRow["txt_WtMeter"] = txt_WtMeter.Text;
                drRow["txt_AvgWtMeter"] = txt_AvgWtMeter.Text;
                drRow["txt_Elongation"] = txt_Elongation.Text;
                drRow["txt_YieldStress"] = txt_YieldStress.Text;
                drRow["txt_UltimateStress"] = txt_UltimateStress.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SteelCalulatedTable"] = dtTable;

        }
        protected void SetPreviousDataSteelCalculated()
        {
            DataTable dt = (DataTable)ViewState["SteelCalulatedTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdSteelCalulated.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_CsArea = (TextBox)grdSteelCalulated.Rows[i].Cells[2].FindControl("txt_CsArea");
                TextBox txt_WtMeter = (TextBox)grdSteelCalulated.Rows[i].Cells[3].FindControl("txt_WtMeter");
                TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[i].Cells[4].FindControl("txt_AvgWtMeter");
                TextBox txt_Elongation = (TextBox)grdSteelCalulated.Rows[i].Cells[5].FindControl("txt_Elongation");
                TextBox txt_YieldStress = (TextBox)grdSteelCalulated.Rows[i].Cells[6].FindControl("txt_YieldStress");
                TextBox txt_UltimateStress = (TextBox)grdSteelCalulated.Rows[i].Cells[7].FindControl("txt_UltimateStress");


                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txt_CsArea.Text = dt.Rows[i]["txt_CsArea"].ToString();
                txt_WtMeter.Text = dt.Rows[i]["txt_WtMeter"].ToString();
                txt_AvgWtMeter.Text = dt.Rows[i]["txt_AvgWtMeter"].ToString();
                txt_Elongation.Text = dt.Rows[i]["txt_Elongation"].ToString();
                txt_YieldStress.Text = dt.Rows[i]["txt_YieldStress"].ToString();
                txt_UltimateStress.Text = dt.Rows[i]["txt_UltimateStress"].ToString();

            }

        }
        #endregion

        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            if (ddl_SubReports.SelectedValue != "---Select---")
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                lnkSave.Enabled = true;
                lnkPrint.Visible = false;
                ddl_ComplainceNote.ClearSelection();
                grdSteelEntry.DataSource = null;
                grdSteelEntry.DataBind();
                grdSteelCalulated.DataSource = null;
                grdSteelCalulated.DataBind();
                //AddRowSteelEntry();
                //AddRowSteelCalculated();
                //DisplayChkTestName();
                //DisplayCalCulatedGrid();
                grdSteelRemark.DataSource = null;
                grdSteelRemark.DataBind();
                //DisplayRemark();

                txt_ReferenceNo.Text = ddl_SubReports.SelectedItem.Text;
                DisplaySteelDetails();

                LoadReferenceNoList();
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
                DateTime DateofTesting = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);

                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, DateofTesting, txt_Description.Text, "", 2, 0, "", 0, ddl_ComplainceNote.SelectedItem.Text, "", 0, 0, 0, "ST");
                    dc.ReportDetails_Update("ST", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "ST", txt_ReferenceNo.Text, "ST", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, DateofTesting, txt_Description.Text, "", 3, 0, "", 0, ddl_ComplainceNote.SelectedItem.Text, "", 0, 0, 0, "ST");
                    dc.ReportDetails_Update("ST", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "ST", txt_ReferenceNo.Text, "ST", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "ST", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                dc.SteelDetailInward_Update(txt_ReferenceNo.Text, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, "","", 0, 0, "", "", "", "", "", "", false, false, true);
                decimal AvgWtMeter = 0;
                int j = 0;
                string LoadType = "";
                if (Rdn_LoadKg.Checked)
                {
                    LoadType = "Kg";
                }
                else if (Rdn_LoadKn.Checked)
                {
                    LoadType = "Kn";
                }
                for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
                {
                    Label lblStar = (Label)grdSteelEntry.Rows[j].Cells[0].FindControl("lblStar");

                    TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                    TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                    TextBox txt_Weight = (TextBox)grdSteelEntry.Rows[i].Cells[3].FindControl("txt_Weight");
                    TextBox txt_Length = (TextBox)grdSteelEntry.Rows[i].Cells[4].FindControl("txt_Length");
                    TextBox txt_FinalLength = (TextBox)grdSteelEntry.Rows[i].Cells[5].FindControl("txt_FinalLength");
                    TextBox txt_YieldLoad = (TextBox)grdSteelEntry.Rows[i].Cells[6].FindControl("txt_YieldLoad");
                    TextBox txt_UltiMateLoad = (TextBox)grdSteelEntry.Rows[i].Cells[7].FindControl("txt_UltiMateLoad");
                    DropDownList ddl_Rebend = (DropDownList)grdSteelEntry.Rows[i].Cells[8].FindControl("ddl_Rebend");
                    DropDownList ddl_Bend = (DropDownList)grdSteelEntry.Rows[i].Cells[9].FindControl("ddl_Bend");

                    Label lblGaugeLength = (Label)grdSteelEntry.Rows[i].FindControl("lblGaugeLength");
                    Label lblWtMeterImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblWtMeterImage1");
                    Label lblTensileImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage1");
                    Label lblTensileImage2 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage2");
                    Label lblTensileImage3 = (Label)grdSteelEntry.Rows[i].FindControl("lblTensileImage3");
                    Label lblBendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblBendImage1");
                    Label lblRebendImage1 = (Label)grdSteelEntry.Rows[i].FindControl("lblRebendImage1");

                    TextBox txt_CsArea = (TextBox)grdSteelCalulated.Rows[j].Cells[2].FindControl("txt_CsArea");
                    TextBox txt_WtMeter = (TextBox)grdSteelCalulated.Rows[j].Cells[3].FindControl("txt_WtMeter");
                    TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[j].Cells[4].FindControl("txt_AvgWtMeter");
                    TextBox txt_Elongation = (TextBox)grdSteelCalulated.Rows[j].Cells[5].FindControl("txt_Elongation");
                    TextBox txt_YieldStress = (TextBox)grdSteelCalulated.Rows[j].Cells[6].FindControl("txt_YieldStress");
                    TextBox txt_UltimateStress = (TextBox)grdSteelCalulated.Rows[j].Cells[7].FindControl("txt_UltimateStress");
                    
                    if (txt_AvgWtMeter.Text != null && txt_AvgWtMeter.Text != "" && txt_AvgWtMeter.Text != "NA")
                    {
                        AvgWtMeter = Convert.ToDecimal(txt_AvgWtMeter.Text);
                    }
                    j++;
                    if (txt_Weight.Text == "")
                    {
                        txt_Weight.Text = "0.00";
                    }
                    if (txt_Length.Text == "")
                    {
                        txt_Length.Text = "0.00";
                    }
                    if (txt_FinalLength.Text == "")
                    {
                        txt_FinalLength.Text = "0.00";
                    }
                    if (txt_YieldLoad.Text == "")
                    {
                        txt_YieldLoad.Text = "0.000";
                    }
                    if (txt_UltiMateLoad.Text == "")
                    {
                        txt_UltiMateLoad.Text = "0.000";
                    }
                    if (txt_UltiMateLoad.Text == "")
                    {
                        txt_UltiMateLoad.Text = "0.00";
                    }
                    if (ddl_Rebend.SelectedItem.Text == "Select")
                    {
                        ddl_Rebend.SelectedItem.Text = "";
                    }
                    if (ddl_Bend.SelectedItem.Text == "Select")
                    {
                        ddl_Bend.SelectedItem.Text = "";
                    }
                    if (txt_CsArea.Text == "")
                    {
                        txt_CsArea.Text = "0.00";
                    }
                    if (txt_WtMeter.Text == "")
                    {
                        txt_WtMeter.Text = "0.00";
                    }
                    if (txt_Elongation.Text == "")
                    {
                        txt_Elongation.Text = "0.00";
                    }
                    if (txt_YieldStress.Text == "")
                    {
                        txt_YieldStress.Text = "000.00";
                    }
                    if (txt_UltimateStress.Text == "")
                    {
                        txt_UltimateStress.Text = "000.00";
                    }
                    if (lblGaugeLength.Text == "")
                    {
                        lblGaugeLength.Text = "0.00";
                    }
                    dc.SteelDetailInward_Update(txt_ReferenceNo.Text, 0, txt_IdMark.Text, Convert.ToDecimal(txt_Weight.Text), Convert.ToDecimal(txt_Length.Text), Convert.ToDecimal(txt_FinalLength.Text), Convert.ToDecimal(txt_YieldLoad.Text),
                                                Convert.ToDecimal(txt_UltiMateLoad.Text), ddl_Rebend.SelectedItem.Text, ddl_Bend.SelectedItem.Text, Convert.ToDecimal(txt_CsArea.Text), Convert.ToDecimal(txt_WtMeter.Text), Convert.ToDecimal(txt_Elongation.Text),
                                                Convert.ToDecimal(txt_YieldStress.Text), Convert.ToDecimal(txt_UltimateStress.Text), 0, "", lblStar.Text, (i + 1), Convert.ToDecimal(lblGaugeLength.Text), lblWtMeterImage1.Text, lblTensileImage1.Text, lblTensileImage2.Text, lblTensileImage3.Text, lblBendImage1.Text, lblRebendImage1.Text, false, false, false);

                }
                dc.SteelDetailInward_Update(txt_ReferenceNo.Text, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, AvgWtMeter, LoadType, "", 0, 0, "", "", "", "", "", "", true, false, false);
          
                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "ST", true);
                for (int i = 0; i < grdSteelRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "ST");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.ST_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "ST");
                            foreach (var c in chkId)
                            {
                                if (c.STDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "ST", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "ST");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "ST");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.ST_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "ST", false);
                            }
                        }
                    }
                }
                #endregion


                //Approve on check
                if (lbl_TestedBy.Text == "Approve By")
                {
                    ApproveReports("ST", txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text)); //, txtEmailId.Text, lblContactNo.Text);
                    //update MISCRLApprStatus to 1 if SITE_CRBypass_bit is 1
                    int siteCRbypssBit = 0; //clsData cd = new clsData();
                    siteCRbypssBit = cd.getClientCrlBypassBit("ST", Convert.ToInt32(lblRecordNo.Text));
                    if (siteCRbypssBit == 1)
                        dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, "ST");

                    //SMS
                    var res = dc.SMSDetailsForReportApproval_View(Convert.ToInt32(lblRecordNo.Text), "ST").ToList();
                    if (res.Count > 0)
                    {
                        cd.sendInwardReportMsg(Convert.ToInt32(res.FirstOrDefault().ENQ_Id), Convert.ToString(res.FirstOrDefault().ENQ_Date_dt), Convert.ToInt32(res.FirstOrDefault().SITE_MonthlyBillingStatus_bit).ToString(), res.FirstOrDefault().crLimitExceededStatus.ToString(), res.FirstOrDefault().BILL_NetAmt_num.ToString(), txt_ReferenceNo.Text, Convert.ToString(res.FirstOrDefault().INWD_ContactNo_var), "Report");
                    }
                    //old dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text), "ST");
                  //  CRLimitExceedEmail(txt_ReferenceNo.Text, lblRecordNo.Text.ToString(), "ST", "Steel Testing");

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
            else if (txt_typeofSteel.Text == "")
            {
                lblMsg.Text = "Please Enter Type Of Bar";
                valid = false;
            }
            else if (txt_Description.Text == "")
            {
                lblMsg.Text = "Please Enter Description";
                valid = false;
            }
            else if (Rdn_LoadKg.Checked == false && Rdn_LoadKn.Checked == false)
            {
                lblMsg.Text = "Please Select one of the Radio button";
                valid = false;
            }
            else if (ddl_ComplainceNote.SelectedItem.Text == "Select")
            {
                lblMsg.Text = "Please Select Compliance Note";
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
            else if (ddl_TestedBy.SelectedItem.Text == "---Select---" && lblEntry.Text == "Check")
            {
                lblMsg.Text = "Please Select the Approve By";
                valid = false;
            }
            else if (ddl_TestedBy.SelectedItem.Text == "---Select---" && lblEntry.Text == "Enter")
            {
                lblMsg.Text = "Please Select the Tested By";
                valid = false;
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
                {
                    TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                    TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                    TextBox txt_Weight = (TextBox)grdSteelEntry.Rows[i].Cells[3].FindControl("txt_Weight");
                    TextBox txt_Length = (TextBox)grdSteelEntry.Rows[i].Cells[4].FindControl("txt_Length");
                    TextBox txt_FinalLength = (TextBox)grdSteelEntry.Rows[i].Cells[5].FindControl("txt_FinalLength");
                    TextBox txt_YieldLoad = (TextBox)grdSteelEntry.Rows[i].Cells[6].FindControl("txt_YieldLoad");
                    TextBox txt_UltiMateLoad = (TextBox)grdSteelEntry.Rows[i].Cells[7].FindControl("txt_UltiMateLoad");
                    DropDownList ddl_Rebend = (DropDownList)grdSteelEntry.Rows[i].Cells[8].FindControl("ddl_Rebend");
                    DropDownList ddl_Bend = (DropDownList)grdSteelEntry.Rows[i].Cells[9].FindControl("ddl_Bend");

                    if (txt_Weight.Visible == true && txt_Weight.Text == "")
                    {
                        lblMsg.Text = "Please Enter Weight for Sr No. " + (i + 1) + ".";
                        txt_Weight.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Length.Text == "" && txt_Length.Visible == true)
                    {
                        lblMsg.Text = "Please Enter Length for Sr No. " + (i + 1) + ".";
                        txt_Length.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_FinalLength.Text == "" && txt_FinalLength.Visible == true)
                    {
                        lblMsg.Text = "Please Enter Final Length for Sr No. " + (i + 1) + ".";
                        txt_FinalLength.Focus();
                        valid = false;
                        break;
                    }
                    //if (txt_YieldLoad.Text == "")
                    //{
                    //    dispalyMsg = "Please Enter Yield Load for row No. " + (i + 1) + ".";
                    //    txt_YieldLoad.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    if (txt_UltiMateLoad.Text == "" && txt_UltiMateLoad.Visible == true)
                    {
                        lblMsg.Text = "Please Enter Ultimate Load for Sr No. " + (i + 1) + ".";
                        txt_UltiMateLoad.Focus();
                        valid = false;
                        break;
                    }
                    if (ddl_Rebend.SelectedItem.Text == "Select" && ddl_Rebend.Visible == true)
                    {
                        lblMsg.Text = "Please Select Rebend for Sr No. " + (i + 1) + ".";
                        txt_UltiMateLoad.Focus();
                        valid = false;
                        break;
                    }
                    if (ddl_Bend.SelectedItem.Text == "Select" && ddl_Bend.Visible == true)
                    {
                        lblMsg.Text = "Please Select Bend for Sr No. " + (i + 1) + ".";
                        txt_UltiMateLoad.Focus();
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
                //// mSubject = "Credit Limit Exceeded";
                //mSubject = "Report Confirmation";

                //mbody = "Dear Customer,<br><br>";


                //if (crLimtFlag == 1)//Credit Limit exceeded Client
                //    mbody = mbody + "We have tested material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ". The report is ready with us. Your total outstanding dues are Rs " + strOutstndingAmt + ".<br>Please arrange to make the payment for the testing to access the report on our Durocrete Mobile App. The copy of the report will be emailed to you on your registered email id once payment is made. For any assistance please contact on our tollfree no. " + tollFree + "<br><br><br>";
                //else
                //    mbody = mbody + "We have tested material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ". The report is ready with us, soft copy of the report has been emailed to you on your registered email id. You can view this report  with help of Durocrete APP on your mobile phone.You can also download the report from our website www.durocrete.in. For any assistance please contact on our tollfree no. " + tollFree + "<br><br><br>";

                //mbody = mbody + "<br>";
                //mbody = mbody + "<br>";
                //mbody = mbody + "<br>";
                //mbody = mbody + "Best Regards,";
                //mbody = mbody + "<br>&nbsp;";
                //mbody = mbody + "<br>";
                //mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                try
                {
                  //  objMail.SendMail(mailIdTo, mailIdCc, mSubject, mbody, "", mReplyTo);
                    dc.Inward_Update_CRLExceedMailStatus(Convert.ToInt32(recordNo), recordType, true);
                    //update enq-outstanding mail count in route table of that ME
                    if (siteRouteId != 0 && siteMeId != 0)
                        dc.Route_Update_OutsandingMailCount(siteRouteId, siteMeId, 2);


                }
                catch { }
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
            int j = 0;
            decimal Cs1 = 0;
            decimal Cs2 = 0;
            decimal CSArea = 0;
            decimal MaxLoad = 0;
            decimal YLoad = 0;
            decimal UltimateStress = 0;
            decimal Yst = 0;
            decimal Elongation = 0;
            decimal gln = 0;
            double SqurRtOfCsArea = 0;
            decimal WtperMeter = 0;
            decimal AvgWtperMeter = 0;
            decimal SumOfWtperMeter = 0;
            string Grade = "";
            int r = 0;
            bool wtpermetr = false;
            var stInwd = dc.AllInwdDetails_View(txt_ReferenceNo.Text , "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "ST");
            foreach (var st in stInwd)
            {
                if (st.TEST_Sr_No == 4)
                {
                    wtpermetr = true;
                    break;
                }
            }
            //delete previous remark
            //DeleteRemark("");
            DeleteRemark("The average weight/meter");
            DeleteRemark("The individual weight/meter");
            DeleteRemark("the minimum percentage elongation");
            DeleteRemark("the minimum ultimate tensile stress");
            DeleteRemark("the minimum yield stress");
            DeleteRemark("the minimum 0.2 % proof stress");
            DeleteRemark("NA indicates - not applicable, as only one specimen was tested.");
            //
            if (txt_GradeSteel.Text != null)
            {
                Grade = txt_GradeSteel.Text.ToString();
            }
            string strDia = "";
            //grdSteelRemark.DataSource = null;
            //grdSteelRemark.DataBind();
            int NoOfrows = grdSteelCalulated.Rows.Count / 2;
            decimal Yield = 0;
            
            //TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[grdSteelCalulated.Rows.Count - 1].Cells[4].FindControl("txt_AvgWtMeter");
            TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[NoOfrows].Cells[4].FindControl("txt_AvgWtMeter");
            for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
            {
                TextBox txt_IdMark1 = (TextBox)grdSteelCalulated.Rows[j].Cells[1].FindControl("txt_IdMark");
                TextBox txt_CsArea = (TextBox)grdSteelCalulated.Rows[j].Cells[2].FindControl("txt_CsArea");
                TextBox txt_WtMeter = (TextBox)grdSteelCalulated.Rows[j].Cells[3].FindControl("txt_WtMeter");
                TextBox txt_Elongation = (TextBox)grdSteelCalulated.Rows[j].Cells[5].FindControl("txt_Elongation");
                TextBox txt_YieldStress = (TextBox)grdSteelCalulated.Rows[j].Cells[6].FindControl("txt_YieldStress");
                TextBox txt_UltimateStress = (TextBox)grdSteelCalulated.Rows[j].Cells[7].FindControl("txt_UltimateStress");

                TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txt_Weight = (TextBox)grdSteelEntry.Rows[i].Cells[3].FindControl("txt_Weight");
                TextBox txt_Length = (TextBox)grdSteelEntry.Rows[i].Cells[4].FindControl("txt_Length");
                TextBox txt_FinalLength = (TextBox)grdSteelEntry.Rows[i].Cells[5].FindControl("txt_FinalLength");
                TextBox txt_YieldLoad = (TextBox)grdSteelEntry.Rows[i].Cells[6].FindControl("txt_YieldLoad");
                TextBox txt_UltiMateLoad = (TextBox)grdSteelEntry.Rows[i].Cells[7].FindControl("txt_UltiMateLoad");
                DropDownList ddl_Rebend = (DropDownList)grdSteelEntry.Rows[i].Cells[8].FindControl("ddl_Rebend");
                DropDownList ddl_Bend = (DropDownList)grdSteelEntry.Rows[i].Cells[9].FindControl("ddl_Bend");
                Label lblStar = (Label)grdSteelEntry.Rows[i].Cells[0].FindControl("lblStar");
                
                if (txt_UltiMateLoad.Text == "")
                {
                    txt_UltiMateLoad.Text = "0.00";
                }
                if (txt_YieldLoad.Text == "")
                {
                    txt_YieldLoad.Text = "0.00";
                }
                if (txt_FinalLength.Text == "")
                {
                    txt_FinalLength.Text = "0.00";
                }
                if (txt_Weight.Text == "")
                {
                    txt_Weight.Text = "0.00";
                }
                if (txt_Length.Text == "")
                {
                    txt_Length.Text = "0.00";
                }

                if (grdSteelEntry.Rows[i].Cells[3].Visible == true &&
                     grdSteelEntry.Rows[i].Cells[4].Visible == true)
                {

                    Cs1 = Convert.ToDecimal(txt_Weight.Text) / 1000;
                    Cs2 = (Convert.ToDecimal(txt_Length.Text) / 1000) * 7850;
                    CSArea = (Cs1 / Cs2) * 1000000;//).ToString("00.00");
                }
                if (grdSteelEntry.Rows[i].Cells[6].Visible == true &&
                     grdSteelEntry.Rows[i].Cells[7].Visible == true)
                {
                    if (Rdn_LoadKg.Checked)
                    {
                        MaxLoad = Convert.ToDecimal(txt_UltiMateLoad.Text) * Convert.ToDecimal(9.81);
                        YLoad = Convert.ToDecimal(txt_YieldLoad.Text) * Convert.ToDecimal(9.81);
                    }
                    else if (Rdn_LoadKn.Checked)
                    {
                        MaxLoad = Convert.ToDecimal(txt_UltiMateLoad.Text) * 1000;
                        YLoad = Convert.ToDecimal(txt_YieldLoad.Text) * 1000;
                    }
                    //}
                    //if (grdSteelEntry.Rows[i].Cells[5].Visible == true)
                    //{
                    UltimateStress = MaxLoad / CSArea;
                    //}
                    if (txt_YieldLoad.Text != null && txt_YieldLoad.Text != "")
                    {
                        Yield = Convert.ToDecimal(txt_YieldLoad.Text);
                        Yst = YLoad / CSArea;
                    }
                    else
                    {
                        Yst = UltimateStress * Convert.ToDecimal(0.8);
                        //AddRowSteelRemark();
                        //TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[r].Cells[1].FindControl("txt_REMARK");
                        //txt_REMARK.Text = "Yield Stress is calculated as 0.8 times of ultimate stress.";
                        //r++;
                    }
                }
                if (grdSteelEntry.Rows[i].Cells[3].Visible == true && grdSteelEntry.Rows[i].Cells[4].Visible == true && grdSteelEntry.Rows[i].Cells[5].Visible == false)
                {
                    //WtPerMeter
                    WtperMeter = Convert.ToDecimal(txt_Weight.Text) / Convert.ToDecimal(txt_Length.Text);
                    SumOfWtperMeter += WtperMeter;
                    //AvgWtperMeter = SumOfWtperMeter / grdSteelEntry.Rows.Count;//9.999
                }
                if (grdSteelEntry.Rows[i].Cells[5].Visible == true)
                {
                    // SqurRtOfCsArea =  FindSquareRoot(Convert.ToDouble(CSArea);
                    SqurRtOfCsArea = Math.Sqrt(Convert.ToDouble(CSArea));
                    gln = Convert.ToDecimal(SqurRtOfCsArea * 5.65);//99.99
                    Elongation = ((Convert.ToDecimal(txt_FinalLength.Text) - gln) / gln) * 100;

                    //WtPerMeter
                    WtperMeter = Convert.ToDecimal(txt_Weight.Text) / Convert.ToDecimal(txt_Length.Text);
                    SumOfWtperMeter += WtperMeter;
                    //AvgWtperMeter = SumOfWtperMeter / grdSteelEntry.Rows.Count;//9.999
                }
                if (grdSteelEntry.Rows.Count == 1)
                {
                    ddl_ComplainceNote.SelectedValue = "2";
                }
                txt_AvgWtMeter.Text = "NA";
                bool starFlagElong = false, starFlagWtPerMtr = false, starFlagYield = false, starFlagUltimate = false;
                if (Elongation > 0)
                {
                    starFlagElong = ChkPerElongation(Elongation, Grade, r);
                    r++;
                }
                if (UltimateStress > 0)
                {
                    starFlagUltimate = ChkUltimateStress(UltimateStress, Grade, r);
                    r++;
                }
                if (Yst > 0)
                {
                    starFlagYield = ChkYiledStress(Yst, Grade, r);
                    r++;
                }
                strDia = txt_Dia.Text.Replace("mm", "").Replace(" ", "");
                if (WtperMeter > 0)
                {
                    if (wtpermetr == true)
                    {
                        starFlagWtPerMtr = ChkWtPerMeter(WtperMeter, strDia, r);
                    }
                    r++;
                }
                lblStar.Text = "";
                if (starFlagWtPerMtr == true)                
                    lblStar.Text = lblStar.Text + "*,";
                else
                    lblStar.Text = lblStar.Text + ",";

                if (starFlagElong == true)
                    lblStar.Text = lblStar.Text + "#,"; //"*,";
                else
                    lblStar.Text = lblStar.Text + ",";

                if (starFlagYield == true)
                    lblStar.Text = lblStar.Text + "$,"; //"*,";
                else
                    lblStar.Text = lblStar.Text + ",";

                if (starFlagUltimate == true)
                    lblStar.Text = lblStar.Text + "~,"; //"*,";
                else
                    lblStar.Text = lblStar.Text + ",";
                txt_IdMark1.Text = txt_IdMark.Text;
                txt_CsArea.Text = Convert.ToDecimal(CSArea).ToString("00.00");
                txt_WtMeter.Text = Convert.ToDecimal(WtperMeter).ToString("0.000");
                txt_Elongation.Text = Convert.ToDecimal(Elongation).ToString("00.0");
                txt_YieldStress.Text = Convert.ToDecimal(Yst).ToString("000.00");
                txt_UltimateStress.Text = Convert.ToDecimal(UltimateStress).ToString("000.00");
                j++;
            }
            //int m = 0;
            //if (Yield > 0)
            //{
            //    AddRowSteelRemark();
            //    TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[m].Cells[1].FindControl("txt_REMARK");
            //    txt_REMARK.Text = "Yield Stress is calculated as 0.8 times of ultimate stress.";
            //}
            AvgWtperMeter = SumOfWtperMeter / grdSteelEntry.Rows.Count;//9.999
            bool avgWtPerMtrStarFlag = false;
            if (AvgWtperMeter > 0 && wtpermetr == true)
            {
                avgWtPerMtrStarFlag = ChkAvgWtPerMeter(AvgWtperMeter, strDia, r);
            }
            Label lblStar1 = (Label)grdSteelEntry.Rows[0].FindControl("lblStar");
            if (avgWtPerMtrStarFlag == true)
                lblStar1.Text = lblStar1.Text + "**,";//"*,";
            else
                lblStar1.Text = lblStar1.Text + ",";
            txt_AvgWtMeter.Text = Convert.ToDecimal(AvgWtperMeter).ToString("0.000");
            if (grdSteelEntry.Rows.Count == 1 && ddl_ComplainceNote.SelectedValue == "2")                            
            {
                //DeleteRemark("The average weight/meter");
                txt_AvgWtMeter.Text = "NA";
                AddRowSteelRemark();
                TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[grdSteelRemark.Rows.Count - 1].FindControl("txt_REMARK");
                txt_REMARK.Text = "NA indicates - not applicable, as only one specimen was tested.";
            }
            DeleteRowRemark();
            if (grdSteelRemark.Rows.Count == 0)
            {
                AddRowSteelRemark();
            }
        }

        private bool ChkWtPerMeter(decimal WtperMeter, string GradeOfSteel, int r)
        {
            //string Remark = "";
            string WtMtStr = "";
            bool FailWtperMtr = false;
            //AddRowSteelRemark();
            //r = grdSteelRemark.Rows.Count - 1;
            //TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[r].Cells[1].FindControl("txt_REMARK");
            switch (GradeOfSteel)
            {
                case "5":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(0.142))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                        // if (ddl_ComplainceNote.SelectedValue == "2")
                        // {
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 0.142 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 0.142 kg.");
                        //}
                        FailWtperMtr = true;


                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 0.142 kg.";
                            AddRemark("The individual weight/meter should not be below 0.142 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                    }

                    break;
                case "6":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(0.204))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* -The individual weight/meter should not be below 0.204 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* -The individual weight/meter should not be below 0.204 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 0.204 kg.";
                            AddRemark("The individual weight/meter should not be below 0.204 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "8":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(0.363))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 0.363 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 0.363 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 0.363 kg.";
                            AddRemark("The individual weight/meter should not be below 0.363 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "10":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(0.567))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 0.567 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 0.567 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 0.567 kg.";
                            AddRemark("The individual weight/meter should not be below 0.567 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "12":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(0.834))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 0.834 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 0.834 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 0.834 kg.";
                            AddRemark("The individual weight/meter should not be below 0.834 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "16":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(1.485))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 0.485 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 1.485 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 1.485 kg.";
                            AddRemark("The individual weight/meter should not be below 1.485 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "20":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(2.371))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 2.371 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 2.371 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 2.371 kg.";
                            AddRemark("The individual weight/meter should not be below 2.371 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "25":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(3.696))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 3.696 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 3.696 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 3.696 kg.";
                            AddRemark("The individual weight/meter should not be below 3.696 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "28":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(4.638))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 4.638 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 4.638 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 4.638 kg.";
                            AddRemark("The individual weight/meter should not be below 4.638 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "32":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(6.058))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 6.058 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 6.058 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 6.058 kg.";
                            AddRemark("The individual weight/meter should not be below 6.058 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;
                case "36":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(6.058))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "* - The individual weight/meter should not be below 6.058 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("* - The individual weight/meter should not be below 6.058 kg.");
                        //}
                        FailWtperMtr = true;
                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 6.058 kg.";
                            AddRemark("The individual weight/meter should not be below 6.058 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }
                    break;
                case "40":
                    if (Math.Round(WtperMeter, 3) < Convert.ToDecimal(9.466))
                    {
                        WtMtStr = "*";
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");
                        FailWtperMtr = true;
                        //delete Remark("The Individual weight/meter should not be below" + CStr(Format (myint,"0.000")) +"kg. );
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        //txt_REMARK.Text = "*- The individual weight/meter should not be below 6.058 kg.";
                        DeleteRemark("The individual weight/meter");
                        AddRemark("*- The individual weight/meter should not be below 9.465 kg.");
                        //}

                    }
                    else if (FailWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //txt_REMARK.Text = "The individual weight/meter should not be below 6.058 kg.";
                            AddRemark("The individual weight/meter should not be below 9.465 kg.");
                        }
                        WtMtStr = WtMtStr + WtperMeter.ToString("0.000");

                    }

                    break;


            }
            //return WtMtStr;
            if (WtMtStr.Contains("*") == true)
                return true;
            else
                return false;
        }

        private bool ChkAvgWtPerMeter(decimal AvgWtperMeter, string GradeOfSteel, int r)
        {
            string myStr = "";
            string AvgWtMtrStr = "";
            bool FailAvgWtperMtr = false;
            //AddRowSteelRemark();
            //r = grdSteelRemark.Rows.Count - 1;
            //TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[r].Cells[1].FindControl("txt_REMARK");
            switch (GradeOfSteel)
            {
                case "5":
                    myStr = "0.143 kg and 0.165 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(0.143) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(0.165))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "6":
                    myStr = "0.237 kg and 0.206 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(0.206) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(0.237))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "8":
                    myStr = "0.422 kg and 0.367 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(0.367) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(0.422))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "10":
                    myStr = "0.660 kg and 0.573 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(0.573) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(0.66))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "12":
                    myStr = "0.932 kg and 0.844 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(0.844) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(0.932))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "16":
                    myStr = "1.659 kg and 1.501 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(1.501) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(1.659))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "20":
                    myStr = "2.544 kg and 2.395 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(2.395) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(2.544))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        // {
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "25":
                    myStr = "3.965 kg and 3.735 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(3.735) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(3.965))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "28":
                    myStr = "4.976 kg and 4.686 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(4.686) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(4.976))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "32":
                    myStr = "6.499 kg and 6.120 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(6.12) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(6.499))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "36":
                    myStr = "7.750 kg and 8.230 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(7.75) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(8.23))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;
                case "40":
                    myStr = "9.564 kg and 10.156 kg.";
                    if (Math.Round(AvgWtperMeter, 3) < Convert.ToDecimal(9.564) || Math.Round(AvgWtperMeter, 3) > Convert.ToDecimal(10.156))
                    {
                        AvgWtMtrStr = "**";
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                        FailAvgWtperMtr = true;
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("The average weight/meter");
                        AddRemark("** - The average weight/meter should be between " + myStr);
                        //}
                    }
                    else if (FailAvgWtperMtr == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("The average weight/meter should be between " + myStr);
                        }
                        AvgWtMtrStr = AvgWtMtrStr + AvgWtperMeter.ToString("0.000");
                    }

                    break;


                    //if (FailAvgWtperMtr == true)
                    //{
                    //delete Remark("* - The average weight/meter should be between & mystr.");

                    //}

                    // AvgWtperMeter = Convert.ToDecimal(AvgWtMtrStr);

            }
            if (AvgWtMtrStr.Contains("**") == true)
                return true;
            else
                return false;
        }

        private bool ChkPerElongation(decimal Elongation, string GradeOfSteel, int r)
        {
            //AddRowSteelRemark();
            bool ElongFlag = false;
            string ElongStr = "";
            //r = grdSteelRemark.Rows.Count - 1;
            //TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[r].Cells[1].FindControl("txt_REMARK");
            switch (GradeOfSteel)
            {
                case "Fe 250":
                    if (Math.Round(Elongation, 1) < 23)
                    {
                        ElongFlag = true;
                        //ElongStr = "***";
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");//"00.0"
                                                                          //if (ddl_ComplainceNote.SelectedValue == "2")
                                                                          //{
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 250 the minimum percentage elongation should be 23%.");
                        // }
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 250 the minimum percentage elongation should be 23%.");
                            }
                            ElongStr = ElongStr + Elongation.ToString("00.0");
                        }
                    }

                    break;
                case "Fe 415":
                    if (Math.Round(Elongation, 1) < (Convert.ToDecimal(14.5)))
                    {
                        ElongFlag = true;
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");//"00.0"
                                                                          //if (ddl_ComplainceNote.SelectedValue == "2")
                                                                          //{
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 415 the minimum percentage elongation should be 14.5%.");
                        // }
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 415 the minimum percentage elongation should be 14.5%.");
                            }
                            ElongStr = ElongStr + Elongation.ToString("00.0");
                        }
                    }

                    break;
                case "Fe 500":
                    if (Math.Round(Elongation, 1) < 12)
                    {
                        ElongFlag = true;
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 500 the minimum percentage elongation should be 12%.");
                        //}
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 500 the minimum percentage elongation should be 12%.");
                            }
                        }
                        ElongStr = ElongStr + Elongation.ToString("00.0");
                    }

                    break;
                case "Fe 550":
                    if (Math.Round(Elongation, 1) < 10)
                    {
                        ElongFlag = true;
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 550 the minimum percentage elongation should be 10%.");
                        //}
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 550 the minimum percentage elongation should be 10%.");
                            }
                        }
                        ElongStr = ElongStr + Elongation.ToString("00.0");

                    }

                    break;
                case "Fe 415D":
                    if (Math.Round(Elongation, 1) < 18)
                    {
                        ElongFlag = true;
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 415D the minimum percentage elongation should be 18%.");
                        //}
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 415D the minimum percentage elongation should be 18%.");
                            }
                        }
                        ElongStr = ElongStr + Elongation.ToString("00.0");

                    }

                    break;
                case "Fe 500D":
                    if (Math.Round(Elongation, 1) < 16)
                    {
                        ElongFlag = true;
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 500D the minimum percentage elongation should be 16%.");
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 500D the minimum percentage elongation should be 16%.");
                            }
                        }
                        ElongStr = ElongStr + Elongation.ToString("00.0");

                    }

                    break;
                case "Fe 550D":
                    if (Math.Round(Elongation, 1) < Convert.ToDecimal(14.5))
                    {
                        ElongFlag = true;
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 550D the minimum percentage elongation should be 14.5%.");
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 550D the minimum percentage elongation should be 14.5%.");
                            }
                        }
                        ElongStr = ElongStr + Elongation.ToString("00.0");

                    }

                    break;
                case "Fe 600":
                    if (Math.Round(Elongation, 1) < 10)
                    {
                        ElongFlag = true;
                        ElongStr = "#";
                        ElongStr = ElongStr + Elongation.ToString("00.0");
                        DeleteRemark("the minimum percentage elongation");
                        AddRemark("#- For Fe 600 the minimum percentage elongation should be 10%.");
                    }
                    else
                    {
                        if (ElongFlag == false)
                        {
                            if (ddl_ComplainceNote.SelectedValue == "2")
                            {
                                AddRemark("For Fe 600 the minimum percentage elongation should be 10%.");
                            }
                        }
                        ElongStr = ElongStr + Elongation.ToString("00.0");

                    }

                    break;
                    // Elongation = Convert.ToDecimal(ElongStr);

            }
            if (ElongStr.Contains("#") == true)
                return true;
            else
                return false;
        }

        private bool ChkYiledStress(decimal YST, string GradeOfSteel, int r)
        {

            bool FailYieldStress = false;
            string YSTStr = "";
            //AddRowSteelRemark();
            //r = grdSteelRemark.Rows.Count - 1;
            //TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[r].Cells[1].FindControl("txt_REMARK");
            switch (GradeOfSteel)
            {
                case "Fe 250":
                    if (Math.Round(YST, 2) < 250)
                    {
                        FailYieldStress = true;
                        //YSTStr = "****";
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        //            if (ddl_ComplainceNote.SelectedValue == "2")
                        //          {
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 250 the minimum yield stress should be 250 N/mm2. ");
                        //        }

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 250 the minimum yield stress should be 250 N/mm2. ");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                case "Fe 415":
                    if (Math.Round(YST, 2) < 415)
                    {
                        FailYieldStress = true;
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        // /     if (ddl_ComplainceNote.SelectedValue == "2")
                        // /    {
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 415 the minimum yield stress should be 415 N/mm2.");
                        // }

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            //AddRemark("For Fe 415 the minimum 0.2 % proof stress/yield should be 415 N/mm2.");
                            AddRemark("For Fe 415 the minimum yield stress should be 415 N/mm2.");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                case "Fe 500":
                    if (Math.Round(YST, 2) < 500)
                    {
                        FailYieldStress = true;
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        //                        if (ddl_ComplainceNote.SelectedValue == "2")
                        //                        {
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 500 the minimum yield stress should be 500 N/mm2.");
                        //                        }

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 500 the minimum yield stress should be 500 N/mm2.");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                case "Fe 550":
                    if (Math.Round(YST, 2) < 550)
                    {
                        FailYieldStress = true;
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        //                      if (ddl_ComplainceNote.SelectedValue == "2")
                        //                    {
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 550 the minimum yield stress should be 550 N/mm2.");
                        ///                  }

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 550 the minimum yield stress should be 550 N/mm2.");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                case "Fe 415D":
                    if (Math.Round(YST, 2) < 415)
                    {
                        FailYieldStress = true;
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        //  if (ddl_ComplainceNote.SelectedValue == "2")
                        // {
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 415D the minimum yield stress should be 415 N/mm2.");
                        // }

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 415D the minimum yield stress should be 415 N/mm2.");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                case "Fe 500D":
                    if (Math.Round(YST, 2) < 500)
                    {
                        FailYieldStress = true;
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 500D the minimum yield stress should be 500 N/mm2.");
                        //}

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 500D the minimum yield stress should be 500 N/mm2.");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                case "Fe 550D":
                    if (Math.Round(YST, 2) < 550)
                    {
                        FailYieldStress = true;
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 550D the minimum yield stress should be 550 N/mm2.");
                        //}

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 550D the minimum yield stress should be 550 N/mm2.");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                case "Fe 600":
                    if (Math.Round(YST, 2) < 600)
                    {
                        FailYieldStress = true;
                        YSTStr = "$";
                        YSTStr = YSTStr + YST.ToString("000.00");

                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum yield stress");
                        AddRemark("$ - For Fe 600 the minimum yield stress should be 600 N/mm2.");
                        //}

                    }
                    else if (FailYieldStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 600 the minimum yield stress should be 600 N/mm2.");
                        }
                        YSTStr = YSTStr + YST.ToString("000.00");
                    }

                    break;
                    // YST = Convert.ToDecimal(YSTStr);

            }
            if (YSTStr.Contains("$") == true)
                return true;
            else
                return false;

        }

        private bool ChkUltimateStress(decimal UltimateStress, string GradeOfSteel, int r)
        {

            bool FailUltimateStress = false;
            string USTStr = "";
            //AddRowSteelRemark();
            //r = grdSteelRemark.Rows.Count - 1;
            //TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[r].Cells[1].FindControl("txt_REMARK");
            switch (GradeOfSteel)
            {
                case "Fe 250":
                    if (Math.Round(UltimateStress, 2) < 410)
                    {
                        //USTStr = "*****";
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");

                        //               if (ddl_ComplainceNote.SelectedValue == "2")
                        //             {
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 250 the minimum ultimate tensile stress should be more than 10% of the actual yield stress, but should not be less than 410.0 N/mm2.");
                        ///           }
                        FailUltimateStress = true;
                    }
                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 250 the minimum ultimate tensile stress should be more than 10% of the actual yield stress, but should not be less than 410.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
                case "Fe 415":
                    decimal USTal = 0;
                    USTal = UltimateStress * Convert.ToDecimal(0.1);
                    USTal = UltimateStress + USTal;
                    if (Math.Round(UltimateStress, 2) < USTal && Math.Round(UltimateStress, 2) < 485)
                    {
                        FailUltimateStress = true;
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                        //        if (ddl_ComplainceNote.SelectedValue == "2")
                        //      {
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 415 the minimum ultimate tensile stress should be more than 10% of the actual yield stress, but should not be less than 485.0 N/mm2.");
                        //    }
                    }

                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 415 the minimum ultimate tensile stress should be more than 10% of the actual yield stress, but should not be less than 485.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
                case "Fe 500":
                    USTal = UltimateStress * Convert.ToDecimal(0.08);
                    USTal = UltimateStress + USTal;
                    if (Math.Round(UltimateStress, 2) < USTal && Math.Round(UltimateStress, 2) < 545)
                    {
                        FailUltimateStress = true;
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                        //  if (ddl_ComplainceNote.SelectedValue == "2")
                        // {
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 500 the minimum ultimate tensile stress should be more than 8% of the actual yield stress, but should not be less than 545.0 N/mm2.");
                        // }
                    }

                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 500 the minimum ultimate tensile stress should be more than 8% of the actual yield stress, but should not be less than 545.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
                case "Fe 550":
                    USTal = UltimateStress * Convert.ToDecimal(0.06);
                    USTal = UltimateStress + USTal;
                    if (Math.Round(UltimateStress, 2) < USTal && Math.Round(UltimateStress, 2) < 585)
                    {
                        FailUltimateStress = true;
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 550 the minimum ultimate tensile stress should be more than 6% of the actual yield stress, but should not be less than 585.0 N/mm2.");
                        //}
                    }

                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 550 the minimum ultimate tensile stress should be more than 6% of the actual yield stress, but should not be less than 585.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
                case "Fe 415D":
                    USTal = UltimateStress * Convert.ToDecimal(0.12);
                    USTal = UltimateStress + USTal;
                    if (Math.Round(UltimateStress, 2) < USTal && Math.Round(UltimateStress, 2) < 500)
                    {
                        FailUltimateStress = true;
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 415D the minimum ultimate tensile stress should be more than 12% of the actual yield stress, but should not be less than 500.0 N/mm2.");
                        //}
                    }

                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 415D the minimum ultimate tensile stress should be more than 12% of the actual yield stress, but should not be less than 500.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
                case "Fe 500D":
                    USTal = UltimateStress * Convert.ToDecimal(0.1);
                    USTal = UltimateStress + USTal;
                    if (Math.Round(UltimateStress, 2) < USTal && Math.Round(UltimateStress, 2) < 565)
                    {
                        FailUltimateStress = true;
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        // {
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 500D the minimum ultimate tensile stress should be more than 10% of the actual yield stress, but should not be less than 565.0 N/mm2.");
                        //}
                    }

                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 500D the minimum ultimate tensile stress should be more than 10% of the actual yield stress, but should not be less than 565.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
                case "Fe 550D":
                    USTal = UltimateStress * Convert.ToDecimal(0.08);
                    USTal = UltimateStress + USTal;
                    if (Math.Round(UltimateStress, 2) < USTal && Math.Round(UltimateStress, 2) < 600)
                    {
                        FailUltimateStress = true;
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 550D the minimum ultimate tensile stress should be more than 8% of the actual yield stress, but should not be less than 600.0 N/mm2.");
                        //}
                    }

                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 550D the minimum ultimate tensile stress should be more than 8% of the actual yield stress, but should not be less than 600.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
                case "Fe 600":
                    USTal = UltimateStress * Convert.ToDecimal(0.06);
                    USTal = UltimateStress + USTal;
                    if (Math.Round(UltimateStress, 2) < USTal && Math.Round(UltimateStress, 2) < 660)
                    {
                        FailUltimateStress = true;
                        USTStr = "~";
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                        //if (ddl_ComplainceNote.SelectedValue == "2")
                        //{
                        DeleteRemark("the minimum ultimate tensile stress");
                        AddRemark("~ - For Fe 600D the minimum ultimate tensile stress should be more than 6% of the actual yield stress, but should not be less than 660.0 N/mm2.");
                        //}
                    }

                    else if (FailUltimateStress == false)
                    {
                        if (ddl_ComplainceNote.SelectedValue == "2")
                        {
                            AddRemark("For Fe 600D the minimum ultimate tensile stress should be more than 6% of the actual yield stress, but should not be less than 660.0 N/mm2.");
                        }
                        USTStr = USTStr + UltimateStress.ToString("000.00");
                    }

                    break;
            }
            if (USTStr.Contains("~") == true)
                return true;
            else
                return false;
        }

        public void DisplayGrid()
        {
            for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
            {
                grdSteelEntry.Rows[i].Cells[3].Visible = false;
                grdSteelEntry.Rows[i].Cells[4].Visible = false;
                grdSteelEntry.Rows[i].Cells[5].Visible = false;
                grdSteelEntry.Rows[i].Cells[6].Visible = false;
                grdSteelEntry.Rows[i].Cells[7].Visible = false;
                grdSteelEntry.Rows[i].Cells[8].Visible = false;
                grdSteelEntry.Rows[i].Cells[9].Visible = false;
                grdSteelEntry.HeaderRow.Cells[3].Visible = false;
                grdSteelEntry.HeaderRow.Cells[4].Visible = false;
                grdSteelEntry.HeaderRow.Cells[5].Visible = false;
                grdSteelEntry.HeaderRow.Cells[6].Visible = false;
                grdSteelEntry.HeaderRow.Cells[7].Visible = false;
                grdSteelEntry.HeaderRow.Cells[8].Visible = false;
                grdSteelEntry.HeaderRow.Cells[9].Visible = false;

            }
        }
        public void DisplayCalGrid()
        {
            for (int i = 0; i < grdSteelCalulated.Rows.Count; i++)
            {
                grdSteelCalulated.Rows[i].Cells[1].Visible = false;
                grdSteelCalulated.Rows[i].Cells[2].Visible = false;
                grdSteelCalulated.Rows[i].Cells[3].Visible = false;
                grdSteelCalulated.Rows[i].Cells[4].Visible = false;
                grdSteelCalulated.Rows[i].Cells[5].Visible = false;
                grdSteelCalulated.Rows[i].Cells[6].Visible = false;
                grdSteelCalulated.Rows[i].Cells[7].Visible = false;
                grdSteelCalulated.HeaderRow.Cells[1].Visible = false;
                grdSteelCalulated.HeaderRow.Cells[2].Visible = false;
                grdSteelCalulated.HeaderRow.Cells[3].Visible = false;
                grdSteelCalulated.HeaderRow.Cells[4].Visible = false;
                grdSteelCalulated.HeaderRow.Cells[5].Visible = false;
                grdSteelCalulated.HeaderRow.Cells[6].Visible = false;
                grdSteelCalulated.HeaderRow.Cells[7].Visible = false;

            }
        }
        public void DisplayCalCulatedGrid()
        {
            DisplayCalGrid();
            int j = 0;
            for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
            {

                TextBox txt_IdMark = (TextBox)grdSteelCalulated.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_CsArea = (TextBox)grdSteelCalulated.Rows[i].Cells[2].FindControl("txt_CsArea");
                TextBox txt_WtMeter = (TextBox)grdSteelCalulated.Rows[i].Cells[3].FindControl("txt_WtMeter");
                TextBox txt_AvgWtMeter = (TextBox)grdSteelCalulated.Rows[i].Cells[4].FindControl("txt_AvgWtMeter");
                TextBox txt_Elongation = (TextBox)grdSteelCalulated.Rows[i].Cells[5].FindControl("txt_Elongation");
                TextBox txt_YieldStress = (TextBox)grdSteelCalulated.Rows[i].Cells[6].FindControl("txt_YieldStress");
                TextBox txt_UltimateStress = (TextBox)grdSteelCalulated.Rows[i].Cells[7].FindControl("txt_UltimateStress");
                if (grdSteelEntry.Rows[i].Cells[2].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[1].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[1].Visible = true;
                }
                if (grdSteelEntry.Rows[i].Cells[3].Visible == true &&
                    grdSteelEntry.Rows[i].Cells[4].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[1].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[1].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[2].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[2].Visible = true;

                    grdSteelCalulated.Rows[j].Cells[3].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[3].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[4].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[4].Visible = true;
                }
                if (grdSteelEntry.Rows[i].Cells[6].Visible == true &&
                   grdSteelEntry.Rows[i].Cells[7].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[1].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[1].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[2].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[2].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[6].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[6].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[7].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[7].Visible = true;
                }
                if (grdSteelEntry.Rows[i].Cells[3].Visible == true &&
                   grdSteelEntry.Rows[i].Cells[4].Visible == true &&
                    grdSteelEntry.Rows[i].Cells[5].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[3].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[3].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[4].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[4].Visible = true;
                }
                if (grdSteelEntry.Rows[i].Cells[5].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[5].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[5].Visible = true;
                }
                if (grdSteelEntry.Rows[i].Cells[8].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[1].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[1].Visible = true;
                }
                if (grdSteelEntry.Rows[i].Cells[9].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[1].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[1].Visible = true;
                }
                if (grdSteelEntry.Rows[i].Cells[3].Visible == true && grdSteelEntry.Rows[i].Cells[4].Visible == true && grdSteelEntry.Rows[i].Cells[5].Visible == true 
                    && grdSteelEntry.Rows[i].Cells[6].Visible == true && grdSteelEntry.Rows[i].Cells[7].Visible == true)
                {
                    grdSteelCalulated.Rows[j].Cells[2].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[3].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[4].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[5].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[6].Visible = true;
                    grdSteelCalulated.Rows[j].Cells[7].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[2].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[3].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[4].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[5].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[6].Visible = true;
                    grdSteelCalulated.HeaderRow.Cells[7].Visible = true;
                }
                if (txt_IdMark.Visible == true)
                {
                    txt_IdMark.Width = 860;
                }
                if (txt_IdMark.Visible == true && txt_CsArea.Visible == true)
                {
                    //txt_IdMark.Width = 450;
                    //txt_CsArea.Width = 400;

                    txt_IdMark.Width = 240;
                    txt_CsArea.Width = 150;
                    txt_WtMeter.Width = 150;
                    txt_AvgWtMeter.Width = 150;
                }
                if (txt_IdMark.Visible == true && txt_CsArea.Visible == true &&
                    txt_YieldStress.Visible == true &&
                    txt_UltimateStress.Visible == true)
                {
                    txt_IdMark.Width = 240;
                    txt_CsArea.Width = 200;
                    txt_YieldStress.Width = 200;
                    txt_UltimateStress.Width = 200;
                }
                if (txt_IdMark.Visible == true && txt_CsArea.Visible == true && txt_WtMeter.Visible == true && txt_AvgWtMeter.Visible == true
                     && txt_Elongation.Visible == true)
                {
                    txt_IdMark.Width = 240;
                    txt_CsArea.Width = 150;
                    txt_WtMeter.Width = 150;
                    txt_AvgWtMeter.Width = 150;
                    txt_Elongation.Width = 150;

                }
                if (txt_IdMark.Visible == true && txt_CsArea.Visible == true && txt_WtMeter.Visible == true && txt_AvgWtMeter.Visible == true
                    && txt_Elongation.Visible == true && txt_YieldStress.Visible == true &&
                    txt_UltimateStress.Visible == true)
                {
                    txt_IdMark.Width = 150;
                    txt_CsArea.Width = 120;
                    txt_WtMeter.Width = 120;
                    txt_AvgWtMeter.Width = 100;
                    txt_Elongation.Width = 100;
                    txt_YieldStress.Width = 100;
                    txt_UltimateStress.Width = 150;
                }


                j++;
            }
        }

        public void DisplayChkTestName()
        {
            DisplayGrid();
            var stInwd = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "ST");
            foreach (var st in stInwd)
            {
                for (int i = 0; i < grdSteelEntry.Rows.Count; i++)
                {
                    TextBox txt_Dia = (TextBox)grdSteelEntry.Rows[i].Cells[1].FindControl("txt_Dia");
                    TextBox txt_IdMark = (TextBox)grdSteelEntry.Rows[i].Cells[2].FindControl("txt_IdMark");
                    TextBox txt_Weight = (TextBox)grdSteelEntry.Rows[i].Cells[3].FindControl("txt_Weight");
                    TextBox txt_Length = (TextBox)grdSteelEntry.Rows[i].Cells[4].FindControl("txt_Length");
                    TextBox txt_FinalLength = (TextBox)grdSteelEntry.Rows[i].Cells[5].FindControl("txt_FinalLength");
                    TextBox txt_YieldLoad = (TextBox)grdSteelEntry.Rows[i].Cells[6].FindControl("txt_YieldLoad");
                    TextBox txt_UltiMateLoad = (TextBox)grdSteelEntry.Rows[i].Cells[7].FindControl("txt_UltiMateLoad");
                    DropDownList ddl_Rebend = (DropDownList)grdSteelEntry.Rows[i].Cells[8].FindControl("ddl_Rebend");
                    DropDownList ddl_Bend = (DropDownList)grdSteelEntry.Rows[i].Cells[9].FindControl("ddl_Bend");

                    if (st.TEST_Sr_No == 3)
                    {
                        grdSteelEntry.Rows[i].Cells[0].Visible = true;
                        grdSteelEntry.Rows[i].Cells[1].Visible = true;
                        grdSteelEntry.Rows[i].Cells[2].Visible = true;
                        grdSteelEntry.Rows[i].Cells[8].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[8].Visible = true;

                        grdSteelEntry.Rows[i].Cells[1].Width = 200;
                        grdSteelEntry.Rows[i].Cells[2].Width = 450;
                        grdSteelEntry.Rows[i].Cells[8].Width = 200;
                        txt_Dia.Width = 200;
                        txt_IdMark.Width = 450;
                        ddl_Rebend.Width = 200;

                    }
                    if (st.TEST_Sr_No == 5)
                    {
                        grdSteelEntry.Rows[i].Cells[0].Visible = true;
                        grdSteelEntry.Rows[i].Cells[1].Visible = true;
                        grdSteelEntry.Rows[i].Cells[2].Visible = true;
                        grdSteelEntry.Rows[i].Cells[9].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[9].Visible = true;

                        grdSteelEntry.Rows[i].Cells[1].Width = 200;
                        grdSteelEntry.Rows[i].Cells[2].Width = 450;
                        grdSteelEntry.Rows[i].Cells[9].Width = 200;
                        txt_Dia.Width = 200;
                        txt_IdMark.Width = 450;
                        ddl_Bend.Width = 200;
                    }

                    if (st.TEST_Sr_No == 2 || st.TEST_Sr_No == 6 || st.TEST_Sr_No == 7 || st.TEST_Sr_No == 8)
                    {
                        grdSteelEntry.Rows[i].Cells[0].Visible = true;
                        grdSteelEntry.Rows[i].Cells[1].Visible = true;
                        grdSteelEntry.Rows[i].Cells[2].Visible = true;
                        grdSteelEntry.Rows[i].Cells[3].Visible = true;
                        grdSteelEntry.Rows[i].Cells[4].Visible = true;
                        grdSteelEntry.Rows[i].Cells[5].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[3].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[4].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[5].Visible = true;

                        grdSteelEntry.Rows[i].Cells[1].Width = 100;
                        grdSteelEntry.Rows[i].Cells[2].Width = 200;
                        grdSteelEntry.Rows[i].Cells[3].Width = 100;
                        grdSteelEntry.Rows[i].Cells[4].Width = 100;
                        grdSteelEntry.Rows[i].Cells[5].Width = 100;
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 200;
                        txt_Weight.Width = 100;
                        txt_Length.Width = 100;
                        txt_FinalLength.Width = 100;
                    }

                    if (st.TEST_Sr_No == 4)
                    {
                        grdSteelEntry.Rows[i].Cells[0].Visible = true;
                        grdSteelEntry.Rows[i].Cells[1].Visible = true;
                        grdSteelEntry.Rows[i].Cells[2].Visible = true;
                        grdSteelEntry.Rows[i].Cells[3].Visible = true;
                        grdSteelEntry.Rows[i].Cells[4].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[3].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[4].Visible = true;

                        grdSteelEntry.Rows[i].Cells[1].Width = 100;
                        grdSteelEntry.Rows[i].Cells[2].Width = 100;
                        grdSteelEntry.Rows[i].Cells[3].Width = 100;
                        grdSteelEntry.Rows[i].Cells[4].Width = 100;
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 100;
                        txt_Weight.Width = 100;
                        txt_Length.Width = 100;

                    }
                    if (st.TEST_Sr_No == 1 || st.TEST_Sr_No == 6 || st.TEST_Sr_No == 7 || st.TEST_Sr_No == 8)
                    {
                        grdSteelEntry.Rows[i].Cells[0].Visible = true;
                        grdSteelEntry.Rows[i].Cells[1].Visible = true;
                        grdSteelEntry.Rows[i].Cells[2].Visible = true;
                        grdSteelEntry.Rows[i].Cells[3].Visible = true;
                        grdSteelEntry.Rows[i].Cells[4].Visible = true;
                        grdSteelEntry.Rows[i].Cells[6].Visible = true;
                        grdSteelEntry.Rows[i].Cells[7].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[3].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[4].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[6].Visible = true;
                        grdSteelEntry.HeaderRow.Cells[7].Visible = true;

                        grdSteelEntry.Rows[i].Cells[1].Width = 80;
                        grdSteelEntry.Rows[i].Cells[2].Width = 100;
                        grdSteelEntry.Rows[i].Cells[3].Width = 80;
                        grdSteelEntry.Rows[i].Cells[4].Width = 80;
                        grdSteelEntry.Rows[i].Cells[6].Width = 80;
                        grdSteelEntry.Rows[i].Cells[7].Width = 100;
                        txt_Dia.Width = 80;
                        txt_IdMark.Width = 100;
                        txt_Weight.Width = 80;
                        txt_Length.Width = 80;
                        txt_YieldLoad.Width = 80;
                        txt_UltiMateLoad.Width = 100;

                    }
                    if (txt_Dia.Visible == true &&
                       txt_IdMark.Visible == true &&
                       ddl_Bend.Visible == true &&
                       ddl_Rebend.Visible == true
                    )
                    {
                        txt_Dia.Width = 90;
                        txt_IdMark.Width = 400;
                        ddl_Bend.Width = 200;
                        ddl_Rebend.Width = 200;
                    }

                    if (txt_Dia.Visible == true &&
                      txt_IdMark.Visible == true &&
                      txt_Weight.Visible == true &&
                      txt_Length.Visible == true
                     )
                    {
                        txt_Dia.Width = 90;
                        txt_IdMark.Width = 400;
                        txt_Weight.Width = 200;
                        txt_Length.Width = 200;

                        if (ddl_Bend.Visible == true && ddl_Rebend.Visible == true)
                        {
                            ddl_Bend.Width = 100;
                            ddl_Rebend.Width = 100;
                            txt_Dia.Width = 80;
                            txt_IdMark.Width = 200;
                            txt_Weight.Width = 100;
                            txt_Length.Width = 100;
                        }
                        if (ddl_Bend.Visible == true)
                        {
                            txt_Dia.Width = 100;
                            ddl_Bend.Width = 230;
                            txt_IdMark.Width = 260;
                            txt_Weight.Width = 150;
                            txt_Length.Width = 150;
                        }
                        if (ddl_Rebend.Visible == true)
                        {
                            txt_Dia.Width = 100;
                            txt_IdMark.Width = 240;
                            txt_Weight.Width = 150;
                            txt_Length.Width = 150;
                            ddl_Rebend.Width = 200;
                        }
                    }
                    if (txt_Dia.Visible == true &&
                     txt_IdMark.Visible == true &&
                     txt_Weight.Visible == true &&
                     txt_Length.Visible == true &&
                     txt_FinalLength.Visible == true
                    )
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 250;
                        txt_Weight.Width = 150;
                        txt_Length.Width = 150;
                        txt_FinalLength.Width = 200;
                        if (ddl_Rebend.Visible == true)
                        {
                            ddl_Rebend.Width = 150;
                            txt_Dia.Width = 100;
                            txt_IdMark.Width = 230;
                            txt_Weight.Width = 150;
                            txt_Length.Width = 150;
                            txt_FinalLength.Width = 100;
                        }
                    }
                    if (txt_Dia.Visible == true &&
                       txt_IdMark.Visible == true &&
                       txt_Weight.Visible == true &&
                       txt_Length.Visible == true &&
                       txt_FinalLength.Visible == true &&
                       ddl_Bend.Visible == true)
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 240;
                        txt_Weight.Width = 100;
                        txt_Length.Width = 100;
                        txt_FinalLength.Width = 140;
                        ddl_Bend.Width = 200;
                    }
                    if (txt_Dia.Visible == true &&
                        txt_IdMark.Visible == true &&
                        txt_Weight.Visible == true &&
                        txt_Length.Visible == true &&
                        txt_YieldLoad.Visible == true &&
                        txt_UltiMateLoad.Visible == true
                     )
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 200;
                        txt_Weight.Width = 120;
                        txt_Length.Width = 120;
                        txt_YieldLoad.Width = 150;
                        txt_UltiMateLoad.Width = 150;

                        if (ddl_Rebend.Visible == true)
                        {
                            ddl_Rebend.Width = 100;
                            txt_Dia.Width = 80;
                            txt_IdMark.Width = 200;
                            txt_Weight.Width = 100;
                            txt_Length.Width = 100;
                            txt_YieldLoad.Width = 150;
                            txt_UltiMateLoad.Width = 150;
                        }
                        if (ddl_Bend.Visible == true)
                        {
                            ddl_Bend.Width = 100;
                            txt_Dia.Width = 80;
                            txt_IdMark.Width = 200;
                            txt_Weight.Width = 100;
                            txt_Length.Width = 100;
                            txt_YieldLoad.Width = 150;
                            txt_UltiMateLoad.Width = 150;
                        }
                        if (ddl_Rebend.Visible == true && ddl_Bend.Visible == true)
                        {
                            ddl_Rebend.Width = 100;
                            ddl_Bend.Width = 100;
                            txt_Dia.Width = 100;
                            txt_IdMark.Width = 160;
                            txt_Weight.Width = 100;
                            txt_Length.Width = 100;
                            txt_YieldLoad.Width = 110;
                            txt_UltiMateLoad.Width = 110;
                        }
                        if (txt_FinalLength.Visible == true)
                        {
                            txt_FinalLength.Width = 100;
                        }

                    }
                    if (txt_Dia.Visible == true &&
                      txt_IdMark.Visible == true &&
                      txt_Weight.Visible == true &&
                      txt_Length.Visible == true &&
                      ddl_Bend.Visible == true &&
                      ddl_Rebend.Visible == true)
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 205;
                        txt_Weight.Width = 140;
                        txt_Length.Width = 140;
                        ddl_Bend.Width = 150;
                        ddl_Rebend.Width = 150;
                    }
                    if (txt_Dia.Visible == true &&
                        txt_IdMark.Visible == true &&
                        txt_Weight.Visible == true &&
                        txt_Length.Visible == true &&
                        txt_FinalLength.Visible == true &&
                        ddl_Rebend.Visible == true &&
                        ddl_Bend.Visible == true)
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 200;
                        txt_Weight.Width = 100;
                        txt_Length.Width = 100;
                        txt_FinalLength.Width = 100;
                        ddl_Bend.Width = 140;
                        ddl_Rebend.Width = 140;
                    }
                    if (txt_Dia.Visible == true &&
                    txt_IdMark.Visible == true &&
                    txt_Weight.Visible == true &&
                    txt_Length.Visible == true &&
                    txt_YieldLoad.Visible == true &&
                    txt_FinalLength.Visible == true &&
                    txt_UltiMateLoad.Visible == true
                   )
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 200;
                        txt_Weight.Width = 100;
                        txt_Length.Width = 100;
                        txt_FinalLength.Width = 100;
                        txt_YieldLoad.Width = 130;
                        txt_UltiMateLoad.Width = 130;
                    }
                    if (txt_Dia.Visible == true &&
                         txt_IdMark.Visible == true &&
                         txt_Weight.Visible == true &&
                         txt_Length.Visible == true &&
                         txt_YieldLoad.Visible == true &&
                         txt_UltiMateLoad.Visible == true &&
                         ddl_Rebend.Visible == true &&
                         ddl_Bend.Visible == true
                    )
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 150;
                        txt_Weight.Width = 100;
                        txt_Length.Width = 100;
                        txt_YieldLoad.Width = 100;
                        txt_UltiMateLoad.Width = 100;
                        ddl_Rebend.Width = 115;
                        ddl_Bend.Width = 115;
                    }
                    if (txt_Dia.Visible == true &&
                      txt_IdMark.Visible == true &&
                      txt_Weight.Visible == true &&
                      txt_Length.Visible == true &&
                      txt_YieldLoad.Visible == true &&
                      txt_FinalLength.Visible == true &&
                      txt_UltiMateLoad.Visible == true &&
                      ddl_Rebend.Visible == true
                     )
                    {
                        txt_Dia.Width = 100;
                        txt_IdMark.Width = 140;
                        txt_Weight.Width = 100;
                        txt_Length.Width = 100;
                        txt_FinalLength.Width = 100;
                        txt_YieldLoad.Width = 100;
                        txt_UltiMateLoad.Width = 100;
                        ddl_Rebend.Width = 140;
                    }
                    if (txt_Dia.Visible == true &&
                       txt_IdMark.Visible == true &&
                       txt_Weight.Visible == true &&
                       txt_Length.Visible == true &&
                       txt_YieldLoad.Visible == true &&
                       txt_FinalLength.Visible == true &&
                       txt_UltiMateLoad.Visible == true &&
                       ddl_Rebend.Visible == true &&
                       ddl_Bend.Visible == true)
                    {

                        txt_Dia.Width = 80;
                        txt_IdMark.Width = 140;
                        txt_Weight.Width = 80;
                        txt_Length.Width = 80;
                        txt_FinalLength.Width = 100;
                        txt_YieldLoad.Width = 100;
                        txt_UltiMateLoad.Width = 100;
                        ddl_Rebend.Width = 100;
                        ddl_Bend.Width = 100;
                    }
                }
            }
        }
        
        #region Steel remark
        protected void AddRowSteelRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SteelRemarkTable"] != null)
            {
                GetCurrentDataSteelRemark();
                dt = (DataTable)ViewState["SteelRemarkTable"];
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

            ViewState["SteelRemarkTable"] = dt;
            grdSteelRemark.DataSource = dt;
            grdSteelRemark.DataBind();
            SetPreviousDataSteelRemark();
        }
        protected void GetCurrentDataSteelRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdSteelRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SteelRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataSteelRemark()
        {
            DataTable dt = (DataTable)ViewState["SteelRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdSteelRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowSteelRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdSteelRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdSteelRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowSteelRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowSteelRemark(int rowIndex)
        {
            GetCurrentDataSteelRemark();
            DataTable dt = ViewState["SteelRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SteelRemarkTable"] = dt;
            grdSteelRemark.DataSource = dt;
            grdSteelRemark.DataBind();
            SetPreviousDataSteelRemark();
        }
        public void DeleteRowRemark()
        {
            if (grdSteelRemark.Rows.Count > 1)
            {
                for (int i = grdSteelRemark.Rows.Count - 1; i > 0; i--)
                {
                    TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text == "")
                    {
                        DeleteRowSteelRemark(i);
                    }
                }
            }
        }
        public void AddRemark(string strRemark)
        {
            bool addFlag = true;
            if (grdSteelRemark.Rows.Count > 0)
            {
                for (int i = 0; i < grdSteelRemark.Rows.Count;i++ )
                {
                    TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text == strRemark)
                    {
                        addFlag = false;
                        break;
                    }
                    else if ((txt_REMARK.Text.Contains("The average weight/meter") == true && strRemark.Contains("The average weight/meter") == true)
                        || (txt_REMARK.Text.Contains("The individual weight/meter") == true && strRemark.Contains("The individual weight/meter") == true)
                        || (txt_REMARK.Text.Contains("the minimum percentage elongation") == true && strRemark.Contains("the minimum percentage elongation") == true)
                        || (txt_REMARK.Text.Contains("the minimum ultimate tensile stress") == true && strRemark.Contains("the minimum ultimate tensile stress") == true)
                        || (txt_REMARK.Text.Contains("the minimum yield stress") == true && strRemark.Contains("the minimum yield stress") == true)
                        || (txt_REMARK.Text.Contains("the minimum 0.2 % proof stress") == true && strRemark.Contains("the minimum 0.2 % proof stress") == true)
                        || (txt_REMARK.Text.Contains("the minimum yield stress") == true && strRemark.Contains("the minimum yield stress") == true))
                    {
                        addFlag = false;
                        break;
                    }
                }
            }
            if (addFlag == true)
            {
                AddRowSteelRemark();
                TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[grdSteelRemark.Rows.Count-1].Cells[1].FindControl("txt_REMARK");
                txt_REMARK.Text = strRemark;
            }
        }
        public void DeleteRemark(string strRemark)
        {
            if (grdSteelRemark.Rows.Count > 0)
            {
                for (int i = 0; i < grdSteelRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdSteelRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text.Contains(strRemark) ==true)
                    {
                        DeleteRowSteelRemark(i);
                        break;
                    }
                }
            }
        }
        #endregion

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.ST_PDFReport(txt_ReferenceNo.Text lblEntry.Text);
                rpt.PrintSelectedReport(txt_ST.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            // RptSTEntryReport();
        }

        //public void RptSTEntryReport()
        //{
        //    string reportPath;
        //    string reportStr = "";
        //    StreamWriter sw;
        //    PrintHTMLReport rptHtml = new PrintHTMLReport();
        //    reportPath = Server.MapPath("~") + "\\report.html";
        //    sw = File.CreateText(reportPath);
        //    reportStr = rptHtml.getDetailReportST();

        //    sw.WriteLine(reportStr);
        //    sw.Close();
        //    NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

        //}

        //public string getDetailReportST()
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
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Reinforcement Steel/Rebars </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var Steel = dc.ReportStatus_View("Steel Testing", null, null, 0, 0, 0,txt_ReferenceNo.Text, 0, 2, 0);
        //    foreach (var s in Steel)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + s.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td width='2%' height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + s.CL_OfficeAddress_var + "</font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "ST" + "-" + s.STINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2></font></td>" +
        //             "<td width='2%'  height=19><font size=2></font></td>" +
        //             "<td width='40%' height=19><font size=2></font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td width='2%'  height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "ST" + "-" + s.STINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + s.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "ST" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Type Of Steel</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + s.STINWD_SteelType_var.ToString() + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(s.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b> Grade Of Steel</b></font></td>" +
        //              "<td height=19><font size=2></font></td>" +
        //              "<td height=19><font size=2>" + "Fe" + " " + s.STINWD_Grade_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(s.STINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //           "<td width='20%' height=19><font size=2><b>Description</b></font></td>" +
        //           "<td width='2%' height=19><font size=2>:</font></td>" +
        //           "<td width='10%' height=19><font size=2>" + s.STINWD_Description_var.ToString() + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Supplier Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + s.STINWD_SupplierName_var + "</font></td>" +
        //            "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //    int i = 0;
        //    int SrNo = 0;
        //    var SteelDetails = dc.SteelDetailInward_Update(txt_ReferenceNo.Text, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, "", false, true, false);
        //    var count = SteelDetails.Count();
        //    var details = dc.SteelDetailInward_Update(txt_ReferenceNo.Text, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, "", false, true, false);
        //    foreach (var ST in details)
        //    {
        //        if (i == 0)
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2%  align=center rowspan=2 valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2%  align=center valign=medium height=19 ><font size=2><b>Dia. Of bar </b></font></td>";
        //            mySql += "<td width= 5%  align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            if (ST.STDETAIL_CSArea_dec != null && ST.STDETAIL_CSArea_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>C/s Area </b></font></td>";
        //            }
        //            if (ST.STDETAIL_WtMeter_dec != null && ST.STDETAIL_WtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Wt/m </b></font></td>";
        //            }
        //            if (ST.STINWD_AvgWtMeter_dec != null && ST.STINWD_AvgWtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Avg Wt/m </b></font></td>";
        //            }
        //            if (ST.STDETAIL_Rebend_var != null && ST.STDETAIL_Rebend_var != "")
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Rebend Test</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Bend_var != null && ST.STDETAIL_Bend_var != "")
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Bend Test</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Elongation_dec != null && ST.STDETAIL_Elongation_dec != 0)
        //            {
        //                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Elongation</b></font></td>";
        //            }
        //            if (ST.STDETAIL_UltimateStress_dec != null && ST.STDETAIL_UltimateStress_dec != 0 && ST.STDETAIL_YieldStress_dec != null && ST.STDETAIL_YieldStress_dec != 0)
        //            {
        //                mySql += "<td colspan=2 width= 5%  align=center valign=medium height=19 ><font size=2><b>Tensile stress </br> (N/mm <sup>2</sup>) </b></font></td>";
        //            }
        //            mySql += "</tr>";
        //            mySql += "<td  align=center valign=medium height=19 ><font size=2><b> (mm) </b></font></td>";
        //            if (ST.STDETAIL_CSArea_dec != null && ST.STDETAIL_CSArea_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(mm<sup>2</sup>)</b></font></td>";
        //            }
        //            if (ST.STDETAIL_WtMeter_dec != null && ST.STDETAIL_WtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(kg) </b></font></td>";
        //            }
        //            if (ST.STINWD_AvgWtMeter_dec != null && ST.STINWD_AvgWtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(kg)</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Rebend_var != null && ST.STDETAIL_Rebend_var != "")
        //            {
        //                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>(135<sup>0</sup>/157.5<sup>0</sup>)</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Bend_var != null && ST.STDETAIL_Bend_var != "")
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(180<sup>0</sup>) </b></font></td>";
        //            }
        //            if (ST.STDETAIL_Elongation_dec != null && ST.STDETAIL_Elongation_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(%) </b></font></td>";
        //            }
        //            if (ST.STDETAIL_YieldStress_dec != null && ST.STDETAIL_YieldStress_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>0.2% Proof</b></font></td>";
        //            }
        //            if (ST.STDETAIL_UltimateStress_dec != null && ST.STDETAIL_UltimateStress_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Ultimate </b></font></td>";
        //            }
        //            mySql += "</tr>";
        //        }

        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + ST.STINWD_Diameter_tint + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + ST.STDETAIL_IdMark_var + "</font></td>";
        //        if (ST.STDETAIL_CSArea_dec != null && ST.STDETAIL_CSArea_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_CSArea_dec + "</font></td>";
        //        }
        //        if (ST.STDETAIL_WtMeter_dec != null && ST.STDETAIL_WtMeter_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_WtMeter_dec + "</font></td>";
        //        }
        //        if (i == 0)
        //        {
        //            if (ST.STINWD_AvgWtMeter_dec != null && ST.STINWD_AvgWtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5% align=center valign=middle horizontalalign=center rowspan= " + count + " height=19 ><font size=2>" + ST.STINWD_AvgWtMeter_dec + "</font></td>";
        //            }
        //        }
        //        if (ST.STDETAIL_Rebend_var != null && ST.STDETAIL_Rebend_var != "")
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_Rebend_var + "</font></td>";
        //        }
        //        if (ST.STDETAIL_Bend_var != null && ST.STDETAIL_Bend_var != "")
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_Bend_var + "</font></td>";
        //        }
        //        if (ST.STDETAIL_Elongation_dec != null && ST.STDETAIL_Elongation_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_Elongation_dec + "</font></td>";
        //        }
        //        if (ST.STDETAIL_YieldStress_dec != null && ST.STDETAIL_YieldStress_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_YieldStress_dec + "</font></td>";
        //        }
        //        if (ST.STDETAIL_UltimateStress_dec != null && ST.STDETAIL_UltimateStress_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_UltimateStress_dec + "</font></td>";
        //        }
        //        mySql += "</tr>";
        //        i++;
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("ST", "");
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
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }


        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "ST");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.STDetail_RemarkId_int), "ST");
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
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.ST_Remark_var.ToString() + "</font></td>";
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
        //        var RecNo = dc.ReportStatus_View("Steel Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text, 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.STINWD_ApprovedBy_tint != null && r.STINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.STINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.STINWD_CheckedBy_tint, -1, "", "", "");
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

        #region not in use
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Steel Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_SubReports.DataTextField = "STINWD_ReferenceNo_var";
                ddl_SubReports.DataValueField = "STINWD_ReferenceNo_var";
                ddl_SubReports.DataSource = testinguser;
                ddl_SubReports.DataBind();
                ddl_SubReports.Items.Insert(0, "---Select---");
                ListItem itemToRemove = ddl_SubReports.Items.FindByValue(Refno);
                if (itemToRemove != null)
                {
                    ddl_SubReports.Items.Remove(itemToRemove);
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
            var testinguser = dc.ReportStatus_View("Steel Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_SubReports.DataTextField = "STINWD_ReferenceNo_var";
            ddl_SubReports.DataValueField = "STINWD_ReferenceNo_var";
            ddl_SubReports.DataSource = testinguser;
            ddl_SubReports.DataBind();
            ddl_SubReports.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_SubReports.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_SubReports.Items.Remove(itemToRemove);
            }
        }
        private void LoadSubRpt()
        {
            string Refno = txt_ReferenceNo.Text;
            var testinguser = dc.ReportStatus_View("Steel Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_SubReports.DataTextField = "STINWD_ReferenceNo_var";
            ddl_SubReports.DataValueField = "STINWD_ReferenceNo_var";
            ddl_SubReports.DataSource = testinguser;
            ddl_SubReports.DataBind();
            ddl_SubReports.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_SubReports.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_SubReports.Items.Remove(itemToRemove);
            }
        }
        #endregion

    }
}
