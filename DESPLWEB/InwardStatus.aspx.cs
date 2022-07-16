using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class InwardStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Inward Status";
                getCurrentDate();
                LoadInwardType();
                rdnPending.Checked = true;

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
                        ddl_InwardTestType.SelectedValue = arrIndMsg[1].ToString().Trim();
                        if (ddl_InwardTestType.SelectedValue == "GT")
                        {
                            chkGTGenerateBill.Visible = true;
                            chkGTGenerateRABill.Visible = true;
                        }
                        BindRecords();
                    }
                }
            }

        }
        public void getCurrentDate()
        {
            txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
            txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        bool prntLabsheet = false;
        protected void grdModifyInward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Control ctrl = e.CommandSource as Control;
            string[] arg = new string[3];
            arg = Convert.ToString(e.CommandArgument).Split(';');
            clsData cd = new clsData();
            string recordType = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            int ReferenceNo = Convert.ToInt32(arg[2]);            
            DateTime CollectDate = Convert.ToDateTime(arg[3]);
            bool withoutBillFlag = Convert.ToBoolean(arg[4]);
            if (e.CommandName == "ApproveInward")
            {                
                if (withoutBillFlag == true && lblWithoutBillRight.Text == "False")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('This is without bill inward, required to approve by authorized person. ');", true);
                }
                else if (withoutBillFlag == true && lblWithoutBillRight.Text == "True" && txtReason.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please enter reason for inward without bill approval');", true);
                    txtReason.Focus();
                }
                else
                {
                    dc.InwardStatus_View(ReferenceNo, "", null, null, 1, 1, "", 0);
                    var rslt = dc.InwardStatus_View(0, "", null, null, 1, 2, recordType, RecordNo);
                    if (withoutBillFlag == true)
                    {
                        dc.WithoutBill_Update_Reason(RecordNo, recordType, Convert.ToInt32(Session["LoginId"]), txtReason.Text.Trim());
                    }
                    //sendProformaInvoiveMail(ReferenceNo);
                    CRLimitExceedEmail(ReferenceNo.ToString(), ddl_InwardTestType.SelectedItem.Text);                    
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record has been successfully approved.');", true);
                    BindRecords();                    
                }
            }
            if (e.CommandName == "PrintInward" || e.CommandName == "PrintLabSheet")
            {
                string fileName = "";
                if (e.CommandName == "PrintLabSheet")
                {
                    prntLabsheet = true;
                    fileName = "LabSheet";
                }
                else
                {
                    prntLabsheet = false;
                    fileName = "Inward";
                }
                fileName = recordType + "_" + fileName;
                //string reportPath;
                string reportStr = "";
                //StreamWriter sw;
                InwardReport InwdRpt = new InwardReport();
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);

                switch (recordType)
                {
                    case "AAC":
                        reportStr = InwdRpt.getDetailReportAACInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "AGGT":
                        reportStr = InwdRpt.getDetailReportAGGT(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "BT-":
                        reportStr = InwdRpt.getDetailReportBrickInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "CCH":
                        reportStr = InwdRpt.getDetailReportCementChemicalInwd(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "CEMT":
                        reportStr = InwdRpt.getDetailReportCementInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "CR":
                        reportStr = InwdRpt.getDetailReportCR(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "CT":
                        if (prntLabsheet == true)
                            reportStr = InwdRpt.getDetailReportCubeInwardLabSheet(RecordNo, ReferenceNo);
                        else
                            reportStr = InwdRpt.getDetailReportCubeInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "FLYASH":
                        reportStr = InwdRpt.getDetailReportFLYASH(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "GGBSCH":
                        reportStr = InwdRpt.getDetailReportGgbsChemicalInwd(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "GGBS":
                        reportStr = InwdRpt.getDetailReportGgbsInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "GT":
                        reportStr = InwdRpt.getDetailReportSoilInvestigation(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "NDT":
                        reportStr = InwdRpt.getDetailReportNDT(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "PILE":
                        reportStr = InwdRpt.getDetailReportPileInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "PT":
                        reportStr = InwdRpt.getDetailReportPavmentBlockInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "SO":
                        reportStr = InwdRpt.getDetailReportSoilTestInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "SOLID":
                        reportStr = InwdRpt.getDetailReportMasonaryInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "ST":
                        reportStr = InwdRpt.getDetailReportSteelInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "STC":
                        reportStr = InwdRpt.getDetailReportSteelChemicalTestInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "TILE":
                        reportStr = InwdRpt.getDetailReportTileInward(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "WT":
                        reportStr = InwdRpt.getDetailReportWT(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "OT":
                        reportStr = InwdRpt.getDetailReportOT(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "RWH":
                        reportStr = InwdRpt.getDetailReportRWH(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "CORECUT":
                        reportStr = InwdRpt.getDetailReportCoreCutting(RecordNo, ReferenceNo, prntLabsheet);
                        break;
                    case "MF":
                        reportStr = InwdRpt.getDetailReportMF(RecordNo, ReferenceNo, prntLabsheet);
                        break;

                }
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

                //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //LinkButton lnkApprove = (LinkButton)grdModifyInward.Rows[gvr.RowIndex].FindControl("lnkApprove");
                //if (lblApprRight.Text == "True")
                //{
                //    lnkApprove.Enabled = true;
                //}
                PrintHTMLReport rpt = new PrintHTMLReport();
                rpt.DownloadHtmlReport(fileName, reportStr);
            }

            if (e.CommandName == "ModifyInward")
            {
                string strURLWithData = "";
                if (recordType == "AAC")
                {

                    strURLWithData = "AAC_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "AAC", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "AGGT")
                {
                    strURLWithData = "Aggregate_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "AGGT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "OT")
                {

                    strURLWithData = "Other_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "OT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "BT-")
                {

                    strURLWithData = "Brick_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "BT-", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "CCH")
                {

                    strURLWithData = "CementChemical_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "CCH", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "CR")
                {

                    strURLWithData = "Core_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "CR", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "FLYASH")
                {

                    strURLWithData = "FlyAsh_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "FLYASH", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "NDT")
                {

                    strURLWithData = "NDT_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "NDT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "PILE")
                {

                    strURLWithData = "Pile_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "PILE", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "TILE")
                {

                    strURLWithData = "Tile_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "TILE", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "WT")
                {

                    strURLWithData = "Water_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "WT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "PT")
                {

                    strURLWithData = "Pavement_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "PT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "CEMT")
                {

                    strURLWithData = "Cement_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "CEMT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "SOLID")
                {

                    strURLWithData = "Solid_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "SOLID", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "SO")
                {

                    strURLWithData = "Soil_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "SO", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);

                }
                else if (recordType == "GT")
                {
                    if (chkGTGenerateRABill.Checked == true)
                        Session["RABill"] = "True";
                    else
                        Session["RABill"] = null;
                    strURLWithData = "SoilInvestigation_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "GT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "RWH")
                {

                    strURLWithData = "RWH_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "RWH", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "CT")
                {

                    strURLWithData = "Cube_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "CT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "ST")
                {

                    strURLWithData = "Steel_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "ST", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "STC")
                {

                    strURLWithData = "STC_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "STC", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "CORECUT")
                {

                    strURLWithData = "CoreCutting_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "CORECUT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "MF")
                {

                    strURLWithData = "MixDesign_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "MF", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "GGBSCH")
                {

                    strURLWithData = "GgbsChemical_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "GGBSCH", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
                else if (recordType == "GGBS")
                {

                    strURLWithData = "Ggbs_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "GGBS", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);
                }
            }
            if (e.CommandName == "EnterEditGTSample")
            {
                if (recordType == "GT")
                {
                    string strURLWithData = "GTSamplesDetails.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "GT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);

                }
            }
            if (e.CommandName == "EnterEditGTSummary")
            {
                if (recordType == "GT")
                {

                    string strURLWithData = "GTSummary.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "GT", RecordNo, ReferenceNo, "", "Edit"));
                    Response.Redirect(strURLWithData);

                }
            }
        }
        public void CRLimitExceedEmail(string referenceNo, string inwardType)
        {
            bool addMailId = true;
            string mailIdTo = "", mailIdCc = "", tempMailId = "", strAllMailId = "", strMEName = "", strMEContactNo = "", strEnqDate = "";
            string strOutstndingAmt = "0", strEnqAmount = "0", strEnqNo = "";
            int siteRouteId = 0, siteMeId = 0, crLimtFlag = 0;
            var enquiry = dc.MailIdForCRLExceed_View(0, Convert.ToInt32(referenceNo), 0, "", "InwardApproval", false).ToList();//chk  crlimt exceeded 
            if (enquiry.Count == 0)
                enquiry = dc.MailIdForCRLExceed_View(0, Convert.ToInt32(referenceNo), 0, "", "InwardApproval", true).ToList();//chk  crlimt is not exceeded 

            if (enquiry.Count > 0)
            {
                foreach (var enq in enquiry)
                {
                    strMEName = enq.MEName;
                    strMEContactNo = enq.MEContactNo;
                    if (enq.MEContactNo == null || enq.MEContactNo == "")
                        strMEContactNo = enq.MEMailId;
                    crLimtFlag = enq.crExceededStatus;
                    strEnqDate = Convert.ToDateTime(enq.ENQ_Date_dt).ToString("dd/MM/yyyy");
                    if (Convert.ToString(enq.CL_BalanceAmt_mny) != "" && Convert.ToString(enq.CL_BalanceAmt_mny) != null)
                        strOutstndingAmt = Convert.ToDecimal(enq.CL_BalanceAmt_mny).ToString("0.00");
                    strEnqAmount = Convert.ToString(enq.PROINV_NetAmt_num);
                    strEnqNo = Convert.ToString(enq.ENQ_Id);                    
                    if (Convert.ToString(enq.SITE_Route_Id) != "" && Convert.ToString(enq.SITE_Route_Id) != null)
                        siteRouteId = Convert.ToInt32(enq.SITE_Route_Id);
                    if (Convert.ToString(enq.MEId) != "" && Convert.ToString(enq.MEId) != null)
                        siteMeId = Convert.ToInt32(enq.MEId);

                    tempMailId = enq.INWD_EmailId_var.Trim();
                    if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                        tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                        tempMailId.ToLower().Contains(".") == false)
                    {
                        addMailId = false;
                    }
                    if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                    {
                        if (mailIdTo != "")
                            mailIdTo += ",";
                        mailIdTo += tempMailId;
                        strAllMailId += "," + tempMailId + ",";
                    }

                    //addMailId = true;
                    //tempMailId = enq.CL_AccEmailId_var.Trim();
                    //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    //    tempMailId.ToLower().Contains(".") == false)
                    //{
                    //    addMailId = false;
                    //}
                    //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                    //{
                    //    if (mailIdTo != "")
                    //        mailIdTo += ",";
                    //    mailIdTo += tempMailId;
                    //    strAllMailId += "," + tempMailId + ",";
                    //}

                    //addMailId = true;
                    //tempMailId = enq.CL_DirectorEmailID_var.Trim();
                    //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    //    tempMailId.ToLower().Contains(".") == false)
                    //{
                    //    addMailId = false;
                    //}
                    //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                    //{
                    //    if (mailIdTo != "")
                    //        mailIdTo += ",";
                    //    mailIdTo += tempMailId;
                    //    strAllMailId += "," + tempMailId + ",";
                    //}

                    //addMailId = true;
                    //tempMailId = enq.CL_EmailID_var.Trim();
                    //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    //    tempMailId.ToLower().Contains(".") == false)
                    //{
                    //    addMailId = false;
                    //}
                    //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                    //{
                    //    if (mailIdTo != "")
                    //        mailIdTo += ",";
                    //    mailIdTo += tempMailId;
                    //    strAllMailId += "," + tempMailId + ",";
                    //}

                    //addMailId = true;
                    //tempMailId = enq.SITE_EmailID_var.Trim();
                    //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                    //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                    //    tempMailId.ToLower().Contains(".") == false)
                    //{
                    //    addMailId = false;
                    //}
                    //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                    //{
                    //    if (mailIdTo != "")
                    //        mailIdTo += ",";
                    //    mailIdTo += tempMailId;
                    //    strAllMailId += "," + tempMailId + ",";
                    //}

                    addMailId = true;
                    tempMailId = enq.EnteredByMailId.Trim();
                    if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                        tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                        tempMailId.ToLower().Contains(".") == false)
                    {
                        addMailId = false;
                    }
                    if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                    {
                        if (mailIdCc != "")
                            mailIdCc += ",";
                        mailIdCc += tempMailId;
                        strAllMailId += "," + tempMailId + ",";
                    }

                    addMailId = true;
                    tempMailId = enq.MEMailId.Trim();
                    if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
                        tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
                        tempMailId.ToLower().Contains(".") == false)
                    {
                        addMailId = false;
                    }
                    if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
                    {
                        if (mailIdCc != "")
                            mailIdCc += ",";
                        mailIdCc += tempMailId;
                        strAllMailId += "," + tempMailId + ",";
                    }
                }

                if (mailIdTo != "")
                {

                    clsSendMail objMail = new clsSendMail();
                    string mSubject = "", mbody = "", mReplyTo = "";
                    //mailIdTo = "shital.bandal@gmail.com";
                    //mailIdCc = "";
                    mSubject = "Material Collection Confirmation";

                    mbody = "Dear Customer,<br><br>";
                    if (crLimtFlag == 1)//Credit Limit exceeded Client
                        mbody = mbody + "We have collected material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ". Your total outstanding dues are Rs " + strOutstndingAmt + ". Please arrange to make the payment at earliest.Please note that your outstanding payments are exceeding the credit limit set in our ERP hence you will not be able to view reports on our web portal and mobile App until old dues are cleared. If you have already made the payment, please ignore this message .<br><br><br>";
                    else
                        mbody = mbody + "We have collected material against your enquiry no. " + strEnqNo + " on date " + strEnqDate + ".<br><br><br>";
                    mbody = mbody + "<br>&nbsp;";
                    mbody = mbody + "<br>&nbsp;";
                    mbody = mbody + "<br>";
                    mbody = mbody + "Best Regards,";
                    mbody = mbody + "<br>";
                    mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                    try
                    {
                        objMail.SendMail(mailIdTo, mailIdCc, mSubject, mbody, "", mReplyTo);
                        //update enq-outstanding mail count in route table of that ME
                        if (siteRouteId != 0 && siteMeId != 0)
                            dc.Route_Update_OutsandingMailCount(siteRouteId, siteMeId, 1);


                    }
                    catch { }
                }
            }
            else//mail for those whose credit limit is not exceeded
            {

            }
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindRecords();
        }

        public void BindRecords()
        {
            grdModifyInward.Visible = true;
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            string gtBill = "";
            if (ddl_InwardTestType.SelectedValue == "GT" && chkGTGenerateBill.Checked == true)
            {
                gtBill = "GTBill";
            }
            else if (ddl_InwardTestType.SelectedValue == "GT" && chkGTGenerateRABill.Checked == true)
            {
                gtBill = "GTRABill";
            }

            if (rdnPending.Checked)
            {
                grdModifyInward.Columns[0].Visible = true;
                grdModifyInward.Columns[10].Visible = true;

                var Inward = dc.InwardStatus_View(0, ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, 0, 0, gtBill, 0);
                grdModifyInward.DataSource = Inward;
                grdModifyInward.DataBind();
            }
            else if (rdnApproved.Checked)
            {
                grdModifyInward.Columns[0].Visible = false;
                grdModifyInward.Columns[10].Visible = false;

                var Inward = dc.InwardStatus_View(0, ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, 1, 0, gtBill, 0);
                grdModifyInward.DataSource = Inward;
                grdModifyInward.DataBind();
            }
            bool inwardRight = false, inwardApproveRight = false;
            lblApprRight.Text = "False";
            lblWithoutBillRight.Text = "False";
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                if (u.USER_Inward_right_bit == true)
                {
                    inwardRight = true;
                }
                if (u.USER_InwardApprove_right_bit == true)
                {
                    inwardApproveRight = true;
                    lblApprRight.Text = "True";
                }
                if (u.USER_WithoutBill_right_bit == true)
                {
                    lblWithoutBillRight.Text = "True";
                }
            }
            if (grdModifyInward.Rows.Count > 0)
            {
                for (int i = 0; i < grdModifyInward.Rows.Count; i++)
                {
                    LinkButton lnkModifyInward = (LinkButton)grdModifyInward.Rows[i].Cells[5].FindControl("lnkModifyInward");
                    LinkButton lnkApprove = (LinkButton)grdModifyInward.Rows[i].Cells[0].FindControl("lnkApprove");
                    if (rdnPending.Checked == true)
                    {
                        lnkModifyInward.Visible = true;
                        lnkApprove.Visible = true;
                    }
                    else
                    {
                        lnkModifyInward.Visible = false;
                        lnkApprove.Visible = false;
                    }
                    lnkModifyInward.Enabled = false;
                    lnkApprove.Enabled = false;
                    if (inwardRight == true)
                    {
                        lnkModifyInward.Enabled = true;
                    }
                    if (inwardApproveRight == true)
                    {
                        lnkApprove.Enabled = true;
                    }
                }
            }
            if (grdModifyInward.Rows.Count > 0)
            {
                lbl_RecordsNo.Text = "Total Records   :  " + grdModifyInward.Rows.Count;
            }
            else
            {
                lbl_RecordsNo.Text = "Total Records   :  " + "0";
            }
            txtReason.Text = "";
        }

        public void sendProformaInvoiveMail(int RefNo)
        {
            //string proformaInvoiceNo = "", EmailId = "", Rectype = "";
            //var inward = dc.Inward_View(RefNo, 0, "", null, null);
            //foreach (var inwd in inward)
            //{
            //    if (inwd.INWD_PROINV_Id != null && inwd.INWD_PROINV_Id != "" && inwd.INWD_PROINV_Id != "")
            //        proformaInvoiceNo = inwd.INWD_PROINV_Id;
            //    EmailId = inwd.INWD_EmailId_var;
            //    Rectype = inwd.INWD_RecordType_var;
            //}
            ////mail report
            //bool sendMail = true;
            //if (EmailId.Trim() == "" || EmailId.Trim().ToLower() == "na@unknown.com" ||
            //    EmailId.Trim().ToLower() == "na" || EmailId.Trim().ToLower().Contains("na@") == true ||
            //    EmailId.Trim().ToLower().Contains("@") == false || EmailId.Trim().ToLower().Contains(".") == false)
            //{
            //    sendMail = false;
            //}
            //if (IsValidEmailAddress(EmailId.Trim()) == false)
            //{
            //    sendMail = false;
            //}

            ////sendMail = false;
            //if (sendMail == true)
            //{
            //    ProformaInvoiceUpdation Proinv = new ProformaInvoiceUpdation();
            //    Proinv.getProformaInvoicePrintString(proformaInvoiceNo, "Email");
            //    //string reportPath = "C:/temp/Veena/" + Rectype + "_" + RefNo.Replace('/', '_') + ".pdf";
            //    string strFileName = "ProformaInvoice_" + proformaInvoiceNo + ".html";
            //    string reportPath = @"C:/temp/" + strFileName;
            //    if (File.Exists(@reportPath))
            //    {
            //        clsSendMail objMail = new clsSendMail();
            //        string mTo = "", mCC = "", mSubject = "", mbody = "", mReplyTo = "";
            //        //mTo = "shital.bandal@gmail.com";
            //        //mTo = EmailId.Trim();
            //        //mCC = "reports.pune@durocrete.acts-int.com";
            //        mCC = "duroreports.pune@gmail.com";
            //        mSubject = "Proforma invoice for ";
            //        if (Rectype == "AAC")
            //            mSubject += "Autoclaved Aerated Cellular Test Report";
            //        else if (Rectype == "CT")
            //            mSubject += "Concrete Cube Compression test.";
            //        else if (Rectype == "ST")
            //            mSubject += "Physical test report for reinforcement steel";
            //        else if (Rectype == "AGGT")
            //            mSubject += "Aggregate test report";
            //        else if (Rectype == "CEMT")
            //            mSubject += "Cement Test Report";
            //        else if (Rectype == "FLYASH")
            //            mSubject += "FLYASH Test Report";
            //        else if (Rectype == "PILE")
            //            mSubject += "PILE Test Report";
            //        else if (Rectype == "TILE")
            //            mSubject += "TILE Test Report";
            //        else if (Rectype == "PT")
            //            mSubject += "Pavement Test Report";
            //        else if (Rectype == "SOLID")
            //            mSubject += "SOLID Test Report";
            //        else if (Rectype == "BT-")
            //            mSubject += "BRICK Test Report";
            //        else if (Rectype == "MF")
            //            mSubject += "Mix Design Test Report";
            //        else if (Rectype == "STC")
            //            mSubject += "Steel Chemical Test Report";
            //        else if (Rectype == "CCH")
            //            mSubject += "Cement Chemical Test Report";
            //        else if (Rectype == "WT")
            //            mSubject += "Mix Design Test Report";
            //        else
            //            mSubject += "Test Report";

            //        mbody = "Dear Sir/Madam,<br><br>";
            //        mbody = mbody + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please find attached " + mSubject + " <br>";
            //        mbody = mbody + "Please feel free to contact in case of any queries." + " <br><br><br>";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "<br>&nbsp;";
            //        mbody = mbody + "<br>";
            //        mbody = mbody + "Best Regards,";
            //       mbody = mbody + "<br>";
            //        mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.(PUNE)";
            //      //  objMail.SendMail(mTo, mCC, mSubject, mbody, reportPath, mReplyTo);
            //    }
            //}
        }

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }

        private void LoadInwardType()
        {
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            //ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataValueField = "MATERIAL_RecordType_var";
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select---");
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdModifyInward.Visible = false;
            lbl_RecordsNo.Text = "";
            if (ddl_InwardTestType.SelectedValue == "GT")
            {
                chkGTGenerateBill.Visible = true;
                chkGTGenerateRABill.Visible = true;
            }
            else
            {
                chkGTGenerateBill.Visible = false;
                chkGTGenerateRABill.Visible = false;
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdModifyInward.Rows.Count > 0 && grdModifyInward.Visible == true)
            {
                //string reportPath;
                string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                reportStr = RptInwardStatus();
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.DownloadHtmlReport("InwardStatus", reportStr);
            }
        }

        protected string RptInwardStatus()
        {
            string reportStr = "", mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Inward Status </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            string Inwardtype = string.Empty;
            if (rdnApproved.Checked)
            {
                Inwardtype = "Approved";
            }
            if (rdnPending.Checked)
            {
                Inwardtype = "Pending";
            }
            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Mat. Recd  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
                "<td width='15%' align=left valign=top height=19><font size=2><b> Total No of Records  </b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + grdModifyInward.Rows.Count + "</font></td>" +
               "</tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Inward Type  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + ddl_InwardTestType.SelectedItem.Text + "  -  " + " ( " + Inwardtype + " )" + "</font></td>" +
               "</tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";

            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Record Type </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Record No  </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Reference No </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Collection Date </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Received Date </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>  Contact No.  </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>  Received By  </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>  Route  </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>  Marketing Executive  </b></font></td>";

            for (int i = 0; i < grdModifyInward.Rows.Count; i++)
            {
                mySql += "<tr>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[1].Text + "</font></td>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[2].Text + "</font></td>";
                mySql += "<td width =10% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[3].Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[4].Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[5].Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[6].Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[7].Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[8].Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + grdModifyInward.Rows[i].Cells[9].Text + "</font></td>";
                mySql += "</tr>";
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;

        }

    }
}
