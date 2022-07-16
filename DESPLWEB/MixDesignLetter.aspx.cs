using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class MixDesignLetter : System.Web.UI.Page
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

                    if (strReq.Contains("=") == true)
                    {
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        txt_RefNo.Text = arrIndMsg[1].ToString().Trim();
                        
                        arrIndMsg = arrMsgs[1].Split('=');
                        lbl_TrialId.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[2].Split('=');
                        lblStatus.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[3].Split('=');
                        lblReportType.Text = arrIndMsg[1].ToString().Trim();
                    }
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Checking MD Letter";
                //lblStatus.Text = "Check";
                lblStatus.Text = "Enter";
                optMDL.Checked = true;
                lblReportType.Text = "MDL";
                LoadOtherPendingReports();
                if (txt_RefNo.Text != "")
                {
                    ddlOtherPendingRpt.SelectedValue = txt_RefNo.Text;
                }
                LoadTestedBy();
                LoadApprovedBy();
                lblEntdChkdBy.Text = "Checked By";
                lblApprdBy.Text = "Approved By";
                txtEntdChkdBy.Text = Session["LoginUserName"].ToString();
                
                if (lbl_TrialId.Text != "")
                {
                    if (CheckForNewTrial() == true)
                    {
                        ShowTrialDetail();
                        DisplayTrialRemark();
                        DisplayMDLetter(lblReportType.Text);
                        lnkMoisuteCorrPrnt.Visible = true;
                        if (grdMD.Rows.Count == 0)
                        {
                            CalculateMDLetter();
                        }
                        CalculateExp28DaysCompStrength();
                    }
                    else
                    {
                        Label lblMsg = (Label)Master.FindControl("lblMsg");
                        lblMsg.Text = "Update trial for new mix design entry.";
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        #region load list
        private void LoadOtherPendingReports()
        {
            ddlOtherPendingRpt.DataSource = null;
            ddlOtherPendingRpt.DataBind();
            string reportStatus = "", mdlStatus = "";
            //reportStatus = "Check";
            reportStatus = "Enter";
            if (optMDL.Checked == true)
                mdlStatus = "MDL";
            else
                mdlStatus = "Final";
            var mflist = dc.MFReport_View(reportStatus, mdlStatus);
            ddlOtherPendingRpt.DataTextField = "MFINWD_ReferenceNo_var";
            ddlOtherPendingRpt.DataSource = mflist;
            ddlOtherPendingRpt.DataBind();
            ddlOtherPendingRpt.Items.Insert(0, "---Select---");
        }
        private void LoadTestedBy()
        {
            byte testBit = 1;
            byte apprBit = 0;

            ddlTestdBy.DataTextField = "USER_Name_var";
            ddlTestdBy.DataValueField = "USER_Id";
            var apprUser = dc.ReportStatus_View("", null, null, 0, testBit, apprBit, "", 0, 0, 0);
            ddlTestdBy.DataSource = apprUser;
            ddlTestdBy.DataBind();
            ddlTestdBy.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadApprovedBy()
        {
            byte testBit = 0;
            byte apprBit = 1;
            
            ddlApprdBy.DataTextField = "USER_Name_var";
            ddlApprdBy.DataValueField = "USER_Id";
            var apprUser = dc.ReportStatus_View("", null, null, 0, testBit, apprBit, "", 0, 0, 0);
            ddlApprdBy.DataSource = apprUser;
            ddlApprdBy.DataBind();
            ddlApprdBy.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        #endregion

        private bool CheckForNewTrial()
        {
            bool newTrial = false;
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var t in data)
            {
                if (t.TrialDetail_ActualTrial != null && t.TrialDetail_ActualTrial.ToString() != "")
                {
                    newTrial = true;
                    break;
                }
            }
            return newTrial;
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            if (ddlOtherPendingRpt.SelectedIndex > 0)
            {
                ClearAllControls();
                if (optMDL.Checked == true)
                    lblReportType.Text = "MDL";
                else if (optFinal.Checked == true)
                    lblReportType.Text = "Final";

                //lblStatus.Text = "Check";
                lblStatus.Text = "Enter";
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblReportType.Text == "MDL")
                {
                    lblheading.Text = "Checking MD Letter";
                }
                else
                {
                    lblheading.Text = "Checking Final Report";
                    lnkCoverShtPrnt.Visible = true;
                    lnkMoisuteCorrPrnt.Visible = true;
                }
                txt_RefNo.Text = ddlOtherPendingRpt.SelectedValue;
                var mixd = dc.Trial_View(txt_RefNo.Text, true);
                lbl_TrialId.Text = mixd.FirstOrDefault().Trial_Id.ToString();

                if (CheckForNewTrial() == true)
                {
                    ShowTrialDetail();
                    DisplayMDLetter(lblReportType.Text);
                    DisplayTrialRemark();
                    lnkMoisuteCorrPrnt.Visible = true;
                    if (grdMD.Rows.Count == 0)
                    {
                        CalculateMDLetter();
                    }
                    CalculateExp28DaysCompStrength();
                }
                else
                {
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Update trial for new mix design entry.";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void DisplayMDLetter(string MFStatus)
        {
            var mfinwd = dc.AllInward_View("MF", 0, txt_RefNo.Text);
            foreach (var mf in mfinwd)
            {
                txtWitnessBy.Text = mf.MFINWD_WitnessBy_var;
                if (txtWitnessBy.Text != "")
                {
                    chkWitnessBy.Checked = true;
                    txtWitnessBy.Visible = true;
                }
                chkFlow.Checked = Convert.ToBoolean(mf.MFINWD_FlowStatus_bit);
                chkAdmixtureInKg.Checked = Convert.ToBoolean(mf.MFINWD_AdmInKgStatus_bit);
            }

            int i = 0;
            var data = dc.MixDesignLetter_View(txt_RefNo.Text, MFStatus);
            foreach (var m in data)
            {
                AddRowMD();
                Label lbl_MaterialName = (Label)grdMD.Rows[i].FindControl("lbl_MaterialName");
                Label lbl_MaterialNameActual = (Label)grdMD.Rows[i].FindControl("lbl_MaterialNameActual");
                Label lbl_MaterialId = (Label)grdMD.Rows[i].FindControl("lbl_MaterialId");
                TextBox txt_Description = (TextBox)grdMD.Rows[i].FindControl("txt_Description");
                TextBox txt_PerM3 = (TextBox)grdMD.Rows[i].Cells[1].FindControl("txt_PerM3");
                TextBox txt_Per50Kg = (TextBox)grdMD.Rows[i].Cells[2].FindControl("txt_Per50Kg");
                TextBox txt_Volume = (TextBox)grdMD.Rows[i].Cells[3].FindControl("txt_Volume");

                lbl_MaterialId.Text = m.MD_MaterialId_int.ToString();
                lbl_MaterialName.Text = m.MD_MaterialName_var;
                lbl_MaterialNameActual.Text = m.MD_MaterialNameActual_var;
                txt_Description.Text = m.MD_Description_var;
                txt_PerM3.Text = m.MD_WeightPerM3_num.ToString();
                txt_Per50Kg.Text = m.MD_WeightPer50Kg_num.ToString();
                txt_Volume.Text = m.MD_Volume_num.ToString();
                if (lbl_MaterialName.Text == "Admixture")
                {
                    chkAdmixtureInKg.Visible = true;
                    if (chkAdmixtureInKg.Checked == true)
                    {
                        txtAdmixtureInKg.Visible = true;
                        var trail = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
                        foreach (var t in trail)
                        {
                            if (t.TrialDetail_MaterialName.Contains("Admixture") == true)
                            {
                                txtAdmixtureInKg.Text = Convert.ToDecimal(t.TrialDetail_ActualTrial * t.TrialDetail_SpecificGravity).ToString("0.000");
                            }
                        }
                    }
                    else
                        txtAdmixtureInKg.Visible = false;
                }
                i++;
            }
            if (ddlBatching.SelectedValue == "Volume Batching")
            {
                grdMD.Columns[4].Visible = true;
            }
            else
            {
                grdMD.Columns[4].Visible = false;
            }
        }
        
        protected void ShowTrialDetail()
        {
            txt_RecType.Text = "MF";
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var m in data)
            {
                if (Convert.ToString(m.Trial_WitnessBy) != "")
                {
                    chkWitnessBy.Checked = true;
                    txtWitnessBy.Visible = true;
                    txtWitnessBy.Text = Convert.ToString(m.Trial_WitnessBy);                   
                }
                lblTrialName.Text = m.Trial_Name.ToString();
                if (m.Trial_OtherInfo != null && m.Trial_OtherInfo != "")
                {
                    if (m.Trial_OtherInfo == "Weight Batching" || m.Trial_OtherInfo == "Volume Batching")
                        ddlBatching.SelectedValue = m.Trial_OtherInfo;
                } 
                AddRowSlump();
                if (m.Trial_SlumpDetails != null && m.Trial_SlumpDetails != "")
                {
                    string[] slumpDetails = m.Trial_SlumpDetails.Split('|');
                    Label lbl_Name = (Label)grdSlump.Rows[0].FindControl("lbl_Name");
                    Label txt_0 = (Label)grdSlump.Rows[0].FindControl("txt_0");
                    Label txt_30 = (Label)grdSlump.Rows[0].FindControl("txt_30");
                    Label txt_60 = (Label)grdSlump.Rows[0].FindControl("txt_60");
                    Label txt_90 = (Label)grdSlump.Rows[0].FindControl("txt_90");
                    Label txt_120 = (Label)grdSlump.Rows[0].FindControl("txt_120");
                    Label txt_150 = (Label)grdSlump.Rows[0].FindControl("txt_150");
                    Label txt_180 = (Label)grdSlump.Rows[0].FindControl("txt_180");
                    lbl_Name.Text = slumpDetails[0];
                    txt_0.Text = slumpDetails[1];
                    txt_30.Text = slumpDetails[2];
                    txt_60.Text = slumpDetails[3];
                    txt_90.Text = slumpDetails[4];
                    txt_120.Text = slumpDetails[5];
                    txt_150.Text = slumpDetails[6];
                    txt_180.Text = slumpDetails[7];
                }
                txtYield.Text = m.Trial_Yield.ToString();
                txtWtOfConcreteInCylinder.Text = m.Trial_WtOfConcreteInCylinder.ToString();
            }

            DisplayCubeCasting();
            MainView.ActiveViewIndex = 0;
            Tab_CubeCompStr.CssClass = "Initiative";
            Tab_OtherInfo.CssClass = "Click";
            // Fetch Inward Data
            var Inwdata = dc.MF_View(txt_RefNo.Text, 0, "MF");
            foreach (var m in Inwdata)
            {
                if (lblStatus.Text == "")
                {
                    txt_KindAttention.Text = Convert.ToString(m.MFINWD_KindAttention);

                    if (m.MFINWD_CoverSheetDetail != null)
                    {
                        string[] days = Convert.ToString(m.MFINWD_CoverSheetDetail).Split('|');
                        foreach (string d in days)
                        {
                            if (d != "")
                            {
                                for (int j = 0; j < grdCubeCasting.Rows.Count; j++)
                                {
                                    CheckBox chkBx = (CheckBox)grdCubeCasting.Rows[j].Cells[0].FindControl("chk_CoverSheet");
                                    TextBox txt_Days = (TextBox)grdCubeCasting.Rows[j].Cells[1].FindControl("txt_Days");

                                    if (d == txt_Days.Text)
                                    {
                                        chkBx.Checked = true;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        //call in ShowTrialDetail
        protected void DisplayCubeCasting()
        {
            int i = 0;
            if (lbl_TrialId.Text != "")
            {
                var cubeCast = dc.OtherCubeTestView(txt_RefNo.Text, txt_RecType.Text, 0, Convert.ToInt32(lbl_TrialId.Text), "Trial", false, false);
                foreach (var c in cubeCast)
                {
                    AddRowCubeCasting();
                    TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Days");
                    TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[2].FindControl("txt_Cubes");
                    TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");

                    txt_Days.Text = Convert.ToString(c.Days_tint);
                    txt_Cubes.Text = Convert.ToString(c.NoOfCubes_tint);
                    txt_AvgCompStr.Text = c.Avg_var;

                    i++;
                }
            }
            if (grdCubeCasting.Rows.Count <= 0)
            {
                AddRowCubeCasting();
            }
        }

        protected void CalculateExp28DaysCompStrength()
        {
            string mGrade = "";
            int temprature = 0;
            var MFDtls = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, txt_RefNo.Text, 0, 0, 0).ToList();
            if (MFDtls.Count() > 0)
            {
                mGrade = MFDtls.FirstOrDefault().MFINWD_Grade_var;
            }
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var m in data)
            {
                int monthOfCasting = Convert.ToInt32(Convert.ToDateTime(m.Trial_Date).ToString("MM"));
                if (monthOfCasting == 1)
                    monthOfCasting = 20;
                else if (monthOfCasting == 2)
                    monthOfCasting = 22;
                else if (monthOfCasting == 3)
                    monthOfCasting = 21;
                else if (monthOfCasting == 4)
                    monthOfCasting = 28;
                else if (monthOfCasting == 5)
                    monthOfCasting = 30;
                else if (monthOfCasting == 6)
                    monthOfCasting = 27;
                else if (monthOfCasting == 7)
                    monthOfCasting = 25;
                else if (monthOfCasting == 8)
                    monthOfCasting = 25;
                else if (monthOfCasting == 9)
                    monthOfCasting = 25;
                else if (monthOfCasting == 10)
                    monthOfCasting = 25;
                else if (monthOfCasting == 11)
                    monthOfCasting = 22;
                else if (monthOfCasting == 12)
                    monthOfCasting = 20;
                temprature = monthOfCasting;
                break;
            }
            decimal cement = 0, flyash = 0, ggbs = 0, wcRatio = 0, admixture = 0, flyashPercent = 0, ggbsPercent = 0;
            for (int i = 0; i <= grdMD.Rows.Count - 1; i++)
            {
                Label lbl_MaterialName = (Label)grdMD.Rows[i].FindControl("lbl_MaterialName");
                TextBox txt_PerM3 = (TextBox)grdMD.Rows[i].Cells[1].FindControl("txt_PerM3");
                if (lbl_MaterialName.Text == "Cement")
                {
                    cement = Convert.ToDecimal(txt_PerM3.Text);
                }
                else if (lbl_MaterialName.Text == "Fly Ash")
                {
                    flyash = Convert.ToDecimal(txt_PerM3.Text);
                    flyashPercent = flyash / (cement + flyash);
                }
                else if (lbl_MaterialName.Text == "G G B S")
                {
                    ggbs = Convert.ToDecimal(txt_PerM3.Text);
                    ggbsPercent = ggbs / (cement + ggbs);
                }
                else if (lbl_MaterialName.Text == "W/C Ratio")
                {
                    wcRatio = Convert.ToDecimal(txt_PerM3.Text);
                }
                else if (lbl_MaterialName.Text == "Admixture")
                {
                    admixture = Convert.ToDecimal(txt_PerM3.Text);
                }
            }
            
            for (int i = 0; i <= grdCubeCasting.Rows.Count-1; i++)
            {
                decimal exp28daysStrength = 0, TargetExp28daysStrength_yellow = 0, TargetExp28daysStrength_green = 0;
                if (Convert.ToDecimal(mGrade.Replace("M", "")) <= 15)
                {
                    TargetExp28daysStrength_yellow = Convert.ToDecimal(Convert.ToDecimal(mGrade.Replace("M", "")) + Convert.ToDecimal(1.65 * 3.5));
                }
                else if (Convert.ToDecimal(mGrade.Replace("M", "")) >= 20  && Convert.ToDecimal(mGrade.Replace("M", "")) <= 25 )
                    TargetExp28daysStrength_yellow = Convert.ToDecimal(Convert.ToDecimal(mGrade.Replace("M", "")) + Convert.ToDecimal(1.65 * 4));
                else if (Convert.ToDecimal(mGrade.Replace("M", "")) >=30)
                {
                    TargetExp28daysStrength_yellow = Convert.ToDecimal(Convert.ToDecimal(mGrade.Replace("M", "")) + Convert.ToDecimal(1.65 * 5));
                }
                TargetExp28daysStrength_green = TargetExp28daysStrength_yellow;
                decimal K = 0;

                TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Days");
                TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Cubes");
                TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");
                TextBox txt_Exp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Exp28DaysCompStr");
                TextBox txt_TargetExp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetExp28DaysCompStr");
                TextBox txt_TargetMeanStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetMeanStr");
                Label lbl_StandardError = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_StandardError");
                Label lbl_RValue = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_RValue");

                if (txt_AvgCompStr.Text != "" && (txt_Days.Text == "3" || txt_Days.Text == "7"))
                {
                    if (flyash > 0 && ggbs == 0)
                    {
                        if (txt_Days.Text == "3")
                        {
                            K = Convert.ToDecimal(11);
                            exp28daysStrength = K;
                            exp28daysStrength += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.514);
                            exp28daysStrength += cement * Convert.ToDecimal(0.024);
                            exp28daysStrength += flyashPercent * Convert.ToDecimal(7.168);
                            exp28daysStrength += wcRatio * Convert.ToDecimal(-8.062);
                            //exp28daysStrength += admixture * Convert.ToDecimal(0.121);
                            //exp28daysStrength += temprature * Convert.ToDecimal(-0.029);
                            exp28daysStrength += Convert.ToDecimal(txt_AvgCompStr.Text) * Convert.ToDecimal(0.541);
                            txt_Exp28DaysCompStr.Text = exp28daysStrength.ToString("0.00");
                            //TargetExp28daysStrength_yellow += Convert.ToDecimal(3.773 * 1.96 / 2);
                            TargetExp28daysStrength_green += Convert.ToDecimal(3.773);
                            txt_TargetExp28DaysCompStr.Text = TargetExp28daysStrength_green.ToString("0.00");
                            txt_TargetMeanStr.Text = TargetExp28daysStrength_yellow.ToString("0.00");
                            lbl_StandardError.Text = "3.773";
                            lbl_RValue.Text = "0.92";
                        }
                        else if (txt_Days.Text == "7")
                        {
                            K = Convert.ToDecimal(11.693);
                            exp28daysStrength = K;
                            exp28daysStrength += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.534);
                            //exp28daysStrength += cement * Convert.ToDecimal(0.016);
                            //exp28daysStrength += flyash * Convert.ToDecimal(-0.05);
                            //exp28daysStrength += flyashPercent * Convert.ToDecimal(22.653);
                            exp28daysStrength += wcRatio * Convert.ToDecimal(-1.966);
                            //exp28daysStrength += admixture * Convert.ToDecimal(0.11);
                            //exp28daysStrength += temprature * Convert.ToDecimal(-0.026);
                            exp28daysStrength += Convert.ToDecimal(txt_AvgCompStr.Text) * Convert.ToDecimal(0.539);
                            txt_Exp28DaysCompStr.Text = exp28daysStrength.ToString("0.00");
                            //TargetExp28daysStrength_yellow += Convert.ToDecimal(3.54 * 1.96 / 2);
                            TargetExp28daysStrength_green += Convert.ToDecimal(3.54);
                            txt_TargetExp28DaysCompStr.Text = TargetExp28daysStrength_green.ToString("0.00");
                            txt_TargetMeanStr.Text = TargetExp28daysStrength_yellow.ToString("0.00");
                            lbl_StandardError.Text = "3.54";
                            lbl_RValue.Text = "0.932";
                        }
                    }
                    else if (flyash == 0 && ggbs > 0)
                    {
                        if (txt_Days.Text == "3")
                        {
                            K = Convert.ToDecimal(55.023);
                            exp28daysStrength = K;
                            exp28daysStrength += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.262);
                            exp28daysStrength += ggbs * Convert.ToDecimal(-0.03);
                            //exp28daysStrength += ggbsPercent * Convert.ToDecimal(7.168);
                            exp28daysStrength += wcRatio * Convert.ToDecimal(-52.82);
                            //exp28daysStrength += admixture * Convert.ToDecimal(0.121);
                            //exp28daysStrength += temprature * Convert.ToDecimal(-0.029);
                            exp28daysStrength += Convert.ToDecimal(txt_AvgCompStr.Text) * Convert.ToDecimal(0.423);
                            txt_Exp28DaysCompStr.Text = exp28daysStrength.ToString("0.00");
                            //TargetExp28daysStrength_yellow += Convert.ToDecimal(4.209 * 1.96 / 2);
                            TargetExp28daysStrength_green += Convert.ToDecimal(4.209);
                            txt_TargetExp28DaysCompStr.Text = TargetExp28daysStrength_green.ToString("0.00");
                            txt_TargetMeanStr.Text = TargetExp28daysStrength_yellow.ToString("0.00");
                            lbl_StandardError.Text = "4.209";
                            lbl_RValue.Text = "0.871";
                        }
                        else if (txt_Days.Text == "7")
                        {
                            K = Convert.ToDecimal(62.596);
                            exp28daysStrength = K;
                            exp28daysStrength += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.16);
                            exp28daysStrength += cement * Convert.ToDecimal(-0.014);
                            exp28daysStrength += ggbs * Convert.ToDecimal(-0.028);
                            exp28daysStrength += ggbsPercent * Convert.ToDecimal(-14.652);
                            exp28daysStrength += wcRatio * Convert.ToDecimal(-53.894);
                            exp28daysStrength += admixture * Convert.ToDecimal(-1.076);
                            exp28daysStrength += temprature * Convert.ToDecimal(-0.052);
                            exp28daysStrength += Convert.ToDecimal(txt_AvgCompStr.Text) * Convert.ToDecimal(0.563);
                            txt_Exp28DaysCompStr.Text = exp28daysStrength.ToString("0.00");
                            //TargetExp28daysStrength_yellow += Convert.ToDecimal(3.664 * 1.96 / 2);
                            TargetExp28daysStrength_green += Convert.ToDecimal(3.664);
                            txt_TargetExp28DaysCompStr.Text = TargetExp28daysStrength_green.ToString("0.00");
                            txt_TargetMeanStr.Text = TargetExp28daysStrength_yellow.ToString("0.00");
                            lbl_StandardError.Text = "3.664";
                            lbl_RValue.Text = "0.92";
                        }
                    }
                    else
                    {
                        if (txt_Days.Text == "3")
                        {
                            K = Convert.ToDecimal(23.464);
                            exp28daysStrength = K;
                            exp28daysStrength += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.445);
                            exp28daysStrength += cement * Convert.ToDecimal(0.013);
                            exp28daysStrength += wcRatio * Convert.ToDecimal(-19.92);
                            exp28daysStrength += admixture * Convert.ToDecimal(0.121);
                            exp28daysStrength += temprature * Convert.ToDecimal(-0.029);
                            exp28daysStrength += Convert.ToDecimal(txt_AvgCompStr.Text) * Convert.ToDecimal(0.533);
                            txt_Exp28DaysCompStr.Text = exp28daysStrength.ToString("0.00");
                            //TargetExp28daysStrength_yellow += Convert.ToDecimal(3.767 * 1.96 / 2);
                            TargetExp28daysStrength_green += Convert.ToDecimal(3.767);
                            txt_TargetExp28DaysCompStr.Text = TargetExp28daysStrength_green.ToString("0.00");
                            txt_TargetMeanStr.Text = TargetExp28daysStrength_yellow.ToString("0.00");
                            lbl_StandardError.Text = "3.767";
                            lbl_RValue.Text = "0.92";
                        }
                        else if (txt_Days.Text == "7")
                        {
                            K = Convert.ToDecimal(33.074);
                            exp28daysStrength = K;
                            exp28daysStrength += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.298);
                            exp28daysStrength += cement * Convert.ToDecimal(-0.001);
                            exp28daysStrength += wcRatio * Convert.ToDecimal(-30.514);
                            exp28daysStrength += admixture * Convert.ToDecimal(0.025);
                            exp28daysStrength += temprature * Convert.ToDecimal(-0.009);
                            exp28daysStrength += Convert.ToDecimal(txt_AvgCompStr.Text) * Convert.ToDecimal(0.535);
                            txt_Exp28DaysCompStr.Text = exp28daysStrength.ToString("0.00");
                            //TargetExp28daysStrength_yellow += Convert.ToDecimal(3.468 * 1.96 / 2);
                            TargetExp28daysStrength_green += Convert.ToDecimal(3.468);
                            txt_TargetExp28DaysCompStr.Text = TargetExp28daysStrength_green.ToString("0.00");
                            txt_TargetMeanStr.Text = TargetExp28daysStrength_yellow.ToString("0.00");
                            lbl_StandardError.Text = "3.468";
                            lbl_RValue.Text = "0.932";
                        }
                    }
                    
                    if (exp28daysStrength >= TargetExp28daysStrength_green)
                    {
                        grdCubeCasting.Rows[i].BackColor = System.Drawing.Color.LightGreen;
                        txt_Days.BackColor = System.Drawing.Color.LightGreen;
                        txt_Cubes.BackColor = System.Drawing.Color.LightGreen;
                        txt_AvgCompStr.BackColor = System.Drawing.Color.LightGreen;
                        txt_Exp28DaysCompStr.BackColor = System.Drawing.Color.LightGreen;
                        txt_TargetExp28DaysCompStr.BackColor = System.Drawing.Color.LightGreen;
                        txt_TargetMeanStr.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (exp28daysStrength >= TargetExp28daysStrength_yellow) //if (exp28daysStrength >= 90 && exp28daysStrength <= 100)
                    {
                        grdCubeCasting.Rows[i].BackColor = System.Drawing.Color.Yellow;
                        txt_Days.BackColor = System.Drawing.Color.Yellow;
                        txt_Cubes.BackColor = System.Drawing.Color.Yellow;
                        txt_AvgCompStr.BackColor = System.Drawing.Color.Yellow;
                        txt_Exp28DaysCompStr.BackColor = System.Drawing.Color.Yellow;
                        txt_TargetExp28DaysCompStr.BackColor = System.Drawing.Color.Yellow;
                        txt_TargetMeanStr.BackColor = System.Drawing.Color.Yellow;
                    }
                    else //if (exp28daysStrength < 90)
                    {
                        grdCubeCasting.Rows[i].BackColor = System.Drawing.Color.Red;
                        txt_Days.BackColor = System.Drawing.Color.Red;
                        txt_Cubes.BackColor = System.Drawing.Color.Red;
                        txt_AvgCompStr.BackColor = System.Drawing.Color.Red;
                        txt_Exp28DaysCompStr.BackColor = System.Drawing.Color.Red;
                        txt_TargetExp28DaysCompStr.BackColor = System.Drawing.Color.Red;
                        txt_TargetMeanStr.BackColor = System.Drawing.Color.Red;
                    }
                }
            }
            
        }
        protected void CalculateMDLetter()
        {
            bool aggt20mmPresent = false;
            decimal yield = 0, binder = 0, WC1 = 0, WC2 = 0, Water = 0, cement = 0, admixture = 0,
                FA1Percent = 0, FA2Percent = 0, CA1Percent = 0, CA2Percent = 0, CA3Percent = 0,
                FA1 = 0, FA2 = 0, CA1 = 0, CA2 = 0, CA3 = 0, finalYield = 0;
            int i = 0, FACount = 0, CACount = 0;
            grdMD.DataSource = null;
            grdMD.DataBind();
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var t in data)
            {
                if (i == 0)
                {
                    yield = Convert.ToDecimal(t.Trial_Yield);
                }
                if (t.TrialDetail_MaterialName != "Plastic Density" && t.TrialDetail_MaterialName != "Total")
                {
                    if (t.TrialDetail_MaterialName == "20 mm")
                    {
                        aggt20mmPresent = true;
                    }
                    else if (t.TrialDetail_MaterialName.Contains("Cement") == true)
                    {
                        cement = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                    }
                    else if (t.TrialDetail_MaterialName.Contains("Admixture") == true)
                    {
                        admixture = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                    }
                    AddRowMD();
                    Label lbl_MaterialName = (Label)grdMD.Rows[i].FindControl("lbl_MaterialName");
                    Label lbl_MaterialNameActual = (Label)grdMD.Rows[i].FindControl("lbl_MaterialNameActual");
                    Label lbl_MaterialId = (Label)grdMD.Rows[i].FindControl("lbl_MaterialId");
                    TextBox txt_Description = (TextBox)grdMD.Rows[i].FindControl("txt_Description");
                    TextBox txt_PerM3 = (TextBox)grdMD.Rows[i].Cells[1].FindControl("txt_PerM3");
                    TextBox txt_Per50Kg = (TextBox)grdMD.Rows[i].Cells[2].FindControl("txt_Per50Kg");
                    
                    lbl_MaterialName.Text = Convert.ToString(t.TrialDetail_MaterialName);
                    lbl_MaterialNameActual.Text = Convert.ToString(t.TrialDetail_MaterialName);
                    lbl_MaterialId.Text = t.Material_Id.ToString();
                    if (t.TrialDetail_MaterialName == "Natural Sand" || t.TrialDetail_MaterialName == "Crushed Sand"
                       || t.TrialDetail_MaterialName == "Stone Dust" || t.TrialDetail_MaterialName == "Grit")
                    {
                        FACount++;
                        lbl_MaterialName.Text = "Fine Aggregate " + FACount;
                        if (t.MaterialDetail_Information != "-" && t.MaterialDetail_Information != "--")
                            txt_Description.Text = t.TrialDetail_MaterialName + " " + t.MaterialDetail_Information;
                        else
                            txt_Description.Text = t.TrialDetail_MaterialName;
                        if (FACount == 1)
                        {
                            FA1 = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        }
                        else if (FACount == 2)
                        {
                            FA2 = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        }
                    }
                    else if (t.TrialDetail_MaterialName == "10 mm" || t.TrialDetail_MaterialName == "20 mm"
                        || t.TrialDetail_MaterialName == "40 mm")
                    {
                        CACount++;
                        lbl_MaterialName.Text = "Coarse Aggregate " + CACount;
                        if (t.MaterialDetail_Information != "-" && t.MaterialDetail_Information != "--")
                            txt_Description.Text = t.TrialDetail_MaterialName + " Angular " + t.MaterialDetail_Information;
                        else
                            txt_Description.Text = t.TrialDetail_MaterialName + " Angular " ;
                        if (CACount == 1)
                        {
                            CA1 = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        }
                        else if (CACount == 2)
                        {
                            CA2 = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        }
                        else if (CACount == 3)
                        {
                            CA3 = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        }
                    }
                    else if (t.TrialDetail_MaterialName == "Cement")
                    {
                        txt_Description.Text = t.Trial_CementUsed;
                    }
                    else if (t.TrialDetail_MaterialName == "Fly Ash")
                    {
                        txt_Description.Text = t.Trial_FlyashUsed;
                    }
                    else if (t.TrialDetail_MaterialName == "Admixture")
                    {
                        txt_Description.Text = t.Trial_Admixture;
                        chkAdmixtureInKg.Visible = true;
                    }
                    else
                    {
                        txt_Description.Text = t.MaterialDetail_Information;
                    }
                    
                    if (t.TrialDetail_MaterialName == "Admixture")
                    {
                        txt_PerM3.Text = Convert.ToDecimal(t.TrialDetail_ActualTrial).ToString("0.000");
                        txt_Per50Kg.Text = ((Convert.ToDecimal(t.TrialDetail_ActualTrial) * 50) / cement).ToString("0.000");
                        txtAdmixtureInKg.Text = Convert.ToDecimal(t.TrialDetail_ActualTrial * t.TrialDetail_SpecificGravity).ToString("0.000");
                    }
                    else
                    {
                        txt_PerM3.Text = Convert.ToDecimal(t.TrialDetail_ActualTrial).ToString("0");
                        txt_Per50Kg.Text = ((Convert.ToDecimal(t.TrialDetail_ActualTrial) * 50) / cement).ToString("0");
                    }

                    if (lbl_MaterialNameActual.Text.Contains("Cement") == true || lbl_MaterialNameActual.Text.Contains("Fly Ash") == true ||
                        lbl_MaterialNameActual.Text.Contains("G G B S") == true || lbl_MaterialNameActual.Text.Contains("Micro Silica") == true
                        || lbl_MaterialNameActual.Text.Contains("Metakaolin") == true)
                    {
                        binder += Convert.ToDecimal(t.TrialDetail_ActualTrial);
                    }
                    if (lbl_MaterialNameActual.Text == "W/C Ratio")
                    {
                        WC1 = Convert.ToDecimal(t.TrialDetail_Weight);
                        WC2 = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        if (WC2 < WC1 - Convert.ToDecimal(0.02))
                        {
                            WC2 = WC1 - Convert.ToDecimal(0.02);
                        }
                        txt_PerM3.Text = WC2.ToString("0.00");
                        txt_Per50Kg.Text = WC2.ToString("0.00");
                        //txt_Per50Kg.Text = ((WC2 * 50) / cement).ToString("0.00");
                    }
                    i++;
                }

            }
            Water = WC2 * binder;
            FA1Percent = ((FA1) / (FA1 + FA2 + CA1 + CA2 + CA3)) * 100;
            FA2Percent = ((FA2) / (FA1 + FA2 + CA1 + CA2 + CA3)) * 100;
            CA1Percent = ((CA1) / (FA1 + FA2 + CA1 + CA2 + CA3)) * 100;
            CA2Percent = ((CA2) / (FA1 + FA2 + CA1 + CA2 + CA3)) * 100;
            CA3Percent = ((CA3) / (FA1 + FA2 + CA1 + CA2 + CA3)) * 100;
            FA1 = (yield - binder - Water - admixture) * (FA1Percent / 100);
            FA2 = (yield - binder - Water - admixture) * (FA2Percent / 100);
            CA1 = (yield - binder - Water - admixture) * (CA1Percent / 100);
            CA2 = (yield - binder - Water - admixture) * (CA2Percent / 100);
            CA3 = (yield - binder - Water - admixture) * (CA3Percent / 100);
            for (int row = 0; row <= grdMD.Rows.Count - 1; row++)
            {
                Label lbl_MaterialName = (Label)grdMD.Rows[row].FindControl("lbl_MaterialName");
                Label lbl_MaterialNameActual = (Label)grdMD.Rows[row].FindControl("lbl_MaterialNameActual");
                Label lbl_MaterialId = (Label)grdMD.Rows[row].FindControl("lbl_MaterialId");
                TextBox txt_Description = (TextBox)grdMD.Rows[row].FindControl("txt_Description");
                TextBox txt_PerM3 = (TextBox)grdMD.Rows[row].Cells[1].FindControl("txt_PerM3");
                TextBox txt_Per50Kg = (TextBox)grdMD.Rows[row].Cells[2].FindControl("txt_Per50Kg");
                TextBox txt_Volume = (TextBox)grdMD.Rows[row].Cells[2].FindControl("txt_Volume");

                if (lbl_MaterialName.Text == "Water")
                {
                    txt_PerM3.Text = Water.ToString("0");
                    txt_Per50Kg.Text = ((Water * 50) / cement).ToString("0");
                }
                else if (lbl_MaterialName.Text == "Fine Aggregate 1")
                {
                    txt_PerM3.Text = FA1.ToString("0");
                    txt_Per50Kg.Text = ((FA1 * 50) / cement).ToString("0");
                }
                else if (lbl_MaterialName.Text == "Fine Aggregate 2")
                {
                    txt_PerM3.Text = FA2.ToString("0");
                    txt_Per50Kg.Text = ((FA2 * 50) / cement).ToString("0");
                }
                else if (lbl_MaterialName.Text == "Coarse Aggregate 1")
                {
                    txt_PerM3.Text = CA1.ToString("0");
                    txt_Per50Kg.Text = ((CA1 * 50) / cement).ToString("0");
                }
                else if (lbl_MaterialName.Text == "Coarse Aggregate 2")
                {
                    txt_PerM3.Text = CA2.ToString("0");
                    txt_Per50Kg.Text = ((CA2 * 50) / cement).ToString("0");
                }
                else if (lbl_MaterialName.Text == "Coarse Aggregate 3")
                {
                    txt_PerM3.Text = CA3.ToString("0");
                    txt_Per50Kg.Text = ((CA3 * 50) / cement).ToString("0");
                }
                finalYield += Convert.ToDecimal(txt_PerM3.Text);

                //volumetric calculation
                decimal lbd = 0;
                txt_Volume.Text = txt_Per50Kg.Text;
                if (lbl_MaterialId.Text != "")
                {
                    var MfInwd = dc.MF_View1(txt_RefNo.Text, Convert.ToInt32(lbl_MaterialId.Text)).ToList();
                    foreach (var aggt in MfInwd)
                    {
                        if (aggt.AGGTINWD_LBD_var != null && decimal.TryParse(aggt.AGGTINWD_LBD_var, out lbd))
                        {
                            decimal volume = 0;
                            volume = Math.Round(Convert.ToDecimal(txt_Per50Kg.Text) / Convert.ToDecimal(aggt.AGGTINWD_LBD_var));
                            while (volume % 5 != 0)
                            {
                                volume++;
                            }
                            txt_Volume.Text = volume.ToString();
                        }
                        else
                        {
                            string[] RefNo1 = Convert.ToString(txt_RefNo.Text).Split('/');
                            var MfInwd1 = dc.MF_View1(RefNo1[0].ToString() + "/%", Convert.ToInt32(lbl_MaterialId.Text)).ToList();
                            foreach (var aggt1 in MfInwd1)
                            {
                                if (aggt1.AGGTINWD_LBD_var != null && decimal.TryParse(aggt1.AGGTINWD_LBD_var, out lbd))
                                {
                                    decimal volume = 0;
                                    volume = Math.Round(Convert.ToDecimal(txt_Per50Kg.Text) / Convert.ToDecimal(aggt1.AGGTINWD_LBD_var));
                                    while (volume % 5 != 0)
                                    {
                                        volume++;
                                    }
                                    txt_Volume.Text = volume.ToString();
                                }
                            }
                        }
                    }
                }
                //
            }
            AddRowMD();
            Label lbl_MaterialNameTotal = (Label)grdMD.Rows[grdMD.Rows.Count - 1].FindControl("lbl_MaterialName");
            Label lbl_MaterialNameActualTotal = (Label)grdMD.Rows[grdMD.Rows.Count - 1].FindControl("lbl_MaterialNameActual");
            Label lbl_MaterialIdTotal = (Label)grdMD.Rows[grdMD.Rows.Count - 1].FindControl("lbl_MaterialId");
            TextBox txt_DescriptionTotal = (TextBox)grdMD.Rows[grdMD.Rows.Count - 1].FindControl("txt_Description");
            TextBox txt_PerM3Total = (TextBox)grdMD.Rows[grdMD.Rows.Count - 1].Cells[1].FindControl("txt_PerM3");
            TextBox txt_Per50KgTotal = (TextBox)grdMD.Rows[grdMD.Rows.Count - 1].Cells[2].FindControl("txt_Per50Kg");
            TextBox txt_VolumeTotal = (TextBox)grdMD.Rows[grdMD.Rows.Count - 1].Cells[2].FindControl("txt_Volume");

            lbl_MaterialNameTotal.Text = "Plastic Density";
            lbl_MaterialNameActualTotal.Text = "Plastic Density";
            lbl_MaterialIdTotal.Text = "0";
            txt_PerM3Total.Text = finalYield.ToString("0");
            txt_Per50KgTotal.Text = "0";
            txt_VolumeTotal.Text = "0";

            grdRemark.DataSource = null;
            grdRemark.DataBind();
            if (ddlBatching.SelectedValue == "Volume Batching")
            {
                grdMD.Columns[4].Visible = true;
            }
            else
            {
                grdMD.Columns[4].Visible = false;
            }
            
            if (aggt20mmPresent == true) //20 mm
            {
                AddRemark("Coarse aggregate is oversize by more than 20 %, hence the coarse aggregate is not recommended for concreting of members having covers of less than 25 mm.");
            }
            if (ddlBatching.SelectedValue == "Volume Batching")
            {
                AddRemark("The volumetric proportions are rounded of to nearest 5 lits, however, in case of volume batching, weight to volume conversion should be done on the basis of Loose Bulk Density of ingredients actually found out at site.");
            }
            if (optMDL.Checked == true)
            {
                //AddRemark("The above mix proportions are based on 3 days compressive strength achieved at our laboratory =  N/mm2.");
                AddRemark("The expected 28 days strength is based on statistical equations established in laboratory using multiple regression.");
                //AddRemark("The above mix proportions are based on accelerated curing method (Boiling Water) as per IS 9013 -1978 RA(2013). Refer Appendix B (Clause 3.1.1) IS 10262 -2009 RA(2014).");
                AddRemark("Given mix proportions will be confirmed on achieving satisfactory 28 day compressive strength at our laboratory.");
                AddRemark("You are requested to take a site trial and give us a feedback, which will enable us to give you the corrections in the mix, before your actual casting.");
                //AddRemark("Corrections for moisture content in sand & aggregate absorption in metal should be made on water / cement ratio (Refer Durocrete Mix Design Manual).");
                AddRemark("Corrections for moisture content in sand & aggregate absorption in metal should be made on water / binder ratio (Refer guidelines provided with this report).");
                //AddRemark("Any change in material like cement, plasticizer, sand or aggregate would require re-validation of design. ");
                AddRemark("Any change in source or type of material like cement, plasticizer, fine or coarse aggregate would require re-validation of mix design. ");
                AddRemark("The above mix design requires certain control practices on site to ensure results.");
            }
            else if (optFinal.Checked == true)
            {
                //AddRemark("Corrections for moisture content in sand & aggregate absorption in metal should be made on water / binder ratio (Refer Durocrete Mix Design Manual).");
                AddRemark("Corrections for moisture content in sand & aggregate absorption in metal should be made on water / binder ratio (Refer guidelines provided with this report).");
                //AddRemark("Any change in material like cement, plasticizer, sand or aggregate would require re-validation of design. ");
                AddRemark("Any change in source or type of material like cement, plasticizer, fine or coarse aggregate would require re-validation of mix design. ");
                AddRemark("The above mix design requires certain control practices on site to ensure results.");
                
            }
            //DisplayTrialRemark();
        }

        //not required display trial remark (may req for rechecking)
        protected void DisplayTrialRemark()
        {
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            int i = 0;
            var re = dc.TrialRemark_View("", txt_RefNo.Text, 0, Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text);
            foreach (var r in re)
            {
                AddRowMDRemark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.TrialRemark_View("", "", Convert.ToInt32(r.TrialDetail_RemarkId), Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text);
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.Trial_Remark_var.ToString();
                    i++;
                }
            }
            if (grdRemark.Rows.Count <= 0)
            {
                AddRowMDRemark();
            }
        }
        
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true && ValidateCoverSht() == true)
            {
                bool validate = false;
                //if (lblStatus.Text == "Check")
                //{
                    for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
                    {
                        CheckBox chkBx = (CheckBox)grdCubeCasting.Rows[i].Cells[0].FindControl("chk_CoverSheet");
                        TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Days");
                        TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[2].FindControl("txt_Cubes");
                        TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");
                        TextBox txt_Exp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Exp28DaysCompStr");
                        TextBox txt_TargetExp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_TargetExp28DaysCompStr");
                        TextBox txt_TargetMeanStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_TargetMeanStr");
                        Label lbl_StandardError = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_StandardError");
                        Label lbl_RValue = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_RValue");

                        if (chkBx.Checked && lblReportType.Text == "MDL")
                        {
                            dc.MDLCoverSheet_Update(txt_RefNo.Text, txt_Days.Text + "|" + txt_AvgCompStr.Text + "|" + txt_Exp28DaysCompStr.Text + "|" + txt_TargetExp28DaysCompStr.Text + "|" + lbl_StandardError.Text + "|" + lbl_RValue.Text + "|" + txt_TargetMeanStr.Text, txt_KindAttention.Text, null, null);
                            lnkCoverShtPrnt.Visible = true;
                        }
                    }
                //}
                if (validate == false)
                {
                    byte MDLReportStatus = 0, FinalReportStatus = 0, enteredBy = 0, checkedBy = 0, testedBy = 0, approvedBy = 0;
                    bool enteredDtFlag = false, checkedDtFlag = false;

                    //if (lblStatus.Text == "Enter")
                    //{
                        if (lblReportType.Text == "MDL")
                        {
                            MDLReportStatus = 2;
                            enteredBy = Convert.ToByte(Session["LoginId"]);
                            testedBy = Convert.ToByte(ddlTestdBy.SelectedItem.Value);
                        }
                        else
                        {
                            FinalReportStatus = 2;
                        }
                        enteredDtFlag = true;
                        checkedDtFlag = false;
                        dc.MixDesignInward_Update_ReportData(MDLReportStatus, FinalReportStatus, txt_RefNo.Text, txtWitnessBy.Text, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, chkFlow.Checked,chkAdmixtureInKg.Checked);
                        dc.MISDetail_Update(0, "MF", txt_RefNo.Text, lblReportType.Text, null, enteredDtFlag, checkedDtFlag, false, false, false, false, false);
                    //}
                    //else if (lblStatus.Text == "Check")
                    //{
                        if (lblReportType.Text == "MDL")
                        {
                            MDLReportStatus = 3;
                        }
                        else
                        {
                            FinalReportStatus = 3;
                        }
                        testedBy = Convert.ToByte(ddlTestdBy.SelectedItem.Value);
                        enteredBy = Convert.ToByte(Session["LoginId"]);
                        checkedBy = Convert.ToByte(Session["LoginId"]);
                        approvedBy = Convert.ToByte(ddlApprdBy.SelectedItem.Value);
                        enteredDtFlag = false;
                        checkedDtFlag = true;
                        dc.MixDesignInward_Update_ReportData(MDLReportStatus, FinalReportStatus, txt_RefNo.Text, txtWitnessBy.Text, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, chkFlow.Checked, chkAdmixtureInKg.Checked);
                        dc.MISDetail_Update(0, "MF", txt_RefNo.Text, lblReportType.Text, null, enteredDtFlag, checkedDtFlag, false, false, false, false, false);
                    //}

                    dc.MixDesignLetter_Update(txt_RefNo.Text, 0, lblReportType.Text, 0, "", "", "", 0, 0, 0, true);
                    //save grid data
                    int materialId = 0;
                    for (int i = 0; i <= grdMD.Rows.Count -1 ; i++)
                    {
                        Label lbl_MaterialName = (Label)grdMD.Rows[i].FindControl("lbl_MaterialName");
                        Label lbl_MaterialNameActual = (Label)grdMD.Rows[i].FindControl("lbl_MaterialNameActual");
                        Label lbl_MaterialId = (Label)grdMD.Rows[i].FindControl("lbl_MaterialId");
                        TextBox txt_Description = (TextBox)grdMD.Rows[i].FindControl("txt_Description");
                        TextBox txt_PerM3 = (TextBox)grdMD.Rows[i].Cells[1].FindControl("txt_PerM3");
                        TextBox txt_Per50Kg = (TextBox)grdMD.Rows[i].Cells[2].FindControl("txt_Per50Kg");
                        TextBox txt_Volume = (TextBox)grdMD.Rows[i].Cells[3].FindControl("txt_Volume");

                        if (lbl_MaterialId.Text != "")
                            materialId = Convert.ToInt32(lbl_MaterialId.Text);
                        else
                            materialId = 0;
                        
                        dc.MixDesignLetter_Update(txt_RefNo.Text, Convert.ToByte(i + 1), lblReportType.Text, materialId, lbl_MaterialName.Text, lbl_MaterialNameActual.Text, txt_Description.Text, Convert.ToDecimal(txt_PerM3.Text), Convert.ToDecimal(txt_Per50Kg.Text), Convert.ToDecimal(txt_Volume.Text), false);
                    }
                    dc.Trail_Update_OtherInfo(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, ddlBatching.SelectedValue , false);
                    #region Remark
                    int RemarkId = 0;
                    dc.TrialRemarkDetail_Update(txt_RefNo.Text, RemarkId, Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text, true);
                    for (int i = 0; i < grdRemark.Rows.Count; i++)
                    {
                        TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                        if (txt_REMARK.Text != "")
                        {
                            bool valid = false;
                            var chcek = dc.TrialRemark_View(txt_REMARK.Text, "", 0, Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text);
                            foreach (var n in chcek)
                            {
                                valid = true;
                                RemarkId = Convert.ToInt32(n.Trial_RemarkId);
                                Boolean chk = false;
                                var chkId = dc.TrialRemark_View("", txt_RefNo.Text, 0, Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text);
                                foreach (var c in chkId)
                                {
                                    if (c.TrialDetail_RemarkId == RemarkId)
                                    {
                                        chk = true;
                                    }
                                }
                                if (chk == false)
                                {
                                    dc.TrialRemarkDetail_Update(txt_RefNo.Text, RemarkId, Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text, false);
                                    break;
                                }
                            }
                            if (valid == false)
                            {
                                dc.TrialRemark_Update(0, txt_REMARK.Text);
                                var chc = dc.TrialRemark_View(txt_REMARK.Text, "", 0, Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text);
                                foreach (var n in chc)
                                {
                                    RemarkId = Convert.ToInt32(n.Trial_RemarkId);
                                    dc.TrialRemarkDetail_Update(txt_RefNo.Text, RemarkId, Convert.ToInt32(lbl_TrialId.Text), lblReportType.Text, false);
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Record Saved Successfully";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lnkPrint.Visible = true;
                    lnkSave.Enabled = false;
                }

            }

        }

        protected void Tab_OtherInfo_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 0;
            Tab_CubeCompStr.CssClass = "Initiative";
            Tab_OtherInfo.CssClass = "Click";
        }
        protected void Tab_CubeCompStr_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 1;
            //if (lblStatus.Text == "Check")
            //{
                txt_KindAttention.Focus();
            //}
            Tab_OtherInfo.CssClass = "Initiative";
            Tab_CubeCompStr.CssClass = "Click";
        }
        
        protected void chk_CoverSheet_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            foreach (GridViewRow rw in grdCubeCasting.Rows)
            {
                CheckBox chkCoverSheet = (CheckBox)rw.FindControl("chk_CoverSheet");
                if (chkCoverSheet != activeCheckBox)
                {
                    chkCoverSheet.Checked = false;
                }
                else
                {
                    chkCoverSheet.Checked = true;
                }
            }
        }

        protected void ddlBatching_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBatching.SelectedValue == "Volume Batching")
            {
                AddRemark("The volumetric proportions are rounded of to nearest 5 lits, however, in case of volume batching, weight to volume conversion should be done on the basis of Loose Bulk Density of ingredients actually found out at site.");
                grdMD.Columns[4].Visible = true;
            }
            else
            { 
                DeleteRemark("The volumetric proportions are rounded of to nearest 5 lits, however, in case of volume batching, weight to volume conversion should be done on the basis of Loose Bulk Density of ingredients actually found out at site.");
                grdMD.Columns[4].Visible = false;
            }
        }

        protected void chkWitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txtWitnessBy.Text = "";
            if (chkWitnessBy.Checked == true)
                txtWitnessBy.Visible = true;
            else
                txtWitnessBy.Visible = false;
        }
        
        protected void optMDL_CheckedChanged(object sender, EventArgs e)
        {
            LoadOtherPendingReports();
        }

        protected void optFinal_CheckedChanged(object sender, EventArgs e)
        {
            LoadOtherPendingReports();
        }

        protected void ClearAllControls()
        {
            grdMD.DataSource = null;
            grdMD.DataBind();
            grdCubeCasting.DataSource = null;
            grdCubeCasting.DataBind();
            grdRemark.DataSource = null;
            grdRemark.DataBind();

            txt_KindAttention.Text = "";
            grdSlump.DataSource = null;
            grdSlump.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnessBy.Visible = false;
            txtWitnessBy.Text = "";
            //ddlTestdBy.Items.Clear();
            if (ddlTestdBy.Items.Count > 0)
                ddlTestdBy.SelectedIndex = 0;
            //ddlApprdBy.Items.Clear();
            if (ddlApprdBy.Items.Count > 0)
                ddlApprdBy.SelectedIndex = 0;

            chkAdmixtureInKg.Visible = false;
            chkAdmixtureInKg.Checked = false;
            txtAdmixtureInKg.Text = "";
            txtAdmixtureInKg.Visible = false;

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            lnkPrint.Visible = false;
            lnkSave.Enabled = true;
        }

        #region validation
        protected Boolean ValidateCoverSht()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            //if (lblStatus.Text == "Check" && lblReportType.Text == "MDL")
            if (lblReportType.Text == "MDL")
            {
                if (txt_KindAttention.Text == "")
                {
                    lblMsg.Text = "Please Enter Kind Attention";
                    MainView.ActiveViewIndex = 1;
                    Tab_OtherInfo.CssClass = "Initiative";
                    Tab_CubeCompStr.CssClass = "Click";
                    txt_KindAttention.Focus();
                    valid = false;
                }
                else if (valid == true)
                {
                    valid = false;
                    for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
                    {
                        CheckBox chkBox = (CheckBox)grdCubeCasting.Rows[i].Cells[0].FindControl("chk_CoverSheet");
                        if (chkBox.Checked)
                        {
                            valid = true;
                        }
                    }
                    if (valid == false)
                    {
                        lblMsg.Text = "Please Select at least one compressive strength. ";
                        valid = false;
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
                    {
                        CheckBox chkBox = (CheckBox)grdCubeCasting.Rows[i].Cells[0].FindControl("chk_CoverSheet");
                        TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_Days");
                        TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");
                        TextBox txt_Exp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Exp28DaysCompStr");
                        TextBox txt_TargetExp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetExp28DaysCompStr");
                        TextBox txt_TargetMeanStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetMeanStr");
                        if (chkBox.Checked)
                        {
                            if (txt_AvgCompStr.Text == "")
                            {
                                lblMsg.Text = "Average Compressive Strength is not available for Selected days ";
                                txt_AvgCompStr.Focus();
                                valid = false;
                                break;
                            }
                            //else if (Convert.ToDecimal(txt_Exp28DaysCompStr.Text) < Convert.ToDecimal(txt_TargetExp28DaysCompStr.Text))
                            //{
                            //    lblMsg.Text = "Expected 28 days Comp Strength should be greater than Target Expected 28 days Comp Strength for Selected days ";
                            //    txt_AvgCompStr.Focus();
                            //    valid = false;
                            //    break;
                            //}
                            else if (txt_TargetExp28DaysCompStr.BackColor == System.Drawing.Color.Red  && Convert.ToBoolean(Session["Superadmin"].ToString()) == false)
                            {
                                lblMsg.Text = "Can not select " + txt_Days.Text + " day compressive strength. ";
                                txt_AvgCompStr.Focus();
                                valid = false;
                                break;
                            }
                        }
                    }
                }
                if (valid == false)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblMsg.Visible = false;
                }
            }
            return valid;
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (txt_RefNo.Text == "")
            {
                lblMsg.Text = "Select report number ";
                txt_RefNo.Focus();
                valid = false;
            }

            if (valid == true)
            {
                // Witness by validation
                if (chkWitnessBy.Checked == true && txtWitnessBy.Text == "")
                {
                    lblMsg.Text = "Please Enter Witness By Name.";
                    txtWitnessBy.Focus();
                    valid = false;
                }
                else if (ddlTestdBy.SelectedIndex <= 0)
                {
                    //dispalyMsg = "Please Select Tested By/Approved By Name.";
                    lblMsg.Text = "Please Select " + lblTestdBy.Text + " Name.";
                    ddlTestdBy.Focus();
                    valid = false;
                }
                else if (ddlApprdBy.SelectedIndex <= 0)
                {
                    //dispalyMsg = "Please Select Tested By/Approved By Name.";
                    lblMsg.Text = "Please Select " + lblApprdBy.Text + " Name.";
                    ddlApprdBy.Focus();
                    valid = false;
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        #endregion

        #region slump grid
        protected void AddRowSlump()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lbl_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_0", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_30", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_60", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_90", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_120", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_150", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_180", typeof(string)));

            dr = dt.NewRow();
            dr["lbl_Name"] = string.Empty;
            dr["txt_0"] = string.Empty;
            dr["txt_30"] = string.Empty;
            dr["txt_60"] = string.Empty;
            dr["txt_90"] = string.Empty;
            dr["txt_120"] = string.Empty;
            dr["txt_150"] = string.Empty;
            dr["txt_180"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["SlumpTable"] = dt;
            grdSlump.DataSource = dt;
            grdSlump.DataBind();
        }
        #endregion

        #region md letter grid
        protected void AddRowMD()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MDTable"] != null)
            {
                GetCurrentDataMD();
                dt = (DataTable)ViewState["MDTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lbl_MaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_MaterialNameActual", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_MaterialId", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Description", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_PerM3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Per50Kg", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Volume", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lbl_MaterialName"] = string.Empty;
            dr["lbl_MaterialNameActual"] = string.Empty;
            dr["lbl_MaterialId"] = string.Empty;
            dr["txt_Description"] = string.Empty;
            dr["txt_PerM3"] = string.Empty;
            dr["txt_Per50Kg"] = string.Empty;
            dr["txt_Volume"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["MDTable"] = dt;
            grdMD.DataSource = dt;
            grdMD.DataBind();
            SetPreviousDataMD();
        }
        protected void GetCurrentDataMD()
        {
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lbl_MaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_MaterialNameActual", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_MaterialId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Description", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_PerM3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Per50Kg", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Volume", typeof(string)));
            for (int i = 0; i < grdMD.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdMD.Rows[i].FindControl("lbl_MaterialName");
                Label lbl_MaterialNameActual = (Label)grdMD.Rows[i].FindControl("lbl_MaterialNameActual");
                Label lbl_MaterialId = (Label)grdMD.Rows[i].FindControl("lbl_MaterialId");
                TextBox txt_Description = (TextBox)grdMD.Rows[i].FindControl("txt_Description");
                TextBox txt_PerM3 = (TextBox)grdMD.Rows[i].Cells[1].FindControl("txt_PerM3");
                TextBox txt_Per50Kg = (TextBox)grdMD.Rows[i].Cells[2].FindControl("txt_Per50Kg");
                TextBox txt_Volume = (TextBox)grdMD.Rows[i].Cells[3].FindControl("txt_Volume");

                drRow = dtTable.NewRow();
                drRow["lbl_MaterialName"] = lbl_MaterialName.Text;
                drRow["lbl_MaterialNameActual"] = lbl_MaterialNameActual.Text;
                drRow["lbl_MaterialId"] = lbl_MaterialId.Text;
                drRow["txt_Description"] = txt_Description.Text;
                drRow["txt_PerM3"] = txt_PerM3.Text;
                drRow["txt_Per50Kg"] = txt_Per50Kg.Text;
                drRow["txt_Volume"] = txt_Volume.Text;
                dtTable.Rows.Add(drRow);
            }
            ViewState["MDTable"] = dtTable;
        }
        protected void SetPreviousDataMD()
        {
            DataTable dt = (DataTable)ViewState["MDTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdMD.Rows[i].FindControl("lbl_MaterialName");
                Label lbl_MaterialNameActual = (Label)grdMD.Rows[i].FindControl("lbl_MaterialNameActual");
                Label lbl_MaterialId = (Label)grdMD.Rows[i].FindControl("lbl_MaterialId");
                TextBox txt_Description = (TextBox)grdMD.Rows[i].FindControl("txt_Description");
                TextBox txt_PerM3 = (TextBox)grdMD.Rows[i].Cells[1].FindControl("txt_PerM3");
                TextBox txt_Per50Kg = (TextBox)grdMD.Rows[i].Cells[2].FindControl("txt_Per50Kg");
                TextBox txt_Volume = (TextBox)grdMD.Rows[i].Cells[3].FindControl("txt_Volume");

                lbl_MaterialName.Text = dt.Rows[i]["lbl_MaterialName"].ToString();
                lbl_MaterialNameActual.Text = dt.Rows[i]["lbl_MaterialNameActual"].ToString();
                lbl_MaterialId.Text = dt.Rows[i]["lbl_MaterialId"].ToString();
                txt_Description.Text = dt.Rows[i]["txt_Description"].ToString();
                txt_PerM3.Text = dt.Rows[i]["txt_PerM3"].ToString();
                txt_Per50Kg.Text = dt.Rows[i]["txt_Per50Kg"].ToString();
                txt_Volume.Text = dt.Rows[i]["txt_Volume"].ToString();
            }
        }
        #endregion

        #region cube strength grid
        protected void AddRowCubeCasting()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CubeCastingTable"] != null)
            {
                GetCurrentDataCubeCasting();
                dt = (DataTable)ViewState["CubeCastingTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_Days", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Cubes", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AvgCompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Exp28DaysCompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TargetExp28DaysCompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TargetMeanStr", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_StandardError", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_RValue", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_Days"] = string.Empty;
            dr["txt_Cubes"] = string.Empty;
            dr["txt_AvgCompStr"] = string.Empty;
            dr["txt_Exp28DaysCompStr"] = string.Empty;
            dr["txt_TargetExp28DaysCompStr"] = string.Empty;
            dr["txt_TargetMeanStr"] = string.Empty;
            dr["lbl_StandardError"] = string.Empty;
            dr["lbl_RValue"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CubeCastingTable"] = dt;
            grdCubeCasting.DataSource = dt;
            grdCubeCasting.DataBind();
            SetPreviousDataCubeCasting();
        }
        protected void GetCurrentDataCubeCasting()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("txt_Days", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Cubes", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgCompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Exp28DaysCompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TargetExp28DaysCompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TargetMeanStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_StandardError", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_RValue", typeof(string)));

            for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Days");
                TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[2].FindControl("txt_Cubes");
                TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");
                TextBox txt_Exp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Exp28DaysCompStr");
                TextBox txt_TargetExp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetExp28DaysCompStr");
                TextBox txt_TargetMeanStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetMeanStr");
                Label lbl_StandardError = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_StandardError");
                Label lbl_RValue = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_RValue");

                drRow = dtTable.NewRow();
                drRow["txt_Days"] = txt_Days.Text;
                drRow["txt_Cubes"] = txt_Cubes.Text;
                drRow["txt_AvgCompStr"] = txt_AvgCompStr.Text;
                drRow["txt_Exp28DaysCompStr"] = txt_Exp28DaysCompStr.Text;
                drRow["txt_TargetExp28DaysCompStr"] = txt_TargetExp28DaysCompStr.Text;
                drRow["txt_TargetMeanStr"] = txt_TargetMeanStr.Text;
                drRow["lbl_StandardError"] = lbl_StandardError.Text;
                drRow["lbl_RValue"] = lbl_RValue.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CubeCastingTable"] = dtTable;
        }
        protected void SetPreviousDataCubeCasting()
        {
            DataTable dt = (DataTable)ViewState["CubeCastingTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Days");
                TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[2].FindControl("txt_Cubes");
                TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");
                TextBox txt_Exp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Exp28DaysCompStr");
                TextBox txt_TargetExp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetExp28DaysCompStr");
                TextBox txt_TargetMeanStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetMeanStr");
                Label lbl_StandardError = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_StandardError");
                Label lbl_RValue = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_RValue");

                txt_Days.Text = dt.Rows[i]["txt_Days"].ToString();
                txt_Cubes.Text = dt.Rows[i]["txt_Cubes"].ToString();
                txt_AvgCompStr.Text = dt.Rows[i]["txt_AvgCompStr"].ToString();
                txt_Exp28DaysCompStr.Text = dt.Rows[i]["txt_Exp28DaysCompStr"].ToString();
                txt_TargetExp28DaysCompStr.Text = dt.Rows[i]["txt_TargetExp28DaysCompStr"].ToString();
                txt_TargetMeanStr.Text = dt.Rows[i]["txt_TargetMeanStr"].ToString();
                lbl_StandardError.Text = dt.Rows[i]["lbl_StandardError"].ToString();
                lbl_RValue.Text = dt.Rows[i]["lbl_RValue"].ToString();
            }
        }
        #endregion

        #region remark grid
        protected void AddRowMDRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MDRemarkTable"] != null)
            {
                GetCurrentDataMDRemark();
                dt = (DataTable)ViewState["MDRemarkTable"];
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
            ViewState["MDRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataMDRemark();
        }
        protected void GetCurrentDataMDRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgCompStr", typeof(string)));

            for (int i = 0; i < grdRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MDRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataMDRemark()
        {
            DataTable dt = (DataTable)ViewState["MDRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                grdRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        public void AddRemark(string strRemark)
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
                AddRowMDRemark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[grdRemark.Rows.Count - 1].Cells[1].FindControl("txt_REMARK");
                txt_REMARK.Text = strRemark;
            }
        }
        public void DeleteRemark(string strRemark)
        {
            if (grdRemark.Rows.Count > 0)
            {
                for (int i = 0; i < grdRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text.Contains(strRemark) == true)
                    {
                        DeleteRowMDRemark(i);
                        break;
                    }
                }
            }
        }
        protected void DeleteRowMDRemark(int rowIndex)
        {
            GetCurrentDataMDRemark();
            DataTable dt = ViewState["MDRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MDRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataMDRemark();
        }
        protected void imgBtnAddRowRemark_Click(object sender, CommandEventArgs e)
        {
            AddRowMDRemark();
        }
        protected void imgBtnDelRowRemark_Click(object sender, CommandEventArgs e)
        {
            if (grdRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowMDRemark(gvr.RowIndex);
            }
        }
        #endregion

        #region print
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.MF_MDLetter_PDFReport(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text), txt_RecType.Text, lblReportType.Text, lblStatus.Text);
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, lblStatus.Text, "", "", lblReportType.Text, "", "", "", "");
        }

        protected void lnkSievePrnt_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rptPdf.MFSieveAnalysis_PDF(txt_RefNo.Text, txt_RecType.Text, "Print");
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, "Print", "", "", "Sieve Analysis", "", "", "", "");
        }

        protected void lnkCoverShtPrnt_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.MDLCoverSheet_PDF(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text), "Print");
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, "Print", "", "", "Cover Sheet", "", "", "", "");
        }

        protected void lnkMoisuteCorrPrnt_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.MoistureCorrection_PDF(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text), "Print");
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, "Print", "", "", "Moisture Correction", "", "", "", "");
        }
        #endregion

        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void chkAdmixtureInKg_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdmixtureInKg.Checked == true)
                txtAdmixtureInKg.Visible = true;
            else
                txtAdmixtureInKg.Visible = false;
        }
    }
}