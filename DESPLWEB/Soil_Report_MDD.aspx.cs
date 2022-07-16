using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net;

namespace DESPLWEB
{
    public partial class Soil_Report_MDD : System.Web.UI.Page
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
                lblheading.Text = "Soil - Report Entry";

                //txtEntdChkdBy.Text = Session["LoginUserName"].ToString();
               // txtRefNo.Text = Session["ReferenceNo"].ToString();
              //  txtSampleName.Text = Session["SoilSampleName"].ToString();

                if (lblStatus.Text == "Enter")
                {
                   // lblStatus.Text = "Enter";
                    lblheading.Text = "Soil - Report Entry";
                }
                else if (lblStatus.Text == "Check")
                {
                  //  lblStatus.Text = "Check";
                    lblheading.Text = "Soil - Report Checking";
                    chkEditMDDOMC.Visible = true;
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
            var apprUser = dc.ReportStatus_View("", null, null, 0, testBit, apprBit, "", 0, 0,0);
            ddlTestdApprdBy.DataSource = apprUser;
            ddlTestdApprdBy.DataBind();
            ddlTestdApprdBy.Items.Insert(0, "---Select---");
        }
        
        protected void txtMDDStdRows_TextChanged(object sender, EventArgs e)
        {
            MDDStdRowsChanged();
        }

        private void MDDStdRowsChanged()
        {
            if (txtMDDStdRows.Text != "")
            {
                if (Convert.ToInt32(txtMDDStdRows.Text) < grdMDDStd.Rows.Count)
                {
                    for (int i = grdMDDStd.Rows.Count; i > Convert.ToInt32(txtMDDStdRows.Text); i--)
                    {
                        if (grdMDDStd.Rows.Count > 1)
                        {
                            DeleteRowMDDStd(i - 1);
                            DeleteRowMDDStdObs(i - 1);
                            DeleteRowMDDStdResult(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtMDDStdRows.Text) > grdMDDStd.Rows.Count)
                {
                    for (int i = grdMDDStd.Rows.Count + 1; i <= Convert.ToInt32(txtMDDStdRows.Text); i++)
                    {
                        AddRowMDDStd();
                        AddRowMDDStdObs();
                        AddRowMDDStdResult();
                    }
                }
            }
        }
        
        #region add/delete rows grdMDDStd grid
        protected void AddRowMDDStd()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MDDStdTable"] != null)
            {
                GetCurrentDataMDDStd();
                dt = (DataTable)ViewState["MDDStdTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtMDDStdMouldVolume", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdMouldNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdMassOfMould", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdMassPlusMat", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdWetDensity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdMassOfWetMat", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtMDDStdMouldVolume"] = string.Empty;
            dr["txtMDDStdMouldNo"] = string.Empty;
            dr["txtMDDStdMassOfMould"] = string.Empty;
            dr["txtMDDStdMassPlusMat"] = string.Empty;
            dr["txtMDDStdWetDensity"] = string.Empty;
            dr["txtMDDStdMassOfWetMat"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MDDStdTable"] = dt;
            grdMDDStd.DataSource = dt;
            grdMDDStd.DataBind();
            SetPreviousDataMDDStd();
        }
        protected void DeleteRowMDDStd(int rowIndex)
        {
            GetCurrentDataMDDStd();
            DataTable dt = ViewState["MDDStdTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MDDStdTable"] = dt;
            grdMDDStd.DataSource = dt;
            grdMDDStd.DataBind();
            SetPreviousDataMDDStd();
        }
        protected void SetPreviousDataMDDStd()
        {
            DataTable dt = (DataTable)ViewState["MDDStdTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtMDDStdMouldVolume = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldVolume");
                TextBox txtMDDStdMouldNo = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldNo");
                TextBox txtMDDStdMassOfMould = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfMould");
                TextBox txtMDDStdMassPlusMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassPlusMat");
                TextBox txtMDDStdWetDensity = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdWetDensity");
                TextBox txtMDDStdMassOfWetMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfWetMat");

                txtMDDStdMouldNo.Text = dt.Rows[i]["txtMDDStdMouldNo"].ToString();
                txtMDDStdMouldVolume.Text = dt.Rows[i]["txtMDDStdMouldVolume"].ToString();                
                txtMDDStdMassOfMould.Text = dt.Rows[i]["txtMDDStdMassOfMould"].ToString();

                if (ddlMDDMouldNo.SelectedIndex > 0)
                {
                    txtMDDStdMouldNo.Text = ddlMDDMouldNo.SelectedItem.Text;
                    string[] strMddNo;
                    strMddNo = ddlMDDMouldNo.SelectedItem.Value.Split('|');
                    txtMDDStdMouldVolume.Text = strMddNo[0];
                    txtMDDStdMassOfMould.Text = strMddNo[1];
                }
                txtMDDStdMassPlusMat.Text = dt.Rows[i]["txtMDDStdMassPlusMat"].ToString();
                txtMDDStdWetDensity.Text = dt.Rows[i]["txtMDDStdWetDensity"].ToString();
                txtMDDStdMassOfWetMat.Text = dt.Rows[i]["txtMDDStdMassOfWetMat"].ToString();
            }
        }
        protected void GetCurrentDataMDDStd()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtMDDStdMouldVolume", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdMouldNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdMassOfMould", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdMassPlusMat", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdWetDensity", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdMassOfWetMat", typeof(string)));
            for (int i = 0; i < grdMDDStd.Rows.Count; i++)
            {
                TextBox txtMDDStdMouldVolume = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldVolume");
                TextBox txtMDDStdMouldNo = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldNo");
                TextBox txtMDDStdMassOfMould = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfMould");
                TextBox txtMDDStdMassPlusMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassPlusMat");
                TextBox txtMDDStdWetDensity = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdWetDensity");
                TextBox txtMDDStdMassOfWetMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfWetMat");

                dr = dt.NewRow();
                dr["txtMDDStdMouldVolume"] = txtMDDStdMouldVolume.Text;
                dr["txtMDDStdMouldNo"] = txtMDDStdMouldNo.Text;
                dr["txtMDDStdMassOfMould"] = txtMDDStdMassOfMould.Text;
                dr["txtMDDStdMassPlusMat"] = txtMDDStdMassPlusMat.Text;
                dr["txtMDDStdWetDensity"] = txtMDDStdWetDensity.Text;
                dr["txtMDDStdMassOfWetMat"] = txtMDDStdMassOfWetMat.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["MDDStdTable"] = dt;

        }
        #endregion

        #region add/delete rows grdMDDStdResult grid
        protected void AddRowMDDStdResult()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MDDStdResultTable"] != null)
            {
                GetCurrentDataMDDStdResult();
                dt = (DataTable)ViewState["MDDStdResultTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtMDDStdResDryDens", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdResMoistCont", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtMDDStdResDryDens"] = string.Empty;
            dr["txtMDDStdResMoistCont"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MDDStdResultTable"] = dt;
            grdMDDStdResult.DataSource = dt;
            grdMDDStdResult.DataBind();
            SetPreviousDataMDDStdResult();
        }
        protected void DeleteRowMDDStdResult(int rowIndex)
        {
            GetCurrentDataMDDStdResult();
            DataTable dt = ViewState["MDDStdResultTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MDDStdResultTable"] = dt;
            grdMDDStdResult.DataSource = dt;
            grdMDDStdResult.DataBind();
            SetPreviousDataMDDStdResult();
        }
        protected void SetPreviousDataMDDStdResult()
        {
            DataTable dt = (DataTable)ViewState["MDDStdResultTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtMDDStdResDryDens = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResDryDens");
                TextBox txtMDDStdResMoistCont = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResMoistCont");

                txtMDDStdResDryDens.Text = dt.Rows[i]["txtMDDStdResDryDens"].ToString();
                txtMDDStdResMoistCont.Text = dt.Rows[i]["txtMDDStdResMoistCont"].ToString();
            }
        }
        protected void GetCurrentDataMDDStdResult()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtMDDStdResDryDens", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdResMoistCont", typeof(string)));
            for (int i = 0; i < grdMDDStdResult.Rows.Count; i++)
            {
                TextBox txtMDDStdResDryDens = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResDryDens");
                TextBox txtMDDStdResMoistCont = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResMoistCont");

                dr = dt.NewRow();
                dr["txtMDDStdResDryDens"] = txtMDDStdResDryDens.Text;
                dr["txtMDDStdResMoistCont"] = txtMDDStdResMoistCont.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["MDDStdResultTable"] = dt;

        }
        #endregion
        
