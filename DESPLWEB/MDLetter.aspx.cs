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
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using System.Net;
//using System.Text;
//using iTextSharp.text.html.simpleparser;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class MDLetter : System.Web.UI.Page
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

                optMDL.Checked = true;
                optEnter.Checked = true;
                lblReportType.Text = "MDL";
                lblStatus.Text = "Enter";
                LoadOtherPendingReports();
                LoadApprovedBy();
                txtEntdChkdBy.Text = Session["LoginUserName"].ToString();
                if (lblReportType.Text == "MDL")
                {
                    if (lblStatus.Text == "Check")
                        lblheading.Text = "Checking MD Letter";
                    else
                        lblheading.Text = "Entering MD Letter";
                }
                else
                {
                    if (lblStatus.Text == "Check")
                        lblheading.Text = "Checking Final Report";
                    else
                        lblheading.Text = "Entering Final Report";
                    lnkCoverShtPrnt.Visible = true;
                    lnkMoisuteCorrPrnt.Visible = true;
                }
                if (lbl_TrialId.Text != "")
                {
                    if (CheckForNewTrial() == false)
                    {
                        ShowTrialDetail();
                        DisplayTrialRemark();

                        ShowTrialInfo();
                        DisplayMDLetter(lblReportType.Text);
                        if (lblReportType.Text == "Final")
                            lnkLoadMDLValues.Visible = true;
                        else
                            lnkLoadMDLValues.Visible = false;
                        if (lblStatus.Text == "Check")
                        {
                            lnkMoisuteCorrPrnt.Visible = true;
                            //LoadApproveBy();
                            txtCementitious.ReadOnly = false;
                            txtCompaction.ReadOnly = false;
                            txtAfter.ReadOnly = false;
                            txtSlumpnotExceed.ReadOnly = false;
                            txtSlumpnotExcdAfter.ReadOnly = false;
                            txtGradeofConcrete.ReadOnly = false;
                            txtStdDev.ReadOnly = false;
                        }
                        else
                        {
                            //LoadTestedBy();
                            ShowMDLetter();
                        }
                    }
                    else
                    {
                        Label lblMsg = (Label)Master.FindControl("lblMsg");
                        lblMsg.Text = "Select MDL/Final Report - New option from menu.";
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        private void LoadApprovedBy()
        {
            byte testBit = 0;
            byte apprBit = 0;
            if (lblStatus.Text == "Enter")
            {
                testBit = 1;
                apprBit = 0;
            }
            else if (lblStatus.Text == "Check")
            {
                testBit = 0;
                apprBit = 1;
            }

            ddlTestdApprdBy.DataTextField = "USER_Name_var";
            ddlTestdApprdBy.DataValueField = "USER_Id";
            var apprUser = dc.ReportStatus_View("", null, null, 0, testBit, apprBit, "", 0, 0, 0);
            ddlTestdApprdBy.DataSource = apprUser;
            ddlTestdApprdBy.DataBind();
            ddlTestdApprdBy.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        protected void ddlOtherPendingRpt_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
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

                if (optEnter.Checked == true)
                    lblStatus.Text = "Enter";
                else if (optCheck.Checked == true)
                    lblStatus.Text = "Check";

                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblReportType.Text == "MDL")
                {
                    if (lblStatus.Text == "Check")
                        lblheading.Text = "Checking MD Letter";
                    else
                        lblheading.Text = "Entering MD Letter";
                }
                else
                {
                    if (lblStatus.Text == "Check")
                        lblheading.Text = "Checking Final Report";
                    else
                        lblheading.Text = "Entering Final Report";
                    lnkCoverShtPrnt.Visible = true;
                    lnkMoisuteCorrPrnt.Visible = true;
                }
                //var mixd = dc.AllInward_View("MF", 0, txt_RefNo.Text);  
                txt_RefNo.Text = ddlOtherPendingRpt.SelectedValue;
                var mixd = dc.Trial_View(txt_RefNo.Text, true);
                lbl_TrialId.Text = mixd.FirstOrDefault().Trial_Id.ToString();

                if (CheckForNewTrial() == false)
                {
                    LoadApprovedBy();
                    ShowTrialDetail();
                    ShowTrialInfo();
                    DisplayMDLetter(lblReportType.Text);
                    if (lblStatus.Text == "Check")
                    {
                        lnkMoisuteCorrPrnt.Visible = true;
                        //LoadApproveBy();
                        txtCementitious.ReadOnly = false;
                        txtCompaction.ReadOnly = false;
                        txtAfter.ReadOnly = false;
                        txtSlumpnotExceed.ReadOnly = false;
                        txtSlumpnotExcdAfter.ReadOnly = false;
                        txtGradeofConcrete.ReadOnly = false;
                        txtStdDev.ReadOnly = false;
                        DisplayTrialRemark();
                    }
                    else
                    {
                        if (lblReportType.Text == "Final")
                            lnkLoadMDLValues.Visible = true;
                        //LoadTestedBy();
                        ShowMDLetter();
                    }
                }
                else
                {
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Select MDL/Final Report - New option from menu.";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ClearAllControls()
        {
            grdMDLetter.DataSource = null;
            grdMDLetter.DataBind();
            grdTrialInfo.DataSource = null;
            grdTrialInfo.DataBind();
            grdCubeCasting.DataSource = null;
            grdCubeCasting.DataBind();
            grdRemark.DataSource = null;
            grdRemark.DataBind();

            txt_KindAttention.Text = "";
            txtCementitious.Text = "";
            txtCompaction.Text = "";
            txtAfter.Text = "";
            txtSlumpnotExceed.Text = "";
            txtCementUsed.Text = "";
            txtSlumpnotExcdAfter.Text = "";
            txtGradeofConcrete.Text = "";
            txtStdDev.Text = "";
            txtAdmixureUsed.Text = "";
            txtSpecReqment.Text = "";
            txtNatureofWork.Text = "";
            txtFlyashUsed.Text = "";

            chkWitnessBy.Checked = false;
            txtWitnessBy.Visible = false;
            txtWitnessBy.Text = "";
            //ddlTestdApprdBy.SelectedIndex = 0;
            ddlTestdApprdBy.Items.Clear();

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            lnkPrint.Visible = false;
            lnkSave.Enabled = true;
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

                if (optEnter.Checked == true)
                {
                    lblEntdChkdBy.Text = "Entered By";
                    lblTestdApprdBy.Text = "Tested By";
                }
                else if (optCheck.Checked == true)
                {
                    lblEntdChkdBy.Text = "Checked By";
                    lblTestdApprdBy.Text = "Approved By";
                }
            }

            int i = 0;
            var data = dc.MDLetter_View(txt_RefNo.Text, MFStatus);
            foreach (var m in data)
            {
                AddRowMDLetter();
                Label lbl_Name = (Label)grdMDLetter.Rows[i].Cells[0].FindControl("lbl_Name");
                TextBox txt_Cement = (TextBox)grdMDLetter.Rows[i].Cells[1].FindControl("txt_Cement");
                TextBox txt_FlyAsh = (TextBox)grdMDLetter.Rows[i].Cells[2].FindControl("txt_FlyAsh");
                TextBox txt_GGBS = (TextBox)grdMDLetter.Rows[i].Cells[3].FindControl("txt_GGBS");
                TextBox txt_MicroSilica = (TextBox)grdMDLetter.Rows[i].Cells[4].FindControl("txt_MicroSilica");
                TextBox txt_Metakaolin = (TextBox)grdMDLetter.Rows[i].Cells[5].FindControl("txt_Metakaolin");

                TextBox txt_WaterBinder = (TextBox)grdMDLetter.Rows[i].Cells[6].FindControl("txt_WaterBinder");
                TextBox txt_Watertobeadded = (TextBox)grdMDLetter.Rows[i].Cells[7].FindControl("txt_Watertobeadded");
                TextBox txt_CrushedSand = (TextBox)grdMDLetter.Rows[i].Cells[8].FindControl("txt_CrushedSand");
                TextBox txt_NaturalSand = (TextBox)grdMDLetter.Rows[i].Cells[9].FindControl("txt_NaturalSand");
                TextBox txt_StoneDust = (TextBox)grdMDLetter.Rows[i].Cells[10].FindControl("txt_StoneDust");
                TextBox txt_Grit = (TextBox)grdMDLetter.Rows[i].Cells[11].FindControl("txt_Grit");
                TextBox txt_10mm = (TextBox)grdMDLetter.Rows[i].Cells[12].FindControl("txt_10mm");
                TextBox txt_20mm = (TextBox)grdMDLetter.Rows[i].Cells[13].FindControl("txt_20mm");
                TextBox txt_40mm = (TextBox)grdMDLetter.Rows[i].Cells[14].FindControl("txt_40mm");
                TextBox txt_Admixture = (TextBox)grdMDLetter.Rows[i].Cells[15].FindControl("txt_Admixture");


                if (i == 0)
                {
                    lbl_Name.Text = "By Weight kg ";
                }
                if (i == 1 || i == 2)
                {
                    lbl_Name.Text = "Wt. in kg /m³ ";
                }
                if (i == 2)
                {
                    Label lblFor = (Label)grdMDLetter.Rows[i - 1].Cells[0].FindControl("lbl_Name");
                    lblFor.Text = "Volume ";
                }

                txt_Cement.Text = Convert.ToString(m.MD_Cement);
                if (Convert.ToDecimal(m.MD_FlyAsh) > 0)
                {
                    txt_FlyAsh.Text = Convert.ToString(m.MD_FlyAsh);
                    grdMDLetter.Columns[2].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[2].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_GGBS) > 0)
                {
                    txt_GGBS.Text = Convert.ToString(m.MD_GGBS);
                    grdMDLetter.Columns[3].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[3].Visible = false;
                }

                if (Convert.ToDecimal(m.MD_MicroSilica) > 0)
                {
                    txt_MicroSilica.Text = Convert.ToString(m.MD_MicroSilica);
                    grdMDLetter.Columns[4].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[4].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_Metakaolin) > 0)
                {
                    txt_Metakaolin.Text = Convert.ToString(m.MD_Metakaolin);
                    grdMDLetter.Columns[5].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[5].Visible = false;
                }
                txt_WaterBinder.Text = Convert.ToString(m.MD_WaterBinder);
                txt_Watertobeadded.Text = Convert.ToString(m.MD_WaterTobeAdded);
                if (Convert.ToDecimal(m.MD_NaturalSand) > 0)
                {
                    txt_NaturalSand.Text = Convert.ToString(m.MD_NaturalSand);
                    grdMDLetter.Columns[8].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[8].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_CrushedSand) > 0)
                {
                    txt_CrushedSand.Text = Convert.ToString(m.MD_CrushedSand);
                    grdMDLetter.Columns[9].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[9].Visible = false;
                }

                if (Convert.ToDecimal(m.MD_StoneDust) > 0)
                {
                    txt_StoneDust.Text = Convert.ToString(m.MD_StoneDust);
                    grdMDLetter.Columns[10].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[10].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_Grit) > 0)
                {
                    txt_Grit.Text = Convert.ToString(m.MD_Grit);
                    grdMDLetter.Columns[11].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[11].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_Tenmm) > 0)
                {
                    txt_10mm.Text = Convert.ToString(m.MD_Tenmm);
                    grdMDLetter.Columns[12].Visible = true;
                }
                else
                {
                    grdMDLetter.Columns[12].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_Twentymm) > 0)
                {
                    txt_20mm.Text = Convert.ToString(m.MD_Twentymm);
                }
                else
                {
                    grdMDLetter.Columns[13].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_Fortymm) > 0)
                {
                    txt_40mm.Text = Convert.ToString(m.MD_Fortymm);
                }
                else
                {
                    grdMDLetter.Columns[14].Visible = false;
                }
                if (Convert.ToDecimal(m.MD_Admixture) > 0)
                {
                    txt_Admixture.Text = Convert.ToString(m.MD_Admixture);
                }
                else
                {
                    grdMDLetter.Columns[15].Visible = false;
                }
                i++;
            }
        }
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

                    ////var CompStr = dc.OtherCubeTestView(txt_RefNo.Text, "MF", Convert.ToByte(txt_Days.Text), 0, "Trial", false, true);
                    //var CompStr = dc.OtherCubeTestView(txt_RefNo.Text, "MF", Convert.ToByte(txt_Days.Text), 0, "Trial", false, false);
                    //foreach (var cms in CompStr)
                    //{
                    //    txt_AvgCompStr.Text = Convert.ToString(cms.Avg_var);
                    //    if (txt_AvgCompStr.Text != "")
                    //        break;
                    //}
                    i++;
                }
            }
            if (grdCubeCasting.Rows.Count <= 0)
            {
                AddRowCubeCasting();
            }
        }

        protected void ShowTrialDetail()
        {

            if (lbl_TrialId.Text != "")
            {
                lbl_TrialId.Text = Convert.ToString(lbl_TrialId.Text);
            }
            txt_RecType.Text = "MF";
            //   txt_RefNo.Text = Convert.ToString(Session["ReportNo"]);
            int i = 0;
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var m in data)
            {
                if (i == 0)
                {
                    if (m.Trial_CementUsed != null && m.Trial_CementUsed != "")
                    {
                        lblCement.Visible = true;
                        txtCementUsed.Visible = true;
                        txtCementUsed.Text = m.Trial_CementUsed.ToString();
                    }
                    if (m.Trial_FlyashUsed != null && m.Trial_FlyashUsed != "")
                    {
                        lblFlyash.Visible = true;
                        txtFlyashUsed.Visible = true;
                        txtFlyashUsed.Text = m.Trial_FlyashUsed.ToString();
                    }
                    if (m.Trial_Admixture != null && m.Trial_Admixture != "")
                    {
                        lblAdmix.Visible = true;
                        txtAdmixureUsed.Visible = true;
                        txtAdmixureUsed.Text = m.Trial_Admixture.ToString();
                    }

                    string[] strSlump = m.Trial_BatchSlumpValue.Split('|');
                    txtSlumpnotExceed.Text = strSlump[0];
                    if (Convert.ToInt32(txtSlumpnotExceed.Text) > Convert.ToInt32(strSlump[1]))
                    {
                        txtSlumpnotExceed.Text = strSlump[1];
                    }
                    if (Convert.ToInt32(txtSlumpnotExceed.Text) > Convert.ToInt32(strSlump[2]))
                    {
                        txtSlumpnotExceed.Text = strSlump[2];
                    }
                    if (m.Trial_RetentionStatus == true)
                    {
                        txtAfter.Text = Convert.ToString(m.Trial_RetTimeDuration);
                        strSlump = m.Trial_RetentionSlumpValue.Split('|');
                        txtSlumpnotExcdAfter.Text = strSlump[0];
                        if (Convert.ToInt32(txtSlumpnotExcdAfter.Text) > Convert.ToInt32(strSlump[1]))
                        {
                            txtSlumpnotExcdAfter.Text = strSlump[1];
                        }
                        if (Convert.ToInt32(txtSlumpnotExcdAfter.Text) > Convert.ToInt32(strSlump[2]))
                        {
                            txtSlumpnotExcdAfter.Text = strSlump[2];
                        }
                    }
                    else
                    {
                        lblAfter.Visible = false;
                        txtAfter.Visible = false;
                        lblAfterMin.Visible = false;
                        lblSlumpNotExcd.Visible = false;
                        txtSlumpnotExcdAfter.Visible = false;
                        lblSlumpNotExcdmm.Visible = false;
                    }
                    if (Convert.ToString(m.Trial_WitnessBy) != "")
                    {
                        chkWitnessBy.Checked = true;
                        txtWitnessBy.Visible = true;
                        txtWitnessBy.Text = Convert.ToString(m.Trial_WitnessBy);
                    }
                    lblTrialName.Text = m.Trial_Name.ToString();

                    i++;
                }
            }

            DisplayCubeCasting();
            MainView.ActiveViewIndex = 0;
            // Fetch Inward Data
            var Inwdata = dc.MF_View(txt_RefNo.Text, 0, "MF");
            foreach (var m in Inwdata)
            {
                txtGradeofConcrete.Text = Convert.ToString(m.MFINWD_Grade_var);
                txtNatureofWork.Text = Convert.ToString(m.MFINWD_NatureofWork_var);
                txtSpecReqment.Text = Convert.ToString(m.MFINWD_SpecialRequirement_var);
                lblRecDate.Text = Convert.ToString(m.MFINWD_ReceivedDate_dt);
                if (Convert.ToDecimal(txtGradeofConcrete.Text.Replace("M", "")) <= 15)
                    txtStdDev.Text = "3.5";
                else if (Convert.ToDecimal(txtGradeofConcrete.Text.Replace("M", "")) >= 20 && Convert.ToDecimal(txtGradeofConcrete.Text.Replace("M", "")) <= 25)
                    txtStdDev.Text = "4";
                else
                    txtStdDev.Text = "5";

                if (lblStatus.Text == "")//  Session["lnkMDLIssue"] == null)
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
        protected void DisplayTrialRemark()
        {
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

        protected void ShowTrialInfo()
        {
            int i = 0;
            var res = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var t in res)
            {
                if (t.TrialDetail_MaterialName != "Total")
                {
                    AddRowTrialInfo();
                    Label lblMaterialName = (Label)grdTrialInfo.Rows[i].Cells[0].FindControl("lblMaterialName");
                    TextBox txt_TrialValue = (TextBox)grdTrialInfo.Rows[i].Cells[1].FindControl("txt_TrialValue");
                    TextBox txt_SpecGravity = (TextBox)grdTrialInfo.Rows[i].Cells[2].FindControl("txt_SpecGravity");

                    lblMaterialName.Text = t.TrialDetail_MaterialName.ToString();
                    txt_TrialValue.Text = t.TrialDetail_Weight.ToString();
                    txt_SpecGravity.Text = t.TrialDetail_SpecificGravity.ToString();
                    if (i == 0)
                    {
                        if (t.Trial_OtherInfo != null)
                        {
                            string[] strOther = t.Trial_OtherInfo.Split('|');
                            txtCementitious.Text = strOther[0];
                            txtCompaction.Text = strOther[1];
                            txtAfter.Text = strOther[2];
                            txtSlumpnotExceed.Text = strOther[3];
                            txtSlumpnotExcdAfter.Text = strOther[4];
                            txtGradeofConcrete.Text = strOther[5];
                            txtStdDev.Text = strOther[6];
                        }
                    }
                    i++;
                }
            }
        }

        protected void ShowMDLetter()
        {
            txtCementitious.Text = "0";
            txtCompaction.Text = "0";
            int j = 0;
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var d in data)
            {
                if (Convert.ToString(d.Trial_Batching) == "Volume Batching")
                {
                    j = 3;
                }
                else if (Convert.ToString(d.Trial_Batching) == "Weight Batching")
                {
                    j = 2;
                }
                break;
            }
            if (grdMDLetter.Rows.Count <= 0)
            {
                for (int i = 0; i < j; i++)
                {
                    AddRowMDLetter();
                    Label lbl_Name = (Label)grdMDLetter.Rows[i].Cells[0].FindControl("lbl_Name");
                    if (i == 0)
                    {
                        lbl_Name.Text = "By Weight kg ";
                    }
                    if (i == 1 || i == 2)
                    {
                        lbl_Name.Text = "Wt. in kg /m³ ";
                    }
                    if (j == 3 && i == 1)
                    {
                        lbl_Name.Text = "Volume ";
                    }
                }
            }
            grdMDLetter.Columns[2].Visible = false;
            grdMDLetter.Columns[3].Visible = false;
            grdMDLetter.Columns[4].Visible = false;
            grdMDLetter.Columns[5].Visible = false;
            grdMDLetter.Columns[8].Visible = false;
            grdMDLetter.Columns[9].Visible = false;
            grdMDLetter.Columns[10].Visible = false;
            grdMDLetter.Columns[11].Visible = false;
            grdMDLetter.Columns[12].Visible = false;
            grdMDLetter.Columns[13].Visible = false;
            grdMDLetter.Columns[14].Visible = false;
            grdMDLetter.Columns[15].Visible = false;

            //var res = dc.MaterialDetail_View(0, txt_RefNo.Text, 0, "", null, null);
            var res = dc.MaterialDetail_View(0, txt_RefNo.Text, 0, "", null, null, "");
            foreach (var t in res)
            {

                if (Convert.ToString(t.Material_List) == "Fly Ash")
                {
                    grdMDLetter.Columns[2].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "G G B S")
                {
                    grdMDLetter.Columns[3].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "Micro Silica")
                {
                    grdMDLetter.Columns[4].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "Metakaolin")
                {
                    grdMDLetter.Columns[5].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "Natural Sand")
                {
                    grdMDLetter.Columns[8].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "Crushed Sand")
                {
                    grdMDLetter.Columns[9].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "Stone Dust")
                {
                    grdMDLetter.Columns[10].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "Grit")
                {
                    grdMDLetter.Columns[11].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "10 mm")
                {
                    grdMDLetter.Columns[12].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "20 mm")
                {
                    grdMDLetter.Columns[13].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "40 mm")
                {
                    grdMDLetter.Columns[14].Visible = true;
                }
                if (Convert.ToString(t.Material_List) == "Admixture")
                {
                    grdMDLetter.Columns[15].Visible = true;
                }
            }
            double tProp = 0, recprop1 = 0, recprop2 = 0, Yield = 0, Fly_Ash_By_Min_Adm = 0, d2 = 0, d3 = 0, cemcon = 0, CemCont = 0, fly_ash_cont = 0, flyashcal = 0, Fly_Ash_By_Min_Arr = 0, wat_to_be_add = 0, Wat_To_Be_Add_Arr = 0, WCRatio = 0;
            double ggbscal = 0, ggbs_cont = 0, ggbs_cont1 = 0, GGBS_By_Min_Arr = 0, wat_to_be_add1 = 0, Wat_To_Add = 0, Cem_Cont_Prop = 0, v1Kgm3 = 0, v2Kgm3 = 0, v3Kgm3 = 0, v4Kgm3 = 0, WaterKgm3 = 0, TempWtKg3 = 0, v1 = 0, Old_Adm = 0; //GGBS_By_Min_Adm = 0,
            double metakaolincal = 0, metakaolin_cont = 0, metakaolin_cont1 = 0, Metakaolin_By_Min_Arr = 0;
            double Silicacal = 0, Silica_cont = 0, Silica_cont1 = 0, Silica_By_Min_Arr = 0;

            bool flgNew = false, flyAshFlag = false, ggbsFlag = false;
            bool flgSilica = false, flgmetakaolin = false;
            clsData obj = new clsData();
            var trial = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text)).ToList();
            foreach (var t in trial)
            {
                if (t.TrialDetail_MaterialName == "Cement" || t.TrialDetail_MaterialName == "Fly Ash"
                    || t.TrialDetail_MaterialName == "G G B S" || t.TrialDetail_MaterialName == "Micro Silica")
                    txtCementitious.Text = (Convert.ToDouble(txtCementitious.Text) + Convert.ToDouble(t.TrialDetail_Weight)).ToString();
                #region textbox
                TextBox txt_Cement3 = new TextBox();
                TextBox txt_FlyAsh3 = new TextBox();
                TextBox txt_GGBS3 = new TextBox();
                TextBox txt_Metakaolin3 = new TextBox();
                TextBox txt_MicroSilica3 = new TextBox();
                TextBox txt_WaterBinder3 = new TextBox();
                TextBox txt_Watertobeadded3 = new TextBox();
                TextBox txt_CrushedSand3 = new TextBox();
                TextBox txt_NaturalSand3 = new TextBox();
                TextBox txt_StoneDust3 = new TextBox();
                TextBox txt_Grit3 = new TextBox();
                TextBox txt_10mm3 = new TextBox();
                TextBox txt_20mm3 = new TextBox();
                TextBox txt_40mm3 = new TextBox();
                TextBox txt_Admixture3 = new TextBox();

                TextBox txt_Cement1 = (TextBox)grdMDLetter.Rows[0].Cells[1].FindControl("txt_Cement");
                TextBox txt_Cement2 = (TextBox)grdMDLetter.Rows[1].Cells[1].FindControl("txt_Cement");

                TextBox txt_FlyAsh1 = (TextBox)grdMDLetter.Rows[0].Cells[2].FindControl("txt_FlyAsh");
                TextBox txt_FlyAsh2 = (TextBox)grdMDLetter.Rows[1].Cells[2].FindControl("txt_FlyAsh");


                TextBox txt_GGBS1 = (TextBox)grdMDLetter.Rows[0].Cells[3].FindControl("txt_GGBS");
                TextBox txt_GGBS2 = (TextBox)grdMDLetter.Rows[1].Cells[3].FindControl("txt_GGBS");
                TextBox txt_MicroSilica1 = (TextBox)grdMDLetter.Rows[0].Cells[4].FindControl("txt_MicroSilica");
                TextBox txt_MicroSilica2 = (TextBox)grdMDLetter.Rows[1].Cells[4].FindControl("txt_MicroSilica");
                TextBox txt_Metakaolin1 = (TextBox)grdMDLetter.Rows[0].Cells[5].FindControl("txt_Metakaolin");
                TextBox txt_Metakaolin2 = (TextBox)grdMDLetter.Rows[1].Cells[5].FindControl("txt_Metakaolin");

                TextBox txt_WaterBinder1 = (TextBox)grdMDLetter.Rows[0].Cells[6].FindControl("txt_WaterBinder");
                TextBox txt_WaterBinder2 = (TextBox)grdMDLetter.Rows[1].Cells[6].FindControl("txt_WaterBinder");

                TextBox txt_Watertobeadded1 = (TextBox)grdMDLetter.Rows[0].Cells[7].FindControl("txt_Watertobeadded");
                TextBox txt_Watertobeadded2 = (TextBox)grdMDLetter.Rows[1].Cells[7].FindControl("txt_Watertobeadded");

                TextBox txt_CrushedSand1 = (TextBox)grdMDLetter.Rows[0].Cells[8].FindControl("txt_CrushedSand");
                TextBox txt_CrushedSand2 = (TextBox)grdMDLetter.Rows[1].Cells[8].FindControl("txt_CrushedSand");

                TextBox txt_NaturalSand1 = (TextBox)grdMDLetter.Rows[0].Cells[9].FindControl("txt_NaturalSand");
                TextBox txt_NaturalSand2 = (TextBox)grdMDLetter.Rows[1].Cells[9].FindControl("txt_NaturalSand");

                TextBox txt_StoneDust1 = (TextBox)grdMDLetter.Rows[0].Cells[10].FindControl("txt_StoneDust");
                TextBox txt_StoneDust2 = (TextBox)grdMDLetter.Rows[1].Cells[10].FindControl("txt_StoneDust");

                TextBox txt_Grit1 = (TextBox)grdMDLetter.Rows[0].Cells[11].FindControl("txt_Grit");
                TextBox txt_Grit2 = (TextBox)grdMDLetter.Rows[1].Cells[11].FindControl("txt_Grit");

                TextBox txt_10mm1 = (TextBox)grdMDLetter.Rows[0].Cells[12].FindControl("txt_10mm");
                TextBox txt_10mm2 = (TextBox)grdMDLetter.Rows[1].Cells[12].FindControl("txt_10mm");

                TextBox txt_20mm1 = (TextBox)grdMDLetter.Rows[0].Cells[13].FindControl("txt_20mm");
                TextBox txt_20mm2 = (TextBox)grdMDLetter.Rows[1].Cells[13].FindControl("txt_20mm");

                TextBox txt_40mm1 = (TextBox)grdMDLetter.Rows[0].Cells[14].FindControl("txt_40mm");
                TextBox txt_40mm2 = (TextBox)grdMDLetter.Rows[1].Cells[14].FindControl("txt_40mm");



                TextBox txt_Admixture1 = (TextBox)grdMDLetter.Rows[0].Cells[15].FindControl("txt_Admixture");
                TextBox txt_Admixture2 = (TextBox)grdMDLetter.Rows[1].Cells[15].FindControl("txt_Admixture");





                if (grdMDLetter.Rows.Count == 3)
                {
                    txt_Cement3 = (TextBox)grdMDLetter.Rows[2].Cells[1].FindControl("txt_Cement");
                    txt_FlyAsh3 = (TextBox)grdMDLetter.Rows[2].Cells[2].FindControl("txt_FlyAsh");
                    txt_GGBS3 = (TextBox)grdMDLetter.Rows[2].Cells[3].FindControl("txt_GGBS");
                    txt_MicroSilica3 = (TextBox)grdMDLetter.Rows[2].Cells[4].FindControl("txt_MicroSilica");
                    txt_Metakaolin3 = (TextBox)grdMDLetter.Rows[2].Cells[5].FindControl("txt_Metakaolin");
                    txt_WaterBinder3 = (TextBox)grdMDLetter.Rows[2].Cells[6].FindControl("txt_WaterBinder");
                    txt_Watertobeadded3 = (TextBox)grdMDLetter.Rows[2].Cells[7].FindControl("txt_Watertobeadded");
                    txt_CrushedSand3 = (TextBox)grdMDLetter.Rows[2].Cells[8].FindControl("txt_CrushedSand");
                    txt_NaturalSand3 = (TextBox)grdMDLetter.Rows[2].Cells[9].FindControl("txt_NaturalSand");
                    txt_StoneDust3 = (TextBox)grdMDLetter.Rows[2].Cells[10].FindControl("txt_StoneDust");
                    txt_Grit3 = (TextBox)grdMDLetter.Rows[2].Cells[11].FindControl("txt_Grit");
                    txt_10mm3 = (TextBox)grdMDLetter.Rows[2].Cells[12].FindControl("txt_10mm");
                    txt_20mm3 = (TextBox)grdMDLetter.Rows[2].Cells[13].FindControl("txt_20mm");
                    txt_40mm3 = (TextBox)grdMDLetter.Rows[2].Cells[14].FindControl("txt_40mm");
                    txt_Admixture3 = (TextBox)grdMDLetter.Rows[2].Cells[15].FindControl("txt_Admixture");
                }
                #endregion

                #region cement
                if (Convert.ToString(t.TrialDetail_MaterialName) == "Cement")
                {
                    Cem_Cont_Prop = Convert.ToDouble(t.TrialDetail_Weight);
                    d2 = (0.15 * 0.15 * 0.15) * Convert.ToDouble(t.Trial_NoOfCubes);
                    d3 = (d2 * 0.2) + d2;

                    cemcon = Convert.ToDouble(t.TrialDetail_Weight) * d3;
                    CemCont = Convert.ToDouble(t.TrialDetail_Weight) / 50;

                    grdMDLetter.Columns[1].Visible = true;
                    txt_Cement1.Text = "50";
                    if (t.Trial_Batching == "Volume Batching")
                        txt_Cement2.Text = "35";
                    if (t.TrialDetail_Volume != null)
                    {
                        tProp = Convert.ToDouble(t.TrialDetail_Weight) / 50;
                        flgNew = true;
                    }
                }
                #endregion

                #region fly ash
                foreach (var t1 in trial)
                {
                    if (Convert.ToString(t1.TrialDetail_MaterialName) == "Plastic Density")
                        Yield = Convert.ToDouble(t1.TrialDetail_Weight);

                    if (Convert.ToString(t1.TrialDetail_MaterialName) == "W/C Ratio")
                        WCRatio = Convert.ToDouble(t1.TrialDetail_Weight);

                }
                recprop1 = Yield / (Cem_Cont_Prop / 50);
                recprop2 = recprop1 - 50 - (WCRatio * 50);

                if (Convert.ToString(t.TrialDetail_MaterialName) == "Fly Ash")
                {
                    grdMDLetter.Columns[2].Visible = true;

                    flyashcal = Convert.ToDouble(t.TrialDetail_Weight);
                    fly_ash_cont = flyashcal * d3;
                    fly_ash_cont = Convert.ToDouble(Math.Round(obj.myRoundOffFn(Convert.ToDecimal(fly_ash_cont)), 3));
                    if (fly_ash_cont > 0)
                    {
                        Fly_Ash_By_Min_Arr = Convert.ToDouble(Convert.ToInt32(flyashcal / CemCont));
                    }
                    wat_to_be_add = (50 + (flyashcal / CemCont)) * Convert.ToDouble(t.TrialDetail_Weight);
                    Wat_To_Be_Add_Arr = Convert.ToDouble(Math.Round(obj.myRoundOffFn(Convert.ToDecimal(wat_to_be_add)), 2));
                    Fly_Ash_By_Min_Adm = Fly_Ash_By_Min_Arr;
                    fly_ash_cont = Convert.ToDouble(t.TrialDetail_Weight);

                    recprop2 = recprop1 - 50 - Fly_Ash_By_Min_Adm - wat_to_be_add;
                    txt_FlyAsh1.Text = Fly_Ash_By_Min_Adm.ToString();
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_FlyAsh2.Text = (Convert.ToDouble(txt_FlyAsh1.Text) / Convert.ToDouble("1.180")).ToString("0");
                    }
                    flyAshFlag = true;
                }
                #endregion

                #region ggbs
                if (Convert.ToString(t.TrialDetail_MaterialName) == "G G B S")
                {
                    grdMDLetter.Columns[3].Visible = true;
                    GGBS_By_Min_Arr = 0;

                    ggbscal = Convert.ToDouble(t.TrialDetail_Weight);
                    ggbs_cont1 = ggbscal * d3;
                    ggbs_cont1 = Convert.ToDouble(Math.Round(Convert.ToDecimal(ggbs_cont1), 3));
                    if (ggbs_cont1 > 0)
                        GGBS_By_Min_Arr = Convert.ToDouble(Convert.ToDouble(ggbscal / CemCont));
                    ggbs_cont = ggbs_cont + Convert.ToDouble(t.TrialDetail_Weight);

                    wat_to_be_add1 = (50 + (ggbscal / CemCont)) * Convert.ToDouble(t.TrialDetail_Weight);
                    txt_GGBS1.Text = GGBS_By_Min_Arr.ToString("0");

                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_GGBS2.Text = (Convert.ToDouble(txt_GGBS1.Text) / Convert.ToDouble("1.007")).ToString("0");
                    }
                    else
                    {
                        txt_GGBS2.Text = ggbscal.ToString("0");
                    }
                    ggbsFlag = true;


                }
                #endregion

                #region micro silica
                if (Convert.ToString(t.TrialDetail_MaterialName) == "Micro Silica")
                {
                    grdMDLetter.Columns[4].Visible = true;
                    Silica_By_Min_Arr = 0;

                    Silicacal = Convert.ToDouble(t.TrialDetail_Weight);
                    Silica_cont1 = Silicacal * d3;
                    Silica_cont1 = Convert.ToDouble(Math.Round(Convert.ToDecimal(Silica_cont1), 3));
                    if (Silica_cont1 > 0)
                        Silica_By_Min_Arr = Convert.ToDouble(Convert.ToInt32(Silicacal / CemCont));
                    Silica_cont = Silica_cont + Convert.ToDouble(t.TrialDetail_Weight);

                    wat_to_be_add1 = (50 + (Silicacal / CemCont)) * Convert.ToDouble(t.TrialDetail_Weight);
                    txt_MicroSilica1.Text = Silica_By_Min_Arr.ToString("0");
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_MicroSilica2.Text = (Convert.ToDouble(txt_MicroSilica1.Text) / Convert.ToDouble("0.674")).ToString("0");
                    }
                    else
                    {
                        txt_MicroSilica2.Text = Silicacal.ToString("0");
                    }
                    //SilicaFlag = true;
                    flgSilica = true;
                }
                #endregion
                #region metakaolin
                if (Convert.ToString(t.TrialDetail_MaterialName) == "Metakaolin")
                {
                    grdMDLetter.Columns[5].Visible = true;
                    Metakaolin_By_Min_Arr = 0;

                    metakaolincal = Convert.ToDouble(t.TrialDetail_Weight);
                    metakaolin_cont1 = metakaolincal * d3;
                    metakaolin_cont1 = Convert.ToDouble(Math.Round(Convert.ToDecimal(metakaolin_cont1), 3));
                    if (metakaolin_cont1 > 0)
                        Metakaolin_By_Min_Arr = Convert.ToDouble(Convert.ToInt32(metakaolincal / CemCont));
                    metakaolin_cont = metakaolin_cont + Convert.ToDouble(t.TrialDetail_Weight);

                    wat_to_be_add1 = (50 + (metakaolincal / CemCont)) * Convert.ToDouble(t.TrialDetail_Weight);
                    txt_Metakaolin1.Text = Metakaolin_By_Min_Arr.ToString("0");
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_Metakaolin2.Text = (Convert.ToDouble(txt_Metakaolin1.Text) / Convert.ToDouble("0.604")).ToString("0");
                    }
                    else
                    {
                        txt_Metakaolin2.Text = metakaolincal.ToString("0");
                    }
                    //metakaolinFlag = true;
                    flgmetakaolin = true;
                }
                #endregion
                #region wc ratio
                if (Convert.ToString(t.TrialDetail_MaterialName) == "W/C Ratio")
                {
                    txt_WaterBinder1.Text = t.TrialDetail_Weight.ToString();
                    txt_WaterBinder2.Text = t.TrialDetail_Weight.ToString();
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_WaterBinder3.Text = t.TrialDetail_Weight.ToString();
                    }
                }
                #endregion

                #region water
                if (Convert.ToString(t.TrialDetail_MaterialName) == "Water")
                {
                    if (flyAshFlag == true || ggbsFlag == true || flgSilica == true || flgmetakaolin == true)
                    {
                        if (flyAshFlag == true)
                            Wat_To_Add = (Convert.ToDouble(txt_Cement1.Text) + Convert.ToDouble(txt_FlyAsh1.Text)) * WCRatio;
                        else if (flgSilica == true)
                            Wat_To_Add = (Convert.ToDouble(txt_Cement1.Text) + Convert.ToDouble(txt_MicroSilica1.Text)) * WCRatio;
                        else if (ggbsFlag == true)
                            Wat_To_Add = (Convert.ToDouble(txt_Cement1.Text) + Convert.ToDouble(txt_GGBS1.Text)) * WCRatio;
                        if (txt_FlyAsh1.Text == "") txt_FlyAsh1.Text = "0";
                        if (txt_Metakaolin1.Text == "") txt_Metakaolin1.Text = "0";
                        if (txt_GGBS1.Text == "") txt_GGBS1.Text = "0";
                        if (txt_MicroSilica1.Text == "") txt_MicroSilica1.Text = "0";

                        Wat_To_Add = (Convert.ToDouble(txt_Cement1.Text) + Convert.ToDouble(txt_FlyAsh1.Text)
                            + Convert.ToDouble(txt_MicroSilica1.Text) + Convert.ToDouble(txt_GGBS1.Text) + Convert.ToDouble(txt_Metakaolin1.Text)) * WCRatio;

                        if (t.Trial_Batching == "Volume Batching")
                        {
                            v1Kgm3 = Cem_Cont_Prop + fly_ash_cont + ggbs_cont + Silica_cont + metakaolin_cont;
                            v2Kgm3 = v1Kgm3 * WCRatio;
                            txt_Cement3.Text = Cem_Cont_Prop.ToString("0");
                            txt_FlyAsh3.Text = fly_ash_cont.ToString("0");
                            // added on 13/12/17
                            if (ggbsFlag == true)
                            {
                                txt_GGBS3.Text = ggbs_cont.ToString("0");
                            }
                            if (flgSilica == true)
                            {
                                txt_MicroSilica3.Text = Silica_cont.ToString("0");
                            }
                            if (flgmetakaolin == true)
                            {
                                txt_Metakaolin3.Text = metakaolin_cont.ToString("0");
                            }
                            // end added
                            v3Kgm3 = v2Kgm3 * WCRatio;
                            v4Kgm3 = v1Kgm3 - v2Kgm3 - v3Kgm3;
                            txt_WaterBinder2.Text = WCRatio.ToString();
                            txt_WaterBinder3.Text = WCRatio.ToString();
                            txt_Watertobeadded1.Text = Math.Round(Wat_To_Add, 2).ToString();
                            txt_Watertobeadded2.Text = Math.Round(Wat_To_Add, 2).ToString();
                            txt_Watertobeadded3.Text = Math.Round(v2Kgm3, 2).ToString();
                        }
                        else
                        {
                            v1Kgm3 = Cem_Cont_Prop + fly_ash_cont + ggbs_cont + Silica_cont + metakaolin_cont;
                            v2Kgm3 = v1Kgm3 * WCRatio;
                            txt_Cement2.Text = Cem_Cont_Prop.ToString("0");
                            txt_FlyAsh2.Text = (fly_ash_cont + ggbs_cont + Silica_cont + metakaolin_cont).ToString("0");
                            v3Kgm3 = v2Kgm3 * WCRatio;
                            v4Kgm3 = v1Kgm3 - v2Kgm3 - v3Kgm3;
                            txt_WaterBinder2.Text = WCRatio.ToString();
                            txt_Watertobeadded1.Text = Math.Round(Wat_To_Add, 2).ToString();
                            txt_Watertobeadded2.Text = Math.Round(v2Kgm3, 0).ToString();
                        }
                    }
                    else
                    {
                        if (t.Trial_Batching == "Volume Batching")
                        {
                            WaterKgm3 = Cem_Cont_Prop * WCRatio;
                            txt_Watertobeadded2.Text = Math.Round(obj.myRoundOffFn(Convert.ToDecimal(WCRatio * 50)), 2).ToString();
                            txt_Watertobeadded3.Text = Math.Round(obj.myRoundOffFn(Convert.ToDecimal(WaterKgm3)), 2).ToString();
                            v1Kgm3 = Yield * 0.15 * 0.15 * 0.15 * 6;
                            v2Kgm3 = (v1Kgm3 * Cem_Cont_Prop) / Yield;
                            txt_Cement3.Text = Cem_Cont_Prop.ToString();
                            v3Kgm3 = v2Kgm3 * WCRatio;
                            txt_Watertobeadded1.Text = Math.Round(obj.myRoundOffFn(Convert.ToDecimal(WCRatio * 50)), 2).ToString();
                            v4Kgm3 = v1Kgm3 - v2Kgm3 - v3Kgm3;
                            txt_WaterBinder2.Text = WCRatio.ToString();
                            txt_WaterBinder3.Text = WCRatio.ToString();
                        }
                        else
                        {

                            WaterKgm3 = Cem_Cont_Prop * WCRatio;
                            //txt_Watertobeadded2.Text = FormatNumber(WaterKgm3,1).ToString();
                            txt_Watertobeadded2.Text = Math.Round(obj.myRoundOffFn(Convert.ToDecimal(WaterKgm3)), 0).ToString();
                            txt_Cement2.Text = Cem_Cont_Prop.ToString("0");
                            if (flgNew == false)
                            {
                                v1Kgm3 = Yield * 0.15 * 0.15 * 0.15 * 6;
                                v2Kgm3 = (v1Kgm3 * Cem_Cont_Prop) / Yield;
                                v3Kgm3 = v2Kgm3 * WCRatio;
                                v4Kgm3 = v1Kgm3 - v2Kgm3 - v3Kgm3;
                                txt_Watertobeadded1.Text = Math.Round(obj.myRoundOffFn(Convert.ToDecimal(WCRatio * 50)), 2).ToString();
                            }
                            else
                            {
                                txt_Watertobeadded1.Text = Math.Round(obj.myRoundOffFn(Convert.ToDecimal(Convert.ToDouble(txt_Watertobeadded2.Text) / tProp)), 2).ToString();
                            }
                            txt_WaterBinder3.Text = WCRatio.ToString();
                        }
                    }
                }

                if (flyAshFlag == true)
                    TempWtKg3 = Convert.ToDouble(Math.Round(obj.myRoundOffFn(Convert.ToDecimal(Yield - v1Kgm3 - v2Kgm3)), 2));
                else
                    TempWtKg3 = Convert.ToDouble(Math.Round(obj.myRoundOffFn(Convert.ToDecimal(Yield - Cem_Cont_Prop - WaterKgm3)), 2));
                #endregion

                #region aggregate materials
                if (Convert.ToString(t.TrialDetail_MaterialName) == "Natural Sand")
                {
                    grdMDLetter.Columns[8].Visible = true;
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_NaturalSand1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString();
                        txt_NaturalSand3.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                        v1 = 0;
                        int MaterialId = 0;
                        var mt = dc.MaterialListView("", "Natural Sand", "Aggregate");
                        foreach (var m in mt)
                        {
                            MaterialId = m.Material_Id;
                        }
                        var aggtOther = dc.AggregateAllTestView(txt_RefNo.Text, MaterialId, "AGGTOT");
                        foreach (var aggt in aggtOther)
                        {
                            if (aggt.AGGTOT_Material_Id == MaterialId && aggt.AGGTINWD_Material_Id == MaterialId)
                            {
                                if (Convert.ToString(aggt.AGGTINWD_LBD_var) != "" && aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var != "Awaited")
                                {
                                    v1 = Convert.ToDouble(txt_NaturalSand1.Text) / Convert.ToDouble(aggt.AGGTINWD_LBD_var);
                                }
                            }
                        }
                        txt_NaturalSand2.Text = Convert.ToInt32(v1).ToString();
                    }
                    else if (flgNew == true)
                    {
                        txt_NaturalSand1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString();
                        txt_NaturalSand2.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString();
                    }
                }

                if (Convert.ToString(t.TrialDetail_MaterialName) == "Crushed Sand")
                {
                    grdMDLetter.Columns[9].Visible = true;
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_CrushedSand1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString();
                        txt_CrushedSand3.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                        v1 = 0;
                        int MaterialId = 0;
                        var mt = dc.MaterialListView("", "Crushed Sand", "Aggregate");
                        foreach (var m in mt)
                        {
                            MaterialId = m.Material_Id;
                        }
                        var aggtOther = dc.AggregateAllTestView(txt_RefNo.Text, MaterialId, "AGGTOT");
                        foreach (var aggt in aggtOther)
                        {
                            if (Convert.ToString(aggt.AGGTINWD_LBD_var) != "" && aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var != "Awaited")
                            {
                                v1 = Convert.ToDouble(txt_CrushedSand1.Text) / Convert.ToDouble(aggt.AGGTINWD_LBD_var);
                            }
                        }
                        txt_CrushedSand2.Text = Convert.ToInt32(v1).ToString();
                    }
                    else if (flgNew == true)
                    {
                        txt_CrushedSand1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString();
                        txt_CrushedSand2.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString();
                    }
                }

                if (Convert.ToString(t.TrialDetail_MaterialName) == "Stone Dust")
                {
                    grdMDLetter.Columns[10].Visible = true;
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_StoneDust1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString();
                        txt_StoneDust3.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                        v1 = 0;
                        int MaterialId = 0;
                        var mt = dc.MaterialListView("", "Stone Dust", "Aggregate");
                        foreach (var m in mt)
                        {
                            MaterialId = m.Material_Id;
                        }
                        var aggtOther = dc.AggregateAllTestView(txt_RefNo.Text, MaterialId, "AGGTOT");
                        foreach (var aggt in aggtOther)
                        {
                            if (Convert.ToString(aggt.AGGTINWD_LBD_var) != "" && aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var != "Awaited")
                            {
                                v1 = Convert.ToDouble(txt_StoneDust1.Text) / Convert.ToDouble(aggt.AGGTINWD_LBD_var);
                            }
                        }
                        txt_StoneDust2.Text = Convert.ToInt32(v1).ToString();
                    }
                    else if (flgNew == true)
                    {
                        txt_StoneDust1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString();
                        txt_StoneDust2.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString();
                    }
                }
                if (Convert.ToString(t.TrialDetail_MaterialName) == "Grit")
                {
                    grdMDLetter.Columns[11].Visible = true;
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_Grit1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_Grit3.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                        v1 = 0;
                        int MaterialId = 0;
                        var mt = dc.MaterialListView("", "Grit", "Aggregate");
                        foreach (var m in mt)
                        {
                            MaterialId = m.Material_Id;
                        }
                        var aggtOther = dc.AggregateAllTestView(txt_RefNo.Text, MaterialId, "AGGTOT");
                        foreach (var aggt in aggtOther)
                        {
                            if (Convert.ToString(aggt.AGGTINWD_LBD_var) != "" && aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var != "Awaited")
                            {
                                v1 = Convert.ToDouble(txt_Grit1.Text) / Convert.ToDouble(aggt.AGGTINWD_LBD_var);
                            }
                        }
                        txt_Grit2.Text = Convert.ToInt32(v1).ToString("0");
                    }
                    else if (flgNew == true)
                    {
                        txt_Grit1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_Grit2.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                    }
                }
                if (Convert.ToString(t.TrialDetail_MaterialName) == "10 mm")
                {
                    grdMDLetter.Columns[12].Visible = true;
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_10mm1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_10mm3.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                        v1 = 0;
                        int MaterialId = 0;
                        var mt = dc.MaterialListView("", "10 mm", "Aggregate");
                        foreach (var m in mt)
                        {
                            MaterialId = m.Material_Id;
                        }
                        var aggtOther = dc.AggregateAllTestView(txt_RefNo.Text, MaterialId, "AGGTOT");
                        foreach (var aggt in aggtOther)
                        {
                            if (Convert.ToString(aggt.AGGTINWD_LBD_var) != "" && aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var != "Awaited")
                            {
                                v1 = Convert.ToDouble(txt_10mm1.Text) / Convert.ToDouble(aggt.AGGTINWD_LBD_var);
                            }
                        }
                        txt_10mm2.Text = Convert.ToInt32(v1).ToString("0");
                    }
                    else if (flgNew == true)
                    {
                        txt_10mm1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_10mm2.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                    }
                }
                if (Convert.ToString(t.TrialDetail_MaterialName) == "20 mm")
                {
                    grdMDLetter.Columns[13].Visible = true;
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_20mm1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_20mm3.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                        v1 = 0;
                        int MaterialId = 0;
                        var mt = dc.MaterialListView("", "20 mm", "Aggregate");
                        foreach (var m in mt)
                        {
                            MaterialId = m.Material_Id;
                        }
                        var aggtOther = dc.AggregateAllTestView(txt_RefNo.Text, MaterialId, "AGGTOT");
                        foreach (var aggt in aggtOther)
                        {
                            if (Convert.ToString(aggt.AGGTINWD_LBD_var) != "" && aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var != "Awaited")
                            {
                                v1 = Convert.ToDouble(txt_20mm1.Text) / Convert.ToDouble(aggt.AGGTINWD_LBD_var);
                            }
                        }
                        txt_20mm2.Text = Convert.ToInt32(v1).ToString("0");
                    }
                    else if (flgNew == true)
                    {
                        txt_20mm1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_20mm2.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                    }
                }
                if (Convert.ToString(t.TrialDetail_MaterialName) == "40 mm")
                {
                    grdMDLetter.Columns[14].Visible = true;
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_40mm1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_40mm3.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                        v1 = 0;
                        int MaterialId = 0;
                        var mt = dc.MaterialListView("", "40 mm", "Aggregate");
                        foreach (var m in mt)
                        {
                            MaterialId = m.Material_Id;
                        }
                        var aggtOther = dc.AggregateAllTestView(txt_RefNo.Text, MaterialId, "AGGTOT");
                        foreach (var aggt in aggtOther)
                        {
                            if (Convert.ToString(aggt.AGGTINWD_LBD_var) != "" && aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var != "Awaited")
                            {
                                v1 = Convert.ToDouble(txt_40mm1.Text) / Convert.ToDouble(aggt.AGGTINWD_LBD_var);
                            }
                        }
                        txt_40mm2.Text = Convert.ToInt32(v1).ToString("0");
                    }
                    else if (flgNew == true)
                    {
                        txt_40mm1.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume) / tProp).ToString("0");
                        txt_40mm2.Text = Convert.ToInt32(Convert.ToDouble(t.TrialDetail_Volume)).ToString("0");
                    }
                }
                #endregion

                #region admixture
                if (Convert.ToString(t.TrialDetail_MaterialName) == "Admixture")
                {
                    grdMDLetter.Columns[15].Visible = true;
                    if (flyAshFlag == true)
                        txt_Admixture1.Text = (((Convert.ToInt32(txt_Cement1.Text) + Convert.ToInt32(txt_FlyAsh1.Text)) * Convert.ToInt32(t.TrialDetail_Weight)) / 50).ToString("0");
                    else
                        txt_Admixture1.Text = t.TrialDetail_Weight.ToString();

                    if (ggbsFlag == true && flyAshFlag == true)
                        txt_Admixture1.Text = (((Convert.ToInt32(txt_Cement1.Text) + Convert.ToInt32(txt_FlyAsh1.Text) + Convert.ToInt32(txt_GGBS1.Text)) * Convert.ToInt32(t.TrialDetail_Weight)) / 50).ToString("0");
                    else if (ggbsFlag == true && flyAshFlag == false)
                        if (flgSilica == true)
                            txt_Admixture1.Text = (((Convert.ToInt32(txt_Cement1.Text) + Convert.ToInt32(txt_MicroSilica1.Text)) * Convert.ToInt32(t.TrialDetail_Weight)) / 50).ToString("0");
                        else

                            txt_Admixture1.Text = (((Convert.ToInt32(txt_Cement1.Text) + Convert.ToInt32(txt_GGBS1.Text)) * Convert.ToInt32(t.TrialDetail_Weight)) / 50).ToString("0");


                    Old_Adm = Convert.ToDouble(txt_Admixture1.Text);
                    if (t.Trial_Batching == "Volume Batching")
                    {
                        txt_Admixture2.Text = txt_Admixture1.Text;
                        txt_Admixture3.Text = (((Cem_Cont_Prop + fly_ash_cont + ggbs_cont) * Convert.ToDouble(t.TrialDetail_Weight)) / 50).ToString("0");
                    }
                    else
                    {
                        txt_Admixture2.Text = (((Cem_Cont_Prop + fly_ash_cont + ggbs_cont) * Convert.ToDouble(t.TrialDetail_Weight)) / 50).ToString("0");
                    }

                    while (Old_Adm % 25 != 0)
                    {
                        Old_Adm++;
                    }
                    txt_Admixture1.Text = Old_Adm.ToString("0");
                }
                #endregion

                if (optEnter.Checked == true)
                {
                    //AddRemark("Materials used are received on " + Convert.ToDateTime(lblRecDate.Text).ToString("dd-MMM-yyyy"));

                    if (grdMDLetter.Columns[13].Visible == true)
                    {   //If rsSieveSizesPassingTable.Fields("Cumu_Wt_Ret") > 20 Then
                        AddRemark("Coarse aggregate is oversize by more than 20 %, hence the coarse aggregate is not recommended for  concreting of members having covers of less than 25 mm.");
                    }
                    if (flgNew == true && t.Trial_Batching == "Volume Batching")  // volume batching
                    {
                        AddRemark("The volumetric proportions are rounded of to nearest 5 lits, however, in case of volume batching, weight to volume conversion should be done on the basis of Loose Bulk Density of ingredients actually found out at site.");
                    }
                    if (optMDL.Checked == true)
                    {
                        //AddRemark("Accelerated compressive strength achieved at our laboratory =  N/mm2.");
                        AddRemark("The above mix proportions are based on 3 days compressive strength achieved at our laboratory =  N/mm2.");
                        AddRemark("Given mix proportions will be confirmed on achieving satisfactory 28 day compressive strength at our laboratory.");
                        AddRemark("The above mix proportions are based on accelerated curing method (Boiling Water) as per IS 9013 -1978 RA(2013). Refer Appendix B (Clause 3.1.1) IS 10262 -2009 RA(2014).");
                        AddRemark("Corrections for moisture content in sand & aggregate absorption in metal should be made on water / cement ratio (Refer Durocrete Mix Design Manual).");
                        AddRemark("You are requested to take a site trial and give us a feedback, which will enable us to give you the corrections in the mix, before your actual casting.");
                        AddRemark("Any change in material like cement, plasticizer, sand or aggregate would require re-validation of design. ");
                        AddRemark("The above mix design requires certain control practices on site to ensure results.");
                    }
                    else if (optFinal.Checked == true)
                    {
                        AddRemark("Any change in material like cement, plasticizer, sand or aggregate would require re-validation of design. ");
                        AddRemark("The above mix design requires certain control practices on site to ensure results.");
                        AddRemark("Corrections for moisture content in sand & aggregate absorption in metal should be made on water / binder ratio (Refer Durocrete Mix Design Manual).");
                    }

                }
                else
                    DisplayTrialRemark();

            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true && ValidateCoverSht() == true)
            {
                bool validate = false;
                if (lblStatus.Text == "Check")
                {
                    for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
                    {
                        CheckBox chkBx = (CheckBox)grdCubeCasting.Rows[i].Cells[0].FindControl("chk_CoverSheet");
                        TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Days");
                        TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[2].FindControl("txt_Cubes");
                        TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");

                        if (chkBx.Checked && lblReportType.Text == "MDL")
                        {
                            dc.MDLCoverSheet_Update(txt_RefNo.Text, txt_Days.Text + "|" + txt_AvgCompStr.Text, txt_KindAttention.Text, null, null);
                            lnkCoverShtPrnt.Visible = true;
                        }
                    }
                }
                if (validate == false)
                {
                    byte MDLReportStatus = 0, FinalReportStatus = 0, enteredBy = 0, checkedBy = 0, testedBy = 0, approvedBy = 0;
                    bool enteredDtFlag = false, checkedDtFlag = false;

                    if (lblStatus.Text == "Enter")
                    {
                        if (lblReportType.Text == "MDL")
                        {
                            MDLReportStatus = 2;
                            enteredBy = Convert.ToByte(Session["LoginId"]);
                            testedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                        }
                        else
                        {
                            FinalReportStatus = 2;
                        }
                        enteredDtFlag = true;
                    }
                    else if (lblStatus.Text == "Check")
                    {
                        if (lblReportType.Text == "MDL")
                        {
                            MDLReportStatus = 3;
                            checkedBy = Convert.ToByte(Session["LoginId"]);
                            approvedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                        }
                        else
                        {
                            FinalReportStatus = 3;
                            checkedBy = Convert.ToByte(Session["LoginId"]);
                            approvedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                        }
                        checkedDtFlag = true;
                    }

                    dc.MixDesignInward_Update_ReportData(MDLReportStatus, FinalReportStatus, txt_RefNo.Text, txtWitnessBy.Text, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, false, false);
                    dc.MISDetail_Update(0, "MF", txt_RefNo.Text, lblReportType.Text, null, enteredDtFlag, checkedDtFlag, false, false, false, false, false);
                    dc.MDLetter_Update(txt_RefNo.Text, 0, lblReportType.Text, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);

                    for (int i = 0; i < grdMDLetter.Rows.Count; i++)
                    {
                        Label lbl_Name = (Label)grdMDLetter.Rows[i].Cells[0].FindControl("lbl_Name");
                        TextBox txt_Cement = (TextBox)grdMDLetter.Rows[i].Cells[1].FindControl("txt_Cement");
                        TextBox txt_FlyAsh = (TextBox)grdMDLetter.Rows[i].Cells[2].FindControl("txt_FlyAsh");
                        TextBox txt_WaterBinder = (TextBox)grdMDLetter.Rows[i].Cells[3].FindControl("txt_WaterBinder");
                        TextBox txt_Watertobeadded = (TextBox)grdMDLetter.Rows[i].Cells[4].FindControl("txt_Watertobeadded");
                        TextBox txt_NaturalSand = (TextBox)grdMDLetter.Rows[i].Cells[6].FindControl("txt_NaturalSand");
                        TextBox txt_CrushedSand = (TextBox)grdMDLetter.Rows[i].Cells[5].FindControl("txt_CrushedSand");
                        TextBox txt_StoneDust = (TextBox)grdMDLetter.Rows[i].Cells[7].FindControl("txt_StoneDust");
                        TextBox txt_Grit = (TextBox)grdMDLetter.Rows[i].Cells[8].FindControl("txt_Grit");
                        TextBox txt_10mm = (TextBox)grdMDLetter.Rows[i].Cells[9].FindControl("txt_10mm");
                        TextBox txt_20mm = (TextBox)grdMDLetter.Rows[i].Cells[10].FindControl("txt_20mm");
                        TextBox txt_40mm = (TextBox)grdMDLetter.Rows[i].Cells[11].FindControl("txt_40mm");
                        TextBox txt_MicroSilica = (TextBox)grdMDLetter.Rows[i].Cells[12].FindControl("txt_MicroSilica");
                        TextBox txt_GGBS = (TextBox)grdMDLetter.Rows[i].Cells[13].FindControl("txt_GGBS");
                        TextBox txt_Admixture = (TextBox)grdMDLetter.Rows[i].Cells[14].FindControl("txt_Admixture");
                        TextBox txt_Metakaolin = (TextBox)grdMDLetter.Rows[i].Cells[15].FindControl("txt_Metakaolin");

                        if (txt_FlyAsh.Text == "")
                        {
                            txt_FlyAsh.Text = "0";
                        }
                        if (txt_GGBS.Text == "")
                        {
                            txt_GGBS.Text = "0";
                        }
                        if (txt_MicroSilica.Text == "")
                        {
                            txt_MicroSilica.Text = "0";
                        }
                        if (txt_Metakaolin.Text == "")
                        {
                            txt_Metakaolin.Text = "0";
                        }
                        if (txt_NaturalSand.Text == "")
                        {
                            txt_NaturalSand.Text = "0";
                        }
                        if (txt_CrushedSand.Text == "")
                        {
                            txt_CrushedSand.Text = "0";
                        }
                        if (txt_StoneDust.Text == "")
                        {
                            txt_StoneDust.Text = "0";
                        }
                        if (txt_Grit.Text == "")
                        {
                            txt_Grit.Text = "0";
                        }
                        if (txt_10mm.Text == "")
                        {
                            txt_10mm.Text = "0";
                        }
                        if (txt_20mm.Text == "")
                        {
                            txt_20mm.Text = "0";
                        }
                        if (txt_40mm.Text == "")
                        {
                            txt_40mm.Text = "0";
                        }
                        if (txt_Admixture.Text == "")
                        {
                            txt_Admixture.Text = "0";
                        }

                        //txt_Admixture.Text = RoundAdm(txt_Admixture.Text);
                        if (txt_GGBS.Text.Trim() != "")
                        {
                            if (txt_GGBS.Text != "0")
                            {
                                txt_FlyAsh.Text = "0";
                            }
                        }

                        if (txt_MicroSilica.Text.Trim() != "")
                        {
                            if (txt_MicroSilica.Text != "0")
                            {
                                txt_FlyAsh.Text = "0";
                            }
                        }


                        dc.MDLetter_Update(txt_RefNo.Text, Convert.ToByte(i), lblReportType.Text, Convert.ToDecimal(txt_Cement.Text), Convert.ToDecimal(txt_FlyAsh.Text), Convert.ToDecimal(txt_WaterBinder.Text), Convert.ToDecimal(txt_Watertobeadded.Text), Convert.ToDecimal(txt_CrushedSand.Text), Convert.ToDecimal(txt_NaturalSand.Text), Convert.ToDecimal(txt_StoneDust.Text), Convert.ToDecimal(txt_Grit.Text),
                            Convert.ToDecimal(txt_10mm.Text), Convert.ToDecimal(txt_20mm.Text), Convert.ToDecimal(txt_40mm.Text), Convert.ToDecimal(txt_MicroSilica.Text), Convert.ToDecimal(txt_GGBS.Text), Convert.ToDecimal(txt_Admixture.Text), Convert.ToDecimal(txt_Metakaolin.Text), false);
                    }

                    if (lblStatus.Text == "Check")
                    {
                        string strOtherInfo = txtCementitious.Text + "|" + txtCompaction.Text + "|" + txtAfter.Text + "|" + txtSlumpnotExceed.Text + "|" + txtSlumpnotExcdAfter.Text + "|" + txtGradeofConcrete.Text + "|" + txtStdDev.Text;
                        dc.Trail_Update_OtherInfo(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, strOtherInfo, chk_Flow.Checked);

                        // dc.UpdateTrial_MaterialNames(txt_RefNo.Text, txt_GradeofConcrete.Text, txt_CementUsed.Text, txt_FlyashUsed.Text, "", txt_Natureofwork.Text, txt_Slump.Text, txt_Admixture.Text, Convert.ToDecimal(txt_NoofCubes.Text), false, "", "", "", "", false, "", false, txt_witnessBy.Text, "", txtTotCementitiousMat.Text, txtFlyAshReplacement.Text, 0, false, false);
                        dc.UpdateTrial_atMDLCheck(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, txtCementUsed.Text, txtFlyashUsed.Text, txtNatureofWork.Text, txtAdmixureUsed.Text);
                    }
                    #region SaveRemark
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

        string FormatNumber<T>(T number, int maxDecimals = 4)
        {
            string x = Regex.Replace(String.Format("{0:n" + maxDecimals + "}", number), @"[" + System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");
            if (x == "")
                x = "0";
            return x;
        }

        protected Boolean ValidateCoverSht()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (lblStatus.Text == "Check" && lblReportType.Text == "MDL")
            {
                if (txt_KindAttention.Text == "")
                {
                    lblMsg.Text = "Please Enter Kind Attention";
                    MainView.ActiveViewIndex = 2;
                    Tab_MaterialDtl.CssClass = "Initiative";
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
                        TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");
                        if (chkBox.Checked)
                        {
                            if (txt_AvgCompStr.Text == "")
                            {
                                lblMsg.Text = "Average Compressive Strength is not available for Selected days ";
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

            //if (lbl_TestedBy.Text == "Entered By" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            //{
            //    lblMsg.Text = "Please Select the Entered By";
            //    valid = false;
            //}
            //else if (lbl_TestedBy.Text == "Approve By" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            //{
            //    lblMsg.Text = "Please Select the Approve By";
            //    valid = false;
            //}

            if (txt_RefNo.Text == "")
            {
                lblMsg.Text = "Select report number ";
                txt_RefNo.Focus();
                valid = false;
            }
            if (valid == true)
            {
                for (int i = 0; i < grdMDLetter.Rows.Count; i++)
                {

                    TextBox txt_Cement = (TextBox)grdMDLetter.Rows[i].Cells[1].FindControl("txt_Cement");
                    TextBox txt_FlyAsh = (TextBox)grdMDLetter.Rows[i].Cells[2].FindControl("txt_FlyAsh");
                    TextBox txt_GGBS = (TextBox)grdMDLetter.Rows[i].Cells[13].FindControl("txt_GGBS");
                    TextBox txt_MicroSilica = (TextBox)grdMDLetter.Rows[i].Cells[12].FindControl("txt_MicroSilica");
                    TextBox txt_Metakaolin = (TextBox)grdMDLetter.Rows[i].Cells[12].FindControl("txt_Metakaolin");
                    TextBox txt_WaterBinder = (TextBox)grdMDLetter.Rows[i].Cells[3].FindControl("txt_WaterBinder");
                    TextBox txt_Watertobeadded = (TextBox)grdMDLetter.Rows[i].Cells[4].FindControl("txt_Watertobeadded");
                    TextBox txt_NaturalSand = (TextBox)grdMDLetter.Rows[i].Cells[6].FindControl("txt_NaturalSand");
                    TextBox txt_CrushedSand = (TextBox)grdMDLetter.Rows[i].Cells[5].FindControl("txt_CrushedSand");
                    TextBox txt_StoneDust = (TextBox)grdMDLetter.Rows[i].Cells[7].FindControl("txt_StoneDust");
                    TextBox txt_Grit = (TextBox)grdMDLetter.Rows[i].Cells[8].FindControl("txt_Grit");
                    TextBox txt_10mm = (TextBox)grdMDLetter.Rows[i].Cells[9].FindControl("txt_10mm");
                    TextBox txt_20mm = (TextBox)grdMDLetter.Rows[i].Cells[10].FindControl("txt_20mm");
                    TextBox txt_40mm = (TextBox)grdMDLetter.Rows[i].Cells[11].FindControl("txt_40mm");
                    TextBox txt_Admixture = (TextBox)grdMDLetter.Rows[i].Cells[14].FindControl("txt_Admixture");

                    if (txt_Cement.Text == "")
                    {
                        lblMsg.Text = "Enter Cement for Sr No. " + (i + 1) + ".";
                        txt_Cement.Focus();
                        valid = false;
                        break;
                    }
                    if (Convert.ToDecimal(txt_Cement.Text) <= 0)
                    {
                        lblMsg.Text = "Cement should be greater than 0 for Sr No. " + (i + 1) + ".";
                        txt_Cement.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_FlyAsh.Text == "" && grdMDLetter.Columns[2].Visible == true)
                    {
                        lblMsg.Text = "Enter Fly Ash for Sr No. " + (i + 1) + ".";
                        txt_FlyAsh.Focus();
                        valid = false;
                        break;
                    }
                    if (grdMDLetter.Columns[2].Visible == true && txt_FlyAsh.Text != "")
                    {
                        if (Convert.ToDecimal(txt_FlyAsh.Text) <= 0)
                        {
                            lblMsg.Text = "Fly Ash should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_FlyAsh.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_GGBS.Text == "" && grdMDLetter.Columns[3].Visible == true)
                    {
                        lblMsg.Text = "Enter GGBS for Sr No. " + (i + 1) + ".";
                        txt_GGBS.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_GGBS.Text != "" && grdMDLetter.Columns[3].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_GGBS.Text) <= 0)
                        {
                            lblMsg.Text = "G G B S should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_GGBS.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_MicroSilica.Text == "" && grdMDLetter.Columns[4].Visible == true)
                    {
                        lblMsg.Text = "Enter Micro Silica for Sr No. " + (i + 1) + ".";
                        txt_MicroSilica.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_MicroSilica.Text != "" && grdMDLetter.Columns[4].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_MicroSilica.Text) <= 0)
                        {
                            lblMsg.Text = "Micro Silica should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_MicroSilica.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_Metakaolin.Text == "" && grdMDLetter.Columns[5].Visible == true)
                    {
                        lblMsg.Text = "Enter Metakaolin for Sr No. " + (i + 1) + ".";
                        txt_Metakaolin.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Metakaolin.Text != "" && grdMDLetter.Columns[5].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_Metakaolin.Text) <= 0)
                        {
                            lblMsg.Text = "Metakaolin should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_Metakaolin.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_WaterBinder.Text == "")
                    {
                        lblMsg.Text = "Enter Water binder for Sr No. " + (i + 1) + ".";
                        txt_WaterBinder.Focus();
                        valid = false;
                        break;
                    }
                    if (Convert.ToDecimal(txt_WaterBinder.Text) <= 0)
                    {
                        lblMsg.Text = "Water binder should be greater than 0 for Sr No. " + (i + 1) + ".";
                        txt_WaterBinder.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Watertobeadded.Text == "")
                    {
                        lblMsg.Text = "Enter Water to be added for Sr No. " + (i + 1) + ".";
                        txt_Watertobeadded.Focus();
                        valid = false;
                        break;
                    }
                    if (Convert.ToDecimal(txt_Watertobeadded.Text) <= 0)
                    {
                        lblMsg.Text = "Water to be added should be greater than 0 for Sr No. " + (i + 1) + ".";
                        txt_Watertobeadded.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_NaturalSand.Text == "" && grdMDLetter.Columns[8].Visible == true)
                    {
                        lblMsg.Text = "Enter Natural Sand for Sr No. " + (i + 1) + ".";
                        txt_NaturalSand.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_NaturalSand.Text != "" && grdMDLetter.Columns[8].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_NaturalSand.Text) <= 0)
                        {
                            lblMsg.Text = "Natural Sand should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_NaturalSand.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_CrushedSand.Text == "" && grdMDLetter.Columns[9].Visible == true)
                    {
                        lblMsg.Text = "Enter Crushed Sand for Sr No. " + (i + 1) + ".";
                        txt_CrushedSand.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_CrushedSand.Text != "" && grdMDLetter.Columns[9].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_CrushedSand.Text) <= 0)
                        {
                            lblMsg.Text = "Crushed Sand should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_CrushedSand.Focus();
                            valid = false;
                            break;
                        }
                    }

                    if (txt_StoneDust.Text == "" && grdMDLetter.Columns[10].Visible == true)
                    {
                        lblMsg.Text = "Enter Stone Dust for Sr No. " + (i + 1) + ".";
                        txt_StoneDust.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_StoneDust.Text != "" && grdMDLetter.Columns[10].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_StoneDust.Text) <= 0)
                        {
                            lblMsg.Text = "Stone Dust should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_StoneDust.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_Grit.Text == "" && grdMDLetter.Columns[11].Visible == true)
                    {
                        lblMsg.Text = "Enter Grit for Sr No. " + (i + 1) + ".";
                        txt_Grit.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Grit.Text != "" && grdMDLetter.Columns[11].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_Grit.Text) <= 0)
                        {
                            lblMsg.Text = "Grit should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_Grit.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_10mm.Text == "" && grdMDLetter.Columns[12].Visible == true)
                    {
                        lblMsg.Text = "Enter 10 mm for Sr No. " + (i + 1) + ".";
                        txt_10mm.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_10mm.Text != "" && grdMDLetter.Columns[12].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_10mm.Text) <= 0)
                        {
                            lblMsg.Text = "10 mm should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_10mm.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_20mm.Text == "" && grdMDLetter.Columns[13].Visible == true)
                    {
                        lblMsg.Text = "Enter 20 mm for Sr No. " + (i + 1) + ".";
                        txt_20mm.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_20mm.Text != "" && grdMDLetter.Columns[13].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_20mm.Text) <= 0)
                        {
                            lblMsg.Text = "20 mm should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_20mm.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_40mm.Text == "" && grdMDLetter.Columns[14].Visible == true)
                    {
                        lblMsg.Text = "Enter 40 mm for Sr No. " + (i + 1) + ".";
                        txt_40mm.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_40mm.Text != "" && grdMDLetter.Columns[14].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_40mm.Text) <= 0)
                        {
                            lblMsg.Text = "40 mm should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_40mm.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_Admixture.Text == "" && grdMDLetter.Columns[15].Visible == true)
                    {
                        lblMsg.Text = "Enter Admixture for Sr No. " + (i + 1) + ".";
                        txt_Admixture.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Admixture.Text != "" && grdMDLetter.Columns[15].Visible == true)
                    {
                        if (Convert.ToDecimal(txt_Admixture.Text) <= 0)
                        {
                            lblMsg.Text = "Admixture should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_Admixture.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (valid == true)
            {
                if (txtCementitious.Text == "")
                {
                    lblMsg.Text = "Enter Total Cementitious Material ";
                    txtCementitious.Focus();
                    valid = false;
                }
                else if (txtCompaction.Text == "")
                {
                    lblMsg.Text = "Enter Compaction Factor ";
                    txtCompaction.Focus();
                    valid = false;
                }
                //else if (txtAfter.Text == "")
                //{
                //    lblMsg.Text = "Enter After ";
                //    txtAfter.Focus();
                //    valid = false;
                //}
                else if (txtSlumpnotExceed.Text == "")
                {
                    lblMsg.Text = "Enter Slump Not to Exceed ";
                    txtSlumpnotExceed.Focus();
                    valid = false;
                }
                else if (txtCementUsed.Text == "")
                {
                    lblMsg.Text = "Enter Cement Used ";
                    txtCementUsed.Focus();
                    valid = false;
                }
                //else if (txtSlumpnotExcdAfter.Text == "")
                //{
                //    lblMsg.Text = "Enter Slump Not to Exceed ";
                //    txtSlumpnotExcdAfter.Focus();
                //    valid = false;
                //}
                else if (txtGradeofConcrete.Text == "")
                {
                    lblMsg.Text = "Enter Grade of Concrete ";
                    txtGradeofConcrete.Focus();
                    valid = false;
                }
                else if (txtStdDev.Text == "")
                {
                    lblMsg.Text = "Enter Std. Dev. Assumed ";
                    txtStdDev.Focus();
                    valid = false;
                }
                else if (txtAdmixureUsed.Text == "")
                {
                    lblMsg.Text = "Enter Admixture Used ";
                    txtAdmixureUsed.Focus();
                    valid = false;
                }
                else if (txtSpecReqment.Text == "")
                {
                    lblMsg.Text = "Enter Special Requirement ";
                    txtSpecReqment.Focus();
                    valid = false;
                }
                else if (txtNatureofWork.Text == "")
                {
                    lblMsg.Text = "Enter Nature of Work ";
                    txtNatureofWork.Focus();
                    valid = false;
                }
                else if (txtFlyashUsed.Text == "")
                {
                    lblMsg.Text = "Enter Fly Ash Used ";
                    txtFlyashUsed.Focus();
                    valid = false;
                }
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
                else if (ddlTestdApprdBy.SelectedIndex <= 0)
                {
                    //dispalyMsg = "Please Select Tested By/Approved By Name.";
                    lblMsg.Text = "Please Select " + lblTestdApprdBy.Text + " Name.";
                    ddlTestdApprdBy.Focus();
                    valid = false;
                }
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
        protected void AddRowTrialInfo()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["TrialInfoTable"] != null)
            {
                GetCurrentDataTrialInfo();
                dt = (DataTable)ViewState["TrialInfoTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TrialValue", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_SpecGravity", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblMaterialName"] = string.Empty;
            dr["txt_TrialValue"] = string.Empty;
            dr["txt_SpecGravity"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["TrialInfoTable"] = dt;
            grdTrialInfo.DataSource = dt;
            grdTrialInfo.DataBind();
            SetPreviousDataTrialInfo();
        }
        protected void GetCurrentDataTrialInfo()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TrialValue", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_SpecGravity", typeof(string)));

            for (int i = 0; i < grdTrialInfo.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdTrialInfo.Rows[i].Cells[0].FindControl("lblMaterialName");
                TextBox txt_TrialValue = (TextBox)grdTrialInfo.Rows[i].Cells[1].FindControl("txt_TrialValue");
                TextBox txt_SpecGravity = (TextBox)grdTrialInfo.Rows[i].Cells[2].FindControl("txt_SpecGravity");

                drRow = dtTable.NewRow();

                drRow["lblMaterialName"] = lblMaterialName.Text;
                drRow["txt_TrialValue"] = txt_TrialValue.Text;
                drRow["txt_SpecGravity"] = txt_SpecGravity.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["TrialInfoTable"] = dtTable;

        }
        protected void SetPreviousDataTrialInfo()
        {
            DataTable dt = (DataTable)ViewState["TrialInfoTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdTrialInfo.Rows[i].Cells[0].FindControl("lblMaterialName");
                TextBox txt_TrialValue = (TextBox)grdTrialInfo.Rows[i].Cells[1].FindControl("txt_TrialValue");
                TextBox txt_SpecGravity = (TextBox)grdTrialInfo.Rows[i].Cells[2].FindControl("txt_SpecGravity");

                lblMaterialName.Text = dt.Rows[i]["lblMaterialName"].ToString();
                txt_TrialValue.Text = dt.Rows[i]["txt_TrialValue"].ToString();
                txt_SpecGravity.Text = dt.Rows[i]["txt_SpecGravity"].ToString();

            }
        }

        protected void AddRowMDLetter()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MDLetterTable"] != null)
            {
                GetCurrentDataMDLetter();
                dt = (DataTable)ViewState["MDLetterTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lbl_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Cement", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FlyAsh", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_GGBS", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_MicroSilica", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Metakaolin", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WaterBinder", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Watertobeadded", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NaturalSand", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CrushedSand", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_StoneDust", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Grit", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_10mm", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_20mm", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_40mm", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Admixture", typeof(string)));
            }
            dr = dt.NewRow();

            dr["lbl_Name"] = string.Empty;
            dr["txt_Cement"] = string.Empty;
            dr["txt_FlyAsh"] = string.Empty;
            dr["txt_GGBS"] = string.Empty;
            dr["txt_MicroSilica"] = string.Empty;
            dr["txt_Metakaolin"] = string.Empty;
            dr["txt_WaterBinder"] = string.Empty;
            dr["txt_Watertobeadded"] = string.Empty;
            dr["txt_NaturalSand"] = string.Empty;
            dr["txt_CrushedSand"] = string.Empty;
            dr["txt_StoneDust"] = string.Empty;
            dr["txt_Grit"] = string.Empty;
            dr["txt_10mm"] = string.Empty;
            dr["txt_20mm"] = string.Empty;
            dr["txt_40mm"] = string.Empty;
            dr["txt_Admixture"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["MDLetterTable"] = dt;
            grdMDLetter.DataSource = dt;
            grdMDLetter.DataBind();
            SetPreviousDataMDLetter();
        }
        protected void GetCurrentDataMDLetter()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lbl_Name", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Cement", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FlyAsh", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_GGBS", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_MicroSilica", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Metakaolin", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WaterBinder", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Watertobeadded", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NaturalSand", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CrushedSand", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_StoneDust", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Grit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_10mm", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_20mm", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_40mm", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Admixture", typeof(string)));


            for (int i = 0; i < grdMDLetter.Rows.Count; i++)
            {
                Label lbl_Name = (Label)grdMDLetter.Rows[i].Cells[0].FindControl("lbl_Name");
                TextBox txt_Cement = (TextBox)grdMDLetter.Rows[i].Cells[1].FindControl("txt_Cement");
                TextBox txt_FlyAsh = (TextBox)grdMDLetter.Rows[i].Cells[2].FindControl("txt_FlyAsh");
                TextBox txt_GGBS = (TextBox)grdMDLetter.Rows[i].Cells[3].FindControl("txt_GGBS");
                TextBox txt_MicroSilica = (TextBox)grdMDLetter.Rows[i].Cells[4].FindControl("txt_MicroSilica");
                TextBox txt_Metakaolin = (TextBox)grdMDLetter.Rows[i].Cells[5].FindControl("txt_Metakaolin");
                TextBox txt_WaterBinder = (TextBox)grdMDLetter.Rows[i].Cells[6].FindControl("txt_WaterBinder");
                TextBox txt_Watertobeadded = (TextBox)grdMDLetter.Rows[i].Cells[7].FindControl("txt_Watertobeadded");
                TextBox txt_NaturalSand = (TextBox)grdMDLetter.Rows[i].Cells[8].FindControl("txt_NaturalSand");
                TextBox txt_CrushedSand = (TextBox)grdMDLetter.Rows[i].Cells[9].FindControl("txt_CrushedSand");
                TextBox txt_StoneDust = (TextBox)grdMDLetter.Rows[i].Cells[10].FindControl("txt_StoneDust");
                TextBox txt_Grit = (TextBox)grdMDLetter.Rows[i].Cells[11].FindControl("txt_Grit");
                TextBox txt_10mm = (TextBox)grdMDLetter.Rows[i].Cells[12].FindControl("txt_10mm");
                TextBox txt_20mm = (TextBox)grdMDLetter.Rows[i].Cells[13].FindControl("txt_20mm");
                TextBox txt_40mm = (TextBox)grdMDLetter.Rows[i].Cells[14].FindControl("txt_40mm");
                TextBox txt_Admixture = (TextBox)grdMDLetter.Rows[i].Cells[15].FindControl("txt_Admixture");

                drRow = dtTable.NewRow();

                drRow["lbl_Name"] = lbl_Name.Text;
                drRow["txt_Cement"] = txt_Cement.Text;
                drRow["txt_FlyAsh"] = txt_FlyAsh.Text;
                drRow["txt_GGBS"] = txt_GGBS.Text;
                drRow["txt_MicroSilica"] = txt_MicroSilica.Text;
                drRow["txt_Metakaolin"] = txt_Metakaolin.Text;
                drRow["txt_WaterBinder"] = txt_WaterBinder.Text;
                drRow["txt_Watertobeadded"] = txt_Watertobeadded.Text;
                drRow["txt_NaturalSand"] = txt_NaturalSand.Text;
                drRow["txt_CrushedSand"] = txt_CrushedSand.Text;
                drRow["txt_StoneDust"] = txt_StoneDust.Text;
                drRow["txt_Grit"] = txt_Grit.Text;
                drRow["txt_10mm"] = txt_10mm.Text;
                drRow["txt_20mm"] = txt_20mm.Text;
                drRow["txt_40mm"] = txt_40mm.Text;
                drRow["txt_Admixture"] = txt_Admixture.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MDLetterTable"] = dtTable;

        }
        protected void SetPreviousDataMDLetter()
        {
            DataTable dt = (DataTable)ViewState["MDLetterTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lbl_Name = (Label)grdMDLetter.Rows[i].Cells[0].FindControl("lbl_Name");
                TextBox txt_Cement = (TextBox)grdMDLetter.Rows[i].Cells[1].FindControl("txt_Cement");
                TextBox txt_FlyAsh = (TextBox)grdMDLetter.Rows[i].Cells[2].FindControl("txt_FlyAsh");
                TextBox txt_GGBS = (TextBox)grdMDLetter.Rows[i].Cells[3].FindControl("txt_GGBS");
                TextBox txt_MicroSilica = (TextBox)grdMDLetter.Rows[i].Cells[4].FindControl("txt_MicroSilica");
                TextBox txt_Metakaolin = (TextBox)grdMDLetter.Rows[i].Cells[5].FindControl("txt_Metakaolin");
                TextBox txt_WaterBinder = (TextBox)grdMDLetter.Rows[i].Cells[6].FindControl("txt_WaterBinder");
                TextBox txt_Watertobeadded = (TextBox)grdMDLetter.Rows[i].Cells[7].FindControl("txt_Watertobeadded");
                TextBox txt_NaturalSand = (TextBox)grdMDLetter.Rows[i].Cells[8].FindControl("txt_NaturalSand");
                TextBox txt_CrushedSand = (TextBox)grdMDLetter.Rows[i].Cells[9].FindControl("txt_CrushedSand");
                TextBox txt_StoneDust = (TextBox)grdMDLetter.Rows[i].Cells[10].FindControl("txt_StoneDust");
                TextBox txt_Grit = (TextBox)grdMDLetter.Rows[i].Cells[11].FindControl("txt_Grit");
                TextBox txt_10mm = (TextBox)grdMDLetter.Rows[i].Cells[12].FindControl("txt_10mm");
                TextBox txt_20mm = (TextBox)grdMDLetter.Rows[i].Cells[13].FindControl("txt_20mm");
                TextBox txt_40mm = (TextBox)grdMDLetter.Rows[i].Cells[14].FindControl("txt_40mm");
                TextBox txt_Admixture = (TextBox)grdMDLetter.Rows[i].Cells[15].FindControl("txt_Admixture");

                lbl_Name.Text = dt.Rows[i]["lbl_Name"].ToString();
                txt_Cement.Text = dt.Rows[i]["txt_Cement"].ToString();
                txt_FlyAsh.Text = dt.Rows[i]["txt_FlyAsh"].ToString();
                txt_GGBS.Text = dt.Rows[i]["txt_GGBS"].ToString();
                txt_MicroSilica.Text = dt.Rows[i]["txt_MicroSilica"].ToString();
                txt_Metakaolin.Text = dt.Rows[i]["txt_Metakaolin"].ToString();
                txt_WaterBinder.Text = dt.Rows[i]["txt_WaterBinder"].ToString();
                txt_Watertobeadded.Text = dt.Rows[i]["txt_Watertobeadded"].ToString();
                txt_NaturalSand.Text = dt.Rows[i]["txt_NaturalSand"].ToString();
                txt_CrushedSand.Text = dt.Rows[i]["txt_CrushedSand"].ToString();
                txt_StoneDust.Text = dt.Rows[i]["txt_StoneDust"].ToString();
                txt_Grit.Text = dt.Rows[i]["txt_Grit"].ToString();
                txt_10mm.Text = dt.Rows[i]["txt_10mm"].ToString();
                txt_20mm.Text = dt.Rows[i]["txt_20mm"].ToString();
                txt_40mm.Text = dt.Rows[i]["txt_40mm"].ToString();
                txt_Admixture.Text = dt.Rows[i]["txt_Admixture"].ToString();

                //if (lblStatus.Text != "Check")
                //{
                    txt_Cement.ReadOnly = true;
                    txt_FlyAsh.ReadOnly = true;
                    txt_GGBS.ReadOnly = true;
                    txt_MicroSilica.ReadOnly = true;
                    txt_Metakaolin.ReadOnly = true;
                    txt_WaterBinder.ReadOnly = true;
                    txt_Watertobeadded.ReadOnly = true;
                    txt_NaturalSand.ReadOnly = true;
                    txt_CrushedSand.ReadOnly = true;
                    txt_StoneDust.ReadOnly = true;
                    txt_Grit.ReadOnly = true;
                    txt_10mm.ReadOnly = true;
                    txt_20mm.ReadOnly = true;
                    txt_40mm.ReadOnly = true;
                    txt_Admixture.ReadOnly = true;
                //}
            }
        }
        protected void Tab_MaterialDtl_Click(object sender, EventArgs e)
        {
            Tab_MaterialDtl.CssClass = "Click";
            Tab_OtherInfo.CssClass = "Initiative";
            Tab_CubeCompStr.CssClass = "Initiative";
            MainView.ActiveViewIndex = 0;
        }
        protected void Tab_OtherInfo_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 1;
            Tab_MaterialDtl.CssClass = "Initiative";
            Tab_CubeCompStr.CssClass = "Initiative";
            Tab_OtherInfo.CssClass = "Click";
        }
        protected void Tab_CubeCompStr_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 2;
            if (lblStatus.Text == "Check")
            {
                txt_KindAttention.Focus();
            }
            Tab_MaterialDtl.CssClass = "Initiative";
            Tab_OtherInfo.CssClass = "Initiative";
            Tab_CubeCompStr.CssClass = "Click";

        }
        //chkCoverSheet.Attributes.Add("onclick", "return false;");
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
            }
            dr = dt.NewRow();
            dr["txt_Days"] = string.Empty;
            dr["txt_Cubes"] = string.Empty;
            dr["txt_AvgCompStr"] = string.Empty;

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

            for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[1].FindControl("txt_Days");
                TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[2].FindControl("txt_Cubes");
                TextBox txt_AvgCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_AvgCompStr");

                drRow = dtTable.NewRow();
                drRow["txt_Days"] = txt_Days.Text;
                drRow["txt_Cubes"] = txt_Cubes.Text;
                drRow["txt_AvgCompStr"] = txt_AvgCompStr.Text;

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

                txt_Days.Text = dt.Rows[i]["txt_Days"].ToString();
                txt_Cubes.Text = dt.Rows[i]["txt_Cubes"].ToString();
                txt_AvgCompStr.Text = dt.Rows[i]["txt_AvgCompStr"].ToString();
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
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            //PrintHTMLReport rptHtml = new PrintHTMLReport();
            //rptHtml.TrialMDLetter_Html(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text), txt_RecType.Text, lblReportType.Text, lblStatus.Text);
            PrintPDFReport rpt = new PrintPDFReport();
            //prnRpt.MF_MDLetter_PDFReport(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text), txt_RecType.Text, lblReportType.Text, lblStatus.Text);
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, lblStatus.Text, "", "", lblReportType.Text, "", "", "", "");
        }
        protected void lnkSievePrnt_Click(object sender, EventArgs e)
        {
            RptMFSieveAnalysis();
        }


        public void RptMFSieveAnalysis()
        {

            //PrintHTMLReport rptHtml = new PrintHTMLReport();
            //rptHtml.TrialSieveAnalysis_Html(txt_RefNo.Text, txt_RecType.Text);

            PrintPDFReport rpt = new PrintPDFReport();
            //rptPdf.MFSieveAnalysis_PDF(txt_RefNo.Text, txt_RecType.Text, "Print");
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, "Print", "", "", "Sieve Analysis", "", "", "", "");
        }
        //public string RptTrialSieveAnalysis(string ReferenceNo, string RecType)
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    int RecordNo = 0;

        //    var AggtTest = dc.ReportStatus_View("Aggregate Testing", null, null, 0, 0, 0, txt_RefNo.Text, 0, 2, 0);
        //    foreach (var aggt in AggtTest)
        //    {

        //        mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Test Report</b></font></td></tr>";
        //        mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Fine & Coarse Aggregate </b></font></td></tr>";
        //        mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //        mySql += "<tr>" +
        //            "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='40%' height=19><font size=2>" + aggt.CL_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + System.DateTime.Now.ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + aggt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + RecType + "-" + aggt.AGGTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //            "</tr>";

        //        RecordNo = Convert.ToInt32(aggt.AGGTINWD_RecordNo_int);
        //        break;
        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr><td  width=20% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td></tr>";

        //    bool SpecGrav = false; bool lbd = false; bool Moist = false; bool Sild = false; bool Impact = false; bool Elong = false; bool Crush = false; bool Flaki = false;
        //    var MFTestname = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, RecType);
        //    foreach (var aggtt in MFTestname)
        //    {
        //        if (RecType == "MF")
        //        {
        //            if (aggtt.AGGTINWD_ImpactValue_var != null && aggtt.AGGTINWD_ImpactValue_var != "")
        //            {
        //                Impact = true;
        //            }
        //            if (aggtt.AGGTINWD_Flakiness_var != null && aggtt.AGGTINWD_Flakiness_var != "")
        //            {
        //                Flaki = true;
        //            }
        //            if (aggtt.AGGTINWD_Flakiness_var != null && aggtt.AGGTINWD_Flakiness_var != "")
        //            {
        //                Flaki = true;
        //            }
        //            if (aggtt.AGGTINWD_LBD_var != null && aggtt.AGGTINWD_LBD_var != "")
        //            {
        //                lbd = true;
        //            }
        //            if (aggtt.AGGTINWD_MoistureContent_var != null && aggtt.AGGTINWD_MoistureContent_var != "")
        //            {
        //                Moist = true;
        //            }
        //            if (aggtt.AGGTINWD_SpecificGravity_var != null && aggtt.AGGTINWD_SpecificGravity_var != "")
        //            {
        //                SpecGrav = true;
        //            }
        //            if (aggtt.AGGTINWD_WaterAborp_var != null && aggtt.AGGTINWD_WaterAborp_var != "")
        //            {
        //                SpecGrav = true;
        //            }
        //            if (aggtt.AGGTINWD_SildContent_var != null && aggtt.AGGTINWD_SildContent_var != "")
        //            {
        //                Sild = true;
        //            }
        //            if (aggtt.AGGTINWD_Elongation_var != null && aggtt.AGGTINWD_Elongation_var != "")
        //            {
        //                Elong = true;
        //            }
        //            if (aggtt.AGGTINWD_CrushingValue_var != null && aggtt.AGGTINWD_CrushingValue_var != "")
        //            {
        //                Crush = true;
        //            }
        //        }
        //    }

        //    int SrNo = 0;
        //    var Mix = dc.MaterialDetail_View(RecordNo, "", 0, "", null, null);
        //    foreach (var m in Mix)
        //    {
        //        if (SrNo != Convert.ToInt32(m.MaterialDetail_SrNo) || SrNo == 0)
        //        {
        //            var MfInwd = dc.MF_View(ReferenceNo, Convert.ToInt32(m.Material_Id), RecType);
        //            foreach (var aggt in MfInwd)
        //            {
        //                if (aggt.AGGTINWD_AggregateName_var == "Natural Sand" || aggt.AGGTINWD_AggregateName_var == "Crushed Sand" || aggt.AGGTINWD_AggregateName_var == "Stone Dust" || aggt.AGGTINWD_AggregateName_var == "Grit")
        //                {
        //                    mySql += "<table>";
        //                    mySql += "<tr><td width= 3% align=left valign=top height=19 ><font size=2><b>" + "Fine Aggregate" + " (" + Convert.ToString(m.Material_List) + ")" + "</b></font></td></tr>";

        //                    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //                    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //                    mySql += "<td width= 2% align=left  valign=top height=19 rowspan=5  ><font size=2> " + " " + " </font></td>";
        //                    mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //                    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //                    mySql += "</tr>";

        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
        //                    if (SpecGrav == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Moisture Content" + " </font></td>";
        //                    if (Moist == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_MoistureContent_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " % " + "</font></td>";
        //                    mySql += "</tr>";

        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
        //                    if (SpecGrav == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
        //                    mySql += "</tr>";

        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
        //                    if (lbd == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Material finer than 75 u </br> (by wet sieving)" + "</font></td>";
        //                    if (Sild == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SildContent_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " %  " + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                if (aggt.AGGTINWD_AggregateName_var == "10 mm" || aggt.AGGTINWD_AggregateName_var == "20 mm" || aggt.AGGTINWD_AggregateName_var == "40 mm" || aggt.AGGTINWD_AggregateName_var == "Mix Aggt")
        //                {
        //                    mySql += "<table>";
        //                    if (Convert.ToString(aggt.AGGTINWD_AggregateName_var) != "Mix Aggt")
        //                    {

        //                        mySql += "<tr><td width= 3% align=left valign=top height=19 ><font size=2><b>" + "Coarse Aggregate" + " (" + Convert.ToString(m.Material_List) + ")" + "</b></font></td></tr>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<tr><td width= 3% align=left valign=top height=19 ><font size=2><b>" + "Fine /Coarse Aggregate" + " (" + Convert.ToString(m.Material_List) + ")" + "</b></font></td></tr>";
        //                    }
        //                    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //                    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //                    mySql += "<td width= 2% align=left  valign=top height=19 rowspan=5  ><font size=2> " + " " + " </font></td>";
        //                    mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //                    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //                    mySql += "</tr>";

        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
        //                    if (SpecGrav == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Impact Value" + " </font></td>";
        //                    if (Impact == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_ImpactValue_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
        //                    mySql += "</tr>";

        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
        //                    if (SpecGrav == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
        //                    mySql += "</tr>";

        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
        //                    if (lbd == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Flakiness Value" + "</font></td>";
        //                    if (Flaki == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Flakiness_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " " + "</font></td>";
        //                    mySql += "</tr>";

        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Elongness Value" + "</font></td>";
        //                    if (Elong == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Elongation_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Crushing Value" + "</font></td>";
        //                    if (Crush == true)
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_CrushingValue_var) + "</font></td>";
        //                    }
        //                    else
        //                    {
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //                    }
        //                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
        //                    mySql += "</tr>";
        //                }

        //                int i = 0;
        //                var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(m.Material_Id), "AGGTSA");
        //                foreach (var aggtt in aggtTest)
        //                {
        //                    if (i == 0)
        //                    {
        //                        mySql += "<table>";
        //                        mySql += "<tr>";
        //                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "Sieve Analysis (by dry sieving) " + "</b></font></td>";
        //                        mySql += "</tr>";

        //                        mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //                        mySql += "<tr>";
        //                        mySql += "<td width= 2%  rowspan=2 align=center valign=middle height=19 ><font size=2><b>Sieve Size</b></font></td>";
        //                        mySql += "<td width= 10%  align=center colspan=3 valign=top height=19 ><font size=2><b>Weight retained</b></font></td>";
        //                        mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2><b>Passing </b></font></td>";
        //                        if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                        {
        //                            mySql += "<td width= 2%   align=center rowspan=2 valign=middle height=19 ><font size=2><b>IS Passing % Limits </b></font></td>";
        //                        }
        //                        mySql += "</tr>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(g)</b></font></td>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>Cummu (%)</b></font></td>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
        //                    }
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_SeiveSize_var.ToString() + "</font></td>";
        //                    mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_Weight_num.ToString() + "</font></td>";
        //                    if (aggtt.AGGTSA_SeiveSize_var != "Total")
        //                    {
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + Convert.ToDecimal(aggtt.AGGTSA_WeightRet_dec).ToString("0.00") + "</font></td>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuWeightRet_dec.ToString() + "</font></td>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuPassing_dec.ToString() + "</font></td>";
        //                        if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                        {
        //                            mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + aggtt.AGGTSA_IsPassingLmt_var.ToString() + " </font></td>";
        //                        }
        //                    }
        //                    else if (aggtt.AGGTSA_SeiveSize_var == "Total")
        //                    {
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "" + "</font></td>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "Fineness Modulus" + "</font></td>";
        //                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggtt.AGGTINWD_FM_var) + "</font></td>";
        //                        if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                        {
        //                            mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + "" + " </font></td>";
        //                        }
        //                    }
        //                    i++;

        //                    mySql += "</tr>";

        //                }
        //                //mySql += "<table>";
        //                //mySql += "<tr><td width= 10% align=left valign=top height=10 ><font size=2>&nbsp;</font></td></tr>";
        //            }
        //        }
        //        SrNo = Convert.ToInt32(m.MaterialDetail_SrNo);
        //    }

        //    mySql += "<table>";
        //    mySql += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";

        //    //SrNo = 0;
        //    //var matid = dc.Material_View("AGGT");
        //    //foreach (var m in matid)
        //    //{
        //    //    var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //    //    foreach (var cd in iscd)
        //    //    {
        //    //        if (SrNo == 0)
        //    //        {
        //    //            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td></tr>";
        //    //        }
        //    //        SrNo++;
        //    //        mySql += "<tr><td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td></tr>";
        //    //    }
        //    //}

        //    //SrNo = 0;
        //    //var re = dc.AllRemark_View("", txt_RefNo.Text, 0, "AGGT");
        //    //foreach (var r in re)
        //    //{
        //    //    var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.AGGTDetail_RemarkId_int), "AGGT");
        //    //    foreach (var remk in remark)
        //    //    {
        //    //        if (SrNo == 0)
        //    //        {
        //    //            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td></tr>";
        //    //        }
        //    //        SrNo++;
        //    //        mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.AGGT_Remark_var.ToString() + "</font></td></tr>";
        //    //    }
        //    //}

        //    mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
        //    mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td></tr>";
        //    mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
        //    mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";

        //    //if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    //{
        //    //    var RecNo = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, txt_RefNo.Text, 0, 0, 0);
        //    //    foreach (var r in RecNo)
        //    //    {
        //    //        if (Convert.ToString(r.AGGTINWD_ApprovedBy_tint) != string.Empty)
        //    //        {
        //    //            var U = dc.User_View(r.AGGTINWD_ApprovedBy_tint, -1, "", "");
        //    //            foreach (var r1 in U)
        //    //            {
        //    //                mySql += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td></tr>";
        //    //                mySql += "<tr><td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td></tr>";
        //    //            }
        //    //        }
        //    //mySql += "<tr>";
        //    //if (WitnessBy != string.Empty)
        //    //{
        //    //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> Witness by : " + WitnessBy + " </font></td>";
        //    //}
        //    //if (Convert.ToString(r.AGGTINWD_CheckedBy_tint) != string.Empty)
        //    //{
        //    //    var lgin = dc.User_View(r.AGGTINWD_CheckedBy_tint, -1, "", "");
        //    //    foreach (var loginusr in lgin)
        //    //    {
        //    //        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    //        mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //    //    }
        //    //    mySql += "</tr>";
        //    //}
        //    //    }
        //    //}


        //    mySql += "<tr><td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td></tr>";
        //    mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td></tr>";
        //    mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td></tr>";
        //    mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td></tr>";

        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        protected void lnkCoverShtPrnt_Click(object sender, EventArgs e)
        {
            //dc.MDLCoverSheet_Update(txt_RefNo.Text, "", "", null,Convert.ToDateTime(System.DateTime.Now.ToString("dd/MM/yyyy")));
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.MDLCoverSheet_PDF(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text), "Print");
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, "Print", "", "", "Cover Sheet", "", "", "", "");

        }
        //public void RptMDLCoverSheet(string ReferenceNo)
        //{
        //    try
        //    {
        //        Paragraph paragraph = new Paragraph();
        //        Document pdfDoc = new Document(PageSize.A4, 55f, 45f, 100f, 0f);               
        //        var fileName = "MDL CoverSheet" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".pdf";
        //        string foldername = "Veena";
        //        if (!Directory.Exists(@"D:\" + foldername))
        //            Directory.CreateDirectory(@"D:/" + foldername);

        //        string Subfoldername = foldername + "/MDL CoverSheet";
        //        if (!Directory.Exists(@"D:\" + Subfoldername))
        //            Directory.CreateDirectory(@"D:/" + Subfoldername);

        //        string Subfoldername1 = Subfoldername + "/MDL CoverSheet";

        //        if (!Directory.Exists(@"D:\" + Subfoldername1))
        //            Directory.CreateDirectory(@"D:/" + Subfoldername1);

        //        PdfWriter.GetInstance(pdfDoc, new FileStream(@"D:/" + Subfoldername1 + "/" + fileName, FileMode.Create));
        //        pdfDoc.Open();
        //        PdfPTable table1 = new PdfPTable(7);
        //        table1.WidthPercentage = 100;
        //        paragraph = new Paragraph();
        //        //Font fontH3 = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.UNDEFINED);
        //        //Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.UNDEFINED);
        //        //Font fontH2 = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.UNDEFINED);
        //        Font fontH4 = new Font(Font.FontFamily.TIMES_ROMAN, 6, Font.UNDEFINED);
        //        Font fontTitle = new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD);
        //        Font fontTitle1 = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
        //        Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.UNDEFINED);
        //        Font fontH2 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD);
        //        Font fontH3 = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.UNDEFINED);
        //        #region data

        //        PdfPCell cell1;
        //        table1 = new PdfPTable(2);
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //       // table1.SetTotalWidth(new float[] { 45f, 15f });

        //        var MFDtls = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, ReferenceNo, 0, 0, 0);
        //        foreach (var mdl in MFDtls)
        //        {
        //            cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);
        //            if (mdl.MFINWD_MDLIssueDt != null)
        //            {
        //                cell1 = new PdfPCell(new Phrase("Date :  " + Convert.ToDateTime(mdl.MFINWD_MDLIssueDt).ToString("dd/MMM/yy"), fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                cell1.Border = PdfPCell.NO_BORDER;
        //                table1.AddCell(cell1);
        //            }
        //            else
        //            {
        //                cell1 = new PdfPCell(new Phrase("Date : - ", fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                cell1.Border = PdfPCell.NO_BORDER;
        //                table1.AddCell(cell1);
        //            }
        //            cell1 = new PdfPCell(new Phrase(mdl.CL_Name_var.ToString(), fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);
        //            cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);

        //            cell1 = new PdfPCell(new Phrase(mdl.CL_OfficeAddress_var.ToString(), fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);

        //            cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);
        //            cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);
        //            cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);

        //            cell1 = new PdfPCell(new Phrase("Kind Attention       : " + Convert.ToString(mdl.MFINWD_KindAttention), fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);

        //            cell1 = new PdfPCell(new Phrase("", fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Border = PdfPCell.NO_BORDER;
        //            table1.AddCell(cell1);
        //        }
        //        pdfDoc.Add(table1);
        //        table1 = new PdfPTable(2);
        //        table1.SpacingBefore = 5;
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //       // table1.SetTotalWidth(new float[] { 20f, 30f });

        //        var mdldtls = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, ReferenceNo, 0, 0, 0);
        //        foreach (var mdl in mdldtls)
        //        {
        //            cell1 = new PdfPCell(new Phrase("Subject                   : " + "Interim Mix Design Report for " + mdl.MFINWD_Grade_var + " grade of concrete.", fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell1.Colspan = 2;
        //            table1.AddCell(cell1);

        //            cell1 = new PdfPCell(new Phrase("Reference Code     : " + "MF- " + mdl.MFINWD_ReferenceNo_var, fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table1.AddCell(cell1);

        //            cell1 = new PdfPCell(new Phrase("Site              : " + mdl.SITE_Name_var, fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table1.AddCell(cell1);

        //            cell1 = new PdfPCell(new Phrase("Material Recd. On : " + Convert.ToDateTime(mdl.INWD_ReceivedDate_dt).ToString("dd/MMM/yy"), fontH1));
        //            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table1.AddCell(cell1);

        //            if (mdl.MFINWD_CoverSheetDetail != null)
        //            {
        //                string[] Days = Convert.ToString(mdl.MFINWD_CoverSheetDetail).Split('|');
        //                foreach (var day in Days)
        //                {
        //                    if (day != "")
        //                    {
        //                        cell1 = new PdfPCell(new Phrase("Basis of Design   : " + day + " day compressive strength", fontH1));
        //                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        table1.AddCell(cell1);
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        pdfDoc.Add(table1);
        //        table1 = new PdfPTable(1);
        //        table1.SpacingBefore = 10;
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //       // table1.SetTotalWidth(new float[] { 58f });

        //        cell1 = new PdfPCell(new Phrase("Dear Sir,   ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("With reference to mix design carried out at the laboratory , please find enclosed the interim   mix design report. This interim report is being issued for site trial and is based on cube compressive strength data obtained in lab.", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("You are requested to verify the mix on site for workability and strength. Site assistance for mix design trial will be provided on request.", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("Kindly inform whether the mix is as per your requirement in the feedback form enclosed.", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("Final mix design report will be issued after 28 day compressive strength test results are available.", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("Any change in material specification will require correction in mix proportions.", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("You are requested to make the correction with the help of Duorocrete mix design manual or get the mix design revalidated.", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("We advise, that mix design should be revalidated / optimized for every  1000 m³ of concrete or 12 months whichever is earlier.", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("  ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("Thanking you ,  ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        pdfDoc.Add(table1);

        //        #endregion
        //        table1 = new PdfPTable(2);
        //        table1.SpacingBefore = 10;
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //      //  table1.SetTotalWidth(new float[] { 30f, 20f });

        //        cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase("Yours Sincerely ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase(" ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase("Encl : 1) Interim mix design report.\n 2) Aggregate test report. ", fontH4));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase(" \n  ", fontH1));
        //        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase("CIN - U28939PN1999PTC014212", fontH3));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        cell1.Colspan = 2;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("Regd.Add - 1160/5, GHARPURE Colony Shivaji Nagar, Pune 411005,Maharashtra India", fontH2));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        cell1.Colspan = 2;
        //        table1.AddCell(cell1);
        //        pdfDoc.Add(table1);
        //        pdfDoc.Close();
        //        string pdfPath = @"D:/" + Subfoldername1 + "/" + fileName;
        //        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //        Response.ContentType = "application/pdf";
        //        Response.WriteFile(pdfPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        string s = ex.Message;
        //    }
        //}

        protected void lnkMoisuteCorrPrnt_Click(object sender, EventArgs e)
        {
            //if (Session["lnkMDLIssue"] != null)
            //{
            //      if (lblStatus.Text == "Check")
            //    {
            //        Session["RefNo"] = txt_RefNo.Text;
            //        Response.Redirect("MoistCorrection.aspx");
            //    }
            //}
            //else
            //{
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.MoistureCorrection_PDF(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text), "Print");
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, "Print", "", "", "Moisture Correction", "", "", "", "");
            //}
        }

        //public void RptMoistureCorrection()
        //{
        //    try
        //    {
        //        Paragraph paragraph = new Paragraph();
        //        Document pdfDoc = new Document(PageSize.A4, 50, 10f, 50f, 0f);
        //        var fileName = "Moisture Correction" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".pdf";
        //        string foldername = "Veena";
        //        if (!Directory.Exists(@"D:\" + foldername))
        //            Directory.CreateDirectory(@"D:/" + foldername);

        //        string Subfoldername = foldername + "/Moisture Correction";
        //        if (!Directory.Exists(@"D:\" + Subfoldername))
        //            Directory.CreateDirectory(@"D:/" + Subfoldername);

        //        string Subfoldername1 = Subfoldername + "/Moisture Correction";

        //        if (!Directory.Exists(@"D:\" + Subfoldername1))
        //            Directory.CreateDirectory(@"D:/" + Subfoldername1);

        //        PdfWriter.GetInstance(pdfDoc, new FileStream(@"D:/" + Subfoldername1 + "/" + fileName, FileMode.Create));
        //        pdfDoc.Open();
        //        PdfPTable table1 = new PdfPTable(7);
        //        paragraph = new Paragraph();
        //        Font fontH3 = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.UNDEFINED);
        //        Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD);
        //        Font fontH2 = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.UNDEFINED);
        //        Font fontH4 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.UNDERLINE);
        //        Font fontH5 = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.UNDERLINE);
        //        #region data

        //        PdfPTable MaindataTable = new PdfPTable(4);
        //        MaindataTable.HorizontalAlignment = Element.ALIGN_LEFT;
        //        MaindataTable.WidthPercentage = 80;
        //        MaindataTable.DefaultCell.Border = PdfPCell.NO_BORDER;
        //        paragraph.Alignment = Element.ALIGN_CENTER;
        //        paragraph.Font = fontH5;
        //        paragraph.Add("Correction For Moisture In Aggregate");
        //        pdfDoc.Add(paragraph);

        //        bool NSandFlag = false, CSandFlag = false;
        //        int trialId = 0;
        //        double NSWaterAbsorption = 0, CSWaterAbsorption = 0;
        //        var res = dc.MaterialDetail_View(0, txt_RefNo.Text, 0, "", null, null);
        //        foreach (var t in res)
        //        {
        //            if (Convert.ToString(t.Material_List) == "Natural Sand")
        //            {
        //                NSandFlag = true;
        //            }
        //            if (Convert.ToString(t.Material_List) == "Crushed Sand")
        //            {
        //                CSandFlag = true;
        //            }
        //        }

        //        var trial = dc.Trial_View(txt_RefNo.Text, false);
        //        foreach (var t in trial)
        //        {
        //            if (t.Trial_Status == 1)
        //            {
        //                trialId = t.Trial_Id;
        //                if (NSandFlag == true)
        //                    NSWaterAbsorption = Convert.ToDouble(t.Trial_WA_NS);
        //                if (CSandFlag == true)
        //                    CSWaterAbsorption = Convert.ToDouble(t.Trial_WA_CS);
        //            }
        //        }

        //        PdfPCell cell1;
        //        MaindataTable.SpacingBefore = 10;
        //        MaindataTable.SetTotalWidth(new float[] { 20f, 35f, 20f, 10f });
        //        var MoistCorr = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, txt_RefNo.Text, 0, 0, 0);
        //        foreach (var moist in MoistCorr)
        //        {
        //            PdfPCell Cust_Namecell = new PdfPCell(new Phrase("Site  ", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase(": " + moist.SITE_Name_var, fontH3));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase("Reference Code", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            Cust_Namecell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase(": MF- " + moist.MFINWD_ReferenceNo_var, fontH3));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase("Grade of Concrete", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase(": " + moist.MFINWD_Grade_var, fontH3));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase(" ", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase(" ", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase(" ", fontH4));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            Cust_Namecell.Colspan = 4;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase("A) Finding Total Moisture in Sand", fontH4));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            Cust_Namecell.Colspan = 4;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase("Take 500 g of sand in a  tray , heat for 5 min, check wt in gram say 'w', calculate moisture = ((500-w)/w) X 100", fontH3));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            Cust_Namecell.Colspan = 4;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            Cust_Namecell = new PdfPCell(new Phrase(" ", fontH4));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            Cust_Namecell.Colspan = 4;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            if (NSandFlag == true && CSandFlag == true)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("B) Correction Applied", fontH4));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                Cust_Namecell.Colspan = 4;
        //                MaindataTable.AddCell(Cust_Namecell);
        //            }
        //            else
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("B) Free water to be added after moisture correction", fontH4));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                Cust_Namecell.Colspan = 4;
        //                MaindataTable.AddCell(Cust_Namecell);
        //            }
        //            break;
        //        }
        //        pdfDoc.Add(MaindataTable);
        //        pdfDoc.Add(table1);

        //        double d2 = 0, d3 = 0, CemCont = 0, flyashcal = 0, fly_ash_cont = 0, Fly_Ash_By_Min_Adm = 0;
        //        double FlyAshWt = 0, WaterCementRatio = 0, WtOfFA2 = 0, WtOfFA1 = 0;
        //        var triald = dc.TrialDetail_View(txt_RefNo.Text, trialId).ToList();
        //        foreach (var t in triald)
        //        {
        //            if (Convert.ToString(t.TrialDetail_MaterialName) == "Cement")
        //            {
        //                d2 = (0.15 * 0.15 * 0.15) * Convert.ToDouble(t.Trial_NoOfCubes);
        //                d3 = (d2 * 0.2) + d2;
        //                CemCont = Convert.ToDouble(t.TrialDetail_Weight) / 50;
        //            }
        //            if (Convert.ToString(t.TrialDetail_MaterialName) == "W/C Ratio")
        //            {
        //                WaterCementRatio = Convert.ToDouble(t.TrialDetail_Weight);
        //            }
        //            if (Convert.ToString(t.TrialDetail_MaterialName) == "Fly Ash")
        //            {
        //                flyashcal = Convert.ToDouble(t.TrialDetail_Weight);
        //                fly_ash_cont = flyashcal * d3;
        //                fly_ash_cont = Convert.ToDouble(FormatNumber(fly_ash_cont, 3));
        //                if (fly_ash_cont > 0)
        //                {
        //                    Fly_Ash_By_Min_Adm = Convert.ToDouble(Convert.ToInt32(flyashcal / CemCont));
        //                }
        //                FlyAshWt = Fly_Ash_By_Min_Adm;
        //            }
        //        }
        //        var md = dc.MDLetter_View(txt_RefNo.Text, "MDL");
        //        foreach (var mdl in md)
        //        {
        //            if (mdl.MD_SrNo == 0)
        //            {
        //                WtOfFA1 = Convert.ToDouble(mdl.MD_NaturalSand);
        //                WtOfFA2 = Convert.ToDouble(mdl.MD_CrushedSand);
        //            }
        //        }
        //        int i = 0;
        //        string strDetails = "", strDetails1 = "", strDetails2 = "";
        //        if (NSandFlag == true && CSandFlag == true)
        //        {
        //            table1 = new PdfPTable(11);
        //            table1.SpacingBefore = 10;
        //            table1.WidthPercentage = 80;
        //            table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table1.SetTotalWidth(new float[] { 5f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f });
        //            if (i == 0)
        //            {
        //                cell1 = new PdfPCell(new Phrase("Total  Moisture(%)", fontH1));
        //                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                cell1.Rowspan = 3;
        //                cell1.Colspan = 2;
        //                table1.AddCell(cell1);

        //                cell1 = new PdfPCell(new Phrase("Crushed Sand (% moisture)", fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                cell1.Colspan = 9;
        //                table1.AddCell(cell1);

        //                string[] header = { "1", "2", "3 ", "4", "5", "6", "7", "8", "9" };
        //                for (int h = 0; h < header.Count(); h++)
        //                {
        //                    cell1 = new PdfPCell(new Phrase(header[h], fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //                cell1 = new PdfPCell(new Phrase("Water to be added (lit) ", fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                cell1.Colspan = 9;
        //                table1.AddCell(cell1);

        //                cell1 = new PdfPCell(new Phrase("N\n a\n t\n u\n r\n a\n l\n \n s\n a\n n\n d", fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                cell1.Rowspan = 10;
        //                table1.AddCell(cell1);
        //            }
        //            for (int row = 1; row <= 10; row++)
        //            {
        //                strDetails = strDetails + row + "~";
        //                strDetails1 = strDetails1 + row + "~";
        //                for (int j = 1; j <= 9; j++)
        //                {
        //                    strDetails = strDetails + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - (WtOfFA1 * ((row - NSWaterAbsorption) / 100)) - (WtOfFA2 * ((j - CSWaterAbsorption) / 100)))) + "~";

        //                    strDetails1 = strDetails1 + Convert.ToString(Convert.ToInt32(WtOfFA1 - (WtOfFA1 * (NSWaterAbsorption - row) / 100)));
        //                    strDetails1 = strDetails1 + " | " + Convert.ToString(Convert.ToInt32(WtOfFA2 - (WtOfFA2 * (CSWaterAbsorption - j) / 100))) + "~";
        //                }
        //            }

        //            string[] Moist = Convert.ToString(strDetails).Split('~');
        //            foreach (var mst in Moist)
        //            {
        //                if (mst != "")
        //                {
        //                    cell1 = new PdfPCell(new Phrase(mst, fontH3));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //            }

        //            pdfDoc.Add(table1);
        //            table1 = new PdfPTable(10);
        //            table1.SpacingBefore = 10;
        //            table1.WidthPercentage = 80;
        //            table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table1.SetTotalWidth(new float[] { 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f });
        //            if (i == 0)
        //            {
        //                string[] header = { "Weight", "1", "2", "3 ", "4", "5", "6", "7", "8", "9" };
        //                for (int h = 0; h < header.Count(); h++)
        //                {
        //                    cell1 = new PdfPCell(new Phrase(header[h], fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    if (h == 0)
        //                    {
        //                        cell1.Rowspan = 3;
        //                        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    }
        //                    table1.AddCell(cell1);
        //                }
        //                string[] subheader = { "NS | CS", "NS | CS", "NS | CS ", "NS | CS", "NS | CS", "NS | CS", "NS | CS", "NS | CS", "NS | CS" };
        //                for (int h = 0; h < subheader.Count(); h++)
        //                {
        //                    cell1 = new PdfPCell(new Phrase(subheader[h], fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //                cell1 = new PdfPCell(new Phrase(" (Weight  in  Kg)  ", fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                cell1.Colspan = 9;
        //                table1.AddCell(cell1);

        //            }

        //            string[] nsics = Convert.ToString(strDetails1).Split('~');
        //            foreach (var nst in nsics)
        //            {
        //                if (nst != "")
        //                {
        //                    cell1 = new PdfPCell(new Phrase(nst, fontH3));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //            }

        //            pdfDoc.Add(table1);
        //            strDetails2 = strDetails2 + "Dry~";
        //            strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((-2 * WtOfFA1) / 100) - ((-2 * WtOfFA2) / 100))) + "~";
        //            strDetails2 = strDetails2 + "Moist~";
        //            strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((2 * WtOfFA1) / 100) - ((2 * WtOfFA2) / 100))) + "~";
        //            strDetails2 = strDetails2 + "Wet~";
        //            strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((5 * WtOfFA1) / 100) - ((5 * WtOfFA2) / 100))) + "~";
        //            strDetails2 = strDetails2 + "Saturated~";
        //            strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((8 * WtOfFA1) / 100) - ((8 * WtOfFA2) / 100))) + "~";
        //        }
        //        else
        //        {
        //            table1 = new PdfPTable(3);
        //            table1.SpacingBefore = 10;
        //            table1.WidthPercentage = 50;
        //            table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table1.SetTotalWidth(new float[] { 12f, 12f, 12f });
        //            if (i == 0)
        //            {
        //                string[] header = { "Total Moisture (%)", "Water to be added (lit)", "Wt. of Fine Aggregate" };
        //                for (int h = 0; h < header.Count(); h++)
        //                {
        //                    cell1 = new PdfPCell(new Phrase(header[h], fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //            }

        //            if (NSandFlag == true)
        //            {
        //                for (int row = 1; row <= 12; row++)
        //                {
        //                    strDetails = strDetails + row + "~";
        //                    strDetails = strDetails + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - (WtOfFA1 * ((Convert.ToDouble(row) - NSWaterAbsorption) / 100)))) + "~";
        //                    strDetails = strDetails + Convert.ToString(Convert.ToInt32(WtOfFA1 - (WtOfFA1 * (NSWaterAbsorption - Convert.ToDouble(row)) / 100))) + "~";
        //                }
        //                strDetails2 = strDetails2 + "Dry~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((-2 * WtOfFA1) / 100))) + "~";
        //                strDetails2 = strDetails2 + "Moist~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((2 * WtOfFA1) / 100))) + "~";
        //                strDetails2 = strDetails2 + "Wet~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((5 * WtOfFA1) / 100))) + "~";
        //                strDetails2 = strDetails2 + "Saturated~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((8 * WtOfFA1) / 100))) + "~";
        //            }
        //            else if (CSandFlag == true)
        //            {
        //                for (int row = 1; row <= 12; row++)
        //                {
        //                    strDetails = strDetails + row + "~";
        //                    strDetails = strDetails + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - (WtOfFA2 * ((Convert.ToDouble(row) - CSWaterAbsorption) / 100)))) + "~";
        //                    strDetails = strDetails + Convert.ToString(Convert.ToInt32(WtOfFA2 - (WtOfFA2 * (CSWaterAbsorption - Convert.ToDouble(row)) / 100))) + "~";
        //                }
        //                strDetails2 = strDetails2 + "Dry~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((-2 * WtOfFA2) / 100))) + "~";
        //                strDetails2 = strDetails2 + "Moist~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((2 * WtOfFA2) / 100))) + "~";
        //                strDetails2 = strDetails2 + "Wet~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((5 * WtOfFA2) / 100))) + "~";
        //                strDetails2 = strDetails2 + "Saturated~";
        //                strDetails2 = strDetails2 + Convert.ToString(Convert.ToInt32(((50 + FlyAshWt) * WaterCementRatio) - ((8 * WtOfFA2) / 100))) + "~";
        //            }
        //            string[] Moist = strDetails.Split('~');
        //            foreach (var mst in Moist)
        //            {
        //                if (mst != "")
        //                {
        //                    cell1 = new PdfPCell(new Phrase(mst, fontH3));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //            }

        //            pdfDoc.Add(table1);
        //        }
        //        table1 = new PdfPTable(1);
        //        table1.SpacingBefore = 10;
        //        table1.WidthPercentage = 80;
        //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        table1.SetTotalWidth(new float[] { 78f });

        //        cell1 = new PdfPCell(new Phrase("C) Method Of Moisture Correction in absence of equipment  ", fontH4));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("It is recommended that total moisture be calculated for every pour by procedure mentioned above .", fontH3));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Phrase("However in absence of proper equipment following corrections may be applied. ", fontH3));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase("The moisture correction calculated in section A & B can also be verified by this table. ", fontH3));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        pdfDoc.Add(table1);

        //        table1 = new PdfPTable(2);
        //        table1.SpacingBefore = 10;
        //        table1.WidthPercentage = 50;
        //        table1.HorizontalAlignment = Element.ALIGN_LEFT;

        //        table1.SetTotalWidth(new float[] { 20f, 20f });
        //        if (i == 0)
        //        {
        //            string[] header = { "Condition of Sand ", "Water to be added (lit) " };
        //            for (int h = 0; h < header.Count(); h++)
        //            {
        //                cell1 = new PdfPCell(new Phrase(header[h], fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);
        //            }
        //        }

        //        string[] Equipmt = Convert.ToString(strDetails2).Split('~');
        //        foreach (var Equip in Equipmt)
        //        {
        //            if (Equip != "")
        //            {
        //                cell1 = new PdfPCell(new Phrase(Equip, fontH3));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);
        //            }
        //        }

        //        pdfDoc.Add(table1);
        //        i++;


        //        table1 = new PdfPTable(1);
        //        table1.SpacingBefore = 10;
        //        table1.WidthPercentage = 80;
        //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1 = new PdfPCell(new Phrase("CIN - U28939PN1999PTC014212", fontH3));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Phrase("Regd.Add - 1160/5, GHARPURE Colony Shivaji Nagar, Pune 411005,Maharashtra India", fontH2));
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.Border = PdfPCell.NO_BORDER;
        //        table1.AddCell(cell1);
        //        pdfDoc.Add(table1);
        //        pdfDoc.Close();
        //        string pdfPath = @"D:/" + Subfoldername1 + "/" + fileName;
        //        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //        Response.ContentType = "application/pdf";
        //        Response.WriteFile(pdfPath);
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        string s = ex.Message;
        //    }
        //}

        protected void lnkLoadMDLValues_Click(object sender, EventArgs e)
        {
            ViewState["MDLetterTable"] = null;
            grdMDLetter.DataSource = null;
            grdMDLetter.DataBind();
            DisplayMDLetter("MDL");
        }

        protected void txtGradeofConcrete_TextChanged(object sender, EventArgs e)
        {
            decimal result;
            if (decimal.TryParse(txtGradeofConcrete.Text.Replace("M", ""), out result))
            {
                if (Convert.ToDecimal(txtGradeofConcrete.Text.Replace("M", "")) <= 15)
                    txtStdDev.Text = "3.5";
                else if (Convert.ToDecimal(txtGradeofConcrete.Text.Replace("M", "")) >= 20 && Convert.ToDecimal(txtGradeofConcrete.Text.Replace("M", "")) <= 25)
                    txtStdDev.Text = "4";
                else
                    txtStdDev.Text = "5";
            }
        }

        protected void optMDL_CheckedChanged(object sender, EventArgs e)
        {
            LoadOtherPendingReports();
        }

        private void LoadOtherPendingReports()
        {
            ddlOtherPendingRpt.DataSource = null;
            ddlOtherPendingRpt.DataBind();
            string reportStatus = "", mdlStatus = "";
            if (optEnter.Checked == true)
                reportStatus = "Enter";
            else
                reportStatus = "Check";
            if (optMDL.Checked == true)
                mdlStatus = "MDL";
            else
                mdlStatus = "Final";
            var mflist = dc.MFReport_View(reportStatus, mdlStatus);
            ddlOtherPendingRpt.DataTextField = "MFINWD_ReferenceNo_var";
            ddlOtherPendingRpt.DataSource = mflist;
            ddlOtherPendingRpt.DataBind();
            ddlOtherPendingRpt.Items.Insert(0, "---Select---");

            if (lblReportType.Text == "Final")
                lnkLoadMDLValues.Visible = true;
            else
                lnkLoadMDLValues.Visible = false;
            //ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(txt_ReferenceNo.Text);
            //if (itemToRemove != null)
            //{
            //    ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            //}
        }

        protected void optFinal_CheckedChanged(object sender, EventArgs e)
        {

            LoadOtherPendingReports();
        }

        protected void optEnter_CheckedChanged(object sender, EventArgs e)
        {
            lblStatus.Text = "Enter";
            LoadOtherPendingReports();
        }

        protected void optCheck_CheckedChanged(object sender, EventArgs e)
        {
            lblStatus.Text = "Check";
            LoadOtherPendingReports();
        }

        protected void AddAllRemarks(Boolean forMDL)
        {
            if (forMDL == true)
            {


            }
            else   // false for final report entry
            {

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
        //public string RoundAdm(string amdValue)
        //{
        //    //Int32  result;
        //    //double aa=0;
        //    //if (Int32.TryParse(amdValue, out result))
        //    //{
        //    //   aa= Convert.ToDouble(amdValue);


        //    //}

        //    //return "b";

        //}


    }
}