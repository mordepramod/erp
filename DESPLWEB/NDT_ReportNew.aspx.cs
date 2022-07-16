using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;

namespace DESPLWEB
{
    public partial class NDT_ReportNew : System.Web.UI.Page
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
                    //if (strReq.Contains("=") == false)
                    //{
                    //    Session.Abandon();
                    //    Response.Redirect("Login.aspx");
                    //}
                    if (strReq.Contains("=") == true)
                    {
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
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "NDT Test - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Session["rowindx"] = 0;
                txt_RecType.Text = "NDT";
                if (txt_RecType.Text != "")
                {
                    lblEntry.Text = "Check";
                    DisplayNDT_Details();
                    DisplayGridNDTData();
                    DisplayRemark();
                    if (lblEntry.Text == "ReEnter")
                    {
                        lblheading.Text = "NDT Test - Report Re-Entry";
                        LoadTestedBy();
                        displayEdited_CorFac();
                    }
                    else if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "NDT Test - Report Check";
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
                        // ViewWitnessBy();
                        displayEdited_CorFac();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
                        //LoadOtherPendingRpt();
                        LoadTestedBy();
                    }
                    LoadReferenceNoList();
                    if (lblEntry.Text == "Enter")
                    {
                        AddAllRemarks();
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
            else if (lblEntry.Text == "ReEnter")
                reportStatus = 3;
            reportStatus = 1;
            var reportList = dc.ReferenceNo_View_StatusWise("NDT", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        private void displayEdited_CorFac()
        {
            Chk_EditedIndStr.Visible = true;
            //lbl_ChkEdited.Visible = true;
            Chk_CorrectionFactor.Visible = true;
            //lbl_chkCorfact.Visible = true;
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
            int inwardStatus = 1;
            if (lblEntry.Text == "ReEnter")
            {
                inwardStatus = 3;
            }
            var testinguser = dc.ReportStatus_View("Non Destructive Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), inwardStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "NDTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "NDTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(txt_ReferenceNo.Text);
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
                var testinguser = dc.ReportStatus_View("Non Destructive Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "NDTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "NDTINWD_ReferenceNo_var";
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
        public void DisplayNDT_Details()
        {
            try
            {
                var InwardNDT = dc.ReportStatus_View("Non Destructive Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var cr in InwardNDT)
                {
                    lblEnquiryNo.Text = cr.INWD_ENQ_Id.ToString();
                    txt_ReferenceNo.Text = cr.NDTINWD_ReferenceNo_var.ToString();
                    txt_RecType.Text = cr.NDTINWD_RecordType_var.ToString();
                    if (Convert.ToString(cr.NDTINWD_TestingDate_dt) != "" && lblEntry.Text != "Enter")
                    {
                        txt_TestingDt.Text = Convert.ToDateTime(cr.NDTINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                    }
                    if (txt_TestingDt.Text == "" || lblEntry.Text == "Enter")
                    {
                        txt_TestingDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (cr.NDTINWD_KindAttention_var != null)
                    {
                        txt_KindAttention.Text = cr.NDTINWD_KindAttention_var.ToString();
                    }
                    txt_ReportNo.Text = cr.NDTINWD_SetOfRecord_var.ToString();
                    ddl_NDTBy.ClearSelection();
                    ddl_NDTBy.Items.FindByText(cr.NDTINWD_NDTBy_var).Selected = true;

                    if (ddl_NablScope.Items.FindByValue(cr.NDTINWD_NablScope_var) != null)
                    {
                        ddl_NablScope.SelectedValue = Convert.ToString(cr.NDTINWD_NablScope_var);
                    }
                    if (Convert.ToString(cr.NDTINWD_NablLocation_int) != null && Convert.ToString(cr.NDTINWD_NablLocation_int) != "")
                    {
                        ddl_NABLLocation.SelectedValue = Convert.ToString(cr.NDTINWD_NablLocation_int);
                    }

                    if (Convert.ToBoolean(cr.NDTINWD_UPVIndirect_bit) == true)
                    {
                        Chk_Indirect.Visible = true;
                        Chk_Indirect.Checked = true;
                    }
                    if (Convert.ToBoolean(cr.NDTINWD_OldGradinig_bit) == true)
                    {
                        ChkOldGrading.Checked = true;
                    }

                    if (cr.NDTINWD_HammerNo_var != null && cr.NDTINWD_HammerNo_var != string.Empty)
                    {
                        ddl_HammerNo.ClearSelection();
                        ddl_HammerNo.Items.FindByText(cr.NDTINWD_HammerNo_var).Selected = true;
                        ddl_HammerNo.Enabled = true;
                    }
                    if (ddl_NDTBy.SelectedItem.Text == "Both" || ddl_NDTBy.SelectedItem.Text == "Rebound Hammer")
                    {
                        ddl_HammerNo.Enabled = true;
                    }
                    else
                    {
                        ddl_HammerNo.Enabled = false;
                    }
                    if (cr.NDTINWD_ClusterAnalysisStatus_bit == true)
                    {
                        chkClusterAnalysis.Checked = true;
                        if (cr.NDTINWD_ClusterAnalysisEquation_var == "Minus 10")
                            optMinus10.Checked = true;
                        else if (cr.NDTINWD_ClusterAnalysisEquation_var == "10")
                            opt10.Checked = true;
                        else if (cr.NDTINWD_ClusterAnalysisEquation_var == "Within 10")
                            optWithin10.Checked = true;
                        else if (cr.NDTINWD_ClusterAnalysisEquation_var == "Shapoorji")
                            optShapoorji.Checked = true;
                    }
                    txt_witnessBy.Visible = false;
                    chk_WitnessBy.Checked = false;
                    if (lblEntry.Text == "Check")
                    {
                        if (cr.NDTINWD_WitnessBy_var != "" && cr.NDTINWD_WitnessBy_var != null)
                        {
                            txt_witnessBy.Visible = true;
                            txt_witnessBy.Text = cr.NDTINWD_WitnessBy_var;
                            chk_WitnessBy.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {

            }
        }

        public void DisplayGridNDTData()
        {
            int i = 0;
            string RowNo = "";
            int RowIndex = 0;
            int k = 0;
            var cr = dc.TestDetail_Title_View(txt_ReferenceNo.Text, 0, "NDT", false);
            foreach (var cor in cr)
            {
                AddRowEnterReportNDTInward(grdNDTInward.Rows.Count);
                TextBox txt_Description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                TextBox txt_Age = (TextBox)grdNDTInward.Rows[i].Cells[10].FindControl("txt_Age");
                TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[i].Cells[11].FindControl("txt_IndStr");
                TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[i].Cells[12].FindControl("txt_ReboundIndexDetails");
                TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                TextBox txt_EditedIndStr = (TextBox)grdNDTInward.Rows[i].Cells[14].FindControl("txt_EditedIndStr");
                TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[i].Cells[15].FindControl("txt_CorrFactor");
                Label lblMergFlag = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");

                TextBox txt_IndStr_10 = (TextBox)grdNDTInward.Rows[i].Cells[17].FindControl("txt_IndStr_10");
                TextBox txt_IndStr10 = (TextBox)grdNDTInward.Rows[i].Cells[18].FindControl("txt_IndStr10");
                TextBox txt_IndStrWithin10 = (TextBox)grdNDTInward.Rows[i].Cells[19].FindControl("txt_IndStrWithin10");
                TextBox txt_IndStrShapoorji = (TextBox)grdNDTInward.Rows[i].Cells[20].FindControl("txt_IndStrShapoorji");
                TextBox txt_IndCorrStr = (TextBox)grdNDTInward.Rows[i].Cells[21].FindControl("txt_IndCorrStr");
                CheckBox chk_IndClusterAnalysis = (CheckBox)grdNDTInward.Rows[i].Cells[22].FindControl("chk_IndClusterAnalysis");
                TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");

                Label lblImage1 = (Label)grdNDTInward.Rows[i].FindControl("lblImage1");
                Label lblImage2 = (Label)grdNDTInward.Rows[i].FindControl("lblImage2");
                Label lblImage3 = (Label)grdNDTInward.Rows[i].FindControl("lblImage3");
                Label lblImage4 = (Label)grdNDTInward.Rows[i].FindControl("lblImage4");
                if (Convert.ToString(cor.Description_var) != "")
                {
                    txt_Description.Text = Convert.ToString(cor.Description_var);
                    lblMergFlag.Text = "0";
                }
                else
                {
                    lblMergFlag.Text = "1";
                    RowNo = RowNo + " " + i;
                    if (Convert.ToString(cor.TitleId_int) != "")
                    {
                        if (Convert.ToInt32(cor.TitleId_int) > 0)
                        {
                            var crr = dc.TestDetail_Title_View(txt_ReferenceNo.Text, Convert.ToInt32(cor.TitleId_int), "NDT", false);
                            foreach (var title in crr)
                            {
                                txt_Description.Text = title.TitleDesc_var.ToString();
                            }
                        }
                    }

                }
                if (Convert.ToString(cor.Grade_var) != "")
                {
                    ddl_Grade.SelectedItem.Text = Convert.ToString(cor.Grade_var);
                }
                if (Convert.ToString(cor.Castingdate_var) != "")
                {
                    txt_CastingDt.Text = Convert.ToString(cor.Castingdate_var);
                }
                if (Convert.ToString(cor.AlphaAngle_var) != "")
                {
                    ddl_AlphaAngle.SelectedItem.Text = Convert.ToString(cor.AlphaAngle_var);
                }
                if (Convert.ToString(cor.ReboundIndex_var) != "")
                {
                    k = 0;
                    string[] Rebound = Convert.ToString(cor.ReboundIndex_var).Split('|');
                    foreach (var RebdIndex in Rebound)
                    {
                        if (RebdIndex != "")
                        {
                            if (k == 0)
                            {
                                ReboundIndex.Text = Convert.ToString(RebdIndex);
                            }
                            k++;
                            if (k == 2)
                            {
                                txt_ReboundIndexDetails.Text = RebdIndex.ToString();
                                break;
                            }
                        }
                    }
                }
                if (Convert.ToString(cor.PulseVelocity_var) != "")
                {
                    k = 0;
                    string[] PulseVelc = Convert.ToString(cor.PulseVelocity_var).Split('|');
                    foreach (var Pulse in PulseVelc)
                    {
                        if (Pulse != "")
                        {
                            if (k == 0)
                            {
                                PulseVel.Text = Convert.ToString(Pulse);
                            }
                            k++;
                            if (k == 2)
                            {
                                txt_PulseVelDetails.Text = Pulse.ToString();
                                break;
                            }
                        }
                    }
                }
                if (Convert.ToString(cor.Age_var) != "")
                {
                    txt_Age.Text = Convert.ToString(cor.Age_var);
                }
                if (Convert.ToString(cor.IndicativeStrength_var) != "")
                {
                    txt_IndStr.Text = Convert.ToString(cor.IndicativeStrength_var);
                }
                if (cor.EditedIndStr_var != null && cor.EditedIndStr_var != "")
                {
                    txt_EditedIndStr.Text = Convert.ToString(cor.EditedIndStr_var);
                    grdNDTInward.Columns[14].Visible = true;
                    Chk_EditedIndStr.Checked = true;
                }
                if (cor.CorrectionFactor_var != null)
                {
                    txt_CorrFactor.Text = Convert.ToString(cor.CorrectionFactor_var);
                    grdNDTInward.Columns[15].Visible = true;
                    Chk_CorrectionFactor.Checked = true;
                    txt_CorectionFact.Visible = true;
                }
                if (chkClusterAnalysis.Checked == true)
                {
                    for (int j = 17; j <= 22; j++)
                    {
                        grdNDTInward.Columns[j].Visible = true;
                    }
                    txt_IndStr_10.Text = cor.IndStrMunus10_var;
                    txt_IndStr10.Text = cor.IndStr10_var;
                    txt_IndStrWithin10.Text = cor.IndStrWithin10_var;
                    txt_IndStrShapoorji.Text = cor.IndStrShapoorji_var;
                    txt_IndCorrStr.Text = cor.CorrStr_var;
                    if (cor.IndStrMunus10_var != "---")
                        chk_IndClusterAnalysis.Checked = true;
                    else
                        chk_IndClusterAnalysis.Checked = false;
                }

                if (Chk_Indirect.Checked == true)
                {
                    grdNDTInward.Columns[23].Visible = true;
                    txt_CorrPulseVal.Text = cor.CorrectedPulseVelocity_var;

                }
                if (Convert.ToString(cor.Image1_var) != null)
                    lblImage1.Text = cor.Image1_var.ToString();
                else
                    lblImage1.Text = "";
                if (Convert.ToString(cor.Image2_var) != null)
                    lblImage2.Text = cor.Image2_var.ToString();
                else
                    lblImage2.Text = "";
                if (Convert.ToString(cor.Image3_var) != null)
                    lblImage3.Text = cor.Image3_var.ToString();
                else
                    lblImage3.Text = "";
                if (Convert.ToString(cor.Image4_var) != null)
                    lblImage4.Text = cor.Image4_var.ToString();
                else
                    lblImage4.Text = "";
                i++;
            }

            if (RowNo != "")
            {
                string[] rowindexx = Convert.ToString(RowNo).Split(' ');
                foreach (var rowx in rowindexx)
                {
                    if (rowx != "")
                    {
                        RowIndex = Convert.ToInt32(rowx);
                        if (RowIndex <= grdNDTInward.Rows.Count - 1)
                        {
                            MergeRow(RowIndex);
                        }
                    }
                }
            }
            Display_CorrFact();
            if (grdNDTInward.Rows.Count <= 0)
            {
                AddRowEnterReportNDTInward(0);
            }
            Display_UPV_ReboundGrid();
        }
        public void Display_CorrFact()
        {
            bool valid = false;
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[i].Cells[15].FindControl("txt_CorrFactor");
                if (txt_CorrFactor.Text != "")
                {
                    valid = true;
                    break;
                }
            }
            if (valid == false)
            {
                grdNDTInward.Columns[15].Visible = false;
                Chk_CorrectionFactor.Checked = false;
                txt_CorectionFact.Visible = false;
            }
        }
        public void Display_UPV_ReboundGrid()
        {

            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                TextBox txt_Age = (TextBox)grdNDTInward.Rows[i].Cells[10].FindControl("txt_Age");
                TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[i].Cells[11].FindControl("txt_IndStr");
                TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");

                grdNDTInward.HeaderRow.Cells[11].Text = "Ind Str(N/mm²)";
                if (ddl_NDTBy.SelectedItem.Text == "Rebound Hammer")
                {
                    if (ddl_NDTBy.SelectedItem.Text == "Rebound Hammer")
                    {
                        grdNDTInward.Columns[9].Visible = false;
                        grdNDTInward.Columns[8].Visible = true;
                        grdNDTInward.Columns[7].Visible = true;
                        grdNDTInward.Columns[23].Visible = false;
                        ReboundIndex.Visible = true;
                        ddl_AlphaAngle.Visible = true;
                        PulseVel.Visible = false;

                        if (Chk_CorrectionFactor.Checked && Chk_EditedIndStr.Checked)
                        {
                            ReboundIndex.Width = 100;
                            if (grdNDTInward.Columns[15].Visible == true)
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 150;
                                ddl_Grade.Width = 70;
                                ddl_AlphaAngle.Width = 70;
                                txt_CastingDt.Width = 80;
                            }
                            else
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 200;
                                ddl_Grade.Width = 80;
                                ddl_AlphaAngle.Width = 100;
                                txt_CastingDt.Width = 100;
                            }
                        }
                        else if (Chk_EditedIndStr.Checked)
                        {
                            if (ddl_Grade.Visible == true)
                            {
                                if (grdNDTInward.Columns[15].Visible == false)
                                {
                                    txt_Description.Width = 200;
                                }
                                else
                                {
                                    if (grdNDTInward.Columns[14].Visible == false)
                                    {
                                        txt_Description.Width = 200;
                                    }
                                    else
                                    {
                                        txt_Description.Width = 220;
                                    }
                                }
                            }
                            ddl_Grade.Width = 80;
                            ddl_AlphaAngle.Width = 100;
                            txt_CastingDt.Width = 100;
                            ReboundIndex.Width = 100;

                        }
                        else if (Chk_CorrectionFactor.Checked)
                        {
                            txt_CastingDt.Width = 100;
                            if (grdNDTInward.Columns[14].Visible == false)
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 200;
                                ddl_Grade.Width = 80;
                                ddl_AlphaAngle.Width = 100;
                                ReboundIndex.Width = 100;
                            }
                            if (grdNDTInward.Columns[15].Visible == false && grdNDTInward.Columns[14].Visible == false)
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 220;
                                ddl_Grade.Width = 100;
                                ddl_AlphaAngle.Width = 110;
                                ReboundIndex.Width = 150;
                            }
                        }
                        else
                        {
                            if (ddl_Grade.Visible == true)
                                txt_Description.Width = 220;
                            ddl_Grade.Width = 100;
                            ddl_AlphaAngle.Width = 110;
                            txt_CastingDt.Width = 100;
                            ReboundIndex.Width = 150;
                        }
                    }
                }
                else if (ddl_NDTBy.SelectedItem.Text == "UPV" || ddl_NDTBy.SelectedItem.Text.Trim() == "UPV with Grading")
                {
                    //grdNDTInward.HeaderRow.Cells[11].Text = "Corrected PV"; //"Ind Str(N/mm²)";
                    if (ddl_NDTBy.SelectedItem.Text == "UPV" || ddl_NDTBy.SelectedItem.Text.Trim() == "UPV with Grading")
                    {
                        if (ddl_NDTBy.SelectedItem.Text.Trim() == "UPV with Grading")
                        {
                            grdNDTInward.HeaderRow.Cells[11].Text = "Concrete Quality Grading";
                        }
                        grdNDTInward.Columns[8].Visible = false;
                        grdNDTInward.Columns[7].Visible = false;
                        grdNDTInward.Columns[9].Visible = true;
                        
                        ReboundIndex.Visible = false;
                        ddl_AlphaAngle.Visible = false;
                        PulseVel.Visible = true;

                        if (Chk_CorrectionFactor.Checked && Chk_EditedIndStr.Checked)
                        {
                            if (grdNDTInward.Columns[15].Visible == true)
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 200;
                                ddl_Grade.Width = 80;
                                txt_CastingDt.Width = 100;
                                PulseVel.Width = 100;
                            }
                            else
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 250;
                                ddl_Grade.Width = 110;
                                txt_CastingDt.Width = 100;
                                PulseVel.Width = 120;
                            }
                        }
                        else if (Chk_EditedIndStr.Checked)
                        {
                            if (ddl_Grade.Visible == true)
                                txt_Description.Width = 250;
                            ddl_Grade.Width = 110;
                            txt_CastingDt.Width = 100;
                            PulseVel.Width = 120;
                        }
                        else if (Chk_CorrectionFactor.Checked)
                        {
                            if (grdNDTInward.Columns[14].Visible == false)
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 250;
                                ddl_Grade.Width = 110;
                                txt_CastingDt.Width = 100;
                                PulseVel.Width = 120;
                            }
                            if (grdNDTInward.Columns[15].Visible == false && grdNDTInward.Columns[14].Visible == false)
                            {
                                if (ddl_Grade.Visible == true)
                                    txt_Description.Width = 250;
                                ddl_Grade.Width = 140;
                                txt_CastingDt.Width = 140;
                                PulseVel.Width = 150;
                            }
                        }
                        else
                        {
                            if (ddl_Grade.Visible == true)
                                txt_Description.Width = 250;
                            ddl_Grade.Width = 140;
                            txt_CastingDt.Width = 140;
                            PulseVel.Width = 155;
                        }
                    }
                }
                else
                {
                    grdNDTInward.Columns[7].Visible = true;
                    grdNDTInward.Columns[8].Visible = true;
                    grdNDTInward.Columns[9].Visible = true;
                    grdNDTInward.Columns[23].Visible = false;
                    ReboundIndex.Visible = true;
                    ddl_AlphaAngle.Visible = true;
                    PulseVel.Visible = true;

                    if (Chk_CorrectionFactor.Checked && Chk_EditedIndStr.Checked)
                    {
                        if (grdNDTInward.Columns[15].Visible == true)
                        {
                            if (ddl_Grade.Visible == true)
                                txt_Description.Width = 100;
                            ddl_Grade.Width = 55;
                            ddl_AlphaAngle.Width = 70;
                            txt_CastingDt.Width = 70;
                            ReboundIndex.Width = 100;
                            PulseVel.Width = 80;
                            txt_IndStr.Width = 100;
                        }
                        else
                        {
                            if (ddl_Grade.Visible == true)
                                txt_Description.Width = 180;
                            ddl_Grade.Width = 60;
                            ddl_AlphaAngle.Width = 70;
                            txt_CastingDt.Width = 70;
                            ReboundIndex.Width = 100;
                            PulseVel.Width = 100;
                            txt_IndStr.Width = 100;
                        }
                    }
                    else if (Chk_EditedIndStr.Checked)
                    {
                        if (ddl_Grade.Visible == true)
                            txt_Description.Width = 160;
                        ddl_Grade.Width = 70;
                        ddl_AlphaAngle.Width = 70;
                        txt_CastingDt.Width = 80;
                        ReboundIndex.Width = 100;
                        PulseVel.Width = 100;
                        txt_IndStr.Width = 100;
                    }
                    else if (Chk_CorrectionFactor.Checked)
                    {
                        if (grdNDTInward.Columns[14].Visible == false)
                        {
                            if (ddl_Grade.Visible == true)
                                txt_Description.Width = 180;
                            ddl_Grade.Width = 60;
                            ddl_AlphaAngle.Width = 70;
                            txt_CastingDt.Width = 70;
                            ReboundIndex.Width = 100;
                            PulseVel.Width = 100;
                            txt_IndStr.Width = 100;
                        }
                        if (grdNDTInward.Columns[15].Visible == false && grdNDTInward.Columns[14].Visible == false)
                        {
                            if (ddl_Grade.Visible == true)
                                txt_Description.Width = 220;
                            ddl_Grade.Width = 80;
                            ddl_AlphaAngle.Width = 100;
                            txt_CastingDt.Width = 80;
                            ReboundIndex.Width = 100;
                            PulseVel.Width = 100;
                            txt_IndStr.Width = 100;
                        }
                    }
                    else
                    {
                        if (ddl_Grade.Visible == true)
                            txt_Description.Width = 240;
                        ddl_Grade.Width = 80;
                        ddl_AlphaAngle.Width = 80;
                        txt_CastingDt.Width = 80;
                        ReboundIndex.Width = 100;
                        PulseVel.Width = 100;
                        txt_IndStr.Width = 100;
                    }

                }

            }

        }
        protected void ddl_NDTBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_NDTBy.SelectedItem.Text == "Both" || ddl_NDTBy.SelectedItem.Text == "Rebound Hammer")
            {
                ddl_HammerNo.Enabled = true;
                Chk_Indirect.Visible = false;
                Chk_Indirect.Checked = false;
            }
            else
            {
                ddl_HammerNo.Enabled = false;
                Chk_Indirect.Visible = true;
            }
            Display_UPV_ReboundGrid();
            if (ddl_NDTBy.SelectedItem.Text == "Both")
            {
                chkClusterAnalysis.Enabled = true;
                optMinus10.Enabled = true;
                opt10.Enabled = true;
                optWithin10.Enabled = true;
                optShapoorji.Enabled = true;

                chkClusterAnalysis.Checked = false;
                optMinus10.Checked = false;
                opt10.Checked = false;
                optWithin10.Checked = false;
                optShapoorji.Checked = false;

                for (int i = 17; i <= 22; i++)
                {
                    grdNDTInward.Columns[i].Visible = true;
                }
            }
            else
            {
                chkClusterAnalysis.Enabled = false;
                optMinus10.Enabled = false;
                opt10.Enabled = false;
                optWithin10.Enabled = false;
                optShapoorji.Enabled = false;

                chkClusterAnalysis.Checked = false;
                optMinus10.Checked = false;
                opt10.Checked = false;
                optWithin10.Checked = false;
                optShapoorji.Checked = false;

                for (int i = 17; i <= 23; i++)
                {
                    grdNDTInward.Columns[i].Visible = false;
                }

                if (Chk_Indirect.Visible == true && Chk_Indirect.Checked == true)
                {
                    for (int i = 0; i < grdNDTInward.Rows.Count; i++)
                    {

                        Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                        TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");

                        txt_CorrPulseVal.Text = PulseVel.Text;
                    }
                }

            }
        }


        protected void AddRowEnterReportNDTInward(int rowIndex)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = grdNDTInward.Rows.Count;
            if (ViewState["NDTTestTable"] != null)
            {
                GetCurrentDataNDTInward();
                dt = (DataTable)ViewState["NDTTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Description", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CastingDt", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_AlphaAngle", typeof(string)));
                dt.Columns.Add(new DataColumn("ReboundIndex", typeof(string)));
                dt.Columns.Add(new DataColumn("PulseVel", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Age", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IndStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_ReboundIndexDetails", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_PulseVelDetails", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_EditedIndStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CorrFactor", typeof(string)));
                dt.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));

