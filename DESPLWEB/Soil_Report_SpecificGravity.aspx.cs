using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    
    public partial class Soil_Report_SpecificGravity : System.Web.UI.Page
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

                if (lblStatus.Text == "Enter")
                {
                    //lblStatus.Text = "Enter";
                    lblheading.Text = "Soil - Report Entry";
                }
                else if (lblStatus.Text == "Check")
                {
                    //  lblStatus.Text = "Check";
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

        protected void txtSpGrvRows_TextChanged(object sender, EventArgs e)
        {
            SpGrvRowsChanged();
        }

        private void SpGrvRowsChanged()
        {
            if (txtSpGrvRows.Text != "")
            {
                if (Convert.ToInt32(txtSpGrvRows.Text) < grdSpGrv.Rows.Count)
                {
                    for (int i = grdSpGrv.Rows.Count; i > Convert.ToInt32(txtSpGrvRows.Text); i--)
                    {
                        if (grdSpGrv.Rows.Count > 1)
                        {
                            DeleteRowSpGrv(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSpGrvRows.Text) > grdSpGrv.Rows.Count)
                {
                    for (int i = grdSpGrv.Rows.Count + 1; i <= Convert.ToInt32(txtSpGrvRows.Text); i++)
                    {
                        AddRowSpGrv();
                    }
                }
            }
        }

        #region add/delete rows grdSpGrv grid
        protected void AddRowSpGrv()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SpGrvTable"] != null)
            {
                GetCurrentDataSpGrv();
                dt = (DataTable)ViewState["SpGrvTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddlSpGrv_BottleNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfEmptyBottle_M1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfDrySoilM2MinusM1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfBottlePlusWater_M4", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_M3MinusM4", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_M2MinusM1_Minus_M3MinusM4", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_SpecificGravity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpGrv_AverageSpecificGravity", typeof(string)));
            }

            dr = dt.NewRow();
            dr["ddlSpGrv_BottleNo"] = string.Empty;
            dr["txtSpGrv_WeightOfBottlePlusDrySoilgm_M2"] = string.Empty;
            dr["txtSpGrv_WeightOfEmptyBottle_M1"] = string.Empty;
            dr["txtSpGrv_WeightOfDrySoilM2MinusM1"] = string.Empty;
            dr["txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3"] = string.Empty;
            dr["txtSpGrv_WeightOfBottlePlusWater_M4"] = string.Empty;
            dr["txtSpGrv_M3MinusM4"] = string.Empty;
            dr["txtSpGrv_M2MinusM1_Minus_M3MinusM4"] = string.Empty;
            dr["txtSpGrv_SpecificGravity"] = string.Empty;
            dr["txtSpGrv_AverageSpecificGravity"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["SpGrvTable"] = dt;
            grdSpGrv.DataSource = dt;
            grdSpGrv.DataBind();
            SetPreviousDataSpGrv();
        }
        protected void DeleteRowSpGrv(int rowIndex)
        {
            GetCurrentDataSpGrv();
            DataTable dt = ViewState["SpGrvTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SpGrvTable"] = dt;
            grdSpGrv.DataSource = dt;
            grdSpGrv.DataBind();
            SetPreviousDataSpGrv();
        }
        protected void SetPreviousDataSpGrv()
        {
            DataTable dt = (DataTable)ViewState["SpGrvTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlSpGrv_BottleNo = (DropDownList)grdSpGrv.Rows[i].FindControl("ddlSpGrv_BottleNo");
                TextBox txtSpGrv_WeightOfBottlePlusDrySoilgm_M2 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2");
                TextBox txtSpGrv_WeightOfEmptyBottle_M1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfEmptyBottle_M1");
                TextBox txtSpGrv_WeightOfDrySoilM2MinusM1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfDrySoilM2MinusM1");
                TextBox txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3");
                TextBox txtSpGrv_WeightOfBottlePlusWater_M4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusWater_M4");
                TextBox txtSpGrv_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M3MinusM4");
                TextBox txtSpGrv_M2MinusM1_Minus_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M2MinusM1_Minus_M3MinusM4");
                TextBox txtSpGrv_SpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_SpecificGravity");
                TextBox txtSpGrv_AverageSpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_AverageSpecificGravity");

                if (dt.Rows[i]["ddlSpGrv_BottleNo"].ToString() != "")
                    ddlSpGrv_BottleNo.Items.FindByText(dt.Rows[i]["ddlSpGrv_BottleNo"].ToString()).Selected = true;
                txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Text = dt.Rows[i]["txtSpGrv_WeightOfBottlePlusDrySoilgm_M2"].ToString();
                txtSpGrv_WeightOfEmptyBottle_M1.Text = dt.Rows[i]["txtSpGrv_WeightOfEmptyBottle_M1"].ToString();
                txtSpGrv_WeightOfDrySoilM2MinusM1.Text = dt.Rows[i]["txtSpGrv_WeightOfDrySoilM2MinusM1"].ToString();
                txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Text = dt.Rows[i]["txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3"].ToString();
                txtSpGrv_WeightOfBottlePlusWater_M4.Text = dt.Rows[i]["txtSpGrv_WeightOfBottlePlusWater_M4"].ToString();
                txtSpGrv_M3MinusM4.Text = dt.Rows[i]["txtSpGrv_M3MinusM4"].ToString();
                txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text = dt.Rows[i]["txtSpGrv_M2MinusM1_Minus_M3MinusM4"].ToString();
                txtSpGrv_SpecificGravity.Text = dt.Rows[i]["txtSpGrv_SpecificGravity"].ToString();
                txtSpGrv_AverageSpecificGravity.Text = dt.Rows[i]["txtSpGrv_AverageSpecificGravity"].ToString();
            }
        }
        protected void GetCurrentDataSpGrv()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ddlSpGrv_BottleNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfEmptyBottle_M1", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfDrySoilM2MinusM1", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_WeightOfBottlePlusWater_M4", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_M3MinusM4", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_M2MinusM1_Minus_M3MinusM4", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_SpecificGravity", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpGrv_AverageSpecificGravity", typeof(string)));
            for (int i = 0; i < grdSpGrv.Rows.Count; i++)
            {
                DropDownList ddlSpGrv_BottleNo = (DropDownList)grdSpGrv.Rows[i].FindControl("ddlSpGrv_BottleNo");
                TextBox txtSpGrv_WeightOfBottlePlusDrySoilgm_M2 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2");
                TextBox txtSpGrv_WeightOfEmptyBottle_M1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfEmptyBottle_M1");
                TextBox txtSpGrv_WeightOfDrySoilM2MinusM1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfDrySoilM2MinusM1");
                TextBox txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3");
                TextBox txtSpGrv_WeightOfBottlePlusWater_M4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusWater_M4");
                TextBox txtSpGrv_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M3MinusM4");
                TextBox txtSpGrv_M2MinusM1_Minus_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M2MinusM1_Minus_M3MinusM4");
                TextBox txtSpGrv_SpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_SpecificGravity");
                TextBox txtSpGrv_AverageSpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_AverageSpecificGravity");

                dr = dt.NewRow();
                dr["ddlSpGrv_BottleNo"] = ddlSpGrv_BottleNo.SelectedItem.Text;
                dr["txtSpGrv_WeightOfBottlePlusDrySoilgm_M2"] = txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Text;
                dr["txtSpGrv_WeightOfEmptyBottle_M1"] = txtSpGrv_WeightOfEmptyBottle_M1.Text;
                dr["txtSpGrv_WeightOfDrySoilM2MinusM1"] = txtSpGrv_WeightOfDrySoilM2MinusM1.Text;
                dr["txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3"] = txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Text;
                dr["txtSpGrv_WeightOfBottlePlusWater_M4"] = txtSpGrv_WeightOfBottlePlusWater_M4.Text;
                dr["txtSpGrv_M3MinusM4"] = txtSpGrv_M3MinusM4.Text;
                dr["txtSpGrv_M2MinusM1_Minus_M3MinusM4"] = txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text;
                dr["txtSpGrv_SpecificGravity"] = txtSpGrv_SpecificGravity.Text;
                dr["txtSpGrv_AverageSpecificGravity"] = txtSpGrv_AverageSpecificGravity.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["SpGrvTable"] = dt;

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
            TabSpGrv.Visible = false;
            grdSpGrv.DataBind();
            grdSpGrv.DataSource = null;
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnesBy.Visible = false;
            ViewState["RemarkTable"] = null;
            ViewState["SpGrvTable"] = null;
            txtWitnesBy.Text = "";
            ddlTestdApprdBy.SelectedIndex = 0;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;
        }

        private void DisplaySoilDetails()
        {
            //Inward details
            var inwd = dc.SoilInward_View(txtRefNo.Text, 0);
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
            TabSpGrv.Visible = false;
            //Test Details            
            int rowNo = 0;
            var test = dc.SoilSampleTest_View(txtRefNo.Text, txtSampleName.Text);
            foreach (var sotest in test)
            {
                rowNo = 0;
                if (sotest.TEST_Sr_No == 16)
                {
                    #region display Shrinkage Limit data

                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlSpGrv.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlSpGrv.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabSpGrv.HeaderText = TabSpGrv.HeaderText + " (Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabSpGrv.HeaderText = TabSpGrv.HeaderText + " (Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabSpGrv.HeaderText = TabSpGrv.HeaderText + " (Checked)";

                    TabSpGrv.Visible = true;
                    txtSpGrvRows.Text = sotest.SOSMPLTEST_Quantity_tint.ToString();
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtSpGrvDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtSpGrvDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtSpGrvDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtSpGrvDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    
                    var SpGrv = dc.SoilSpecificGravity_View(txtRefNo.Text, txtSampleName.Text);
                    foreach (var soSpGrv in SpGrv)
                    {
                        ddlSpecGravOf.SelectedValue = soSpGrv.SOSpGrv_SpecificGravityOf_var;
                        //specific gravity
                        AddRowSpGrv();
                        DropDownList ddlSpGrv_BottleNo = (DropDownList)grdSpGrv.Rows[rowNo].FindControl("ddlSpGrv_BottleNo");
                        TextBox txtSpGrv_WeightOfBottlePlusDrySoilgm_M2 = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2");
                        TextBox txtSpGrv_WeightOfEmptyBottle_M1 = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_WeightOfEmptyBottle_M1");
                        TextBox txtSpGrv_WeightOfDrySoilM2MinusM1 = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_WeightOfDrySoilM2MinusM1");
                        TextBox txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3 = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3");
                        TextBox txtSpGrv_WeightOfBottlePlusWater_M4 = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_WeightOfBottlePlusWater_M4");
                        TextBox txtSpGrv_M3MinusM4 = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_M3MinusM4");
                        TextBox txtSpGrv_M2MinusM1_Minus_M3MinusM4 = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_M2MinusM1_Minus_M3MinusM4");
                        TextBox txtSpGrv_SpecificGravity = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_SpecificGravity");
                        TextBox txtSpGrv_AverageSpecificGravity = (TextBox)grdSpGrv.Rows[rowNo].FindControl("txtSpGrv_AverageSpecificGravity");

                        ddlSpGrv_BottleNo.Items.FindByText(Convert.ToDecimal(soSpGrv.SOSpGrv_BottleNo_dec).ToString("0")).Selected = true;
                        txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Text = soSpGrv.SOSpGrv_WeightOfBottlePlusDrySoilgm_M2_dec.ToString();
                        txtSpGrv_WeightOfEmptyBottle_M1.Text = soSpGrv.SOSpGrv_WeightOfEmptyBottle_M1_dec.ToString();
                        txtSpGrv_WeightOfDrySoilM2MinusM1.Text = soSpGrv.SOSpGrv_WeightIfDrySoilM2MinusM1_dec.ToString();
                        txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Text = soSpGrv.SOSpGrv_WeightIfBottlePlusSoilPlusWater_M3_dec.ToString();
                        txtSpGrv_WeightOfBottlePlusWater_M4.Text = soSpGrv.SOSpGrv_WeightIfBottleWater_M4_dec.ToString();
                        txtSpGrv_M3MinusM4.Text = soSpGrv.SOSpGrv_M3MinusM4_dec.ToString();
                        txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text = soSpGrv.SOSpGrv_M2MinusM1_Minus_M3MinusM4_dec.ToString();
                        txtSpGrv_SpecificGravity.Text = soSpGrv.SOSpGrv_SpecificGravity_dec.ToString();
                        if (rowNo == 0)
                            txtSpGrv_AverageSpecificGravity.Text = soSpGrv.SOSpGrv_AverageSpecificGravity_dec.ToString();

                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        SpGrvRowsChanged();
                    }
                    else
                    {
                        txtSpGrvRows.Text = rowNo.ToString();
                    }
                    #endregion
                }

            }

            if (TabSpGrv.Visible == true)
                TabContainerSoil.ActiveTabIndex = 0;

            //Remark details
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

        private void CalculatSpGrv()
        {
            decimal temp = 0, totSpGrv = 0;
            //Specific gravity Calculation
            for (int i = 0; i < grdSpGrv.Rows.Count; i++)
            {
                DropDownList ddlSpGrv_BottleNo = (DropDownList)grdSpGrv.Rows[i].FindControl("ddlSpGrv_BottleNo");
                TextBox txtSpGrv_WeightOfBottlePlusDrySoilgm_M2 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2");
                TextBox txtSpGrv_WeightOfEmptyBottle_M1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfEmptyBottle_M1");
                TextBox txtSpGrv_WeightOfDrySoilM2MinusM1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfDrySoilM2MinusM1");
                TextBox txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3");
                TextBox txtSpGrv_WeightOfBottlePlusWater_M4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusWater_M4");
                TextBox txtSpGrv_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M3MinusM4");
                TextBox txtSpGrv_M2MinusM1_Minus_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M2MinusM1_Minus_M3MinusM4");
                TextBox txtSpGrv_SpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_SpecificGravity");
                TextBox txtSpGrv_AverageSpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_AverageSpecificGravity");

                if (txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Text != "" && txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Text != ""
                    && txtSpGrv_WeightOfEmptyBottle_M1.Text != "" && txtSpGrv_WeightOfBottlePlusWater_M4.Text != "")
                {
                    temp = (Convert.ToDecimal(txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Text) - Convert.ToDecimal(txtSpGrv_WeightOfEmptyBottle_M1.Text));
                    if (temp < 0)
                        temp = 0;
                    txtSpGrv_WeightOfDrySoilM2MinusM1.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Text) - Convert.ToDecimal(txtSpGrv_WeightOfBottlePlusWater_M4.Text));
                    if (temp < 0)
                        temp = 0;
                    txtSpGrv_M3MinusM4.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtSpGrv_WeightOfDrySoilM2MinusM1.Text) - Convert.ToDecimal(txtSpGrv_M3MinusM4.Text));
                    if (temp < 0)
                        temp = 0;
                    txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text = temp.ToString("0.0");

                    temp = 0;
                    if (Convert.ToDecimal(txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text) > 0)
                    {
                        temp = Convert.ToDecimal(txtSpGrv_WeightOfDrySoilM2MinusM1.Text) / Convert.ToDecimal(txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text);
                        if (temp < 0)
                            temp = 0;
                    }
                    txtSpGrv_SpecificGravity.Text = temp.ToString("0.0");

                    totSpGrv += Convert.ToDecimal(txtSpGrv_SpecificGravity.Text);
                }
                TextBox txtSpGrv_AverageSpecificGravity_Result = (TextBox)grdSpGrv.Rows[0].FindControl("txtSpGrv_AverageSpecificGravity");
                temp = 0;
                if (grdSpGrv.Rows.Count > 0)
                {
                    temp = totSpGrv / Convert.ToDecimal(txtSpGrvRows.Text);
                }
                txtSpGrv_AverageSpecificGravity_Result.Text = temp.ToString("0.00");
            }

        }

        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            bool dataFlag = false;
            //validate Shrinkage limit data
            #region validate Shrinkage limit data
            if (TabSpGrv.Visible == true && valid == true && TabSpGrv.Enabled == true)
            {
                if (txtSpGrvRows.Text == "" || txtSpGrvRows.Text == "0")
                {
                    dispalyMsg = "Shrinkage Limit Rows can not be zero or blank.";
                    txtSpGrvRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                //date validation
                else if (txtSpGrvDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtSpGrvDateOfTesting.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtSpGrvDateOfTesting.Text != "")
                {
                    string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime TestingDate = DateTime.ParseExact(txtSpGrvDateOfTesting.Text, "dd/MM/yyyy", null);
                    DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                    if (TestingDate > CurrentDate)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtSpGrvDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdSpGrv.Rows.Count; i++)
                    {
                        dataFlag = true;
                        DropDownList ddlSpGrv_BottleNo = (DropDownList)grdSpGrv.Rows[i].FindControl("ddlSpGrv_BottleNo");
                        TextBox txtSpGrv_WeightOfBottlePlusDrySoilgm_M2 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2");
                        TextBox txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3");
                        
                        if (ddlSpGrv_BottleNo.SelectedIndex <= 0)
                        {
                            dispalyMsg = "Select No. of Dish for Specific Gravity for row no " + (i + 1) + ".";
                            ddlSpGrv_BottleNo.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        if (txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Text == "")
                        {
                            dispalyMsg = "Enter Weight of Dish + Dry soil for Specific Gravity for row number " + (i + 1) + ".";
                            txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Text == "")
                        {
                            dispalyMsg = "Enter Weight of Dish + Wet soil for Specific Gravity for row number " + (i + 1) + ".";
                            txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                    }
                }

            }
            #endregion

            if (dataFlag == false)
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
                        TabContainerSoil.ActiveTabIndex = 1;
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
                    dispalyMsg = "Select NABL Scope.";
                    valid = false;
                    ddl_NablScope.Focus();
                }
                else if (ddl_NABLLocation.SelectedItem.Text == "--Select--")
                {
                    dispalyMsg = "Select NABL Location.";
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
                byte reportStatus = 0, enteredBy = 0, checkedBy = 0, testedBy = 0, approvedBy = 0;
                //DateTime testingDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                DateTime testingDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
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

                if (TabSpGrv.Visible == true)
                    testingDate = DateTime.ParseExact(txtSpGrvDateOfTesting.Text, "dd/MM/yyyy", null);

                //test data update

                // Shrinkage limit
                #region save Shrinkage limit data
                if (TabSpGrv.Visible == true && pnlSpGrv.Enabled == true)
                {
                    decimal SpGrv = 0;
                    dc.SoilSpecificGravity_Update(0, txtRefNo.Text, txtSampleName.Text, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
                    for (int i = 0; i < grdSpGrv.Rows.Count; i++)
                    {
                        DropDownList ddlSpGrv_BottleNo = (DropDownList)grdSpGrv.Rows[i].FindControl("ddlSpGrv_BottleNo");
                        TextBox txtSpGrv_WeightOfBottlePlusDrySoilgm_M2 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2");
                        TextBox txtSpGrv_WeightOfEmptyBottle_M1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfEmptyBottle_M1");
                        TextBox txtSpGrv_WeightOfDrySoilM2MinusM1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfDrySoilM2MinusM1");
                        TextBox txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3");
                        TextBox txtSpGrv_WeightOfBottlePlusWater_M4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusWater_M4");
                        TextBox txtSpGrv_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M3MinusM4");
                        TextBox txtSpGrv_M2MinusM1_Minus_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M2MinusM1_Minus_M3MinusM4");
                        TextBox txtSpGrv_SpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_SpecificGravity");
                        TextBox txtSpGrv_AverageSpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_AverageSpecificGravity");
                        
                        SpGrv = 0;
                        if (i == 0)
                        {
                            SpGrv = Convert.ToDecimal(txtSpGrv_AverageSpecificGravity.Text);
                        }

                        dc.SoilSpecificGravity_Update(i + 1, txtRefNo.Text, txtSampleName.Text, ddlSpecGravOf.SelectedValue, Convert.ToDecimal(ddlSpGrv_BottleNo.SelectedItem.Text), Convert.ToDecimal(txtSpGrv_WeightOfBottlePlusDrySoilgm_M2.Text), Convert.ToDecimal(txtSpGrv_WeightOfEmptyBottle_M1.Text), Convert.ToDecimal(txtSpGrv_WeightOfDrySoilM2MinusM1.Text)
                           , Convert.ToDecimal(txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3.Text), Convert.ToDecimal(txtSpGrv_WeightOfBottlePlusWater_M4.Text), Convert.ToDecimal(txtSpGrv_M3MinusM4.Text), Convert.ToDecimal(txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text), Convert.ToDecimal(txtSpGrv_SpecificGravity.Text), SpGrv, false);
                    }

                    var test = dc.Test(16, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    testingDate = DateTime.ParseExact(txtSpGrvDateOfTesting.Text, "dd/MM/yyyy", null);
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtSpGrvRows.Text), true);
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
                dc.SoilInward_Update_ReportData(reportStatus, txtRefNo.Text, txtWitnesBy.Text, 0, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, testingDate, null);
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

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        private void Calculate()
        {
            if (TabSpGrv.Visible == true && TabSpGrv.Enabled == true)
                CalculatSpGrv();
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

        protected void grdSpGrv_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Weight of empty Bottle
            //Weight of empty Pycnometer
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var soset = dc.SoilSetting_View("Weight of empty " + ddlSpecGravOf.SelectedValue);

                DropDownList ddlSpGrv_BottleNo = (e.Row.FindControl("ddlSpGrv_BottleNo") as DropDownList);                
                ddlSpGrv_BottleNo.DataSource = soset;
                ddlSpGrv_BottleNo.DataTextField = "SOSET_F1_var";
                ddlSpGrv_BottleNo.DataValueField = "F2plusF3";
                ddlSpGrv_BottleNo.DataBind();
                //if (ddlSpGrv_BottleNo.Items.Count > 0)
                    ddlSpGrv_BottleNo.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        protected void ddlSpGrv_BottleNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                DropDownList ddlSpGrv_BottleNo = (DropDownList)gvr.FindControl("ddlSpGrv_BottleNo");
                TextBox txtSpGrv_WeightOfEmptyBottle_M1 = (TextBox)gvr.FindControl("txtSpGrv_WeightOfEmptyBottle_M1");
                TextBox txtSpGrv_WeightOfBottlePlusWater_M4 = (TextBox)gvr.FindControl("txtSpGrv_WeightOfBottlePlusWater_M4");

                TextBox txtSpGrv_WeightOfDrySoilM2MinusM1 = (TextBox)gvr.FindControl("txtSpGrv_WeightOfDrySoilM2MinusM1");
                TextBox txtSpGrv_M3MinusM4 = (TextBox)gvr.FindControl("txtSpGrv_M3MinusM4");
                TextBox txtSpGrv_M2MinusM1_Minus_M3MinusM4 = (TextBox)gvr.FindControl("txtSpGrv_M2MinusM1_Minus_M3MinusM4");
                TextBox txtSpGrv_SpecificGravity = (TextBox)gvr.FindControl("txtSpGrv_SpecificGravity");
                TextBox txtSpGrv_AverageSpecificGravity = (TextBox)gvr.FindControl("txtSpGrv_AverageSpecificGravity");
                                
                if (ddlSpGrv_BottleNo.SelectedIndex > 0)
                {
                    string[] strval = ddlSpGrv_BottleNo.SelectedValue.Split('|'); 
                    txtSpGrv_WeightOfEmptyBottle_M1.Text = strval[0];
                    txtSpGrv_WeightOfBottlePlusWater_M4.Text = strval[1];
                    if (txtSpGrv_WeightOfDrySoilM2MinusM1.Text != "")
                    {
                        CalculatSpGrv(); 
                    }
                }
                else
                {
                    txtSpGrv_WeightOfEmptyBottle_M1.Text = "";
                    txtSpGrv_WeightOfDrySoilM2MinusM1.Text = "";
                    txtSpGrv_WeightOfBottlePlusWater_M4.Text = "";
                    txtSpGrv_M3MinusM4.Text = "";
                    txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text = "";
                    txtSpGrv_SpecificGravity.Text = "";
                    txtSpGrv_AverageSpecificGravity.Text = "";
                }
            }
        }

        protected void ddlSpecGravOf_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdSpGrv.Rows.Count; i++)
            {
                DropDownList ddlSpGrv_BottleNo = (DropDownList)grdSpGrv.Rows[i].FindControl("ddlSpGrv_BottleNo");
                //TextBox txtSpGrv_WeightOfBottlePlusDrySoilgm_M2 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusDrySoilgm_M2");
                TextBox txtSpGrv_WeightOfEmptyBottle_M1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfEmptyBottle_M1");
                TextBox txtSpGrv_WeightOfDrySoilM2MinusM1 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfDrySoilM2MinusM1");
                //TextBox txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusSoilPlusWater_M3");
                TextBox txtSpGrv_WeightOfBottlePlusWater_M4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_WeightOfBottlePlusWater_M4");
                TextBox txtSpGrv_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M3MinusM4");
                TextBox txtSpGrv_M2MinusM1_Minus_M3MinusM4 = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_M2MinusM1_Minus_M3MinusM4");
                TextBox txtSpGrv_SpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_SpecificGravity");
                TextBox txtSpGrv_AverageSpecificGravity = (TextBox)grdSpGrv.Rows[i].FindControl("txtSpGrv_AverageSpecificGravity");

                var soset = dc.SoilSetting_View("Weight of empty " + ddlSpecGravOf.SelectedValue);

                ddlSpGrv_BottleNo.ClearSelection();
                ddlSpGrv_BottleNo.Items.Clear();
                ddlSpGrv_BottleNo.DataSource = soset;
                ddlSpGrv_BottleNo.DataTextField = "SOSET_F1_var";
                ddlSpGrv_BottleNo.DataValueField = "F2plusF3";
                ddlSpGrv_BottleNo.DataBind();
                //if (ddlSpGrv_BottleNo.Items.Count > 0)
                    ddlSpGrv_BottleNo.Items.Insert(0, new ListItem("Select", "0"));

                txtSpGrv_WeightOfEmptyBottle_M1.Text = "";
                txtSpGrv_WeightOfDrySoilM2MinusM1.Text = "";
                txtSpGrv_WeightOfBottlePlusWater_M4.Text = "";
                txtSpGrv_M3MinusM4.Text = "";
                txtSpGrv_M2MinusM1_Minus_M3MinusM4.Text = "";
                txtSpGrv_SpecificGravity.Text = "";
                txtSpGrv_AverageSpecificGravity.Text = "";
            }
        }
    }
}