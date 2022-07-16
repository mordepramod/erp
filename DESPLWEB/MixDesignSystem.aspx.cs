using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class MixDesignSystem : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Mix Design - System";
                LoadReferenceNoList();

                //ddlAggregate.SelectedValue = "20 mm";
                //optFlakynessLessThan30.Checked = true;
                //ddlGrade.SelectedValue = "M 40";
                //txtSlump.Text = "110";
                //chkFlyashPercent.Checked = true;
                //txtFlyashPercent.Text = "25";
                //txtAdmixturePercent.Text = "1";
                //ddlReductionType.SelectedValue = "Type A (naptha)";
                //txt600MicronPercent.Text = "30";
                //txt4p75PassingPercent.Text = "90";
                //txt300micronPassing.Text = "20";
                //txtSPGCement.Text = "3.15";
                //txtSPGFlyash.Text = "2.1";
                //txtSPGFA.Text = "2.75";
                //txtSPGAggt.Text = "2.95";
                //txt2p36PassingPercent.Text = "75";

                //txtResultWaterUnitCost.Text = "10";
                //txtResultCementUnitCost.Text = "10";
                //txtResultFlyashUnitCost.Text = "10";
                //txtResultGGBSUnitCost.Text = "10";
                //txtResultMicrosilicaUnitCost.Text = "10";
                //txtResultAdmixtureUnitCost.Text = "10";
                //txtResultFA1UnitCost.Text = "10";
                //txtResult10mmUnitCost.Text = "10";
                //txtResult20mmUnitCost.Text = "10";
                //txtResult40mmUnitCost.Text = "10";
            }
        }

        private void LoadReferenceNoList()
        {
            var reportList = dc.MaterialDetail_View_List("Trial");
            ddlReferenceNo.DataTextField = "MaterialDetail_RefNo";
            ddlReferenceNo.DataSource = reportList;
            ddlReferenceNo.DataBind();
            ddlReferenceNo.Items.Insert(0, "---Select---");
        }

        private void LoadTrialList()
        {
            var trial = dc.MixDesign_System_View(ddlReferenceNo.SelectedItem.Value, 0, "", false);
            ddlTrial.DataTextField = "MFSys_TrialName_var";
            ddlTrial.DataValueField = "MFSys_TrialId_int";
            ddlTrial.DataSource = trial;
            ddlTrial.DataBind();
            ddlTrial.Items.Insert(0, new ListItem("---New---", "0"));
        }

        protected void chkFlyashPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFlyashPercent.Checked == true)
            {
                if (txtFlyashPercent.Text == "" || txtFlyashPercent.Text == "0.00")
                    txtFlyashPercent.Text = "25";
            }
            else
            {
                txtFlyashPercent.Text = "";
            }
        }

        protected void chkGGBSPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGGBSPercent.Checked == true)
            {
                if (txtGGBSPercent.Text == "" || txtGGBSPercent.Text == "0.00")
                    txtGGBSPercent.Text = "40";
            }
            else
            {
                txtGGBSPercent.Text = "";
            }
        }

        protected void chkMicrosillicaPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMicrosillicaPercent.Checked == true)
            {
                if (txtMicrosillicaPercent.Text == "" || txtMicrosillicaPercent.Text == "0.00")
                    txtMicrosillicaPercent.Text = "5";
            }
            else
            {
                txtMicrosillicaPercent.Text = "";
            }
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
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
            CalculateIndividualCost();
            CalculateTotalCost();
            pnlResult.Visible = true;
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true && ValidateCostForSave() == true)
            {
                Calculate();
                CalculateIndividualCost();
                CalculateTotalCost();
                int trialId = 0;
                string trialName = "", strFlakyness = "", strAggregateType ="";
                decimal flyashPercent = 0, ggbsPercent = 0, microsilicaPercent = 0, SPGFlyash = 0, SPGGGBS = 0, SPGMicroSilica = 0;
                bool updateFlag = false ;
                if (chkFlyashPercent.Checked == true)
                {
                    flyashPercent = Convert.ToDecimal(txtFlyashPercent.Text);                    
                }
                SPGFlyash = Convert.ToDecimal(txtSPGFlyash.Text);
                if (chkGGBSPercent.Checked == true)
                {
                    ggbsPercent = Convert.ToDecimal(txtGGBSPercent.Text);
                }
                SPGGGBS = Convert.ToDecimal(txtSPGGGBS.Text);
                if (chkMicrosillicaPercent.Checked == true)
                {
                    microsilicaPercent = Convert.ToDecimal(txtMicrosillicaPercent.Text);
                }
                SPGMicroSilica = Convert.ToDecimal(txtSPGMicroSilica.Text);
                if (optFlakynessLessThan30.Checked == true)
                    strFlakyness = "LessThan30";
                else if (optFlakyness30to40.Checked == true)
                    strFlakyness = "30to40";
                else if (optFlakynessGreaterThan40.Checked == true)
                    strFlakyness = "GreaterThan40";
                if (chkRoundedAggregate.Checked == true && chkNaturalSand.Checked == true)
                {
                    strAggregateType = "RoundedAggregate_And_NaturalSand";
                }
                else if (chkRoundedAggregate.Checked == true )
                {
                    strAggregateType = "RoundedAggregate";
                }
                else if (chkNaturalSand.Checked == true)
                {
                    strAggregateType = "NaturalSand";
                }
                
                if (ddlTrial.SelectedIndex > 0)
                {
                    updateFlag = true;
                    //trialId = Convert.ToInt32(ddlTrial.SelectedValue.Replace("Trial",""));
                    trialId = Convert.ToInt32(ddlTrial.SelectedValue);
                    trialName = ddlTrial.SelectedItem.Text;
                }
                else
                {
                    trialId = ddlTrial.Items.Count;
                    trialName = "Trial" + ddlTrial.Items.Count.ToString();
                }

                dc.MixDesign_System_Update(ddlReferenceNo.SelectedValue, trialId, trialName, ddlGrade.SelectedValue, ddlType.SelectedValue, Convert.ToInt32(txtSlump.Text)
                    , flyashPercent, ggbsPercent, microsilicaPercent, ddlAggregate.SelectedValue, strFlakyness, strAggregateType, Convert.ToDecimal(txtAdmixturePercent.Text)
                    , ddlReductionType.SelectedValue, Convert.ToDecimal(txt4p75PassingPercent.Text), Convert.ToDecimal(txt2p36PassingPercent.Text), Convert.ToDecimal(txt600MicronPercent.Text)
                    , Convert.ToDecimal(txt300micronPassing.Text), Convert.ToDecimal(txt150micronPassing.Text), Convert.ToDecimal(txtSPGCement.Text), SPGFlyash, SPGGGBS, SPGMicroSilica, Convert.ToDecimal(txtSPGFA.Text), Convert.ToDecimal(txtSPGAggt.Text)
                    , Convert.ToDecimal(txtResultPlasticDensity.Text), Convert.ToDecimal(txtResultWCRatio.Text), Convert.ToDecimal(txtResultBinderContent.Text), Convert.ToDecimal(txtResultCementPercent.Text), Convert.ToDecimal(txtResultFlyashPercent.Text)
                    , Convert.ToDecimal(txtResultGGBSPercent.Text), Convert.ToDecimal(txtResultMicrosilicaPercent.Text), Convert.ToDecimal(txtResultAdmixturePercent.Text), Convert.ToDecimal(txtResultFAPercent.Text), Convert.ToDecimal(txtResult10mmPercent.Text)
                    , Convert.ToDecimal(txtResult20mmPercent.Text), Convert.ToDecimal(txtResult40mmPercent.Text), Convert.ToDecimal(txtResultW300.Text), Convert.ToDecimal(txtResultMortarVolume.Text), Convert.ToDecimal(txtResultWater.Text)
                    , Convert.ToDecimal(txtResultCement.Text), Convert.ToDecimal(txtResultFlyash.Text), Convert.ToDecimal(txtResultGGBS.Text), Convert.ToDecimal(txtResultMicrosilica.Text), Convert.ToDecimal(txtResultAdmixture.Text)
                    , Convert.ToDecimal(txtResultFA1.Text), Convert.ToDecimal(txtResultFA2.Text), Convert.ToDecimal(txtResult10mm.Text), Convert.ToDecimal(txtResult20mm.Text), Convert.ToDecimal(txtResult40mm.Text), Convert.ToDecimal(txtResultWaterCost.Text)
                    , Convert.ToDecimal(txtResultCementCost.Text), Convert.ToDecimal(txtResultFlyashCost.Text), Convert.ToDecimal(txtResultGGBSCost.Text), Convert.ToDecimal(txtResultMicrosilicaCost.Text), Convert.ToDecimal(txtResultAdmixtureCost.Text)
                    , Convert.ToDecimal(txtResultFA1Cost.Text), Convert.ToDecimal(txtResultFA2Cost.Text), Convert.ToDecimal(txtResult10mmCost.Text), Convert.ToDecimal(txtResult20mmCost.Text), Convert.ToDecimal(txtResult40mmCost.Text), Convert.ToDecimal(txtTotalCost.Text), chk10mmPresent.Checked
                    , ddlCementType.SelectedValue, Convert.ToDecimal(txtPercentageFA1.Text), Convert.ToDecimal(txtPercentageFA2.Text), Convert.ToDecimal(txtMaxAdmixturePercent.Text), Convert.ToDecimal(txtResultWaterPowderRatio.Text), chkApprove.Checked, updateFlag); 

                dc.MixDesign_Cost_Update(ddlReferenceNo.SelectedValue, 0, 0, 0, 0, 0, 0,0, 0, 0, 0, 0, true);

                dc.MixDesign_Cost_Update(ddlReferenceNo.SelectedValue, Convert.ToDecimal(txtResultWaterUnitCost.Text), Convert.ToDecimal(txtResultCementUnitCost.Text), Convert.ToDecimal(txtResultFlyashUnitCost.Text), Convert.ToDecimal(txtResultGGBSUnitCost.Text)
                    , Convert.ToDecimal(txtResultMicrosilicaUnitCost.Text), Convert.ToDecimal(txtResultAdmixtureUnitCost.Text), Convert.ToDecimal(txtResultFA1UnitCost.Text), Convert.ToDecimal(txtResultFA2UnitCost.Text), Convert.ToDecimal(txtResult10mmUnitCost.Text), Convert.ToDecimal(txtResult20mmUnitCost.Text), Convert.ToDecimal(txtResult40mmUnitCost.Text), false);   

                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Trial Saved Successfully');", true);
                LoadTrialList();
                //if (chkApprove.Checked == true)
                //{
                //    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                //    string strURLWithData = "Trial.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&AddNewTrial={1}&TrialId={2}", ddlReferenceNo.SelectedValue, "AddNewTrial", "0"));
                //    Response.Redirect(strURLWithData);
                //}
                ClearAllControls(); 
            }
        }

        protected void lnkSaveCostMaster_Click(object sender, EventArgs e)
        {
            if (ValidateCost() == true)
            {
                dc.MixDesign_CostMaster_Update(Convert.ToDecimal(txtResultWaterUnitCost.Text), Convert.ToDecimal(txtResultCementUnitCost.Text), Convert.ToDecimal(txtResultFlyashUnitCost.Text), Convert.ToDecimal(txtResultGGBSUnitCost.Text)
                    , Convert.ToDecimal(txtResultMicrosilicaUnitCost.Text), Convert.ToDecimal(txtResultAdmixtureUnitCost.Text), Convert.ToDecimal(txtResultFA1UnitCost.Text), Convert.ToDecimal(txtResultFA2UnitCost.Text), Convert.ToDecimal(txtResult10mmUnitCost.Text), Convert.ToDecimal(txtResult20mmUnitCost.Text), Convert.ToDecimal(txtResult40mmUnitCost.Text));   

                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Cost master saved successfully." + "');", true);
            }
        }

        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            int tempSlump = 0;
            if (ddlReferenceNo.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Reference No.";
                ddlReferenceNo.Focus();
                valid = false;
            }
            else if (ddlGrade.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Grade.";
                ddlGrade.Focus();
                valid = false;
            }
            else if (txtSlump.Text == "")
            {
                lblMsg.Text = "Enter Slump.";
                txtSlump.Focus();
                valid = false;
            }
            else if (int.TryParse(txtSlump.Text, out tempSlump) == false)
            {
                lblMsg.Text = "Slump should be numeric value.";
                txtSlump.Focus();
                valid = false;
            }
            else if (Convert.ToInt32(txtSlump.Text) <= 75 && ddlType.SelectedValue == "Pumpable")
            {
                lblMsg.Text = "For pumpable concrete mix design slump should be greater than 75.";
                txtSlump.Focus();
                valid = false;
            }
            else if (ddlCementType.SelectedValue == "Type E" && Convert.ToDecimal(ddlGrade.Text.Replace("M ", "")) > Convert.ToDecimal("80"))
            {
                lblMsg.Text = "Can not select cement type E for grade greater than 80.";
                ddlCementType.Focus();
                valid = false;
            }
            else if (ddlCementType.SelectedValue == "Type D" && Convert.ToDecimal(ddlGrade.Text.Replace("M ", "")) > Convert.ToDecimal("75"))
            {
                lblMsg.Text = "Can not select cement type E for grade greater than 75.";
                ddlCementType.Focus();
                valid = false;
            }
            else if (chkFlyashPercent.Checked == true && txtFlyashPercent.Text == "")
            {
                lblMsg.Text = "Enter FlyAsh %.";
                txtFlyashPercent.Focus();
                valid = false;
            }
            else if (chkGGBSPercent.Checked == true && txtGGBSPercent.Text == "")
            {
                lblMsg.Text = "Enter GGBS %.";
                txtGGBSPercent.Focus();
                valid = false;
            }
            else if (chkMicrosillicaPercent.Checked == true && txtMicrosillicaPercent.Text == "")
            {
                lblMsg.Text = "Enter Microsillica %.";
                txtMicrosillicaPercent.Focus();
                valid = false;
            }
            else if (ddlAggregate.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Maximum Size of Aggregate.";
                ddlAggregate.Focus();
                valid = false;
            }
            else if (ddlAggregate.SelectedValue == "40 mm" && ddlType.SelectedValue == "Self Compacting")
            {
                lblMsg.Text = "Maximum Size of Aggregate should not be 40 mm for self compacting concrete.";
                ddlAggregate.Focus();
                valid = false;
            }
            else if (optFlakynessLessThan30.Checked == false &&
                optFlakyness30to40.Checked == false &&
                optFlakynessGreaterThan40.Checked == false)
            {
                lblMsg.Text = "Select Flakyness.";
                valid = false;
            }
            else if (txtAdmixturePercent.Text == "")
            {
                lblMsg.Text = "Enter Admixture Percent.";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (Convert.ToDecimal(txtAdmixturePercent.Text) == 0 && ddlType.SelectedValue == "Self Compacting")
            {
                lblMsg.Text = "Admixture % should be greater than zero for self compacting concrete.";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (Convert.ToDecimal(txtAdmixturePercent.Text) > 0 && ddlReductionType.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select ddlReduction Type.";
                ddlReductionType.Focus();
                valid = false;
            }
            else if (Convert.ToDecimal(txtAdmixturePercent.Text) > 0 && txtMaxAdmixturePercent.Text == "")
            {
                lblMsg.Text = "Enter Maximum Admixture %.";
                txtMaxAdmixturePercent.Focus();
                valid = false;
            }
            else if (Convert.ToDecimal(txtAdmixturePercent.Text) > Convert.ToDecimal(txtMaxAdmixturePercent.Text))
            {
                lblMsg.Text = "Admixture % should be less than or equal to Maximum Admixture %.";
                txtMaxAdmixturePercent.Focus();
                valid = false;
            }
            else if (ddlReductionType.SelectedValue == "Type A (naptha)"
                && Convert.ToDecimal(txtAdmixturePercent.Text) > Convert.ToDecimal("2")) 
            {
                lblMsg.Text = "Admixture % should be less than 2 for " + ddlReductionType.SelectedValue + ".";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (ddlReductionType.SelectedValue == "Type B (Mid PC)"
                && Convert.ToDecimal(txtAdmixturePercent.Text) > Convert.ToDecimal("2"))
            {
                lblMsg.Text = "Admixture % should be less than 2 for " + ddlReductionType.SelectedValue + ".";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (ddlReductionType.SelectedValue == "Type C (PC)"
                && Convert.ToDecimal(txtAdmixturePercent.Text) > Convert.ToDecimal("1.8")) 
            {
                lblMsg.Text = "Admixture % should be less than 1.8 for " + ddlReductionType.SelectedValue + ".";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (ddlReductionType.SelectedValue == "Type A (naptha)" && ddlType.SelectedValue == "Self Compacting"
            && Convert.ToDecimal(txtAdmixturePercent.Text) < Convert.ToDecimal("1.3")) 
            {
                lblMsg.Text = "Admixture % should be greater than or equal to 1.3 for " + ddlReductionType.SelectedValue + " for self compacting concrete.";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (ddlReductionType.SelectedValue == "Type B (Mid PC)" && ddlType.SelectedValue == "Self Compacting"
                && Convert.ToDecimal(txtAdmixturePercent.Text) < Convert.ToDecimal("1.1")) 
            {
                lblMsg.Text = "Admixture % should be greater than or equal to 1.1 for " + ddlReductionType.SelectedValue + " for self compacting concrete.";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (ddlReductionType.SelectedValue == "Type C (PC)" && ddlType.SelectedValue == "Self Compacting"
                && Convert.ToDecimal(txtAdmixturePercent.Text) < Convert.ToDecimal("0.9"))
            {
                lblMsg.Text = "Admixture % should be greater than or equal to 0.9 for " + ddlReductionType.SelectedValue + " for self compacting concrete.";
                txtAdmixturePercent.Focus();
                valid = false;
            }
            else if (txtPercentageFA1.Text == "")
            {
                lblMsg.Text = "Enter FA1 percentage.";
                txtPercentageFA1.Focus();
                valid = false;
            }
            else if (txtPercentageFA2.Text == "")
            {
                lblMsg.Text = "Enter FA2 percentage.";
                txtPercentageFA2.Focus();
                valid = false;
            }
            else if (txtPercentageFA1.Text != "" && txtPercentageFA2.Text != "" 
                && (Convert.ToDecimal(txtPercentageFA1.Text) + Convert.ToDecimal(txtPercentageFA2.Text)) != 100)
            {
                lblMsg.Text = "Fine Aggregate Percentage total should be 100.";
                txtPercentageFA1.Focus();
                valid = false;
            }
            else if (txt4p75PassingPercent.Text == "")
            {
                lblMsg.Text = "Enter 4.75 passing %.";
                txt4p75PassingPercent.Focus();
                valid = false;
            }
            else if (txt2p36PassingPercent.Text == "")
            {
                lblMsg.Text = "Enter % passing 2.36.";
                txt2p36PassingPercent.Focus();
                valid = false;
            }
            else if (txt600MicronPercent.Text == "")
            {
                lblMsg.Text = "Enter 600 micron %.";
                txt600MicronPercent.Focus();
                valid = false;
            }
            else if (txt300micronPassing.Text == "")
            {
                lblMsg.Text = "Enter 300 micron passing.";
                txt300micronPassing.Focus();
                valid = false;
            }
            else if (txt150micronPassing.Text == "" && ddlType.SelectedValue == "Self Compacting")
            {
                lblMsg.Text = "Enter 150 micron passing.";
                txt150micronPassing.Focus();
                valid = false;
            }
            else if (txtSPGCement.Text == "" || Convert.ToDecimal(txtSPGCement.Text) == 0)
            {
                lblMsg.Text = "Enter SPG Cement.";
                txtSPGCement.Focus();
                valid = false;
            }
            else if (chkFlyashPercent.Checked == true && Convert.ToDecimal(txtFlyashPercent.Text) > 0 
                && (txtSPGFlyash.Text == "" || Convert.ToDecimal(txtSPGFlyash.Text) == 0))
            {
                lblMsg.Text = "Enter SPG Flyash.";
                txtSPGFlyash.Focus();
                valid = false;
            }
            else if (chkGGBSPercent.Checked == true && Convert.ToDecimal(txtGGBSPercent.Text) > 0 
                && (txtSPGGGBS.Text == "" || Convert.ToDecimal(txtSPGGGBS.Text) == 0))
            {
                lblMsg.Text = "Enter SPG GGBS.";
                txtSPGGGBS.Focus();
                valid = false;
            }
            else if (chkMicrosillicaPercent.Checked == true && Convert.ToDecimal(txtMicrosillicaPercent.Text) > 0 
                && (txtSPGMicroSilica.Text == "" || Convert.ToDecimal(txtSPGMicroSilica.Text) == 0))
            {
                lblMsg.Text = "Enter SPG Microsilica.";
                txtSPGMicroSilica.Focus();
                valid = false;
            }
            else if (txtSPGFA.Text == "")
            {
                lblMsg.Text = "Enter SPG FA.";
                txtSPGFA.Focus();
                valid = false;
            }
            else if (txtSPGAggt.Text == "")
            {
                lblMsg.Text = "Enter SPG Aggregate.";
                txtSPGAggt.Focus();
                valid = false;
            }
            else if (txtPercentageFA1.Text != "" && txtPercentageFA2.Text == "")
            {
                txtPercentageFA2.Text = (100 - Convert.ToDecimal(txtPercentageFA1.Text)).ToString();
            }
            else if (txtPercentageFA1.Text == "" && txtPercentageFA2.Text != "")
            {
                txtPercentageFA1.Text = (100 - Convert.ToDecimal(txtPercentageFA2.Text)).ToString();
            }
            if (txt150micronPassing.Text == "")
            {
                txt150micronPassing.Text = "0";
            }
            if (valid == false)
            {
                //lblMsg.Visible = true;
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + lblMsg.Text + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }

        protected Boolean ValidateCost()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (ddlReferenceNo.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Reference No.";
                ddlReferenceNo.Focus();
                valid = false;
            }
            else if (txtResultWaterUnitCost.Text == "")
            {
                lblMsg.Text = "Enter Water unit cost.";
                txtResultWaterUnitCost.Focus();
                valid = false;
            }
            else if (txtResultCementUnitCost.Text == "")
            {
                lblMsg.Text = "Enter Cement unit cost.";
                txtResultCementUnitCost.Focus();
                valid = false;
            }
            else if (txtResultFlyashUnitCost.Text == "")
            {
                lblMsg.Text = "Enter Flyash unit cost.";
                txtResultFlyashUnitCost.Focus();
                valid = false;
            }
            else if (txtResultGGBSUnitCost.Text == "")
            {
                lblMsg.Text = "Enter GGBS unit cost.";
                txtResultGGBSUnitCost.Focus();
                valid = false;
            }
            else if (txtResultMicrosilicaUnitCost.Text == "")
            {
                lblMsg.Text = "Enter Microsilica unit cost.";
                txtResultMicrosilicaUnitCost.Focus();
                valid = false;
            }
            else if (txtResultAdmixtureUnitCost.Text == "")
            {
                lblMsg.Text = "Enter Admixture unit cost.";
                txtResultAdmixtureUnitCost.Focus();
                valid = false;
            }
            else if (txtResultFA1UnitCost.Text == "")
            {
                lblMsg.Text = "Enter Fine Aggregate 1 unit cost.";
                txtResultFA1UnitCost.Focus();
                valid = false;
            }
            else if (txtResultFA2UnitCost.Text == "")
            {
                lblMsg.Text = "Enter Fine Aggregate 2 unit cost.";
                txtResultFA2UnitCost.Focus();
                valid = false;
            }
            else if (txtResult10mmUnitCost.Text == "")
            {
                lblMsg.Text = "Enter 10 mm unit cost.";
                txtResult10mmUnitCost.Focus();
                valid = false;
            }
            else if (txtResult20mmUnitCost.Text == "")
            {
                lblMsg.Text = "Enter 20 mm unit cost.";
                txtResult20mmUnitCost.Focus();
                valid = false;
            }
            else if (txtResult40mmUnitCost.Text == "")
            {
                lblMsg.Text = "Enter 40 mm unit cost.";
                txtResult40mmUnitCost.Focus();
                valid = false;
            }

            if (valid == false)
            {
                //lblMsg.Visible = true;
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + lblMsg.Text + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }

        protected Boolean ValidateCostForSave()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (ddlReferenceNo.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Reference No.";
                ddlReferenceNo.Focus();
                valid = false;
            }
            if (txtResultWaterUnitCost.Text == "")
            {
                txtResultWaterUnitCost.Text = "0";
            }
            if (txtResultCementUnitCost.Text == "")
            {
                txtResultCementUnitCost.Text = "0";
            }
            if (txtResultFlyashUnitCost.Text == "")
            {
                txtResultFlyashUnitCost.Text = "0";
            }
            if (txtResultGGBSUnitCost.Text == "")
            {
                txtResultGGBSUnitCost.Text = "0";
            }
            if (txtResultMicrosilicaUnitCost.Text == "")
            {
                txtResultMicrosilicaUnitCost.Text = "0";
            }
            if (txtResultAdmixtureUnitCost.Text == "")
            {
                txtResultAdmixtureUnitCost.Text = "0";
            }
            if (txtResultFA1UnitCost.Text == "")
            {
                txtResultFA1UnitCost.Text = "0";
            }
            if (txtResultFA2UnitCost.Text == "")
            {
                txtResultFA2UnitCost.Text = "0";
            }
            if (txtResult10mmUnitCost.Text == "")
            {
                txtResult10mmUnitCost.Text = "0";
            }
            if (txtResult20mmUnitCost.Text == "")
            {
                txtResult20mmUnitCost.Text = "0";
            }
            if (txtResult40mmUnitCost.Text == "")
            {
                txtResult40mmUnitCost.Text = "0";
            }
           
            if (valid == false)
            {
                //lblMsg.Visible = true;
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + lblMsg.Text + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }

        protected void Calculate()
        {
            if (ValidateData() == true)
            {
                FetchFAPercentWise();
                decimal WaterCementRatio = 0, RevisedWaterCementRatio = 0, WaterDemand = 0, MortarPercent = 0, BinderContent = 0,
                    CementContent = 0, Flyash = 0, GGBS = 0, MicroSillica = 0, WeightOfCA = 0, WFA = 0, W300 = 0, MortarVolume = 0,
                    PlasticDensity = 0, FAPercent = 0, CAPercent = 0, w10mm = 0, w20mm = 0, w40mm = 0, AdmixturePercent2 = 0;
                decimal tempVal = 0;
                decimal grade = 0;
                if (ddlGrade.SelectedIndex > 0)
                {
                    grade = Convert.ToDecimal(ddlGrade.SelectedValue.Replace("M ", ""));
                }
                #region 1. calculate water cement ratio
                switch (ddlGrade.SelectedValue)
                {
                    case "M 5":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.7");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.67");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.65");
                        break;
                    case "M 10":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.65");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.63");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.6");
                        break;
                    case "M 15":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.6");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.57");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.55");
                        break;
                    case "M 20":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.54");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.51");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.49");
                        break;
                    case "M 25":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.5");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.47");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.45");
                        break;
                    case "M 30":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.46");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.43");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.41");
                        break;
                    case "M 35":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.42");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.39");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.37");
                        break;
                    case "M 40":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.38");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.35");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.33");
                        break;
                    case "M 45":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.35");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.32");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.3");
                        break;
                    case "M 50":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.33");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.3");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.28");
                        break;
                    case "M 55":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.31");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.28");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.26");
                        break;
                    case "M 60":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.29");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.26");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.24");
                        break;
                    case "M 65":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.27");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.25");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.23");
                        break;
                    case "M 70":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.25");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.23");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.22");
                        break;
                    case "M 75":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.23");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.21");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = Convert.ToDecimal("0.19");
                        break;
                    case "M 80":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.21");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = Convert.ToDecimal("0.19");
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = 0;
                        break;
                    case "M 85":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.2");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = 0;
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = 0;
                        break;
                    case "M 90":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.19");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = 0;
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = 0;
                        break;
                    case "M 95":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.18");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = 0;
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = 0;
                        break;
                    case "M 100":
                        if (ddlCementType.SelectedValue == "Type F")
                            tempVal = Convert.ToDecimal("0.17");
                        else if (ddlCementType.SelectedValue == "Type E")
                            tempVal = 0;
                        else if (ddlCementType.SelectedValue == "Type D")
                            tempVal = 0;
                        break;
                }
                WaterCementRatio = tempVal;
                #endregion

                #region 2. calculate Revised water cement ratio
                tempVal = 0;
                if (WaterCementRatio > 0)
                {
                    tempVal = WaterCementRatio;
                    if (txtFlyashPercent.Text != "")
                    {
                        if (grade <= 40)
                            tempVal = tempVal - (Convert.ToDecimal(txtFlyashPercent.Text) / 100) / 10;
                        else
                            tempVal = tempVal - (Convert.ToDecimal(txtFlyashPercent.Text) / 100) / 20;
                    }
                    if (txtGGBSPercent.Text != "")
                    {
                        if (grade <= 40)
                            tempVal = tempVal - (Convert.ToDecimal(txtGGBSPercent.Text) / 100) / 20;
                        else
                            tempVal = tempVal - (Convert.ToDecimal(txtGGBSPercent.Text) / 100) / 40;
                    }
                }
                RevisedWaterCementRatio = tempVal;
                #endregion

                #region 3. calculate water demand
                tempVal = 0;
                if (ddlType.SelectedValue == "Self Compacting")
                {
                    if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                        tempVal = 250;
                    else if (ddlAggregate.SelectedValue == "10 mm")
                        tempVal = 265;
                }
                else
                {
                    if (txtSlump.Text != "")
                    {
                        int slump = Convert.ToInt32(txtSlump.Text);
                        if (slump >= 0 && slump <= 25)
                        {
                            if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                                tempVal = 175;
                            else if (ddlAggregate.SelectedValue == "10 mm")
                                tempVal = 190;
                            else if (ddlAggregate.SelectedValue == "40 mm")
                                tempVal = 165;
                        }
                        else if (slump >= 26 && slump <= 50)
                        {
                            if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                                tempVal = 180;
                            else if (ddlAggregate.SelectedValue == "10 mm")
                                tempVal = 195;
                            else if (ddlAggregate.SelectedValue == "40 mm")
                                tempVal = 170;
                        }
                        else if (slump >= 51 && slump <= 75)
                        {
                            if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                                tempVal = 185;
                            else if (ddlAggregate.SelectedValue == "10 mm")
                                tempVal = 200;
                            else if (ddlAggregate.SelectedValue == "40 mm")
                                tempVal = 175;
                        }
                        else if (slump >= 76 && slump <= 100)
                        {
                            if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                                tempVal = 190;
                            else if (ddlAggregate.SelectedValue == "10 mm")
                                tempVal = 205;
                            else if (ddlAggregate.SelectedValue == "40 mm")
                                tempVal = 180;
                        }
                        else if (slump >= 101 && slump <= 125)
                        {
                            if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                                tempVal = 195;
                            else if (ddlAggregate.SelectedValue == "10 mm")
                                tempVal = 210;
                            else if (ddlAggregate.SelectedValue == "40 mm")
                                tempVal = 185;
                        }
                        else if (slump >= 126 && slump <= 150)
                        {
                            if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                                tempVal = 200;
                            else if (ddlAggregate.SelectedValue == "10 mm")
                                tempVal = 215;
                            else if (ddlAggregate.SelectedValue == "40 mm")
                                tempVal = 190;
                        }
                        else if (slump >= 151)
                        {
                            if (ddlAggregate.SelectedValue == "20 mm" || grade >= 60)
                                tempVal = 205; 
                            else if (ddlAggregate.SelectedValue == "10 mm")
                                tempVal = 220; 
                            else if (ddlAggregate.SelectedValue == "40 mm")
                                tempVal = 195; 
                        }
                    }
                }
                //add
                if (optFlakyness30to40.Checked == true)
                {
                    tempVal = tempVal + 10;
                }
                else if (optFlakynessGreaterThan40.Checked == true)
                {
                    tempVal = tempVal + 15;
                }
                
                //subtract
                if (chkRoundedAggregate.Checked == true)
                {
                    tempVal = tempVal - 10;
                }
                if (chkNaturalSand.Checked == true)
                {
                    tempVal = tempVal - 10;
                }
                if (chk10mmPresent.Checked == false)
                {
                    tempVal = tempVal - 10;
                }
                WaterDemand = tempVal;
                #endregion

                #region 4.Admixture, 5.Reduction, 6.calculate revised water demand
                tempVal = 0;
                //Admixture
                if (txtAdmixturePercent.Text != "")
                {
                    tempVal = Convert.ToDecimal(txtAdmixturePercent.Text);
                }
                //Reduction
                decimal reduction = 0;
                if (ddlReductionType.SelectedValue == "Type A (naptha)")
                {
                    switch (tempVal.ToString("0.00"))
                    {
                        case "0.40":
                            reduction = 10;
                            break;
                        case "0.50":
                            reduction = 15;
                            break;
                        case "0.60":
                            reduction = 20;
                            break;
                        case "0.70":
                            reduction = 25;
                            break;
                        case "0.80":
                            reduction = 30;
                            break;
                        case "0.90":
                            reduction = 35;
                            break;
                        case "1.00":
                            reduction = 40;
                            break;
                        case "1.10":
                            reduction = 45;
                            break;
                        case "1.20":
                            reduction = 50;
                            break;
                        case "1.30":
                            reduction = 55;
                            break;
                        case "1.40":
                            reduction = 60;
                            break;
                        case "1.50":
                            reduction = 65;
                            break;
                        case "1.60":
                            reduction = 70;
                            break;
                        case "1.70":
                            reduction = 75;
                            break;
                        case "1.80":
                            reduction = 80;
                            break;
                        case "1.90":
                            reduction = 85;
                            break;
                        case "2.00":
                            reduction = 90;
                            break;
                    }
                }
                else if (ddlReductionType.SelectedValue == "Type B (Mid PC)")
                {
                    switch (tempVal.ToString("0.00"))
                    {
                        case "0.40":
                            reduction = 20;
                            break;
                        case "0.50":
                            reduction = 25;
                            break;
                        case "0.60":
                            reduction = 30;
                            break;
                        case "0.70":
                            reduction = 35;
                            break;
                        case "0.80":
                            reduction = 40;
                            break;
                        case "0.90":
                            reduction = 45;
                            break;
                        case "1.00":
                            reduction = 50;
                            break;
                        case "1.10":
                            reduction = 55;
                            break;
                        case "1.20":
                            reduction = 60;
                            break;
                        case "1.30":
                            reduction = 65;
                            break;
                        case "1.40":
                            reduction = 70;
                            break;
                        case "1.50":
                            reduction = 75;
                            break;
                        case "1.60":
                            reduction = 80;
                            break;
                        case "1.70":
                            reduction = 85;
                            break;
                        case "1.80":
                            reduction = 90;
                            break;
                        case "1.90":
                            reduction = 95;
                            break;
                        case "2.00":
                            reduction = 100;
                            break;

                    }
                }
                else if (ddlReductionType.SelectedValue == "Type C (PC)")
                {
                    switch (tempVal.ToString("0.00"))
                    {
                        case "0.60":
                            reduction = 40;
                            break;
                        case "0.70":
                            reduction = 45;
                            break;
                        case "0.80":
                            reduction = 50;
                            break;
                        case "0.90":
                            reduction = 55;
                            break;
                        case "1.00":
                            reduction = 60;
                            break;
                        case "1.10":
                            reduction = 65;
                            break;
                        case "1.20":
                            reduction = 70;
                            break;
                        case "1.30":
                            reduction = 75;
                            break;
                        case "1.40":
                            reduction = 80;
                            break;
                        case "1.50":
                            reduction = 85;
                            break;
                        case "1.60":
                            reduction = 90;
                            break;
                        case "1.70":
                            reduction = 95;
                            break;
                        case "1.80":
                            reduction = 100;
                            break;
                    }
                }
                //revised water demand 3-5
                WaterDemand = WaterDemand - reduction;
                #endregion

                #region 8. Calculate mortar %
                decimal mortarContent = 0;
                tempVal = 0;                
                if (ddlType.SelectedValue == "Self Compacting")
                {
                    tempVal = 58;
                }
                else
                {
                    if (txtSlump.Text != "")
                    {
                        // non pumpable changed with -2 and pumpable changed with -1
                        int slump = Convert.ToInt32(txtSlump.Text);
                        if (slump >= 0 && slump <= 25)
                        {
                            if (ddlType.SelectedValue == "Non Pumpable")
                                tempVal = 46;// 48;
                        }
                        else if (slump >= 26 && slump <= 50)
                        {
                            if (ddlType.SelectedValue == "Non Pumpable")
                                tempVal = 47;// 49;
                        }
                        else if (slump >= 51 && slump <= 75)
                        {
                            if (ddlType.SelectedValue == "Non Pumpable")
                                tempVal = 48;
                        }
                        else if (slump >= 76 && slump <= 100)
                        {
                            if (ddlType.SelectedValue == "Non Pumpable")
                                tempVal = 49;
                            else
                                tempVal = 51; //52
                        }
                        else if (slump >= 101 && slump <= 125)
                        {
                            if (ddlType.SelectedValue == "Non Pumpable")
                                tempVal = 50;
                            else
                                tempVal = 52; //53
                        }
                        else if (slump >= 126 && slump <= 150)
                        {
                            if (ddlType.SelectedValue == "Non Pumpable")
                                tempVal = 51;
                            else
                                tempVal = 53; //54
                        }
                        else if (slump >= 151)
                        {
                            // changed on 10/09/20 
                            if (ddlType.SelectedValue == "Non Pumpable")
                                tempVal = 56;     
                            else
                                tempVal = 58; 
                        }
                    }
                }
                // added on 28/08
                if (Convert.ToDecimal(txt2p36PassingPercent.Text) > 80 && Convert.ToDecimal(txt2p36PassingPercent.Text) < 90)
                {
                    tempVal = tempVal - 1;
                }
                else if (Convert.ToDecimal(txt2p36PassingPercent.Text) < 80 && Convert.ToDecimal(txt2p36PassingPercent.Text) > 70)
                {
                    tempVal = tempVal - 2;
                }
                else if (Convert.ToDecimal(txt2p36PassingPercent.Text) < 70)
                {
                    tempVal = tempVal - 3;
                }
                //
                mortarContent = tempVal;
                //add
                if (optFlakyness30to40.Checked == true)
                {
                    tempVal = tempVal + 1;
                }
                else if (optFlakynessGreaterThan40.Checked == true)
                {
                    tempVal = tempVal + 2;
                }
                if (ddlAggregate.SelectedValue == "10 mm" && grade < 60)
                {
                    tempVal = tempVal + 3;
                }
                if (txt600MicronPercent.Text != "")
                {
                    if (Convert.ToDecimal(txt600MicronPercent.Text) >= 25 && Convert.ToDecimal(txt600MicronPercent.Text) <= 30)
                        tempVal = tempVal + 1;
                    else if (Convert.ToDecimal(txt600MicronPercent.Text) >= 20 && Convert.ToDecimal(txt600MicronPercent.Text) < 25)
                        tempVal = tempVal + 2;
                    else if (Convert.ToDecimal(txt600MicronPercent.Text) >= 15 && Convert.ToDecimal(txt600MicronPercent.Text) < 20)
                        tempVal = tempVal + 3;
                    else if (Convert.ToDecimal(txt600MicronPercent.Text) < 15)
                        tempVal = tempVal + 4;
                }
                //subtract
                if (ddlAggregate.SelectedValue == "40 mm")
                {
                    tempVal = tempVal - 2;
                }
                if (chkRoundedAggregate.Checked == true)
                {
                    tempVal = tempVal - 2;
                }
                if (txt600MicronPercent.Text != "")
                {
                    if (Convert.ToDecimal(txt600MicronPercent.Text) >= 40 && Convert.ToDecimal(txt600MicronPercent.Text) <= 45)
                        tempVal = tempVal - 1;
                    else if (Convert.ToDecimal(txt600MicronPercent.Text) >= 45 && Convert.ToDecimal(txt600MicronPercent.Text) <= 50)
                        tempVal = tempVal - 2;
                    else if (Convert.ToDecimal(txt600MicronPercent.Text) >= 50 && Convert.ToDecimal(txt600MicronPercent.Text) <= 55)
                        tempVal = tempVal - 3;
                }
                MortarPercent = tempVal;
                tempVal = 0;
                //a. add water
                if (mortarContent < MortarPercent)
                {
                    tempVal = 5 * (MortarPercent - mortarContent);
                    WaterDemand = WaterDemand + tempVal;
                }
                //b. reduce water
                else if (mortarContent > MortarPercent)
                {
                    tempVal = 5 * (mortarContent - MortarPercent);
                    WaterDemand = WaterDemand - tempVal;
                }

                //Binder Content
                tempVal = 0;
                tempVal = WaterDemand / RevisedWaterCementRatio;
                if (tempVal < 300 && grade >= 20)
                {
                    tempVal = 300;
                }
                BinderContent = tempVal;
                //Cement Content
                tempVal = 1;
                if (txtFlyashPercent.Text != "")
                    tempVal = tempVal - (Convert.ToDecimal(txtFlyashPercent.Text) / 100);
                if (txtGGBSPercent.Text != "")
                    tempVal = tempVal - (Convert.ToDecimal(txtGGBSPercent.Text) / 100);
                if (txtMicrosillicaPercent.Text != "")
                    tempVal = tempVal - (Convert.ToDecimal(txtMicrosillicaPercent.Text) /100);
                tempVal = BinderContent * tempVal;
                CementContent = tempVal;
                //Flyash
                tempVal = 0;
                if (txtFlyashPercent.Text != "")
                {
                    tempVal = BinderContent * (Convert.ToDecimal(txtFlyashPercent.Text) / 100);
                }
                Flyash = tempVal;
                //GGBS
                tempVal = 0;
                if (txtGGBSPercent.Text != "")
                {
                    tempVal = BinderContent * (Convert.ToDecimal(txtGGBSPercent.Text) / 100);
                }
                GGBS = tempVal;
                //Microsillica
                tempVal = 0;
                if (txtMicrosillicaPercent.Text != "")
                {
                    tempVal = BinderContent * (Convert.ToDecimal(txtMicrosillicaPercent.Text) / 100);
                }
                MicroSillica = tempVal;
                #endregion

                #region 11. Calculate FA
                tempVal = 0;
                decimal plasticDensityInitial = 2500;
                if (MortarPercent != 0 && txt4p75PassingPercent.Text != "")
                {
                    tempVal = ((MortarPercent / 100) * plasticDensityInitial - BinderContent - WaterDemand) / (Convert.ToDecimal(txt4p75PassingPercent.Text) / 100);
                }
                WFA = tempVal;
                #endregion

                #region 12. Calculate 300 micron passing
                tempVal = 0;
                if (txt300micronPassing.Text != "")
                {
                    tempVal = BinderContent + WFA * (Convert.ToDecimal(txt300micronPassing.Text) / 100);
                }
                W300 = tempVal;

                //Mortar Volume
                tempVal = 0;
                if (txtSPGCement.Text != "")
                {
                    tempVal = (CementContent / Convert.ToDecimal(txtSPGCement.Text));
                }
                if (chkFlyashPercent.Checked == true &&  txtSPGFlyash.Text != "" && Convert.ToDecimal(txtSPGFlyash.Text) > 0)
                {
                    tempVal += (Flyash / Convert.ToDecimal(txtSPGFlyash.Text));
                }
                if (chkGGBSPercent.Checked == true && txtSPGGGBS.Text != "" && Convert.ToDecimal(txtSPGGGBS.Text) > 0)
                {
                    tempVal += (GGBS / Convert.ToDecimal(txtSPGGGBS.Text));
                }
                if (chkMicrosillicaPercent.Checked == true && txtSPGMicroSilica.Text != "" && Convert.ToDecimal(txtSPGMicroSilica.Text) > 0)
                {
                    tempVal += (MicroSillica / Convert.ToDecimal(txtSPGMicroSilica.Text));
                }
                tempVal += WaterDemand;
                if (txtSPGFA.Text != "")
                {
                    tempVal += (WFA * (Convert.ToDecimal(txt2p36PassingPercent.Text) / 100)) / Convert.ToDecimal(txtSPGFA.Text);
                }
                MortarVolume = tempVal;
                #endregion

                #region 13. Correct FA %
                tempVal = 0;
                decimal LL = 0, UL = 0;
                if (txtSlump.Text != "")
                {
                    int slump = Convert.ToInt32(txtSlump.Text);
                    if (slump >= 0 && slump <= 25)
                    {
                        LL = 360;
                        UL = 500;
                    }
                    else if (slump >= 26 && slump <= 50)
                    {
                        LL = 370;
                        UL = 520;
                    }
                    else if (slump >= 51 && slump <= 75)
                    {
                        LL = 390;
                        UL = 540;
                    }
                    else if (slump >= 76 && slump <= 100)
                    {
                        LL = 410;
                        UL = 560;
                    }
                    else if (slump >= 101 && slump <= 125)
                    {
                        LL = 430;
                        UL = 580;
                    }
                    //else if (slump >= 126 && slump <= 150)
                    else if (slump >= 126)
                    {
                        LL = 450;
                        UL = 600;
                    }
                }
                if (ddlType.SelectedValue == "Non Pumpable")
                {
                    LL = LL - 30;
                }
                //Add grade 
                decimal MLF = 0, MVL = 0;
                MLF = UL + grade;
                if (ddlType.SelectedValue == "Self Compacting")
                {
                    MVL = 600;
                }
                else if (ddlType.SelectedValue == "Non Pumpable")
                {
                    MVL = 470;   // 490
                    if (Convert.ToDecimal(txtSlump.Text) > 150)
                        MVL = 500;
                    else if (Convert.ToDecimal(txtSlump.Text) > 100)
                        MVL = 490;  //510
                    else if (Convert.ToDecimal(txtSlump.Text) > 70)
                        MVL = 480; //500
                }
                else
                {
                    MVL = 510;//520

                }
                //Add
                if (optFlakyness30to40.Checked == true)
                {
                    MVL = MVL + 15;
                }
                else if (optFlakynessGreaterThan40.Checked == true)
                {
                    MVL = MVL + 30;
                }

                if (ddlAggregate.SelectedValue == "10 mm"  ) 
                {
                    MVL = MVL + 30; 
                }
                else if (grade > 50 && grade < 70)
                {
                    MVL = MVL + 15;
                }
                else if (grade >= 70)
                {
                    MVL = MVL + 30;
                }
                if (txtSlump.Text != "" && ddlType.SelectedValue != "Self Compacting")
                {
                    // commented on 30 Aug 19
                    int slump = Convert.ToInt32(txtSlump.Text);
                    if (ddlType.SelectedValue  != "Non Pumpable")  // for only for pumpable
                    {
                        if (slump >= 100 && slump <= 125)
                        {
                            MVL = MVL + 5; //10
                        }
                        else if (slump >= 126 && slump <= 150)
                        {
                            MVL = MVL + 10; //20
                        }
                        else if (slump >= 151)
                        {   // changes done on 10/09/20
                            MVL = MVL + 40; // 20; //30
                        }
                    }
                }
                if (chk10mmPresent.Checked == false)
                {
                    MVL = MVL + 10; //20
                }
                //Subtract
                if (chkRoundedAggregate.Checked == true)
                {
                    //MVL = MVL - 10;
                    // changes for -20 done on 10/09/20
                    if (Convert.ToDecimal(txt2p36PassingPercent.Text) < 75)
                    {
                        MVL = MVL - 20;
                    }
                    else if (Convert.ToDecimal(txt2p36PassingPercent.Text) >= 75 && Convert.ToDecimal(txt2p36PassingPercent.Text) <= 85)
                    {
                        MVL = MVL - 10;
                    }
                }
                if (ddlAggregate.SelectedValue == "40 mm")
                {
                    MVL = MVL - 20;
                }
                //
                if (txtAdmixturePercent.Text != "")
                    AdmixturePercent2 = Convert.ToDecimal(txtAdmixturePercent.Text);
                
                #region 100
                decimal flyAsh_Extra = 0;
            Label100:
                if (W300 < LL || Math.Round(MortarVolume) < MVL)
                {
                    if ((WFA / (2500 - BinderContent - WaterDemand)) < Convert.ToDecimal(0.55) && MortarVolume < MVL)
                    {
                        WFA = WFA + 5;
                        if (AdmixturePercent2 > 0 && AdmixturePercent2 < Convert.ToDecimal(txtMaxAdmixturePercent.Text))
                        {
                            AdmixturePercent2 = AdmixturePercent2 + Convert.ToDecimal("0.02");
                        }
                        else
                        {
                            WaterDemand = WaterDemand + 1;                            
                        }                        
                    }
                    else if (chkFlyashPercent.Checked == true)
                    {
                        flyAsh_Extra = flyAsh_Extra + 5;
                    }
                    else
                    {
                        WaterDemand = WaterDemand + 1;
                    }
                    #region calculate binder
                    //Binder Content
                    tempVal = 0;
                    tempVal = WaterDemand / RevisedWaterCementRatio;
                    BinderContent = tempVal;
                    //Cement Content
                    tempVal = 1;
                    if (txtFlyashPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtFlyashPercent.Text)/ 100);
                    if (txtGGBSPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtGGBSPercent.Text) / 100);
                    if (txtMicrosillicaPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtMicrosillicaPercent.Text) / 100);
                    tempVal = BinderContent * tempVal;
                    CementContent = tempVal;
                    //Flyash
                    tempVal = 0;
                    if (txtFlyashPercent.Text != "")
                    {
                        tempVal = BinderContent * ((Convert.ToDecimal(txtFlyashPercent.Text) + flyAsh_Extra) / 100);
                    }
                    Flyash = tempVal;
                    //GGBS
                    tempVal = 0;
                    if (txtGGBSPercent.Text != "")
                    {
                        tempVal = BinderContent * (Convert.ToDecimal(txtGGBSPercent.Text) / 100);
                    }
                    GGBS = tempVal;
                    //Microsillica
                    tempVal = 0;
                    if (txtMicrosillicaPercent.Text != "")
                    {
                        tempVal = BinderContent * (Convert.ToDecimal(txtMicrosillicaPercent.Text) / 100);
                    }
                    MicroSillica = tempVal;                    
                    // Calculate mortar content - not used
                    //tempVal = 0;
                    //tempVal = (BinderContent + WFA * Convert.ToDecimal(txt4p75PassingPercent.Text)) / 2500;
                    //txtMortarContent.Text = tempVal.ToString("0.00");
                    // Calculate Mortar Volume
                    tempVal = 0;
                    if (txtSPGCement.Text != "")
                    {
                        tempVal = (CementContent / Convert.ToDecimal(txtSPGCement.Text));
                    }
                    if (chkFlyashPercent.Checked == true && txtSPGFlyash.Text != "" && Convert.ToDecimal(txtSPGFlyash.Text) > 0)
                    {
                        tempVal += (Flyash / Convert.ToDecimal(txtSPGFlyash.Text));
                    }
                    if (chkGGBSPercent.Checked == true && txtSPGGGBS.Text != "" && Convert.ToDecimal(txtSPGGGBS.Text) > 0)
                    {
                        tempVal += (GGBS / Convert.ToDecimal(txtSPGGGBS.Text));
                    }
                    if (chkMicrosillicaPercent.Checked == true && txtSPGMicroSilica.Text != "" && Convert.ToDecimal(txtSPGMicroSilica.Text) > 0)
                    {
                        tempVal += (MicroSillica / Convert.ToDecimal(txtSPGMicroSilica.Text));
                    }
                    tempVal += WaterDemand;
                    if (txtSPGFA.Text != "")
                    {
                        tempVal += (WFA * (Convert.ToDecimal(txt2p36PassingPercent.Text) / 100)) / Convert.ToDecimal(txtSPGFA.Text);
                    }
                    MortarVolume = tempVal;
                    // Calculate W300
                    tempVal = 0;
                    //tempVal = BinderContent + WFA * W300;
                    tempVal = (BinderContent + (WFA) * (Convert.ToDecimal(txt300micronPassing.Text) / 100)) + flyAsh_Extra;
                    W300 = tempVal;
                    #endregion 
                    goto Label100;
                }
                #endregion
            
                #region 200
            Label200:
                if (W300 > UL + grade || MortarVolume > MVL + 5)
                {
                    if (W300 < LL + 20 || MortarVolume < MVL + 5)
                    {
                        goto Label300;
                    }

                    WFA = WFA - 5;
                    WaterDemand = WaterDemand - 1;
                    if (AdmixturePercent2 > 0 && AdmixturePercent2 < Convert.ToDecimal(txtMaxAdmixturePercent.Text))
                    {
                        AdmixturePercent2 = AdmixturePercent2 + Convert.ToDecimal("0.02");
                        WaterDemand = WaterDemand - 1;
                    }
                    BinderContent = WaterDemand / RevisedWaterCementRatio;
                    // Calculate W300
                    tempVal = 0;
                    tempVal = BinderContent + (WFA * (Convert.ToDecimal(txt300micronPassing.Text) / 100)) + flyAsh_Extra;
                    W300 = tempVal;
                    //Cement Content
                    tempVal = 1;
                    if (txtFlyashPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtFlyashPercent.Text) / 100);
                    if (txtGGBSPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtGGBSPercent.Text) / 100);
                    if (txtMicrosillicaPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtMicrosillicaPercent.Text) / 100);
                    tempVal = BinderContent * tempVal;
                    CementContent = tempVal;
                    //Flyash
                    tempVal = 0;
                    if (txtFlyashPercent.Text != "")
                    {
                        tempVal = BinderContent * ((Convert.ToDecimal(txtFlyashPercent.Text) + flyAsh_Extra) / 100);
                    }
                    Flyash = tempVal;
                    //GGBS
                    tempVal = 0;
                    if (txtGGBSPercent.Text != "")
                    {
                        tempVal = BinderContent * (Convert.ToDecimal(txtGGBSPercent.Text) / 100);
                    }
                    GGBS = tempVal;
                    //Microsillica
                    tempVal = 0;
                    if (txtMicrosillicaPercent.Text != "")
                    {
                        tempVal = BinderContent * (Convert.ToDecimal(txtMicrosillicaPercent.Text) / 100);
                    }
                    MicroSillica = tempVal;
                    // Calculate Mortar Volume
                    tempVal = 0;
                    if (txtSPGCement.Text != "")
                    {
                        tempVal = (CementContent / Convert.ToDecimal(txtSPGCement.Text));
                    }
                    if (chkFlyashPercent.Checked == true && txtSPGFlyash.Text != "" && Convert.ToDecimal(txtSPGFlyash.Text) > 0)
                    {
                        tempVal += (Flyash / Convert.ToDecimal(txtSPGFlyash.Text));
                    }
                    if (chkGGBSPercent.Checked == true && txtSPGGGBS.Text != "" && Convert.ToDecimal(txtSPGGGBS.Text) > 0)
                    {
                        tempVal += (GGBS / Convert.ToDecimal(txtSPGGGBS.Text));
                    }
                    if (chkMicrosillicaPercent.Checked == true && txtSPGMicroSilica.Text != "" && Convert.ToDecimal(txtSPGMicroSilica.Text) > 0)
                    {
                        tempVal += (MicroSillica / Convert.ToDecimal(txtSPGMicroSilica.Text));
                    }
                    tempVal += WaterDemand;
                    if (txtSPGFA.Text != "")
                    {
                        tempVal += (WFA * (Convert.ToDecimal(txt2p36PassingPercent.Text)/100)) / Convert.ToDecimal(txtSPGFA.Text);
                    }
                    MortarVolume = tempVal;
                    
                    goto Label200;
                }
                #endregion

                #region 300
            Label300:
                if (Math.Round(MortarVolume) < MVL)
                {
                    goto Label100;
                }
                #endregion
                                                
                #region 500
                decimal Cement_Extra = 0, Ggbs_Extra = 0;
                int count = 0;
                decimal waterPowderRatio = 0;
            Label500:
                if (ddlType.SelectedValue == "Self Compacting")
                {
                    // Calculate Powder Volume
                    decimal powderVolume = 0;
                    tempVal = 0;
                    if (txtSPGCement.Text != "")
                    {
                        tempVal = (CementContent / Convert.ToDecimal(txtSPGCement.Text));
                    }
                    if (chkFlyashPercent.Checked == true && txtSPGFlyash.Text != "" && Convert.ToDecimal(txtSPGFlyash.Text) > 0)
                    {
                        tempVal += (Flyash / Convert.ToDecimal(txtSPGFlyash.Text));
                    }
                    if (chkGGBSPercent.Checked == true && txtSPGGGBS.Text != "" && Convert.ToDecimal(txtSPGGGBS.Text) > 0)
                    {
                        tempVal += (GGBS / Convert.ToDecimal(txtSPGGGBS.Text));
                    }
                    if (chkMicrosillicaPercent.Checked == true && txtSPGMicroSilica.Text != "" && Convert.ToDecimal(txtSPGMicroSilica.Text) > 0)
                    {
                        tempVal += (MicroSillica / Convert.ToDecimal(txtSPGMicroSilica.Text));
                    }
                    if (txtSPGFA.Text != "")
                    {
                        tempVal += (WFA * (Convert.ToDecimal(txt150micronPassing.Text) / 100)) / Convert.ToDecimal(txtSPGFA.Text);
                    }
                    powderVolume = tempVal;
                    //
                    if ((WaterDemand / powderVolume) > 1)
                    {
                        if (AdmixturePercent2 < Convert.ToDecimal(txtMaxAdmixturePercent.Text))
                        {
                            WaterDemand = WaterDemand - 1;
                            AdmixturePercent2 = AdmixturePercent2 + Convert.ToDecimal("0.02");
                        }
                        else if (chkGGBSPercent.Checked == true)
                        {
                            Ggbs_Extra = Ggbs_Extra + 5;
                            WaterDemand = WaterDemand + Convert.ToDecimal(0.5);
                        }
                        else if (chkFlyashPercent.Checked == true)
                        {
                            flyAsh_Extra = flyAsh_Extra + 5;
                        }
                        else
                        {
                            Cement_Extra = Cement_Extra + 5;
                            WaterDemand = WaterDemand + Convert.ToDecimal(0.2);
                        }
                    }
                    else if ((WaterDemand / powderVolume) < Convert.ToDecimal(0.6) && MortarVolume > 580)
                    {
                        if (RevisedWaterCementRatio > Convert.ToDecimal(0.35))
                        {
                            AdmixturePercent2 = AdmixturePercent2 + Convert.ToDecimal("0.02");
                            WaterDemand = WaterDemand + 1;
                        }
                        else
                        {
                            goto Label600;
                        }
                    }
                    #region calculate binder 
                    //Binder Content
                    BinderContent = WaterDemand / RevisedWaterCementRatio;
                    //Cement Content
                    tempVal = 1;
                    if (txtFlyashPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtFlyashPercent.Text) / 100);
                    if (txtGGBSPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtGGBSPercent.Text) / 100);
                    if (txtMicrosillicaPercent.Text != "")
                        tempVal = tempVal - (Convert.ToDecimal(txtMicrosillicaPercent.Text) / 100);
                    tempVal = BinderContent * tempVal;
                    CementContent = tempVal;
                    //Flyash
                    tempVal = 0;
                    if (txtFlyashPercent.Text != "")
                    {
                        tempVal = BinderContent * ((Convert.ToDecimal(txtFlyashPercent.Text) + flyAsh_Extra) / 100);
                    }
                    Flyash = tempVal;
                    //GGBS
                    tempVal = 0;
                    if (txtGGBSPercent.Text != "")
                    {
                        tempVal = BinderContent * ((Convert.ToDecimal(txtGGBSPercent.Text) + Ggbs_Extra)  / 100);
                    }
                    GGBS = tempVal;
                    //Microsillica
                    tempVal = 0;
                    if (txtMicrosillicaPercent.Text != "")
                    {
                        tempVal = BinderContent * (Convert.ToDecimal(txtMicrosillicaPercent.Text) / 100);
                    }
                    MicroSillica = tempVal;                    
                    // Calculate mortar content - not used
                    //tempVal = 0;
                    //tempVal = (BinderContent + WFA * Convert.ToDecimal(txt4p75PassingPercent.Text)) / 2500;
                    //txtMortarContent.Text = tempVal.ToString("0.00");
                    // Calculate Mortar Volume
                    tempVal = 0;
                    if (txtSPGCement.Text != "")
                    {
                        tempVal = (CementContent / Convert.ToDecimal(txtSPGCement.Text));
                    }
                    if (chkFlyashPercent.Checked == true && txtSPGFlyash.Text != "" && Convert.ToDecimal(txtSPGFlyash.Text) > 0)
                    {
                        tempVal += (Flyash / Convert.ToDecimal(txtSPGFlyash.Text));
                    }
                    if (chkGGBSPercent.Checked == true && txtSPGGGBS.Text != "" && Convert.ToDecimal(txtSPGGGBS.Text) > 0)
                    {
                        tempVal += (GGBS / Convert.ToDecimal(txtSPGGGBS.Text));
                    }
                    if (chkMicrosillicaPercent.Checked == true && txtSPGMicroSilica.Text != "" && Convert.ToDecimal(txtSPGMicroSilica.Text) > 0)
                    {
                        tempVal += (MicroSillica / Convert.ToDecimal(txtSPGMicroSilica.Text));
                    }
                    tempVal += WaterDemand;
                    if (txtSPGFA.Text != "")
                    {
                        tempVal += (WFA * (Convert.ToDecimal(txt2p36PassingPercent.Text) / 100)) / Convert.ToDecimal(txtSPGFA.Text);
                    }
                    MortarVolume = tempVal;
                    // Calculate W300
                    tempVal = 0;
                    tempVal = BinderContent + (WFA * (Convert.ToDecimal(txt300micronPassing.Text) / 100)) + flyAsh_Extra + Ggbs_Extra + Cement_Extra;
                    W300 = tempVal;
                    #endregion
                    waterPowderRatio = WaterDemand / powderVolume;
                    count++;
                    if (count >= 20)
                    {
                        goto Label600;
                    }
                    if (((WaterDemand / powderVolume) > 1) ||
                        ((WaterDemand / powderVolume) < Convert.ToDecimal(0.6) && MortarVolume > 580))
                    {
                        goto Label500;
                    }
                }
                #endregion

            Label600:
                BinderContent = BinderContent + flyAsh_Extra + Ggbs_Extra + Cement_Extra;
                Flyash = Flyash + flyAsh_Extra;
                GGBS = GGBS + Ggbs_Extra;
                CementContent = CementContent + Cement_Extra;

                #endregion

                #region 14. Calculate Weight of CA
                tempVal = 0;
                tempVal = 990;
                if (txtSPGCement.Text != "")
                {
                    tempVal -= (CementContent / Convert.ToDecimal(txtSPGCement.Text));
                }
                if (chkFlyashPercent.Checked == true && txtSPGFlyash.Text != "" && Convert.ToDecimal(txtSPGFlyash.Text) > 0)
                {
                    tempVal -= (Flyash / Convert.ToDecimal(txtSPGFlyash.Text));
                }
                if (chkGGBSPercent.Checked == true && txtSPGGGBS.Text != "" && Convert.ToDecimal(txtSPGGGBS.Text) > 0)
                {
                    tempVal -= (GGBS / Convert.ToDecimal(txtSPGGGBS.Text));
                }
                if (chkMicrosillicaPercent.Checked == true && txtSPGMicroSilica.Text != "" && Convert.ToDecimal(txtSPGMicroSilica.Text) > 0)
                {
                    tempVal -= (MicroSillica / Convert.ToDecimal(txtSPGMicroSilica.Text));
                }
                tempVal -= WaterDemand;
                if (txtSPGFA.Text != "")
                {
                    tempVal -= WFA / Convert.ToDecimal(txtSPGFA.Text);
                }
                tempVal = tempVal * Convert.ToDecimal(txtSPGAggt.Text);
                WeightOfCA = tempVal;

                tempVal = 0;
                tempVal = (WFA / (WFA + WeightOfCA)) * 100;
                FAPercent = tempVal;

                tempVal = 0;
                tempVal = (WeightOfCA / (WFA + WeightOfCA)) * 100;
                CAPercent = tempVal;

                #endregion

                #region 16. plastic density
                tempVal = 0;
                tempVal = WFA + WeightOfCA + BinderContent + WaterDemand;
                PlasticDensity = tempVal;

                tempVal = 0;
                decimal percent40mm = 0, percent20mm = 0, percent10mm = 0;
                if (ddlAggregate.SelectedValue == "10 mm")
                {
                    percent10mm = 100;
                }
                else if (ddlAggregate.SelectedValue == "20 mm" && chk10mmPresent.Checked == false)
                {
                    percent20mm = 100;
                }
                else if (ddlAggregate.SelectedValue == "20 mm" && chk10mmPresent.Checked == true)
                {
                    switch (ddlGrade.SelectedValue)
                    {
                        case "M 5":
                        case "M 10":
                        case "M 15":
                        case "M 20":
                            percent10mm = 35; // 25;
                            percent20mm = 65; // 75;
                            break;
                        case "M 25":
                            percent10mm = 35; // 30 ;
                            percent20mm = 65;  // 70;
                            break;
                        case "M 30":
                            percent10mm = 40;// 35;
                            percent20mm = 60;// 65;
                            break;
                        case "M 35":
                            percent10mm = 40;
                            percent20mm = 60;
                            break;
                        case "M 40":
                            percent10mm = 45;
                            percent20mm = 55;
                            break;
                        case "M 45":
                            percent10mm = 50;
                            percent20mm = 50;
                            break;
                        case "M 50":
                            percent10mm = 55;
                            percent20mm = 45;
                            break;
                        case "M 55":
                            percent10mm = 60;
                            percent20mm = 40;
                            break;
                        case "M 60":
                            percent10mm = 65;
                            percent20mm = 35;
                            break;
                        case "M 65":
                            percent10mm = 70;
                            percent20mm = 30;
                            break;
                        case "M 70":
                            percent10mm = 75;
                            percent20mm = 25;
                            break;
                        case "M 75":
                            percent10mm = 100;
                            break;
                        case "M 80":
                            percent10mm = 100;
                            break;
                        case "M 85":
                            percent10mm = 100;
                            break;
                        case "M 90":
                            percent10mm = 100;
                            break;
                        case "M 95":
                            percent10mm = 100;
                            break;
                        case "M 100":
                            percent10mm = 100;
                            break;
                    }
                    if (ddlType.SelectedValue == "Self Compacting")
                    {
                        if (percent10mm < 70)
                        {
                            percent10mm = 70;
                            percent20mm = 30;
                        }
                    }
                }
                else if (ddlAggregate.SelectedValue == "40 mm")
                {
                    switch (ddlGrade.SelectedValue)
                    {
                        case "M 5":
                        case "M 10":
                        case "M 15":
                        case "M 20":
                            percent10mm = 15;
                            percent20mm = 35;
                            percent40mm = 50;
                            break;
                        case "M 25":
                            percent10mm = 20;
                            percent20mm = 35;
                            percent40mm = 45;
                            break;
                        case "M 30":
                            percent10mm = 25;
                            percent20mm = 35;
                            percent40mm = 40;
                            break;
                        case "M 35":
                            percent10mm = 30;
                            percent20mm = 40;
                            percent40mm = 30;
                            break;
                    }
                }
                tempVal = 0;
                tempVal = WeightOfCA * (percent40mm / 100);
                w40mm = tempVal;

                tempVal = 0;
                tempVal = WeightOfCA * (percent20mm / 100);
                w20mm = tempVal;

                tempVal = 0;
                tempVal = WeightOfCA * (percent10mm / 100);
                w10mm = tempVal;

                tempVal = (100 - FAPercent) * (percent10mm / 100);
                percent10mm = Math.Round(tempVal, 2);
                tempVal = (100 - FAPercent) * (percent20mm / 100);
                percent20mm = Math.Round(tempVal, 2);
                tempVal = (100 - FAPercent) * (percent40mm / 100);
                percent40mm = Math.Round(tempVal, 2);
                #endregion

                #region display result
                txtResultPlasticDensity.Text = PlasticDensity.ToString("0.00");
                txtResultWCRatio.Text = RevisedWaterCementRatio.ToString("0.00");
                txtResultWater.Text = WaterDemand.ToString("0.00");
                txtResultBinderContent.Text = BinderContent.ToString("0.00");
                txtResultCement.Text = CementContent.ToString("0.00");
                if (txtFlyashPercent.Text != "")
                    txtResultFlyashPercent.Text = txtFlyashPercent.Text;
                else
                    txtResultFlyashPercent.Text = "0";
                txtResultFlyash.Text = Flyash.ToString("0.00");
                if (txtGGBSPercent.Text != "")
                    txtResultGGBSPercent.Text = txtGGBSPercent.Text;
                else
                    txtResultGGBSPercent.Text = "0";
                txtResultGGBS.Text = GGBS.ToString("0.00");
                if (txtMicrosillicaPercent.Text != "")
                    txtResultMicrosilicaPercent.Text = txtMicrosillicaPercent.Text;
                else
                    txtResultMicrosilicaPercent.Text = "0";
                txtResultMicrosilica.Text = MicroSillica.ToString("0.00");
                tempVal = 0;
                if (txtResultFlyashPercent.Text != "")
                    tempVal += Convert.ToDecimal(txtResultFlyashPercent.Text);
                if (txtResultGGBSPercent.Text != "")
                    tempVal += Convert.ToDecimal(txtResultGGBSPercent.Text);
                if (txtResultMicrosilicaPercent.Text != "")
                    tempVal += Convert.ToDecimal(txtResultMicrosilicaPercent.Text);

                txtResultCementPercent.Text = (100 - tempVal).ToString("0.00");
                txtResultAdmixturePercent.Text = AdmixturePercent2.ToString("0.00");
                txtResultAdmixture.Text = ((AdmixturePercent2 * Convert.ToDecimal(txtResultBinderContent.Text)) / 100).ToString("0.00");
                txtResultFAPercent.Text = FAPercent.ToString("0.00");
                txtResultFA1.Text = (WFA * (Convert.ToDecimal(txtPercentageFA1.Text) / 100)).ToString("0.00");
                txtResultFA2.Text = (WFA * (Convert.ToDecimal(txtPercentageFA2.Text) / 100)).ToString("0.00");
                txtResult10mmPercent.Text = percent10mm.ToString("0.00");
                txtResult10mm.Text = w10mm.ToString("0.00");
                txtResult20mmPercent.Text = percent20mm.ToString("0.00");
                txtResult20mm.Text = w20mm.ToString("0.00");
                txtResult40mmPercent.Text = percent40mm.ToString("0.00");
                txtResult40mm.Text = w40mm.ToString("0.00");
                txtResultW300.Text = W300.ToString("0.00");
                txtResultMortarVolume.Text = (MortarVolume / 10).ToString("0.00");
                txtResultWaterPowderRatio.Text = waterPowderRatio.ToString("0.00");
                #endregion
            }
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (ddlReferenceNo.SelectedIndex == 0)
            {
                lblMsg.Text = "Select report number.";
                lblMsg.Visible = true;
            }
            else
            {
                ClearAllControls();
                LoadTrialList();
            }
        }

        private void ClearAllControls()
        {
            ddlGrade.SelectedIndex = 0;
            ddlAggregate.SelectedIndex = 0;
            optFlakynessLessThan30.Checked = false;
            optFlakyness30to40.Checked = false;
            optFlakynessGreaterThan40.Checked = false;
            txtSlump.Text = "";
            chkFlyashPercent.Checked = false;
            txtFlyashPercent.Text = "";
            chkGGBSPercent.Checked = false;
            txtGGBSPercent.Text = "";
            chkMicrosillicaPercent.Checked = false;
            txtMicrosillicaPercent.Text = "";
            txtAdmixturePercent.Text = "";
            txtMaxAdmixturePercent.Text = "";
            ddlReductionType.SelectedIndex = 0;
            txt600MicronPercent.Text = "";
            txt4p75PassingPercent.Text = "";
            txt300micronPassing.Text = "";
            txt150micronPassing.Text = "";
            txtSPGCement.Text = "";
            txtSPGFlyash.Text = "";
            txtSPGGGBS.Text = "";
            txtSPGMicroSilica.Text = "";
            txtSPGFA.Text = "";
            txtSPGAggt.Text = "";
            txt2p36PassingPercent.Text = "";
            chkRoundedAggregate.Checked = false;
            chkNaturalSand.Checked = false;
            chk10mmPresent.Checked = true;

            txtResultPlasticDensity.Text = "";
            txtResultWCRatio.Text = "";
            txtResultWater.Text = "";
            txtResultWaterUnitCost.Text = "";
            txtResultWaterCost.Text = "";
            txtResultBinderContent.Text = "";
            txtResultCementPercent.Text = "";
            txtResultCement.Text = "";
            txtResultCementUnitCost.Text = "";
            txtResultCementCost.Text = "";
            txtResultFlyashPercent.Text = "";
            txtResultFlyash.Text = "";
            txtResultFlyashUnitCost.Text = "";
            txtResultFlyashCost.Text = "";
            txtResultGGBSPercent.Text = "";
            txtResultGGBS.Text = "";
            txtResultGGBSUnitCost.Text = "";
            txtResultGGBSCost.Text = "";
            txtResultMicrosilicaPercent.Text = "";
            txtResultMicrosilica.Text = "";
            txtResultMicrosilicaUnitCost.Text = "";
            txtResultMicrosilicaCost.Text = "";
            txtResultAdmixturePercent.Text = "";
            txtResultAdmixture.Text = "";
            txtResultAdmixtureUnitCost.Text = "";
            txtResultAdmixtureCost.Text = "";
            txtResultFAPercent.Text = "";
            txtResultFA1.Text = "";
            txtResultFA1UnitCost.Text = "";
            txtResultFA1Cost.Text = "";
            txtResultFA2.Text = "";
            txtResultFA2UnitCost.Text = "";
            txtResultFA2Cost.Text = "";
            txtResult10mmPercent.Text = "";
            txtResult10mm.Text = "";
            txtResult10mmUnitCost.Text = "";
            txtResult10mmCost.Text = "";
            txtResult20mmPercent.Text = "";
            txtResult20mm.Text = "";
            txtResult20mmUnitCost.Text = "";
            txtResult20mmCost.Text = "";
            txtResult40mmPercent.Text = "";
            txtResult40mm.Text = "";
            txtResult40mmUnitCost.Text = "";
            txtResult40mmCost.Text = "";
            txtResultW300.Text = "";
            txtTotalCost.Text = "";
            txtResultMortarVolume.Text = "";
            txtResultWaterPowderRatio.Text = "";
            ddlCementType.SelectedIndex = 0;
            txtPercentageFA1.Text = "100";
            txtPercentageFA2.Text = "0";
            chkApprove.Checked = false;
        }

        protected void ddlReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReferenceNo.SelectedIndex > 0)
            {
                ClearAllControls();
                LoadTrialList();
                LoadDetails();
                pnlDetails.Visible = true;
            }
            else
            {
                pnlDetails.Visible = false;
            }
        }

        protected void ddlTrial_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            LoadDetails();
        }

        private void LoadDetails()
        {
            //int NaturalSandMaterialId = 0, CrushedSandMaterialId = 0;
            int trialId = Convert.ToInt32(ddlTrial.SelectedValue);
            if (trialId == 0)
            {
                //load data from inward details
                var mixdesign = dc.MixDesign_View(ddlReferenceNo.SelectedValue, 1);
                foreach (var m in mixdesign)
                {
                    if (m.MFINWD_Grade_var =="M 37")
                        ddlGrade.SelectedValue = "M 40";
                    else
                        ddlGrade.SelectedValue = m.MFINWD_Grade_var;
                    if (m.TEST_Sr_No == 4 || m.TEST_Sr_No == 8)
                        ddlType.SelectedIndex = 0;
                    else if (m.TEST_Sr_No == 1 || m.TEST_Sr_No == 5)
                        ddlType.SelectedIndex = 1;
                    else if (m.TEST_Sr_No == 2 || m.TEST_Sr_No == 6)
                        ddlType.SelectedIndex = 2; 
                    txtSlump.Text = m.MFINWD_Slump_var;
                }
                var material = dc.MaterialListView(ddlReferenceNo.SelectedValue, "", "Aggregate");
                foreach (var mt in material)
                {
                    if (mt.Material_List == "40 mm")
                    {
                        ddlAggregate.SelectedValue = "40 mm";
                    }
                    else if (mt.Material_List == "20 mm" && ddlAggregate.SelectedValue != "40 mm")
                    {
                        ddlAggregate.SelectedValue = "20 mm";
                    }
                    else if (mt.Material_List == "10 mm" && ddlAggregate.SelectedValue != "20 mm")
                    {
                        ddlAggregate.SelectedValue = "10 mm";
                    }
                }
                txt4p75PassingPercent.Text = "0";
                txt2p36PassingPercent.Text = "0";
                txt600MicronPercent.Text = "0";
                txt300micronPassing.Text = "0";
                txt150micronPassing.Text = "0";
                txtPercentageFA1.Text = "100";
                txtPercentageFA2.Text = "0"; 
                
                string strCementUsed = "";
                var mat = dc.MaterialDetail_View(0, ddlReferenceNo.SelectedValue, 0, "Cement", null, null, "");
                foreach (var m in mat)
                {
                    if (Convert.ToString(m.MaterialDetail_Information) != "")
                    {
                        strCementUsed = m.MaterialDetail_Information.ToString();
                    }
                }
                if (strCementUsed.Contains("1489") == true || strCementUsed.Contains("455") == true ||
                    strCementUsed.Contains("PPC") == true || strCementUsed.Contains("PSC") == true)
                {
                    txtSPGCement.Text = "2.97";
                }
                else if (strCementUsed.Contains("8112") == true || strCementUsed.Contains("12269") == true ||
                    strCementUsed.Contains("43") == true || strCementUsed.Contains("53") == true)
                {
                    txtSPGCement.Text = "3.15";
                }
                txtSPGFlyash.Text = "2.26";
                txtSPGMicroSilica.Text = "2.20";
                txtSPGGGBS.Text = "2.90";
                
                txtSPGFA.Text = "0";
                txtSPGAggt.Text = "0";
                FetchFAPercentWise();

                //load saved data from mix design cost table if available
                var trialCost = dc.MixDesign_Cost_View(ddlReferenceNo.SelectedValue).ToList();
                if (trialCost.Count > 0)
                {
                    foreach (var trlc in trialCost)
                    {
                        txtResultWaterUnitCost.Text = trlc.MFCost_Water_dec.ToString();
                        txtResultCementUnitCost.Text = trlc.MFCost_Cement_dec.ToString();
                        txtResultFlyashUnitCost.Text = trlc.MFCost_Flyash_dec.ToString();
                        txtResultGGBSUnitCost.Text = trlc.MFCost_GGBS_dec.ToString();
                        txtResultMicrosilicaUnitCost.Text = trlc.MFCost_Microsilica_dec.ToString();
                        txtResultAdmixtureUnitCost.Text = trlc.MFCost_Admixture_dec.ToString();
                        txtResultFA1UnitCost.Text = trlc.MFCost_FineAggregate1_dec.ToString();
                        txtResultFA2UnitCost.Text = trlc.MFCost_FineAggregate2_dec.ToString();
                        txtResult10mmUnitCost.Text = trlc.MFCost_10mm_dec.ToString();
                        txtResult20mmUnitCost.Text = trlc.MFCost_20mm_dec.ToString();
                        txtResult40mmUnitCost.Text = trlc.MFCost_40mm_dec.ToString();
                    }
                }
                else
                {
                    //load saved data from mix design cost master table if available
                    var masterCost = dc.MixDesign_CostMaster_View().ToList();
                    foreach (var mct in masterCost)
                    {
                        txtResultWaterUnitCost.Text = mct.MFCostM_Water_dec.ToString();
                        txtResultCementUnitCost.Text = mct.MFCostM_Cement_dec.ToString();
                        txtResultFlyashUnitCost.Text = mct.MFCostM_Flyash_dec.ToString();
                        txtResultGGBSUnitCost.Text = mct.MFCostM_GGBS_dec.ToString();
                        txtResultMicrosilicaUnitCost.Text = mct.MFCostM_Microsilica_dec.ToString();
                        txtResultAdmixtureUnitCost.Text = mct.MFCostM_Admixture_dec.ToString();
                        txtResultFA1UnitCost.Text = mct.MFCostM_FineAggregate1_dec.ToString();
                        txtResultFA2UnitCost.Text = mct.MFCostM_FineAggregate2_dec.ToString();
                        txtResult10mmUnitCost.Text = mct.MFCostM_10mm_dec.ToString();
                        txtResult20mmUnitCost.Text = mct.MFCostM_20mm_dec.ToString();
                        txtResult40mmUnitCost.Text = mct.MFCostM_40mm_dec.ToString();
                    }
                }
            }
            else
            {
                //load saved data from mix design system table
                var trial = dc.MixDesign_System_View(ddlReferenceNo.SelectedValue, Convert.ToInt32(ddlTrial.SelectedValue), "", false).ToList();
                foreach (var trl in trial)
                {
                    //input
                    ddlGrade.SelectedValue = trl.MFSys_Grade_var;
                    txt4p75PassingPercent.Text = trl.MFSys_4p75Percent_dec.ToString();
                    ddlType.SelectedValue = trl.MFSys_MFType_var;
                    txt2p36PassingPercent.Text = trl.MFSys_2p36Percent_dec.ToString();
                    txtSlump.Text = trl.MFSys_Slump_int.ToString();
                    if (trl.MFSys_CementType_var != null)
                        ddlCementType.SelectedValue = trl.MFSys_CementType_var;
                    txtPercentageFA1.Text = trl.MFSys_FA1Percent_dec.ToString();
                    txtPercentageFA2.Text = trl.MFSys_FA2Percent_dec.ToString();
                    txt600MicronPercent.Text = trl.MFSys_600MicronPercent_dec.ToString();
                    if (trl.MFSys_FlyashPercent_dec > 0)
                        chkFlyashPercent.Checked = true;
                    txtFlyashPercent.Text = trl.MFSys_FlyashPercent_dec.ToString();
                    txt300micronPassing.Text = trl.MFSys_300MicronPercent_dec.ToString();
                    txt150micronPassing.Text = trl.MFSys_150MicronPercent_dec.ToString();
                    if (trl.MFSys_GGBSPercent_dec > 0)
                        chkGGBSPercent.Checked = true;
                    txtGGBSPercent.Text = trl.MFSys_GGBSPercent_dec.ToString();
                    txtSPGCement.Text = trl.MFSys_SPGCement_dec.ToString();
                    if (trl.MFSys_MicrosilicaPercent_dec > 0)
                        chkMicrosillicaPercent.Checked = true;
                    txtMicrosillicaPercent.Text = trl.MFSys_MicrosilicaPercent_dec.ToString();
                    txtSPGFlyash.Text = trl.MFSys_SPGFlyash_dec.ToString();
                    ddlAggregate.SelectedValue = trl.MFSys_AggregateSize_var;
                    txtSPGGGBS.Text = trl.MFSys_SPGGGBS_dec.ToString();
                    if (trl.MFSys_Flakyness_var == "LessThan30")
                        optFlakynessLessThan30.Checked = true;
                    else if (trl.MFSys_Flakyness_var == "30to40")
                        optFlakyness30to40.Checked = true;
                    else if (trl.MFSys_Flakyness_var == "GreaterThan40")
                        optFlakynessGreaterThan40.Checked = true;
                    txtSPGMicroSilica.Text = trl.MFSys_SPGMicrosilica_dec.ToString();
                    if (trl.MFSys_AggregateType_var.Contains("RoundedAggregate") == true)
                        chkRoundedAggregate.Checked = true;
                    if (trl.MFSys_AggregateType_var.Contains("NaturalSand") == true)
                        chkNaturalSand.Checked = true;
                    if (trl.MFSys_10mmPresent_bit == true)
                        chk10mmPresent.Checked = true;
                    else
                        chk10mmPresent.Checked = false;
                    txtSPGFA.Text = trl.MFSys_SPGFineAggregate_dec.ToString();
                    txtAdmixturePercent.Text = trl.MFSys_AdmixturePercent_dec.ToString();
                    txtMaxAdmixturePercent.Text = trl.MFSys_MaxAdmixturePercent_dec.ToString();
                    txtSPGAggt.Text = trl.MFSys_SPGAggregate_dec.ToString();
                    ddlReductionType.SelectedValue = trl.MFSys_ReductionType_var;

                    //Result
                    txtResultPlasticDensity.Text = trl.MFSys_ResultPlasticDensity_dec.ToString();
                    txtResultWCRatio.Text = trl.MFSys_ResultWCRatio_dec.ToString();
                    txtResultWater.Text = trl.MFSys_ResultWater_dec.ToString();
                    txtResultWaterUnitCost.Text = "";
                    txtResultWaterCost.Text = trl.MFSys_ResultWaterCost_dec.ToString();
                    txtResultBinderContent.Text = trl.MFSys_ResultBinderContent_dec.ToString();
                    txtResultCementPercent.Text = trl.MFSys_ResultCementPercent_dec.ToString();
                    txtResultCement.Text = trl.MFSys_ResultCement_dec.ToString();
                    txtResultCementUnitCost.Text = "";
                    txtResultCementCost.Text = trl.MFSys_ResultCementCost_dec.ToString();
                    txtResultFlyashPercent.Text = trl.MFSys_ResultFlyashPercent_dec.ToString();
                    txtResultFlyash.Text = trl.MFSys_ResultFlyash_dec.ToString();
                    txtResultFlyashUnitCost.Text = "";
                    txtResultFlyashCost.Text = trl.MFSys_ResultFlyashCost_dec.ToString();
                    txtResultGGBSPercent.Text = trl.MFSys_ResultGGBSPercent_dec.ToString();
                    txtResultGGBS.Text = trl.MFSys_ResultGGBS_dec.ToString();
                    txtResultGGBSUnitCost.Text = "";
                    txtResultGGBSCost.Text = trl.MFSys_ResultGGBSCost_dec.ToString();
                    txtResultMicrosilicaPercent.Text = trl.MFSys_ResultMicrosilicaPercent_dec.ToString();
                    txtResultMicrosilica.Text = trl.MFSys_ResultMicrosilica_dec.ToString();
                    txtResultMicrosilicaUnitCost.Text = "";
                    txtResultMicrosilicaCost.Text = trl.MFSys_ResultMicrosilicaCost_dec.ToString();
                    txtResultAdmixturePercent.Text = trl.MFSys_ResultAdmixtureDosage_dec.ToString();
                    txtResultAdmixture.Text = trl.MFSys_ResultAdmixture_dec.ToString();
                    txtResultAdmixtureUnitCost.Text = "";
                    txtResultAdmixtureCost.Text = trl.MFSys_ResultAdmixtureCost_dec.ToString();
                    txtResultFAPercent.Text = trl.MFSys_ResultFineAggregatePercent_dec.ToString();
                    txtResultFA1.Text = trl.MFSys_ResultFineAggregate1_dec.ToString();
                    txtResultFA1UnitCost.Text = "";
                    txtResultFA1Cost.Text = trl.MFSys_ResultFineAggregate1Cost_dec.ToString();
                    txtResultFA2.Text = trl.MFSys_ResultFineAggregate2_dec.ToString();
                    txtResultFA2UnitCost.Text = "";
                    txtResultFA2Cost.Text = trl.MFSys_ResultFineAggregate2Cost_dec.ToString();
                    txtResult10mmPercent.Text = trl.MFSys_Result10mmPercent_dec.ToString();
                    txtResult10mm.Text = trl.MFSys_Result10mm_dec.ToString();
                    txtResult10mmUnitCost.Text = "";
                    txtResult10mmCost.Text = trl.MFSys_Result10mmCost_dec.ToString();
                    txtResult20mmPercent.Text = trl.MFSys_Result20mmPercent_dec.ToString();
                    txtResult20mm.Text = trl.MFSys_Result20mm_dec.ToString();
                    txtResult20mmUnitCost.Text = "";
                    txtResult20mmCost.Text = trl.MFSys_Result20mmCost_dec.ToString();
                    txtResult40mmPercent.Text = trl.MFSys_Result40mmPercent_dec.ToString();
                    txtResult40mm.Text = trl.MFSys_Result40mm_dec.ToString();
                    txtResult40mmCost.Text = trl.MFSys_Result40mmCost_dec.ToString();
                    txtResultW300.Text = trl.MFSys_ResultW300_dec.ToString();
                    txtTotalCost.Text = trl.MFSys_TotalCost_dec.ToString();
                    txtResultMortarVolume.Text = trl.MFSys_ResultMortarVolumePercent_dec.ToString();
                    txtResultWaterPowderRatio.Text = trl.MFSys_ResultWaterPowderRatio_dec.ToString();
                    chkApprove.Checked = Convert.ToBoolean(trl.MFSys_ApproveStatus_bit);
                }
                //load saved data from mix design cost table
                var trialCost = dc.MixDesign_Cost_View(ddlReferenceNo.SelectedValue).ToList();
                foreach (var trlc in trialCost)
                {
                    txtResultWaterUnitCost.Text = trlc.MFCost_Water_dec.ToString();
                    txtResultCementUnitCost.Text = trlc.MFCost_Cement_dec.ToString();
                    txtResultFlyashUnitCost.Text = trlc.MFCost_Flyash_dec.ToString();
                    txtResultGGBSUnitCost.Text = trlc.MFCost_GGBS_dec.ToString();
                    txtResultMicrosilicaUnitCost.Text = trlc.MFCost_Microsilica_dec.ToString();
                    txtResultAdmixtureUnitCost.Text = trlc.MFCost_Admixture_dec.ToString();
                    txtResultFA1UnitCost.Text = trlc.MFCost_FineAggregate1_dec.ToString();
                    txtResultFA2UnitCost.Text = trlc.MFCost_FineAggregate2_dec.ToString();
                    txtResult10mmUnitCost.Text = trlc.MFCost_10mm_dec.ToString();
                    txtResult20mmUnitCost.Text = trlc.MFCost_20mm_dec.ToString();
                    txtResult40mmUnitCost.Text = trlc.MFCost_40mm_dec.ToString();
                }
                Calculate();
                CalculateIndividualCost();
                CalculateTotalCost();
            }
        }
        
        private void CalculateIndividualCost()
        {
            txtResultWaterCost.Text = "0.00";
            if (txtResultWater.Text != "" && txtResultWaterUnitCost.Text != "")
            {
                txtResultWaterCost.Text = (Convert.ToDecimal(txtResultWater.Text) * Convert.ToDecimal(txtResultWaterUnitCost.Text)).ToString("0.00");
            }
            txtResultCementCost.Text = "0.00";
            if (txtResultCement.Text != "" && txtResultCementUnitCost.Text != "")
            {
                txtResultCementCost.Text = (Convert.ToDecimal(txtResultCement.Text) * Convert.ToDecimal(txtResultCementUnitCost.Text)).ToString("0.00");
            }
            txtResultFlyashCost.Text = "0.00";
            if (txtResultFlyash.Text != "" && txtResultFlyashUnitCost.Text != "")
            {
                txtResultFlyashCost.Text = (Convert.ToDecimal(txtResultFlyash.Text) * Convert.ToDecimal(txtResultFlyashUnitCost.Text)).ToString("0.00");
            }
            txtResultGGBSCost.Text = "0.00";
            if (txtResultGGBS.Text != "" && txtResultGGBSUnitCost.Text != "")
            {
                txtResultGGBSCost.Text = (Convert.ToDecimal(txtResultGGBS.Text) * Convert.ToDecimal(txtResultGGBSUnitCost.Text)).ToString("0.00");
            }
            txtResultMicrosilicaCost.Text = "0.00";
            if (txtResultMicrosilica.Text != "" && txtResultMicrosilicaUnitCost.Text != "")
            {
                txtResultMicrosilicaCost.Text = (Convert.ToDecimal(txtResultMicrosilica.Text) * Convert.ToDecimal(txtResultMicrosilicaUnitCost.Text)).ToString("0.00");
            }
            txtResultAdmixtureCost.Text = "0.00";
            if (txtResultAdmixture.Text != "" && txtResultAdmixtureUnitCost.Text != "")
            {
                txtResultAdmixtureCost.Text = (Convert.ToDecimal(txtResultAdmixture.Text) * Convert.ToDecimal(txtResultAdmixtureUnitCost.Text)).ToString("0.00");
            }
            txtResultFA1Cost.Text = "0.00";
            if (txtResultFA1.Text != "" && txtResultFA1UnitCost.Text != "")
            {
                txtResultFA1Cost.Text = (Convert.ToDecimal(txtResultFA1.Text) * Convert.ToDecimal(txtResultFA1UnitCost.Text)).ToString("0.00");
            }
            txtResultFA2Cost.Text = "0.00";
            if (txtResultFA2.Text != "" && txtResultFA2UnitCost.Text != "")
            {
                txtResultFA2Cost.Text = (Convert.ToDecimal(txtResultFA2.Text) * Convert.ToDecimal(txtResultFA2UnitCost.Text)).ToString("0.00");
            }
            txtResult10mmCost.Text = "0.00";
            if (txtResult10mm.Text != "" && txtResult10mmUnitCost.Text != "")
            {
                txtResult10mmCost.Text = (Convert.ToDecimal(txtResult10mm.Text) * Convert.ToDecimal(txtResult10mmUnitCost.Text)).ToString("0.00");
            }
            txtResult20mmCost.Text = "0.00";
            if (txtResult20mm.Text != "" && txtResult20mmUnitCost.Text != "")
            {
                txtResult20mmCost.Text = (Convert.ToDecimal(txtResult20mm.Text) * Convert.ToDecimal(txtResult20mmUnitCost.Text)).ToString("0.00");
            }
            txtResult40mmCost.Text = "0.00";
            if (txtResult40mm.Text != "" && txtResult40mmUnitCost.Text != "")
            {
                txtResult40mmCost.Text = (Convert.ToDecimal(txtResult40mm.Text) * Convert.ToDecimal(txtResult40mmUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        private void CalculateTotalCost()
        {
            txtTotalCost.Text = "0.00";
            decimal totalCost = 0;
            if (txtResultWaterCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultWaterCost.Text);
            }
            if (txtResultCementCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultCementCost.Text);
            }
            if (txtResultFlyashCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultFlyashCost.Text);
            }
            if (txtResultGGBSCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultGGBSCost.Text);
            }
            if (txtResultMicrosilicaCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultMicrosilicaCost.Text);
            }
            if (txtResultAdmixtureCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultAdmixtureCost.Text);
            }
            if (txtResultFA1Cost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultFA1Cost.Text);
            }
            if (txtResultFA2Cost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResultFA2Cost.Text);
            }
            if (txtResult10mmCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResult10mmCost.Text);
            }
            if (txtResult20mmCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResult20mmCost.Text);
            }
            if (txtResult40mmCost.Text != "")
            {
                totalCost += Convert.ToDecimal(txtResult40mmCost.Text);
            }
            txtTotalCost.Text = totalCost.ToString("0.00");
        }

        protected void txtResultWaterUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultWaterCost.Text = "0.00";
            if (txtResultWater.Text != "" && txtResultWaterUnitCost.Text != "")
            {
                txtResultWaterCost.Text = (Convert.ToDecimal(txtResultWater.Text) * Convert.ToDecimal(txtResultWaterUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResultCementUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultCementCost.Text = "0.00";
            if (txtResultCement.Text != "" && txtResultCementUnitCost.Text != "")
            {
                txtResultCementCost.Text = (Convert.ToDecimal(txtResultCement.Text) * Convert.ToDecimal(txtResultCementUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResultFlyashUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultFlyashCost.Text = "0.00";
            if (txtResultFlyash.Text != "" && txtResultFlyashUnitCost.Text != "")
            {
                txtResultFlyashCost.Text = (Convert.ToDecimal(txtResultFlyash.Text) * Convert.ToDecimal(txtResultFlyashUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResultGGBSUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultGGBSCost.Text = "0.00";
            if (txtResultGGBS.Text != "" && txtResultGGBSUnitCost.Text != "")
            {
                txtResultGGBSCost.Text = (Convert.ToDecimal(txtResultGGBS.Text) * Convert.ToDecimal(txtResultGGBSUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResultMicrosilicaUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultMicrosilicaCost.Text = "0.00";
            if (txtResultMicrosilica.Text != "" && txtResultMicrosilicaUnitCost.Text != "")
            {
                txtResultMicrosilicaCost.Text = (Convert.ToDecimal(txtResultMicrosilica.Text) * Convert.ToDecimal(txtResultMicrosilicaUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResultAdmixtureUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultAdmixtureCost.Text = "0.00";
            if (txtResultAdmixture.Text != "" && txtResultAdmixtureUnitCost.Text != "")
            {
                txtResultAdmixtureCost.Text = (Convert.ToDecimal(txtResultAdmixture.Text) * Convert.ToDecimal(txtResultAdmixtureUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResultFA1UnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultFA1Cost.Text = "0.00";
            if (txtResultFA1.Text != "" && txtResultFA1UnitCost.Text != "")
            {
                txtResultFA1Cost.Text = (Convert.ToDecimal(txtResultFA1.Text) * Convert.ToDecimal(txtResultFA1UnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResultFA2UnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResultFA2Cost.Text = "0.00";
            if (txtResultFA2.Text != "" && txtResultFA2UnitCost.Text != "")
            {
                txtResultFA2Cost.Text = (Convert.ToDecimal(txtResultFA2.Text) * Convert.ToDecimal(txtResultFA2UnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResult10mmUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResult10mmCost.Text = "0.00";
            if (txtResult10mm.Text != "" && txtResult10mmUnitCost.Text != "")
            {
                txtResult10mmCost.Text = (Convert.ToDecimal(txtResult10mm.Text) * Convert.ToDecimal(txtResult10mmUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResult20mmUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResult20mmCost.Text = "0.00";
            if (txtResult20mm.Text != "" && txtResult20mmUnitCost.Text != "")
            {
                txtResult20mmCost.Text = (Convert.ToDecimal(txtResult20mm.Text) * Convert.ToDecimal(txtResult20mmUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void txtResult40mmUnitCost_TextChanged(object sender, EventArgs e)
        {
            txtResult40mmCost.Text = "0.00";
            if (txtResult40mm.Text != "" && txtResult40mmUnitCost.Text != "")
            {
                txtResult40mmCost.Text = (Convert.ToDecimal(txtResult40mm.Text) * Convert.ToDecimal(txtResult40mmUnitCost.Text)).ToString("0.00");
            }
            CalculateTotalCost();
        }

        protected void FetchFAPercentWise()
        {
            decimal percent40mm = 0, percent20mm = 0, percent10mm = 0;
            if (ddlAggregate.SelectedValue == "10 mm")
            {
                percent10mm = 100;
            }
            else if (ddlAggregate.SelectedValue == "20 mm" && chk10mmPresent.Checked == false)
            {
                percent20mm = 100;
            }
            else if (ddlAggregate.SelectedValue == "20 mm" && chk10mmPresent.Checked == true)
            {
                switch (ddlGrade.SelectedValue)
                {
                    case "M 5":
                    case "M 10":
                    case "M 15":
                    case "M 20":
                        percent10mm = 25;
                        percent20mm = 75;
                        break;
                    case "M 25":
                        percent10mm = 30;
                        percent20mm = 70;
                        break;
                    case "M 30":
                        percent10mm = 35;
                        percent20mm = 65;
                        break;
                    case "M 35":
                        percent10mm = 40;
                        percent20mm = 60;
                        break;
                    case "M 40":
                        percent10mm = 45;
                        percent20mm = 55;
                        break;
                    case "M 45":
                        percent10mm = 50;
                        percent20mm = 50;
                        break;
                    case "M 50":
                        percent10mm = 55;
                        percent20mm = 45;
                        break;
                    case "M 55":
                        percent10mm = 65;
                        percent20mm = 40;
                        break;
                    case "M 60":
                        percent10mm = 70;
                        percent20mm = 35;
                        break;
                    case "M 65":
                        percent10mm = 75;
                        percent20mm = 30;
                        break;
                    case "M 70":
                        percent10mm = 100;
                        percent20mm = 25;
                        break;
                    case "M 75":
                        percent10mm = 100;
                        break;
                    case "M 80":
                        percent10mm = 100;
                        break;
                    case "M 85":
                        percent10mm = 100;
                        break;
                    case "M 90":
                        percent10mm = 100;
                        break;
                    case "M 95":
                        percent10mm = 100;
                        break;
                    case "M 100":
                        percent10mm = 100;
                        break;
                }
                if (ddlType.SelectedValue == "Self Compacting")
                {
                    if (percent10mm < 70)
                    {
                        percent10mm = 70;
                        percent20mm = 30;
                    }
                }
            }
            else if (ddlAggregate.SelectedValue == "40 mm")
            {
                switch (ddlGrade.SelectedValue)
                {
                    case "M 5":
                    case "M 10":
                    case "M 15":
                    case "M 20":
                        percent10mm = 15;
                        percent20mm = 35;
                        percent40mm = 40;
                        break;
                    case "M 25":
                        percent10mm = 20;
                        percent20mm = 35;
                        percent40mm = 45;
                        break;
                    case "M 30":
                        percent10mm = 25;
                        percent20mm = 35;
                        percent40mm = 40;
                        break;
                    case "M 35":
                        percent10mm = 30;
                        percent20mm = 40;
                        percent40mm = 30;
                        break;
                }
            }

            int FA1Id = 0, FA2Id = 0;
            var material = dc.MaterialListView(ddlReferenceNo.SelectedValue, "", "Aggregate");
            foreach (var mt in material)
            {
                if (mt.Material_List == "Natural Sand" || mt.Material_List == "Crushed Sand"
                    || mt.Material_List == "Stone Dust" || mt.Material_List == "Grit")
                {
                    if (FA1Id == 0)
                        FA1Id = mt.Material_Id;
                    else if (FA2Id == 0)
                        FA2Id = mt.Material_Id;
                }
            }
            txt4p75PassingPercent.Text = "0";
            txt2p36PassingPercent.Text = "0";
            txt600MicronPercent.Text = "0";
            txt300micronPassing.Text = "0";
            txt150micronPassing.Text = "0";
            string[] strRefNo1 = ddlReferenceNo.SelectedValue.Split('/');
            string[] strRefNo2 = strRefNo1[1].Split('-');
            if (FA1Id > 0 && FA2Id == 0)
            {
                string refNo = ddlReferenceNo.SelectedValue;
                var seiveAnalysis = dc.AggregateAllTestView(refNo, FA1Id, "AGGTSA").ToList();
                if (seiveAnalysis.Count == 0 && refNo != strRefNo1[0] + "/" + strRefNo2[0] + "-" + "1")
                {
                    refNo = strRefNo1[0] + "/" + strRefNo2[0] + "-" + "1";
                    var seiveAnalysis1 = dc.AggregateAllTestView(refNo, FA1Id, "AGGTSA").ToList();
                    seiveAnalysis = seiveAnalysis1;
                }
                foreach (var sa in seiveAnalysis)
                {
                    if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                    {
                        txt4p75PassingPercent.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "2.36 mm")
                    {
                        txt2p36PassingPercent.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "600 micron")
                    {
                        txt600MicronPercent.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "300 micron")
                    {
                        txt300micronPassing.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "150 micron")
                    {
                        txt150micronPassing.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                }
            }
            else if (FA1Id > 0 && FA2Id > 0)
            {
                string refNo = ddlReferenceNo.SelectedValue;
                var seiveAnalysis = dc.AggregateAllTestView(refNo, FA1Id, "AGGTSA").ToList();
                if (seiveAnalysis.Count == 0 && refNo != strRefNo1[0] + "/" + strRefNo2[0] + "-" + "1")
                {
                    refNo = strRefNo1[0] + "/" + strRefNo2[0] + "-" + "1";
                    var seiveAnalysis1 = dc.AggregateAllTestView(refNo, FA1Id, "AGGTSA").ToList();
                    seiveAnalysis = seiveAnalysis1;
                }
                foreach (var sa in seiveAnalysis)
                {
                    if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                    {
                        txt4p75PassingPercent.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "2.36 mm")
                    {
                        txt2p36PassingPercent.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "600 micron")
                    {
                        txt600MicronPercent.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "300 micron")
                    {
                        txt300micronPassing.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "150 micron")
                    {
                        txt150micronPassing.Text = sa.AGGTSA_CumuPassing_dec.ToString();
                    }
                }

                refNo = ddlReferenceNo.SelectedValue;
                var seiveAnalysis2 = dc.AggregateAllTestView(ddlReferenceNo.SelectedValue, FA2Id, "AGGTSA").ToList();
                if (seiveAnalysis2.Count == 0 && refNo != strRefNo1[0] + "/" + strRefNo2[0] + "-" + "1")
                {
                    refNo = strRefNo1[0] + "/" + strRefNo2[0] + "-" + "1";
                    var seiveAnalysis1 = dc.AggregateAllTestView(refNo, FA2Id, "AGGTSA").ToList();
                    seiveAnalysis2 = seiveAnalysis1;
                }
                foreach (var sa in seiveAnalysis2)
                {
                    if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                    {
                        txt4p75PassingPercent.Text = ((Convert.ToDecimal(txt4p75PassingPercent.Text) * (Convert.ToDecimal(txtPercentageFA1.Text) / 100)) + (sa.AGGTSA_CumuPassing_dec * (Convert.ToDecimal(txtPercentageFA2.Text) / 100))).ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "2.36 mm")
                    {
                        txt2p36PassingPercent.Text = ((Convert.ToDecimal(txt2p36PassingPercent.Text) * (Convert.ToDecimal(txtPercentageFA1.Text) / 100)) + (sa.AGGTSA_CumuPassing_dec * (Convert.ToDecimal(txtPercentageFA2.Text) / 100))).ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "600 micron")
                    {
                        txt600MicronPercent.Text = ((Convert.ToDecimal(txt600MicronPercent.Text) * (Convert.ToDecimal(txtPercentageFA1.Text) / 100)) + (sa.AGGTSA_CumuPassing_dec * (Convert.ToDecimal(txtPercentageFA2.Text) / 100))).ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "300 micron")
                    {
                        txt300micronPassing.Text = ((Convert.ToDecimal(txt300micronPassing.Text) * (Convert.ToDecimal(txtPercentageFA1.Text) / 100)) + (sa.AGGTSA_CumuPassing_dec * (Convert.ToDecimal(txtPercentageFA2.Text) / 100))).ToString();
                    }
                    else if (sa.AGGTSA_SeiveSize_var == "150 micron")
                    {
                        txt150micronPassing.Text = ((Convert.ToDecimal(txt150micronPassing.Text) * (Convert.ToDecimal(txtPercentageFA1.Text) / 100)) + (sa.AGGTSA_CumuPassing_dec * (Convert.ToDecimal(txtPercentageFA2.Text) / 100))).ToString();
                    }
                }
            }

            decimal SPGFA = 0, SPGAggt = 0;
            clsData objcls = new clsData();
            string mySql = "select distinct(Material_Id ) from tbl_MaterialList,tbl_MaterialDetail ";
            mySql += " where tbl_MaterialList.Material_Id= tbl_MaterialDetail.MaterialDetail_Id and Material_Type='Aggregate'";
            mySql += " and tbl_MaterialDetail.MaterialDetail_RefNo='" + ddlReferenceNo.SelectedValue + "'";
            DataTable dtMatrls = objcls.getGeneralData(mySql);
            string[] RefNo1 = Convert.ToString(ddlReferenceNo.SelectedValue).Split('/');
            Int32 mID = 0, prevMatId = 0;
            bool FA1Used = false, FA2Used = false;
            for (int i1 = 0; i1 < dtMatrls.Rows.Count; i1++)
            {
                mID = Convert.ToInt32(dtMatrls.Rows[i1]["material_id"].ToString());
                var MfInwd = dc.MF_View1(RefNo1[0].ToString() + "/%", mID);
                prevMatId = 0;
                foreach (var aggt in MfInwd)
                {
                    if (prevMatId == 0 || Convert.ToInt32(aggt.AGGTINWD_Material_Id) != prevMatId)
                    {
                        decimal Specgrav = 0;
                        if (aggt.AGGTINWD_AggregateName_var.ToString() == "Natural Sand" || aggt.AGGTINWD_AggregateName_var.ToString() == "Crushed Sand"
                            || aggt.AGGTINWD_AggregateName_var.ToString() == "Stone Dust" || aggt.AGGTINWD_AggregateName_var.ToString() == "Grit")
                        {
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                {
                                    if (FA1Id > 0 && FA2Id > 0)
                                    {
                                        if (FA1Used == false)
                                        {
                                            SPGFA += Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var) * (Convert.ToDecimal(txtPercentageFA1.Text) / 100);
                                            FA1Used = true ;
                                        }
                                        else if (FA2Used == false)
                                        {
                                            SPGFA += Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var) * (Convert.ToDecimal(txtPercentageFA2.Text) / 100);
                                            FA2Used = true;
                                        }
                                    }
                                    else if (FA1Id > 0)
                                    {
                                        SPGFA += Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var);
                                    }
                                }
                            }
                        }
                        else if (aggt.AGGTINWD_AggregateName_var.ToString() == "10 mm" || aggt.AGGTINWD_AggregateName_var.ToString() == "20 mm"
                            || aggt.AGGTINWD_AggregateName_var.ToString() == "40 mm")
                        {
                            if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                            {
                                if (Decimal.TryParse(aggt.AGGTINWD_SpecificGravity_var.ToString(), out Specgrav))
                                {
                                    if (aggt.AGGTINWD_AggregateName_var.ToString() == "10 mm")
                                        SPGAggt += Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var) * (percent10mm /100);
                                    else if (aggt.AGGTINWD_AggregateName_var.ToString() == "20 mm")
                                        SPGAggt += Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var) * (percent20mm / 100);
                                    else if (aggt.AGGTINWD_AggregateName_var.ToString() == "40 mm")
                                        SPGAggt += Convert.ToDecimal(aggt.AGGTINWD_SpecificGravity_var) * (percent40mm / 100);
                                }
                            }
                        }
                        prevMatId = Convert.ToInt32(aggt.AGGTINWD_Material_Id);
                    }
                }
            }
            txtSPGFA.Text = (SPGFA).ToString("0.00");
            txtSPGAggt.Text = (SPGAggt).ToString("0.00");
        }

        protected void lnkFetchFAPercentWise_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            if (ddlReferenceNo.SelectedIndex <= 0)
            {
                strMsg = "Select Reference No.";
                ddlReferenceNo.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
            else if (txtPercentageFA1.Text == "")
            {
                strMsg = "Enter FA1 percentage.";
                txtPercentageFA1.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
            else if (txtPercentageFA2.Text == "")
            {
                strMsg = "Enter FA2 percentage.";
                txtPercentageFA2.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
            else if (txtPercentageFA1.Text != "" && txtPercentageFA2.Text != ""
                && (Convert.ToDecimal(txtPercentageFA1.Text) + Convert.ToDecimal(txtPercentageFA2.Text)) != 100)
            {
                strMsg = "Fine Aggregate Percentage total should be 100.";
                txtPercentageFA2.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
            else
            {
                FetchFAPercentWise();
            }
        }

        protected void lnkTrial_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            if (ddlReferenceNo.SelectedIndex <= 0)
            {
                strMsg = "Select Reference No.";
                ddlReferenceNo.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
            else
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strURLWithData = "Trial.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&AddNewTrial={1}&TrialId={2}", ddlReferenceNo.SelectedValue, "AddNewTrial", "0"));
                Response.Redirect(strURLWithData);
            }
        }        

        
    }
}