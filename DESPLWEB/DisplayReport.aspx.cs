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
    public partial class DisplayReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                bool viewRight = false;
                var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_View_right_bit == true)
                        viewRight = true;
                }
                if (viewRight == true)
                {
                    Label lblheading = (Label)Master.FindControl("lblheading");
                    lblheading.Text = "Display Report";
                    LoadInwardType();
                    txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
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

        protected void LoadOtherTestList()
        {
            var test = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            ddl_Category.DataSource = test;
            ddl_Category.DataTextField = "TEST_Name_var";
            ddl_Category.DataValueField = "TEST_Id";
            ddl_Category.DataBind();
            ddl_Category.Items.Insert(0, new ListItem("---Select All---", "0"));
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
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (optPrinted.Checked == true)
            //    {
            //        LinkButton lnkPrintReport = (LinkButton)e.Row.FindControl("lnkPrintReport");
            //        lnkPrintReport.Visible = false;
            //        TextBox txtEmailId = (TextBox)e.Row.FindControl("txtEmailId");
            //        txtEmailId.ReadOnly = true;
            //    }
            //}
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
            string finalStatus = "";
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                //if (ddlMF.SelectedValue == "FinalNew" || ddlMF.SelectedValue == "Final")    
                //    //finalStatus = " " + "Final";
                //    finalStatus = " " + "FinalNew";
                //else
                    finalStatus = " " + ddlMF.SelectedValue;
            }

            int categoryId = 0;
            if (ddl_InwardTestType.SelectedItem.Text == "Other Testing")
            {
                categoryId = Convert.ToInt32(ddl_Category.SelectedValue);
            }

            byte rptStatus = 10;
            var Inward = dc.ReportStatus_View_All(ddl_InwardTestType.SelectedItem.Text + finalStatus, Fromdate, Todate, rptStatus, ClientId,categoryId,-1).ToList();
            grdReports.DataSource = Inward;
            grdReports.DataBind();
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "Trial")
            {
                grdReports.Columns[7].Visible = true;
                grdReports.Columns[8].Visible = false;
            }
            else if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "Cube Strength")
            {
                grdReports.Columns[7].Visible = true;
                grdReports.Columns[8].Visible = true;
            }
            else
            {
                grdReports.Columns[7].Visible = false;
                grdReports.Columns[8].Visible = false;
            }
            lbl_RecordsNo.Text = "Total Records   :  " + grdReports.Rows.Count;
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                for (int j = i + 1; j < grdReports.Rows.Count; j++)
                {
                    if (grdReports.Rows[i].Cells[1].Text == grdReports.Rows[j].Cells[1].Text)
                    {
                        grdReports.Rows[j].Cells[0].Text = "";
                        grdReports.Rows[j].Cells[1].Text = "";
                        grdReports.Rows[j].Cells[2].Text = "";
                        grdReports.Rows[j].Cells[3].Text = "";
                        grdReports.Rows[j].Cells[4].Text = "";
                    }
                }
            }
        }

        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[3];
            arg = strReportDetails.Split(';');

            string Recordtype = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            string ReferenceNo = Convert.ToString(arg[2]);
            string mfType = "";
            Boolean flgmsg = false;
            string[] strRefNo = ReferenceNo.Split('/');
            var chkBlockStatus = dc.Inward_View_ClientBlock(Convert.ToInt32(strRefNo[0])).ToList();
            if (chkBlockStatus.FirstOrDefault ().CL_ByPassCRLimitChecking_bit ==false &&
                 (chkBlockStatus.FirstOrDefault().CL_BlockStatus_bit == true  || 
                 (chkBlockStatus.FirstOrDefault().CL_Limit_mny > 0 && chkBlockStatus.FirstOrDefault().CL_BalanceAmt_mny > chkBlockStatus.FirstOrDefault().CL_Limit_mny)) )                                            
            {
                flgmsg = true;
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client blocked as per credit policy, So can not display report.');", true);
            }
            if (flgmsg == false)
            {
                if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "FinalNew")
                {
                    mfType = "Final";
                }
                else if (ddl_InwardTestType.SelectedItem.Text != "Mix Design" ||
                    (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "MDL"))
                {
                    var chkBalance = dc.Inward_View_ClientBalance(ReferenceNo, Recordtype + mfType);
                    if (chkBalance.Count() > 0)
                    {
                        flgmsg = true;
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client balance amount is greater than limit, can not view report.');", true);
                    }
                }
            }
            if (flgmsg == false )
            {
                GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowindex = grdrow.RowIndex;
                TextBox txtEmailIdTo = (TextBox)grdReports.Rows[rowindex].FindControl("txtEmailIdTo");
                TextBox txtEmailIdCc = (TextBox)grdReports.Rows[rowindex].FindControl("txtEmailIdCc");
                Label lblTrialId = (Label)grdReports.Rows[rowindex].FindControl("lblTrialId");
                Label lblCubeDays = (Label)grdReports.Rows[rowindex].FindControl("lblCubeDays");

                string strStatus = "";
                if (ddlStationary.SelectedItem.Text == "Regular")
                    strStatus = "Display";
                else if (ddlStationary.SelectedItem.Text == "Logo - With NABL")
                    strStatus = "DisplayLogoWithNABL";
                else if (ddlStationary.SelectedItem.Text == "Logo - Without NABL")
                    strStatus = "DisplayLogoWithoutNABL";
                
                if (e.CommandName == "SendMail")
                {
                    strStatus += "Email";
                }
                PrintPDFReport rpt = new PrintPDFReport();                
                rpt.PrintSelectedReport(Recordtype, ReferenceNo, strStatus, lblTrialId.Text, lblCubeDays.Text, ddlMF.SelectedValue, "", "", "", "");
                if (e.CommandName == "SendMail")
                {
                    string billNo = "0";
                    var bill = dc.Inward_View(0, RecordNo, Recordtype, null, null);
                    foreach (var b in bill)
                    {
                        if (chkEmailBill.Checked == true && ReferenceNo.Substring(ReferenceNo.Length - 2) == "-1")
                        {
                            if (b.INWD_BILL_Id != "0")
                            {
                                billNo = b.INWD_BILL_Id;
                                var billtable = dc.Bill_View(billNo, 0, 0, "", 0, false, false, null, null);
                                foreach (var bl in billtable)
                                {
                                    if (bl.BILL_ApproveStatus_bit == false)
                                    {
                                        billNo = "0";
                                    }
                                }
                                if (billNo != "0")//if (billNo > 0)
                                {
                                    PrintPDFReport obj = new PrintPDFReport();
                                    obj.Bill_PDFPrint(billNo, false, "DisplayLogoEmail");
                                }
                            }
                        }
                    }
                    
                    //send mail, //mail report
                    bool sendMail = true;
                    if (txtEmailIdTo.Text.Trim() == "" || txtEmailIdTo.Text.Trim().ToLower() == "na@unknown.com" ||
                        txtEmailIdTo.Text.Trim().ToLower() == "na" || txtEmailIdTo.Text.Trim().ToLower().Contains("na@") == true ||
                        txtEmailIdTo.Text.Trim().ToLower().Contains("@") == false || txtEmailIdTo.Text.Trim().ToLower().Contains(".") == false)
                    {
                        sendMail = false;
                    }
                    if (IsValidEmailAddress(txtEmailIdTo.Text.Trim()) == false)
                    {
                        sendMail = false;
                    }
                    //sendMail = false;
                    if (sendMail == true)
                    {
                        //string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                        ////string strfoldername = "D:/ERPReports/";

                        //if (cnStr.ToLower().Contains("mumbai") == true)
                        //    strfoldername += "Mumbai";
                        //else if (cnStr.ToLower().Contains("nashik") == true)
                        //    strfoldername += "Nashik";
                        //else
                        //    strfoldername += "Pune";
                        string strfoldername = "C:/temp/Veena/";

                        string reportPath = "";
                        //reportPath = strfoldername;

                        if (Recordtype == "MF")
                        {
                            if (ddlMF.SelectedValue == "Sieve Analysis")
                                reportPath = strfoldername + "SieveAnalysis_" + ReferenceNo.Replace('/', '_') + ".pdf";
                            else if (ddlMF.SelectedValue == "Moisture Correction")
                                reportPath = strfoldername + "MoistureCorrection_" + ReferenceNo.Replace('/', '_') + ".pdf";
                            else if (ddlMF.SelectedValue == "Cover Sheet")
                                reportPath = strfoldername + "MDLCoverSheet_" + ReferenceNo.Replace('/', '_') + ".pdf";
                            else if (ddlMF.SelectedValue == "MDL")
                                reportPath = strfoldername + "MF_" + ReferenceNo.Replace('/', '_') + ".pdf";
                            else if (ddlMF.SelectedValue == "Final")
                                reportPath = strfoldername + "MF_" + ReferenceNo.Replace('/', '_') + ".pdf";
                            else if (ddlMF.SelectedValue == "Blank Trial")
                                reportPath = "";
                            else if (ddlMF.SelectedValue == "Trial")
                                reportPath = "";
                            else if (ddlMF.SelectedValue == "Cube Strength")
                                reportPath = strfoldername + "CT_" + ReferenceNo.Replace('/', '_') + ".pdf";

                        }
                        else if (Recordtype == "OT")
                        {
                            var q = dc.OtherReport_View(ReferenceNo).ToList();
                            foreach (var quote in q)
                            {
                                reportPath = "C:/temp/Veena/" + quote.OTRPT_FileName_var;

                            }
                        }
                        else
                        {
                             reportPath = "C:/temp/Veena/" + Recordtype + "_" + ReferenceNo.Replace('/', '_') + ".pdf";
                        }

                        if (@reportPath == "")
                        {
                            reportPath = "C:/temp/Veena/" + Recordtype + "_" + ReferenceNo.Replace('/', '_') + ".pdf";
                        }
                        if (File.Exists(@reportPath))
                        {
                            if (billNo != "0")
                            {
                                if (File.Exists("C:/temp/Veena/" + "Bill_" + billNo + ".pdf"))
                                    reportPath += "," + "C:/temp/Veena/" + "Bill_" + billNo + ".pdf";
                            }

                            clsSendMail objMail = new clsSendMail();
                            string mTo = "", mCC = "", mSubject = "", mbody = "", mReplyTo = "", TestType = "";
                            //mTo = "shital.bandal@gmail.com";
                            mTo = txtEmailIdTo.Text.Trim();
                            //mCC = "reports.pune@durocrete.acts-int.com";
                            //mCC = "duroreports.pune@gmail.com";
                            if (txtEmailIdCc.Text != "")
                                mCC = txtEmailIdCc.Text;

                            mSubject = "";
                            TestType = "";
                            if (Recordtype == "CT")
                            {                           
                                mSubject = "Concrete Cube Compression test.";
                                TestType = "Concrete Cube test";
                            }
                            else if (Recordtype == "ST")
                                mSubject = "Physical test report for reinforcement steel";
                            else if (Recordtype == "AGGT")
                                mSubject = "Aggregate test report";
                            else if (Recordtype == "CEMT")
                                mSubject = "Cement Test Report";
                            else if (Recordtype == "FLYASH")
                                mSubject = "FLYASH Test Report";
                            else if (Recordtype == "PILE")
                                mSubject = "PILE Test Report";
                            else if (Recordtype == "TILE")
                                mSubject = "TILE Test Report";
                            else if (Recordtype == "PT")
                                mSubject = "Pavement Test Report";
                            else if (Recordtype == "SOLID")
                                mSubject = "SOLID Test Report";
                            else if (Recordtype == "BT-")
                                mSubject = "BRICK Test Report";
                            else if (Recordtype == "MF")
                                mSubject = "Mix Design Test Report";
                            else if (Recordtype == "STC")
                                mSubject = "Steel Chemical Test Report";
                            else if (Recordtype == "CCH")
                                mSubject = "Cement Chemical Test Report";
                            else if (Recordtype == "WT")
                                mSubject = "Water Test Report";
                            else if (Recordtype == "AAC")
                                mSubject = "Autoclaved Aerated Cellular Test Report";
                            else if (Recordtype == "NDT")
                                mSubject = "Non Destructive Test Report";
                            else if (Recordtype == "SO")
                                mSubject = "Soil Test Report";
                            else if (Recordtype == "CR")
                                mSubject = "Concrete Core Test Report";
                            else if (Recordtype == "OT")
                                mSubject = "Other Test Report";
                            else if (Recordtype == "GGBSCH")
                                mSubject = "GGBS Chemical Test Report";
                            else if (Recordtype == "GGBS")
                                mSubject = "GGBS Test Report";
                            else
                                mSubject = TestType + " Test Report";

                            if (TestType == "")
                                TestType = mSubject.Replace("Report", "");

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
                            objMail.SendMail(mTo, mCC, mSubject, mbody, reportPath, mReplyTo);
                        }
                    }
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Email Send Successfully.";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                }
            }            
        }
        
        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                //lblMF.Visible = true;
                ddlMF.Visible = true;
            }
            else
            {
                //lblMF.Visible = false;
                ddlMF.Visible = false;
            }

            if (ddl_InwardTestType.SelectedItem.Text == "Other Testing")
            {
                LoadOtherTestList();
                lblCategory.Visible = true;
                ddl_Category.Visible = true;
            }
            else
            {
                lblCategory.Visible = false;
                ddl_Category.Visible = false;
            }

            ddlStationary.Items.Clear();
            ddlStationary.Items.Add("Regular");
            var material = dc.Material_View("", ddl_InwardTestType.SelectedItem.Text + "%");
            foreach (var mat in material)
            {
                if (mat.MATERIAL_NablPrn_bit == true)
                {
                    ddlStationary.Items.Add("Logo - With NABL");
                    ddlStationary.Items.Add("Logo - Without NABL");
                }
                else
                    ddlStationary.Items.Add("Logo - Without NABL");
            }
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

        protected void optPrinted_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
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

        protected void ddlMF_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
        }
        

    }

}
