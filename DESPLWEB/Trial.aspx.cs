using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class Trial : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                        ddl_OtherPendingRpt.SelectedValue = txt_RefNo.Text;

                        arrIndMsg = arrMsgs[1].Split('=');
                        lblStatus.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[2].Split('=');
                        lbl_TrialId.Text = arrIndMsg[1].ToString().Trim();
                    }
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Trial Calculation";
                getCurrentDate();
                LoadReferenceNoList();
                if (txt_RefNo.Text != "")
                {
                    ClearAllControls();
                    LoadTrialList();
                    LoadTrialDetails();

                    //AddRowSlump();
                    //if (lblStatus.Text == "AddNewTrial")
                    //{
                    //    ShowNewTrial();
                    //    DisplayTrialDetails();
                    //}
                    //else
                    //{
                    //    ShowTrailData();
                    //}
                    //TrialGrd();
                }
            }
        }

        private void LoadReferenceNoList()
        {
            var reportList = dc.MaterialDetail_View_List("Trial");
            ddl_OtherPendingRpt.DataTextField = "MaterialDetail_RefNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            //ddl_OtherPendingRpt.Items.Remove(txt_RefNo.Text);
        }

        private void LoadTrialList()
        {
            var trial = dc.Trial_View(ddl_OtherPendingRpt.SelectedItem.Value, false);
            ddlTrial.DataTextField = "Trial_Name";
            ddlTrial.DataValueField = "Trial_Id";
            ddlTrial.DataSource = trial;
            ddlTrial.DataBind();
            ddlTrial.Items.Insert(0, new ListItem("---New---", "0"));
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (ddl_OtherPendingRpt.SelectedIndex == 0)
            {
                lblMsg.Text = "Select report number.";
                lblMsg.Visible = true;
            }
            else
            {
                ClearAllControls();
                LoadTrialList();

                txt_RefNo.Text = ddl_OtherPendingRpt.SelectedItem.Value;
                lblStatus.Text = "AddNewTrial";
                lbl_TrialId.Text = "0";

                LoadTrialDetails();
            }
        }

        protected void ddlTrial_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            LoadTrialDetails();
        }

        protected void LoadTrialDetails()
        {
            lblStatus.Text = "";
            lbl_TrialId.Text = ddlTrial.SelectedValue;
            //AddRowSlump();
            if (ddlTrial.SelectedIndex == 0)
            {
                lblStatus.Text = "AddNewTrial";
                ShowNewTrial();
                DisplayTrialDetails();
            }
            else
            {
                ShowTrailData();
            }
            TrialGrd();
            CheckMDLStatus();
            LoadSystemTrialDetails();
            if (ddlTrial.SelectedIndex == 0)
            {
                lnkSave.Enabled = true;
            }
            //else
            //{
            //    lnkSave.Enabled = false;
            //}
        }

        protected void LoadSystemTrialDetails()
        {
            int FACount = 0;
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                if (lbl_MaterialName.Text.Contains("Natural Sand") == true)                
                {
                    FACount++;
                }
                else if (lbl_MaterialName.Text.Contains("Crushed Sand") == true)
                {
                    FACount++;
                }
                if (FACount > 1)
                    break;
            }
            var trial = dc.MixDesign_System_View(ddl_OtherPendingRpt.SelectedValue, Convert.ToInt32(ddlTrial.SelectedValue), "", true).ToList();
            foreach (var trl in trial)
            {
                bool FA1Found = false;
                for (int i = 0; i < grdTrail.Rows.Count; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_TrailSystem = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_TrailSystem");

                    if (lbl_MaterialName.Text.Contains("Cement") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultCement_dec).ToString("0");
                    }
                    else if (lbl_MaterialName.Text.Contains("Fly Ash") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultFlyash_dec).ToString("0");
                    }
                    else if (lbl_MaterialName.Text.Contains("G G B S") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultGGBS_dec).ToString("0");
                    }
                    else if (lbl_MaterialName.Text.Contains("Micro Silica") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultMicrosilica_dec).ToString("0");
                    }
                    //Metakaolin
                    else if (lbl_MaterialName.Text.Contains("W/C Ratio") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultWCRatio_dec).ToString("0.00");
                    }
                    else if (lbl_MaterialName.Text.Contains("Water") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultWater_dec).ToString("0");
                    }
                    else if (lbl_MaterialName.Text.Contains("Natural Sand") == true)
                    {
                        if (FACount > 1 && trl.MFSys_FA1Percent_dec != null && trl.MFSys_FA1Percent_dec > 0)
                        {
                            if (FA1Found == true)
                                txt_TrailSystem.Text = ((Convert.ToDecimal(trl.MFSys_FA2Percent_dec) * Convert.ToDecimal(trl.MFSys_ResultFineAggregatePercent_dec)) / 100).ToString("0.00");
                            else
                                txt_TrailSystem.Text = ((Convert.ToDecimal(trl.MFSys_FA1Percent_dec) * Convert.ToDecimal(trl.MFSys_ResultFineAggregatePercent_dec)) / 100).ToString("0.00");
                            FA1Found = true;
                        }
                        else
                        {
                            txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultFineAggregatePercent_dec).ToString();
                        }
                    }
                    else if (lbl_MaterialName.Text.Contains("Crushed Sand") == true) //Stone Dust, Grit, Mix Aggregate                       
                    {
                        if (FACount > 1 && trl.MFSys_FA1Percent_dec != null && trl.MFSys_FA1Percent_dec > 0)
                        {
                            if (FA1Found == true)
                                txt_TrailSystem.Text = ((Convert.ToDecimal(trl.MFSys_FA2Percent_dec) * Convert.ToDecimal(trl.MFSys_ResultFineAggregatePercent_dec)) / 100).ToString("0.00");
                            else
                                txt_TrailSystem.Text = ((Convert.ToDecimal(trl.MFSys_FA1Percent_dec) * Convert.ToDecimal(trl.MFSys_ResultFineAggregatePercent_dec)) / 100).ToString("0.00");
                            FA1Found = true;
                        }
                        else
                        {
                            txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultFineAggregatePercent_dec).ToString();
                        }
                    }
                    else if (lbl_MaterialName.Text.Contains("10 mm") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_Result10mmPercent_dec).ToString();
                    }
                    else if (lbl_MaterialName.Text.Contains("20 mm") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_Result20mmPercent_dec).ToString();
                    }
                    else if (lbl_MaterialName.Text.Contains("40 mm") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_Result40mmPercent_dec).ToString();
                    }
                    else if (lbl_MaterialName.Text.Contains("Admixture") == true)
                    {
                        txt_TrailSystem.Text = Convert.ToDecimal(trl.MFSys_ResultAdmixtureDosage_dec).ToString("0.00") + " (%)";
                    }
                }                
            }
            
        }

        protected void CheckMDLStatus()
        {
            lnkSave.Enabled = true;
            lnkSaveOtherInfo.Enabled = true; 
            //if (ddlTrial.SelectedIndex > 0)
            //{
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAdmin_right_bit == false)
                    {
                        var mfinwd = dc.MF_View(txt_RefNo.Text, 0, txt_RecType.Text);
                        foreach (var mf in mfinwd)
                        {
                            if (mf.MFINWD_Status_tint >= 3)
                            {
                                lnkSave.Enabled = false;
                                lnkSaveOtherInfo.Enabled = false;
                            }
                        }
                    }
                }
            //}
        }

        protected void ClearAllControls()
        {
            //txt_RefNo.Text = "";
            //txt_RecType.Text = "";
            txt_GradeofConcrete.Text = "";
            txt_CementUsed.Text = "";
            txt_NoofCubes.Text = "";
            txt_Natureofwork.Text = "";
            txt_Admixture.Text = "";
            txtTotCementitiousMat.Text = "";
            txt_Slump.Text = "";
            txt_FlyashUsed.Text = "";
            txtFlyAshReplacement.Text = "";

            lblFlyAshReplacement.Visible = false;
            lblTotCementitiousMat.Visible = false;
            txtTotCementitiousMat.Visible = false;
            txtFlyAshReplacement.Visible = false;

            txt_McNaturalSand.Text = "";
            txt_McCrushed.Text = "";
            txt_McStoneDust.Text = "";
            txt_McGrit.Text = "";
            txt_Mc10mm.Text = "";
            txt_Mc20mm.Text = "";
            txt_Mc40mm.Text = "";

            lbl_McNatural.Visible = false;
            lbl_McCrushed.Visible = false;
            lbl_McStoneDust.Visible = false;
            lbl_McGrit.Visible = false;
            lbl_Mc10mm.Visible = false;
            lbl_Mc20mm.Visible = false;
            lbl_Mc40mm.Visible = false;
            txt_McNaturalSand.Visible = false;
            txt_McCrushed.Visible = false;
            txt_McStoneDust.Visible = false;
            txt_McGrit.Visible = false;
            txt_Mc10mm.Visible = false;
            txt_Mc20mm.Visible = false;
            txt_Mc40mm.Visible = false;

            txt_waNaturalSand.Text = "";
            txt_waCrushedSand.Text = "";
            txt_waStoneDust.Text = "";
            txt_waGrit.Text = "";
            txt_wa10mm.Text = "";
            txt_wa20mm.Text = "";
            txt_wa40mm.Text = "";

            lbl_waNatural.Visible = false;
            lbl_waCrushed.Visible = false;
            lbl_waStoneDust.Visible = false;
            lbl_waGrit.Visible = false;
            lbl_wa10mm.Visible = false;
            lbl_wa20mm.Visible = false;
            lbl_wa40mm.Visible = false;
            txt_waNaturalSand.Visible = false;
            txt_waCrushedSand.Visible = false;
            txt_waStoneDust.Visible = false;
            txt_waGrit.Visible = false;
            txt_wa10mm.Visible = false;
            txt_wa20mm.Visible = false;
            txt_wa40mm.Visible = false;

            chk_WitnessBy.Checked = false;
            txt_witnessBy.Text = "";
            txt_witnessBy.Visible = false;
            txt_SuperwiseBy.Text = "";

            txtYield.Text = "2500";
            txtWtOfConcreteInCylinder.Text = ""; 
            txtComment.Text = "";

            chkValidationFor.Checked = false;

            grdTrail.DataSource = null;
            grdTrail.DataBind();
            grdAllInAGGT.DataSource = null;
            grdAllInAGGT.DataBind();
            getCurrentDate();
        }

        public void getCurrentDate()
        {
            txt_TrialDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_TrialTime.Text = DateTime.Now.ToShortTimeString();
        }

        protected void ShowNewTrial()
        {
            var res = dc.MF_View(Convert.ToString(txt_RefNo.Text), 0, "");
            foreach (var m in res)
            {
                txt_RefNo.Text = m.MFINWD_ReferenceNo_var.ToString();
                txt_RecType.Text = m.MFINWD_RecordType_var.ToString();
                txt_GradeofConcrete.Text = m.MFINWD_Grade_var.ToString();
                txt_RefNo.Text = m.MFINWD_ReferenceNo_var.ToString();
                txt_Natureofwork.Text = m.MFINWD_NatureofWork_var.ToString();
                txt_Slump.Text = m.MFINWD_Slump_var.ToString();
            }
            
            txt_FlyashUsed.Text = "---";
            string[] MatrialName = { "Cement", "Admixture", "Fly Ash" };
            for (int s = 0; s < MatrialName.Count(); s++)
            {
                var mat = dc.MaterialDetail_View(0, txt_RefNo.Text, 0, MatrialName[s].ToString(), null, null, "");
                foreach (var m in mat)
                {
                    if (Convert.ToString(m.MaterialDetail_Information) != "")
                    {
                        if (MatrialName[s] == "Cement")
                        {
                            txt_CementUsed.Text = m.MaterialDetail_Information.ToString();
                        }
                        if (MatrialName[s] == "Admixture")
                        {
                            txt_Admixture.Text = m.MaterialDetail_Information.ToString();
                        }
                        if (MatrialName[s] == "Fly Ash")
                        {
                            txt_FlyashUsed.Text = m.MaterialDetail_Information.ToString();
                        }
                    }
                }
            }

            if (grdTrail.Rows.Count <= 0)
            {
                DisplayTrialGrd();
            }
        }

        protected void ShowTrailData()
        {
            int i = 0;
            //txt_RefNo.Text = Convert.ToString(txt_RefNo.Text); 
            txt_RecType.Text = "MF";
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var t in data)
            {
                if (i == 0)
                {
                    txt_CementUsed.Text = Convert.ToString(t.Trial_CementUsed);
                    txt_GradeofConcrete.Text = Convert.ToString(t.Trial_Grade);
                    txt_TrialDate.Text = Convert.ToDateTime(t.Trial_Date).ToString("dd/MM/yyyy");
                    txt_TrialTime.Text = new DateTime().Add(TimeSpan.Parse(t.Trial_Time.ToString())).ToString("hh:mm tt").ToString();
                    txt_Slump.Text = Convert.ToString(t.Trial_Slump);
                    txt_Natureofwork.Text = Convert.ToString(t.Trial_NatureofWork);
                    txt_Admixture.Text = Convert.ToString(t.Trial_Admixture);
                    txt_FlyashUsed.Text = Convert.ToString(t.Trial_FlyashUsed);
                    txt_NoofCubes.Text = Convert.ToString(t.Trial_NoOfCubes);
                    txtTotCementitiousMat.Text = Convert.ToString(t.Trial_TotalCemetitiousMat);
                    txtFlyAshReplacement.Text = Convert.ToString(t.Trial_FlyAshRep);
                    lbl_TrialId.Text = t.Trial_Id.ToString();
                    //moisture content
                    if (t.Trial_MC_CS != "" && t.Trial_MC_CS != null)
                    {
                        txt_McCrushed.Text = t.Trial_MC_CS.ToString();
                        txt_McCrushed.Visible = true;
                        lbl_McCrushed.Visible = true;
                    }
                    if (t.Trial_MC_GT != "" && t.Trial_MC_GT != null)
                    {
                        txt_McGrit.Text = t.Trial_MC_GT.ToString();
                        txt_McGrit.Visible = true;
                        lbl_McGrit.Visible = true;
                    }
                    if (t.Trial_MC_NS != "" && t.Trial_MC_NS != null)
                    {
                        txt_McNaturalSand.Text = t.Trial_MC_NS.ToString();
                        txt_McNaturalSand.Visible = true;
                        lbl_McNatural.Visible = true;
                    }
                    if (t.Trial_MC_SD != "" && t.Trial_MC_SD != null)
                    {
                        txt_McStoneDust.Text = t.Trial_MC_SD.ToString();
                        txt_McStoneDust.Visible = true;
                        lbl_McStoneDust.Visible = true;
                    }
                    if (t.Trial_MC_10mm != "" && t.Trial_MC_10mm != null)
                    {
                        txt_Mc10mm.Text = t.Trial_MC_10mm.ToString();
                        txt_Mc10mm.Visible = true;
                        lbl_Mc10mm.Visible = true;
                    }
                    if (t.Trial_MC_20mm != "" && t.Trial_MC_20mm != null)
                    {
                        txt_Mc20mm.Text = t.Trial_MC_20mm.ToString();
                        txt_Mc20mm.Visible = true;
                        lbl_Mc20mm.Visible = true;
                    }
                    if (t.Trial_MC_40mm != "" && t.Trial_MC_40mm != null)
                    {
                        txt_Mc40mm.Text = t.Trial_MC_40mm.ToString();
                        txt_Mc40mm.Visible = true;
                        lbl_Mc40mm.Visible = true;
                    }
                    //water content
                    if (t.Trial_WA_NS != "" && t.Trial_WA_NS != null)
                    {
                        txt_waNaturalSand.Text = t.Trial_WA_NS.ToString();
                        txt_waNaturalSand.Visible = true;
                        lbl_waNatural.Visible = true;
                    }
                    if (t.Trial_WA_SD != "" && t.Trial_WA_SD != null)
                    {
                        txt_waStoneDust.Text = t.Trial_WA_SD.ToString();
                        txt_waStoneDust.Visible = true;
                        lbl_waStoneDust.Visible = true;
                    }
                    if (t.Trial_WA_GT != "" && t.Trial_WA_GT != null)
                    {
                        txt_waGrit.Text = t.Trial_WA_GT.ToString();
                        txt_waGrit.Visible = true;
                        lbl_waGrit.Visible = true;
                    }
                    if (t.Trial_WA_CS != "" && t.Trial_WA_CS != null)
                    {
                        txt_waCrushedSand.Text = t.Trial_WA_CS.ToString();
                        txt_waCrushedSand.Visible = true;
                        lbl_waCrushed.Visible = true;
                    }
                    if (t.Trial_WA_10mm != "" && t.Trial_WA_10mm != null)
                    {
                        txt_wa10mm.Text = t.Trial_WA_10mm.ToString();
                        txt_wa10mm.Visible = true;
                        lbl_wa10mm.Visible = true;
                    }
                    if (t.Trial_WA_20mm != "" && t.Trial_WA_20mm != null)
                    {
                        txt_wa20mm.Text = t.Trial_WA_20mm.ToString();
                        txt_wa20mm.Visible = true;
                        lbl_wa20mm.Visible = true;
                    }
                    if (t.Trial_WA_40mm != "" && t.Trial_WA_40mm != null)
                    {
                        txt_wa40mm.Text = t.Trial_WA_40mm.ToString();
                        txt_wa40mm.Visible = true;
                        lbl_wa40mm.Visible = true;
                    }
                    if (Convert.ToString(t.Trial_WitnessBy) != "")
                    {
                        chk_WitnessBy.Checked = true;
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = Convert.ToString(t.Trial_WitnessBy);
                    }
                    txt_SuperwiseBy.Text = Convert.ToString(t.Trial_SuperwiseBy);

                    chkValidationFor.Checked = Convert.ToBoolean(t.Trial_ValidationFor_bit);
                }
                AddRowTrail();
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");
                TextBox txt_MatId = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_MatId");
                Label lbl_TrailUnit = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit");
                Label lbl_TrailUnit2 = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit2");
                TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                TextBox txt_CorrectionsNew = (TextBox)grdTrail.Rows[i].FindControl("txt_CorrectionsNew");
                TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].FindControl("txt_TrailActual");

                lbl_MaterialName.Text = Convert.ToString(t.TrialDetail_MaterialName);
                txt_Trail.Text = Convert.ToString(t.TrialDetail_Weight);
                txt_SpGrav.Text = Convert.ToString(t.TrialDetail_SpecificGravity);
                txt_Volume.Text = Convert.ToString(t.TrialDetail_Volume);
                txt_Reqdwt.Text = Convert.ToString(t.TrialDetail_ReqdWt);
                txt_Corrections.Text = Convert.ToString(t.TrialDetail_Corrections);
                txt_NetWt.Text = Convert.ToString(t.TrialDetail_NetWeight);
                txt_MatId.Text = t.Material_Id.ToString();
                txt_SurfaceMoisture.Text = t.TrialDetail_SurfaceMoisture.ToString();
                txt_CorrectionsNew.Text = t.TrialDetail_CorrectionsNew.ToString();
                txt_TrailActual.Text = t.TrialDetail_ActualTrial.ToString();
                if (t.TrialDetail_ActualTrial != null)
                {
                    if (lbl_MaterialName.Text == "Admixture")
                    {
                        txt_TrailActual.Text = Convert.ToDecimal(t.TrialDetail_ActualTrial).ToString("0.000");
                    }
                    else if (lbl_MaterialName.Text == "W/C Ratio")
                    {
                        txt_TrailActual.Text = Convert.ToDecimal(t.TrialDetail_ActualTrial).ToString("0.00");
                    }
                    else
                    {
                        txt_TrailActual.Text = Convert.ToDecimal(t.TrialDetail_ActualTrial).ToString("0");
                    }
                }
                if (lbl_MaterialName.Text.Contains("Cement") == true || lbl_MaterialName.Text == "Fly Ash" || lbl_MaterialName.Text == "G G B S"
                    || lbl_MaterialName.Text == "Micro Silica" || lbl_MaterialName.Text == "Metakaolin")
                {
                    lbl_TrailUnit.Text = "(kg)";
                }
                else if (lbl_MaterialName.Text == "Water")
                {
                    lbl_TrailUnit.Text = "(lit)";
                }
                else if (lbl_MaterialName.Text == "W/C Ratio")
                {
                    lbl_TrailUnit.Text = "(%)";
                }
                else if (lbl_MaterialName.Text == "Natural Sand" || lbl_MaterialName.Text == "Crushed Sand" ||
                    lbl_MaterialName.Text == "Stone Dust" || lbl_MaterialName.Text == "Grit" ||
                    lbl_MaterialName.Text == "Mix Aggregate" || lbl_MaterialName.Text == "10 mm" ||
                    lbl_MaterialName.Text == "20 mm" || lbl_MaterialName.Text == "40 mm")
                {
                    lbl_TrailUnit.Text = "(%)";
                }
                else if (lbl_MaterialName.Text == "Admixture")
                {
                    lbl_TrailUnit.Text = "(lit)";
                    lbl_TrailUnit2.Text = "(%)";
                    lbl_TrailUnit2.Visible = true;
                }
                
                i++;

            }
            if (grdTrail.Rows.Count <= 0)
            {
                DisplayTrialGrd();
            }
            
        }

        protected void DisplayTrialDetails()
        {
            //var MaterialName = dc.MaterialListView(txt_RefNo.Text, "", "Aggregate");
            //foreach (var c in MaterialName)
            //{
            //var Inward_AggT = dc.MF_View(txt_RefNo.Text, Convert.ToInt32(c.Material_Id), "MF");
            //foreach (var aggt in Inward_AggT)
            //{

            clsData objcls = new clsData();
            string mySql = "select distinct(Material_Id ) from tbl_MaterialList,tbl_MaterialDetail ";
            mySql += " where tbl_MaterialList.Material_Id= tbl_MaterialDetail.MaterialDetail_Id and Material_Type='Aggregate'";
            mySql += " and tbl_MaterialDetail.MaterialDetail_RefNo='" + txt_RefNo.Text + "'";
            DataTable dtMatrls = objcls.getGeneralData(mySql);
            string[] RefNo1 = Convert.ToString(txt_RefNo.Text).Split('/');

            //foreach (var m in Mix)
            Int32 mID = 0, SrNo = 0;
            for (int i1 = 0; i1 < dtMatrls.Rows.Count; i1++)
            {
                mID = Convert.ToInt32(dtMatrls.Rows[i1]["material_id"].ToString());
                var MfInwd = dc.MF_View1(RefNo1[0].ToString() + "/%", mID);
                SrNo = 0;
                foreach (var aggt in MfInwd)
                {
                    if (SrNo == 0 || Convert.ToInt32(aggt.AGGTINWD_Material_Id) != SrNo)
                    {
                        decimal Specgrav = 0;
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "Natural Sand")
                        {
                            txt_waNaturalSand.Visible = true;
                            lbl_waNatural.Visible = true;
                            txt_McNaturalSand.Visible = true;
                            lbl_McNatural.Visible = true;
                            if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                            {
                                txt_waNaturalSand.Text = aggt.AGGTINWD_WaterAborp_var.ToString();

                            }
                            //if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                            //{
                            //    txt_McNaturalSand.Text = aggt.AGGTINWD_MoistureContent_var.ToString();

                            //}
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                for (int i = 0; i < grdTrail.Rows.Count; i++)
                                {
                                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                                    //TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                                    if (lbl_MaterialName.Text.Trim() == "Natural Sand")
                                    {
                                        //if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "" &&
                                        //    aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                                        //{
                                        //    txt_SurfaceMoisture.Text = (Convert.ToDecimal(aggt.AGGTINWD_MoistureContent_var) - Convert.ToDecimal(aggt.AGGTINWD_WaterAborp_var)).ToString();
                                        //}
                                        if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                        {
                                            txt_SpGrav.Text = Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var).ToString("0.00");
                                        }
                                        else
                                        {
                                            txt_SpGrav.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                                        }
                                        txt_SpGrav.CssClass = "caltextbox";
                                        break;
                                    }
                                }
                            }
                        }
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "Crushed Sand")
                        {
                            txt_waCrushedSand.Visible = true;
                            lbl_waCrushed.Visible = true;
                            if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                            {
                                txt_waCrushedSand.Text = aggt.AGGTINWD_WaterAborp_var.ToString();

                            }
                            txt_McCrushed.Visible = true;
                            lbl_McCrushed.Visible = true;
                            //if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                            //{
                            //    txt_McCrushed.Text = aggt.AGGTINWD_MoistureContent_var.ToString();

                            //}
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                for (int i = 0; i < grdTrail.Rows.Count; i++)
                                {
                                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                                    //TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                                    if (lbl_MaterialName.Text.Trim() == "Crushed Sand")
                                    {
                                        //if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "" &&
                                        //    aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                                        //{
                                        //    txt_SurfaceMoisture.Text = (Convert.ToDecimal(aggt.AGGTINWD_MoistureContent_var) - Convert.ToDecimal(aggt.AGGTINWD_WaterAborp_var)).ToString();
                                        //}
                                        if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                        {
                                            txt_SpGrav.Text = Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var).ToString("0.00");
                                        }
                                        else
                                        {
                                            txt_SpGrav.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                                        }
                                        txt_SpGrav.CssClass = "caltextbox";
                                        break;
                                    }
                                }
                            }
                        }
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "Stone Dust")
                        {
                            txt_waStoneDust.Visible = true;
                            lbl_waStoneDust.Visible = true;
                            txt_McStoneDust.Visible = true;
                            lbl_McStoneDust.Visible = true;
                            if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                            {
                                txt_waStoneDust.Text = aggt.AGGTINWD_WaterAborp_var.ToString();

                            }
                            //if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                            //{
                            //    txt_McStoneDust.Text = aggt.AGGTINWD_MoistureContent_var.ToString();

                            //}
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                for (int i = 0; i < grdTrail.Rows.Count; i++)
                                {
                                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                                    //TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                                    if (lbl_MaterialName.Text.Trim() == "Stone Dust")
                                    {
                                        //if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "" &&
                                        //    aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                                        //{
                                        //    txt_SurfaceMoisture.Text = (Convert.ToDecimal(aggt.AGGTINWD_MoistureContent_var) - Convert.ToDecimal(aggt.AGGTINWD_WaterAborp_var)).ToString();
                                        //}
                                        if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                        {
                                            txt_SpGrav.Text = Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var).ToString("0.00");
                                        }
                                        else
                                        {
                                            txt_SpGrav.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                                        }
                                        txt_SpGrav.CssClass = "caltextbox";
                                        break;
                                    }
                                }
                            }
                        }
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "Grit")
                        {
                            txt_waGrit.Visible = true;
                            lbl_waGrit.Visible = true;
                            txt_McGrit.Visible = true;
                            lbl_McGrit.Visible = true;
                            if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                            {
                                txt_waGrit.Text = aggt.AGGTINWD_WaterAborp_var.ToString();

                            }
                            //if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                            //{
                            //    txt_McGrit.Text = aggt.AGGTINWD_MoistureContent_var.ToString();
                            //}
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                for (int i = 0; i < grdTrail.Rows.Count; i++)
                                {
                                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                                    //TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                                    if (lbl_MaterialName.Text.Trim() == "Grit")
                                    {
                                        //if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "" &&
                                        //    aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                                        //{
                                        //    txt_SurfaceMoisture.Text = (Convert.ToDecimal(aggt.AGGTINWD_MoistureContent_var) - Convert.ToDecimal(aggt.AGGTINWD_WaterAborp_var)).ToString();
                                        //}
                                        if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                        {
                                            txt_SpGrav.Text = Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var).ToString("0.00");
                                        }
                                        else
                                        {
                                            txt_SpGrav.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                                        }
                                        txt_SpGrav.CssClass = "caltextbox";
                                        break;
                                    }
                                }
                            }
                        }
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "10 mm")
                        {
                            txt_wa10mm.Visible = true;
                            lbl_wa10mm.Visible = true;
                            txt_Mc10mm.Visible = true;
                            lbl_Mc10mm.Visible = true;
                            if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                            {
                                txt_wa10mm.Text = aggt.AGGTINWD_WaterAborp_var.ToString();

                            }
                            //if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                            //{
                            //    txt_Mc10mm.Text = aggt.AGGTINWD_MoistureContent_var.ToString();
                            //}
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                for (int i = 0; i < grdTrail.Rows.Count; i++)
                                {
                                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                                    //TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                                    if (lbl_MaterialName.Text.Trim() == "10 mm")
                                    {
                                        //if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "" &&
                                        //    aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                                        //{
                                        //    txt_SurfaceMoisture.Text = (Convert.ToDecimal(aggt.AGGTINWD_MoistureContent_var) - Convert.ToDecimal(aggt.AGGTINWD_WaterAborp_var)).ToString();
                                        //}
                                        if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                        {
                                            txt_SpGrav.Text = Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var).ToString("0.00");
                                        }
                                        else
                                        {
                                            txt_SpGrav.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                                        }
                                        txt_SpGrav.CssClass = "caltextbox";
                                        break;
                                    }
                                }
                            }
                        }
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "20 mm")
                        {
                            txt_wa20mm.Visible = true;
                            lbl_wa20mm.Visible = true;
                            txt_Mc20mm.Visible = true;
                            lbl_Mc20mm.Visible = true;
                            if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                            {
                                txt_wa20mm.Text = aggt.AGGTINWD_WaterAborp_var.ToString();

                            }
                            //if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                            //{
                            //    txt_Mc20mm.Text = aggt.AGGTINWD_MoistureContent_var.ToString();
                            //}
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                for (int i = 0; i < grdTrail.Rows.Count; i++)
                                {
                                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                                    //TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                                    if (lbl_MaterialName.Text.Trim() == "20 mm")
                                    {
                                        //if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "" &&
                                        //    aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                                        //{
                                        //    txt_SurfaceMoisture.Text = (Convert.ToDecimal(aggt.AGGTINWD_MoistureContent_var) - Convert.ToDecimal(aggt.AGGTINWD_WaterAborp_var)).ToString();
                                        //}
                                        if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                        {
                                            txt_SpGrav.Text = Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var).ToString("0.00");
                                        }
                                        else
                                        {
                                            txt_SpGrav.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                                        }
                                        txt_SpGrav.CssClass = "caltextbox";
                                        break;
                                    }
                                }
                            }
                        }
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "40 mm")
                        {
                            txt_wa40mm.Visible = true;
                            lbl_wa40mm.Visible = true;
                            txt_Mc40mm.Visible = true;
                            lbl_Mc40mm.Visible = true;
                            if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                            {
                                txt_wa40mm.Text = aggt.AGGTINWD_WaterAborp_var.ToString();

                            }
                            //if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                            //{
                            //    txt_Mc40mm.Text = aggt.AGGTINWD_MoistureContent_var.ToString();
                            //}
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                for (int i = 0; i < grdTrail.Rows.Count; i++)
                                {
                                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                                    //TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                                    if (lbl_MaterialName.Text.Trim() == "40 mm")
                                    {
                                        //if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "" &&
                                        //    aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                                        //{
                                        //    txt_SurfaceMoisture.Text = (Convert.ToDecimal(aggt.AGGTINWD_MoistureContent_var) - Convert.ToDecimal(aggt.AGGTINWD_WaterAborp_var)).ToString();
                                        //}
                                        if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                        {
                                            txt_SpGrav.Text = Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var).ToString("0.00");
                                        }
                                        else
                                        {
                                            txt_SpGrav.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                                        }
                                        txt_SpGrav.CssClass = "caltextbox";
                                        break;
                                    }
                                }
                            }
                        }
                        SrNo = Convert.ToInt32(aggt.AGGTINWD_Material_Id);
                    }//srno end
                }
            }
        }

        protected void TrialGrd()
        {
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                TextBox txt_CorrectionsNew = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_CorrectionsNew");

                if (lbl_MaterialName.Text == "Fibre")
                {
                    txt_SpGrav.ReadOnly = false;
                }
                if (lbl_MaterialName.Text == "Total")
                {
                    txt_Trail.ReadOnly = true;
                    txt_SpGrav.ReadOnly = true;
                    txt_CorrectionsNew.ReadOnly = true;
                }
                //if (lbl_MaterialName.Text == "Fly Ash")
                //{
                //    lblTotCementitiousMat.Visible = true;
                //    txtTotCementitiousMat.Visible = true;
                //    lblFlyAshReplacement.Visible = true;
                //    txtFlyAshReplacement.Visible = true;
                //}
            }
        }

        protected void DisplayTrialGrd()
        {
            int i = 0;
            //string privMat = "";
            bool Admixture = false;
            int wcRowCnt = 0;
            var res = dc.MaterialDetail_View(0, txt_RefNo.Text, 0, "", null, null, "").ToList();
            foreach (var t in res)
            {
                if (t.Material_List == "Cement")
                    wcRowCnt++;
                else if (t.Material_List == "Fly Ash")
                    wcRowCnt++;
                else if (t.Material_List == "Micro Silica")
                    wcRowCnt++;
                else if (t.Material_List == "G G B S")
                    wcRowCnt++;
                else if (t.Material_List == "Metakaolin")
                    wcRowCnt++;
            }
            foreach (var t in res)
            {
                if (Convert.ToString(t.Material_List).Trim() == "Admixture")
                {
                    Admixture = true;
                }
                else
                {
                    //if ((privMat == "Cement" || privMat == "Fly Ash") && (t.Material_List != "Fly Ash"))
                    if (wcRowCnt == i)
                    {
                        AddRowTrail();
                        Label lblMaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                        Label lblTrailUnit = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit");
                        TextBox txtSpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                        lblMaterialName.Text = "W/C Ratio";
                        lblTrailUnit.Text = "(%)";
                        txtSpGrav.Text = "0.00";
                        i++;
                    }
                    AddRowTrail();
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                    TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");
                    TextBox txt_MatId = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_MatId");
                    Label lbl_TrailUnit = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit");
                    TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_SurfaceMoisture");

                    txt_MatId.Text = t.Material_Id.ToString();

                    lbl_MaterialName.Text = t.Material_List.ToString();
                    if (lbl_MaterialName.Text.Contains("Cement") == true || lbl_MaterialName.Text == "Fly Ash" || lbl_MaterialName.Text == "G G B S"
                    || lbl_MaterialName.Text == "Micro Silica" || lbl_MaterialName.Text == "Metakaolin")
                    {
                        lbl_TrailUnit.Text = "(kg)";
                    }
                    else if (lbl_MaterialName.Text == "Natural Sand" || lbl_MaterialName.Text == "Crushed Sand" ||
                        lbl_MaterialName.Text == "Stone Dust" || lbl_MaterialName.Text == "Grit" ||
                        lbl_MaterialName.Text == "Mix Aggregate" || lbl_MaterialName.Text == "10 mm" ||
                        lbl_MaterialName.Text == "20 mm" || lbl_MaterialName.Text == "40 mm")
                    {
                        lbl_TrailUnit.Text = "(%)";
                    }

                    if (Convert.ToString(t.Material_List) == "Cement")
                    {

                        if (txt_CementUsed.Text.Contains("1489") == true || txt_CementUsed.Text.Contains("455") == true ||
                            txt_CementUsed.Text.Contains("PPC") == true || txt_CementUsed.Text.Contains("PSC") == true)
                        {
                            txt_SpGrav.Text = "2.97";
                        }
                        else if (txt_CementUsed.Text.Contains("8112") == true || txt_CementUsed.Text.Contains("12269") == true ||
                            txt_CementUsed.Text.Contains("43") == true || txt_CementUsed.Text.Contains("53") == true)
                        {
                            txt_SpGrav.Text = "3.15";
                        }
                    }
                    if (Convert.ToString(t.Material_List) == "Fly Ash")
                    {
                        txt_SpGrav.Text = "2.26";
                    }
                    if (Convert.ToString(t.Material_List) == "Micro Silica")
                    {
                        txt_SpGrav.Text = "2.20";
                    }
                    if (Convert.ToString(t.Material_List) == "G G B S")
                    {
                        txt_SpGrav.Text = "2.90";
                    }
                    if (Convert.ToString(t.Material_List) == "Admixture")
                    {
                        txt_SpGrav.Text = "1.10";
                    }
                    if (Convert.ToString(t.Material_List) == "Fibre")
                    {
                        //txt_SpGrav.Text = "";
                        txt_SpGrav.ReadOnly = false;
                    }
                    //privMat = lbl_MaterialName.Text;
                    i++;
                }
            }

            for (int j = 0; j < 4; j++)
            {
                if (j == 1 && Admixture == false)
                {
                    //
                }
                else
                {
                    AddRowTrail();
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                    TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");
                    Label lbl_TrailUnit = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit");
                    Label lbl_TrailUnit2 = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit2");
                    TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_SurfaceMoisture");
                    TextBox txt_CorrectionsNew = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_CorrectionsNew");
                    TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_TrailActual");
                    
                    if (j == 0)
                    {
                        lbl_MaterialName.Text = "Water";
                        lbl_TrailUnit.Text = "(lit)";
                        txt_SpGrav.Text = "1.00";
                    }
                    if (Admixture == true)
                    {
                        if (j == 1)
                        {
                            lbl_MaterialName.Text = "Admixture";
                            lbl_TrailUnit.Text = "(lit)";
                            lbl_TrailUnit2.Text = "(%)";
                            lbl_TrailUnit2.Visible = true;
                            txt_SpGrav.Text = "1.10";
                        }
                    }
                    if (j == 2)
                    {
                        lbl_MaterialName.Text = "Plastic Density";
                    }
                    if (j == 3)
                    {
                        lbl_MaterialName.Text = "Total";
                        txt_Trail.ReadOnly = true;
                        txt_SpGrav.ReadOnly = true;
                        txt_Volume.ReadOnly = true;
                        txt_Reqdwt.ReadOnly = true;
                        txt_Corrections.ReadOnly = true;
                        txt_NetWt.ReadOnly = true;
                        txt_SurfaceMoisture.ReadOnly = true;
                        txt_CorrectionsNew.ReadOnly = true;
                        txt_TrailActual.ReadOnly = true;
                    }
                    if (lbl_MaterialName.Text == "10 mm" || lbl_MaterialName.Text == "20 mm" || lbl_MaterialName.Text == "Crushed Sand" || lbl_MaterialName.Text == "Natural Sand")
                    {
                        txt_SpGrav.ReadOnly = true;
                    }
                    else
                    {
                        txt_SpGrav.ReadOnly = false;
                    }
                    i++;
                }
            }
            
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
                DateTime TrialDate = DateTime.ParseExact(txt_TrialDate.Text, "dd/MM/yyyy", null);
                if (lblStatus.Text == "") //lblStatus.Text == "")
                {
                    dc.Trail_Update(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, txt_McNaturalSand.Text, txt_McCrushed.Text, txt_McStoneDust.Text, txt_McGrit.Text, txt_Mc10mm.Text, txt_Mc20mm.Text, txt_Mc40mm.Text, txt_waNaturalSand.Text, txt_waCrushedSand.Text, txt_waStoneDust.Text, txt_waGrit.Text, txt_wa10mm.Text, txt_wa20mm.Text, txt_wa40mm.Text, TrialDate, TimeSpan.Parse(Convert.ToDateTime(txt_TrialTime.Text).ToString("HH:mm:ss")), txt_GradeofConcrete.Text, txt_CementUsed.Text, txt_FlyashUsed.Text, "", txt_Natureofwork.Text, txt_Slump.Text, txt_Admixture.Text, Convert.ToDecimal(txt_NoofCubes.Text), false, "", "", "", "", false, "", false, txt_witnessBy.Text, txt_SuperwiseBy.Text, "", txtTotCementitiousMat.Text, txtFlyAshReplacement.Text,"",0,"", 0, chkValidationFor.Checked, 0, false, true);

                    dc.TrailDetail_Update(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, "", "", 0, 0, 0, "", 0, 0, 0, 0, true);
                }
                else
                {
                    lbl_TrialId.Text = "0";
                    int trialId = 0;
                    trialId = dc.Trail_Update(0, txt_RefNo.Text, txt_McNaturalSand.Text, txt_McCrushed.Text, txt_McStoneDust.Text, txt_McGrit.Text, txt_Mc10mm.Text, txt_Mc20mm.Text, txt_Mc40mm.Text, txt_waNaturalSand.Text, txt_waCrushedSand.Text, txt_waStoneDust.Text, txt_waGrit.Text, txt_wa10mm.Text, txt_wa20mm.Text, txt_wa40mm.Text, TrialDate, TimeSpan.Parse(Convert.ToDateTime(txt_TrialTime.Text).ToString("HH:mm:ss")), txt_GradeofConcrete.Text, txt_CementUsed.Text, txt_FlyashUsed.Text, "", txt_Natureofwork.Text, txt_Slump.Text, txt_Admixture.Text, Convert.ToDecimal(txt_NoofCubes.Text), false, "", "", "", "", false, "", false, txt_witnessBy.Text, txt_SuperwiseBy.Text, "", txtTotCementitiousMat.Text, txtFlyAshReplacement.Text, "", 0, "", 0, chkValidationFor.Checked, 0, false, false);
                    lbl_TrialId.Text = trialId.ToString(); 
                }

                for (int i = 0; i < grdTrail.Rows.Count; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                    TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");
                    TextBox txt_MatId = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_MatId");

                    TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                    TextBox txt_CorrectionsNew = (TextBox)grdTrail.Rows[i].FindControl("txt_CorrectionsNew");
                    TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].FindControl("txt_TrailActual");

                    if (txt_MatId.Text == "")
                    {
                        txt_MatId.Text = "0";
                    }
                    if (txt_Reqdwt.Text == "")
                    {
                        txt_Reqdwt.Text = "0";
                    }
                    if (txt_Corrections.Text == "")
                    {
                        txt_Corrections.Text = "0";
                    }
                    if (txt_NetWt.Text == "")
                    {
                        txt_NetWt.Text = "0";
                    }
                    if (txt_SpGrav.Text == "")
                    {
                        txt_SpGrav.Text = "0";
                    }
                    if (txt_Volume.Text == "")
                    {
                        txt_Volume.Text = "0";
                    }

                    if (txt_SurfaceMoisture.Text == "")
                    {
                        txt_SurfaceMoisture.Text = "0";
                    }
                    if (txt_CorrectionsNew.Text == "")
                    {
                        txt_CorrectionsNew.Text = "0";
                    }
                    if (txt_TrailActual.Text == "")
                    {
                        txt_TrailActual.Text = "0";
                    }

                    dc.TrailDetail_Update(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, lbl_MaterialName.Text, txt_Trail.Text, Convert.ToDecimal(txt_SpGrav.Text), Convert.ToDecimal(txt_Volume.Text),
                            Convert.ToDecimal(txt_Reqdwt.Text), txt_Corrections.Text, Convert.ToDecimal(txt_NetWt.Text), Convert.ToDecimal(txt_SurfaceMoisture.Text), Convert.ToDecimal(txt_CorrectionsNew.Text), Convert.ToDecimal(txt_TrailActual.Text), false);

                }
                if (lblStatus.Text == "AddNewTrial")
                {
                    //  dc.MFStatus_Update(txt_RefNo.Text, false, 1, false, false, false, false, false, Convert.ToInt32(lbl_MaterialId.Text));
                    dc.MFStatus_Update(txt_RefNo.Text, false, 1, false, false, false, false, false, 0, 0);
                }
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;

                LoadReferenceNoList();
                ddlTrial.Items.Clear();

            }
        }

        protected void Calculation()
        {
            for (int i = 0; i <= grdTrail.Rows.Count - 1; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_SurfaceMoisture");
                if (lbl_MaterialName.Text == "Natural Sand" && txt_McNaturalSand.Visible == true)
                {
                    txt_SurfaceMoisture.Text = (Convert.ToDecimal(txt_McNaturalSand.Text) - Convert.ToDecimal(txt_waNaturalSand.Text)).ToString();
                }
                else if (lbl_MaterialName.Text == "Crushed Sand" && txt_McCrushed.Visible == true)
                {
                    txt_SurfaceMoisture.Text = (Convert.ToDecimal(txt_McCrushed.Text) - Convert.ToDecimal(txt_waCrushedSand.Text)).ToString();
                }
                else if (lbl_MaterialName.Text == "Stone Dust" && txt_McStoneDust.Visible == true)
                {
                    txt_SurfaceMoisture.Text = (Convert.ToDecimal(txt_McStoneDust.Text) - Convert.ToDecimal(txt_waStoneDust.Text)).ToString();
                }
                else if (lbl_MaterialName.Text == "Grit" && txt_McGrit.Visible == true)
                {
                    txt_SurfaceMoisture.Text = (Convert.ToDecimal(txt_McGrit.Text) - Convert.ToDecimal(txt_waGrit.Text)).ToString();
                }
                else if (lbl_MaterialName.Text == "10 mm" && txt_Mc10mm.Visible == true)
                {
                    txt_SurfaceMoisture.Text = (Convert.ToDecimal(txt_Mc10mm.Text) - Convert.ToDecimal(txt_wa10mm.Text)).ToString();
                }
                else if (lbl_MaterialName.Text == "20 mm" && txt_Mc20mm.Visible == true)
                {
                    txt_SurfaceMoisture.Text = (Convert.ToDecimal(txt_Mc20mm.Text) - Convert.ToDecimal(txt_wa20mm.Text)).ToString();
                }
                else if (lbl_MaterialName.Text == "40 mm" && txt_Mc40mm.Visible == true)
                {
                    txt_SurfaceMoisture.Text = (Convert.ToDecimal(txt_Mc40mm.Text) - Convert.ToDecimal(txt_wa40mm.Text)).ToString();
                }
            }
            decimal totReqdWt = 0;
            for (int i = 0; i <= grdTrail.Rows.Count - 1; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");

                if (lbl_MaterialName.Text.Contains("Cement") == true)
                {
                    totReqdWt = totReqdWt + Convert.ToDecimal(txt_Trail.Text);
                }
                else if (lbl_MaterialName.Text.Contains("Fly Ash") == true || lbl_MaterialName.Text.Contains("G G B S") == true ||
                    lbl_MaterialName.Text.Contains("G G B S") == true || lbl_MaterialName.Text.Contains("Micro Silica") == true
                    || lbl_MaterialName.Text.Contains("Metakaolin") == true)
                {
                    totReqdWt = totReqdWt + Convert.ToDecimal(txt_Trail.Text);
                }
                else if (lbl_MaterialName.Text.Contains("W/C Ratio") == true)
                {
                    totReqdWt = totReqdWt * Convert.ToDecimal(txt_Trail.Text);
                }
                else if (lbl_MaterialName.Text.Contains("Water") == true)
                {
                    txt_Trail.Text = totReqdWt.ToString("0");
                }
            }

            decimal[] Col1_WFA = new decimal[grdTrail.Rows.Count - 1];
            decimal[] VFA = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col2_VolM3 = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col3_WtM3 = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col4_ReqWt = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col5_MoistCorr = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col6_NetWt = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col8_CorrReqWt = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col9_NewVol = new decimal[grdTrail.Rows.Count - 1];
            decimal[] Col10_ActualTrial = new decimal[grdTrail.Rows.Count - 1];

            decimal totCemMat = 0, tempWAG = 0, Col2_total = 0, Col3_total = 0, Col4_total = 0, 
                Col5_total = 0, Col6_total = 0, Col9_total = 0, Col10_total = 0;
            tempWAG = 2550;
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].FindControl("txt_Trail");
                if (lbl_MaterialName.Text.Contains("Cement") == true || lbl_MaterialName.Text == "Fly Ash" || lbl_MaterialName.Text == "G G B S"
                    || lbl_MaterialName.Text == "Micro Silica" || lbl_MaterialName.Text == "Metakaolin" || lbl_MaterialName.Text == "Water")
                {
                    tempWAG = tempWAG - Convert.ToDecimal(txt_Trail.Text);
                }
                if (lbl_MaterialName.Text.Contains("Cement") == true ||
                    lbl_MaterialName.Text.Contains("Fly Ash") == true || lbl_MaterialName.Text.Contains("G G B S") == true ||
                    lbl_MaterialName.Text.Contains("G G B S") == true || lbl_MaterialName.Text.Contains("Micro Silica") == true
                    || lbl_MaterialName.Text.Contains("Metakaolin") == true)
                {
                    totCemMat = totCemMat + Convert.ToDecimal(txt_Trail.Text);
                }
            }
            decimal W = 0, FAtotal = 0;
            W = 1000;
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].FindControl("txt_SpGrav");

                decimal SPG = 0;
                if (txt_SpGrav.Text != "")
                {
                    SPG = Convert.ToDecimal(txt_SpGrav.Text);
                }
                if (lbl_MaterialName.Text.Contains("Cement") == true || lbl_MaterialName.Text == "Fly Ash" || lbl_MaterialName.Text == "G G B S"
                    || lbl_MaterialName.Text == "Micro Silica" || lbl_MaterialName.Text == "Metakaolin" )
                {
                    if (SPG != 0)
                        W = W - (Convert.ToDecimal(txt_Trail.Text) / SPG);
                }
                else if (lbl_MaterialName.Text == "Water")
                {
                    W = W - Convert.ToDecimal(txt_Trail.Text) ;
                }
                else if (lbl_MaterialName.Text == "Natural Sand" || lbl_MaterialName.Text == "Crushed Sand" ||
                        lbl_MaterialName.Text == "Stone Dust" || lbl_MaterialName.Text == "Grit" ||
                        lbl_MaterialName.Text == "Mix Aggregate" || lbl_MaterialName.Text == "10 mm" ||
                        lbl_MaterialName.Text == "20 mm" || lbl_MaterialName.Text == "40 mm")
                {
                    if (SPG != 0)
                        FAtotal = FAtotal + ((Convert.ToDecimal(txt_Trail.Text) / 100) / SPG);
                }
            }
            W = W / FAtotal;
            decimal volume = Convert.ToDecimal(0.15 * 0.15 * 0.15 * 1.2) * Convert.ToDecimal(txt_NoofCubes.Text);
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].FindControl("txt_SpGrav");
                TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].FindControl("txt_SurfaceMoisture");
                TextBox txt_CorrectionsNew = (TextBox)grdTrail.Rows[i].FindControl("txt_CorrectionsNew");

                if (lbl_MaterialName.Text != "W/C Ratio" && lbl_MaterialName.Text != "Plastic Density"
                    && lbl_MaterialName.Text != "Total")
                {
                    decimal SPG = 0;
                    if (txt_SpGrav.Text != "")
                    {
                        SPG = Convert.ToDecimal(txt_SpGrav.Text);
                    }
                    if (lbl_MaterialName.Text == "Natural Sand" || lbl_MaterialName.Text == "Crushed Sand" ||
                        lbl_MaterialName.Text == "Stone Dust" || lbl_MaterialName.Text == "Grit" ||
                        lbl_MaterialName.Text == "Mix Aggregate" || lbl_MaterialName.Text == "10 mm" ||
                        lbl_MaterialName.Text == "20 mm" || lbl_MaterialName.Text == "40 mm")
                    {
                        Col2_VolM3[i] = 0;
                        Col3_WtM3[i] = (Convert.ToDecimal(txt_Trail.Text) / 100) * W;
                    }
                    else if (lbl_MaterialName.Text == "Admixture")
                    {
                        if (SPG != 0)
                            Col2_VolM3[i] = ((Convert.ToDecimal(txt_Trail.Text)/100) * totCemMat) / SPG;
                        Col3_WtM3[i] = Col2_VolM3[i] * SPG;
                    }
                    else
                    {
                        Col2_VolM3[i] = Convert.ToDecimal(txt_Trail.Text) / SPG;
                        Col3_WtM3[i] = Col2_VolM3[i] * SPG;
                    }
                    Col4_ReqWt[i] = Col3_WtM3[i] * volume;
                    if (txt_SurfaceMoisture.Text.Trim() != "")
                    {
                        Col5_MoistCorr[i] = Col4_ReqWt[i] * (Convert.ToDecimal(txt_SurfaceMoisture.Text)/100);
                    }
                    if (lbl_MaterialName.Text == "Water")
                    {
                        Col5_MoistCorr[i] = Col5_total * -1;
                    }
                    Col6_NetWt[i] = Col4_ReqWt[i] + Col5_MoistCorr[i];
                    decimal correction = 0;
                    if (txt_CorrectionsNew.Text != "")
                    {
                        correction = Convert.ToDecimal(txt_CorrectionsNew.Text);
                    }
                    Col8_CorrReqWt[i] = Col4_ReqWt[i] + correction;
                    if (SPG != 0)
                        Col9_NewVol[i] = Col8_CorrReqWt[i] / SPG;

                    Col2_total += Col2_VolM3[i];
                    Col3_total += Col3_WtM3[i];
                    Col4_total += Col4_ReqWt[i];
                    Col5_total += Col5_MoistCorr[i];
                    Col6_total += Col6_NetWt[i];
                    if (lbl_MaterialName.Text != "Admixture")
                        Col9_total += Col9_NewVol[i];
                }
            }
            
            decimal wcRatio = 0;
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].FindControl("lbl_MaterialName");
                TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].FindControl("txt_TrailActual");

                if (lbl_MaterialName.Text != "W/C Ratio" && lbl_MaterialName.Text != "Plastic Density"
                    && lbl_MaterialName.Text != "Total") 
                {
                    Col10_ActualTrial[i] = (Col8_CorrReqWt[i] / Col9_total) * 1000;
                    Col10_total += Col10_ActualTrial[i];
                }

                if (lbl_MaterialName.Text.Contains("Cement") == true)
                {
                    wcRatio = wcRatio + Col10_ActualTrial[i];
                }
                else if (lbl_MaterialName.Text.Contains("Fly Ash") == true || lbl_MaterialName.Text.Contains("G G B S") == true ||
                    lbl_MaterialName.Text.Contains("G G B S") == true || lbl_MaterialName.Text.Contains("Micro Silica") == true
                    || lbl_MaterialName.Text.Contains("Metakaolin") == true)
                {
                    wcRatio = wcRatio + Col10_ActualTrial[i];
                }
                else if (lbl_MaterialName.Text.Contains("Water") == true)
                {
                    wcRatio = Col10_ActualTrial[i] / wcRatio;
                }
            }
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].FindControl("txt_Trail");
                TextBox txt_Volume = (TextBox)grdTrail.Rows[i].FindControl("txt_Volume");
                TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].FindControl("txt_Reqdwt");
                TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].FindControl("txt_Corrections");
                TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].FindControl("txt_NetWt");
                TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].FindControl("txt_TrailActual");

                txt_Volume.Text = "0.000";
                txt_Reqdwt.Text = "0.000";
                txt_Corrections.Text = "0.000";
                txt_NetWt.Text = "0.000";
                if (chkValidationFor.Checked == false)
                {
                    txt_TrailActual.Text = "0.000";
                }
                if (lbl_MaterialName.Text == "W/C Ratio")
                {
                    if (chkValidationFor.Checked == false)
                    {
                        txt_TrailActual.Text = wcRatio.ToString("0.00");
                    }
                }
                else if (lbl_MaterialName.Text == "Plastic Density") 
                {
                    if (chkValidationFor.Checked == false)
                    {
                        txt_TrailActual.Text = Col3_total.ToString("0");
                    }
                }
                else if (lbl_MaterialName.Text == "Total")
                {
                    txt_Volume.Text = Col2_total.ToString("0.000");
                    txt_Reqdwt.Text = Col4_total.ToString("0.000");
                    txt_Corrections.Text = Col5_total.ToString("0.000");
                    txt_NetWt.Text = Col6_total.ToString("0.000");
                    if (chkValidationFor.Checked == false)
                    {
                        txt_TrailActual.Text = Col10_total.ToString("0");
                    }
                }
                else 
                {
                    txt_Volume.Text = Col2_VolM3[i].ToString("0.000");
                    txt_Reqdwt.Text = Col4_ReqWt[i].ToString("0.000");
                    txt_Corrections.Text = Col5_MoistCorr[i].ToString("0.000");
                    txt_NetWt.Text = Col6_NetWt[i].ToString("0.000");
                    if (lbl_MaterialName.Text == "Admixture")
                    {
                        if (chkValidationFor.Checked == false)
                        {
                            txt_TrailActual.Text = Col10_ActualTrial[i].ToString("0.000");
                        }
                    }
                    else if (lbl_MaterialName.Text == "W/C Ratio")
                    {
                        if (chkValidationFor.Checked == false)
                        {
                            txt_TrailActual.Text = Col10_ActualTrial[i].ToString("0.00");
                        }
                    }
                    else
                    {
                        if (chkValidationFor.Checked == false)
                        {
                            txt_TrailActual.Text = Col10_ActualTrial[i].ToString("0");
                        }
                    }
                }
            }
        }

        protected void CalculationOld()
        {
            bool FlyAshFlag = false, AdmFlag = false;
            int CemCnt = 0, FlyAshCnt = 0, GGBSCnt = 0, AggCnt = 0, AdmCnt = 0, SilicaCnt = 0, MetakaolinCnt=0;

            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");

                if (lbl_MaterialName.Text.Contains("Cement") == true)
                {
                    CemCnt = i + 1;
                    FlyAshCnt = CemCnt; GGBSCnt = CemCnt; AggCnt = CemCnt; AdmCnt = CemCnt;
                }
                else if (lbl_MaterialName.Text == "Fly Ash")
                {
                    FlyAshCnt = i + 1;
                    FlyAshFlag = true;
                    GGBSCnt = FlyAshCnt; AggCnt = FlyAshCnt; AdmCnt = FlyAshCnt;
                }
                else if (lbl_MaterialName.Text == "G G B S")
                {
                    GGBSCnt = i + 1;
                    AggCnt = GGBSCnt; AdmCnt = GGBSCnt;
                }
                else if (lbl_MaterialName.Text == "Micro Silica")
                {
                    SilicaCnt = i + 1;
                    AggCnt = SilicaCnt;
                    AdmCnt = SilicaCnt;
                }
                else if (lbl_MaterialName.Text == "Metakaolin")
                {
                    MetakaolinCnt = i + 1;
                    AggCnt = MetakaolinCnt;
                    AdmCnt = MetakaolinCnt;
                }
                else if (lbl_MaterialName.Text == "W/C Ratio")
                {
                    AggCnt = i + 1; AdmCnt = i + 1;
                }
                else if (lbl_MaterialName.Text == "Natural Sand" || lbl_MaterialName.Text == "Crushed Sand" ||
                    lbl_MaterialName.Text == "Stone Dust" || lbl_MaterialName.Text == "Grit" ||
                    lbl_MaterialName.Text == "Mix Aggregate" || lbl_MaterialName.Text == "10 mm" ||
                    lbl_MaterialName.Text == "20 mm" || lbl_MaterialName.Text == "40 mm")
                {
                    AggCnt = i + 1;
                    AdmCnt = AggCnt;
                }
                else if (lbl_MaterialName.Text == "Admixture")
                {
                    AdmCnt = i + 1;
                    AdmFlag = true;
                }

            }
            double TempVal = 0, TotCementitiousMat, FlyAshReplacement, fly_ash_cont = 0;
            if (double.TryParse(txtTotCementitiousMat.Text, out TotCementitiousMat) &&
                double.TryParse(txtFlyAshReplacement.Text, out FlyAshReplacement))
            {
                fly_ash_cont = Convert.ToDouble(txtTotCementitiousMat.Text) * (Convert.ToDouble(txtFlyAshReplacement.Text) / 100);
                for (int i = 0; i < grdTrail.Rows.Count; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    if (lbl_MaterialName.Text == "Fly Ash")
                    {
                        txt_Trail.Text = fly_ash_cont.ToString();
                    }
                }
                for (int i = 0; i < grdTrail.Rows.Count; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    if (lbl_MaterialName.Text.Contains("Cement") == true)
                    {
                        txt_Trail.Text = (Convert.ToDouble(txtTotCementitiousMat.Text) - fly_ash_cont).ToString();
                        break;
                    }
                }
            }
            double tot = 0, totReqdWt = 0; //, totFlyAsh = 0;
            for (int i = 0; i <= AggCnt; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");

                if (lbl_MaterialName.Text.Contains("mm") == true || lbl_MaterialName.Text.ToLower().Contains("sand") == true ||
                    lbl_MaterialName.Text.Contains("Stone Dust") == true || lbl_MaterialName.Text.Contains("Grit") == true)
                {
                    tot = tot + Convert.ToDouble(txt_Trail.Text);
                }
                if (lbl_MaterialName.Text.Contains("Cement") == true)
                {
                    totReqdWt = totReqdWt + Convert.ToDouble(txt_Trail.Text);
                    TempVal = i;
                }
                else if (lbl_MaterialName.Text.Contains("Fly Ash") == true || lbl_MaterialName.Text.Contains("G G B S") == true ||
                    lbl_MaterialName.Text.Contains("G G B S") == true ||  lbl_MaterialName.Text.Contains("Micro Silica") == true
                    || lbl_MaterialName.Text.Contains("Metakaolin") == true )
                {
                    totReqdWt = totReqdWt + Convert.ToDouble(txt_Trail.Text);
                }
                else if (lbl_MaterialName.Text.Contains("W/C Ratio") == true)
                {
                    totReqdWt = totReqdWt * Convert.ToDouble(txt_Trail.Text);
                    TextBox txt_TrailAggtCnt = (TextBox)grdTrail.Rows[AggCnt].Cells[1].FindControl("txt_Trail");
                    txt_TrailAggtCnt.Text = totReqdWt.ToString("0");
                }
            }
            TextBox txt_Trail_lastRow = (TextBox)grdTrail.Rows[grdTrail.Rows.Count - 1].Cells[1].FindControl("txt_Trail");
            txt_Trail_lastRow.Text = tot.ToString();
            double totOtherVol = 0, mPlasticDensity = 0, totVolume = 0;
            totReqdWt = 0;
            double Trail, SpGrav;
            for (int i = 0; i <= grdTrail.Rows.Count - 1; i++)
            {
                totVolume = 0;
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");

                if ((lbl_MaterialName.Text.Contains("Cement") == true || lbl_MaterialName.Text.Contains("Water") == true ||
                    lbl_MaterialName.Text.Contains("Fly Ash") == true || lbl_MaterialName.Text.Contains("Admixture") == true ||
                    lbl_MaterialName.Text.Contains("G G B S") == true || lbl_MaterialName.Text.Contains("Micro Silica") == true || lbl_MaterialName.Text.Contains("Metakaolin") == true) &&
                    double.TryParse(txt_Trail.Text, out Trail) &&
                        double.TryParse(txt_SpGrav.Text, out SpGrav) && Convert.ToDouble(txt_SpGrav.Text) > 0)
                {
                    if (lbl_MaterialName.Text.Contains("Admixture") == true)
                    {
                        totVolume = (Convert.ToDouble(txt_Trail.Text) / 50) * Convert.ToDouble(txt_Trail.Text) / (Convert.ToDouble(txt_SpGrav.Text) * 1000) / 1000;
                        totVolume = Convert.ToDouble(FormatNumber(totVolume, 4));
                    }
                    else
                    {
                        totVolume = Convert.ToDouble(txt_Trail.Text) / (Convert.ToDouble(txt_SpGrav.Text) * 1000);
                    }
                    txt_Volume.Text = FormatNumber(totVolume, 3);
                    totOtherVol = totOtherVol + Convert.ToDouble(FormatNumber(totVolume, 3));
                    if (lbl_MaterialName.Text.Contains("Cement") == true || lbl_MaterialName.Text.Contains("Water") == true ||
                        lbl_MaterialName.Text.Contains("Fly Ash") == true ||
                        lbl_MaterialName.Text.Contains("G G B S") == true || lbl_MaterialName.Text.Contains("Micro Silica") == true || lbl_MaterialName.Text.Contains("Metakaolin") == true)
                    {
                        mPlasticDensity = mPlasticDensity + Convert.ToDouble(txt_Trail.Text);
                    }
                }
                else if (lbl_MaterialName.Text.Contains("Admixture") == true)
                {
                    TextBox txt_Trail_temp = (TextBox)grdTrail.Rows[Convert.ToInt32(TempVal)].Cells[1].FindControl("txt_Trail");
                    totReqdWt = Convert.ToDouble(txt_Trail_temp.Text) / 50 * Convert.ToInt32(txt_Trail.Text.Replace(",", ""));
                    txt_Volume.Text = FormatNumber(totReqdWt, 3);
                }
            }
            int plden = 0;
            double totAggtVol;
            totAggtVol = 1 - totOtherVol;
            totAggtVol = Convert.ToDouble(FormatNumber(totAggtVol, 3)); // cement volume.
            // aggregate volume calculations
            for (int i = 0; i <= grdTrail.Rows.Count - 1; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");

                if ((lbl_MaterialName.Text.Contains("Natural Sand") == true || lbl_MaterialName.Text.Contains("Crushed Sand") == true ||
                    lbl_MaterialName.Text.Contains("Stone Dust") == true || lbl_MaterialName.Text.Contains("Grit") == true ||
                        lbl_MaterialName.Text.Contains("Mix Aggregate") == true || lbl_MaterialName.Text.Contains("10 mm") == true ||
                        lbl_MaterialName.Text.Contains("20 mm") == true || lbl_MaterialName.Text.Contains("40 mm") == true) &&
                    double.TryParse(txt_Trail.Text, out Trail) &&
                        double.TryParse(txt_SpGrav.Text, out SpGrav))
                {
                    totVolume = totAggtVol * (Convert.ToDouble(txt_Trail.Text) / 100) * Convert.ToDouble(txt_SpGrav.Text) * 1000;
                    txt_Volume.Text = FormatNumber(totVolume, 3);
                    mPlasticDensity = mPlasticDensity + totVolume;
                }
                if (lbl_MaterialName.Text.Contains("Plastic Density"))
                    plden = i;

            }
            TextBox txt_Trail_plden = (TextBox)grdTrail.Rows[plden].Cells[1].FindControl("txt_Trail");
            txt_Trail_plden.Text = Convert.ToInt32(mPlasticDensity).ToString();
            // old calculations

            double d2, d3, cemcon, CemCont;
            d2 = (0.15 * 0.15 * 0.15) * Convert.ToDouble(txt_NoofCubes.Text);
            d3 = (d2 * 0.2) + d2;

            TextBox txt_Trail1 = (TextBox)grdTrail.Rows[0].Cells[1].FindControl("txt_Trail");
            cemcon = (Convert.ToDouble(txt_Trail1.Text)) * d3;
            CemCont = Convert.ToDouble(txt_Trail1.Text) / 50;

            for (int i = 0; i <= CemCnt - 1; i++)
            {
                TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");

                txt_Reqdwt.Text = FormatNumber(cemcon, 3);
                txt_Corrections.Text = "Nil";
                txt_NetWt.Text = FormatNumber(cemcon, 3);
                totReqdWt = totReqdWt + cemcon;
            }

            double flyashcal = 0, wat_to_be_add = 0, WC = 0, without;
            int[] Fly_Ash_By_Min_Arr = new int[10];
            int[] Wat_To_Be_Add_Arr = new int[10];
            if (FlyAshFlag == true)
            {
                for (int i = CemCnt; i <= FlyAshCnt - 1; i++)
                {
                    TextBox txt_Trail2 = (TextBox)grdTrail.Rows[1].Cells[1].FindControl("txt_Trail");
                    TextBox txt_Trail_FlyAshCnt = (TextBox)grdTrail.Rows[FlyAshCnt].Cells[1].FindControl("txt_Trail");

                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");

                    flyashcal = Convert.ToDouble(txt_Trail2.Text);
                    fly_ash_cont = flyashcal * d3;
                    fly_ash_cont = Convert.ToDouble(FormatNumber(fly_ash_cont, 3));

                    if (fly_ash_cont > 0)
                    {
                        //cemtcon/
                        if (CemCont > 0)
                            Fly_Ash_By_Min_Arr[i] = Convert.ToInt32(Convert.ToInt32(flyashcal / CemCont).ToString().Replace(",", ""));
                        else
                            Fly_Ash_By_Min_Arr[i] = 0;
                    }
                    wat_to_be_add = (50 + (flyashcal / CemCont)) * Convert.ToDouble(txt_Trail_FlyAshCnt.Text);
                    Wat_To_Be_Add_Arr[i] = Convert.ToInt32(Convert.ToString(Convert.ToInt32(wat_to_be_add)).Replace(",", ""));
                    flyashcal = fly_ash_cont;

                    txt_Reqdwt.Text = FormatNumber(fly_ash_cont, 3);
                    txt_Corrections.Text = "Nil";
                    txt_NetWt.Text = FormatNumber(fly_ash_cont, 3);
                    WC = Convert.ToDouble(txt_Trail_FlyAshCnt.Text);
                    WC = WC * (cemcon + flyashcal);
                    without = d3 - cemcon - WC - flyashcal;
                }
            }

            double ggbscal = 0, ggbs_cont, wat_to_be_add1;
            int[] GGBS_By_Min_Arr = new int[10];
            if (GGBSCnt > 0)
            {
                for (int i = GGBSCnt - 1; i < GGBSCnt; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");

                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");

                    if (lbl_MaterialName.Text.ToLower().Contains("g g b s") == true || lbl_MaterialName.Text.ToLower().Contains("silica") == true)
                    {
                        ggbscal = Convert.ToDouble(txt_Trail.Text);
                        ggbs_cont = ggbscal * d3;
                        ggbs_cont = Convert.ToDouble(FormatNumber(ggbs_cont, 3));
                        if (ggbs_cont > 0)
                        {
                            //cemtcon/
                            if (CemCont > 0)
                                GGBS_By_Min_Arr[i] = Convert.ToInt32(Convert.ToInt32(ggbscal / CemCont).ToString().Replace(",", ""));
                            else
                                GGBS_By_Min_Arr[i] = 0;
                        }
                        wat_to_be_add1 = (50 + (ggbscal / CemCont)) * Convert.ToDouble(txt_Trail.Text);
                        Wat_To_Be_Add_Arr[i] = Convert.ToInt32(Convert.ToInt32(wat_to_be_add).ToString().Replace(",", ""));
                        ggbscal = ggbs_cont;
                        txt_Reqdwt.Text = FormatNumber(ggbs_cont, 3);
                        txt_Corrections.Text = "Nil";
                        txt_NetWt.Text = FormatNumber(ggbs_cont, 3);
                        WC = Convert.ToDouble(txt_Trail.Text);
                        WC = WC * (cemcon + ggbscal);
                        without = d3 - cemcon - WC - ggbscal;
                    }
                }
            }

            double silicacal = 0, silica_cont=0,  wat_to_be_add2=0;
            int[] Silica_By_Min_Arr = new int[10];
            if (SilicaCnt > 0)
            {
                for (int i = SilicaCnt - 1; i < SilicaCnt; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");

                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");

                    if (lbl_MaterialName.Text.ToLower().Contains("silica") == true)
                    {
                        silicacal = Convert.ToDouble(txt_Trail.Text);
                        silica_cont = silicacal * d3;
                        silica_cont = Convert.ToDouble(FormatNumber(silica_cont, 3));
                        if (silica_cont > 0)
                        {
                            //cemtcon/
                            if (CemCont > 0)
                                Silica_By_Min_Arr[i] = Convert.ToInt32(Convert.ToInt32(silicacal / CemCont).ToString().Replace(",", ""));
                            else
                                Silica_By_Min_Arr[i] = 0;
                        }
                        wat_to_be_add2 = (50 + (silicacal / CemCont)) * Convert.ToDouble(txt_Trail.Text);
                        Wat_To_Be_Add_Arr[i] = Convert.ToInt32(Convert.ToInt32(wat_to_be_add).ToString().Replace(",", ""));
                        silicacal = silica_cont;
                        txt_Reqdwt.Text = FormatNumber(silica_cont, 3);
                        txt_Corrections.Text = "Nil";
                        txt_NetWt.Text = FormatNumber(silica_cont, 3);
                        WC = Convert.ToDouble(txt_Trail.Text);
                        WC = WC * (cemcon + silicacal);
                        without = d3 - cemcon - WC - silicacal;
                    }
                }
            }
            //
            double metakaolincal = 0, metakaolin_cont = 0,  wat_to_be_add3=0;
            int[] metakaolin_By_Min_Arr = new int[10];
            if (MetakaolinCnt > 0)
            {
                for (int i = MetakaolinCnt - 1; i < MetakaolinCnt; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");

                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");

                    if (lbl_MaterialName.Text.ToLower().Contains("metakaolin") == true)
                    {
                        metakaolincal = Convert.ToDouble(txt_Trail.Text);
                        metakaolin_cont = metakaolincal * d3;
                        metakaolin_cont = Convert.ToDouble(FormatNumber(metakaolin_cont, 3));
                        if (metakaolin_cont > 0)
                        {
                            //cemtcon/
                            if (CemCont > 0)
                                metakaolin_By_Min_Arr[i] = Convert.ToInt32(Convert.ToInt32(metakaolincal / CemCont).ToString().Replace(",", ""));
                            else
                                metakaolin_By_Min_Arr[i] = 0;
                        }
                        wat_to_be_add3 = (50 + (metakaolincal / CemCont)) * Convert.ToDouble(txt_Trail.Text);
                        Wat_To_Be_Add_Arr[i] = Convert.ToInt32(Convert.ToInt32(wat_to_be_add).ToString().Replace(",", ""));
                        metakaolincal = metakaolin_cont;
                        txt_Reqdwt.Text = FormatNumber(metakaolin_cont, 3);
                        txt_Corrections.Text = "Nil";
                        txt_NetWt.Text = FormatNumber(metakaolin_cont, 3);
                        WC = Convert.ToDouble(txt_Trail.Text);
                        WC = WC * (cemcon + metakaolincal);
                        without = d3 - cemcon - WC - metakaolincal;
                    }
                }
            }

            if (FlyAshFlag == false &&  GGBSCnt == 0  )
            {
                TextBox txt_Trail2 = (TextBox)grdTrail.Rows[2].Cells[1].FindControl("txt_Trail");
                WC = Convert.ToDouble(txt_Trail2.Text);
                WC = WC * cemcon;
                without = d3 - cemcon - WC;
            }
            else
            {
                for (int i = 3; i <= grdTrail.Rows.Count - 2; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    if (lbl_MaterialName.Text.ToLower().Contains("water") == true)
                    {
                        WC = Convert.ToDouble(txt_Trail.Text);
                        WC = WC * d3;
                        break;
                    }
                }
            }

            // adm req wt not to be added in calculations            
            double admwt;
            if (AdmFlag == true)
            {
                for (int i = AggCnt + 1; i <= AdmCnt - 1; i++)
                {
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    
                    TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                    TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                    TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");

                    if (FlyAshFlag == true)
                        admwt = (Convert.ToDouble(txt_Trail.Text) * (cemcon + flyashcal +ggbscal+silicacal+metakaolincal  )) / 50;
                    else
                        admwt = (Convert.ToDouble(txt_Trail.Text) * (cemcon +ggbscal+silicacal+metakaolincal)) / 50;

                    txt_Reqdwt.Text = FormatNumber(admwt, 3);
                    txt_Corrections.Text = "Nil";
                    txt_NetWt.Text = FormatNumber(admwt, 3);
                }
            }

            TextBox txt_Reqdwt_AggCnt = (TextBox)grdTrail.Rows[AggCnt].Cells[4].FindControl("txt_Reqdwt");
            TextBox txt_Corrections_AggCnt = (TextBox)grdTrail.Rows[AggCnt].Cells[5].FindControl("txt_Corrections");
            TextBox txt_NetWt_AggCnt = (TextBox)grdTrail.Rows[AggCnt].Cells[6].FindControl("txt_NetWt");

            WC = Convert.ToDouble(FormatNumber(WC, 3));
            txt_Reqdwt_AggCnt.Text = WC.ToString();
            txt_Corrections_AggCnt.Text = "Nil";
            txt_NetWt_AggCnt.Text = WC.ToString();
            totReqdWt = totReqdWt + WC;
            double fa1, NSMoisture = 0, ca1,  totalMoisture = 0;
            for (int i = FlyAshCnt + 1; i <= AggCnt; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");

                #region old calculation
                //if (lbl_MaterialName.Text.Contains("Natural Sand") == true)
                //{
                //    fa1 = Convert.ToDouble(txt_Volume.Text);
                //    fa1 = fa1 * d3;
                //    txt_Reqdwt.Text = FormatNumber(fa1, 3);
                //    txt_Corrections.Text = "Nil";
                //    totReqdWt = totReqdWt + fa1;

                //    if (txt_McNaturalSand.Text != "" && txt_waNaturalSand.Text != "")
                //    {
                //        NSMoisture = Convert.ToDouble(txt_McNaturalSand.Text) - Convert.ToDouble(txt_waNaturalSand.Text);
                //        NSMoisture = (fa1 * NSMoisture) / 100;
                //        NSMoisture = Convert.ToDouble(FormatNumber(NSMoisture * (-1), 3));
                //    }
                //    if (NSMoisture < 0)
                //    {
                //        fa1 = fa1 + (NSMoisture * (-1));
                //        txt_Corrections.Text = "+" + (NSMoisture * (-1)).ToString();
                //    }
                //    else
                //    {
                //        //RWT.TextMatrix(i, 1) = "Nil"
                //        fa1 = fa1 + (NSMoisture * (-1));
                //        txt_Corrections.Text = "- " + NSMoisture.ToString();
                //    }
                //    txt_NetWt.Text = FormatNumber(fa1, 3);
                //    WC = WC + NSMoisture;
                //}
                //if (lbl_MaterialName.Text.Contains("Crushed Sand") == true || lbl_MaterialName.Text.Contains("Stone Dust") == true
                //    || lbl_MaterialName.Text.Contains("Grit") == true)
                //{
                //    fa2 = Convert.ToDouble(txt_Volume.Text);
                //    fa2 = fa2 * d3;
                //    txt_Reqdwt.Text = FormatNumber(fa2, 3);
                //    totReqdWt = totReqdWt + fa2;

                //    if (txt_McCrushed.Text != "" && txt_waCrushedSand.Text != "")
                //    {
                //        CSMoisture = Convert.ToDouble(txt_McCrushed.Text) - Convert.ToDouble(txt_waCrushedSand.Text);
                //        CSMoisture = (fa2 * CSMoisture) / 100;
                //        CSMoisture = Convert.ToDouble(FormatNumber(CSMoisture * (-1), 3));
                //    }
                //    if (CSMoisture < 0)
                //    {
                //        fa2 = fa2 + ((-1) * CSMoisture);
                //        txt_Corrections.Text = "+" + (CSMoisture * (-1)).ToString();
                //    }
                //    else
                //    {
                //        //RWT.TextMatrix(i, 1) = "Nil"
                //        fa2 = fa2 + ((-1) * CSMoisture);
                //        txt_Corrections.Text = "- " + CSMoisture.ToString();
                //    }
                //    txt_NetWt.Text = FormatNumber(fa2, 3);
                //    WC = WC + CSMoisture;
                //}

                //TempVal = Convert.ToDouble(FormatNumber(NSMoisture + CSMoisture, 3));

                //if (TempVal > 0)
                //    txt_Corrections_AggCnt.Text = "+ " + TempVal.ToString();
                //else
                //    txt_Corrections_AggCnt.Text = TempVal.ToString();

                //if (lbl_MaterialName.Text.Contains("10 mm") == true)
                //{
                //    ca1 = Convert.ToDouble(txt_Volume.Text);
                //    ca1 = ca1 * d3;
                //    txt_Reqdwt.Text = FormatNumber(ca1, 3);
                //    txt_Corrections.Text = "Nil";
                //    txt_NetWt.Text = FormatNumber(ca1, 3);
                //    totReqdWt = totReqdWt + ca1;
                //}

                //if (lbl_MaterialName.Text.Contains("20 mm") == true || lbl_MaterialName.Text.Contains("40 mm") == true
                //    || lbl_MaterialName.Text.Contains("Mix Aggregate") == true)
                //{
                //    ca2 = Convert.ToDouble(txt_Volume.Text);
                //    ca2 = ca2 * d3;
                //    txt_Reqdwt.Text = FormatNumber(ca2, 3);
                //    txt_Corrections.Text = "Nil";
                //    txt_NetWt.Text = FormatNumber(ca2, 3);
                //    totReqdWt = totReqdWt + ca2;
                //}
                #endregion
                NSMoisture = 0;
                if (lbl_MaterialName.Text.Contains("Natural Sand") == true || lbl_MaterialName.Text.Contains("Crushed Sand") == true 
                    || lbl_MaterialName.Text.Contains("Stone Dust") == true || lbl_MaterialName.Text.Contains("Grit") == true
                     || lbl_MaterialName.Text.Contains("10 mm") == true || lbl_MaterialName.Text.Contains("20 mm") == true || lbl_MaterialName.Text.Contains("40 mm") == true)
                {

                    fa1 = Convert.ToDouble(txt_Volume.Text);
                    fa1 = fa1 * d3;
                    txt_Reqdwt.Text = FormatNumber(fa1, 3);
                    txt_Corrections.Text = "Nil";
                    totReqdWt = totReqdWt + fa1;
                    if (lbl_MaterialName.Text.Contains("Natural Sand") == true)
                    {
                        if (txt_McNaturalSand.Text != "" && txt_waNaturalSand.Text != "")
                        {
                            NSMoisture = Convert.ToDouble(txt_McNaturalSand.Text) - Convert.ToDouble(txt_waNaturalSand.Text);
                        }
                    }
                    else if (lbl_MaterialName.Text.Contains("Crushed Sand") == true || lbl_MaterialName.Text.Contains("Stone Dust") == true
                        || lbl_MaterialName.Text.Contains("Grit") == true)
                    {
                        if (txt_McCrushed.Text != "" && txt_waCrushedSand.Text != "")
                        {
                            NSMoisture = Convert.ToDouble(txt_McCrushed.Text) - Convert.ToDouble(txt_waCrushedSand.Text);
                        }
                    }
                    else if (lbl_MaterialName.Text.Contains("10 mm") == true)
                    {
                        if (txt_Mc10mm.Text != "" && txt_wa10mm.Text != "")
                        {
                            NSMoisture = Convert.ToDouble(txt_Mc10mm.Text) - Convert.ToDouble(txt_wa10mm.Text);
                        }
                    }
                    else if (lbl_MaterialName.Text.Contains("20 mm") == true)
                    {
                        if (txt_Mc20mm.Text != "" && txt_wa20mm.Text != "")
                        {
                            NSMoisture = Convert.ToDouble(txt_Mc20mm.Text) - Convert.ToDouble(txt_wa20mm.Text);
                        }
                    }
                    else if (lbl_MaterialName.Text.Contains("40 mm") == true)
                    {
                        if (txt_Mc40mm.Text != "" && txt_wa40mm.Text != "")
                        {
                            NSMoisture = Convert.ToDouble(txt_Mc40mm.Text) - Convert.ToDouble(txt_wa40mm.Text);
                        }
                    }
                    NSMoisture = (fa1 * NSMoisture) / 100;
                    NSMoisture = Convert.ToDouble(FormatNumber(NSMoisture * (-1), 3));
                    if (NSMoisture < 0)
                    {
                        fa1 = fa1 + (NSMoisture * (-1));
                        txt_Corrections.Text = "+" + (NSMoisture * (-1)).ToString();
                    }
                    else
                    {
                        fa1 = fa1 + (NSMoisture * (-1));
                        txt_Corrections.Text = "- " + NSMoisture.ToString();
                    }
                    txt_NetWt.Text = FormatNumber(fa1, 3);
                    WC = WC + NSMoisture;

                    totalMoisture += NSMoisture;
                    TempVal = Convert.ToDouble(FormatNumber(totalMoisture, 3));

                    if (TempVal > 0)
                        txt_Corrections_AggCnt.Text = "+ " + TempVal.ToString();
                    else
                        txt_Corrections_AggCnt.Text = TempVal.ToString();
                }
                

                //if (lbl_MaterialName.Text.Contains("10 mm") == true || lbl_MaterialName.Text.Contains("20 mm") == true
                //    || lbl_MaterialName.Text.Contains("40 mm") == true || lbl_MaterialName.Text.Contains("Mix Aggregate") == true)
                if (lbl_MaterialName.Text.Contains("Mix Aggregate") == true)
                {
                    ca1 = Convert.ToDouble(txt_Volume.Text);
                    ca1 = ca1 * d3;
                    txt_Reqdwt.Text = FormatNumber(ca1, 3);
                    txt_Corrections.Text = "Nil";
                    txt_NetWt.Text = FormatNumber(ca1, 3);
                    totReqdWt = totReqdWt + ca1;
                }
            }

            //TextBox txt_NetWt_AggCnt = (TextBox)grdTrail.Rows[AggCnt].Cells[6].FindControl("txt_SpGrav");
            TextBox txt_Reqdwt_rowcntminus1 = (TextBox)grdTrail.Rows[grdTrail.Rows.Count - 1].Cells[4].FindControl("txt_Reqdwt");

            txt_NetWt_AggCnt.Text = FormatNumber(WC, 3);
            txt_Reqdwt_rowcntminus1.Text = FormatNumber(totReqdWt, 3);
        }

        string FormatNumber<T>(T number, int maxDecimals = 4)
        {
            string x = Regex.Replace(String.Format("{0:n" + maxDecimals + "}", number), @"[" + System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");
            if (x == "")
                x = "0";
            return x;
        }

        protected void Lnk_Calculate_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
            }
        }

        protected void lnksieveprnt_Click(object sender, EventArgs e)
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //PrintHTMLReport rptHtml = new PrintHTMLReport();
            //reportStr = rptHtml.TrialSieveAnalysis_Html(txt_RefNo.Text, txt_RecType.Text);
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            //PrintHTMLReport rptHtml = new PrintHTMLReport();
            //rptHtml.TrialSieveAnalysis_Html(txt_RefNo.Text, txt_RecType.Text);
            PrintPDFReport rpt = new PrintPDFReport();
            //rptPdf.MFSieveAnalysis_PDF(txt_RefNo.Text, txt_RecType.Text, "Print");
            rpt.PrintSelectedReport(txt_RecType.Text, txt_RefNo.Text, "Print", "", "", "Sieve Analysis", "", "", "", "");
        }

        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TrialDate = DateTime.ParseExact(txt_TrialDate.Text, "dd/MM/yyyy", null);
            DateTime CurrentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);

            decimal Total = 0;
            
            //var mdlStatus = dc.MixDesign_View(txt_RefNo.Text, 1);
            //if (mdlStatus.FirstOrDefault().MFINWD_Status_tint > 1)
            //{
            //    lblMsg.Text = "Mix design letter has been entered, can not update trial details. ";
            //    valid = false;
            //}
            if (TrialDate > CurrentDate1)
            {
                lblMsg.Text = "Trial Date should be less than or equal to the Current Date.";
                txt_TrialDate.Focus();
                valid = false;
            }
            else if (txt_TrialTime.Text == "")
            {
                lblMsg.Text = "Enter Trail Time ";
                txt_TrialTime.Focus();
                valid = false;
            }
            else if (txt_CementUsed.Text == "")
            {
                lblMsg.Text = " Enter Cement Used ";
                txt_CementUsed.Focus();
                valid = false;
            }
            else if (txt_Admixture.Text == "")
            {
                lblMsg.Text = " Enter Admixture Used ";
                txt_Admixture.Focus();
                valid = false;
            }
            else if (txt_FlyashUsed.Text == "")
            {
                lblMsg.Text = " Enter Fly Ash Used ";
                txt_FlyashUsed.Focus();
                valid = false;
            }
            else if (txt_NoofCubes.Text == "")
            {
                lblMsg.Text = " Enter No of Cubes ";
                txt_NoofCubes.Focus();
                valid = false;
            }
            else if (txtTotCementitiousMat.Visible == true && txtTotCementitiousMat.Text == "")
            {
                lblMsg.Text = " Enter Total Cementitious Material ";
                txtTotCementitiousMat.Focus();
                valid = false;
            }
            else if (txtFlyAshReplacement.Visible == true && txtFlyAshReplacement.Text == "")
            {
                lblMsg.Text = " Enter Fly Ash Replacement ";
                txtFlyAshReplacement.Focus();
                valid = false;
            }
            else if (txt_McNaturalSand.Visible == true && txt_McNaturalSand.Text  == "")
            {
                lblMsg.Text = " Enter Total Moisture for Natural Sand ";
                txt_McNaturalSand.Focus();
                valid = false;
            }
            else if (txt_waNaturalSand.Visible == true && txt_waNaturalSand.Text == "")
            {
                lblMsg.Text = " Enter Water Absoption for Natural Sand ";
                txt_waNaturalSand.Focus();
                valid = false;
            }
            else if (txt_McCrushed.Visible == true && txt_McCrushed.Text == "")
            {
                lblMsg.Text = " Enter Total Moisture for Crushed Sand ";
                txt_McCrushed.Focus();
                valid = false;
            }
            else if (txt_waCrushedSand.Visible == true && txt_waCrushedSand.Text == "")
            {
                lblMsg.Text = " Enter Water Absoption for Crushed Sand ";
                txt_waCrushedSand.Focus();
                valid = false;
            }
            else if (txt_McStoneDust.Visible == true && txt_McStoneDust.Text == "")
            {
                lblMsg.Text = " Enter Total Moisture for Stone Dust ";
                txt_McStoneDust.Focus();
                valid = false;
            }
            else if (txt_waStoneDust.Visible == true && txt_waStoneDust.Text == "")
            {
                lblMsg.Text = " Enter Water Absoption for Stone Dust ";
                txt_waStoneDust.Focus();
                valid = false;
            }
            else if (txt_McGrit.Visible == true && txt_McGrit.Text == "")
            {
                lblMsg.Text = " Enter Total Moisture for Grit ";
                txt_McGrit.Focus();
                valid = false;
            }
            else if (txt_waGrit.Visible == true && txt_waGrit.Text == "")
            {
                lblMsg.Text = " Enter Water Absoption for Grit ";
                txt_waGrit.Focus();
                valid = false;
            }
            else if (txt_Mc10mm.Visible == true && txt_Mc10mm.Text == "")
            {
                lblMsg.Text = " Enter Total Moisture for 10 mm ";
                txt_Mc10mm.Focus();
                valid = false;
            }
            else if (txt_wa10mm.Visible == true && txt_wa10mm.Text == "")
            {
                lblMsg.Text = " Enter Water Absoption for 10 mm ";
                txt_wa10mm.Focus();
                valid = false;
            }
            else if (txt_Mc20mm.Visible == true && txt_Mc20mm.Text == "")
            {
                lblMsg.Text = " Enter Total Moisture for 20 mm ";
                txt_Mc20mm.Focus();
                valid = false;
            }
            else if (txt_wa20mm.Visible == true && txt_wa20mm.Text == "")
            {
                lblMsg.Text = " Enter Water Absoption for 20 mm ";
                txt_wa20mm.Focus();
                valid = false;
            }
            else if (txt_Mc40mm.Visible == true && txt_Mc40mm.Text == "")
            {
                lblMsg.Text = " Enter Total Moisture for 40 mm ";
                txt_Mc40mm.Focus();
                valid = false;
            }
            else if (txt_wa40mm.Visible == true && txt_wa40mm.Text == "")
            {
                lblMsg.Text = " Enter Water Absoption for 40 mm ";
                txt_wa40mm.Focus();
                valid = false;
            }
            else if (txt_SuperwiseBy.Text.Trim() == "")
            {
                lblMsg.Text = " Enter Supervise By ";
                txt_SuperwiseBy.Focus();
                valid = false;
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdTrail.Rows.Count; i++)
                {
                    Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");

                    if (txt_Trail.Text == "" && lbl_MaterialName.Text != "Total" && lbl_MaterialName.Text.Trim() != "Plastic Density" && lbl_MaterialName.Text != "Water")
                    {
                        lblMsg.Text = "Enter Weight for " + lbl_MaterialName.Text;
                        txt_Trail.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_SpGrav.Text == "" && lbl_MaterialName.Text != "Total" && lbl_MaterialName.Text.Trim() != "Plastic Density" && lbl_MaterialName.Text.Trim() != "W/C Ratio" && lbl_MaterialName.Text.Trim() != "Admixture")
                    {
                        lblMsg.Text = "Enter Specific Gravity for " + lbl_MaterialName.Text;
                        txt_SpGrav.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_SpGrav.Text != "" && lbl_MaterialName.Text.Trim() == "Admixture" && (Convert.ToDecimal(txt_SpGrav.Text) < Convert.ToDecimal("0.95") || Convert.ToDecimal(txt_SpGrav.Text) > Convert.ToDecimal("1.15")))
                    {
                        lblMsg.Text = "Enter Specific Gravity for " + lbl_MaterialName.Text + " between 0.95 to 1.15 ";
                        txt_SpGrav.Focus();
                        valid = false;
                        break;
                    }
                    if (lbl_MaterialName.Text.Trim() == "Natural Sand" || lbl_MaterialName.Text.Trim() == "Crushed Sand" || lbl_MaterialName.Text.Trim() == "Stone Dust" || lbl_MaterialName.Text.Trim() == "10 mm" || lbl_MaterialName.Text.Trim() == "20 mm" || lbl_MaterialName.Text.Trim() == "40 mm" || lbl_MaterialName.Text.Trim() == "Grit")
                    {
                        Total += Convert.ToDecimal(txt_Trail.Text);
                    }
                }
                if (Total < 100 && valid == true)
                {
                    lblMsg.Text = "Total Trial of Aggregate material should be greater than 100 ";
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
                if (Total > 100)
                {
                    Calculation();
                }
            }
            return valid;
        }

        protected void AddRowTrail()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["TrailTable"] != null)
            {
                GetCurrentDataTrail();
                dt = (DataTable)ViewState["TrailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lbl_MaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_TrailUnit", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_TrailUnit2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TrailSystem", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Trail", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_SpGrav", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Volume", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Reqdwt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Corrections", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NetWt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_MatId", typeof(string)));

                dt.Columns.Add(new DataColumn("txt_SurfaceMoisture", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CorrectionsNew", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TrailActual", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lbl_MaterialName"] = string.Empty;
            dr["lbl_TrailUnit"] = string.Empty;
            dr["lbl_TrailUnit2"] = string.Empty;
            dr["txt_TrailSystem"] = string.Empty;
            dr["txt_Trail"] = string.Empty;
            dr["txt_SpGrav"] = string.Empty;
            dr["txt_Volume"] = string.Empty;
            dr["txt_Reqdwt"] = string.Empty;
            dr["txt_Corrections"] = string.Empty;
            dr["txt_NetWt"] = string.Empty;
            dr["txt_MatId"] = string.Empty;

            dr["txt_SurfaceMoisture"] = string.Empty;
            dr["txt_CorrectionsNew"] = string.Empty;
            dr["txt_TrailActual"] = string.Empty;
            
            dt.Rows.Add(dr);
            ViewState["TrailTable"] = dt;
            grdTrail.DataSource = dt;
            grdTrail.DataBind();
            SetPreviousDataTrail();
        }

        protected void GetCurrentDataTrail()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lbl_MaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_TrailUnit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_TrailUnit2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TrailSystem", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Trail", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_SpGrav", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Volume", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Reqdwt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Corrections", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NetWt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_MatId", typeof(string)));

            dtTable.Columns.Add(new DataColumn("txt_SurfaceMoisture", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CorrectionsNew", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TrailActual", typeof(string)));
            for (int i = 0; i < grdTrail.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                Label lbl_TrailUnit = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit");
                Label lbl_TrailUnit2 = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit2");
                TextBox txt_TrailSystem = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_TrailSystem");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");
                TextBox txt_MatId = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_MatId");

                TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_SurfaceMoisture");
                TextBox txt_CorrectionsNew = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_CorrectionsNew");
                TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_TrailActual");

                drRow = dtTable.NewRow();
                drRow["lbl_MaterialName"] = lbl_MaterialName.Text;
                drRow["lbl_TrailUnit"] = lbl_TrailUnit.Text;
                drRow["lbl_TrailUnit2"] = lbl_TrailUnit2.Text;
                drRow["txt_TrailSystem"] = txt_TrailSystem.Text;
                drRow["txt_Trail"] = txt_Trail.Text;
                drRow["txt_SpGrav"] = txt_SpGrav.Text;
                drRow["txt_Volume"] = txt_Volume.Text;
                drRow["txt_Reqdwt"] = txt_Reqdwt.Text;
                drRow["txt_Corrections"] = txt_Corrections.Text;
                drRow["txt_NetWt"] = txt_NetWt.Text;
                drRow["txt_MatId"] = txt_MatId.Text;

                drRow["txt_SurfaceMoisture"] = txt_SurfaceMoisture.Text;
                drRow["txt_CorrectionsNew"] = txt_CorrectionsNew.Text;
                drRow["txt_TrailActual"] = txt_TrailActual.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["TrailTable"] = dtTable;

        }

        protected void SetPreviousDataTrail()
        {
            DataTable dt = (DataTable)ViewState["TrailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                Label lbl_TrailUnit = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit");
                Label lbl_TrailUnit2 = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_TrailUnit2");
                TextBox txt_TrailSystem = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_TrailSystem");
                TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                TextBox txt_Reqdwt = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_Reqdwt");
                TextBox txt_Corrections = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_Corrections");
                TextBox txt_NetWt = (TextBox)grdTrail.Rows[i].Cells[6].FindControl("txt_NetWt");
                TextBox txt_MatId = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_MatId");
                
                TextBox txt_SurfaceMoisture = (TextBox)grdTrail.Rows[i].Cells[4].FindControl("txt_SurfaceMoisture");
                TextBox txt_CorrectionsNew = (TextBox)grdTrail.Rows[i].Cells[5].FindControl("txt_CorrectionsNew");
                TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_TrailActual");

                lbl_MaterialName.Text = dt.Rows[i]["lbl_MaterialName"].ToString();
                lbl_TrailUnit.Text = dt.Rows[i]["lbl_TrailUnit"].ToString();
                lbl_TrailUnit2.Text = dt.Rows[i]["lbl_TrailUnit2"].ToString();
                txt_TrailSystem.Text = dt.Rows[i]["txt_TrailSystem"].ToString();
                txt_Trail.Text = dt.Rows[i]["txt_Trail"].ToString();
                txt_SpGrav.Text = dt.Rows[i]["txt_SpGrav"].ToString();
                txt_Volume.Text = dt.Rows[i]["txt_Volume"].ToString();
                txt_Reqdwt.Text = dt.Rows[i]["txt_Reqdwt"].ToString();
                txt_Corrections.Text = dt.Rows[i]["txt_Corrections"].ToString();
                txt_NetWt.Text = dt.Rows[i]["txt_NetWt"].ToString();
                txt_MatId.Text = dt.Rows[i]["txt_MatId"].ToString();

                txt_SurfaceMoisture.Text = dt.Rows[i]["txt_SurfaceMoisture"].ToString();
                txt_CorrectionsNew.Text = dt.Rows[i]["txt_CorrectionsNew"].ToString();
                txt_TrailActual.Text = dt.Rows[i]["txt_TrailActual"].ToString();
                if (lbl_MaterialName.Text == "Plastic Density")
                {
                    grdTrail.Rows[i].Visible = false;
                }
                if (lbl_MaterialName.Text != "Admixture")
                {
                    txt_SpGrav.ReadOnly = true;
                    txt_SpGrav.CssClass = "caltextbox";
                }
                if (lbl_MaterialName.Text == "W/C Ratio")
                {
                    txt_CorrectionsNew.ReadOnly = true;
                    txt_CorrectionsNew.CssClass = "caltextbox";
                }
                if (lbl_MaterialName.Text == "Admixture")
                {
                    lbl_TrailUnit2.Visible = true;
                }
                if (chkValidationFor.Checked == true)
                {
                    txt_TrailActual.CssClass = "";
                    txt_TrailActual.ReadOnly = false;
                }
                else
                {
                    txt_TrailActual.CssClass = "caltextbox";
                    txt_TrailActual.ReadOnly = true;
                }
            }
        }

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

            var mixdesign = dc.MixDesign_View(txt_RefNo.Text, 1);
            foreach (var m in mixdesign)
            {
                Label lbl_Name = (Label)grdSlump.Rows[0].FindControl("lbl_Name");
                if (m.TEST_Sr_No == 2 || m.TEST_Sr_No == 6) //self compacting
                {
                    lbl_Name.Text = "Flow (mm)";
                }
                else
                {
                    lbl_Name.Text = "Slump (mm)";
                }
            }
        }

        protected void AddRowTestSchedule()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lbl_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("chk_1daySch", typeof(string)));
            dt.Columns.Add(new DataColumn("chk_3daySch", typeof(string)));
            dt.Columns.Add(new DataColumn("chk_7daySch", typeof(string)));
            dt.Columns.Add(new DataColumn("chk_28daySch", typeof(string)));
            dt.Columns.Add(new DataColumn("chk_56daySch", typeof(string)));
            dt.Columns.Add(new DataColumn("chk_90daySch", typeof(string)));
            dt.Columns.Add(new DataColumn("chk_OtherdaySch", typeof(string)));

            dt.Columns.Add(new DataColumn("lbl_1dayStr", typeof(string)));            
            dt.Columns.Add(new DataColumn("lbl_3dayStr", typeof(string)));            
            dt.Columns.Add(new DataColumn("lbl_7dayStr", typeof(string)));            
            dt.Columns.Add(new DataColumn("lbl_28dayStr", typeof(string)));            
            dt.Columns.Add(new DataColumn("lbl_56dayStr", typeof(string)));            
            dt.Columns.Add(new DataColumn("lbl_90dayStr", typeof(string)));
            dt.Columns.Add(new DataColumn("lbl_OtherdayStr", typeof(string)));

            dt.Columns.Add(new DataColumn("lbl_3dayExp28DaysCompStr", typeof(string)));
            dt.Columns.Add(new DataColumn("lbl_7dayExp28DaysCompStr", typeof(string)));
            dt.Columns.Add(new DataColumn("lbl_3dayTargetExp28DaysCompStr", typeof(string)));
            dt.Columns.Add(new DataColumn("lbl_7dayTargetExp28DaysCompStr", typeof(string)));
            dt.Columns.Add(new DataColumn("lbl_3dayTargetMeanStr", typeof(string)));
            dt.Columns.Add(new DataColumn("lbl_7dayTargetMeanStr", typeof(string)));

            dt.Columns.Add(new DataColumn("txt_1dayStr", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_3dayStr", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_7dayStr", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_28dayStr", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_56dayStr", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_90dayStr", typeof(string)));
            dt.Columns.Add(new DataColumn("txt_OtherdayStr", typeof(string)));

            dr = dt.NewRow();
            dr["lbl_Name"] = string.Empty;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["lbl_Name"] = string.Empty;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["lbl_Name"] = string.Empty;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["lbl_Name"] = string.Empty;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["lbl_Name"] = string.Empty;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["lbl_Name"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["TestScheduleTable"] = dt;
            grdTestSchedule.DataSource = dt;
            grdTestSchedule.DataBind();

            for (int i = 0; i <= 5; i++)
            {
                Label lbl_Name = (Label)grdTestSchedule.Rows[i].FindControl("lbl_Name");
                CheckBox chk_1daySch = (CheckBox)grdTestSchedule.Rows[i].FindControl("chk_1daySch");
                CheckBox chk_3daySch = (CheckBox)grdTestSchedule.Rows[i].FindControl("chk_3daySch");
                CheckBox chk_7daySch = (CheckBox)grdTestSchedule.Rows[i].FindControl("chk_7daySch");
                CheckBox chk_28daySch = (CheckBox)grdTestSchedule.Rows[i].FindControl("chk_28daySch");
                CheckBox chk_56daySch = (CheckBox)grdTestSchedule.Rows[i].FindControl("chk_56daySch");
                CheckBox chk_90daySch = (CheckBox)grdTestSchedule.Rows[i].FindControl("chk_90daySch");
                CheckBox chk_OtherdaySch = (CheckBox)grdTestSchedule.Rows[i].FindControl("chk_OtherdaySch");
                Label lbl_1dayStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_1dayStr");
                Label lbl_3dayStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_3dayStr");
                Label lbl_7dayStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_7dayStr");
                Label lbl_28dayStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_28dayStr");
                Label lbl_56dayStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_56dayStr");
                Label lbl_90dayStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_90dayStr");
                Label lbl_OtherdayStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_OtherdayStr");
                Label lbl_3dayExp28DaysCompStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_3dayExp28DaysCompStr");
                Label lbl_7dayExp28DaysCompStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_7dayExp28DaysCompStr");
                Label lbl_3dayTargetExp28DaysCompStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_3dayTargetExp28DaysCompStr");
                Label lbl_7dayTargetExp28DaysCompStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_7dayTargetExp28DaysCompStr");
                Label lbl_3dayTargetMeanStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_3dayTargetMeanStr");
                Label lbl_7dayTargetMeanStr = (Label)grdTestSchedule.Rows[i].FindControl("lbl_7dayTargetMeanStr");
                TextBox txt_1dayStr = (TextBox)grdTestSchedule.Rows[i].FindControl("txt_1dayStr");
                TextBox txt_3dayStr = (TextBox)grdTestSchedule.Rows[i].FindControl("txt_3dayStr");
                TextBox txt_7dayStr = (TextBox)grdTestSchedule.Rows[i].FindControl("txt_7dayStr");
                TextBox txt_28dayStr = (TextBox)grdTestSchedule.Rows[i].FindControl("txt_28dayStr");
                TextBox txt_56dayStr = (TextBox)grdTestSchedule.Rows[i].FindControl("txt_56dayStr");
                TextBox txt_90dayStr = (TextBox)grdTestSchedule.Rows[i].FindControl("txt_90dayStr");
                TextBox txt_OtherdayStr = (TextBox)grdTestSchedule.Rows[i].FindControl("txt_OtherdayStr");

                if (i == 0)
                {
                    lbl_Name.Text = "Select";
                    lbl_1dayStr.Visible = false;
                    lbl_3dayStr.Visible = false;
                    lbl_7dayStr.Visible = false;
                    lbl_28dayStr.Visible = false;
                    lbl_56dayStr.Visible = false;
                    lbl_90dayStr.Visible = false;
                    lbl_OtherdayStr.Visible = false;
                    chk_1daySch.Checked = false;
                    chk_3daySch.Checked = false;
                    chk_7daySch.Checked = false;
                    chk_28daySch.Checked = false;
                    chk_56daySch.Checked = false;
                    chk_90daySch.Checked = false;
                    chk_OtherdaySch.Checked = false;
                    lbl_3dayExp28DaysCompStr.Visible = false;
                    lbl_7dayExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetExp28DaysCompStr.Visible = false;
                    lbl_7dayTargetExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetMeanStr.Visible = false;
                    lbl_7dayTargetMeanStr.Visible = false;
                    txt_1dayStr.Visible = false;
                    txt_3dayStr.Visible = false;
                    txt_7dayStr.Visible = false;
                    txt_28dayStr.Visible = false;
                    txt_56dayStr.Visible = false;
                    txt_90dayStr.Visible = false;
                    txt_OtherdayStr.Visible = false;
                }
                else if (i == 1)
                {
                    lbl_Name.Text = "Avg Strength";
                    chk_1daySch.Visible = false;
                    chk_3daySch.Visible = false;
                    chk_7daySch.Visible = false;
                    chk_28daySch.Visible = false;
                    chk_56daySch.Visible = false;
                    chk_90daySch.Visible = false;
                    chk_OtherdaySch.Visible = false;
                    lbl_1dayStr.Text = "";
                    lbl_3dayStr.Text = "";
                    lbl_7dayStr.Text = "";
                    lbl_28dayStr.Text = "";
                    lbl_56dayStr.Text = "";
                    lbl_90dayStr.Text = "";
                    lbl_OtherdayStr.Text = "";
                    lbl_3dayExp28DaysCompStr.Text = "";
                    lbl_7dayExp28DaysCompStr.Text = "";
                    lbl_3dayTargetExp28DaysCompStr.Text = "";
                    lbl_7dayTargetExp28DaysCompStr.Text = "";
                    lbl_3dayTargetMeanStr.Text = "";
                    lbl_7dayTargetMeanStr.Text = "";
                    lbl_3dayExp28DaysCompStr.Visible = false;
                    lbl_7dayExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetExp28DaysCompStr.Visible = false;
                    lbl_7dayTargetExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetMeanStr.Visible = false;
                    lbl_7dayTargetMeanStr.Visible = false;
                    txt_1dayStr.Visible = false;
                    txt_3dayStr.Visible = false;
                    txt_7dayStr.Visible = false;
                    txt_28dayStr.Visible = false;
                    txt_56dayStr.Visible = false;
                    txt_90dayStr.Visible = false;
                    txt_OtherdayStr.Visible = false;
                }
                else if (i == 2)
                {
                    lbl_Name.Text = "Expected 28 days Comp Strength";
                    chk_1daySch.Visible = false;
                    chk_3daySch.Visible = false;
                    chk_7daySch.Visible = false;
                    chk_28daySch.Visible = false;
                    chk_56daySch.Visible = false;
                    chk_90daySch.Visible = false;
                    chk_OtherdaySch.Visible = false;
                    lbl_1dayStr.Text = "";
                    lbl_3dayStr.Text = "";
                    lbl_7dayStr.Text = "";
                    lbl_28dayStr.Text = "";
                    lbl_56dayStr.Text = "";
                    lbl_90dayStr.Text = "";
                    lbl_OtherdayStr.Text = "";
                    lbl_3dayExp28DaysCompStr.Text = "";
                    lbl_7dayExp28DaysCompStr.Text = "";
                    lbl_3dayTargetExp28DaysCompStr.Text = "";
                    lbl_7dayTargetExp28DaysCompStr.Text = "";
                    lbl_3dayTargetMeanStr.Text = "";
                    lbl_7dayTargetMeanStr.Text = "";

                    lbl_1dayStr.Visible = false;
                    lbl_3dayStr.Visible = false;
                    lbl_7dayStr.Visible = false;
                    lbl_28dayStr.Visible = false;
                    lbl_56dayStr.Visible = false;
                    lbl_90dayStr.Visible = false;
                    lbl_OtherdayStr.Visible = false;
                    lbl_3dayTargetExp28DaysCompStr.Visible = false;
                    lbl_7dayTargetExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetMeanStr.Visible = false;
                    lbl_7dayTargetMeanStr.Visible = false;
                    txt_1dayStr.Visible = false;
                    txt_3dayStr.Visible = false;
                    txt_7dayStr.Visible = false;
                    txt_28dayStr.Visible = false;
                    txt_56dayStr.Visible = false;
                    txt_90dayStr.Visible = false;
                    txt_OtherdayStr.Visible = false;
                }
                else if (i == 3)
                {
                    lbl_Name.Text = "Target Expected 28 days Comp Strength";
                    chk_1daySch.Visible = false;
                    chk_3daySch.Visible = false;
                    chk_7daySch.Visible = false;
                    chk_28daySch.Visible = false;
                    chk_56daySch.Visible = false;
                    chk_90daySch.Visible = false;
                    chk_OtherdaySch.Visible = false;
                    lbl_1dayStr.Text = "";
                    lbl_3dayStr.Text = "";
                    lbl_7dayStr.Text = "";
                    lbl_28dayStr.Text = "";
                    lbl_56dayStr.Text = "";
                    lbl_90dayStr.Text = "";
                    lbl_OtherdayStr.Text = "";
                    lbl_3dayExp28DaysCompStr.Text = "";
                    lbl_7dayExp28DaysCompStr.Text = "";
                    lbl_3dayTargetExp28DaysCompStr.Text = "";
                    lbl_7dayTargetExp28DaysCompStr.Text = "";
                    lbl_3dayTargetMeanStr.Text = "";
                    lbl_7dayTargetMeanStr.Text = "";

                    lbl_1dayStr.Visible = false;
                    lbl_3dayStr.Visible = false;
                    lbl_7dayStr.Visible = false;
                    lbl_28dayStr.Visible = false;
                    lbl_56dayStr.Visible = false;
                    lbl_90dayStr.Visible = false;
                    lbl_OtherdayStr.Visible = false;
                    lbl_3dayExp28DaysCompStr.Visible = false;
                    lbl_7dayExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetMeanStr.Visible = false;
                    lbl_7dayTargetMeanStr.Visible = false;
                    txt_1dayStr.Visible = false;
                    txt_3dayStr.Visible = false;
                    txt_7dayStr.Visible = false;
                    txt_28dayStr.Visible = false;
                    txt_56dayStr.Visible = false;
                    txt_90dayStr.Visible = false;
                    txt_OtherdayStr.Visible = false;
                }
                else if (i == 4)
                {
                    lbl_Name.Text = "Target Mean Strength";
                    chk_1daySch.Visible = false;
                    chk_3daySch.Visible = false;
                    chk_7daySch.Visible = false;
                    chk_28daySch.Visible = false;
                    chk_56daySch.Visible = false;
                    chk_90daySch.Visible = false;
                    chk_OtherdaySch.Visible = false;
                    lbl_1dayStr.Text = "";
                    lbl_3dayStr.Text = "";
                    lbl_7dayStr.Text = "";
                    lbl_28dayStr.Text = "";
                    lbl_56dayStr.Text = "";
                    lbl_90dayStr.Text = "";
                    lbl_OtherdayStr.Text = "";
                    lbl_3dayExp28DaysCompStr.Text = "";
                    lbl_7dayExp28DaysCompStr.Text = "";
                    lbl_3dayTargetExp28DaysCompStr.Text = "";
                    lbl_7dayTargetExp28DaysCompStr.Text = "";
                    lbl_3dayTargetMeanStr.Text = "";
                    lbl_7dayTargetMeanStr.Text = "";

                    lbl_1dayStr.Visible = false;
                    lbl_3dayStr.Visible = false;
                    lbl_7dayStr.Visible = false;
                    lbl_28dayStr.Visible = false;
                    lbl_56dayStr.Visible = false;
                    lbl_90dayStr.Visible = false;
                    lbl_OtherdayStr.Visible = false;
                    lbl_3dayExp28DaysCompStr.Visible = false;
                    lbl_7dayExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetExp28DaysCompStr.Visible = false;
                    lbl_7dayTargetExp28DaysCompStr.Visible = false;
                    txt_1dayStr.Visible = false;
                    txt_3dayStr.Visible = false;
                    txt_7dayStr.Visible = false;
                    txt_28dayStr.Visible = false;
                    txt_56dayStr.Visible = false;
                    txt_90dayStr.Visible = false;
                    txt_OtherdayStr.Visible = false;
                }
                else if (i == 5)
                {
                    lbl_Name.Text = "Curing Tank Detail";
                    chk_1daySch.Visible = false;
                    chk_3daySch.Visible = false;
                    chk_7daySch.Visible = false;
                    chk_28daySch.Visible = false;
                    chk_56daySch.Visible = false;
                    chk_90daySch.Visible = false;
                    chk_OtherdaySch.Visible = false;
                    lbl_1dayStr.Text = "";
                    lbl_3dayStr.Text = "";
                    lbl_7dayStr.Text = "";
                    lbl_28dayStr.Text = "";
                    lbl_56dayStr.Text = "";
                    lbl_90dayStr.Text = "";
                    lbl_OtherdayStr.Text = "";
                    lbl_3dayExp28DaysCompStr.Text = "";
                    lbl_7dayExp28DaysCompStr.Text = "";
                    lbl_3dayTargetExp28DaysCompStr.Text = "";
                    lbl_7dayTargetExp28DaysCompStr.Text = "";
                    lbl_3dayTargetMeanStr.Text = "";
                    lbl_7dayTargetMeanStr.Text = "";

                    lbl_1dayStr.Visible = false;
                    lbl_3dayStr.Visible = false;
                    lbl_7dayStr.Visible = false;
                    lbl_28dayStr.Visible = false;
                    lbl_56dayStr.Visible = false;
                    lbl_90dayStr.Visible = false;
                    lbl_OtherdayStr.Visible = false;
                    lbl_3dayExp28DaysCompStr.Visible = false;
                    lbl_7dayExp28DaysCompStr.Visible = false;
                    lbl_3dayTargetMeanStr.Visible = false;
                    lbl_7dayTargetMeanStr.Visible = false;

                  
                }
            }
        }

        protected void CalculateExp28DaysCompStrength()
        {
            string mGrade = "";
            int temprature = 0;
            decimal cement = 0, flyash = 0, flyashPercent = 0, ggbs = 0, ggbsPercent = 0, wcRatio = 0, admixture = 0;
            var MFDtls = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, txt_RefNo.Text, 0, 0, 0).ToList();
            if (MFDtls.Count() > 0)
            {
                mGrade = MFDtls.FirstOrDefault().MFINWD_Grade_var;
            }
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text)).ToList();
            if (data.FirstOrDefault().TrialDetail_ActualTrial != null)
            {
                foreach (var t in data)
                {
                    int monthOfCasting = Convert.ToInt32(Convert.ToDateTime(t.Trial_Date).ToString("MM"));
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
                    //break;

                    if (t.TrialDetail_MaterialName == "Cement")
                    {
                        cement = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                    }
                    else if (t.TrialDetail_MaterialName == "Fly Ash")
                    {
                        flyash = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        flyashPercent = flyash / (cement + flyash);
                    }
                    else if (t.TrialDetail_MaterialName == "G G B S")
                    {
                        ggbs = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                        ggbsPercent = ggbs / (cement + ggbs);
                    }
                    else if (t.TrialDetail_MaterialName == "W/C Ratio")
                    {
                        wcRatio = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                    }
                    else if (t.TrialDetail_MaterialName == "Admixture")
                    {
                        admixture = Convert.ToDecimal(t.TrialDetail_ActualTrial);
                    }
                }

                //for (int i = 0; i <= grdTestSchedule.Columns.Count - 1; i++)
                //{
                decimal exp28daysStrength_3day = 0, TargetExp28daysStrength_3day_yellow = 0, exp28daysStrength_7day = 0, TargetExp28daysStrength_7day_yellow = 0, TargetExp28daysStrength_3day_green = 0, TargetExp28daysStrength_7day_green = 0;
                if (Convert.ToDecimal(mGrade.Replace("M", "")) <= 15)
                {
                    TargetExp28daysStrength_3day_yellow = Convert.ToDecimal(Convert.ToDecimal(mGrade.Replace("M", "")) + Convert.ToDecimal(1.65 * 3.5));
                }
                else if (Convert.ToDecimal(mGrade.Replace("M", "")) >= 20 && Convert.ToDecimal(mGrade.Replace("M", "")) <= 25)
                {
                    TargetExp28daysStrength_3day_yellow = Convert.ToDecimal(Convert.ToDecimal(mGrade.Replace("M", "")) + Convert.ToDecimal(1.65 * 4));
                }
                else if (Convert.ToDecimal(mGrade.Replace("M", "")) >= 30)
                {
                    TargetExp28daysStrength_3day_yellow = Convert.ToDecimal(Convert.ToDecimal(mGrade.Replace("M", "")) + Convert.ToDecimal(1.65 * 5));
                }
                TargetExp28daysStrength_7day_yellow = TargetExp28daysStrength_3day_yellow;
                TargetExp28daysStrength_3day_green = TargetExp28daysStrength_3day_yellow;
                TargetExp28daysStrength_7day_green = TargetExp28daysStrength_3day_yellow;

                decimal K = 0;
                CheckBox chk_3daySch = (CheckBox)grdTestSchedule.Rows[0].Cells[2].FindControl("chk_3daySch");
                Label lbl_3dayStr = (Label)grdTestSchedule.Rows[1].Cells[2].FindControl("lbl_3dayStr");
                Label lbl_3dayExp28DaysCompStr = (Label)grdTestSchedule.Rows[2].Cells[2].FindControl("lbl_3dayExp28DaysCompStr");
                Label lbl_3dayTargetExp28DaysCompStr = (Label)grdTestSchedule.Rows[3].Cells[2].FindControl("lbl_3dayTargetExp28DaysCompStr");
                Label lbl_3dayTargetMeanStr = (Label)grdTestSchedule.Rows[4].Cells[2].FindControl("lbl_3dayTargetMeanStr");

                CheckBox chk_7daySch = (CheckBox)grdTestSchedule.Rows[0].Cells[2].FindControl("chk_7daySch");
                Label lbl_7dayStr = (Label)grdTestSchedule.Rows[1].Cells[2].FindControl("lbl_7dayStr");
                Label lbl_7dayExp28DaysCompStr = (Label)grdTestSchedule.Rows[2].Cells[2].FindControl("lbl_7dayExp28DaysCompStr");
                Label lbl_7dayTargetExp28DaysCompStr = (Label)grdTestSchedule.Rows[3].Cells[2].FindControl("lbl_7dayTargetExp28DaysCompStr");
                Label lbl_7dayTargetMeanStr = (Label)grdTestSchedule.Rows[4].Cells[2].FindControl("lbl_7dayTargetMeanStr");

                //TextBox txt_AvgCompStr = (TextBox)grdTestSchedule.Rows[i].Cells[3].FindControl("txt_AvgCompStr");
                //TextBox txt_Exp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Exp28DaysCompStr");
                //TextBox txt_TargetExp28DaysCompStr = (TextBox)grdCubeCasting.Rows[i].Cells[5].FindControl("txt_TargetExp28DaysCompStr");
                //Label lbl_StandardError = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_StandardError");
                //Label lbl_RValue = (Label)grdCubeCasting.Rows[i].Cells[5].FindControl("lbl_RValue");

                if (flyash > 0 && ggbs == 0)
                {
                    //3days  
                    if (lbl_3dayStr.Text != "")
                    {
                        K = Convert.ToDecimal(11);
                        exp28daysStrength_3day = K;
                        exp28daysStrength_3day += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.514);
                        exp28daysStrength_3day += cement * Convert.ToDecimal(0.024);
                        exp28daysStrength_3day += flyashPercent * Convert.ToDecimal(7.168);
                        exp28daysStrength_3day += wcRatio * Convert.ToDecimal(-8.062);
                        //exp28daysStrength += admixture * Convert.ToDecimal(0.121);
                        //exp28daysStrength += temprature * Convert.ToDecimal(-0.029);
                        exp28daysStrength_3day += Convert.ToDecimal(lbl_3dayStr.Text) * Convert.ToDecimal(0.541);
                        lbl_3dayExp28DaysCompStr.Text = exp28daysStrength_3day.ToString("0.00");
                        //TargetExp28daysStrength_3day_yellow += Convert.ToDecimal(3.773 * 1.96 / 2);
                        TargetExp28daysStrength_3day_green += Convert.ToDecimal(3.773);
                        lbl_3dayTargetExp28DaysCompStr.Text = TargetExp28daysStrength_3day_green.ToString("0.00");
                        lbl_3dayTargetMeanStr.Text = TargetExp28daysStrength_3day_yellow.ToString("0.00");
                        //lbl_StandardError.Text = "3.773";
                        //lbl_RValue.Text = "0.92";
                    }
                    //7days
                    if (lbl_7dayStr.Text != "")
                    {
                        K = Convert.ToDecimal(11.693);
                        exp28daysStrength_7day = K;
                        exp28daysStrength_7day += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.534);
                        //exp28daysStrength += cement * Convert.ToDecimal(0.016);
                        //exp28daysStrength += flyash * Convert.ToDecimal(-0.05);
                        //exp28daysStrength += flyashPercent * Convert.ToDecimal(22.653);
                        exp28daysStrength_7day += wcRatio * Convert.ToDecimal(-1.966);
                        //exp28daysStrength += admixture * Convert.ToDecimal(0.11);
                        //exp28daysStrength += temprature * Convert.ToDecimal(-0.026);
                        exp28daysStrength_7day += Convert.ToDecimal(lbl_7dayStr.Text) * Convert.ToDecimal(0.539);
                        lbl_7dayExp28DaysCompStr.Text = exp28daysStrength_7day.ToString("0.00");
                        //TargetExp28daysStrength_7day_yellow += Convert.ToDecimal(3.54 * 1.96 / 2);
                        TargetExp28daysStrength_7day_green += Convert.ToDecimal(3.54);
                        lbl_7dayTargetExp28DaysCompStr.Text = TargetExp28daysStrength_7day_green.ToString("0.00");
                        lbl_7dayTargetMeanStr.Text = TargetExp28daysStrength_7day_yellow.ToString("0.00");
                        //lbl_StandardError.Text = "3.54";
                        //lbl_RValue.Text = "0.932";
                    }

                }
                else if (flyash == 0 && ggbs > 0)
                {
                    //3days 
                    if (lbl_3dayStr.Text != "")
                    {
                        K = Convert.ToDecimal(55.023);
                        exp28daysStrength_3day = K;
                        exp28daysStrength_3day += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.262);
                        exp28daysStrength_3day += ggbs * Convert.ToDecimal(-0.03);
                        //exp28daysStrength += ggbsPercent * Convert.ToDecimal(7.168);
                        exp28daysStrength_3day += wcRatio * Convert.ToDecimal(-52.82);
                        //exp28daysStrength += admixture * Convert.ToDecimal(0.121);
                        //exp28daysStrength += temprature * Convert.ToDecimal(-0.029);
                        exp28daysStrength_3day += Convert.ToDecimal(lbl_3dayStr.Text) * Convert.ToDecimal(0.423);
                        lbl_3dayExp28DaysCompStr.Text = exp28daysStrength_3day.ToString("0.00");
                        //TargetExp28daysStrength_3day_yellow += Convert.ToDecimal(4.209 * 1.96 / 2);
                        TargetExp28daysStrength_3day_green += Convert.ToDecimal(4.209);
                        lbl_3dayTargetExp28DaysCompStr.Text = TargetExp28daysStrength_3day_green.ToString("0.00");
                        lbl_3dayTargetMeanStr.Text = TargetExp28daysStrength_3day_yellow.ToString("0.00");
                        //lbl_StandardError.Text = "4.209";
                        //lbl_RValue.Text = "0.871";
                    }
                    //7days
                    if (lbl_7dayStr.Text != "")
                    {
                        K = Convert.ToDecimal(62.596);
                        exp28daysStrength_7day = K;
                        exp28daysStrength_7day += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.16);
                        exp28daysStrength_7day += cement * Convert.ToDecimal(-0.014);
                        exp28daysStrength_7day += ggbs * Convert.ToDecimal(-0.028);
                        exp28daysStrength_7day += ggbsPercent * Convert.ToDecimal(-14.652);
                        exp28daysStrength_7day += wcRatio * Convert.ToDecimal(-53.894);
                        exp28daysStrength_7day += admixture * Convert.ToDecimal(-1.076);
                        exp28daysStrength_7day += temprature * Convert.ToDecimal(-0.052);
                        exp28daysStrength_7day += Convert.ToDecimal(lbl_7dayStr.Text) * Convert.ToDecimal(0.563);
                        lbl_7dayExp28DaysCompStr.Text = exp28daysStrength_7day.ToString("0.00");
                        //TargetExp28daysStrength_7day_yellow += Convert.ToDecimal(3.664 * 1.96 / 2);
                        TargetExp28daysStrength_7day_green += Convert.ToDecimal(3.664);
                        lbl_7dayTargetExp28DaysCompStr.Text = TargetExp28daysStrength_7day_green.ToString("0.00");
                        lbl_7dayTargetMeanStr.Text = TargetExp28daysStrength_7day_yellow.ToString("0.00");
                        //lbl_StandardError.Text = "3.664";
                        //lbl_RValue.Text = "0.92";
                    }
                }
                else
                {
                    //3days
                    if (lbl_3dayStr.Text != "")
                    {
                        K = Convert.ToDecimal(23.464);
                        exp28daysStrength_3day = K;
                        exp28daysStrength_3day += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.445);
                        exp28daysStrength_3day += cement * Convert.ToDecimal(0.013);
                        exp28daysStrength_3day += wcRatio * Convert.ToDecimal(-19.92);
                        exp28daysStrength_3day += admixture * Convert.ToDecimal(0.121);
                        exp28daysStrength_3day += temprature * Convert.ToDecimal(-0.029);
                        exp28daysStrength_3day += Convert.ToDecimal(lbl_3dayStr.Text) * Convert.ToDecimal(0.533);
                        lbl_3dayExp28DaysCompStr.Text = exp28daysStrength_3day.ToString("0.00");
                        //TargetExp28daysStrength_3day_yellow += Convert.ToDecimal(3.767 * 1.96 / 2);
                        TargetExp28daysStrength_3day_green += Convert.ToDecimal(3.767);
                        lbl_3dayTargetExp28DaysCompStr.Text = TargetExp28daysStrength_3day_green.ToString("0.00");
                        lbl_3dayTargetMeanStr.Text = TargetExp28daysStrength_3day_yellow.ToString("0.00");
                        //lbl_StandardError.Text = "3.767";
                        //lbl_RValue.Text = "0.92";
                    }
                    //7days
                    if (lbl_7dayStr.Text != "")
                    {
                        K = Convert.ToDecimal(33.074);
                        exp28daysStrength_7day = K;
                        exp28daysStrength_7day += Convert.ToDecimal(mGrade.Replace("M", "")) * Convert.ToDecimal(0.298);
                        exp28daysStrength_7day += cement * Convert.ToDecimal(-0.001);
                        exp28daysStrength_7day += wcRatio * Convert.ToDecimal(-30.514);
                        exp28daysStrength_7day += admixture * Convert.ToDecimal(0.025);
                        exp28daysStrength_7day += temprature * Convert.ToDecimal(-0.009);
                        exp28daysStrength_7day += Convert.ToDecimal(lbl_7dayStr.Text) * Convert.ToDecimal(0.535);
                        lbl_7dayExp28DaysCompStr.Text = exp28daysStrength_7day.ToString("0.00");
                        //TargetExp28daysStrength_7day_yellow += Convert.ToDecimal(3.468 * 1.96 / 2);
                        TargetExp28daysStrength_7day_green += Convert.ToDecimal(3.468);
                        lbl_7dayTargetExp28DaysCompStr.Text = TargetExp28daysStrength_7day_green.ToString("0.00");
                        lbl_7dayTargetMeanStr.Text = TargetExp28daysStrength_7day_yellow.ToString("0.00");
                        //lbl_StandardError.Text = "3.468";
                        //lbl_RValue.Text = "0.932";
                    }
                }

                if (lbl_3dayStr.Text != "")
                {
                    if (exp28daysStrength_3day >= TargetExp28daysStrength_3day_green)
                    {
                        grdTestSchedule.Rows[0].Cells[2].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[1].Cells[2].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[2].Cells[2].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[3].Cells[2].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[4].Cells[2].BackColor = System.Drawing.Color.LightGreen;
                        chk_3daySch.BackColor = System.Drawing.Color.LightGreen;
                        lbl_3dayStr.BackColor = System.Drawing.Color.LightGreen;
                        lbl_3dayExp28DaysCompStr.BackColor = System.Drawing.Color.LightGreen;
                        lbl_3dayTargetExp28DaysCompStr.BackColor = System.Drawing.Color.LightGreen;
                        lbl_3dayTargetMeanStr.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (exp28daysStrength_3day >= TargetExp28daysStrength_3day_yellow)
                    {
                        grdTestSchedule.Rows[0].Cells[2].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[1].Cells[2].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[2].Cells[2].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[3].Cells[2].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[4].Cells[2].BackColor = System.Drawing.Color.Yellow;
                        chk_3daySch.BackColor = System.Drawing.Color.Yellow;
                        lbl_3dayStr.BackColor = System.Drawing.Color.Yellow;
                        lbl_3dayExp28DaysCompStr.BackColor = System.Drawing.Color.Yellow;
                        lbl_3dayTargetExp28DaysCompStr.BackColor = System.Drawing.Color.Yellow;
                        lbl_3dayTargetMeanStr.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        grdTestSchedule.Rows[0].Cells[2].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[1].Cells[2].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[2].Cells[2].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[3].Cells[2].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[4].Cells[2].BackColor = System.Drawing.Color.Red;
                        chk_3daySch.BackColor = System.Drawing.Color.Red;
                        lbl_3dayStr.BackColor = System.Drawing.Color.Red;
                        lbl_3dayExp28DaysCompStr.BackColor = System.Drawing.Color.Red;
                        lbl_3dayTargetExp28DaysCompStr.BackColor = System.Drawing.Color.Red;
                        lbl_3dayTargetMeanStr.BackColor = System.Drawing.Color.Red;
                    }
                }
                if (lbl_7dayStr.Text != "")
                {
                    if (exp28daysStrength_7day >= TargetExp28daysStrength_7day_green)
                    {
                        grdTestSchedule.Rows[0].Cells[3].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[1].Cells[3].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[2].Cells[3].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[3].Cells[3].BackColor = System.Drawing.Color.LightGreen;
                        grdTestSchedule.Rows[4].Cells[3].BackColor = System.Drawing.Color.LightGreen;
                        chk_7daySch.BackColor = System.Drawing.Color.LightGreen;
                        lbl_7dayStr.BackColor = System.Drawing.Color.LightGreen;
                        lbl_7dayExp28DaysCompStr.BackColor = System.Drawing.Color.LightGreen;
                        lbl_7dayTargetExp28DaysCompStr.BackColor = System.Drawing.Color.LightGreen;
                        lbl_7dayTargetMeanStr.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (exp28daysStrength_7day >= TargetExp28daysStrength_7day_yellow)
                    {
                        grdTestSchedule.Rows[0].Cells[3].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[1].Cells[3].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[2].Cells[3].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[3].Cells[3].BackColor = System.Drawing.Color.Yellow;
                        grdTestSchedule.Rows[4].Cells[3].BackColor = System.Drawing.Color.Yellow;
                        chk_7daySch.BackColor = System.Drawing.Color.Yellow;
                        lbl_7dayStr.BackColor = System.Drawing.Color.Yellow;
                        lbl_7dayExp28DaysCompStr.BackColor = System.Drawing.Color.Yellow;
                        lbl_7dayTargetExp28DaysCompStr.BackColor = System.Drawing.Color.Yellow;
                        lbl_7dayTargetMeanStr.BackColor = System.Drawing.Color.Yellow;                    
                    }
                    else// if (exp28daysStrength_7day < 90)
                    {
                        grdTestSchedule.Rows[0].Cells[3].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[1].Cells[3].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[2].Cells[3].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[3].Cells[3].BackColor = System.Drawing.Color.Red;
                        grdTestSchedule.Rows[4].Cells[3].BackColor = System.Drawing.Color.Red;
                        chk_7daySch.BackColor = System.Drawing.Color.Red;
                        lbl_7dayStr.BackColor = System.Drawing.Color.Red;
                        lbl_7dayExp28DaysCompStr.BackColor = System.Drawing.Color.Red;
                        lbl_7dayTargetExp28DaysCompStr.BackColor = System.Drawing.Color.Red;
                        lbl_7dayTargetMeanStr.BackColor = System.Drawing.Color.Red;                    
                    }
                }
            }

        }
        protected void chk_WitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            ShowWitnessBy();
        }

        private void ShowWitnessBy()
        {
            txt_witnessBy.Text = "";
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

        protected void lnkAllInAggt_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
                LoadAllInAGGTData();
                ModalPopupExtender1.Show();
            }
        }

        protected void imgCloseAllInAGGT_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }

        protected void LoadAllInAGGTData()
        {
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("SieveSize", typeof(string)));
            dtTable.Columns.Add(new DataColumn("NaturalSand", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CrushedSand", typeof(string)));
            dtTable.Columns.Add(new DataColumn("StoneDust", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Grit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("10mm", typeof(string)));
            dtTable.Columns.Add(new DataColumn("20mm", typeof(string)));
            dtTable.Columns.Add(new DataColumn("40mm", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MixAggregate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("NaturalSandPer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CrushedSandPer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("StoneDustPer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("GritPer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("10mmPer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("20mmPer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("40mmPer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MixAggregatePer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CombinedPassing", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Passing(IS:383)", typeof(string)));

            for (int i = 1; i <= 16; i++)
            {
                grdAllInAGGT.Columns[i].Visible = false;
            }
            string[] strSieveSize = { "40 mm", "25 mm", "20 mm", "16 mm", "12.5 mm", "10 mm", "4.75 mm", "2.36 mm", "1.18 mm", "600 micron", "300 micron", "150 micron", "75 micron", "Pan" };
            decimal[,] strValues = new decimal[14, 17];
            for (int i = 0; i <= grdTrail.Rows.Count - 1; i++)
            {
                Label lbl_MaterialName = (Label)grdTrail.Rows[i].Cells[0].FindControl("lbl_MaterialName");
                if ((lbl_MaterialName.Text.Contains("Natural Sand") == true || lbl_MaterialName.Text.Contains("Crushed Sand") == true ||
                    lbl_MaterialName.Text.Contains("Stone Dust") == true || lbl_MaterialName.Text.Contains("Grit") == true ||
                        lbl_MaterialName.Text.Contains("Mix Aggregate") == true || lbl_MaterialName.Text.Contains("10 mm") == true ||
                        lbl_MaterialName.Text.Contains("20 mm") == true || lbl_MaterialName.Text.Contains("40 mm") == true))
                {
                    TextBox txt_Trail = (TextBox)grdTrail.Rows[i].Cells[1].FindControl("txt_Trail");
                    TextBox txt_SpGrav = (TextBox)grdTrail.Rows[i].Cells[2].FindControl("txt_SpGrav");
                    TextBox txt_Volume = (TextBox)grdTrail.Rows[i].Cells[3].FindControl("txt_Volume");
                    TextBox txt_MatId = (TextBox)grdTrail.Rows[i].Cells[7].FindControl("txt_MatId");
                    double Trail = 0;
                    double SpGrav = 0;
                    double volume = 0;
                    bool flgFound = false;
                    if (double.TryParse(txt_Trail.Text, out Trail) &&
                            double.TryParse(txt_SpGrav.Text, out SpGrav) && double.TryParse(txt_Volume.Text, out volume))
                    {
                        if (lbl_MaterialName.Text.Contains("Natural Sand") == true)
                        {
                            grdAllInAGGT.Columns[9].HeaderText = "Natural Sand( " + Trail + " %)";
                            grdAllInAGGT.Columns[1].Visible = true;
                            grdAllInAGGT.Columns[9].Visible = true;
                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 0] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 8] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 0] == 0)
                                    {
                                        strValues[j, 0] = 100;
                                        strValues[j, 8] = (strValues[j, 0] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }
                        else if (lbl_MaterialName.Text.Contains("Crushed Sand") == true)
                        {
                            grdAllInAGGT.Columns[10].HeaderText = "Crushed Sand( " + Trail + " %)";
                            grdAllInAGGT.Columns[2].Visible = true;
                            grdAllInAGGT.Columns[10].Visible = true;

                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 1] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 9] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 1] == 0)
                                    {
                                        strValues[j, 1] = 100;
                                        strValues[j, 9] = (strValues[j, 1] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }
                        else if (lbl_MaterialName.Text.Contains("Stone Dust") == true)
                        {
                            grdAllInAGGT.Columns[11].HeaderText = "Stone Dust( " + Trail + " %)";
                            grdAllInAGGT.Columns[3].Visible = true;
                            grdAllInAGGT.Columns[11].Visible = true;

                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 2] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 10] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 2] == 0)
                                    {
                                        strValues[j, 2] = 100;
                                        strValues[j, 10] = (strValues[j, 2] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }
                        else if (lbl_MaterialName.Text.Contains("Grit") == true)
                        {
                            grdAllInAGGT.Columns[12].HeaderText = "Grit( " + Trail + " %)";
                            grdAllInAGGT.Columns[4].Visible = true;
                            grdAllInAGGT.Columns[12].Visible = true;

                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 3] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 11] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 3] == 0)
                                    {
                                        strValues[j, 3] = 100;
                                        strValues[j, 11] = (strValues[j, 3] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }
                        else if (lbl_MaterialName.Text.Contains("10 mm") == true)
                        {
                            grdAllInAGGT.Columns[13].HeaderText = "10 mm( " + Trail + " %)";
                            grdAllInAGGT.Columns[5].Visible = true;
                            grdAllInAGGT.Columns[13].Visible = true;

                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 4] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 12] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 4] == 0)
                                    {
                                        strValues[j, 4] = 100;
                                        strValues[j, 12] = (strValues[j, 4] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }
                        else if (lbl_MaterialName.Text.Contains("20 mm") == true)
                        {
                            grdAllInAGGT.Columns[14].HeaderText = "20 mm( " + Trail + " %)";
                            grdAllInAGGT.Columns[6].Visible = true;
                            grdAllInAGGT.Columns[14].Visible = true;

                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 5] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 13] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 5] == 0)
                                    {
                                        strValues[j, 5] = 100;
                                        strValues[j, 13] = (strValues[j, 5] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }
                        else if (lbl_MaterialName.Text.Contains("40 mm") == true)
                        {
                            grdAllInAGGT.Columns[15].HeaderText = "40 mm( " + Trail + " %)";
                            grdAllInAGGT.Columns[7].Visible = true;
                            grdAllInAGGT.Columns[15].Visible = true;

                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 6] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 14] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 6] == 0)
                                    {
                                        strValues[j, 6] = 100;
                                        strValues[j, 14] = (strValues[j, 6] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }
                        else if (lbl_MaterialName.Text.Contains("Mix Aggregate") == true)
                        {
                            grdAllInAGGT.Columns[16].HeaderText = "Mix Aggregate( " + Trail + " %)";
                            grdAllInAGGT.Columns[8].Visible = true;
                            grdAllInAGGT.Columns[16].Visible = true;

                            var aggtTest = dc.AggregateAllTestView(txt_RefNo.Text, Convert.ToInt32(txt_MatId.Text), "AGGTSA");
                            foreach (var agtest in aggtTest)
                            {
                                for (int j = 0; j < 14; j++)
                                {
                                    if (strSieveSize[j] == agtest.AGGTSA_SeiveSize_var)
                                    {
                                        strValues[j, 7] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec);
                                        strValues[j, 15] = Convert.ToDecimal(agtest.AGGTSA_CumuPassing_dec * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                        flgFound = true;
                                        break;
                                    }
                                    else if (flgFound == false && strValues[j, 7] == 0)
                                    {
                                        strValues[j, 7] = 100;
                                        strValues[j, 15] = (strValues[j, 7] * Convert.ToDecimal(Trail)) / Convert.ToDecimal(100);
                                        strValues[j, 16] = strValues[j, 9] + strValues[j, 10] + strValues[j, 11] + strValues[j, 12] + strValues[j, 13] + strValues[j, 14] + strValues[j, 15] + strValues[j, 16];
                                    }
                                }
                            }
                        }

                    }
                }
            }
            //
            for (int i = 0; i < 14; i++)
            {
                drRow = dtTable.NewRow();
                drRow["SieveSize"] = strSieveSize[i];

                drRow["NaturalSand"] = strValues[i, 0];
                drRow["CrushedSand"] = strValues[i, 1];
                drRow["StoneDust"] = strValues[i, 2];
                drRow["Grit"] = strValues[i, 3];
                drRow["10mm"] = strValues[i, 4];
                drRow["20mm"] = strValues[i, 5];
                drRow["40mm"] = strValues[i, 6];
                drRow["MixAggregate"] = strValues[i, 7];

                drRow["NaturalSandPer"] = strValues[i, 8].ToString("0.00");
                drRow["CrushedSandPer"] = strValues[i, 9].ToString("0.00");
                drRow["StoneDustPer"] = strValues[i, 10].ToString("0.00");
                drRow["GritPer"] = strValues[i, 11].ToString("0.00");
                drRow["10mmPer"] = strValues[i, 12].ToString("0.00");
                drRow["20mmPer"] = strValues[i, 13].ToString("0.00");
                drRow["40mmPer"] = strValues[i, 14].ToString("0.00");
                drRow["MixAggregatePer"] = strValues[i, 15].ToString("0.00");

                drRow["CombinedPassing"] = strValues[i, 16].ToString("0.00");
                if (strSieveSize[i] == "20 mm")
                    drRow["Passing(IS:383)"] = "95-100";
                else if (strSieveSize[i] == "4.75 mm")
                    drRow["Passing(IS:383)"] = "30-50";
                else if (strSieveSize[i] == "600 micron")
                    drRow["Passing(IS:383)"] = "10-35";
                else if (strSieveSize[i] == "150 micron")
                    drRow["Passing(IS:383)"] = "0-6";
                else
                    drRow["Passing(IS:383)"] = "---";
                dtTable.Rows.Add(drRow);

            }
            grdAllInAGGT.DataSource = dtTable;
            grdAllInAGGT.DataBind();
            for (int i = 0; i < 14; i++)
            {
                if (grdAllInAGGT.Rows[i].Cells[0].Text == "20 mm")
                {
                    if (Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) < 95 || Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) > 100)
                        grdAllInAGGT.Rows[i].BackColor = System.Drawing.Color.LightPink;
                }
                else if (grdAllInAGGT.Rows[i].Cells[0].Text == "4.75 mm")
                {
                    if (Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) < 30 || Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) > 50)
                        grdAllInAGGT.Rows[i].BackColor = System.Drawing.Color.LightPink;
                }
                else if (grdAllInAGGT.Rows[i].Cells[0].Text == "600 micron")
                {
                    if (Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) < 10 || Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) > 35)
                        grdAllInAGGT.Rows[i].BackColor = System.Drawing.Color.LightPink;
                }
                else if (grdAllInAGGT.Rows[i].Cells[0].Text == "150 micron")
                {
                    if (Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) < 0 || Convert.ToDecimal(grdAllInAGGT.Rows[i].Cells[17].Text) > 6)
                        grdAllInAGGT.Rows[i].BackColor = System.Drawing.Color.LightPink;
                }

            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (ddl_OtherPendingRpt.SelectedIndex > 0 && ddlTrial.SelectedIndex > 0)
            {
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.TrialInformation_Html(ddl_OtherPendingRpt.SelectedValue,Convert.ToInt32(ddlTrial.SelectedValue));
            }
            else if (txt_RefNo.Text != "" && lbl_TrialId.Text != "" && lbl_TrialId.Text != "0")
            {
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.TrialInformation_Html(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            }
        }

        protected void lnkTrialOtherInfo_Click(object sender, EventArgs e)
        {
            if (lbl_TrialId.Text != "0" && lbl_TrialId.Text != "")
            {
                ModalPopupExtender2.Show();
                LoadTrialOtherInformation();
            }
        }

        protected void LoadTrialOtherInformation()
        {
            txtYield.Text = "";
            txtWtOfConcreteInCylinder.Text = "";
            txtComment.Text = "";
            chk_ApproveTrial.Checked = false;
            chk_CubeCasting.Checked = false;
            lblTrialInfoMessage.Visible = false;
            CheckMDLStatus();
            lnkMDLetter.Enabled = false;
            AddRowSlump();
            AddRowTestSchedule();
            var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
            foreach (var t in data)
            {
                if (t.Trial_SlumpDetails != null && t.Trial_SlumpDetails != "")
                {
                    string[] slumpDetails = t.Trial_SlumpDetails.Split('|');
                    Label lbl_Name = (Label)grdSlump.Rows[0].FindControl("lbl_Name");
                    TextBox txt_0 = (TextBox)grdSlump.Rows[0].FindControl("txt_0");
                    TextBox txt_30 = (TextBox)grdSlump.Rows[0].FindControl("txt_30");
                    TextBox txt_60 = (TextBox)grdSlump.Rows[0].FindControl("txt_60");
                    TextBox txt_90 = (TextBox)grdSlump.Rows[0].FindControl("txt_90");
                    TextBox txt_120 = (TextBox)grdSlump.Rows[0].FindControl("txt_120");
                    TextBox txt_150 = (TextBox)grdSlump.Rows[0].FindControl("txt_150");
                    TextBox txt_180 = (TextBox)grdSlump.Rows[0].FindControl("txt_180");
                    lbl_Name.Text = slumpDetails[0];
                    txt_0.Text = slumpDetails[1];
                    txt_30.Text = slumpDetails[2];
                    txt_60.Text = slumpDetails[3];
                    txt_90.Text = slumpDetails[4];
                    txt_120.Text = slumpDetails[5];
                    txt_150.Text = slumpDetails[6];
                    txt_180.Text = slumpDetails[7];
                }
                txtYield.Text = t.Trial_Yield.ToString();
                txtWtOfConcreteInCylinder.Text = t.Trial_WtOfConcreteInCylinder.ToString();
                txtComment.Text = t.Trial_Comment;
                chk_ApproveTrial.Checked = Convert.ToBoolean(t.Trial_MDletter_Status);
                break;
            }
            if (txtYield.Text == "")
            {
                TextBox txt_TrailActual = (TextBox)grdTrail.Rows[grdTrail.Rows.Count - 1].FindControl("txt_TrailActual");
                txtYield.Text = txt_TrailActual.Text;
            }

            CheckBox chk_1daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_1daySch");
            CheckBox chk_3daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_3daySch");
            CheckBox chk_7daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_7daySch");
            CheckBox chk_28daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_28daySch");
            CheckBox chk_56daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_56daySch");
            CheckBox chk_90daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_90daySch");
            CheckBox chk_OtherdaySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_OtherdaySch");
            Label lbl_1dayStr = (Label)grdTestSchedule.Rows[1].FindControl("lbl_1dayStr");
            Label lbl_3dayStr = (Label)grdTestSchedule.Rows[1].FindControl("lbl_3dayStr");
            Label lbl_7dayStr = (Label)grdTestSchedule.Rows[1].FindControl("lbl_7dayStr");
            Label lbl_28dayStr = (Label)grdTestSchedule.Rows[1].FindControl("lbl_28dayStr");
            Label lbl_56dayStr = (Label)grdTestSchedule.Rows[1].FindControl("lbl_56dayStr");
            Label lbl_90dayStr = (Label)grdTestSchedule.Rows[1].FindControl("lbl_90dayStr");
            Label lbl_OtherdayStr = (Label)grdTestSchedule.Rows[1].FindControl("lbl_OtherdayStr");
            TextBox txt_1dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_1dayStr");
            TextBox txt_3dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_3dayStr");
            TextBox txt_7dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_7dayStr");
            TextBox txt_28dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_28dayStr");
            TextBox txt_56dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_56dayStr");
            TextBox txt_90dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_90dayStr");
            TextBox txt_OtherdayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_OtherdayStr");


            var cubeCast = dc.OtherCubeTestView(txt_RefNo.Text, txt_RecType.Text, 0, Convert.ToInt32(lbl_TrialId.Text), "Trial", false, false);
            foreach (var c in cubeCast)
            {
                if (c.Days_tint == 1)
                {
                    chk_1daySch.Checked = true;
                    lbl_1dayStr.Text = c.Avg_var;
                    txt_1dayStr.Text = c.Curing_Detail;
                }
                else if (c.Days_tint == 3)
                {
                    chk_3daySch.Checked = true;
                    lbl_3dayStr.Text = c.Avg_var;
                    txt_3dayStr.Text = c.Curing_Detail;
                }
                else if (c.Days_tint == 7)
                {
                    chk_7daySch.Checked = true;
                    lbl_7dayStr.Text = c.Avg_var;
                    txt_7dayStr.Text = c.Curing_Detail;
                }
                else if (c.Days_tint == 28)
                {
                    chk_28daySch.Checked = true;
                    lbl_28dayStr.Text = c.Avg_var;
                    txt_28dayStr.Text = c.Curing_Detail;
                }
                else if (c.Days_tint == 56)
                {
                    chk_56daySch.Checked = true;
                    lbl_56dayStr.Text = c.Avg_var;
                    txt_56dayStr.Text = c.Curing_Detail;
                }
                else if (c.Days_tint == 90)
                {
                    chk_90daySch.Checked = true;
                    lbl_90dayStr.Text = c.Avg_var;
                    txt_90dayStr.Text = c.Curing_Detail;
                }
                else
                {
                    chk_OtherdaySch.Checked = true;
                    lbl_OtherdayStr.Text = c.Avg_var;
                    txtOtherDay.Text = c.Days_tint.ToString();
                    txt_OtherdayStr.Text = c.Curing_Detail;
                }
            }
            CalculateExp28DaysCompStrength();
        }

        protected void lnkSaveOtherInfo_Click(object sender, EventArgs e)
        {
            CheckBox chk_1daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_1daySch");
            CheckBox chk_3daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_3daySch");
            CheckBox chk_7daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_7daySch");
            CheckBox chk_28daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_28daySch");
            CheckBox chk_56daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_56daySch");
            CheckBox chk_90daySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_90daySch");
            CheckBox chk_OtherdaySch = (CheckBox)grdTestSchedule.Rows[0].FindControl("chk_OtherdaySch");
            int noOfCubes = Convert.ToInt32(txt_NoofCubes.Text);
            if (chk_1daySch.Checked == true)
            {
                noOfCubes = noOfCubes - 3;
            }
            if (chk_3daySch.Checked == true)
            {
                noOfCubes = noOfCubes - 3;
            }
            if (chk_7daySch.Checked == true)
            {
                noOfCubes = noOfCubes - 3;
            }
            if (chk_28daySch.Checked == true)
            {
                noOfCubes = noOfCubes - 3;
            }
            if (chk_56daySch.Checked == true)
            {
                noOfCubes = noOfCubes - 3;
            }
            if (chk_90daySch.Checked == true)
            {
                noOfCubes = noOfCubes - 3;
            }
            if (chk_OtherdaySch.Checked == true)
            {
                noOfCubes = noOfCubes - 3;
            }
            //var mdlStatus = dc.MixDesign_View(txt_RefNo.Text, 1);
            //if (mdlStatus.FirstOrDefault().MFINWD_Status_tint > 1)
            //{
            //    lblTrialInfoMessage.Text = "Mix design letter for selected report has been entered, can not update trial information. ";
            //    lblTrialInfoMessage.Visible = true;
            //    lblTrialInfoMessage.ForeColor = System.Drawing.Color.Red;
            //}
            int otherDay = 0;
            if (noOfCubes < 0 && chk_CubeCasting.Checked == true)
            {
                lblTrialInfoMessage.Text = "Select only " + (Convert.ToInt32(txt_NoofCubes.Text) / 3).ToString("0") + " testing schedule. ";
                lblTrialInfoMessage.Visible = true;
                lblTrialInfoMessage.ForeColor = System.Drawing.Color.Red;
            }
            else if (chk_OtherdaySch.Checked == true && txtOtherDay.Text == "")
            {
                lblTrialInfoMessage.Text = "Please Enter Other day ";
                lblTrialInfoMessage.Visible = true;
                lblTrialInfoMessage.ForeColor = System.Drawing.Color.Red;
                txtOtherDay.Focus();
            }
            else if (chk_OtherdaySch.Checked == true && int.TryParse(txtOtherDay.Text, out otherDay) == false)
            {
                lblTrialInfoMessage.Text = "Please Enter valid Other day ";
                lblTrialInfoMessage.Visible = true;
                lblTrialInfoMessage.ForeColor = System.Drawing.Color.Red;
                txtOtherDay.Focus();
            }
            else
            {
                string slumpDetails = "";
                Label lbl_Name = (Label)grdSlump.Rows[0].FindControl("lbl_Name");
                TextBox txt_0 = (TextBox)grdSlump.Rows[0].FindControl("txt_0");
                TextBox txt_30 = (TextBox)grdSlump.Rows[0].FindControl("txt_30");
                TextBox txt_60 = (TextBox)grdSlump.Rows[0].FindControl("txt_60");
                TextBox txt_90 = (TextBox)grdSlump.Rows[0].FindControl("txt_90");
                TextBox txt_120 = (TextBox)grdSlump.Rows[0].FindControl("txt_120");
                TextBox txt_150 = (TextBox)grdSlump.Rows[0].FindControl("txt_150");
                TextBox txt_180 = (TextBox)grdSlump.Rows[0].FindControl("txt_180");

                TextBox txt_1dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_1dayStr");
                TextBox txt_3dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_3dayStr");
                TextBox txt_7dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_7dayStr");
                TextBox txt_28dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_28dayStr");
                TextBox txt_56dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_56dayStr");
                TextBox txt_90dayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_90dayStr");
                TextBox txt_OtherdayStr = (TextBox)grdTestSchedule.Rows[5].FindControl("txt_OtherdayStr");

                slumpDetails = lbl_Name.Text + "|" + txt_0.Text + "|" + txt_30.Text + "|" + txt_60.Text + "|" + txt_90.Text + "|" + txt_120.Text + "|" + txt_150.Text + "|" + txt_180.Text + "|";
                decimal yield = 0, WtOfConcreteInCylinder = 0;
                if (txtYield.Text.Trim() != "")
                    yield = Convert.ToDecimal(txtYield.Text.Trim());
                if (txtWtOfConcreteInCylinder.Text.Trim() != "")
                    WtOfConcreteInCylinder = Convert.ToDecimal(txtWtOfConcreteInCylinder.Text.Trim());

                bool MDLetter = false;
                int TrialStatus = 0;
                DateTime Castingdate = DateTime.Now;
                DateTime TestingDt = DateTime.Now;

                if (chk_ApproveTrial.Checked)
                {
                    MDLetter = true;
                    TrialStatus = 1;
                    dc.MDLetter_Update(txt_RefNo.Text, 0, "MDL", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
                    dc.MDLetter_Update(txt_RefNo.Text, 0, "Final", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
                    dc.MixDesignLetter_Update(txt_RefNo.Text, 0, "MDL", 0, "", "", "", 0, 0,0, true);
                    dc.MixDesignLetter_Update(txt_RefNo.Text, 0, "Final", 0, "", "", "", 0, 0, 0,true);
                    //dc.AllInwd_Update(txt_RefNo.Text, "", 0, null, "", "", 1, 0, "", 0, "", "", 0, 0, 0, "MF");
                    dc.MixDesignInward_Update_ReportData(1, 0, txt_RefNo.Text, "", 0, 0, 0, 0, 0, 0, 0,false,false);
                }

                dc.Trail_Update(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", null, null, "", "", "", "", "", "", "",
                              0, MDLetter, "", "", "", "", false, "", true, "","", "", "", "", slumpDetails, yield, txtComment.Text, WtOfConcreteInCylinder, false, TrialStatus, true, false);

                if (chk_CubeCasting.Checked)
                {
                    var otherInfo = dc.OtherCubeTestView(txt_RefNo.Text, txt_RecType.Text, 0, Convert.ToInt32(lbl_TrialId.Text), "Trial", false, false);
                    foreach (var o in otherInfo)
                    {
                        dc.OtherCubeTestDetail_Update(txt_RefNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", o.Days_tint, "", "", null, "Trial", "MF", "", "", "", false, false, true, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, 0, 0, 1, "", Convert.ToInt32(lbl_TrialId.Text),"", false, true);


                    Castingdate = DateTime.ParseExact(txt_TrialDate.Text, "dd/MM/yyyy", null);
                    byte days = 0;
                    //UPDATE Other Cube Test
                    if (chk_1daySch.Checked == true)
                    {
                        days = 1;
                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, days, 3, 1, "Trial", Convert.ToInt32(lbl_TrialId.Text), txt_1dayStr.Text, false, false);
                        TestingDt = Castingdate.AddDays(days);
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, days, txt_TrialDate.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    if (chk_3daySch.Checked == true)
                    {
                        days = 3;
                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, days, 3, 1, "Trial", Convert.ToInt32(lbl_TrialId.Text), txt_3dayStr.Text, false, false);
                        TestingDt = Castingdate.AddDays(days);
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, days, txt_TrialDate.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    if (chk_7daySch.Checked == true)
                    {
                        days = 7;
                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, days, 3, 1, "Trial", Convert.ToInt32(lbl_TrialId.Text), txt_7dayStr.Text, false, false);
                        TestingDt = Castingdate.AddDays(days);
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, days, txt_TrialDate.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    if (chk_28daySch.Checked == true)
                    {
                        days = 28;
                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, days, 3, 1, "Trial", Convert.ToInt32(lbl_TrialId.Text), txt_28dayStr.Text, false, false);
                        TestingDt = Castingdate.AddDays(days);
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, days, txt_TrialDate.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    if (chk_56daySch.Checked == true)
                    {
                        days = 56;
                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, days, 3, 1, "Trial", Convert.ToInt32(lbl_TrialId.Text), txt_56dayStr.Text, false, false);
                        TestingDt = Castingdate.AddDays(days);
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, days, txt_TrialDate.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    if (chk_90daySch.Checked == true)
                    {
                        days = 90;
                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, days, 3, 1, "Trial", Convert.ToInt32(lbl_TrialId.Text), txt_90dayStr.Text, false, false);
                        TestingDt = Castingdate.AddDays(days);
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, days, txt_TrialDate.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    if (chk_OtherdaySch.Checked == true)
                    {
                        days = Convert.ToByte(txtOtherDay.Text) ;
                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, days, 3, 1, "Trial", Convert.ToInt32(lbl_TrialId.Text), txt_1dayStr.Text, false, false);
                        TestingDt = Castingdate.AddDays(days);
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, days, txt_TrialDate.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));
                    }
                }
                lblTrialInfoMessage.Text = "Record Saved Successfully";
                lblTrialInfoMessage.Visible = true;
                lblTrialInfoMessage.ForeColor = System.Drawing.Color.Green;
                lnkSaveOtherInfo.Enabled = false;
                if (chk_ApproveTrial.Checked == true)
                {
                    lnkMDLetter.Enabled = true;
                }
            }
        }

        protected void imgCloseOtherInfo_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }        

        protected void lnkCancelOtherInfo_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
        }

        protected void txtWtOfConcreteInCylinder_TextChanged(object sender, EventArgs e)
        {
            decimal wtOfcyl = 0;
            if (txtWtOfConcreteInCylinder.Text.Trim() != "" && Decimal.TryParse(txtWtOfConcreteInCylinder.Text.Trim(), out wtOfcyl) == true) 
            {
                txtYield.Text = (Convert.ToDecimal(txtWtOfConcreteInCylinder.Text) / Convert.ToDecimal("0.01471")).ToString("0");
            }
        }

        protected void lnkMDLetter_Click(object sender, EventArgs e)
        {
            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            string strURLWithData = "MixDesignLetter.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&TrailId={1}&Action={2}&MDtype={3}", txt_RefNo.Text,  lbl_TrialId.Text, "Check", "MDL"));
            Response.Redirect(strURLWithData);
        }

        protected void chkValidationFor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkValidationFor.Checked == true)
            {
                for (int i = 0; i < grdTrail.Rows.Count; i++)
                {
                    TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].FindControl("txt_TrailActual");
                    txt_TrailActual.CssClass = "";
                    txt_TrailActual.ReadOnly = false;
                }
            }
            else
            {
                for (int i = 0; i < grdTrail.Rows.Count; i++)
                {
                    TextBox txt_TrailActual = (TextBox)grdTrail.Rows[i].FindControl("txt_TrailActual");
                    txt_TrailActual.CssClass = "caltextbox";
                    txt_TrailActual.ReadOnly = true;
                    txt_TrailActual.Text = "";
                }
                if (ValidateData() == true)
                {
                    Calculation();
                }
            }
        }
    }
}


