using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Data;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class BillStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Bill Status";
                //txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtFromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadInwardTypeList();
                LoadOtherTestList();
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAccount_right_bit == true)
                    {
                        lnkSendForReapproval.Visible = true;
                    }
                }
            }
        }

        private void LoadInwardTypeList()
        {
            var inwd = dc.Material_View("", "").ToList();
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_RecordType_var";
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, new ListItem("All", ""));

            //ddl_InwardTestType.Items.Insert(0, new ListItem("---Select---","No"));
            ddl_InwardTestType.Items.Insert(1, new ListItem("Coupon", "---"));
            ddl_InwardTestType.Items.Insert(2, new ListItem("Monthly", "Monthly"));

            ddlRecordType.DataSource = inwd;
            ddlRecordType.DataTextField = "MATERIAL_Name_var";
            ddlRecordType.DataValueField = "MATERIAL_RecordType_var";
            ddlRecordType.DataBind();
            ddlRecordType.Items.Insert(0, new ListItem("---All---"));

        }
     
        public void LoadBillList(bool ModifiedFlag)
        {
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int ClientId = 0;
            byte apprStatus = 2;
            bool billStatus = false;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            if (ddlStatus.SelectedValue == "1")
            {
                billStatus = true;
            }
            apprStatus = Convert.ToByte(ddlApproveStatus.SelectedValue); 
            
            var bill = dc.Bill_View_Status(ClientId, ddl_InwardTestType.SelectedItem.Value, 0, billStatus, Fromdate, Todate, ModifiedFlag,  Convert.ToByte(ddlClientApproveStatus.SelectedValue), apprStatus);
            grdModifyBill.DataSource = bill;
            grdModifyBill.DataBind();
            
            var inwd = dc.Section_View().ToList();
            if (grdModifyBill.Rows.Count > 0)
            {
                int otherSectionId=0;
                foreach (var item in inwd)
                {
                    if (item.Section_Name_var.ToLower().Equals("others"))
                        otherSectionId = Convert.ToInt32(item.Section_Id.ToString());
                }
                bool billRight = false, billSuperAccountRight = false, billPrintRight = false, billApproveRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Bill_right_bit == true)
                        billRight = true;
                    if (u.USER_BillPrint_right_bit == true)
                        billPrintRight = true;
                    if (u.USER_Approve_right_bit == true)
                        billApproveRight = true;
                    if (u.USER_SuperAccount_right_bit == true)
                        billSuperAccountRight = true;
                }
                decimal billTotal = 0, billPaidAmount = 0, billPendingAmount = 0;
                for (int i = 0; i < grdModifyBill.Rows.Count; i++)
                {
                    string recType = grdModifyBill.Rows[i].Cells[2].Text.ToString();
                    LinkButton lnkModifyBill = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkModifyBill");
                    LinkButton lnkPrintBill = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkPrintBill");
                    LinkButton lnkApproveBill = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkApproveBill");
                    Label lblApproveStatus = (Label)grdModifyBill.Rows[i].FindControl("lblApproveStatus");
                    LinkButton lnkEmailBill = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkEmailBill");
                    LinkButton lnkUpdateGst = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkUpdateGst");
                    DropDownList ddl_Inward = (DropDownList)grdModifyBill.Rows[i].FindControl("ddl_Inward");
                    Label lblSectionId = (Label)grdModifyBill.Rows[i].FindControl("lblSectionId");
                    ddl_Inward.DataSource = inwd;
                    ddl_Inward.DataTextField = "Section_Name_var";
                    ddl_Inward.DataValueField = "Section_Id";
                    ddl_Inward.DataBind();
                    ddl_Inward.Items.Insert(0, new ListItem("---Select---", "0"));
                    if (lblSectionId.Text != "" && Convert.ToInt32(lblSectionId.Text) > 0)
                    {
                        ddl_Inward.SelectedValue = lblSectionId.Text;
                    }
                    else if(recType=="OT")
                    {
                        ddl_Inward.SelectedValue = otherSectionId.ToString(); 
                    }
                    else
                    {
                        int sec_id = 0,flag=0;
                        if (recType != "---" && recType != "Monthly")
                        {
                            foreach (var item in inwd)
                            {
                                if (Convert.ToString(item.Section_RecordType_var) != null && Convert.ToString(item.Section_RecordType_var) != "")
                                {
                                    if (flag == 1)
                                        break;
                                   
                                    string[] arr = item.Section_RecordType_var.Split(',');
                                    for (int j = 0; j < arr.Length; j++)
                                    {
                                        if (arr[j].Equals(recType))
                                        {
                                            sec_id = Convert.ToInt32(item.Section_Id);
                                            flag = 1;
                                        }
                                    }
                                  
                                 }
                              
                            }
                        }
                        ddl_Inward.SelectedValue = sec_id.ToString();
                    }
                    

                    if (billSuperAccountRight == true)
                        lnkUpdateGst.Enabled = true;
                    else
                        lnkUpdateGst.Enabled = false;
                    if (billRight == true) // && lblApproveStatus.Text != "True")
                    {
                        lnkModifyBill.Enabled = true;
                    }
                    else
                    {
                        lnkModifyBill.Enabled = false;
                    }
                    if (billApproveRight == true && lblApproveStatus.Text != "True")
                    {
                        lnkApproveBill.Enabled = true;
                    }
                    else
                    {
                        lnkApproveBill.Enabled = false;
                    }
                    if (billPrintRight == true)
                    {
                        lnkPrintBill.Enabled = true;
                        if (lblApproveStatus.Text == "True")
                        {
                            lnkEmailBill.Enabled = true;
                        }
                        else
                        {
                            lnkEmailBill.Enabled = false;
                        }
                    }
                    else
                    {
                        lnkPrintBill.Enabled = false;
                        lnkEmailBill.Enabled = false;
                    }
                    billTotal += Convert.ToDecimal(grdModifyBill.Rows[i].Cells[6].Text);
                    if (grdModifyBill.Rows[i].Cells[13].Text != "" && grdModifyBill.Rows[i].Cells[14].Text != "&nbsp;")
                        billPaidAmount += Convert.ToDecimal(grdModifyBill.Rows[i].Cells[14].Text);
                    if (grdModifyBill.Rows[i].Cells[14].Text != "" && grdModifyBill.Rows[i].Cells[15].Text != "&nbsp;")
                        billPendingAmount += Convert.ToDecimal(grdModifyBill.Rows[i].Cells[15].Text);
                }
                lblTotal.Text = "Total : " + billTotal.ToString("0.00");
                lblPaidAmount.Text = "Total Paid Amount : " + billPaidAmount.ToString("0.00");
                lblPendingAmount.Text = "Total Pending Amount : " + billPendingAmount.ToString("0.00");
            }
        }

        protected void grdModifyInward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                DropDownList ddl = (DropDownList)gvr.FindControl("ddl_Inward");
                Session["BillId"] = Convert.ToString(e.CommandArgument);
                string billid = Convert.ToString(e.CommandArgument);
                //string txtPending = grdModifyBill.Rows[RowIndex].Cells[15].Text;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    var bill = dc.Bill_View(billid, 0, 0, "", 0, false, false, null, null).ToList();
                    if (u.USER_BillPrint_right_bit == true && e.CommandName == "lnkPrintBill")
                    {
                        //if (bill.FirstOrDefault().BILL_PrintLock_bit == false)
                        //{
                        printBill(Session["BillId"].ToString(), "pdf", "Print");
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Can not print bill, monthly bill will be generated. ');", true);
                        //}
                        break;
                    }
                    else if (u.USER_BillPrint_right_bit == true && e.CommandName == "lnkEmailBill")
                    {
                        PrintPDFReport obj = new PrintPDFReport();
                        obj.Bill_PDFPrint(billid, false, "DisplayLogoEmail1");
                        break;
                    }
                    else if (u.USER_Bill_right_bit == true && e.CommandName == "lnkModifyBill")
                    {
                        bool modifyBillFlag = true;
                        //var bill = dc.Bill_View(Convert.ToInt32(billid), 0, 0, "", 0, false, false, null, null);
                        foreach (var bl in bill)
                        {
                            var master = dc.MasterSetting_View(0);
                            foreach (var mst in master)
                            {
                                if (mst.MASTER_BillLockDate_dt != null)
                                {
                                    if (mst.MASTER_BillLockDate_dt >= bl.BILL_Date_dt)
                                    {
                                        modifyBillFlag = false;
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill Modification Locked by Account Dept. ..');", true);
                                    }
                                }
                            }
                            if (modifyBillFlag == true)
                            {
                                if (bl.BILL_RecordType_var == "Monthly")
                                {
                                    modifyBillFlag = false;
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Can not modify monthly bill. Update from Monthly Billing Screen..');", true);
                                }
                            }
                            if (modifyBillFlag == true)
                            {
                                var rcpt = dc.CashDetail_View_bill(billid).ToList();
                                if (rcpt.Count > 0)
                                {
                                    modifyBillFlag = false;
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Receipt has been added for this Bill Number  !'+ '\\n' +'It can not be modified ');", true);
                                }
                            }
                        }
                        if (modifyBillFlag == true)
                        {
                            string CouponBill = "False";
                            if (ddl_InwardTestType.SelectedItem.Value == "---")
                                CouponBill = "True";
                            //    Session["CouponBill"] = "True";
                            //else
                            //    Session["CouponBill"] = "False";

                            //Response.Redirect("Bill.aspx?BillId=" + billid + "&CouponBill=" + CouponBill);

                            //Query string ecrypt
                            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                            string strURLWithData = "Bill.aspx?" + obj.Encrypt(string.Format("BillId={0}&CouponBill={1}", billid, CouponBill));
                            //Response.Redirect(strURLWithData);
                            PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                            //
                        }
                    }
                    else if (u.USER_Approve_right_bit == true && e.CommandName == "lnkApproveBill")
                    {
                        Label lblMsg = (Label)Master.FindControl("lblMsg");
                        lblMsg.Visible = false;
                        int sectnId = 0;
                        string sectnType = ddl.SelectedItem.Text.ToString();
                        LinkButton lnkDownloadFile = (LinkButton)grdModifyBill.Rows[RowIndex].FindControl("lnkDownloadFile");
                        LinkButton lnkDownloadMOUFile = (LinkButton)grdModifyBill.Rows[RowIndex].FindControl("lnkDownloadMOUFile");

                        if (sectnType == "---Select---")
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Please Select Section Type.";
                            ddl.Focus();
                            return;
                        }
                        else if (lnkDownloadFile.Text == "" && Convert.ToDecimal(grdModifyBill.Rows[RowIndex].Cells[15].Text.ToString()) > 0 && lnkDownloadMOUFile.Text == "")
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Can not approve bill, please upload PO file.";
                            FileUpload FileUploadPO = (FileUpload)grdModifyBill.Rows[RowIndex].FindControl("FileUploadPO");
                            FileUploadPO.Focus();
                            return;
                        }
                        else
                        {
                            sectnId = Convert.ToInt32(ddl.SelectedValue.ToString());
                        }
                        dc.Bill_Update_ApproveStatus(billid, Convert.ToInt32(Session["LoginId"]), true, sectnId);
                        //mail bill to client
                        var clMail = dc.Bill_View(billid, 0, 0, "", 0, false, false, null, null).ToList();
                        string EmailId = "", inwardEmailId = "";
                        if (clMail.FirstOrDefault().CL_AccEmailId_var != null && clMail.FirstOrDefault().CL_AccEmailId_var != "")
                            EmailId = clMail.FirstOrDefault().CL_AccEmailId_var;
                        if (clMail.FirstOrDefault().InwardContactMailId != null && clMail.FirstOrDefault().InwardContactMailId != "")
                            inwardEmailId = clMail.FirstOrDefault().InwardContactMailId;
                        bool sendMail = true;
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
                        if (EmailId.Contains(inwardEmailId) == false)
                        {
                            if (EmailId != "")
                                EmailId += ",";
                            EmailId += inwardEmailId;
                        }
                        //sendMail = false;
                        if (sendMail == true)
                        {
                            PrintPDFReport obj = new PrintPDFReport();
                            obj.Bill_PDFPrint(billid, false, "DisplayLogoEmail1");
                            if (File.Exists("C:/temp/Veena/" + "Bill_" + billid + ".pdf"))
                            {
                                string reportPath = "C:/temp/Veena/" + "Bill_" + billid + ".pdf";
                                if (File.Exists(@reportPath))
                                {
                                    clsSendMail objMail = new clsSendMail();
                                    string mTo = "", mCC = "", mSubject = "", mbody = "", mReplyTo = "";
                                    mTo = EmailId.Trim();
                                    //mTo = "shital.bandal@gmail.com";
                                    mCC = "";
                                    mSubject = "Invoice";

                                    mbody = "Dear Sir/Madam,<br><br>";
                                    mbody = mbody + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please find attached " + mSubject + " <br>";
                                    mbody = mbody + "Please feel free to contact in case of any queries." + " <br><br><br>";
                                    mbody = mbody + "<br>&nbsp;";
                                    mbody = mbody + "<br>&nbsp;";
                                    mbody = mbody + "<br>";
                                    mbody = mbody + "Best Regards,";
                                    mbody = mbody + "<br>&nbsp;";
                                    mbody = mbody + "<br>";
                                    mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";
                                    try
                                    {
                                        objMail.SendMail(mTo, mCC, mSubject, mbody, reportPath, mReplyTo);
                                    }
                                    catch { }
                                }
                            }
                        }
                        Label lblApproveStatus = (Label)grdModifyBill.Rows[RowIndex].FindControl("lblApproveStatus");
                        //LinkButton lnkModifyBill = (LinkButton)grdModifyBill.Rows[RowIndex].FindControl("lnkModifyBill");
                        LinkButton lnkApproveBill = (LinkButton)grdModifyBill.Rows[RowIndex].FindControl("lnkApproveBill");
                        lblApproveStatus.Text = "True";
                        //lnkModifyBill.Enabled = false;
                        lnkApproveBill.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Approved Successfully !');", true);
                    }
                    else if (u.USER_SuperAccount_right_bit == true && e.CommandName == "lnkUpdateGst")
                    {
                        var siteView = dc.Site_View(Convert.ToInt32(grdModifyBill.Rows[RowIndex].Cells[19].Text.ToString()), 0, 0, "").ToList();
                        var clientView = dc.Client_View(Convert.ToInt32(grdModifyBill.Rows[RowIndex].Cells[18].Text.ToString()), 0, "", "").ToList();

                        clsData db = new clsData();
                        db.updateBillGst(Session["BillId"].ToString(), siteView.FirstOrDefault().SITE_GSTNo_var, clientView.FirstOrDefault().CL_GstNo_var);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Updated Successfully !');", true);
                        LoadBillList(false);
                    }
                    else if (e.CommandName == "lnkViewRate")
                    {
                        ModalPopupExtender1.Show();
                        lblBillId.Text = billid;
                        grdRate.DataSource = null;
                        grdRate.DataBind();
                        ViewState["TestDetails"] = null;
                        //ddlRecordType.SelectedIndex = -1;
                        ddlRecordType.SelectedIndex = 0;
                        ddlOtherTest.Visible = false;
                    }
                    else if (e.CommandName == "lnkUploadPO")
                    {
                        FileUpload FileUploadPO = (FileUpload)grdModifyBill.Rows[RowIndex].FindControl("FileUploadPO");
                        LinkButton lnkDownloadFile = (LinkButton)grdModifyBill.Rows[RowIndex].FindControl("lnkDownloadFile");
                        if (FileUploadPO.HasFile == false)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('No file available..');", true);
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
                            //if (lnkDownloadFile.Text != "")
                            //{
                            //    filename = lnkDownloadFile.Text + ", " + filename;
                            //}

                            dc.Bill_Update_POFile(billid, filename);
                            LoadBillList(false);
                            ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Uploaded Successfully !');", true);
                        }
                    }
                    else if (e.CommandName == "DownloadFile")
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

                        filePath += billid;
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
                    else if (e.CommandName == "DownloadMOUFile")
                    {
                        string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                        string filePath = "D:/MOUFiles/";
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            filePath += "Mumbai/";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            filePath += "Nashik/";
                        else if (cnStr.ToLower().Contains("metro") == true)
                            filePath += "Metro/";
                        else
                            filePath += "Pune/";

                        filePath += billid;
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
                    else if (e.CommandName == "ViewProposal")
                    {
                        if (billid != "")
                        {
                            string[] strVal = billid.Split(';');
                            PrintPDFReport rpt = new PrintPDFReport();
                            rpt.Proposal_PDF(Convert.ToInt32(strVal[0]), 0, Convert.ToInt32(strVal[1]), false, "View", strVal[2]);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Access is Denied !');", true);
                    }
                }
            }
        }
        protected void grdModifyInward_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdModifyBill.PageIndex = e.NewPageIndex;
            LoadBillList(false);
        }
        protected void grdModifyInward_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
            if (gvr != null)
            {
                gvr.Visible = true;
            }
        }
        protected void grdModifyInward_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (grdModifyBill.PageIndex + 1) + " of " + grdModifyBill.PageCount;
            }
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
        private void printBill(string billNo, string filetype, string Action)
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //BillUpdation bill = new BillUpdation();
            //string strFileName = "Bill_" + Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".html";
            //reportPath = @"C:/temp/" + strFileName;
            ////reportPath = Server.MapPath("~") + "\\report.html";
            ////reportPath = Server.MapPath("~") + strFileName;
            //sw = File.CreateText(reportPath);
            //reportStr = bill.getBillPrintString(Convert.ToInt32(billNo), chkDuplicateBill.Checked);

            //sw.WriteLine(reportStr);
            //sw.Close();
            ////NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            ////NewWindows.Redirect(strFileName, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

            //Response.ContentType = "text/HTML";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName);
            //Response.TransmitFile(reportPath);
            //Response.End();
            if (filetype == "html")
            {
                BillUpdation bill = new BillUpdation();
                bill.getBillPrintString(billNo, chkDuplicateBill.Checked);
            }
            else
            {
                PrintPDFReport obj = new PrintPDFReport();
                obj.Bill_PDFPrint(billNo, chkDuplicateBill.Checked, Action);

            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadBillList(false);
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkCouponBill_Click(object sender, EventArgs e)
        {
            Session["BillId"] = null;
            Session["CouponBill"] = "True";
            Response.Redirect("Bill.aspx");
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdModifyBill.Rows.Count > 0 && grdModifyBill.Visible == true)
            {

                string Subheader = "";
                Subheader = "" + "|" + lblBilldt.Text + "|" + txtFromDate.Text + " - " + txtToDate.Text + "|" + lblInwdtype.Text + "|" + ddl_InwardTestType.SelectedItem.Text;
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                PrintHTMLReport rpt = new PrintHTMLReport();
                for (int j = 0; j <= grdModifyBill.Columns.Count-1; j++)
                {
                    //if (grdModifyBill.Columns[j].Visible == true)
                  
                    //{ 
                        if (grdModifyBill.Columns[j].HeaderText !="Section" && grdModifyBill.Columns[j].HeaderText != "Gst No")
                        grdColumn += grdModifyBill.Columns[j].HeaderText + "|";
                    //}
                }
                for (int i = 0; i <= grdModifyBill.Rows.Count-1; i++)
                {
                    grddata += "$";
                    for (int c = 0; c <= grdModifyBill.Rows[i].Cells.Count-1; c++)
                    {
                        grddata += grdModifyBill.Rows[i].Cells[c].Text + "~";
                    }
                }
                grdview = grdColumn + grddata;
                //reportStr = rpt.RptHTMLgrid("Bill Status", Subheader, grdview);
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                rpt.RptHTMLgrid("Bill Status", Subheader, grdview);

            }

        }

        protected void lnkPrintPendingList_Click(object sender, EventArgs e)
        {
            if (grdModifyBill.Rows.Count > 0 && grdModifyBill.Visible == true)
            {

                string Subheader = "";
                Subheader = "" + "|" + lblBilldt.Text + "|" + txtFromDate.Text + " - " + txtToDate.Text + "|" + lblInwdtype.Text + "|" + ddl_InwardTestType.SelectedItem.Text;
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                PrintHTMLReport rpt = new PrintHTMLReport();
                for (int j = 0; j < grdModifyBill.Columns.Count - 1; j++)
                {
                    if (grdModifyBill.Columns[j].Visible == true)
                    {
                        grdColumn += grdModifyBill.Columns[j].HeaderText + "|";
                    }
                }
                for (int i = 0; i < grdModifyBill.Rows.Count; i++)
                {
                    Label lblApproveStatus = (Label)grdModifyBill.Rows[i].FindControl("lblApproveStatus");
                    if (lblApproveStatus.Text == "False")
                    {
                        grddata += "$";
                        for (int c = 0; c < grdModifyBill.Rows[i].Cells.Count-1; c++)
                        {
                            grddata += grdModifyBill.Rows[i].Cells[c].Text + "~";
                        }
                    }
                }
                grdview = grdColumn + grddata;
                //reportStr = rpt.RptHTMLgrid("Bill Status", Subheader, grdview);
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                rpt.RptHTMLgrid("Approval pending bill list", Subheader, grdview);

            }

        }
        
        protected void lnkEnteredBillPrint_Click(object sender, EventArgs e)
        {
            if (txtBillNo.Text.Trim() != "")
            {
                //string tempBillNo ;
                if (txtBillNo.Text.Trim() != "")
                {
                    var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null).ToList();
                    if (bill.Count > 0)
                    {
                        //if (bill.FirstOrDefault().BILL_PrintLock_bit == false)
                        //{
                        //printBill(txtBillNo.Text, "html", "");
                        printBill(txtBillNo.Text, "pdf", "Print");
                        //}
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill not available. ..');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Enter valid bill number. ..');", true);
                }
            }
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

        protected void lnkTemp_Click(object sender, EventArgs e)
        {


            //xDisc = xDisc / 100;
            //if (xDisc > 0)
            //{
            //    discPerFlag = true;
            //}
            //xSrvTax = Convert.ToDecimal("14");
            //if (xSrvTax > 0)
            //{
            //    xSrvTax = xSrvTax / 100;
            //}            
            //xSwTax = Convert.ToDecimal("0.5");
            //xKkTax = Convert.ToDecimal("0.5");            

            //if (xSwTax > 0)
            //{
            //    xSwTax = xSwTax / 100;
            //}
            //if (xKkTax > 0)
            //{
            //    xKkTax = xKkTax / 100;
            //}


            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("BillId", typeof(string)));
            var bill = dc.Bill_View_temp("");
            foreach (var b in bill)
            {
                decimal xGrossAmt = 0, xNetAmt = 0, xDisc = 0, xDiscAmt = 0, xSrvTax = 0, xSrvTaxAmt = 0, xSwTax = 0, xSwTaxAmt = 0, xKkTax = 0, xKkTaxAmt = 0, edCess = 0, highEdCess = 0, roundOff = 0;
                decimal billTotal = 0;
                bool billMistakeFlg = false;
                billTotal = 0;
                var billdetail = dc.Bill_View_temp(b.BILL_Id);
                foreach (var bd in billdetail)
                {
                    billTotal = billTotal + Convert.ToDecimal(bd.BILLD_Amt_num);
                }
                if (billTotal == 0 && b.BILL_Status_bit == false)
                    billMistakeFlg = true;
                if (billMistakeFlg == false)
                {
                    //Calculation
                    xGrossAmt = billTotal;
                    xDisc = Convert.ToDecimal(b.BILL_Discount_num);
                    xSrvTax = Convert.ToDecimal(b.BILL_SerTax_num);
                    xSwTax = Convert.ToDecimal(b.BILL_SwachhBharatTax_num);
                    xKkTax = Convert.ToDecimal(b.BILL_KisanKrishiTax_num);

                    if (xDisc > 0)
                    {
                        xDisc = xDisc / 100;
                    }
                    if (xSrvTax > 0)
                    {
                        xSrvTax = xSrvTax / 100;
                    }
                    if (xSwTax > 0)
                    {
                        xSwTax = xSwTax / 100;
                    }
                    if (xKkTax > 0)
                    {
                        xKkTax = xKkTax / 100;
                    }

                    if (xDisc > 0)
                        xDiscAmt = xGrossAmt * xDisc;
                    if (xSrvTax > 0)
                        xSrvTaxAmt = (xGrossAmt - xDiscAmt) * xSrvTax;
                    xSrvTaxAmt = Decimal.Round(xSrvTaxAmt, 2);
                    if (xSwTax > 0)
                        xSwTaxAmt = (xGrossAmt - xDiscAmt) * xSwTax;
                    xSwTaxAmt = Decimal.Round(xSwTaxAmt, 2);
                    if (xKkTax > 0)
                        xKkTaxAmt = (xGrossAmt - xDiscAmt) * xKkTax;
                    xKkTaxAmt = Decimal.Round(xKkTaxAmt, 2);

                    xNetAmt = xGrossAmt - xDiscAmt + xSrvTaxAmt + xSwTaxAmt + xKkTaxAmt + edCess + highEdCess;

                    xNetAmt = Decimal.Round(xNetAmt, 0);
                    roundOff = Decimal.Round((xNetAmt - (xGrossAmt - xDiscAmt + xSrvTaxAmt + xSwTaxAmt + xKkTaxAmt + edCess + highEdCess)), 2);
                    //xNetAmt = Decimal.Round(xNetAmt, 2);
                    xSrvTax = xSrvTax * 100;
                    xSwTax = xSwTax * 100;
                    xKkTax = xKkTax * 100;
                    xDisc = xDisc * 100;

                    if (xNetAmt != b.BILL_NetAmt_num)
                        billMistakeFlg = true;
                }
                if (billMistakeFlg == true)
                {
                    dr = dt.NewRow();
                    dr["BillId"] = b.BILL_Id;
                    dt.Rows.Add(dr);
                }
            }
            grdTemp.DataSource = dt;
            grdTemp.DataBind();
            grdTemp.Visible = true;
        }

        protected void lnkSendForReapproval_Click(object sender, EventArgs e)
        {
            txtBillNo.Text = txtBillNo.Text.Trim();
            if (txtBillNo.Text.Trim() != "")
            {
                //string tempBillNo ;
                if (txtBillNo.Text.Trim() != "")
                {
                    var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null).ToList();
                    if (bill.Count > 0)
                    {
                        if (bill.FirstOrDefault().BILL_ApproveStatus_bit == true)
                        {
                            dc.Bill_Update_ApproveStatus(txtBillNo.Text, Convert.ToInt32(Session["LoginId"]), false,0);
                            for (int i = 0; i < grdModifyBill.Rows.Count; i++)
                            {
                                if (grdModifyBill.Rows[i].Cells[0].Text == txtBillNo.Text)
                                {
                                    Label lblApproveStatus = (Label)grdModifyBill.Rows[i].FindControl("lblApproveStatus");
                                    LinkButton lnkModifyBill = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkModifyBill");
                                    LinkButton lnkApproveBill = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkApproveBill");
                                    lblApproveStatus.Text = "False";
                                    lnkModifyBill.Enabled = true;
                                    lnkApproveBill.Enabled = true;
                                    break;
                                }
                            }
                            ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill sent for re-approval. ..');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill not approved. ..');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill not available. ..');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Enter valid bill number. ..');", true);
                }
            }
        }

        protected void lnkMonthlyBillDetail_Click(object sender, EventArgs e)
        {
            if (txtBillNo.Text.Trim() != "")
            {
                //string tempBillNo ;
                if (txtBillNo.Text.Trim() != "")
                {
                    var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null).ToList();
                    if (bill.Count > 0)
                    {
                        if (bill[0].BILL_RecordType_var == "Monthly")
                        {
                            BillUpdation billPrint = new BillUpdation();
                            billPrint.getMonthlyBillDetailPrint(txtBillNo.Text);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Can not print details for regular bill..');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill not available. ..');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Enter valid bill number. ..');", true);
                }
            }
        }

        protected void lnkFetchModifiedBills_Click(object sender, EventArgs e)
        {
            LoadBillList(true); 
        }

        protected void imgCloseSitewiseRateListPopup_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        
        protected void LoadOtherTestList()
        {
            var test = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            ddlOtherTest.DataSource = test;
            ddlOtherTest.DataTextField = "TEST_Name_var";
            ddlOtherTest.DataValueField = "TEST_Id";
            ddlOtherTest.DataBind();
            ddlOtherTest.Items.Insert(0, new ListItem("---All---", "0"));
        }
        protected void ddlRecordType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRecordType.SelectedValue == "OT")
            {
                ddlOtherTest.Visible = true;
                ddlOtherTest.SelectedIndex = 0;
            }
            else
            {
                ddlOtherTest.Visible = false;
            }
        }

        protected void ImgBtnSearchRate_Click(object sender, ImageClickEventArgs e)
        {
            int CL_ID = 0, SITE_ID = 0;
            var bill = dc.Bill_View(lblBillId.Text, 0, 0, "", 0, false, false, null, null);
            foreach (var bl in bill)
            {
                CL_ID = Convert.ToInt32(bl.BILL_CL_Id);
                SITE_ID = Convert.ToInt32(bl.BILL_SITE_Id);
            }

            var rt = dc.SiteWiseRate_View(CL_ID, SITE_ID, 0, false);
            grdRate.DataSource = rt;
            grdRate.DataBind();

            DataTable dt = new DataTable();
            dt = GetDataTable(grdRate);
            ViewState["TestDetails"] = dt;

            string recordTypeValue = "";
            if (ddlRecordType.SelectedIndex != -1 && ddlRecordType.SelectedItem.Text != "")
            {
                if (ViewState["TestDetails"] != null)
                {
                    dt = (DataTable)ViewState["TestDetails"];
                    DataView dv = new DataView(dt);
                    if (ddlRecordType.SelectedItem.Text != "---All---" && dt.Rows.Count > 0)
                    {
                        recordTypeValue = ddlRecordType.SelectedValue;
                        if (recordTypeValue == "OT")
                        {
                            recordTypeValue = "OTHER";
                            string strFilter = "Test_RecType_var = '" + recordTypeValue + "'";
                            if (ddlOtherTest.SelectedValue != "0")
                            {
                                strFilter += "and otherTestType = '" + ddlOtherTest.SelectedItem.Text + "'";
                            }
                            dv.RowFilter = strFilter;
                        }
                        else
                        {
                            dv.RowFilter = "Test_RecType_var = '" + recordTypeValue + "'";
                        }
                    }
                    grdRate.DataSource = dv;
                    grdRate.DataBind();
                }
            }

        }

        DataTable GetDataTable(GridView dtg)
        {
            DataTable dt = new DataTable();
            string[] arr = { "Sr.No", "Test_Id", "SITERATE_Id", "Test_RecType_var", "TEST_Name_var", "otherTestType", "TEST_From_num", "TEST_To_num", "TEST_Rate_int", "SITERATE_Test_Rate_int" };
            // add the columns to the datatable
            if (dtg.HeaderRow != null)
            {
                for (int i = 0; i < arr.Count(); i++)
                {
                    dt.Columns.Add(arr[i]);
                }
            }

            // add each of the data rows to the table
            foreach (GridViewRow row in dtg.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();
                Label lblSrNo = (Label)row.FindControl("lblSrNo");
                Label lblTestId = (Label)row.FindControl("lblTestId");
                Label lblRateId = (Label)row.FindControl("lblRateId");
                Label lblRecordType = (Label)row.FindControl("lblRecordType");
                Label lblTestName = (Label)row.FindControl("lblTestName");
                Label lblOtherTestType = (Label)row.FindControl("lblOtherTestType");
                Label lblCriteria = (Label)row.FindControl("lblCriteria");
                Label lblRate = (Label)row.FindControl("lblRate");
                Label lblNewRate = (Label)row.FindControl("lblNewRate");
                
                string[] a = lblCriteria.Text.Split('-');//TEST_From_num-TEST_To_num
                dr[0] = lblSrNo.Text;
                dr[1] = lblTestId.Text;
                dr[2] = lblRateId.Text;
                dr[3] = lblRecordType.Text;
                dr[4] = lblTestName.Text;
                dr[5] = lblOtherTestType.Text;
                dr[6] = a[0];
                dr[7] = a[1];
                dr[8] = lblRate.Text;
                dr[9] = lblNewRate.Text;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        protected void lnkTemp2_Click(object sender, EventArgs e)
        {
            int miscTestId = 0, testFoundCnt = 0, testNotFoundCnt = 0;
            var otherTest = dc.Test_View(0, 0, "MS", 0, 0, 0).ToList();
            if (otherTest.Count() > 0)
            {
                miscTestId = Convert.ToInt32(otherTest.FirstOrDefault().TEST_Id);
            }
            var bill = dc.Bill_View_temp2("", 0, 0, false).ToList();
            foreach (var b in bill)
            {
                if (b.BILL_RecordType_var == "---")
                {
                    var testCoupBill = dc.Test(0, "", 0, "COUPON", "Concrete Cube Testing Coupons", 0);
                    foreach (var testId in testCoupBill)
                    {
                        dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, testId.TEST_Id, false);
                    }
                }
                else if (b.BILL_RecordType_var == "OT" && b.BILL_RecordNo_int > 0)
                {
                    var otherInward = dc.AllInward_View("OT", Convert.ToInt32(b.BILL_RecordNo_int), "");
                    foreach (var otinwd in otherInward)
                    {
                        bool testFound = false;
                        var testList = dc.Test_View(20, 0, "", 0, 0, otinwd.OTINWD_TEST_Id).ToList();
                        foreach (var t in testList)
                        {
                            if (b.BILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true &&
                                b.BILLD_ActualRate_num == t.TEST_Rate_int)
                            {
                                dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, t.TEST_Id, false);
                                testFound = true;
                                testFoundCnt++;
                                break;
                            }
                        }
                        if (testFound == false)
                        {
                            foreach (var t in testList)
                            {
                                if (b.BILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true)
                                {
                                    dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, t.TEST_Id, false);
                                    testFound = true;
                                    testFoundCnt++;
                                    break;
                                }
                            }
                        }
                        if (testFound == false)
                        {
                            //dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, miscTestId, false);
                            testNotFoundCnt++;
                        }
                    }
                }
                else
                {
                    string recType = b.BILL_RecordType_var;
                    if (recType == "OT")
                        recType = "OTHER";
                    bool testFound = false;
                    var testList = dc.Test_View(0, 0, b.BILL_RecordType_var, 0, 0, 0).ToList();
                    foreach (var t in testList)
                    {
                        if (b.BILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true &&
                            b.BILLD_ActualRate_num == t.TEST_Rate_int)
                        {
                            dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, t.TEST_Id, false);
                            testFound = true;
                            testFoundCnt++;
                            break;
                        }
                    }
                    if (testFound == false)
                    {
                        foreach (var t in testList)
                        {
                            if (b.BILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true)
                            {
                                dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, t.TEST_Id, false);
                                testFound = true;
                                testFoundCnt++;
                                break;
                            }
                        }
                    }
                    if (testFound == false && b.BILL_RecordType_var == "ST")
                    {
                        foreach (var t in testList)
                        {
                            if (b.BILLD_TEST_Name_var.ToLower().Contains("all") == true &&
                                t.TEST_Name_var.ToLower().Contains("all") == true)
                            {
                                dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, t.TEST_Id, false);
                                testFound = true;
                                testFoundCnt++;
                                break;
                            }
                        }
                    }
                    if (testFound == false)
                    {
                        //dc.Bill_View_temp2(b.BILL_Id, b.BILLD_Id, miscTestId, false);
                        testNotFoundCnt++;
                    }
                }
            }
            string dispalyMsg = "Test Id's updated successfully. test found " + testFoundCnt + " test not found " + testNotFoundCnt;
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + dispalyMsg + "');", true);
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = dispalyMsg;

        }

        protected void lnkTemp3_Click(object sender, EventArgs e)
        {
            int miscTestId = 0, testFoundCnt = 0, testNotFoundCnt = 0;
            var otherTest = dc.Test_View(0, 0, "MS", 0, 0, 0).ToList();
            if (otherTest.Count() > 0)
            {
                miscTestId = Convert.ToInt32(otherTest.FirstOrDefault().TEST_Id);
            }
            var bill = dc.Bill_View_temp2("", 0, 0, true).ToList();
            foreach (var b in bill)
            {
                string recordType = "";
                var mat = dc.Material_View("", b.BILLD_TEST_Name_var + "%").ToList();
                if (mat.Count() > 0)
                {
                    recordType = mat.FirstOrDefault().MATERIAL_RecordType_var;
                }
                else
                {
                    recordType = "OT";
                }
                int recordNo = 0;
                if (b.BILLD_ReferenceNo_int > 0)
                {
                    var inwd = dc.Inward_View(b.BILLD_ReferenceNo_int, 0, "OT", null, null).ToList();
                    recordNo = Convert.ToInt32(inwd.FirstOrDefault().INWD_RecordNo_int);
                }
                if (recordType == "OT" && recordNo > 0)
                {
                    var otherInward = dc.AllInward_View("OT", recordNo, "");
                    foreach (var otinwd in otherInward)
                    {
                        bool testFound = false;
                        var testList = dc.Test_View(20, 0, "", 0, 0, otinwd.OTINWD_TEST_Id).ToList();
                        foreach (var t in testList)
                        {
                            if (b.MBILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true &&
                                b.MBILLD_ActualRate_num == t.TEST_Rate_int)
                            {
                                dc.Bill_View_temp2(b.BILL_Id, b.MBILLD_Id, t.TEST_Id, true);
                                testFound = true;
                                testFoundCnt++;
                                break;
                            }
                        }
                        if (testFound == false)
                        {
                            foreach (var t in testList)
                            {
                                if (b.MBILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true)
                                {
                                    dc.Bill_View_temp2(b.BILL_Id, b.MBILLD_Id, t.TEST_Id, true);
                                    testFound = true;
                                    testFoundCnt++;
                                    break;
                                }
                            }
                        }
                        if (testFound == false)
                        {
                            //dc.Bill_View_temp2(b.BILL_Id, b.MBILLD_Id, miscTestId, false);
                            testNotFoundCnt++;
                        }
                    }
                }
                else
                {
                    string recType = recordType;
                    if (recordType == "OT")
                        recType = "OTHER";
                    bool testFound = false;
                    var testList = dc.Test_View(0, 0, recType, 0, 0, 0).ToList();
                    foreach (var t in testList)
                    {
                        if (b.MBILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true &&
                            b.MBILLD_ActualRate_num == t.TEST_Rate_int)
                        {
                            dc.Bill_View_temp2(b.BILL_Id, b.MBILLD_Id, t.TEST_Id, true);
                            testFound = true;
                            testFoundCnt++;
                            break;
                        }
                    }
                    if (testFound == false)
                    {
                        foreach (var t in testList)
                        {
                            if (b.MBILLD_TEST_Name_var.Contains(t.TEST_Name_var) == true)
                            {
                                dc.Bill_View_temp2(b.BILL_Id, b.MBILLD_Id, t.TEST_Id, true);
                                testFound = true;
                                testFoundCnt++;
                                break;
                            }
                        }
                    }
                    if (testFound == false && recordType == "ST")
                    {
                        foreach (var t in testList)
                        {
                            if (b.MBILLD_TEST_Name_var.ToLower().Contains("all") == true &&
                                t.TEST_Name_var.ToLower().Contains("all") == true)
                            {
                                dc.Bill_View_temp2(b.BILL_Id, b.MBILLD_Id, t.TEST_Id, true);
                                testFound = true;
                                testFoundCnt++;
                                break;
                            }
                        }
                    }
                    if (testFound == false)
                    {
                        //dc.Bill_View_temp2(b.BILL_Id, b.MBILLD_Id, miscTestId, false);
                        testNotFoundCnt++;
                    }
                }

            }
            string dispalyMsg = "Test Id's updated successfully. test found " + testFoundCnt + " test not found " + testNotFoundCnt;
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + dispalyMsg + "');", true);
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = dispalyMsg;

        }

        protected void ImgBtnSearchClApprStatusWise_Click(object sender, ImageClickEventArgs e)
        {
            LoadBillList(false);
        }
    }
}