using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

//Print date ll be overwrite on duplicate print all inward

namespace DESPLWEB
{
    public partial class Aggregate_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Aggregate - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                  
                    if (strReq.Contains("=") == true)
                    {
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        txt_RecType.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[1].Split('=');
                        lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[2].Split('=');
                        txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[3].Split('=');
                        lblEntry.Text = arrIndMsg[1].ToString().Trim();

                        //MF
                        arrIndMsg = arrMsgs[4].Split('=');
                        lblReportNo.Text = arrIndMsg[1].ToString().Trim();
                    }
                }

                if (txt_RecType.Text != "")
                {
                    txt_SampleCondition.Text  = "Satisfactory";
                    DisplayAGGT_Details();
                    Display_Alldata();

                    LoadReferenceNoList();
                    if (lblEntry.Text == "Check")
                    {
                        //lblEntry.Text = Session["Check"].ToString();//
                        lblheading.Text = "Aggregate - Report Check";
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
                        ViewWitnessBy();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
                        //LoadOtherPendingRpt();
                        LoadTestedBy();
                    }
                }
                else //lblReportNo.Text !=""
                {
                    lblheading.Text = "Mix Design";
                    txt_RecType.Text = "MF";
                    lbl_OtherPending.Text = "Material Name";
                    //DisplayMFDtls();
                    LoadTestedBy();

                    lbl_OtherPendingMF.Visible = true;
                    ddl_OtherPendingRptMF.Visible = true;
                    LoadReferenceNoListMF();

                    //Lnk_Calculate.Enabled = false;
                    lnkSave.Enabled = false;
                    lnkPrint.Enabled = false;
                }
                if (lbl_OtherPending.Text != "Material Name")
                {
                    chkComplete.Visible = true;
                }
            }
        }
        private void LoadReferenceNoListMF()
        {
            var reportList = dc.MaterialDetail_View_List("SieveAnalysis");
            ddl_OtherPendingRptMF.DataTextField = "MaterialDetail_RefNo";
            ddl_OtherPendingRptMF.DataSource = reportList;
            ddl_OtherPendingRptMF.DataBind();
            ddl_OtherPendingRptMF.Items.Insert(0, "---Select---");
            ddl_OtherPendingRptMF.Items.Remove(txt_ReferenceNo.Text);
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEntry.Text == "Enter")
                reportStatus = 1;
            else if (lblEntry.Text == "Check")
                reportStatus = 2;
                        
            var reportList = dc.ReferenceNo_View_StatusWise("AGGT", reportStatus,0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        public void ShowTabMaterialwise()
        {
            if (ddl_OtherPendingRpt.SelectedItem.Text == "Natural Sand" || ddl_OtherPendingRpt.SelectedItem.Text == "Crushed Sand" || ddl_OtherPendingRpt.SelectedItem.Text == "Stone Dust" ||
                ddl_OtherPendingRpt.SelectedItem.Text == "Grit")
            {
             
                Tab_SpecGravity.Enabled = true;
                Tab_OtherTest.Enabled = true;
                
                Pnl_LBD.Visible = true;
                Pnl_WaterAbsorbtion.Visible = true;
                Pnl_MoistureContent.Visible = true;
                Pnl_SildContent.Visible = true;

                lbl_LBDStatus.Text = "1";
                lbl_WtStatus.Text = "1";
                lbl_MoistStatus.Text = "1";
                lbl_SildStatus.Text = "1";
            }
            if (ddl_OtherPendingRpt.SelectedItem.Text == "10 mm" || ddl_OtherPendingRpt.SelectedItem.Text == "20 mm" || ddl_OtherPendingRpt.SelectedItem.Text == "40 mm")
            {
                Tab_SpecGravity.Enabled = true;
                Tab_Elong.Enabled = true;
                Tab_Flaki.Enabled = true;
                Tab_OtherTest.Enabled = true;
                Pnl_LBD.Visible = true;
                Pnl_WaterAbsorbtion.Visible = true;

                //Pnl_MoistureContent.Visible = false;
                Pnl_MoistureContent.Visible = true;
                Pnl_SildContent.Visible = false;

                lbl_LBDStatus.Text = "1";
                lbl_WtStatus.Text = "1";
                //lbl_MoistStatus.Text = "";
                lbl_MoistStatus.Text = "1";
                lbl_SildStatus.Text = "";

            }

        }

        protected void DisplayMFDtls()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = "Please Select Material Name";
            lblMsg.Visible = true;
            txt_ReferenceNo.Text = lblReportNo.Text;// Convert.ToString(txt_ReportNo.Text);
            txt_ReportNo.Text = lblReportNo.Text; // Convert.ToString(txt_ReportNo.Text);
            txt_DateOfTesting.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txt_RecType.Text = "MF";
            LoadMaterialList();
            Tab_Remark.Enabled = false;
            Tab_SieveAnalysis.Enabled = true;            
            DisplayTab();
            Show_Tab();
        }

        protected void grdempty()
        {
            grdAggtEntryRptInward.DataSource = null;
            grdAggtEntryRptInward.DataBind();
            grd_SpecificGravity.DataSource = null;
            grd_SpecificGravity.DataBind();
            grd_Flakiness.DataSource = null;
            grd_Flakiness.DataBind();
            grd_Elongation.DataSource = null;
            grd_Elongation.DataBind();
            grd_ImpactValue.DataSource = null;
            grd_ImpactValue.DataBind();
            grd_CrushValue.DataSource = null;
            grd_CrushValue.DataBind();

            lbl_FMres.Text = "Awaited";
            lbl_Specres.Text = "Awaited";
            lbl_Flakires.Text = "Awaited";
            lbl_Elongres.Text = "Awaited";
            lbl_Impactres.Text = "Awaited";
            lbl_Crushingres.Text = "Awaited";

            txt_TotalwtofAggregate.Text = "";
            txt_VolumeofCylender.Text = "";
            txt_LBDresult.Text = "Awaited";

            txt_InitialWt.Text = "";
            txt_DryWt.Text = "";
            txt_MoistureContent.Text = "Awaited";

            txt_SSDforWT.Text = "";
            txt_OvenDrywtforWT.Text = "";
            txt_WaterAbsorption.Text = "Awaited";

            txt_InitialWtofSild.Text = "";
            txt_RetainedWtmicronSieve.Text = "";
            txt_SildContent.Text = "Awaited";

            lblSSDImage1.Text = "";
            lblSSDImage2.Text = "";
            lblWtBottleSampleImage1.Text = "";
            lblWtBottleSampleImage2.Text = "";
            lblOverDryWeightImage1.Text = "";
            lblOverDryWeightImage2.Text = "";
            lnkSGViewImages.Visible = false;
        }
        
        protected void ddl_MaterialName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbl_OtherPending.Text == "Material Name" && ddl_OtherPendingRpt.SelectedItem.Text != "---Select---")
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Please Select Material Name";
                lblMsg.Visible = false;
                lnkSave.Enabled = true;
                lbl_FMres.Text = "Awaited";
                txt_MaterialName.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                ShowTabMaterialwise();

                DisplayMF_SieveAnalysis();

                lbl_Specres.Text = "Awaited";
                grd_SpecificGravity.DataSource = null;
                grd_SpecificGravity.DataBind();
                lblSSDImage1.Text = "";
                lblSSDImage2.Text = "";
                lblWtBottleSampleImage1.Text = "";
                lblWtBottleSampleImage2.Text = "";
                lblOverDryWeightImage1.Text = "";
                lblOverDryWeightImage2.Text = "";
                lnkSGViewImages.Visible = false;

                txt_TotalwtofAggregate.Text = "";
                txt_VolumeofCylender.Text = "";
                txt_LBDresult.Text = "Awaited";

                txt_InitialWt.Text = "";
                txt_DryWt.Text = "";
                txt_MoistureContent.Text = "Awaited";

                txt_SSDforWT.Text = "";
                txt_OvenDrywtforWT.Text = "";
                txt_WaterAbsorption.Text = "Awaited";

                txt_InitialWtofSild.Text = "";
                txt_RetainedWtmicronSieve.Text = "";
                txt_SildContent.Text = "Awaited";

                grd_SpecificGravity.DataSource = null;
                grd_SpecificGravity.DataBind();

                grd_Flakiness.DataSource = null;
                grd_Flakiness.DataBind();

                grd_Elongation.DataSource = null;
                grd_Elongation.DataBind();

                grd_ImpactValue.DataSource = null;
                grd_ImpactValue.DataBind();

                grd_CrushValue.DataSource = null;
                grd_CrushValue.DataBind();

                DisplayGrid_SpecificGravity();
                DisplayGrid_Flakiness();
                DisplayGrid_Elongation();
                DisplayGrid_ImpactValue();
                DisplayGrid_CrushingValue();
                DisplayGrid_OtherTest();
            }
        }
        public void DisplayMF_SieveAnalysis()
        {
            grdAggtEntryRptInward.DataSource = null;
            grdAggtEntryRptInward.DataBind();
            int i = 0;
            if (ddl_OtherPendingRpt.SelectedItem.Text != "---Select---")
            {
                grdAggtEntryRptInward.Visible = true;
                var MFaggtInward = dc.MF_View(txt_ReferenceNo.Text, Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue), "MF");
                foreach (var mf in MFaggtInward)
                {
                    if (i == 0)
                    {
                        if (mf.AGGTINWD_FM_var != null && mf.AGGTINWD_FM_var != string.Empty)
                        {
                            lbl_FMres.Text = mf.AGGTINWD_FM_var.ToString();
                        }

                        txt_ReferenceNo.Text = mf.AGGTINWD_ReferenceNo_var.ToString();
                        txt_RecType.Text = mf.AGGTINWD_RecordType_var.ToString();
                        txt_ReportNo.Text = mf.AGGTINWD_SetOfRecord_var.ToString();
                        
                        if (Convert.ToString(mf.AGGTINWD_TestedDate_dt) != "")
                        {
                            txt_DateOfTesting.Text = Convert.ToDateTime(mf.AGGTINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                        }
                        if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                        {
                            txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        }
                        if (mf.AGGTINWD_SampleCondition_var != string.Empty && mf.AGGTINWD_SampleCondition_var != null)
                        {
                            txt_SampleCondition.Text = mf.AGGTINWD_SampleCondition_var.ToString();
                        }
                        else
                        {
                            //txt_SampleCondition.Text = string.Empty;
                            txt_SampleCondition.Text = "Satisfactory";
                        }
                        txt_SupplierName.Text = mf.AGGTINWD_SupplierName_var.ToString();
                        txt_Description.Text = mf.AGGTINWD_Description_var.ToString();
                        if (mf.AGGTINWD_WitnessBy_var.ToString() != null && mf.AGGTINWD_WitnessBy_var.ToString() != "")
                        {
                            txt_witnessBy.Visible = true;
                            txt_witnessBy.Text = mf.AGGTINWD_WitnessBy_var.ToString();
                            chk_WitnessBy.Checked = true;
                        }
                        else
                        {
                            txt_witnessBy.Visible = false;
                            chk_WitnessBy.Checked = false;
                            txt_witnessBy.Text = "";
                        }
                        i++;
                    }
                }
                DisplayGrid_SieveAnalysis();
            }
            else
            {
                grdAggtEntryRptInward.Visible = false;
            }
            ShowSieveAnalysis();
            if (grdAggtEntryRptInward.Rows.Count <= 0)
            {
                DisplayGridAGGTData();
            }
        }
        //private void LoadMaterialList()
        //{
        //    ddl_MaterialName.DataTextField = "Material_List";
        //    ddl_MaterialName.DataValueField = "Material_Id";
        //    var MaterialName = dc.MaterialListView(Convert.ToString(txt_ReportNo.Text),"", "Aggregate");
        //    ddl_MaterialName.DataSource = MaterialName;
        //    ddl_MaterialName.DataBind();
        //    ddl_MaterialName.Items.Insert(0, "---Select---");
        //}

        private void LoadMaterialList()
        {
            ddl_OtherPendingRpt.DataTextField = "Material_List";
            ddl_OtherPendingRpt.DataValueField = "Material_Id";
            var MaterialName = dc.MaterialListView(Convert.ToString(lblReportNo.Text), "", "Aggregate");
            ddl_OtherPendingRpt.DataSource = MaterialName;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            var MaterialName1 = dc.MaterialListView(Convert.ToString(lblReportNo.Text), "", "Aggregate");
            foreach (var mt in MaterialName1)
            {
                lblRecordNo.Text = mt.MaterialDetail_RecordNo.ToString();
                lblReportNo.Text = mt.MaterialDetail_RecordNo.ToString();
            }
            MaterialName.Dispose();
            MaterialName1.Dispose();
        }

        private void LoadTestedBy()
        {
            ddl_TestedBy.DataTextField = "USER_Name_var";
            ddl_TestedBy.DataValueField = "USER_Id";
            var testinguser = dc.ReportStatus_View("", null, null, 0, 1, 0, "", 0, 0, 0);
            ddl_TestedBy.DataSource = testinguser;
            ddl_TestedBy.DataBind();
            ddl_TestedBy.Items.Insert(0, "---Select---");
        }
        private void LoadApproveBy()
        {
            if (lblEntry.Text == "Check")
            {
                ddl_TestedBy.DataTextField = "USER_Name_var";
                ddl_TestedBy.DataValueField = "USER_Id";
                var testinguser = dc.ReportStatus_View("", null, null, 0, 0, 1, "", 0, 0, 0);
                ddl_TestedBy.DataSource = testinguser;
                ddl_TestedBy.DataBind();
                ddl_TestedBy.Items.Insert(0, "---Select---");
            }
            else
            {
                LoadTestedBy();
            }
        }
        private void LoadOtherPendingRpt()
        {
            var testinguser = dc.ReportStatus_View("Aggregate Testing", null, null, 0, 0, 0, "", Convert.ToInt32( lblRecordNo.Text ), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "AGGTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "AGGTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(txt_ReferenceNo.Text);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                var testinguser = dc.ReportStatus_View("Aggregate Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text ), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "AGGTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "AGGTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataSource = testinguser;
                ddl_OtherPendingRpt.DataBind();
                ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
                ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(txt_ReferenceNo.Text);
                if (itemToRemove != null)
                {
                    ddl_OtherPendingRpt.Items.Remove(itemToRemove);
                }
                lbl_TestedBy.Text = "Approve By";
            }
            else
            {
                LoadOtherPendingRpt();
            }
        }
        public void ViewWitnessBy()
        {
              txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ct = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text , 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.AGGTINWD_WitnessBy_var.ToString() != null && c.AGGTINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.AGGTINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }
        }


        public void Display_Alldata()
        {
            DisplayTab();
            DisplayGrid_SieveAnalysis();
            DisplayGrid_SpecificGravity();
            DisplayGrid_Flakiness();
            DisplayGrid_Elongation();
            DisplayGrid_ImpactValue();
            DisplayGrid_CrushingValue();
            DisplayGrid_OtherTest();
            DisplayRemark();
            Show_Tab();

        }
        protected void Tab_SieveAnalysis_Click(object sender, EventArgs e)
        {
            Tab_SieveAnalysis.CssClass = "Click";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Flaki.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
            MainView.ActiveViewIndex = 0;
            DisplayGrid_SieveAnalysis();
            Show_Tab();
        }
        protected void Tab_SpecGravity_Click(object sender, EventArgs e)
        {
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
            MainView.ActiveViewIndex = 1;
            DisplayGrid_SpecificGravity();
            Show_Tab();
        }
        protected void Tab_Flaki_Click(object sender, EventArgs e)
        {
            Tab_Flaki.CssClass = "Click";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
            MainView.ActiveViewIndex = 2;
            DisplayGrid_Flakiness();
            Show_Tab();
        }

        protected void Tab_Elong_Click(object sender, EventArgs e)
        {
            Tab_Elong.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
            MainView.ActiveViewIndex = 3;
            DisplayGrid_Elongation();
            Show_Tab();
        }
        protected void Tab_Impval_Click(object sender, EventArgs e)
        {
            Tab_Impval.CssClass = "Click";
            Tab_Elong.CssClass = "Initiative";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
            MainView.ActiveViewIndex = 4;
            Show_Tab();
            DisplayGrid_ImpactValue();
        }

        protected void Tab_Crushval_Click(object sender, EventArgs e)
        {
            Tab_Crushval.CssClass = "Click";
            Tab_Impval.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
            MainView.ActiveViewIndex = 5;
            DisplayGrid_CrushingValue();
            Show_Tab();
        }
        protected void Tab_OtherTest_Click(object sender, EventArgs e)
        {
            Tab_OtherTest.CssClass = "Click";
            Tab_Crushval.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
            MainView.ActiveViewIndex = 6;
            DisplayGrid_OtherTest();
            Show_Tab();
        }
        protected void Tab_Remark_Click(object sender, EventArgs e)
        {
            Tab_Remark.CssClass = "Click";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            MainView.ActiveViewIndex = 7;
            DisplayRemark();
            Show_Tab();
        }
        public void DisplayAGGT_Details()
        {
            try
            {
              
                // ddl_MaterialName.Items.Insert(0, "---Select---");
                string TestNamevar = "";
                string TestSrNo = "";
                var Inward_AggT = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text, 0, 0, 0);
                foreach (var aggt in Inward_AggT)
                {
                    txt_ReferenceNo.Text = aggt.AGGTINWD_ReferenceNo_var.ToString();
                   // Session["ReferenceNo"] = txt_ReferenceNo.Text;
                    txt_RecType.Text = aggt.AGGTINWD_RecordType_var.ToString();
                    txt_ReportNo.Text = aggt.AGGTINWD_SetOfRecord_var.ToString();
                    if (Convert.ToString(aggt.AGGTINWD_TestedDate_dt) != "")
                    {
                        txt_DateOfTesting.Text = Convert.ToDateTime(aggt.AGGTINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txt_DateOfTesting.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (aggt.AGGTINWD_SampleCondition_var != string.Empty && aggt.AGGTINWD_SampleCondition_var != null)
                    {
                        txt_SampleCondition.Text = aggt.AGGTINWD_SampleCondition_var.ToString();
                    }
                    else
                    {
                        txt_SampleCondition.Text = string.Empty;
                    }
                    txt_SupplierName.Text = aggt.AGGTINWD_SupplierName_var.ToString();
                    txt_Description.Text = aggt.AGGTINWD_Description_var.ToString();
                    txt_MaterialName.Text = aggt.AGGTINWD_AggregateName_var.ToString();
                    if (ddl_NablScope.Items.FindByValue(aggt.AGGTINWD_NablScope_var) != null)
                    {
                        ddl_NablScope.SelectedValue = Convert.ToString(aggt.AGGTINWD_NablScope_var);
                    }

                    if (Convert.ToString(aggt.AGGTINWD_NablLocation_int) != null && Convert.ToString(aggt.AGGTINWD_NablLocation_int) != "")
                    {
                        ddl_NABLLocation.SelectedValue = Convert.ToString(aggt.AGGTINWD_NablLocation_int);
                    }
                    var data = dc.MaterialListView("", txt_MaterialName.Text.Trim(), "");
                    foreach (var d in data)
                    {
                        lbl_MaterialId.Text = d.Material_Id.ToString();
                    }
                    if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var.ToString() != "")
                    {
                        lbl_Specres.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                    }
                    if (aggt.AGGTINWD_LBD_var != null && aggt.AGGTINWD_LBD_var.ToString() != "")
                    {
                        txt_LBDresult.Text = aggt.AGGTINWD_LBD_var.ToString();
                    }
                    if (aggt.AGGTINWD_WaterAborp_var != null && aggt.AGGTINWD_WaterAborp_var.ToString() != "")
                    {
                        txt_WaterAbsorption.Text = aggt.AGGTINWD_WaterAborp_var.ToString();
                    }
                    if (aggt.AGGTINWD_MoistureContent_var != null && aggt.AGGTINWD_MoistureContent_var.ToString() != "")
                    {
                        txt_MoistureContent.Text = aggt.AGGTINWD_MoistureContent_var.ToString();
                    }
                    if (aggt.AGGTINWD_CrushingValue_var != null && aggt.AGGTINWD_CrushingValue_var.ToString() != "")
                    {
                        lbl_Crushingres.Text = aggt.AGGTINWD_CrushingValue_var.ToString();
                    }
                    if (aggt.AGGTINWD_ImpactValue_var != null && aggt.AGGTINWD_ImpactValue_var.ToString() != "")
                    {
                        lbl_Impactres.Text = aggt.AGGTINWD_ImpactValue_var.ToString();
                    }
                    if (aggt.AGGTINWD_Elongation_var != null && aggt.AGGTINWD_Elongation_var.ToString() != "")
                    {
                        lbl_Elongres.Text = aggt.AGGTINWD_Elongation_var.ToString();
                    }
                    if (aggt.AGGTINWD_Flakiness_var != null && aggt.AGGTINWD_Flakiness_var.ToString() != "")
                    {
                        lbl_Flakires.Text = aggt.AGGTINWD_Flakiness_var.ToString();
                    }
                    var aggtTestname = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AGGT");
                    foreach (var aggtt in aggtTestname)
                    {
                        TestSrNo = aggtt.TEST_Sr_No.ToString();
                        TestNamevar = aggtt.TEST_Name_var.ToString();

                        if (TestSrNo == "1" && TestSrNo != string.Empty)//srNo =1 //Sieve Analysis
                        {
                            Tab_SieveAnalysis.Enabled = true;
                        }
                        if (TestSrNo == "2" && TestSrNo != string.Empty)//SrNo=3  //Mat.Fina Than 75u.
                        {
                            Tab_OtherTest.Enabled = true;
                            Pnl_SildContent.Visible = true;
                            lbl_SildStatus.Text = "1";
                        }
                        if (TestSrNo == "3" && TestSrNo != string.Empty)//SrNo=3  //Spec.Grav & WaterAbs.
                        {
                            Tab_OtherTest.Enabled = true;
                            Pnl_WaterAbsorbtion.Visible = true;
                            Tab_SpecGravity.Enabled = true;
                            lbl_WtStatus.Text = "1";
                        }
                        if (TestSrNo == "4" && TestSrNo != string.Empty)//SrNo=3  //LBD or Bulk Density
                        {
                            Tab_OtherTest.Enabled = true;
                            Pnl_LBD.Visible = true;
                            lbl_LBDStatus.Text = "1";
                        }
                        if (TestSrNo == "5" && TestSrNo != string.Empty)//SrNo=3  //Flakiness
                        {
                            Tab_Flaki.Enabled = true;
                        }
                        if (TestSrNo == "6" && TestSrNo != string.Empty)//SrNo=3  //Elongation
                        {
                            Tab_Elong.Enabled = true;
                        }
                        if (TestSrNo == "7" && TestSrNo != string.Empty)//SrNo=3  //Impact Value
                        {
                            Tab_Impval.Enabled = true;
                        }
                        if (TestSrNo == "8" && TestSrNo != string.Empty)//SrNo=3  //Crushed value
                        {
                            Tab_Crushval.Enabled = true;
                        }
                        if (TestSrNo == "9" && TestSrNo != string.Empty)//SrNo=3  //Moisture Content
                        {
                            Tab_OtherTest.Enabled = true;
                            Pnl_MoistureContent.Visible = true;
                            lbl_MoistStatus.Text = "1";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {

            }
        }
        public void Show_Tab()
        {
            if (Tab_SieveAnalysis.Enabled == false)
            {
                Tab_SieveAnalysis.CssClass = "DisableTab";
            }
            if (Tab_SpecGravity.Enabled == false)
            {
                Tab_SpecGravity.CssClass = "DisableTab";
            }
            if (Tab_Flaki.Enabled == false)
            {
                Tab_Flaki.CssClass = "DisableTab";
            }
            if (Tab_Elong.Enabled == false)
            {
                Tab_Elong.CssClass = "DisableTab";
            }
            if (Tab_Impval.Enabled == false)
            {
                Tab_Impval.CssClass = "DisableTab";
            }
            if (Tab_Crushval.Enabled == false)
            {
                Tab_Crushval.CssClass = "DisableTab";
            }
            if (Tab_OtherTest.Enabled == false)
            {
                Tab_OtherTest.CssClass = "DisableTab";
            }

            if (lbl_OtherPending.Text == "Material Name")
            {
                Tab_Remark.CssClass = "Initiative";
                Tab_Remark.CssClass = "DisableTab";
            }
        }
        public void DisplayTab()
        {
            for (int i = 0; i < 7; i++)
            {
                if (Tab_SieveAnalysis.Enabled == true)
                {
                    MainView.ActiveViewIndex = 0;
                    lbl_FMres.Text = "Awaited";
                    Tab_SieveAnalysis.CssClass = "Click";
                    Tab_Sieve();
                    break;
                }
                if (Tab_SpecGravity.Enabled == true)
                {
                    MainView.ActiveViewIndex = 1;
                    lbl_Specres.Text = "Awaited";
                    Tab_SpecGravity.CssClass = "Click";
                    Tab_Spec();
                    break;
                }
                if (Tab_Flaki.Enabled == true)
                {
                    MainView.ActiveViewIndex = 2;
                    lbl_Flaki.Text = "Awaited";
                    Tab_Flaki.CssClass = "Click";
                    Tab_Flakee();
                    break;
                }
                if (Tab_Elong.Enabled == true)
                {
                    MainView.ActiveViewIndex = 3;
                    lbl_Elongres.Text = "Awaited";
                    Tab_Elong.CssClass = "Click";
                    Tab_Elongness();
                    break;
                }
                if (Tab_Impval.Enabled == true)
                {
                    MainView.ActiveViewIndex = 4;
                    lbl_Impactres.Text = "Awaited";
                    Tab_Impval.CssClass = "Click";
                    Tab_Impac();
                    break;
                }
                if (Tab_Crushval.Enabled == true)
                {
                    MainView.ActiveViewIndex = 5;
                    lbl_Crushingres.Text = "Awaited";
                    Tab_Crushval.CssClass = "Click";
                    Tab_Crushing();
                    break;
                }
                if (Tab_OtherTest.Enabled == true)
                {
                    MainView.ActiveViewIndex = 6;
                    Tab_OtherTest.CssClass = "Click";
                    Tab_Other();
                    if (Pnl_LBD.Visible == true)
                    {
                        txt_LBDresult.Text = "Awaited";
                    }
                    if (Pnl_WaterAbsorbtion.Visible == true)
                    {
                        txt_WaterAbsorption.Text = "Awaited";
                    }
                    if (Pnl_MoistureContent.Visible == true)
                    {
                        txt_MoistureContent.Text = "Awaited";
                    }
                    if (Pnl_SildContent.Visible == true)
                    {
                        txt_SildContent.Text = "Awaited";
                    }
                    break;
                }
                if (Tab_Remark.Enabled == true)
                {
                    MainView.ActiveViewIndex = 7;
                    Tab_Remark.CssClass = "Click";
                    Tab_Remarks();
                    break;
                }
            }
        }
        public void DisplayGrid_SpecificGravity()
        {

            if (grd_SpecificGravity.Rows.Count <= 0)
            {
                int MaterialId = 0;
                if (lbl_OtherPending.Text == "Material Name")
                {
                    MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                }
                int i = 0;
                var aggt_SG = dc.AggregateAllTestView(txt_ReferenceNo.Text, MaterialId, "AGGTSG");
                foreach (var aggt in aggt_SG)
                {
                    if (i == 0)
                    {
                        if (aggt.AGGTINWD_SpecificGravity_var != null && aggt.AGGTINWD_SpecificGravity_var != string.Empty)
                        {
                            lbl_Specres.Text = aggt.AGGTINWD_SpecificGravity_var.ToString();
                        }
                        else
                        {

                            var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                            foreach (var d in data)
                            {
                                if (d.AGGTINWD_SpecificGravity_var != null && d.AGGTINWD_SpecificGravity_var != string.Empty)
                                {
                                    lbl_Specres.Text = d.AGGTINWD_SpecificGravity_var.ToString();

                                }
                            }

                        }
                    }
                    AddRowforSpecificGravity();
                    TextBox txt_SSD = (TextBox)grd_SpecificGravity.Rows[i].Cells[1].FindControl("txt_SSD");
                    TextBox txt_WtOfBottleSample = (TextBox)grd_SpecificGravity.Rows[i].Cells[2].FindControl("txt_WtOfBottleSample");
                    TextBox txt_WtOfBottleDistilled = (TextBox)grd_SpecificGravity.Rows[i].Cells[3].FindControl("txt_WtOfBottleDistilled");
                    TextBox txt_OvenDryWeight = (TextBox)grd_SpecificGravity.Rows[i].Cells[4].FindControl("txt_OvenDryWeight");
                    TextBox txt_SpecificGravity = (TextBox)grd_SpecificGravity.Rows[i].Cells[5].FindControl("txt_SpecificGravity");

                    txt_SSD.Text = aggt.AGGTSG_SSD_dec.ToString();
                    txt_WtOfBottleSample.Text = aggt.AGGTSG_WtBottleSample_dec.ToString();
                    txt_WtOfBottleDistilled.Text = aggt.AGGTSG_WtBottleDistilled_dec.ToString();
                    txt_OvenDryWeight.Text = aggt.AGGTSG_OvenDryWeight_dec.ToString();
                    txt_SpecificGravity.Text = aggt.AGGTSG_SpecificGravity_dec.ToString();

                    if (i == 0)
                    {
                        if (aggt.AGGTSG_SSDImage1_var != null)
                            lblSSDImage1.Text = aggt.AGGTSG_SSDImage1_var;
                        if (aggt.AGGTSG_WtBottleSampleImage1_var != null)
                            lblWtBottleSampleImage1.Text = aggt.AGGTSG_WtBottleSampleImage1_var;
                        if (aggt.AGGTSG_OverDryWeightImage1_var != null)
                            lblOverDryWeightImage1.Text = aggt.AGGTSG_OverDryWeightImage1_var;
                    }
                    else if (i == 1)
                    {
                        if (aggt.AGGTSG_SSDImage1_var != null)
                            lblSSDImage2.Text = aggt.AGGTSG_SSDImage1_var;
                        if (aggt.AGGTSG_WtBottleSampleImage1_var != null)
                            lblWtBottleSampleImage2.Text = aggt.AGGTSG_WtBottleSampleImage1_var;
                        if (aggt.AGGTSG_OverDryWeightImage1_var != null)
                            lblOverDryWeightImage2.Text = aggt.AGGTSG_OverDryWeightImage1_var;
                    }
                    if (lblSSDImage1.Text != "" || lblWtBottleSampleImage1.Text != "" || lblOverDryWeightImage1.Text != ""
                        || lblSSDImage2.Text != "" || lblWtBottleSampleImage2.Text != "" || lblOverDryWeightImage2.Text != "")
                    {
                        lnkSGViewImages.Visible = true;
                    }
                    i++;
                }
                if (grd_SpecificGravity.Rows.Count == 1)
                {
                    AddRowforSpecificGravity();
                }
                if (grd_SpecificGravity.Rows.Count <= 0)
                {
                    DisplaySpec_Gravity();
                }

            }
        }
        public void DisplayGrid_Flakiness()
        {
            if (grd_Flakiness.Rows.Count <= 0)
            {
                int MaterialId = 0;
                if (lbl_OtherPending.Text == "Material Name")
                {
                    MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                }
                int i = 0;
                var aggt_FK = dc.AggregateAllTestView(txt_ReferenceNo.Text, MaterialId, "AGGTFK");
                foreach (var aggt in aggt_FK)
                {
                    if (i == 0)
                    {
                        if (aggt.AGGTINWD_Flakiness_var != null && aggt.AGGTINWD_Flakiness_var != string.Empty)
                        {
                            lbl_Flakires.Text = aggt.AGGTINWD_Flakiness_var.ToString();
                        }
                        else
                        {

                            var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                            foreach (var d in data)
                            {
                                if (d.AGGTINWD_Flakiness_var != null && d.AGGTINWD_Flakiness_var != string.Empty)
                                {
                                    lbl_Flakires.Text = d.AGGTINWD_Flakiness_var.ToString();
                                }
                            }

                        }
                    }
                    AddRowforFlakiness();
                    TextBox txt_SieveSize = (TextBox)grd_Flakiness.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                    TextBox txt_picesespassing = (TextBox)grd_Flakiness.Rows[i].Cells[3].FindControl("txt_picesespassing");
                    TextBox txt_GaugePassingWt = (TextBox)grd_Flakiness.Rows[i].Cells[4].FindControl("txt_GaugePassingWt");
                    TextBox txt_GaugePassingSampleNo = (TextBox)grd_Flakiness.Rows[i].Cells[5].FindControl("txt_GaugePassingSampleNo");
                    TextBox txt_Total = (TextBox)grd_Flakiness.Rows[i].Cells[6].FindControl("txt_Total");

                    txt_SieveSize.Text = aggt.AGGTFK_SieveSize_var.ToString();
                    txt_RetainedmassonSieve.Text = aggt.AGGTFK_RetainedmassSeive_dec.ToString();
                    txt_picesespassing.Text = aggt.AGGTFK_Picesespassing_dec.ToString();
                    txt_GaugePassingWt.Text = aggt.AGGTFK_Gaugepassingwt_dec.ToString();
                    txt_GaugePassingSampleNo.Text = aggt.AGGTFK_GaugepassingSample_dec.ToString();
                    txt_Total.Text = aggt.AGGTFK_Total_dec.ToString();
                    i++;
                }
                if (grd_Flakiness.Rows.Count <= 0)
                {
                    DisplayFlakinessIndex();
                }
            }
        }
        public void DisplayGrid_Elongation()
        {
            if (grd_Elongation.Rows.Count <= 0)
            {
                int MaterialId = 0;
                if (lbl_OtherPending.Text == "Material Name")
                {
                    MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                }
                int i = 0;
                var aggtEL = dc.AggregateAllTestView(txt_ReferenceNo.Text, MaterialId, "AGGTEL");
                foreach (var aggt in aggtEL)
                {
                    if (i == 0)
                    {
                        if (aggt.AGGTINWD_Elongation_var != null && aggt.AGGTINWD_Elongation_var != string.Empty)
                        {
                            lbl_Elongres.Text = aggt.AGGTINWD_Elongation_var.ToString();
                        }
                        else
                        {

                            var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                            foreach (var d in data)
                            {
                                if (d.AGGTINWD_Elongation_var != null && d.AGGTINWD_Elongation_var != string.Empty)
                                {
                                    lbl_Elongres.Text = d.AGGTINWD_Elongation_var.ToString();
                                }
                            }

                        }
                    }
                    AddRowforElongation();
                    TextBox txt_SieveSize = (TextBox)grd_Elongation.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                    TextBox txt_picesesretained = (TextBox)grd_Elongation.Rows[i].Cells[3].FindControl("txt_picesesretained");
                    TextBox txt_GaugeretainedWt = (TextBox)grd_Elongation.Rows[i].Cells[4].FindControl("txt_GaugeretainedWt");
                    TextBox txt_GaugeretainedSampleNo = (TextBox)grd_Elongation.Rows[i].Cells[5].FindControl("txt_GaugeretainedSampleNo");
                    TextBox txt_Total = (TextBox)grd_Elongation.Rows[i].Cells[6].FindControl("txt_Total");

                    txt_SieveSize.Text = aggt.AGGTEL_SieveSize_var.ToString();
                    txt_RetainedmassonSieve.Text = aggt.AGGTEL_RetainedmassSieve_dec.ToString();
                    txt_picesesretained.Text = aggt.AGGTEL_Picesesretainedmass_dec.ToString();
                    txt_GaugeretainedWt.Text = aggt.AGGTEL_Gaugeretainedwt_dec.ToString();
                    txt_GaugeretainedSampleNo.Text = aggt.AGGTEL_GaugeretainedSample_dec.ToString();
                    txt_Total.Text = aggt.AGGTEL_Total_dec.ToString();
                    i++;
                }
                if (grd_Elongation.Rows.Count <= 0)
                {
                    DisplayElongationIndex();
                }
            }
        }
        public void DisplayGrid_CrushingValue()
        {
            if (grd_CrushValue.Rows.Count <= 0)
            {
                int MaterialId = 0;
                if (lbl_OtherPending.Text == "Material Name")
                {
                    MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                }
                int i = 0;
                var aggtcv = dc.AggregateAllTestView(txt_ReferenceNo.Text, MaterialId, "AGGTCV");
                foreach (var aggt in aggtcv)
                {
                    if (i == 0)
                    {
                        if (aggt.AGGTINWD_CrushingValue_var != null && aggt.AGGTINWD_CrushingValue_var != string.Empty)
                        {
                            lbl_Crushingres.Text = aggt.AGGTINWD_CrushingValue_var.ToString();
                        }
                        else
                        {

                            var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                            foreach (var d in data)
                            {
                                if (d.AGGTINWD_CrushingValue_var != null && d.AGGTINWD_CrushingValue_var != string.Empty)
                                {
                                    lbl_Crushingres.Text = d.AGGTINWD_CrushingValue_var.ToString();
                                }
                            }

                        }
                    }
                    AddRowforCrushedValue();
                    TextBox txt_Initialwtsample = (TextBox)grd_CrushValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                    TextBox txt_WtSampleretained = (TextBox)grd_CrushValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                    TextBox txt_WtSamplepassing = (TextBox)grd_CrushValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                    TextBox txt_Passing = (TextBox)grd_CrushValue.Rows[i].Cells[6].FindControl("txt_Passing");

                    txt_Initialwtsample.Text = aggt.AGGTCV_InitialWtSample_dec.ToString();
                    txt_WtSampleretained.Text = aggt.AGGTCV_WtSampleRetained_dec.ToString();
                    txt_WtSamplepassing.Text = aggt.AGGTCV_WtSamplePassing_dec.ToString();
                    txt_Passing.Text = aggt.AGGTCV_Passing.ToString();
                    i++;
                }
                DisplayCrushingValue();
            }
        }
        public void DisplayGrid_ImpactValue()
        {
            if (grd_ImpactValue.Rows.Count <= 0)
            {
                int MaterialId = 0;
                if (lbl_OtherPending.Text == "Material Name")
                {
                    MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                }
                int i = 0;
                var aggtim = dc.AggregateAllTestView(txt_ReferenceNo.Text, MaterialId, "AGGTIM");
                foreach (var aggt in aggtim)
                {
                    if (i == 0)
                    {
                        if (aggt.AGGTINWD_ImpactValue_var != null && aggt.AGGTINWD_ImpactValue_var != string.Empty)
                        {
                            lbl_Impactres.Text = aggt.AGGTINWD_ImpactValue_var.ToString();
                        }
                        else
                        {

                            var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                            foreach (var d in data)
                            {
                                if (d.AGGTINWD_ImpactValue_var != null && d.AGGTINWD_ImpactValue_var != string.Empty)
                                {
                                    lbl_Impactres.Text = d.AGGTINWD_ImpactValue_var.ToString();
                                }
                            }

                        }
                    }
                    AddRowforImpactValue();
                    TextBox txt_Initialwtsample = (TextBox)grd_ImpactValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                    TextBox txt_WtSampleretained = (TextBox)grd_ImpactValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                    TextBox txt_WtSamplepassing = (TextBox)grd_ImpactValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                    TextBox txt_Passing = (TextBox)grd_ImpactValue.Rows[i].Cells[6].FindControl("txt_Passing");

                    txt_Initialwtsample.Text = aggt.AGGTIM_InitialSamplewt_dec.ToString();
                    txt_WtSampleretained.Text = aggt.AGGTIM_WtSampleRetained_dec.ToString();
                    txt_WtSamplepassing.Text = aggt.AGGTIM_WtSamplePassing_dec.ToString();
                    txt_Passing.Text = aggt.AGGTIM_Passing_dec.ToString();
                    i++;
                }
                if (grd_ImpactValue.Rows.Count <= 0)
                {
                    DisplayImpactValue();
                }
            }
        }
        public void DisplayGrid_OtherTest()
        {
            int MaterialId = 0;
            if (lbl_OtherPending.Text == "Material Name")
            {
                MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
            }
            var aggtOther = dc.AggregateAllTestView(txt_ReferenceNo.Text, MaterialId, "AGGTOT");
            foreach (var aggt in aggtOther)
            {
                if (txt_TotalwtofAggregate.Text == string.Empty && txt_VolumeofCylender.Text == string.Empty)
                {
                    if (Convert.ToString(aggt.AGGTOT_TotalWtAgg_dec) != string.Empty)
                    {
                        txt_TotalwtofAggregate.Text = aggt.AGGTOT_TotalWtAgg_dec.ToString();
                    }
                    if (Convert.ToString(aggt.AGGTOT_VolCylinder_dec) != string.Empty)
                    {
                        txt_VolumeofCylender.Text = aggt.AGGTOT_VolCylinder_dec.ToString();
                    }
                    if (Convert.ToString(aggt.AGGTINWD_LBD_var) != string.Empty && aggt.AGGTINWD_LBD_var != null)
                    {
                        txt_LBDresult.Text = aggt.AGGTINWD_LBD_var.ToString();
                    }
                    else
                    {

                        var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                        foreach (var d in data)
                        {
                            if (d.AGGTINWD_LBD_var != string.Empty && d.AGGTINWD_LBD_var != null)
                            {
                                txt_LBDresult.Text = d.AGGTINWD_LBD_var.ToString();
                            }
                        }

                    }
                }
                if (txt_SSDforWT.Text == string.Empty && txt_OvenDrywtforWT.Text == string.Empty)
                {
                    if (Convert.ToString(aggt.AGGTOT_SSDWaterAbsor_dec) != string.Empty)
                    {
                        txt_SSDforWT.Text = aggt.AGGTOT_SSDWaterAbsor_dec.ToString();
                    }
                    if (Convert.ToString(aggt.AGGTOT_OvenDryWt_dec) != string.Empty)
                    {
                        txt_OvenDrywtforWT.Text = aggt.AGGTOT_OvenDryWt_dec.ToString();
                    }
                    if (Convert.ToString(aggt.AGGTINWD_WaterAborp_var) != string.Empty && aggt.AGGTINWD_WaterAborp_var != null)
                    {
                        txt_WaterAbsorption.Text = Convert.ToString(aggt.AGGTINWD_WaterAborp_var);
                    }
                    else
                    {

                        var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                        foreach (var d in data)
                        {
                            if (d.AGGTINWD_WaterAborp_var != string.Empty && d.AGGTINWD_WaterAborp_var != null)
                            {
                                txt_WaterAbsorption.Text = d.AGGTINWD_WaterAborp_var.ToString();
                            }
                        }

                    }
                }
                if (txt_InitialWt.Text == string.Empty && txt_DryWt.Text == string.Empty)
                {
                    if (Convert.ToString(aggt.AGGTOT_InitialWt_dec) != string.Empty)
                    {
                        txt_InitialWt.Text = aggt.AGGTOT_InitialWt_dec.ToString();
                    }
                    if (Convert.ToString(aggt.AGGTOT_DryWt_dec) != string.Empty)
                    {
                        txt_DryWt.Text = aggt.AGGTOT_DryWt_dec.ToString();
                    }
                    if (Convert.ToString(aggt.AGGTINWD_MoistureContent_var) != string.Empty && aggt.AGGTINWD_MoistureContent_var != null)
                    {
                        txt_MoistureContent.Text = Convert.ToString(aggt.AGGTINWD_MoistureContent_var);
                    }
                    else
                    {

                        var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                        foreach (var d in data)
                        {
                            if (d.AGGTINWD_MoistureContent_var != string.Empty && d.AGGTINWD_MoistureContent_var != null)
                            {
                                txt_MoistureContent.Text = d.AGGTINWD_MoistureContent_var.ToString();
                            }
                        }

                    }
                }
                if (txt_InitialWtofSild.Text == string.Empty && txt_RetainedWtmicronSieve.Text == string.Empty)
                {
                    if (Convert.ToString(aggt.AGGTOT_InitialWtSieve_dec) != string.Empty)
                    {
                        txt_InitialWtofSild.Text = Convert.ToString(aggt.AGGTOT_InitialWtSieve_dec);
                    }
                    if (Convert.ToString(aggt.AGGTOT_RetainedWtSieve_dec) != string.Empty)
                    {
                        txt_RetainedWtmicronSieve.Text = Convert.ToString(aggt.AGGTOT_RetainedWtSieve_dec);
                    }
                    if (Convert.ToString(aggt.AGGTINWD_SildContent_var) != string.Empty && aggt.AGGTINWD_SildContent_var != null)
                    {
                        txt_SildContent.Text = Convert.ToString(aggt.AGGTINWD_SildContent_var);
                    }
                    else
                    {

                        var data = dc.MF_View(txt_ReferenceNo.Text, MaterialId, "MF");
                        foreach (var d in data)
                        {
                            if (d.AGGTINWD_SildContent_var != string.Empty && d.AGGTINWD_SildContent_var != null)
                            {
                                txt_SildContent.Text = d.AGGTINWD_SildContent_var.ToString();
                            }
                        }

                    }
                }
            }
        }
        public void DisplayGrid_SieveAnalysis()
        {
            if (grdAggtEntryRptInward.Rows.Count <= 0)
            {
                int MaterialId = 0;
                if (lbl_OtherPending.Text == "Material Name")
                {
                    MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                }

                int i = 0;
                var aggtTestname = dc.AggregateAllTestView(txt_ReferenceNo.Text, MaterialId, "AGGTSA");
                foreach (var aggtt in aggtTestname)
                {
                    if (i == 0)
                    {
                        if (aggtt.AGGTINWD_FM_var != null && aggtt.AGGTINWD_FM_var != string.Empty)
                        {
                            lbl_FMres.Text = aggtt.AGGTINWD_FM_var.ToString();
                        }

                    }
                    AddRowEnterReportAGGTInward();
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[2].FindControl("txt_Wt");
                    TextBox txt_WtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[3].FindControl("txt_WtRet");
                    TextBox txt_CumWtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[4].FindControl("txt_CumWtRet");
                    TextBox txt_CumuPassing = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[5].FindControl("txt_CumuPassing");
                    TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");

                    txt_SieveSize.Text = aggtt.AGGTSA_SeiveSize_var.ToString();
                    txt_Wt.Text = aggtt.AGGTSA_Weight_num.ToString();
                    txt_WtRet.Text = aggtt.AGGTSA_WeightRet_dec.ToString();
                    txt_CumWtRet.Text = aggtt.AGGTSA_CumuWeightRet_dec.ToString();
                    txt_CumuPassing.Text = aggtt.AGGTSA_CumuPassing_dec.ToString();
                    if (aggtt.AGGTSA_IsPassingLmt_var != null)
                        txt_Ispassinglmt.Text = aggtt.AGGTSA_IsPassingLmt_var.ToString();
                    if (txt_SieveSize.Text.Trim() == "Total")
                    {
                        txt_Wt.ReadOnly = true;
                        if (aggtt.AGGTSA_WeightRet_dec == 0)
                        {
                            txt_WtRet.Text = "";
                        }
                        if (aggtt.AGGTSA_CumuWeightRet_dec == 0)
                        {
                            txt_CumWtRet.Text = "";
                        }
                        if (aggtt.AGGTSA_CumuPassing_dec == 0)
                        {
                            txt_CumuPassing.Text = "";
                        }
                    }
                    i++;
                }
                ShowSieveAnalysis();
                if (grdAggtEntryRptInward.Rows.Count <= 0)
                {
                    DisplayGridAGGTData();
                }
            }
        }

        public void DisplaySpec_Gravity()
        {
            if (grd_SpecificGravity.Rows.Count <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    AddRowforSpecificGravity();
                }
            }
        }
        public void DisplayImpactValue()
        {
            if (grd_ImpactValue.Rows.Count <= 0)
            {
                AddRowforImpactValue();
            }
        }
        public void DisplayCrushingValue()
        {
            if (grd_CrushValue.Rows.Count <= 0)
            {
                AddRowforCrushedValue();
            }
        }
        public void DisplayGridAGGTData()
        {
            int i = 0;
            if (txt_MaterialName.Text.Trim() == "Natural Sand")
            {
                //string[] AGGT_SeiveSize = { "10 mm", "4.75 mm", "2.36 mm", "1.18 mm", "600 micron", "300 micron", "150 micron", "75 micron", "Pan", "Total" };
                string[] AGGT_SeiveSize = { "10 mm", "4.75 mm", "2.36 mm", "1.18 mm", "600 micron", "300 micron", "150 micron", "Pan", "Total" };
                for (int s = 0; s < AGGT_SeiveSize.Count(); s++)
                {
                    AddRowEnterReportAGGTInward();
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");
                    txt_SieveSize.Text = AGGT_SeiveSize[s];
                    //txt_Ispassinglmt.Text = "---";
                    i++;
                }
            }
            if (txt_MaterialName.Text.Trim() == "Crushed Sand" || txt_MaterialName.Text.Trim() == "Stone Dust" || txt_MaterialName.Text.Trim() == "Grit")
            {
                string[] AGGT_SeiveSize = { "10 mm", "4.75 mm", "2.36 mm", "1.18 mm", "600 micron", "300 micron", "150 micron", "75 micron", "Pan", "Total" };
                for (int s = 0; s < AGGT_SeiveSize.Count(); s++)
                {
                    AddRowEnterReportAGGTInward();
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");
                    txt_SieveSize.Text = AGGT_SeiveSize[s];
                    //   txt_Ispassinglmt.Text = "---";
                    i++;
                }
            }
            if (txt_MaterialName.Text.Trim() == "10 mm")
            {
                string[] AGGT_IsPasingLmt_10mm = { "---", "---", "---", "---", "100", "85 to 100", "0 to 20", "---", "---" };
                string[] AGGT_SeiveSize = { "40 mm", "25 mm", "20 mm", "16 mm", "12.5 mm", "10 mm", "4.75 mm", "2.36 mm", "Pan", "Total" };
                for (int s = 0; s < AGGT_SeiveSize.Count(); s++)
                {
                    AddRowEnterReportAGGTInward();
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");
                    txt_SieveSize.Text = AGGT_SeiveSize[s];
                    
                    if (s >= AGGT_IsPasingLmt_10mm.Count() - 1)
                    {
                        txt_Ispassinglmt.Text = "---";
                    }
                    else
                    {
                        txt_Ispassinglmt.Text = AGGT_IsPasingLmt_10mm[s];
                    }
                    
                    i++;
                }
            }
            if (txt_MaterialName.Text.Trim() == "20 mm" || txt_MaterialName.Text.Trim() == "Mix Aggt")
            {
                string[] AGGT_IsPasingLmt_20mm = { "100", "---", "85 to 100", "---", "---", "0 to 20", "0 to 5", "---" };
                string[] AGGT_IsPasingLmt_mixAggt = { "100", "---", "95 to 100", "---", "---", "25 to 55", "0 to 10", "---", "---" };
                string[] AGGT_SeiveSize = { "40 mm", "25 mm", "20 mm", "16 mm", "12.5 mm", "10 mm", "4.75 mm", "Pan", "Total" };
                for (int s = 0; s < AGGT_SeiveSize.Count(); s++)
                {
                    AddRowEnterReportAGGTInward();
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");
                    txt_SieveSize.Text = AGGT_SeiveSize[s];
                    
                    if (txt_MaterialName.Text.Trim() == "20 mm")
                    {
                        if (s >= AGGT_IsPasingLmt_20mm.Count() - 1)
                        {
                            txt_Ispassinglmt.Text = "---";
                        }
                        else
                        {
                            txt_Ispassinglmt.Text = AGGT_IsPasingLmt_20mm[s];
                        }
                    }
                    if (txt_MaterialName.Text.Trim() == "Mix Aggt")
                    {
                        if (s >= AGGT_IsPasingLmt_20mm.Count() - 1)
                        {
                            txt_Ispassinglmt.Text = "---";
                        }
                        else
                        {
                            txt_Ispassinglmt.Text = AGGT_IsPasingLmt_mixAggt[s];
                        }
                    }
                    i++;
                }
            }

            if (txt_MaterialName.Text.Trim() == "40 mm")
            {
                //string[] AGGT_IsPasingLmt_40mm = { "100", "---", "85-100", "---", "---", "0-20", "---" };
                string[] AGGT_IsPasingLmt_40mm = { "85 to 100", "---", "0 to 20", "---", "---", "0 to 5", "---" };                
                string[] AGGT_SeiveSize = { "40 mm", "25 mm", "20 mm", "16 mm", "12.5 mm", "10 mm", "Pan", "Total" };
                for (int s = 0; s < AGGT_SeiveSize.Count(); s++)
                {
                    AddRowEnterReportAGGTInward();
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");
                    txt_SieveSize.Text = AGGT_SeiveSize[s];

                    if (txt_MaterialName.Text.Trim() == "40 mm")
                    {
                        if (s >= AGGT_IsPasingLmt_40mm.Count() - 1)
                        {
                            txt_Ispassinglmt.Text = "---";
                        }
                        else
                        {
                            txt_Ispassinglmt.Text = AGGT_IsPasingLmt_40mm[s];
                        }
                    }
                    i++;
                }
            }
            ShowSieveAnalysis();

        }
        public void ShowSieveAnalysis()
        {
            for (int j = 0; j < grdAggtEntryRptInward.Rows.Count; j++)
            {
                TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[j].Cells[1].FindControl("txt_SieveSize");
                TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[j].Cells[2].FindControl("txt_Wt");
                TextBox txt_WtRet = (TextBox)grdAggtEntryRptInward.Rows[j].Cells[3].FindControl("txt_WtRet");
                TextBox txt_CumWtRet = (TextBox)grdAggtEntryRptInward.Rows[j].Cells[4].FindControl("txt_CumWtRet");
                TextBox txt_CumuPassing = (TextBox)grdAggtEntryRptInward.Rows[j].Cells[5].FindControl("txt_CumuPassing");
                TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[j].Cells[6].FindControl("txt_Ispassinglmt");

                if (txt_MaterialName.Text.Trim() == "10 mm" || txt_MaterialName.Text.Trim() == "20 mm" || txt_MaterialName.Text.Trim() == "40 mm" || txt_MaterialName.Text.Trim() == "Mix Aggt")
                {
                    grdAggtEntryRptInward.Columns[6].Visible = true;
                    txt_Wt.Width = 150;
                    txt_WtRet.Width = 150;
                    txt_CumWtRet.Width = 150;
                    txt_CumuPassing.Width = 150;
                }
                else
                {
                    grdAggtEntryRptInward.Columns[6].Visible = false;
                    txt_Wt.Width = 160;
                    txt_WtRet.Width = 200;
                    txt_CumWtRet.Width = 200;
                    txt_CumuPassing.Width = 200;
                }
                if (txt_SieveSize.Text == "Total")
                {
                    txt_Wt.ReadOnly = true;
                }
            }
        }
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            FetchRefNo();
        }
        public void FetchRefNo()
        {
            if ((ddl_OtherPendingRpt.SelectedValue != "---Select---" && txt_RecType.Text == "AGGT") ||
                (ddl_OtherPendingRptMF.SelectedValue != "---Select---" && txt_RecType.Text == "MF"))
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                lnkSave.Enabled = true;
                TabDisable();

                grdempty();
                lnkSave.Enabled = true;
                lnkPrint.Visible = false;
                if (txt_RecType.Text == "MF")
                {
                    ddl_OtherPendingRpt.Items.Clear();
                    lblReportNo.Text = ddl_OtherPendingRptMF.SelectedItem.Text;
                    DisplayMFDtls();
                }
                else
                {
                    txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                    DisplayAGGT_Details();
                    LoadReferenceNoList();
                    ViewWitnessBy();
                    LoadApproveBy();
                    Display_Alldata();
                    grdAggtRemark.DataSource = null;
                    grdAggtRemark.DataBind();
                    DisplayRemark();
                }
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "Select report number.";
            }
        }
        public void TabDisable()
        {
            Tab_SieveAnalysis.Enabled = false;
            Tab_SpecGravity.Enabled = false;
            Tab_Flaki.Enabled = false;
            Tab_Elong.Enabled = false;
            Tab_Impval.Enabled = false;
            Tab_Crushval.Enabled = false;
            Tab_OtherTest.Enabled = false;
            Pnl_LBD.Visible = false;
            Pnl_WaterAbsorbtion.Visible = false;
            Pnl_MoistureContent.Visible = false;
            Pnl_SildContent.Visible = false;
        }
        public void DisplayRemark()
        {
            if (grdAggtRemark.Rows.Count <= 0)
            {
                int i = 0;
                var re = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "AGGT");
                foreach (var r in re)
                {
                    AddRowAGGT_Remark();
                    TextBox txt_REMARK = (TextBox)grdAggtRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.AGGTDetail_RemarkId_int), "AGGT");
                    foreach (var rem in remark)
                    {
                        txt_REMARK.Text = rem.AGGT_Remark_var.ToString();
                        i++;
                    }
                }
                if (grdAggtRemark.Rows.Count <= 0)
                {
                    AddRowAGGT_Remark();
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);

                if (lbl_OtherPending.Text != "Material Name")
                {
                    //if (lbl_FMres.Text != "Awaited" && lbl_Specres.Text != "Awaited" && lbl_Impactres.Text != "Awaited" && lbl_Flakires.Text != "Awaited" && lbl_Elongres.Text != "Awaited" && lbl_Crushingres.Text != "Awaited" && txt_LBDresult.Text != "Awaited" && txt_WaterAbsorption.Text != "Awaited" && txt_SildContent.Text != "Awaited" && txt_MoistureContent.Text != "Awaited")
                    //{
                    byte reportStatus = 0;
                        if (lbl_TestedBy.Text == "Tested By")
                        {   
                            if(chkComplete.Checked == true)
                                reportStatus = 2;
                            else
                                reportStatus = 1;
                            dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_SupplierName.Text, reportStatus, 0, "", 0, "", "", 0, 0, 0, "AGGT");
                            dc.ReportDetails_Update("AGGT", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                            dc.MISDetail_Update(0, "AGGT", txt_ReferenceNo.Text, "AGGT", null, true, false, false, false, false, false, false);
                        }
                        else if (lbl_TestedBy.Text == "Approve By")
                        {
                            if (chkComplete.Checked == true)
                                reportStatus = 3;
                            else
                                reportStatus = 2;
                            dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_SupplierName.Text, reportStatus, 0, "", 0, "", "", 0, 0, 0, "AGGT");
                            dc.ReportDetails_Update("AGGT", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                            dc.MISDetail_Update(0, "AGGT", txt_ReferenceNo.Text, "AGGT", null, false, true, false, false, false, false, false);
                        }
                    //}
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "AGGT", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                int MaterialId = 0;
                if (lbl_OtherPending.Text == "Material Name")
                {
                    MaterialId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                    clsData obj= new clsData();
                    dc.MaterialDetail_UpdateStatus("SieveAnalysis", obj.getRecordNo(txt_ReferenceNo.Text), MaterialId, 1); 
                }
                else
                {
                    if (lbl_MaterialId.Text != "")
                    {
                        MaterialId = Convert.ToInt32(lbl_MaterialId.Text);
                    }
                }
                //Sieve Analysis
                if (Tab_SieveAnalysis.Enabled == true)
                {
                    if (lbl_OtherPending.Text != "Material Name")
                    {
                        dc.AggregateTestSA_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", MaterialId, true);
                    }
                    else
                    {
                        if (lbl_FMres.Text != "Awaited")
                        {
                            if (lbl_TestedBy.Text == "Tested By")
                            {
                                dc.MFAggtInward_Update(txt_RecType.Text, txt_ReferenceNo.Text, TestingDt, txt_Description.Text, txt_SupplierName.Text, txt_SampleCondition.Text,1, txt_witnessBy.Text, Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue));
                            }
                        }
                        dc.AggregateTestSA_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue), true);
                    }
                    for (int i = 0; i < grdAggtEntryRptInward.Rows.Count; i++)
                    {
                        TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                        TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[2].FindControl("txt_Wt");
                        TextBox txt_WtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[3].FindControl("txt_WtRet");
                        TextBox txt_CumWtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[4].FindControl("txt_CumWtRet");
                        TextBox txt_CumuPassing = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[5].FindControl("txt_CumuPassing");
                        TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");
                        if (txt_SieveSize.Text.Trim() == "Total")
                        {
                            txt_WtRet.Text = "0";
                            txt_CumWtRet.Text = "0";
                            txt_CumuPassing.Text = "0";
                        }

                        dc.AggregateTestSA_Update(txt_ReferenceNo.Text, txt_SieveSize.Text, Convert.ToDecimal(txt_Wt.Text), Convert.ToDecimal(txt_WtRet.Text), Convert.ToDecimal(txt_CumWtRet.Text), Convert.ToDecimal(txt_CumuPassing.Text), txt_Ispassinglmt.Text, MaterialId, false);
                    }
                }
                //Specific Gravity
                if (Tab_SpecGravity.Enabled == true)
                {
                    if (lbl_Specres.Text != "Awaited")
                    {
                        dc.AggregateTestSG_Update(txt_ReferenceNo.Text, 0, 0, 0, 0, 0, MaterialId, 0, "","","",true);
                        for (int i = 0; i < grd_SpecificGravity.Rows.Count; i++)
                        {
                            TextBox txt_SSD = (TextBox)grd_SpecificGravity.Rows[i].Cells[1].FindControl("txt_SSD");
                            TextBox txt_WtOfBottleSample = (TextBox)grd_SpecificGravity.Rows[i].Cells[2].FindControl("txt_WtOfBottleSample");
                            TextBox txt_WtOfBottleDistilled = (TextBox)grd_SpecificGravity.Rows[i].Cells[3].FindControl("txt_WtOfBottleDistilled");
                            TextBox txt_OvenDryWeight = (TextBox)grd_SpecificGravity.Rows[i].Cells[4].FindControl("txt_OvenDryWeight");
                            TextBox txt_SpecificGravity = (TextBox)grd_SpecificGravity.Rows[i].Cells[5].FindControl("txt_SpecificGravity");

                            if (txt_SSD.Text != "" && txt_WtOfBottleSample.Text != "" && txt_WtOfBottleDistilled.Text != "" && txt_OvenDryWeight.Text != "")
                            {
                                string ssgImage = "", wtofbottleImage = "", ovenDrWtImage = "";
                                if (i == 0)
                                {
                                    ssgImage = lblSSDImage1.Text;
                                    wtofbottleImage = lblWtBottleSampleImage1.Text;
                                    ovenDrWtImage = lblOverDryWeightImage1.Text;
                                }
                                else if (i == 1)
                                {
                                    ssgImage = lblSSDImage1.Text;
                                    wtofbottleImage = lblWtBottleSampleImage1.Text;
                                    ovenDrWtImage = lblOverDryWeightImage1.Text;
                                }
                                dc.AggregateTestSG_Update(txt_ReferenceNo.Text, Convert.ToDecimal(txt_SSD.Text), Convert.ToDecimal(txt_WtOfBottleSample.Text), Convert.ToDecimal(txt_WtOfBottleDistilled.Text), Convert.ToDecimal(txt_OvenDryWeight.Text), Convert.ToDecimal(txt_SpecificGravity.Text), MaterialId,(i+1), ssgImage, wtofbottleImage,ovenDrWtImage, false);
                            }
                        }

                    }
                }
                //Flakiness
                if (Tab_Flaki.Enabled == true)
                {
                    if (lbl_Flakires.Text != "Awaited")
                    {
                        dc.AggregateTestFK_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, 0, MaterialId, true);
                        for (int i = 0; i < grd_Flakiness.Rows.Count; i++)
                        {
                            TextBox txt_SieveSize = (TextBox)grd_Flakiness.Rows[i].Cells[1].FindControl("txt_SieveSize");
                            TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                            TextBox txt_picesespassing = (TextBox)grd_Flakiness.Rows[i].Cells[3].FindControl("txt_picesespassing");
                            TextBox txt_GaugePassingWt = (TextBox)grd_Flakiness.Rows[i].Cells[4].FindControl("txt_GaugePassingWt");
                            TextBox txt_GaugePassingSampleNo = (TextBox)grd_Flakiness.Rows[i].Cells[5].FindControl("txt_GaugePassingSampleNo");
                            TextBox txt_Total = (TextBox)grd_Flakiness.Rows[i].Cells[6].FindControl("txt_Total");

                            dc.AggregateTestFK_Update(txt_ReferenceNo.Text, txt_SieveSize.Text, Convert.ToDecimal(txt_RetainedmassonSieve.Text), Convert.ToDecimal(txt_picesespassing.Text), Convert.ToDecimal(txt_GaugePassingWt.Text), Convert.ToDecimal(txt_GaugePassingSampleNo.Text), Convert.ToDecimal(txt_Total.Text), MaterialId, false);
                        }
                    }
                }
                //Elnogation
                if (Tab_Elong.Enabled == true)
                {
                    if (lbl_Elongres.Text != "Awaited")
                    {
                        dc.AggregateTestEL_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, 0, MaterialId, true);
                        for (int i = 0; i < grd_Elongation.Rows.Count; i++)
                        {
                            TextBox txt_SieveSize = (TextBox)grd_Elongation.Rows[i].Cells[1].FindControl("txt_SieveSize");
                            TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                            TextBox txt_picesesretained = (TextBox)grd_Elongation.Rows[i].Cells[3].FindControl("txt_picesesretained");
                            TextBox txt_GaugeretainedWt = (TextBox)grd_Elongation.Rows[i].Cells[4].FindControl("txt_GaugeretainedWt");
                            TextBox txt_GaugeretainedSampleNo = (TextBox)grd_Elongation.Rows[i].Cells[5].FindControl("txt_GaugeretainedSampleNo");
                            TextBox txt_Total = (TextBox)grd_Elongation.Rows[i].Cells[6].FindControl("txt_Total");

                            dc.AggregateTestEL_Update(txt_ReferenceNo.Text, txt_SieveSize.Text, Convert.ToDecimal(txt_RetainedmassonSieve.Text), Convert.ToDecimal(txt_picesesretained.Text), Convert.ToDecimal(txt_GaugeretainedWt.Text), Convert.ToDecimal(txt_GaugeretainedSampleNo.Text), Convert.ToDecimal(txt_Total.Text), MaterialId, false);
                        }
                    }
                }
                //Impact Value
                if (Tab_Impval.Enabled == true)
                {
                    if (lbl_Impactres.Text != "Awaited")
                    {
                        dc.AggregateTestIM_Update(txt_ReferenceNo.Text, 0, 0, 0, 0, MaterialId, true);
                        for (int i = 0; i < grd_ImpactValue.Rows.Count; i++)
                        {
                            TextBox txt_Initialwtsample = (TextBox)grd_ImpactValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                            TextBox txt_WtSampleretained = (TextBox)grd_ImpactValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                            TextBox txt_WtSamplepassing = (TextBox)grd_ImpactValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                            TextBox txt_Passing = (TextBox)grd_ImpactValue.Rows[i].Cells[6].FindControl("txt_Passing");

                            dc.AggregateTestIM_Update(txt_ReferenceNo.Text, Convert.ToDecimal(txt_Initialwtsample.Text), Convert.ToDecimal(txt_WtSampleretained.Text), Convert.ToDecimal(txt_WtSamplepassing.Text), Convert.ToDecimal(txt_Passing.Text), MaterialId, false);
                        }

                    }
                }
                //Crushing Value
                if (Tab_Crushval.Enabled == true)
                {
                    if (lbl_Crushingres.Text != "Awaited")
                    {
                        dc.AggregateTestCV_Update(txt_ReferenceNo.Text, 0, 0, 0, 0, MaterialId, true);
                        for (int i = 0; i < grd_CrushValue.Rows.Count; i++)
                        {
                            TextBox txt_Initialwtsample = (TextBox)grd_CrushValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                            TextBox txt_WtSampleretained = (TextBox)grd_CrushValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                            TextBox txt_WtSamplepassing = (TextBox)grd_CrushValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                            TextBox txt_Passing = (TextBox)grd_CrushValue.Rows[i].Cells[6].FindControl("txt_Passing");

                            dc.AggregateTestCV_Update(txt_ReferenceNo.Text, Convert.ToDecimal(txt_Initialwtsample.Text), Convert.ToDecimal(txt_WtSampleretained.Text), Convert.ToDecimal(txt_WtSamplepassing.Text), Convert.ToDecimal(txt_Passing.Text), MaterialId, false);
                        }
                    }
                }
                //OtherTests
                if (Tab_OtherTest.Enabled == true)
                {
                    //if (Pnl_LBD.Visible == true || lbl_LBDStatus.Text == "1")
                    //{
                    if (txt_LBDresult.Text != "Awaited" && txt_LBDresult.Text != "")
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, Convert.ToDecimal(txt_TotalwtofAggregate.Text), Convert.ToDecimal(txt_VolumeofCylender.Text), 0, 0, 0, 0, 0, 0, MaterialId, "LBD");
                    }
                    else
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, null, null, 0, 0, 0, 0, 0, 0, MaterialId, "LBD");
                    }
                    // }
                    //if (Pnl_WaterAbsorbtion.Visible == true || lbl_WtStatus.Text == "1")
                    //{
                    if (txt_WaterAbsorption.Text != "Awaited" && txt_WaterAbsorption.Text != "")
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, 0, 0, Convert.ToDecimal(txt_SSDforWT.Text), Convert.ToDecimal(txt_OvenDrywtforWT.Text), 0, 0, 0, 0, MaterialId, "Water Absorption");
                    }
                    else
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, 0, 0, null, null, 0, 0, 0, 0, MaterialId, "Water Absorption");
                    }
                    // }
                    //if (Pnl_MoistureContent.Visible == true || lbl_MoistStatus.Text == "1")
                    //{
                    if (txt_MoistureContent.Text != "Awaited" && txt_MoistureContent.Text != "")
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, 0, 0, 0, 0, Convert.ToDecimal(txt_InitialWt.Text), Convert.ToDecimal(txt_DryWt.Text), 0, 0, MaterialId, "Moisture Content");
                    }
                    else
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, 0, 0, 0, 0, null, null, 0, 0, MaterialId, "Moisture Content");
                    }
                    // }
                    //if (Pnl_SildContent.Visible == true || lbl_SildStatus.Text == "1")
                    //{
                    if (txt_SildContent.Text != "Awaited" && txt_SildContent.Text != "")
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, 0, 0, 0, 0, 0, 0, Convert.ToDecimal(txt_InitialWtofSild.Text), Convert.ToDecimal(txt_RetainedWtmicronSieve.Text), MaterialId, "Sild Content");
                    }
                    else
                    {
                        dc.AggregateTestOther_Update(txt_ReferenceNo.Text, 0, 0, 0, 0, 0, 0, null, null, MaterialId, "Sild Content");
                    }
                    //}
                }

                if (lbl_OtherPending.Text != "Material Name")
                {
                    dc.AgggregateTestDetail_Update(txt_RecType.Text, txt_ReferenceNo.Text, lbl_FMres.Text, lbl_Specres.Text, lbl_Flakires.Text, lbl_Elongres.Text, lbl_Impactres.Text, lbl_Crushingres.Text, txt_LBDresult.Text, txt_WaterAbsorption.Text, txt_MoistureContent.Text, txt_SildContent.Text, txt_SampleCondition.Text, 0);
                }
                else
                {
                    dc.AgggregateTestDetail_Update(txt_RecType.Text, txt_ReferenceNo.Text, lbl_FMres.Text, lbl_Specres.Text, lbl_Flakires.Text, lbl_Elongres.Text, lbl_Impactres.Text, lbl_Crushingres.Text, txt_LBDresult.Text, txt_WaterAbsorption.Text, txt_MoistureContent.Text, txt_SildContent.Text, txt_SampleCondition.Text, MaterialId); //Convert.ToInt32(ddl_MaterialName.SelectedValue));
                }
                //Check Status : -
                if (lbl_OtherPending.Text == "Material Name")
                {
                    bool valid = false;
                    bool chkvalid = true;
                    foreach (ListItem lstItem in ddl_OtherPendingRpt.Items)
                    {
                        if (lstItem.Value != "---Select---")
                        {
                            var Inward_AggT = dc.MF_View(txt_ReferenceNo.Text, Convert.ToInt32(lstItem.Value), "MF");
                            foreach (var mf in Inward_AggT)
                            {
                                if (mf.Material_List.Trim() == "Natural Sand")
                                {
                                    if (mf.AGGTINWD_WaterAborp_var != null && mf.AGGTINWD_WaterAborp_var.ToString() != "" && mf.AGGTINWD_WaterAborp_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_MoistureContent_var != null && mf.AGGTINWD_MoistureContent_var.ToString() != "" && mf.AGGTINWD_MoistureContent_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_SpecificGravity_var != null && mf.AGGTINWD_SpecificGravity_var.ToString() != "" && mf.AGGTINWD_SpecificGravity_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                }
                                if (mf.Material_List.Trim() == "Crushed Sand")
                                {
                                    if (mf.AGGTINWD_WaterAborp_var != null && mf.AGGTINWD_WaterAborp_var.ToString() != "" && mf.AGGTINWD_WaterAborp_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_MoistureContent_var != null && mf.AGGTINWD_MoistureContent_var.ToString() != "" && mf.AGGTINWD_MoistureContent_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_SpecificGravity_var != null && mf.AGGTINWD_SpecificGravity_var.ToString() != "" && mf.AGGTINWD_SpecificGravity_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                }
                                if (mf.Material_List.Trim() == "Stone Dust")
                                {
                                    if (mf.AGGTINWD_WaterAborp_var != null && mf.AGGTINWD_WaterAborp_var.ToString() != "" && mf.AGGTINWD_WaterAborp_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_MoistureContent_var != null && mf.AGGTINWD_MoistureContent_var.ToString() != "" && mf.AGGTINWD_MoistureContent_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_SpecificGravity_var != null && mf.AGGTINWD_SpecificGravity_var.ToString() != "" && mf.AGGTINWD_SpecificGravity_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                }
                                if (mf.Material_List.Trim() == "Grit")
                                {
                                    if (mf.AGGTINWD_WaterAborp_var != null && mf.AGGTINWD_WaterAborp_var.ToString() != "" && mf.AGGTINWD_WaterAborp_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_MoistureContent_var != null && mf.AGGTINWD_MoistureContent_var.ToString() != "" && mf.AGGTINWD_MoistureContent_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                    if (mf.AGGTINWD_SpecificGravity_var != null && mf.AGGTINWD_SpecificGravity_var.ToString() != "" && mf.AGGTINWD_SpecificGravity_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                }
                                if (mf.Material_List.Trim() == "10 mm")
                                {
                                    if (mf.AGGTINWD_WaterAborp_var != null && mf.AGGTINWD_WaterAborp_var.ToString() != "" && mf.AGGTINWD_WaterAborp_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }

                                }
                                if (mf.Material_List.Trim() == "20 mm")
                                {
                                    if (mf.AGGTINWD_WaterAborp_var != null && mf.AGGTINWD_WaterAborp_var.ToString() != "" && mf.AGGTINWD_WaterAborp_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                }
                                if (mf.Material_List.Trim() == "40 mm")
                                {
                                    if (mf.AGGTINWD_WaterAborp_var != null && mf.AGGTINWD_WaterAborp_var.ToString() != "" && mf.AGGTINWD_WaterAborp_var.ToString() != "Awaited")
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        valid = false;
                                    }
                                }
                                break;

                            }
                            if (valid == false)
                            {
                                chkvalid = false;
                            }
                        }
                    }
                    if (chkvalid == true)
                    {
                        dc.MFStatus_Update(txt_ReferenceNo.Text, true, 0, false, false, false, false, false, 0, 0);
                    }
                }
                
                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "AGGT", true);
                for (int i = 0; i < grdAggtRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdAggtRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "AGGT");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.AGGT_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "AGGT");
                            foreach (var c in chkId)
                            {
                                if (c.AGGTDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "AGGT", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "AGGT");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "AGGT");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.AGGT_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "AGGT", false);
                            }
                        }
                    }
                }
                #endregion
                lnkPrint.Visible = true;
                lnkPrint.Enabled = true;

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;

                lnkSave.Enabled = false;
                if (txt_RecType.Text == "MF")
                {
                    LoadReferenceNoListMF();
                }
                
            }
        }

        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
            DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);

            if (txt_DateOfTesting.Text == "")
            {
                lblMsg.Text = "Please Enter Date of Testing";
                txt_DateOfTesting.Focus();
                valid = false;
            }
            else if (TestingDate > currentDate1)
            {
                lblMsg.Text = "Testing Date should be always less than or equal to the Current Date.";
                valid = false;
            }
            else if (ddl_OtherPendingRpt.SelectedItem.Text == "---Select---" && lbl_OtherPending.Text == "Material Name")
            {
                lblMsg.Text = "Please Select Material Name";
                ddl_OtherPendingRpt.Focus();
                valid = false;
            }
            else if (txt_Description.Text == "")
            {
                lblMsg.Text = "Please Enter Description";
                txt_Description.Focus();
                valid = false;
            }
            else if (txt_SupplierName.Text == "")
            {
                lblMsg.Text = "Please Enter Supplier Name";
                txt_SupplierName.Focus();
                valid = false;
            }
            else if (txt_SampleCondition.Text == "")
            {
                lblMsg.Text = "Please Enter Sample Condition";
                txt_SampleCondition.Focus();
                valid = false;
            }
            else if (ddl_NablScope.SelectedItem.Text == "--Select--")
            {
                lblMsg.Text = "Select 'NABL Scope'.";
                valid = false;
                ddl_NablScope.Focus();
            }
            else if (ddl_NABLLocation.SelectedItem.Text == "--Select--")
            {
                lblMsg.Text = "Select 'NABL Location'.";
                valid = false;
                ddl_NABLLocation.Focus();
            }
            else if (chk_WitnessBy.Checked && txt_witnessBy.Text == "" && txt_witnessBy.Enabled == true)
            {
                lblMsg.Text = "Please Enter Witness By";
                txt_witnessBy.Focus();
                valid = false;
            }
            else if (lblEntry.Text == "Check" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Approve By";
                valid = false;
            }
            else if (lblEntry.Text == "Enter" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Tested By";
                valid = false;
            }
            else if (ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Tested By";
                valid = false;
            }
            else if (valid == true)
            {
                bool validValue = false;
                if (Tab_SieveAnalysis.Enabled == true && valid == true)
                {
                    for (int i = 0; i < grdAggtEntryRptInward.Rows.Count; i++)
                    {
                        TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                        TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[2].FindControl("txt_Wt");

                        if (txt_Wt.Text == "" && txt_SieveSize.Text != "Total")
                        {
                            lblMsg.Text = "Enter Weight for Sr No. " + (i + 1) + ".";
                            Tab_Sieve();
                            txt_Wt.Focus();
                            valid = false;
                            break;
                            //txt_Wt.Text = "0";
                        }
                    }
                }
                if (Tab_SpecGravity.Enabled == true && valid == true)
                {
                    for (int i = 0; i < grd_SpecificGravity.Rows.Count; i++)
                    {
                        TextBox txt_SSD = (TextBox)grd_SpecificGravity.Rows[i].Cells[1].FindControl("txt_SSD");
                        TextBox txt_WtOfBottleSample = (TextBox)grd_SpecificGravity.Rows[i].Cells[2].FindControl("txt_WtOfBottleSample");
                        TextBox txt_WtOfBottleDistilled = (TextBox)grd_SpecificGravity.Rows[i].Cells[3].FindControl("txt_WtOfBottleDistilled");
                        TextBox txt_OvenDryWeight = (TextBox)grd_SpecificGravity.Rows[i].Cells[4].FindControl("txt_OvenDryWeight");

                        if (txt_SSD.Text != "" || txt_WtOfBottleSample.Text != "" || txt_WtOfBottleDistilled.Text != "" || txt_OvenDryWeight.Text != "")
                        {
                            validValue = true;
                            break;
                        }
                    }
                    if (validValue == true)
                    {
                        validValue = false;
                        for (int i = 0; i < grd_SpecificGravity.Rows.Count; i++)
                        {
                            TextBox txt_SSD = (TextBox)grd_SpecificGravity.Rows[i].Cells[1].FindControl("txt_SSD");
                            TextBox txt_WtOfBottleSample = (TextBox)grd_SpecificGravity.Rows[i].Cells[2].FindControl("txt_WtOfBottleSample");
                            TextBox txt_WtOfBottleDistilled = (TextBox)grd_SpecificGravity.Rows[i].Cells[3].FindControl("txt_WtOfBottleDistilled");
                            TextBox txt_OvenDryWeight = (TextBox)grd_SpecificGravity.Rows[i].Cells[4].FindControl("txt_OvenDryWeight");
                            TextBox txt_SpecificGravity = (TextBox)grd_SpecificGravity.Rows[i].Cells[5].FindControl("txt_SpecificGravity");

                            if (i == 1)
                            {
                                if (txt_SSD.Text == "" && txt_WtOfBottleSample.Text == "" && txt_WtOfBottleDistilled.Text == "" && txt_OvenDryWeight.Text == "")
                                {
                                    break;
                                }
                            }
                            if (txt_SSD.Text == "")
                            {
                                lblMsg.Text = "Enter S.S.D for Sr No. " + (i + 1) + ".";
                                Tab_Spec();
                                txt_SSD.Focus();
                                valid = false;
                                break;
                                // txt_SSD.Text = "0";
                            }
                            if (txt_WtOfBottleSample.Text == "")
                            {
                                lblMsg.Text = "Enter Weight of the Bottle,Sample & Distilled Water for Sr No. " + (i + 1) + ".";
                                Tab_Spec();
                                txt_WtOfBottleSample.Focus();
                                valid = false;
                                break;
                                // txt_WtOfBottleSample.Text = "0";
                            }
                            if (txt_WtOfBottleDistilled.Text == "")
                            {
                                lblMsg.Text = "Enter Weight of Bottle & Distilled Water for Sr No. " + (i + 1) + ".";
                                MainView.ActiveViewIndex = 1;
                                Tab_Spec();
                                txt_WtOfBottleDistilled.Focus();
                                valid = false;
                                break;
                                // txt_WtOfBottleDistilled.Text = "0";
                            }
                            if (txt_OvenDryWeight.Text == "")
                            {
                                lblMsg.Text = "Enter Oven Dry Weight for Sr No. " + (i + 1) + ".";
                                Tab_Spec();
                                txt_OvenDryWeight.Focus();
                                valid = false;
                                break;
                                // txt_OvenDryWeight.Text = "0";
                            }
                        }
                    }
                }
                if (Tab_Flaki.Enabled == true && valid == true)
                {
                    validValue = false;
                    for (int i = 0; i < grd_Flakiness.Rows.Count; i++)
                    {
                        TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                        TextBox txt_picesespassing = (TextBox)grd_Flakiness.Rows[i].Cells[3].FindControl("txt_picesespassing");
                        if (txt_RetainedmassonSieve.Text != "" || txt_picesespassing.Text != "")
                        {
                            validValue = true;
                            break;
                        }
                    }
                    if (validValue == true)
                    {
                        validValue = false;
                        for (int i = 0; i < grd_Flakiness.Rows.Count; i++)
                        {
                            TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                            TextBox txt_picesespassing = (TextBox)grd_Flakiness.Rows[i].Cells[3].FindControl("txt_picesespassing");

                            if (txt_RetainedmassonSieve.Text == "")
                            {
                                lblMsg.Text = "Enter Retained Mass on Sieve for Sr No. " + (i + 1) + ".";
                                Tab_Flakee();
                                txt_RetainedmassonSieve.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_picesespassing.Text == "")
                            {
                                lblMsg.Text = "Enter piceses passing for Sr No. " + (i + 1) + ".";
                                MainView.ActiveViewIndex = 2;
                                Tab_Flakee();
                                txt_picesespassing.Focus();
                                valid = false;
                                break;
                            }
                        }
                    }
                }
                if (Tab_Elong.Enabled == true && valid == true)
                {
                    validValue = false;

                    for (int i = 0; i < grd_Elongation.Rows.Count; i++)
                    {
                        TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                        TextBox txt_picesesretained = (TextBox)grd_Elongation.Rows[i].Cells[3].FindControl("txt_picesesretained");
                        if (txt_RetainedmassonSieve.Text != "" || txt_picesesretained.Text != "")
                        {
                            validValue = true;
                            break;
                        }
                    }
                    if (validValue == true)
                    {
                        validValue = false;
                        for (int i = 0; i < grd_Elongation.Rows.Count; i++)
                        {
                            TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                            TextBox txt_picesesretained = (TextBox)grd_Elongation.Rows[i].Cells[3].FindControl("txt_picesesretained");

                            if (txt_RetainedmassonSieve.Text == "")
                            {
                                lblMsg.Text = "Enter Retained Mass on Sieve for Sr No. " + (i + 1) + ".";
                                Tab_Elongness();
                                txt_RetainedmassonSieve.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_picesesretained.Text == "")
                            {
                                lblMsg.Text = "Mass No. Of piceses retained for Sr No. " + (i + 1) + ".";
                                Tab_Elongness();
                                txt_picesesretained.Focus();
                                valid = false;
                                break;
                            }
                        }
                    }
                }
                if (Tab_Impval.Enabled == true && valid == true)
                {
                    validValue = false;
                    for (int i = 0; i < grd_ImpactValue.Rows.Count; i++)
                    {
                        TextBox txt_Initialwtsample = (TextBox)grd_ImpactValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                        TextBox txt_WtSampleretained = (TextBox)grd_ImpactValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");

                        if (txt_Initialwtsample.Text != "" || txt_WtSampleretained.Text != "")
                        {
                            validValue = true;
                            break;
                        }
                    }
                    if (validValue == true)
                    {
                        validValue = false;
                        for (int i = 0; i < grd_ImpactValue.Rows.Count; i++)
                        {
                            TextBox txt_Initialwtsample = (TextBox)grd_ImpactValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                            TextBox txt_WtSampleretained = (TextBox)grd_ImpactValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");

                            if (txt_Initialwtsample.Text == "")
                            {
                                lblMsg.Text = "Enter Initial Wt. of Sample for Sr No. " + (i + 1) + ".";
                                Tab_Impac();
                                txt_Initialwtsample.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_WtSampleretained.Text == "")
                            {
                                lblMsg.Text = "Wt. of Sample retained on 2.36 mm for Sr No. " + (i + 1) + ".";
                                Tab_Impac();
                                txt_WtSampleretained.Focus();
                                valid = false;
                                break;
                            }
                            else if (Convert.ToDouble(txt_Initialwtsample.Text) <= Convert.ToDouble(txt_WtSampleretained.Text))
                            {
                                lblMsg.Text = "Initial Wt. of Sample should be greater than Wt. of Sample retained on 2.36 mm for Sr No.  " + (i + 1) + ".";
                                Tab_Impac();
                                txt_Initialwtsample.Focus();
                                valid = false;
                                break;
                            }
                        }
                    }
                }
                if (Tab_Crushval.Enabled == true && valid == true)
                {
                    validValue = false;
                    for (int i = 0; i < grd_CrushValue.Rows.Count; i++)
                    {
                        TextBox txt_Initialwtsample = (TextBox)grd_CrushValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                        TextBox txt_WtSampleretained = (TextBox)grd_CrushValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                        if (txt_Initialwtsample.Text != "" || txt_WtSampleretained.Text != "")
                        {
                            validValue = true;
                            break;
                        }
                    }
                    if (validValue == true)
                    {
                        validValue = false;
                        for (int i = 0; i < grd_CrushValue.Rows.Count; i++)
                        {
                            TextBox txt_Initialwtsample = (TextBox)grd_CrushValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                            TextBox txt_WtSampleretained = (TextBox)grd_CrushValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");

                            if (txt_Initialwtsample.Text == "")
                            {
                                lblMsg.Text = "Enter Initial Wt. of Sample for Sr No. " + (i + 1) + ".";
                                Tab_Crushing();
                                txt_Initialwtsample.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_WtSampleretained.Text == "")
                            {
                                lblMsg.Text = "Wt. of Sample retained on 2.36 mm for Sr No. " + (i + 1) + ".";
                                Tab_Crushing();
                                txt_WtSampleretained.Focus();
                                valid = false;
                                break;
                            }
                            else if (Convert.ToDouble(txt_Initialwtsample.Text) <= Convert.ToDouble(txt_WtSampleretained.Text))
                            {
                                lblMsg.Text = "Initial Wt. of Sample should be greater than Wt. of Sample retained on 2.36 mm for Sr No.  " + (i + 1) + ".";
                                Tab_Crushing();
                                txt_Initialwtsample.Focus();
                                valid = false;
                                break;
                            }
                        }
                    }
                }
                if (valid == true)
                {
                    if (Tab_OtherTest.Enabled == true)
                    {
                        validValue = true;
                        if (txt_TotalwtofAggregate.Text != "" || txt_VolumeofCylender.Text != "")
                        {
                            if (txt_TotalwtofAggregate.Text == "")
                            {
                                lblMsg.Text = "Enter Total Weight of the Aggregate";
                                Tab_Other();
                                txt_TotalwtofAggregate.Focus();
                                valid = false;
                                validValue = false;
                                // txt_TotalwtofAggregate.Text = "0";
                            }
                            else if (txt_VolumeofCylender.Text == "")
                            {
                                lblMsg.Text = "Enter Volume of the Cylender";
                                Tab_Other();
                                txt_VolumeofCylender.Focus();
                                valid = false;
                                validValue = false;
                                //txt_VolumeofCylender.Text = "0";
                            }
                        }
                        if (txt_SSDforWT.Text != "" || txt_OvenDrywtforWT.Text != "" && validValue == true)
                        {
                            if (txt_SSDforWT.Text == "")
                            {
                                lblMsg.Text = "Enter SSD";
                                Tab_Other();
                                txt_SSDforWT.Focus();
                                valid = false;
                                validValue = false;
                                // txt_SSDforWT.Text = "0";
                            }
                            else if (txt_OvenDrywtforWT.Text == "")
                            {
                                lblMsg.Text = "Enter Oven Dry Weight";
                                Tab_Other();
                                txt_OvenDrywtforWT.Focus();
                                valid = false;
                                validValue = false;
                                // txt_OvenDrywtforWT.Text = "0";
                            }
                            else if (Convert.ToDecimal(txt_SSDforWT.Text) < Convert.ToDecimal(txt_OvenDrywtforWT.Text))
                            {
                                lblMsg.Text = "SSD should be greater Oven Dry Weight";
                                Tab_Other();
                                txt_SSDforWT.Focus();
                                valid = false;
                                validValue = false;
                            }
                        }
                        if (txt_InitialWt.Text != "" || txt_DryWt.Text != "" && validValue == true)
                        {
                            if (txt_InitialWt.Text == "")
                            {
                                lblMsg.Text = "Enter Initial Weight of Moisture Content";
                                Tab_Other();
                                txt_InitialWt.Focus();
                                valid = false;
                                validValue = false;
                                // txt_InitialWt.Text = "0";
                            }
                            else if (txt_DryWt.Text == "")
                            {
                                lblMsg.Text = "Enter Dry Weight";
                                Tab_Other();
                                txt_DryWt.Focus();
                                valid = false;
                                validValue = false;
                                //txt_DryWt.Text = "0";
                            }
                            else if (Convert.ToDecimal(txt_InitialWt.Text) < Convert.ToDecimal(txt_DryWt.Text))
                            {
                                lblMsg.Text = "Initial Weight should be greater than Dry Weight";
                                Tab_Other();
                                txt_InitialWt.Focus();
                                valid = false;
                                validValue = false;
                            }
                        }
                        if (txt_InitialWtofSild.Text != "" || txt_RetainedWtmicronSieve.Text != "" && validValue == true)
                        {
                            if (txt_InitialWtofSild.Text == "")
                            {
                                lblMsg.Text = "Enter Initial Weight of Sild Content";
                                Tab_Other();
                                txt_InitialWtofSild.Focus();
                                valid = false;
                                validValue = false;
                            }
                            else if (txt_RetainedWtmicronSieve.Text == "")
                            {
                                lblMsg.Text = "Enter Retained Weight on 75 micron Sieve";
                                Tab_Other();
                                txt_RetainedWtmicronSieve.Focus();
                                valid = false;
                                validValue = false;
                            }
                            else if (Convert.ToDecimal(txt_InitialWtofSild.Text) < Convert.ToDecimal(txt_RetainedWtmicronSieve.Text))
                            {
                                lblMsg.Text = "Initial Weight should be greater than the Retained Weight on 75 micron Sieve";
                                Tab_Other();
                                txt_InitialWtofSild.Focus();
                                valid = false;
                                validValue = false;
                            }
                        }
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
                Calculation();
            }
            return valid;
        }
        protected void Lnk_Calculate_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
            }
        }
        public void Calculation()
        {
            double SpecificGravity = 0;
            double tempValue = 0;
            decimal val = 0;
            if (Tab_SieveAnalysis.Enabled == true)
            {
                double TotalAmt = 0;
                double PreviousrowWt = 0;
                double FM = 0;
                for (int i = 0; i < grdAggtEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[2].FindControl("txt_Wt");
                    if (txt_SieveSize.Text != "Total")
                    {
                        TotalAmt += Convert.ToDouble(txt_Wt.Text);
                    }
                    else
                    {
                        txt_Wt.Text = TotalAmt.ToString();
                        break;
                    }
                }

                for (int i = 0; i < grdAggtEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[2].FindControl("txt_Wt");
                    TextBox txt_WtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[3].FindControl("txt_WtRet");
                    TextBox txt_CumWtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[4].FindControl("txt_CumWtRet");
                    TextBox txt_CumuPassing = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[5].FindControl("txt_CumuPassing");
                    TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");
                    if (i > 0)
                    {
                        TextBox txt_CumWtRetpre = (TextBox)grdAggtEntryRptInward.Rows[i - 1].Cells[4].FindControl("txt_CumWtRet");
                        PreviousrowWt = Convert.ToDouble(txt_CumWtRetpre.Text);
                    }
                    else
                    {
                        PreviousrowWt = 0;
                    }
                    if (TotalAmt > 0)
                    {
                        txt_WtRet.Text = ((Convert.ToDouble(txt_Wt.Text) * 100) / TotalAmt).ToString("0.000");
                    }
                    
                    txt_CumWtRet.Text = (PreviousrowWt + Convert.ToDouble(txt_WtRet.Text)).ToString("0.00");
                    txt_CumuPassing.Text = (100 - Convert.ToDouble(txt_CumWtRet.Text)).ToString("0.00");

                    val = Convert.ToDecimal(txt_CumWtRet.Text);
                    if (val > 100)
                        val = Convert.ToDecimal(100.00);
                    else if (val < 0)
                        val = Convert.ToDecimal(0.00);
                    txt_CumWtRet.Text  = Convert.ToString(val);

                    val = Convert.ToDecimal(txt_CumuPassing.Text);
                    if (val > 100)
                        val = Convert.ToDecimal(100.00);
                    else if (val < 0)
                        val = Convert.ToDecimal(0.00);

                    txt_CumuPassing.Text = Convert.ToString(val);

                    if (txt_SieveSize.Text != "Pan" && txt_SieveSize.Text != "Total" && txt_SieveSize.Text != "75 micron")
                    {
                        FM += Convert.ToDouble(txt_CumWtRet.Text);
                    }
                    //}
                    if (txt_SieveSize.Text == "Total")
                    {
                        txt_WtRet.Text = "";
                        txt_CumWtRet.Text = "";
                        txt_CumuPassing.Text = "";
                        txt_Ispassinglmt.Text = "";
                    }
                }

                lbl_FMres.Visible = true;
                lbl_FM.Visible = true;
                lbl_FMres.Text = (FM / 100).ToString("0.000");

            }
            #region Specific gravity
            if (Tab_SpecGravity.Enabled == true)
            {
                for (int i = 0; i < grd_SpecificGravity.Rows.Count; i++)
                {
                    TextBox txt_SSD = (TextBox)grd_SpecificGravity.Rows[i].Cells[1].FindControl("txt_SSD");
                    TextBox txt_WtOfBottleSample = (TextBox)grd_SpecificGravity.Rows[i].Cells[2].FindControl("txt_WtOfBottleSample");
                    TextBox txt_WtOfBottleDistilled = (TextBox)grd_SpecificGravity.Rows[i].Cells[3].FindControl("txt_WtOfBottleDistilled");
                    TextBox txt_OvenDryWeight = (TextBox)grd_SpecificGravity.Rows[i].Cells[4].FindControl("txt_OvenDryWeight");
                    TextBox txt_SpecificGravity = (TextBox)grd_SpecificGravity.Rows[i].Cells[5].FindControl("txt_SpecificGravity");
                    if (i == 0)
                    {
                        if (Tab_OtherTest.Enabled == true && Pnl_WaterAbsorbtion.Visible == true || lbl_WtStatus.Text == "1")
                        {
                            txt_SSDforWT.Text = txt_SSD.Text;
                            txt_OvenDrywtforWT.Text = txt_OvenDryWeight.Text;
                        }
                    }
                    if (txt_SSD.Text != "" && txt_WtOfBottleSample.Text != "" && txt_WtOfBottleDistilled.Text != "" && txt_OvenDryWeight.Text != "")
                    {
                        txt_SpecificGravity.Text = (Convert.ToDouble(txt_OvenDryWeight.Text) / (Convert.ToDouble(txt_SSD.Text) - (Convert.ToDouble(txt_WtOfBottleSample.Text) - Convert.ToDouble(txt_WtOfBottleDistilled.Text)))).ToString("0.00");

                        SpecificGravity += Convert.ToDouble(txt_SpecificGravity.Text);

                        if (i == 1)
                        {
                            TextBox txt_SpecificGravity1st = (TextBox)grd_SpecificGravity.Rows[i - 1].Cells[5].FindControl("txt_SpecificGravity");

                            if (Convert.ToDouble(txt_SpecificGravity.Text) == 0)
                            {
                                lbl_Specres.Text = txt_SpecificGravity1st.Text;
                            }
                            else if (Convert.ToDouble(txt_SpecificGravity1st.Text) == 0)
                            {
                                lbl_Specres.Text = txt_SpecificGravity.Text;
                            }
                        }
                        if (Convert.ToDouble(txt_SpecificGravity.Text) > 0)
                        {
                            if (SpecificGravity > 0)
                            {
                                lbl_Specres.Text = (SpecificGravity / 2).ToString("0.00");
                            }
                        }
                        else
                        {
                            lbl_Specres.Text = "0";//
                        }
                    }
                    else
                    {
                        lbl_Specres.Text = "Awaited";//
                    }
                    if (i == 1)
                    {
                        if (txt_SSD.Text == "" && txt_WtOfBottleSample.Text == "" && txt_WtOfBottleDistilled.Text == "" && txt_OvenDryWeight.Text == "")
                        {
                            TextBox txt_SpecificGravity1st = (TextBox)grd_SpecificGravity.Rows[i - 1].Cells[5].FindControl("txt_SpecificGravity");

                            if (txt_SpecificGravity1st.Text != "")
                            {
                                lbl_Specres.Text = txt_SpecificGravity1st.Text;
                            }
                        }
                    }
                    if (txt_SSD.Text == "" && txt_WtOfBottleSample.Text == "" && txt_WtOfBottleDistilled.Text == "" && txt_OvenDryWeight.Text == "")
                    {
                        txt_SpecificGravity.Text = "";
                    }
                }
            }
            #endregion
            #region other tests
            if (Tab_OtherTest.Enabled == true)
            {
                // LBD
                if (Pnl_LBD.Visible == true || lbl_LBDStatus.Text =="1")
                {
                    if (txt_TotalwtofAggregate.Text != "" && txt_VolumeofCylender.Text != "")
                    {
                        if (Convert.ToDouble(txt_VolumeofCylender.Text) > 0)
                        {
                            txt_LBDresult.Text = (Convert.ToDouble(txt_TotalwtofAggregate.Text) / Convert.ToDouble(txt_VolumeofCylender.Text)).ToString("0.00");
                        }
                        else if (Convert.ToDouble(txt_VolumeofCylender.Text) == 0)
                        {
                            txt_LBDresult.Text = (Convert.ToDouble(txt_TotalwtofAggregate.Text)).ToString("0.00");
                        }
                    }
                    else
                    {
                        txt_LBDresult.Text = "Awaited";//
                    }
                }
                else
                {
                    txt_LBDresult.Text = "";//
                }
                //WA
                if (Pnl_WaterAbsorbtion.Visible == true || lbl_WtStatus.Text == "1")
                {
                    if (txt_SSDforWT.Text != "" && txt_OvenDrywtforWT.Text != "")
                    {
                        txt_WaterAbsorption.Text = (((Convert.ToDouble(txt_SSDforWT.Text) - Convert.ToDouble(txt_OvenDrywtforWT.Text)) / Convert.ToDouble(txt_OvenDrywtforWT.Text)) * 100).ToString("0.00");
                    }
                    else
                    {
                        txt_WaterAbsorption.Text = "Awaited";//
                    }
                }
                else
                {
                    txt_WaterAbsorption.Text = "";//
                }
                //Molisture Content
                if (Pnl_MoistureContent.Visible == true || lbl_MoistStatus.Text == "1")
                {
                    if (txt_InitialWt.Text != "" && txt_DryWt.Text != "")
                    {
                        txt_MoistureContent.Text = (((Convert.ToDouble(txt_InitialWt.Text) - Convert.ToDouble(txt_DryWt.Text)) / Convert.ToDouble(txt_DryWt.Text)) * 100).ToString("0.00");
                    }
                    else
                    {
                        txt_MoistureContent.Text = "Awaited";//
                    }
                }
                else
                {
                    txt_MoistureContent.Text = "";//
                }
                //Sild Content
                if (Pnl_SildContent.Visible == true || lbl_SildStatus.Text == "1")
                {
                    if (txt_InitialWtofSild.Text != "" && txt_RetainedWtmicronSieve.Text != "")
                    {
                        txt_SildContent.Text = (((Convert.ToDouble(txt_InitialWtofSild.Text) - (Convert.ToDouble(txt_RetainedWtmicronSieve.Text))) / Convert.ToDouble(txt_InitialWtofSild.Text) * 100)).ToString("0.00");
                    }
                    else
                    {
                        txt_SildContent.Text = "Awaited";//
                    }
                }
                else
                {
                    txt_SildContent.Text = "";//
                }
            }
#endregion
            #region Flakiness & elongation
            if (Tab_Flaki.Enabled == true)
            {
                double Totalval = 0;
                double subTotal = 0;
                for (int i = 0; i < grd_Flakiness.Rows.Count; i++)
                {
                    TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");                    
                    if (txt_RetainedmassonSieve.Text != "" )
                    {
                        Totalval += Convert.ToDouble(txt_RetainedmassonSieve.Text);
                    }
                }
                        
                lbl_Flakires.Text = "Awaited";//
                
                if (Totalval > 0)
                {
                    lbl_Flakires.Text = "";
                    for (int i = 0; i < grd_Flakiness.Rows.Count; i++)
                    {
                        TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                        TextBox txt_picesespassing = (TextBox)grd_Flakiness.Rows[i].Cells[3].FindControl("txt_picesespassing");
                        TextBox txt_GaugePassingWt = (TextBox)grd_Flakiness.Rows[i].Cells[4].FindControl("txt_GaugePassingWt");
                        TextBox txt_GaugePassingSampleNo = (TextBox)grd_Flakiness.Rows[i].Cells[5].FindControl("txt_GaugePassingSampleNo");
                        TextBox txt_Total = (TextBox)grd_Flakiness.Rows[i].Cells[6].FindControl("txt_Total");

                        if (txt_RetainedmassonSieve.Text != "" && txt_picesespassing.Text != "")
                        {
                            if (Convert.ToDouble(txt_RetainedmassonSieve.Text) == 0 && Convert.ToDouble(txt_picesespassing.Text) == 0)
                            {
                                txt_GaugePassingWt.Text = "0";
                                txt_GaugePassingSampleNo.Text = "0";
                                txt_Total.Text = "0";
                            }
                            else
                            {
                                txt_GaugePassingWt.Text = (Convert.ToDouble(txt_picesespassing.Text) / Convert.ToDouble(txt_RetainedmassonSieve.Text)).ToString("0.0000");
                                txt_GaugePassingSampleNo.Text = (Convert.ToDouble(txt_RetainedmassonSieve.Text) / Totalval).ToString("0.00000");
                                txt_Total.Text = (Convert.ToDouble(txt_GaugePassingWt.Text) * Convert.ToDouble(txt_GaugePassingSampleNo.Text)).ToString();

                                subTotal += Convert.ToDouble(txt_GaugePassingWt.Text) * Convert.ToDouble(txt_GaugePassingSampleNo.Text);
                                txt_GaugePassingWt.Text = Convert.ToDouble(txt_GaugePassingWt.Text).ToString("0.00");
                                txt_GaugePassingSampleNo.Text = Convert.ToDouble(txt_GaugePassingSampleNo.Text).ToString("0.00");
                                txt_Total.Text = Convert.ToDouble(txt_Total.Text).ToString("0.000");
                            }                                                        
                        }
                        else
                        {
                            lbl_Flakires.Text = "Awaited";//
                        }
                    }
                    subTotal = subTotal * 100;
                    lbl_Flakires.Text = subTotal.ToString("0.0");
                }
            }
            if (Tab_Elong.Enabled == true)
            {
                lbl_Elongres.Text = "Awaited";//
                double Totalval = 0;
                double subTotal = 0;
                for (int i = 0; i < grd_Elongation.Rows.Count; i++)
                {
                    TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");                   
                    if (txt_RetainedmassonSieve.Text != "")
                    {
                        Totalval += Convert.ToDouble(txt_RetainedmassonSieve.Text);
                    }
                }
                if (Totalval > 0)
                {
                    
                    lbl_Elongres.Text = "";
                    for (int i = 0; i < grd_Elongation.Rows.Count; i++)
                    {
                        TextBox txt_SieveSize = (TextBox)grd_Elongation.Rows[i].Cells[1].FindControl("txt_SieveSize");
                        TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                        TextBox txt_picesesretained = (TextBox)grd_Elongation.Rows[i].Cells[3].FindControl("txt_picesesretained");
                        TextBox txt_GaugeretainedWt = (TextBox)grd_Elongation.Rows[i].Cells[4].FindControl("txt_GaugeretainedWt");
                        TextBox txt_GaugeretainedSampleNo = (TextBox)grd_Elongation.Rows[i].Cells[5].FindControl("txt_GaugeretainedSampleNo");
                        TextBox txt_Total = (TextBox)grd_Elongation.Rows[i].Cells[6].FindControl("txt_Total");

                        if (txt_RetainedmassonSieve.Text != "" && txt_picesesretained.Text != "")
                        {
                            if (Convert.ToDouble(txt_RetainedmassonSieve.Text) == 0 && Convert.ToDouble(txt_picesesretained.Text) == 0)
                            {
                                txt_GaugeretainedWt.Text = "0";
                                txt_GaugeretainedSampleNo.Text = "0";
                                txt_Total.Text = "0";
                            }
                            else
                            {
                                txt_GaugeretainedWt.Text = (Convert.ToDouble(txt_picesesretained.Text) / Convert.ToDouble(txt_RetainedmassonSieve.Text)).ToString("0.0000");
                                txt_GaugeretainedSampleNo.Text = (Convert.ToDouble(txt_RetainedmassonSieve.Text) / Totalval).ToString("0.00000");
                                txt_Total.Text = (Convert.ToDouble(txt_GaugeretainedWt.Text) * Convert.ToDouble(txt_GaugeretainedSampleNo.Text)).ToString();

                                subTotal += Convert.ToDouble(txt_GaugeretainedWt.Text) * Convert.ToDouble(txt_GaugeretainedSampleNo.Text);
                                txt_GaugeretainedWt.Text = Convert.ToDouble(txt_GaugeretainedWt.Text).ToString("0.00");
                                txt_GaugeretainedSampleNo.Text = Convert.ToDouble(txt_GaugeretainedSampleNo.Text).ToString("0.00");
                                txt_Total.Text = Convert.ToDouble(txt_Total.Text).ToString("0.000");
                            }
                        }
                        else
                        {
                            lbl_Elongres.Text = "Awaited";//
                        }
                    }
                    subTotal = subTotal * 100;
                    lbl_Elongres.Text = subTotal.ToString("0.0");
                }
            }
#endregion
            #region Impact n Crushing value
            if (Tab_Impval.Enabled == true)
            {
                
                tempValue = 0;
                for (int i = 0; i < grd_ImpactValue.Rows.Count; i++)
                {
                    TextBox txt_Initialwtsample = (TextBox)grd_ImpactValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                    TextBox txt_WtSampleretained = (TextBox)grd_ImpactValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                    TextBox txt_WtSamplepassing = (TextBox)grd_ImpactValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                    TextBox txt_Passing = (TextBox)grd_ImpactValue.Rows[i].Cells[6].FindControl("txt_Passing");

                    if (txt_Initialwtsample.Text != "" && txt_WtSampleretained.Text != "")
                    {
                        if (Convert.ToDouble(txt_Initialwtsample.Text) > Convert.ToDouble(txt_WtSampleretained.Text))
                        {
                            txt_WtSamplepassing.Text = (Convert.ToDouble(txt_Initialwtsample.Text) - Convert.ToDouble(txt_WtSampleretained.Text)).ToString();
                        }

                        if (Convert.ToDouble(txt_Initialwtsample.Text) == 0)
                        {
                            txt_Passing.Text = "0";
                        }
                        else if (Convert.ToDouble(txt_Initialwtsample.Text) > 0)
                        {
                            if (Convert.ToDouble(txt_Initialwtsample.Text) > 0)
                            {
                                txt_Passing.Text = (100 * (Convert.ToDouble(txt_WtSamplepassing.Text) / Convert.ToDouble(txt_Initialwtsample.Text))).ToString("00.00");
                            }
                        }
                        tempValue += Convert.ToDouble(txt_Passing.Text);
                    }
                    else
                    {
                        lbl_Impactres.Text = "Awaited";//
                    }
                }
                if (grd_ImpactValue.Rows.Count > 0)
                {
                    lbl_Impactres.Text = (tempValue / Convert.ToInt32(grd_ImpactValue.Rows.Count)).ToString();
                    lbl_Impactres.Text = Convert.ToDouble(lbl_Impactres.Text).ToString("00");
                }
                if (lbl_Impactres.Text == "00") lbl_Impactres.Text = "Awaited";
            }
            
            if (Tab_Crushval.Enabled == true)
            {
                tempValue = 0;
                for (int i = 0; i < grd_CrushValue.Rows.Count; i++)
                {
                    TextBox txt_Initialwtsample = (TextBox)grd_CrushValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                    TextBox txt_WtSampleretained = (TextBox)grd_CrushValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                    TextBox txt_WtSamplepassing = (TextBox)grd_CrushValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                    TextBox txt_Passing = (TextBox)grd_CrushValue.Rows[i].Cells[6].FindControl("txt_Passing");

                    if (txt_Initialwtsample.Text != "" && txt_WtSampleretained.Text != "")
                    {
                        if (Convert.ToDouble(txt_Initialwtsample.Text) > Convert.ToDouble(txt_WtSampleretained.Text))
                        {
                            txt_WtSamplepassing.Text = (Convert.ToDouble(txt_Initialwtsample.Text) - Convert.ToDouble(txt_WtSampleretained.Text)).ToString();

                        }
                        if (Convert.ToDouble(txt_Initialwtsample.Text) == 0)
                        {
                            txt_Passing.Text = "0";
                        }
                        else if (Convert.ToDouble(txt_Initialwtsample.Text) > 0)
                        {                            
                            txt_Passing.Text = (100 * (Convert.ToDouble(txt_WtSamplepassing.Text) / Convert.ToDouble(txt_Initialwtsample.Text))).ToString("00.00");                            
                        }
                        tempValue += Convert.ToDouble(txt_Passing.Text);
                    }
                    else
                    {
                        lbl_Crushingres.Text = "Awaited";//
                    }
                }
                if (grd_CrushValue.Rows.Count > 0)
                {
                    lbl_Crushingres.Text = (tempValue / Convert.ToInt32(grd_CrushValue.Rows.Count)).ToString();
                    lbl_Crushingres.Text = Convert.ToDouble(lbl_Crushingres.Text).ToString("00");
                }
                if (lbl_Crushingres.Text == "00") lbl_Crushingres.Text = "Awaited";
            }
#endregion
        }
        public void Tab_Remarks()
        {
            MainView.ActiveViewIndex = 7;
            Tab_Remark.CssClass = "Click";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
        }
        public void Tab_Sieve()
        {
            MainView.ActiveViewIndex = 0;
            Tab_SieveAnalysis.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
        }
        public void Tab_Spec()
        {
            MainView.ActiveViewIndex = 1;
            Tab_SpecGravity.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
        }
        public void Tab_Flakee()
        {
            MainView.ActiveViewIndex = 2;
            Tab_Flaki.CssClass = "Click";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
        }
        public void Tab_Elongness()
        {
            MainView.ActiveViewIndex = 3;
            Tab_Elong.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
        }
        public void Tab_Impac()
        {
            MainView.ActiveViewIndex = 4;
            Tab_Impval.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
        }
        public void Tab_Crushing()
        {
            MainView.ActiveViewIndex = 5;
            Tab_Crushval.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_OtherTest.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
        }
        public void Tab_Other()
        {
            MainView.ActiveViewIndex = 6;
            Tab_OtherTest.CssClass = "Click";
            Tab_Flaki.CssClass = "Initiative";
            Tab_SieveAnalysis.CssClass = "Initiative";
            Tab_SpecGravity.CssClass = "Initiative";
            Tab_Elong.CssClass = "Initiative";
            Tab_Impval.CssClass = "Initiative";
            Tab_Crushval.CssClass = "Initiative";
            Tab_Remark.CssClass = "Initiative";
        }
        protected void AddRowEnterReportAGGTInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AGGTTestTable"] != null)
            {
                GetCurrentDataAGGTInward();
                dt = (DataTable)ViewState["AGGTTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_SieveSize", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Wt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtRet", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CumWtRet", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CumuPassing", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Ispassinglmt", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_SieveSize"] = string.Empty;
            dr["txt_Wt"] = string.Empty;
            dr["txt_WtRet"] = string.Empty;
            dr["txt_CumWtRet"] = string.Empty;
            dr["txt_CumuPassing"] = string.Empty;
            dr["txt_Ispassinglmt"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["AGGTTestTable"] = dt;
            grdAggtEntryRptInward.DataSource = dt;
            grdAggtEntryRptInward.DataBind();
            SetPreviousDataAGGTInward();

        }
        protected void GetCurrentDataAGGTInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_SieveSize", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Wt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtRet", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CumWtRet", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CumuPassing", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Ispassinglmt", typeof(string)));

            for (int i = 0; i < grdAggtEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[2].FindControl("txt_Wt");
                TextBox txt_WtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[3].FindControl("txt_WtRet");
                TextBox txt_CumWtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[4].FindControl("txt_CumWtRet");
                TextBox txt_CumuPassing = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[5].FindControl("txt_CumuPassing");
                TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");

                drRow = dtTable.NewRow();
                drRow["txt_SieveSize"] = txt_SieveSize.Text;
                drRow["txt_Wt"] = txt_Wt.Text;
                drRow["txt_WtRet"] = txt_WtRet.Text;
                drRow["txt_CumWtRet"] = txt_CumWtRet.Text;
                drRow["txt_CumuPassing"] = txt_CumuPassing.Text;
                drRow["txt_Ispassinglmt"] = txt_Ispassinglmt.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["AGGTTestTable"] = dtTable;
        }
        protected void SetPreviousDataAGGTInward()
        {
            DataTable dt = (DataTable)ViewState["AGGTTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[1].FindControl("txt_SieveSize");
                TextBox txt_Wt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[2].FindControl("txt_Wt");
                TextBox txt_WtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[3].FindControl("txt_WtRet");
                TextBox txt_CumWtRet = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[4].FindControl("txt_CumWtRet");
                TextBox txt_CumuPassing = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[5].FindControl("txt_CumuPassing");
                TextBox txt_Ispassinglmt = (TextBox)grdAggtEntryRptInward.Rows[i].Cells[6].FindControl("txt_Ispassinglmt");

                txt_SieveSize.Text = dt.Rows[i]["txt_SieveSize"].ToString();
                txt_Wt.Text = dt.Rows[i]["txt_Wt"].ToString();
                txt_WtRet.Text = dt.Rows[i]["txt_WtRet"].ToString();
                txt_CumWtRet.Text = dt.Rows[i]["txt_CumWtRet"].ToString();
                txt_CumuPassing.Text = dt.Rows[i]["txt_CumuPassing"].ToString();
                txt_Ispassinglmt.Text = dt.Rows[i]["txt_Ispassinglmt"].ToString();
            }

        }

        protected void AddRowforFlakiness()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["FlakinessTestTable"] != null)
            {
                GetCurrentDataforFlakiness();
                dt = (DataTable)ViewState["FlakinessTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_SieveSize", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_RetainedmassonSieve", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_picesespassing", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_GaugePassingWt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_GaugePassingSampleNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Total", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_SieveSize"] = string.Empty;
            dr["txt_RetainedmassonSieve"] = string.Empty;
            dr["txt_picesespassing"] = string.Empty;
            dr["txt_GaugePassingWt"] = string.Empty;
            dr["txt_GaugePassingSampleNo"] = string.Empty;
            dr["txt_Total"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["FlakinessTestTable"] = dt;
            grd_Flakiness.DataSource = dt;
            grd_Flakiness.DataBind();
            SetPreviousDataforFlakiness();

        }
        protected void GetCurrentDataforFlakiness()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_SieveSize", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_RetainedmassonSieve", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_picesespassing", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_GaugePassingWt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_GaugePassingSampleNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Total", typeof(string)));

            for (int i = 0; i < grd_Flakiness.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grd_Flakiness.Rows[i].Cells[1].FindControl("txt_SieveSize");
                TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                TextBox txt_picesespassing = (TextBox)grd_Flakiness.Rows[i].Cells[3].FindControl("txt_picesespassing");
                TextBox txt_GaugePassingWt = (TextBox)grd_Flakiness.Rows[i].Cells[4].FindControl("txt_GaugePassingWt");
                TextBox txt_GaugePassingSampleNo = (TextBox)grd_Flakiness.Rows[i].Cells[5].FindControl("txt_GaugePassingSampleNo");
                TextBox txt_Total = (TextBox)grd_Flakiness.Rows[i].Cells[6].FindControl("txt_Total");

                drRow = dtTable.NewRow();
                drRow["txt_SieveSize"] = txt_SieveSize.Text;
                drRow["txt_RetainedmassonSieve"] = txt_RetainedmassonSieve.Text;
                drRow["txt_picesespassing"] = txt_picesespassing.Text;
                drRow["txt_GaugePassingWt"] = txt_GaugePassingWt.Text;
                drRow["txt_GaugePassingSampleNo"] = txt_GaugePassingSampleNo.Text;
                drRow["txt_Total"] = txt_Total.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["FlakinessTestTable"] = dtTable;
        }
        protected void SetPreviousDataforFlakiness()
        {
            DataTable dt = (DataTable)ViewState["FlakinessTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grd_Flakiness.Rows[i].Cells[1].FindControl("txt_SieveSize");
                TextBox txt_RetainedmassonSieve = (TextBox)grd_Flakiness.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                TextBox txt_picesespassing = (TextBox)grd_Flakiness.Rows[i].Cells[3].FindControl("txt_picesespassing");
                TextBox txt_GaugePassingWt = (TextBox)grd_Flakiness.Rows[i].Cells[4].FindControl("txt_GaugePassingWt");
                TextBox txt_GaugePassingSampleNo = (TextBox)grd_Flakiness.Rows[i].Cells[5].FindControl("txt_GaugePassingSampleNo");
                TextBox txt_Total = (TextBox)grd_Flakiness.Rows[i].Cells[6].FindControl("txt_Total");

                txt_SieveSize.Text = dt.Rows[i]["txt_SieveSize"].ToString();
                txt_RetainedmassonSieve.Text = dt.Rows[i]["txt_RetainedmassonSieve"].ToString();
                txt_picesespassing.Text = dt.Rows[i]["txt_picesespassing"].ToString();
                txt_GaugePassingWt.Text = dt.Rows[i]["txt_GaugePassingWt"].ToString();
                txt_GaugePassingSampleNo.Text = dt.Rows[i]["txt_GaugePassingSampleNo"].ToString();
                txt_Total.Text = dt.Rows[i]["txt_Total"].ToString();
            }

        }

        public void DisplayFlakinessIndex()
        {
            if (grd_Flakiness.Rows.Count <= 0)
            {
                int i = 0;
                string[] Flaki_Seive = { "63-50", "50-40", "40-31.5", "31.5-25", "25-20", "20-16", "16-12.5", "12.5-10", "10-6.3" };
                for (int s = 0; s < Flaki_Seive.Count(); s++)
                {
                    AddRowforFlakiness();
                    TextBox txt_SieveSize = (TextBox)grd_Flakiness.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    txt_SieveSize.Text = Flaki_Seive[s];
                    i++;
                }
            }
        }
        public void DisplayElongationIndex()
        {
            if (grd_Elongation.Rows.Count <= 0)
            {
                int i = 0;
                string[] Flaki_Seive = { "63-50", "50-40", "40-31.5", "31.5-25", "25-20", "20-16", "16-12.5", "12.5-10", "10-6.3" };
                for (int s = 0; s < Flaki_Seive.Count(); s++)
                {
                    AddRowforElongation();
                    TextBox txt_SieveSize = (TextBox)grd_Elongation.Rows[i].Cells[1].FindControl("txt_SieveSize");
                    txt_SieveSize.Text = Flaki_Seive[s];
                    i++;
                }
            }

        }

        #region Elongation Grid
        protected void AddRowforElongation()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ElongationTestTable"] != null)
            {
                GetCurrentDataforElongation();
                dt = (DataTable)ViewState["ElongationTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_SieveSize", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_RetainedmassonSieve", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_picesesretained", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_GaugeretainedWt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_GaugeretainedSampleNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Total", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_SieveSize"] = string.Empty;
            dr["txt_RetainedmassonSieve"] = string.Empty;
            dr["txt_picesesretained"] = string.Empty;
            dr["txt_GaugeretainedWt"] = string.Empty;
            dr["txt_GaugeretainedSampleNo"] = string.Empty;
            dr["txt_Total"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ElongationTestTable"] = dt;
            grd_Elongation.DataSource = dt;
            grd_Elongation.DataBind();
            SetPreviousDataforElongation();

        }
        protected void GetCurrentDataforElongation()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_SieveSize", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_RetainedmassonSieve", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_picesesretained", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_GaugeretainedWt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_GaugeretainedSampleNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Total", typeof(string)));

            for (int i = 0; i < grd_Elongation.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grd_Elongation.Rows[i].Cells[1].FindControl("txt_SieveSize");
                TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                TextBox txt_picesesretained = (TextBox)grd_Elongation.Rows[i].Cells[3].FindControl("txt_picesesretained");
                TextBox txt_GaugeretainedWt = (TextBox)grd_Elongation.Rows[i].Cells[4].FindControl("txt_GaugeretainedWt");
                TextBox txt_GaugeretainedSampleNo = (TextBox)grd_Elongation.Rows[i].Cells[5].FindControl("txt_GaugeretainedSampleNo");
                TextBox txt_Total = (TextBox)grd_Elongation.Rows[i].Cells[6].FindControl("txt_Total");

                drRow = dtTable.NewRow();
                drRow["txt_SieveSize"] = txt_SieveSize.Text;
                drRow["txt_RetainedmassonSieve"] = txt_RetainedmassonSieve.Text;
                drRow["txt_picesesretained"] = txt_picesesretained.Text;
                drRow["txt_GaugeretainedWt"] = txt_GaugeretainedWt.Text;
                drRow["txt_GaugeretainedSampleNo"] = txt_GaugeretainedSampleNo.Text;
                drRow["txt_Total"] = txt_Total.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ElongationTestTable"] = dtTable;
        }
        protected void SetPreviousDataforElongation()
        {
            DataTable dt = (DataTable)ViewState["ElongationTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grd_Elongation.Rows[i].Cells[1].FindControl("txt_SieveSize");
                TextBox txt_RetainedmassonSieve = (TextBox)grd_Elongation.Rows[i].Cells[2].FindControl("txt_RetainedmassonSieve");
                TextBox txt_picesesretained = (TextBox)grd_Elongation.Rows[i].Cells[3].FindControl("txt_picesesretained");
                TextBox txt_GaugeretainedWt = (TextBox)grd_Elongation.Rows[i].Cells[4].FindControl("txt_GaugeretainedWt");
                TextBox txt_GaugeretainedSampleNo = (TextBox)grd_Elongation.Rows[i].Cells[5].FindControl("txt_GaugeretainedSampleNo");
                TextBox txt_Total = (TextBox)grd_Elongation.Rows[i].Cells[6].FindControl("txt_Total");

                txt_SieveSize.Text = dt.Rows[i]["txt_SieveSize"].ToString();
                txt_RetainedmassonSieve.Text = dt.Rows[i]["txt_RetainedmassonSieve"].ToString();
                txt_picesesretained.Text = dt.Rows[i]["txt_picesesretained"].ToString();
                txt_GaugeretainedWt.Text = dt.Rows[i]["txt_GaugeretainedWt"].ToString();
                txt_GaugeretainedSampleNo.Text = dt.Rows[i]["txt_GaugeretainedSampleNo"].ToString();
                txt_Total.Text = dt.Rows[i]["txt_Total"].ToString();
            }

        }
        #endregion
        //
        protected void imgBtnImpactAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowforImpactValue();
        }
        protected void imgBtnImpactDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grd_ImpactValue.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grd_ImpactValue.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowforImpactValue(gvr.RowIndex);
            }
        }
        protected void DeleteRowforImpactValue(int rowIndex)
        {
            GetCurrentDataforImpactValue();
            DataTable dt = ViewState["ImpactValueTestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ImpactValueTestTable"] = dt;
            grd_ImpactValue.DataSource = dt;
            grd_ImpactValue.DataBind();
            SetPreviousDataforImpactValue();
        }
        protected void imgBtnCrushAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowforCrushedValue();
        }
        protected void imgBtnCrushDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grd_CrushValue.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grd_CrushValue.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowforCrushedValue(gvr.RowIndex);
            }
        }
        protected void DeleteRowforCrushedValue(int rowIndex)
        {
            GetCurrentDataforCrushedValue();
            DataTable dt = ViewState["CrushedValueTestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CrushedValueTestTable"] = dt;
            grd_CrushValue.DataSource = dt;
            grd_CrushValue.DataBind();
            SetPreviousDataforCrushedValue();
        }

        #region SpecificGravity Grid
        protected void AddRowforSpecificGravity()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SpecificGravityTestTable"] != null)
            {
                GetCurrentDataforSpecificGravity();
                dt = (DataTable)ViewState["SpecificGravityTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_SSD", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtOfBottleSample", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtOfBottleDistilled", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_OvenDryWeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_SpecificGravity", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_SSD"] = string.Empty;
            dr["txt_WtOfBottleSample"] = string.Empty;
            dr["txt_WtOfBottleDistilled"] = string.Empty;
            dr["txt_OvenDryWeight"] = string.Empty;
            dr["txt_SpecificGravity"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["SpecificGravityTestTable"] = dt;
            grd_SpecificGravity.DataSource = dt;
            grd_SpecificGravity.DataBind();
            SetPreviousDataforSpecificGravity();

        }
        protected void GetCurrentDataforSpecificGravity()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_SSD", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtOfBottleSample", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtOfBottleDistilled", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_OvenDryWeight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_SpecificGravity", typeof(string)));

            for (int i = 0; i < grd_SpecificGravity.Rows.Count; i++)
            {
                TextBox txt_SSD = (TextBox)grd_SpecificGravity.Rows[i].Cells[1].FindControl("txt_SSD");
                TextBox txt_WtOfBottleSample = (TextBox)grd_SpecificGravity.Rows[i].Cells[2].FindControl("txt_WtOfBottleSample");
                TextBox txt_WtOfBottleDistilled = (TextBox)grd_SpecificGravity.Rows[i].Cells[3].FindControl("txt_WtOfBottleDistilled");
                TextBox txt_OvenDryWeight = (TextBox)grd_SpecificGravity.Rows[i].Cells[4].FindControl("txt_OvenDryWeight");
                TextBox txt_SpecificGravity = (TextBox)grd_SpecificGravity.Rows[i].Cells[5].FindControl("txt_SpecificGravity");

                drRow = dtTable.NewRow();
                drRow["txt_SSD"] = txt_SSD.Text;
                drRow["txt_WtOfBottleSample"] = txt_WtOfBottleSample.Text;
                drRow["txt_WtOfBottleDistilled"] = txt_WtOfBottleDistilled.Text;
                drRow["txt_OvenDryWeight"] = txt_OvenDryWeight.Text;
                drRow["txt_SpecificGravity"] = txt_SpecificGravity.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SpecificGravityTestTable"] = dtTable;
        }
        protected void SetPreviousDataforSpecificGravity()
        {
            DataTable dt = (DataTable)ViewState["SpecificGravityTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_SSD = (TextBox)grd_SpecificGravity.Rows[i].Cells[1].FindControl("txt_SSD");
                TextBox txt_WtOfBottleSample = (TextBox)grd_SpecificGravity.Rows[i].Cells[2].FindControl("txt_WtOfBottleSample");
                TextBox txt_WtOfBottleDistilled = (TextBox)grd_SpecificGravity.Rows[i].Cells[3].FindControl("txt_WtOfBottleDistilled");
                TextBox txt_OvenDryWeight = (TextBox)grd_SpecificGravity.Rows[i].Cells[4].FindControl("txt_OvenDryWeight");
                TextBox txt_SpecificGravity = (TextBox)grd_SpecificGravity.Rows[i].Cells[5].FindControl("txt_SpecificGravity");

                txt_SSD.Text = dt.Rows[i]["txt_SSD"].ToString();
                txt_WtOfBottleSample.Text = dt.Rows[i]["txt_WtOfBottleSample"].ToString();
                txt_WtOfBottleDistilled.Text = dt.Rows[i]["txt_WtOfBottleDistilled"].ToString();
                txt_OvenDryWeight.Text = dt.Rows[i]["txt_OvenDryWeight"].ToString();
                txt_SpecificGravity.Text = dt.Rows[i]["txt_SpecificGravity"].ToString();
            }

        }
        #endregion

        #region ImpactValue Grid

        protected void AddRowforImpactValue()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ImpactValueTestTable"] != null)
            {
                GetCurrentDataforImpactValue();
                dt = (DataTable)ViewState["ImpactValueTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Initialwtsample", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtSampleretained", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtSamplepassing", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Passing", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_Initialwtsample"] = string.Empty;
            dr["txt_WtSampleretained"] = string.Empty;
            dr["txt_WtSamplepassing"] = string.Empty;
            dr["txt_Passing"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ImpactValueTestTable"] = dt;
            grd_ImpactValue.DataSource = dt;
            grd_ImpactValue.DataBind();
            SetPreviousDataforImpactValue();

        }
        protected void GetCurrentDataforImpactValue()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Initialwtsample", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtSampleretained", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtSamplepassing", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Passing", typeof(string)));

            for (int i = 0; i < grd_ImpactValue.Rows.Count; i++)
            {
                TextBox txt_Initialwtsample = (TextBox)grd_ImpactValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                TextBox txt_WtSampleretained = (TextBox)grd_ImpactValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                TextBox txt_WtSamplepassing = (TextBox)grd_ImpactValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                TextBox txt_Passing = (TextBox)grd_ImpactValue.Rows[i].Cells[6].FindControl("txt_Passing");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_Initialwtsample"] = txt_Initialwtsample.Text;
                drRow["txt_WtSampleretained"] = txt_WtSampleretained.Text;
                drRow["txt_WtSamplepassing"] = txt_WtSamplepassing.Text;
                drRow["txt_Passing"] = txt_Passing.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ImpactValueTestTable"] = dtTable;
        }
        protected void SetPreviousDataforImpactValue()
        {
            DataTable dt = (DataTable)ViewState["ImpactValueTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Initialwtsample = (TextBox)grd_ImpactValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                TextBox txt_WtSampleretained = (TextBox)grd_ImpactValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                TextBox txt_WtSamplepassing = (TextBox)grd_ImpactValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                TextBox txt_Passing = (TextBox)grd_ImpactValue.Rows[i].Cells[6].FindControl("txt_Passing");

                grd_ImpactValue.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_Initialwtsample.Text = dt.Rows[i]["txt_Initialwtsample"].ToString();
                txt_WtSampleretained.Text = dt.Rows[i]["txt_WtSampleretained"].ToString();
                txt_WtSamplepassing.Text = dt.Rows[i]["txt_WtSamplepassing"].ToString();
                txt_Passing.Text = dt.Rows[i]["txt_Passing"].ToString();
            }

        }

        #endregion

        #region CrushingValue Grid

        protected void AddRowforCrushedValue()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CrushedValueTestTable"] != null)
            {
                GetCurrentDataforCrushedValue();
                dt = (DataTable)ViewState["CrushedValueTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Initialwtsample", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtSampleretained", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtSamplepassing", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Passing", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_Initialwtsample"] = string.Empty;
            dr["txt_WtSampleretained"] = string.Empty;
            dr["txt_WtSamplepassing"] = string.Empty;
            dr["txt_Passing"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CrushedValueTestTable"] = dt;
            grd_CrushValue.DataSource = dt;
            grd_CrushValue.DataBind();
            SetPreviousDataforCrushedValue();

        }
        protected void GetCurrentDataforCrushedValue()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Initialwtsample", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtSampleretained", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtSamplepassing", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Passing", typeof(string)));

            for (int i = 0; i < grd_CrushValue.Rows.Count; i++)
            {
                TextBox txt_Initialwtsample = (TextBox)grd_CrushValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                TextBox txt_WtSampleretained = (TextBox)grd_CrushValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                TextBox txt_WtSamplepassing = (TextBox)grd_CrushValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                TextBox txt_Passing = (TextBox)grd_CrushValue.Rows[i].Cells[6].FindControl("txt_Passing");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_Initialwtsample"] = txt_Initialwtsample.Text;
                drRow["txt_WtSampleretained"] = txt_WtSampleretained.Text;
                drRow["txt_WtSamplepassing"] = txt_WtSamplepassing.Text;
                drRow["txt_Passing"] = txt_Passing.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CrushedValueTestTable"] = dtTable;
        }
        protected void SetPreviousDataforCrushedValue()
        {
            DataTable dt = (DataTable)ViewState["CrushedValueTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Initialwtsample = (TextBox)grd_CrushValue.Rows[i].Cells[3].FindControl("txt_Initialwtsample");
                TextBox txt_WtSampleretained = (TextBox)grd_CrushValue.Rows[i].Cells[4].FindControl("txt_WtSampleretained");
                TextBox txt_WtSamplepassing = (TextBox)grd_CrushValue.Rows[i].Cells[5].FindControl("txt_WtSamplepassing");
                TextBox txt_Passing = (TextBox)grd_CrushValue.Rows[i].Cells[6].FindControl("txt_Passing");

                grd_CrushValue.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_Initialwtsample.Text = dt.Rows[i]["txt_Initialwtsample"].ToString();
                txt_WtSampleretained.Text = dt.Rows[i]["txt_WtSampleretained"].ToString();
                txt_WtSamplepassing.Text = dt.Rows[i]["txt_WtSamplepassing"].ToString();
                txt_Passing.Text = dt.Rows[i]["txt_Passing"].ToString();
            }

        }

        #endregion
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowAGGT_Remark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdAggtRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdAggtRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowAGGT_Remark(gvr.RowIndex);
            }
        }

        protected void DeleteRowAGGT_Remark(int rowIndex)
        {
            GetCurrentDataAGGT_Remark();
            DataTable dt = ViewState["AGGTRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete() ;
            ViewState["AGGTRemarkTable"] = dt;
            grdAggtRemark.DataSource = dt;
            grdAggtRemark.DataBind();
            SetPreviousDataAGGT_Remark();
        }

        protected void AddRowAGGT_Remark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AGGTRemarkTable"] != null)
            {
                GetCurrentDataAGGT_Remark();
                dt = (DataTable)ViewState["AGGTRemarkTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_REMARK"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["AGGTRemarkTable"] = dt;
            grdAggtRemark.DataSource = dt;
            grdAggtRemark.DataBind();
            SetPreviousDataAGGT_Remark();
        }
        protected void GetCurrentDataAGGT_Remark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdAggtRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdAggtRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["AGGTRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataAGGT_Remark()
        {
            DataTable dt = (DataTable)ViewState["AGGTRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdAggtRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                grdAggtRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }

        protected void chk_WitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txt_witnessBy.Text = string.Empty;
            if (chk_WitnessBy.Checked)
            {
                txt_witnessBy.Visible = true;
                txt_witnessBy.Focus();
            }
            else
            {
                txt_witnessBy.Visible = false;
               
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            int MatId = 0; string MaterialName = "";
            if (lbl_OtherPending.Text == "Material Name")
            {
                MatId = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
                MaterialName = ddl_OtherPendingRpt.SelectedItem.Text;
            }
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_RecType.Text == "AGGT")
            {   
                //rpt.Aggregate_PDFReport(txt_ReferenceNo.Text, txt_RecType.Text, MaterialName, MatId, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", MaterialName, MatId.ToString(), "", "");
            }
            else
            {
                //PrintHTMLReport rpt = new PrintHTMLReport();
                //rpt.TrialSieveAnalysis_Html(txt_ReferenceNo.Text, txt_RecType.Text);
                
                //rptPdf.MFSieveAnalysis_PDF(txt_ReferenceNo.Text, txt_RecType.Text, "Print");
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, "Print", "", "", "Sieve Analysis", "", "", "", "");
            }

        }

       

        //protected string getDetailReportAggtTesting()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    string AggregateType = string.Empty;
        //    string AggregateName = string.Empty;
        //    string WitnessBy = string.Empty;
        //    var AggtTest = dc.ReportStatus_View("Aggregate Testing", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);
        //    foreach (var aggt in AggtTest)
        //    {
        //        AggregateName = aggt.AGGTINWD_AggregateName_var.ToString();
        //        if (Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "10 mm" || Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "20 mm" || Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "40 mm")
        //        {
        //            AggregateType = "Coarse Aggregate";
        //        }
        //        else
        //        {
        //            AggregateType = "Fine Aggregate";
        //        }
        //        mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //        mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>" + AggregateType + "   " + "(" + Convert.ToString(aggt.AGGTINWD_AggregateName_var) + ")" + " </b></font></td></tr>";

        //        mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //        mySql += "<tr>" +
        //            "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td width='40%' height=19><font size=2>" + aggt.CL_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "-" + "</font></td>" +
        //            "<td height=19><font size=2>&nbsp;</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + aggt.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "AGGT" + "-" + aggt.AGGTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //          "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //          "<td width='2%' height=19><font size=2></font></td>" +
        //          "<td width='10%' height=19><font size=2></font></td>" +
        //          "<td height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
        //          "<td height=19><font size=2>:</font></td>" +
        //          "<td height=19><font size=2>" + "AGGT" + "-" + aggt.AGGTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //          "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + aggt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "DT" + "-" + " " + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +

        //             "<td width='20%' height=19><font size=2>  </font></td>" +
        //             "<td width='2%' height=19><font size=2> </font></td>" +
        //             "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td height=19><font size=2><b>Date of receipt </b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + Convert.ToDateTime(aggt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";
        //        WitnessBy = aggt.AGGTINWD_WitnessBy_var.ToString();
        //        break;

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";
        //    mySql += "</table>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";
        //    bool SpecGrav = false;
        //    bool lbd = false;
        //    bool Moist = false;
        //    bool Sild = false;
        //    bool Sieve = false;
        //    bool Impact = false;
        //    bool Elong = false;
        //    bool Crush = false;
        //    bool Flaki = false;
        //    var aggtTestname = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AGGT");
        //    foreach (var aggtt in aggtTestname)
        //    {
        //        if (aggtt.TEST_Sr_No == 1)
        //        {
        //            Sieve = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 3)
        //        {
        //            SpecGrav = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 4)
        //        {
        //            lbd = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 9)
        //        {
        //            Moist = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 2)
        //        {
        //            Sild = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 7)
        //        {
        //            Impact = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 6)
        //        {
        //            Elong = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 8)
        //        {
        //            Crush = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 5)
        //        {
        //            Flaki = true;
        //        }
        //    }
        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //    mySql += "<td width= 2% align=left  valign=top height=19 rowspan=5  ><font size=2> " + " " + " </font></td>";
        //    mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //    mySql += "</tr>";

        //    var Inward_aggt = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text, 0, 0, 0);
        //    foreach (var aggt in Inward_aggt)
        //    {
        //        if (aggt.AGGTINWD_AggregateName_var == "Natural Sand" || aggt.AGGTINWD_AggregateName_var == "Crushed Sand" || aggt.AGGTINWD_AggregateName_var == "Stone Dust" || aggt.AGGTINWD_AggregateName_var == "Grit")
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Moisture Content" + " </font></td>";
        //            if (Moist == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_MoistureContent_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " % " + "</font></td>";
        //            mySql += "</tr>";

        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
        //            mySql += "</tr>";
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
        //            if (lbd == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Material finer than 75 u </br> (by wet sieving)" + "</font></td>";
        //            if (Sild == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SildContent_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " %  " + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //        if (aggt.AGGTINWD_AggregateName_var == "10 mm" || aggt.AGGTINWD_AggregateName_var == "20 mm" || aggt.AGGTINWD_AggregateName_var == "40 mm" || aggt.AGGTINWD_AggregateName_var == "Mix Aggt")
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Impact Value" + " </font></td>";
        //            if (Impact == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_ImpactValue_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
        //            mySql += "</tr>";

        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
        //            mySql += "</tr>";
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
        //            if (lbd == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Flakiness Value" + "</font></td>";
        //            if (Flaki == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Flakiness_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " " + "</font></td>";
        //            mySql += "</tr>";
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Elongness Value" + "</font></td>";
        //            if (Elong == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Elongation_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Crushing Value" + "</font></td>";
        //            if (Crush == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_CrushingValue_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "</table>";

        //    if (Sieve == true)
        //    {
        //        mySql += "<table>";
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "Sieve Analysis (by dry sieving) " + "</b></font></td>";
        //        mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>" + AggregateName + "</b></font></td>";
        //        mySql += "</tr>";
        //        mySql += "</table>";
        //        mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //        int i = 0;
        //        var aggtTest = dc.AggregateAllTestView(txt_ReferenceNo.Text, Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue), "AGGTSA");
        //        foreach (var aggtt in aggtTest)
        //        {
        //            if (i == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2%  rowspan=2 align=center valign=middle height=19 ><font size=2><b>Sieve Size</b></font></td>";
        //                mySql += "<td width= 10%  align=center colspan=3 valign=top height=19 ><font size=2><b>Weight retained</b></font></td>";
        //                mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2><b>Passing </b></font></td>";
        //                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                {
        //                    mySql += "<td width= 2%   align=center rowspan=2 valign=middle height=19 ><font size=2><b>IS Passing % Limits </b></font></td>";
        //                }
        //                mySql += "</tr>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(g)</b></font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>Cummu (%)</b></font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
        //            }
        //            mySql += "<tr>";
        //            mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_SeiveSize_var.ToString() + "</font></td>";
        //            mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_Weight_num.ToString() + "</font></td>";
        //            if (aggtt.AGGTSA_SeiveSize_var != "Total")
        //            {
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + Convert.ToDecimal(aggtt.AGGTSA_WeightRet_dec).ToString("0.00") + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuWeightRet_dec.ToString() + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuPassing_dec.ToString() + "</font></td>";
        //                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                {
        //                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + aggtt.AGGTSA_IsPassingLmt_var.ToString() + " </font></td>";
        //                }
        //            }
        //            else if (aggtt.AGGTSA_SeiveSize_var == "Total")
        //            {
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "" + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "Fineness Modulus" + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTINWD_FM_var.ToString() + "</font></td>";
        //                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                {
        //                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + "" + " </font></td>";
        //                }
        //            }
        //            i++;
        //        }
        //        mySql += "</tr>";
        //        mySql += "</table>";
        //    }


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    int SrNo = 0;
        //    var matid = dc.Material_View("AGGT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "AGGT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.AGGTDetail_RemarkId_int), "AGGT");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.AGGT_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (lblEntry.Text == "Check")
        //    {
        //        var RecNo = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, txt_ReferenceNo.Text, 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.AGGTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.AGGTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.AGGTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                mySql += "<tr>";
        //                if (WitnessBy != string.Empty)
        //                {
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> Witness by : " + WitnessBy + " </font></td>";
        //                }
        //                var lgin = dc.User_View(r.AGGTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {

        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";

        //                }
        //                mySql += "</tr>";
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";

        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //protected void imgClosePopup_Click(object sender, EventArgs e)
        //{
        //    Label lbl_Back = (Label)Master.FindControl("lbl_Back");
        //    Session["lbl_Back"] = lbl_Back.Text;
        //    Response.Redirect("ReportStatus.aspx");
        //}
        
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkSGViewImages_Click(object sender, EventArgs e)
        {
            if (lblSSDImage1.Text != "" || lblWtBottleSampleImage1.Text != "" || lblOverDryWeightImage1.Text != ""
                || lblSSDImage2.Text != "" || lblWtBottleSampleImage2.Text != "" || lblOverDryWeightImage2.Text != "")
            {
                string strImages = getMixDesignImagesString(txt_ReferenceNo.Text);
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.DownloadHtmlReport("MixDesignImages", strImages);
            }
        }

        protected string getMixDesignImagesString(string referenceNo)
        {
            string reportStr = "", myStr = "";
            myStr += "<html>";
            myStr += "<head>";
            myStr += "<style type='text/css'>";
            myStr += "body {margin-left:2em margin-right:2em}";
            myStr += "</style>";
            myStr += "</head>";

            myStr += "<tr><td width='100%'height=105>";
            //myStr += "<img border=0 src='Images/" + imgName + ".JPG' >";
            myStr += "&nbsp;</td></tr>";

            myStr += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            myStr += "<tr><td width='99%' colspan=3 height=19>&nbsp;</td></tr>";
            myStr += "<tr><td width='99%' colspan=3 align=center valign=top height=19><font size=4><b>Steel Images</b></font></td></tr>";
            myStr += "<tr><td width='99%' colspan=3>&nbsp;</td></tr>";

            myStr += "<tr><td width= 20% align=left valign=top height=19 ><font size=2>" + "Reference No : " + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + ":" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + referenceNo + "</font></td></tr>";
            
            myStr += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            myStr += "<tr><td colspan=3 width= 10% align=left valign=top>";
            myStr += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            string imgLink = "http://92.204.136.64:81/mixappphoto/";
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                imgLink += "mumbai/";
            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                imgLink += "nashik/";
            }
                        
            int srNo = 1;
            myStr += "<tr>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Sr. No." + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "SSD" + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "wt of Bottle + Sample" + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Oven Dry Wt" + "</font></td>";
            myStr += "</tr>";


            if (lblSSDImage1.Text != "" || lblWtBottleSampleImage1.Text != "" || lblOverDryWeightImage1.Text != "")
            {

                myStr += "<tr>";
                myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + srNo.ToString() + "</font></td>";
                if (lblSSDImage1.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblSSDImage1.Text) == true)
                {
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + lblSSDImage1.Text + "' >";
                    myStr += "</td>";
                }

                if (lblWtBottleSampleImage1.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblWtBottleSampleImage1.Text) == true)
                {
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + lblWtBottleSampleImage1.Text + "' >";
                    myStr += "</td>";
                }

                if (lblOverDryWeightImage1.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblOverDryWeightImage1.Text) == true)
                {
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + lblOverDryWeightImage1.Text + "' >";
                    myStr += "</td>";
                }
                myStr += "</tr>";
                myStr += "<tr><td width= 5% align=left valign=top height=19 colspan=2><font size=2>&nbsp;</font></td></tr>";

            }
            srNo++;
            if (lblSSDImage2.Text != "" || lblWtBottleSampleImage2.Text != "" || lblOverDryWeightImage2.Text != "")
            {

                myStr += "<tr>";
                myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + srNo.ToString() + "</font></td>";
                if (lblSSDImage2.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblSSDImage2.Text) == true)
                {
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + lblSSDImage2.Text + "' >";
                    myStr += "</td>";
                }

                if (lblWtBottleSampleImage2.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblWtBottleSampleImage2.Text) == true)
                {
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + lblWtBottleSampleImage2.Text + "' >";
                    myStr += "</td>";
                }

                if (lblOverDryWeightImage2.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblOverDryWeightImage2.Text) == true)
                {
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + lblOverDryWeightImage2.Text + "' >";
                    myStr += "</td>";
                }
                myStr += "</tr>";
                myStr += "<tr><td width= 5% align=left valign=top height=19 colspan=2><font size=2>&nbsp;</font></td></tr>";
                
            }
            myStr += "</table>";
            myStr += "</td></tr>";

            myStr += "</table>";
            myStr += "</html>";
            return reportStr = myStr;

        }
    }
}