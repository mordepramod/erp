using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportMixDesign : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Mix Design Report";
                
                bool superAdminRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAdmin_right_bit == true)
                        superAdminRight = true;
                }
                if (superAdminRight == true)
                {
                    ddl_Grade.SelectedValue = "M 20";
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            #region variables
            //cement
            decimal slump1Cement = 0, slump1WCRatio = 0, slump1Water = 0, slump1FlyAsh = 0, slump1GGBS = 0, slump110mm = 0, slump120mm = 0, slump1300micronPassing = 0, slump1Admixture = 0, slump14p75mmPassing = 0, slump1PasticDensity = 0, slump1Sand = 0, slump1SandWt = 0, slump1Strength3days = 0, slump1Strength7days = 0, slump1Strength28days = 0;
            int slump1CementCnt = 0, slump1WCRatioCnt = 0, slump1WaterCnt = 0, slump1FlyAshCnt = 0, slump1GGBSCnt = 0, slump110mmCnt = 0, slump120mmCnt = 0, slump1300micronPassingCnt = 0, slump1AdmixtureCnt = 0, slump14p75mmPassingCnt = 0, slump1PasticDensityCnt = 0, slump1SandCnt = 0, slump1Strength3daysCnt = 0, slump1Strength7daysCnt = 0, slump1Strength28daysCnt = 0;
            decimal slump2Cement = 0, slump2WCRatio = 0, slump2Water = 0, slump2FlyAsh = 0, slump2GGBS = 0, slump210mm = 0, slump220mm = 0, slump2300micronPassing = 0, slump2Admixture = 0, slump24p75mmPassing = 0, slump2PasticDensity = 0, slump2Sand = 0, slump2SandWt = 0, slump2Strength3days = 0, slump2Strength7days = 0, slump2Strength28days = 0;
            int slump2CementCnt = 0, slump2WCRatioCnt = 0, slump2WaterCnt = 0, slump2FlyAshCnt = 0, slump2GGBSCnt = 0, slump210mmCnt = 0, slump220mmCnt = 0, slump2300micronPassingCnt = 0, slump2AdmixtureCnt = 0, slump24p75mmPassingCnt = 0, slump2PasticDensityCnt = 0, slump2SandCnt = 0, slump2Strength3daysCnt = 0, slump2Strength7daysCnt = 0, slump2Strength28daysCnt = 0;
            decimal slump3Cement = 0, slump3WCRatio = 0, slump3Water = 0, slump3FlyAsh = 0, slump3GGBS = 0, slump310mm = 0, slump320mm = 0, slump3300micronPassing = 0, slump3Admixture = 0, slump34p75mmPassing = 0, slump3PasticDensity = 0, slump3Sand = 0, slump3SandWt = 0, slump3Strength3days = 0, slump3Strength7days = 0, slump3Strength28days = 0;
            int slump3CementCnt = 0, slump3WCRatioCnt = 0, slump3WaterCnt = 0, slump3FlyAshCnt = 0, slump3GGBSCnt = 0, slump310mmCnt = 0, slump320mmCnt = 0, slump3300micronPassingCnt = 0, slump3AdmixtureCnt = 0, slump34p75mmPassingCnt = 0, slump3PasticDensityCnt = 0, slump3SandCnt = 0, slump3Strength3daysCnt = 0, slump3Strength7daysCnt = 0, slump3Strength28daysCnt = 0;
            decimal slump4Cement = 0, slump4WCRatio = 0, slump4Water = 0, slump4FlyAsh = 0, slump4GGBS = 0, slump410mm = 0, slump420mm = 0, slump4300micronPassing = 0, slump4Admixture = 0, slump44p75mmPassing = 0, slump4PasticDensity = 0, slump4Sand = 0, slump4SandWt = 0, slump4Strength3days = 0, slump4Strength7days = 0, slump4Strength28days = 0;
            int slump4CementCnt = 0, slump4WCRatioCnt = 0, slump4WaterCnt = 0, slump4FlyAshCnt = 0, slump4GGBSCnt = 0, slump410mmCnt = 0, slump420mmCnt = 0, slump4300micronPassingCnt = 0, slump4AdmixtureCnt = 0, slump44p75mmPassingCnt = 0, slump4PasticDensityCnt = 0, slump4SandCnt = 0, slump4Strength3daysCnt = 0, slump4Strength7daysCnt = 0, slump4Strength28daysCnt = 0;
            decimal slump5Cement = 0, slump5WCRatio = 0, slump5Water = 0, slump5FlyAsh = 0, slump5GGBS = 0, slump510mm = 0, slump520mm = 0, slump5300micronPassing = 0, slump5Admixture = 0, slump54p75mmPassing = 0, slump5PasticDensity = 0, slump5Sand = 0, slump5SandWt = 0, slump5Strength3days = 0, slump5Strength7days = 0, slump5Strength28days = 0;
            int slump5CementCnt = 0, slump5WCRatioCnt = 0, slump5WaterCnt = 0, slump5FlyAshCnt = 0, slump5GGBSCnt = 0, slump510mmCnt = 0, slump520mmCnt = 0, slump5300micronPassingCnt = 0, slump5AdmixtureCnt = 0, slump54p75mmPassingCnt = 0, slump5PasticDensityCnt = 0, slump5SandCnt = 0, slump5Strength3daysCnt = 0, slump5Strength7daysCnt = 0, slump5Strength28daysCnt = 0;
            decimal slump6Cement = 0, slump6WCRatio = 0, slump6Water = 0, slump6FlyAsh = 0, slump6GGBS = 0, slump610mm = 0, slump620mm = 0, slump6300micronPassing = 0, slump6Admixture = 0, slump64p75mmPassing = 0, slump6PasticDensity = 0, slump6Sand = 0, slump6SandWt = 0, slump6Strength3days = 0, slump6Strength7days = 0, slump6Strength28days = 0;
            int slump6CementCnt = 0, slump6WCRatioCnt = 0, slump6WaterCnt = 0, slump6FlyAshCnt = 0, slump6GGBSCnt = 0, slump610mmCnt = 0, slump620mmCnt = 0, slump6300micronPassingCnt = 0, slump6AdmixtureCnt = 0, slump64p75mmPassingCnt = 0, slump6PasticDensityCnt = 0, slump6SandCnt = 0, slump6Strength3daysCnt = 0, slump6Strength7daysCnt = 0, slump6Strength28daysCnt = 0;
            //Flyash + cement
            decimal fslump1Cement = 0, fslump1WCRatio = 0, fslump1Water = 0, fslump1FlyAsh = 0, fslump1GGBS = 0, fslump110mm = 0, fslump120mm = 0, fslump1300micronPassing = 0, fslump1Admixture = 0, fslump14p75mmPassing = 0, fslump1PasticDensity = 0, fslump1Sand = 0, fslump1SandWt = 0, fslump1Strength3days = 0, fslump1Strength7days = 0, fslump1Strength28days = 0;
            int fslump1CementCnt = 0, fslump1WCRatioCnt = 0, fslump1WaterCnt = 0, fslump1FlyAshCnt = 0, fslump1GGBSCnt = 0, fslump110mmCnt = 0, fslump120mmCnt = 0, fslump1300micronPassingCnt = 0, fslump1AdmixtureCnt = 0, fslump14p75mmPassingCnt = 0, fslump1PasticDensityCnt = 0, fslump1SandCnt = 0, fslump1Strength3daysCnt = 0, fslump1Strength7daysCnt = 0, fslump1Strength28daysCnt = 0;
            decimal fslump2Cement = 0, fslump2WCRatio = 0, fslump2Water = 0, fslump2FlyAsh = 0, fslump2GGBS = 0, fslump210mm = 0, fslump220mm = 0, fslump2300micronPassing = 0, fslump2Admixture = 0, fslump24p75mmPassing = 0, fslump2PasticDensity = 0, fslump2Sand = 0, fslump2SandWt = 0, fslump2Strength3days = 0, fslump2Strength7days = 0, fslump2Strength28days = 0;
            int fslump2CementCnt = 0, fslump2WCRatioCnt = 0, fslump2WaterCnt = 0, fslump2FlyAshCnt = 0, fslump2GGBSCnt = 0, fslump210mmCnt = 0, fslump220mmCnt = 0, fslump2300micronPassingCnt = 0, fslump2AdmixtureCnt = 0, fslump24p75mmPassingCnt = 0, fslump2PasticDensityCnt = 0, fslump2SandCnt = 0, fslump2Strength3daysCnt = 0, fslump2Strength7daysCnt = 0, fslump2Strength28daysCnt = 0;
            decimal fslump3Cement = 0, fslump3WCRatio = 0, fslump3Water = 0, fslump3FlyAsh = 0, fslump3GGBS = 0, fslump310mm = 0, fslump320mm = 0, fslump3300micronPassing = 0, fslump3Admixture = 0, fslump34p75mmPassing = 0, fslump3PasticDensity = 0, fslump3Sand = 0, fslump3SandWt = 0, fslump3Strength3days = 0, fslump3Strength7days = 0, fslump3Strength28days = 0;
            int fslump3CementCnt = 0, fslump3WCRatioCnt = 0, fslump3WaterCnt = 0, fslump3FlyAshCnt = 0, fslump3GGBSCnt = 0, fslump310mmCnt = 0, fslump320mmCnt = 0, fslump3300micronPassingCnt = 0, fslump3AdmixtureCnt = 0, fslump34p75mmPassingCnt = 0, fslump3PasticDensityCnt = 0, fslump3SandCnt = 0, fslump3Strength3daysCnt = 0, fslump3Strength7daysCnt = 0, fslump3Strength28daysCnt = 0;
            decimal fslump4Cement = 0, fslump4WCRatio = 0, fslump4Water = 0, fslump4FlyAsh = 0, fslump4GGBS = 0, fslump410mm = 0, fslump420mm = 0, fslump4300micronPassing = 0, fslump4Admixture = 0, fslump44p75mmPassing = 0, fslump4PasticDensity = 0, fslump4Sand = 0, fslump4SandWt = 0, fslump4Strength3days = 0, fslump4Strength7days = 0, fslump4Strength28days = 0;
            int fslump4CementCnt = 0, fslump4WCRatioCnt = 0, fslump4WaterCnt = 0, fslump4FlyAshCnt = 0, fslump4GGBSCnt = 0, fslump410mmCnt = 0, fslump420mmCnt = 0, fslump4300micronPassingCnt = 0, fslump4AdmixtureCnt = 0, fslump44p75mmPassingCnt = 0, fslump4PasticDensityCnt = 0, fslump4SandCnt = 0, fslump4Strength3daysCnt = 0, fslump4Strength7daysCnt = 0, fslump4Strength28daysCnt = 0;
            decimal fslump5Cement = 0, fslump5WCRatio = 0, fslump5Water = 0, fslump5FlyAsh = 0, fslump5GGBS = 0, fslump510mm = 0, fslump520mm = 0, fslump5300micronPassing = 0, fslump5Admixture = 0, fslump54p75mmPassing = 0, fslump5PasticDensity = 0, fslump5Sand = 0, fslump5SandWt = 0, fslump5Strength3days = 0, fslump5Strength7days = 0, fslump5Strength28days = 0;
            int fslump5CementCnt = 0, fslump5WCRatioCnt = 0, fslump5WaterCnt = 0, fslump5FlyAshCnt = 0, fslump5GGBSCnt = 0, fslump510mmCnt = 0, fslump520mmCnt = 0, fslump5300micronPassingCnt = 0, fslump5AdmixtureCnt = 0, fslump54p75mmPassingCnt = 0, fslump5PasticDensityCnt = 0, fslump5SandCnt = 0, fslump5Strength3daysCnt = 0, fslump5Strength7daysCnt = 0, fslump5Strength28daysCnt = 0;
            decimal fslump6Cement = 0, fslump6WCRatio = 0, fslump6Water = 0, fslump6FlyAsh = 0, fslump6GGBS = 0, fslump610mm = 0, fslump620mm = 0, fslump6300micronPassing = 0, fslump6Admixture = 0, fslump64p75mmPassing = 0, fslump6PasticDensity = 0, fslump6Sand = 0, fslump6SandWt = 0, fslump6Strength3days = 0, fslump6Strength7days = 0, fslump6Strength28days = 0;
            int fslump6CementCnt = 0, fslump6WCRatioCnt = 0, fslump6WaterCnt = 0, fslump6FlyAshCnt = 0, fslump6GGBSCnt = 0, fslump610mmCnt = 0, fslump620mmCnt = 0, fslump6300micronPassingCnt = 0, fslump6AdmixtureCnt = 0, fslump64p75mmPassingCnt = 0, fslump6PasticDensityCnt = 0, fslump6SandCnt = 0, fslump6Strength3daysCnt = 0, fslump6Strength7daysCnt = 0, fslump6Strength28daysCnt = 0;
            //GGBS + Cement
            decimal gslump1Cement = 0, gslump1WCRatio = 0, gslump1Water = 0, gslump1FlyAsh = 0, gslump1GGBS = 0, gslump110mm = 0, gslump120mm = 0, gslump1300micronPassing = 0, gslump1Admixture = 0, gslump14p75mmPassing = 0, gslump1PasticDensity = 0, gslump1Sand = 0, gslump1SandWt = 0, gslump1Strength3days = 0, gslump1Strength7days = 0, gslump1Strength28days = 0;
            int gslump1CementCnt = 0, gslump1WCRatioCnt = 0, gslump1WaterCnt = 0, gslump1FlyAshCnt = 0, gslump1GGBSCnt = 0, gslump110mmCnt = 0, gslump120mmCnt = 0, gslump1300micronPassingCnt = 0, gslump1AdmixtureCnt = 0, gslump14p75mmPassingCnt = 0, gslump1PasticDensityCnt = 0, gslump1SandCnt = 0, gslump1Strength3daysCnt = 0, gslump1Strength7daysCnt = 0, gslump1Strength28daysCnt = 0;
            decimal gslump2Cement = 0, gslump2WCRatio = 0, gslump2Water = 0, gslump2FlyAsh = 0, gslump2GGBS = 0, gslump210mm = 0, gslump220mm = 0, gslump2300micronPassing = 0, gslump2Admixture = 0, gslump24p75mmPassing = 0, gslump2PasticDensity = 0, gslump2Sand = 0, gslump2SandWt = 0, gslump2Strength3days = 0, gslump2Strength7days = 0, gslump2Strength28days = 0;
            int gslump2CementCnt = 0, gslump2WCRatioCnt = 0, gslump2WaterCnt = 0, gslump2FlyAshCnt = 0, gslump2GGBSCnt = 0, gslump210mmCnt = 0, gslump220mmCnt = 0, gslump2300micronPassingCnt = 0, gslump2AdmixtureCnt = 0, gslump24p75mmPassingCnt = 0, gslump2PasticDensityCnt = 0, gslump2SandCnt = 0, gslump2Strength3daysCnt = 0, gslump2Strength7daysCnt = 0, gslump2Strength28daysCnt = 0;
            decimal gslump3Cement = 0, gslump3WCRatio = 0, gslump3Water = 0, gslump3FlyAsh = 0, gslump3GGBS = 0, gslump310mm = 0, gslump320mm = 0, gslump3300micronPassing = 0, gslump3Admixture = 0, gslump34p75mmPassing = 0, gslump3PasticDensity = 0, gslump3Sand = 0, gslump3SandWt = 0, gslump3Strength3days = 0, gslump3Strength7days = 0, gslump3Strength28days = 0;
            int gslump3CementCnt = 0, gslump3WCRatioCnt = 0, gslump3WaterCnt = 0, gslump3FlyAshCnt = 0, gslump3GGBSCnt = 0, gslump310mmCnt = 0, gslump320mmCnt = 0, gslump3300micronPassingCnt = 0, gslump3AdmixtureCnt = 0, gslump34p75mmPassingCnt = 0, gslump3PasticDensityCnt = 0, gslump3SandCnt = 0, gslump3Strength3daysCnt = 0, gslump3Strength7daysCnt = 0, gslump3Strength28daysCnt = 0;
            decimal gslump4Cement = 0, gslump4WCRatio = 0, gslump4Water = 0, gslump4FlyAsh = 0, gslump4GGBS = 0, gslump410mm = 0, gslump420mm = 0, gslump4300micronPassing = 0, gslump4Admixture = 0, gslump44p75mmPassing = 0, gslump4PasticDensity = 0, gslump4Sand = 0, gslump4SandWt = 0, gslump4Strength3days = 0, gslump4Strength7days = 0, gslump4Strength28days = 0;
            int gslump4CementCnt = 0, gslump4WCRatioCnt = 0, gslump4WaterCnt = 0, gslump4FlyAshCnt = 0, gslump4GGBSCnt = 0, gslump410mmCnt = 0, gslump420mmCnt = 0, gslump4300micronPassingCnt = 0, gslump4AdmixtureCnt = 0, gslump44p75mmPassingCnt = 0, gslump4PasticDensityCnt = 0, gslump4SandCnt = 0, gslump4Strength3daysCnt = 0, gslump4Strength7daysCnt = 0, gslump4Strength28daysCnt = 0;
            decimal gslump5Cement = 0, gslump5WCRatio = 0, gslump5Water = 0, gslump5FlyAsh = 0, gslump5GGBS = 0, gslump510mm = 0, gslump520mm = 0, gslump5300micronPassing = 0, gslump5Admixture = 0, gslump54p75mmPassing = 0, gslump5PasticDensity = 0, gslump5Sand = 0, gslump5SandWt = 0, gslump5Strength3days = 0, gslump5Strength7days = 0, gslump5Strength28days = 0;
            int gslump5CementCnt = 0, gslump5WCRatioCnt = 0, gslump5WaterCnt = 0, gslump5FlyAshCnt = 0, gslump5GGBSCnt = 0, gslump510mmCnt = 0, gslump520mmCnt = 0, gslump5300micronPassingCnt = 0, gslump5AdmixtureCnt = 0, gslump54p75mmPassingCnt = 0, gslump5PasticDensityCnt = 0, gslump5SandCnt = 0, gslump5Strength3daysCnt = 0, gslump5Strength7daysCnt = 0, gslump5Strength28daysCnt = 0;
            decimal gslump6Cement = 0, gslump6WCRatio = 0, gslump6Water = 0, gslump6FlyAsh = 0, gslump6GGBS = 0, gslump610mm = 0, gslump620mm = 0, gslump6300micronPassing = 0, gslump6Admixture = 0, gslump64p75mmPassing = 0, gslump6PasticDensity = 0, gslump6Sand = 0, gslump6SandWt = 0, gslump6Strength3days = 0, gslump6Strength7days = 0, gslump6Strength28days = 0;
            int gslump6CementCnt = 0, gslump6WCRatioCnt = 0, gslump6WaterCnt = 0, gslump6FlyAshCnt = 0, gslump6GGBSCnt = 0, gslump610mmCnt = 0, gslump620mmCnt = 0, gslump6300micronPassingCnt = 0, gslump6AdmixtureCnt = 0, gslump64p75mmPassingCnt = 0, gslump6PasticDensityCnt = 0, gslump6SandCnt = 0, gslump6Strength3daysCnt = 0, gslump6Strength7daysCnt = 0, gslump6Strength28daysCnt = 0;
            #endregion
            int CrushedSandId = 0, NaturalSandId = 0;
            var material = dc.MaterialListView("", "Crushed Sand", "");
            CrushedSandId = material.FirstOrDefault().Material_Id;
            var material1 = dc.MaterialListView("", "Natural Sand", "");
            NaturalSandId = material1.FirstOrDefault().Material_Id;
            var mfinward = dc.MixDesign_Analysis(ddl_Grade.SelectedValue, "", 0, ddl_Test.SelectedValue,  1);
            foreach (var mfinwd in mfinward)
            {
                var trialData = dc.MixDesign_Analysis("", mfinwd.MFINWD_ReferenceNo_var, 0, "", 2).ToList();
                
                string[] strSlump = trialData.FirstOrDefault().Trial_BatchSlumpValue.Split('|');
                decimal slump = (Convert.ToDecimal(strSlump[0]) + Convert.ToDecimal(strSlump[1]) + Convert.ToDecimal(strSlump[2])) / 3;
                
                bool onlyCementFlag = false, CementFlyAshFlag = false, CementGGBSFlag = false;
                bool CementFlag = false, FlyAshFlag = false, GGBSFlag = false, CrushedSandFlag = false, NaturalSandFlag = false;
                int aggregateMatId = 0;
                foreach (var trl in trialData)
                {
                    if (trl.TrialDetail_MaterialName == "Cement")
                    {
                        CementFlag = true;
                    }
                    else if (trl.TrialDetail_MaterialName == "Fly Ash")
                    {
                        FlyAshFlag = true;
                    }
                    else if (trl.TrialDetail_MaterialName == "G G B S")
                    {
                        GGBSFlag = true;
                    }
                    else if (trl.TrialDetail_MaterialName == "Crushed Sand")
                    {
                        CrushedSandFlag = true;
                    }
                    else if (trl.TrialDetail_MaterialName == "Natural Sand")
                    {
                        NaturalSandFlag = true;
                    }
                }
                if (CementFlag == true && FlyAshFlag == false && GGBSFlag == false)
                {
                    onlyCementFlag = true;
                }
                else if (CementFlag == true && FlyAshFlag == true && GGBSFlag == false)
                {
                    CementFlyAshFlag = true;
                }
                else if (CementFlag == true && FlyAshFlag == false && GGBSFlag == true)
                {
                    CementGGBSFlag = true;
                }
                if (CrushedSandFlag == true && NaturalSandFlag == true)
                {
                    onlyCementFlag = false;
                    CementFlyAshFlag = false;
                    CementGGBSFlag = false;
                }
                else if (CrushedSandFlag == true)
                {
                    aggregateMatId = CrushedSandId;
                }
                else if (NaturalSandFlag == true)
                {
                    aggregateMatId = NaturalSandId;
                }
                var SieveA = dc.MixDesign_Analysis("", mfinwd.MFINWD_ReferenceNo_var, aggregateMatId,"", 3).ToList();
                //sieve analysis
                bool safoundflag = false;
                if (ddl_600micron.SelectedValue != "---All---")
                {   
                    foreach (var sa in SieveA)
                    {
                        if (sa.AGGTSA_SeiveSize_var == "600 micron")
                        {
                            safoundflag = true;
                            decimal a600micron = Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                            string[] strLimit = ddl_600micron.SelectedValue.Split('-');
                            if (a600micron < Convert.ToDecimal(strLimit[0]) || a600micron > Convert.ToDecimal(strLimit[1]))
                            {
                                safoundflag = false;
                            }
                        }
                    }
                    if (safoundflag == false)
                    {
                        onlyCementFlag = false;
                        CementFlyAshFlag = false;
                        CementGGBSFlag = false;
                    }
                }
                decimal Strength3days = 0, Strength7days = 0, Strength28days = 0;
                var cubeCast = dc.OtherCubeTestView(mfinwd.MFINWD_ReferenceNo_var, "MF", 0, trialData.FirstOrDefault().Trial_Id, "Trial", false, false);
                foreach (var c in cubeCast)
                {
                    if (c.Days_tint == 3 && decimal.TryParse(c.Avg_var, out Strength3days) == true)
                    {
                        Strength3days = Convert.ToDecimal(c.Avg_var);
                    }
                    else if (c.Days_tint == 7 && decimal.TryParse(c.Avg_var, out Strength7days) == true)
                    {
                        Strength7days = Convert.ToDecimal(c.Avg_var);
                    }
                    else if (c.Days_tint == 28 && decimal.TryParse(c.Avg_var, out Strength28days) == true)
                    {
                        Strength28days = Convert.ToDecimal(c.Avg_var);
                    }
                }
                decimal tempCement = 0, tempSand = 0, tempReqWt = 0, tempPlasticDensity = 0, tempSandReqWt = 0;
                if (onlyCementFlag == true)
                {
                    #region only cement
                    if (slump >= 0 && slump <= 50)
                    {                        
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                slump1Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump1CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                slump1WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump1WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                slump1Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump1WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                slump1FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump1FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                slump1GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump1GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                slump110mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump110mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                slump120mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump120mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                slump1Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                slump1AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                slump1PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump1PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            { 
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        slump1Sand += (100 - tempSand);
                        slump1SandCnt++;
                        slump1SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                slump1300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump1300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                slump14p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump14p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            slump1Strength3days += Strength3days;
                            slump1Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            slump1Strength7days += Strength7days;
                            slump1Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            slump1Strength28days += Strength28days;
                            slump1Strength28daysCnt++;
                        }
                    }

                    else if (slump > 50 && slump <= 75)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                slump2Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump2CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                slump2WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump2WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                slump2Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump2WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                slump2FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump2FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                slump2GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump2GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                slump210mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump210mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                slump220mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump220mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                slump2Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                slump2AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                slump2PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump2PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        slump2Sand += (100 - tempSand);
                        slump2SandCnt++;
                        slump2SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                slump2300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump2300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                slump24p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump24p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            slump2Strength3days += Strength3days;
                            slump2Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            slump2Strength7days += Strength7days;
                            slump2Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            slump2Strength28days += Strength28days;
                            slump2Strength28daysCnt++;
                        }
                    }
                    else if (slump > 75 && slump <= 100)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                slump3Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump3CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                slump3WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump3WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                slump3Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump3WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                slump3FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump3FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                slump3GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump3GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                slump310mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump310mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                slump320mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump320mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                slump3Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                slump3AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                slump3PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump3PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        slump3Sand += (100 - tempSand);
                        slump3SandCnt++;
                        slump3SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                slump3300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump3300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                slump34p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump34p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            slump3Strength3days += Strength3days;
                            slump3Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            slump3Strength7days += Strength7days;
                            slump3Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            slump3Strength28days += Strength28days;
                            slump3Strength28daysCnt++;
                        }
                    }
                    else if (slump > 100 && slump <= 125)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                slump4Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump4CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                slump4WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump4WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                slump4Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump4WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                slump4FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump4FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                slump4GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump4GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                slump410mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump410mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                slump420mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump420mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                slump4Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                slump4AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                slump4PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump4PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        slump4Sand += (100 - tempSand);
                        slump4SandCnt++;
                        slump4SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                slump4300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump4300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                slump44p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump44p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            slump4Strength3days += Strength3days;
                            slump4Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            slump4Strength7days += Strength7days;
                            slump4Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            slump4Strength28days += Strength28days;
                            slump4Strength28daysCnt++;
                        }
                    }
                    else if (slump > 125 && slump <= 150)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                slump5Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump5CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                slump5WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump5WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                slump5Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump5WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                slump5FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump5FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                slump5GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump5GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                slump510mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump510mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                slump520mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump520mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                slump5Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                slump5AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                slump5PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump5PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        slump5Sand += (100 - tempSand);
                        slump5SandCnt++;
                        slump5SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                slump5300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump5300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                slump54p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump54p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            slump5Strength3days += Strength3days;
                            slump5Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            slump5Strength7days += Strength7days;
                            slump5Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            slump5Strength28days += Strength28days;
                            slump5Strength28daysCnt++;
                        }
                    }
                    else if (slump > 150)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                slump6Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump6CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                slump6WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump6WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                slump6Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump6WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                slump6FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump6FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                slump6GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump6GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                slump610mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump610mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                slump620mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump620mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                slump6Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                slump6AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                slump6PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                slump6PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        slump6Sand += (100 - tempSand);
                        slump6SandCnt++;
                        slump6SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                slump6300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump6300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                slump64p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                slump64p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            slump6Strength3days += Strength3days;
                            slump6Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            slump6Strength7days += Strength7days;
                            slump6Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            slump6Strength28days += Strength28days;
                            slump6Strength28daysCnt++;
                        }
                    }
                    #endregion
                }

                else if (CementFlyAshFlag == true)
                {
                    #region FlyAsh + Cement
                    if (slump >= 0 && slump <= 50)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                fslump1Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump1CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                fslump1WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump1WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                fslump1Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump1WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                fslump1FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump1FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                fslump1GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump1GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                fslump110mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump110mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                fslump120mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump120mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                fslump1Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                fslump1AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                fslump1PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump1PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        fslump1Sand += (100 - tempSand);
                        fslump1SandCnt++;
                        fslump1SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                fslump1300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump1300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                fslump14p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump14p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            fslump1Strength3days += Strength3days;
                            fslump1Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            fslump1Strength7days += Strength7days;
                            fslump1Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            fslump1Strength28days += Strength28days;
                            fslump1Strength28daysCnt++;
                        }
                    }

                    else if (slump > 50 && slump <= 75)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                fslump2Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump2CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                fslump2WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump2WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                fslump2Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump2WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                fslump2FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump2FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                fslump2GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump2GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                fslump210mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump210mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                fslump220mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump220mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                fslump2Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                fslump2AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                fslump2PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump2PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        fslump2Sand += (100 - tempSand);
                        fslump2SandCnt++;
                        fslump2SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                fslump2300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump2300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                fslump24p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump24p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            fslump2Strength3days += Strength3days;
                            fslump2Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            fslump2Strength7days += Strength7days;
                            fslump2Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            fslump2Strength28days += Strength28days;
                            fslump2Strength28daysCnt++;
                        }
                    }
                    else if (slump > 75 && slump <= 100)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                fslump3Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump3CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                fslump3WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump3WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                fslump3Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump3WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                fslump3FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump3FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                fslump3GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump3GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                fslump310mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump310mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                fslump320mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump320mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                fslump3Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                fslump3AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                fslump3PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump3PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        fslump3Sand += (100 - tempSand);
                        fslump3SandCnt++;
                        fslump3SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                fslump3300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump3300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                fslump34p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump34p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            fslump3Strength3days += Strength3days;
                            fslump3Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            fslump3Strength7days += Strength7days;
                            fslump3Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            fslump3Strength28days += Strength28days;
                            fslump3Strength28daysCnt++;
                        }
                    }
                    else if (slump > 100 && slump <= 125)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                fslump4Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump4CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                fslump4WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump4WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                fslump4Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump4WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                fslump4FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump4FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                fslump4GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump4GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                fslump410mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump410mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                fslump420mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump420mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                fslump4Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                fslump4AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                fslump4PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump4PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        fslump4Sand += (100 - tempSand);
                        fslump4SandCnt++;
                        fslump4SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                fslump4300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump4300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                fslump44p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump44p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            fslump4Strength3days += Strength3days;
                            fslump4Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            fslump4Strength7days += Strength7days;
                            fslump4Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            fslump4Strength28days += Strength28days;
                            fslump4Strength28daysCnt++;
                        }
                    }
                    else if (slump > 125 && slump <= 150)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                fslump5Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump5CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                fslump5WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump5WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                fslump5Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump5WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                fslump5FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump5FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                fslump5GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump5GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                fslump510mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump510mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                fslump520mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump520mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                fslump5Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                fslump5AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                fslump5PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump5PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        fslump5Sand += (100 - tempSand);
                        fslump5SandCnt++;
                        fslump5SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                fslump5300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump5300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                fslump54p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump54p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            fslump5Strength3days += Strength3days;
                            fslump5Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            fslump5Strength7days += Strength7days;
                            fslump5Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            fslump5Strength28days += Strength28days;
                            fslump5Strength28daysCnt++;
                        }
                    }
                    else if (slump > 150)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                fslump6Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump6CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                fslump6WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump6WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                fslump6Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump6WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                fslump6FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump6FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                fslump6GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump6GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                fslump610mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump610mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                fslump620mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump620mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                fslump6Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                fslump6AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                fslump6PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                fslump6PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        fslump6Sand += (100 - tempSand);
                        fslump6SandCnt++;
                        fslump6SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                fslump6300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump6300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                fslump64p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                fslump64p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            fslump6Strength3days += Strength3days;
                            fslump6Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            fslump6Strength7days += Strength7days;
                            fslump6Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            fslump6Strength28days += Strength28days;
                            fslump6Strength28daysCnt++;
                        }
                    }
                    #endregion
                }
                else if (CementGGBSFlag == true)
                {
                    #region GGBS +Cement
                    if (slump >= 0 && slump <= 50)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                gslump1Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump1CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                gslump1WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump1WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                gslump1Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump1WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                gslump1FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump1FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                gslump1GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump1GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                gslump110mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump110mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                gslump120mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump120mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                gslump1Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                gslump1AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                gslump1PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump1PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        gslump1Sand += (100 - tempSand);
                        gslump1SandCnt++;
                        gslump1SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                gslump1300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump1300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                gslump14p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump14p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            gslump1Strength3days += Strength3days;
                            gslump1Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            gslump1Strength7days += Strength7days;
                            gslump1Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            gslump1Strength28days += Strength28days;
                            gslump1Strength28daysCnt++;
                        }
                    }

                    else if (slump > 50 && slump <= 75)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                gslump2Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump2CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                gslump2WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump2WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                gslump2Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump2WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                gslump2FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump2FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                gslump2GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump2GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                gslump210mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump210mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                gslump220mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump220mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                gslump2Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                gslump2AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                gslump2PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump2PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        gslump2Sand += (100 - tempSand);
                        gslump2SandCnt++;
                        gslump2SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                gslump2300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump2300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                gslump24p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump24p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            gslump2Strength3days += Strength3days;
                            gslump2Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            gslump2Strength7days += Strength7days;
                            gslump2Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            gslump2Strength28days += Strength28days;
                            gslump2Strength28daysCnt++;
                        }
                    }
                    else if (slump > 75 && slump <= 100)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                gslump3Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump3CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                gslump3WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump3WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                gslump3Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump3WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                gslump3FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump3FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                gslump3GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump3GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                gslump310mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump310mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                gslump320mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump320mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                gslump3Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                gslump3AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                gslump3PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump3PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        gslump3Sand += (100 - tempSand);
                        gslump3SandCnt++;
                        gslump3SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                gslump3300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump3300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                gslump34p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump34p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            gslump3Strength3days += Strength3days;
                            gslump3Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            gslump3Strength7days += Strength7days;
                            gslump3Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            gslump3Strength28days += Strength28days;
                            gslump3Strength28daysCnt++;
                        }
                    }
                    else if (slump > 100 && slump <= 125)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                gslump4Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump4CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                gslump4WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump4WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                gslump4Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump4WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                gslump4FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump4FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                gslump4GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump4GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                gslump410mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump410mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                gslump420mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump420mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                gslump4Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                gslump4AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                gslump4PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump4PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        gslump4Sand += (100 - tempSand);
                        gslump4SandCnt++;
                        gslump4SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                gslump4300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump4300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                gslump44p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump44p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            gslump4Strength3days += Strength3days;
                            gslump4Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            gslump4Strength7days += Strength7days;
                            gslump4Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            gslump4Strength28days += Strength28days;
                            gslump4Strength28daysCnt++;
                        }
                    }
                    else if (slump > 125 && slump <= 150)
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                gslump5Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump5CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                gslump5WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump5WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                gslump5Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump5WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                gslump5FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump5FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                gslump5GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump5GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                gslump510mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump510mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                gslump520mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump520mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                gslump5Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                gslump5AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                gslump5PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump5PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        gslump5Sand += (100 - tempSand);
                        gslump5SandCnt++;
                        gslump5SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                gslump5300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump5300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                gslump54p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump54p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            gslump5Strength3days += Strength3days;
                            gslump5Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            gslump5Strength7days += Strength7days;
                            gslump5Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            gslump5Strength28days += Strength28days;
                            gslump5Strength28daysCnt++;
                        }
                    }
                    else if (slump > 150 )
                    {
                        foreach (var trl in trialData)
                        {
                            if (trl.TrialDetail_MaterialName == "Cement")
                            {
                                gslump6Cement += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump6CementCnt++;
                                tempCement = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "W/C Ratio")
                            {
                                gslump6WCRatio += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump6WCRatioCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Water")
                            {
                                gslump6Water += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump6WaterCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Fly Ash")
                            {
                                gslump6FlyAsh += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump6FlyAshCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "G G B S")
                            {
                                gslump6GGBS += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump6GGBSCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "10 mm")
                            {
                                gslump610mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump610mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "20 mm")
                            {
                                gslump620mm += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump620mmCnt++;
                                tempSand += Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            else if (trl.TrialDetail_MaterialName == "Admixture")
                            {
                                gslump6Admixture += (Convert.ToDecimal(trl.TrialDetail_Weight) * tempCement) / 50000;
                                gslump6AdmixtureCnt++;
                            }
                            else if (trl.TrialDetail_MaterialName == "Plastic Density")
                            {
                                gslump6PasticDensity += Convert.ToDecimal(trl.TrialDetail_Weight);
                                gslump6PasticDensityCnt++;
                                tempPlasticDensity = Convert.ToDecimal(trl.TrialDetail_Weight);
                            }
                            if (trl.TrialDetail_MaterialName != "Admixture" && trl.TrialDetail_MaterialName != "Total")
                            {
                                tempReqWt += Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                            if (trl.TrialDetail_MaterialName == "Crushed Sand" || trl.TrialDetail_MaterialName == "Natural Sand")
                            {
                                tempSandReqWt = Convert.ToDecimal(trl.TrialDetail_ReqdWt);
                            }
                        }
                        gslump6Sand += (100 - tempSand);
                        gslump6SandCnt++;
                        gslump6SandWt += (tempPlasticDensity / tempReqWt) * tempSandReqWt;
                        //sieve analysis
                        foreach (var sa in SieveA)
                        {
                            if (sa.AGGTSA_SeiveSize_var == "300 micron")
                            {
                                gslump6300micronPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump6300micronPassingCnt++;
                            }
                            if (sa.AGGTSA_SeiveSize_var == "4.75 mm")
                            {
                                gslump64p75mmPassing += Convert.ToDecimal(sa.AGGTSA_CumuPassing_dec);
                                gslump64p75mmPassingCnt++;
                            }
                        }
                        if (Strength3days > 0)
                        {
                            gslump6Strength3days += Strength3days;
                            gslump6Strength3daysCnt++;
                        }
                        if (Strength7days > 0)
                        {
                            gslump6Strength7days += Strength7days;
                            gslump6Strength7daysCnt++;
                        }
                        if (Strength28days > 0)
                        {
                            gslump6Strength28days += Strength28days;
                            gslump6Strength28daysCnt++;
                        }
                    }
                    #endregion
                }
            }

            
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("Slump", typeof(string)));
            dt.Columns.Add(new DataColumn("Cement", typeof(string)));
            dt.Columns.Add(new DataColumn("WCRatio", typeof(string)));
            dt.Columns.Add(new DataColumn("Water", typeof(string)));
            dt.Columns.Add(new DataColumn("FlyAsh", typeof(string)));
            dt.Columns.Add(new DataColumn("GGBS", typeof(string)));
            dt.Columns.Add(new DataColumn("10mm", typeof(string)));
            dt.Columns.Add(new DataColumn("20mm", typeof(string)));
            dt.Columns.Add(new DataColumn("Sand", typeof(string)));
            dt.Columns.Add(new DataColumn("300micronPassing", typeof(string)));
            dt.Columns.Add(new DataColumn("300micronPlusCement", typeof(string)));
            dt.Columns.Add(new DataColumn("Admixture", typeof(string)));
            dt.Columns.Add(new DataColumn("Cement4.75mmWaterDivPlasticDensity", typeof(string)));
            dt.Columns.Add(new DataColumn("3DaysStrength", typeof(string)));
            dt.Columns.Add(new DataColumn("7DaysStrength", typeof(string)));
            dt.Columns.Add(new DataColumn("28DaysStrength", typeof(string)));
            dt.Columns.Add(new DataColumn("AvgCount", typeof(string)));

            dr1 = dt.NewRow();
            dr1["Slump"] = "Cement";
            dt.Rows.Add(dr1);
            #region cement
            // cement - 0 to 50
            dr1 = dt.NewRow();
            dr1["Slump"] = "0 to 50";
            if (slump1CementCnt > 0)
                dr1["AvgCount"] = slump1CementCnt.ToString();
            if (slump1CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(slump1Cement / slump1CementCnt).ToString("0.00");
            if (slump1WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(slump1WCRatio / slump1WCRatioCnt).ToString("0.00");
            if (slump1WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(slump1Water / slump1WaterCnt).ToString("0.00");
            if (slump1FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(slump1FlyAsh / slump1FlyAshCnt).ToString("0.00");
            if (slump1GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(slump1GGBS / slump1GGBSCnt).ToString("0.00");
            if (slump1SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(slump1Sand / slump1SandCnt).ToString("0.00");
            if (slump110mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(slump110mm / slump110mmCnt).ToString("0.00");
            if (slump120mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(slump120mm / slump120mmCnt).ToString("0.00");
            if (slump1300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(slump1300micronPassing / slump1300micronPassingCnt).ToString("0.00");
            if (slump1Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(slump1Strength3days / slump1Strength3daysCnt).ToString("0.00");
            if (slump1Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(slump1Strength7days / slump1Strength7daysCnt).ToString("0.00");
            if (slump1Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(slump1Strength28days / slump1Strength28daysCnt).ToString("0.00");

            decimal tempVal = 0;
            if (slump1CementCnt > 0 || slump1300micronPassingCnt > 0 || slump1FlyAshCnt > 0 || slump1GGBSCnt > 0)
            {
                if (slump1CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump1Cement / slump1CementCnt);
                if (slump1300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(slump1300micronPassing / slump1300micronPassingCnt);
                if (slump1FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(slump1FlyAsh / slump1FlyAshCnt);
                if (slump1GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(slump1GGBS / slump1GGBSCnt);

                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }

            if (slump1AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(slump1Admixture / slump1AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (slump1PasticDensityCnt > 0)
            {
                //dr1["Cement4.75mmWaterDivPlasticDensity"] = ((Convert.ToDecimal(slump1Cement / slump1CementCnt) + Convert.ToDecimal(slump14p75mmPassing / slump14p75mmPassingCnt)
                //    + Convert.ToDecimal(slump1Water / slump1WaterCnt)) / (Convert.ToDecimal(slump1PasticDensity) / slump1PasticDensityCnt)).ToString("0.00");
                if (slump1CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump1Cement / slump1CementCnt);
                if (slump14p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(slump1SandWt / slump1SandCnt) * Convert.ToDecimal((slump14p75mmPassing / slump14p75mmPassingCnt)/100));
                if (slump1WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump1Water / slump1WaterCnt);
                if (slump1FlyAshCnt > 0)
                    tempVal = tempVal +  Convert.ToDecimal(slump1FlyAsh / slump1FlyAshCnt);
                if (slump1GGBSCnt > 0)
                    tempVal = tempVal +Convert.ToDecimal(slump1GGBS / slump1GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(slump1PasticDensity) / slump1PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");
                
            }
            dt.Rows.Add(dr1);
            // cement - 50 to 75
            dr1 = dt.NewRow();
            dr1["Slump"] = "50 to 75";
            if (slump2CementCnt > 0)
                dr1["AvgCount"] = slump2CementCnt.ToString();
            if (slump2CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(slump2Cement / slump2CementCnt).ToString("0.00");
            if (slump2WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(slump2WCRatio / slump2WCRatioCnt).ToString("0.00");
            if (slump2WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(slump2Water / slump2WaterCnt).ToString("0.00");
            if (slump2FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(slump2FlyAsh / slump2FlyAshCnt).ToString("0.00");
            if (slump2GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(slump2GGBS / slump2GGBSCnt).ToString("0.00");
            if (slump2SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(slump2Sand / slump2SandCnt).ToString("0.00");
            if (slump210mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(slump210mm / slump210mmCnt).ToString("0.00");
            if (slump220mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(slump220mm / slump220mmCnt).ToString("0.00");
            if (slump2300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(slump2300micronPassing / slump2300micronPassingCnt).ToString("0.00");
            if (slump2Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(slump2Strength3days / slump2Strength3daysCnt).ToString("0.00");
            if (slump2Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(slump2Strength7days / slump2Strength7daysCnt).ToString("0.00");
            if (slump2Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(slump2Strength28days / slump2Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (slump2CementCnt > 0 || slump2300micronPassingCnt > 0 || slump2FlyAshCnt > 0 || slump2GGBSCnt > 0)
            {
                if (slump2CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump2Cement / slump2CementCnt);
                if (slump2300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(slump2300micronPassing / slump2300micronPassingCnt);
                if (slump2FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(slump2FlyAsh / slump2FlyAshCnt);
                if (slump2GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(slump2GGBS / slump2GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (slump2AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(slump2Admixture / slump2AdmixtureCnt).ToString("0.00");
            
            tempVal = 0;
            if (slump2PasticDensityCnt > 0)
            {
                if (slump2CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump2Cement / slump2CementCnt);
                if (slump24p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(slump2SandWt / slump2SandCnt) * Convert.ToDecimal((slump24p75mmPassing / slump24p75mmPassingCnt)/100));
                if (slump2WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump2Water / slump2WaterCnt);
                if (slump2FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump2FlyAsh / slump2FlyAshCnt);
                if (slump2GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump2GGBS / slump2GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(slump2PasticDensity) / slump2PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement - 75 to 100
            dr1 = dt.NewRow();
            dr1["Slump"] = "75 to 100";
            if (slump3CementCnt > 0)
                dr1["AvgCount"] = slump3CementCnt.ToString();
            if (slump3CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(slump3Cement / slump3CementCnt).ToString("0.00");
            if (slump3WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(slump3WCRatio / slump3WCRatioCnt).ToString("0.00");
            if (slump3WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(slump3Water / slump3WaterCnt).ToString("0.00");
            if (slump3FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(slump3FlyAsh / slump3FlyAshCnt).ToString("0.00");
            if (slump3GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(slump3GGBS / slump3GGBSCnt).ToString("0.00");
            if (slump3SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(slump3Sand / slump3SandCnt).ToString("0.00");
            if (slump310mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(slump310mm / slump310mmCnt).ToString("0.00");
            if (slump320mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(slump320mm / slump320mmCnt).ToString("0.00");
            if (slump3300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(slump3300micronPassing / slump3300micronPassingCnt).ToString("0.00");
            if (slump3Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(slump3Strength3days / slump3Strength3daysCnt).ToString("0.00");
            if (slump3Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(slump3Strength7days / slump3Strength7daysCnt).ToString("0.00");
            if (slump3Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(slump3Strength28days / slump3Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (slump3CementCnt > 0 || slump3300micronPassingCnt > 0 || slump3FlyAshCnt > 0 || slump3GGBSCnt > 0)
            {
                if (slump3CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump3Cement / slump3CementCnt);
                if (slump3300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(slump3300micronPassing / slump3300micronPassingCnt);
                if (slump3FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(slump3FlyAsh / slump3FlyAshCnt);
                if (slump3GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(slump3GGBS / slump3GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (slump3AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(slump3Admixture / slump3AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (slump3PasticDensityCnt > 0)
            {
                if (slump3CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump3Cement / slump3CementCnt);
                if (slump34p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(slump3SandWt / slump3SandCnt) * Convert.ToDecimal((slump34p75mmPassing / slump34p75mmPassingCnt)/100));
                if (slump3WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump3Water / slump3WaterCnt);
                if (slump3FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump3FlyAsh / slump3FlyAshCnt);
                if (slump3GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump3GGBS / slump3GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(slump3PasticDensity) / slump3PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement - 100 to 125
            dr1 = dt.NewRow();
            dr1["Slump"] = "100 to 125";
            if (slump4CementCnt > 0)
                dr1["AvgCount"] = slump4CementCnt.ToString();
            if (slump4CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(slump4Cement / slump4CementCnt).ToString("0.00");
            if (slump4WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(slump4WCRatio / slump4WCRatioCnt).ToString("0.00");
            if (slump4WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(slump4Water / slump4WaterCnt).ToString("0.00");
            if (slump4FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(slump4FlyAsh / slump4FlyAshCnt).ToString("0.00");
            if (slump4GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(slump4GGBS / slump4GGBSCnt).ToString("0.00");
            if (slump4SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(slump4Sand / slump4SandCnt).ToString("0.00");
            if (slump410mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(slump410mm / slump410mmCnt).ToString("0.00");
            if (slump420mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(slump420mm / slump420mmCnt).ToString("0.00");
            if (slump4300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(slump4300micronPassing / slump4300micronPassingCnt).ToString("0.00");
            if (slump4Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(slump4Strength3days / slump4Strength3daysCnt).ToString("0.00");
            if (slump4Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(slump4Strength7days / slump4Strength7daysCnt).ToString("0.00");
            if (slump4Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(slump4Strength28days / slump4Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (slump4CementCnt > 0 || slump4300micronPassingCnt > 0 || slump4FlyAshCnt > 0 || slump4GGBSCnt > 0)
            {
                if (slump4CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump4Cement / slump4CementCnt);
                if (slump4300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(slump4300micronPassing / slump4300micronPassingCnt);
                if (slump4FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(slump4FlyAsh / slump4FlyAshCnt);
                if (slump4GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(slump4GGBS / slump4GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (slump4AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(slump4Admixture / slump4AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (slump4PasticDensityCnt > 0)
            {
                if (slump4CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump4Cement / slump4CementCnt);
                if (slump44p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(slump4SandWt / slump4SandCnt) * Convert.ToDecimal((slump44p75mmPassing / slump44p75mmPassingCnt)/100));
                if (slump4WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump4Water / slump4WaterCnt);
                if (slump4FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump4FlyAsh / slump4FlyAshCnt);
                if (slump4GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump4GGBS / slump4GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(slump4PasticDensity) / slump4PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement - 125 to 150
            dr1 = dt.NewRow();
            dr1["Slump"] = "125 to 150";
            if (slump5CementCnt > 0)
                dr1["AvgCount"] = slump5CementCnt.ToString();
            if (slump5CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(slump5Cement / slump5CementCnt).ToString("0.00");
            if (slump5WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(slump5WCRatio / slump5WCRatioCnt).ToString("0.00");
            if (slump5WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(slump5Water / slump5WaterCnt).ToString("0.00");
            if (slump5FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(slump5FlyAsh / slump5FlyAshCnt).ToString("0.00");
            if (slump5GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(slump5GGBS / slump5GGBSCnt).ToString("0.00");
            if (slump5SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(slump5Sand / slump5SandCnt).ToString("0.00");
            if (slump510mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(slump510mm / slump510mmCnt).ToString("0.00");
            if (slump520mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(slump520mm / slump520mmCnt).ToString("0.00");
            if (slump5300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(slump5300micronPassing / slump5300micronPassingCnt).ToString("0.00");
            if (slump5Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(slump5Strength3days / slump5Strength3daysCnt).ToString("0.00");
            if (slump5Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(slump5Strength7days / slump5Strength7daysCnt).ToString("0.00");
            if (slump5Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(slump5Strength28days / slump5Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (slump5CementCnt > 0 || slump5300micronPassingCnt > 0 || slump5FlyAshCnt > 0 || slump5GGBSCnt > 0)
            {
                if (slump5CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump5Cement / slump5CementCnt);
                if (slump5300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(slump5300micronPassing / slump5300micronPassingCnt);
                if (slump5FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(slump5FlyAsh / slump5FlyAshCnt);
                if (slump5GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(slump5GGBS / slump5GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (slump5AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(slump5Admixture / slump5AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (slump5PasticDensityCnt > 0)
            {
                if (slump5CementCnt > 0)
                    tempVal = Convert.ToDecimal(slump5Cement / slump5CementCnt);
                if (slump54p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(slump5SandWt / slump5SandCnt) * Convert.ToDecimal((slump54p75mmPassing / slump54p75mmPassingCnt)/100));
                if (slump5WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump5Water / slump5WaterCnt);
                if (slump5FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump5FlyAsh / slump5FlyAshCnt);
                if (slump5GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(slump5GGBS / slump5GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(slump5PasticDensity) / slump5PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);

            if (ddl_Test.SelectedIndex == 2)
            {
                // cement -  150 and above
                dr1 = dt.NewRow();
                dr1["Slump"] = "150 and Above";
                if (slump6CementCnt > 0)
                    dr1["AvgCount"] = slump6CementCnt.ToString();
                if (slump6CementCnt > 0)
                    dr1["Cement"] = Convert.ToDecimal(slump6Cement / slump6CementCnt).ToString("0.00");
                if (slump6WCRatioCnt > 0)
                    dr1["WCRatio"] = Convert.ToDecimal(slump6WCRatio / slump6WCRatioCnt).ToString("0.00");
                if (slump6WaterCnt > 0)
                    dr1["Water"] = Convert.ToDecimal(slump6Water / slump6WaterCnt).ToString("0.00");
                if (slump6FlyAshCnt > 0)
                    dr1["FlyAsh"] = Convert.ToDecimal(slump6FlyAsh / slump6FlyAshCnt).ToString("0.00");
                if (slump6GGBSCnt > 0)
                    dr1["GGBS"] = Convert.ToDecimal(slump6GGBS / slump6GGBSCnt).ToString("0.00");
                if (slump6SandCnt > 0)
                    dr1["Sand"] = Convert.ToDecimal(slump6Sand / slump6SandCnt).ToString("0.00");
                if (slump610mmCnt > 0)
                    dr1["10mm"] = Convert.ToDecimal(slump610mm / slump610mmCnt).ToString("0.00");
                if (slump620mmCnt > 0)
                    dr1["20mm"] = Convert.ToDecimal(slump620mm / slump620mmCnt).ToString("0.00");
                if (slump6300micronPassingCnt > 0)
                    dr1["300micronPassing"] = Convert.ToDecimal(slump6300micronPassing / slump6300micronPassingCnt).ToString("0.00");
                if (slump6Strength3daysCnt > 0)
                    dr1["3DaysStrength"] = Convert.ToDecimal(slump6Strength3days / slump6Strength3daysCnt).ToString("0.00");
                if (slump6Strength7daysCnt > 0)
                    dr1["7DaysStrength"] = Convert.ToDecimal(slump6Strength7days / slump6Strength7daysCnt).ToString("0.00");
                if (slump6Strength28daysCnt > 0)
                    dr1["28DaysStrength"] = Convert.ToDecimal(slump6Strength28days / slump6Strength28daysCnt).ToString("0.00");

                tempVal = 0;
                if (slump6CementCnt > 0 || slump6300micronPassingCnt > 0 || slump6FlyAshCnt > 0 || slump6GGBSCnt > 0)
                {
                    if (slump6CementCnt > 0)
                        tempVal = Convert.ToDecimal(slump6Cement / slump6CementCnt);
                    if (slump6300micronPassingCnt > 0)
                        tempVal += Convert.ToDecimal(slump6300micronPassing / slump6300micronPassingCnt);
                    if (slump6FlyAshCnt > 0)
                        tempVal += Convert.ToDecimal(slump6FlyAsh / slump6FlyAshCnt);
                    if (slump6GGBSCnt > 0)
                        tempVal += Convert.ToDecimal(slump6GGBS / slump6GGBSCnt);
                    dr1["300micronPlusCement"] = tempVal.ToString("0.00");
                }
                if (slump6AdmixtureCnt > 0)
                    dr1["Admixture"] = Convert.ToDecimal(slump6Admixture / slump6AdmixtureCnt).ToString("0.00");

                tempVal = 0;
                if (slump6PasticDensityCnt > 0)
                {
                    if (slump6CementCnt > 0)
                        tempVal = Convert.ToDecimal(slump6Cement / slump6CementCnt);
                    if (slump64p75mmPassingCnt > 0)
                        tempVal = tempVal + (Convert.ToDecimal(slump6SandWt / slump6SandCnt) * Convert.ToDecimal((slump64p75mmPassing / slump64p75mmPassingCnt)/100));
                    if (slump6WaterCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(slump6Water / slump6WaterCnt);
                    if (slump6FlyAshCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(slump6FlyAsh / slump6FlyAshCnt);
                    if (slump6GGBSCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(slump6GGBS / slump6GGBSCnt);

                    tempVal = tempVal / (Convert.ToDecimal(slump6PasticDensity) / slump6PasticDensityCnt);
                    dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

                }
                dt.Rows.Add(dr1);
            }
            #endregion
            //Flyash + Cement
            dr1 = dt.NewRow();
            dr1["Slump"] = " ";
            dt.Rows.Add(dr1);
            dr1 = dt.NewRow();
            dr1["Slump"] = "Flyash + Cement";
            dt.Rows.Add(dr1);
            #region Flyash + cement
            // cement+flyash - 0 to 50
            dr1 = dt.NewRow();
            dr1["Slump"] = "0 to 50";
            if (fslump1CementCnt > 0)
                dr1["AvgCount"] = fslump1CementCnt.ToString();
            if (fslump1CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(fslump1Cement / fslump1CementCnt).ToString("0.00");
            if (fslump1WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(fslump1WCRatio / fslump1WCRatioCnt).ToString("0.00");
            if (fslump1WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(fslump1Water / fslump1WaterCnt).ToString("0.00");
            if (fslump1FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(fslump1FlyAsh / fslump1FlyAshCnt).ToString("0.00");
            if (fslump1GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(fslump1GGBS / fslump1GGBSCnt).ToString("0.00");
            if (fslump1SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(fslump1Sand / fslump1SandCnt).ToString("0.00");
            if (fslump110mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(fslump110mm / fslump110mmCnt).ToString("0.00");
            if (fslump120mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(fslump120mm / fslump120mmCnt).ToString("0.00");
            if (fslump1300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(fslump1300micronPassing / fslump1300micronPassingCnt).ToString("0.00");
            if (fslump1Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(fslump1Strength3days / fslump1Strength3daysCnt).ToString("0.00");
            if (fslump1Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(fslump1Strength7days / fslump1Strength7daysCnt).ToString("0.00");
            if (fslump1Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(fslump1Strength28days / fslump1Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (fslump1CementCnt > 0 || fslump1300micronPassingCnt > 0 || fslump1FlyAshCnt > 0 || fslump1GGBSCnt > 0)
            {
                if (fslump1CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump1Cement / fslump1CementCnt);
                if (fslump1300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(fslump1300micronPassing / fslump1300micronPassingCnt);
                if (fslump1FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(fslump1FlyAsh / fslump1FlyAshCnt);
                if (fslump1GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(fslump1GGBS / fslump1GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (fslump1AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(fslump1Admixture / fslump1AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (fslump1PasticDensityCnt > 0)
            {
                //dr1["Cement4.75mmWaterDivPlasticDensity"] = ((Convert.ToDecimal(fslump1Cement / fslump1CementCnt) + Convert.ToDecimal(fslump14p75mmPassing / fslump14p75mmPassingCnt)
                //    + Convert.ToDecimal(fslump1Water / fslump1WaterCnt)) / (Convert.ToDecimal(fslump1PasticDensity) / fslump1PasticDensityCnt)).ToString("0.00");

                if (fslump1CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump1Cement / fslump1CementCnt);
                if (fslump14p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(fslump1SandWt / fslump1SandCnt) * Convert.ToDecimal((fslump14p75mmPassing / fslump14p75mmPassingCnt)/100));
                if (fslump1WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump1Water / fslump1WaterCnt);
                if (fslump1FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump1FlyAsh / fslump1FlyAshCnt);
                if (fslump1GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump1GGBS / fslump1GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(fslump1PasticDensity) / fslump1PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+flyash - 50 to 75
            dr1 = dt.NewRow();
            dr1["Slump"] = "50 to 75";
            if (fslump2CementCnt > 0)
                dr1["AvgCount"] = fslump2CementCnt.ToString();
            if (fslump2CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(fslump2Cement / fslump2CementCnt).ToString("0.00");
            if (fslump2WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(fslump2WCRatio / fslump2WCRatioCnt).ToString("0.00");
            if (fslump2WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(fslump2Water / fslump2WaterCnt).ToString("0.00");
            if (fslump2FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(fslump2FlyAsh / fslump2FlyAshCnt).ToString("0.00");
            if (fslump2GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(fslump2GGBS / fslump2GGBSCnt).ToString("0.00");
            if (fslump2SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(fslump2Sand / fslump2SandCnt).ToString("0.00");
            if (fslump210mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(fslump210mm / fslump210mmCnt).ToString("0.00");
            if (fslump220mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(fslump220mm / fslump220mmCnt).ToString("0.00");
            if (fslump2300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(fslump2300micronPassing / fslump2300micronPassingCnt).ToString("0.00");
            if (fslump2Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(fslump2Strength3days / fslump2Strength3daysCnt).ToString("0.00");
            if (fslump2Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(fslump2Strength7days / fslump2Strength7daysCnt).ToString("0.00");
            if (fslump2Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(fslump2Strength28days / fslump2Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (fslump2CementCnt > 0 || fslump2300micronPassingCnt > 0 || fslump2FlyAshCnt > 0 || fslump2GGBSCnt > 0)
            {
                if (fslump2CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump2Cement / fslump2CementCnt);
                if (fslump2300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(fslump2300micronPassing / fslump2300micronPassingCnt);
                if (fslump2FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(fslump2FlyAsh / fslump2FlyAshCnt);
                if (fslump2GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(fslump2GGBS / fslump2GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (fslump2AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(fslump2Admixture / fslump2AdmixtureCnt).ToString("0.00");
            
            tempVal = 0;
            if (fslump2PasticDensityCnt > 0)
            {
                if (fslump2CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump2Cement / fslump2CementCnt);
                if (fslump24p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(fslump2SandWt / fslump2SandCnt) * Convert.ToDecimal((fslump24p75mmPassing / fslump24p75mmPassingCnt)/100));
                if (fslump2WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump2Water / fslump2WaterCnt);
                if (fslump2FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump2FlyAsh / fslump2FlyAshCnt);
                if (fslump2GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump2GGBS / fslump2GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(fslump2PasticDensity) / fslump2PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+flyash - 75 to 100
            dr1 = dt.NewRow();
            dr1["Slump"] = "75 to 100";
            if (fslump3CementCnt > 0)
                dr1["AvgCount"] = fslump3CementCnt.ToString();
            if (fslump3CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(fslump3Cement / fslump3CementCnt).ToString("0.00");
            if (fslump3WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(fslump3WCRatio / fslump3WCRatioCnt).ToString("0.00");
            if (fslump3WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(fslump3Water / fslump3WaterCnt).ToString("0.00");
            if (fslump3FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(fslump3FlyAsh / fslump3FlyAshCnt).ToString("0.00");
            if (fslump3GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(fslump3GGBS / fslump3GGBSCnt).ToString("0.00");
            if (fslump3SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(fslump3Sand / fslump3SandCnt).ToString("0.00");
            if (fslump310mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(fslump310mm / fslump310mmCnt).ToString("0.00");
            if (fslump320mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(fslump320mm / fslump320mmCnt).ToString("0.00");
            if (fslump3300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(fslump3300micronPassing / fslump3300micronPassingCnt).ToString("0.00");
            if (fslump3Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(fslump3Strength3days / fslump3Strength3daysCnt).ToString("0.00");
            if (fslump3Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(fslump3Strength7days / fslump3Strength7daysCnt).ToString("0.00");
            if (fslump3Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(fslump3Strength28days / fslump3Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (fslump3CementCnt > 0 || fslump3300micronPassingCnt > 0 || fslump3FlyAshCnt > 0 || fslump3GGBSCnt > 0)
            {
                if (fslump3CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump3Cement / fslump3CementCnt);
                if (fslump3300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(fslump3300micronPassing / fslump3300micronPassingCnt);
                if (fslump3FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(fslump3FlyAsh / fslump3FlyAshCnt);
                if (fslump3GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(fslump3GGBS / fslump3GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (fslump3AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(fslump3Admixture / fslump3AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (fslump3PasticDensityCnt > 0)
            {
               if (fslump3CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump3Cement / fslump3CementCnt);
                if (fslump34p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(fslump3SandWt / fslump3SandCnt) * Convert.ToDecimal((fslump34p75mmPassing / fslump34p75mmPassingCnt)/100));
                if (fslump3WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump3Water / fslump3WaterCnt);
                if (fslump3FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump3FlyAsh / fslump3FlyAshCnt);
                if (fslump3GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump3GGBS / fslump3GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(fslump3PasticDensity) / fslump3PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+flyash - 100 to 125
            dr1 = dt.NewRow();
            dr1["Slump"] = "100 to 125";
            if (fslump4CementCnt > 0)
                dr1["AvgCount"] = fslump4CementCnt.ToString();
            if (fslump4CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(fslump4Cement / fslump4CementCnt).ToString("0.00");
            if (fslump4WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(fslump4WCRatio / fslump4WCRatioCnt).ToString("0.00");
            if (fslump4WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(fslump4Water / fslump4WaterCnt).ToString("0.00");
            if (fslump4FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(fslump4FlyAsh / fslump4FlyAshCnt).ToString("0.00");
            if (fslump4GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(fslump4GGBS / fslump4GGBSCnt).ToString("0.00");
            if (fslump4SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(fslump4Sand / fslump4SandCnt).ToString("0.00");
            if (fslump410mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(fslump410mm / fslump410mmCnt).ToString("0.00");
            if (fslump420mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(fslump420mm / fslump420mmCnt).ToString("0.00");
            if (fslump4300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(fslump4300micronPassing / fslump4300micronPassingCnt).ToString("0.00");
            if (fslump4Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(fslump4Strength3days / fslump4Strength3daysCnt).ToString("0.00");
            if (fslump4Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(fslump4Strength7days / fslump4Strength7daysCnt).ToString("0.00");
            if (fslump4Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(fslump4Strength28days / fslump4Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (fslump4CementCnt > 0 || fslump4300micronPassingCnt > 0 || fslump4FlyAshCnt > 0 || fslump4GGBSCnt > 0)
            {
                if (fslump4CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump4Cement / fslump4CementCnt);
                if (fslump4300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(fslump4300micronPassing / fslump4300micronPassingCnt);
                if (fslump4FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(fslump4FlyAsh / fslump4FlyAshCnt);
                if (fslump4GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(fslump4GGBS / fslump4GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (fslump4AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(fslump4Admixture / fslump4AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (fslump4PasticDensityCnt > 0)
            {
                if (fslump4CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump4Cement / fslump4CementCnt);
                if (fslump44p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(fslump4SandWt / fslump4SandCnt) * Convert.ToDecimal((fslump44p75mmPassing / fslump44p75mmPassingCnt)/100));
                if (fslump4WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump4Water / fslump4WaterCnt);
                if (fslump4FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump4FlyAsh / fslump4FlyAshCnt);
                if (fslump4GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump4GGBS / fslump4GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(fslump4PasticDensity) / fslump4PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+flyash - 125 to 150
            dr1 = dt.NewRow();
            dr1["Slump"] = "125 to 150";
            if (fslump5CementCnt > 0)
                dr1["AvgCount"] = fslump5CementCnt.ToString();
            if (fslump5CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(fslump5Cement / fslump5CementCnt).ToString("0.00");
            if (fslump5WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(fslump5WCRatio / fslump5WCRatioCnt).ToString("0.00");
            if (fslump5WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(fslump5Water / fslump5WaterCnt).ToString("0.00");
            if (fslump5FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(fslump5FlyAsh / fslump5FlyAshCnt).ToString("0.00");
            if (fslump5GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(fslump5GGBS / fslump5GGBSCnt).ToString("0.00");
            if (fslump5SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(fslump5Sand / fslump5SandCnt).ToString("0.00");
            if (fslump510mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(fslump510mm / fslump510mmCnt).ToString("0.00");
            if (fslump520mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(fslump520mm / fslump520mmCnt).ToString("0.00");
            if (fslump5300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(fslump5300micronPassing / fslump5300micronPassingCnt).ToString("0.00");
            if (fslump5Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(fslump5Strength3days / fslump5Strength3daysCnt).ToString("0.00");
            if (fslump5Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(fslump5Strength7days / fslump5Strength7daysCnt).ToString("0.00");
            if (fslump5Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(fslump5Strength28days / fslump5Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (fslump5CementCnt > 0 || fslump5300micronPassingCnt > 0 || fslump5FlyAshCnt > 0 || fslump5GGBSCnt > 0)
            {
                if (fslump5CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump5Cement / fslump5CementCnt);
                if (fslump5300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(fslump5300micronPassing / fslump5300micronPassingCnt);
                if (fslump5FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(fslump5FlyAsh / fslump5FlyAshCnt);
                if (fslump5GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(fslump5GGBS / fslump5GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (fslump5AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(fslump5Admixture / fslump5AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (fslump5PasticDensityCnt > 0)
            {
                if (fslump5CementCnt > 0)
                    tempVal = Convert.ToDecimal(fslump5Cement / fslump5CementCnt);
                if (fslump54p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(fslump5SandWt / fslump5SandCnt) * Convert.ToDecimal((fslump54p75mmPassing / fslump54p75mmPassingCnt)/100));
                if (fslump5WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump5Water / fslump5WaterCnt);
                if (fslump5FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump5FlyAsh / fslump5FlyAshCnt);
                if (fslump5GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(fslump5GGBS / fslump5GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(fslump5PasticDensity) / fslump5PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            if (ddl_Test.SelectedIndex == 2)
            {
                // cement+flyash -  150 and above
                dr1 = dt.NewRow();
                dr1["Slump"] = "150 and Above";
                if (fslump6CementCnt > 0)
                    dr1["AvgCount"] = fslump6CementCnt.ToString();
                if (fslump6CementCnt > 0)
                    dr1["Cement"] = Convert.ToDecimal(fslump6Cement / fslump6CementCnt).ToString("0.00");
                if (fslump6WCRatioCnt > 0)
                    dr1["WCRatio"] = Convert.ToDecimal(fslump6WCRatio / fslump6WCRatioCnt).ToString("0.00");
                if (fslump6WaterCnt > 0)
                    dr1["Water"] = Convert.ToDecimal(fslump6Water / fslump6WaterCnt).ToString("0.00");
                if (fslump6FlyAshCnt > 0)
                    dr1["FlyAsh"] = Convert.ToDecimal(fslump6FlyAsh / fslump6FlyAshCnt).ToString("0.00");
                if (fslump6GGBSCnt > 0)
                    dr1["GGBS"] = Convert.ToDecimal(fslump6GGBS / fslump6GGBSCnt).ToString("0.00");
                if (fslump6SandCnt > 0)
                    dr1["Sand"] = Convert.ToDecimal(fslump6Sand / fslump6SandCnt).ToString("0.00");
                if (fslump610mmCnt > 0)
                    dr1["10mm"] = Convert.ToDecimal(fslump610mm / fslump610mmCnt).ToString("0.00");
                if (fslump620mmCnt > 0)
                    dr1["20mm"] = Convert.ToDecimal(fslump620mm / fslump620mmCnt).ToString("0.00");
                if (fslump6300micronPassingCnt > 0)
                    dr1["300micronPassing"] = Convert.ToDecimal(fslump6300micronPassing / fslump6300micronPassingCnt).ToString("0.00");
                if (fslump6Strength3daysCnt > 0)
                    dr1["3DaysStrength"] = Convert.ToDecimal(fslump6Strength3days / fslump6Strength3daysCnt).ToString("0.00");
                if (fslump6Strength7daysCnt > 0)
                    dr1["7DaysStrength"] = Convert.ToDecimal(fslump6Strength7days / fslump6Strength7daysCnt).ToString("0.00");
                if (fslump6Strength28daysCnt > 0)
                    dr1["28DaysStrength"] = Convert.ToDecimal(fslump6Strength28days / fslump6Strength28daysCnt).ToString("0.00");

                tempVal = 0;
                if (fslump6CementCnt > 0 || fslump6300micronPassingCnt > 0 || fslump6FlyAshCnt > 0 || fslump6GGBSCnt > 0)
                {
                    if (fslump6CementCnt > 0)
                        tempVal = Convert.ToDecimal(fslump6Cement / fslump6CementCnt);
                    if (fslump6300micronPassingCnt > 0)
                        tempVal += Convert.ToDecimal(fslump6300micronPassing / fslump6300micronPassingCnt);
                    if (fslump6FlyAshCnt > 0)
                        tempVal += Convert.ToDecimal(fslump6FlyAsh / fslump6FlyAshCnt);
                    if (fslump6GGBSCnt > 0)
                        tempVal += Convert.ToDecimal(fslump6GGBS / fslump6GGBSCnt);
                    dr1["300micronPlusCement"] = tempVal.ToString("0.00");
                }
                if (fslump6AdmixtureCnt > 0)
                    dr1["Admixture"] = Convert.ToDecimal(fslump6Admixture / fslump6AdmixtureCnt).ToString("0.00");

                tempVal = 0;
                if (fslump6PasticDensityCnt > 0)
                {
                    if (fslump6CementCnt > 0)
                        tempVal = Convert.ToDecimal(fslump6Cement / fslump6CementCnt);
                    if (fslump64p75mmPassingCnt > 0)
                        tempVal = tempVal + (Convert.ToDecimal(fslump6SandWt / fslump6SandCnt) * Convert.ToDecimal((fslump64p75mmPassing / fslump64p75mmPassingCnt)/100));
                    if (fslump6WaterCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(fslump6Water / fslump6WaterCnt);
                    if (fslump6FlyAshCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(fslump6FlyAsh / fslump6FlyAshCnt);
                    if (fslump6GGBSCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(fslump6GGBS / fslump6GGBSCnt);

                    tempVal = tempVal / (Convert.ToDecimal(fslump6PasticDensity) / fslump6PasticDensityCnt);
                    dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

                }
                dt.Rows.Add(dr1);
            }
            #endregion
            //GGBS + Cement
            dr1 = dt.NewRow();
            dr1["Slump"] = " ";
            dt.Rows.Add(dr1);
            dr1 = dt.NewRow();
            dr1["Slump"] = "GGBS + Cement";
            dt.Rows.Add(dr1);
            #region GGBS + cement
            // cement+ggbs - 0 to 50
            dr1 = dt.NewRow();
            dr1["Slump"] = "0 to 50";
            if (gslump1CementCnt > 0)
                dr1["AvgCount"] = gslump1CementCnt.ToString();
            if (gslump1CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(gslump1Cement / gslump1CementCnt).ToString("0.00");
            if (gslump1WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(gslump1WCRatio / gslump1WCRatioCnt).ToString("0.00");
            if (gslump1WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(gslump1Water / gslump1WaterCnt).ToString("0.00");
            if (gslump1FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(gslump1FlyAsh / gslump1FlyAshCnt).ToString("0.00");
            if (gslump1GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(gslump1GGBS / gslump1GGBSCnt).ToString("0.00");
            if (gslump1SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(gslump1Sand / gslump1SandCnt).ToString("0.00");
            if (gslump110mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(gslump110mm / gslump110mmCnt).ToString("0.00");
            if (gslump120mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(gslump120mm / gslump120mmCnt).ToString("0.00");
            if (gslump1300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(gslump1300micronPassing / gslump1300micronPassingCnt).ToString("0.00");
            if (gslump1Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(gslump1Strength3days / gslump1Strength3daysCnt).ToString("0.00");
            if (gslump1Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(gslump1Strength7days / gslump1Strength7daysCnt).ToString("0.00");
            if (gslump1Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(gslump1Strength28days / gslump1Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (gslump1CementCnt > 0 || gslump1300micronPassingCnt > 0 || gslump1FlyAshCnt > 0 || gslump1GGBSCnt > 0)
            {
                if (gslump1CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump1Cement / gslump1CementCnt);
                if (gslump1300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(gslump1300micronPassing / gslump1300micronPassingCnt);
                if (gslump1FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(gslump1FlyAsh / gslump1FlyAshCnt);
                if (gslump1GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(gslump1GGBS / gslump1GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (gslump1AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(gslump1Admixture / gslump1AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (gslump1PasticDensityCnt > 0)
            {
                //dr1["Cement4.75mmWaterDivPlasticDensity"] = ((Convert.ToDecimal(gslump1Cement / gslump1CementCnt) + Convert.ToDecimal(gslump14p75mmPassing / gslump14p75mmPassingCnt)
                //    + Convert.ToDecimal(gslump1Water / gslump1WaterCnt)) / (Convert.ToDecimal(gslump1PasticDensity) / gslump1PasticDensityCnt)).ToString("0.00");

                if (gslump1CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump1Cement / gslump1CementCnt);
                if (gslump14p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(gslump1SandWt / gslump1SandCnt) * Convert.ToDecimal((gslump14p75mmPassing / gslump14p75mmPassingCnt)/100));
                if (gslump1WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump1Water / gslump1WaterCnt);
                if (gslump1FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump1FlyAsh / gslump1FlyAshCnt);
                if (gslump1GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump1GGBS / gslump1GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(gslump1PasticDensity) / gslump1PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+ggbs - 50 to 75
            dr1 = dt.NewRow();
            dr1["Slump"] = "50 to 75";
            if (gslump2CementCnt > 0)
                dr1["AvgCount"] = gslump2CementCnt.ToString();
            if (gslump2CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(gslump2Cement / gslump2CementCnt).ToString("0.00");
            if (gslump2WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(gslump2WCRatio / gslump2WCRatioCnt).ToString("0.00");
            if (gslump2WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(gslump2Water / gslump2WaterCnt).ToString("0.00");
            if (gslump2FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(gslump2FlyAsh / gslump2FlyAshCnt).ToString("0.00");
            if (gslump2GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(gslump2GGBS / gslump2GGBSCnt).ToString("0.00");
            if (gslump2SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(gslump2Sand / gslump2SandCnt).ToString("0.00");
            if (gslump210mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(gslump210mm / gslump210mmCnt).ToString("0.00");
            if (gslump220mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(gslump220mm / gslump220mmCnt).ToString("0.00");
            if (gslump2300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(gslump2300micronPassing / gslump2300micronPassingCnt).ToString("0.00");
            if (gslump2Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(gslump2Strength3days / gslump2Strength3daysCnt).ToString("0.00");
            if (gslump2Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(gslump2Strength7days / gslump2Strength7daysCnt).ToString("0.00");
            if (gslump2Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(gslump2Strength28days / gslump2Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (gslump2CementCnt > 0 || gslump2300micronPassingCnt > 0 || gslump2FlyAshCnt > 0 || gslump2GGBSCnt > 0)
            {
                if (gslump2CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump2Cement / gslump2CementCnt);
                if (gslump2300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(gslump2300micronPassing / gslump2300micronPassingCnt);
                if (gslump2FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(gslump2FlyAsh / gslump2FlyAshCnt);
                if (gslump2GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(gslump2GGBS / gslump2GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (gslump2AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(gslump2Admixture / gslump2AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (gslump2PasticDensityCnt > 0)
            {
                if (gslump2CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump2Cement / gslump2CementCnt);
                if (gslump24p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(gslump2SandWt / gslump2SandCnt) * Convert.ToDecimal((gslump24p75mmPassing / gslump24p75mmPassingCnt)/100));
                if (gslump2WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump2Water / gslump2WaterCnt);
                if (gslump2FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump2FlyAsh / gslump2FlyAshCnt);
                if (gslump2GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump2GGBS / gslump2GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(gslump2PasticDensity) / gslump2PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+ggbs - 75 to 100
            dr1 = dt.NewRow();
            dr1["Slump"] = "75 to 100";
            if (gslump3CementCnt > 0)
                dr1["AvgCount"] = gslump3CementCnt.ToString();
            if (gslump3CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(gslump3Cement / gslump3CementCnt).ToString("0.00");
            if (gslump3WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(gslump3WCRatio / gslump3WCRatioCnt).ToString("0.00");
            if (gslump3WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(gslump3Water / gslump3WaterCnt).ToString("0.00");
            if (gslump3FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(gslump3FlyAsh / gslump3FlyAshCnt).ToString("0.00");
            if (gslump3GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(gslump3GGBS / gslump3GGBSCnt).ToString("0.00");
            if (gslump3SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(gslump3Sand / gslump3SandCnt).ToString("0.00");
            if (gslump310mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(gslump310mm / gslump310mmCnt).ToString("0.00");
            if (gslump320mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(gslump320mm / gslump320mmCnt).ToString("0.00");
            if (gslump3300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(gslump3300micronPassing / gslump3300micronPassingCnt).ToString("0.00");
            if (gslump3Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(gslump3Strength3days / gslump3Strength3daysCnt).ToString("0.00");
            if (gslump3Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(gslump3Strength7days / gslump3Strength7daysCnt).ToString("0.00");
            if (gslump3Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(gslump3Strength28days / gslump3Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (gslump3CementCnt > 0 || gslump3300micronPassingCnt > 0 || gslump3FlyAshCnt > 0 || gslump3GGBSCnt > 0)
            {
                if (gslump3CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump3Cement / gslump3CementCnt);
                if (gslump3300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(gslump3300micronPassing / gslump3300micronPassingCnt);
                if (gslump3FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(gslump3FlyAsh / gslump3FlyAshCnt);
                if (gslump3GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(gslump3GGBS / gslump3GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (gslump3AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(gslump3Admixture / gslump3AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (gslump3PasticDensityCnt > 0)
            {
                if (gslump3CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump3Cement / gslump3CementCnt);
                if (gslump34p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(gslump3SandWt / gslump3SandCnt) * Convert.ToDecimal((gslump34p75mmPassing / gslump34p75mmPassingCnt)/100));
                if (gslump3WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump3Water / gslump3WaterCnt);
                if (gslump3FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump3FlyAsh / gslump3FlyAshCnt);
                if (gslump3GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump3GGBS / gslump3GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(gslump3PasticDensity) / gslump3PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+ggbs - 100 to 125
            dr1 = dt.NewRow();
            dr1["Slump"] = "100 to 125";
            if (gslump4CementCnt > 0)
                dr1["AvgCount"] = gslump4CementCnt.ToString();
            if (gslump4CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(gslump4Cement / gslump4CementCnt).ToString("0.00");
            if (gslump4WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(gslump4WCRatio / gslump4WCRatioCnt).ToString("0.00");
            if (gslump4WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(gslump4Water / gslump4WaterCnt).ToString("0.00");
            if (gslump4FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(gslump4FlyAsh / gslump4FlyAshCnt).ToString("0.00");
            if (gslump4GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(gslump4GGBS / gslump4GGBSCnt).ToString("0.00");
            if (gslump4SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(gslump4Sand / gslump4SandCnt).ToString("0.00");
            if (gslump410mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(gslump410mm / gslump410mmCnt).ToString("0.00");
            if (gslump420mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(gslump420mm / gslump420mmCnt).ToString("0.00");
            if (gslump4300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(gslump4300micronPassing / gslump4300micronPassingCnt).ToString("0.00");
            if (gslump4Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(gslump4Strength3days / gslump4Strength3daysCnt).ToString("0.00");
            if (gslump4Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(gslump4Strength7days / gslump4Strength7daysCnt).ToString("0.00");
            if (gslump4Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(gslump4Strength28days / gslump4Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (gslump4CementCnt > 0 || gslump4300micronPassingCnt > 0 || gslump4FlyAshCnt > 0 || gslump4GGBSCnt > 0)
            {
                if (gslump4CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump4Cement / gslump4CementCnt);
                if (gslump4300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(gslump4300micronPassing / gslump4300micronPassingCnt);
                if (gslump4FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(gslump4FlyAsh / gslump4FlyAshCnt);
                if (gslump4GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(gslump4GGBS / gslump4GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (gslump4AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(gslump4Admixture / gslump4AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (gslump4PasticDensityCnt > 0)
            {
                if (gslump4CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump4Cement / gslump4CementCnt);
                if (gslump44p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(gslump4SandWt / gslump4SandCnt) * Convert.ToDecimal((gslump44p75mmPassing / gslump44p75mmPassingCnt)/100));
                if (gslump4WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump4Water / gslump4WaterCnt);
                if (gslump4FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump4FlyAsh / gslump4FlyAshCnt);
                if (gslump4GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump4GGBS / gslump4GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(gslump4PasticDensity) / gslump4PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            // cement+ggbs - 125 to 150
            dr1 = dt.NewRow();
            dr1["Slump"] = "125 to 150";
            if (gslump5CementCnt > 0)
                dr1["AvgCount"] = gslump5CementCnt.ToString();
            if (gslump5CementCnt > 0)
                dr1["Cement"] = Convert.ToDecimal(gslump5Cement / gslump5CementCnt).ToString("0.00");
            if (gslump5WCRatioCnt > 0)
                dr1["WCRatio"] = Convert.ToDecimal(gslump5WCRatio / gslump5WCRatioCnt).ToString("0.00");
            if (gslump5WaterCnt > 0)
                dr1["Water"] = Convert.ToDecimal(gslump5Water / gslump5WaterCnt).ToString("0.00");
            if (gslump5FlyAshCnt > 0)
                dr1["FlyAsh"] = Convert.ToDecimal(gslump5FlyAsh / gslump5FlyAshCnt).ToString("0.00");
            if (gslump5GGBSCnt > 0)
                dr1["GGBS"] = Convert.ToDecimal(gslump5GGBS / gslump5GGBSCnt).ToString("0.00");
            if (gslump5SandCnt > 0)
                dr1["Sand"] = Convert.ToDecimal(gslump5Sand / gslump5SandCnt).ToString("0.00");
            if (gslump510mmCnt > 0)
                dr1["10mm"] = Convert.ToDecimal(gslump510mm / gslump510mmCnt).ToString("0.00");
            if (gslump520mmCnt > 0)
                dr1["20mm"] = Convert.ToDecimal(gslump520mm / gslump520mmCnt).ToString("0.00");
            if (gslump5300micronPassingCnt > 0)
                dr1["300micronPassing"] = Convert.ToDecimal(gslump5300micronPassing / gslump5300micronPassingCnt).ToString("0.00");
            if (gslump5Strength3daysCnt > 0)
                dr1["3DaysStrength"] = Convert.ToDecimal(gslump5Strength3days / gslump5Strength3daysCnt).ToString("0.00");
            if (gslump5Strength7daysCnt > 0)
                dr1["7DaysStrength"] = Convert.ToDecimal(gslump5Strength7days / gslump5Strength7daysCnt).ToString("0.00");
            if (gslump5Strength28daysCnt > 0)
                dr1["28DaysStrength"] = Convert.ToDecimal(gslump5Strength28days / gslump5Strength28daysCnt).ToString("0.00");

            tempVal = 0;
            if (gslump5CementCnt > 0 || gslump5300micronPassingCnt > 0 || gslump5FlyAshCnt > 0 || gslump5GGBSCnt > 0)
            {
                if (gslump5CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump5Cement / gslump5CementCnt);
                if (gslump5300micronPassingCnt > 0)
                    tempVal += Convert.ToDecimal(gslump5300micronPassing / gslump5300micronPassingCnt);
                if (gslump5FlyAshCnt > 0)
                    tempVal += Convert.ToDecimal(gslump5FlyAsh / gslump5FlyAshCnt);
                if (gslump5GGBSCnt > 0)
                    tempVal += Convert.ToDecimal(gslump5GGBS / gslump5GGBSCnt);
                dr1["300micronPlusCement"] = tempVal.ToString("0.00");
            }
            if (gslump5AdmixtureCnt > 0)
                dr1["Admixture"] = Convert.ToDecimal(gslump5Admixture / gslump5AdmixtureCnt).ToString("0.00");

            tempVal = 0;
            if (gslump5PasticDensityCnt > 0)
            {
                if (gslump5CementCnt > 0)
                    tempVal = Convert.ToDecimal(gslump5Cement / gslump5CementCnt);
                if (gslump54p75mmPassingCnt > 0)
                    tempVal = tempVal + (Convert.ToDecimal(gslump5SandWt / gslump5SandCnt) * Convert.ToDecimal((gslump54p75mmPassing / gslump54p75mmPassingCnt)/100));
                if (gslump5WaterCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump5Water / gslump5WaterCnt);
                if (gslump5FlyAshCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump5FlyAsh / gslump5FlyAshCnt);
                if (gslump5GGBSCnt > 0)
                    tempVal = tempVal + Convert.ToDecimal(gslump5GGBS / gslump5GGBSCnt);

                tempVal = tempVal / (Convert.ToDecimal(gslump5PasticDensity) / gslump5PasticDensityCnt);
                dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

            }
            dt.Rows.Add(dr1);
            if (ddl_Test.SelectedIndex == 2)
            {
                // cement+ggbs -  150 and above
                dr1 = dt.NewRow();
                dr1["Slump"] = "150 and Above";
                if (gslump6CementCnt > 0)
                    dr1["AvgCount"] = gslump6CementCnt.ToString();
                if (gslump6CementCnt > 0)
                    dr1["Cement"] = Convert.ToDecimal(gslump6Cement / gslump6CementCnt).ToString("0.00");
                if (gslump6WCRatioCnt > 0)
                    dr1["WCRatio"] = Convert.ToDecimal(gslump6WCRatio / gslump6WCRatioCnt).ToString("0.00");
                if (gslump6WaterCnt > 0)
                    dr1["Water"] = Convert.ToDecimal(gslump6Water / gslump6WaterCnt).ToString("0.00");
                if (gslump6FlyAshCnt > 0)
                    dr1["FlyAsh"] = Convert.ToDecimal(gslump6FlyAsh / gslump6FlyAshCnt).ToString("0.00");
                if (gslump6GGBSCnt > 0)
                    dr1["GGBS"] = Convert.ToDecimal(gslump6GGBS / gslump6GGBSCnt).ToString("0.00");
                if (gslump6SandCnt > 0)
                    dr1["Sand"] = Convert.ToDecimal(gslump6Sand / gslump6SandCnt).ToString("0.00");
                if (gslump610mmCnt > 0)
                    dr1["10mm"] = Convert.ToDecimal(gslump610mm / gslump610mmCnt).ToString("0.00");
                if (gslump620mmCnt > 0)
                    dr1["20mm"] = Convert.ToDecimal(gslump620mm / gslump620mmCnt).ToString("0.00");
                if (gslump6300micronPassingCnt > 0)
                    dr1["300micronPassing"] = Convert.ToDecimal(gslump6300micronPassing / gslump6300micronPassingCnt).ToString("0.00");
                if (gslump6Strength3daysCnt > 0)
                    dr1["3DaysStrength"] = Convert.ToDecimal(gslump6Strength3days / gslump6Strength3daysCnt).ToString("0.00");
                if (gslump6Strength7daysCnt > 0)
                    dr1["7DaysStrength"] = Convert.ToDecimal(gslump6Strength7days / gslump6Strength7daysCnt).ToString("0.00");
                if (gslump6Strength28daysCnt > 0)
                    dr1["28DaysStrength"] = Convert.ToDecimal(gslump6Strength28days / gslump6Strength28daysCnt).ToString("0.00");

                tempVal = 0;
                if (gslump6CementCnt > 0 || gslump6300micronPassingCnt > 0 || gslump6FlyAshCnt > 0 || gslump6GGBSCnt > 0)
                {
                    if (gslump6CementCnt > 0)
                        tempVal = Convert.ToDecimal(gslump6Cement / gslump6CementCnt);
                    if (gslump6300micronPassingCnt > 0)
                        tempVal += Convert.ToDecimal(gslump6300micronPassing / gslump6300micronPassingCnt);
                    if (gslump6FlyAshCnt > 0)
                        tempVal += Convert.ToDecimal(gslump6FlyAsh / gslump6FlyAshCnt);
                    if (gslump6GGBSCnt > 0)
                        tempVal += Convert.ToDecimal(gslump6GGBS / gslump6GGBSCnt);
                    dr1["300micronPlusCement"] = tempVal.ToString("0.00");
                }
                if (gslump6AdmixtureCnt > 0)
                    dr1["Admixture"] = Convert.ToDecimal(gslump6Admixture / gslump6AdmixtureCnt).ToString("0.00");

                tempVal = 0;
                if (gslump6PasticDensityCnt > 0)
                {
                    if (gslump6CementCnt > 0)
                        tempVal = Convert.ToDecimal(gslump6Cement / gslump6CementCnt);
                    if (gslump64p75mmPassingCnt > 0)
                        tempVal = tempVal + (Convert.ToDecimal(gslump6SandWt / gslump6SandCnt) * Convert.ToDecimal((gslump64p75mmPassing / gslump64p75mmPassingCnt)/100));
                    if (gslump6WaterCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(gslump6Water / gslump6WaterCnt);
                    if (gslump6FlyAshCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(gslump6FlyAsh / gslump6FlyAshCnt);
                    if (gslump6GGBSCnt > 0)
                        tempVal = tempVal + Convert.ToDecimal(gslump6GGBS / gslump6GGBSCnt);

                    tempVal = tempVal / (Convert.ToDecimal(gslump6PasticDensity) / gslump6PasticDensityCnt);
                    dr1["Cement4.75mmWaterDivPlasticDensity"] = tempVal.ToString("0.00");

                }
                dt.Rows.Add(dr1);
            }
            #endregion
            //

            grdMF.DataSource = dt;
            grdMF.DataBind();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdMF.Rows.Count > 0 && grdMF.Visible == true)
            {
                string Subheader = "";

                //Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text;
                Subheader += "|" + "" + "|" + "";

                PrintGrid.PrintGridView(grdMF, Subheader, "MixDesignReport");
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

    }
}