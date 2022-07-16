using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class ApproveReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                bool approveRight = false;
                var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_RptApproval_right_bit == true)
                        approveRight = true;
                }
                if (approveRight == true)
                {
                    Label lblheading = (Label)Master.FindControl("lblheading");
                    lblheading.Text = "Approve Report";
                    LoadInwardType();
                    txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    chk_Sign.Checked = true;
                    optPending.Checked = true;
                    
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        
        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select---");
        }
        protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (optApproved.Checked == true)
                {
                    LinkButton lnkApproveReport = (LinkButton)e.Row.FindControl("lnkApproveReport");
                    lnkApproveReport.Visible = false;
                    //TextBox txtEmailId = (TextBox)e.Row.FindControl("txtEmailId");
                    //txtEmailId.ReadOnly = true;
                }
            }
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdReports.Visible = true;
            DisplayReports();
        }

        public void DisplayReports()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            byte rptStatus = 3;
            if (optPending.Checked == true)
                rptStatus = 3;
            else if (optApproved.Checked == true)
                rptStatus = 4;
            string finalStatus = "";
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "Final")
                finalStatus = " " + ddlMF.SelectedValue;
            if (ddl_InwardTestType.SelectedItem.Text == "Other Testing")
            {
                var Inward = dc.ReportStatus_View_OtherTesting(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, rptStatus, ClientId, -1).ToList();
                grdReports.DataSource = Inward;
                grdReports.DataBind();
            }
            else if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                var Inward = dc.ReportStatus_View_MixDesign(ddl_InwardTestType.SelectedItem.Text + finalStatus, Fromdate, Todate, rptStatus, ClientId).ToList();
                grdReports.DataSource = Inward;
                grdReports.DataBind();
            }
            else if (ddl_InwardTestType.SelectedItem.Text == "GGBS Chemical Testing" || ddl_InwardTestType.SelectedItem.Text == "GGBS Testing")
            {
                var Inward = dc.ReportStatus_View_GGBSTesting(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, rptStatus, ClientId, -1).ToList();
                grdReports.DataSource = Inward;
                grdReports.DataBind();
            }
            else
            {
                var Inward = dc.ReportStatus_View_All(ddl_InwardTestType.SelectedItem.Text + finalStatus, Fromdate, Todate, rptStatus, ClientId,0, -1).ToList();
                grdReports.DataSource = Inward;
                grdReports.DataBind();
            }
            lbl_RecordsNo.Text = "Total Records   :  " + grdReports.Rows.Count;
        }

        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[3];
            arg = strReportDetails.Split(';');

            string Recordtype = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            string ReferenceNo = Convert.ToString(arg[2]);
            clsData cd = new clsData(); int siteCRbypssBit = 0;
            if (e.CommandName == "ApproveReport")
            {
                ApproveReports(Recordtype, ReferenceNo, RecordNo); //, txtEmailId.Text, lblContactNo.Text);
                #region SMS
                // ( if bal >cr limit & crlimit > 0) or client_block=1 then exceed limit=1
                var res = dc.SMSDetailsForReportApproval_View(RecordNo, Recordtype).ToList();
                if (res.Count > 0)
                {
                    cd.sendInwardReportMsg(Convert.ToInt32(res.FirstOrDefault().ENQ_Id), Convert.ToString(res.FirstOrDefault().ENQ_Date_dt), Convert.ToInt32(res.FirstOrDefault().SITE_MonthlyBillingStatus_bit).ToString(), res.FirstOrDefault().crLimitExceededStatus.ToString(), res.FirstOrDefault().BILL_NetAmt_num.ToString(), ReferenceNo, Convert.ToString(res.FirstOrDefault().INWD_ContactNo_var), "Report");
                }
                #endregion
                #region Email
                SendMailOnReportApproval(ReferenceNo);
                #endregion

                //update MISCRLApprStatus to 1 if SITE_CRBypass_bit is 1
                // siteCRbypssBit = cd.getSITECRBypassBit(Recordtype, RecordNo);
                // client wise bypass
                siteCRbypssBit = cd.getClientCrlBypassBit(Recordtype, RecordNo);
                if (siteCRbypssBit == 1)
                    dc.MISDETAIL_Update_CRLimitBit(ReferenceNo, Recordtype);


                //CRLimitExceedEmail(ReferenceNo, RecordNo.ToString(), Recordtype, ddl_InwardTestType.SelectedItem.Text);
                DisplayReports();
            }
            else if (e.CommandName == "ViewReport")
            {
                //PrintReport(Recordtype, ReferenceNo, "View");
                PrintPDFReport rpt = new PrintPDFReport();
                rpt.PrintSelectedReport(Recordtype, ReferenceNo, "View", "", "", ddlMF.SelectedValue, "", "", "", "");
            }
            else if (e.CommandName == "UpdateContactDetails")
            {
                ModalPopupExtender1.Show();
                lnkSaveContactDetail.Enabled = true;
                lblContactMessage.Visible = false;
                var inward = dc.Inward_View(0, RecordNo, Recordtype, null, null);
                foreach (var inwd in inward)
                {
                    lblRecordType.Text = Recordtype;
                    lblRecordNo.Text = RecordNo.ToString();
                    lblContactId.Text = inwd.INWD_CONT_Id.ToString();
                    txtContactName.Text = inwd.ContactName;
                    txtContactNo.Text = inwd.INWD_ContactNo_var;
                    txtContactEmail.Text = inwd.INWD_EmailId_var;
                }
            }
        }
        public void SendMailOnReportApproval(string referenceNo)
        {
            bool sendMail = true;
            string mTo = "", mCC = "", mSubject = "", mbody = "";
            var inward = dc.Inward_View(Convert.ToInt32(referenceNo.Split('/')[0]), 0, "", null, null);
            foreach (var inwd in inward)
            {
                bool validMailId = true;
                string EmailId = inwd.CL_AccEmailId_var.Replace("/", ",");                
                if (EmailId.Trim() == "" || EmailId.Trim().ToLower() == "na@unknown.com" ||
                    EmailId.Trim().ToLower() == "na" || EmailId.Trim().ToLower().Contains("na@") == true ||
                    EmailId.Trim().ToLower().Contains("@") == false || EmailId.Trim().ToLower().Contains(".") == false)
                {
                    validMailId = false;
                }
                if (IsValidEmailAddress(EmailId.Trim()) == false)
                {
                    validMailId = false;
                }
                if (validMailId == true)
                {
                    mTo = EmailId;
                }
                validMailId = true;
                EmailId = inwd.INWD_EmailId_var.Replace("/", ","); ;
                if (EmailId.Trim() == "" || EmailId.Trim().ToLower() == "na@unknown.com" ||
                    EmailId.Trim().ToLower() == "na" || EmailId.Trim().ToLower().Contains("na@") == true ||
                    EmailId.Trim().ToLower().Contains("@") == false || EmailId.Trim().ToLower().Contains(".") == false)
                {
                    validMailId = false;
                }
                if (IsValidEmailAddress(EmailId.Trim()) == false)
                {
                    validMailId = false;
                }
                if (validMailId == true)
                {
                    if (mTo != "")
                        mTo += "," + EmailId;
                    else
                        mTo = EmailId;
                }
                //validMailId = true;
                //EmailId = inwd.CL_EmailID_var.Replace("/", ","); ;
                //if (EmailId.Trim() == "" || EmailId.Trim().ToLower() == "na@unknown.com" ||
                //    EmailId.Trim().ToLower() == "na" || EmailId.Trim().ToLower().Contains("na@") == true ||
                //    EmailId.Trim().ToLower().Contains("@") == false || EmailId.Trim().ToLower().Contains(".") == false)
                //{
                //    validMailId = false;
                //}
                //if (IsValidEmailAddress(EmailId.Trim()) == false)
                //{
                //    validMailId = false;
                //}
                //if (validMailId == true)
                //{
                //    mCC = EmailId;
                //}
                //if (mTo == "")
                //    mTo = mCC;                
            }
            if (mTo == "" && mCC == "")
                sendMail = false;

            //mTo = "shital.bandal@gmail.com";
            //mCC = "";
            if (sendMail == true)
            {
                mSubject = "Report Update.";                
                mbody = "Dear Sir/Madam,<br><br>";
                mbody = mbody + "This is to inform you that Test report No " + referenceNo + " for the material " + ddl_InwardTestType.SelectedItem.Text.Replace("Testing", "") + " tested at our laboratory is <br>";
                mbody = mbody + "ready. Soft copy of report will get emailed to your authorised email ID immediately after you <br> ";
                mbody = mbody + "approve the bill on our web portal . <br><br>";
                mbody = mbody + "Soft copy of report has been emailed to your authorised email ID. <br><br>";
                mbody = mbody + "You can also download the report and view it on your mobile through the Durocrete APP. All the <br>";
                mbody = mbody + "reports of your projects tested at Durocrete can be accessed and downloaded by you through our <br>";
                mbody = mbody + "ERP up to 10 years from the date of report.  <br><br>";
                mbody = mbody + "Method to view report and approve bill <br><br>";
                mbody = mbody + "&nbsp;&nbsp;&nbsp; 1.	Log in to website www.durocrete.in <br>";
                mbody = mbody + "&nbsp;&nbsp;&nbsp; 2.	From ‘User login’ and select ‘Existing User Login’. <br>";
                mbody = mbody + "&nbsp;&nbsp;&nbsp; 3.	Select laboratory location, enter login credentials provided to you.<br>";
                mbody = mbody + "&nbsp;&nbsp;&nbsp; 4.	Using the tab ‘Update Mobile Users’ add authorised persons to view report / approve bill. <br>";
                mbody = mbody + "&nbsp;&nbsp;&nbsp; 5.	Select ‘Bill Approval’ and you can view and approve bills. <br>";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "Our customer support teams will assist you in case of difficulty. You can also call our toll free no <br>";
                mbody = mbody + "18001206465. Look forward to serving you at all times ." + " <br><br><br>";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "Best Regards,";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "Customer Support ";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>&nbsp;";
                //mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.(PUNE)";
                clsSendMail objMail = new clsSendMail();
                try
                {
                    objMail.SendMail(mTo, mCC, mSubject, mbody, "", "");
                }
                catch { }
            }
            
        }
        // Not in use
        //public void CRLimitExceedEmail(string referenceNo, string recordNo, string recordType, string inwardType)
        //{
        //    bool addMailId = true;
        //    string mailIdTo = "", mailIdCc = "", tempMailId = "", strAllMailId = "", strMEName = "", strMEContactNo = "", strEnqDate = "";
        //    string strOutstndingAmt = "0", strEnqAmount = "0", strEnqNo = "", tollFree="";
        //    int siteRouteId = 0, siteMeId = 0, crLimtFlag = 0;
        //    clsData cd = new clsData();
        //    var enquiry = dc.MailIdForCRLExceed_View(0, 0, Convert.ToInt32(recordNo), recordType, "ReportApproval",false).ToList();//chk  crlimt is exceeded
        //    if (enquiry.Count == 0)
        //        enquiry = dc.MailIdForCRLExceed_View(0, 0,Convert.ToInt32(recordNo), recordType, "ReportApproval", true).ToList();//chk  crlimt is not exceeded 

        //    if (enquiry.Count > 0)
        //    {  
        //        foreach (var enq in enquiry)
        //        {
        //            strMEName = enq.MEName;
        //            strMEContactNo = enq.MEContactNo;
        //            if (enq.MEContactNo == null || enq.MEContactNo == "")
        //                strMEContactNo = enq.MEMailId;
        //            crLimtFlag = enq.crExceededStatus;
        //            strEnqDate = Convert.ToDateTime(enq.ENQ_Date_dt).ToString("dd/MM/yyyy");
        //            if (Convert.ToString(enq.CL_BalanceAmt_mny) != "" && Convert.ToString(enq.CL_BalanceAmt_mny) != null)
        //                strOutstndingAmt = Convert.ToDecimal(enq.CL_BalanceAmt_mny).ToString("0.00");
        //            strEnqAmount = Convert.ToString(enq.PROINV_NetAmt_num);                    
        //            strEnqNo = Convert.ToString(enq.ENQ_Id);
        //            tempMailId = enq.INWD_EmailId_var.Trim();
        //           if (PrintPDFReport.cnStr.ToLower().Contains("mumbai") == true)
        //                tollFree = "18001214070";
        //            else if (PrintPDFReport.cnStr.ToLower().Contains("nashik") == true)
        //                tollFree = "7720006754";
        //            else
        //                tollFree = "18001206465";

        //            if (Convert.ToString(enq.SITE_Route_Id) != "" && Convert.ToString(enq.SITE_Route_Id) != null)
        //                siteRouteId = Convert.ToInt32(enq.SITE_Route_Id);
        //            if (Convert.ToString(enq.MEId) != "" && Convert.ToString(enq.MEId) != null)
        //                siteMeId = Convert.ToInt32(enq.MEId);

        //            if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
        //                tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
        //                tempMailId.ToLower().Contains(".") == false)
        //            {
        //                addMailId = false;
        //            }
        //            if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
        //            {
        //                if (mailIdTo != "")
        //                    mailIdTo += ",";
        //                mailIdTo += tempMailId;
        //                strAllMailId += "," + tempMailId + ",";
        //            }

        //            addMailId = true;
        //            tempMailId = enq.CL_AccEmailId_var.Trim();
        //            if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
        //                tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
        //                tempMailId.ToLower().Contains(".") == false)
        //            {
        //                addMailId = false;
        //            }
        //            if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
        //            {
        //                if (mailIdTo != "")
        //                    mailIdTo += ",";
        //                mailIdTo += tempMailId;
        //                strAllMailId += "," + tempMailId + ",";
        //            }

        //            addMailId = true;
        //            tempMailId = enq.CL_DirectorEmailID_var.Trim();
        //            if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
        //                tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
        //                tempMailId.ToLower().Contains(".") == false)
        //            {
        //                addMailId = false;
        //            }
        //            if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
        //            {
        //                if (mailIdTo != "")
        //                    mailIdTo += ",";
        //                mailIdTo += tempMailId;
        //                strAllMailId += "," + tempMailId + ",";
        //            }

        //            addMailId = true;
        //            tempMailId = enq.CL_EmailID_var.Trim();
        //            if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
        //                tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
        //                tempMailId.ToLower().Contains(".") == false)
        //            {
        //                addMailId = false;
        //            }
        //            if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
        //            {
        //                if (mailIdTo != "")
        //                    mailIdTo += ",";
        //                mailIdTo += tempMailId;
        //                strAllMailId += "," + tempMailId + ",";
        //            }

        //            addMailId = true;
        //            //tempMailId = enq.SITE_EmailID_var.Trim();
        //            //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
        //            //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
        //            //    tempMailId.ToLower().Contains(".") == false)
        //            //{
        //            //    addMailId = false;
        //            //}
        //            //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
        //            //{
        //            //    if (mailIdTo != "")
        //            //        mailIdTo += ",";
        //            //    mailIdTo += tempMailId;
        //            //    strAllMailId += "," + tempMailId + ",";
        //            //}

        //            //addMailId = true;
        //            //tempMailId = enq.EnteredByMailId.Trim();
        //            //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
        //            //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
        //            //    tempMailId.ToLower().Contains(".") == false)
        //            //{
        //            //    addMailId = false;
        //            //}
        //            //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
        //            //{
        //            //    if (mailIdCc != "")
        //            //        mailIdCc += ",";
        //            //    mailIdCc += tempMailId;
        //            //    strAllMailId += "," + tempMailId + ",";
        //            //}

        //            //addMailId = true;
        //            //tempMailId = enq.MEMailId.Trim();
        //            //if (tempMailId == "" || tempMailId.ToLower() == "na@unknown.com" || tempMailId.ToLower() == "na" ||
        //            //    tempMailId.ToLower().Contains("na@") == true || tempMailId.ToLower().Contains("@") == false ||
        //            //    tempMailId.ToLower().Contains(".") == false)
        //            //{
        //            //    addMailId = false;
        //            //}
        //            //if (addMailId == true && strAllMailId.Contains("," + tempMailId + ",") == false)
        //            //{
        //            //    if (mailIdCc != "")
        //            //        mailIdCc += ",";
        //            //    mailIdCc += tempMailId;
        //            //    strAllMailId += "," + tempMailId + ",";
        //            //}
        //        }
             
        //        if (mailIdTo != "")
        //        {



        //            //clsSendMail objMail = new clsSendMail();
        //            //string mSubject = "", mbody = "", mReplyTo = "";
        //            ////mailIdTo = "shital.bandal@gmail.com";
        //            ////mailIdCc = "";
        //            //mSubject = "Report Confirmation";
        //            ////New
        //            ////mbody = "Dear Customer,<br><br>";

        //            //if (crLimtFlag == 1)//Credit Limit exceeded Client
        //            //    mbody = mbody + "Dear Customer,<br><br>We have tested material against your enquiry no. " + strEnqNo + "(Reference No : " + referenceNo + ") on date " + strEnqDate + ", total cost of testing is Rs. " + strEnqAmount + ". The report is ready with us. Your total outstanding dues are Rs " + strOutstndingAmt + ".<br>Please arrange to make the payment for the testing to access the report on our Durocrete Mobile App. The copy of the report will be emailed to you on your registered email id once payment is made. For any assistance please contact on our tollfree no. "+ tollFree + ".Please ignore if payment already done.<br><br><br>";
        //            //else
        //            //    mbody = mbody + "Dear Customer,<br><br>We have tested material against your enquiry no. " + strEnqNo + "(Reference No : " + referenceNo + ") on date " + strEnqDate + ", total cost of testing is Rs. " + strEnqAmount + ". The report is ready with us, soft copy of the report has been emailed to you on your registered email id. You can view this report with help of Durocrete APP on your mobile phone.You can also download the report from our website www.durocrete.in. For any assistance please contact on our tollfree no. " + tollFree + "<br><br><br>";

        //            //mbody = mbody + "<br>&nbsp;";
        //            //mbody = mbody + "<br>&nbsp;";
        //            //mbody = mbody + "<br>";
        //            //mbody = mbody + "Best Regards.";
        //            //mbody = mbody + "<br>";
        //            //mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                  
        //            try
        //            {
        //                //objMail.SendMail(mailIdTo, mailIdCc, mSubject, mbody, "", mReplyTo);
        //                dc.Inward_Update_CRLExceedMailStatus(Convert.ToInt32(recordNo), recordType, true);
        //                //update enq-outstanding mail count in route table of that ME
        //                if (siteRouteId != 0 && siteMeId != 0)
        //                    dc.Route_Update_OutsandingMailCount(siteRouteId, siteMeId, 2);
        //            }
        //            catch { }
        //        }
        //    }
        //}
        
        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
                ddlMF.Visible = true;
            else
                ddlMF.Visible = false;
        }
        private void ClearReportList()
        {
            grdReports.Visible = false;
            lbl_RecordsNo.Text = "";
            grdReports.DataSource = null;
            grdReports.DataBind();
        }
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                else
                {
                    lblClientId.Text = "0";
                }
            }
        }

        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
         
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, -1, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }

        protected void optPending_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
        }

        protected void optApproved_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
        }


        public void ApproveReports(string Recordtype, string ReferenceNo, int RecordNo) //, string EmailId, string ContactNo)
        {
            string tempRecType = Recordtype;
            string testType = Recordtype;
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" )
            {
                if (ddlMF.SelectedValue == "Final")
                {
                    tempRecType = tempRecType + " " + ddlMF.SelectedValue;
                    testType = "Final";
                }
                else
                {
                    testType = "MDL";
                }
            }
            //dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, Convert.ToByte(lblUserId.Text), Convert.ToBoolean(chk_Sign.Checked), "Approved By");
            //dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, true, false, false, false, false);
            
            #region Bill Generation
            bool approveRptFlag = true;
            bool generateBillFlag = false;
            string BillNo = "0";
            if (DateTime.Now.Day >= 26)
            {
                generateBillFlag = false;
            }
            if (generateBillFlag == true)
            {
                var inward = dc.Inward_View(0, RecordNo, Recordtype, null, null).ToList();
                foreach (var inwd in inward)
                {
                    if (inwd.INWD_BILL_Id != null && inwd.INWD_BILL_Id != "0")
                    {
                        BillNo = inwd.INWD_BILL_Id;
                        generateBillFlag = false;
                    }
                    if (inwd.SITE_MonthlyBillingStatus_bit != null && inwd.SITE_MonthlyBillingStatus_bit == true)
                    {
                        generateBillFlag = false;
                    }
                    if (inwd.INWD_MonthlyBill_bit == true)
                    {
                        generateBillFlag = false;
                    }
                }
            }
            if (generateBillFlag == true && Recordtype == "CT")
            {
                var ctinwd = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, RecordNo, "", 0, Recordtype);
                foreach (var ct in ctinwd)
                {
                    if (ct.CTINWD_CouponNo_var != null && ct.CTINWD_CouponNo_var != "")
                    {
                        generateBillFlag = false;
                        break;
                    }
                }
            }
            if (generateBillFlag == true)
            {
                var withoutbill = dc.WithoutBill_View(RecordNo, Recordtype);
                if (withoutbill.Count() > 0)
                {
                    generateBillFlag = false;
                }
            }
            if (generateBillFlag == true)
            {
                int NewrecNo = 0;
                clsData clsObj = new clsData();
                NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                //var gstbillCount = dc.Bill_View("0", 0, 0, "", 0, false, false, DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                if (gstbillCount.Count() != NewrecNo - 1)
                {
                    generateBillFlag = false;
                    approveRptFlag = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill No. mismatch. Can not approve report.');", true);
                }
            }
            if (approveRptFlag == true)
            {
                //Generate bill
                if (generateBillFlag == true)
                {
                    BillUpdation bill = new BillUpdation();
                    BillNo = bill.UpdateBill(Recordtype, RecordNo, BillNo);
                }
                //
                dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, Convert.ToByte(lblUserId.Text), Convert.ToBoolean(chk_Sign.Checked), "Approved By");
                dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, true, false, false, false, false);
            }
            #endregion          
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Report Approved Successfully.";
            lblMsg.Visible = true;
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

        protected void lnkSaveContactDetail_Click(object sender, EventArgs e)
        {
            lblContactMessage.Visible = false;
            txtContactNo.Text = txtContactNo.Text.Trim();
            txtContactEmail.Text = txtContactEmail.Text.Trim();
            if (Page.IsValid)
            {
                if (lblContactId.Text != "" && lblContactId.Text != "0")
                {
                    dc.Contact_Update_details(Convert.ToInt32(lblContactId.Text), txtContactNo.Text, txtContactEmail.Text, lblRecordType.Text, Convert.ToInt32(lblRecordNo.Text),txtContactName.Text);
                    lblContactMessage.Text = "Updated Successfully";
                    lblContactMessage.ForeColor = System.Drawing.Color.Green;
                    lblContactMessage.Visible = true;
                    lnkSaveContactDetail.Enabled = false;
                }             
            }
        }

        protected void imgCloseContactDetailPopup_Click(object sender, ImageClickEventArgs e)
        {
            lblContactId.Text = "";
            lblRecordType.Text = "";
            lblRecordNo.Text = "";
            txtContactName.Text = "";

            txtContactNo.Text = "";
            txtContactEmail.Text = "";
            ModalPopupExtender1.Hide();
        }

        protected void lnkCancelContactDetail_Click(object sender, EventArgs e)
        {
            lblContactId.Text = "";
            lblRecordType.Text = "";
            lblRecordNo.Text = "";
            txtContactName.Text = "";
            
            txtContactNo.Text = "";
            txtContactEmail.Text = "";
            ModalPopupExtender1.Hide();
        }

        
    }

}
