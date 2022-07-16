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
    public partial class Soil_Report_Sample : System.Web.UI.Page
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
                    txtRecordType.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    txtReportNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txtRefNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblStatus.Text = arrIndMsg[1].ToString().Trim();
                }


                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Soil - Report Entry";

               // txtRefNo.Text = Session["ReferenceNo"].ToString();
                //if (lblStatus.Text == "Enter")
                //    lblStatus.Text = "Enter";
                //else if (lblStatus.Text == "Check")
                //    lblStatus.Text = "Check";

                LoadReferenceNoList();
                LoadSoilTest();
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblStatus.Text == "Enter")
                reportStatus = 1;
            else if (lblStatus.Text == "Check")
                reportStatus = 2;

            var reportList = dc.SoilInward_View("", reportStatus);
            ddlRefNo.DataTextField = "SOINWD_ReferenceNo_var";
            ddlRefNo.DataSource = reportList;
            ddlRefNo.DataBind();
            ddlRefNo.Items.Insert(0, new ListItem("---Select---", "0"));
            ddlRefNo.Items.Remove(txtRefNo.Text);
        }
        
        private void LoadSoilTest()
        {
            //Inward details
            var inwd = dc.SoilInward_View(txtRefNo.Text, 0);
            foreach (var soinwd in inwd)
            {
                 txtRefNo.Text = soinwd.SOINWD_ReferenceNo_var.ToString();
                txtReportNo.Text = soinwd.SOINWD_SetOfRecord_var;
                txtSupplierName.Text = soinwd.SOINWD_SupplierName_var;
                txtDesc.Text = soinwd.SOINWD_Description_var;
                txtIdMark.Text = soinwd.SOINWD_IdMark_var;                
            }
            //Test Details
            var sample = dc.SoilSampleTest_View(txtRefNo.Text, "").ToList();
            grdSample.DataSource = sample;
            grdSample.DataBind();
            var test = dc.SoilTest_View(txtRefNo.Text);
            chkTest.DataSource = test;
            chkTest.DataTextField = "TEST_Name_var";
            chkTest.DataValueField = "TEST_Id";
            chkTest.DataBind();
            if (grdSample.Rows.Count <= 0)
                FirstGridViewRowOfSample();
            string samplename ="";
            for (int i = 0; i < grdSample.Rows.Count;i++ )
            {
                Label lblSampleName = (Label) grdSample.Rows[i].FindControl("lblSampleName");
                if (samplename == lblSampleName.Text && samplename!="")
                {
                    lblSampleName.Text = "";
                    ImageButton imgInsertSample = (ImageButton)grdSample.Rows[i].FindControl("imgInsertSample");
                    ImageButton imgEditSample = (ImageButton)grdSample.Rows[i].FindControl("imgEditSample");
                    imgInsertSample.Visible = false;
                    imgEditSample.Visible = false;
                }
                if (lblSampleName.Text!="")
                    samplename = lblSampleName.Text;
            }
        }
        private void FirstGridViewRowOfSample()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("TEST_Sr_No", typeof(string)));
            dt.Columns.Add(new DataColumn("SOSMPLTEST_SampleName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("TEST_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("action1", typeof(string)));
            dr = dt.NewRow();
            dr["TEST_Sr_No"] = 0;
            dr["SOSMPLTEST_SampleName_var"] = string.Empty;
            dr["TEST_Name_var"] = string.Empty;
            dr["action1"] = string.Empty;
            dt.Rows.Add(dr);

            grdSample.DataSource = dt;
            grdSample.DataBind();

        }
        protected void imgInsertSample_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            lblAddSample.Text = "Add New Sample";
            lblSamplename.Text = "0";
            txtSampleName.ReadOnly = false;
        }
        protected void imgEditSample_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            lblAddSample.Text = "Edit Sample";
            //txtSampleName.ReadOnly = true;
            lblSamplename.Text = e.CommandArgument.ToString();
            var smpl = dc.SoilSampleTest_View(txtRefNo.Text, lblSamplename.Text);
            foreach (var sample in smpl)
            {
                txtSampleName.Text = sample.SOSMPLTEST_SampleName_var;                
                foreach (ListItem chk in chkTest.Items)
                {
                    if (chk.Value == sample.SOSMPLTEST_TEST_Id.ToString())
                    {
                        chk.Selected = true;
                        break;
                    }
                }                
            }
        }
        private void ClearAllControls()
        {
            txtSampleName.Text = "";
            lblTest.Visible = false;
            lblSampleMessage.Visible = false;
            lnkSaveSample.Enabled = true;
        }
        
        protected void imgCloseSamplePopup_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender1.Hide();
        }
        
        protected void lnkCancelSample_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender1.Hide();
            LoadSoilTest();
        }

        protected void lnkSaveSample_Click(object sender, EventArgs e)
        {
            lblSampleMessage.Visible = false;
            lblTest.Visible = true;
            foreach (ListItem chk in chkTest.Items)
            {
                if (chk.Selected == true)
                {                    
                    lblTest.Visible = false;
                    break;
                }
            }            
            if (Page.IsValid && lblTest.Visible == false)
            {
                bool sampleStatus = false;

                //check for duplicate sample
                var smpl = dc.SoilSampleTest_View(txtRefNo.Text, txtSampleName.Text.Trim());
                foreach (var sample in smpl)
                {
                    if ( lblSamplename.Text.ToString() == "")
                    {
                        sampleStatus = true;
                        lblSampleMessage.Text = "Duplicate Sample Name..";
                        lblSampleMessage.Visible = true;
                        break;
                    }
                    else if ( lblSamplename.Text.ToString() != "")
                    {
                        if (sample.SOSMPLTEST_SampleName_var !=  lblSamplename.Text.ToString())
                        {
                            sampleStatus = true;
                            lblSampleMessage.Text = "Duplicate Sample Name..";
                            lblSampleMessage.Visible = true;
                            break;
                        }
                    }
                }
                //
                if (sampleStatus == false)
                {
                    foreach (ListItem chk in chkTest.Items)
                    {
                        dc.SoilSampleTest_Update(txtSampleName.Text, txtRefNo.Text, Convert.ToInt32(chk.Value), 0, null, 0, chk.Selected);                        
                    }

                    lblSampleMessage.Text = "Updated Successfully";
                    lblSampleMessage.Visible = true;
                    //LoadSoilTest();
                    //ClearAllControls();                 
                    //ModalPopupExtender1.Hide();  
                    lnkSaveSample.Enabled = false;
                }
            }
        }
        
        protected void lnkAction_Click(object sender, CommandEventArgs e)
        {
            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();               
            int rowindex = Convert.ToInt32(e.CommandArgument);
            Label lblSampleName = (Label)grdSample.Rows[rowindex].FindControl("lblSampleName1");
            Label lblTestSrNo = (Label)grdSample.Rows[rowindex].FindControl("lblTestId");
            LinkButton lnkAction = (LinkButton)grdSample.Rows[rowindex].FindControl("lnkAction");

            if (lnkAction.Text == "Enter" || lnkAction.Text == "Check")
            {
                if (lblTestSrNo.Text == "1" || lblTestSrNo.Text == "2" || lblTestSrNo.Text == "12"
                    || lblTestSrNo.Text == "13" || lblTestSrNo.Text == "14")
                {
                    // Response.Redirect("Soil_Report_FSI.aspx");
                    string strURLWithData = "Soil_Report_FSI.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);

                }
                else if (lblTestSrNo.Text == "3" || lblTestSrNo.Text == "4")
                {
                    //  Response.Redirect("Soil_Report_MDD.aspx");
                    string strURLWithData = "Soil_Report_MDD.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);
                }
                else if (lblTestSrNo.Text == "5")
                {
                    // Response.Redirect("Soil_Report_LLPLPI.aspx");
                    string strURLWithData = "Soil_Report_LLPLPI.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);
                }
                else if (lblTestSrNo.Text == "10" || lblTestSrNo.Text == "11")
                {
                    //Response.Redirect("Soil_Report_FDTS.aspx");
                    string strURLWithData = "Soil_Report_FDTS.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);
                }
                else if (lblTestSrNo.Text == "6" || lblTestSrNo.Text == "7")
                {
                    //Response.Redirect("Soil_Report_Gradation.aspx");
                    string strURLWithData = "Soil_Report_Gradation.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);
                }
                else if (lblTestSrNo.Text == "8" || lblTestSrNo.Text == "9")
                {
                    // Response.Redirect("SoilReportCBR.aspx");
                    string strURLWithData = "Soil_Report_CBR.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);
                }
                else if (lblTestSrNo.Text == "15")
                {
                    string strURLWithData = "Soil_Report_ShrinkageLimit.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);
                }
                else if (lblTestSrNo.Text == "16")
                {
                    string strURLWithData = "Soil_Report_SpecificGravity.aspx?" + obj.Encrypt(string.Format("RefNo={0}&SoilSampleName={1}&Action={2}", txtRefNo.Text, lblSampleName.Text, lnkAction.Text));
                    Response.Redirect(strURLWithData);
                }
            }
            //else if (lnkAction.Text == "Approve")
            //{
            //    dc.ReportDetails_Update("SO", txtRefNo.Text, 0, 0, 0, Convert.ToByte(Session["LoginId"].ToString()), false, "Approved By");
            //    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, false, false, true, false, false, false, false);

            //    Label lblMsg = (Label)Master.FindControl("lblMsg");
            //    lblMsg.Text = "Report Approved Successfully.";
            //    lblMsg.Visible = true;
            //}
            //else if (lnkAction.Text == "Print")
            //{
            //    byte apprBy = 0;
            //    //if (ddlApprovedBy.SelectedIndex > 0)
            //    //    apprBy = Convert.ToByte(ddlApprovedBy.SelectedValue);
                
            //    dc.ReportDetails_Update("SO", txtRefNo.Text, 0, 0, 0, apprBy, false, "Printed By");
            //    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, false, false, false, true, false, false, false);
                
            //    PrintPDFReport rpt = new PrintPDFReport();
            //    rpt.Soil_PDFReport(txtRefNo.Text, lblSampleName.Text, "Print");
            //    //rpt.Soil_PDFReport(txtRefNo.Text, lblSampleName.Text, "View");
            //}
            //else if (lnkAction.Text == "Outward")
            //{
            //    dc.ReportDetails_Update("SO", txtRefNo.Text, 0, 0, 0, 0, false, "Outward By");
            //    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, false, false, false, false, true, false, false);
            //    //dc.Outward_Update(Recordtype, ReferenceNo, RecordNo, SetOfRecord, testType, txtOutwardBy.Text, txtOutwardTo.Text, DateTime.ParseExact(txtOutwardDate.Text, "dd/MM/yyyy", null), 0, "", null, "", false);
                
            //    PrintPDFReport rpt = new PrintPDFReport();
            //    rpt.Soil_PDFReport(txtRefNo.Text, lblSampleName.Text, "Print");
            //}
            //else if (lnkAction.Text == "Physical Outward")
            //{
            //    dc.ReportDetails_Update("SO", txtRefNo.Text, 0, 0, 0, 0, false, "Phy Outward By");
            //    dc.MISDetail_Update(0, "SO", txtRefNo.Text, "SO", null, false, false, false, false, false,true, false);
            //    //dc.Outward_Update(Recordtype, ReferenceNo, RecordNo, SetOfRecord, testType, "", "", null, Convert.ToInt32(txtRegisterNo.Text), txtDeliveredTo.Text, DateTime.ParseExact(txtDeliveredDate.Text, "dd/MM/yyyy", null), txtRemark.Text, true);

            //    PrintPDFReport rpt = new PrintPDFReport();
            //    rpt.Soil_PDFReport(txtRefNo.Text, lblSampleName.Text, "Print");
            //}
        }
        
        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            if (ddlRefNo.SelectedIndex > 0)
            {
                //ClearData();
                txtRefNo.Text = ddlRefNo.SelectedValue;
                //Session["ReferenceNo"] = txtRefNo.Text;
                LoadSoilTest();
                LoadReferenceNoList();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Please Select Reference No.');", true);
            }
        }
        
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
        }
                
       


    }

}