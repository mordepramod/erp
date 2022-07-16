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
    public partial class Soil_Report_Gradation : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        string reportCBRGradValues = "";
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
             //   txtRefNo.Text = Session["ReferenceNo"].ToString();
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
                getGradationData();
                getGraCobble();
                AddRemark();

                lnkPrint.Visible = false;
                lnkSave.Enabled = true;
                lnkCalculate.Enabled = true;
            }
        }

        //#region Calibration of Sand

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
        public void getGradationData()
        {
            string[] arryForTest = new string[21];
            arryForTest[0] = "100";
            arryForTest[1] = "63";
            arryForTest[2] = "40";
            arryForTest[3] = "31.5";
            arryForTest[4] = "25";
            arryForTest[5] = "20";
            arryForTest[6] = "16";
            arryForTest[7] = "10";
            arryForTest[8] = "6.3";
            arryForTest[9] = "4.75";
            arryForTest[10] = "2.36";
            arryForTest[11] = "1.18";
            arryForTest[12] = "1";
            arryForTest[13] = "0.600";
            arryForTest[14] = "0.425";
            arryForTest[15] = "0.300";
            arryForTest[16] = "0.150";
            arryForTest[17] = "0.090";
            arryForTest[18] = "0.075";
            arryForTest[19] = "Pan";
            arryForTest[20] = "Total";

            //if remark is not present add empty row 
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("txtGrdISSieve", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdWtRetained", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdPercentRetained1", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdWtPassing", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdPassing", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdPercentRetained2", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdRemarks", typeof(string)));

            for (int i = 0; i < 21; i++)
            {
                dr1 = dt1.NewRow();
                dr1["txtGrdISSieve"] = string.Empty;
                dr1["txtGrdWtRetained"] = string.Empty;
                dr1["txtGrdPercentRetained1"] = string.Empty;
                dr1["txtGrdWtPassing"] = string.Empty;
                dr1["txtGrdPassing"] = string.Empty;
                dr1["txtGrdPercentRetained2"] = string.Empty;
                dr1["txtGrdRemarks"] = string.Empty;
                dt1.Rows.Add(dr1);
            }

            ViewState["GradationTable"] = dt1;
            grdGrdation.DataSource = dt1;
            grdGrdation.DataBind();

            for (int i = 0; i < 21; i++)
            {
                // AddNewRow();
                TextBox txtGrdISSieve = (TextBox)grdGrdation.Rows[i].Cells[0].FindControl("txtGrdISSieve");
                txtGrdISSieve.Text = arryForTest[i];
            }

            int rowNo = 0;
            var test = dc.SoilSampleTest_View(txtRefNo.Text, txtSampleName.Text);
            foreach (var sotest in test)
            {
                rowNo = 0;
                if (sotest.TEST_Sr_No == 6 || sotest.TEST_Sr_No == 7)
                {
                    if (sotest.TEST_Sr_No == 6)
                        lblGradationType.Text = "Dry";
                    else if (sotest.TEST_Sr_No == 7)
                        lblGradationType.Text = "Wet";

                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlGradation.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlGradation.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabGradation.HeaderText = TabGradation.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabGradation.HeaderText = TabGradation.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabGradation.HeaderText = TabGradation.HeaderText + "(Checked)";

                    TabGradation.Visible = true;

                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtGrdDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtGrdDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtGrdDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtGrdDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    TextBox txtGrdWtRetained1 = (TextBox)grdGrdation.Rows[20].Cells[1].FindControl("txtGrdWtRetained");
                    TextBox txtGrdRemarks1 = (TextBox)grdGrdation.Rows[20].Cells[5].FindControl("txtGrdRemarks");
                    txtGrdWtRetained1.BackColor = System.Drawing.Color.LightGray;
                    txtGrdRemarks1.BackColor = System.Drawing.Color.LightGray;
                    txtGrdWtRetained1.ReadOnly = true;
                    txtGrdRemarks1.ReadOnly = true;
                    var GradationData = dc.SoilGradation_View(txtRefNo.Text, txtSampleName.Text);
                    foreach (var grdData in GradationData)
                    {
                        TextBox txtGrdWtRetained = (TextBox)grdGrdation.Rows[rowNo].Cells[1].FindControl("txtGrdWtRetained");
                        TextBox txtGrdPercentRetained1 = (TextBox)grdGrdation.Rows[rowNo].Cells[2].FindControl("txtGrdPercentRetained1");
                        TextBox txtGrdWtPassing = (TextBox)grdGrdation.Rows[rowNo].Cells[3].FindControl("txtGrdWtPassing");
                        TextBox txtGrdPassing = (TextBox)grdGrdation.Rows[rowNo].Cells[4].FindControl("txtGrdPassing");
                        TextBox txtGrdPercentRetained2 = (TextBox)grdGrdation.Rows[rowNo].Cells[5].FindControl("txtGrdPercentRetained2");
                        TextBox txtGrdRemarks = (TextBox)grdGrdation.Rows[rowNo].Cells[5].FindControl("txtGrdRemarks");

                        if (rowNo == 20)
                        {
                            txtGrdPercentRetained1.Text = "";
                            txtGrdWtPassing.Text = "";
                            txtGrdPassing.Text = "";
                            txtGrdPercentRetained2.Text = "";
                            txtGrdRemarks.Text = "";

                            txtGrdWtRetained.Text = grdData.SOGRD_WtRetained_dec.ToString();

                        }
                        else
                        {
                            txtGrdWtRetained.Text = grdData.SOGRD_WtRetained_dec.ToString();
                            txtGrdPercentRetained1.Text = grdData.SOGRD_PercentRetainedCumu_dec.ToString();
                            txtGrdWtPassing.Text = grdData.SOGRD_WtPassing_dec.ToString();
                            txtGrdPassing.Text = grdData.SOGRD_PercentPassing_dec.ToString();
                            txtGrdPercentRetained2.Text = grdData.SOGRD_PercentRetained_dec.ToString();
                            txtGrdRemarks.Text = grdData.SOGRD_Remark_var;
                        }
                        rowNo++;
                    }
                }
            }
        }
        public void getGraCobble()
        {

            string[] arryForCobble = new string[5];
            arryForCobble[0] = "Cobble %";
            arryForCobble[1] = "GRAVEL %";
            arryForCobble[2] = "SAND %";
            arryForCobble[3] = "SILT & CLAY %";
            arryForCobble[4] = "Total";

            string[] arryForValues = new string[3];
            arryForValues[0] = "Coarse";
            arryForValues[1] = "Fine";
            arryForValues[2] = "Medium";

            //if remark is not present add empty row 
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("txtGrdCobble", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdValues", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdHundrad1", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdHundrad2", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtGrdHundrad3", typeof(string)));

            for (int i = 0; i < 8; i++)
            {
                dr1 = dt1.NewRow();
                dr1["txtGrdCobble"] = string.Empty;
                dr1["txtGrdValues"] = string.Empty;
                dr1["txtGrdHundrad1"] = string.Empty;
                dr1["txtGrdHundrad2"] = string.Empty;
                dr1["txtGrdHundrad3"] = string.Empty;
                dt1.Rows.Add(dr1);
            }

            ViewState["GradCobbleTable"] = dt1;
            grdGrd.DataSource = dt1;
            grdGrd.DataBind();
            int j = 0;
            for (int i = 0; i < 8; i++)
            {
                // AddNewRow();
                TextBox txtGrdCobble = (TextBox)grdGrd.Rows[i].Cells[0].FindControl("txtGrdCobble");
                TextBox txtGrdValues = (TextBox)grdGrd.Rows[i].Cells[1].FindControl("txtGrdValues");
                TextBox txtGrdHundrad1 = (TextBox)grdGrd.Rows[i].Cells[2].FindControl("txtGrdHundrad1");
                TextBox txtGrdHundrad2 = (TextBox)grdGrd.Rows[i].Cells[3].FindControl("txtGrdHundrad2");
                TextBox txtGrdHundrad3 = (TextBox)grdGrd.Rows[i].Cells[4].FindControl("txtGrdHundrad3");

                if (i == 0 || i == 1 || i == 3 || i == 6 || i == 7)
                {
                    txtGrdCobble.Text = arryForCobble[j];
                    j++;
                }
                if (i == 0)
                {
                    txtGrdValues.Text = "---";
                }
                if (i == 1 || i == 3)
                {
                    txtGrdValues.Text = arryForValues[0];
                }
                if (i == 2 || i == 5)
                {
                    txtGrdValues.Text = arryForValues[1];
                }
                if (i == 4)
                {
                    txtGrdValues.Text = arryForValues[2];
                }

            }

            var GradationData = dc.SoilInward_View(txtRefNo.Text, 0);
            foreach (var soinwd in GradationData)
            {
                txtRefNo.Text = soinwd.SOINWD_ReferenceNo_var.ToString();
                txtReportNo.Text = soinwd.SOINWD_SetOfRecord_var;
                txtWitnesBy.Text = soinwd.SOINWD_WitnessBy_var;
                if (ddl_NablScope.Items.FindByValue(soinwd.SOINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(soinwd.SOINWD_NablScope_var);
                }
                if (Convert.ToString(soinwd.SOINWD_NablLocation_int) != null && Convert.ToString(soinwd.SOINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(soinwd.SOINWD_NablLocation_int);
                }
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

                if (soinwd.SOINWD_ReportDetails_var != null && soinwd.SOINWD_ReportDetails_var != "")
                {
                    try
                    {

                        string[] strResult = soinwd.SOINWD_ReportDetails_var.Split(',');
                        string[] strReportDetails = strResult[0].Split('=');
                        string[] strGradation = strReportDetails[1].Split('~');
                        string[] strGrdCommonData = strReportDetails[2].Split('|');

                        #region Grd Data
                        int counter = 0;
                        for (int i = 0; i < 8; i++)
                        {

                            TextBox txtGrdCobble = (TextBox)grdGrd.Rows[i].Cells[0].FindControl("txtGrdCobble");
                            TextBox txtGrdValues = (TextBox)grdGrd.Rows[i].Cells[1].FindControl("txtGrdValues");
                            TextBox txtGrdHundrad1 = (TextBox)grdGrd.Rows[i].Cells[2].FindControl("txtGrdHundrad1");
                            TextBox txtGrdHundrad2 = (TextBox)grdGrd.Rows[i].Cells[3].FindControl("txtGrdHundrad2");
                            TextBox txtGrdHundrad3 = (TextBox)grdGrd.Rows[i].Cells[4].FindControl("txtGrdHundrad3");

                            string[] strGradInDetail = strGradation[counter].Split('|');
                            txtGrdCobble.Text = strGradInDetail[0];

                            txtGrdValues.Text = strGradInDetail[1];
                            txtGrdHundrad1.Text = strGradInDetail[2];
                            txtGrdHundrad2.Text = strGradInDetail[3];
                            if (strGradInDetail[4].Contains("~"))
                            {
                                strGradation[4] = strGradInDetail[4].Remove(strGradation[4].Length - 1);
                            }
                            txtGrdHundrad3.Text = strGradInDetail[4];
                            counter++;
                        }

                        #endregion

                        #region display common data
                        txtGrdOriginalWt.Text = strGrdCommonData[0];
                        txtGrdWtSampleWashing.Text = strGrdCommonData[1];
                        txtGrdOvenDryswt.Text = strGrdCommonData[2];
                        txtGrdD10.Text = strGrdCommonData[3];
                        txtGrdD30.Text = strGrdCommonData[4];
                        txtGrdD60.Text = strGrdCommonData[5];
                        txtGrdCu.Text = strGrdCommonData[7];
                        txtGrdCc.Text = strGrdCommonData[6];
                        #endregion
                    }
                    catch { }
                }
            }

            mergeCell();
        }
        public void AddRemark()
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
        private void Calculate()
        {
            CalculateGrd1();
        }
        private void CalculateGrd1()
        {
            if (validateGradation() == true)
            {
                decimal sum = 0;
                decimal temp = 0;
                for (int i = 0; i < 21; i++)
                {
                    TextBox txtGrdWtRetained = (TextBox)grdGrdation.Rows[i].Cells[1].FindControl("txtGrdWtRetained");
                    TextBox txtGrdWtPanValue = (TextBox)grdGrdation.Rows[19].Cells[1].FindControl("txtGrdWtRetained");
                    
                    if (i < 20)
                    {
                        sum += Convert.ToDecimal(txtGrdWtRetained.Text);
                    }
                    else if (i == 20)
                    {
                        txtGrdWtRetained.Text = sum.ToString();
                        txtGrdOvenDryswt.Text = sum.ToString();
                        temp = Convert.ToDecimal(txtGrdOriginalWt.Text) - sum + Convert.ToDecimal(txtGrdWtPanValue.Text);
                        txtGrdWtSampleWashing.Text = temp.ToString("0.00");
                    }
                    
                }
                for (int i = 0; i < 21; i++)
                {
                    temp = 0;
                    TextBox txtGrdWtRetained = (TextBox)grdGrdation.Rows[i].Cells[1].FindControl("txtGrdWtRetained");
                    TextBox txtGrdPercentRetained1 = (TextBox)grdGrdation.Rows[i].Cells[2].FindControl("txtGrdPercentRetained1");
                    TextBox txtGrdWtPassing = (TextBox)grdGrdation.Rows[i].Cells[3].FindControl("txtGrdWtPassing");
                    TextBox txtGrdPassing = (TextBox)grdGrdation.Rows[i].Cells[4].FindControl("txtGrdPassing");
                    TextBox txtGrdPercentRetained2 = (TextBox)grdGrdation.Rows[i].Cells[5].FindControl("txtGrdPercentRetained2");
                    TextBox txtGrdRemarks = (TextBox)grdGrdation.Rows[i].Cells[6].FindControl("txtGrdRemarks");
                    if (i == 20)
                    {
                        txtGrdPercentRetained1.Text = "";
                        txtGrdWtPassing.Text = "";
                        txtGrdPassing.Text = "";
                        txtGrdPercentRetained2.Text = "";
                        txtGrdRemarks.Text = "";
                    }
                    else
                    {                        
                        temp = (100 * Convert.ToDecimal(txtGrdWtRetained.Text)) / sum;
                        if (temp < 0)
                            temp = 0;
                        txtGrdPercentRetained1.Text = temp.ToString("0.00");

                        if (i == 0)
                        {
                            temp = (sum - Convert.ToDecimal(txtGrdWtRetained.Text));
                            if (temp < 0)
                                temp = 0;
                            txtGrdWtPassing.Text = temp.ToString("0.00");
                        }
                        else
                        {
                            TextBox txtGrdWtPassingFor = (TextBox)grdGrdation.Rows[i - 1].Cells[3].FindControl("txtGrdWtPassing");
                            temp = (Convert.ToDecimal(txtGrdWtPassingFor.Text) - Convert.ToDecimal(txtGrdWtRetained.Text));
                            if (temp < 0)
                                temp = 0;
                            txtGrdWtPassing.Text = temp.ToString("0.00");
                        }

                        temp = (100 * Convert.ToDecimal(txtGrdWtPassing.Text)) / sum;
                        if (temp < 0)
                            temp = 0;
                        txtGrdPassing.Text = temp.ToString("0.00");

                        temp = (100 - Convert.ToDecimal(txtGrdPassing.Text));
                        if (temp < 0)
                            temp = 0;
                        txtGrdPercentRetained2.Text = temp.ToString("0.00");                        
                    }
                }
                for (int i = 0; i < 21; i++)
                {
                    TextBox txtGrdWtRetained = (TextBox)grdGrdation.Rows[i].Cells[1].FindControl("txtGrdWtRetained");
                    TextBox txtGrdPercentRetained1 = (TextBox)grdGrdation.Rows[i].Cells[2].FindControl("txtGrdPercentRetained1");
                    TextBox txtGrdWtPassing = (TextBox)grdGrdation.Rows[i].Cells[3].FindControl("txtGrdWtPassing");
                    TextBox txtGrdPassing = (TextBox)grdGrdation.Rows[i].Cells[4].FindControl("txtGrdPassing");
                    TextBox txtGrdPercentRetained2 = (TextBox)grdGrdation.Rows[i].Cells[5].FindControl("txtGrdPercentRetained2");
                    TextBox txtGrdRemarks = (TextBox)grdGrdation.Rows[i].Cells[6].FindControl("txtGrdRemarks");


                    if (i == 1 || i == 5 || i == 9 || i == 10 || i == 14 || i == 18 || i == 19)
                    {
                        TextBox txtGrdHundrad2For1 = (TextBox)grdGrd.Rows[0].Cells[3].FindControl("txtGrdHundrad2");
                        TextBox txtGrdHundrad2For2 = (TextBox)grdGrd.Rows[1].Cells[3].FindControl("txtGrdHundrad2");
                        TextBox txtGrdHundrad2For3 = (TextBox)grdGrd.Rows[2].Cells[3].FindControl("txtGrdHundrad2");
                        TextBox txtGrdHundrad2For4 = (TextBox)grdGrd.Rows[3].Cells[3].FindControl("txtGrdHundrad2");
                        TextBox txtGrdHundrad2For5 = (TextBox)grdGrd.Rows[4].Cells[3].FindControl("txtGrdHundrad2");
                        TextBox txtGrdHundrad2For6 = (TextBox)grdGrd.Rows[5].Cells[3].FindControl("txtGrdHundrad2");

                        decimal result = 0;
                        for (int j = 0; j < 8; j++)
                        {

                            TextBox txtGrdCobble = (TextBox)grdGrd.Rows[j].Cells[0].FindControl("txtGrdCobble");
                            TextBox txtGrdValues = (TextBox)grdGrd.Rows[j].Cells[1].FindControl("txtGrdValues");
                            TextBox txtGrdHundrad1 = (TextBox)grdGrd.Rows[j].Cells[2].FindControl("txtGrdHundrad1");
                            TextBox txtGrdHundrad2 = (TextBox)grdGrd.Rows[j].Cells[3].FindControl("txtGrdHundrad2");
                            TextBox txtGrdHundrad3 = (TextBox)grdGrd.Rows[j].Cells[4].FindControl("txtGrdHundrad3");

                            if (j == 0 && i == 1)
                            {
                                txtGrdHundrad1.Text = txtGrdPercentRetained2.Text;
                                txtGrdHundrad2.Text = txtGrdPercentRetained2.Text;
                                txtGrdHundrad3.Text = txtGrdPercentRetained2.Text;
                                break;
                            }
                            if (j == 1 && i == 5)
                            {
                                txtGrdHundrad1.Text = txtGrdPercentRetained2.Text;
                                TextBox txtGrdHundrad1For = (TextBox)grdGrd.Rows[1].Cells[2].FindControl("txtGrdHundrad1");
                                TextBox txtGrdHundrad2For = (TextBox)grdGrd.Rows[0].Cells[3].FindControl("txtGrdHundrad2");

                                result = Convert.ToDecimal(txtGrdHundrad1For.Text) - Convert.ToDecimal(txtGrdHundrad2For.Text);
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad2.Text = result.ToString("0.00");
                                break;
                            }
                            if (j == 2 && i == 9)
                            {
                                //for2
                                txtGrdHundrad1.Text = txtGrdPercentRetained2.Text;
                                //for3
                                TextBox txtGrdHundrad1For = (TextBox)grdGrd.Rows[2].Cells[2].FindControl("txtGrdHundrad1");
                                TextBox txtGrdHundrad2For0 = (TextBox)grdGrd.Rows[0].Cells[3].FindControl("txtGrdHundrad2");
                                TextBox txtGrdHundrad2For1Row1 = (TextBox)grdGrd.Rows[1].Cells[3].FindControl("txtGrdHundrad2");
                                result = Convert.ToDecimal(txtGrdHundrad1For.Text) - (Convert.ToDecimal(txtGrdHundrad2For0.Text) + Convert.ToDecimal(txtGrdHundrad2For1Row1.Text));
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad2.Text = result.ToString("0.00");
                                //for4
                                TextBox txtGrdHundrad2Val1 = (TextBox)grdGrd.Rows[1].Cells[3].FindControl("txtGrdHundrad2");
                                TextBox txtGrdHundrad2Val2 = (TextBox)grdGrd.Rows[2].Cells[3].FindControl("txtGrdHundrad2");
                                result = Convert.ToDecimal(txtGrdHundrad2Val1.Text) + Convert.ToDecimal(txtGrdHundrad2Val2.Text);
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad3.Text = result.ToString("0.00");
                                break;
                            }
                            if (j == 3 && i == 10)
                            {
                                txtGrdHundrad1.Text = txtGrdPercentRetained2.Text;
                                result = Convert.ToDecimal(txtGrdHundrad1.Text) - (Convert.ToDecimal(txtGrdHundrad2For1.Text) + Convert.ToDecimal(txtGrdHundrad2For2.Text) + Convert.ToDecimal(txtGrdHundrad2For3.Text));
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad2.Text = result.ToString("0.00");
                                break;
                            }
                            if (j == 4 && i == 14)
                            {
                                txtGrdHundrad1.Text = txtGrdPercentRetained2.Text;
                                result = Convert.ToDecimal(txtGrdHundrad1.Text) - (Convert.ToDecimal(txtGrdHundrad2For1.Text) + Convert.ToDecimal(txtGrdHundrad2For2.Text) + Convert.ToDecimal(txtGrdHundrad2For3.Text) + Convert.ToDecimal(txtGrdHundrad2For4.Text));
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad2.Text = result.ToString("0.00");
                                break;
                            }
                            if (j == 5 && i == 18)
                            {
                                txtGrdHundrad1.Text = txtGrdPercentRetained2.Text;
                                result = Convert.ToDecimal(txtGrdHundrad1.Text) - (Convert.ToDecimal(txtGrdHundrad2For1.Text) + Convert.ToDecimal(txtGrdHundrad2For2.Text) + Convert.ToDecimal(txtGrdHundrad2For3.Text) + Convert.ToDecimal(txtGrdHundrad2For4.Text) + Convert.ToDecimal(txtGrdHundrad2For5.Text));
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad2.Text = result.ToString("0.00");
                                result = Convert.ToDecimal(txtGrdHundrad2For4.Text) + Convert.ToDecimal(txtGrdHundrad2For5.Text) + Convert.ToDecimal(txtGrdHundrad2.Text);
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad3.Text = result.ToString("0.00");
                                TextBox txtGrdHundrad3For3Row = (TextBox)grdGrd.Rows[3].Cells[4].FindControl("txtGrdHundrad3");
                                TextBox txtGrdHundrad3For4Row = (TextBox)grdGrd.Rows[4].Cells[4].FindControl("txtGrdHundrad3");
                                txtGrdHundrad3For3Row.Text = result.ToString("0.00");
                                txtGrdHundrad3For4Row.Text = result.ToString("0.00");
                                break;
                            }
                            if (j == 6 && i == 19)
                            {
                                TextBox txtGrdPercentRetained2For = (TextBox)grdGrdation.Rows[i - 1].Cells[5].FindControl("txtGrdPercentRetained2");
                                result = Convert.ToDecimal(txtGrdPercentRetained2.Text) - Convert.ToDecimal(txtGrdPercentRetained2For.Text);
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad1.Text = result.ToString("0.00");

                                //result = Convert.ToDecimal(txtGrdHundrad1.Text) - (Convert.ToDecimal(txtGrdHundrad2For1.Text) + Convert.ToDecimal(txtGrdHundrad2For2.Text) + Convert.ToDecimal(txtGrdHundrad2For3.Text) + Convert.ToDecimal(txtGrdHundrad2For4.Text) + Convert.ToDecimal(txtGrdHundrad2For5.Text) + Convert.ToDecimal(txtGrdHundrad2For6.Text));
                                result = 100 - (Convert.ToDecimal(txtGrdHundrad2For1.Text) + Convert.ToDecimal(txtGrdHundrad2For2.Text) + Convert.ToDecimal(txtGrdHundrad2For3.Text) + Convert.ToDecimal(txtGrdHundrad2For4.Text) + Convert.ToDecimal(txtGrdHundrad2For5.Text) + Convert.ToDecimal(txtGrdHundrad2For6.Text));
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad2.Text = result.ToString("0.00");
                                txtGrdHundrad3.Text = txtGrdHundrad2.Text;
                                TextBox txtGrdHundrad3For1 = (TextBox)grdGrd.Rows[0].Cells[4].FindControl("txtGrdHundrad3");
                                TextBox txtGrdHundrad3For2 = (TextBox)grdGrd.Rows[2].Cells[4].FindControl("txtGrdHundrad3");
                                TextBox txtGrdHundrad3For3 = (TextBox)grdGrd.Rows[5].Cells[4].FindControl("txtGrdHundrad3");
                                TextBox txtGrdHundrad3For4 = (TextBox)grdGrd.Rows[6].Cells[4].FindControl("txtGrdHundrad3");

                                TextBox txtGrdHundrad3ForRow7 = (TextBox)grdGrd.Rows[j + 1].Cells[4].FindControl("txtGrdHundrad3");
                                result = Convert.ToDecimal(txtGrdHundrad3For1.Text) + Convert.ToDecimal(txtGrdHundrad3For2.Text) + Convert.ToDecimal(txtGrdHundrad3For3.Text) + Convert.ToDecimal(txtGrdHundrad3For4.Text);
                                if (result < 0)
                                    result = 0;
                                txtGrdHundrad3ForRow7.Text = result.ToString("0.00");

                                TextBox txtGrdHundrad3ForPreRow = (TextBox)grdGrd.Rows[1].Cells[4].FindControl("txtGrdHundrad3");
                                txtGrdHundrad3ForPreRow.Text = txtGrdHundrad3For2.Text;
                            }
                        }
                    }
                }

                if (txtGrdD60.Text != "" && txtGrdD10.Text != "" && txtGrdD30.Text != "")
                {
                    temp = Convert.ToDecimal(txtGrdD60.Text) / Convert.ToDecimal(txtGrdD10.Text);
                    if (temp < 0)
                        temp = 0;
                    txtGrdCu.Text = temp.ToString("0.00");
                    temp = (Convert.ToDecimal(txtGrdD30.Text) * Convert.ToDecimal(txtGrdD30.Text)) / (Convert.ToDecimal(txtGrdD10.Text) * Convert.ToDecimal(txtGrdD60.Text));
                    if (temp < 0)
                        temp = 0;
                    txtGrdCc.Text = temp.ToString("0.00");
                }
                mergeCell();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
        }

        protected void mergeCell()
        {
            for (int rowIndex = 0; rowIndex < grdGrd.Rows.Count; rowIndex++)
            {
                if (rowIndex == 1)
                {
                    GridViewRow row = grdGrd.Rows[rowIndex];
                    GridViewRow nextRow = grdGrd.Rows[rowIndex + 1];
                    row.Cells[4].RowSpan = 2;
                    nextRow.Cells[4].Visible = false;
                    row.BackColor = System.Drawing.Color.LightGray;

                }
                if (rowIndex == 3)
                {
                    GridViewRow row = grdGrd.Rows[rowIndex];
                    GridViewRow nextRow = grdGrd.Rows[rowIndex + 1];
                    GridViewRow nextRow1 = grdGrd.Rows[rowIndex + 2];
                    row.Cells[4].RowSpan = 3;
                    row.BackColor = System.Drawing.Color.LightGray;
                    nextRow.Cells[4].Visible = false;
                    nextRow1.Cells[4].Visible = false;
                }
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
                Calculate();
                DateTime testingDate = new DateTime();
                testingDate = DateTime.ParseExact(txtGrdDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
                if (TabGradation.Visible == true)
                    //testingDate = Convert.ToDateTime(txtGrdDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtGrdDateOfTesting.Text, "dd/MM/yyyy", null);


                #region save Gradation data
                if (TabGradation.Visible == true)
                {
                    dc.SoilGradation_Update(0, txtRefNo.Text, txtSampleName.Text, "", 0, 0, 0, 0, 0, "", true);
                    for (int i = 0; i < grdGrdation.Rows.Count; i++)
                    {
                        decimal f1 = 0, f2 = 0, f3 = 0, f4 = 0, f5 = 0; string remark = null;
                        TextBox txtGrdISSieve = (TextBox)grdGrdation.Rows[i].Cells[0].FindControl("txtGrdISSieve");
                        TextBox txtGrdWtRetained = (TextBox)grdGrdation.Rows[i].Cells[1].FindControl("txtGrdWtRetained");
                        TextBox txtGrdPercentRetained1 = (TextBox)grdGrdation.Rows[i].Cells[2].FindControl("txtGrdPercentRetained1");
                        TextBox txtGrdWtPassing = (TextBox)grdGrdation.Rows[i].Cells[3].FindControl("txtGrdWtPassing");
                        TextBox txtGrdPassing = (TextBox)grdGrdation.Rows[i].Cells[4].FindControl("txtGrdPassing");
                        TextBox txtGrdPercentRetained2 = (TextBox)grdGrdation.Rows[i].Cells[5].FindControl("txtGrdPercentRetained2");
                        TextBox txtGrdRemarks = (TextBox)grdGrdation.Rows[i].Cells[6].FindControl("txtGrdRemarks");
                        if (txtGrdWtRetained.Text != "")
                            f1 = Convert.ToDecimal(txtGrdWtRetained.Text);
                        if (txtGrdPercentRetained1.Text != "")
                            f2 = Convert.ToDecimal(txtGrdPercentRetained1.Text);
                        if (txtGrdWtPassing.Text != "")
                            f3 = Convert.ToDecimal(txtGrdWtPassing.Text);
                        if (txtGrdPassing.Text != "")
                            f4 = Convert.ToDecimal(txtGrdPassing.Text);
                        if (txtGrdPercentRetained2.Text != "")
                            f4 = Convert.ToDecimal(txtGrdPercentRetained2.Text);
                        if (txtGrdRemarks.Text != "")
                            remark = txtGrdRemarks.Text;
                        dc.SoilGradation_Update(0, txtRefNo.Text, txtSampleName.Text, txtGrdISSieve.Text, f1, f2,
                            f3, f4, f5, remark, false);
                    }
                    int testSrNo = 0;
                    if (lblGradationType.Text == "Dry")
                        testSrNo = 6;
                    else
                        testSrNo = 7;
                    var test = dc.Test(testSrNo, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, 0, true);
                }
                #endregion

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

                #region Save GrdCobble grid data into Report details var
                string reportDetails = "";

                for (int i = 0; i < grdGrd.Rows.Count; i++)
                {
                    TextBox txtGrdCobble = (TextBox)grdGrd.Rows[i].Cells[0].FindControl("txtGrdCobble");
                    TextBox txtGrdValues = (TextBox)grdGrd.Rows[i].Cells[1].FindControl("txtGrdValues");
                    TextBox txtGrdHundrad1 = (TextBox)grdGrd.Rows[i].Cells[2].FindControl("txtGrdHundrad1");
                    TextBox txtGrdHundrad2 = (TextBox)grdGrd.Rows[i].Cells[3].FindControl("txtGrdHundrad2");
                    TextBox txtGrdHundrad3 = (TextBox)grdGrd.Rows[i].Cells[4].FindControl("txtGrdHundrad3");

                    if (i == 0)
                    {
                        reportDetails = "Gradation=";
                    }
                    reportDetails = reportDetails + txtGrdCobble.Text + "|" + txtGrdValues.Text + "|" + txtGrdHundrad1.Text + "|" + txtGrdHundrad2.Text + "|" + txtGrdHundrad3.Text + "~";
                    if (i == grdGrd.Rows.Count - 1)
                    {
                        reportDetails = reportDetails + "=";
                    }
                }

                reportDetails = reportDetails + txtGrdOriginalWt.Text + "|" + txtGrdWtSampleWashing.Text + "|" + txtGrdOvenDryswt.Text + "|" + txtGrdD10.Text + "|" + txtGrdD30.Text + "|" +
                    txtGrdD60.Text + "|" + txtGrdCc.Text + "|" + txtGrdCu.Text;
                var reportData = dc.SoilInward_View(txtRefNo.Text, 0);
                reportCBRGradValues = reportData.FirstOrDefault().SOINWD_ReportDetails_var;

                if (reportCBRGradValues != "" && reportCBRGradValues != null)
                {
                    if (reportCBRGradValues.Contains(","))
                    {
                        string[] FinalCBRGradArray = reportCBRGradValues.Split(',');
                        reportDetails = reportDetails + "," + FinalCBRGradArray[1];
                    }
                    else
                    {
                        reportDetails = reportDetails + ",";
                    }
                }
                else
                {
                    reportDetails = reportDetails + ",";
                }
                #endregion
                dc.SoilInward_Update_ReportData(reportStatus, txtRefNo.Text, txtWitnesBy.Text, 0, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, testingDate, reportDetails);

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
        protected Boolean ValidateData()
        {
            dispalyMsg = "";
            Boolean valid = true;

            //validate Gradation data
            #region validate Gradation data
            if (TabGradation.Visible == true)
            {
                //date validation
                if (txtGrdDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtGrdDateOfTesting.Focus();
                    valid = false;
                    TabContainerGradation.ActiveTabIndex = 0;
                }
                else if (txtGrdDateOfTesting.Text != "")
                {
                    DateTime testingDate = DateTime.ParseExact(txtGrdDateOfTesting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (testingDate > System.DateTime.Now)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtGrdDateOfTesting.Focus();
                        valid = false;
                        TabContainerGradation.ActiveTabIndex = 0;
                    }
                }                

                if (valid == true)
                {
                    valid = validateGradation();
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdGrdation.Rows.Count; i++)
                    {
                       
                        TextBox txtGrdRemarks = (TextBox)grdGrdation.Rows[i].Cells[6].FindControl("txtGrdRemarks");
                        if (i != 20)
                        {
                            if (txtGrdRemarks.Text == "")
                            {
                                dispalyMsg = "Enter Remarks for row number " + (i + 1) + ".";
                                txtGrdRemarks.Focus();
                                valid = false;
                                TabContainerGradation.ActiveTabIndex = 0;
                                break;
                            }
                        }
                    }
                }

                if (valid == true)
                {                    
                    if (txtGrdCu.Text == "")
                    {
                        dispalyMsg = "Enter value for Cu";
                        txtGrdCu.Focus();
                        valid = false;
                        TabContainerGradation.ActiveTabIndex = 0;
                    }
                    else if (txtGrdCc.Text == "")
                    {
                        dispalyMsg = "Enter value for Cc";
                        txtGrdCc.Focus();
                        valid = false;
                        TabContainerGradation.ActiveTabIndex = 0;
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
                            TabContainerGradation.ActiveTabIndex = 1;
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

        protected bool validateGradation()
        {
            Boolean valid = true;
            
            if (txtGrdOriginalWt.Text == "")
            {
                dispalyMsg = "Enter value for Original Wt. (g).";
                txtGrdOriginalWt.Focus();
                valid = false;
                TabContainerGradation.ActiveTabIndex = 0;
            }
            
            if (valid == true)
            {
                for (int i = 0; i < grdGrdation.Rows.Count; i++)
                {
                    TextBox txtGrdWtRetained = (TextBox)grdGrdation.Rows[i].Cells[1].FindControl("txtGrdWtRetained");

                    if (i != 20)
                    {

                        if (txtGrdWtRetained.Text == "")
                        {
                            dispalyMsg = "Enter Wt. Retained (g) for row number " + (i + 1) + ".";
                            txtGrdWtRetained.Focus();
                            valid = false;
                            TabContainerGradation.ActiveTabIndex = 0;
                            break;
                        }
                    }

                }
            }
            if (valid == true)
            {
                if (txtGrdD10.Text == "")
                {
                    dispalyMsg = "Enter value for D10";
                    txtGrdD10.Focus();
                    valid = false;
                    TabContainerGradation.ActiveTabIndex = 0;
                }
                else if (txtGrdD30.Text == "")
                {
                    dispalyMsg = "Enter value for D30";
                    txtGrdD30.Focus();
                    valid = false;
                    TabContainerGradation.ActiveTabIndex = 0;
                }

                else if (txtGrdD60.Text == "")
                {
                    dispalyMsg = "Enter value for D60";
                    txtGrdD60.Focus();
                    valid = false;
                    TabContainerGradation.ActiveTabIndex = 0;
                }
            }
            return valid;
        }
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

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.Soil_PDFReport(txtRefNo.Text, txtSampleName.Text,lblStatus.Text);
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