                dt.Columns.Add(new DataColumn("txt_IndStr_10", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IndStr10", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IndStrWithin10", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IndStrShapoorji", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IndCorrStr", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_IndClusterAnalysis", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CorrPulseVal", typeof(string)));

                dt.Columns.Add(new DataColumn("lblImage1", typeof(string)));
                dt.Columns.Add(new DataColumn("lblImage2", typeof(string)));
                dt.Columns.Add(new DataColumn("lblImage3", typeof(string)));
                dt.Columns.Add(new DataColumn("lblImage4", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_Description"] = string.Empty;
            dr["ddl_Grade"] = string.Empty;
            dr["txt_CastingDt"] = string.Empty;
            dr["ddl_AlphaAngle"] = string.Empty;
            dr["ReboundIndex"] = string.Empty;
            dr["PulseVel"] = string.Empty;
            dr["txt_Age"] = string.Empty;
            dr["txt_IndStr"] = string.Empty;
            dr["txt_ReboundIndexDetails"] = string.Empty;
            dr["txt_PulseVelDetails"] = string.Empty;
            dr["txt_EditedIndStr"] = string.Empty;
            dr["txt_CorrFactor"] = string.Empty;
            dr["lblMergFlag"] = string.Empty;

            dr["txt_IndStr_10"] = string.Empty;
            dr["txt_IndStr10"] = string.Empty;
            dr["txt_IndStrWithin10"] = string.Empty;
            dr["txt_IndStrShapoorji"] = string.Empty;
            dr["txt_IndCorrStr"] = string.Empty;
            dr["chk_IndClusterAnalysis"] = string.Empty;
            dr["txt_CorrPulseVal"] = string.Empty;

            dr["lblImage1"] = string.Empty;
            dr["lblImage2"] = string.Empty;
            dr["lblImage3"] = string.Empty;
            dr["lblImage4"] = string.Empty;

            if ((rowIndex + 1) != count)
                dt.Rows.InsertAt(dr, rowIndex);
            else
                dt.Rows.Add(dr);
            ViewState["NDTTestTable"] = dt;
            grdNDTInward.DataSource = dt;
            grdNDTInward.DataBind();
            SetPreviousDataNDTInward();

        }
        protected void GetCurrentDataNDTInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Description", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CastingDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_AlphaAngle", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ReboundIndex", typeof(string)));
            dtTable.Columns.Add(new DataColumn("PulseVel", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Age", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IndStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_ReboundIndexDetails", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_PulseVelDetails", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_EditedIndStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CorrFactor", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));

            dtTable.Columns.Add(new DataColumn("txt_IndStr_10", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IndStr10", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IndStrWithin10", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IndStrShapoorji", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IndCorrStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_IndClusterAnalysis", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CorrPulseVal", typeof(string)));

            dtTable.Columns.Add(new DataColumn("lblImage1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblImage2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblImage3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblImage4", typeof(string)));

            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                TextBox txt_Age = (TextBox)grdNDTInward.Rows[i].Cells[10].FindControl("txt_Age");
                TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[i].Cells[11].FindControl("txt_IndStr");
                TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[i].Cells[12].FindControl("txt_ReboundIndexDetails");
                TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                TextBox txt_EditedIndStr = (TextBox)grdNDTInward.Rows[i].Cells[14].FindControl("txt_EditedIndStr");
                TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[i].Cells[15].FindControl("txt_CorrFactor");
                Label lblMergFlag = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");

                TextBox txt_IndStr_10 = (TextBox)grdNDTInward.Rows[i].Cells[17].FindControl("txt_IndStr_10");
                TextBox txt_IndStr10 = (TextBox)grdNDTInward.Rows[i].Cells[18].FindControl("txt_IndStr10");
                TextBox txt_IndStrWithin10 = (TextBox)grdNDTInward.Rows[i].Cells[19].FindControl("txt_IndStrWithin10");
                TextBox txt_IndStrShapoorji = (TextBox)grdNDTInward.Rows[i].Cells[20].FindControl("txt_IndStrShapoorji");
                TextBox txt_IndCorrStr = (TextBox)grdNDTInward.Rows[i].Cells[21].FindControl("txt_IndCorrStr");
                CheckBox chk_IndClusterAnalysis = (CheckBox)grdNDTInward.Rows[i].Cells[22].FindControl("chk_IndClusterAnalysis");
                TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");

                Label lblImage1 = (Label)grdNDTInward.Rows[i].FindControl("lblImage1");
                Label lblImage2 = (Label)grdNDTInward.Rows[i].FindControl("lblImage2");
                Label lblImage3 = (Label)grdNDTInward.Rows[i].FindControl("lblImage3");
                Label lblImage4 = (Label)grdNDTInward.Rows[i].FindControl("lblImage4");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_Description"] = txt_Description.Text;
                drRow["ddl_Grade"] = ddl_Grade.Text;
                drRow["txt_CastingDt"] = txt_CastingDt.Text;
                drRow["PulseVel"] = PulseVel.Text;
                drRow["ddl_AlphaAngle"] = ddl_AlphaAngle.Text;
                drRow["ReboundIndex"] = ReboundIndex.Text;
                drRow["txt_Age"] = txt_Age.Text;
                drRow["txt_IndStr"] = txt_IndStr.Text;
                drRow["txt_ReboundIndexDetails"] = txt_ReboundIndexDetails.Text;
                drRow["txt_PulseVelDetails"] = txt_PulseVelDetails.Text;
                drRow["txt_EditedIndStr"] = txt_EditedIndStr.Text;
                drRow["txt_CorrFactor"] = txt_CorrFactor.Text;
                drRow["lblMergFlag"] = lblMergFlag.Text;

                drRow["txt_IndStr_10"] = txt_IndStr_10.Text;
                drRow["txt_IndStr10"] = txt_IndStr10.Text;
                drRow["txt_IndStrWithin10"] = txt_IndStrWithin10.Text;
                drRow["txt_IndStrShapoorji"] = txt_IndStrShapoorji.Text;
                drRow["txt_IndCorrStr"] = txt_IndCorrStr.Text;
                drRow["chk_IndClusterAnalysis"] = chk_IndClusterAnalysis.Checked;
                drRow["txt_CorrPulseVal"] = txt_CorrPulseVal.Text;

                drRow["lblImage1"] = lblImage1.Text;
                drRow["lblImage2"] = lblImage2.Text;
                drRow["lblImage3"] = lblImage3.Text;
                drRow["lblImage4"] = lblImage4.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["NDTTestTable"] = dtTable;
        }
        protected void SetPreviousDataNDTInward()
        {
            DataTable dt = (DataTable)ViewState["NDTTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                TextBox txt_Age = (TextBox)grdNDTInward.Rows[i].Cells[10].FindControl("txt_Age");
                TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[i].Cells[11].FindControl("txt_IndStr");
                TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[i].Cells[12].FindControl("txt_ReboundIndexDetails");
                TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                TextBox txt_EditedIndStr = (TextBox)grdNDTInward.Rows[i].Cells[14].FindControl("txt_EditedIndStr");
                TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[i].Cells[15].FindControl("txt_CorrFactor");
                Label lblMergFlag = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");

                TextBox txt_IndStr_10 = (TextBox)grdNDTInward.Rows[i].Cells[17].FindControl("txt_IndStr_10");
                TextBox txt_IndStr10 = (TextBox)grdNDTInward.Rows[i].Cells[18].FindControl("txt_IndStr10");
                TextBox txt_IndStrWithin10 = (TextBox)grdNDTInward.Rows[i].Cells[19].FindControl("txt_IndStrWithin10");
                TextBox txt_IndStrShapoorji = (TextBox)grdNDTInward.Rows[i].Cells[20].FindControl("txt_IndStrShapoorji");
                TextBox txt_IndCorrStr = (TextBox)grdNDTInward.Rows[i].Cells[21].FindControl("txt_IndCorrStr");
                CheckBox chk_IndClusterAnalysis = (CheckBox)grdNDTInward.Rows[i].Cells[22].FindControl("chk_IndClusterAnalysis");
                TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");

                Label lblImage1 = (Label)grdNDTInward.Rows[i].FindControl("lblImage1");
                Label lblImage2 = (Label)grdNDTInward.Rows[i].FindControl("lblImage2");
                Label lblImage3 = (Label)grdNDTInward.Rows[i].FindControl("lblImage3");
                Label lblImage4 = (Label)grdNDTInward.Rows[i].FindControl("lblImage4");

                grdNDTInward.Rows[i].Cells[3].Text = (i + 1).ToString();
                txt_Description.Text = dt.Rows[i]["txt_Description"].ToString();
                ddl_Grade.Text = dt.Rows[i]["ddl_Grade"].ToString();
                txt_CastingDt.Text = dt.Rows[i]["txt_CastingDt"].ToString();
                PulseVel.Text = dt.Rows[i]["PulseVel"].ToString();
                ddl_AlphaAngle.Text = dt.Rows[i]["ddl_AlphaAngle"].ToString();
                ReboundIndex.Text = dt.Rows[i]["ReboundIndex"].ToString();
                txt_Age.Text = dt.Rows[i]["txt_Age"].ToString();
                txt_IndStr.Text = dt.Rows[i]["txt_IndStr"].ToString();
                txt_ReboundIndexDetails.Text = dt.Rows[i]["txt_ReboundIndexDetails"].ToString();
                txt_PulseVelDetails.Text = dt.Rows[i]["txt_PulseVelDetails"].ToString();
                txt_EditedIndStr.Text = dt.Rows[i]["txt_EditedIndStr"].ToString();
                txt_CorrFactor.Text = dt.Rows[i]["txt_CorrFactor"].ToString();
                lblMergFlag.Text = dt.Rows[i]["lblMergFlag"].ToString();

                txt_IndStr_10.Text = dt.Rows[i]["txt_IndStr_10"].ToString();
                txt_IndStr10.Text = dt.Rows[i]["txt_IndStr10"].ToString();
                txt_IndStrWithin10.Text = dt.Rows[i]["txt_IndStrWithin10"].ToString();
                txt_IndStrShapoorji.Text = dt.Rows[i]["txt_IndStrShapoorji"].ToString();
                txt_IndCorrStr.Text = dt.Rows[i]["txt_IndCorrStr"].ToString();
                if (dt.Rows[i]["chk_IndClusterAnalysis"].ToString() != "")
                {
                    chk_IndClusterAnalysis.Checked = Convert.ToBoolean(dt.Rows[i]["chk_IndClusterAnalysis"].ToString());
                }
                txt_CorrPulseVal.Text = dt.Rows[i]["txt_CorrPulseVal"].ToString();

                lblImage1.Text = dt.Rows[i]["lblImage1"].ToString();
                lblImage2.Text = dt.Rows[i]["lblImage2"].ToString();
                lblImage3.Text = dt.Rows[i]["lblImage3"].ToString();
                lblImage4.Text = dt.Rows[i]["lblImage4"].ToString();

            }

        }
        protected void imgBtnNDTTestDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdNDTInward.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdNDTInward.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowReportCoreInward(gvr.RowIndex);
                ShowMergeRow();
                Display_UPV_ReboundGrid();
            }
        }
        protected void DeleteRowReportCoreInward(int rowIndex)
        {
            GetCurrentDataNDTInward();
            DataTable dt = ViewState["NDTTestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["NDTTestTable"] = dt;
            grdNDTInward.DataSource = dt;
            grdNDTInward.DataBind();
            SetPreviousDataNDTInward();
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                string PulseVels = "";
                string ReboundIndexs = "";
                string strClusterAnaEq = "";

                DateTime TestingDt = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
                if (ddl_HammerNo.Enabled == false)
                {
                    ddl_HammerNo.SelectedItem.Text = "";
                }
                if (chk_WitnessBy.Checked == false)
                {
                    txt_witnessBy.Text = "";
                }
                if (chkClusterAnalysis.Checked == true)
                {
                    if (optMinus10.Checked == true)
                        strClusterAnaEq = "Minus 10";
                    else if (opt10.Checked == true)
                        strClusterAnaEq = "10";
                    else if (optWithin10.Checked == true)
                        strClusterAnaEq = "Within 10";
                    else if (optShapoorji.Checked == true)
                        strClusterAnaEq = "Shapoorji";
                }
                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.NDT_TestInward_Update(txt_ReferenceNo.Text, 2, TestingDt, txt_KindAttention.Text, ddl_HammerNo.SelectedItem.Text, ddl_NDTBy.SelectedItem.Text, "NDT", txt_witnessBy.Text, chkClusterAnalysis.Checked, strClusterAnaEq, Chk_Indirect.Checked,ChkOldGrading.Checked );
                    dc.ReportDetails_Update("NDT", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "NDT", txt_ReferenceNo.Text, "NDT", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.NDT_TestInward_Update(txt_ReferenceNo.Text, 3, TestingDt, txt_KindAttention.Text, ddl_HammerNo.SelectedItem.Text, ddl_NDTBy.SelectedItem.Text, "NDT", txt_witnessBy.Text, chkClusterAnalysis.Checked, strClusterAnaEq, Chk_Indirect.Checked, ChkOldGrading.Checked);
                    dc.ReportDetails_Update("NDT", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "NDT", txt_ReferenceNo.Text, "NDT", null, false, true, false, false, false, false, false);
                }

                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "NDT", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));


