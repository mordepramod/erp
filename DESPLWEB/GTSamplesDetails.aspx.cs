using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class GTSamplesDetails : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

                    arrIndMsg = arrMsgs[1].Split('=');
                    txtRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txtRefNo.Text = arrIndMsg[1].ToString().Trim();

                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "GT Sample Details";
                //txtRefNo.Text = Session["ReferenceNo"].ToString();
                //txtRecordNo.Text = Session["RecordNo"].ToString();
                
                LoadUserList();
                displayGTDetails();
                rdbSample.SelectedValue = "Sampling";
                displaySampleDetails();
                DisplayLabIdNo();
            }
        }

        private void LoadUserList()
        {
            ddlSampledBy.DataTextField = "USER_Name_var";
            ddlSampledBy.DataValueField = "USER_Id";

            ddlEntdChkdBy.DataTextField = "USER_Name_var";
            ddlEntdChkdBy.DataValueField = "USER_Id";

            ddlTestdApprdBy.DataTextField = "USER_Name_var";
            ddlTestdApprdBy.DataValueField = "USER_Id";
                        
            var sampleUser = dc.User_View_Rights(""); //all user
            ddlSampledBy.DataSource = sampleUser;
            ddlSampledBy.DataBind();
            ddlSampledBy.Items.Insert(0, "---Select---");
                        
            var chkUser = dc.User_View_Rights("check"); //check right user
            ddlEntdChkdBy.DataSource = chkUser;
            ddlEntdChkdBy.DataBind();
            ddlEntdChkdBy.Items.Insert(0, "---Select---");

            var apprUser = dc.User_View_Rights("approve"); //appr right user
            ddlTestdApprdBy.DataSource = apprUser;
            ddlTestdApprdBy.DataBind();
            ddlTestdApprdBy.Items.Insert(0, "---Select---");
        }
        
        private void displayGTDetails()
        {
            var gtInwordData = dc.GTInward_View(txtRefNo.Text, 0);
            foreach (var gtInwd in gtInwordData)
            {
                if (gtInwd.GTINW_ReceivedDate_dt == null)
                    txtDateOfReceipt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                else
                    txtDateOfReceipt.Text = Convert.ToDateTime(gtInwd.GTINW_ReceivedDate_dt).ToString("dd/MM/yyyy");

                if (gtInwd.GTINW_ReceivedDate_dt == null)
                    txtDateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                else
                    txtDateOfTesting.Text = Convert.ToDateTime(gtInwd.GTINW_TestedDate_dt).ToString("dd/MM/yyyy");

                txtClient.Text = gtInwd.CL_Name_var;
                txtRecordNo.Text = gtInwd.GTINW_RecordNo_int.ToString();
                txtSiteName.Text = gtInwd.SITE_Name_var;

                if (gtInwd.GTINW_SampledBy_int != null)
                {
                    ddlSampledBy.SelectedValue = gtInwd.GTINW_SampledBy_int.ToString();
                    ddlEntdChkdBy.SelectedValue = gtInwd.GTINW_SampleCheckedBy_int.ToString();
                    ddlTestdApprdBy.SelectedValue = gtInwd.GTINW_SampleApprovedBy_int.ToString();
                }
            }
        }
        
        private void displaySampleDetails()
        {
            int rowNo = 0;
            var gtSamplesDetails = dc.GTSample_View(txtRefNo.Text, "Rock", "");
            foreach (var gtSamples in gtSamplesDetails)
            {
                AddRowGTSample();
                Label lblLabIdNo = (Label)grdGTRock.Rows[rowNo].FindControl("lblLabIdNo");
                TextBox txtLabIDNo = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtLabIDNo");
                TextBox txtBHNo = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtBHNo");
                TextBox txtDepth = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtDepth");
                TextBox txtPieceNo = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtPieceNo");
                TextBox txtDescription = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtDescription");
                TextBox txtAvgdiaofCore = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtAvgdiaofCore");
                TextBox txtheightCore = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtheightCore");
                TextBox txtSSDwtWater = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtSSDwtWater");
                TextBox txtSSDwtSurfaceDry = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtSSDwtSurfaceDry");
                TextBox txtOvenDryWt = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtOvenDryWt");
                TextBox txtLoadatfailure = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtLoadatfailure");
                TextBox txtComment = (TextBox)grdGTRock.Rows[rowNo].FindControl("txtComment");

                lblLabIdNo.Text = gtSamples.GTSMPL_LabIdNo_var;
                txtLabIDNo.Text = gtSamples.GTSMPL_LabIdNo_var;
                txtBHNo.Text = gtSamples.GTSMPL_BHNo_var;
                txtDepth.Text = gtSamples.GTSMPL_Depth_var;
                txtPieceNo.Text = gtSamples.GTSMPL_PieceNo_var;
                txtDescription.Text = gtSamples.GTSMPL_Description_var;
                txtAvgdiaofCore.Text = gtSamples.GTSMPL_AvgDiaCore_var;
                txtheightCore.Text = gtSamples.GTSMPL_AvgHeightCore_var;
                txtSSDwtWater.Text = gtSamples.GTSMPL_SSdWtInWater_var;
                txtSSDwtSurfaceDry.Text = gtSamples.GTSMPL_SSDWtSurfaceDry_var;
                txtOvenDryWt.Text = gtSamples.GTSMPL_OvenDryWt_var;
                txtLoadatfailure.Text = gtSamples.GTSMPL_LoadAtFailure_var;
                txtComment.Text = gtSamples.GTSMPL_Comment_var;

                rowNo++;
            }
            if (rowNo == 0 && rdbSample.SelectedValue == "Sampling")
                AddRowGTSample();

            rowNo = 0;
            var gtSamplesDetail = dc.GTSample_View(txtRefNo.Text, "Soil", "");
            foreach (var gtSamples in gtSamplesDetail)
            {
                AddRowGTSoil();
                Label lblLabIdNo = (Label)grdGTSoil.Rows[rowNo].FindControl("lblLabIdNo");
                TextBox txtLabIDNo = (TextBox)grdGTSoil.Rows[rowNo].FindControl("txtLabIDNo");
                TextBox txtBHNo = (TextBox)grdGTSoil.Rows[rowNo].FindControl("txtBHNo");
                TextBox txtDepth = (TextBox)grdGTSoil.Rows[rowNo].FindControl("txtDepth");
                TextBox txtDSNoSPTNo = (TextBox)grdGTSoil.Rows[rowNo].FindControl("txtDSNoSPTNo");
                TextBox txtDescription = (TextBox)grdGTSoil.Rows[rowNo].FindControl("txtDescription");
                TextBox txtMoisture = (TextBox)grdGTSoil.Rows[rowNo].FindControl("txtMoisture");

                lblLabIdNo.Text = gtSamples.GTSMPL_LabIdNo_var;
                txtLabIDNo.Text = gtSamples.GTSMPL_LabIdNo_var;
                txtBHNo.Text = gtSamples.GTSMPL_BHNo_var;
                txtDepth.Text = gtSamples.GTSMPL_Depth_var;
                txtDSNoSPTNo.Text = gtSamples.GTSMPL_DSSPTNo_var;
                txtDescription.Text = gtSamples.GTSMPL_Description_var;
                txtMoisture.Text = gtSamples.GTSMPL_Moisture_var;

                rowNo++;
            }
            if (rowNo == 0)
                AddRowGTSoil();
        }

        private void DisplayLabIdNo()
        {
            for (int i = 0; i < grdGTRock.Rows.Count; i++)
            {
                TextBox txtLabIDNo = (TextBox)grdGTRock.Rows[i].FindControl("txtLabIDNo");
                int count = i + 1;
                txtLabIDNo.Text = txtRecordNo.Text + "/" + count.ToString();
            }
            for (int i = 0; i < grdGTSoil.Rows.Count; i++)
            {
                TextBox txtLabIDNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtLabIDNo");
                int count = i + 1;
                txtLabIDNo.Text = txtRecordNo.Text + "/" + count.ToString();
            }
        }

        protected void rdbSample_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdGTRock.DataSource = null;
            grdGTRock.DataBind();
            grdGTSoil.DataSource = null;
            grdGTSoil.DataBind();
            if (rdbSample.SelectedValue == "Sampling")
            {
                grdGTRock.Columns[0].Visible = true;
                grdGTRock.Columns[1].Visible = true;
                grdGTRock.Columns[4].Visible = true;
                grdGTRock.Columns[5].Visible = true;
                grdGTRock.Columns[6].Visible = true;
                grdGTRock.Columns[7].Visible = true;
                grdGTRock.Columns[8].Visible = false;
                grdGTRock.Columns[9].Visible = false;
                grdGTRock.Columns[10].Visible = false;
                grdGTRock.Columns[11].Visible = false;
                grdGTRock.Columns[12].Visible = false;
                grdGTRock.Columns[13].Visible = false;
                grdGTRock.Columns[14].Visible = false;
                lblSoil.Visible = true;
                grdGTSoil.Visible = true;
            }
            else if (rdbSample.SelectedValue == "Testing")
            {
                grdGTRock.Columns[0].Visible = false;
                grdGTRock.Columns[1].Visible = false;
                grdGTRock.Columns[4].Visible = false;
                grdGTRock.Columns[5].Visible = false;
                grdGTRock.Columns[6].Visible = false;
                grdGTRock.Columns[7].Visible = false;
                grdGTRock.Columns[8].Visible = true;
                grdGTRock.Columns[9].Visible = true;
                grdGTRock.Columns[10].Visible = true;
                grdGTRock.Columns[11].Visible = true;
                grdGTRock.Columns[12].Visible = true;
                grdGTRock.Columns[13].Visible = true;
                grdGTRock.Columns[14].Visible = true;
                lblSoil.Visible = false;
                grdGTSoil.Visible = false;
            }
            displaySampleDetails();
            DisplayLabIdNo(); 
        }
        
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                byte sampledBy = 0, checkedBy = 0, approvedBy = 0;
                bool samplingSatus, updateFlag=false;

                sampledBy = Convert.ToByte(ddlSampledBy.SelectedItem.Value);
                checkedBy = Convert.ToByte(ddlEntdChkdBy.SelectedItem.Value);
                approvedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                dc.GTInward_Update_SampleData(txtRefNo.Text, sampledBy, checkedBy, approvedBy);

                if (rdbSample.SelectedValue == "Sampling")
                    samplingSatus = true;
                else
                    samplingSatus = false;
                
                for (int i = 0; i < grdGTRock.Rows.Count; i++)
                {
                    TextBox txtSrNo = (TextBox)grdGTRock.Rows[i].FindControl("txtSrNo");
                    Label lblLabIDNo = (Label)grdGTRock.Rows[i].FindControl("lblLabIDNo");
                    TextBox txtLabIDNo = (TextBox)grdGTRock.Rows[i].FindControl("txtLabIDNo");
                    TextBox txtBHNo = (TextBox)grdGTRock.Rows[i].FindControl("txtBHNo");
                    TextBox txtDepth = (TextBox)grdGTRock.Rows[i].FindControl("txtDepth");
                    TextBox txtPieceNo = (TextBox)grdGTRock.Rows[i].FindControl("txtPieceNo");
                    TextBox txtDescription = (TextBox)grdGTRock.Rows[i].FindControl("txtDescription");
                    TextBox txtAvgdiaofCore = (TextBox)grdGTRock.Rows[i].FindControl("txtAvgdiaofCore");
                    TextBox txtheightCore = (TextBox)grdGTRock.Rows[i].FindControl("txtheightCore");
                    TextBox txtSSDwtWater = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtWater");
                    TextBox txtSSDwtSurfaceDry = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtSurfaceDry");
                    TextBox txtOvenDryWt = (TextBox)grdGTRock.Rows[i].FindControl("txtOvenDryWt");
                    TextBox txtLoadatfailure = (TextBox)grdGTRock.Rows[i].FindControl("txtLoadatfailure");
                    TextBox txtComment = (TextBox)grdGTRock.Rows[i].FindControl("txtComment");

                    //var recordData = dc.GTSample_View(txtRefNo.Text, "Rock", txtLabIDNo.Text);
                    //if (recordData.Count() > 0)
                    updateFlag = false;
                    if (lblLabIDNo.Text !="")
                    {
                        updateFlag = true;
                    }
                    dc.GTSample_Update(txtRefNo.Text, "Rock", Convert.ToInt32(txtSrNo.Text), txtLabIDNo.Text, txtBHNo.Text, txtDepth.Text, txtPieceNo.Text, "", txtDescription.Text, "", txtAvgdiaofCore.Text, txtheightCore.Text, txtSSDwtWater.Text, txtSSDwtSurfaceDry.Text, txtOvenDryWt.Text, txtLoadatfailure.Text, txtComment.Text, samplingSatus, updateFlag);
                }
                
                for (int i = 0; i < grdGTSoil.Rows.Count; i++)
                {
                    TextBox txtSrNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtSrNo");
                    Label lblLabIDNo = (Label)grdGTSoil.Rows[i].FindControl("lblLabIDNo");
                    TextBox txtLabIDNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtLabIDNo");
                    TextBox txtBHNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtBHNo");
                    TextBox txtDepth = (TextBox)grdGTSoil.Rows[i].FindControl("txtDepth");
                    TextBox txtDSNoSPTNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtDSNoSPTNo");
                    TextBox txtDescription = (TextBox)grdGTSoil.Rows[i].FindControl("txtDescription");
                    TextBox txtMoisture = (TextBox)grdGTSoil.Rows[i].FindControl("txtMoisture");

                    //var recordData = dc.GTSample_View(txtRefNo.Text, "Soil", txtLabIDNo.Text);
                    //if (recordData.Count() > 0)
                    updateFlag = false;
                    if (lblLabIDNo.Text != "")
                    {
                        updateFlag = true;
                    }
                    dc.GTSample_Update(txtRefNo.Text, "Soil", Convert.ToInt32(txtSrNo.Text), txtLabIDNo.Text, txtBHNo.Text, txtDepth.Text, "", txtDSNoSPTNo.Text, txtDescription.Text, txtMoisture.Text, "", "", "", "", "", "", "", samplingSatus, updateFlag);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Updated Successfully');", true);
            }
        }
                
        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;


            if (txtDateOfTesting.Text == "")
            {
                dispalyMsg = "Date Of Testing can not be blank.";
                txtDateOfTesting.Focus();
                valid = false;
            }

            else if (txtDateOfReceipt.Text == "")
            {
                dispalyMsg = "Date Of Receipt can not be blank.";
                txtDateOfReceipt.Focus();
                valid = false;
            }
            else if (txtSiteName.Text == "")
            {
                dispalyMsg = "Please Enter Site Name";
                txtSiteName.Focus();
                valid = false;
            }
            else if (txtClient.Text == "")
            {
                dispalyMsg = "Please Enter Client";
                txtClient.Focus();
                valid = false;
            }
            //date validation            


            if (rdbSample.SelectedValue == "Sampling")
            {
                #region validate Rock + Soil data

                if (valid == true)
                {
                    if (grdGTRock.Rows.Count == 0 && grdGTSoil.Rows.Count == 0)
                    {                       
                        dispalyMsg = "Please Enter data for updation.";
                        grdGTRock.Focus();
                        valid = false;                        
                    }
                    for (int i = 0; i < grdGTRock.Rows.Count; i++)
                    {
                        TextBox txtBHNo = (TextBox)grdGTRock.Rows[i].FindControl("txtBHNo");
                        TextBox txtDepth = (TextBox)grdGTRock.Rows[i].FindControl("txtDepth");
                        TextBox txtPieceNo = (TextBox)grdGTRock.Rows[i].FindControl("txtPieceNo");
                        TextBox txtDescription = (TextBox)grdGTRock.Rows[i].FindControl("txtDescription");

                        if (txtBHNo.Text == "")
                        {
                            dispalyMsg = "Enter BH No for rock row no " + (i + 1) + ".";
                            txtBHNo.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtDepth.Text == "")
                        {
                            dispalyMsg = "Enter Depth for rock row no " + (i + 1) + ".";
                            txtDepth.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtPieceNo.Text == "")
                        {
                            dispalyMsg = "Enter Piece No for rock row no " + (i + 1) + ".";
                            txtPieceNo.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtDescription.Text == "")
                        {
                            dispalyMsg = "Enter Description for rock row no " + (i + 1) + ".";
                            txtDescription.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < grdGTSoil.Rows.Count; i++)
                    {

                        TextBox txtBHNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtBHNo");
                        TextBox txtDepth = (TextBox)grdGTSoil.Rows[i].FindControl("txtDepth");
                        TextBox txtDSNoSPTNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtDSNoSPTNo");
                        TextBox txtDescription = (TextBox)grdGTSoil.Rows[i].FindControl("txtDescription");
                        TextBox txtMoisture = (TextBox)grdGTSoil.Rows[i].FindControl("txtMoisture");
                        if (txtBHNo.Text == "")
                        {
                            dispalyMsg = "Enter BH No for soil row no " + (i + 1) + ".";
                            txtBHNo.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtDepth.Text == "")
                        {
                            dispalyMsg = "Enter Depth for soil row no " + (i + 1) + ".";
                            txtDepth.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtDSNoSPTNo.Text == "")
                        {
                            dispalyMsg = "Enter DS No. / SPT No. for soil row no " + (i + 1) + ".";
                            txtDSNoSPTNo.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtDescription.Text == "")
                        {
                            dispalyMsg = "Enter Description for soil row no " + (i + 1) + ".";
                            txtDescription.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtMoisture.Text == "")
                        {
                            dispalyMsg = "Enter Moisture for soil row no " + (i + 1) + ".";
                            txtMoisture.Focus();
                            valid = false;
                            break;
                        }
                    }
                }

                #endregion
            }
            else if (rdbSample.SelectedValue == "Testing")
            {
                #region validate Rock  data

                if (valid == true)
                {
                    if (grdGTRock.Rows.Count == 0 )
                    {
                        dispalyMsg = "Please Enter data for updation.";
                        grdGTRock.Focus();
                        valid = false;
                    }
                    for (int i = 0; i < grdGTRock.Rows.Count; i++)                    
                    {
                        TextBox txtAvgdiaofCore = (TextBox)grdGTRock.Rows[i].FindControl("txtAvgdiaofCore");
                        TextBox txtheightCore = (TextBox)grdGTRock.Rows[i].FindControl("txtheightCore");
                        TextBox txtSSDwtWater = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtWater");
                        TextBox txtSSDwtSurfaceDry = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtSurfaceDry");
                        TextBox txtOvenDryWt = (TextBox)grdGTRock.Rows[i].FindControl("txtOvenDryWt");
                        TextBox txtLoadatfailure = (TextBox)grdGTRock.Rows[i].FindControl("txtLoadatfailure");
                        TextBox txtComment = (TextBox)grdGTRock.Rows[i].FindControl("txtComment");
                        if (txtAvgdiaofCore.Text == "")
                        {
                            dispalyMsg = "Enter Avg. dia of core for rock row no " + (i + 1) + ".";
                            txtAvgdiaofCore.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtheightCore.Text == "")
                        {
                            dispalyMsg = "Enter Avg. height of core for rock row no " + (i + 1) + ".";
                            txtheightCore.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtSSDwtWater.Text == "")
                        {
                            dispalyMsg = "Enter SSD wt in water for rock row no " + (i + 1) + ".";
                            txtSSDwtWater.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtSSDwtSurfaceDry.Text == "")
                        {
                            dispalyMsg = "Enter SSD wt surface dry for rock row no " + (i + 1) + ".";
                            txtSSDwtSurfaceDry.Focus();
                            valid = false;
                            break;
                        }

                        else if (txtOvenDryWt.Text == "")
                        {
                            dispalyMsg = "Enter Oven Dry Wt for rock row no " + (i + 1) + ".";
                            txtOvenDryWt.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtLoadatfailure.Text == "")
                        {
                            dispalyMsg = "Enter Load at failure for rock row no " + (i + 1) + ".";
                            txtLoadatfailure.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtComment.Text == "")
                        {
                            dispalyMsg = "Enter Comment for rock row no " + (i + 1) + ".";
                            txtComment.Focus();
                            valid = false;
                            break;
                        }
                    }
                }

                #endregion
            }
            if (valid == true)
            {
                if (ddlSampledBy.SelectedIndex <= 0)
                {
                    dispalyMsg = "Please Select Sampled By Name.";
                    ddlSampledBy.Focus();
                    valid = false;
                }
                else if (ddlEntdChkdBy.SelectedIndex <= 0)
                {
                    dispalyMsg = "Please Select Checked By Name.";
                    ddlEntdChkdBy.Focus();
                    valid = false;
                }
                else if (ddlTestdApprdBy.SelectedIndex <= 0)
                {
                    dispalyMsg = "Please Select Approved By Name.";
                    ddlTestdApprdBy.Focus();
                    valid = false;
                }
                
            }
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }
        
        #region add/delete rows GTSample grid
        protected void AddRowGTSample()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["GTSampleTable"] != null)
            {
                GetCurrentDataGTSample();
                dt = (DataTable)ViewState["GTSampleTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblLabIDNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLabIDNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtBHNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDepth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPieceNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgdiaofCore", typeof(string)));
                dt.Columns.Add(new DataColumn("txtheightCore", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSSDwtWater", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSSDwtSurfaceDry", typeof(string)));
                dt.Columns.Add(new DataColumn("txtOvenDryWt", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLoadatfailure", typeof(string)));
                dt.Columns.Add(new DataColumn("txtComment", typeof(string)));

            }

            dr = dt.NewRow();
            dr["lblLabIDNo"] = string.Empty;
            dr["txtLabIDNo"] = string.Empty;
            dr["txtBHNo"] = string.Empty;
            dr["txtDepth"] = string.Empty;
            dr["txtPieceNo"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["txtAvgdiaofCore"] = string.Empty;
            dr["txtheightCore"] = string.Empty;
            dr["txtSSDwtWater"] = string.Empty;
            dr["txtSSDwtSurfaceDry"] = string.Empty;
            dr["txtOvenDryWt"] = string.Empty;
            dr["txtLoadatfailure"] = string.Empty;
            dr["txtComment"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["GTSampleTable"] = dt;
            grdGTRock.DataSource = dt;
            grdGTRock.DataBind();
            SetPreviousDataGTSample();
        }
        protected void DeleteRowGTSample(int rowIndex)
        {
            GetCurrentDataGTSample();
            DataTable dt = ViewState["GTSampleTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["GTSampleTable"] = dt;
            grdGTRock.DataSource = dt;
            grdGTRock.DataBind();
            SetPreviousDataGTSample();
        }
        protected void SetPreviousDataGTSample()
        {
            DataTable dt = (DataTable)ViewState["GTSampleTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Label lblLabIDNo = (Label)grdGTRock.Rows[i].FindControl("lblLabIDNo");
                TextBox txtLabIDNo = (TextBox)grdGTRock.Rows[i].FindControl("txtLabIDNo");
                TextBox txtBHNo = (TextBox)grdGTRock.Rows[i].FindControl("txtBHNo");
                TextBox txtDepth = (TextBox)grdGTRock.Rows[i].FindControl("txtDepth");
                TextBox txtPieceNo = (TextBox)grdGTRock.Rows[i].FindControl("txtPieceNo");
                TextBox txtDescription = (TextBox)grdGTRock.Rows[i].FindControl("txtDescription");
                TextBox txtAvgdiaofCore = (TextBox)grdGTRock.Rows[i].FindControl("txtAvgdiaofCore");
                TextBox txtheightCore = (TextBox)grdGTRock.Rows[i].FindControl("txtheightCore");
                TextBox txtSSDwtWater = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtWater");
                TextBox txtSSDwtSurfaceDry = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtSurfaceDry");
                TextBox txtOvenDryWt = (TextBox)grdGTRock.Rows[i].FindControl("txtOvenDryWt");
                TextBox txtLoadatfailure = (TextBox)grdGTRock.Rows[i].FindControl("txtLoadatfailure");
                TextBox txtComment = (TextBox)grdGTRock.Rows[i].FindControl("txtComment");

                lblLabIDNo.Text = dt.Rows[i]["lblLabIDNo"].ToString();
                txtLabIDNo.Text = dt.Rows[i]["txtLabIDNo"].ToString();
                txtBHNo.Text = dt.Rows[i]["txtBHNo"].ToString();
                txtDepth.Text = dt.Rows[i]["txtDepth"].ToString();
                txtPieceNo.Text = dt.Rows[i]["txtPieceNo"].ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                txtAvgdiaofCore.Text = dt.Rows[i]["txtAvgdiaofCore"].ToString();
                txtheightCore.Text = dt.Rows[i]["txtheightCore"].ToString();
                txtSSDwtWater.Text = dt.Rows[i]["txtSSDwtWater"].ToString();
                txtSSDwtSurfaceDry.Text = dt.Rows[i]["txtSSDwtSurfaceDry"].ToString();
                txtOvenDryWt.Text = dt.Rows[i]["txtOvenDryWt"].ToString();
                txtLoadatfailure.Text = dt.Rows[i]["txtLoadatfailure"].ToString();
                txtComment.Text = dt.Rows[i]["txtComment"].ToString();

            }
        }
        protected void GetCurrentDataGTSample()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtLabIDNo", typeof(string)));
            dt.Columns.Add(new DataColumn("lblLabIDNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtBHNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDepth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPieceNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("txtAvgdiaofCore", typeof(string)));
            dt.Columns.Add(new DataColumn("txtheightCore", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSSDwtWater", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSSDwtSurfaceDry", typeof(string)));
            dt.Columns.Add(new DataColumn("txtOvenDryWt", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLoadatfailure", typeof(string)));
            dt.Columns.Add(new DataColumn("txtComment", typeof(string)));


            for (int i = 0; i < grdGTRock.Rows.Count; i++)
            {
                Label lblLabIDNo = (Label)grdGTRock.Rows[i].FindControl("lblLabIDNo");
                TextBox txtLabIDNo = (TextBox)grdGTRock.Rows[i].FindControl("txtLabIDNo");
                TextBox txtBHNo = (TextBox)grdGTRock.Rows[i].FindControl("txtBHNo");
                TextBox txtDepth = (TextBox)grdGTRock.Rows[i].FindControl("txtDepth");
                TextBox txtPieceNo = (TextBox)grdGTRock.Rows[i].FindControl("txtPieceNo");
                TextBox txtDescription = (TextBox)grdGTRock.Rows[i].FindControl("txtDescription");
                TextBox txtAvgdiaofCore = (TextBox)grdGTRock.Rows[i].FindControl("txtAvgdiaofCore");
                TextBox txtheightCore = (TextBox)grdGTRock.Rows[i].FindControl("txtheightCore");
                TextBox txtSSDwtWater = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtWater");
                TextBox txtSSDwtSurfaceDry = (TextBox)grdGTRock.Rows[i].FindControl("txtSSDwtSurfaceDry");
                TextBox txtOvenDryWt = (TextBox)grdGTRock.Rows[i].FindControl("txtOvenDryWt");
                TextBox txtLoadatfailure = (TextBox)grdGTRock.Rows[i].FindControl("txtLoadatfailure");
                TextBox txtComment = (TextBox)grdGTRock.Rows[i].FindControl("txtComment");


                dr = dt.NewRow();
                dr["lblLabIDNo"] = lblLabIDNo.Text;
                dr["txtLabIDNo"] = txtLabIDNo.Text;
                dr["txtBHNo"] = txtBHNo.Text;
                dr["txtDepth"] = txtDepth.Text;
                dr["txtPieceNo"] = txtPieceNo.Text;
                dr["txtDescription"] = txtDescription.Text;
                dr["txtAvgdiaofCore"] = txtAvgdiaofCore.Text;
                dr["txtheightCore"] = txtheightCore.Text;
                dr["txtSSDwtWater"] = txtSSDwtWater.Text;
                dr["txtSSDwtSurfaceDry"] = txtSSDwtSurfaceDry.Text;
                dr["txtOvenDryWt"] = txtOvenDryWt.Text;
                dr["txtLoadatfailure"] = txtLoadatfailure.Text;
                dr["txtComment"] = txtComment.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["GTSampleTable"] = dt;

        }

        protected void btnAddRowRock_Click(object sender, EventArgs e)
        {
            AddRowGTSample();
            DisplayLabIdNo();
        }
        protected void btnDeleteRowRock_Click(object sender, EventArgs e)
        {
            if (grdGTRock.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                Label lblLabIDNo = (Label)gvr.FindControl("lblLabIDNo");
                if (lblLabIDNo.Text == "")
                {
                    DeleteRowGTSample(gvr.RowIndex);
                    DisplayLabIdNo();
                }
            }
        }
        #endregion

        #region add/delete rows GTSoil grid
        protected void AddRowGTSoil()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["GTSoilTable"] != null)
            {
                GetCurrentDataGTSoil();
                dt = (DataTable)ViewState["GTSoilTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblLabIDNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLabIDNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtBHNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDepth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDSNoSPTNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMoisture", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblLabIDNo"] = string.Empty;
            dr["txtLabIDNo"] = string.Empty;
            dr["txtBHNo"] = string.Empty;
            dr["txtDepth"] = string.Empty;
            dr["txtDSNoSPTNo"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["txtMoisture"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["GTSoilTable"] = dt;
            grdGTSoil.DataSource = dt;
            grdGTSoil.DataBind();
            SetPreviousDataGTSoil();
        }
        protected void DeleteRowGTSoil(int rowIndex)
        {
            GetCurrentDataGTSoil();
            DataTable dt = ViewState["GTSoilTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["GTSoilTable"] = dt;
            grdGTSoil.DataSource = dt;
            grdGTSoil.DataBind();
            SetPreviousDataGTSoil();
        }
        protected void SetPreviousDataGTSoil()
        {
            DataTable dt = (DataTable)ViewState["GTSoilTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblLabIDNo = (Label)grdGTSoil.Rows[i].FindControl("lblLabIDNo");
                TextBox txtLabIDNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtLabIDNo");
                TextBox txtBHNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtBHNo");
                TextBox txtDepth = (TextBox)grdGTSoil.Rows[i].FindControl("txtDepth");
                TextBox txtDSNoSPTNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtDSNoSPTNo");
                TextBox txtDescription = (TextBox)grdGTSoil.Rows[i].FindControl("txtDescription");
                TextBox txtMoisture = (TextBox)grdGTSoil.Rows[i].FindControl("txtMoisture");

                lblLabIDNo.Text = dt.Rows[i]["lblLabIDNo"].ToString();
                txtLabIDNo.Text = dt.Rows[i]["txtLabIDNo"].ToString();
                txtBHNo.Text = dt.Rows[i]["txtBHNo"].ToString();
                txtDepth.Text = dt.Rows[i]["txtDepth"].ToString();
                txtDSNoSPTNo.Text = dt.Rows[i]["txtDSNoSPTNo"].ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                txtMoisture.Text = dt.Rows[i]["txtMoisture"].ToString();
            }
        }
        protected void GetCurrentDataGTSoil()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lblLabIDNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLabIDNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtBHNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDepth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDSNoSPTNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMoisture", typeof(string)));

            for (int i = 0; i < grdGTSoil.Rows.Count; i++)
            {
                Label lblLabIDNo = (Label)grdGTSoil.Rows[i].FindControl("lblLabIDNo");
                TextBox txtLabIDNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtLabIDNo");
                TextBox txtBHNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtBHNo");
                TextBox txtDepth = (TextBox)grdGTSoil.Rows[i].FindControl("txtDepth");
                TextBox txtDSNoSPTNo = (TextBox)grdGTSoil.Rows[i].FindControl("txtDSNoSPTNo");
                TextBox txtDescription = (TextBox)grdGTSoil.Rows[i].FindControl("txtDescription");
                TextBox txtMoisture = (TextBox)grdGTSoil.Rows[i].FindControl("txtMoisture");

                dr = dt.NewRow();
                dr["lblLabIDNo"] = lblLabIDNo.Text;
                dr["txtLabIDNo"] = txtLabIDNo.Text;
                dr["txtBHNo"] = txtBHNo.Text;
                dr["txtDepth"] = txtDepth.Text;
                dr["txtDSNoSPTNo"] = txtDSNoSPTNo.Text;
                dr["txtDescription"] = txtDescription.Text;
                dr["txtMoisture"] = txtMoisture.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["GTSoilTable"] = dt;

        }

        protected void btnAddRowSoil_Click(object sender, EventArgs e)
        {
            AddRowGTSoil();
            DisplayLabIdNo();
        }
        protected void btnDeleteRowSoil_Click(object sender, EventArgs e)
        {
            if (grdGTSoil.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                Label lblLabIDNo = (Label)gvr.FindControl("lblLabIDNo");
                if (lblLabIDNo.Text == "")
                {
                    DeleteRowGTSoil(gvr.RowIndex);
                    DisplayLabIdNo();
                }
            }
        }
        #endregion

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
        }
    }
}