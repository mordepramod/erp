using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Soil_Report_LLPLPI : System.Web.UI.Page
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
        
        protected void txtLLRows_TextChanged(object sender, EventArgs e)
        {
            LLRowsChanged();
        }

        private void LLRowsChanged()
        {
            if (txtLLRows.Text != "")
            {
                if (Convert.ToInt32(txtLLRows.Text) < grdLL.Rows.Count)
                {
                    for (int i = grdLL.Rows.Count; i > Convert.ToInt32(txtLLRows.Text); i--)
                    {
                        if (grdLL.Rows.Count > 1)
                        {
                            DeleteRowLL(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtLLRows.Text) > grdLL.Rows.Count)
                {
                    for (int i = grdLL.Rows.Count + 1; i <= Convert.ToInt32(txtLLRows.Text); i++)
                    {
                        AddRowLL();
                    }
                }
            }
        }

        protected void txtPLRows_TextChanged(object sender, EventArgs e)
        {
            PLRowsChanged();
        }

        private void PLRowsChanged()
        {
            if (txtPLRows.Text != "")
            {
                if (Convert.ToInt32(txtPLRows.Text) < grdPL.Rows.Count)
                {
                    for (int i = grdPL.Rows.Count; i > Convert.ToInt32(txtPLRows.Text); i--)
                    {
                        if (grdPL.Rows.Count > 1)
                        {
                            DeleteRowPL(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtPLRows.Text) > grdPL.Rows.Count)
                {
                    for (int i = grdPL.Rows.Count + 1; i <= Convert.ToInt32(txtPLRows.Text); i++)
                    {
                        AddRowPL();
                    }
                }
            }
        }
        
        #region add/delete rows grdLL grid
        protected void AddRowLL()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["LLTable"] != null)
            {
                GetCurrentDataLL();
                dt = (DataTable)ViewState["LLTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddlLLContNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLNoOfBlows", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLMassWetSPlusCont", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLMassDrySPlusCont", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLMassOfCont", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLMassOfMoist", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLMassOfDryS", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLMoist", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLLLiquidLimit", typeof(string)));
            }

            dr = dt.NewRow();
            dr["ddlLLContNo"] = string.Empty;
            dr["txtLLNoOfBlows"] = string.Empty;
            dr["txtLLMassWetSPlusCont"] = string.Empty;
            dr["txtLLMassDrySPlusCont"] = string.Empty;
            dr["txtLLMassOfCont"] = string.Empty;
            dr["txtLLMassOfMoist"] = string.Empty;
            dr["txtLLMassOfDryS"] = string.Empty;
            dr["txtLLMoist"] = string.Empty;
            dr["txtLLLiquidLimit"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["LLTable"] = dt;
            grdLL.DataSource = dt;
            grdLL.DataBind();
            SetPreviousDataLL();
        }
        protected void DeleteRowLL(int rowIndex)
        {
            GetCurrentDataLL();
            DataTable dt = ViewState["LLTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["LLTable"] = dt;
            grdLL.DataSource = dt;
            grdLL.DataBind();
            SetPreviousDataLL();
        }
        protected void SetPreviousDataLL()
        {
            DataTable dt = (DataTable)ViewState["LLTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlLLContNo = (DropDownList)grdLL.Rows[i].FindControl("ddlLLContNo");
                TextBox txtLLNoOfBlows = (TextBox)grdLL.Rows[i].FindControl("txtLLNoOfBlows");
                TextBox txtLLMassWetSPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassWetSPlusCont");
                TextBox txtLLMassDrySPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassDrySPlusCont");
                TextBox txtLLMassOfCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfCont");
                TextBox txtLLMassOfMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfMoist");
                TextBox txtLLMassOfDryS = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfDryS");
                TextBox txtLLMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMoist");
                TextBox txtLLLiquidLimit = (TextBox)grdLL.Rows[i].FindControl("txtLLLiquidLimit");

                if (dt.Rows[i]["ddlLLContNo"].ToString()!="")
                    ddlLLContNo.Items.FindByText(dt.Rows[i]["ddlLLContNo"].ToString()).Selected = true;
                txtLLNoOfBlows.Text = dt.Rows[i]["txtLLNoOfBlows"].ToString();
                txtLLMassWetSPlusCont.Text = dt.Rows[i]["txtLLMassWetSPlusCont"].ToString();
                txtLLMassDrySPlusCont.Text = dt.Rows[i]["txtLLMassDrySPlusCont"].ToString();
                txtLLMassOfCont.Text = dt.Rows[i]["txtLLMassOfCont"].ToString();
                txtLLMassOfMoist.Text = dt.Rows[i]["txtLLMassOfMoist"].ToString();
                txtLLMassOfDryS.Text = dt.Rows[i]["txtLLMassOfDryS"].ToString();
                txtLLMoist.Text = dt.Rows[i]["txtLLMoist"].ToString();
                txtLLLiquidLimit.Text = dt.Rows[i]["txtLLLiquidLimit"].ToString();
            }
        }
        protected void GetCurrentDataLL()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ddlLLContNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLNoOfBlows", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLMassWetSPlusCont", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLMassDrySPlusCont", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLMassOfCont", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLMassOfMoist", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLMassOfDryS", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLMoist", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLLLiquidLimit", typeof(string)));
            for (int i = 0; i < grdLL.Rows.Count; i++)
            {
                DropDownList ddlLLContNo = (DropDownList)grdLL.Rows[i].FindControl("ddlLLContNo");
                TextBox txtLLNoOfBlows = (TextBox)grdLL.Rows[i].FindControl("txtLLNoOfBlows");
                TextBox txtLLMassWetSPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassWetSPlusCont");
                TextBox txtLLMassDrySPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassDrySPlusCont");
                TextBox txtLLMassOfCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfCont");
                TextBox txtLLMassOfMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfMoist");
                TextBox txtLLMassOfDryS = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfDryS");
                TextBox txtLLMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMoist");
                TextBox txtLLLiquidLimit = (TextBox)grdLL.Rows[i].FindControl("txtLLLiquidLimit");

                dr = dt.NewRow();
                dr["ddlLLContNo"] = ddlLLContNo.SelectedItem.Text ;
                dr["txtLLNoOfBlows"] = txtLLNoOfBlows.Text;
                dr["txtLLMassWetSPlusCont"] = txtLLMassWetSPlusCont.Text;
                dr["txtLLMassDrySPlusCont"] = txtLLMassDrySPlusCont.Text;
                dr["txtLLMassOfCont"] = txtLLMassOfCont.Text;
                dr["txtLLMassOfMoist"] = txtLLMassOfMoist.Text;
                dr["txtLLMassOfDryS"] = txtLLMassOfDryS.Text;
                dr["txtLLMoist"] = txtLLMoist.Text;
                dr["txtLLLiquidLimit"] = txtLLLiquidLimit.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["LLTable"] = dt;

        }
        #endregion

        #region add/delete rows grdPL grid
        protected void AddRowPL()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PLTable"] != null)
            {
                GetCurrentDataPL();
                dt = (DataTable)ViewState["PLTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddlPLContNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPLMassWetSPlusCont", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPLMassDrySPlusCont", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPLMassOfCont", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPLMassOfMoist", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPLMassOfDryS", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPLMoist", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPLPlasticLimit", typeof(string)));
            }

            dr = dt.NewRow();
            dr["ddlPLContNo"] = string.Empty;
            dr["txtPLMassWetSPlusCont"] = string.Empty;
            dr["txtPLMassDrySPlusCont"] = string.Empty;
            dr["txtPLMassOfCont"] = string.Empty;
            dr["txtPLMassOfMoist"] = string.Empty;
            dr["txtPLMassOfDryS"] = string.Empty;
            dr["txtPLMoist"] = string.Empty;
            dr["txtPLPlasticLimit"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["PLTable"] = dt;
            grdPL.DataSource = dt;
            grdPL.DataBind();
            SetPreviousDataPL();
        }
        protected void DeleteRowPL(int rowIndex)
        {
            GetCurrentDataPL();
            DataTable dt = ViewState["PLTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PLTable"] = dt;
            grdPL.DataSource = dt;
            grdPL.DataBind();
            SetPreviousDataPL();
        }
        protected void SetPreviousDataPL()
        {
            DataTable dt = (DataTable)ViewState["PLTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlPLContNo = (DropDownList)grdPL.Rows[i].FindControl("ddlPLContNo");
                TextBox txtPLMassWetSPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassWetSPlusCont");
                TextBox txtPLMassDrySPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassDrySPlusCont");
                TextBox txtPLMassOfCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfCont");
                TextBox txtPLMassOfMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfMoist");
                TextBox txtPLMassOfDryS = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfDryS");
                TextBox txtPLMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMoist");
                TextBox txtPLPlasticLimit = (TextBox)grdPL.Rows[i].FindControl("txtPLPlasticLimit");

                if (dt.Rows[i]["ddlPLContNo"].ToString() != "")
                    ddlPLContNo.Items.FindByText(dt.Rows[i]["ddlPLContNo"].ToString()).Selected = true;
                txtPLMassWetSPlusCont.Text = dt.Rows[i]["txtPLMassWetSPlusCont"].ToString();
                txtPLMassDrySPlusCont.Text = dt.Rows[i]["txtPLMassDrySPlusCont"].ToString();
                txtPLMassOfCont.Text = dt.Rows[i]["txtPLMassOfCont"].ToString();
                txtPLMassOfMoist.Text = dt.Rows[i]["txtPLMassOfMoist"].ToString();
                txtPLMassOfDryS.Text = dt.Rows[i]["txtPLMassOfDryS"].ToString();
                txtPLMoist.Text = dt.Rows[i]["txtPLMoist"].ToString();
                txtPLPlasticLimit.Text = dt.Rows[i]["txtPLPlasticLimit"].ToString();
            }
        }
        protected void GetCurrentDataPL()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ddlPLContNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPLMassWetSPlusCont", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPLMassDrySPlusCont", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPLMassOfCont", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPLMassOfMoist", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPLMassOfDryS", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPLMoist", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPLPlasticLimit", typeof(string)));
            for (int i = 0; i < grdPL.Rows.Count; i++)
            {
                DropDownList ddlPLContNo = (DropDownList)grdPL.Rows[i].FindControl("ddlPLContNo");
                TextBox txtPLMassWetSPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassWetSPlusCont");
                TextBox txtPLMassDrySPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassDrySPlusCont");
                TextBox txtPLMassOfCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfCont");
                TextBox txtPLMassOfMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfMoist");
                TextBox txtPLMassOfDryS = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfDryS");
                TextBox txtPLMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMoist");
                TextBox txtPLPlasticLimit = (TextBox)grdPL.Rows[i].FindControl("txtPLPlasticLimit");

                dr = dt.NewRow();
                dr["ddlPLContNo"] = ddlPLContNo.SelectedItem.Text;
                dr["txtPLMassWetSPlusCont"] = txtPLMassWetSPlusCont.Text;
                dr["txtPLMassDrySPlusCont"] = txtPLMassDrySPlusCont.Text;
                dr["txtPLMassOfCont"] = txtPLMassOfCont.Text;
                dr["txtPLMassOfMoist"] = txtPLMassOfMoist.Text;
                dr["txtPLMassOfDryS"] = txtPLMassOfDryS.Text;
                dr["txtPLMoist"] = txtPLMoist.Text;
                dr["txtPLPlasticLimit"] = txtPLPlasticLimit.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["PLTable"] = dt;

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
            TabLLPL.Visible = false;
            grdLL.DataBind();
            grdLL.DataSource = null;
            grdPL.DataBind();
            grdPL.DataSource = null;
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnesBy.Visible = false;
            ViewState["RemarkTable"] = null;
            ViewState["LLTable"] = null;
            ViewState["PLTable"] = null;
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
            TabLLPL.Visible = false;            
            //Test Details            
            int rowNo = 0;
            var test = dc.SoilSampleTest_View(txtRefNo.Text,txtSampleName.Text);
            foreach (var sotest in test)
            {
                rowNo = 0;
                if (sotest.TEST_Sr_No == 5)
                {
                    #region display LLPl data
                   
                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")                    
                        pnlLLPL.Enabled = false;                    
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlLLPL.Enabled = false;
                    
                    if (sotest.SOSMPLTEST_Status_tint == 0 )
                        TabLLPL.HeaderText = TabLLPL.HeaderText + " (Yet to Entered)";                        
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabLLPL.HeaderText = TabLLPL.HeaderText + " (Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabLLPL.HeaderText = TabLLPL.HeaderText + " (Checked)";

                    TabLLPL.Visible = true;                    
                    txtLLRows.Text = sotest.SOSMPLTEST_Quantity_tint.ToString();
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtLLPLDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtLLPLDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtLLPLDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtLLPLDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    var llpl = dc.SoilLLPL_View(txtRefNo.Text, txtSampleName.Text,false);
                    foreach (var sollpl in llpl)
                    {
                        //Liquid Limit
                        AddRowLL();
                        DropDownList ddlLLContNo = (DropDownList)grdLL.Rows[rowNo].FindControl("ddlLLContNo");
                        TextBox txtLLNoOfBlows = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLNoOfBlows");
                        TextBox txtLLMassWetSPlusCont = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLMassWetSPlusCont");
                        TextBox txtLLMassDrySPlusCont = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLMassDrySPlusCont");
                        TextBox txtLLMassOfCont = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLMassOfCont");
                        TextBox txtLLMassOfMoist = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLMassOfMoist");
                        TextBox txtLLMassOfDryS = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLMassOfDryS");
                        TextBox txtLLMoist = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLMoist");
                        TextBox txtLLLiquidLimit = (TextBox)grdLL.Rows[rowNo].FindControl("txtLLLiquidLimit");

                        ddlLLContNo.Items.FindByText(Convert.ToDecimal(sollpl.SOLLPL_ContNo_dec).ToString("0")).Selected = true;
                        txtLLNoOfBlows.Text = sollpl.SOLLPL_NoOfBlows_dec.ToString();
                        txtLLMassWetSPlusCont.Text = sollpl.SOLLPL_MassWetSPlusCont_dec.ToString();
                        txtLLMassDrySPlusCont.Text = sollpl.SOLLPL_MassDrySPlusCont_dec.ToString();
                        txtLLMassOfCont.Text = sollpl.SOLLPL_MassOfCont_dec.ToString();
                        txtLLMassOfMoist.Text = sollpl.SOLLPL_MassOfMoist_dec.ToString();
                        txtLLMassOfDryS.Text = sollpl.SOLLPL_MassOfDryS_dec.ToString();
                        txtLLMoist.Text = sollpl.SOLLPL_Moist_dec.ToString();
                        txtLLLiquidLimit.Text = sollpl.SOLLPL_LLPL_var;
                        
                        if (rowNo == 0)
                        {
                            txtPI.Text = sollpl.SOLLPL_PI_dec.ToString();                           
                        }
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        LLRowsChanged();
                    }
                    else
                    {
                        txtLLRows.Text = rowNo.ToString();
                    }
                    rowNo = 0;
                    var pl = dc.SoilLLPL_View(txtRefNo.Text, txtSampleName.Text, true);
                    foreach (var sopl in pl)
                    {                        
                        //Plastic Limit
                        AddRowPL();
                        DropDownList ddlPLContNo = (DropDownList)grdPL.Rows[rowNo].FindControl("ddlPLContNo");
                        TextBox txtPLMassWetSPlusCont = (TextBox)grdPL.Rows[rowNo].FindControl("txtPLMassWetSPlusCont");
                        TextBox txtPLMassDrySPlusCont = (TextBox)grdPL.Rows[rowNo].FindControl("txtPLMassDrySPlusCont");
                        TextBox txtPLMassOfCont = (TextBox)grdPL.Rows[rowNo].FindControl("txtPLMassOfCont");
                        TextBox txtPLMassOfMoist = (TextBox)grdPL.Rows[rowNo].FindControl("txtPLMassOfMoist");
                        TextBox txtPLMassOfDryS = (TextBox)grdPL.Rows[rowNo].FindControl("txtPLMassOfDryS");
                        TextBox txtPLMoist = (TextBox)grdPL.Rows[rowNo].FindControl("txtPLMoist");
                        TextBox txtPLPlasticLimit = (TextBox)grdPL.Rows[rowNo].FindControl("txtPLPlasticLimit");

                        ddlPLContNo.Items.FindByText(Convert.ToDecimal(sopl.SOLLPL_ContNo_dec).ToString("0")).Selected = true;
                        txtPLMassWetSPlusCont.Text = sopl.SOLLPL_MassWetSPlusCont_dec.ToString();
                        txtPLMassDrySPlusCont.Text = sopl.SOLLPL_MassDrySPlusCont_dec.ToString();
                        txtPLMassOfCont.Text = sopl.SOLLPL_MassOfCont_dec.ToString();
                        txtPLMassOfMoist.Text = sopl.SOLLPL_MassOfMoist_dec.ToString();
                        txtPLMassOfDryS.Text = sopl.SOLLPL_MassOfDryS_dec.ToString();
                        txtPLMoist.Text = sopl.SOLLPL_Moist_dec.ToString();
                        txtPLPlasticLimit.Text = sopl.SOLLPL_LLPL_var;
                                                
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        PLRowsChanged();
                    }
                    else
                    {
                        txtPLRows.Text = rowNo.ToString();
                    }
                    #endregion
                }
                
            }
            
            if (TabLLPL.Visible == true)
                TabContainerSoil.ActiveTabIndex = 0;

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

        private void CalculateLLPL()
        {
            decimal temp = 0, MoistPer = 0, ConePentr = 0, totMoistPer = 0, totConePentr = 0, LL = 0, PL=0;
            //LL Calculation
            for (int i = 0; i < grdLL.Rows.Count; i++)
            {
                TextBox txtLLNoOfBlows = (TextBox)grdLL.Rows[i].FindControl("txtLLNoOfBlows");
                TextBox txtLLMassWetSPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassWetSPlusCont"); //3
                TextBox txtLLMassDrySPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassDrySPlusCont"); //4
                TextBox txtLLMassOfCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfCont"); //5
                TextBox txtLLMassOfMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfMoist"); //6
                TextBox txtLLMassOfDryS = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfDryS"); //7
                TextBox txtLLMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMoist"); //8                
                TextBox txtLLLiquidLimit = (TextBox)grdLL.Rows[i].FindControl("txtLLLiquidLimit"); //9

                if (txtLLNoOfBlows.Text != "" && txtLLMassWetSPlusCont.Text != "" 
                    && txtLLMassDrySPlusCont.Text != "" && txtLLMassOfCont.Text != "" )
                {
                    temp = (Convert.ToDecimal(txtLLMassWetSPlusCont.Text) - Convert.ToDecimal(txtLLMassDrySPlusCont.Text));
                    if (temp < 0)
                        temp = 0;
                    txtLLMassOfMoist.Text = temp.ToString("0.000");

                    temp = (Convert.ToDecimal(txtLLMassDrySPlusCont.Text) - Convert.ToDecimal(txtLLMassOfCont.Text));
                    if (temp < 0)
                        temp = 0;
                    txtLLMassOfDryS.Text = temp.ToString("0.0");

                    temp = 0;
                    if (Convert.ToDecimal(txtLLMassOfDryS.Text) > 0)
                    {
                        temp = (Convert.ToDecimal(txtLLMassOfMoist.Text) / Convert.ToDecimal(txtLLMassOfDryS.Text)) * 100;
                        if (temp < 0)
                            temp = 0;
                    }
                    txtLLMoist.Text = temp.ToString("0");
                    MoistPer = Convert.ToDecimal(txtLLMoist.Text);
                    ConePentr = (MoistPer / (Convert.ToDecimal(0.65) + (Convert.ToDecimal(0.0175) * Convert.ToDecimal(txtLLNoOfBlows.Text))));
                    totMoistPer = totMoistPer + MoistPer;
                    totConePentr = totConePentr + ConePentr;
                }
                txtLLLiquidLimit.Text = "";
            }
            if (grdLL.Rows.Count > 0)
            {
                TextBox txtLLLiquidLimit = (TextBox)grdLL.Rows[grdLL.Rows.Count / 2].FindControl("txtLLLiquidLimit"); //9
                if (grdLL.Rows.Count == 2)
                {
                    txtLLLiquidLimit.Text = (totConePentr / Convert.ToDecimal(grdLL.Rows.Count)).ToString("0");
                }
                else
                {
                    txtLLLiquidLimit.Text = (totMoistPer / Convert.ToDecimal(grdLL.Rows.Count)).ToString("0");
                }
                LL= Convert.ToDecimal(txtLLLiquidLimit.Text);
            }
            //PL Calculation
            totMoistPer = 0;
            for (int i = 0; i < grdPL.Rows.Count; i++)
            {
                TextBox txtPLMassWetSPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassWetSPlusCont"); //2
                TextBox txtPLMassDrySPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassDrySPlusCont"); //3
                TextBox txtPLMassOfCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfCont"); //4
                TextBox txtPLMassOfMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfMoist"); //5
                TextBox txtPLMassOfDryS = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfDryS"); //6
                TextBox txtPLMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMoist"); //7 
                TextBox txtPLPlasticLimit = (TextBox)grdPL.Rows[grdPL.Rows.Count / 2].FindControl("txtPLPlasticLimit"); //8

                if (txtPLMassWetSPlusCont.Text != "" && txtPLMassDrySPlusCont.Text != "" &&
                    txtPLMassOfCont.Text != "" )
                {
                    temp = (Convert.ToDecimal(txtPLMassWetSPlusCont.Text) - Convert.ToDecimal(txtPLMassDrySPlusCont.Text));
                    if (temp < 0)
                        temp = 0;
                    txtPLMassOfMoist.Text = temp.ToString("0.000");

                    temp = (Convert.ToDecimal(txtPLMassDrySPlusCont.Text) - Convert.ToDecimal(txtPLMassOfCont.Text));
                    if (temp < 0)
                        temp = 0;
                    txtPLMassOfDryS.Text = temp.ToString("0.0");

                    temp = 0;
                    if (Convert.ToDecimal(txtPLMassOfDryS.Text) > 0)
                    {
                        temp = (Convert.ToDecimal(txtPLMassOfMoist.Text) / Convert.ToDecimal(txtPLMassOfDryS.Text)) * 100;
                        if (temp < 0)
                            temp = 0;
                    }
                    txtPLMoist.Text = temp.ToString("0");
                    MoistPer = Convert.ToDecimal(txtPLMoist.Text);
                    totMoistPer = totMoistPer + MoistPer;                    
                }
                txtPLPlasticLimit.Text = "";
            }
            if (grdPL.Rows.Count > 0)
            {
                TextBox txtPLPlasticLimit = (TextBox)grdPL.Rows[grdPL.Rows.Count / 2].FindControl("txtPLPlasticLimit"); //8                
                txtPLPlasticLimit.Text = (totMoistPer / Convert.ToDecimal(grdPL.Rows.Count)).ToString("0");
                PL= Convert.ToDecimal(txtPLPlasticLimit.Text);
            }
            if (grdLL.Rows.Count > 0 && grdPL.Rows.Count > 0)
            { 
                temp=0;
                if (LL  > PL)
                    temp = LL - PL;

                txtPI.Text = temp.ToString("0");                
            }

        }
                
        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            bool dataFlag = false;     
            //validate LL/PL data
            #region validate LL/PL data
            if (TabLLPL.Visible == true && valid == true && TabLLPL.Enabled == true)
            {
                if (txtLLRows.Text == "" || txtLLRows.Text == "0")
                {
                    dispalyMsg = "Liquid Limit Rows can not be zero or blank.";
                    txtLLRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtPLRows.Text == "" || txtPLRows.Text == "0")
                {
                    dispalyMsg = "Plastic Limit Rows can not be zero or blank.";
                    txtPLRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }  
                //date validation
                else if (txtLLPLDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtLLPLDateOfTesting.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtLLPLDateOfTesting.Text != "")
                {
                    //DateTime dateTest = DateTime.Now;
                    //dateTest = Convert.ToDateTime(txtLLPLDateOfTesting.Text);
                    string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime TestingDate = DateTime.ParseExact(txtLLPLDateOfTesting.Text, "dd/MM/yyyy", null);
                    DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                    //if (dateTest > System.DateTime.Now)
                    if (TestingDate > CurrentDate)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtLLPLDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdLL.Rows.Count; i++)
                    {
                        dataFlag = true;
                        DropDownList ddlLLContNo = (DropDownList)grdLL.Rows[i].FindControl("ddlLLContNo"); //1
                        TextBox txtLLNoOfBlows = (TextBox)grdLL.Rows[i].FindControl("txtLLNoOfBlows"); //2
                        TextBox txtLLMassWetSPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassWetSPlusCont"); //3
                        TextBox txtLLMassDrySPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassDrySPlusCont"); //4                        

                        if (ddlLLContNo.SelectedIndex <= 0)
                        {
                            dispalyMsg = "Select Container No. for Liquid Limit for row no " + (i + 1) + ".";
                            ddlLLContNo.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        if (txtLLNoOfBlows.Text == "")
                        {
                            dispalyMsg = "Enter No Of Blows for Liquid Limit for row number " + (i + 1) + ".";
                            txtLLNoOfBlows.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtLLMassWetSPlusCont.Text == "")
                        {
                            dispalyMsg = "Enter Mass Wet S + Cont for Liquid Limit for row number " + (i + 1) + ".";
                            txtLLMassWetSPlusCont.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtLLMassDrySPlusCont.Text == "")
                        {
                            dispalyMsg = "Enter Mass Dry S + Cont for Liquid Limit for row number " + (i + 1) + ".";
                            txtLLMassDrySPlusCont.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }                        
                    }
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdPL.Rows.Count; i++)
                    {
                        dataFlag = true;
                        DropDownList ddlPLContNo = (DropDownList)grdPL.Rows[i].FindControl("ddlPLContNo"); //1
                        TextBox txtPLMassWetSPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassWetSPlusCont"); //2
                        TextBox txtPLMassDrySPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassDrySPlusCont"); //3                        

                        if (ddlPLContNo.SelectedIndex <= 0)
                        {
                            dispalyMsg = "Select Container No. for Plastic Limit for row no " + (i + 1) + ".";
                            ddlPLContNo.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }                       
                        else if (txtPLMassWetSPlusCont.Text == "")
                        {
                            dispalyMsg = "Enter Mass Wet S + Cont for Plastic Limit for row number " + (i + 1) + ".";
                            txtPLMassWetSPlusCont.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtPLMassDrySPlusCont.Text == "")
                        {
                            dispalyMsg = "Enter Mass Dry S + Cont for Plastic Limit for row number " + (i + 1) + ".";
                            txtPLMassDrySPlusCont.Focus();
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

                if (TabLLPL.Visible == true)
                    //testingDate = Convert.ToDateTime(txtLLPLDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtLLPLDateOfTesting.Text, "dd/MM/yyyy", null);
                                
                //test data update
                
                // LL/PL/PI
                #region save LL/PL/PI data
                if (TabLLPL.Visible == true && pnlLLPL.Enabled == true)
                {
                    decimal PI = 0;
                    dc.SoilLLPL_Update(0, txtRefNo.Text, txtSampleName.Text, 0, 0, 0, 0, 0, 0, 0, 0,"", 0, false, true);
                    for (int i = 0; i < grdLL.Rows.Count; i++)
                    {
                        DropDownList ddlLLContNo = (DropDownList)grdLL.Rows[i].FindControl("ddlLLContNo");
                        TextBox txtLLNoOfBlows = (TextBox)grdLL.Rows[i].FindControl("txtLLNoOfBlows");
                        TextBox txtLLMassWetSPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassWetSPlusCont");
                        TextBox txtLLMassDrySPlusCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassDrySPlusCont");
                        TextBox txtLLMassOfCont = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfCont");
                        TextBox txtLLMassOfMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfMoist");
                        TextBox txtLLMassOfDryS = (TextBox)grdLL.Rows[i].FindControl("txtLLMassOfDryS");
                        TextBox txtLLMoist = (TextBox)grdLL.Rows[i].FindControl("txtLLMoist");
                        TextBox txtLLLiquidLimit = (TextBox)grdLL.Rows[i].FindControl("txtLLLiquidLimit");

                        PI = 0;
                        if (i == 0)
                        {
                            PI = Convert.ToDecimal(txtPI.Text);
                        }

                        dc.SoilLLPL_Update(i + 1, txtRefNo.Text, txtSampleName.Text, Convert.ToDecimal(ddlLLContNo.SelectedItem.Text), Convert.ToDecimal(txtLLNoOfBlows.Text),
                            Convert.ToDecimal(txtLLMassWetSPlusCont.Text), Convert.ToDecimal(txtLLMassDrySPlusCont.Text), Convert.ToDecimal(txtLLMassOfCont.Text),
                            Convert.ToDecimal(txtLLMassOfMoist.Text), Convert.ToDecimal(txtLLMassOfDryS.Text), Convert.ToDecimal(txtLLMoist.Text),
                            txtLLLiquidLimit.Text, PI, false , false);
                    }
                   
                    for (int i = 0; i < grdPL.Rows.Count; i++)
                    {
                        DropDownList ddlPLContNo = (DropDownList)grdPL.Rows[i].FindControl("ddlPLContNo");
                        TextBox txtPLMassWetSPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassWetSPlusCont");
                        TextBox txtPLMassDrySPlusCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassDrySPlusCont");
                        TextBox txtPLMassOfCont = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfCont");
                        TextBox txtPLMassOfMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfMoist");
                        TextBox txtPLMassOfDryS = (TextBox)grdPL.Rows[i].FindControl("txtPLMassOfDryS");
                        TextBox txtPLMoist = (TextBox)grdPL.Rows[i].FindControl("txtPLMoist");
                        TextBox txtPLPlasticLimit = (TextBox)grdPL.Rows[i].FindControl("txtPLPlasticLimit");
                        
                        dc.SoilLLPL_Update(i + 1, txtRefNo.Text, txtSampleName.Text, Convert.ToDecimal(ddlPLContNo.SelectedItem.Text),0,
                            Convert.ToDecimal(txtPLMassWetSPlusCont.Text), Convert.ToDecimal(txtPLMassDrySPlusCont.Text), Convert.ToDecimal(txtPLMassOfCont.Text),
                            Convert.ToDecimal(txtPLMassOfMoist.Text), Convert.ToDecimal(txtPLMassOfDryS.Text), Convert.ToDecimal(txtPLMoist.Text),
                           txtPLPlasticLimit.Text, 0, true, false);
                    }
                    var test = dc.Test(5, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    testingDate = DateTime.ParseExact(txtLLPLDateOfTesting.Text, "dd/MM/yyyy", null);
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtLLRows.Text), true);
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
            if (TabLLPL.Visible == true && TabLLPL.Enabled == true)
                CalculateLLPL();           
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

        protected void grdLL_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlLLContNo = (e.Row.FindControl("ddlLLContNo") as DropDownList);
                var soset = dc.SoilSetting_View("Weight Of Container");
                ddlLLContNo.DataSource = soset;
                ddlLLContNo.DataTextField = "SOSET_F1_var";
                ddlLLContNo.DataValueField = "SOSET_F2_var";
                ddlLLContNo.DataBind();
                if (ddlLLContNo.Items.Count > 0)
                    ddlLLContNo.Items.Insert(0, new ListItem("Select", "0")); 
                                                                
            }
        }

        protected void grdPL_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlPLContNo = (e.Row.FindControl("ddlPLContNo") as DropDownList);
                var soset = dc.SoilSetting_View("Weight Of Container");
                ddlPLContNo.DataSource = soset;
                ddlPLContNo.DataTextField = "SOSET_F1_var";
                ddlPLContNo.DataValueField = "SOSET_F2_var";
                ddlPLContNo.DataBind();
                if (ddlPLContNo.Items.Count > 0)
                    ddlPLContNo.Items.Insert(0, new ListItem("Select", "0"));

            }
        }
                
        protected void ddlLLContNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtLLMassOfCont = (TextBox)gvr.FindControl("txtLLMassOfCont");
                DropDownList ddlLLContNo = (DropDownList)gvr.FindControl("ddlLLContNo");
                if (ddlLLContNo.SelectedIndex > 0)
                {
                    txtLLMassOfCont.Text = ddlLLContNo.SelectedItem.Value;
                }
            }
        }

        protected void ddlPLContNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtPLMassOfCont = (TextBox)gvr.FindControl("txtPLMassOfCont");
                DropDownList ddlPLContNo = (DropDownList)gvr.FindControl("ddlPLContNo");
                if (ddlPLContNo.SelectedIndex > 0)
                {
                    txtPLMassOfCont.Text = ddlPLContNo.SelectedItem.Value;
                }
            }
        }
    }
}