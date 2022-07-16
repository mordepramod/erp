using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Soil_Report_ShrinkageLimit : System.Web.UI.Page
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

        protected void txtShLimRows_TextChanged(object sender, EventArgs e)
        {
            ShLimRowsChanged();
        }

        private void ShLimRowsChanged()
        {
            if (txtShLimRows.Text != "")
            {
                if (Convert.ToInt32(txtShLimRows.Text) < grdShLim.Rows.Count)
                {
                    for (int i = grdShLim.Rows.Count; i > Convert.ToInt32(txtShLimRows.Text); i--)
                    {
                        if (grdShLim.Rows.Count > 1)
                        {
                            DeleteRowShLim(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtShLimRows.Text) > grdShLim.Rows.Count)
                {
                    for (int i = grdShLim.Rows.Count + 1; i <= Convert.ToInt32(txtShLimRows.Text); i++)
                    {
                        AddRowShLim();
                    }
                }
            }
        }

        #region add/delete rows grdShLim grid
        protected void AddRowShLim()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ShLimTable"] != null)
            {
                GetCurrentDataShLim();
                dt = (DataTable)ViewState["ShLimTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddlShLim_NOOfDish", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfDishPlusDrySoil", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfDishPlusWetSoil", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfWater", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfEmptyDish", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfWetSoil_M2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfDrySoil_M1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_MoistureContentRatio_W1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_MoistureContentPercent", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfDishPlusMercuryFilling", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfMercury", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_VolumeOfWetSoilPat_V", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfWeighingDishPlusDisplacedMercury", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfWeighingDishEmpty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_WeightOfDisplacedMercury", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_VolumeOfDrySoilPat_Vd", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_ShrinkageLimit", typeof(string)));
                dt.Columns.Add(new DataColumn("txtShLim_AverageShrinkageLimit", typeof(string)));
            }

            dr = dt.NewRow();
            dr["ddlShLim_NOOfDish"] = string.Empty;
            dr["txtShLim_WeightOfDishPlusDrySoil"] = string.Empty;
            dr["txtShLim_WeightOfDishPlusWetSoil"] = string.Empty;
            dr["txtShLim_WeightOfWater"] = string.Empty;
            dr["txtShLim_WeightOfEmptyDish"] = string.Empty;
            dr["txtShLim_WeightOfWetSoil_M2"] = string.Empty;
            dr["txtShLim_WeightOfDrySoil_M1"] = string.Empty;
            dr["txtShLim_MoistureContentRatio_W1"] = string.Empty;
            dr["txtShLim_WeightOfDishPlusMercuryFilling"] = string.Empty;
            dr["txtShLim_WeightOfMercury"] = string.Empty;
            dr["txtShLim_VolumeOfWetSoilPat_V"] = string.Empty;
            dr["txtShLim_WeightOfWeighingDishPlusDisplacedMercury"] = string.Empty;
            dr["txtShLim_WeightOfWeighingDishEmpty"] = string.Empty;
            dr["txtShLim_WeightOfDisplacedMercury"] = string.Empty;
            dr["txtShLim_VolumeOfDrySoilPat_Vd"] = string.Empty;
            dr["txtShLim_ShrinkageLimit"] = string.Empty;
            dr["txtShLim_AverageShrinkageLimit"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["ShLimTable"] = dt;
            grdShLim.DataSource = dt;
            grdShLim.DataBind();
            SetPreviousDataShLim();
        }
        protected void DeleteRowShLim(int rowIndex)
        {
            GetCurrentDataShLim();
            DataTable dt = ViewState["ShLimTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ShLimTable"] = dt;
            grdShLim.DataSource = dt;
            grdShLim.DataBind();
            SetPreviousDataShLim();
        }
        protected void SetPreviousDataShLim()
        {
            DataTable dt = (DataTable)ViewState["ShLimTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlShLim_NOOfDish = (DropDownList)grdShLim.Rows[i].FindControl("ddlShLim_NOOfDish");
                TextBox txtShLim_WeightOfDishPlusDrySoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusDrySoil");
                TextBox txtShLim_WeightOfDishPlusWetSoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusWetSoil");
                TextBox txtShLim_WeightOfWater = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWater");
                TextBox txtShLim_WeightOfEmptyDish = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfEmptyDish");
                TextBox txtShLim_WeightOfWetSoil_M2 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWetSoil_M2");
                TextBox txtShLim_WeightOfDrySoil_M1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDrySoil_M1");
                TextBox txtShLim_MoistureContentRatio_W1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentRatio_W1");
                TextBox txtShLim_MoistureContentPercent = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentPercent");
                TextBox txtShLim_WeightOfDishPlusMercuryFilling = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusMercuryFilling");
                TextBox txtShLim_WeightOfMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfMercury");
                TextBox txtShLim_VolumeOfWetSoilPat_V = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfWetSoilPat_V");
                TextBox txtShLim_WeightOfWeighingDishPlusDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishPlusDisplacedMercury");
                TextBox txtShLim_WeightOfWeighingDishEmpty = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishEmpty");
                TextBox txtShLim_WeightOfDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDisplacedMercury");
                TextBox txtShLim_VolumeOfDrySoilPat_Vd = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfDrySoilPat_Vd");
                TextBox txtShLim_ShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_ShrinkageLimit");
                TextBox txtShLim_AverageShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_AverageShrinkageLimit");

                if (dt.Rows[i]["ddlShLim_NOOfDish"].ToString() != "")
                    ddlShLim_NOOfDish.Items.FindByText(dt.Rows[i]["ddlShLim_NOOfDish"].ToString()).Selected = true;
                txtShLim_WeightOfDishPlusDrySoil.Text = dt.Rows[i]["txtShLim_WeightOfDishPlusDrySoil"].ToString();
                txtShLim_WeightOfDishPlusWetSoil.Text = dt.Rows[i]["txtShLim_WeightOfDishPlusWetSoil"].ToString();
                txtShLim_WeightOfWater.Text = dt.Rows[i]["txtShLim_WeightOfWater"].ToString();
                txtShLim_WeightOfEmptyDish.Text = dt.Rows[i]["txtShLim_WeightOfEmptyDish"].ToString();
                txtShLim_WeightOfWetSoil_M2.Text = dt.Rows[i]["txtShLim_WeightOfWetSoil_M2"].ToString();
                txtShLim_WeightOfDrySoil_M1.Text = dt.Rows[i]["txtShLim_WeightOfDrySoil_M1"].ToString();
                txtShLim_MoistureContentRatio_W1.Text = dt.Rows[i]["txtShLim_MoistureContentRatio_W1"].ToString();
                txtShLim_MoistureContentPercent.Text = dt.Rows[i]["txtShLim_MoistureContentPercent"].ToString();
                txtShLim_WeightOfDishPlusMercuryFilling.Text = dt.Rows[i]["txtShLim_WeightOfDishPlusMercuryFilling"].ToString();
                txtShLim_WeightOfMercury.Text = dt.Rows[i]["txtShLim_WeightOfMercury"].ToString();
                txtShLim_VolumeOfWetSoilPat_V.Text = dt.Rows[i]["txtShLim_VolumeOfWetSoilPat_V"].ToString();
                txtShLim_WeightOfWeighingDishPlusDisplacedMercury.Text = dt.Rows[i]["txtShLim_WeightOfWeighingDishPlusDisplacedMercury"].ToString();
                txtShLim_WeightOfWeighingDishEmpty.Text = dt.Rows[i]["txtShLim_WeightOfWeighingDishEmpty"].ToString();
                txtShLim_WeightOfDisplacedMercury.Text = dt.Rows[i]["txtShLim_WeightOfDisplacedMercury"].ToString();
                txtShLim_VolumeOfDrySoilPat_Vd.Text = dt.Rows[i]["txtShLim_VolumeOfDrySoilPat_Vd"].ToString();
                txtShLim_ShrinkageLimit.Text = dt.Rows[i]["txtShLim_ShrinkageLimit"].ToString();
                txtShLim_AverageShrinkageLimit.Text = dt.Rows[i]["txtShLim_AverageShrinkageLimit"].ToString();
            }
        }
        protected void GetCurrentDataShLim()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ddlShLim_NOOfDish", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfDishPlusDrySoil", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfDishPlusWetSoil", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfWater", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfEmptyDish", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfWetSoil_M2", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfDrySoil_M1", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_MoistureContentRatio_W1", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_MoistureContentPercent", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfDishPlusMercuryFilling", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfMercury", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_VolumeOfWetSoilPat_V", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfWeighingDishPlusDisplacedMercury", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfWeighingDishEmpty", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_WeightOfDisplacedMercury", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_VolumeOfDrySoilPat_Vd", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_ShrinkageLimit", typeof(string)));
            dt.Columns.Add(new DataColumn("txtShLim_AverageShrinkageLimit", typeof(string)));
            for (int i = 0; i < grdShLim.Rows.Count; i++)
            {
                DropDownList ddlShLim_NOOfDish = (DropDownList)grdShLim.Rows[i].FindControl("ddlShLim_NOOfDish");
                TextBox txtShLim_WeightOfDishPlusDrySoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusDrySoil");
                TextBox txtShLim_WeightOfDishPlusWetSoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusWetSoil");
                TextBox txtShLim_WeightOfWater = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWater");
                TextBox txtShLim_WeightOfEmptyDish = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfEmptyDish");
                TextBox txtShLim_WeightOfWetSoil_M2 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWetSoil_M2");
                TextBox txtShLim_WeightOfDrySoil_M1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDrySoil_M1");
                TextBox txtShLim_MoistureContentRatio_W1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentRatio_W1");
                TextBox txtShLim_MoistureContentPercent = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentPercent");
                TextBox txtShLim_WeightOfDishPlusMercuryFilling = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusMercuryFilling");
                TextBox txtShLim_WeightOfMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfMercury");
                TextBox txtShLim_VolumeOfWetSoilPat_V = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfWetSoilPat_V");
                TextBox txtShLim_WeightOfWeighingDishPlusDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishPlusDisplacedMercury");
                TextBox txtShLim_WeightOfWeighingDishEmpty = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishEmpty");
                TextBox txtShLim_WeightOfDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDisplacedMercury");
                TextBox txtShLim_VolumeOfDrySoilPat_Vd = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfDrySoilPat_Vd");
                TextBox txtShLim_ShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_ShrinkageLimit");
                TextBox txtShLim_AverageShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_AverageShrinkageLimit");

                dr = dt.NewRow();
                dr["ddlShLim_NOOfDish"] = ddlShLim_NOOfDish.SelectedItem.Text;
                dr["txtShLim_WeightOfDishPlusDrySoil"] = txtShLim_WeightOfDishPlusDrySoil.Text;
                dr["txtShLim_WeightOfDishPlusWetSoil"] = txtShLim_WeightOfDishPlusWetSoil.Text;
                dr["txtShLim_WeightOfWater"] = txtShLim_WeightOfWater.Text;
                dr["txtShLim_WeightOfEmptyDish"] = txtShLim_WeightOfEmptyDish.Text;
                dr["txtShLim_WeightOfWetSoil_M2"] = txtShLim_WeightOfWetSoil_M2.Text;
                dr["txtShLim_WeightOfDrySoil_M1"] = txtShLim_WeightOfDrySoil_M1.Text;
                dr["txtShLim_MoistureContentRatio_W1"] = txtShLim_MoistureContentRatio_W1.Text;
                dr["txtShLim_MoistureContentPercent"] = txtShLim_MoistureContentPercent.Text;
                dr["txtShLim_WeightOfDishPlusMercuryFilling"] = txtShLim_WeightOfDishPlusMercuryFilling.Text;
                dr["txtShLim_WeightOfMercury"] = txtShLim_WeightOfMercury.Text;
                dr["txtShLim_VolumeOfWetSoilPat_V"] = txtShLim_VolumeOfWetSoilPat_V.Text;
                dr["txtShLim_WeightOfWeighingDishPlusDisplacedMercury"] = txtShLim_WeightOfWeighingDishPlusDisplacedMercury.Text;
                dr["txtShLim_WeightOfWeighingDishEmpty"] = txtShLim_WeightOfWeighingDishEmpty.Text;
                dr["txtShLim_WeightOfDisplacedMercury"] = txtShLim_WeightOfDisplacedMercury.Text;
                dr["txtShLim_VolumeOfDrySoilPat_Vd"] = txtShLim_VolumeOfDrySoilPat_Vd.Text;
                dr["txtShLim_ShrinkageLimit"] = txtShLim_ShrinkageLimit.Text;
                dr["txtShLim_AverageShrinkageLimit"] = txtShLim_AverageShrinkageLimit.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["ShLimTable"] = dt;

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
            TabShLim.Visible = false;
            grdShLim.DataBind();
            grdShLim.DataSource = null;
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnesBy.Visible = false;
            ViewState["RemarkTable"] = null;
            ViewState["ShLimTable"] = null;
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
            TabShLim.Visible = false;
            //Test Details            
            int rowNo = 0;
            var test = dc.SoilSampleTest_View(txtRefNo.Text, txtSampleName.Text);
            foreach (var sotest in test)
            {
                rowNo = 0;
                if (sotest.TEST_Sr_No == 15)
                {
                    #region display Shrinkage Limit data

                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")
                        pnlShLim.Enabled = false;
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlShLim.Enabled = false;

                    if (sotest.SOSMPLTEST_Status_tint == 0)
                        TabShLim.HeaderText = TabShLim.HeaderText + " (Yet to Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabShLim.HeaderText = TabShLim.HeaderText + " (Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabShLim.HeaderText = TabShLim.HeaderText + " (Checked)";

                    TabShLim.Visible = true;
                    txtShLimRows.Text = sotest.SOSMPLTEST_Quantity_tint.ToString();
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtShLimDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtShLimDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtShLimDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtShLimDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var shlim = dc.SoilShrinkageLimit_View(txtRefNo.Text, txtSampleName.Text);
                    foreach (var soshlim in shlim)
                    {
                        //Shinkage Limit
                        AddRowShLim();
                        DropDownList ddlShLim_NOOfDish = (DropDownList)grdShLim.Rows[rowNo].FindControl("ddlShLim_NOOfDish");
                        TextBox txtShLim_WeightOfDishPlusDrySoil = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfDishPlusDrySoil");
                        TextBox txtShLim_WeightOfDishPlusWetSoil = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfDishPlusWetSoil");
                        TextBox txtShLim_WeightOfWater = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfWater");
                        TextBox txtShLim_WeightOfEmptyDish = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfEmptyDish");
                        TextBox txtShLim_WeightOfWetSoil_M2 = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfWetSoil_M2");
                        TextBox txtShLim_WeightOfDrySoil_M1 = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfDrySoil_M1");
                        TextBox txtShLim_MoistureContentRatio_W1 = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_MoistureContentRatio_W1");
                        TextBox txtShLim_MoistureContentPercent = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_MoistureContentPercent");
                        TextBox txtShLim_WeightOfDishPlusMercuryFilling = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfDishPlusMercuryFilling");
                        TextBox txtShLim_WeightOfMercury = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfMercury");
                        TextBox txtShLim_VolumeOfWetSoilPat_V = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_VolumeOfWetSoilPat_V");
                        TextBox txtShLim_WeightOfWeighingDishPlusDisplacedMercury = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfWeighingDishPlusDisplacedMercury");
                        TextBox txtShLim_WeightOfWeighingDishEmpty = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfWeighingDishEmpty");
                        TextBox txtShLim_WeightOfDisplacedMercury = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_WeightOfDisplacedMercury");
                        TextBox txtShLim_VolumeOfDrySoilPat_Vd = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_VolumeOfDrySoilPat_Vd");
                        TextBox txtShLim_ShrinkageLimit = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_ShrinkageLimit");
                        TextBox txtShLim_AverageShrinkageLimit = (TextBox)grdShLim.Rows[rowNo].FindControl("txtShLim_AverageShrinkageLimit");

                        ddlShLim_NOOfDish.Items.FindByText(Convert.ToDecimal(soshlim.SOShLim_NoOfDish_dec).ToString("0")).Selected = true;
                        txtShLim_WeightOfDishPlusDrySoil.Text = soshlim.SOShLim_WeightOfDishPlusDrySoil_dec.ToString();
                        txtShLim_WeightOfDishPlusWetSoil.Text = soshlim.SOShLim_WeightOfDishPlusWetSoil_dec.ToString();
                        txtShLim_WeightOfWater.Text = soshlim.SOShLim_WeightOfWater_dec.ToString();
                        txtShLim_WeightOfEmptyDish.Text = soshlim.SOShLim_WeightOfEmptyDish_dec.ToString();
                        txtShLim_WeightOfWetSoil_M2.Text = soshlim.SOShLim_WeightOfWetSoil_M2_dec.ToString();
                        txtShLim_WeightOfDrySoil_M1.Text = soshlim.SOShLim_WeightOfDrySoil_M1_dec.ToString();
                        txtShLim_MoistureContentRatio_W1.Text = soshlim.SOShLim_MoistureContentRatio_W1_dec.ToString();
                        txtShLim_MoistureContentPercent.Text = soshlim.SOShLim_MoistureContentPercent_dec.ToString();
                        txtShLim_WeightOfDishPlusMercuryFilling.Text = soshlim.SOShLim_WeightOfDishPlusMercuryFilling_dec.ToString();
                        txtShLim_WeightOfMercury.Text = soshlim.SOShLim_WeightOfMercury_dec.ToString();
                        txtShLim_VolumeOfWetSoilPat_V.Text = soshlim.SOShLim_VolumeOfWetSoilPat_V_dec.ToString();
                        txtShLim_WeightOfWeighingDishPlusDisplacedMercury.Text = soshlim.SOShLim_WeightOfWeighingDishPlusDisplacedMercury_dec.ToString();
                        txtShLim_WeightOfWeighingDishEmpty.Text = soshlim.SOShLim_WeightOfWeighingDishEmpty_dec.ToString();
                        txtShLim_WeightOfDisplacedMercury.Text = soshlim.SOShLim_WeightOfDisplacedMercury_dec.ToString();
                        txtShLim_VolumeOfDrySoilPat_Vd.Text = soshlim.SOShLim_VolumeOfDrySoilPat_dec.ToString();
                        txtShLim_ShrinkageLimit.Text = soshlim.SOShLim_ShrinkageLimit_dec.ToString();
                        if (rowNo == 0)
                            txtShLim_AverageShrinkageLimit.Text = soshlim.SOShLim_AverageShrinkageLimit_dec.ToString();
                        
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        ShLimRowsChanged();
                    }
                    else
                    {
                        txtShLimRows.Text = rowNo.ToString();
                    }
                    #endregion
                }

            }

            if (TabShLim.Visible == true)
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

        private void CalculatShLim()
        {
            decimal temp = 0, totShLim = 0;
            //LL Calculation
            for (int i = 0; i < grdShLim.Rows.Count; i++)
            {
                DropDownList ddlShLim_NOOfDish = (DropDownList)grdShLim.Rows[i].FindControl("ddlShLim_NOOfDish");
                TextBox txtShLim_WeightOfDishPlusDrySoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusDrySoil");
                TextBox txtShLim_WeightOfDishPlusWetSoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusWetSoil");
                TextBox txtShLim_WeightOfWater = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWater");
                TextBox txtShLim_WeightOfEmptyDish = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfEmptyDish");
                TextBox txtShLim_WeightOfWetSoil_M2 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWetSoil_M2");
                TextBox txtShLim_WeightOfDrySoil_M1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDrySoil_M1");
                TextBox txtShLim_MoistureContentRatio_W1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentRatio_W1");
                TextBox txtShLim_MoistureContentPercent = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentPercent");
                TextBox txtShLim_WeightOfDishPlusMercuryFilling = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusMercuryFilling");
                TextBox txtShLim_WeightOfMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfMercury");
                TextBox txtShLim_VolumeOfWetSoilPat_V = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfWetSoilPat_V");
                TextBox txtShLim_WeightOfWeighingDishPlusDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishPlusDisplacedMercury");
                TextBox txtShLim_WeightOfWeighingDishEmpty = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishEmpty");
                TextBox txtShLim_WeightOfDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDisplacedMercury");
                TextBox txtShLim_VolumeOfDrySoilPat_Vd = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfDrySoilPat_Vd");
                TextBox txtShLim_ShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_ShrinkageLimit");
                TextBox txtShLim_AverageShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_AverageShrinkageLimit");


                if (txtShLim_WeightOfDishPlusDrySoil.Text != "" && txtShLim_WeightOfDishPlusWetSoil.Text != ""
                    && txtShLim_WeightOfMercury.Text != "" && txtShLim_WeightOfDisplacedMercury.Text != "" && txtShLim_WeightOfWeighingDishEmpty.Text != "")
                {
                    temp = (Convert.ToDecimal(txtShLim_WeightOfDishPlusWetSoil.Text) - Convert.ToDecimal(txtShLim_WeightOfDishPlusDrySoil.Text));
                    if (temp < 0)
                        temp = 0;
                    txtShLim_WeightOfWater.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtShLim_WeightOfDishPlusWetSoil.Text) - Convert.ToDecimal(txtShLim_WeightOfEmptyDish.Text));
                    if (temp < 0)
                        temp = 0;
                    txtShLim_WeightOfWetSoil_M2.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtShLim_WeightOfDishPlusDrySoil.Text) - Convert.ToDecimal(txtShLim_WeightOfEmptyDish.Text));
                    if (temp < 0)
                        temp = 0;
                    txtShLim_WeightOfDrySoil_M1.Text = temp.ToString("0.0");

                    temp = 0;
                    if (Convert.ToDecimal(txtShLim_WeightOfDrySoil_M1.Text) > 0)
                    {
                        temp = Convert.ToDecimal(txtShLim_WeightOfWater.Text) / Convert.ToDecimal(txtShLim_WeightOfDrySoil_M1.Text);
                        if (temp < 0)
                            temp = 0;
                    }
                    txtShLim_MoistureContentRatio_W1.Text = temp.ToString("0.0");

                    temp = 0;
                    if (Convert.ToDecimal(txtShLim_WeightOfDrySoil_M1.Text) > 0)
                    {
                        temp = (Convert.ToDecimal(txtShLim_WeightOfWater.Text) / Convert.ToDecimal(txtShLim_WeightOfDrySoil_M1.Text))*100;
                        if (temp < 0)
                            temp = 0;
                    }
                    txtShLim_MoistureContentPercent.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtShLim_WeightOfMercury.Text) + Convert.ToDecimal(txtShLim_WeightOfWeighingDishEmpty.Text));
                    if (temp < 0)
                        temp = 0;
                    txtShLim_WeightOfDishPlusMercuryFilling.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtShLim_WeightOfMercury.Text) / Convert.ToDecimal("13.6"));
                    if (temp < 0)
                        temp = 0;
                    txtShLim_VolumeOfWetSoilPat_V.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtShLim_WeightOfWeighingDishEmpty.Text) + Convert.ToDecimal(txtShLim_WeightOfDisplacedMercury.Text));
                    if (temp < 0)
                        temp = 0;
                    txtShLim_WeightOfWeighingDishPlusDisplacedMercury.Text = temp.ToString("0.0");

                    temp = (Convert.ToDecimal(txtShLim_WeightOfDisplacedMercury.Text) / Convert.ToDecimal("13.6"));
                    if (temp < 0)
                        temp = 0;
                    txtShLim_VolumeOfDrySoilPat_Vd.Text = temp.ToString("0.0");

                    if (Convert.ToDecimal(txtShLim_WeightOfDrySoil_M1.Text) > 0)
                    {
                        temp = (Convert.ToDecimal(txtShLim_MoistureContentRatio_W1.Text) - (Convert.ToDecimal(txtShLim_VolumeOfWetSoilPat_V.Text) - Convert.ToDecimal(txtShLim_VolumeOfDrySoilPat_Vd.Text)) / Convert.ToDecimal(txtShLim_WeightOfDrySoil_M1.Text)) * 100;
                        if (temp < 0)
                            temp = 0;
                    }
                    txtShLim_ShrinkageLimit.Text = temp.ToString("0.00");

                    totShLim += Convert.ToDecimal(txtShLim_ShrinkageLimit.Text);                    
                }
                TextBox txtShLim_AverageShrinkageLimit_Result = (TextBox)grdShLim.Rows[0].FindControl("txtShLim_AverageShrinkageLimit");
                temp = 0;
                if (grdShLim.Rows.Count > 0)
                {
                    temp = totShLim / Convert.ToDecimal(txtShLimRows.Text);                    
                }
                txtShLim_AverageShrinkageLimit_Result.Text = temp.ToString("0.00");
            }
            
        }

        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            bool dataFlag = false;
            //validate Shrinkage limit data
            #region validate Shrinkage limit data
            if (TabShLim.Visible == true && valid == true && TabShLim.Enabled == true)
            {
                if (txtShLimRows.Text == "" || txtShLimRows.Text == "0")
                {
                    dispalyMsg = "Shrinkage Limit Rows can not be zero or blank.";
                    txtShLimRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                //date validation
                else if (txtShLimDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtShLimDateOfTesting.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtShLimDateOfTesting.Text != "")
                {
                    string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime TestingDate = DateTime.ParseExact(txtShLimDateOfTesting.Text, "dd/MM/yyyy", null);
                    DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                    if (TestingDate > CurrentDate)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtShLimDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdShLim.Rows.Count; i++)
                    {
                        dataFlag = true;
                        DropDownList ddlShLim_NOOfDish = (DropDownList)grdShLim.Rows[i].FindControl("ddlShLim_NOOfDish");
                        TextBox txtShLim_WeightOfDishPlusDrySoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusDrySoil");
                        TextBox txtShLim_WeightOfDishPlusWetSoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusWetSoil");
                        TextBox txtShLim_WeightOfMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfMercury");
                        TextBox txtShLim_WeightOfWeighingDishEmpty = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishEmpty");
                        TextBox txtShLim_WeightOfDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDisplacedMercury");

                        if (ddlShLim_NOOfDish.SelectedIndex <= 0)
                        {
                            dispalyMsg = "Select No. of Dish for Shrinkage Limit for row no " + (i + 1) + ".";
                            ddlShLim_NOOfDish.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        if (txtShLim_WeightOfDishPlusDrySoil.Text == "")
                        {
                            dispalyMsg = "Enter Weight of Dish + Dry soil for Shrinkage Limit for row number " + (i + 1) + ".";
                            txtShLim_WeightOfDishPlusDrySoil.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtShLim_WeightOfDishPlusWetSoil.Text == "")
                        {
                            dispalyMsg = "Enter Weight of Dish + Wet soil for Shrinkage Limit for row number " + (i + 1) + ".";
                            txtShLim_WeightOfDishPlusWetSoil.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtShLim_WeightOfMercury.Text == "")
                        {
                            dispalyMsg = "Enter Weight of Mercury for Shrinkage Limit for row number " + (i + 1) + ".";
                            txtShLim_WeightOfMercury.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtShLim_WeightOfWeighingDishEmpty.Text == "")
                        {
                            dispalyMsg = "Enter Weight of Weighing Dish empty for Shrinkage Limit for row number " + (i + 1) + ".";
                            txtShLim_WeightOfWeighingDishEmpty.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtShLim_WeightOfDisplacedMercury.Text == "")
                        {
                            dispalyMsg = "Enter Weight of Displaced Mercury for Shrinkage Limit for row number " + (i + 1) + ".";
                            txtShLim_WeightOfDisplacedMercury.Focus();
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

                if (TabShLim.Visible == true)
                    testingDate = DateTime.ParseExact(txtShLimDateOfTesting.Text, "dd/MM/yyyy", null);

                //test data update

                // Shrinkage limit
                #region save Shrinkage limit data
                if (TabShLim.Visible == true && pnlShLim.Enabled == true)
                {
                    decimal ShLim = 0;
                    dc.SoilShrinkageLimit_Update(0, txtRefNo.Text, txtSampleName.Text, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
                    for (int i = 0; i < grdShLim.Rows.Count; i++)
                    {
                        DropDownList ddlShLim_NOOfDish = (DropDownList)grdShLim.Rows[i].FindControl("ddlShLim_NOOfDish");
                        TextBox txtShLim_WeightOfDishPlusDrySoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusDrySoil");
                        TextBox txtShLim_WeightOfDishPlusWetSoil = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusWetSoil");
                        TextBox txtShLim_WeightOfWater = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWater");
                        TextBox txtShLim_WeightOfEmptyDish = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfEmptyDish");
                        TextBox txtShLim_WeightOfWetSoil_M2 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWetSoil_M2");
                        TextBox txtShLim_WeightOfDrySoil_M1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDrySoil_M1");
                        TextBox txtShLim_MoistureContentRatio_W1 = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentRatio_W1");
                        TextBox txtShLim_MoistureContentPercent = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_MoistureContentPercent");
                        TextBox txtShLim_WeightOfDishPlusMercuryFilling = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDishPlusMercuryFilling");
                        TextBox txtShLim_WeightOfMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfMercury");
                        TextBox txtShLim_VolumeOfWetSoilPat_V = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfWetSoilPat_V");
                        TextBox txtShLim_WeightOfWeighingDishPlusDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishPlusDisplacedMercury");
                        TextBox txtShLim_WeightOfWeighingDishEmpty = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfWeighingDishEmpty");
                        TextBox txtShLim_WeightOfDisplacedMercury = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_WeightOfDisplacedMercury");
                        TextBox txtShLim_VolumeOfDrySoilPat_Vd = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_VolumeOfDrySoilPat_Vd");
                        TextBox txtShLim_ShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_ShrinkageLimit");
                        TextBox txtShLim_AverageShrinkageLimit = (TextBox)grdShLim.Rows[i].FindControl("txtShLim_AverageShrinkageLimit");

                        ShLim = 0;
                        if (i == 0)
                        {
                            ShLim = Convert.ToDecimal(txtShLim_AverageShrinkageLimit.Text);
                        }

                        dc.SoilShrinkageLimit_Update(i + 1, txtRefNo.Text, txtSampleName.Text, Convert.ToDecimal(ddlShLim_NOOfDish.SelectedItem.Text), Convert.ToDecimal(txtShLim_WeightOfDishPlusDrySoil.Text), Convert.ToDecimal(txtShLim_WeightOfDishPlusWetSoil.Text), Convert.ToDecimal(txtShLim_WeightOfWater.Text)
                           , Convert.ToDecimal(txtShLim_WeightOfEmptyDish.Text), Convert.ToDecimal(txtShLim_WeightOfWetSoil_M2.Text), Convert.ToDecimal(txtShLim_WeightOfDrySoil_M1.Text), Convert.ToDecimal(txtShLim_MoistureContentRatio_W1.Text), Convert.ToDecimal(txtShLim_MoistureContentPercent.Text), Convert.ToDecimal(txtShLim_WeightOfDishPlusMercuryFilling.Text)
                           , Convert.ToDecimal(txtShLim_WeightOfMercury.Text), Convert.ToDecimal(txtShLim_VolumeOfWetSoilPat_V.Text), Convert.ToDecimal(txtShLim_WeightOfWeighingDishPlusDisplacedMercury.Text), Convert.ToDecimal(txtShLim_WeightOfWeighingDishEmpty.Text), Convert.ToDecimal(txtShLim_WeightOfDisplacedMercury.Text), Convert.ToDecimal(txtShLim_VolumeOfDrySoilPat_Vd.Text), Convert.ToDecimal(txtShLim_ShrinkageLimit.Text), ShLim, false);
                    }
                    
                    var test = dc.Test(15, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    testingDate = DateTime.ParseExact(txtShLimDateOfTesting.Text, "dd/MM/yyyy", null);
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtShLimRows.Text), true);
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
            if (TabShLim.Visible == true && TabShLim.Enabled == true)
                CalculatShLim();
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

        protected void grdShLim_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlShLim_NOOfDish = (e.Row.FindControl("ddlShLim_NOOfDish") as DropDownList);
                var soset = dc.SoilSetting_View("Weight of empty Dish");
                ddlShLim_NOOfDish.DataSource = soset;
                ddlShLim_NOOfDish.DataTextField = "SOSET_F1_var";
                ddlShLim_NOOfDish.DataValueField = "SOSET_F2_var";
                ddlShLim_NOOfDish.DataBind();
                if (ddlShLim_NOOfDish.Items.Count > 0)
                    ddlShLim_NOOfDish.Items.Insert(0, new ListItem("Select", "0"));

            }
        }
                
        protected void ddlShLim_NOOfDish_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtShLim_WeightOfEmptyDish = (TextBox)gvr.FindControl("txtShLim_WeightOfEmptyDish");
                DropDownList ddlShLim_NOOfDish = (DropDownList)gvr.FindControl("ddlShLim_NOOfDish");
                if (ddlShLim_NOOfDish.SelectedIndex > 0)
                {
                    txtShLim_WeightOfEmptyDish.Text = ddlShLim_NOOfDish.SelectedItem.Value;
                }
            }
        }
                
    }
}