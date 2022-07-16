using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Soil_Report_FSI : System.Web.UI.Page    
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
               // txtSampleName.Text = Session["SoilSampleName"].ToString();

                if  (lblStatus.Text == "Enter")
                {
                    //lblStatus.Text = "Enter";
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
        
        #region add/delete rows grdFSI grid
        protected void AddRowFSI()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["FSITable"] != null)
            {
                GetCurrentDataFSI();
                dt = (DataTable)ViewState["FSITable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtFSIDeterminationNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFSIKeroscene", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFSIDistilledWater", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFSI", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFSIAvg", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtFSIDeterminationNo"] = string.Empty;
            dr["txtFSIKeroscene"] = string.Empty;
            dr["txtFSIDistilledWater"] = string.Empty;
            dr["txtFSI"] = string.Empty;
            dr["txtFSIAvg"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["FSITable"] = dt;
            grdFSI.DataSource = dt;
            grdFSI.DataBind();
            SetPreviousDataFSI();
        }
        protected void DeleteRowFSI(int rowIndex)
        {
            GetCurrentDataFSI();
            DataTable dt = ViewState["FSITable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["FSITable"] = dt;
            grdFSI.DataSource = dt;
            grdFSI.DataBind();
            SetPreviousDataFSI();
        }
        protected void SetPreviousDataFSI()
        {
            DataTable dt = (DataTable)ViewState["FSITable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtFSIDeterminationNo = (TextBox)grdFSI.Rows[i].FindControl("txtFSIDeterminationNo");
                TextBox txtFSIKeroscene = (TextBox)grdFSI.Rows[i].FindControl("txtFSIKeroscene");
                TextBox txtFSIDistilledWater = (TextBox)grdFSI.Rows[i].FindControl("txtFSIDistilledWater");
                TextBox txtFSI = (TextBox)grdFSI.Rows[i].FindControl("txtFSI");
                TextBox txtFSIAvg = (TextBox)grdFSI.Rows[i].FindControl("txtFSIAvg");

                txtFSIDeterminationNo.Text = dt.Rows[i]["txtFSIDeterminationNo"].ToString();
                txtFSIKeroscene.Text = dt.Rows[i]["txtFSIKeroscene"].ToString();
                txtFSIDistilledWater.Text = dt.Rows[i]["txtFSIDistilledWater"].ToString();
                txtFSI.Text = dt.Rows[i]["txtFSI"].ToString();
                txtFSIAvg.Text = dt.Rows[i]["txtFSIAvg"].ToString();
            }
        }
        protected void GetCurrentDataFSI()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtFSIDeterminationNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtFSIKeroscene", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtFSIDistilledWater", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtFSI", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtFSIAvg", typeof(string)));
            for (int i = 0; i < grdFSI.Rows.Count; i++)
            {
                TextBox txtFSIDeterminationNo = (TextBox)grdFSI.Rows[i].Cells[1].FindControl("txtFSIDeterminationNo");
                TextBox txtFSIKeroscene = (TextBox)grdFSI.Rows[i].Cells[2].FindControl("txtFSIKeroscene");
                TextBox txtFSIDistilledWater = (TextBox)grdFSI.Rows[i].Cells[2].FindControl("txtFSIDistilledWater");
                TextBox txtFSI = (TextBox)grdFSI.Rows[i].Cells[2].FindControl("txtFSI");
                TextBox txtFSIAvg = (TextBox)grdFSI.Rows[i].Cells[3].FindControl("txtFSIAvg");

                drRow = dtTable.NewRow();
                drRow["txtFSIDeterminationNo"] = txtFSIDeterminationNo.Text;
                drRow["txtFSIKeroscene"] = txtFSIKeroscene.Text;
                drRow["txtFSIDistilledWater"] = txtFSIDistilledWater.Text;
                drRow["txtFSI"] = txtFSI.Text;
                drRow["txtFSIAvg"] = txtFSIAvg.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["FSITable"] = dtTable;

        }
        protected void txtFSIRows_TextChanged(object sender, EventArgs e)
        {
            FSIRowsChanged();
        }
        private void FSIRowsChanged()
        {
            if (txtFSIRows.Text != "")
            {
                if (Convert.ToInt32(txtFSIRows.Text) < grdFSI.Rows.Count)
                {
                    for (int i = grdFSI.Rows.Count; i > Convert.ToInt32(txtFSIRows.Text); i--)
                    {
                        if (grdFSI.Rows.Count > 1)
                        {
                            DeleteRowFSI(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtFSIRows.Text) > grdFSI.Rows.Count)
                {
                    for (int i = grdFSI.Rows.Count + 1; i <= Convert.ToInt32(txtFSIRows.Text); i++)
                    {
                        AddRowFSI();
                    }
                }
            }
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
        
        private void ClearData()
        {
            TabRemark.Visible = true;
            TabFSI.Visible = false;            
            TabWC.Visible = false;            
            grdFSI.DataSource = null;
            grdFSI.DataBind();
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnesBy.Visible = false;
            ViewState["RemarkTable"] = null;
            ViewState["FSITable"] = null;            
            txtWitnesBy.Text = "";
            ddlTestdApprdBy.SelectedIndex = 0;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;
        }

        private void DisplaySoilDetails()
        { 
            //Inward details
            var inwd = dc.SoilInward_View(txtRefNo.Text,0);
            foreach (var soinwd in inwd)
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
            }
            TabFSI.Visible = false;
            TabWC.Visible = false;
            TabClassification.Visible = false;
            TabDirectShear.Visible = false;
            TabPH.Visible = false;
            //Test Details            
            int rowNo = 0;
            var test = dc.SoilSampleTest_View(txtRefNo.Text,txtSampleName.Text);
            foreach (var sotest in test)
            {
                rowNo = 0;
                if (sotest.TEST_Sr_No == 1)
                {
                    #region display FSI data
                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")                    
                        pnlFSI.Enabled = false;                    
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")                    
                        pnlFSI.Enabled = false;
                    
                    if (sotest.SOSMPLTEST_Status_tint == 0 )                    
                        TabFSI.HeaderText = TabFSI.HeaderText + "(Yet to Entered)";                    
                    else if (sotest.SOSMPLTEST_Status_tint == 1)                    
                        TabFSI.HeaderText = TabFSI.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabFSI.HeaderText = TabFSI.HeaderText + "(Checked)";
                    
                    TabFSI.Visible = true;
                    txtFSIRows.Text = sotest.SOSMPLTEST_Quantity_tint.ToString();
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtFSIDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtFSIDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtFSIDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtFSIDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var fsi = dc.SoilFSI_View(txtRefNo.Text,txtSampleName.Text);
                    foreach (var sofsi in fsi)
                    {
                        AddRowFSI();
                        TextBox txtFSIDeterminationNo = (TextBox)grdFSI.Rows[rowNo].FindControl("txtFSIDeterminationNo");
                        TextBox txtFSIKeroscene = (TextBox)grdFSI.Rows[rowNo].FindControl("txtFSIKeroscene");
                        TextBox txtFSIDistilledWater = (TextBox)grdFSI.Rows[rowNo].FindControl("txtFSIDistilledWater");
                        TextBox txtFSI = (TextBox)grdFSI.Rows[rowNo].FindControl("txtFSI");
                        TextBox txtFSIAvg = (TextBox)grdFSI.Rows[rowNo].FindControl("txtFSIAvg");
                                                
                        txtFSIDeterminationNo.Text = sofsi.SOFSI_DeterminationNo_var;
                        txtFSIKeroscene.Text = sofsi.SOFSI_Keroscene_dec.ToString();
                        txtFSIDistilledWater.Text = sofsi.SOFSI_DistilledWater_dec.ToString();
                        txtFSI.Text = sofsi.SOFSI_FSI_dec.ToString();
                        txtFSIAvg.Text = sofsi.SOFSI_Average_var;
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        FSIRowsChanged();
                    }
                    else
                    {
                        txtFSIRows.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (sotest.TEST_Sr_No == 2)
                {
                    #region display WC data
                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlWC.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlWC.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabWC.HeaderText = TabWC.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabWC.HeaderText = TabWC.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabWC.HeaderText = TabWC.HeaderText + "(Checked)";

                    var soset = dc.SoilSetting_View("Weight Of Container");
                    ddlWCContNo.DataSource = soset;
                    ddlWCContNo.DataValueField = "SOSET_F2_var";
                    ddlWCContNo.DataTextField = "SOSET_F1_var";
                    ddlWCContNo.DataBind();
                    if (ddlWCContNo.Items.Count > 0)
                        ddlWCContNo.Items.Insert(0, new ListItem("---Select---","0")); 
                    TabWC.Visible = true;                    
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtWCDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtWCDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtWCDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtWCDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    decimal wt = 0;
                    var wc = dc.SoilWC_View(txtRefNo.Text, txtSampleName.Text, sotest.TEST_Sr_No);
                    foreach (var sowc in wc)
                    {
                        txtWCDryWt.Text = sowc.SOWC_DryWeight_dec.ToString();
                        txtWCWetWt.Text = sowc.SOWC_WetWeight_dec.ToString();
                        wt =Convert.ToDecimal(sowc.SOWC_ContainerWeight_var);
                        ddlWCContNo.SelectedValue = wt.ToString("0.000");
                        //ddlWCContNo.SelectedItem.Text = sowc.SOWC_ContainerNo_dec.ToString();
                        txtWCContWt.Text = sowc.SOWC_ContainerWeight_var;
                        txtWCWaterContent.Text = sowc.SOWC_WaterContent_dec.ToString();
                    }
                    #endregion
                }
                else if (sotest.TEST_Sr_No == 12)
                {
                    #region display Classification data
                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlCL.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlCL.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabClassification.HeaderText = TabClassification.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabClassification.HeaderText = TabClassification.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabClassification.HeaderText = TabClassification.HeaderText + "(Checked)";
      
                    TabClassification.Visible = true;
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtCLDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtCLDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtCLDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtCLDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var cl = dc.SoilWC_View(txtRefNo.Text, txtSampleName.Text, sotest.TEST_Sr_No);
                    foreach (var socl in cl)
                    {
                        txtCLClassification.Text = socl.SOWC_ContainerNo_var;
                        txtCLMappingColour.Text = socl.SOWC_ContainerWeight_var;
                    }                    
                    #endregion
                }
                else if (sotest.TEST_Sr_No == 13)
                {
                    #region display direct shear data
                   
                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlDS.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlDS.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabDirectShear.HeaderText = TabDirectShear.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabDirectShear.HeaderText = TabDirectShear.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabDirectShear.HeaderText = TabDirectShear.HeaderText + "(Checked)";

                    TabDirectShear.Visible = true;
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtDSDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtDSDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtDSDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtDSDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var ds = dc.SoilWC_View(txtRefNo.Text, txtSampleName.Text, sotest.TEST_Sr_No);
                    foreach (var sods in ds)
                    {
                        txtDSCohesionC.Text = sods.SOWC_ContainerNo_var;
                        txtDSAngle.Text = sods.SOWC_ContainerWeight_var;
                    }
                    
                    #endregion
                }
                else if (sotest.TEST_Sr_No == 14)
                {
                    #region display Ph data

                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlPH.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlPH.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabPH.HeaderText = TabPH.HeaderText + "(Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabPH.HeaderText = TabPH.HeaderText + "(Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabPH.HeaderText = TabPH.HeaderText + "(Checked)";

                    TabPH.Visible = true;
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtPHDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtPHDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtPHDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtPHDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var ph = dc.SoilWC_View(txtRefNo.Text, txtSampleName.Text, sotest.TEST_Sr_No);
                    foreach (var soph in ph)
                    {
                        txtPH.Text = soph.SOWC_ContainerNo_var;
                    }

                    #endregion
                }
            }
            
            if (TabFSI.Visible == true)
                TabContainerSoil.ActiveTabIndex = 0;
            else if (TabWC.Visible == true)
                TabContainerSoil.ActiveTabIndex = 1;
            else if (TabClassification.Visible == true)
                TabContainerSoil.ActiveTabIndex = 2;
            else if (TabDirectShear.Visible == true)
                TabContainerSoil.ActiveTabIndex = 3;
            else if (TabPH.Visible == true)
                TabContainerSoil.ActiveTabIndex = 4;

            //Remark details
            rowNo=0;
            var remark = dc.SoilRemarkDetail_View(txtRefNo.Text);
            foreach (var rem in remark)
            {
                AddRowRemark();                
                TextBox txtRemark = (TextBox)grdRemark.Rows[rowNo].FindControl("txtRemark");
                txtRemark.Text = rem.SOREM_Remark_var;
                rowNo++;
            }
            if (rowNo==0)
                AddRowRemark();
                
        }

        private void CalculateFSI()
        {            
            decimal fsi =0, totFsi = 0;            
            for (int i = 0; i < grdFSI.Rows.Count; i++)
            {
                TextBox txtFSIKeroscene = (TextBox)grdFSI.Rows[i].FindControl("txtFSIKeroscene");
                TextBox txtFSIDistilledWater = (TextBox)grdFSI.Rows[i].FindControl("txtFSIDistilledWater");
                TextBox txtFSI = (TextBox)grdFSI.Rows[i].FindControl("txtFSI");

                if (txtFSIKeroscene.Text != "" && txtFSIDistilledWater.Text != "")
                {
                    fsi = Convert.ToDecimal(txtFSIDistilledWater.Text) - Convert.ToDecimal(txtFSIKeroscene.Text);
                    if (fsi < 0)
                        fsi = 0;

                    if (Convert.ToDecimal(txtFSIKeroscene.Text) != 0)
                        fsi = (fsi / Convert.ToDecimal(txtFSIKeroscene.Text)) * 100;

                    txtFSI.Text = fsi.ToString("0.00");
                    totFsi = totFsi + fsi;
                }
            }
            TextBox txtFSIAvg = (TextBox)grdFSI.Rows[(grdFSI.Rows.Count / 2)].FindControl("txtFSIAvg");
            txtFSIAvg.Text = (totFsi / grdFSI.Rows.Count).ToString("0.00");

            //if (grdFSI.Rows.Count < 5)
            //    txtFSIAvg.Text = "***";
        }
        private void CalculateWC()
        { 
            txtWCWaterContent.Text ="0.00"; 
            decimal wc=0;
            if (txtWCDryWt.Text != "" && txtWCWetWt.Text != "" && txtWCContWt.Text !="")
            {
                if (Convert.ToDecimal(txtWCDryWt.Text) > 0)
                {
                    wc = 100 * ((Convert.ToDecimal(txtWCWetWt.Text) - Convert.ToDecimal(txtWCContWt.Text)) - (Convert.ToDecimal(txtWCDryWt.Text) - Convert.ToDecimal(txtWCContWt.Text))) / (Convert.ToDecimal(txtWCDryWt.Text) - Convert.ToDecimal(txtWCContWt.Text));
                    if (wc < 0)
                        wc = 0;

                    txtWCWaterContent.Text = wc.ToString("0.00");
                }
            }
        }
        
        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            bool dataFlag = false;     
            //validate FSI data
            #region validate FSI data
            if (TabFSI.Visible == true && valid == true && pnlFSI.Enabled ==true)
            {                            
                if (txtFSIRows.Text == "" || txtFSIRows.Text == "0")
                {
                    dispalyMsg = "Rows can not be zero or blank.";
                    txtFSIRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                //date validation
                else if (txtFSIDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtFSIDateOfTesting.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtFSIDateOfTesting.Text != "")
                {
                    //DateTime dateTest = DateTime.Now;
                    //dateTest = Convert.ToDateTime(txtFSIDateOfTesting.Text);
                    string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime TestingDate = DateTime.ParseExact(txtFSIDateOfTesting.Text, "dd/MM/yyyy", null);
                    DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                    //if (dateTest > System.DateTime.Now)
                    if (TestingDate > CurrentDate)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtFSIDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdFSI.Rows.Count; i++)
                    {
                        dataFlag = true;
                        TextBox txtFSIDeterminationNo = (TextBox)grdFSI.Rows[i].FindControl("txtFSIDeterminationNo");    
                        TextBox txtFSIKeroscene = (TextBox)grdFSI.Rows[i].FindControl("txtFSIKeroscene");
                        TextBox txtFSIDistilledWater = (TextBox)grdFSI.Rows[i].FindControl("txtFSIDistilledWater");

                        if (txtFSIDeterminationNo.Text == "")
                        {
                            dispalyMsg = "Enter Determination No. for row no " + (i + 1) + ".";
                            txtFSIDeterminationNo.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtFSIKeroscene.Text == "")
                        {
                            dispalyMsg = "Enter Keroscene for row number " + (i + 1) + ".";
                            txtFSIKeroscene.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtFSIDistilledWater.Text == "")
                        {
                            dispalyMsg = "Enter Distilled Water for row number " + (i + 1) + ".";
                            txtFSIDistilledWater.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }                       
                    }
                }
            }
            #endregion
            //validate Water content data
            #region validate WC data
            if (TabWC.Visible == true && valid == true && pnlWC.Enabled == true)
            {
                if (txtWCDryWt.Text != "" || txtWCWetWt.Text != "" | ddlWCContNo.SelectedIndex > 0)
                {
                    dataFlag = true;
                    if (txtWCDryWt.Text == "")
                    {
                        dispalyMsg = "Enter Dry Weight .";
                        txtWCDryWt.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                    }
                    else if (txtWCWetWt.Text == "")
                    {
                        dispalyMsg = "Enter Wet Weight .";
                        txtWCDryWt.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                    }
                    else if (Convert.ToDecimal(txtWCWetWt.Text) < Convert.ToDecimal(txtWCDryWt.Text))
                    {
                        dispalyMsg = "Wet Weight should be greater than equal to 'Dry Weight'.";
                        txtWCDryWt.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                    }
                    else if (ddlWCContNo.SelectedIndex <= 0)
                    {
                        dispalyMsg = "Select Container No.";
                        ddlWCContNo.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                    }
                    //date validation
                    else if (txtWCDateOfTesting.Text == "")
                    {
                        dispalyMsg = "Date Of Testing can not be blank.";
                        txtWCDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 1;
                    }
                    else if (txtWCDateOfTesting.Text != "")
                    {
                        //DateTime dateTest = DateTime.Now;
                        //dateTest = Convert.ToDateTime(txtWCDateOfTesting.Text);
                        string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                        DateTime TestingDate = DateTime.ParseExact(txtWCDateOfTesting.Text, "dd/MM/yyyy", null);
                        DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                        //if (dateTest > System.DateTime.Now)
                        if (TestingDate > CurrentDate)
                        {
                            dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                            txtWCDateOfTesting.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 1;
                        }
                    }
                }
            }
            #endregion
            //validate classification data
            #region validate CL data
            if (TabClassification.Visible == true && valid == true && pnlCL.Enabled == true)
            {
                if (txtCLClassification.Text != "" || txtCLMappingColour.Text != "")
                {
                    dataFlag = true;
                    if (txtCLClassification.Text == "")
                    {
                        dispalyMsg = "Enter Classification .";
                        txtCLClassification.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 2;
                    }
                    else if (txtCLMappingColour.Text == "")
                    {
                        dispalyMsg = "Enter MappingColour .";
                        txtCLMappingColour.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 2;
                    }                    
                    //date validation
                    else if (txtCLDateOfTesting.Text == "")
                    {
                        dispalyMsg = "Date Of Testing can not be blank.";
                        txtCLDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 2;
                    }
                    else if (txtCLDateOfTesting.Text != "")
                    {
                        //DateTime dateTest = DateTime.Now;
                        //dateTest = Convert.ToDateTime(txtCLDateOfTesting.Text);
                        string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                        DateTime TestingDate = DateTime.ParseExact(txtCLDateOfTesting.Text, "dd/MM/yyyy", null);
                        DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                        //if (dateTest > System.DateTime.Now)
                        if (TestingDate > CurrentDate)
                        {
                            dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                            txtCLDateOfTesting.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 2;
                        }
                    }
                }
            }
            #endregion
            //validate Direct shear data
            #region validate DS data
            if (TabDirectShear.Visible == true && valid == true && pnlDS.Enabled == true)
            {
                if (txtDSCohesionC.Text != "" || txtDSAngle.Text != "")
                {
                    dataFlag = true;
                    if (txtDSCohesionC.Text == "")
                    {
                        dispalyMsg = "Enter Cohesion - C .";
                        txtDSCohesionC.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 3;
                    }
                    else if (txtDSAngle.Text == "")
                    {
                        dispalyMsg = "Enter Angle of Internal Friction - Ø .";
                        txtDSAngle.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 3;
                    }
                    //date validation
                    else if (txtDSDateOfTesting.Text == "")
                    {
                        dispalyMsg = "Date Of Testing can not be blank.";
                        txtDSDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 3;
                    }
                    else if (txtDSDateOfTesting.Text != "")
                    {
                        //DateTime dateTest = DateTime.Now;
                        //dateTest = Convert.ToDateTime(txtDSDateOfTesting.Text);
                        string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                        DateTime TestingDate = DateTime.ParseExact(txtDSDateOfTesting.Text, "dd/MM/yyyy", null);
                        DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                        //if (dateTest > System.DateTime.Now)
                        if (TestingDate > CurrentDate)
                        {
                            dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                            txtDSDateOfTesting.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 3;
                        }
                    }
                }
            }
            #endregion
            //validate PH data
            #region validate PH data
            if (TabPH.Visible == true && valid == true && pnlPH.Enabled == true)
            {
                if (txtPH.Text != "" )
                {
                    dataFlag = true;
                    if (txtPH.Text == "")
                    {
                        dispalyMsg = "Enter pH .";
                        txtPH.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 4;
                    }                    
                    //date validation
                    else if (txtPHDateOfTesting.Text == "")
                    {
                        dispalyMsg = "Date Of Testing can not be blank.";
                        txtPHDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 4;
                    }
                    else if (txtPHDateOfTesting.Text != "")
                    {
                        //DateTime dateTest = DateTime.Now;
                        //dateTest = Convert.ToDateTime(txtPHDateOfTesting.Text);
                        string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                        DateTime TestingDate = DateTime.ParseExact(txtPHDateOfTesting.Text, "dd/MM/yyyy", null);
                        DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                        //if (dateTest > System.DateTime.Now)
                        if (TestingDate > CurrentDate)
                        {
                            dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                            txtPHDateOfTesting.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 4;
                        }
                    }
                }
            }
            #endregion

            if (dataFlag == false && valid == true)
            {
                dispalyMsg = "Please Enter data for at least one test.";
                valid = false;
            }

            if (valid == true)
            {
                for (int i = 0; i <= grdRemark.Rows.Count - 1; i++)
                {
                    TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                    txtRemark.Text = txtRemark.Text.Trim();
                    if (txtRemark.Text == "" && grdRemark.Rows.Count > 1)
                    {
                        dispalyMsg = "Please Enter Remark.";
                        TabContainerSoil.ActiveTabIndex = 5;
                        txtRemark.Focus();
                        valid = false;
                        break;
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

        protected void lnkSave_Click(object sender, EventArgs e)
        {            
            if (ValidateData() == true)
            {
                Calculate();
                //inward update
                byte reportStatus = 0, enteredBy = 0 , checkedBy = 0, testedBy = 0 ,approvedBy = 0;
                //DateTime testingDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                DateTime testingDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                int testId=0;                
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

                if (TabFSI.Visible == true)
                    //testingDate = Convert.ToDateTime(txtFSIDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtFSIDateOfTesting.Text, "dd/MM/yyyy", null);
                else if (TabWC.Visible == true)
                    //testingDate = Convert.ToDateTime(txtWCDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtWCDateOfTesting.Text, "dd/MM/yyyy", null);
                else if (TabClassification.Visible == true)
                    //testingDate = Convert.ToDateTime(txtCLDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtCLDateOfTesting.Text, "dd/MM/yyyy", null);
                else if (TabDirectShear.Visible == true)
                    //testingDate = Convert.ToDateTime(txtDSDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtDSDateOfTesting.Text, "dd/MM/yyyy", null);
                else if (TabPH.Visible == true)
                    //testingDate = Convert.ToDateTime(txtPHDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtPHDateOfTesting.Text, "dd/MM/yyyy", null);
                                
                //test data update
                
                //FSI
                #region save FSI data
                if (TabFSI.Visible == true && pnlFSI.Enabled == true)
                {
                    dc.SoilFSI_Update(0, txtRefNo.Text, txtSampleName.Text, "", 0, 0, 0, "", true);
                    for (int i = 0; i < grdFSI.Rows.Count; i++)
                    {
                        TextBox txtFSIDeterminationNo = (TextBox)grdFSI.Rows[i].FindControl("txtFSIDeterminationNo");
                        TextBox txtFSIKeroscene = (TextBox)grdFSI.Rows[i].FindControl("txtFSIKeroscene");
                        TextBox txtFSIDistilledWater = (TextBox)grdFSI.Rows[i].FindControl("txtFSIDistilledWater");
                        TextBox txtFSI = (TextBox)grdFSI.Rows[i].FindControl("txtFSI");
                        TextBox txtFSIAvg = (TextBox)grdFSI.Rows[i].FindControl("txtFSIAvg");

                        dc.SoilFSI_Update(i + 1, txtRefNo.Text, txtSampleName.Text, txtFSIDeterminationNo.Text, Convert.ToDecimal(txtFSIKeroscene.Text), Convert.ToDecimal(txtFSIDistilledWater.Text), Convert.ToDecimal(txtFSI.Text), txtFSIAvg.Text, false);
                    }

                    var test = dc.Test(1, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    testingDate = DateTime.ParseExact(txtFSIDateOfTesting.Text, "dd/MM/yyyy", null);
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtFSIRows.Text), true);
                }
                #endregion
                //Water Content
                #region save WC data
                if (TabWC.Visible == true && pnlWC.Enabled == true)
                {
                    if (txtWCDryWt.Text != "")
                    {
                        dc.SoilWC_Update(2, txtRefNo.Text, txtSampleName.Text, Convert.ToDecimal(txtWCDryWt.Text), Convert.ToDecimal(txtWCWetWt.Text), ddlWCContNo.SelectedItem.Text, ddlWCContNo.SelectedItem.Value, Convert.ToDecimal(txtWCWaterContent.Text));

                        var test = dc.Test(2, "", 0, "SO", "", 0);
                        foreach (var tst in test)
                        {
                            testId = Convert.ToInt32(tst.TEST_Id);
                        }
                        testingDate = DateTime.ParseExact(txtWCDateOfTesting.Text, "dd/MM/yyyy", null);
                        dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, 1, true);
                    }
                }
                #endregion
                //Classification
                #region save CL data
                if (TabClassification.Visible == true && pnlCL.Enabled == true)
                {
                    if (txtCLClassification.Text != "")
                    {
                        dc.SoilWC_Update(12, txtRefNo.Text, txtSampleName.Text, 0,0,txtCLClassification.Text, txtCLMappingColour.Text,0);

                        var test = dc.Test(12, "", 0, "SO", "", 0);
                        foreach (var tst in test)
                        {
                            testId = Convert.ToInt32(tst.TEST_Id);
                        }
                        testingDate = DateTime.ParseExact(txtCLDateOfTesting.Text, "dd/MM/yyyy", null);
                        dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, 1, true);
                    }
                }
                #endregion
                //Direct Shear
                #region save direct shear data
                if (TabDirectShear.Visible == true && pnlDS.Enabled == true)
                {
                    if (txtDSCohesionC.Text != "")
                    {
                        dc.SoilWC_Update(13, txtRefNo.Text, txtSampleName.Text, 0, 0, txtDSCohesionC.Text, txtDSAngle.Text, 0);

                        var test = dc.Test(13, "", 0, "SO", "", 0);
                        foreach (var tst in test)
                        {
                            testId = Convert.ToInt32(tst.TEST_Id);
                        }
                        testingDate = DateTime.ParseExact(txtDSDateOfTesting.Text, "dd/MM/yyyy", null);
                        dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, 1, true);
                    }
                }
                #endregion
                //pH
                #region save pH data
                if (TabPH.Visible == true && pnlDS.Enabled == true)
                {
                    if (txtPH.Text != "")
                    {
                        dc.SoilWC_Update(14, txtRefNo.Text, txtSampleName.Text, 0, 0, txtPH.Text, "", 0);

                        var test = dc.Test(14, "", 0, "SO", "", 0);
                        foreach (var tst in test)
                        {
                            testId = Convert.ToInt32(tst.TEST_Id);
                        }
                        testingDate = DateTime.ParseExact(txtPHDateOfTesting.Text, "dd/MM/yyyy", null);
                        dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, 1, true);
                    }
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

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        private void Calculate()
        {
            if (TabFSI.Visible == true && pnlFSI.Enabled ==true)
                CalculateFSI();
            if (TabWC.Visible == true && pnlWC.Enabled == true)
                CalculateWC();
            
        }
        protected void chkWitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txtWitnesBy.Text = "";
            if (chkWitnessBy.Checked == true)
                txtWitnesBy.Visible = true;
            else
                txtWitnesBy.Visible = false;
        }
                
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Redirect("Soil_Report_Sample.aspx");
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void ddlWCContNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtWCContWt.Text = "";
            if (ddlWCContNo.SelectedIndex > 0)
                txtWCContWt.Text = ddlWCContNo.SelectedItem.Value; 
        }
        
       
    }
}