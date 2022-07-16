using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net;
using System.Globalization;

namespace DESPLWEB
{
    public partial class Soil_Report_FDTS : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        string dispalyMsg = "";
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
                    txtRefNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    txtSampleName.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    lblStatus.Text = arrIndMsg[1].ToString().Trim();
                }
                Label lblheading = (Label)Master.FindControl("lblheading");
                
                //txtEntdChkdBy.Text = Session["LoginUserName"].ToString();
                //txtRefNo.Text = Session["ReferenceNo"].ToString();
                //txtSampleName.Text = Session["SoilSampleName"].ToString();

                if (lblStatus.Text == "Enter")
                {
                  //  lblStatus.Text = "Enter";
                    lblheading.Text = "Soil - Report Entry";
                }
                else if (lblStatus.Text == "Check")
                {
                   // lblStatus.Text = "Check";
                    lblheading.Text = "Soil - Report Checking";
                }
              
                LoadApprovedBy();
                DisplaySoilDetails();
            }
        }

        private void LoadDropdownForSand()
        {
            var smallSand = dc.SoilSetting_View("Calibration of Sand / Cylinder Cone-small");
            var mediumSand = dc.SoilSetting_View("Calibration of Sand / Cylinder Cone-medium");
            var bigSand = dc.SoilSetting_View("Calibration of Sand / Cylinder Cone-big");

            string[] arrySource = new string[3];
            string[] arrySand = new string[3];
            int i = 0;
            foreach (var ss in smallSand)
            {
                arrySource[i] = ss.SOSET_F1_var;
                arrySand[i] = "Small";
                i++;
                break;
            }
            foreach (var ss in mediumSand)
            {
                arrySource[i] = ss.SOSET_F1_var;
                arrySand[i] = "Medium";
                i++;
                break;
            }
            foreach (var ss in bigSand)
            {
                arrySource[i] = ss.SOSET_F1_var;
                arrySand[i] = "Big";
                i++;
                break;
            }

            ddlFDTSCylinder.Items.Insert(0, new ListItem("--Select--", "0"));
            for (int j = 0; j < i; j++)
            {
                ddlFDTSCylinder.Items.Insert(j + 1, new ListItem(arrySand[j], arrySource[j]));
            }
            ddlFDTSCylinder.DataBind();    
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
            ddlTestdApprdBy.Items.Insert(0, "---Select---");
        }

        private void DisplaySoilDetails()
        {
            //Inward details
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;
            var inwd = dc.SoilInward_View(txtRefNo.Text, 0);
            foreach (var soinwd in inwd)
            {
                if (ddl_NablScope.Items.FindByValue(soinwd.SOINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(soinwd.SOINWD_NablScope_var);
                }

                if (Convert.ToString(soinwd.SOINWD_NablLocation_int) != null && Convert.ToString(soinwd.SOINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(soinwd.SOINWD_NablLocation_int);
                }
                txtRefNo.Text = soinwd.SOINWD_ReferenceNo_var.ToString();
                txtReportNo.Text = soinwd.SOINWD_SetOfRecord_var;
                txtWitnesBy.Text = soinwd.SOINWD_WitnessBy_var;

                if (txtWitnesBy.Text != "")
                {
                    chkWitnessBy.Checked = true;
                    txtWitnesBy.Visible = true;
                }
                //if (soinwd.SOINWD_Status_tint == 1)
                if (lblStatus.Text == "Enter")
                {
                    //lblEntdChkdBy.Text = "Entered By";
                    lblTestdApprdBy.Text = "Tested By";
                }
                //else if (soinwd.SOINWD_Status_tint == 2)
                else if (lblStatus.Text == "Check")
                {
                    //lblEntdChkdBy.Text = "Checked By";
                    lblTestdApprdBy.Text = "Approved By";
                }
            }

            //Test Details   
            int rowNo = 0;
            var test = dc.SoilSampleTest_View(txtRefNo.Text, txtSampleName.Text);
            foreach (var sotest in test)
            {
                rowNo = 0;
                if (sotest.TEST_Sr_No == 10)
                {
                    #region display FDTS data
                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlFDTS.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlFDTS.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabFDTS.HeaderText = TabFDTS.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabFDTS.HeaderText = TabFDTS.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabFDTS.HeaderText = TabFDTS.HeaderText + "(Checked)";

                    LoadDropdownForSand();
                    TabFDTS.Visible = true;
                    pnlFDTS.Visible = true;
                    var soilIndData  = dc.SoilInward_View(txtRefNo.Text, 0).ToList();
                    txtFDTSRows.Text = soilIndData.FirstOrDefault().SOINWD_Pits_tint.ToString();
                    
                    var soilMDDData = dc.SoilMDD_View(txtRefNo.Text, txtSampleName.Text,2);
                    foreach (var soilMDD in soilMDDData)
                    {
                        //string[] arryMDD = soilMDDData.FirstOrDefault().SOMDD_Result_var.Split('|');
                        string[] arryMDD = soilMDD.SOMDD_Result_var.Split('|');
                        txtFDTSLabMDD.Text = arryMDD[0];
                        txtFDTSOMC.Text = arryMDD[1];
                    }
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtFDTSDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtFDTSDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtFDTSDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtFDTSDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var SoilFDTSData = dc.SoilFDTBySand_View(txtRefNo.Text, txtSampleName.Text);
                    foreach (var SoilFDTS in SoilFDTSData)
                    {
                        //Reading
                        AddRowFDTS();
                        TextBox txtFDTSLocation = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSLocation");
                        TextBox txtFDTSInitialWeight = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSInitialWeight");
                        TextBox txtFDTSWtAfterPouring = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSWtAfterPouring");
                        TextBox txtFDTSSandInConeHole = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSSandInConeHole");
                        TextBox txtFDTSSandInCone = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSSandInCone");
                        TextBox txtFDTSSandInHole = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSSandInHole");

                        TextBox txtFDTSBulkDensity = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSBulkDensity");
                        TextBox txtFDTSVolHole = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSVolHole");
                        TextBox txtFDTSWetSoilSamples = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSWetSoilSamples");
                        TextBox txtFDTSWetDensity = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSWetDensity");
                        TextBox txtFDTSInitialcontainerSample = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSInitialcontainerSample");
                        TextBox txtFDTSContainerSample = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSContainerSample");
                        DropDownList ddlFDTSContainerNo = (DropDownList)grdFDTS.Rows[rowNo].FindControl("ddlFDTSContainerNo");
                        TextBox txtFDTSWtofContainer = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSWtofContainer");
                        TextBox txtFDTSWC = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSWC");
                        TextBox txtFDTSDryDensity = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSDryDensity");
                        TextBox txtFDTSCompaction = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSCompaction");
                        TextBox txtFDTSDepthOfHole = (TextBox)grdFDTS.Rows[rowNo].FindControl("txtFDTSDepthOfHole");

                        txtFDTSLocation.Text = SoilFDTS.SOFDTS_Location_var;

                        txtFDTSInitialWeight.Text = SoilFDTS.SOFDTS_InitialWeight_dec.ToString();
                        txtFDTSWtAfterPouring.Text = SoilFDTS.SOFDTS_WtAfterPouring_dec.ToString();
                        txtFDTSSandInConeHole.Text = SoilFDTS.SOFDTS_SandInConePlusHole_dec.ToString();
                        txtFDTSSandInCone.Text = SoilFDTS.SOFDTS_SandInCone_dec.ToString();
                        txtFDTSSandInHole.Text = SoilFDTS.SOFDTS_SandInHole_dec.ToString();

                        txtFDTSBulkDensity.Text = SoilFDTS.SOFDTS_BulkDensityOfSand_dec.ToString();
                        txtFDTSVolHole.Text = SoilFDTS.SOFDTS_VolOfHole_dec.ToString();
                        txtFDTSWetSoilSamples.Text = SoilFDTS.SOFDTS_WtOfWetSoilSamplesfromHole_dec.ToString();
                        txtFDTSWetDensity.Text = SoilFDTS.SOFDTS_WetDensity_dec.ToString();
                        txtFDTSInitialcontainerSample.Text = SoilFDTS.SOFDTS_InitialWtOfcontainerPlusSample_dec.ToString();
                        txtFDTSContainerSample.Text = SoilFDTS.SOFDTS_WtOfContainerPlusSampleAfterDrying_dec.ToString();

                        ddlFDTSContainerNo.Items.FindByText(Convert.ToDecimal(SoilFDTS.SOFDTS_ContainerNo_dec).ToString("0")).Selected = true;
                        txtFDTSWtofContainer.Text = SoilFDTS.SOFDTS_WtOfContainer_dec.ToString();
                        txtFDTSWC.Text = SoilFDTS.SOFDTS_WC_dec.ToString();
                        txtFDTSDryDensity.Text = SoilFDTS.SOFDTS_DryDensity_dec.ToString();
                        txtFDTSCompaction.Text = SoilFDTS.SOFDTS_Compaction_dec.ToString();
                        txtFDTSDepthOfHole.Text = SoilFDTS.SOFDTS_DepthOfHole_dec.ToString();

                        if (rowNo == 0)
                        {
                            string[] strMddResult = SoilFDTS.SOFDTS_OtherDetails_var.Split('|');
                            ddlFDTSCylinder.SelectedValue = strMddResult[0];                         
                        }
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        FDTSRowsChanged();
                    }                    
                    #endregion
                }
                else if (sotest.TEST_Sr_No == 11)
                {
                    #region display FDTCC data
                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlFDTCoreCutter.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlFDTCoreCutter.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabFDTCoreCutter.HeaderText = TabFDTCoreCutter.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabFDTCoreCutter.HeaderText = TabFDTCoreCutter.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabFDTCoreCutter.HeaderText = TabFDTCoreCutter.HeaderText + "(Checked)";

                    TabFDTCoreCutter.Visible = true;
                    pnlFDTCoreCutter.Visible = true;
                    //row dat
                    var soilIndData = dc.SoilInward_View(txtRefNo.Text, 0).ToList();
                    txtFDTCCRows.Text = soilIndData.FirstOrDefault().SOINWD_Cores_tint.ToString();

                    // mdd omc data
                    var soilMDDData = dc.SoilMDD_View(txtRefNo.Text, txtSampleName.Text, 2);
                    foreach (var soilMDD in soilMDDData)
                    {
                        //string[] arryMDD = soilMDDData.FirstOrDefault().SOMDD_Result_var.Split('|');
                        string[] arryMDD = soilMDD.SOMDD_Result_var.Split('|');
                        txtMaxDryDensity.Text = arryMDD[0];
                        txtOptimumMoisture.Text = arryMDD[1];
                    }
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtFDTCCDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtFDTCCDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtFDTCCDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtFDTCCDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var SoilFDTSData = dc.SoilFDTByCore_View(txtRefNo.Text, txtSampleName.Text);
                    foreach (var SoilFDTS in SoilFDTSData)
                    {
                        //Reading
                        AddRowFDTCC();
                        TextBox txtFDTCCLocation = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCLocation");
                        TextBox txtFDTCCDeterminationNo = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCDeterminationNo");
                        TextBox txtFDTCCWtofCoreCutterWetSoil = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCWtofCoreCutterWetSoil");
                        DropDownList ddlFDTCCCoreCutterNo = (DropDownList)grdFDTCC.Rows[rowNo].FindControl("ddlFDTCCCoreCutterNo");
                        TextBox txtFDTCCWtofCoreCutter = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCWtofCoreCutter");
                        TextBox txtFDTCCWtofWetSoil = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCWtofWetSoil");
                        TextBox txtFDTCCVolCoreCutter = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCVolCoreCutter");
                        TextBox txtFDTCCBulkDensity = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCBulkDensity");
                        DropDownList ddlFDTCCContainerNo = (DropDownList)grdFDTCC.Rows[rowNo].FindControl("ddlFDTCCContainerNo");
                        TextBox txtFDTCCWtContainerWetSoilSample = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCWtContainerWetSoilSample");
                        TextBox txtFDTCCWtofContainerDrySoil = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCWtofContainerDrySoil");
                        TextBox txtFDTCCWeightofContainer = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCWeightofContainer");
                        TextBox txtFDTCCMoistureContent = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCMoistureContent");
                        TextBox txtFDTCCDryDensity = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCDryDensity");
                        TextBox txtFDTCCCompaction = (TextBox)grdFDTCC.Rows[rowNo].FindControl("txtFDTCCCompaction");

                        txtFDTCCLocation.Text = SoilFDTS.SOFDTC_Location_var;
                        txtFDTCCDeterminationNo.Text = Convert.ToInt32(SoilFDTS.SOFDTC_DeterminationNo_dec).ToString();
                        txtFDTCCWtofCoreCutterWetSoil.Text = SoilFDTS.SOFDTC_WtOfCoreCutterPlusWetSoil_dec.ToString();
                        //ddlFDTCCCoreCutterNo.Items.FindByText(Convert.ToDecimal(SoilFDTS.SOFDTC_CoreCutterNo_dec).ToString("0")).Selected = true;                        
                        ddlFDTCCCoreCutterNo.SelectedValue = Convert.ToDecimal(SoilFDTS.SOFDTC_CoreCutterNo_dec).ToString("0");
                        txtFDTCCWtofCoreCutter.Text = SoilFDTS.SOFDTC_WtOfCoreCutter_dec.ToString();
                        txtFDTCCWtofWetSoil.Text = SoilFDTS.SOFDTC_WtOfWetSoil_dec.ToString();
                        txtFDTCCVolCoreCutter.Text = SoilFDTS.SOFDTC_VolumeOfCoreCutter_dec.ToString();
                        txtFDTCCBulkDensity.Text = SoilFDTS.SOFDTC_BulkDensity_dec.ToString();
                        //ddlFDTCCContainerNo.Items.FindByText(Convert.ToDecimal(SoilFDTS.SOFDTC_ContainerNo_dec).ToString("0")).Selected = true;
                        ddlFDTCCContainerNo.SelectedValue = Convert.ToDecimal(SoilFDTS.SOFDTC_ContainerNo_dec).ToString("0");
                        txtFDTCCWtContainerWetSoilSample.Text = SoilFDTS.SOFDTC_WtOfContWithWetSoilSample_dec.ToString();
                        txtFDTCCWtofContainerDrySoil.Text = SoilFDTS.SOFDTC_WtOfContWithDrySoil_dec.ToString();
                        txtFDTCCWeightofContainer.Text = SoilFDTS.SOFDTC_WeightOfContainer_dec.ToString();
                        txtFDTCCMoistureContent.Text = SoilFDTS.SOFDTC_MoistureContent_dec.ToString();
                        txtFDTCCDryDensity.Text = SoilFDTS.SOFDTC_DryDensity_dec.ToString();
                        txtFDTCCCompaction.Text = SoilFDTS.SOFDTC_Compaction_dec.ToString();


                        if (rowNo == 0)
                        {
                            string[] strMddResult = SoilFDTS.SOFDTC_OtherDetails_var.Split('|');
                            txtMaxDryDensity.Text = strMddResult[0];
                            txtOptimumMoisture.Text = strMddResult[1];
                        }
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        FDTCCRowsChanged();
                    }
                    
                    #endregion
                }
            }

            ////Remark details
            rowNo = 0;
            var remark = dc.SoilRemarkDetail_View(txtRefNo.Text);
            foreach (var rem in remark)
            {
                AddRowRemark();
                TextBox txtRemark = (TextBox)grdRemark.Rows[rowNo].FindControl("txtRemark");
                txtRemark.Text = rem.SOREM_Remark_var;
                rowNo++;
            }
            if (rowNo == 0)
                AddRowRemark();

        }

        #region add/delete rows grdFDTS grid
        protected void AddRowFDTS()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["FDTSTable"] != null)
            {
                GetCurrentDataFDTS();
                dt = (DataTable)ViewState["FDTSTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtFDTSLocation", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSInitialWeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSWtAfterPouring", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSSandInConeHole", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSSandInCone", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSSandInHole", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSBulkDensity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSVolHole", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSWetSoilSamples", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSWetDensity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSInitialcontainerSample", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSContainerSample", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlFDTSContainerNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSWtofContainer", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSWC", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSDryDensity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSCompaction", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTSDepthOfHole", typeof(string)));
            }

            dr = dt.NewRow();

            dr["txtFDTSLocation"] = string.Empty;
            dr["txtFDTSInitialWeight"] = string.Empty;
            dr["txtFDTSWtAfterPouring"] = string.Empty;
            dr["txtFDTSSandInConeHole"] = string.Empty;
            dr["txtFDTSSandInCone"] = string.Empty;
            dr["txtFDTSSandInHole"] = string.Empty;
            dr["txtFDTSBulkDensity"] = string.Empty;
            dr["txtFDTSVolHole"] = string.Empty;
            dr["txtFDTSWetSoilSamples"] = string.Empty;
            dr["txtFDTSWetDensity"] = string.Empty;
            dr["txtFDTSInitialcontainerSample"] = string.Empty;
            dr["txtFDTSContainerSample"] = string.Empty;
            dr["ddlFDTSContainerNo"] = string.Empty;
            dr["txtFDTSWtofContainer"] = string.Empty;
            dr["txtFDTSWC"] = string.Empty;
            dr["txtFDTSDryDensity"] = string.Empty;
            dr["txtFDTSCompaction"] = string.Empty;
            dr["txtFDTSDepthOfHole"] = string.Empty;


            dt.Rows.Add(dr);

            ViewState["FDTSTable"] = dt;
            grdFDTS.DataSource = dt;
            grdFDTS.DataBind();
            SetPreviousDataFDTS();
        }
        protected void DeleteRowFDTS(int rowIndex)
        {
            GetCurrentDataFDTS();
            DataTable dt = ViewState["FDTSTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["FDTSTable"] = dt;
            grdFDTS.DataSource = dt;
            grdFDTS.DataBind();
            SetPreviousDataFDTS();
        }
        protected void SetPreviousDataFDTS()
        {
            DataTable dt = (DataTable)ViewState["FDTSTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtFDTSLocation = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSLocation");
                TextBox txtFDTSInitialWeight = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialWeight");
                TextBox txtFDTSWtAfterPouring = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtAfterPouring");
                TextBox txtFDTSSandInConeHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInConeHole");
                TextBox txtFDTSSandInCone = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInCone");
                TextBox txtFDTSSandInHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInHole");
                TextBox txtFDTSBulkDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSBulkDensity");
                TextBox txtFDTSVolHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSVolHole");
                TextBox txtFDTSWetSoilSamples = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetSoilSamples");
                TextBox txtFDTSWetDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetDensity");
                TextBox txtFDTSInitialcontainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialcontainerSample");
                TextBox txtFDTSContainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSContainerSample");

                DropDownList ddlFDTSContainerNo = (DropDownList)grdFDTS.Rows[i].FindControl("ddlFDTSContainerNo");
                TextBox txtFDTSWtofContainer = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtofContainer");
                TextBox txtFDTSWC = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWC");
                TextBox txtFDTSDryDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDryDensity");
                TextBox txtFDTSCompaction = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSCompaction");
                TextBox txtFDTSDepthOfHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDepthOfHole");


                txtFDTSLocation.Text = dt.Rows[i]["txtFDTSLocation"].ToString();
                txtFDTSInitialWeight.Text = dt.Rows[i]["txtFDTSInitialWeight"].ToString();
                txtFDTSWtAfterPouring.Text = dt.Rows[i]["txtFDTSWtAfterPouring"].ToString();
                txtFDTSSandInConeHole.Text = dt.Rows[i]["txtFDTSSandInConeHole"].ToString();
                txtFDTSSandInCone.Text = dt.Rows[i]["txtFDTSSandInCone"].ToString();
                txtFDTSSandInHole.Text = dt.Rows[i]["txtFDTSSandInHole"].ToString();
                txtFDTSBulkDensity.Text = dt.Rows[i]["txtFDTSBulkDensity"].ToString();
                txtFDTSVolHole.Text = dt.Rows[i]["txtFDTSVolHole"].ToString();
                txtFDTSWetSoilSamples.Text = dt.Rows[i]["txtFDTSWetSoilSamples"].ToString();
                txtFDTSWetDensity.Text = dt.Rows[i]["txtFDTSWetDensity"].ToString();
                txtFDTSInitialcontainerSample.Text = dt.Rows[i]["txtFDTSInitialcontainerSample"].ToString();
                txtFDTSContainerSample.Text = dt.Rows[i]["txtFDTSContainerSample"].ToString();
                ddlFDTSContainerNo.SelectedValue = dt.Rows[i]["ddlFDTSContainerNo"].ToString();
                txtFDTSWtofContainer.Text = dt.Rows[i]["txtFDTSWtofContainer"].ToString();
                txtFDTSWC.Text = dt.Rows[i]["txtFDTSWC"].ToString();
                txtFDTSDryDensity.Text = dt.Rows[i]["txtFDTSDryDensity"].ToString();
                txtFDTSCompaction.Text = dt.Rows[i]["txtFDTSCompaction"].ToString();
                txtFDTSDepthOfHole.Text = dt.Rows[i]["txtFDTSDepthOfHole"].ToString();

            }
        }
        protected void GetCurrentDataFDTS()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtFDTSLocation", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSInitialWeight", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSWtAfterPouring", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSSandInConeHole", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSSandInCone", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSSandInHole", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSBulkDensity", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSVolHole", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSWetSoilSamples", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSWetDensity", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSInitialcontainerSample", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSContainerSample", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlFDTSContainerNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSWtofContainer", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSWC", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSDryDensity", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSCompaction", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTSDepthOfHole", typeof(string)));

            for (int i = 0; i < grdFDTS.Rows.Count; i++)
            {
                TextBox txtFDTSLocation = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSLocation");
                TextBox txtFDTSInitialWeight = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialWeight");
                TextBox txtFDTSWtAfterPouring = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtAfterPouring");
                TextBox txtFDTSSandInConeHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInConeHole");
                TextBox txtFDTSSandInCone = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInCone");
                TextBox txtFDTSSandInHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInHole");
                TextBox txtFDTSBulkDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSBulkDensity");
                TextBox txtFDTSVolHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSVolHole");
                TextBox txtFDTSWetSoilSamples = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetSoilSamples");
                TextBox txtFDTSWetDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetDensity");
                TextBox txtFDTSInitialcontainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialcontainerSample");
                TextBox txtFDTSContainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSContainerSample");
                DropDownList ddlFDTSContainerNo = (DropDownList)grdFDTS.Rows[i].FindControl("ddlFDTSContainerNo");
                TextBox txtFDTSWtofContainer = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtofContainer");
                TextBox txtFDTSWC = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWC");
                TextBox txtFDTSDryDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDryDensity");
                TextBox txtFDTSCompaction = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSCompaction");
                TextBox txtFDTSDepthOfHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDepthOfHole");

                dr = dt.NewRow();

                dr["txtFDTSLocation"] = txtFDTSLocation.Text;
                dr["txtFDTSInitialWeight"] = txtFDTSInitialWeight.Text;
                dr["txtFDTSWtAfterPouring"] = txtFDTSWtAfterPouring.Text;
                dr["txtFDTSSandInConeHole"] = txtFDTSSandInConeHole.Text;
                dr["txtFDTSSandInCone"] = txtFDTSSandInCone.Text;
                dr["txtFDTSSandInHole"] = txtFDTSSandInHole.Text;
                dr["txtFDTSBulkDensity"] = txtFDTSBulkDensity.Text;
                dr["txtFDTSVolHole"] = txtFDTSVolHole.Text;
                dr["txtFDTSWetSoilSamples"] = txtFDTSWetSoilSamples.Text;
                dr["txtFDTSWetDensity"] = txtFDTSWetDensity.Text;
                dr["txtFDTSInitialcontainerSample"] = txtFDTSInitialcontainerSample.Text;
                dr["txtFDTSContainerSample"] = txtFDTSContainerSample.Text;
                dr["ddlFDTSContainerNo"] = ddlFDTSContainerNo.SelectedValue;
                dr["txtFDTSWtofContainer"] = txtFDTSWtofContainer.Text;
                dr["txtFDTSWC"] = txtFDTSWC.Text;
                dr["txtFDTSDryDensity"] = txtFDTSDryDensity.Text;
                dr["txtFDTSCompaction"] = txtFDTSCompaction.Text;
                dr["txtFDTSDepthOfHole"] = txtFDTSDepthOfHole.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["FDTSTable"] = dt;

        }
        #endregion

        #region add/delete rows grdFDTCC grid
        protected void AddRowFDTCC()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["FDTCCTable"] != null)
            {
                GetCurrentDataFDTCC();
                dt = (DataTable)ViewState["FDTCCTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtFDTCCLocation", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCDeterminationNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCWtofCoreCutterWetSoil", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlFDTCCCoreCutterNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCWtofCoreCutter", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCWtofWetSoil", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCVolCoreCutter", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCBulkDensity", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlFDTCCContainerNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCWtContainerWetSoilSample", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCWtofContainerDrySoil", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCWeightofContainer", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCMoistureContent", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCDryDensity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFDTCCCompaction", typeof(string)));

            }

            dr = dt.NewRow();

            dr["txtFDTCCLocation"] = string.Empty;
            dr["txtFDTCCDeterminationNo"] = string.Empty;
            dr["txtFDTCCWtofCoreCutterWetSoil"] = string.Empty;
            dr["ddlFDTCCCoreCutterNo"] = string.Empty;
            dr["txtFDTCCWtofCoreCutter"] = string.Empty;
            dr["txtFDTCCWtofWetSoil"] = string.Empty;
            dr["txtFDTCCVolCoreCutter"] = string.Empty;
            dr["txtFDTCCBulkDensity"] = string.Empty;
            dr["ddlFDTCCContainerNo"] = string.Empty;
            dr["txtFDTCCWtContainerWetSoilSample"] = string.Empty;
            dr["txtFDTCCWtofContainerDrySoil"] = string.Empty;
            dr["txtFDTCCWeightofContainer"] = string.Empty;
            dr["txtFDTCCMoistureContent"] = string.Empty;
            dr["txtFDTCCDryDensity"] = string.Empty;
            dr["txtFDTCCCompaction"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["FDTCCTable"] = dt;
            grdFDTCC.DataSource = dt;
            grdFDTCC.DataBind();
            SetPreviousDataFDTCC();
        }
        protected void DeleteRowFDTCC(int rowIndex)
        {
            GetCurrentDataFDTCC();
            DataTable dt = ViewState["FDTCCTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["FDTCCTable"] = dt;
            grdFDTCC.DataSource = dt;
            grdFDTCC.DataBind();
            SetPreviousDataFDTCC();
        }
        protected void SetPreviousDataFDTCC()
        {
            DataTable dt = (DataTable)ViewState["FDTCCTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtFDTCCLocation = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCLocation");
                TextBox txtFDTCCDeterminationNo = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDeterminationNo");
                TextBox txtFDTCCWtofCoreCutterWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutterWetSoil");
                DropDownList ddlFDTCCCoreCutterNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCCoreCutterNo");
                TextBox txtFDTCCWtofCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutter");
                TextBox txtFDTCCWtofWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofWetSoil");
                TextBox txtFDTCCVolCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCVolCoreCutter");
                TextBox txtFDTCCBulkDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCBulkDensity");
                DropDownList ddlFDTCCContainerNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCContainerNo");
                TextBox txtFDTCCWtContainerWetSoilSample = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtContainerWetSoilSample");
                TextBox txtFDTCCWtofContainerDrySoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofContainerDrySoil");
                TextBox txtFDTCCWeightofContainer = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWeightofContainer");
                TextBox txtFDTCCMoistureContent = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCMoistureContent");
                TextBox txtFDTCCDryDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDryDensity");
                TextBox txtFDTCCCompaction = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCCompaction");


                txtFDTCCLocation.Text = dt.Rows[i]["txtFDTCCLocation"].ToString();
                txtFDTCCDeterminationNo.Text = dt.Rows[i]["txtFDTCCDeterminationNo"].ToString();
                txtFDTCCWtofCoreCutterWetSoil.Text = dt.Rows[i]["txtFDTCCWtofCoreCutterWetSoil"].ToString();
                ddlFDTCCCoreCutterNo.SelectedValue = dt.Rows[i]["ddlFDTCCCoreCutterNo"].ToString();
                txtFDTCCWtofCoreCutter.Text = dt.Rows[i]["txtFDTCCWtofCoreCutter"].ToString();
                txtFDTCCWtofWetSoil.Text = dt.Rows[i]["txtFDTCCWtofWetSoil"].ToString();
                txtFDTCCVolCoreCutter.Text = dt.Rows[i]["txtFDTCCVolCoreCutter"].ToString();
                txtFDTCCBulkDensity.Text = dt.Rows[i]["txtFDTCCBulkDensity"].ToString();
                ddlFDTCCContainerNo.SelectedValue = dt.Rows[i]["ddlFDTCCContainerNo"].ToString();
                txtFDTCCWtContainerWetSoilSample.Text = dt.Rows[i]["txtFDTCCWtContainerWetSoilSample"].ToString();
                txtFDTCCWtofContainerDrySoil.Text = dt.Rows[i]["txtFDTCCWtofContainerDrySoil"].ToString();
                txtFDTCCWeightofContainer.Text = dt.Rows[i]["txtFDTCCWeightofContainer"].ToString();
                txtFDTCCMoistureContent.Text = dt.Rows[i]["txtFDTCCMoistureContent"].ToString();
                txtFDTCCDryDensity.Text = dt.Rows[i]["txtFDTCCDryDensity"].ToString();
                txtFDTCCCompaction.Text = dt.Rows[i]["txtFDTCCCompaction"].ToString();

            }
        }
        protected void GetCurrentDataFDTCC()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtFDTCCLocation", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCDeterminationNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCWtofCoreCutterWetSoil", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlFDTCCCoreCutterNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCWtofCoreCutter", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCWtofWetSoil", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCVolCoreCutter", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCBulkDensity", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlFDTCCContainerNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCWtContainerWetSoilSample", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCWtofContainerDrySoil", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCWeightofContainer", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCMoistureContent", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCDryDensity", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFDTCCCompaction", typeof(string)));

            for (int i = 0; i < grdFDTCC.Rows.Count; i++)
            {
                TextBox txtFDTCCLocation = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCLocation");
                TextBox txtFDTCCDeterminationNo = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDeterminationNo");
                TextBox txtFDTCCWtofCoreCutterWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutterWetSoil");
                DropDownList ddlFDTCCCoreCutterNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCCoreCutterNo");
                TextBox txtFDTCCWtofCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutter");
                TextBox txtFDTCCWtofWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofWetSoil");
                TextBox txtFDTCCVolCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCVolCoreCutter");
                TextBox txtFDTCCBulkDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCBulkDensity");
                DropDownList ddlFDTCCContainerNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCContainerNo");
                TextBox txtFDTCCWtContainerWetSoilSample = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtContainerWetSoilSample");
                TextBox txtFDTCCWtofContainerDrySoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofContainerDrySoil");
                TextBox txtFDTCCWeightofContainer = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWeightofContainer");
                TextBox txtFDTCCMoistureContent = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCMoistureContent");
                TextBox txtFDTCCDryDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDryDensity");
                TextBox txtFDTCCCompaction = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCCompaction");


                dr = dt.NewRow();
                dr["txtFDTCCLocation"] = txtFDTCCLocation.Text;
                dr["txtFDTCCDeterminationNo"] = txtFDTCCDeterminationNo.Text;
                dr["txtFDTCCWtofCoreCutterWetSoil"] = txtFDTCCWtofCoreCutterWetSoil.Text;
                dr["ddlFDTCCCoreCutterNo"] = ddlFDTCCCoreCutterNo.SelectedValue;
                dr["txtFDTCCWtofCoreCutter"] = txtFDTCCWtofCoreCutter.Text;
                dr["txtFDTCCWtofWetSoil"] = txtFDTCCWtofWetSoil.Text;
                dr["txtFDTCCVolCoreCutter"] = txtFDTCCVolCoreCutter.Text;
                dr["txtFDTCCBulkDensity"] = txtFDTCCBulkDensity.Text;
                dr["ddlFDTCCContainerNo"] = ddlFDTCCContainerNo.SelectedValue;
                dr["txtFDTCCWtContainerWetSoilSample"] = txtFDTCCWtContainerWetSoilSample.Text;
                dr["txtFDTCCWtofContainerDrySoil"] = txtFDTCCWtofContainerDrySoil.Text;
                dr["txtFDTCCWeightofContainer"] = txtFDTCCWeightofContainer.Text;
                dr["txtFDTCCMoistureContent"] = txtFDTCCMoistureContent.Text;
                dr["txtFDTCCDryDensity"] = txtFDTCCDryDensity.Text;
                dr["txtFDTCCCompaction"] = txtFDTCCCompaction.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["FDTCCTable"] = dt;

        }
        #endregion

        #region add/delete rows grdRemark grid
        protected void AddRowRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["RemarkTable"] != null)
            {
                GetCurrentDataRemark();
                dt = (DataTable)ViewState["RemarkTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtRemark", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtRemark"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["RemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataRemark();
        }
        protected void DeleteRowRemark(int rowIndex)
        {
            GetCurrentDataRemark();
            DataTable dt = ViewState["RemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["RemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataRemark();
        }
        protected void SetPreviousDataRemark()
        {
            DataTable dt = (DataTable)ViewState["RemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                txtRemark.Text = dt.Rows[i]["txtRemark"].ToString();
            }
        }
        protected void GetCurrentDataRemark()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtRemark", typeof(string)));
            for (int i = 0; i < grdRemark.Rows.Count; i++)
            {
                TextBox txtRemark = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txtRemark");

                dr = dt.NewRow();
                dr["txtRemark"] = txtRemark.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["RemarkTable"] = dt;

        }
        protected void imgBtnAddRow_Click(object sender, EventArgs e)
        {
            AddRowRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, EventArgs e)
        {
            if (grdRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowRemark(gvr.RowIndex);
            }
        }
        #endregion

      

        protected void ddlFDTSCylinder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFDTSCylinder.SelectedIndex > 0)
            {              
                for (int i = 0; i < grdFDTS.Rows.Count; i++)
                {
                    TextBox txtFDTSInitialWeight = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialWeight");
                    txtFDTSInitialWeight.Text = ddlFDTSCylinder.SelectedValue;
                }
            }

        }
        protected void grdFDTS_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlFDTSContainerNo = (e.Row.FindControl("ddlFDTSContainerNo") as DropDownList);
                var soset = dc.SoilSetting_View("Weight Of Container");
                ddlFDTSContainerNo.DataSource = soset;
                ddlFDTSContainerNo.DataTextField = "SOSET_F1_var";
                ddlFDTSContainerNo.DataValueField = "SOSET_F2_var";
                ddlFDTSContainerNo.DataBind();
                if (ddlFDTSContainerNo.Items.Count > 0)
                    ddlFDTSContainerNo.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        protected void grdFDTCC_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlFDTCCContainerNo = (e.Row.FindControl("ddlFDTCCContainerNo") as DropDownList);
                var soset = dc.SoilSetting_View("Weight Of Container");
                ddlFDTCCContainerNo.DataSource = soset;
                ddlFDTCCContainerNo.DataTextField = "SOSET_F1_var";
                //ddlFDTCCContainerNo.DataValueField = "SOSET_F2_var";
                ddlFDTCCContainerNo.DataBind();
                if (ddlFDTCCContainerNo.Items.Count > 0)
                    ddlFDTCCContainerNo.Items.Insert(0, new ListItem("Select", "0"));


                DropDownList ddlFDTCCCoreCutterNo = (e.Row.FindControl("ddlFDTCCCoreCutterNo") as DropDownList);
                var soset1 = dc.SoilSetting_View("Core Cutter Dimensions");
                ddlFDTCCCoreCutterNo.DataSource = soset1;
                ddlFDTCCCoreCutterNo.DataTextField = "SOSET_F1_var";
                //ddlFDTCCCoreCutterNo.DataValueField = "SOSET_F2_var";
                ddlFDTCCCoreCutterNo.DataBind();
                if (ddlFDTCCCoreCutterNo.Items.Count > 0)
                    ddlFDTCCCoreCutterNo.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        protected void ddlFDTSContainerNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtFDTSWtofContainer = (TextBox)gvr.FindControl("txtFDTSWtofContainer");
                DropDownList ddlFDTSContainerNo = (DropDownList)gvr.FindControl("ddlFDTSContainerNo");
                if (ddlFDTSContainerNo.SelectedIndex > 0)
                {
                    txtFDTSWtofContainer.Text = ddlFDTSContainerNo.SelectedItem.Value;
                }
            }
        }

        protected void ddlFDTCCContainerNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtFDTCCWeightofContainer = (TextBox)gvr.FindControl("txtFDTCCWeightofContainer");
                DropDownList ddlFDTCCContainerNo = (DropDownList)gvr.FindControl("ddlFDTCCContainerNo");
                if (ddlFDTCCContainerNo.SelectedIndex > 0)
                {
                    txtFDTCCWeightofContainer.Text = ddlFDTCCContainerNo.SelectedItem.Value;
                }
            }
        }

        protected void ddlFDTCCCoreCutterNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtFDTCCWtofCoreCutter = (TextBox)gvr.FindControl("txtFDTCCWtofCoreCutter");
                DropDownList ddlFDTCCCoreCutterNo = (DropDownList)gvr.FindControl("ddlFDTCCCoreCutterNo");
                if (ddlFDTCCCoreCutterNo.SelectedIndex > 0)
                {
                    txtFDTCCWtofCoreCutter.Text = ddlFDTCCCoreCutterNo.SelectedItem.Value;
                  
                }
            }
        }
        protected void chkWitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txtWitnesBy.Text = "";
            if (chkWitnessBy.Checked == true)
                txtWitnesBy.Visible = true;
            else
                txtWitnesBy.Visible = false;
        }
        private void FDTSRowsChanged()
        {
            if (txtFDTSRows.Text != "")
            {
                if (Convert.ToInt32(txtFDTSRows.Text) < grdFDTS.Rows.Count)
                {
                    for (int i = grdFDTS.Rows.Count; i > Convert.ToInt32(txtFDTSRows.Text); i--)
                    {
                        if (grdFDTS.Rows.Count > 1)
                        {
                            DeleteRowFDTS(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtFDTSRows.Text) > grdFDTS.Rows.Count)
                {
                    for (int i = grdFDTS.Rows.Count + 1; i <= Convert.ToInt32(txtFDTSRows.Text); i++)
                    {
                        AddRowFDTS();
                    }
                }
            }
        }
        private void FDTCCRowsChanged()
        {
            if (txtFDTCCRows.Text != "")
            {
                if (Convert.ToInt32(txtFDTCCRows.Text) < grdFDTCC.Rows.Count)
                {
                    for (int i = grdFDTCC.Rows.Count; i > Convert.ToInt32(txtFDTCCRows.Text); i--)
                    {
                        if (grdFDTCC.Rows.Count > 1)
                        {
                            DeleteRowFDTCC(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtFDTCCRows.Text) > grdFDTCC.Rows.Count)
                {
                    for (int i = grdFDTCC.Rows.Count + 1; i <= Convert.ToInt32(txtFDTCCRows.Text); i++)
                    {
                        AddRowFDTCC();
                    }
                }
            }
        }
        private void Calculate()
        {
            if (TabFDTS.Visible == true && validateFDTS() == true)
            {
                CalculateFDTS();
            }
            if (TabFDTCoreCutter.Visible == true && validateFDTCC() == true)
            {
                CalculateFDTCC();
            }
        }
        private void CalculateFDTS()
        {
            
                decimal result = 0;
                for (int i = 0; i < grdFDTS.Rows.Count; i++)
                {
                    TextBox txtFDTSLocation = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSLocation");
                    TextBox txtFDTSInitialWeight = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialWeight");
                    TextBox txtFDTSWtAfterPouring = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtAfterPouring");
                    TextBox txtFDTSSandInConeHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInConeHole");
                    TextBox txtFDTSSandInCone = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInCone");
                    TextBox txtFDTSSandInHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInHole");

                    TextBox txtFDTSBulkDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSBulkDensity");
                    TextBox txtFDTSVolHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSVolHole");
                    TextBox txtFDTSWetSoilSamples = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetSoilSamples");
                    TextBox txtFDTSWetDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetDensity");
                    TextBox txtFDTSInitialcontainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialcontainerSample");
                    TextBox txtFDTSContainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSContainerSample");

                    DropDownList ddlFDTSContainerNo = (DropDownList)grdFDTS.Rows[i].FindControl("ddlFDTSContainerNo");
                    TextBox txtFDTSWtofContainer = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtofContainer");
                    TextBox txtFDTSWC = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWC");
                    TextBox txtFDTSDryDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDryDensity");
                    TextBox txtFDTSCompaction = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSCompaction");
                    TextBox txtFDTSDepthOfHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDepthOfHole");

                    var wtdata = dc.SoilSetting_View("Weight Of Container").ToList();
                    foreach (var containerData in wtdata)
                    {
                        if (containerData.SOSET_F1_var == ddlFDTSContainerNo.SelectedItem.Text)
                        {
                            txtFDTSWtofContainer.Text = containerData.SOSET_F2_var;
                            break;
                        }
                    }


                    if (txtFDTSInitialWeight.Text != "" && txtFDTSWtAfterPouring.Text != "")
                    {
                        result = Convert.ToDecimal(txtFDTSInitialWeight.Text) - Convert.ToDecimal(txtFDTSWtAfterPouring.Text);
                        if (result < 0) result = 0;
                        txtFDTSSandInConeHole.Text = result.ToString("0.00");
                    }

                    if (txtFDTSSandInConeHole.Text != "" && txtFDTSSandInCone.Text != "")
                    {
                        result = Convert.ToDecimal(txtFDTSSandInConeHole.Text) - Convert.ToDecimal(txtFDTSSandInCone.Text);
                        if (result < 0) result = 0;
                        txtFDTSSandInHole.Text = result.ToString();
                    }
                    if (txtFDTSSandInHole.Text != "" && txtFDTSBulkDensity.Text != "")
                    {
                        result = Convert.ToDecimal(txtFDTSSandInHole.Text) / Convert.ToDecimal(txtFDTSBulkDensity.Text);
                        if (result < 0) result = 0;
                        txtFDTSVolHole.Text = result.ToString("0.0");
                    }
                    if (txtFDTSVolHole.Text != "" && txtFDTSWetSoilSamples.Text != "")
                    {
                        result = Convert.ToDecimal(txtFDTSWetSoilSamples.Text) / Convert.ToDecimal(txtFDTSVolHole.Text);
                        if (result < 0) result = 0;
                        txtFDTSWetDensity.Text = result.ToString("0.00");
                    }

                    if (txtFDTSInitialcontainerSample.Text != "" && txtFDTSContainerSample.Text != "" && txtFDTSWtofContainer.Text != "")
                    {
                        result = (Convert.ToDecimal(txtFDTSInitialcontainerSample.Text) - Convert.ToDecimal(txtFDTSContainerSample.Text)) / (Convert.ToDecimal(txtFDTSContainerSample.Text) - Convert.ToDecimal(txtFDTSWtofContainer.Text)) * 100;
                        if (result < 0) result = 0;
                        txtFDTSWC.Text = result.ToString("0.00");
                    }

                    if (txtFDTSWC.Text != "" && txtFDTSWetDensity.Text != "")
                    {
                        result = Convert.ToDecimal(txtFDTSWetDensity.Text) / (1 + Convert.ToDecimal(txtFDTSWC.Text) / 100);
                        if (result < 0) result = 0;
                        txtFDTSDryDensity.Text = result.ToString("0.00");
                    }
                    if (txtFDTSDryDensity.Text != "")
                    {
                        result = 100 * Convert.ToDecimal(txtFDTSDryDensity.Text) / Convert.ToDecimal(txtFDTSLabMDD.Text);
                        if (result < 0) result = 0;
                        txtFDTSCompaction.Text = result.ToString("0.00");
                    }
                }
            

        }
        private void CalculateFDTCC()
        {
            
                decimal result = 0;
                for (int i = 0; i < grdFDTCC.Rows.Count; i++)
                {
                    TextBox txtFDTCCWtofCoreCutterWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutterWetSoil");
                    TextBox txtFDTCCWtofCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutter");
                    DropDownList ddlFDTCCCoreCutterNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCCoreCutterNo");
                    TextBox txtFDTCCWtofWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofWetSoil");
                    TextBox txtFDTCCVolCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCVolCoreCutter");
                    TextBox txtFDTCCBulkDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCBulkDensity");
                    DropDownList ddlFDTCCContainerNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCContainerNo");
                    TextBox txtFDTCCWtContainerWetSoilSample = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtContainerWetSoilSample");
                    TextBox txtFDTCCWtofContainerDrySoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofContainerDrySoil");
                    TextBox txtFDTCCWeightofContainer = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWeightofContainer");
                    TextBox txtFDTCCMoistureContent = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCMoistureContent");
                    TextBox txtFDTCCDryDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDryDensity");
                    TextBox txtFDTCCCompaction = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCCompaction");

                    var wtdata = dc.SoilSetting_View("Weight Of Container").ToList();
                    foreach (var containerData in wtdata)
                    {
                        if (containerData.SOSET_F1_var == ddlFDTCCContainerNo.SelectedItem.Text)
                        {
                            txtFDTCCWeightofContainer.Text = containerData.SOSET_F2_var;
                            break;
                        }
                    }
                    var ccdata = dc.SoilSetting_View("Core Cutter Dimensions").ToList();
                    foreach (var containerData in ccdata)
                    {
                        if (containerData.SOSET_F1_var == ddlFDTCCCoreCutterNo.SelectedItem.Text)
                        {
                            txtFDTCCWtofCoreCutter.Text = containerData.SOSET_F2_var;
                            txtFDTCCVolCoreCutter.Text = containerData.SOSET_F5_var;
                            break;
                        }
                    }

                    if (txtFDTCCWtofCoreCutter.Text != "" && txtFDTCCWtofWetSoil.Text != "")
                    {
                        result = Convert.ToDecimal(txtFDTCCWtofCoreCutter.Text) + Convert.ToDecimal(txtFDTCCWtofWetSoil.Text);
                        if (result < 0) result = 0;
                        txtFDTCCWtofCoreCutterWetSoil.Text = Math.Round(result).ToString();
                    }
                    if (txtFDTCCWtofWetSoil.Text != "" && txtFDTCCVolCoreCutter.Text != "")
                    {
                        result = Convert.ToDecimal(txtFDTCCWtofWetSoil.Text) / Convert.ToDecimal(txtFDTCCVolCoreCutter.Text);
                        if (result < 0) result = 0;
                        txtFDTCCBulkDensity.Text = result.ToString("0.00");
                    }
                    if (txtFDTCCWtContainerWetSoilSample.Text != "" && txtFDTCCWtofContainerDrySoil.Text != "" && txtFDTCCWeightofContainer.Text != "")
                    {
                        result = ((Convert.ToDecimal(txtFDTCCWtContainerWetSoilSample.Text) - Convert.ToDecimal(txtFDTCCWtofContainerDrySoil.Text)) / (Convert.ToDecimal(txtFDTCCWtofContainerDrySoil.Text) - Convert.ToDecimal(txtFDTCCWeightofContainer.Text))) * 100;
                        if (result < 0) result = 0;
                        txtFDTCCMoistureContent.Text = result.ToString("0.00");
                    }
                    if (txtFDTCCBulkDensity.Text != "" && txtFDTCCMoistureContent.Text != "")
                    {
                        result = (Convert.ToDecimal(txtFDTCCBulkDensity.Text) * 100) / (100 + Convert.ToDecimal(txtFDTCCMoistureContent.Text));
                        if (result < 0) result = 0;
                        txtFDTCCDryDensity.Text = result.ToString("0.00");
                    }
                    if (txtFDTCCDryDensity.Text != "")
                    {
                        result = (Convert.ToDecimal(txtFDTCCDryDensity.Text) / Convert.ToDecimal(txtMaxDryDensity.Text)) * 100;
                        if (result < 0) result = 0;
                        txtFDTCCCompaction.Text = result.ToString("0.00");
                    }
                }
            
        }

        protected Boolean ValidateData()
        {
           
            Boolean valid = true;
           
            //validate FSI data
            #region validate FDTS data
            if (TabFDTS.Visible == true)
            {

                if (txtFDTSRows.Text == "" || txtFDTSRows.Text == "0")
                {
                    dispalyMsg = "Rows can not be zero or blank.";
                    txtFDTSRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (ddlFDTSCylinder.SelectedIndex <= 0)
                {
                    dispalyMsg = "Select Sand Pouring Cylinder.";
                    ddlFDTSCylinder.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                //date validation
                else if (txtFDTSDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtFDTSDateOfTesting.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtFDTSLabMDD.Text == "")
                {
                    dispalyMsg = "Enter value for LAB MDD.";
                    txtFDTSLabMDD.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtFDTSOMC.Text == "")
                {
                    dispalyMsg = "Enter value for OMC.";
                    txtFDTSOMC.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtFDTSDateOfTesting.Text != "")
                {
                    DateTime testingDate = DateTime.ParseExact(txtFDTSDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (testingDate > System.DateTime.Now)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtFDTSDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                }
                if (valid == true)
                {
                    valid = validateFDTS();
                }

            }           
            #endregion

            #region validate FDTCC data
            if (TabFDTCoreCutter.Visible == true)
            {

                if (txtFDTCCRows.Text == "" || txtFDTCCRows.Text == "0")
                {
                    dispalyMsg = "Rows can not be zero or blank.";
                    txtFDTCCRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 1;
                }

                //date validation
                else if (txtFDTCCDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtFDTCCDateOfTesting.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 1;
                }
                else if (txtFDTCCDateOfTesting.Text != "")
                {
                    DateTime testingDate = DateTime.ParseExact(txtFDTCCDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (testingDate > System.DateTime.Now)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtFDTCCDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                    }
                }
                else if (txtMaxDryDensity.Text == "")
                {
                    dispalyMsg = "Enter value for Max Dry Density.";
                    txtMaxDryDensity.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 1;
                }
                else if (txtOptimumMoisture.Text == "")
                {
                    dispalyMsg = "Enter value for Optimum Moisture.";
                    txtOptimumMoisture.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 1;
                }

                if (valid == true)
                {
                    valid = validateFDTCC();
                }

            }
            #endregion


            if (valid == true)
            {                
                if (grdRemark.Rows.Count > 1)
                {
                    for (int i = 0; i <= grdRemark.Rows.Count - 1; i++)
                    {
                        TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                        txtRemark.Text = txtRemark.Text.Trim();
                        if (txtRemark.Text == "" && grdRemark.Rows.Count > 1)
                        {
                            dispalyMsg = "Please Enter Remark.";
                            TabContainerSoil.ActiveTabIndex = 1;
                            txtRemark.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (valid == true)
            {
                // Witness by validation
                if (chkWitnessBy.Checked == true && txtWitnesBy.Text == "")
                {
                    dispalyMsg = "Please Enter Witness By Name.";
                    txtWitnesBy.Focus();
                    valid = false;
                }
                else if (ddlTestdApprdBy.SelectedIndex <= 0)
                {
                    dispalyMsg = "Please Select Tested By/Approved By Name.";
                    ddlTestdApprdBy.Focus();
                    valid = false;
                }
                else if (ddl_NablScope.SelectedItem.Text == "--Select--")
                {
                    dispalyMsg = "Select 'NABL Scope'.";
                    valid = false;
                    ddl_NablScope.Focus();
                }
                else if (ddl_NABLLocation.SelectedItem.Text == "--Select--")
                {
                    dispalyMsg = "Select 'NABL Location'.";
                    valid = false;
                    ddl_NABLLocation.Focus();
                }
            }
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }

        protected bool validateFDTS()
        {
            Boolean valid = true;            
            if (txtFDTSLabMDD.Text == "")
            {
                dispalyMsg = "Enter value for LAB MDD.";
                txtFDTSLabMDD.Focus();
                valid = false;
                TabContainerSoil.ActiveTabIndex = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            else if (txtFDTSOMC.Text == "")
            {
                dispalyMsg = "Enter value for OMC.";
                txtFDTSOMC.Focus();
                valid = false;
                TabContainerSoil.ActiveTabIndex = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }            
            if (valid == true)
            {
                for (int i = 0; i < grdFDTS.Rows.Count; i++)
                {                    
                    TextBox txtFDTSLocation = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSLocation");
                    TextBox txtFDTSInitialWeight = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialWeight");
                    TextBox txtFDTSWtAfterPouring = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtAfterPouring");
                    TextBox txtFDTSSandInConeHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInConeHole");
                    TextBox txtFDTSSandInCone = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInCone");
                    TextBox txtFDTSSandInHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInHole");
                    TextBox txtFDTSBulkDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSBulkDensity");
                    TextBox txtFDTSVolHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSVolHole");
                    TextBox txtFDTSWetSoilSamples = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetSoilSamples");
                    TextBox txtFDTSWetDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetDensity");
                    TextBox txtFDTSInitialcontainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialcontainerSample");
                    TextBox txtFDTSContainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSContainerSample");
                    DropDownList ddlFDTSContainerNo = (DropDownList)grdFDTS.Rows[i].FindControl("ddlFDTSContainerNo");
                    TextBox txtFDTSWtofContainer = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtofContainer");
                    TextBox txtFDTSWC = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWC");
                    TextBox txtFDTSDryDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDryDensity");
                    TextBox txtFDTSCompaction = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSCompaction");
                    TextBox txtFDTSDepthOfHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDepthOfHole");

                    if (txtFDTSLocation.Text == "")
                    {
                        dispalyMsg = "Enter Location for row number " + (i + 1) + ".";
                        txtFDTSLocation.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (txtFDTSWtAfterPouring.Text == "")
                    {
                        dispalyMsg = "Enter Wt. after pouring for row no " + (i + 1) + ".";
                        txtFDTSWtAfterPouring.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (txtFDTSSandInCone.Text == "")
                    {
                        dispalyMsg = "Enter Sand in Cone for row number " + (i + 1) + ".";
                        txtFDTSSandInCone.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (txtFDTSBulkDensity.Text == "")
                    {
                        dispalyMsg = "Enter Bulk Density of Sand for row number " + (i + 1) + ".";
                        txtFDTSBulkDensity.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (txtFDTSWetSoilSamples.Text == "")
                    {
                        dispalyMsg = "Enter Wt. of Wet Soil Samples from Hole for row number " + (i + 1) + ".";
                        txtFDTSWetSoilSamples.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (txtFDTSInitialcontainerSample.Text == "")
                    {
                        dispalyMsg = "Enter Initial Wt. of Container + Sample for row number " + (i + 1) + ".";
                        txtFDTSInitialcontainerSample.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (txtFDTSContainerSample.Text == "")
                    {
                        dispalyMsg = "Enter Wt. of Container + Sample after drying for row number " + (i + 1) + ".";
                        txtFDTSContainerSample.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (ddlFDTSContainerNo.SelectedIndex <= 0)
                    {
                        dispalyMsg = "Select Container No. for row no " + (i + 1) + ".";
                        ddlFDTSContainerNo.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                    else if (txtFDTSDepthOfHole.Text == "")
                    {
                        dispalyMsg = "Enter Depth of Hole (Cm) for row number " + (i + 1) + ".";
                        txtFDTSDepthOfHole.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                        break;
                    }
                }
            }
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }

        protected bool validateFDTCC()
        {
            Boolean valid = true;
            if (txtMaxDryDensity.Text == "")
            {
                dispalyMsg = "Enter value for LAB MDD.";
                txtMaxDryDensity.Focus();
                valid = false;
                TabContainerSoil.ActiveTabIndex = 1;                
            }
            else if (txtOptimumMoisture.Text == "")
            {
                dispalyMsg = "Enter value for OMC.";
                txtOptimumMoisture.Focus();
                valid = false;
                TabContainerSoil.ActiveTabIndex = 1;
                
            }
            if (valid == true)
            {
                for (int i = 0; i < grdFDTCC.Rows.Count; i++)
                {

                    TextBox txtFDTCCLocation = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCLocation");
                    TextBox txtFDTCCDeterminationNo = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDeterminationNo");
                    DropDownList ddlFDTCCCoreCutterNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCCoreCutterNo");
                    TextBox txtFDTCCWtofWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofWetSoil");
                    DropDownList ddlFDTCCContainerNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCContainerNo");
                    TextBox txtFDTCCWtContainerWetSoilSample = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtContainerWetSoilSample");
                    TextBox txtFDTCCWtofContainerDrySoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofContainerDrySoil");


                    if (txtFDTCCLocation.Text == "")
                    {
                        dispalyMsg = "Enter Location for row number " + (i + 1) + ".";
                        txtFDTCCLocation.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                        break;
                    }
                    else if (txtFDTCCDeterminationNo.Text == "")
                    {
                        dispalyMsg = "Enter Determination No. for row no " + (i + 1) + ".";
                        txtFDTCCDeterminationNo.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                        break;
                    }
                    else if (ddlFDTCCCoreCutterNo.SelectedIndex <= 0)
                    {
                        dispalyMsg = "Select Core Cutter No. for row no " + (i + 1) + ".";
                        ddlFDTCCCoreCutterNo.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                        break;
                    }
                    else if (txtFDTCCWtofWetSoil.Text == "")
                    {
                        dispalyMsg = "Enter Wt of Wet Soil for row number " + (i + 1) + ".";
                        txtFDTCCWtofWetSoil.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                        break;
                    }
                    else if (ddlFDTCCContainerNo.SelectedIndex <= 0)
                    {
                        dispalyMsg = "Select Container No. for row no " + (i + 1) + ".";
                        ddlFDTCCContainerNo.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                        break;
                    }
                    else if (txtFDTCCWtContainerWetSoilSample.Text == "")
                    {
                        dispalyMsg = "Enter Wt of Container with Wet Soil Sample for row number " + (i + 1) + ".";
                        txtFDTCCWtContainerWetSoilSample.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                        break;
                    }
                    else if (txtFDTCCWtofContainerDrySoil.Text == "")
                    {
                        dispalyMsg = "Enter Wt of Container with Dry Soil for row number " + (i + 1) + ".";
                        txtFDTCCWtofContainerDrySoil.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                        break;
                    }
                }
            }
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {            
            Calculate();            
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                CalculateFDTS();
                CalculateFDTCC();
                //inward update
                byte reportStatus = 0, enteredBy = 0, checkedBy = 0, testedBy = 0, approvedBy = 0;

                int testId = 0;
                if (lblStatus.Text == "Enter")
                {
                    reportStatus = 1;
                    enteredBy = Convert.ToByte(Session["LoginId"]);
                    testedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, true, false, false, false, false, false, false);
                }
                else if (lblStatus.Text == "Check")
                {
                    reportStatus = 2;
                    checkedBy = Convert.ToByte(Session["LoginId"]);
                    approvedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txtRefNo.Text, "SO", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                DateTime testingDate = new DateTime();
                if (TabFDTS.Visible == true)
                {
                    testingDate = DateTime.ParseExact(txtFDTSDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else if (TabFDTCoreCutter.Visible == true)
                {
                    testingDate = DateTime.ParseExact(txtFDTCCDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                #region save FDTS data
                if (TabFDTS.Visible == true)
                {

                    string strResult = "";
                    dc.SoilFDTBySand_Update(0, txtRefNo.Text, txtSampleName.Text, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", true);
                    for (int i = 0; i < grdFDTS.Rows.Count; i++)
                    {
                        TextBox txtFDTSLocation = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSLocation");
                        TextBox txtFDTSInitialWeight = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialWeight");
                        TextBox txtFDTSWtAfterPouring = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtAfterPouring");
                        TextBox txtFDTSSandInConeHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInConeHole");
                        TextBox txtFDTSSandInCone = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInCone");
                        TextBox txtFDTSSandInHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSSandInHole");

                        TextBox txtFDTSBulkDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSBulkDensity");
                        TextBox txtFDTSVolHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSVolHole");
                        TextBox txtFDTSWetSoilSamples = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetSoilSamples");
                        TextBox txtFDTSWetDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWetDensity");
                        TextBox txtFDTSInitialcontainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSInitialcontainerSample");
                        TextBox txtFDTSContainerSample = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSContainerSample");

                        DropDownList ddlFDTSContainerNo = (DropDownList)grdFDTS.Rows[i].FindControl("ddlFDTSContainerNo");
                        TextBox txtFDTSWtofContainer = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWtofContainer");
                        TextBox txtFDTSWC = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSWC");
                        TextBox txtFDTSDryDensity = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDryDensity");
                        TextBox txtFDTSCompaction = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSCompaction");
                        TextBox txtFDTSDepthOfHole = (TextBox)grdFDTS.Rows[i].FindControl("txtFDTSDepthOfHole");

                        if (i == 0)
                        {
                            strResult = ddlFDTSCylinder.SelectedValue + "|" + txtFDTSLabMDD.Text + "|" + txtFDTSOMC.Text;
                        }

                        dc.SoilFDTBySand_Update(0, txtRefNo.Text, txtSampleName.Text, txtFDTSLocation.Text, Convert.ToDecimal(txtFDTSInitialWeight.Text), Convert.ToDecimal(txtFDTSWtAfterPouring.Text),
                            Convert.ToDecimal(txtFDTSSandInConeHole.Text), Convert.ToDecimal(txtFDTSSandInCone.Text), Convert.ToDecimal(txtFDTSSandInHole.Text),
                            Convert.ToDecimal(txtFDTSBulkDensity.Text), Convert.ToDecimal(txtFDTSVolHole.Text), Convert.ToDecimal(txtFDTSWetSoilSamples.Text),
                            Convert.ToDecimal(txtFDTSWetDensity.Text), Convert.ToDecimal(txtFDTSInitialcontainerSample.Text), Convert.ToDecimal(txtFDTSContainerSample.Text),
                            Convert.ToDecimal(ddlFDTSContainerNo.SelectedItem.Text), Convert.ToDecimal(txtFDTSWtofContainer.Text), Convert.ToDecimal(txtFDTSWC.Text),
                            Convert.ToDecimal(txtFDTSDryDensity.Text), Convert.ToDecimal(txtFDTSCompaction.Text), Convert.ToDecimal(txtFDTSDepthOfHole.Text), strResult, false);
                    }
                    int testSrNo = 10;

                    var test = dc.Test(testSrNo, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtFDTSRows.Text), true);
                }
                #endregion

                #region save FDTCC data
                if (TabFDTCoreCutter.Visible == true)
                {

                    string strResult = "";
                    dc.SoilFDTByCore_Update(0, txtRefNo.Text, txtSampleName.Text, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", true);
                    for (int i = 0; i < grdFDTCC.Rows.Count; i++)
                    {
                        TextBox txtFDTCCLocation = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCLocation");
                        TextBox txtFDTCCDeterminationNo = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDeterminationNo");
                        TextBox txtFDTCCWtofCoreCutterWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutterWetSoil");
                        DropDownList ddlFDTCCCoreCutterNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCCoreCutterNo");
                        TextBox txtFDTCCWtofCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofCoreCutter");
                        TextBox txtFDTCCWtofWetSoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofWetSoil");
                        TextBox txtFDTCCVolCoreCutter = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCVolCoreCutter");
                        TextBox txtFDTCCBulkDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCBulkDensity");
                        DropDownList ddlFDTCCContainerNo = (DropDownList)grdFDTCC.Rows[i].FindControl("ddlFDTCCContainerNo");
                        TextBox txtFDTCCWtContainerWetSoilSample = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtContainerWetSoilSample");
                        TextBox txtFDTCCWtofContainerDrySoil = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWtofContainerDrySoil");
                        TextBox txtFDTCCWeightofContainer = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCWeightofContainer");
                        TextBox txtFDTCCMoistureContent = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCMoistureContent");
                        TextBox txtFDTCCDryDensity = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCDryDensity");
                        TextBox txtFDTCCCompaction = (TextBox)grdFDTCC.Rows[i].FindControl("txtFDTCCCompaction");


                        if (i == 0)
                        {
                            strResult = txtMaxDryDensity.Text + "|" + txtOptimumMoisture.Text;
                        }

                        dc.SoilFDTByCore_Update(0,
                        txtRefNo.Text,
                        txtSampleName.Text,
                        txtFDTCCLocation.Text,
                        Convert.ToDecimal(txtFDTCCDeterminationNo.Text),
                        Convert.ToDecimal(txtFDTCCWtofCoreCutterWetSoil.Text),
                        Convert.ToDecimal(ddlFDTCCCoreCutterNo.SelectedItem.Text),
                        Convert.ToDecimal(txtFDTCCWtofCoreCutter.Text),
                        Convert.ToDecimal(txtFDTCCWtofWetSoil.Text),
                        Convert.ToDecimal(txtFDTCCVolCoreCutter.Text),
                        Convert.ToDecimal(txtFDTCCBulkDensity.Text),
                        Convert.ToDecimal(ddlFDTCCContainerNo.SelectedItem.Text),
                        Convert.ToDecimal(txtFDTCCWtContainerWetSoilSample.Text),
                        Convert.ToDecimal(txtFDTCCWtofContainerDrySoil.Text),
                        Convert.ToDecimal(txtFDTCCWeightofContainer.Text),
                        Convert.ToDecimal(txtFDTCCMoistureContent.Text),
                        Convert.ToDecimal(txtFDTCCDryDensity.Text),
                        Convert.ToDecimal(txtFDTCCCompaction.Text), strResult, false);
                    }
                    int testSrNo = 11;

                    var test = dc.Test(testSrNo, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }

                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtFDTCCRows.Text), true);
                }
                #endregion
                //Inward table data                
                var sampleTest = dc.SoilSampleTest_View(txtRefNo.Text, txtSampleName.Text);
                foreach (var smpl in sampleTest)
                {
                    if (smpl.SOSMPLTEST_Status_tint == 0 && reportStatus >= 0)
                        reportStatus = 1;
                    else if (smpl.SOSMPLTEST_Status_tint == 1 && reportStatus >= 1)
                        reportStatus = 2;
                    else if (smpl.SOSMPLTEST_Status_tint == 2 && reportStatus >= 2)
                        reportStatus = 3;
                }
                dc.SoilInward_Update_ReportData(reportStatus, txtRefNo.Text, txtWitnesBy.Text, 0, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, testingDate,null);
                //remark update
                dc.SoilRemarkDetail_Update(0, txtRefNo.Text, true);
                string remId = "";
                for (int i = 0; i <= grdRemark.Rows.Count - 1; i++)
                {
                    TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                    if (txtRemark.Text != "")
                    {
                        dc.SoilRemark_View(txtRemark.Text, ref remId);
                        if (remId == "" || remId == null)
                        {
                            remId = dc.SoilRemark_Update(txtRemark.Text).ToString();
                        }
                        dc.SoilRemarkDetail_Update(Convert.ToInt32(remId), txtRefNo.Text, false);
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);
                lnkPrint.Visible = true;
                lnkSave.Enabled = false;
                lnkCalculate.Enabled = false;
            }
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Soil_Report_Sample.aspx");
            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            string strURLWithData = "Soil_Report_Sample.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "SO", txtReportNo.Text, txtRefNo.Text, lblStatus.Text));
            Response.Redirect(strURLWithData);
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.Soil_PDFReport(txtRefNo.Text, txtSampleName.Text, lblStatus.Text);
            rpt.PrintSelectedReport(txtRecordType.Text, txtRefNo.Text, lblStatus.Text, "", "", "", "", "", txtSampleName.Text, "");
        }
    }
}