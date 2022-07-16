using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class NDT_ReportTestReqWise : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Report for Non-destructive Testing";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                LoadReferenceNoList();
                LoadApproveBy();
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 10;
            var reportList = dc.ReferenceNo_View_StatusWise("NDT", reportStatus, 0);
            ddlReferenceNo.DataTextField = "ReferenceNo";
            ddlReferenceNo.DataSource = reportList;
            ddlReferenceNo.DataBind();
            ddlReferenceNo.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadApproveBy()
        {
            ddlApproveBy.DataTextField = "USER_Name_var";
            ddlApproveBy.DataValueField = "USER_Id";
            var user = dc.User_View_Rightwise("RptApproval");
            ddlApproveBy.DataSource = user;
            ddlApproveBy.DataBind();
            ddlApproveBy.Items.Insert(0, "---Select---");
        }
        protected void ddlReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            if (ddlReferenceNo.SelectedIndex > 0)
            {
                bool IndStrUpdFlag = true;
                var wbs = dc.NDTWBS_View_All(ddlReferenceNo.SelectedItem.Text, 0, "", "", "", "");
                foreach (var w in wbs)
                {
                    if (w.NDTWBS_Status_bit == false)
                    {
                        IndStrUpdFlag = false;
                        break;
                    }
                }
                if (IndStrUpdFlag == true)
                {
                    DisplayReportDetails();
                }
                else
                {
                    lnkSave.Enabled = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please update indicative strength.');", true);
                }
            }
        }
        protected void ClearAllControls()
        {
            //ddlReferenceNo.SelectedIndex = 0;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            lblReportId.Text = "";
            lnkFetchAsPerCurrentData.Visible = false;
            txtClientName.Text = "";
            txtSiteName.Text = "";
            txtAddress.Text = "";
            ddlProjectDesc.SelectedIndex = 0;
            ddlRccStruct.SelectedIndex = 0;
            txtAge.Text = "";
            ddlAgeIn.SelectedIndex = 0;
            chkPurposeOfEvaluation1.Checked = false;
            chkPurposeOfEvaluation2.Checked = false;
            chkPurposeOfEvaluation3.Checked = false;
            chkPurposeOfEvaluation4.Checked = false;
            txtPurposeOfEvaluationOther.Text = "";
            grdNDTCoverage.DataSource = null;
            grdNDTCoverage.DataBind();
            chkReasonForTest1.Checked = false;
            chkReasonForTest2.Checked = false;
            chkReasonForTest3.Checked = false;
            chkReasonForTest4.Checked = false;
            chkReasonForTest5.Checked = false;
            chkReasonForTest6.Checked = false;
            chkReasonForTest7.Checked = false;
            chkReasonForTest8.Checked = false;
            txtReasonForTestOther.Text = "";
            grdTest.DataSource = null;
            grdTest.DataBind();
            txtSampleSize1.Text = "";
            grdNDTSample.DataSource = null;
            grdNDTSample.DataBind();
            txtMethodOfSelection.Text = "";
            grdSummaryResultUPV.DataSource = null;
            grdSummaryResultUPV.DataBind();
            grdSummaryResultRH.DataSource = null;
            grdSummaryResultRH.DataBind();
            grdNetStdErrs.DataSource = null;
            grdNetStdErrs.DataBind();
            grdSummaryCompStr.DataSource = null;
            grdSummaryCompStr.DataBind();
            grdMembersGrade5.DataSource = null;
            grdMembersGrade5.DataBind();
            grdMembersGrade10.DataSource = null;
            grdMembersGrade10.DataBind();
            grdReadBelow3p5ForM20.DataSource = null;
            grdReadBelow3p5ForM20.DataBind();
            grdReadBelow3p75ForM25.DataSource = null;
            grdReadBelow3p75ForM25.DataBind();
            optConclusion1.Checked = false;
            optConclusion2.Checked = false;
            optConclusion3.Checked = false;
            optConclusion4.Checked = false;
            txtConclusion1_1.Text = "Although In -situ concrete grade is lower than the design grade in our opinion  it may be accepted considering the in-situ factors of safety  considered in design . However final decision for the same rests with the  structural engineer after considering the loads on members. ";
            txtConclusion1_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion1_3.Text = "";
            txtConclusion2_1.Text = "Insitu strengths are lower than the design grade of concrete . Since the concrete is exhibiting large variation in strengths . Additional testing is  recommended  to reduce the uncertainty of prediction of strength";
            txtConclusion2_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion2_3.Text = "";
            txtConclusion3_1.Text = "Since in-situ compressive strengths are significantly lower than design grade of concrete , the structure may require retrofitting to make it fit for intended use .";
            txtConclusion3_2.Text = "Additional Testing is recommended to refine the estimated strengths for purpose of   retrofit design .";
            txtConclusion3_3.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion3_4.Text = "";
            txtConclusion4_1.Text = "The in-situ compressive strengths are satisfactory considering the design grade of concrete. ";
            txtConclusion4_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion4_3.Text = "";
            grdConcRecom.DataSource = null;
            grdConcRecom.DataBind();                     
            ViewState["Test_Table"] = null;
            ddlApproveBy.SelectedIndex = 0;            
        }
        protected void DisplayReportDetails()
        {
            lnkSave.Enabled = true;
            lnkPrint.Visible = false;

            var ndtreport = dc.NDTReport_View(ddlReferenceNo.SelectedItem.Value).ToList();
            foreach (var ndtrpt in ndtreport)
            {
                string[] strval;
                string[] strval1;
                lnkFetchAsPerCurrentData.Visible = true;
                lblReportId.Text = ndtrpt.NDTRPT_Id.ToString();
                txtClientName.Text = ndtrpt.CL_Name_var;
                txtSiteName.Text = ndtrpt.SITE_Name_var;
                txtAddress.Text = ndtrpt.SITE_Address_var;

                ddlProjectDesc.Text = ndtrpt.NDTRPT_ProjectDesc_var;
                ddlRccStruct.Text = ndtrpt.NDTRPT_RCCStructure_var;
                txtAge.Text = ndtrpt.NDTRPT_AgeOfConcr_int.ToString();
                ddlAgeIn.Text = ndtrpt.NDTRPT_AgeOfConcrIn_var;

                string[] strVal = ndtrpt.NDTRPT_PurposeOfEvaluation_var.Split('~');
                if (strVal[0].Contains(chkPurposeOfEvaluation1.Text) == true)
                    chkPurposeOfEvaluation1.Checked = true;
                if (strVal[1].Contains(chkPurposeOfEvaluation2.Text) == true)
                    chkPurposeOfEvaluation2.Checked = true;
                if (strVal[2].Contains(chkPurposeOfEvaluation3.Text) == true)
                    chkPurposeOfEvaluation3.Checked = true;
                if (strVal[3].Contains(chkPurposeOfEvaluation4.Text) == true)
                {
                    chkPurposeOfEvaluation4.Checked = true;
                    txtPurposeOfEvaluationOther.Text = strVal[4];
                }

                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("NoOfMembers", typeof(string)));

                strval = ndtrpt.NDTRPT_NDTCoverage_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["GradeOfConcrete"] = strval1[2];
                    dr1["MemberType"] = strval1[3];
                    dr1["NoOfMembers"] = strval1[4];
                    dt1.Rows.Add(dr1);
                }
                grdNDTCoverage.DataSource = dt1;
                grdNDTCoverage.DataBind();

                strVal = ndtrpt.NDTRPT_ReasonForTesting_var.Split('~');
                if (strVal[0].Contains(chkReasonForTest1.Text) == true)
                    chkReasonForTest1.Checked = true;
                if (strVal[1].Contains(chkReasonForTest2.Text) == true)
                    chkReasonForTest2.Checked = true;
                if (strVal[2].Contains(chkReasonForTest3.Text) == true)
                    chkReasonForTest3.Checked = true;
                if (strVal[3].Contains(chkReasonForTest4.Text) == true)
                    chkReasonForTest4.Checked = true;
                if (strVal[4].Contains(chkReasonForTest5.Text) == true)
                    chkReasonForTest5.Checked = true;
                if (strVal[5].Contains(chkReasonForTest6.Text) == true)
                    chkReasonForTest6.Checked = true;
                if (strVal[6].Contains(chkReasonForTest7.Text) == true)
                    chkReasonForTest7.Checked = true;
                if (strVal[7].Contains(chkReasonForTest8.Text) == true)
                {
                    chkReasonForTest8.Checked = true;
                    txtReasonForTestOther.Text = strVal[8];
                }

                strval = ndtrpt.NDTRPT_TypeOfNDTUsed_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    AddRowTest();
                    TextBox txtTypeOfTest = (TextBox)grdTest.Rows[i].FindControl("txtTypeOfTest");
                    TextBox txtTestingMethod = (TextBox)grdTest.Rows[i].FindControl("txtTestingMethod");
                    //TextBox txtISReference = (TextBox)grdTest.Rows[i].FindControl("txtISReference");

                    strval1 = strval[i].Split('~');

                    grdTest.Rows[i].Cells[2].Text = (i + 1).ToString();
                    txtTypeOfTest.Text = strval1[0];
                    txtTestingMethod.Text = strval1[1];
                    //txtISReference.Text = strval1[2];
                }

                txtSampleSize1.Text = ndtrpt.NDTRPT_SampleSize_var;

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("NoOfMembersTested", typeof(string)));
                dt1.Columns.Add(new DataColumn("PercOfSample", typeof(string)));
                strval = ndtrpt.NDTRPT_NDTSample_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["GradeOfConcrete"] = strval1[2];
                    dr1["MemberType"] = strval1[3];
                    dr1["NoOfMembersTested"] = strval1[4];
                    dr1["PercOfSample"] = strval1[5];
                    dt1.Rows.Add(dr1);
                }
                grdNDTSample.DataSource = dt1;
                grdNDTSample.DataBind();

                txtMethodOfSelection.Text = ndtrpt.NDTRPT_MethodOfSelOfMembers_var;

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("AverageUPV", typeof(string)));
                dt1.Columns.Add(new DataColumn("StandardDeviation", typeof(string)));
                dt1.Columns.Add(new DataColumn("Remark", typeof(string)));
                strval = ndtrpt.NDTRPT_SummaryResultUPV_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["GradeOfConcrete"] = strval1[2];
                    dr1["MemberType"] = strval1[3];
                    dr1["AverageUPV"] = strval1[4];
                    dr1["StandardDeviation"] = strval1[5];
                    dr1["Remark"] = strval1[6];
                    dt1.Rows.Add(dr1);
                }
                grdSummaryResultUPV.DataSource = dt1;
                grdSummaryResultUPV.DataBind();

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("AverageRH", typeof(string)));
                dt1.Columns.Add(new DataColumn("StandardDeviation", typeof(string)));
                strval = ndtrpt.NDTRPT_SummaryResultRH_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["GradeOfConcrete"] = strval1[2];
                    dr1["MemberType"] = strval1[3];
                    dr1["AverageRH"] = strval1[4];
                    dr1["StandardDeviation"] = strval1[5];
                    dt1.Rows.Add(dr1);
                }
                grdSummaryResultRH.DataSource = dt1;
                grdSummaryResultRH.DataBind();

                if (ndtrpt.NDTRPT_ReadBelow3p5ForM20_var != null && ndtrpt.NDTRPT_ReadBelow3p5ForM20_var != "")
                {
                    dt1 = new DataTable();
                    dr1 = null;
                    dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                    dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                    dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Description", typeof(string)));
                    strval = ndtrpt.NDTRPT_ReadBelow3p5ForM20_var.Split('^');
                    for (int i = 0; i < strval.Count() - 1; i++)
                    {
                        dr1 = dt1.NewRow();
                        strval1 = strval[i].Split('~');
                        dr1["SrNo"] = (i + 1).ToString();
                        dr1["Building"] = strval1[0];
                        dr1["Floor"] = strval1[1];
                        dr1["MemberType"] = strval1[2];
                        dr1["MemberId"] = strval1[3];
                        dr1["Description"] = strval1[4];
                        dt1.Rows.Add(dr1);
                    }
                    grdReadBelow3p5ForM20.DataSource = dt1;
                    grdReadBelow3p5ForM20.DataBind();
                }

                if (ndtrpt.NDTRPT_ReadBelow3p75ForM25_var != null && ndtrpt.NDTRPT_ReadBelow3p75ForM25_var != "")
                {
                    dt1 = new DataTable();
                    dr1 = null;
                    dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                    dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                    dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Description", typeof(string)));
                    strval = ndtrpt.NDTRPT_ReadBelow3p75ForM25_var.Split('^');
                    for (int i = 0; i < strval.Count() - 1; i++)
                    {
                        dr1 = dt1.NewRow();
                        strval1 = strval[i].Split('~');
                        dr1["SrNo"] = (i + 1).ToString();
                        dr1["Building"] = strval1[0];
                        dr1["Floor"] = strval1[1];
                        dr1["MemberType"] = strval1[2];
                        dr1["MemberId"] = strval1[3];
                        dr1["Description"] = strval1[4];
                        dt1.Rows.Add(dr1);
                    }
                    grdReadBelow3p75ForM25.DataSource = dt1;
                    grdReadBelow3p75ForM25.DataBind();
                }

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("StandardError", typeof(string)));
                strval = ndtrpt.NDTRPT_NetStdErrs_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["GradeOfConcrete"] = strval1[2];
                    dr1["MemberType"] = strval1[3];
                    dr1["StandardError"] = strval1[4];
                    dt1.Rows.Add(dr1);
                }
                grdNetStdErrs.DataSource = dt1;
                grdNetStdErrs.DataBind();

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("95ConfInSituCompStr", typeof(string)));
                dt1.Columns.Add(new DataColumn("RecomGradeOfConcrete", typeof(string)));
                strval = ndtrpt.NDTRPT_SummaryCompStr_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["GradeOfConcrete"] = strval1[2];
                    dr1["MemberType"] = strval1[3];
                    dr1["95ConfInSituCompStr"] = strval1[4];
                    dr1["RecomGradeOfConcrete"] = strval1[5];
                    dt1.Rows.Add(dr1);
                }
                grdSummaryCompStr.DataSource = dt1;
                grdSummaryCompStr.DataBind();

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("NoOfMembers", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));
                strval = ndtrpt.NDTRPT_MembersGrade5_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["MemberType"] = strval1[2];
                    dr1["NoOfMembers"] = strval1[3];
                    dr1["MemberId"] = strval1[4];
                    dt1.Rows.Add(dr1);
                }
                grdMembersGrade5.DataSource = dt1;
                grdMembersGrade5.DataBind();

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("NoOfMembers", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));
                strval = ndtrpt.NDTRPT_MembersGrade10_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["MemberType"] = strval1[2];
                    dr1["NoOfMembers"] = strval1[3];
                    dr1["MemberId"] = strval1[4];
                    dt1.Rows.Add(dr1);
                }
                grdMembersGrade10.DataSource = dt1;
                grdMembersGrade10.DataBind();

                dt1 = new DataTable();
                dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("Option", typeof(string)));
                dt1.Columns.Add(new DataColumn("ConclusionRecommendation", typeof(string)));
                strval = ndtrpt.NDTRPT_ConclusionRecmd_var.Split('^');
                for (int i = 0; i < strval.Count() - 1; i++)
                {
                    dr1 = dt1.NewRow();
                    strval1 = strval[i].Split('~');
                    dr1["SrNo"] = (i + 1).ToString();
                    dr1["Building"] = strval1[0];
                    dr1["Floor"] = strval1[1];
                    dr1["GradeOfConcrete"] = strval1[2];
                    dr1["MemberType"] = strval1[3];
                    dr1["Option"] = strval1[4];
                    dr1["ConclusionRecommendation"] = strval1[5];
                    dt1.Rows.Add(dr1);
                }
                grdConcRecom.DataSource = dt1;
                grdConcRecom.DataBind();

            }
            if (ndtreport.Count() == 0)
            {
                for (int j = 0; j < 2; j++)
                {
                    AddRowTest();
                    TextBox txtTypeOfTest = (TextBox)grdTest.Rows[j].FindControl("txtTypeOfTest");
                    TextBox txtTestingMethod = (TextBox)grdTest.Rows[j].FindControl("txtTestingMethod");
                    //TextBox txtISReference = (TextBox)grdTest.Rows[j].FindControl("txtISReference");

                    grdTest.Rows[j].Cells[2].Text = (j + 1).ToString();
                    if (j == 0)
                    {
                        txtTypeOfTest.Text = "Ultrasonic Pulse Velocity Testing";
                        txtTestingMethod.Text = "IS 516 (part 5 section-1) : 2018 ";
                    }
                    else if (j == 1)
                    {
                        txtTypeOfTest.Text = "Shmitz Hammer";
                        txtTestingMethod.Text = "IS 516 (part 5 section-4) : 2020 ";
                    }
                    //else if (j == 2)
                    //    txtTypeOfTest.Text = "Core Testing";
                }
                txtSampleSize1.Text = "a) UPV and Shmitz Hammer – 15 % of  members ";
                //txtSampleSize2.Text = "b) Cores –  Min 6 for 30m3 of concrete and 3 additional cores for every 30m3 of concrete ";
                txtMethodOfSelection.Text = "Random selection";

                FetchAsPerCurrentData();
            }
        }
        protected void FetchAsPerCurrentData()
        {
            var inward = dc.Inward_View(Convert.ToInt32(ddlReferenceNo.SelectedItem.Value.Split('/')[0].ToString()), 0, "NDT", null, null);
            foreach (var inwd in inward)
            {
                txtClientName.Text = inwd.ClientName;
                txtSiteName.Text = inwd.SiteName;
                txtAddress.Text = inwd.SITE_Address_var;
            }

            var wbs = dc.NDTWBS_View(ddlReferenceNo.SelectedValue, "", "", "", "", "", "").ToList();

            #region NDT Coverage
            int i = 0;
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("NoOfMembers", typeof(string)));
            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["SrNo"] = (i + 1).ToString();
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["GradeOfConcrete"] = w.Grade_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["NoOfMembers"] = string.Empty;
                dt1.Rows.Add(dr1);
                i++;
            }
            grdNDTCoverage.DataSource = dt1;
            grdNDTCoverage.DataBind();
            #endregion

            #region NDT Sample
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("NoOfMembersTested", typeof(string)));
            dt1.Columns.Add(new DataColumn("PercOfSample", typeof(string)));

            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["SrNo"] = (i + 1).ToString();
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["GradeOfConcrete"] = w.Grade_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["NoOfMembersTested"] = w.NoOfMembersTested;
                dr1["PercOfSample"] = "";
                dt1.Rows.Add(dr1);
                i++;
            }
            grdNDTSample.DataSource = dt1;
            grdNDTSample.DataBind();
            #endregion

            #region Summary of result of upv
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("AverageUPV", typeof(string)));
            dt1.Columns.Add(new DataColumn("StandardDeviation", typeof(string)));
            dt1.Columns.Add(new DataColumn("Remark", typeof(string)));
            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["SrNo"] = (i + 1).ToString();
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["GradeOfConcrete"] = w.Grade_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["AverageUPV"] = "";
                dr1["StandardDeviation"] = string.Empty;
                dr1["Remark"] = string.Empty;
                dt1.Rows.Add(dr1);
                i++;
            }
            grdSummaryResultUPV.DataSource = dt1;
            grdSummaryResultUPV.DataBind();
            #endregion

            #region Summary of result of RH
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("AverageRH", typeof(string)));
            dt1.Columns.Add(new DataColumn("StandardDeviation", typeof(string)));
            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["SrNo"] = (i + 1).ToString();
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["GradeOfConcrete"] = w.Grade_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["AverageRH"] = "";
                dr1["StandardDeviation"] = string.Empty;
                dt1.Rows.Add(dr1);
                i++;
            }
            grdSummaryResultRH.DataSource = dt1;
            grdSummaryResultRH.DataBind();
            #endregion

            #region Net standard errors
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("StandardError", typeof(string)));
            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["SrNo"] = (i + 1).ToString();
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["GradeOfConcrete"] = w.Grade_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["StandardError"] = string.Empty;
                dt1.Rows.Add(dr1);
                i++;
            }
            grdNetStdErrs.DataSource = dt1;
            grdNetStdErrs.DataBind();
            #endregion

            #region Summary of compressive strength
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("95ConfInSituCompStr", typeof(string)));
            dt1.Columns.Add(new DataColumn("RecomGradeOfConcrete", typeof(string)));
            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["SrNo"] = (i + 1).ToString();
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["GradeOfConcrete"] = w.Grade_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["95ConfInSituCompStr"] = string.Empty;
                dr1["RecomGradeOfConcrete"] = string.Empty;
                dt1.Rows.Add(dr1);                
                i++;
            }
            grdSummaryCompStr.DataSource = dt1;
            grdSummaryCompStr.DataBind();
            
            #endregion

            #region Members having readings below 3.5 for M20
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            var ndtdetail = dc.NDTWBS_View(ddlReferenceNo.SelectedValue, "", "", "M 20", "", "", "");
            foreach (var nd in ndtdetail)
            {
                if (nd.PulseVelocity_var.Split('|')[0].ToString() != "NO")
                {
                    if (Convert.ToDecimal(nd.PulseVelocity_var.Split('|')[0]) < Convert.ToDecimal("3.5"))
                    {
                        dr1 = dt1.NewRow();
                        dr1["SrNo"] = (i + 1).ToString();
                        dr1["Building"] = nd.NDTWBS_Building_var;
                        dr1["Floor"] = nd.NDTWBS_Floor_var;
                        dr1["MemberType"] = nd.NDTWBS_MemberType_var;
                        dr1["MemberId"] = nd.NDTWBS_MemberId_var;
                        dr1["Description"] = nd.Description_var;
                        dt1.Rows.Add(dr1);
                        i++;
                    }
                }
            }
            grdReadBelow3p5ForM20.DataSource = dt1;
            grdReadBelow3p5ForM20.DataBind();
            #endregion

            #region Members having readings below 3.75 for M25
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            ndtdetail = dc.NDTWBS_View(ddlReferenceNo.SelectedValue, "", "", "M 25", "", "", "");
            foreach (var nd in ndtdetail)
            {
                if (nd.PulseVelocity_var.Split('|')[0].ToString() != "NO")
                {
                    if (Convert.ToDecimal(nd.PulseVelocity_var.Split('|')[0]) < Convert.ToDecimal("3.75"))
                    {
                        dr1 = dt1.NewRow();
                        dr1["SrNo"] = (i + 1).ToString();
                        dr1["Building"] = nd.NDTWBS_Building_var;
                        dr1["Floor"] = nd.NDTWBS_Floor_var;
                        dr1["MemberType"] = nd.NDTWBS_MemberType_var;
                        dr1["MemberId"] = nd.NDTWBS_MemberId_var;
                        dr1["Description"] = nd.Description_var;
                        dt1.Rows.Add(dr1);
                        i++;
                    }
                }
            }
            grdReadBelow3p75ForM25.DataSource = dt1;
            grdReadBelow3p75ForM25.DataBind();
            #endregion

            #region Conclusion / Recommendation
            i = 0;
            dt1 = new DataTable();
            dr1 = null;
            dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("GradeOfConcrete", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("Option", typeof(string)));
            dt1.Columns.Add(new DataColumn("ConclusionRecommendation", typeof(string)));
            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["SrNo"] = (i + 1).ToString();
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["GradeOfConcrete"] = w.Grade_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["Option"] = string.Empty;
                dr1["ConclusionRecommendation"] = string.Empty;
                dt1.Rows.Add(dr1);
                i++;
            }
            grdConcRecom.DataSource = dt1;
            grdConcRecom.DataBind();

            optConclusion1.Checked = false;
            optConclusion2.Checked = false;
            optConclusion3.Checked = false;
            optConclusion4.Checked = false;
            txtConclusion1_1.Text = "Although In -situ concrete grade is lower than the design grade in our opinion  it may be accepted considering the in-situ factors of safety  considered in design . However final decision for the same rests with the  structural engineer after considering the loads on members. ";
            txtConclusion1_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion1_3.Text = "";
            txtConclusion2_1.Text = "Insitu strengths are lower than the design grade of concrete . Since the concrete is exhibiting large variation in strengths . Additional testing is  recommended  to reduce the uncertainty of prediction of strength";
            txtConclusion2_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion2_3.Text = "";
            txtConclusion3_1.Text = "Since in-situ compressive strengths are significantly lower than design grade of concrete , the structure may require retrofitting to make it fit for intended use .";
            txtConclusion3_2.Text = "Additional Testing is recommended to refine the estimated strengths for purpose of   retrofit design .";
            txtConclusion3_3.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion3_4.Text = "";
            txtConclusion4_1.Text = "The in-situ compressive strengths are satisfactory considering the design grade of concrete. ";
            txtConclusion4_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion4_3.Text = "";
            #endregion
        }
        private void CalculateSummary()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            bool valid = true;
            for (int i = 0; i < grdNDTCoverage.Rows.Count; i++)
            {
                TextBox txtNoOfMembers = (TextBox)grdNDTCoverage.Rows[i].FindControl("txtNoOfMembers");
                TextBox txtNoOfMembersTested = (TextBox)grdNDTSample.Rows[i].FindControl("txtNoOfMembersTested");
                if (txtNoOfMembers.Text == "")
                {
                    //lblMsg.Text = "Enter No. of Members for Sr No. " + (i + 1) + ".";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter No. of Members for Sr No. " + (i + 1) + "..');", true);
                    txtNoOfMembers.Focus();
                    valid = false;
                    break;
                }
                else if (txtNoOfMembersTested.Text == "")
                {
                    //lblMsg.Text = "Enter No. of Members for Sr No. " + (i + 1) + ".";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter No. of Members tested for Sr No. " + (i + 1) + "..');", true);
                    txtNoOfMembersTested.Focus();
                    valid = false;
                    break;
                }
                //else if (Convert.ToDecimal(txtNoOfMembers.Text) < Convert.ToDecimal(grdNDTSample.Rows[i].Cells[5].Text))
                else if (Convert.ToDecimal(txtNoOfMembers.Text) < Convert.ToDecimal(txtNoOfMembersTested.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('No. of Members should be greater than or equal to No. of members tested for Sr No. " + (i + 1) + "..');", true);
                    txtNoOfMembers.Focus();
                    valid = false;
                    break;
                }
            }
            if (valid == true)
            {
                decimal[] AVG = new decimal[500];
                decimal[] AVGCrList = new decimal[500];
                decimal[] AVGCrEqList = new decimal[500];
                decimal[] Grade = new decimal[500];
                decimal[] SDstrength = new decimal[500];
                decimal[] SDstrengthCr = new decimal[500];
                decimal[] R = new decimal[500];
                decimal[] K = new decimal[500];
                decimal[] AVGInsitu = new decimal[500];
                decimal[] coreFlag = new decimal[500];

                //NDT Sample
                for (int i = 0; i < grdNDTSample.Rows.Count; i++)
                {
                    TextBox txtNoOfMembers = (TextBox)grdNDTCoverage.Rows[i].FindControl("txtNoOfMembers");
                    TextBox txtNoOfMembersTested = (TextBox)grdNDTSample.Rows[i].FindControl("txtNoOfMembersTested");
                    //grdNDTSample.Rows[i].Cells[6].Text = ((Convert.ToInt32(grdNDTSample.Rows[i].Cells[5].Text) * 100) / Convert.ToInt32(txtNoOfMembers.Text)).ToString("0");
                    grdNDTSample.Rows[i].Cells[6].Text = ((Convert.ToInt32(txtNoOfMembersTested.Text) * 100) / Convert.ToInt32(txtNoOfMembers.Text)).ToString("0");
                }
                int prevWbsId = 0;
                //Summary of result of UPV
                for (int i = 0; i < grdSummaryResultUPV.Rows.Count; i++)
                {
                    int count = 0, count2 = 0, countCr = 0, countCrEq = 0;
                    decimal sumUpv = 0, sumRh = 0, sumPredictedStrength = 0, sumPredictedStrengthCr = 0, sumEquivalantStrengthCr = 0, AVG1 = 0, AVG2 = 0, AVG3 = 0, AVGCr = 0, AVGCrEq = 0, A = 0, A2 = 0, A3 = 0, ACr = 0, ACrEq = 0, C = 0, n = 0, nCr = 0, r = 0, SDstr, SDUpv, SDRh, SDstrCr;
                    decimal[] X = new decimal[500];
                    decimal[] X2 = new decimal[500];
                    decimal[] XRh = new decimal[500];
                    decimal[] XCr = new decimal[500];
                    int[] XCrCount = new int[500];
                    decimal[] XCrEq = new decimal[500];
                    int[] XCrEqCount = new int[500];

                    var ndtdetail = dc.NDTWBS_View(ddlReferenceNo.SelectedValue, grdSummaryResultUPV.Rows[i].Cells[1].Text, grdSummaryResultUPV.Rows[i].Cells[2].Text, grdSummaryResultUPV.Rows[i].Cells[3].Text, grdSummaryResultUPV.Rows[i].Cells[4].Text, "", "").ToList();
                    foreach (var nd in ndtdetail)
                    {
                        //if (nd.PulseVelocity_var.Split('|')[0].ToString() != "NO")
                        //{
                        //    sumUpv += Convert.ToDecimal(nd.PulseVelocity_var.Split('|')[0].ToString());
                        //    X2[count] = Convert.ToDecimal(Convert.ToDecimal(nd.PulseVelocity_var.Split('|')[0].ToString()));
                        //    count2++;
                        //}
                        //sumRh += Convert.ToDecimal(nd.ReboundIndex_var.Split('|')[0].ToString());
                        //XRh[count] = Convert.ToDecimal(Convert.ToDecimal(nd.ReboundIndex_var.Split('|')[0].ToString()));

                        //sumPredictedStrength += Convert.ToDecimal(nd.IndicativeStrength_var);
                        //X[count] = Convert.ToDecimal(nd.IndicativeStrength_var);
                        //count++;

                        decimal sumUpvTemp = 0, sumRhTemp = 0, sumPredictedStrengthTemp = 0;
                        int upvCntTemp = 0, cntTemp = 0;
                        var ndtdetail1 = dc.NDTWBS_View(ddlReferenceNo.SelectedValue, grdSummaryResultUPV.Rows[i].Cells[1].Text, grdSummaryResultUPV.Rows[i].Cells[2].Text, grdSummaryResultUPV.Rows[i].Cells[3].Text, grdSummaryResultUPV.Rows[i].Cells[4].Text, "", nd.Description_var).ToList();
                        foreach (var nd1 in ndtdetail1)
                        {
                            if (nd1.PulseVelocity_var.Split('|')[0].ToString() != "NO")
                            {
                                sumUpvTemp += Convert.ToDecimal(nd1.PulseVelocity_var.Split('|')[0].ToString());
                                upvCntTemp++;
                            }
                            sumRhTemp += Convert.ToDecimal(nd1.ReboundIndex_var.Split('|')[0].ToString());
                            sumPredictedStrengthTemp += Convert.ToDecimal(nd1.IndicativeStrength_var);
                            cntTemp++;
                        }

                        if (upvCntTemp > 0)
                        {
                            sumUpv += Convert.ToDecimal(sumUpvTemp / upvCntTemp);
                            X2[count] = Convert.ToDecimal(sumUpvTemp / upvCntTemp);
                            count2++;
                        }
                        sumRh += Convert.ToDecimal(sumRhTemp / cntTemp);
                        XRh[count] = Convert.ToDecimal(sumRhTemp / cntTemp);

                        sumPredictedStrength += Convert.ToDecimal(sumPredictedStrengthTemp / cntTemp);
                        X[count] = Convert.ToDecimal(sumPredictedStrengthTemp / cntTemp);
                        count++;

                        if ((nd.NDTWBS_EqCubeStrOfCore1_dec != null && nd.NDTWBS_EqCubeStrOfCore1_dec > 0)
                                || (nd.NDTWBS_EqCubeStrOfCore2_dec != null && nd.NDTWBS_EqCubeStrOfCore2_dec > 0)
                                || (nd.NDTWBS_EqCubeStrOfCore3_dec != null && nd.NDTWBS_EqCubeStrOfCore3_dec > 0))
                        {
                            coreFlag[i] = 1;

                            if (prevWbsId != nd.NDTWBS_Id)
                            {
                                if (prevWbsId != 0)
                                {
                                    countCr++;
                                    countCrEq++;
                                }
                            }

                            XCr[countCr] += Convert.ToDecimal(nd.IndicativeStrength_var);
                            XCrCount[countCr]++;

                            if (prevWbsId != nd.NDTWBS_Id)
                            {
                                if (nd.NDTWBS_EqCubeStrOfCore1_dec != null && nd.NDTWBS_EqCubeStrOfCore1_dec > 0)
                                {
                                    XCrEq[countCrEq] += Convert.ToDecimal(nd.NDTWBS_EqCubeStrOfCore1_dec);
                                    XCrEqCount[countCrEq]++;
                                }
                                if (nd.NDTWBS_EqCubeStrOfCore2_dec != null && nd.NDTWBS_EqCubeStrOfCore2_dec > 0)
                                {
                                    XCrEq[countCrEq] += Convert.ToDecimal(nd.NDTWBS_EqCubeStrOfCore2_dec);
                                    XCrEqCount[countCrEq]++;
                                }
                                if (nd.NDTWBS_EqCubeStrOfCore3_dec != null && nd.NDTWBS_EqCubeStrOfCore3_dec > 0)
                                {
                                    XCrEq[countCrEq] += Convert.ToDecimal(nd.NDTWBS_EqCubeStrOfCore3_dec);
                                    XCrEqCount[countCrEq]++;
                                }
                            }
                            prevWbsId = nd.NDTWBS_Id;
                        }

                    }

                    AVG2 = Math.Round((sumUpv / count2), 2);
                    grdSummaryResultUPV.Rows[i].Cells[5].Text = AVG2.ToString("0.00");

                    AVG3 = Math.Round((sumRh / count), 2);
                    grdSummaryResultRH.Rows[i].Cells[5].Text = AVG3.ToString("0.00");

                    AVG1 = Math.Round((sumPredictedStrength / count), 2);
                    AVG[i] = AVG1;

                    for (int temp = 0; temp < count; temp++)
                    {
                        decimal xMinusAvg1 = X[temp] - AVG1;
                        A += (xMinusAvg1 * xMinusAvg1);

                        decimal x2MinusAvg2 = X2[temp] - AVG2;
                        A2 += (x2MinusAvg2 * x2MinusAvg2);

                        decimal x3MinusAvg3 = XRh[temp] - AVG3;
                        A3 += (x3MinusAvg3 * x3MinusAvg3);
                    }
                    //n = Convert.ToDecimal(grdNDTSample.Rows[i].Cells[5].Text);
                    TextBox txtNoOfMembersTested = (TextBox)grdNDTSample.Rows[i].FindControl("txtNoOfMembersTested");
                    n = Convert.ToDecimal(txtNoOfMembersTested.Text);
                    if (n < 3)
                        SDstr = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(A / n)));
                    else
                        SDstr = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(A / (n - 1))));
                    SDstrength[i] = SDstr;

                    if (coreFlag[i] == 1)
                    {
                        countCr++; countCrEq++;
                        for (int temp = 0; temp < countCr; temp++)
                        {
                            XCr[temp] = XCr[temp] / XCrCount[temp];
                            XCrEq[temp] = XCrEq[temp] / XCrEqCount[temp];

                            sumPredictedStrengthCr += XCr[temp];
                            sumEquivalantStrengthCr += XCrEq[temp];
                        }

                        AVGCr = Math.Round((sumPredictedStrengthCr / (countCr)), 2);
                        AVGCrList[i] = AVGCr;

                        AVGCrEq = Math.Round((sumEquivalantStrengthCr / (countCrEq)), 2);
                        AVGCrList[i] = AVGCrEq;

                        K[i] = Math.Round(AVGCrEq / AVGCr, 2);
                        AVGInsitu[i] = Math.Round(AVG1 * K[i], 2);

                        decimal xCrMinusAvgCr = 0, xCrEqMinusAvgCrEq = 0;
                        for (int temp = 0; temp < countCr; temp++)
                        {
                            xCrMinusAvgCr = XCr[temp] - AVGCr;
                            ACr += (xCrMinusAvgCr * xCrMinusAvgCr);

                            xCrEqMinusAvgCrEq = XCrEq[temp] - AVGCrEq;
                            ACrEq += (xCrEqMinusAvgCrEq * xCrEqMinusAvgCrEq);

                            C += (xCrMinusAvgCr * xCrEqMinusAvgCrEq);
                        }

                        nCr = countCr;
                        if (n < 3)
                            SDstrCr = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(ACr / nCr)));
                        else
                            SDstrCr = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(ACr / (nCr - 1))));
                        SDstrengthCr[i] = SDstrCr;
                        r = Math.Round(C / Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(ACr * ACrEq))), 2);
                        R[i] = r;

                    }
                    if (n < 3)
                        SDUpv = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(A2 / n)));
                    else
                        SDUpv = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(A2 / (n - 1))));
                    grdSummaryResultUPV.Rows[i].Cells[6].Text = SDUpv.ToString("0.00");
                    if (n < 3)
                        SDRh = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(A3 / n)));
                    else
                        SDRh = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(A3 / (n - 1))));
                    grdSummaryResultRH.Rows[i].Cells[6].Text = SDRh.ToString("0.00");

                    if (AVG2 < 3)
                        grdSummaryResultUPV.Rows[i].Cells[7].Text = "Poor";
                    else if (AVG2 >= 3 && AVG2 <= Convert.ToDecimal(3.75))
                        grdSummaryResultUPV.Rows[i].Cells[7].Text = "Doubtful";
                    else if (AVG2 >= Convert.ToDecimal(3.75) && AVG2 <= Convert.ToDecimal(4.40))
                        grdSummaryResultUPV.Rows[i].Cells[7].Text = "Good";
                    else if (AVG2 > Convert.ToDecimal(4.40))
                        grdSummaryResultUPV.Rows[i].Cells[7].Text = "Excellent";

                }
                //Net standard errors
                for (int i = 0; i < grdNetStdErrs.Rows.Count; i++)
                {

                    decimal SE1 = 0, SE2 = 0, SE3 = 0;
                    SE1 = SDstrength[i] * Convert.ToDecimal(Math.Sqrt(1 - 0.91 * 0.91));
                    TextBox txtNoOfMembers = (TextBox)grdNDTCoverage.Rows[i].FindControl("txtNoOfMembers");
                    decimal N = 0, n = 0;
                    N = Convert.ToDecimal(txtNoOfMembers.Text);
                    //n = Convert.ToDecimal(grdNDTSample.Rows[i].Cells[5].Text);
                    TextBox txtNoOfMembersTested = (TextBox)grdNDTSample.Rows[i].FindControl("txtNoOfMembersTested");
                    n = Convert.ToDecimal(txtNoOfMembersTested.Text);
                    if (N > 2) //condition added cause (N-2)=0 then result 0. shital
                        SE2 = SDstrength[i] / n * Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((N - n) / (N - 2))));
                    if (coreFlag[i] == 1)
                        SE3 = SDstrength[i] * Convert.ToDecimal(Math.Sqrt(1 - Convert.ToDouble(R[i] * R[i])));
                    else
                        SE3 = SDstrength[i] * Convert.ToDecimal(Math.Sqrt(1 - 0.85 * 0.85));

                    decimal A = (SE1 * SE1) + (SE2 * SE2) + (SE3 * SE3);
                    decimal NSE = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(A)));

                    grdNetStdErrs.Rows[i].Cells[5].Text = NSE.ToString("0.00");

                }
                //Summary of Compressive Strengths
                decimal[] Z = { 2,2,2,2,2,2,Convert.ToDecimal("1.94"), Convert.ToDecimal("1.89"), Convert.ToDecimal("1.85"), Convert.ToDecimal("1.83"), Convert.ToDecimal("1.81"), Convert.ToDecimal("1.79"), Convert.ToDecimal("1.78")
                    , Convert.ToDecimal("1.77"), Convert.ToDecimal("1.76"), Convert.ToDecimal("1.75"), Convert.ToDecimal("1.74"), Convert.ToDecimal("1.73"), Convert.ToDecimal("1.73"), Convert.ToDecimal("1.73"), Convert.ToDecimal("1.72"), Convert.ToDecimal("1.72")
                    , Convert.ToDecimal("1.72"), Convert.ToDecimal("1.71"), Convert.ToDecimal("1.71"), Convert.ToDecimal("1.71"), Convert.ToDecimal("1.71"), Convert.ToDecimal("1.7"), Convert.ToDecimal("1.7"), Convert.ToDecimal("1.69"), Convert.ToDecimal("1.65")};
                for (int i = 0; i < grdSummaryCompStr.Rows.Count; i++)
                {
                    decimal temp95PercentConfi = 0;
                    //decimal n = Convert.ToDecimal(grdNDTSample.Rows[i].Cells[5].Text);
                    TextBox txtNoOfMembersTested = (TextBox)grdNDTSample.Rows[i].FindControl("txtNoOfMembersTested");
                    decimal n = Convert.ToDecimal(txtNoOfMembersTested.Text);
                    if (coreFlag[i] == 1)
                    {
                        if (n >= 30)
                            temp95PercentConfi = AVGInsitu[i] - (Z[30] * Convert.ToDecimal(grdNetStdErrs.Rows[i].Cells[5].Text));
                        else
                            temp95PercentConfi = AVGInsitu[i] - (Z[Convert.ToInt32(n)] * Convert.ToDecimal(grdNetStdErrs.Rows[i].Cells[5].Text));
                    }
                    else
                    {
                        if (n >= 30)
                            temp95PercentConfi = AVG[i] - (Z[30] * Convert.ToDecimal(grdNetStdErrs.Rows[i].Cells[5].Text));
                        else
                            temp95PercentConfi = AVG[i] - (Z[Convert.ToInt32(n)] * Convert.ToDecimal(grdNetStdErrs.Rows[i].Cells[5].Text));
                    }
                    if (grdSummaryCompStr.Rows[i].Cells[3].Text != "NA" && Math.Round((temp95PercentConfi / Convert.ToDecimal("0.85")), 1) > Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[3].Text.Replace("M ", "")))
                        Grade[i] = Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[3].Text.Replace("M ", ""));
                    else
                        Grade[i] = Math.Round((temp95PercentConfi / Convert.ToDecimal("0.85")), 1);

                    grdSummaryCompStr.Rows[i].Cells[5].Text = temp95PercentConfi.ToString("0.00");
                    //grdSummaryCompStr.Rows[i].Cells[6].Text = "M " + (Math.Round(Grade[i])).ToString();
                    //if (Grade[i].ToString().Contains("."))
                    //    grdSummaryCompStr.Rows[i].Cells[6].Text = "M " + Grade[i].ToString().Split('.')[0];
                    //else
                    //    grdSummaryCompStr.Rows[i].Cells[6].Text = "M " + Grade[i].ToString();
                    //if (Grade[i] < 0)
                    //    grdSummaryCompStr.Rows[i].Cells[6].Text = "Less Than M 5";
                    //else
                        grdSummaryCompStr.Rows[i].Cells[6].Text = "M " + (Math.Floor(Grade[i])).ToString();
                }

                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("Building", typeof(string)));
                dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt1.Columns.Add(new DataColumn("NoOfMembers", typeof(string)));
                dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("Building", typeof(string)));
                dt2.Columns.Add(new DataColumn("Floor", typeof(string)));
                dt2.Columns.Add(new DataColumn("MemberType", typeof(string)));
                dt2.Columns.Add(new DataColumn("NoOfMembers", typeof(string)));
                dt2.Columns.Add(new DataColumn("MemberId", typeof(string)));
                //Members - Grade 5 to 10 MPa , //Members - Grade > 10 MPa
                int srNo1 = 0, srNo2 = 0;
                for (int i = 0; i < grdSummaryCompStr.Rows.Count; i++)
                {
                    if (grdSummaryCompStr.Rows[i].Cells[3].Text != "NA")
                    {
                        if (Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[3].Text.Replace("M ", "").Replace("* ", "")) - Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[6].Text.Replace("M ", "").Replace("* ", "")) >= 5 &&
                            Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[3].Text.Replace("M ", "").Replace("* ", "")) - Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[6].Text.Replace("M ", "").Replace("* ", "")) <= 10)
                        {
                            var ndtdetail = dc.NDTWBS_View(ddlReferenceNo.SelectedValue, grdSummaryCompStr.Rows[i].Cells[1].Text, grdSummaryCompStr.Rows[i].Cells[2].Text, grdSummaryCompStr.Rows[i].Cells[3].Text, grdSummaryCompStr.Rows[i].Cells[4].Text, "shownoofmembers", "");
                            foreach (var nd in ndtdetail)
                            {
                                dr1 = dt1.NewRow();
                                dr1["SrNo"] = (srNo1 + 1).ToString();
                                dr1["Building"] = nd.NDTWBS_Building_var;
                                dr1["Floor"] = nd.NDTWBS_Floor_var;
                                dr1["MemberType"] = nd.NDTWBS_MemberType_var;
                                dr1["NoOfMembers"] = nd.NoOfMembersTested;
                                dr1["MemberId"] = nd.NDTWBS_MemberId_var;
                                dt1.Rows.Add(dr1);
                                srNo1++;
                            }
                        }
                        else if (Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[3].Text.Replace("M ", "")) - Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[6].Text.Replace("M ", "")) > 10)
                        {
                            var ndtdetail = dc.NDTWBS_View(ddlReferenceNo.SelectedValue, grdSummaryCompStr.Rows[i].Cells[1].Text, grdSummaryCompStr.Rows[i].Cells[2].Text, grdSummaryCompStr.Rows[i].Cells[3].Text, grdSummaryCompStr.Rows[i].Cells[4].Text, "shownoofmembers", "");
                            foreach (var nd in ndtdetail)
                            {
                                dr1 = dt2.NewRow();
                                dr1["SrNo"] = (srNo2 + 1).ToString();
                                dr1["Building"] = nd.NDTWBS_Building_var;
                                dr1["Floor"] = nd.NDTWBS_Floor_var;
                                dr1["MemberType"] = nd.NDTWBS_MemberType_var;
                                dr1["NoOfMembers"] = nd.NoOfMembersTested;
                                dr1["MemberId"] = nd.NDTWBS_MemberId_var;
                                srNo2++;
                                dt2.Rows.Add(dr1);
                            }
                        }
                    }
                }
                grdMembersGrade5.DataSource = dt1;
                grdMembersGrade5.DataBind();

                grdMembersGrade10.DataSource = dt2;
                grdMembersGrade10.DataBind();

                for (int i = 0; i < grdSummaryCompStr.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[5].Text) < 5)
                        grdSummaryCompStr.Rows[i].Cells[5].Text = "Less Than 5";

                    if (Convert.ToDecimal(grdSummaryCompStr.Rows[i].Cells[6].Text.Replace("M ", "")) < 5)
                        grdSummaryCompStr.Rows[i].Cells[6].Text = "Less Than M 5";
                }
            }
        }
        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            CalculateSummary();
        }

        #region grdTest Type of NDT used for evaluation
        protected void imgBtnAddRowTest_Click(object sender, CommandEventArgs e)
        {
            AddRowTest();
        }

        protected void imgBtnDeleteRowTest_Click(object sender, CommandEventArgs e)
        {
            if (grdTest.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdTest.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowTest(gvr.RowIndex);
            }
        }

        protected void DeleteRowTest(int rowIndex)
        {
            if (grdTest.Rows.Count > 1)
            {
                GetCurrentDataTest();
                DataTable dt = ViewState["Test_Table"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["Test_Table"] = dt;
                grdTest.DataSource = dt;
                grdTest.DataBind();
                SetPreviousDataTest();
            }
        }

        protected void AddRowTest()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["Test_Table"] != null)
            {
                GetCurrentDataTest();
                dt = (DataTable)ViewState["Test_Table"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTypeOfTest", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestingMethod", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtISReference", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtTypeOfTest"] = string.Empty;
            dr["txtTestingMethod"] = string.Empty;
            //dr["txtISReference"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["Test_Table"] = dt;
            grdTest.DataSource = dt;
            grdTest.DataBind();
            SetPreviousDataTest();
        }

        protected void GetCurrentDataTest()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTypeOfTest", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTestingMethod", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtISReference", typeof(string)));


            for (int i = 0; i < grdTest.Rows.Count; i++)
            {
                TextBox txtTypeOfTest = (TextBox)grdTest.Rows[i].FindControl("txtTypeOfTest");
                TextBox txtTestingMethod = (TextBox)grdTest.Rows[i].FindControl("txtTestingMethod");
                //TextBox txtISReference = (TextBox)grdTest.Rows[i].FindControl("txtISReference");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtTypeOfTest"] = txtTypeOfTest.Text;
                drRow["txtTestingMethod"] = txtTestingMethod.Text;
                //drRow["txtISReference"] = txtISReference.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["Test_Table"] = dtTable;

        }

        protected void SetPreviousDataTest()
        {
            DataTable dt = (DataTable)ViewState["Test_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtTypeOfTest = (TextBox)grdTest.Rows[i].FindControl("txtTypeOfTest");
                TextBox txtTestingMethod = (TextBox)grdTest.Rows[i].FindControl("txtTestingMethod");
                //TextBox txtISReference = (TextBox)grdTest.Rows[i].FindControl("txtISReference");

                grdTest.Rows[i].Cells[2].Text = (i + 1).ToString();
                txtTypeOfTest.Text = dt.Rows[i]["txtTypeOfTest"].ToString();
                txtTestingMethod.Text = dt.Rows[i]["txtTestingMethod"].ToString();
                //txtISReference.Text = dt.Rows[i]["txtISReference"].ToString();
            }
        }
        #endregion

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                CalculateSummary();
                int NDTRPT_Id = 0;
                if (lblReportId.Text != "")
                    NDTRPT_Id = Convert.ToInt32(lblReportId.Text);
                string PurposeOfEvaluation = "", NDTCoverage = "", ReasonForTesting = "", TypeOfNDTUsed = "",
                    SampleSize = "", NdtSample = "", MethodOfSelOfMembers = "", SummaryResultUPV = "", SummaryResultRH ="", NetStdErrs = "",
                    SummaryCompStr = "", MembersLowerThanGrade5 = "", MembersLowerThanGrade10 = "", ConclusionRecom = "", ReadBelow3p5ForM20 = "", ReadBelow3p75ForM25 = "";

                if (chkPurposeOfEvaluation1.Checked == true)
                    PurposeOfEvaluation = chkPurposeOfEvaluation1.Text + "~";
                else
                    PurposeOfEvaluation = "~";
                if (chkPurposeOfEvaluation2.Checked == true)
                    PurposeOfEvaluation += chkPurposeOfEvaluation2.Text + "~";
                else
                    PurposeOfEvaluation += "~";
                if (chkPurposeOfEvaluation3.Checked == true)
                    PurposeOfEvaluation += chkPurposeOfEvaluation3.Text + "~";
                else
                    PurposeOfEvaluation += "~";
                if (chkPurposeOfEvaluation4.Checked == true)
                    PurposeOfEvaluation += chkPurposeOfEvaluation4.Text + "~" + txtPurposeOfEvaluationOther.Text.Trim();
                else
                    PurposeOfEvaluation += "~~";

                for (int i = 0; i < grdNDTCoverage.Rows.Count; i++)
                {
                    TextBox txtNoOfMembers = (TextBox)grdNDTCoverage.Rows[i].FindControl("txtNoOfMembers");
                    NDTCoverage += grdNDTCoverage.Rows[i].Cells[1].Text.Trim() + "~" + grdNDTCoverage.Rows[i].Cells[2].Text.Trim() + "~" + grdNDTCoverage.Rows[i].Cells[3].Text.Trim() + "~" + grdNDTCoverage.Rows[i].Cells[4].Text.Trim() + "~" + txtNoOfMembers.Text.Trim() + "^";
                }

                if (chkReasonForTest1.Checked == true)
                    ReasonForTesting = chkReasonForTest1.Text + "~";
                else
                    ReasonForTesting = "~";
                if (chkReasonForTest2.Checked == true)
                    ReasonForTesting += chkReasonForTest2.Text + "~";
                else
                    ReasonForTesting += "~";
                if (chkReasonForTest3.Checked == true)
                    ReasonForTesting += chkReasonForTest3.Text + "~";
                else
                    ReasonForTesting += "~";
                if (chkReasonForTest4.Checked == true)
                    ReasonForTesting += chkReasonForTest4.Text + "~";
                else
                    ReasonForTesting += "~";
                if (chkReasonForTest5.Checked == true)
                    ReasonForTesting += chkReasonForTest5.Text + "~";
                else
                    ReasonForTesting += "~";
                if (chkReasonForTest6.Checked == true)
                    ReasonForTesting += chkReasonForTest6.Text + "~";
                else
                    ReasonForTesting += "~";
                if (chkReasonForTest7.Checked == true)
                    ReasonForTesting += chkReasonForTest7.Text + "~";
                else
                    ReasonForTesting += "~";
                if (chkReasonForTest8.Checked == true)
                    ReasonForTesting += chkReasonForTest8.Text + "~" + txtReasonForTestOther.Text;
                else
                    ReasonForTesting += "~";

                for (int i = 0; i < grdTest.Rows.Count; i++)
                {
                    TextBox txtTypeOfTest = (TextBox)grdTest.Rows[i].FindControl("txtTypeOfTest");
                    TextBox txtTestingMethod = (TextBox)grdTest.Rows[i].FindControl("txtTestingMethod");
                    //TextBox txtISReference = (TextBox)grdTest.Rows[i].FindControl("txtISReference");

                    TypeOfNDTUsed += txtTypeOfTest.Text.Trim() + "~" + txtTestingMethod.Text.Trim() + "^"; //+ "~" + txtISReference.Text.Trim() + "^";
                }

                SampleSize = txtSampleSize1.Text.Trim();

                for (int i = 0; i < grdNDTSample.Rows.Count; i++)
                {
                    TextBox txtNoOfMembersTested = (TextBox)grdNDTSample.Rows[i].FindControl("txtNoOfMembersTested");
                    //NdtSample += grdNDTSample.Rows[i].Cells[1].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[2].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[3].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[4].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[5].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[6].Text.Trim() + "^";
                    NdtSample += grdNDTSample.Rows[i].Cells[1].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[2].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[3].Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[4].Text.Trim() + "~" + txtNoOfMembersTested.Text.Trim() + "~" + grdNDTSample.Rows[i].Cells[6].Text.Trim() + "^";
                }
                MethodOfSelOfMembers = txtMethodOfSelection.Text.Trim();

                for (int i = 0; i < grdSummaryResultUPV.Rows.Count; i++)
                {
                    if (grdSummaryResultUPV.Rows[i].Cells[7].Text == "&nbsp;")
                        grdSummaryResultUPV.Rows[i].Cells[7].Text = "";
                    SummaryResultUPV += grdSummaryResultUPV.Rows[i].Cells[1].Text.Trim() + "~" + grdSummaryResultUPV.Rows[i].Cells[2].Text.Trim() + "~" + grdSummaryResultUPV.Rows[i].Cells[3].Text.Trim() + "~" + grdSummaryResultUPV.Rows[i].Cells[4].Text.Trim() + "~" + grdSummaryResultUPV.Rows[i].Cells[5].Text.Trim() + "~" + grdSummaryResultUPV.Rows[i].Cells[6].Text.Trim() + "~" + grdSummaryResultUPV.Rows[i].Cells[7].Text.Trim() + "^";
                }

                for (int i = 0; i < grdSummaryResultRH.Rows.Count; i++)
                {
                    SummaryResultRH += grdSummaryResultRH.Rows[i].Cells[1].Text.Trim() + "~" + grdSummaryResultRH.Rows[i].Cells[2].Text.Trim() + "~" + grdSummaryResultRH.Rows[i].Cells[3].Text.Trim() + "~" + grdSummaryResultRH.Rows[i].Cells[4].Text.Trim() + "~" + grdSummaryResultRH.Rows[i].Cells[5].Text.Trim() + "~" + grdSummaryResultRH.Rows[i].Cells[6].Text.Trim() + "^";
                }

                for (int i = 0; i < grdNetStdErrs.Rows.Count; i++)
                {
                    NetStdErrs += grdNetStdErrs.Rows[i].Cells[1].Text.Trim() + "~" + grdNetStdErrs.Rows[i].Cells[2].Text.Trim() + "~" + grdNetStdErrs.Rows[i].Cells[3].Text.Trim() + "~" + grdNetStdErrs.Rows[i].Cells[4].Text.Trim() + "~" + grdNetStdErrs.Rows[i].Cells[5].Text.Trim() + "^";
                }

                for (int i = 0; i < grdSummaryCompStr.Rows.Count; i++)
                {
                    SummaryCompStr += grdSummaryCompStr.Rows[i].Cells[1].Text.Trim() + "~" + grdSummaryCompStr.Rows[i].Cells[2].Text.Trim() + "~" + grdSummaryCompStr.Rows[i].Cells[3].Text.Trim() + "~" + grdSummaryCompStr.Rows[i].Cells[4].Text.Trim() + "~" + grdSummaryCompStr.Rows[i].Cells[5].Text.Trim() + "~" + grdSummaryCompStr.Rows[i].Cells[6].Text.Trim() + "^";
                }
               
                for (int i = 0; i < grdMembersGrade5.Rows.Count; i++)
                {
                    MembersLowerThanGrade5 += grdMembersGrade5.Rows[i].Cells[1].Text.Trim() + "~" + grdMembersGrade5.Rows[i].Cells[2].Text.Trim() + "~" + grdMembersGrade5.Rows[i].Cells[3].Text.Trim() + "~" + grdMembersGrade5.Rows[i].Cells[4].Text.Trim() + "~" + grdMembersGrade5.Rows[i].Cells[5].Text.Trim() + "^";
                }

                for (int i = 0; i < grdMembersGrade10.Rows.Count; i++)
                {
                    MembersLowerThanGrade10 += grdMembersGrade10.Rows[i].Cells[1].Text.Trim() + "~" + grdMembersGrade10.Rows[i].Cells[2].Text.Trim() + "~" + grdMembersGrade10.Rows[i].Cells[3].Text.Trim() + "~" + grdMembersGrade10.Rows[i].Cells[4].Text.Trim() + "~" + grdMembersGrade10.Rows[i].Cells[5].Text.Trim() + "^";
                }

                //if (optConclusion1.Checked == true)
                //    ConclusionRecom = "1" + "~" + txtConclusion1_1.Text.Trim() + "~" + txtConclusion1_2.Text.Trim();
                //else if (optConclusion2.Checked == true)
                //    ConclusionRecom = "2" + "~" + txtConclusion2_1.Text.Trim() + "~" + txtConclusion2_2.Text.Trim();
                //else if (optConclusion3.Checked == true)
                //    ConclusionRecom = "3" + "~" + txtConclusion3_1.Text.Trim() + "~" + txtConclusion3_2.Text.Trim() + "~" + txtConclusion3_3.Text.Trim();
                //else if (optConclusion4.Checked == true)
                //    ConclusionRecom = "4" + "~" + txtConclusion4_1.Text.Trim() + "~" + txtConclusion4_2.Text.Trim();

                for (int i = 0; i < grdReadBelow3p5ForM20.Rows.Count; i++)
                {
                    if (grdReadBelow3p5ForM20.Rows[i].Cells[1].Text.Trim() != "")
                        ReadBelow3p5ForM20 += grdReadBelow3p5ForM20.Rows[i].Cells[1].Text.Trim() + "~" + grdReadBelow3p5ForM20.Rows[i].Cells[2].Text.Trim() + "~" + grdReadBelow3p5ForM20.Rows[i].Cells[3].Text.Trim() + "~" + grdReadBelow3p5ForM20.Rows[i].Cells[4].Text.Trim() + "~" + grdReadBelow3p5ForM20.Rows[i].Cells[5].Text.Trim() + "^";
                }

                for (int i = 0; i < grdReadBelow3p75ForM25.Rows.Count; i++)
                {
                    if (grdReadBelow3p75ForM25.Rows[i].Cells[1].Text.Trim() != "")
                        ReadBelow3p75ForM25 += grdReadBelow3p75ForM25.Rows[i].Cells[1].Text.Trim() + "~" + grdReadBelow3p75ForM25.Rows[i].Cells[2].Text.Trim() + "~" + grdReadBelow3p75ForM25.Rows[i].Cells[3].Text.Trim() + "~" + grdReadBelow3p75ForM25.Rows[i].Cells[4].Text.Trim() + "~" + grdReadBelow3p75ForM25.Rows[i].Cells[5].Text.Trim() + "^";
                }

                for (int i = 0; i < grdConcRecom.Rows.Count; i++)
                {
                    Label lblConclusionRecommendation = (Label)grdConcRecom.Rows[i].FindControl("lblConclusionRecommendation");
                    ConclusionRecom += grdConcRecom.Rows[i].Cells[2].Text.Trim() + "~" + grdConcRecom.Rows[i].Cells[3].Text.Trim() + "~" + grdConcRecom.Rows[i].Cells[4].Text.Trim() + "~" + grdConcRecom.Rows[i].Cells[5].Text.Trim() + "~" + grdConcRecom.Rows[i].Cells[6].Text.Trim() + "~" + lblConclusionRecommendation.Text + "^";
                }

                dc.NDTReport_Update(NDTRPT_Id, ddlReferenceNo.SelectedValue, txtClientName.Text, txtSiteName.Text, txtAddress.Text, ddlProjectDesc.SelectedItem.Text, ddlRccStruct.SelectedItem.Text, Convert.ToInt32(txtAge.Text), ddlAgeIn.SelectedItem.Text, PurposeOfEvaluation, NDTCoverage, ReasonForTesting, TypeOfNDTUsed,
                    SampleSize, NdtSample, MethodOfSelOfMembers, SummaryResultUPV, SummaryResultRH, NetStdErrs, SummaryCompStr, MembersLowerThanGrade5, MembersLowerThanGrade10, ConclusionRecom, ReadBelow3p5ForM20, ReadBelow3p75ForM25, Convert.ToByte(ddlApproveBy.SelectedValue));

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Updated Successfully.";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
                lnkPrint.Visible = true;
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (ddlReferenceNo.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Reference No.";
                ddlReferenceNo.Focus();
                valid = false;
            }
            else if (ddlProjectDesc.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Project Description";
                ddlProjectDesc.Focus();
                valid = false;
            }
            else if (ddlRccStruct.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select RCC Structure";
                ddlRccStruct.Focus();
                valid = false;
            }
            else if (txtAge.Text == "")
            {
                lblMsg.Text = "Input Age of concrete";
                txtAge.Focus();
                valid = false;
            }
            else if (chkPurposeOfEvaluation1.Checked == false && chkPurposeOfEvaluation2.Checked == false
                && chkPurposeOfEvaluation3.Checked == false && chkPurposeOfEvaluation4.Checked == false)
            {
                lblMsg.Text = "Select Purpose of Evaluation";
                chkPurposeOfEvaluation1.Focus();
                valid = false;
            }
            else if (chkPurposeOfEvaluation4.Checked == true && txtPurposeOfEvaluationOther.Text == "")
            {
                lblMsg.Text = "Input Purpose of Evaluation";
                txtPurposeOfEvaluationOther.Focus();
                valid = false;
            }
            if (valid == true)
            {
                for (int i = 0; i < grdNDTCoverage.Rows.Count; i++)
                {
                    TextBox txtNoOfMembers = (TextBox)grdNDTCoverage.Rows[i].FindControl("txtNoOfMembers");
                    if (txtNoOfMembers.Text == "")
                    {
                        lblMsg.Text = "Enter No. of Members for Sr No. " + (i + 1) + ".";
                        txtNoOfMembers.Focus();
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == true)
            {
                if (chkReasonForTest1.Checked == false && chkReasonForTest2.Checked == false
                && chkReasonForTest3.Checked == false && chkReasonForTest4.Checked == false
                && chkReasonForTest5.Checked == false && chkReasonForTest6.Checked == false
                && chkReasonForTest7.Checked == false && chkReasonForTest8.Checked == false)
                {
                    lblMsg.Text = "Select Reason for NDT testing";
                    chkPurposeOfEvaluation1.Focus();
                    valid = false;
                }
                else if (chkReasonForTest8.Checked == true && txtReasonForTestOther.Text == "")
                {
                    lblMsg.Text = "Input Reason for NDT testing";
                    txtReasonForTestOther.Focus();
                    valid = false;
                }
            }
            if (valid == true)
            {
                for (int i = 0; i < grdTest.Rows.Count; i++)
                {
                    TextBox txtTypeOfTest = (TextBox)grdTest.Rows[i].FindControl("txtTypeOfTest");
                    TextBox txtTestingMethod = (TextBox)grdTest.Rows[i].FindControl("txtTestingMethod");
                    //TextBox txtISReference = (TextBox)grdTest.Rows[i].FindControl("txtISReference");

                    if (txtTypeOfTest.Text == "")
                    {
                        lblMsg.Text = "Enter Type of Test for Sr No. " + (i + 1) + ".";
                        txtTypeOfTest.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtTestingMethod.Text == "")
                    {
                        lblMsg.Text = "Enter Testing method for Sr No. " + (i + 1) + ".";
                        txtTestingMethod.Focus();
                        valid = false;
                        break;
                    }
                    //else if (txtISReference.Text == "")
                    //{
                    //    lblMsg.Text = "Enter IS Reference for Sr No. " + (i + 1) + ".";
                    //    txtISReference.Focus();
                    //    valid = false;
                    //    break;
                    //}
                }
            }
            //if (valid == true && txtSampleSize1.Text.Trim() == "")
            //{
            //    lblMsg.Text = "Input Recommended Sample size";
            //    txtSampleSize1.Focus();
            //    valid = false;
            //}
            if (valid == true && txtMethodOfSelection.Text.Trim() == "")
            {
                lblMsg.Text = "Input Method of Selection of Members";
                txtMethodOfSelection.Focus();
                valid = false;
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdConcRecom.Rows.Count; i++)
                {
                    grdConcRecom.Rows[i].Cells[6].Text = grdConcRecom.Rows[i].Cells[6].Text.Replace("&amp;", "").Replace("&nbsp;","").Replace("nbsp;", ""); 
                    if (grdConcRecom.Rows[i].Cells[6].Text == "")
                    {
                        lblMsg.Text = "Select Conclusion / Recommendation for Sr No. " + (i + 1) + ".";
                        grdConcRecom.Focus();
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == true && ddlApproveBy.SelectedIndex <= 0)
            {
                lblMsg.Text = "Please Select the Approve By";
                ddlApproveBy.Focus();
                valid = false;
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
        protected void lnkExit_Click(object sender, EventArgs e)
        {
            //Response.Redirect("ProposalList.aspx");
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkFetchAsPerCurrentData_Click(object sender, EventArgs e)
        {
            FetchAsPerCurrentData();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (ddlReferenceNo.SelectedIndex > 0)
            {
                rpt.PrintSelectedReport("NDT", ddlReferenceNo.SelectedValue, "Check", "", "", "", "", "", "", "");
            }
        }

        protected void chkSelectWBS_CheckedChanged(object sender, EventArgs e)
        {
            txtConclusion1_1.Text = "Although In -situ concrete grade is lower than the design grade in our opinion  it may be accepted considering the in-situ factors of safety  considered in design . However final decision for the same rests with the  structural engineer after considering the loads on members. ";
            txtConclusion1_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion1_3.Text = "";
            txtConclusion2_1.Text = "Insitu strengths are lower than the design grade of concrete . Since the concrete is exhibiting large variation in strengths . Additional testing is  recommended  to reduce the uncertainty of prediction of strength";
            txtConclusion2_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion2_3.Text = "";
            txtConclusion3_1.Text = "Since in-situ compressive strengths are significantly lower than design grade of concrete , the structure may require retrofitting to make it fit for intended use .";
            txtConclusion3_2.Text = "Additional Testing is recommended to refine the estimated strengths for purpose of   retrofit design .";
            txtConclusion3_3.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion3_4.Text = "";
            txtConclusion4_1.Text = "The in-situ compressive strengths are satisfactory considering the design grade of concrete. ";
            txtConclusion4_2.Text = "Advice of structural engineer should be taken to decide further course of action";
            txtConclusion4_3.Text = "";
            optConclusion1.Checked = false;
            optConclusion2.Checked = false;
            optConclusion3.Checked = false;
            optConclusion4.Checked = false;

            lblSelectedWbs.Text = "";
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            int j = gvr.RowIndex;

            for (int i = 0; i < grdConcRecom.Rows.Count; i++)
            {
                if (i != j)
                {
                    CheckBox chkSelectWBS1 = (CheckBox)grdConcRecom.Rows[i].FindControl("chkSelectWBS");
                    chkSelectWBS1.Checked = false;
                }
            }
            CheckBox chkSelectWBS = (CheckBox)grdConcRecom.Rows[j].FindControl("chkSelectWBS");
            if (chkSelectWBS.Checked == true)
            {
                lblSelectedWbs.Text = "Building - " + grdConcRecom.Rows[j].Cells[2].Text + ", Floor - " + grdConcRecom.Rows[j].Cells[3].Text + ", Grade of concrete - " + grdConcRecom.Rows[j].Cells[4].Text + ", Member Type - " + grdConcRecom.Rows[j].Cells[5].Text + ", Recommended Grade of concrete (MPa) - " + grdSummaryCompStr.Rows[j].Cells[6].Text;

                if (grdConcRecom.Rows[j].Cells[6].Text != "")
                {
                    Label lblConclusionRecommendation = (Label)grdConcRecom.Rows[j].FindControl("lblConclusionRecommendation");
                    string[] strval = lblConclusionRecommendation.Text.Split('#');
                    if (grdConcRecom.Rows[j].Cells[6].Text == "1")
                    {
                        optConclusion1.Checked = true;
                        txtConclusion1_1.Text = strval[0];
                        txtConclusion1_2.Text = strval[1];
                        txtConclusion1_3.Text = strval[2];
                    }
                    else if (grdConcRecom.Rows[j].Cells[6].Text == "2")
                    {
                        optConclusion2.Checked = true;
                        txtConclusion2_1.Text = strval[0];
                        txtConclusion2_2.Text = strval[1];
                        txtConclusion2_3.Text = strval[2];
                    }
                    else if (grdConcRecom.Rows[j].Cells[6].Text == "3")
                    {
                        optConclusion3.Checked = true;
                        txtConclusion3_1.Text = strval[0];
                        txtConclusion3_2.Text = strval[1];
                        txtConclusion3_3.Text = strval[2];
                        txtConclusion3_4.Text = strval[3];
                    }
                    else if (grdConcRecom.Rows[j].Cells[6].Text == "4")
                    {
                        optConclusion4.Checked = true;
                        txtConclusion4_1.Text = strval[0];
                        txtConclusion4_2.Text = strval[1];
                        txtConclusion4_3.Text = strval[2];
                    }
                }
            }

        }

        protected void lnkSelectConcRecom_Click(object sender, EventArgs e)
        {
            if (optConclusion1.Checked == false && optConclusion2.Checked == false
                && optConclusion3.Checked == false && optConclusion4.Checked == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Conclusion / Recommendation.');", true);
            }
            else
            {
                for (int i = 0; i < grdConcRecom.Rows.Count; i++)
                {
                    CheckBox chkSelectWBS = (CheckBox)grdConcRecom.Rows[i].FindControl("chkSelectWBS");
                    Label lblConclusionRecommendation = (Label)grdConcRecom.Rows[i].FindControl("lblConclusionRecommendation");
                    if (chkSelectWBS.Checked == true)
                    {
                        if (optConclusion1.Checked == true)
                        {
                            grdConcRecom.Rows[i].Cells[6].Text = "1";
                            lblConclusionRecommendation.Text = txtConclusion1_1.Text + "#" + txtConclusion1_2.Text + "#" + txtConclusion1_3.Text;
                        }
                        else if (optConclusion2.Checked == true)
                        {
                            grdConcRecom.Rows[i].Cells[6].Text = "2";
                            lblConclusionRecommendation.Text = txtConclusion2_1.Text + "#" + txtConclusion2_2.Text + "#" + txtConclusion2_3.Text;
                        }
                        else if (optConclusion3.Checked == true)
                        {
                            grdConcRecom.Rows[i].Cells[6].Text = "3";
                            lblConclusionRecommendation.Text = txtConclusion3_1.Text + "#" + txtConclusion3_2.Text + "#" + txtConclusion3_3.Text + "#" + txtConclusion3_4.Text;
                        }
                        else if (optConclusion4.Checked == true)
                        {
                            grdConcRecom.Rows[i].Cells[6].Text = "4";
                            lblConclusionRecommendation.Text = txtConclusion4_1.Text + "#" + txtConclusion4_2.Text + "#" + txtConclusion4_3.Text;
                        }
                    }

                }
            }
        }
    }
}