        #region add/delete rows grdMDDStdObs grid
        protected void AddRowMDDStdObs()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MDDStdObsTable"] != null)
            {
                GetCurrentDataMDDStdObs();
                dt = (DataTable)ViewState["MDDStdObsTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddlMDDStdObsContNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdObsContWtSample", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdObsContDrySample", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdObsMassOfCont", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdObsMassOfWater", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdObsMassOfDryMat", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMDDStdObsMoistCont", typeof(string)));
            }

            dr = dt.NewRow();
            dr["ddlMDDStdObsContNo"] = string.Empty;
            dr["txtMDDStdObsContWtSample"] = string.Empty;
            dr["txtMDDStdObsContDrySample"] = string.Empty;
            dr["txtMDDStdObsMassOfCont"] = string.Empty;
            dr["txtMDDStdObsMassOfWater"] = string.Empty;
            dr["txtMDDStdObsMassOfDryMat"] = string.Empty;
            dr["txtMDDStdObsMoistCont"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MDDStdObsTable"] = dt;
            grdMDDStdObs.DataSource = dt;
            grdMDDStdObs.DataBind();
            SetPreviousDataMDDStdObs();
        }
        protected void DeleteRowMDDStdObs(int rowIndex)
        {
            GetCurrentDataMDDStdObs();
            DataTable dt = ViewState["MDDStdObsTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MDDStdObsTable"] = dt;
            grdMDDStdObs.DataSource = dt;
            grdMDDStdObs.DataBind();
            SetPreviousDataMDDStdObs();
        }
        protected void SetPreviousDataMDDStdObs()
        {
            DataTable dt = (DataTable)ViewState["MDDStdObsTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlMDDStdObsContNo = (DropDownList)grdMDDStdObs.Rows[i].FindControl("ddlMDDStdObsContNo");
                TextBox txtMDDStdObsContWtSample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContWtSample");
                TextBox txtMDDStdObsContDrySample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContDrySample");
                TextBox txtMDDStdObsMassOfCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfCont");
                TextBox txtMDDStdObsMassOfWater = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfWater");
                TextBox txtMDDStdObsMassOfDryMat = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfDryMat");
                TextBox txtMDDStdObsMoistCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMoistCont");

                //ddlMDDStdObsContNo.Text = dt.Rows[i]["txtMDDStdObsContNo"].ToString();
                //ddlMDDStdObsContNo.SelectedValue = dt.Rows[i]["txtMDDStdObsMassOfCont"].ToString();
                if (dt.Rows[i]["ddlMDDStdObsContNo"].ToString() !="")
                    ddlMDDStdObsContNo.Items.FindByValue(dt.Rows[i]["ddlMDDStdObsContNo"].ToString()).Selected = true;
                txtMDDStdObsContWtSample.Text = dt.Rows[i]["txtMDDStdObsContWtSample"].ToString();
                txtMDDStdObsContDrySample.Text = dt.Rows[i]["txtMDDStdObsContDrySample"].ToString();
                txtMDDStdObsMassOfCont.Text = dt.Rows[i]["txtMDDStdObsMassOfCont"].ToString();
                txtMDDStdObsMassOfWater.Text = dt.Rows[i]["txtMDDStdObsMassOfWater"].ToString();
                txtMDDStdObsMassOfDryMat.Text = dt.Rows[i]["txtMDDStdObsMassOfDryMat"].ToString();
                txtMDDStdObsMoistCont.Text = dt.Rows[i]["txtMDDStdObsMoistCont"].ToString();
            }
        }
        protected void GetCurrentDataMDDStdObs()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ddlMDDStdObsContNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdObsContWtSample", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdObsContDrySample", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdObsMassOfCont", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdObsMassOfWater", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdObsMassOfDryMat", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMDDStdObsMoistCont", typeof(string)));
            for (int i = 0; i < grdMDDStdObs.Rows.Count; i++)
            {
                DropDownList ddlMDDStdObsContNo = (DropDownList)grdMDDStdObs.Rows[i].FindControl("ddlMDDStdObsContNo");
                TextBox txtMDDStdObsContWtSample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContWtSample");
                TextBox txtMDDStdObsContDrySample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContDrySample");
                TextBox txtMDDStdObsMassOfCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfCont");
                TextBox txtMDDStdObsMassOfWater = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfWater");
                TextBox txtMDDStdObsMassOfDryMat = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfDryMat");
                TextBox txtMDDStdObsMoistCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMoistCont");

                dr = dt.NewRow();
                dr["ddlMDDStdObsContNo"] = ddlMDDStdObsContNo.Text;
                dr["txtMDDStdObsContWtSample"] = txtMDDStdObsContWtSample.Text;
                dr["txtMDDStdObsContDrySample"] = txtMDDStdObsContDrySample.Text;
                dr["txtMDDStdObsMassOfCont"] = txtMDDStdObsMassOfCont.Text;
                dr["txtMDDStdObsMassOfWater"] = txtMDDStdObsMassOfWater.Text;
                dr["txtMDDStdObsMassOfDryMat"] = txtMDDStdObsMassOfDryMat.Text;
                dr["txtMDDStdObsMoistCont"] = txtMDDStdObsMoistCont.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["MDDStdObsTable"] = dt;

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
            TabMDD.Visible = false;
            grdMDDStd.DataBind();
            grdMDDStd.DataSource = null;
            grdMDDStdObs.DataBind();
            grdMDDStdObs.DataSource = null;
            grdMDDStdResult.DataBind();
            grdMDDStdResult.DataSource = null;
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnesBy.Visible = false;
            ViewState["RemarkTable"] = null;
            ViewState["FSITable"] = null;
            ViewState["MDDStdTable"] = null;
            ViewState["MDDStdObsTable"] = null;
            ViewState["MDDStsResultTable"] = null;
            txtWitnesBy.Text = "";
            ddlTestdApprdBy.SelectedIndex = 0;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;
        }
        
        private void DisplaySoilDetails()
        {
            //string strSettingFor = "";
            //if (txtMDDType.Text == "Standard")
            //    strSettingFor = "MDD Std Mould Volume";
            //else
            //    strSettingFor = "MDD Modi Mould Volume";
            //var soset = dc.SoilSetting_View(strSettingFor);
            //ddlMDDMouldNo.DataSource = soset;
            //ddlMDDMouldNo.DataTextField = "SOSET_F1_var";
            //ddlMDDMouldNo.DataValueField = "F2plusF3";
            //ddlMDDMouldNo.DataBind();
            //if (ddlMDDMouldNo.Items.Count > 0)
            //    ddlMDDMouldNo.Items.Insert(0, new ListItem("---Select---", "0")); 
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
            TabMDD.Visible = false;            
            //Test Details            
            int rowNo = 0;
            var test = dc.SoilSampleTest_View(txtRefNo.Text,txtSampleName.Text);
            foreach (var sotest in test)
            {
                rowNo = 0;
                if (sotest.TEST_Sr_No == 3 || sotest.TEST_Sr_No == 4)
                {
                    #region display MDD data
                    if (sotest.TEST_Sr_No == 3 )
                        txtMDDType.Text ="Standard" ;
                    else if (sotest.TEST_Sr_No == 4)
                        txtMDDType.Text ="Modified" ;

                    if (sotest.SOSMPLTEST_Status_tint != 0 && lblStatus.Text == "Enter")                    
                        pnlMDD.Enabled = false;                    
                    else if (sotest.SOSMPLTEST_Status_tint != 1 && lblStatus.Text == "Check")
                        pnlMDD.Enabled = false;
                    
                    if (sotest.SOSMPLTEST_Status_tint == 0 )
                        //TabMDD.HeaderText = TabMDD.HeaderText + "(Yet to Entered)"; 
                        TabMDD.HeaderText ="MDD/OMC (Yet to Entered)";  
                    else if (sotest.SOSMPLTEST_Status_tint == 1)
                        TabMDD.HeaderText = "MDD/OMC (Entered)";
                    else if (sotest.SOSMPLTEST_Status_tint == 2)
                        TabMDD.HeaderText = "MDD/OMC (Checked)";

                    TabMDD.Visible = true;                    
                    txtMDDStdRows.Text = sotest.SOSMPLTEST_Quantity_tint.ToString();
                    if (sotest.SOSMPLTEST_TestedDate_dt == null)
                        txtMDDDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtMDDDateOfTesting.Text = Convert.ToDateTime(sotest.SOSMPLTEST_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (txtMDDDateOfTesting.Text == "" || lblStatus.Text == "Enter")
                    {
                        txtMDDDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    string strSettingFor = "";
                    if (txtMDDType.Text == "Standard")
                        strSettingFor = "MDD Std Mould Volume";
                    else
                        strSettingFor = "MDD Modi Mould Volume";
                    var soset = dc.SoilSetting_View(strSettingFor);
                    ddlMDDMouldNo.DataSource = soset;
                    ddlMDDMouldNo.DataTextField = "SOSET_F1_var";
                    ddlMDDMouldNo.DataValueField = "F2plusF3";
                    ddlMDDMouldNo.DataBind();
                    if (ddlMDDMouldNo.Items.Count > 0)
                        ddlMDDMouldNo.Items.Insert(0, new ListItem("---Select---", "0"));

                    var mdd = dc.SoilMDD_View(txtRefNo.Text, txtSampleName.Text,  Convert.ToByte(txtMDDType.Text == "Modified"));
                    foreach (var somdd in mdd)
                    {
                        //Reading
                        AddRowMDDStd();
                        TextBox txtMDDStdMouldNo = (TextBox)grdMDDStd.Rows[rowNo].FindControl("txtMDDStdMouldNo");
                        TextBox txtMDDStdMouldVolume = (TextBox)grdMDDStd.Rows[rowNo].FindControl("txtMDDStdMouldVolume");                        
                        TextBox txtMDDStdMassOfMould = (TextBox)grdMDDStd.Rows[rowNo].FindControl("txtMDDStdMassOfMould");
                        TextBox txtMDDStdMassPlusMat = (TextBox)grdMDDStd.Rows[rowNo].FindControl("txtMDDStdMassPlusMat");
                        TextBox txtMDDStdWetDensity = (TextBox)grdMDDStd.Rows[rowNo].FindControl("txtMDDStdWetDensity");
                        TextBox txtMDDStdMassOfWetMat = (TextBox)grdMDDStd.Rows[rowNo].FindControl("txtMDDStdMassOfWetMat");
                                                
                        txtMDDStdMouldVolume.Text = somdd.SOMDD_MouldVolume_dec.ToString();
                        txtMDDStdMouldNo.Text = somdd.SOMDD_MouldNo_dec.ToString();
                        txtMDDStdMassOfMould.Text = somdd.SOMDD_MassOfMould_dec.ToString();
                        txtMDDStdMassPlusMat.Text = somdd.SOMDD_MassPlusMat_dec.ToString();
                        txtMDDStdWetDensity.Text = somdd.SOMDD_WetDensity_dec.ToString();
                        txtMDDStdMassOfWetMat.Text = somdd.SOMDD_MassOfWetMat_dec.ToString();

                        //Observations
                        AddRowMDDStdObs();
                        DropDownList ddlMDDStdObsContNo = (DropDownList)grdMDDStdObs.Rows[rowNo].FindControl("ddlMDDStdObsContNo");
                        TextBox txtMDDStdObsContWtSample = (TextBox)grdMDDStdObs.Rows[rowNo].FindControl("txtMDDStdObsContWtSample");
                        TextBox txtMDDStdObsContDrySample = (TextBox)grdMDDStdObs.Rows[rowNo].FindControl("txtMDDStdObsContDrySample");
                        TextBox txtMDDStdObsMassOfCont = (TextBox)grdMDDStdObs.Rows[rowNo].FindControl("txtMDDStdObsMassOfCont");
                        TextBox txtMDDStdObsMassOfWater = (TextBox)grdMDDStdObs.Rows[rowNo].FindControl("txtMDDStdObsMassOfWater");
                        TextBox txtMDDStdObsMassOfDryMat = (TextBox)grdMDDStdObs.Rows[rowNo].FindControl("txtMDDStdObsMassOfDryMat");
                        TextBox txtMDDStdObsMoistCont = (TextBox)grdMDDStdObs.Rows[rowNo].FindControl("txtMDDStdObsMoistCont");
                                                
                        //ddlMDDStdObsContNo.SelectedValue = somdd.SOMDD_MassOfCont_dec.ToString();
                        ddlMDDStdObsContNo.Items.FindByText(Convert.ToDecimal(somdd.SOMDD_ContNo_dec).ToString("0")).Selected = true;
                        txtMDDStdObsContWtSample.Text = somdd.SOMDD_ContWtSample_dec.ToString();
                        txtMDDStdObsContDrySample.Text = somdd.SOMDD_ContDrySample_dec.ToString();
                        txtMDDStdObsMassOfCont.Text = somdd.SOMDD_MassOfCont_dec.ToString();
                        txtMDDStdObsMassOfWater.Text = somdd.SOMDD_MassOfWater_dec.ToString();
                        txtMDDStdObsMassOfDryMat.Text = somdd.SOMDD_MassOfDryMat_dec.ToString();
                        txtMDDStdObsMoistCont.Text = somdd.SOMDD_MoistCont_dec.ToString();

                        //Result
                        AddRowMDDStdResult();
                        TextBox txtMDDStdResDryDens = (TextBox)grdMDDStdResult.Rows[rowNo].FindControl("txtMDDStdResDryDens");
                        TextBox txtMDDStdResMoistCont = (TextBox)grdMDDStdResult.Rows[rowNo].FindControl("txtMDDStdResMoistCont");

                        txtMDDStdResDryDens.Text = somdd.SOMDD_DryDens_dec.ToString();
                        txtMDDStdResMoistCont.Text = somdd.SOMDD_MoistContInPerc_dec.ToString();
                        //
                        if (rowNo == 0)
                        {
                            ddlMDDMouldNo.ClearSelection();
                            ddlMDDMouldNo.Items.FindByText(Convert.ToDecimal(somdd.SOMDD_MouldNo_dec).ToString("0")).Selected = true;
                            //ddlMDDMouldNo.SelectedValue = Convert.ToDecimal(somdd.SOMDD_MouldVolume_dec).ToString("0");
                            string[] strMddResult = somdd.SOMDD_Result_var.Split('|');
                            txtMDDStd.Text = strMddResult[0];
                            txtOMCStd.Text = strMddResult[1];
                            txtMDDStdSampleForCBR.Text = strMddResult[2];
                            txtMDDStdWaterForCBR.Text = strMddResult[3];
                        }
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        MDDStdRowsChanged();
                    }
                    else
                    {
                        txtMDDStdRows.Text = rowNo.ToString();
                    }
                    #endregion
                }
                
            }
            
            if (TabMDD.Visible == true)
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

        private void CalculateMDD()
        {            
            decimal temp =0,c=0,s=0;
            for (int i = 0; i < grdMDDStd.Rows.Count; i++)
            {   
                TextBox txtMDDStdMouldVolume = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldVolume"); //2
                TextBox txtMDDStdMassOfMould = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfMould");  //3
                TextBox txtMDDStdMassPlusMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassPlusMat");  //4
                TextBox txtMDDStdWetDensity = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdWetDensity");    //5
                TextBox txtMDDStdMassOfWetMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfWetMat"); //6

                DropDownList ddlMDDStdObsContNo = (DropDownList)grdMDDStdObs.Rows[i].FindControl("ddlMDDStdObsContNo"); //1
                TextBox txtMDDStdObsContWtSample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContWtSample"); //2
                TextBox txtMDDStdObsContDrySample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContDrySample"); //3
                TextBox txtMDDStdObsMassOfCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfCont"); //4
                TextBox txtMDDStdObsMassOfWater = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfWater"); //5
                TextBox txtMDDStdObsMassOfDryMat = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfDryMat"); //6
                TextBox txtMDDStdObsMoistCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMoistCont"); //7

                TextBox txtMDDStdResDryDens = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResDryDens"); //1
                TextBox txtMDDStdResMoistCont = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResMoistCont"); //2

                var wtdata = dc.SoilSetting_View("Weight Of Container").ToList();
                foreach (var containerData in wtdata)
                {
                    if (containerData.SOSET_F1_var == ddlMDDStdObsContNo.SelectedItem.Text)
                    {
                        txtMDDStdObsMassOfCont.Text = containerData.SOSET_F2_var;
                        break;
                    }
                }

                if (txtMDDStdMouldVolume.Text != "" && txtMDDStdMassOfMould.Text != "" && txtMDDStdMassPlusMat.Text != "")
                {
                    temp = 0;
                    if (Convert.ToDecimal(txtMDDStdMouldVolume.Text) > 0)
                    {
                        temp = (Convert.ToDecimal(txtMDDStdMassPlusMat.Text) - Convert.ToDecimal(txtMDDStdMassOfMould.Text)) / Convert.ToDecimal(txtMDDStdMouldVolume.Text);
                        if (temp < 0)
                            temp = 0;                        
                    }
                    txtMDDStdWetDensity.Text = temp.ToString();
                    temp = (Convert.ToDecimal(txtMDDStdMassPlusMat.Text) - Convert.ToDecimal(txtMDDStdMassOfMould.Text));
                    if (temp < 0)
                        temp = 0;
                    txtMDDStdMassOfWetMat.Text = temp.ToString();
                }

                if (txtMDDStdObsContWtSample.Text != "" && txtMDDStdObsContDrySample.Text != "" && txtMDDStdObsMassOfCont.Text != "")
                {
                    temp = (Convert.ToDecimal(txtMDDStdObsContWtSample.Text) - Convert.ToDecimal(txtMDDStdObsContDrySample.Text));
                    if (temp < 0)
                        temp = 0;
                    txtMDDStdObsMassOfWater.Text = temp.ToString();

                    temp = (Convert.ToDecimal(txtMDDStdObsContDrySample.Text) - Convert.ToDecimal(txtMDDStdObsMassOfCont.Text));
                    if (temp < 0)
                        temp = 0;
                    txtMDDStdObsMassOfDryMat.Text = temp.ToString();
                    temp = 0;
                    if (Convert.ToDecimal(txtMDDStdObsMassOfDryMat.Text) > 0)
                    {
                        temp = (Convert.ToDecimal(txtMDDStdObsMassOfWater.Text) / Convert.ToDecimal(txtMDDStdObsMassOfDryMat.Text));
                        if (temp < 0)
                            temp = 0;
                    }
                    txtMDDStdObsMoistCont.Text = temp.ToString();
                }

                if (txtMDDStdWetDensity.Text != "" && txtMDDStdObsMoistCont.Text != "")
                {
                    temp = (Convert.ToDecimal(txtMDDStdWetDensity.Text) / (1 + Convert.ToDecimal(txtMDDStdObsMoistCont.Text)));
                    if (temp < 0)
                        temp = 0;
                    txtMDDStdResDryDens.Text = temp.ToString("0.00");

                    temp = Convert.ToDecimal(txtMDDStdObsMoistCont.Text) * 100;
                    if (temp < 0)
                        temp = 0;
                    txtMDDStdResMoistCont.Text = temp.ToString("0.00");

                    if (chkEditMDDOMC.Visible == false ||
                        (chkEditMDDOMC.Visible == true && chkEditMDDOMC.Checked == false))
                    {
                        if (Convert.ToDecimal(txtMDDStdResDryDens.Text) > 0)
                        {
                            txtMDDStd.Text = txtMDDStdResDryDens.Text;
                            txtOMCStd.Text = txtMDDStdResMoistCont.Text;
                        }
                        else
                        {
                            txtMDDStd.Text = "0.00";
                            txtOMCStd.Text = "0.00";
                        }
                        c = Convert.ToDecimal(txtMDDStd.Text) * (1 + Convert.ToDecimal(0.01) * Convert.ToDecimal(txtOMCStd.Text));
                        s = Convert.ToDecimal(txtMDDStdMouldVolume.Text);
                        txtMDDStdSampleForCBR.Text = (c * s).ToString("0");
                        txtMDDStdWaterForCBR.Text = ((6000 * Convert.ToDecimal(txtOMCStd.Text)) / 100).ToString("0.0");
                    }
                }
                if (txtMDDStdWetDensity.Text != "")
                {
                    temp = Convert.ToDecimal(txtMDDStdWetDensity.Text);
                    txtMDDStdWetDensity.Text = temp.ToString("0.00");
                }
                if (txtMDDStdMassOfWetMat.Text != "")
                {
                    temp = Convert.ToDecimal(txtMDDStdMassOfWetMat.Text);
                    txtMDDStdMassOfWetMat.Text = temp.ToString("0.00");
                }

                if (txtMDDStdObsMassOfWater.Text != "")
                {
                    temp = Convert.ToDecimal(txtMDDStdObsMassOfWater.Text);
                    txtMDDStdObsMassOfWater.Text = temp.ToString("0.00");
                }
                if (txtMDDStdObsMassOfDryMat.Text != "")
                {
                    temp = Convert.ToDecimal(txtMDDStdObsMassOfDryMat.Text);
                    txtMDDStdObsMassOfDryMat.Text = temp.ToString("0.00");
                }
                if (txtMDDStdObsMoistCont.Text != "")
                {
                    temp = Convert.ToDecimal(txtMDDStdObsMoistCont.Text);
                    txtMDDStdObsMoistCont.Text = temp.ToString("0.00");
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
            if (TabMDD.Visible == true && valid == true && pnlMDD.Enabled ==true)
            {
                if (txtMDDStdRows.Text == "" || txtMDDStdRows.Text == "0")
                {
                    dispalyMsg = "Rows can not be zero or blank.";
                    txtMDDStdRows.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (ddlMDDMouldNo.SelectedIndex <= 0)
                {
                    dispalyMsg = "Select Mould No.";
                    ddlMDDMouldNo.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                //date validation
                else if (txtMDDDateOfTesting.Text == "")
                {
                    dispalyMsg = "Date Of Testing can not be blank.";
                    txtMDDDateOfTesting.Focus();
                    valid = false;
                    TabContainerSoil.ActiveTabIndex = 0;
                }
                else if (txtMDDDateOfTesting.Text != "")
                {
                    //DateTime dateTest = DateTime.Now;
                    //dateTest = Convert.ToDateTime(txtMDDDateOfTesting.Text);
                    string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime TestingDate = DateTime.ParseExact(txtMDDDateOfTesting.Text, "dd/MM/yyyy", null);
                    DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                    //if (dateTest > System.DateTime.Now)
                    if (TestingDate > CurrentDate)
                    {
                        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                        txtMDDDateOfTesting.Focus();
                        valid = false;
                        TabContainerSoil.ActiveTabIndex = 0;
                    }
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdMDDStd.Rows.Count; i++)
                    {
                        dataFlag = true;
                        TextBox txtMDDStdMassPlusMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassPlusMat");  //4
                        DropDownList ddlMDDStdObsContNo = (DropDownList)grdMDDStdObs.Rows[i].FindControl("ddlMDDStdObsContNo"); //2
                        TextBox txtMDDStdObsContWtSample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContWtSample"); //3
                        TextBox txtMDDStdObsContDrySample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContDrySample"); //4

                        if (txtMDDStdMassPlusMat.Text == "")
                        {
                            dispalyMsg = "Enter Mass of Mould + Material for row number " + (i + 1) + ".";
                            txtMDDStdMassPlusMat.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        if (ddlMDDStdObsContNo.SelectedIndex <= 0)
                        {
                            dispalyMsg = "Select Container No. for row no " + (i + 1) + ".";
                            ddlMDDStdObsContNo.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtMDDStdObsContWtSample.Text == "")
                        {
                            dispalyMsg = "Enter Cont + Wet Sample for row number " + (i + 1) + ".";
                            txtMDDStdObsContWtSample.Focus();
                            valid = false;
                            TabContainerSoil.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtMDDStdObsContDrySample.Text == "")
                        {
                            dispalyMsg = "Enter Cont + Dry Sample for row number " + (i + 1) + ".";
                            txtMDDStdObsContDrySample.Focus();
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

                if (TabMDD.Visible == true)
                    //testingDate = Convert.ToDateTime(txtMDDDateOfTesting.Text);
                    testingDate = DateTime.ParseExact(txtMDDDateOfTesting.Text, "dd/MM/yyyy", null);
                                
                //test data update
                
                //MDD/OMC
                #region save MDD data
                if (TabMDD.Visible == true && pnlMDD.Enabled == true)
                {
                    string strResult = "";
                    dc.SoilMDD_Update(0, txtRefNo.Text, txtSampleName.Text, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", txtMDDType.Text == "Modified", true);
                    for (int i = 0; i < grdMDDStd.Rows.Count; i++)
                    {
                        TextBox txtMDDStdMouldNo = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldNo");
                        TextBox txtMDDStdMouldVolume = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldVolume");
                        TextBox txtMDDStdMassOfMould = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfMould");
                        TextBox txtMDDStdMassPlusMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassPlusMat");
                        TextBox txtMDDStdWetDensity = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdWetDensity");
                        TextBox txtMDDStdMassOfWetMat = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfWetMat");                        
                        //Observations                       
                        DropDownList ddlMDDStdObsContNo = (DropDownList)grdMDDStdObs.Rows[i].FindControl("ddlMDDStdObsContNo");
                        TextBox txtMDDStdObsContWtSample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContWtSample");
                        TextBox txtMDDStdObsContDrySample = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsContDrySample");
                        TextBox txtMDDStdObsMassOfCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfCont");
                        TextBox txtMDDStdObsMassOfWater = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfWater");
                        TextBox txtMDDStdObsMassOfDryMat = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMassOfDryMat");
                        TextBox txtMDDStdObsMoistCont = (TextBox)grdMDDStdObs.Rows[i].FindControl("txtMDDStdObsMoistCont");                        
                        //Result
                        TextBox txtMDDStdResDryDens = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResDryDens");
                        TextBox txtMDDStdResMoistCont = (TextBox)grdMDDStdResult.Rows[i].FindControl("txtMDDStdResMoistCont");
                        //
                        if (i == 0)
                        {
                            strResult = txtMDDStd.Text + "|" + txtOMCStd.Text + "|" + txtMDDStdSampleForCBR.Text + "|" + txtMDDStdWaterForCBR.Text;
                        }

                        dc.SoilMDD_Update(i+1, txtRefNo.Text, txtSampleName.Text, Convert.ToDecimal(txtMDDStdMouldNo.Text), Convert.ToDecimal(txtMDDStdMouldVolume.Text), 
                            Convert.ToDecimal(txtMDDStdMassOfMould.Text), Convert.ToDecimal(txtMDDStdMassPlusMat.Text), Convert.ToDecimal(txtMDDStdWetDensity.Text), 
                            Convert.ToDecimal(txtMDDStdMassOfWetMat.Text), Convert.ToDecimal(ddlMDDStdObsContNo.SelectedItem.Text), Convert.ToDecimal(txtMDDStdObsContWtSample.Text), 
                            Convert.ToDecimal(txtMDDStdObsContDrySample.Text), Convert.ToDecimal(txtMDDStdObsMassOfCont.Text), Convert.ToDecimal(txtMDDStdObsMassOfWater.Text), 
                            Convert.ToDecimal(txtMDDStdObsMassOfDryMat.Text), Convert.ToDecimal(txtMDDStdObsMoistCont.Text), Convert.ToDecimal(txtMDDStdResDryDens.Text), 
                            Convert.ToDecimal(txtMDDStdResMoistCont.Text), strResult, txtMDDType.Text == "Modified", false);
                    }
                    int testSrNo = 0;
                    if(txtMDDType.Text == "Modified")
                        testSrNo = 4;
                    else
                        testSrNo = 3;
                    var test = dc.Test(testSrNo, "", 0, "SO", "", 0);
                    foreach (var tst in test)
                    {
                        testId = Convert.ToInt32(tst.TEST_Id);
                    }
                    testingDate = DateTime.ParseExact(txtMDDDateOfTesting.Text, "dd/MM/yyyy", null);
                    dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, testId, reportStatus, testingDate, Convert.ToByte(txtMDDStdRows.Text), true);
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
            if (TabMDD.Visible == true && pnlMDD.Enabled ==true)
                CalculateMDD();           
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
        
        protected void grdMDDStdObs_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {               
                DropDownList ddlMDDStdObsContNo = (e.Row.FindControl("ddlMDDStdObsContNo") as DropDownList);
                var soset = dc.SoilSetting_View("Weight Of Container");
                ddlMDDStdObsContNo.DataSource = soset;
                ddlMDDStdObsContNo.DataTextField = "SOSET_F1_var";
                //ddlMDDStdObsContNo.DataValueField = "SOSET_F2_var";
                ddlMDDStdObsContNo.DataBind();
                if (ddlMDDStdObsContNo.Items.Count > 0)
                    ddlMDDStdObsContNo.Items.Insert(0, new ListItem("Select", "0")); 
                                                                
            }
        }

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    Page.ClientScript.GetPostBackEventReference(dllTest, "");
        //    dllTest.Attributes.Add("onchange", "__doPostBack('" + dllTest.UniqueID + "','');");
        //    Page.ClientScript.RegisterForEventValidation(dllTest.UniqueID);
        //    base.Render(writer);
        //}

        protected void ddlMDDMouldNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMDDMouldNo.SelectedIndex > 0)
            {
                for (int i = 0; i < grdMDDStd.Rows.Count; i++)
                {
                    TextBox txtMDDStdMouldNo = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldNo");
                    TextBox txtMDDStdMouldVolume = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMouldVolume");
                    TextBox txtMDDStdMassOfMould = (TextBox)grdMDDStd.Rows[i].FindControl("txtMDDStdMassOfMould");

                    txtMDDStdMouldNo.Text = ddlMDDMouldNo.SelectedItem.Text;
                    string[] strMddNo;
                    strMddNo = ddlMDDMouldNo.SelectedItem.Value.Split('|');
                    txtMDDStdMouldVolume.Text = strMddNo[0];
                    txtMDDStdMassOfMould.Text = strMddNo[1];
                }
            }

        }
        protected void ddlMDDStdObsContNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtMDDStdObsMassOfCont = (TextBox)gvr.FindControl("txtMDDStdObsMassOfCont");
                DropDownList ddlMDDStdObsContNo = (DropDownList)gvr.FindControl("ddlMDDStdObsContNo");
                if (ddlMDDStdObsContNo.SelectedIndex > 0)
                {
                    txtMDDStdObsMassOfCont.Text = ddlMDDStdObsContNo.SelectedItem.Value;
                }
            }
        }
        protected void grdMDDStdObs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //int index = e.NewEditIndex;
            //var ddlMDDStdObsContNo = grdMDDStdObs.Rows[index].FindControl("ddlMDDStdObsContNo") as DropDownList;
            //var txtMDDStdObsMassOfCont = grdMDDStdObs.Rows[index].FindControl("txtMDDStdObsMassOfCont") as TextBox;
            //string text = ddlMDDStdObsContNo.SelectedItem.Text;
            //string value = ddlMDDStdObsContNo.SelectedItem.Value;
            //txtMDDStdObsMassOfCont.Text = ddlMDDStdObsContNo.SelectedItem.Value;
        }

        protected void chkEditMDDOMC_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditMDDOMC.Checked == true)
            {
                txtMDDStd.ReadOnly = false;
                txtOMCStd.ReadOnly = false;
                txtMDDStdSampleForCBR.ReadOnly = false;
                txtMDDStdWaterForCBR.ReadOnly = false;
            }
            else
            {
                txtMDDStd.ReadOnly = true;
                txtOMCStd.ReadOnly = true;
                txtMDDStdSampleForCBR.ReadOnly = true;
                txtMDDStdWaterForCBR.ReadOnly = true;
            }
        }
        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);

        //    if (MyDDL.SelectedIndex == 0)
        //    {
        //        MyDDL_SelectedIndexChanged(MyDDL, e);
        //        MyUpdatePanel.Update();
        //    }

        //}
        

    }
}