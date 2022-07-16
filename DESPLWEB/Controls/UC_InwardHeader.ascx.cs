using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace DESPLWEB.Controls
{
    public partial class UC_InwardHeader : System.Web.UI.UserControl
    {
        public delegate void TextChangedEventHandler(Object sender, EventArgs e);
        public delegate void ClickEventHandler(Object sender, EventArgs e);

        public event TextChangedEventHandler TextChanged = delegate { };
        public event ClickEventHandler Click = delegate { };

        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (EnquiryNo != "" || RecType != "")
                {
                    LoadDetails();
                }
            }            
        }

        protected void LoadDetails()
        {
            double CL_BalanceAmt_mny = 0, CL_Limit_mny = 0;
            if (EnquiryNo != "")
            {
                #region new inward
                clsData clsDt = new clsData(); 
                var enquiry = dc.Enquiry_View(Convert.ToInt32(EnquiryNo), 1, 0).ToList();
                if (clsDt.checkGSTInfoUpdated(enquiry.FirstOrDefault().ENQ_CL_Id.ToString(), enquiry.FirstOrDefault().ENQ_SITE_Id.ToString(), enquiry.FirstOrDefault().MATERIAL_RecordType_var) == false)
                {
                    lblGst.Text = "Please update client & site GST details. Can not add inward.";
                    lblGst.Visible = true;
                    txtEnquiryNo.Text = ""; lblBilling.Text = ""; 
                }
                else
                {
                    lblGst.Visible = false;
                    LoadBuildingList();
                    CollectionDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString()).ToString("dd/MM/yyyy");
                    //ReceivedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy");
                    ReceivedDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss");
                    CollectionTime = DateTime.Now.ToShortTimeString();
                    string Materialtype = ""; 
                    //var enquiry = dc.Enquiry_View(Convert.ToInt32(EnquiryNo), 1, 0);
                    foreach (var enq in enquiry)
                    {
                        ClientName = enq.CL_Name_var;
                        SiteName = enq.SITE_Name_var;
                        EnquiryNo = enq.ENQ_Id.ToString();
                        ContactNo = enq.CONT_ContactNo_var;
                        EmailId = enq.CONT_EmailID_var;
                       // Discount = enq.CL_DiscSetting_var;

                        lblClId.Text = enq.CL_Id.ToString();
                        lblSiteId.Text = enq.SITE_Id.ToString();
                        SiteMonthlyStatus = Convert.ToInt32(enq.SITE_MonthlyBillingStatus_bit).ToString();
                        lblRecType.Text = enq.MATERIAL_RecordType_var.ToString();

                        LoadContactPersonList(Convert.ToInt32(enq.SITE_Id), Convert.ToInt32(enq.CL_Id));
                        ContactPerson = enq.CONT_Id.ToString();
                        Materialtype = enq.MATERIAL_RecordType_var.ToString();
                        MaterialId = enq.MATERIAL_Id.ToString();

                        EnqNoApp = enq.ENQ_MobileAppEnqNo_int.ToString();
                        EnqDate =enq.ENQ_Date_dt.ToString();
                        CL_BalanceAmt_mny = Convert.ToDouble(enq.CL_BalanceAmt_mny);
                        CL_Limit_mny = Convert.ToDouble(enq.CL_Limit_mny);
                        if (CL_BalanceAmt_mny > CL_Limit_mny)
                            ClCreditLimitExcededStatus = "1";
                        else
                            ClCreditLimitExcededStatus = "0";

                    }

                    if (SiteMonthlyStatus == "1")
                    {
                        lblBilling.Text = "Billing - Monthly";
                    }
                    else
                    {
                        lblBilling.Text = "Billing - Regular";
                    }
                    var currentNo = dc.getCurrentNumber("RecordNo", Materialtype);
                    foreach (var no in currentNo)
                    {
                        if (string.IsNullOrEmpty(no.CurrentNo.ToString()) == true)
                            RecordNo = "1";
                        else
                            //RecordNo = (no.CurrentNo +1).ToString();
                            RecordNo = (no.CurrentNo).ToString();
                    }
                    currentNo = dc.getCurrentNumber("ReferenceNo", "");
                    foreach (var no in currentNo)
                    {
                        if (string.IsNullOrEmpty(no.CurrentNo.ToString()) == true)
                            ReferenceNo = "1";
                        else
                            ReferenceNo = (no.CurrentNo + 1).ToString();
                    }
                    currentNo = dc.getCurrentNumber("BillNo", "");
                    foreach (var no in currentNo)
                    {
                        if (string.IsNullOrEmpty(no.CurrentNo.ToString()) == true)
                            BillNo = "1";
                        else
                            BillNo = (no.CurrentNo + 1).ToString();
                    }
                    //if (lblClId.Text != "" && lblSiteId.Text != "")
                    //{
                    //    ShowDiscount(Materialtype, Convert.ToInt32(lblClId.Text), Convert.ToInt32(lblSiteId.Text));
                    //}

                    //if (Session["Superadmin"] != null)
                    //{
                    //    if (Session["Superadmin"].ToString() == "True" || Session["InwardApproveRight"].ToString() == "True")
                    //        chkOtherClient.Visible = true;
                    //}
                    //if (Session["WithoutBillRight"] != null)
                    //{
                    //    if (Session["WithoutBillRight"].ToString() == "True")
                    //        chkOtherClient.Visible = true;
                    //}
                    chkOtherClient.Visible = true;
                    //delete report client id and site id from enquiry client table
                    dc.EnquiryClient_Update(Convert.ToInt32(EnquiryNo), 0, 0);
                    //
                }
                #endregion                
            }
            if (lblGst.Visible == false)
            {
                if (RecType != "" && RecordNo != "")
                {
                    if (RecType == "NDT" || RecType == "CR" || RecType == "SO" || RecType == "GT" || RecType == "OT" || RecType == "AAC")
                    {
                        lblCharges.Visible = false;
                        txtCharges.Visible = false;
                    }
                    #region modify inward

                    var Modify = dc.ModifyInward_View(Convert.ToInt32(RecordNo), Convert.ToInt32(ReferenceNo), null, RecType, null, null);
                    foreach (var n in Modify)
                    {
                        LoadBuildingList();
                        cmbBuilding.SelectedItem.Text = n.INWD_Building_var.ToString();
                        lblClId.Text = n.INWD_CL_Id.ToString();
                        lblSiteId.Text = n.INWD_SITE_Id.ToString();
                        LoadContactPersonList(Convert.ToInt32(lblSiteId.Text), Convert.ToInt32(lblClId.Text));
                        ContactPerson = n.INWD_CONT_Id.ToString();
                        SiteMonthlyStatus = Convert.ToInt32(n.SITE_MonthlyBillingStatus_bit).ToString();
                        EnquiryNo = n.INWD_ENQ_Id.ToString();
                        BillNo = n.INWD_BILL_Id.ToString();
                        if (n.INWD_PROINV_Id != null)
                            ProformaInvoiceNo = n.INWD_PROINV_Id.ToString();
                        //MaterialId = n.MATERIAL_Id.ToString();
                        //if (lblClId.Text != "" && lblSiteId.Text != "")
                        //{
                        //    ShowDiscount(Convert.ToString(n.MATERIAL_RecordType_var), Convert.ToInt32(lblClId.Text), Convert.ToInt32(lblSiteId.Text));
                        //}
                        MaterialId = n.ENQ_MATERIAL_Id.ToString();
                        if (lblClId.Text != "" && lblSiteId.Text != "")
                        {
                            ShowDiscount(RecType, Convert.ToInt32(lblClId.Text), Convert.ToInt32(lblSiteId.Text));
                        }
                        ReceivedDate = Convert.ToDateTime(n.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy HH:mm:ss");
                        CollectionDate = Convert.ToDateTime(n.INWD_CollectionDate_dt).ToString("dd/MM/yyyy");
                        EnqDate = Convert.ToDateTime(n.ENQ_Date_dt).ToString("dd/MM/yyyy");
                        CL_BalanceAmt_mny = Convert.ToDouble(n.CL_BalanceAmt_mny);
                        CL_Limit_mny = Convert.ToDouble(n.CL_Limit_mny);

                        if (CL_BalanceAmt_mny > CL_Limit_mny)
                            ClCreditLimitExcededStatus = "1";
                        else
                            ClCreditLimitExcededStatus = "0";

                        if (n.INWD_POFileName_var != null && n.INWD_POFileName_var != "")
                            lblPOFileName.Text = n.INWD_POFileName_var;
                    }

                    if (SiteMonthlyStatus == "1")
                    {
                        lblBilling.Text = "Billing - Monthly";
                    }
                    else
                    {
                        lblBilling.Text = "Billing - Regular";
                    }

                    #endregion
                }

                if (RecType != "")
                {
                    var material = dc.Material_View(RecType, "");
                    foreach (var mat in material)
                    {
                        MaterialId = mat.MATERIAL_Id.ToString();
                    }
                }
                LoadEnquiryList(Convert.ToInt32(MaterialId));

                if (EnquiryNo != "")
                {
                    ddlEnquiryList.SelectedValue = EnquiryNo.ToString();
                }
            }
        }

        public object DataSource
        {
            set 
            { 
                ddlEnquiryList.DataSource = value;
                ddlEnquiryList.DataTextField = "ENQ_Id";
                ddlEnquiryList.DataBind();
                ddlEnquiryList.Items.Insert(0, new ListItem("---Select---", "0")); 
            }
        }
        protected void LoadEnquiryList(int materialId)
        {
            var enqList = dc.Enquiry_View(0, 1, materialId);
            ddlEnquiryList.DataSource = enqList;
            ddlEnquiryList.DataTextField = "ENQ_Id";
            ddlEnquiryList.DataBind();
            ddlEnquiryList.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void LoadBuildingList()
        {
            var cl = dc.Inward_Building_View();
            cmbBuilding.DataTextField = "INWD_Building_var";
            cmbBuilding.DataValueField = "INWD_Building_var";
            cmbBuilding.DataSource = cl;
            cmbBuilding.DataBind();
            cmbBuilding.Items.Insert(0, "");
        }

        private void LoadContactPersonList( int SiteId , int ClId)
        {
            var cl = dc.Contact_View(0, SiteId, ClId, "");
            cmbContactPerson.DataTextField = "CONT_Name_var";
            cmbContactPerson.DataValueField = "CONT_Id";
            cmbContactPerson.DataSource = cl;
            cmbContactPerson.DataBind();
            cmbContactPerson.Items.Insert(0, "---Select---");
        }
        
        protected void txtSubsets_TextChanged(object sender, EventArgs e)
        {
            TextChanged(this, e);
        }

        protected void lnkFetch_Click(object sender, EventArgs e)        
        {
            if (ddlEnquiryList.SelectedValue != "0")
            {
                lblBilling.Text = "";
                lblMonthlyStatus.Text = "0";
                lblCrLimitStatus.Text = "0";
                lblEnqDate.Text = "";
                txtClientName.Text = "";
                chkOtherClient.Checked = false;
                //chkPropRateMatch.Checked = false;
                lblReceivedDate.Text = "";
                txtCollectionDate.Text = "";
                txtCollectionTime.Text = "";
                txtSiteName.Text = "";
                cmbContactPerson.DataSource = null;
                cmbContactPerson.DataBind();
                txtEnquiryNo.Text = "";
                txtContactNo.Text = "";
                txtRecordNo.Text = "";
                txtEmailId.Text = "";
                txtReferenceNo.Text = "";
                cmbBuilding.DataSource = null;
                cmbBuilding.DataBind();
                txtBillNo.Text = "";
                txtBillNo.ReadOnly = true;
                txtProformaInvoiceNo.Text = "";
                txtWorkOrder.Text = "";
                txtCharges.Text = "";
                //txtDiscount.Text = "";
                txtTotalQty.Text = "";
                txtSubsets.Text = "";
                lblPOFileName.Text = "";

                EnquiryNo = ddlEnquiryList.SelectedValue;
               
                LoadDetails();
                Click(this, e);
            }
        }
        public void ShowDiscount(string Materialtype , int ClId, int SiteId)
        {
            //Discount = "0";
            //var chk = dc.AllInwardDiscount_View(ClId, SiteId);
            //foreach (var c in chk)
            //{
            //    switch (Materialtype)
            //    {
            //        case "AAC":
            //            if (c.DISCOUNT_CORECUT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_AAC_tint.ToString();
            //            }
            //            break;
            //        case "CORECUT":
            //            if (c.DISCOUNT_CORECUT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_CORECUT_tint.ToString();
            //            }
            //            break;
            //        case "TILE":
            //            if (c.DISCOUNT_TILE_tint != null)
            //            {
            //                Discount = c.DISCOUNT_TILE_tint.ToString();
            //            }
            //            break;
            //        case "BT-":
            //            if (c.DISCOUNT_BT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_BT_tint.ToString();
            //            }
            //            break;
            //        case "MF":
            //            if (c.DISCOUNT_MF_tint != null)
            //            {
            //                Discount = c.DISCOUNT_MF_tint.ToString();
            //            }
            //            break;
            //        case "GT":
            //            if (c.DISCOUNT_GT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_GT_tint.ToString();
            //            }
            //            break;
            //        case "RWH":
            //            if (c.DISCOUNT_RWH_tint != null)
            //            {
            //                Discount = c.DISCOUNT_RWH_tint.ToString();
            //            }
            //            break;
            //        case "CT":
            //            if (c.DISCOUNT_CT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_CT_tint.ToString();
            //            }
            //            break;
            //        case "PILE":
            //            if (c.DISCOUNT_PILE_tint != null)
            //            {
            //                Discount = c.DISCOUNT_PILE_tint.ToString();
            //            }
            //            break;
            //        case "WT":
            //            if (c.DISCOUNT_WT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_WT_tint.ToString();
            //            }
            //            break;
            //        case "STC":
            //            if (c.DISCOUNT_STC_tint != null)
            //            {
            //                Discount = c.DISCOUNT_STC_tint.ToString();
            //            }
            //            break;
            //        case "CCH":
            //            if (c.DISCOUNT_CCH_tint != null)
            //            {
            //                Discount = c.DISCOUNT_CCH_tint.ToString();
            //            }
            //            break;
            //        case "ST":
            //            if (c.DISCOUNT_ST_tint != null)
            //            {
            //                Discount = c.DISCOUNT_ST_tint.ToString();
            //            }
            //            break;
            //        case "SOILD":
            //            if (c.DISCOUNT_SOLID_tint != null)
            //            {
            //                Discount = c.DISCOUNT_SOLID_tint.ToString();
            //            }
            //            break;
            //        case "OT":
            //            if (c.DISCOUNT_OT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_OT_tint.ToString();
            //            }
            //            break;
            //        case "CEMT":
            //            if (c.DISCOUNT_CEMT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_CEMT_tint.ToString();
            //            }
            //            break;
            //        case "FLYASH":
            //            if (c.DISCOUNT_FLYASH_tint != null)
            //            {
            //                Discount = c.DISCOUNT_FLYASH_tint.ToString();
            //            }
            //            break;
            //        case "SOLID":
            //            if (c.DISCOUNT_SOLID_tint != null)
            //            {
            //                Discount = c.DISCOUNT_SOLID_tint.ToString();
            //            }
            //            break;
            //        case "PT":
            //            if (c.DISCOUNT_PT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_PT_tint.ToString();
            //            }
            //            break;
            //        case "CR":
            //            if (c.DISCOUNT_CR_tint != null)
            //            {
            //                Discount = c.DISCOUNT_CR_tint.ToString();
            //            }
            //            break;
            //        case "NDT":
            //            if (c.DISCOUNT_NDT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_NDT_tint.ToString();
            //            }
            //            break;
            //        case "AGGT":
            //            if (c.DISCOUNT_AGGT_tint != null)
            //            {
            //                Discount = c.DISCOUNT_AGGT_tint.ToString();
            //            }
            //            break;
            //    }
            //}
        }
        public string EnqDate
        {
            get { return lblEnqDate.Text; }
            set { lblEnqDate.Text = value; }
        }
        public string ClCreditLimitExcededStatus
        {
            get { return lblCrLimitStatus.Text; }
            set { lblCrLimitStatus.Text = value; }
        }

        public string SiteMonthlyStatus
        {
            get { return lblMonthlyStatus.Text; }
            set { lblMonthlyStatus.Text = value; }
        }
        public string EnqNoApp
        {
            get { return lblEnqNoApp.Text; }
            set { lblEnqNoApp.Text = value; }
        }
        public string MaterialId
        {
            get { return lblMaterialId.Text; }
            set { lblMaterialId.Text = value; }
        }
        public string InwdStatus
        {
            get { return lblInwdStatus.Text; }
            set { lblInwdStatus.Text = value; }
        }
        public string RecType
        {
            get { return lblRecType.Text; }
            set { lblRecType.Text = value; }
        }
        public string ClientId
        {
            get { return lblClId.Text; }
            set { lblClId.Text = value; }
        }
        public string SiteId
        {
            get { return lblSiteId.Text; }
            set { lblSiteId.Text = value; }
        }
        public string ClientName
        {
            get { return txtClientName.Text; }
            set { txtClientName.Text = value; }
        }
        public string CollectionDate
        {
            get { return txtCollectionDate.Text; }
            set { txtCollectionDate.Text = value; }
        }
        public string ReceivedDate
        {
            get { return lblReceivedDate.Text; }
            set { lblReceivedDate.Text = value; }
        }
        public string CollectionTime
        {
            get { return txtCollectionTime.Text; }
            set { txtCollectionTime.Text = value; }
        }
        public string SiteName
        {
            get { return txtSiteName.Text; }
            set { txtSiteName.Text = value; }
        }
        public string ContactPerson
        {
            get { return cmbContactPerson.Text; }
            set { cmbContactPerson.Text = value; }
        }
        public string ContactPersonId
        {
            get { return cmbContactPerson.SelectedItem.Value; }
            set { cmbContactPerson.SelectedValue = value; }
        }
        public string Building
        {
            get { return cmbBuilding.Text; }
            set { cmbBuilding.SelectedValue = value; }
        }
        public string EnquiryNo
        {
            get { return txtEnquiryNo.Text; }
            set { txtEnquiryNo.Text = value; }
        }
        public string ContactNo
        {
            get { return txtContactNo.Text; }
            set { txtContactNo.Text = value; }
        }
        public string RecordNo
        {
            get { return txtRecordNo.Text; }
            set { txtRecordNo.Text = value; }
        }
        public string EmailId
        {
            get { return txtEmailId.Text; }
            set { txtEmailId.Text = value; }
        }
        public string ReferenceNo
        {
            get { return txtReferenceNo.Text; }
            set { txtReferenceNo.Text = value; }
        }
        public string BillNo
        {
            get { return txtBillNo.Text; }
            set { txtBillNo.Text = value; }
        }
        public string ProformaInvoiceNo
        {
            get { return txtProformaInvoiceNo.Text; }
            set { txtProformaInvoiceNo.Text = value; }
        }
        public string WorkOrder
        {
            get { return txtWorkOrder.Text; }
            set { txtWorkOrder.Text = value; }
        }
        //public string Discount
        //{
        //    get { return txtDiscount.Text; }
        //    set { txtDiscount.Text = value; }
        //}
        public string Charges
        {
            get { return txtCharges.Text; }
            set { txtCharges.Text = value; }
        }
        public string TotalQty
        {
            get { return txtTotalQty.Text; }
            set { txtTotalQty.Text = value; }
        }
        public string Subsets
        {
            get { return txtSubsets.Text; }
            set { txtSubsets.Text = value; }
        }
        public bool OtherClient
        {
            get { return chkOtherClient.Checked; }
            set { chkOtherClient.Checked = value; }
        }
        public string TestReqFormNo
        {
            get { return lblTestReqFormNo.Text; }
            set { lblTestReqFormNo.Text = value; }
        }
        //public bool ProposalRateMatch
        //{
        //    get { return //chkPropRateMatch.Checked; }
        //    set { //chkPropRateMatch.Checked = value; }
        //}
        public string POFileName
        {
            get { return lblPOFileName.Text; }
            set { lblPOFileName.Text = value; }
        }
        protected void cmbContactPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbContactPerson.SelectedIndex > 0)
            {
                var contactp = dc.Contact_View(Convert.ToInt32(cmbContactPerson.SelectedValue), 0, 0, "");
                foreach (var cont in contactp)
                {
                    txtContactNo.Text = cont.CONT_ContactNo_var;
                    txtEmailId.Text = cont.CONT_EmailID_var;
                }
            }
            else
            {
                txtContactNo.Text = "";
                txtEmailId.Text = "";
            }
        }

        protected void chkOtherClient_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOtherClient.Checked == true)
            {
                txtBillNo.ReadOnly = false;
            }
            else
            {
                txtBillNo.ReadOnly = true;
                txtBillNo.Text = "";
            }
        }

        protected void lnkViewProposal_Click(object sender, EventArgs e)
        {
            if (ddlEnquiryList.SelectedValue != "")
            {
                ////if (rbProposalApp.Checked)
                ////{
                ////    bool notRegClient = false;
                ////    var result = dc.EnquiryApp_View(Convert.ToInt32(enqNo), -1);
                ////    foreach (var item in result)
                ////    {
                ////        if (item.ENQ_CONT_Id == 0)
                ////            notRegClient = true;
                ////    }

                ////    rpt.ProposalApp_PDF(Convert.ToInt32(enqNo), 1, Convert.ToInt32(arg[2]), false, true, "View", lblProposalNo.Text, notRegClient);
                ////}
                ////else
                ////    rpt.Proposal_PDF(Convert.ToInt32(enqNo), 0, Convert.ToInt32(arg[2]), Convert.ToBoolean(arg[1]), "View", lblProposalNo.Text);

                string StrEnqNo = "0";
                if (Convert.ToInt32(ddlEnquiryList.SelectedValue) == 0)
                {
                    StrEnqNo = txtEnquiryNo.Text;
                }
                else
                {
                    StrEnqNo = ddlEnquiryList.SelectedValue.ToString();
                }
                var proposal = dc.Proposal_View_EnquiryWise(Convert.ToInt32(StrEnqNo)).ToList();
                foreach (var prop in proposal)
                {
                    PrintPDFReport rpt = new PrintPDFReport();
                    rpt.Proposal_PDF(Convert.ToInt32(StrEnqNo), 0, prop.Proposal_Id, false, "View", prop.Proposal_No);
                    break;
                }                
            }
        }

        protected void lnkUploadPO_Click(object sender, EventArgs e)
        {
            if (FileUploadPO.HasFile == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('No file available..');", true);
            }
            else if (FileUploadPO.HasFile == true)
            {
                string filename = "";
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                filename = Path.GetFileName(FileUploadPO.PostedFile.FileName);
                string ext = Path.GetExtension(filename);
                string filePath = "D:/POFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";
                if (!Directory.Exists(@filePath))
                    Directory.CreateDirectory(@filePath);
                filePath += Path.GetFileName(filename);
                FileUploadPO.PostedFile.SaveAs(filePath);
                
                //dc.Bill_Update_POFile(billid, filename);
                lblPOFileName.Text = filename;
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Uploaded Successfully !');", true);
            }
        }

        protected void lnkDownloadFile_Click(object sender, EventArgs e)
        {
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            string filePath = "D:/POFiles/";
            if (cnStr.ToLower().Contains("mumbai") == true)
                filePath += "Mumbai/";
            else if (cnStr.ToLower().Contains("nashik") == true)
                filePath += "Nashik/";
            else if (cnStr.ToLower().Contains("metro") == true)
                filePath += "Metro/";
            else
                filePath += "Pune/";

            filePath += lblPOFileName.Text;
            if (File.Exists(@filePath))
            {
                HttpResponse res = HttpContext.Current.Response;
                res.Clear();
                res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                res.ContentType = "application/octet-stream";
                res.WriteFile(filePath);
                res.Flush();
                res.End();
            }
        }
    }
}