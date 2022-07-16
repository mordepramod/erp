using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace DESPLWEB
{
    public partial class Soil_Report_CBR : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        string reportCBRGradValues = "";
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
               // txtRefNo.Text = Session["ReferenceNo"].ToString();
               // txtSampleName.Text = Session["SoilSampleName"].ToString();

                if (lblStatus.Text == "Enter")
                {
                   // lblStatus.Text = "Enter";
                    lblheading.Text = "Soil - Report Entry";
                }
                else if (lblStatus.Text == "Check")
                {
                   // lblStatus.Text = "Check";
                    lblheading.Text = "Soil - Report Checking";
                }

                LoadApprovedBy();
                DisplaySoilDetails();
                loadRemark();
                lnkPrint.Visible = false;
                lnkSave.Enabled = true;
                lnkCalculate.Enabled = true;
            }
        }
        protected void cbSelect_CheckedChanged(object sender, EventArgs e)
        {
            int mouldNo = 0, j = 0;
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            j = gvr.RowIndex;
            for (int i = 0; i < grdMould.Rows.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)grdMould.Rows[i].FindControl("cbSelect");
                DropDownList ddlMouldNo = (DropDownList)grdMould.Rows[i].FindControl("ddlMouldNo");

                if (cbSelect.Checked == true)
                {
                    if (ddlMouldNo.SelectedItem.Text == "Select")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Select Mould No.');", true);
                        cbSelect.Checked = false;
                        break;
                    }
                    else
                    {
                        if (i==j)
                            mouldNo = Convert.ToInt32(ddlMouldNo.SelectedItem.Text);
                    }
                }
                if (i != j)
                {
                    cbSelect.Checked = false;
                }
            }
            if (mouldNo != 0)
            {
                DisplayMouldDetails(mouldNo);
            }
            else
            {
                clearData();
            }
        }
        private void clearData()
        {
            grdDensity.Enabled = false;
            grdWaterContent.Enabled = false;
            grdPenetration.Enabled = false;
            ddlRingSize.Enabled = false;
            txtMouldingDate.Enabled = false;
            txtPenetrationDate.Enabled = false;
            txtASPGCBRPent25.Enabled = false;
            txtASPGCBRPent50.Enabled = false;
 
            for (int i = 0; i < 8; i++)
            {
                TextBox txtConditionSpecimen = (TextBox)grdDensity.Rows[i].Cells[0].FindControl("txtConditionSpecimen");
                TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[i].Cells[1].FindControl("txtBeforeSoaking");
                TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[i].Cells[2].FindControl("txtAfterSoaking");

                txtBeforeSoaking.Text = "";
                txtAfterSoaking.Text = "";
            }

            for (int i = 0; i < 7; i++)
            {
                TextBox txtgrdWCCol1 = (TextBox)grdWaterContent.Rows[i].Cells[0].FindControl("txtgrdWCCol1");
                TextBox txtBeforeCompaction = (TextBox)grdWaterContent.Rows[i].Cells[1].FindControl("txtBeforeCompaction");
                TextBox txtAfterCompaction = (TextBox)grdWaterContent.Rows[i].Cells[2].FindControl("txtAfterCompaction");
                TextBox txtTop30mm = (TextBox)grdWaterContent.Rows[i].Cells[3].FindControl("txtTop30mm");
                TextBox txtBulk = (TextBox)grdWaterContent.Rows[i].Cells[4].FindControl("txtBulk");

                DropDownList ddlBeforeCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlBeforeCompaction");
                DropDownList ddlAfterCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlAfterCompaction");
                DropDownList ddlTop30mm = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlTop30mm");
                DropDownList ddlBulk = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlBulk");

                ddlBeforeCompaction.SelectedValue = "0";
                ddlAfterCompaction.SelectedValue = "0";
                ddlTop30mm.SelectedValue = "0";
                ddlBulk.SelectedValue = "0";

                txtBeforeCompaction.Text = "";
                txtAfterCompaction.Text = "";
                txtTop30mm.Text = "";
                txtBulk.Text = "";

                if (i > 2)
                {
                    txtBeforeCompaction.BackColor = System.Drawing.Color.LightGray;
                    txtAfterCompaction.BackColor = System.Drawing.Color.LightGray;
                    txtTop30mm.BackColor = System.Drawing.Color.LightGray;
                    txtBulk.BackColor = System.Drawing.Color.LightGray;
                    txtBeforeCompaction.ReadOnly = true;
                    txtAfterCompaction.ReadOnly = true;
                    txtTop30mm.ReadOnly = true;
                    txtBulk.ReadOnly = true;
                }
                else if (i > 0)
                {
                    ddlBeforeCompaction.Visible = false;
                    ddlAfterCompaction.Visible = false;
                    ddlTop30mm.Visible = false;
                    ddlBulk.Visible = false;
                }
                else if (i == 0)
                {
                    txtBeforeCompaction.Visible = false;
                    txtAfterCompaction.Visible = false;
                    txtTop30mm.Visible = false;
                    txtBulk.Visible = false;
                }
            }

            for (int i = 0; i < 12; i++)
            {
                DropDownList ddlLoadDialReading = (DropDownList)grdPenetration.Rows[i].Cells[1].FindControl("ddlLoadDialReading");
                TextBox txtCorrectedloadfromChart = (TextBox)grdPenetration.Rows[i].Cells[2].FindControl("txtCorrectedloadfromChart");
                TextBox txtCBR = (TextBox)grdPenetration.Rows[i].Cells[3].FindControl("txtCBR");

                ddlLoadDialReading.SelectedValue = "0";
                txtCorrectedloadfromChart.Text = "";
                txtCBR.Text = "";
            }

            txtCalCBRPent25.Text = "";
            txtASPGCBRPent25.Text = "";
            txtCalCBRPent50.Text = "";
            txtASPGCBRPent50.Text = "";
            txtMouldingDate.Text = "";
            txtPenetrationDate.Text = "";
            ddlRingSize.SelectedIndex = 0;
        }
        private void DisplaySoilDetails()
        {
            grdDensity.Enabled = false;
            grdWaterContent.Enabled = false;
            grdPenetration.Enabled = false;
            ddlRingSize.Enabled = false;
            txtMouldingDate.Enabled = false;
            txtPenetrationDate.Enabled = false;
            txtASPGCBRPent25.Enabled = false;
            txtASPGCBRPent50.Enabled = false;

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
                try
                {
                    if (soinwd.SOINWD_ReportDetails_var != "" && soinwd.SOINWD_ReportDetails_var != null)
                    {
                        string[] arryReportDetails = soinwd.SOINWD_ReportDetails_var.Split(',');
                        string[] arryFinalCBR = arryReportDetails[1].Split('=');
                        txtFinalCBRSample.Text = arryFinalCBR[1];
                    }
                }
                catch { }


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
            var test = dc.SoilSampleTest_View(txtRefNo.Text, txtSampleName.Text);
            int rowNo = 0;
            foreach (var sotest in test)
            {
                if (sotest.TEST_Sr_No == 8 || sotest.TEST_Sr_No == 9)
                {
                    if (sotest.TEST_Sr_No == 8)
                    {
                        txtCheckSoak.Text = "Soaked";
                        lblCBRSoaked.Text = "CBR-Soaked";
                        grdDensity.Columns[2].Visible = true;
                        grdWaterContent.Columns[3].Visible = true;
                        grdWaterContent.Columns[4].Visible = true;
                    }
                    else if (sotest.TEST_Sr_No == 9)
                    {
                        txtCheckSoak.Text = "Unsoaked";
                        lblCBRSoaked.Text = "CBR-Unsoaked";
                        grdDensity.Columns[2].Visible = false;
                        grdWaterContent.Columns[3].Visible = false;
                        grdWaterContent.Columns[4].Visible = false;
                    }

                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlCBR.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlCBR.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabCBR.HeaderText = TabCBR.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabCBR.HeaderText = TabCBR.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabCBR.HeaderText = TabCBR.HeaderText + "(Checked)";

                    var cbrData = dc.SoilCBR_View(txtRefNo.Text, txtSampleName.Text, Convert.ToInt32(sotest.TEST_Sr_No), 0).ToList();
                    rowNo = 0;
                    txtMouldsCasted.Text = cbrData.Count.ToString();
                    foreach (var mouldData in cbrData)
                    {
                        AddRowMould();
                        DropDownList ddlMouldNo = (DropDownList)grdMould.Rows[rowNo].FindControl("ddlMouldNo");
                        ddlMouldNo.SelectedValue = mouldData.SOCBR_MouldNo_int.ToString();
                        rowNo++;
                    }

                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtCBRDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtCBRDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtCBRDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtCBRDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    string[] arryForTest = new string[8];
                    arryForTest[0] = "Wt. of Compacted Sample, Mould and Base Plate";
                    arryForTest[1] = "Wt. of Mould and Base Plate, g";
                    arryForTest[2] = "Wt. of Sample, g";
                    arryForTest[3] = "Height of Specimen, cm";
                    arryForTest[4] = "Vol. of Specimen, cm3";
                    arryForTest[5] = "Wet Density, g/cm3";
                    arryForTest[6] = "Moisture Content, %";
                    arryForTest[7] = "Dry Density, g/cm3";

                    DataTable dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("txtConditionSpecimen", typeof(string)));
                    dt1.Columns.Add(new DataColumn("txtBeforeSoaking", typeof(string)));
                    dt1.Columns.Add(new DataColumn("txtAfterSoaking", typeof(string)));

                    for (int i = 0; i < 8; i++)
                    {
                        dr1 = dt1.NewRow();
                        dr1["txtConditionSpecimen"] = string.Empty;
                        dr1["txtBeforeSoaking"] = string.Empty;
                        dr1["txtAfterSoaking"] = string.Empty;
                        dt1.Rows.Add(dr1);
                    }

                    ViewState["DensityTable"] = dt1;
                    grdDensity.DataSource = dt1;
                    grdDensity.DataBind();

                    for (int i = 0; i < 8; i++)
                    {
                        TextBox txtConditionSpecimen = (TextBox)grdDensity.Rows[i].Cells[0].FindControl("txtConditionSpecimen");
                        TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[i].Cells[1].FindControl("txtBeforeSoaking");
                        TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[i].Cells[2].FindControl("txtAfterSoaking");
                        txtConditionSpecimen.Text = arryForTest[i];
                        if (i > 1)
                        {
                            txtBeforeSoaking.BackColor = System.Drawing.Color.LightGray;
                            txtAfterSoaking.BackColor = System.Drawing.Color.LightGray;
                            txtAfterSoaking.ReadOnly = true;
                            txtBeforeSoaking.ReadOnly = true;
                        }
                    }
                    
                    string[] arryForTestWC = new string[7];
                    arryForTestWC[0] = "Container no.";
                    arryForTestWC[1] = "Mass of wet sample + container";
                    arryForTestWC[2] = "Mass of dry sample + container";
                    arryForTestWC[3] = "Mass of container";
                    arryForTestWC[4] = "Mass of moisture";
                    arryForTestWC[5] = "Mass of dry sample";
                    arryForTestWC[6] = "% Moisture";
                    
                    ///
                    DataTable dtWC = new DataTable();
                    DataRow drWC = null;
                    dtWC.Columns.Add(new DataColumn("txtgrdWCCol1", typeof(string)));
                    dtWC.Columns.Add(new DataColumn("txtBeforeCompaction", typeof(string)));
                    dtWC.Columns.Add(new DataColumn("txtAfterCompaction", typeof(string)));
                    dtWC.Columns.Add(new DataColumn("txtTop30mm", typeof(string)));
                    dtWC.Columns.Add(new DataColumn("txtBulk", typeof(string)));
                    
                    for (int i = 0; i < 7; i++)
                    {
                        drWC = dtWC.NewRow();
                        drWC["txtgrdWCCol1"] = string.Empty;
                        drWC["txtBeforeCompaction"] = string.Empty;
                        drWC["txtAfterCompaction"] = string.Empty;
                        drWC["txtTop30mm"] = string.Empty;
                        drWC["txtBulk"] = string.Empty;
                        dtWC.Rows.Add(drWC);
                    }

                    ViewState["WaterContentTable"] = dtWC;
                    grdWaterContent.DataSource = dtWC;
                    grdWaterContent.DataBind();

                    for (int i = 0; i < 7; i++)
                    {
                        TextBox txtgrdWCCol1 = (TextBox)grdWaterContent.Rows[i].Cells[0].FindControl("txtgrdWCCol1");
                        TextBox txtBeforeCompaction = (TextBox)grdWaterContent.Rows[i].Cells[1].FindControl("txtBeforeCompaction");
                        TextBox txtAfterCompaction = (TextBox)grdWaterContent.Rows[i].Cells[2].FindControl("txtAfterCompaction");
                        TextBox txtTop30mm = (TextBox)grdWaterContent.Rows[i].Cells[3].FindControl("txtTop30mm");
                        TextBox txtBulk = (TextBox)grdWaterContent.Rows[i].Cells[4].FindControl("txtBulk");

                        DropDownList ddlBeforeCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlBeforeCompaction");
                        DropDownList ddlAfterCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlAfterCompaction");
                        DropDownList ddlTop30mm = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlTop30mm");
                        DropDownList ddlBulk = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlBulk");

                        txtgrdWCCol1.Text = arryForTestWC[i];
                        if (i > 2)
                        {
                            txtBeforeCompaction.BackColor = System.Drawing.Color.LightGray;
                            txtAfterCompaction.BackColor = System.Drawing.Color.LightGray;
                            txtTop30mm.BackColor = System.Drawing.Color.LightGray;
                            txtBulk.BackColor = System.Drawing.Color.LightGray;
                            txtBeforeCompaction.ReadOnly = true;
                            txtAfterCompaction.ReadOnly = true;
                            txtTop30mm.ReadOnly = true;
                            txtBulk.ReadOnly = true;
                        }
                        if (i > 0)
                        {
                            ddlBeforeCompaction.Visible = false;
                            ddlAfterCompaction.Visible = false;
                            ddlTop30mm.Visible = false;
                            ddlBulk.Visible = false;
                        }
                        if (i == 0)
                        {
                            txtBeforeCompaction.Visible = false;
                            txtAfterCompaction.Visible = false;
                            txtTop30mm.Visible = false;
                            txtBulk.Visible = false;
                        }

                    }

                    rowNo = 0;
                    string[] arryForPenetration = new string[12];
                    arryForPenetration[0] = "0.5 mm";
                    arryForPenetration[1] = "1.0 mm";
                    arryForPenetration[2] = "1.5 mm";
                    arryForPenetration[3] = "2.0 mm";
                    arryForPenetration[4] = "2.5 mm";
                    arryForPenetration[5] = "3.0 mm";
                    arryForPenetration[6] = "4.0 mm";
                    arryForPenetration[7] = "5.0 mm";
                    arryForPenetration[8] = "6.0 mm";
                    arryForPenetration[9] = "7.5 mm";
                    arryForPenetration[10] = "10.0 mm";
                    arryForPenetration[11] = "12.5 mm";

                    DataTable dtPenetration = new DataTable();
                    DataRow drPenetration = null;
                    dtPenetration.Columns.Add(new DataColumn("txtPenetration", typeof(string)));
                    dtPenetration.Columns.Add(new DataColumn("txtLoadDialReading", typeof(string)));
                    dtPenetration.Columns.Add(new DataColumn("txtCorrectedloadfromChart", typeof(string)));
                    dtPenetration.Columns.Add(new DataColumn("txtCBR", typeof(string)));

                    for (int i = 0; i < 12; i++)
                    {
                        drPenetration = dtPenetration.NewRow();
                        drPenetration["txtPenetration"] = string.Empty;
                        drPenetration["txtLoadDialReading"] = string.Empty;
                        drPenetration["txtCorrectedloadfromChart"] = string.Empty;
                        drPenetration["txtCBR"] = string.Empty;
                        dtPenetration.Rows.Add(drPenetration);
                    }

                    ViewState["PenetrationTable"] = dtPenetration;
                    grdPenetration.DataSource = dtPenetration;
                    grdPenetration.DataBind();

                    for (int i = 0; i < 12; i++)
                    {
                        TextBox txtPenetration = (TextBox)grdPenetration.Rows[i].Cells[0].FindControl("txtPenetration");
                        txtPenetration.Text = arryForPenetration[i];
                    }                    
                }
            }
        }

        private void DisplayMouldDetails(int mouldNo)
        {
            grdDensity.Enabled = true;
            grdWaterContent.Enabled = true;
            grdPenetration.Enabled = true;
            ddlRingSize.Enabled = true;
            txtMouldingDate.Enabled = true;
            txtPenetrationDate.Enabled = true;
            txtASPGCBRPent25.Enabled = true;
            txtASPGCBRPent50.Enabled = true;

            int srNo = 0, rowNo=0;
            if (txtCheckSoak.Text == "Soaked")
                srNo = 8;
            else
                srNo = 9;

            var cbrData = dc.SoilCBR_View(txtRefNo.Text, txtSampleName.Text, srNo, mouldNo).ToList();
            foreach (var grdData in cbrData)
            {
                ddlRingSize.SelectedValue = grdData.SOCBR_ProvingRingSize_var;
                Load_LoadDialReading();
                string[] arryDensity = grdData.SOCBR_DensityData_var.Split('~');
                for (int i = 0; i < grdDensity.Rows.Count; i++)
                {
                    TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[rowNo].Cells[1].FindControl("txtBeforeSoaking");
                    TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[rowNo].Cells[2].FindControl("txtAfterSoaking");
                    string[] arryDensityDetails = arryDensity[i].Split('|');
                    txtBeforeSoaking.Text = arryDensityDetails[0];
                    txtAfterSoaking.Text = arryDensityDetails[1];
                    rowNo++;
                }
            

                rowNo = 0;
            
                string[] arryWaterContent = grdData.SOCBR_WaterContentData_var.Split('~');
                for (int i = 0; i < grdWaterContent.Rows.Count; i++)
                {
                    TextBox txtBeforeCompaction = (TextBox)grdWaterContent.Rows[rowNo].Cells[1].FindControl("txtBeforeCompaction");
                    TextBox txtAfterCompaction = (TextBox)grdWaterContent.Rows[rowNo].Cells[2].FindControl("txtAfterCompaction");
                    TextBox txtTop30mm = (TextBox)grdWaterContent.Rows[rowNo].Cells[3].FindControl("txtTop30mm");
                    TextBox txtBulk = (TextBox)grdWaterContent.Rows[rowNo].Cells[4].FindControl("txtBulk");
                    string[] arryWCDetails = arryWaterContent[i].Split('|');
                    if (i == 0)
                    {
                        DropDownList ddlBeforeCompaction = (DropDownList)grdWaterContent.Rows[rowNo].Cells[1].FindControl("ddlBeforeCompaction");
                        DropDownList ddlAfterCompaction = (DropDownList)grdWaterContent.Rows[rowNo].Cells[1].FindControl("ddlAfterCompaction");
                        DropDownList ddlTop30mm = (DropDownList)grdWaterContent.Rows[rowNo].Cells[1].FindControl("ddlTop30mm");
                        DropDownList ddlBulk = (DropDownList)grdWaterContent.Rows[rowNo].Cells[1].FindControl("ddlBulk");
                        string[] arryWCDetails1 = arryWaterContent[3].Split('|');
                        ddlBeforeCompaction.SelectedValue = arryWCDetails[0] + "|" + arryWCDetails1[0];
                        ddlAfterCompaction.SelectedValue = arryWCDetails[1] + "|" + arryWCDetails1[1];
                        if (txtCheckSoak.Text == "Soaked")
                        {
                            ddlTop30mm.SelectedValue = arryWCDetails[2] + "|" + arryWCDetails1[2];
                            ddlBulk.SelectedValue = arryWCDetails[3] + "|" + arryWCDetails1[3];
                        }
                    }
                    else
                    {
                        txtBeforeCompaction.Text = arryWCDetails[0];
                        txtAfterCompaction.Text = arryWCDetails[1];
                        if (txtCheckSoak.Text == "Soaked")
                        {
                            txtTop30mm.Text = arryWCDetails[2];
                            txtBulk.Text = arryWCDetails[3];
                        }
                    }
                    rowNo++;
                }
            

                rowNo = 0;
            
                string[] arryPenetration = grdData.SOCBR_PenetrationData_var.Split('~');
                for (int i = 0; i < grdPenetration.Rows.Count; i++)
                {
                    DropDownList ddlLoadDialReading = (DropDownList)grdPenetration.Rows[rowNo].Cells[1].FindControl("ddlLoadDialReading");
                    TextBox txtCorrectedloadfromChart = (TextBox)grdPenetration.Rows[rowNo].Cells[2].FindControl("txtCorrectedloadfromChart");
                    TextBox txtCBR = (TextBox)grdPenetration.Rows[rowNo].Cells[3].FindControl("txtCBR");

                    string[] arryPenetrationDetails = arryPenetration[i].Split('|');
                    ddlLoadDialReading.SelectedValue = arryPenetrationDetails[0];
                    //ddlLoadDialReading.SelectedValue = arryPenetration[i];
                    txtCorrectedloadfromChart.Text = arryPenetrationDetails[1];
                    txtCBR.Text = arryPenetrationDetails[2];
                    rowNo++;
                }
            

            
                string[] arryforCBR = grdData.SOCBR_CBR_var.Split('|');
                txtCalCBRPent25.Text = arryforCBR[0];
                txtASPGCBRPent25.Text = arryforCBR[1];
                txtCalCBRPent50.Text = arryforCBR[2];
                txtASPGCBRPent50.Text = arryforCBR[3];
                txtMouldingDate.Text = Convert.ToDateTime(grdData.SOCBR_MouldingDate_dt).ToString("dd/MM/yyyy"); // grdData.SOCBR_MouldingDate_dt.ToString();
                txtPenetrationDate.Text = Convert.ToDateTime(grdData.SOCBR_PenetrationDate_dt).ToString("dd/MM/yyyy");//grdData.SOCBR_PenetrationDate_dt.ToString();
            }

        }
        private void loadRemark()
        {
            ////Remark details
            int rowNo = 0;
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


        #region add/delete rows grdMouldNo grid
        protected void AddRowMould()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MouldTable"] != null)
            {
                GetCurrentDataMould();
                dt = (DataTable)ViewState["MouldTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddlMouldNo", typeof(string)));
                dt.Columns.Add(new DataColumn("cbSelect", typeof(string)));                
            }

            dr = dt.NewRow();
            dr["ddlMouldNo"] = string.Empty;
            dr["cbSelect"] = "False";
            dt.Rows.Add(dr);

            ViewState["MouldTable"] = dt;
            grdMould.DataSource = dt;
            grdMould.DataBind();
            SetPreviousDataMould();
        }
        protected void DeleteRowMould(int rowIndex)
        {
            GetCurrentDataMould();
            DataTable dt = ViewState["MouldTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MouldTable"] = dt;
            grdMould.DataSource = dt;
            grdMould.DataBind();
            SetPreviousDataMould();
        }
        protected void SetPreviousDataMould()
        {
            DataTable dt = (DataTable)ViewState["MouldTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlMouldNo = (DropDownList)grdMould.Rows[i].FindControl("ddlMouldNo");
                CheckBox cbSelect = (CheckBox)grdMould.Rows[i].FindControl("cbSelect");
                ddlMouldNo.SelectedValue = dt.Rows[i]["ddlMouldNo"].ToString();
                cbSelect.Checked = Convert.ToBoolean(dt.Rows[i]["cbSelect"].ToString());
            }
        }
        protected void GetCurrentDataMould()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ddlMouldNo", typeof(string)));
            dt.Columns.Add(new DataColumn("cbSelect", typeof(string)));
            
            for (int i = 0; i < grdMould.Rows.Count; i++)
            {
                DropDownList ddlMouldNo = (DropDownList)grdMould.Rows[i].FindControl("ddlMouldNo");
                CheckBox cbSelect = (CheckBox)grdMould.Rows[i].FindControl("cbSelect");

                dr = dt.NewRow();
                dr["ddlMouldNo"] = ddlMouldNo.SelectedValue;
                dr["cbSelect"] = cbSelect.Checked;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["MouldTable"] = dt;
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


        private void MouldRowsChanged()
        {
            if (txtMouldsCasted.Text != "")
            {
                if (Convert.ToInt32(txtMouldsCasted.Text) < grdMould.Rows.Count)
                {
                    for (int i = grdMould.Rows.Count; i > Convert.ToInt32(txtMouldsCasted.Text); i--)
                    {
                        if (grdMould.Rows.Count > 1)
                        {
                            DeleteRowMould(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtMouldsCasted.Text) > grdMould.Rows.Count)
                {
                    for (int i = grdMould.Rows.Count + 1; i <= Convert.ToInt32(txtMouldsCasted.Text); i++)
                    {
                        AddRowMould();
                    }
                }
            }
        }

        protected void txtMouldsCasted_TextChanged(object sender, EventArgs e)
        {
            MouldRowsChanged();
        }

        protected void grdPenetration_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Test No.";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 3;
                HeaderGridRow.Cells.Add(HeaderCell);

                grdPenetration.Controls[0].Controls.AddAt(0, HeaderGridRow);

            }
        }

        protected void grdWaterContent_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (txtCheckSoak.Text == "CBR Soaked")
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "After Soaking";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    grdWaterContent.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }

            }
        }

        protected void grdMould_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlMouldNo = (e.Row.FindControl("ddlMouldNo") as DropDownList);
                var soset = dc.SoilSetting_View("CBR Modi Mould Volume").ToList();
                ddlMouldNo.DataSource = soset;
                ddlMouldNo.DataTextField = "SOSET_F1_var";
                ddlMouldNo.DataBind();
                if (ddlMouldNo.Items.Count > 0)
                    ddlMouldNo.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        protected void grdPenetration_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlLoadDialReading = (e.Row.FindControl("ddlLoadDialReading") as DropDownList);
                var soset = dc.SoilSetting_View("CBR Proving Ring" + "-" + ddlRingSize.SelectedValue).ToList();
                ddlLoadDialReading.DataSource = soset;
                ddlLoadDialReading.DataTextField = "SOSET_F1_var";
                ddlLoadDialReading.DataBind();
                if (ddlLoadDialReading.Items.Count > 0)
                    ddlLoadDialReading.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        protected void ddlLoadDialReading_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            //if (gvr != null)
            //{
            //    TextBox txtCorrectedloadfromChart = (TextBox)gvr.FindControl("txtCorrectedloadfromChart");
            //    DropDownList ddlLoadDialReading = (DropDownList)gvr.FindControl("ddlLoadDialReading");

            //    if (ddlLoadDialReading.SelectedIndex > 0)
            //    {
            //        txtCorrectedloadfromChart.Text = ddlLoadDialReading.SelectedItem.Value;
            //        txtCorrectedloadfromChart.Focus();

            //    }
            //}
        }


        protected void grdWaterContent_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlBeforeCompaction = (e.Row.FindControl("ddlBeforeCompaction") as DropDownList);
                DropDownList ddlAfterCompaction = (e.Row.FindControl("ddlAfterCompaction") as DropDownList);
                DropDownList ddlTop30mm = (e.Row.FindControl("ddlTop30mm") as DropDownList);
                DropDownList ddlBulk = (e.Row.FindControl("ddlBulk") as DropDownList);


                var soset = dc.SoilSetting_View("Weight Of Container");
                ddlBeforeCompaction.DataSource = soset;
                ddlBeforeCompaction.DataTextField = "SOSET_F1_var";
                ddlBeforeCompaction.DataValueField = "F1plusF2";
                ddlBeforeCompaction.DataBind();
                if (ddlBeforeCompaction.Items.Count > 0)
                    ddlBeforeCompaction.Items.Insert(0, new ListItem("Select", "0"));

                ddlAfterCompaction.DataSource = soset;
                ddlAfterCompaction.DataTextField = "SOSET_F1_var";
                ddlAfterCompaction.DataValueField = "F1plusF2";
                ddlAfterCompaction.DataBind();
                if (ddlAfterCompaction.Items.Count > 0)
                    ddlAfterCompaction.Items.Insert(0, new ListItem("Select", "0"));

                ddlTop30mm.DataSource = soset;
                ddlTop30mm.DataTextField = "SOSET_F1_var";
                ddlTop30mm.DataValueField = "F1plusF2";
                ddlTop30mm.DataBind();
                if (ddlTop30mm.Items.Count > 0)
                    ddlTop30mm.Items.Insert(0, new ListItem("Select", "0"));

                ddlBulk.DataSource = soset;
                ddlBulk.DataTextField = "SOSET_F1_var";
                ddlBulk.DataValueField = "F1plusF2";
                ddlBulk.DataBind();
                if (ddlBulk.Items.Count > 0)
                    ddlBulk.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        protected void ddlBeforeCompaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            //if (gvr != null)
            //{
            //    TextBox txtBeforeCompaction = (TextBox)grdWaterContent.Rows[3].FindControl("txtBeforeCompaction");
            //    DropDownList ddlBeforeCompaction = (DropDownList)gvr.FindControl("ddlBeforeCompaction");

            //    if (ddlBeforeCompaction.SelectedIndex > 0)
            //    {
            //        txtBeforeCompaction.Text = ddlBeforeCompaction.SelectedItem.Value;
            //        txtBeforeCompaction.Focus();

            //    }
            //}
        }

        protected void ddlAfterCompaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            //if (gvr != null)
            //{
            //    TextBox txtAfterCompaction = (TextBox)grdWaterContent.Rows[3].FindControl("txtAfterCompaction");
            //    DropDownList ddlAfterCompaction = (DropDownList)gvr.FindControl("ddlAfterCompaction");

            //    if (ddlAfterCompaction.SelectedIndex > 0)
            //    {
            //        txtAfterCompaction.Text = ddlAfterCompaction.SelectedItem.Value;
            //        txtAfterCompaction.Focus();
            //    }
            //}
        }


        protected void ddlTop30mm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            //if (gvr != null)
            //{
            //    TextBox txtTop30mm = (TextBox)grdWaterContent.Rows[3].FindControl("txtTop30mm");
            //    DropDownList ddlTop30mm = (DropDownList)gvr.FindControl("ddlTop30mm");

            //    if (ddlTop30mm.SelectedIndex > 0)
            //    {
            //        txtTop30mm.Text = ddlTop30mm.SelectedItem.Value;
            //        txtTop30mm.Focus();
            //    }
            //}
        }

        protected void ddlBulk_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            //if (gvr != null)
            //{
            //    TextBox txtBulk = (TextBox)grdWaterContent.Rows[3].FindControl("txtBulk");
            //    DropDownList ddlBulk = (DropDownList)gvr.FindControl("ddlBulk");

            //    if (ddlBulk.SelectedIndex > 0)
            //    {
            //        txtBulk.Text = ddlBulk.SelectedItem.Value;
            //        txtBulk.Focus();
            //    }
            //}
        }

        protected void ddlMouldNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            //if (gvr != null)
            //{
            //    TextBox txtVolume = (TextBox)gvr.FindControl("txtVolume");
            //    TextBox txtHeight = (TextBox)gvr.FindControl("txtHeight");
            //    DropDownList ddlMouldNo = (DropDownList)gvr.FindControl("ddlMouldNo");

            //    if (ddlMouldNo.SelectedIndex > 0)
            //    {
            //        string[] strMddNo;
            //        strMddNo = ddlMouldNo.SelectedItem.Value.Split('|');
            //        txtVolume.Text = strMddNo[0];
            //        txtHeight.Text = strMddNo[1];
            //        ddlMouldNo.Focus();
            //    }
            //}
        }
        protected void ddlRingSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_LoadDialReading();
        }
        protected void Load_LoadDialReading()
        {
            for (int i = 0; i < grdPenetration.Rows.Count; i++)
            {
                DropDownList ddlLoadDialReading = (DropDownList)grdPenetration.Rows[i].Cells[1].FindControl("ddlLoadDialReading");
                TextBox txtCorrectedloadfromChart = (TextBox)grdPenetration.Rows[i].Cells[2].FindControl("txtCorrectedloadfromChart");
                TextBox txtCBR = (TextBox)grdPenetration.Rows[i].Cells[3].FindControl("txtCBR");

                ddlLoadDialReading.DataSource = null;
                ddlLoadDialReading.DataBind();
                ddlLoadDialReading.Items.Clear();
                txtCorrectedloadfromChart.Text = "";
                txtCBR.Text = "";
                var soset = dc.SoilSetting_View("CBR Proving Ring" + "-" + ddlRingSize.SelectedValue).ToList();
                ddlLoadDialReading.DataSource = soset;
                ddlLoadDialReading.DataTextField = "SOSET_F1_var";
                ddlLoadDialReading.DataBind();
                if (ddlLoadDialReading.Items.Count > 0)
                    ddlLoadDialReading.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                CalculateCBR();
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

                //reportStatus = 1;
                DateTime testingDate = new DateTime();
                if (TabCBR.Visible == true)
                {
                    testingDate = DateTime.ParseExact(txtCBRDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                #region save CBR Soil report data
                if (TabCBR.Visible == true)
                {

                    string densityData = "", waterContentData = "", PenetrationData = ""; //strResult = ""
                    DateTime date1, date2;
                    int mouldNo = 0;
                    int testSrNo = 0;
                    if (lblCBRSoaked.Text == "CBR-Soaked")
                    {
                        testSrNo = 8;
                    }
                    else if (lblCBRSoaked.Text == "CBR-Unsoaked")
                    {
                        testSrNo = 9;
                    }

                    for (int i = 0; i < grdMould.Rows.Count; i++)
                    {
                        CheckBox cbSelect = (CheckBox)grdMould.Rows[i].FindControl("cbSelect");
                        DropDownList ddlMouldNo = (DropDownList)grdMould.Rows[i].FindControl("ddlMouldNo");
                        if (cbSelect.Checked == true)
                        {
                            if (ddlMouldNo.SelectedItem.Text == "Select")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Select Mould No.');", true);
                                cbSelect.Checked = false;
                                return;
                            }
                            else
                            {
                                mouldNo = Convert.ToInt32(ddlMouldNo.SelectedItem.Text);
                            }
                            break;
                        }
                    }
                    //dc.SoilCBR_Update(testSrNo, txtRefNo.Text, txtSampleName.Text, mouldNo, "", "", "", null, null, "", true);
                    for (int i = 0; i < grdDensity.Rows.Count; i++)
                    {
                        TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[i].Cells[1].FindControl("txtBeforeSoaking");
                        TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[i].Cells[2].FindControl("txtAfterSoaking");
                        densityData += txtBeforeSoaking.Text + "|" + txtAfterSoaking.Text + "~";
                    }
                    for (int i = 0; i < grdWaterContent.Rows.Count; i++)
                    {
                        TextBox txtBeforeCompaction = (TextBox)grdWaterContent.Rows[i].Cells[1].FindControl("txtBeforeCompaction");
                        TextBox txtAfterCompaction = (TextBox)grdWaterContent.Rows[i].Cells[2].FindControl("txtAfterCompaction");
                        TextBox txtTop30mm = (TextBox)grdWaterContent.Rows[i].Cells[3].FindControl("txtTop30mm");
                        TextBox txtBulk = (TextBox)grdWaterContent.Rows[i].Cells[4].FindControl("txtBulk");
                        if (i == 0)
                        {
                            DropDownList ddlBeforeCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlBeforeCompaction");
                            DropDownList ddlAfterCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlAfterCompaction");
                            DropDownList ddlTop30mm = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlTop30mm");
                            DropDownList ddlBulk = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlBulk");
                            if (txtCheckSoak.Text == "Soaked")
                            {
                                waterContentData += ddlBeforeCompaction.SelectedItem.Text + "|" + ddlAfterCompaction.SelectedItem.Text + "|" + ddlTop30mm.SelectedItem.Text + "|" + ddlBulk.SelectedItem.Text + "~";
                            }
                            else
                            {
                                waterContentData += ddlBeforeCompaction.SelectedItem.Text + "|" + ddlAfterCompaction.SelectedItem.Text + "||~";
                            }
                        }
                        else
                        {
                            waterContentData += txtBeforeCompaction.Text + "|" + txtAfterCompaction.Text + "|" + txtTop30mm.Text + "|" + txtBulk.Text + "~";
                        }
                    }

                    for (int i = 0; i < grdPenetration.Rows.Count; i++)
                    {
                        DropDownList ddlLoadDialReading = (DropDownList)grdPenetration.Rows[i].Cells[1].FindControl("ddlLoadDialReading");
                        TextBox txtCorrectedloadfromChart = (TextBox)grdPenetration.Rows[i].Cells[2].FindControl("txtCorrectedloadfromChart");
                        TextBox txtCBR = (TextBox)grdPenetration.Rows[i].Cells[3].FindControl("txtCBR");

                        PenetrationData += ddlLoadDialReading.SelectedItem.Text + "|" + txtCorrectedloadfromChart.Text + "|" + txtCBR.Text + "~";
                    }

                    string strCBRCalData = txtCalCBRPent25.Text + "|" + txtASPGCBRPent25.Text + "|" + txtCalCBRPent50.Text + "|" + txtASPGCBRPent50.Text;
                    date1 = DateTime.ParseExact(txtMouldingDate.Text, "dd/MM/yyyy", null);
                    date2 = DateTime.ParseExact(txtPenetrationDate.Text, "dd/MM/yyyy", null);
                    dc.SoilCBR_Update(testSrNo, txtRefNo.Text, txtSampleName.Text, mouldNo, densityData, waterContentData, PenetrationData, date1, date2, strCBRCalData, ddlRingSize.SelectedValue, false);


                    var test = dc.Test(testSrNo, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtMouldsCasted.Text), true);
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


                string reportsDetails = "";
                reportsDetails = ",Final CBR= " + txtFinalCBRSample.Text;
                var reportData = dc.SoilInward_View(txtRefNo.Text, 0);
                reportCBRGradValues = reportData.FirstOrDefault().SOINWD_ReportDetails_var;
                if (reportCBRGradValues != "" && reportCBRGradValues != null)
                {
                    if (reportCBRGradValues.Contains(","))
                    {
                        string[] FinalCBRGradArray = reportCBRGradValues.Split(',');
                        reportsDetails = FinalCBRGradArray[0] + reportsDetails;
                    }
                    else
                    {
                        reportsDetails = reportCBRGradValues + reportsDetails;
                    }
                }


                dc.SoilInward_Update_ReportData(reportStatus, txtRefNo.Text, txtWitnesBy.Text, 0, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, testingDate, reportsDetails);

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
                //if (lblStatus.Text == "Enter")
                //{
                //    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, true, false, false, false, false, false, false);
                //}
                //else if (lblStatus.Text == "Check")
                //{
                //    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, false, true, false, false, false, false, false);
                //}
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);

                lnkPrint.Visible = true;
                //lnkSave.Enabled = false;
                //lnkCalculate.Enabled = false;                
            }
        }
        protected Boolean ValidateCBR()
        {
            string dispalyMsg = "";
            Boolean valid = true;

            //validate CBR data
            #region validate CBR data
            if (TabCBR.Visible == true)
            {
                if (grdMould.Rows.Count == 0)
                {
                    dispalyMsg = "Enter No. of mould casted.";
                    txtMouldsCasted.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else
                //check mould no selected or not                
                {
                    valid = false;
                    for (int i = 0; i < grdMould.Rows.Count; i++)
                    {
                        CheckBox cbSelect = (CheckBox)grdMould.Rows[i].FindControl("cbSelect");
                        DropDownList ddlMouldNo = (DropDownList)grdMould.Rows[i].FindControl("ddlMouldNo");

                        if (cbSelect.Checked == true)
                        {
                            if (ddlMouldNo.SelectedItem.Text != "Select")
                            {
                                valid = true;
                                break;
                            }
                        }
                    }
                    if (valid == false)
                    {
                        dispalyMsg = "Select mould number.";
                        grdMould.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                }
                
                if (valid == true)
                {                    
                    if (txtMouldsCasted.Text == "")
                    {
                        dispalyMsg = "Enter Number of moulds casted";
                        txtMouldsCasted.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[i].Cells[1].FindControl("txtBeforeSoaking");
                            TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[i].Cells[2].FindControl("txtAfterSoaking");

                            if (txtBeforeSoaking.Text == "")
                            {
                                dispalyMsg = "Enter Before Soaking for row number " + (i + 1) + ".";
                                txtBeforeSoaking.Focus();
                                valid = false;
                                TabContainerSoil.ActiveTabIndex = 0;
                                break;
                            }
                            if (txtCheckSoak.Text == "Soaked")
                            {
                                if (txtAfterSoaking.Text == "")
                                {
                                    dispalyMsg = "Enter After Soaking for row number " + (i + 1) + ".";
                                    txtAfterSoaking.Focus();
                                    valid = false;
                                    TabContainerSoil.ActiveTabIndex = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        DropDownList ddlBeforeCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[1].FindControl("ddlBeforeCompaction");
                        DropDownList ddlAfterCompaction = (DropDownList)grdWaterContent.Rows[i].Cells[2].FindControl("ddlAfterCompaction");
                        DropDownList ddlTop30mm = (DropDownList)grdWaterContent.Rows[i].Cells[2].FindControl("ddlTop30mm");
                        DropDownList ddlBulk = (DropDownList)grdWaterContent.Rows[i].Cells[2].FindControl("ddlBulk");

                        TextBox txtBeforeCompaction = (TextBox)grdWaterContent.Rows[i].Cells[1].FindControl("txtBeforeCompaction");
                        TextBox txtAfterCompaction = (TextBox)grdWaterContent.Rows[i].Cells[2].FindControl("txtAfterCompaction");
                        TextBox txtTop30mm = (TextBox)grdWaterContent.Rows[i].Cells[3].FindControl("txtTop30mm");
                        TextBox txtBulk = (TextBox)grdWaterContent.Rows[i].Cells[4].FindControl("txtBulk");
                        if (i == 0)
                        {
                            if (ddlBeforeCompaction.SelectedItem.Text == "Select")
                            {
                                dispalyMsg = "Enter Container No. for Before Compaction for row number " + (i + 1) + ".";
                                ddlBeforeCompaction.Focus();
                                valid = false;
                                TabContainerSoil.ActiveTabIndex = 0;
                                break;
                            }
                            else if (ddlAfterCompaction.SelectedItem.Text == "Select")
                            {
                                dispalyMsg = "Enter Container No. for After Compaction for row number " + (i + 1) + ".";
                                ddlAfterCompaction.Focus();
                                valid = false;
                                TabContainerSoil.ActiveTabIndex = 0;
                                break;
                            }
                            if (txtCheckSoak.Text == "Soaked")
                            {
                                if (ddlTop30mm.SelectedItem.Text == "Select")
                                {
                                    dispalyMsg = "Enter Container No. for Top 30mm Compaction for row number " + (i + 1) + ".";
                                    ddlTop30mm.Focus();
                                    valid = false;
                                    TabContainerSoil.ActiveTabIndex = 0;
                                    break;
                                }
                                else if (ddlBulk.SelectedItem.Text == "Select")
                                {
                                    dispalyMsg = "Enter Container No. for Bulk for row number " + (i + 1) + ".";
                                    ddlBulk.Focus();
                                    valid = false;
                                    TabContainerSoil.ActiveTabIndex = 0;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (txtBeforeCompaction.Text == "")
                            {
                                dispalyMsg = "Enter Before Compaction for row number " + (i + 1) + ".";
                                txtBeforeCompaction.Focus();
                                valid = false;
                                TabContainerSoil.ActiveTabIndex = 0;
                                break;
                            }
                            if (txtAfterCompaction.Text == "")
                            {
                                dispalyMsg = "Enter  After Compaction for row number " + (i + 1) + ".";
                                txtAfterCompaction.Focus();
                                valid = false;
                                TabContainerSoil.ActiveTabIndex = 0;
                                break;
                            }
                            if (txtCheckSoak.Text == "Soaked")
                            {
                                if (txtTop30mm.Text == "")
                                {
                                    dispalyMsg = "Enter Top 30 mm for row number " + (i + 1) + ".";
                                    txtTop30mm.Focus();
                                    valid = false;
                                    TabContainerSoil.ActiveTabIndex = 0;
                                    break;
                                }

                                if (txtBulk.Text == "")
                                {
                                    dispalyMsg = "Enter Bulk for row number " + (i + 1) + ".";
                                    txtBulk.Focus();
                                    valid = false;
                                    TabContainerSoil.ActiveTabIndex = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DropDownList ddlLoadDialReading = (DropDownList)grdPenetration.Rows[i].Cells[1].FindControl("ddlLoadDialReading");
                        TextBox txtCorrectedloadfromChart = (TextBox)grdPenetration.Rows[i].Cells[2].FindControl("txtCorrectedloadfromChart");
                        if (ddlLoadDialReading.SelectedItem.Text == "Select")
                        {
                            dispalyMsg = "Select Load Dial Reading for row number " + (i + 1) + ".";
                            ddlLoadDialReading.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                    }
                }
            }
            #endregion
            
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }
        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;

            //validate CBR data
            #region validate CBR data
            if (TabCBR.Visible == true)
            {
                if (ValidateCBR() == false)
                {
                    valid = false;
                }
                //date validation
                if (valid == true)
                {
                    if (txtCBRDateOfTesting.Text == "")
                    {
                        dispalyMsg = "Date Of Testing can not be blank.";
                        txtCBRDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                    else if (txtCBRDateOfTesting.Text != "")
                    {
                        DateTime testingDate = DateTime.ParseExact(txtCBRDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (testingDate > System.DateTime.Now)
                        {
                            dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                            txtCBRDateOfTesting.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                        }
                    }
                }

                if (valid == true)
                {
                    if (txtMouldingDate.Text == "")
                    {
                        dispalyMsg = "Date Of Moulding can not be blank.";
                        txtMouldingDate.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                    else if (txtPenetrationDate.Text == "")
                    {
                        dispalyMsg = "Date Of Penetration can not be blank.";
                        txtPenetrationDate.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }                                     
                }

                if (valid == true)
                {
                    if (txtASPGCBRPent25.Text == "")
                    {
                        dispalyMsg = "Please Enter CBR(Pent at 2.5mm).";
                        txtASPGCBRPent25.Focus();
                        valid = false;
                    }
                    else if (txtASPGCBRPent50.Text == "")
                    {
                        dispalyMsg = "Please Enter CBR(Pent at 5.0mm).";
                        txtASPGCBRPent50.Focus();
                        valid = false;
                    }

                    else if (txtFinalCBRSample.Text == "")
                    {
                        dispalyMsg = "Please Enter Final CBR for tested sample.";
                        txtFinalCBRSample.Focus();
                        valid = false;
                    }
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
            if (valid == false && dispalyMsg !="")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }
        private void Calculate()
        {
            if (ValidateCBR() == true)
            {
                CalculateCBR();
            }
        }
        private void CalculateCBR()
        {            
            decimal temp = 0;
            TextBox txtBeforeSoaking3 = (TextBox)grdDensity.Rows[3].FindControl("txtBeforeSoaking");
            TextBox txtBeforeSoaking4 = (TextBox)grdDensity.Rows[4].FindControl("txtBeforeSoaking");
            TextBox txtAfterSoaking3 = (TextBox)grdDensity.Rows[3].FindControl("txtAfterSoaking");
            TextBox txtAfterSoaking4 = (TextBox)grdDensity.Rows[4].FindControl("txtAfterSoaking");

            for (int i = 0; i < grdMould.Rows.Count; i++)
            {
                TextBox txtHeight = (TextBox)grdMould.Rows[i].FindControl("txtHeight");
                TextBox txtVolume = (TextBox)grdMould.Rows[i].FindControl("txtVolume");
                CheckBox cbSelect = (CheckBox)grdMould.Rows[i].FindControl("cbSelect");
                DropDownList ddlMouldNo = (DropDownList)grdMould.Rows[i].FindControl("ddlMouldNo");

                if (cbSelect.Checked == true)
                {
                    //TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[3].FindControl("txtBeforeSoaking");
                    //TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[4].FindControl("txtBeforeSoaking");

                    var wtdata = dc.SoilSetting_View("CBR Modi Mould Volume").ToList();
                    foreach (var containerData in wtdata)
                    {
                        if (containerData.SOSET_F1_var == ddlMouldNo.SelectedItem.Text)
                        {
                            txtHeight.Text = containerData.SOSET_F3_var;
                            txtVolume.Text = containerData.SOSET_F2_var;
                            txtBeforeSoaking3.Text = containerData.SOSET_F3_var;
                            txtBeforeSoaking4.Text = containerData.SOSET_F2_var;
                            if (grdDensity.Columns.Count == 3)
                            {
                                //TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[3].FindControl("txtAfterSoaking");
                                //TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[4].FindControl("txtAfterSoaking");
                                txtAfterSoaking3.Text = containerData.SOSET_F3_var;
                                txtAfterSoaking4.Text = containerData.SOSET_F2_var;
                            }
                            break;
                        }
                    }
                    break;
                }
            }

            //for (int i = 1; i < grdWaterContent.Columns.Count; i++)
            //{
            DropDownList ddlBeforeCompaction = (DropDownList)grdWaterContent.Rows[0].Cells[0].FindControl("ddlBeforeCompaction");
            TextBox txtBeforeCompaction1 = (TextBox)grdWaterContent.Rows[1].Cells[1].FindControl("txtBeforeCompaction");
            TextBox txtBeforeCompaction2 = (TextBox)grdWaterContent.Rows[2].Cells[1].FindControl("txtBeforeCompaction");
            TextBox txtBeforeCompaction3 = (TextBox)grdWaterContent.Rows[3].Cells[1].FindControl("txtBeforeCompaction");
            TextBox txtBeforeCompaction4 = (TextBox)grdWaterContent.Rows[4].Cells[1].FindControl("txtBeforeCompaction");
            TextBox txtBeforeCompaction5 = (TextBox)grdWaterContent.Rows[5].Cells[1].FindControl("txtBeforeCompaction");
            TextBox txtBeforeCompaction6 = (TextBox)grdWaterContent.Rows[6].Cells[1].FindControl("txtBeforeCompaction");

            DropDownList ddlAfterCompaction = (DropDownList)grdWaterContent.Rows[0].Cells[0].FindControl("ddlAfterCompaction");
            TextBox txtAfterCompaction1 = (TextBox)grdWaterContent.Rows[1].Cells[2].FindControl("txtAfterCompaction");
            TextBox txtAfterCompaction2 = (TextBox)grdWaterContent.Rows[2].Cells[2].FindControl("txtAfterCompaction");
            TextBox txtAfterCompaction3 = (TextBox)grdWaterContent.Rows[3].Cells[2].FindControl("txtAfterCompaction");
            TextBox txtAfterCompaction4 = (TextBox)grdWaterContent.Rows[4].Cells[2].FindControl("txtAfterCompaction");
            TextBox txtAfterCompaction5 = (TextBox)grdWaterContent.Rows[5].Cells[2].FindControl("txtAfterCompaction");
            TextBox txtAfterCompaction6 = (TextBox)grdWaterContent.Rows[6].Cells[2].FindControl("txtAfterCompaction");

            DropDownList ddlTop30mm = (DropDownList)grdWaterContent.Rows[0].Cells[0].FindControl("ddlTop30mm");
            TextBox txtTop30mm1 = (TextBox)grdWaterContent.Rows[1].Cells[3].FindControl("txtTop30mm");
            TextBox txtTop30mm2 = (TextBox)grdWaterContent.Rows[2].Cells[3].FindControl("txtTop30mm");
            TextBox txtTop30mm3 = (TextBox)grdWaterContent.Rows[3].Cells[3].FindControl("txtTop30mm");
            TextBox txtTop30mm4 = (TextBox)grdWaterContent.Rows[4].Cells[3].FindControl("txtTop30mm");
            TextBox txtTop30mm5 = (TextBox)grdWaterContent.Rows[5].Cells[3].FindControl("txtTop30mm");
            TextBox txtTop30mm6 = (TextBox)grdWaterContent.Rows[6].Cells[3].FindControl("txtTop30mm");

            DropDownList ddlBulk = (DropDownList)grdWaterContent.Rows[0].Cells[0].FindControl("ddlBulk");
            TextBox txtBulk1 = (TextBox)grdWaterContent.Rows[1].Cells[3].FindControl("txtBulk");
            TextBox txtBulk2 = (TextBox)grdWaterContent.Rows[2].Cells[3].FindControl("txtBulk");
            TextBox txtBulk3 = (TextBox)grdWaterContent.Rows[3].Cells[3].FindControl("txtBulk");
            TextBox txtBulk4 = (TextBox)grdWaterContent.Rows[4].Cells[3].FindControl("txtBulk");
            TextBox txtBulk5 = (TextBox)grdWaterContent.Rows[5].Cells[3].FindControl("txtBulk");
            TextBox txtBulk6 = (TextBox)grdWaterContent.Rows[6].Cells[3].FindControl("txtBulk");

            string[] beforeCompac = ddlBeforeCompaction.SelectedValue.Split('|');
            txtBeforeCompaction3.Text = beforeCompac[1];
                        
            string[] afterCompac = ddlAfterCompaction.SelectedValue.Split('|');
            txtAfterCompaction3.Text = afterCompac[1];
            
            if (txtCheckSoak.Text == "Soaked")
            {
                string[] Top30mm = ddlTop30mm.SelectedValue.Split('|');
                txtTop30mm3.Text = Top30mm[1];

                string[] Bulk3 = ddlBulk.SelectedValue.Split('|');
                txtBulk3.Text = Bulk3[1];
            }

            //BeforeCompaction Calculation
            temp = Convert.ToDecimal(txtBeforeCompaction1.Text) - Convert.ToDecimal(txtBeforeCompaction2.Text);
            if (temp < 0)
                temp = 0;
            txtBeforeCompaction4.Text = temp.ToString();

            temp = Convert.ToDecimal(txtBeforeCompaction2.Text) - Convert.ToDecimal(txtBeforeCompaction3.Text);
            if (temp < 0)
                temp = 0;
            txtBeforeCompaction5.Text = temp.ToString();
            
            temp = 0;
            if (Convert.ToDecimal(txtBeforeCompaction5.Text) != 0)
            {
                temp = (Convert.ToDecimal(txtBeforeCompaction1.Text) - Convert.ToDecimal(txtBeforeCompaction2.Text)) / Convert.ToDecimal(txtBeforeCompaction5.Text) * 100;
            }            

            txtBeforeCompaction6.Text = temp.ToString();

            //AfterCompaction Calculation
            temp = Convert.ToDecimal(txtAfterCompaction1.Text) - Convert.ToDecimal(txtAfterCompaction2.Text);
            if (temp < 0)
                temp = 0;
            txtAfterCompaction4.Text = temp.ToString();

            temp = Convert.ToDecimal(txtAfterCompaction2.Text) - Convert.ToDecimal(txtAfterCompaction3.Text);
            if (temp < 0)
                temp = 0;
            txtAfterCompaction5.Text = temp.ToString();

            temp = 0;
            if (Convert.ToDecimal(txtAfterCompaction5.Text) != 0)
            {
                temp = (Convert.ToDecimal(txtAfterCompaction1.Text) - Convert.ToDecimal(txtAfterCompaction2.Text)) / Convert.ToDecimal(txtAfterCompaction5.Text) * 100;
            }
            txtAfterCompaction6.Text = temp.ToString();

            if (txtCheckSoak.Text == "Soaked")
            {
                //Top30mm Calculation
                temp = Convert.ToDecimal(txtTop30mm1.Text) - Convert.ToDecimal(txtTop30mm2.Text);
                if (temp < 0)
                    temp = 0;
                txtTop30mm4.Text = temp.ToString();

                temp = Convert.ToDecimal(txtTop30mm2.Text) - Convert.ToDecimal(txtTop30mm3.Text);
                if (temp < 0)
                    temp = 0;
                txtTop30mm5.Text = temp.ToString();

                temp = 0;
                if (Convert.ToDecimal(txtTop30mm5.Text) != 0)
                {
                    temp = (Convert.ToDecimal(txtTop30mm1.Text) - Convert.ToDecimal(txtTop30mm2.Text)) / Convert.ToDecimal(txtTop30mm5.Text) * 100;
                }
                txtTop30mm6.Text = temp.ToString();
                //Bulk Calculation
                temp = Convert.ToDecimal(txtBulk1.Text) - Convert.ToDecimal(txtBulk2.Text);
                if (temp < 0)
                    temp = 0;
                txtBulk4.Text = temp.ToString();

                temp = Convert.ToDecimal(txtBulk2.Text) - Convert.ToDecimal(txtBulk3.Text);
                if (temp < 0)
                    temp = 0;
                txtBulk5.Text = temp.ToString();

                temp = 0;
                if (Convert.ToDecimal(txtBulk5.Text) != 0)
                {
                    temp = (Convert.ToDecimal(txtBulk1.Text) - Convert.ToDecimal(txtBulk2.Text)) / Convert.ToDecimal(txtBulk5.Text) * 100;
                }
                txtBulk6.Text = temp.ToString();
            }
            TextBox txtBeforeSoaking0 = (TextBox)grdDensity.Rows[0].FindControl("txtBeforeSoaking");
            TextBox txtBeforeSoaking1 = (TextBox)grdDensity.Rows[1].FindControl("txtBeforeSoaking");
            TextBox txtBeforeSoaking2 = (TextBox)grdDensity.Rows[2].FindControl("txtBeforeSoaking");
            //TextBox txtBeforeSoaking3 = (TextBox)grdDensity.Rows[3].FindControl("txtBeforeSoaking");
            //TextBox txtBeforeSoaking4 = (TextBox)grdDensity.Rows[4].FindControl("txtBeforeSoaking");
            TextBox txtBeforeSoaking5 = (TextBox)grdDensity.Rows[5].FindControl("txtBeforeSoaking");
            TextBox txtBeforeSoaking6 = (TextBox)grdDensity.Rows[6].FindControl("txtBeforeSoaking");
            TextBox txtBeforeSoaking7 = (TextBox)grdDensity.Rows[7].FindControl("txtBeforeSoaking");

            temp = Convert.ToDecimal(txtBeforeSoaking0.Text) - Convert.ToDecimal(txtBeforeSoaking1.Text);
            if (temp < 0)
                temp = 0;
            txtBeforeSoaking2.Text = temp.ToString();

            temp = 0;
            if (Convert.ToDecimal(txtBeforeSoaking4.Text) != 0)
            {
                temp = Convert.ToDecimal(txtBeforeSoaking2.Text) / Convert.ToDecimal(txtBeforeSoaking4.Text);
            }
            txtBeforeSoaking5.Text = temp.ToString();

            temp = (Convert.ToDecimal(txtBeforeCompaction6.Text) + Convert.ToDecimal(txtAfterCompaction6.Text)) / 2;
            txtBeforeSoaking6.Text = temp.ToString();

            temp = 0;
            if (Convert.ToDecimal(txtBeforeSoaking6.Text) != 0)
            {
                temp = Convert.ToDecimal(txtBeforeSoaking5.Text) / (1 + Convert.ToDecimal(txtBeforeSoaking6.Text) / 100);
            }
            txtBeforeSoaking7.Text = temp.ToString();

            if (txtCheckSoak.Text == "Soaked")
            {
                TextBox txtAfterSoaking0 = (TextBox)grdDensity.Rows[0].FindControl("txtAfterSoaking");
                TextBox txtAfterSoaking1 = (TextBox)grdDensity.Rows[1].FindControl("txtAfterSoaking");
                TextBox txtAfterSoaking2 = (TextBox)grdDensity.Rows[2].FindControl("txtAfterSoaking");
                //TextBox txtAfterSoaking3 = (TextBox)grdDensity.Rows[3].FindControl("txtAfterSoaking");
                //TextBox txtAfterSoaking4 = (TextBox)grdDensity.Rows[4].FindControl("txtAfterSoaking");
                TextBox txtAfterSoaking5 = (TextBox)grdDensity.Rows[5].FindControl("txtAfterSoaking");
                TextBox txtAfterSoaking6 = (TextBox)grdDensity.Rows[6].FindControl("txtAfterSoaking");
                TextBox txtAfterSoaking7 = (TextBox)grdDensity.Rows[7].FindControl("txtAfterSoaking");
            
                temp = Convert.ToDecimal(txtAfterSoaking0.Text) - Convert.ToDecimal(txtAfterSoaking1.Text);
                if (temp < 0)
                    temp = 0;
                txtAfterSoaking2.Text = temp.ToString();

                temp = 0;
                if (Convert.ToDecimal(txtAfterSoaking4.Text) != 0)
                {
                    temp = Convert.ToDecimal(txtAfterSoaking2.Text) / Convert.ToDecimal(txtAfterSoaking4.Text);
                }
                txtAfterSoaking5.Text = temp.ToString();
            
                temp = (Convert.ToDecimal(txtTop30mm6.Text) + Convert.ToDecimal(txtBulk6.Text)) / 2;
                txtAfterSoaking6.Text = temp.ToString();
            
                temp = 0;
                if (Convert.ToDecimal(txtAfterSoaking6.Text) != 0)
                {
                    temp = Convert.ToDecimal(txtAfterSoaking5.Text) / (1 + Convert.ToDecimal(txtAfterSoaking6.Text) / 100);
                }
                txtAfterSoaking7.Text = temp.ToString();
            }
            //Display corrected load from ddlPentration
            var soset = dc.SoilSetting_View("CBR Proving Ring" + "-" + ddlRingSize.SelectedValue).ToList();
            for (int pentrId = 0; pentrId < grdPenetration.Rows.Count; pentrId++)
            {
                DropDownList ddlLoadDialReading = (DropDownList)grdPenetration.Rows[pentrId].Cells[1].FindControl("ddlLoadDialReading");
                TextBox txtCorrectedloadfromChart = (TextBox)grdPenetration.Rows[pentrId].Cells[2].FindControl("txtCorrectedloadfromChart");
                foreach (var containerData in soset)
                {
                    if (containerData.SOSET_F1_var == ddlLoadDialReading.SelectedItem.Text)
                    {
                        txtCorrectedloadfromChart.Text = containerData.SOSET_F2_var;
                        break;
                    }
                }
            }

            TextBox txtCorrectedloadfromChart4 = (TextBox)grdPenetration.Rows[4].Cells[2].FindControl("txtCorrectedloadfromChart");
            TextBox txtCBR4 = (TextBox)grdPenetration.Rows[4].Cells[3].FindControl("txtCBR");

            TextBox txtCorrectedloadfromChart7 = (TextBox)grdPenetration.Rows[7].Cells[2].FindControl("txtCorrectedloadfromChart");
            TextBox txtCBR7 = (TextBox)grdPenetration.Rows[7].Cells[3].FindControl("txtCBR");

            temp = Convert.ToDecimal(txtCorrectedloadfromChart4.Text) / 1370 * 100;
            txtCBR4.Text = temp.ToString("0.0");
            txtCalCBRPent25.Text = temp.ToString("0.0");

            temp = Convert.ToDecimal(txtCorrectedloadfromChart7.Text) / 2055 * 100;
            txtCBR7.Text = temp.ToString("0.0");
            txtCalCBRPent50.Text = temp.ToString("0.0");

            for (int i = 4; i < grdWaterContent.Rows.Count; i++)
            {
                TextBox txtBeforeCompaction = (TextBox)grdWaterContent.Rows[i].Cells[1].FindControl("txtBeforeCompaction");
                TextBox txtAfterCompaction = (TextBox)grdWaterContent.Rows[i].Cells[2].FindControl("txtAfterCompaction");
                TextBox txtTop30mm = (TextBox)grdWaterContent.Rows[i].Cells[3].FindControl("txtTop30mm");
                TextBox txtBulk = (TextBox)grdWaterContent.Rows[i].Cells[3].FindControl("txtBulk");

                if (i == 4)
                {
                    temp = Convert.ToDecimal(txtBeforeCompaction.Text);
                    txtBeforeCompaction.Text = temp.ToString("0.000");

                    temp = Convert.ToDecimal(txtAfterCompaction.Text);
                    txtAfterCompaction.Text = temp.ToString("0.000");
                    if (txtCheckSoak.Text == "Soaked")
                    {
                        temp = Convert.ToDecimal(txtTop30mm.Text);
                        txtTop30mm.Text = temp.ToString("0.000");

                        temp = Convert.ToDecimal(txtBulk.Text);
                        txtBulk.Text = temp.ToString("0.000");
                    }
                }
                else
                {
                    temp = Convert.ToDecimal(txtBeforeCompaction.Text);
                    txtBeforeCompaction.Text = temp.ToString("0.00");

                    temp = Convert.ToDecimal(txtAfterCompaction.Text);
                    txtAfterCompaction.Text = temp.ToString("0.00");
                    if (txtCheckSoak.Text == "Soaked")
                    {
                        temp = Convert.ToDecimal(txtTop30mm.Text);
                        txtTop30mm.Text = temp.ToString("0.00");

                        temp = Convert.ToDecimal(txtBulk.Text);
                        txtBulk.Text = temp.ToString("0.00");
                    }
                }
            }

            for (int i = 2; i < grdDensity.Rows.Count; i++)
            {
                TextBox txtBeforeSoaking = (TextBox)grdDensity.Rows[i].FindControl("txtBeforeSoaking");
                TextBox txtAfterSoaking = (TextBox)grdDensity.Rows[i].FindControl("txtAfterSoaking");

                temp = Convert.ToDecimal(txtBeforeSoaking.Text);
                txtBeforeSoaking.Text = temp.ToString("0.00");
                if (txtCheckSoak.Text == "Soaked")
                {
                    temp = Convert.ToDecimal(txtAfterSoaking.Text);
                    txtAfterSoaking.Text = temp.ToString("0.00");
                }
            }

            for (int i = 0; i < grdPenetration.Rows.Count; i++)
            {
                if (i != 4 && i != 7)
                {
                    TextBox txtCBR = (TextBox)grdPenetration.Rows[i].Cells[3].FindControl("txtCBR");
                    txtCBR.Text = "";
                }
            }
        }
        
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.Soil_PDFReport(txtRefNo.Text, txtSampleName.Text, lblStatus.Text);
            rpt.PrintSelectedReport(txtRecordType.Text, txtRefNo.Text, lblStatus.Text, "", "", "", "", "", txtSampleName.Text, "");
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Soil_Report_Sample.aspx");
            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            string strURLWithData = "Soil_Report_Sample.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "SO", txtReportNo.Text, txtRefNo.Text, lblStatus.Text));
            Response.Redirect(strURLWithData);
        }
    }
}