                //if (ddlReferenceBill.Visible ==true && ddlReferenceBill.SelectedIndex > 0)
                //{
                //    dc.Inward_Update_BillNo(Convert.ToInt32( txt_ReportNo.Text.Substring(0, txt_ReportNo.Text.IndexOf('/'))), txt_RecType.Text, ddlReferenceBill.SelectedItem.Text);
                //}
                dc.NDTDetail_Update(txt_ReferenceNo.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, true, "", "", "", "", "", 0);
                dc.Title_Update(txt_ReferenceNo.Text, "", "NDT", 0, true);

                for (int i = 0; i < grdNDTInward.Rows.Count; i++)
                {
                    int TitleId = 0;
                    TextBox txt_description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                    DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                    TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                    DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                    Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                    Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                    TextBox txt_Age = (TextBox)grdNDTInward.Rows[i].Cells[10].FindControl("txt_Age");
                    TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[i].Cells[11].FindControl("txt_IndStr");
                    TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[i].Cells[12].FindControl("txt_ReboundIndexDetails");
                    TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                    TextBox txt_EditedIndStr = (TextBox)grdNDTInward.Rows[i].Cells[14].FindControl("txt_EditedIndStr");
                    TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[i].Cells[15].FindControl("txt_CorrFactor");
                    Label lblMergFlag = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");

                    TextBox txt_IndStr_10 = (TextBox)grdNDTInward.Rows[i].Cells[17].FindControl("txt_IndStr_10");
                    TextBox txt_IndStr10 = (TextBox)grdNDTInward.Rows[i].Cells[18].FindControl("txt_IndStr10");
                    TextBox txt_IndStrWithin10 = (TextBox)grdNDTInward.Rows[i].Cells[19].FindControl("txt_IndStrWithin10");
                    TextBox txt_IndStrShapoorji = (TextBox)grdNDTInward.Rows[i].Cells[20].FindControl("txt_IndStrShapoorji");
                    TextBox txt_IndCorrStr = (TextBox)grdNDTInward.Rows[i].Cells[21].FindControl("txt_IndCorrStr");
                    CheckBox chk_IndClusterAnalysis = (CheckBox)grdNDTInward.Rows[i].Cells[22].FindControl("chk_IndClusterAnalysis");
                    TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");

                    Label lblImage1 = (Label)grdNDTInward.Rows[i].FindControl("lblImage1");
                    Label lblImage2 = (Label)grdNDTInward.Rows[i].FindControl("lblImage2");
                    Label lblImage3 = (Label)grdNDTInward.Rows[i].FindControl("lblImage3");
                    Label lblImage4 = (Label)grdNDTInward.Rows[i].FindControl("lblImage4");

                    if (lblMergFlag.Text != "1")
                    {
                        if (ReboundIndex.Visible == true)
                        {
                            ReboundIndexs = ReboundIndex.Text + "|" + txt_ReboundIndexDetails.Text;
                        }
                        else
                        {
                            ReboundIndexs = "";
                        }
                        if (PulseVel.Visible == true)
                        {
                            PulseVels = PulseVel.Text + "|" + txt_PulseVelDetails.Text;
                        }
                        else
                        {
                            PulseVels = "";
                        }
                        if (chkClusterAnalysis.Checked == false)
                        {
                            txt_IndStr_10.Text = "";
                            txt_IndStr10.Text = "";
                            txt_IndStrWithin10.Text = "";
                            txt_IndStrShapoorji.Text = "";
                            txt_IndCorrStr.Text = "";
                            chk_IndClusterAnalysis.Checked = false;
                        }
                        dc.NDTDetail_Update(txt_ReferenceNo.Text, txt_description.Text, ddl_Grade.SelectedItem.Text, txt_CastingDt.Text, ddl_AlphaAngle.SelectedItem.Text, ReboundIndexs, PulseVels, txt_Age.Text, txt_IndStr.Text, txt_EditedIndStr.Text, txt_CorrFactor.Text, txt_IndStr_10.Text, txt_IndStr10.Text, txt_IndStrWithin10.Text, txt_IndStrShapoorji.Text, txt_IndCorrStr.Text, TitleId, false, txt_CorrPulseVal.Text, lblImage1.Text, lblImage2.Text, lblImage3.Text, lblImage4.Text, 0);
                    }
                    else
                    {
                        dc.Title_Update(txt_ReferenceNo.Text, txt_description.Text, "NDT", 0, false);
                        var t = dc.TestDetail_Title_View(txt_ReferenceNo.Text, 0, "NDT", true);
                        foreach (var tt in t)
                        {
                            TitleId = Convert.ToInt32(tt.TitleId_int);
                        }
                        dc.NDTDetail_Update(txt_ReferenceNo.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", TitleId, false, "", "", "", "", "", 0);
                    }
                }

                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "NDT", true);
                for (int i = 0; i < grdRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "NDT");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.NDT_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "NDT");
                            foreach (var c in chkId)
                            {
                                if (c.NDTDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "NDT", false);
                            }
                            break;
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "NDT");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "NDT");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.NDT_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "NDT", false);
                            }
                        }
                    }
                }
                #endregion
                lnkPrint.Visible = true;

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
                //Calculation();
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
            else if (txt_KindAttention.Text == "")
            {
                lblMsg.Text = "Please Enter Kind Attention";
                valid = false;
            }
            else if (ddl_HammerNo.Enabled == true && ddl_HammerNo.SelectedItem.Text == "Select")
            {
                lblMsg.Text = "Please Select Hammer No.";
                valid = false;
            }
            if (ddl_NablScope.SelectedItem.Text == "--Select--" && valid == true)
            {
                lblMsg.Text = "Select 'NABL Scope'.";
                valid = false;
                ddl_NablScope.Focus();
            }
            else if (ddl_NABLLocation.SelectedItem.Text == "--Select--" && valid == true)
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
            else if ((lblEntry.Text == "Enter" || lblEntry.Text == "ReEnter") && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Tested By";
                valid = false;
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdNDTInward.Rows.Count; i++)
                {
                    TextBox txt_Description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                    DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                    TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                    DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                    Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                    Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");

                    if (txt_Description.Text == "" && txt_Description.Visible == true &&

                             grdNDTInward.Rows[i].Cells[5].Visible == true &&
                             grdNDTInward.Rows[i].Cells[6].Visible == true &&
                             grdNDTInward.Rows[i].Cells[7].Visible == true &&
                             grdNDTInward.Rows[i].Cells[8].Visible == true &&
                             grdNDTInward.Rows[i].Cells[9].Visible == true &&
                             grdNDTInward.Rows[i].Cells[10].Visible == true &&
                             grdNDTInward.Rows[i].Cells[11].Visible == true)
                    {
                        lblMsg.Text = "Enter Description for Sr No. " + (i + 1) + ".";
                        txt_Description.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddl_Grade.SelectedItem.Text == "Select" && ddl_Grade.Visible == true)
                    {
                        lblMsg.Text = "Select Grade for Sr No. " + (i + 1) + ".";
                        valid = false;
                        break;
                    }
                    else if (txt_CastingDt.Text == "" && txt_CastingDt.Visible == true)
                    {
                        lblMsg.Text = "Enter Casting Date for Sr No. " + (i + 1) + ".";
                        txt_CastingDt.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddl_AlphaAngle.SelectedItem.Text == "Select" && ddl_AlphaAngle.Visible == true)
                    {
                        lblMsg.Text = "Select Alpha Angle for Sr No. " + (i + 1) + ".";
                        ddl_AlphaAngle.Focus();
                        valid = false;
                        break;
                    }
                    else if (ReboundIndex.Text == "" && ReboundIndex.Visible == true)
                    {
                        lblMsg.Text = "Enter Rebound Index for Sr No. " + (i + 1) + ".";
                        ReboundIndex.Focus();
                        valid = false;
                        break;
                    }
                    else if (PulseVel.Text == "" && PulseVel.Visible == true)
                    {
                        lblMsg.Text = "Enter Pulse Vel for Sr No. " + (i + 1) + ".";
                        PulseVel.Focus();
                        valid = false;
                        break;
                    }
                    else if (PulseVel.Text == "" && PulseVel.Visible == true)
                    {
                        lblMsg.Text = "Enter Pulse Velocity for Sr No. " + (i + 1) + ".";
                        PulseVel.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Description.Text == "" && txt_Description.Visible == true &&

                            grdNDTInward.Rows[i].Cells[5].Visible == false &&
                            grdNDTInward.Rows[i].Cells[6].Visible == false &&
                            grdNDTInward.Rows[i].Cells[7].Visible == false &&
                            grdNDTInward.Rows[i].Cells[8].Visible == false &&
                            grdNDTInward.Rows[i].Cells[9].Visible == false &&
                            grdNDTInward.Rows[i].Cells[10].Visible == false &&
                            grdNDTInward.Rows[i].Cells[11].Visible == false)
                    {
                        lblMsg.Text = "Enter Title  Sr No. between " + (i) + " and " + (i + 1) + ".";
                        txt_Description.Focus();
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
        protected void ReboundIndex_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            divReboundIndex.Visible = true;
            divPulseVel.Visible = false;
            txt_R1.Focus();
            Chk_NoReboudIndx.Checked = false;
            txt_R1.Text = string.Empty;
            txt_R2.Text = string.Empty;
            txt_R3.Text = string.Empty;
            txt_R4.Text = string.Empty;
            txt_R5.Text = string.Empty;
            txt_R6.Text = string.Empty;
            txt_R7.Text = string.Empty;
            txt_R8.Text = string.Empty;
            txt_R9.Text = string.Empty;
            txt_R10.Text = string.Empty;
            Button lnk = (Button)sender;
            GridViewRow row = (GridViewRow)lnk.Parent.Parent;
            int rowindex = row.RowIndex;

            Button ReboundIndex = (Button)grdNDTInward.Rows[rowindex].Cells[8].FindControl("ReboundIndex");
            TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[rowindex].Cells[12].FindControl("txt_ReboundIndexDetails");
            Session["rowindx"] = Convert.ToInt32(rowindex);
            if (ReboundIndex.Text == "NO")
            {
                Chk_NoReboudIndx.Checked = true;
            }
            if (txt_ReboundIndexDetails.Text != string.Empty)
            {
                int k = 0;
                string[] Tresult = Convert.ToString(txt_ReboundIndexDetails.Text).Split(',');
                foreach (var RE in Tresult)
                {
                    if (k == 0 && txt_R1.Text == string.Empty)
                    {
                        txt_R1.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 1 && txt_R2.Text == string.Empty)
                    {
                        txt_R2.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 2 && txt_R3.Text == string.Empty)
                    {
                        txt_R3.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 3 && txt_R4.Text == string.Empty)
                    {
                        txt_R4.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 4 && txt_R5.Text == string.Empty)
                    {
                        txt_R5.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 5 && txt_R6.Text == string.Empty)
                    {
                        txt_R6.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 6 && txt_R7.Text == string.Empty)
                    {
                        txt_R7.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 7 && txt_R8.Text == string.Empty)
                    {
                        txt_R8.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 8 && txt_R9.Text == string.Empty)
                    {
                        txt_R9.Text = RE.ToString();
                        k++;
                    }
                    else if (k == 9 && txt_R10.Text == string.Empty)
                    {
                        txt_R10.Text = RE.ToString();
                        k++;
                    }
                }
            }
        }
        protected void PulseVel_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            divPulseVel.Visible = true;
            divReboundIndex.Visible = false;
            Chk_NO.Checked = false;
            txt_Distance.Focus();
            txt_Distance.Text = string.Empty;
            txt_Time.Text = string.Empty;
            Button lnk = (Button)sender;
            GridViewRow row = (GridViewRow)lnk.Parent.Parent;
            int rowindex = row.RowIndex;
            Button PulseVel = (Button)grdNDTInward.Rows[rowindex].Cells[9].FindControl("PulseVel");
            TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[rowindex].Cells[13].FindControl("txt_PulseVelDetails");
            Session["rowindx"] = Convert.ToInt32(rowindex);
            if (PulseVel.Text == "NO")
            {
                Chk_NO.Checked = true;
            }
            if (txt_PulseVelDetails.Text != string.Empty)
            {
                int k = 0;
                string[] Tresult = Convert.ToString(txt_PulseVelDetails.Text).Split(',');
                foreach (var PULSE in Tresult)
                {
                    if (k == 0 && txt_Distance.Text == string.Empty)
                    {
                        txt_Distance.Text = PULSE.ToString();
                        k++;
                    }
                    else if (k == 1 && txt_Time.Text == string.Empty)
                    {
                        txt_Time.Text = PULSE.ToString();
                        k++;
                    }
                }
            }
            
        }
        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            if (divReboundIndex.Visible == true)
            {
                double sclavgcnt = 0;
                double Upperlimt = 0;
                double Lowerlimt = 0;
                int Counter = 0;
                double Total = 0;
                Button ReboundIndex = (Button)grdNDTInward.Rows[Convert.ToInt32(Session["rowindx"])].Cells[8].FindControl("ReboundIndex");
                TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[Convert.ToInt32(Session["rowindx"])].Cells[12].FindControl("txt_ReboundIndexDetails");
                TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[Convert.ToInt32(Session["rowindx"])].Cells[15].FindControl("txt_CorrFactor");
                txt_ReboundIndexDetails.Text = string.Empty;
                txt_ReboundIndexDetails.Text = txt_ReboundIndexDetails.Text + txt_R1.Text + "," + txt_R2.Text + "," + txt_R3.Text + "," + txt_R4.Text + "," + txt_R5.Text + "," + txt_R6.Text + "," + txt_R7.Text + "," + txt_R8.Text + "," + txt_R9.Text + "," + txt_R10.Text;

                txt_R1.Text = txt_R1.Text.ToUpper();
                txt_R2.Text = txt_R2.Text.ToUpper();
                txt_R3.Text = txt_R3.Text.ToUpper();
                txt_R4.Text = txt_R4.Text.ToUpper();
                txt_R5.Text = txt_R5.Text.ToUpper();
                txt_R6.Text = txt_R6.Text.ToUpper();
                txt_R7.Text = txt_R7.Text.ToUpper();
                txt_R8.Text = txt_R8.Text.ToUpper();
                txt_R9.Text = txt_R9.Text.ToUpper();
                txt_R10.Text = txt_R10.Text.ToUpper();

                if (Chk_NoReboudIndx.Checked)
                {
                    ReboundIndex.Text = "NO";
                    txt_ReboundIndexDetails.Text = "";
                }
                else
                {
                    if (txt_R1.Text == "NO" && txt_R2.Text == "NO" && txt_R3.Text == "NO" && txt_R4.Text == "NO" && txt_R5.Text == "NO" && txt_R6.Text == "NO" && txt_R7.Text == "NO" && txt_R8.Text == "NO" && txt_R9.Text == "NO" && txt_R10.Text == "NO")
                    {
                        ReboundIndex.Text = "NO";
                        txt_ReboundIndexDetails.Text = "";
                    }
                    if (txt_R1.Text == "" && txt_R2.Text == "" && txt_R3.Text == "" && txt_R4.Text == "" && txt_R5.Text == "" && txt_R6.Text == "" && txt_R7.Text == "" && txt_R8.Text == "" && txt_R9.Text == "" && txt_R10.Text == "")
                    {
                        ReboundIndex.Text = "---";
                        txt_ReboundIndexDetails.Text = "";
                    }
                    if (txt_R1.Text == "NO" || txt_R2.Text == "NO" || txt_R3.Text == "NO" || txt_R4.Text == "NO" || txt_R5.Text == "NO" || txt_R6.Text == "NO" || txt_R7.Text == "NO" || txt_R8.Text == "NO" || txt_R9.Text == "NO" || txt_R10.Text == "NO" ||
                        txt_R1.Text == "" || txt_R2.Text == "" || txt_R3.Text == "" || txt_R4.Text == "" || txt_R5.Text == "" || txt_R6.Text == "" || txt_R7.Text == "" || txt_R8.Text == "" || txt_R9.Text == "" || txt_R10.Text == "")
                    {
                        if (txt_R1.Text == "NO" || txt_R1.Text == "")
                        {
                            txt_R1.Text = "0";
                        }
                        if (txt_R2.Text == "NO" || txt_R2.Text == "")
                        {
                            txt_R2.Text = "0";
                        }
                        if (txt_R3.Text == "NO" || txt_R3.Text == "")
                        {
                            txt_R3.Text = "0";
                        }
                        if (txt_R4.Text == "NO" || txt_R4.Text == "")
                        {
                            txt_R4.Text = "0";
                        }
                        if (txt_R5.Text == "NO" || txt_R5.Text == "")
                        {
                            txt_R5.Text = "0";
                        }
                        if (txt_R6.Text == "NO" || txt_R6.Text == "")
                        {
                            txt_R6.Text = "0";
                        }
                        if (txt_R7.Text == "NO" || txt_R7.Text == "")
                        {
                            txt_R7.Text = "0";
                        }
                        if (txt_R8.Text == "NO" || txt_R8.Text == "")
                        {
                            txt_R8.Text = "0";
                        }
                        if (txt_R9.Text == "NO" || txt_R9.Text == "")
                        {
                            txt_R9.Text = "0";
                        }
                        if (txt_R10.Text == "NO" || txt_R10.Text == "")
                        {
                            txt_R10.Text = "0";
                        }
                    }
                    sclavgcnt = ((Convert.ToDouble(txt_R1.Text) + Convert.ToDouble(txt_R2.Text) + Convert.ToDouble(txt_R3.Text) + Convert.ToDouble(txt_R4.Text) + Convert.ToDouble(txt_R5.Text) + Convert.ToDouble(txt_R6.Text) + Convert.ToDouble(txt_R7.Text) + Convert.ToDouble(txt_R8.Text) + Convert.ToDouble(txt_R9.Text) + Convert.ToDouble(txt_R10.Text)) / 10);
                    ReboundIndex.Text = Convert.ToDouble(sclavgcnt).ToString("0.00");//chk again

                    Upperlimt = sclavgcnt + (sclavgcnt * 0.15);
                    Lowerlimt = sclavgcnt - (sclavgcnt * 0.15);

                    if (Convert.ToDouble(txt_R1.Text) >= Lowerlimt && Convert.ToDouble(txt_R1.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R1.Text);
                    }
                    if (Convert.ToDouble(txt_R2.Text) >= Lowerlimt && Convert.ToDouble(txt_R2.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R2.Text);
                    }
                    if (Convert.ToDouble(txt_R3.Text) >= Lowerlimt && Convert.ToDouble(txt_R3.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R3.Text);
                    }
                    if (Convert.ToDouble(txt_R4.Text) >= Lowerlimt && Convert.ToDouble(txt_R4.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R4.Text);
                    }
                    if (Convert.ToDouble(txt_R5.Text) >= Lowerlimt && Convert.ToDouble(txt_R5.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R5.Text);
                    }
                    if (Convert.ToDouble(txt_R6.Text) >= Lowerlimt && Convert.ToDouble(txt_R6.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R6.Text);
                    }
                    if (Convert.ToDouble(txt_R7.Text) >= Lowerlimt && Convert.ToDouble(txt_R7.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R7.Text);
                    }
                    if (Convert.ToDouble(txt_R8.Text) >= Lowerlimt && Convert.ToDouble(txt_R8.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R8.Text);
                    }
                    if (Convert.ToDouble(txt_R9.Text) >= Lowerlimt && Convert.ToDouble(txt_R9.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R9.Text);
                    }
                    if (Convert.ToDouble(txt_R10.Text) >= Lowerlimt && Convert.ToDouble(txt_R10.Text) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(txt_R10.Text);
                    }

                    if (Total > 0)
                    {
                        sclavgcnt = 0;
                        sclavgcnt = Total / Counter;
                        ReboundIndex.Text = Convert.ToDouble(sclavgcnt).ToString("0.00");
                        if (ddl_NDTBy.SelectedItem.Text.Trim() == "Rebound Hammer")
                        {
                            double CalCorrFactor = 0;
                            if (grdNDTInward.Columns[15].Visible == true)
                            {
                                if (txt_CorrFactor.Text != "")
                                {
                                    if (double.TryParse(txt_CorrFactor.Text, out CalCorrFactor))
                                    {

                                        CalCorrFactor = Convert.ToDouble(ReboundIndex.Text) * Convert.ToDouble(txt_CorrFactor.Text);
                                        IndStrOnSclAvg(CalCorrFactor, Convert.ToInt32(Session["rowindx"]));//Mluitply by CorectionFcator
                                    }
                                }
                                else
                                {
                                    IndStrOnSclAvg(Convert.ToDouble(ReboundIndex.Text), Convert.ToInt32(Session["rowindx"]));//Mluitply by CorectionFcator
                                }
                            }
                            else
                            {
                                IndStrOnSclAvg(Convert.ToDouble(ReboundIndex.Text), Convert.ToInt32(Session["rowindx"]));//Mluitply by CorectionFcator
                            }
                        }
                    }

                }
                ModalPopupExtender1.Hide();
            }
            else if (divPulseVel.Visible == true)
            {

                Button PulseVel = (Button)grdNDTInward.Rows[Convert.ToInt32(Session["rowindx"])].Cells[9].FindControl("PulseVel");
                TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[Convert.ToInt32(Session["rowindx"])].Cells[13].FindControl("txt_PulseVelDetails");
                txt_PulseVelDetails.Text = string.Empty;
                txt_PulseVelDetails.Text = txt_PulseVelDetails.Text + txt_Distance.Text + "," + txt_Time.Text;
                TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[Convert.ToInt32(Session["rowindx"])].Cells[23].FindControl("txt_CorrPulseVal");
                if (Chk_Indirect.Checked)
                    txt_CorrPulseVal.Visible = true;
                else
                    txt_CorrPulseVal.Visible = false;

                txt_Distance.Text = txt_Distance.Text.ToUpper();
                txt_Time.Text = txt_Time.Text.ToUpper();

                if (Chk_NO.Checked || txt_Distance.Text == "NO" || txt_Time.Text == "NO")
                {
                    PulseVel.Text = "NO";
                    txt_CorrPulseVal.Text = "NO";
                }
                else if (txt_Distance.Text == "" || txt_Time.Text == "" || Convert.ToDouble(txt_Distance.Text) == 0 || Convert.ToDouble(txt_Time.Text) == 0)
                {
                    PulseVel.Text = "---";
                    txt_CorrPulseVal.Text = "---";
                }
                else
                {
                    PulseVel.Text = (Convert.ToDecimal(txt_Distance.Text) / Convert.ToDecimal(txt_Time.Text)).ToString("0.00");
                    txt_CorrPulseVal.Text = (Convert.ToDecimal(txt_Distance.Text) / Convert.ToDecimal(txt_Time.Text)).ToString("0.00");
                }
                ModalPopupExtender1.Hide();

            }
        }
        protected void IndStrOnSclAvg(double ReboundIndex, int GridrowIndex)
        {
            int scl = 0;
            scl = Convert.ToInt32(Math.Round(ReboundIndex));
            DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[GridrowIndex].Cells[7].FindControl("ddl_AlphaAngle");
            TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[GridrowIndex].Cells[11].FindControl("txt_IndStr");
            if (scl == 20 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((100 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 20 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((160 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 21 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((120 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 21 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((170 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 22 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((130 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 22 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((180 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 23 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((140 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 23 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((200 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 24 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((160 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 24 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((210 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 25 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((100 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 25 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((180 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 25 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((230 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 26 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((110 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 26 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((190 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 26 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((250 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 27 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((130 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 27 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((210 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 27 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((260 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 28 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((140 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 28 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((220 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 28 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((280 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 29 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((160 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 29 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((240 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 29 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((300 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 30 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((180 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 30 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((260 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 30 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((310 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 31 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((190 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 31 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((270 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 31 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((330 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 32 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((210 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 32 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((290 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 32 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((340 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 33 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((220 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 33 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((300 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 33 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((360 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 34 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((240 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 34 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((320 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 34 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((380 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 35 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((260 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 35 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((340 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 35 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((400 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 36 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((270 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 36 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((270 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 36 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((360 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 36 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((420 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 37 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((290 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 37 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((380 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 37 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((430 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 38 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((310 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 38 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((400 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 38 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((450 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 39 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((330 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 39 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((410 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 39 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((470 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 40 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((350 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 40 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((430 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 40 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((490 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 41 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((360 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 41 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((450 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 41 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((510 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 42 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((380 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 42 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((470 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 42 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((530 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 43 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((410 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 43 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((490 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 43 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((550 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 44 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((420 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 44 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((510 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 44 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((570 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 45 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((450 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 45 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((530 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 45 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((590 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 46 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((470 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 46 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((550 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 46 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((610 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 47 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((490 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 47 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((570 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 47 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((630 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 48 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((500 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 48 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((590 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 48 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((650 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 49 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((530 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 49 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((610 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 49 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((670 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 50 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((550 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 50 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((640 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 50 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = -90°")
            {
                txt_IndStr.Text = Convert.ToDouble((690 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 51 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((570 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 51 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((650 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 52 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((590 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 52 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((680 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 53 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((610 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 53 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = 0°")
            {
                txt_IndStr.Text = Convert.ToDouble((700 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 54 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((640 * 9.81) / 100).ToString("##.00");
            }
            else if (scl == 55 && ddl_AlphaAngle.SelectedItem.Text.Trim() == "a = +90°")
            {
                txt_IndStr.Text = Convert.ToDouble((660 * 9.81) / 100).ToString("##.00");
            }
            else
            {
                // txt_IndStr.Text = "---";
                txt_IndStr.Text = Convert.ToDouble(scl * 3.6686 - 129.09).ToString("##.00");
            }
        }

        public void Calculation()
        {
            Int32 mAge = 0;
            //Boolean flgNA = false;

            double pv = 0;
            double SrRead = 0;
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                TextBox txt_Age = (TextBox)grdNDTInward.Rows[i].Cells[10].FindControl("txt_Age");
                TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[i].Cells[11].FindControl("txt_IndStr");
                TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[i].Cells[12].FindControl("txt_ReboundIndexDetails");
                TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                TextBox txt_EditedIndStr = (TextBox)grdNDTInward.Rows[i].Cells[14].FindControl("txt_EditedIndStr");
                TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[i].Cells[15].FindControl("txt_CorrFactor");
                Label lblMergFlag = (Label)grdNDTInward.Rows[i].Cells[16].FindControl("lblMergFlag");

                TextBox txt_IndStr_10 = (TextBox)grdNDTInward.Rows[i].Cells[17].FindControl("txt_IndStr_10");
                TextBox txt_IndStr10 = (TextBox)grdNDTInward.Rows[i].Cells[18].FindControl("txt_IndStr10");
                TextBox txt_IndStrWithin10 = (TextBox)grdNDTInward.Rows[i].Cells[19].FindControl("txt_IndStrWithin10");
                TextBox txt_IndStrShapoorji = (TextBox)grdNDTInward.Rows[i].Cells[20].FindControl("txt_IndStrShapoorji");
                TextBox txt_IndCorrStr = (TextBox)grdNDTInward.Rows[i].Cells[21].FindControl("txt_IndCorrStr");
                CheckBox chk_IndClusterAnalysis = (CheckBox)grdNDTInward.Rows[i].Cells[22].FindControl("chk_IndClusterAnalysis");
                TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");

                double number = 0;
                if (lblMergFlag.Text != "1")
                {
                    txt_CastingDt.Text = txt_CastingDt.Text.ToUpper();
                    if (txt_CastingDt.Text.ToUpper() != "NA" && txt_CastingDt.Text != "")
                    {
                        DateTime Testdt = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
                        DateTime Castdt = DateTime.ParseExact(txt_CastingDt.Text, "dd/MM/yyyy", null);

                        mAge = Convert.ToInt32((Testdt - Castdt).TotalDays);
                        txt_Age.Text = Convert.ToInt32(mAge).ToString();

                    }
                    else
                    {
                        txt_Age.Text = "NA";
                        //flgNA = true;
                        mAge = 28;
                    }

                    pv = 0;

                    // added on 30/09/19
                    if (Chk_Indirect.Checked == true)
                    {
                        if (double.TryParse(txt_CorrPulseVal.Text, out number))
                        {
                            pv = Convert.ToDouble(txt_CorrPulseVal.Text);
                        }

                    } // for indirect method
                    else if (double.TryParse(PulseVel.Text, out number))
                    {

                        pv = Convert.ToDouble(PulseVel.Text);
                    }
                    SrRead = 0;
                    if (double.TryParse(ReboundIndex.Text, out number))
                    {
                        SrRead = Convert.ToDouble(ReboundIndex.Text);

                    }
                    if (mAge > 28)
                    {
                        mAge = 28;
                    }
                    if (ddl_NDTBy.SelectedItem.Text.Trim() == "UPV")// )
                    {
                        if (pv == 0)
                        {
                            txt_IndStr.Text = "-1".ToString();
                        }
                        else
                        {

                            txt_IndStr.Text = (mAge * 0.0615 + 15.199 * pv - 38.8334).ToString();
                            if (Convert.ToDouble(txt_IndStr.Text) < 0)
                            {
                                txt_IndStr.Text = "0";
                            }
                        }
                    }
                    else if (ddl_NDTBy.SelectedItem.Text.Trim() == "Both")
                    {
                        if (chkClusterAnalysis.Checked == true)
                        {
                            txt_IndStr_10.Text = "---";
                            txt_IndStr10.Text = "---";
                            txt_IndStrWithin10.Text = "---";
                            txt_IndStrShapoorji.Text = "---";
                            txt_IndCorrStr.Text = "---";
                        }
                        //
                        if (txt_CorrFactor.Text == "")
                        {
                            if (PulseVel.Text == "---")
                            {
                                IndStrOnSclAvg(SrRead, i);
                            }
                            else
                            {
                                if (chkClusterAnalysis.Checked == true && chk_IndClusterAnalysis.Checked == true)
                                {
                                    txt_IndStr_10.Text = (mAge * 27.65 + SrRead * 0.9755 + pv * 8.86 - 821.25).ToString("##.00");
                                    txt_IndStr10.Text = (mAge * 0.67 + 1.473 * SrRead + pv * 12.58 - 85.357).ToString("##.00");
                                    txt_IndStrWithin10.Text = (SrRead * 1.21 + pv * 13.05 - mAge * 5.956 + 99.33).ToString("##.00");
                                    txt_IndStrShapoorji.Text = (SrRead * 0.749 + pv * 34.35 + mAge * 0.01 - 135.34).ToString("##.00");
                                    if (optMinus10.Checked == true)
                                        txt_IndStr.Text = (mAge * 27.65 + SrRead * 0.9755 + pv * 8.86 - 821.25).ToString("##.00");
                                    else if (opt10.Checked == true)
                                        txt_IndStr.Text = (mAge * 0.67 + 1.473 * SrRead + pv * 12.58 - 85.357).ToString("##.00");
                                    else if (optWithin10.Checked == true)
                                        txt_IndStr.Text = (SrRead * 1.21 + pv * 13.05 - mAge * 5.956 + 99.33).ToString("##.00");
                                    else if (optShapoorji.Checked == true)
                                        txt_IndStr.Text = (SrRead * 0.749 + pv * 34.35 + mAge * 0.01 - 135.34).ToString("##.00");
                                }
                                else
                                {
                                    if (SrRead == 0 || pv == 0)
                                    {
                                        txt_IndStr.Text = "0";
                                    }
                                    else if (SrRead == 0 && pv > 0)
                                    {
                                        txt_IndStr.Text = (mAge * 0.0615 + 15.199 * pv - 38.8334).ToString("##.00");
                                    }
                                    else if (SrRead > 0 && pv > 0)
                                    {
                                        txt_IndStr.Text = (mAge * 0.121 + SrRead * 1.111 + pv * 7.797 - 43.175).ToString("##.00");
                                    }
                                }
                            }
                        }
                        else  // correction factor applied then following
                        {
                            double CorrFactor = 0;
                            if (double.TryParse(txt_CorrFactor.Text, out number))
                            {
                                CorrFactor = Convert.ToDouble(txt_CorrFactor.Text);
                            }
                            if (chkClusterAnalysis.Checked == true && chk_IndClusterAnalysis.Checked == true)
                            {
                                txt_IndStr_10.Text = (mAge * 27.65 + (SrRead * 0.9755 * CorrFactor) + pv * 8.86 - 821.25).ToString("##.00");
                                txt_IndStr10.Text = (mAge * 0.67 + 1.473 * (SrRead * CorrFactor) + pv * 12.58 - 85.357).ToString("##.00");
                                txt_IndStrWithin10.Text = ((SrRead * CorrFactor) * 1.21 + pv * 13.05 - mAge * 5.956 + 99.33).ToString("##.00");
                                txt_IndStrShapoorji.Text = ((SrRead * CorrFactor) * 0.749 + pv * 34.35 + mAge * 0.01 - 135.34).ToString("##.00");

                                if (optMinus10.Checked == true)
                                    txt_IndStr.Text = (mAge * 27.65 + SrRead * CorrFactor * 0.9755 + pv * 8.86 - 821.25).ToString("##.00");
                                else if (opt10.Checked == true)
                                    txt_IndStr.Text = (mAge * 0.67 + 1.473 * SrRead * CorrFactor + pv * 12.58 - 85.357).ToString("##.00");
                                else if (optWithin10.Checked == true)
                                    txt_IndStr.Text = (SrRead * CorrFactor * 1.21 + pv * 13.05 - mAge * 5.956 + 99.33).ToString("##.00");
                                else if (optShapoorji.Checked == true)
                                    txt_IndStr.Text = (SrRead * CorrFactor * 0.749 + pv * 34.35 + mAge * 0.01 - 135.34).ToString("##.00");

                            }
                            else if (SrRead > 0)
                            {
                                if (pv == 0)
                                {
                                    IndStrOnSclAvg(SrRead, i);
                                    if (CorrFactor > 0)
                                    {
                                        if (double.TryParse(txt_IndStr.Text, out number))
                                        {
                                            txt_IndStr.Text = (Convert.ToDouble(txt_IndStr.Text) * CorrFactor).ToString();
                                        }
                                    }
                                }
                                else if (pv > 0)
                                {
                                    txt_IndStr.Text = (mAge * 0.121 + SrRead * CorrFactor * 1.111 + pv * 7.797 - 43.175).ToString();
                                }
                            }
                            else
                            {
                                txt_IndStr.Text = "0";
                            }

                        }
                    }
                    else if (ddl_NDTBy.SelectedItem.Text.Trim() == "UPV with Grading")
                    {
                        if (pv > 0)
                        {

                            if (ChkOldGrading.Checked == true)
                            {
                                if (pv < 3)
                                {
                                    txt_IndStr.Text = "Doubtful";// "Doubtful";
                                }
                                else if (pv >= 3 && pv <= 3.5)
                                {
                                    txt_IndStr.Text = "Medium";// "Medium";
                                }
                                else if (pv > 3.5 && pv <= 4.5)
                                {
                                    txt_IndStr.Text = "Good";
                                }
                                else if (pv > 4.5)
                                {
                                    txt_IndStr.Text = "Excellent";
                                }
                            }
                            else
                            {
                                // added on 7th Jan 21
                                if (DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null) > DateTime.ParseExact("31/12/2020", "dd/MM/yyyy", null))
                                {
                                    string mgrd = ddl_Grade.SelectedItem.Value.ToString();
                                    mgrd = mgrd.Replace("M", "");
                                    if (mgrd.Trim() == "" || mgrd.Trim() == "NA")
                                    {
                                        mgrd = "0";
                                    }
                                    if (Convert.ToInt32(mgrd) <= 25)
                                    {

                                        if (pv < 3.5)
                                        {
                                            txt_IndStr.Text = "Doubtful";
                                        }
                                        else if (pv >= 3.5 && pv <= 4.5)
                                        {
                                            txt_IndStr.Text = "Good";
                                        }
                                        else if (pv > 4.5)
                                        {
                                            txt_IndStr.Text = "Excellent";
                                        }
                                    }
                                    else
                                    {
                                        if (pv < 3.75)
                                        {
                                            txt_IndStr.Text = "Doubtful";
                                        }
                                        else if (pv >= 3.75 && pv <= 4.5)
                                        {
                                            txt_IndStr.Text = "Good";
                                        }
                                        else if (pv > 4.5)
                                        {
                                            txt_IndStr.Text = "Excellent";
                                        }

                                    }

                                }
                                else
                                {
                                    if (pv < 3)
                                    {
                                        txt_IndStr.Text = "Poor";
                                    }
                                    else if (pv >= 3 && pv <= 3.75)
                                    {
                                        txt_IndStr.Text = "Doubtful";
                                    }
                                    else if (pv > 3.75 && pv <= 4.40)
                                    {
                                        txt_IndStr.Text = "Good";
                                    }
                                    else if (pv > 4.4)
                                    {
                                        txt_IndStr.Text = "Excellent";
                                    }
                                }
                            }

                        }
                        else
                        {
                            txt_IndStr.Text = "-1".ToString();
                        }
                    }
                    else if (ddl_NDTBy.SelectedItem.Text.Trim() == "Rebound Hammer" && SrRead > 0)
                    {
                        if (grdNDTInward.Columns[15].Visible == true)
                        {
                            double CorrFactor = 0;
                            if (double.TryParse(txt_CorrFactor.Text, out number))
                            {
                                CorrFactor = Convert.ToDouble(txt_CorrFactor.Text);
                            }
                            if (CorrFactor > 0)
                            {
                                IndStrOnSclAvg(SrRead, Convert.ToInt32(i));//rowindex)
                                if (double.TryParse(txt_IndStr.Text, out number))
                                {
                                    txt_IndStr.Text = (Convert.ToDouble(txt_IndStr.Text) * CorrFactor).ToString();
                                }
                            }
                            else
                            {
                                IndStrOnSclAvg(SrRead, Convert.ToInt32(i));
                            }
                        }
                        else
                        {
                            IndStrOnSclAvg(SrRead, i);
                        }
                    }
                    else
                    {
                        txt_IndStr.Text = "---";
                    }

                    if (double.TryParse(txt_IndStr.Text, out number))
                    {
                        txt_IndStr.Text = Convert.ToInt32(Math.Round(Convert.ToDouble(txt_IndStr.Text))).ToString();
                    }
                    if (grdNDTInward.Columns[14].Visible == true)
                    {
                        if (txt_EditedIndStr.Text == string.Empty)
                        {
                            txt_EditedIndStr.Text = txt_IndStr.Text;
                        }
                    }
                }
                //
                //if (ddl_NDTBy.SelectedItem.Text.Trim() != "UPV with Grading")
                if (ddl_NDTBy.SelectedItem.Text.Trim() != "UPV with Grading")
                {
                    if (double.TryParse(txt_IndStr.Text, out number))
                    {
                        double mStr = Convert.ToDouble(txt_IndStr.Text);
                        if (mStr > 0)
                        {
                            if (mStr >= 5 && mStr <= 10.4)
                            {
                                txt_IndStr.Text = "5 To 10";
                            }
                            else if (mStr > 0 && mStr < 5)
                            {
                                txt_IndStr.Text = "Less Than 5";
                            }
                        }
                        else if (ddl_NDTBy.SelectedItem.Text.Trim() == "Rebound Hammer Only"
                            || ddl_NDTBy.SelectedItem.Text.Trim() == "DUCT")
                        {
                            if (mStr < 5)
                            {
                                txt_IndStr.Text = "Less Than 5";
                            }
                        }
                        else if (ddl_NDTBy.SelectedItem.Text.Trim() == "Both")
                        {
                            if (mStr != 0 && mStr < 5)
                            {
                                txt_IndStr.Text = "Less Than 5";
                            }
                            else
                            {
                                txt_IndStr.Text = "---";
                            }
                        }
                    }
                }

            } // for end


            bool valid = false;
            bool validNA = false;
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_Age = (TextBox)grdNDTInward.Rows[i].Cells[10].FindControl("txt_Age");

                if (ddl_Grade.SelectedItem.Text == "NA" || txt_CastingDt.Text == "NA" || txt_Age.Text == "NA")
                {
                    //flgNA = true;
                    valid = true;
                    for (int J = 0; J < grdRemark.Rows.Count; J++)
                    {
                        TextBox txt_REMARK = (TextBox)grdRemark.Rows[J].Cells[1].FindControl("txt_REMARK");
                        if (txt_REMARK.Text.Trim() == "NA  indicates - not available")
                        {
                            validNA = true;
                            break;
                        }
                    }
                    if (validNA == true || valid == true)
                    {
                        break;
                    }
                }
            }


            if (valid == true && validNA == false)
            {
                AddRemark("NA  indicates - not available");
            }

            //AddRemark("NDT was carried out using Sclerometer (Mechanical concrete test hammer) Proceq make,Instrument No. "+ddl_HammerNo.Text   + " and Digital Ultrasonic Testing Machine PUNDIT");
            //AddRemark("Mean error for the test hammer is 18% to 20% & Accuracy of ‘PUNDIT' is +/- 0.1 microsecond for entire range.");
            //AddRemark("Each hammer reading is average of 10 readings at same position.");
            //AddRemark("Indicative strengths in above table are calculated from regression equation based on hammer readings and pulse velocity readings. The predicted strengths bear a correlation of 0.84 with actual strengths as per calibration done in our laboratory.");
            //AddRemark("The above relation is likely to be affected by difference between the type of aggregates used in concrete at site then and those used in our laboratory concrete during calibration.");
        }

        protected void AddAllRemarks()
        {
            if (ddl_NDTBy.SelectedItem.Text.Trim() == "Rebound Hammer" || ddl_NDTBy.SelectedItem.Text.Trim() == "Both")
            {
                AddRemark("NDT was carried out using Sclerometer (Mechanical concrete test hammer) Proceq make,Instrument No. " + ddl_HammerNo.Text + " and Digital Ultrasonic Testing Machine PUNDIT");
            }
            if (ddl_NDTBy.SelectedItem.Text.Trim() == "Both")
            {
                AddRemark("Indicative strengths in above table are calculated from regression equation based on hammer readings and pulse velocity readings. The predicted strengths bear a correlation of 0.84 with actual strengths as per calibration done in our laboratory.");
            }
            AddRemark("Mean error for the test hammer is 18% to 20% & Accuracy of ‘PUNDIT' is +/- 0.1 microsecond for entire range.");
            AddRemark("Each hammer reading is average of 10 readings at same position.");
            AddRemark("The above relation is likely to be affected by difference between the type of aggregates used in concrete at site then and those used in our laboratory concrete during calibration.");
        }

        protected void AddRemark(string strRemark)
        {
            bool addFlag = true;
            if (grdRemark.Rows.Count > 0)
            {
                for (int i = 0; i < grdRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text == strRemark)
                    {
                        addFlag = false;
                        break;
                    }
                }
            }
            if (addFlag == true)
            {
                AddRowNDT_Remark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[grdRemark.Rows.Count - 1].Cells[1].FindControl("txt_REMARK");
                txt_REMARK.Text = strRemark;
            }
        }
        protected void imgBtnMergeRow_Click(object sender, CommandEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            GridViewRow row = (GridViewRow)img.Parent.Parent;
            int rowindex = row.RowIndex;
            Label lblMergFlag = (Label)grdNDTInward.Rows[rowindex].FindControl("lblMergFlag");
            if (lblMergFlag.Text == "1")
                UnMergeRow(rowindex);
            else
                MergeRow(rowindex);
            Display_UPV_ReboundGrid();
        }
        protected void imgBtnNDTTestAddRow_Click(object sender, CommandEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            GridViewRow row = (GridViewRow)img.Parent.Parent;
            int rowindex = row.RowIndex;
            AddRowEnterReportNDTInward(rowindex);
            ShowMergeRow();
            Display_UPV_ReboundGrid();
        }
        public void ShowMergeRow()
        {
            //int rowindex = 0;
            //if (grdNDTInward.Rows.Count > 1)
            //{
            //    if (ViewState["RowNo"] != null)
            //    {
            //        string[] rowindexx = Convert.ToString(ViewState["RowNo"]).Split(' ');
            //        foreach (var rowx in rowindexx)
            //        {
            //            if (rowx != "")
            //            {
            //                if (int.TryParse(rowx, out rowindex))
            //                {
            //                    rowindex = Convert.ToInt32(rowx);
            //                    if (rowindex < grdNDTInward.Rows.Count - 1)
            //                    {
            //                        ShowMerge(rowindex);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    ViewState["RowNo"] = null;
            //}
            int j = 1;
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                Label lblMergFlag1 = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");
                if (lblMergFlag1.Text == "1")
                {
                    grdNDTInward.Rows[i].Cells[3].Text = "";
                    MergeRow(i);
                }
                else
                {
                    grdNDTInward.Rows[i].Cells[3].Text = (j++).ToString();
                    UnMergeRow(i);
                }
            }
        }
        public void MergeRow(int rowindex)
        {
            Label lblMergFlag = (Label)grdNDTInward.Rows[rowindex].FindControl("lblMergFlag");
            TextBox txt_Description = (TextBox)grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[4].FindControl("txt_Description");
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[4].ColumnSpan += 12;
            txt_Description.Width = grdNDTInward.Width;
            txt_Description.CssClass = "Titlecol";
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[5].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[6].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[7].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[8].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[9].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[10].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[11].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[14].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[15].Visible = false;

            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[17].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[18].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[19].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[20].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[21].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[22].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[24].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[25].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[26].Visible = false;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[27].Visible = false;
            lblMergFlag.Text = "1";
            int j = 1;
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                Label lblMergFlag1 = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");
                grdNDTInward.Rows[i].Cells[3].Text = (j++).ToString();
                if (lblMergFlag1.Text == "1")
                {
                    j--;
                    grdNDTInward.Rows[i].Cells[3].Text = "";
                }
            }
        }
        public void UnMergeRow(int rowindex)
        {
            Label lblMergFlag = (Label)grdNDTInward.Rows[rowindex].FindControl("lblMergFlag");
            TextBox txt_Description = (TextBox)grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[4].FindControl("txt_Description");
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[4].ColumnSpan = 1;
            txt_Description.Width = 150;
            txt_Description.CssClass = "Detailcol";
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[5].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[6].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[7].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[8].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[9].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[10].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[11].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[14].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[15].Visible = true;
            
            if (chkClusterAnalysis.Checked == true)
            {
                grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[17].Visible = true;
                grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[18].Visible = true;
                grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[19].Visible = true;
                grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[20].Visible = true;
                grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[21].Visible = true;
                grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[22].Visible = true;
            }
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[24].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[25].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[26].Visible = true;
            grdNDTInward.Rows[Convert.ToInt32(rowindex)].Cells[27].Visible = true;
            lblMergFlag.Text = "0";
            int j = 1;
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                Label lblMergFlag1 = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");
                grdNDTInward.Rows[i].Cells[3].Text = (j++).ToString();
                if (lblMergFlag1.Text == "1")
                {
                    j--;
                    grdNDTInward.Rows[i].Cells[3].Text = "";
                }
            }
        }
                
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "NDT");
            foreach (var r in re)
            {
                AddRowNDT_Remark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.NDTDetail_RemarkId_int), "NDT");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.NDT_Remark_var.ToString();
                    i++;
                }
            }
            if (grdRemark.Rows.Count <= 0)
            {
                //AddRowNDT_Remark();
                AddAllRemarks();
            }
        }
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowNDT_Remark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowNDT_Remark(gvr.RowIndex);
            }
        }

        protected void DeleteRowNDT_Remark(int rowIndex)
        {

            GetCurrentDataNDT_Remark();
            DataTable dt = ViewState["NDTRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["NDTRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataNDT_Remark();
        }
        protected void AddRowNDT_Remark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["NDTRemarkTable"] != null)
            {
                GetCurrentDataNDT_Remark();
                dt = (DataTable)ViewState["NDTRemarkTable"];
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
            ViewState["NDTRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataNDT_Remark();
        }

        protected void GetCurrentDataNDT_Remark()
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
            ViewState["NDTRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataNDT_Remark()
        {
            DataTable dt = (DataTable)ViewState["NDTRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        
        protected void Chk_EditedIndStr_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                TextBox txt_IndStr = (TextBox)grdNDTInward.Rows[i].Cells[11].FindControl("txt_IndStr");
                TextBox txt_EditedIndStr = (TextBox)grdNDTInward.Rows[i].Cells[14].FindControl("txt_EditedIndStr");
                if (Chk_EditedIndStr.Checked)
                {
                    grdNDTInward.Columns[14].Visible = true;
                    txt_EditedIndStr.Text = txt_IndStr.Text;
                }
                //txt_EditedIndStr.Visible = true;
                else
                {
                    grdNDTInward.Columns[14].Visible = false;
                    // txt_EditedIndStr.Visible = false;
                }
            }
            Display_UPV_ReboundGrid();

        }
        protected void txt_CorectionFact_TextChanged(object sender, EventArgs e)
        {
            Display_CorrFactrorGrid();
            Display_UPV_ReboundGrid();
        }
        protected void Chk_CorrectionFactor_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_CorrectionFactor.Checked)
            {
                txt_CorectionFact.Visible = true;
                txt_CorectionFact.Text = string.Empty;
                txt_CorectionFact.Focus();
            }
            else
            {
                txt_CorectionFact.Visible = false;
            }
            Display_CorrFactrorGrid();
            Display_UPV_ReboundGrid();
        }
        public void Display_CorrFactrorGrid()
        {
            decimal CorrectFact = 0;
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                TextBox txt_CorrFactor = (TextBox)grdNDTInward.Rows[i].Cells[15].FindControl("txt_CorrFactor");
                if (Chk_CorrectionFactor.Checked && txt_CorectionFact.Text != string.Empty)
                {
                    if (Decimal.TryParse(txt_CorectionFact.Text, out CorrectFact))
                    {
                        if (Convert.ToDecimal(txt_CorectionFact.Text) > 0)
                        {
                            grdNDTInward.Columns[15].Visible = true;
                            txt_CorrFactor.Text = txt_CorectionFact.Text;
                        }
                    }
                    else
                    {
                        txt_CorectionFact.Text = string.Empty;
                    }
                }
                else
                {
                    grdNDTInward.Columns[15].Visible = false;
                }
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
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            FetchRefNo();
        }
        public void FetchRefNo()
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---" && ddl_OtherPendingRpt.SelectedValue != "0")
            {
                try
                {
                    lnkSave.Enabled = true;
                    lnkPrint.Visible = false;
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Visible = false;
                    txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                    txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                    DisplayNDT_Details();
                    //LoadOtherPendingCheckRpt();
                    LoadReferenceNoList();
                    LoadApproveBy();

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                finally
                {
                    grdNDTInward.DataSource = null;
                    grdNDTInward.DataBind();
                    DisplayGridNDTData();
                    grdRemark.DataSource = null;
                    grdRemark.DataBind();
                    DisplayRemark();
                }
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.NDT_PDFReport(txt_ReferenceNo.Text, lblEntry.Text, "");
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            //  RptNDT_Testing();
        }

        public void RptNDT_Testing()
        {
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.NDTReport_Html(txt_ReferenceNo.Text, lblEntry.Text);
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

        protected void chkClusterAnalysis_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClusterAnalysis.Checked == true)
            {
                for (int i = 0; i < grdNDTInward.Rows.Count; i++)
                {
                    CheckBox chk_IndClusterAnalysis = (CheckBox)grdNDTInward.Rows[i].Cells[22].FindControl("chk_IndClusterAnalysis");
                    chk_IndClusterAnalysis.Checked = true;
                }

                for (int i = 17; i <= 22; i++)
                {
                    grdNDTInward.Columns[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < grdNDTInward.Rows.Count; i++)
                {
                    TextBox txt_IndStr_10 = (TextBox)grdNDTInward.Rows[i].Cells[17].FindControl("txt_IndStr_10");
                    TextBox txt_IndStr10 = (TextBox)grdNDTInward.Rows[i].Cells[18].FindControl("txt_IndStr10");
                    TextBox txt_IndStrWithin10 = (TextBox)grdNDTInward.Rows[i].Cells[19].FindControl("txt_IndStrWithin10");
                    TextBox txt_IndStrShapoorji = (TextBox)grdNDTInward.Rows[i].Cells[20].FindControl("txt_IndStrShapoorji");
                    TextBox txt_IndCorrStr = (TextBox)grdNDTInward.Rows[i].Cells[21].FindControl("txt_IndCorrStr");
                    CheckBox chk_IndClusterAnalysis = (CheckBox)grdNDTInward.Rows[i].Cells[22].FindControl("chk_IndClusterAnalysis");

                    txt_IndStr_10.Text = "";
                    txt_IndStr10.Text = "";
                    txt_IndStrWithin10.Text = "";
                    txt_IndStrShapoorji.Text = "";
                    txt_IndCorrStr.Text = "";
                    chk_IndClusterAnalysis.Checked = false;
                }
                for (int i = 17; i <= 22; i++)
                {
                    grdNDTInward.Columns[i].Visible = false;
                }
                optMinus10.Checked = false;
                opt10.Checked = false;
                optWithin10.Checked = false;
                optShapoorji.Checked = false;
            }
        }

        protected void Chk_Indirect_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_Indirect.Checked)
            {
                Display_CorrPulseVelGrid();
            }
            else
                grdNDTInward.Columns[23].Visible = false;


        }

        public void Display_CorrPulseVelGrid()
        {
            for (int i = 0; i < grdNDTInward.Rows.Count; i++)
            {
                Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                TextBox txt_CorrPulseVal = (TextBox)grdNDTInward.Rows[i].Cells[23].FindControl("txt_CorrPulseVal");
                txt_CorrPulseVal.Visible = true;
                grdNDTInward.Columns[23].Visible = true;
                txt_CorrPulseVal.Text = PulseVel.Text;

            }
        }

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {

            lnkSave.Enabled = true;
            lnkPrint.Visible = false;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;

            #region DisplayNDT_Details();
            var ndtAppData = dc.NDTApp_ndtDetails_View(lblEnquiryNo.Text, "All", "", "", "", "","").ToList();
            foreach (var ndtapp in ndtAppData)
            {
                txt_TestingDt.Text = Convert.ToDateTime(ndtapp.enquiry_date).ToString("dd/MM/yyyy");
                ddl_NDTBy.ClearSelection();
                bool upvFlag = false, hammFlag = false;
                foreach (var ndtBy in ndtAppData)
                {
                    if (ndtBy.fk_machine_id == "UPV")
                        upvFlag = true;
                    else if (ndtBy.fk_machine_id == "RH")
                        hammFlag = true;
                }
                if (upvFlag == true && hammFlag == true)
                    ddl_NDTBy.SelectedValue = "Both";
                else if (upvFlag == true && hammFlag == false)
                    ddl_NDTBy.SelectedValue = "UPV";
                else if (upvFlag == false && hammFlag == true)
                    ddl_NDTBy.SelectedValue = "Rebound Hammer";

                if (ndtapp.probing_type == 2)
                {
                    Chk_Indirect.Visible = true;
                    Chk_Indirect.Checked = true;
                }

                if (ndtapp.hammer_no != null && ndtapp.hammer_no != string.Empty)
                {
                    ddl_HammerNo.ClearSelection();
                    ddl_HammerNo.Items.FindByText(ndtapp.hammer_no).Selected = true;
                    ddl_HammerNo.Enabled = true;
                }
                if (ddl_NDTBy.SelectedItem.Text == "Both" || ddl_NDTBy.SelectedItem.Text == "Rebound Hammer")
                {
                    ddl_HammerNo.Enabled = true;
                }
                else
                {
                    ddl_HammerNo.Enabled = false;
                }
                break;
            }
            #endregion

            grdNDTInward.DataSource = null;
            grdNDTInward.DataBind();
            #region DisplayGridNDTData();
            int i = 0;
            string RowNo = "";
            string mgrd;
            int RowIndex = 0;
            var ndtAppDataTitle = dc.NDTApp_ndtDetails_View(lblEnquiryNo.Text, "", "", "", "","", "").ToList();
            foreach (var nt in ndtAppDataTitle)
            {
                AddRowEnterReportNDTInward(grdNDTInward.Rows.Count);
                TextBox txt_Description = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                Label lblMergFlag = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");

                lblMergFlag.Text = "1";
                RowNo = RowNo + " " + i;
                txt_Description.Text = nt.title + " " + nt.floor_type + " " + nt.member_type + " " + nt.id_mark;
                if (Chk_Indirect.Checked == true)
                {
                    grdNDTInward.Columns[23].Visible = true;
                }
                i++;

                bool upvFound = false;
                int rhRow = 0;
                var ndtAppDataRh = dc.NDTApp_ndtDetails_View(lblEnquiryNo.Text, nt.title, nt.floor_type, nt.member_type, nt.id_mark,"", "RH").ToList();
                foreach (var nrh in ndtAppDataRh)
                {
                    AddRowEnterReportNDTInward(grdNDTInward.Rows.Count);
                    TextBox txt_DescriptionRh = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                    DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                    TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                    DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                    TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[i].Cells[12].FindControl("txt_ReboundIndexDetails");
                    //TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                    Label lblMergFlagRh = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");
                    Label lblImage1 = (Label)grdNDTInward.Rows[i].FindControl("lblImage1");
                    Label lblImage2 = (Label)grdNDTInward.Rows[i].FindControl("lblImage2");

                    txt_DescriptionRh.Text = nrh.location;
                    lblMergFlagRh.Text = "0";

                    mgrd = Convert.ToString(nrh.grade).ToUpper();                    
                    mgrd = mgrd.Replace("M", "");
                    mgrd = "M " + mgrd.Trim();
                    ddl_Grade.ClearSelection();
                    //if (Convert.ToString(nrh.grade) != "" && ddl_Grade.Items.FindByValue(nrh.grade.ToUpper()) != null)
                    if (mgrd  != "M" && ddl_Grade.Items.FindByValue(nrh.grade.ToUpper()) != null && nrh.grade.ToUpper() != "NA")
                    {
                        ddl_Grade.Items.FindByValue(mgrd).Selected = true;
                    }
                    else
                    {

                        ddl_Grade.Items.FindByValue("NA").Selected = true;
                    }
                    if (Convert.ToString(nrh.cast_date) != "")
                    {
                        if (Convert.ToDateTime(nrh.cast_date).ToString("dd/MM/yyyy") == "01/01/1900")
                            txt_CastingDt.Text = "NA";
                        else
                            txt_CastingDt.Text = Convert.ToDateTime(nrh.cast_date).ToString("dd/MM/yyyy");
                    }
                    if (Convert.ToString(nrh.angle) != "")
                    {
                        if (nrh.angle.Trim() == "0")
                            ddl_AlphaAngle.SelectedIndex = 3;
                        else if (nrh.angle.Trim() == "+90")
                            ddl_AlphaAngle.SelectedIndex = 2;
                        else if (nrh.angle.Trim() == "-90")
                            ddl_AlphaAngle.SelectedIndex = 1;
                    }
                    //txt_ReboundIndexDetails.Text = nrh.reading1 + "," + nrh.reading2 + "," + nrh.reading3 + "," + nrh.reading4 + "," + nrh.reading5 + "," + nrh.reading6 + "," + nrh.reading7 + "," + nrh.reading8 + "," + nrh.reading9 + "," + nrh.reading10;
                    txt_ReboundIndexDetails.Text = nrh.reading1.ToString() + "," + nrh.reading2.ToString() + "," + nrh.reading3.ToString() + "," + nrh.reading4.ToString() + "," + nrh.reading5.ToString() + "," + nrh.reading6.ToString() + "," + nrh.reading7.ToString() + "," + nrh.reading8.ToString() + "," + nrh.reading9.ToString() + "," + nrh.reading10.ToString();
                    #region calculate rebound index

                    double sclavgcnt = 0;
                    double Upperlimt = 0;
                    double Lowerlimt = 0;
                    int Counter = 0;
                    double Total = 0;
                    Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");

                    sclavgcnt = ((Convert.ToDouble(nrh.reading1.ToString()) + Convert.ToDouble(nrh.reading2.ToString()) + Convert.ToDouble(nrh.reading3.ToString()) + Convert.ToDouble(nrh.reading4.ToString()) + Convert.ToDouble(nrh.reading5.ToString()) + Convert.ToDouble(nrh.reading6.ToString()) + Convert.ToDouble(nrh.reading7.ToString()) + Convert.ToDouble(nrh.reading8.ToString()) + Convert.ToDouble(nrh.reading9.ToString()) + Convert.ToDouble(nrh.reading10.ToString())) / 10);
                    ReboundIndex.Text = Convert.ToDouble(sclavgcnt).ToString("0.00");//chk again

                    Upperlimt = sclavgcnt + (sclavgcnt * 0.15);
                    Lowerlimt = sclavgcnt - (sclavgcnt * 0.15);

                    if (Convert.ToDouble(nrh.reading1.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading1.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading1.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading2.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading2.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading2.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading3.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading3.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading3.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading4.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading4.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading4.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading5.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading5.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading5.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading6.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading6.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading6.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading7.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading7.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading7.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading8.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading8.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading8.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading9.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading9.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading9.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading10.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading10.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading10.ToString());
                    }

                    if (Total > 0)
                    {
                        sclavgcnt = 0;
                        sclavgcnt = Total / Counter;
                        ReboundIndex.Text = Convert.ToDouble(sclavgcnt).ToString("0.00");                        
                        IndStrOnSclAvg(Convert.ToDouble(ReboundIndex.Text), i);//Mluitply by CorectionFcator                            
                    }
                    #endregion
                    //txt_PulseVelDetails.Text = ndtapp.distance + "," + ndtapp.time;
                    if (Convert.ToString(nrh.image1) != null && Convert.ToString(nrh.image1) != "NULL")
                        lblImage1.Text = nrh.image1;
                    if (Convert.ToString(nrh.image2) != null && Convert.ToString(nrh.image2) != "NULL")
                        lblImage2.Text = nrh.image2;
                    if (ddl_NDTBy.SelectedValue == "Both")
                    {
                        var ndtAppDataUpv = dc.NDTApp_ndtDetails_View(lblEnquiryNo.Text, nt.title, nt.floor_type, nt.member_type, nt.id_mark, nrh.location, "UPV").ToList();
                        foreach (var nupv in ndtAppDataUpv)
                        {
                            upvFound = true;
                            TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                            Label lblImage3 = (Label)grdNDTInward.Rows[i].FindControl("lblImage3");
                            Label lblImage4 = (Label)grdNDTInward.Rows[i].FindControl("lblImage4");
                            
                            pnlIndirect.Visible = false;
                            if (nupv.probing_type == 2)
                            {
                                pnlIndirect.Visible = true;
                                txt_PulseVelDetails.Text = "";
                                lblDistances.Text = "Distance";
                                lblTimes.Text = "Time";

                                lbl150.Text = nupv.time_1.ToString();
                                lbl300.Text = nupv.time_2.ToString();
                                lbl450.Text = nupv.time_3.ToString();
                                lbl550.Text = nupv.time_4.ToString();                                
                            }
                            else
                            {
                                txt_PulseVelDetails.Text = nupv.distance + "," + nupv.ndt_time;
                                #region calculate pulse velocity
                                Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                                PulseVel.Text = (Convert.ToDecimal(nupv.distance) / Convert.ToDecimal(nupv.ndt_time)).ToString("0.00");
                                #endregion
                            }

                            if (Convert.ToString(nupv.image1) != null && Convert.ToString(nupv.image1) != "NULL")
                                lblImage3.Text = nupv.image1;
                            if (Convert.ToString(nupv.image2) != null && Convert.ToString(nupv.image2) != "NULL")
                                lblImage4.Text = nupv.image2;                            
                        }
                    }
                    i++;
                    rhRow++;
                }

                if (ddl_NDTBy.SelectedValue != "Both" || upvFound == false)
                {
                    int upvRow = 0;
                    i = i - rhRow;
                    var ndtAppDataUpv = dc.NDTApp_ndtDetails_View(lblEnquiryNo.Text, nt.title, nt.floor_type, nt.member_type, nt.id_mark,"", "UPV").ToList();
                    foreach (var nupv in ndtAppDataUpv)
                    {
                        if (upvRow >= rhRow)
                            AddRowEnterReportNDTInward(grdNDTInward.Rows.Count);
                        TextBox txt_DescriptionRh = (TextBox)grdNDTInward.Rows[i].Cells[4].FindControl("txt_Description");
                        DropDownList ddl_Grade = (DropDownList)grdNDTInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                        TextBox txt_CastingDt = (TextBox)grdNDTInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                        DropDownList ddl_AlphaAngle = (DropDownList)grdNDTInward.Rows[i].Cells[7].FindControl("ddl_AlphaAngle");
                        //TextBox txt_ReboundIndexDetails = (TextBox)grdNDTInward.Rows[i].Cells[12].FindControl("txt_ReboundIndexDetails");
                        TextBox txt_PulseVelDetails = (TextBox)grdNDTInward.Rows[i].Cells[13].FindControl("txt_PulseVelDetails");
                        Label lblMergFlagRh = (Label)grdNDTInward.Rows[i].FindControl("lblMergFlag");
                        Label lblImage3 = (Label)grdNDTInward.Rows[i].FindControl("lblImage3");
                        Label lblImage4 = (Label)grdNDTInward.Rows[i].FindControl("lblImage4");

                        txt_DescriptionRh.Text = nupv.location;
                        lblMergFlagRh.Text = "0";

                        ddl_Grade.ClearSelection();
                        if (Convert.ToString(nupv.grade) != "" && ddl_Grade.Items.FindByValue(nupv.grade.ToUpper()) != null)
                        {
                            //ddl_Grade.Items.FindByText(nupv.grade.ToUpper()).Selected = true;
                            ddl_Grade.Items.FindByValue(nupv.grade.ToUpper()).Selected = true;
                        }
                        else
                        {
                            ddl_Grade.Items.FindByValue("NA").Selected = true;
                        }
                        if (Convert.ToString(nupv.cast_date) != "")
                        {
                            if (Convert.ToDateTime(nupv.cast_date).ToString("dd/MM/yyyy") == "01/01/1900")
                                txt_CastingDt.Text = "NA";
                            else
                                txt_CastingDt.Text = Convert.ToDateTime(nupv.cast_date).ToString("dd/MM/yyyy");
                        }
                        //if (Convert.ToString(nupv.angle) != "" && nupv.angle != "NULL")
                        //{
                        //    ddl_AlphaAngle.SelectedItem.Text = "a = " + nupv.angle;
                        //}
                        //txt_ReboundIndexDetails.Text = nrh.reading1 + "," + nrh.reading2 + "," + nrh.reading3 + "," + nrh.reading4 + "," + nrh.reading5 + "," + nrh.reading6 + "," + nrh.reading7 + "," + nrh.reading8 + "," + nrh.reading9 + "," + nrh.reading10;
                        pnlIndirect.Visible = false;
                        if (nupv.probing_type == 2)
                        {
                            pnlIndirect.Visible = true;
                            //txt_PulseVelDetails.Text = "150 : "+ nupv.time_1+",300 : " + nupv.time_2+",450 : " + nupv.time_3 +", 550 : " + nupv.time_4;
                            //:nupv.distance + "," + nupv.ndt_time;
                            txt_PulseVelDetails.Text = "";
                            //Label15.Text =" Time   : " + nupv.time_1 + "    " + nupv.time_2 + "   " + nupv.time_3 + "    " + nupv.time_4;
                            lblDistances.Text = "Distance";
                            lblTimes.Text = "Time";

                            lbl150.Text = nupv.time_1.ToString();
                            lbl300.Text = nupv.time_2.ToString();
                            lbl450.Text = nupv.time_3.ToString();
                            lbl550.Text = nupv.time_4.ToString();
                        }
                        else
                        {
                            //lbl150.Text = "";
                            //lbl300.Text = "";
                            //lbl450.Text = "";
                            //lbl550.Text = "";
                            //lblDistances.Text = "";
                            //lblTimes.Text = "";
                            txt_PulseVelDetails.Text = nupv.distance + "," + nupv.ndt_time;
                            #region calculate pulse velocity
                            Button PulseVel = (Button)grdNDTInward.Rows[i].Cells[9].FindControl("PulseVel");
                            PulseVel.Text = (Convert.ToDecimal(nupv.distance) / Convert.ToDecimal(nupv.ndt_time)).ToString("0.00");
                            #endregion
                        }

                        if (Convert.ToString(nupv.image1) != null && Convert.ToString(nupv.image1) != "NULL")
                            lblImage3.Text = nupv.image1;
                        if (Convert.ToString(nupv.image2) != null && Convert.ToString(nupv.image2) != "NULL")
                            lblImage4.Text = nupv.image2;
                        //if (upvRow >= rhRow)
                        i++;
                        upvRow++;
                    }
                    if (upvRow < rhRow)
                    {
                        i = i + (rhRow - upvRow);
                    }
                }
            }

            if (RowNo != "")
            {
                string[] rowindexx = Convert.ToString(RowNo).Split(' ');
                foreach (var rowx in rowindexx)
                {
                    if (rowx != "")
                    {
                        RowIndex = Convert.ToInt32(rowx);
                        if (RowIndex <= grdNDTInward.Rows.Count - 1)
                        {
                            MergeRow(RowIndex);
                        }
                    }
                }
            }
            Display_CorrFact();
            Display_UPV_ReboundGrid();
            #endregion DisplayGridNDTData
            if (grdNDTInward.Rows.Count > 0)
            {
                //ValidateData();
                Calculation();
            }
        }

        protected void grdNDTInward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "")
            {
                GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowindex = grdrow.RowIndex;
                string url = "";
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                if (cnStr.ToLower().Contains("mumbai") == true)
                    url = "http://92.204.136.64:81/ndtphoto/mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    url = "http://92.204.136.64:81/ndtphoto/nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    url = "http://92.204.136.64:81/ndtphoto/metro/";
                else
                    url = "http://92.204.136.64:81/ndtphoto/pune/";

                if (e.CommandName == "ViewImage1")
                {
                    Label lblImage1 = (Label)grdNDTInward.Rows[rowindex].FindControl("lblImage1");
                    if (lblImage1.Text.Trim() != "")
                    {
                        url += lblImage1.Text;
                        //Response.Write("<script>window.open('" + ulr + "','_blank')</script>");
                        Redirect(url, "_blank", "menubar=0,width=600,height=500");
                    }
                }
                else if (e.CommandName == "ViewImage2")
                {
                    Label lblImage2 = (Label)grdNDTInward.Rows[rowindex].FindControl("lblImage2");
                    if (lblImage2.Text.Trim() != "")
                    {
                        url += lblImage2.Text;
                        Redirect(url, "_blank", "menubar=0,width=600,height=500");
                    }
                }
                else if (e.CommandName == "ViewImage3")
                {
                    Label lblImage3 = (Label)grdNDTInward.Rows[rowindex].FindControl("lblImage3");
                    if (lblImage3.Text.Trim() != "")
                    {
                        url += lblImage3.Text;
                        Redirect(url, "_blank", "menubar=0,width=600,height=500");
                    }
                }
                else if (e.CommandName == "ViewImage4")
                {
                    Label lblImage4 = (Label)grdNDTInward.Rows[rowindex].FindControl("lblImage4");
                    if (lblImage4.Text.Trim() != "")
                    {
                        url += lblImage4.Text;
                        Redirect(url, "_blank", "menubar=0,width=600,height=500");
                    }
                }
            }
        }
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
        protected void ChkOldGrading_CheckedChanged(object sender, EventArgs e)
        {
            if (ddl_NDTBy.SelectedItem.Text.Trim() != "UPV with Grading")
            {
                ChkOldGrading.Checked = false;
            }
        }
    }
}