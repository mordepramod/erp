using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;

namespace DESPLWEB
{
    public partial class MFStatus : System.Web.UI.Page
    {
        PrintHTMLReport rptHtml = new PrintHTMLReport();
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "MF Status";
                getCurrentDate();
            }
        }


        public void getCurrentDate()
        {
            txt_FromDate.Text = DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy");
            txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadMFStatusList();
        }

        private void LoadMFStatusList()
        {
            grdMFStatus.DataSource = null;
            grdMFStatus.DataBind();
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            int ClientId = 0;
            string reportNo = "";

            if (txt_RptNo.Text != "")
                reportNo = txt_RptNo.Text;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            int i = 0;
            //var MFlist = dc.MaterialDetail_View(0, "", 0, "", Fromdate, Todate, "");
            var MFlist = dc.MFStatus_View(ClientId, Fromdate, Todate, reportNo);
            foreach (var mf in MFlist)
            {
                AddRowMF();
                Label lblRecvdDate = (Label)grdMFStatus.Rows[i].Cells[1].FindControl("lblRecvdDate");
                Label lblReportNo = (Label)grdMFStatus.Rows[i].Cells[2].FindControl("lblReportNo");
                LinkButton lnkBlnkTrialPt = (LinkButton)grdMFStatus.Rows[i].Cells[3].FindControl("lnkBlnkTrialPt");
                LinkButton lnkSA = (LinkButton)grdMFStatus.Rows[i].Cells[4].FindControl("lnkSA");
                LinkButton lnkTrail = (LinkButton)grdMFStatus.Rows[i].Cells[6].FindControl("lnkTrail");
                Label lbl_TrialId = (Label)grdMFStatus.Rows[i].Cells[7].FindControl("lbl_TrialId");
                LinkButton lnkTrialPt = (LinkButton)grdMFStatus.Rows[i].Cells[8].FindControl("lnkTrialPt");
                LinkButton lnkOtherInfo = (LinkButton)grdMFStatus.Rows[i].Cells[9].FindControl("lnkOtherInfo");
                LinkButton lnkMDLIssue = (LinkButton)grdMFStatus.Rows[i].Cells[10].FindControl("lnkMDLIssue");
                LinkButton lnkFinalReport = (LinkButton)grdMFStatus.Rows[i].Cells[11].FindControl("lnkFinalReport");

                lblRecvdDate.Text = Convert.ToDateTime(mf.MaterialDetail_ReceivedDt).ToString("dd/MM/yyyy");
                lblReportNo.Text = mf.MaterialDetail_RefNo.ToString();

                var st = dc.Trial_View(lblReportNo.Text, false );
                foreach (var t in st)
                {
                    if (t.Trial_Name != null && t.Trial_Name != "")
                    {
                        if (lnkTrail.Text != "")
                        {
                            i++;
                            AddRowMF();
                            LinkButton lnkTrailt = (LinkButton)grdMFStatus.Rows[i].Cells[6].FindControl("lnkTrail");
                            Label lblRecvdDatet = (Label)grdMFStatus.Rows[i].Cells[1].FindControl("lblRecvdDate");
                            Label lblReporttNo = (Label)grdMFStatus.Rows[i].Cells[2].FindControl("lblReportNo");
                            Label lbl_TrialIdt = (Label)grdMFStatus.Rows[i].Cells[7].FindControl("lbl_TrialId");

                            lblRecvdDatet.Text = lblRecvdDate.Text;
                            lblReporttNo.Text = lblReportNo.Text;
                            lnkTrailt.Text = t.Trial_Name.ToString();
                            lbl_TrialIdt.Text = t.Trial_Id.ToString();
                        }
                        lbl_TrialId.Text = t.Trial_Id.ToString();
                        lnkTrail.Text = t.Trial_Name.ToString();
                    }
                }
                i++;
            }
            Bind();

        }
        private void Bind()
        {
            for (int i = 0; i < grdMFStatus.Rows.Count; i++)
            {
                int TrialStatus = 0;

                Label lblRecvdDate = (Label)grdMFStatus.Rows[i].Cells[1].FindControl("lblRecvdDate");
                Label lblReportNo = (Label)grdMFStatus.Rows[i].Cells[2].FindControl("lblReportNo");
                LinkButton lnkBlnkTrialPt = (LinkButton)grdMFStatus.Rows[i].Cells[3].FindControl("lnkBlnkTrialPt");
                LinkButton lnkSA = (LinkButton)grdMFStatus.Rows[i].Cells[4].FindControl("lnkSA");
                ImageButton imgBtnAddTrial = (ImageButton)grdMFStatus.Rows[i].Cells[5].FindControl("imgBtnAddTrial");
                LinkButton lnkTrail = (LinkButton)grdMFStatus.Rows[i].Cells[6].FindControl("lnkTrail");
                Label lbl_TrialId = (Label)grdMFStatus.Rows[i].Cells[7].FindControl("lbl_TrialId");
                LinkButton lnkTrialPt = (LinkButton)grdMFStatus.Rows[i].Cells[8].FindControl("lnkTrialPt");
                LinkButton lnkOtherInfo = (LinkButton)grdMFStatus.Rows[i].Cells[9].FindControl("lnkOtherInfo");
                LinkButton lnkCompStr = (LinkButton)grdMFStatus.Rows[i].Cells[10].FindControl("lnkCompStr");
                LinkButton lnkMDLIssue = (LinkButton)grdMFStatus.Rows[i].Cells[11].FindControl("lnkMDLIssue");
                LinkButton lnkFinalReport = (LinkButton)grdMFStatus.Rows[i].Cells[12].FindControl("lnkFinalReport");
                LinkButton lnkMoistcor = (LinkButton)grdMFStatus.Rows[i].Cells[13].FindControl("lnkMoistcor");
                LinkButton lnkCoverSht = (LinkButton)grdMFStatus.Rows[i].Cells[14].FindControl("lnkCoverSht");
                var MF = dc.MaterialDetail_View(0, lblReportNo.Text, 0, "", null, null, "");
                foreach (var m in MF)
                {
                    if (m.MaterialDetail_SieveAnalysis != true && m.Material_Type == "Aggregate")
                    {
                        lnkSA.Text = "Pending";
                        break;
                    }
                    else
                    {
                        lnkSA.Text = "Complete";
                    }
                }
                if (lnkSA.Text == "Complete")
                {
                    lnkSA.ForeColor = System.Drawing.Color.Green;
                    lnkSA.CssClass = "linkbtn";
                    imgBtnAddTrial.Enabled = true;
                }
                else
                {
                    imgBtnAddTrial.Enabled = false;
                }
                if (lnkTrail.Text != "")
                {
                    lnkOtherInfo.Text = "Enter";
                    lnkOtherInfo.Enabled = true;
                }
                if (lbl_TrialId.Text != "")
                {
                    var data = dc.TrialDetail_View(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text));
                    foreach (var d in data)
                    {
                        TrialStatus = Convert.ToInt32(d.Trial_Status);

                        if (d.Trial_Batching != null && d.Trial_Batching != "")
                        {
                            lnkOtherInfo.Text = "Complete";
                        }
                        if (d.Trial_Status == 1)
                        {
                            lnkMDLIssue.Enabled = true;
                            lnkMDLIssue.Text = "Enter";
                        }
                    }
                }
                if (lnkTrail.Text != "") //if (lnkOtherInfo.Text == "Complete")
                {
                    lnkTrialPt.Enabled = true;
                    lnkTrialPt.Text = "Print";//
                    lnkCompStr.Enabled = true;
                    lnkCompStr.Text = "Comp Str";
                    //
                }
                var mf = dc.MF_View(lblReportNo.Text, 0, "MF");
                foreach (var f in mf)
                {
                    if (f.MFINWD_Status_tint >= 1)
                    {
                        lnkSA.Enabled = true;
                    }
                    if (f.MFINWD_Status_tint >= 1 && lnkSA.Text == "Complete")//(f.MFINWD_Status_tint >= 1 && lnkTrail.Text != "")
                    {
                        lnkBlnkTrialPt.Text = "Print";//
                        lnkBlnkTrialPt.Enabled = true;
                    }
                    lnkMDLIssue.Text = "To be Enter";
                    lnkMDLIssue.Enabled = true;
                    lnkFinalReport.Enabled = true;
                    lnkFinalReport.Text = "To be Enter";
                    if (f.MFINWD_ReferenceNo_var == "45666/1-1")
                    {
                        lnkMDLIssue.Text = "To be Enter";
                    }
                    if (TrialStatus == 1)
                    {
                        if (f.MFINWD_Status_tint == 3)
                        {
                            lnkMDLIssue.Text = "To be Approve";
                            lnkMDLIssue.Enabled = true;
                        }
                        else if (f.MFINWD_Status_tint == 4)
                        {
                            lnkMDLIssue.Text = "To be Print";
                            lnkMDLIssue.Enabled = true;
                        }
                        else if (f.MFINWD_Status_tint == 5)
                        {
                            lnkMDLIssue.Text = "To be Outward";
                            lnkMDLIssue.Enabled = true;
                        }
                        else if (f.MFINWD_Status_tint >= 6)
                        {
                            lnkMDLIssue.Text = "Outward Complete";
                            lnkMDLIssue.Enabled = true;
                        }
                        if (f.MFINWD_Status_tint >= 3)
                        {

                            if (f.MFINWD_FinalRptStatus <= 1)
                            {
                                lnkFinalReport.Enabled = true;
                                lnkFinalReport.Text = "To be Enter";
                            }
                            if (lnkMDLIssue.Enabled == true && f.MFINWD_FinalRptStatus == 2)
                            {
                                lnkFinalReport.Enabled = true;
                                lnkFinalReport.Text = "To be Approve";
                            }
                            if (lnkMDLIssue.Enabled == true && f.MFINWD_FinalRptStatus == 3)
                            {
                                lnkFinalReport.Enabled = true;
                                lnkFinalReport.Text = "To be Print";
                            }
                            if (lnkMDLIssue.Enabled == true && f.MFINWD_FinalRptStatus == 4)
                            {
                                lnkFinalReport.Enabled = true;
                                lnkFinalReport.Text = "To be Outward";
                            }
                            if (lnkMDLIssue.Enabled == true && f.MFINWD_FinalRptStatus > 4)
                            {
                                lnkFinalReport.Enabled = true;
                                lnkFinalReport.Text = "Outward Complete";
                            }
                        }
                    }
                    else
                    {
                        lnkMDLIssue.Text = "";
                        lnkFinalReport.Text = "";
                    }
                   
                    if (f.MFINWD_Status_tint > 2)
                    {
                        lnkMoistcor.Visible = true;
                        lnkCoverSht.Visible = true;
                    }
                }
              
            }
        }
        
        protected void AddRowMF()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MFTable"] != null)
            {
                GetCurrentDataMF();
                dt = (DataTable)ViewState["MFTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("MaterialDetail_ReceivedDt", typeof(string)));
                dt.Columns.Add(new DataColumn("MaterialDetail_RefNo", typeof(string)));
                dt.Columns.Add(new DataColumn("lnkSA", typeof(string)));
                dt.Columns.Add(new DataColumn("MaterialDetail_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_TrialId", typeof(string)));
                dt.Columns.Add(new DataColumn("MaterialDetail_OtherInfo", typeof(string)));
                dt.Columns.Add(new DataColumn("MaterialDetail_MDLIssue", typeof(string)));
                dt.Columns.Add(new DataColumn("MaterialDetail_FinalReport", typeof(string)));

            }
            dr = dt.NewRow();
            dr["MaterialDetail_ReceivedDt"] = string.Empty;
            dr["MaterialDetail_RefNo"] = string.Empty;
            dr["lnkSA"] = string.Empty;
            dr["MaterialDetail_Id"] = string.Empty;
            dr["lbl_TrialId"] = string.Empty;
            dr["MaterialDetail_OtherInfo"] = string.Empty;
            dr["MaterialDetail_MDLIssue"] = string.Empty;
            dr["MaterialDetail_FinalReport"] = string.Empty;


            dt.Rows.Add(dr);
            ViewState["MFTable"] = dt;
            grdMFStatus.DataSource = dt;
            grdMFStatus.DataBind();
            SetPreviousDataMF();
        }
        protected void GetCurrentDataMF()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("MaterialDetail_ReceivedDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MaterialDetail_RefNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lnkSA", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MaterialDetail_Id", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_TrialId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MaterialDetail_OtherInfo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MaterialDetail_MDLIssue", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MaterialDetail_FinalReport", typeof(string)));

            for (int i = 0; i < grdMFStatus.Rows.Count; i++)
            {
                Label lblRecvdDate = (Label)grdMFStatus.Rows[i].Cells[1].FindControl("lblRecvdDate");
                Label lblReportNo = (Label)grdMFStatus.Rows[i].Cells[2].FindControl("lblReportNo");
                LinkButton lnkSA = (LinkButton)grdMFStatus.Rows[i].Cells[3].FindControl("lnkSA");
                LinkButton lnkTrail = (LinkButton)grdMFStatus.Rows[i].Cells[5].FindControl("lnkTrail");
                Label lbl_TrialId = (Label)grdMFStatus.Rows[i].Cells[6].FindControl("lbl_TrialId");
                LinkButton lnkOtherInfo = (LinkButton)grdMFStatus.Rows[i].Cells[7].FindControl("lnkOtherInfo");
                LinkButton lnkMDLIssue = (LinkButton)grdMFStatus.Rows[i].Cells[8].FindControl("lnkMDLIssue");
                LinkButton lnkFinalReport = (LinkButton)grdMFStatus.Rows[i].Cells[9].FindControl("lnkFinalReport");

                drRow = dtTable.NewRow();
                drRow["MaterialDetail_ReceivedDt"] = lblRecvdDate.Text;
                drRow["MaterialDetail_RefNo"] = lblReportNo.Text;
                drRow["lnkSA"] = lnkSA.Text;
                drRow["MaterialDetail_Id"] = lnkTrail.Text;
                drRow["lbl_TrialId"] = lbl_TrialId.Text;
                drRow["MaterialDetail_OtherInfo"] = lnkOtherInfo.Text;
                drRow["MaterialDetail_MDLIssue"] = lnkMDLIssue.Text;
                drRow["MaterialDetail_FinalReport"] = lnkFinalReport.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MFTable"] = dtTable;

        }
        protected void SetPreviousDataMF()
        {
            DataTable dt = (DataTable)ViewState["MFTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblRecvdDate = (Label)grdMFStatus.Rows[i].Cells[1].FindControl("lblRecvdDate");
                Label lblReportNo = (Label)grdMFStatus.Rows[i].Cells[2].FindControl("lblReportNo");
                LinkButton lnkSA = (LinkButton)grdMFStatus.Rows[i].Cells[3].FindControl("lnkSA");
                LinkButton lnkTrail = (LinkButton)grdMFStatus.Rows[i].Cells[5].FindControl("lnkTrail");
                Label lbl_TrialId = (Label)grdMFStatus.Rows[i].Cells[6].FindControl("lbl_TrialId");
                LinkButton lnkOtherInfo = (LinkButton)grdMFStatus.Rows[i].Cells[7].FindControl("lnkOtherInfo");
                LinkButton lnkMDLIssue = (LinkButton)grdMFStatus.Rows[i].Cells[8].FindControl("lnkMDLIssue");
                LinkButton lnkFinalReport = (LinkButton)grdMFStatus.Rows[i].Cells[9].FindControl("lnkFinalReport");

                lblRecvdDate.Text = dt.Rows[i]["MaterialDetail_ReceivedDt"].ToString();
                lblReportNo.Text = dt.Rows[i]["MaterialDetail_RefNo"].ToString();
                lnkSA.Text = dt.Rows[i]["lnkSA"].ToString();
                lnkTrail.Text = dt.Rows[i]["MaterialDetail_Id"].ToString();
                lbl_TrialId.Text = dt.Rows[i]["lbl_TrialId"].ToString();
                lnkOtherInfo.Text = dt.Rows[i]["MaterialDetail_OtherInfo"].ToString();
                lnkMDLIssue.Text = dt.Rows[i]["MaterialDetail_MDLIssue"].ToString();
                lnkFinalReport.Text = dt.Rows[i]["MaterialDetail_FinalReport"].ToString();


            }
        }

        protected void lnkMDLIssue_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");
         
            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");
           
            LinkButton lnkMDLIssue = (LinkButton)clickedRow.FindControl("lnkMDLIssue");
         
            if (lnkMDLIssue.Text == "" || lnkMDLIssue.Text == "To be Enter" )
            {
                //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                //string strURLWithData = "MDLetter.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&TrailId={1}&Action={2}&MDtype={3}", lblReportNo.Text, lbl_TrialId.Text, lnkMDLIssue.Text ,"MDL"));
                //Response.Redirect(strURLWithData);
            }
            else      //       if (lnkMDLIssue.Text == "Print")
            {
                //DateTime MDLIssueDt = DateTime.ParseExact(System.DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                //dc.MDLCoverSheet_Update(lblReportNo.Text, "", "", null, MDLIssueDt);
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                //reportStr = rptHtml.TrialMDLetter_Html(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text), "MF", "MDL", lnkMDLIssue.Text);
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                //rptHtml.TrialMDLetter_Html(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text), "MF", "MDL", lnkMDLIssue.Text);
                PrintPDFReport rpt = new PrintPDFReport();
                //rpt.MF_MDLetter_PDFReport(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text), "MF", "MDL", "Display");
                rpt.PrintSelectedReport("MF", lblReportNo.Text, "Display", "", "", "MDL", "", "", "", "");
            }

        }
        protected void lnkFinalReport_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");

            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");

            LinkButton lnkMDLIssue = (LinkButton)clickedRow.FindControl("lnkMDLIssue");
            LinkButton lnkFinalReport = (LinkButton)clickedRow.FindControl("lnkFinalReport");

            if (lnkFinalReport.Text == "" || lnkFinalReport.Text == "To be Check" || lnkFinalReport.Text == "To be Enter")
            {
                //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                //string strURLWithData = "MDLetter.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&TrailId={1}&Action={2}&MDtype={3}", lblReportNo.Text, lbl_TrialId.Text, lnkFinalReport.Text, "Final"));
                //Response.Redirect(strURLWithData);
            }
            else  //if (lnkFinalReport.Text == "Print")
            {
                //DateTime MDLFinalDt = DateTime.ParseExact(System.DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                //dc.MDLCoverSheet_Update(lblReportNo.Text, "", "", MDLFinalDt, null);

                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                //reportStr = rptHtml.TrialMDLetter_Html(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text), "MF", "Final", lnkFinalReport.Text);
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                PrintPDFReport rpt = new PrintPDFReport();
                //rpt.MF_MDLetter_PDFReport(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text), "MF", "Final", "Display");
                rpt.PrintSelectedReport("MF", lblReportNo.Text, "Display", "", "", "Final", "", "", "", "");
            }
        }
        protected void lnkSA_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");

            LinkButton lnkSA = (LinkButton)clickedRow.FindControl("lnkSA");
            if (lnkSA.Text == "Pending")
            {
                //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                //string strURLWithData = "Aggregate_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}", "", "", "", "", lblReportNo.Text));
                //Response.Redirect(strURLWithData);
            }
            else if (lnkSA.Text == "Complete")
            {
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                //reportStr = rptHtml.TrialSieveAnalysis_Html(Convert.ToString(lblReportNo.Text), "MF");
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                //rptHtml.TrialSieveAnalysis_Html(Convert.ToString(lblReportNo.Text), "MF");
                PrintPDFReport rpt = new PrintPDFReport();
                //rptPdf.MFSieveAnalysis_PDF(lblReportNo.Text, "MF","Print");
                rpt.PrintSelectedReport("MF", lblReportNo.Text, "Print", "", "", "Sieve Analysis", "", "", "", "");
            }
        }
        protected void imgBtnAddTrial_Click(object sender, CommandEventArgs e)
        {

            GridViewRow clickedRow = ((ImageButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");

            //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            //string strURLWithData = "Trial.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&AddNewTrial={1}&TrialId={2}", lblReportNo.Text, e.CommandName.ToString(),""));
            //Response.Redirect(strURLWithData);

        }
        protected void lnkTrail_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");

            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");

            //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            //string strURLWithData = "Trial.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&AddNewTrial={1}&TrialId={2}", lblReportNo.Text,"", lbl_TrialId.Text));
            //Response.Redirect(strURLWithData);

        }
        protected void lnkCompStr_Click(object sender, EventArgs e)
        {

            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            LinkButton lnkCompStr = (LinkButton)clickedRow.FindControl("lnkCompStr");
        
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");
          
            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");
         
            //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            ////string strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}", "", "", "", "", lblReportNo.Text, lbl_TrialId.Text, "TrialCubeCompStr", "", "", ""));
            //string strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}&Days={10}", "", "", "", "", lblReportNo.Text, lbl_TrialId.Text, "TrialCubeCompStr", "", "", "", ""));
            //Response.Redirect(strURLWithData);

        }
        protected void lnkOtherInfo_Click(object sender, EventArgs e)
        {

            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");

            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");


            //EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            //string strURLWithData = "TrialOtherInfo.aspx?" + obj.Encrypt(string.Format("ReportNo={0}&TrailId={1}", lblReportNo.Text, lbl_TrialId.Text ));
            //Response.Redirect(strURLWithData);

        }
        protected void lnkTrialPt_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");
          
            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");
          
            if (lbl_TrialId.Text == "")
            {
                lbl_TrialId.Text = "0";
            }
            RptTrialInfo(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text));

        }
        protected void lnkBlnkTrialPt_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");
           
            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");
        
            if (lbl_TrialId.Text == "")
            {
                lbl_TrialId.Text = "0";
            }
            RptBlankTrial(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text));
        }
        protected void lnkMoistcor_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");
       
            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");
         
            LinkButton lnkMDLIssue = (LinkButton)clickedRow.FindControl("lnkMDLIssue");
            LinkButton lnkFinalReport = (LinkButton)clickedRow.FindControl("lnkFinalReport");
          
            if (lbl_TrialId.Text == "")
            {
                lbl_TrialId.Text = "0";
            }
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.MoistureCorrection_PDF(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text), "Print");
            rpt.PrintSelectedReport("MF", lblReportNo.Text, "Print", "", "", "Moisture Correction", "", "", "", "");
        }
        protected void lnkCoverSht_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblReportNo = (Label)clickedRow.FindControl("lblReportNo");
          
            Label lbl_TrialId = (Label)clickedRow.FindControl("lbl_TrialId");
           
            LinkButton lnkMDLIssue = (LinkButton)clickedRow.FindControl("lnkMDLIssue");
            LinkButton lnkFinalReport = (LinkButton)clickedRow.FindControl("lnkFinalReport");
          
            if (lbl_TrialId.Text == "")
            {
                lbl_TrialId.Text = "0";
            }
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.MDLCoverSheet_PDF(lblReportNo.Text, Convert.ToInt32(lbl_TrialId.Text), "Print");
            rpt.PrintSelectedReport("MF", lblReportNo.Text, "Print", "", "", "Cover Sheet", "", "", "", "");
        }
        
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }        
        public void RptBlankTrial(string ReportNo , int TrialId)
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);

            //reportStr = getTrialProportion(ReportNo, TrialId);
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.TrialProportion_Html(ReportNo, TrialId);
        }
        //protected string getTrialProportion(string ReferenceNo, int TrialId)
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //    mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b> Trial Proportions </b></font> </td></tr>";

        //    int RecordNo = 0;
        //    var MFInwd = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, ReferenceNo, 0, 0, 0);
        //    foreach (var m in MFInwd)
        //    {

        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2>Customer Name</font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + m.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2>Record No.</font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "MF" + "-" + m.MFINWD_ReferenceNo_var + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";

        //        mySql += "<tr>" +
        //          "<td width='20%' align=left valign=top height=19><font size=2>Site Name</font></td>" +
        //           "<td width='2%' height=19><font size=2>:</font></td>" +
        //           "<td width='40%' height=19><font size=2>" + m.SITE_Name_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2>Grade of Concrete</font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + m.MFINWD_Grade_var + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td width='20%' height=19><font size=2>Special Requirement</font></td>" +
        //           "<td width='2%' height=19><font size=2>:</font></td>" +
        //           "<td width='10%' height=19><font size=2>" + m.MFINWD_SpecialRequirement_var + "</font></td>" +
        //           "<td height=19><font size=2>Nature of Work</font></td>" +
        //           "<td width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + m.MFINWD_NatureofWork_var.ToString() + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +

        //           "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //           "<td width='2%' height=19><font size=2></font></td>" +
        //           "<td width='10%' height=19><font size=2> </font></td>" +
        //           "<td height=19><font size=2> Slump Requirement </font></td>" +
        //           "<td width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + m.MFINWD_Slump_var.ToString() + "</font></td>" +
        //           "</tr>";
        //        RecordNo = Convert.ToInt32(m.MFINWD_RecordNo_int);
        //        break;
        //    }
        //    var trial = dc.TrialDetail_View(ReferenceNo, TrialId);
        //    foreach (var t in trial)
        //    {
        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2>Cement Name</font></td>" +
        //           "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + Convert.ToString(t.Trial_CementUsed) + "</font></td>" +
        //           "<td align=left valign=top height=19><font size=2> </font></td>" +
        //           "<td width='2%' height=19><font size=2> </font></td>" +
        //           "<td height=19><font size=2> </font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2> Admixture Name </font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToString(t.Trial_Admixture) + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2>Fly Ash Used Name</font></td>" +
        //            "<td width='2%' height=19><font size=2> : </font></td>" +
        //            "<td height=19><font size=2></font>" + Convert.ToString(t.Trial_FlyashUsed) + "</td>" +
        //            "</tr>";
        //        break;
        //    }

        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 8% align=left valign=top height=19 ><font size=2>&nbsp; Trial Date : </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 8% align=left valign=top height=19 ><font size=2>&nbsp; Material : </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial 1 </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial 2 </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial 3 </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial 4 </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial 5 </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial 6 </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Final Trial  </font></td>";
        //    mySql += "</tr>";

        //    int MatSrNo = 0;
        //    bool NS = false; bool CS = false; bool Grit = false; bool SD = false; bool Tenmm = false; bool Twentymm = false; bool Fortymm = false;

        //    var Mix = dc.MaterialDetail_View(RecordNo, "", 0, "", null, null);
        //    foreach (var m in Mix)
        //    {
        //        if (MatSrNo != Convert.ToInt32(m.MaterialDetail_SrNo) || MatSrNo == 0)
        //        {
        //            mySql += "<tr>";
        //            if (Convert.ToString(m.Material_List).Trim() == "Natural Sand")
        //            {
        //                NS = true;
        //            }
        //            if (Convert.ToString(m.Material_List).Trim() == "Crushed Sand")
        //            {
        //                CS = true;
        //            }
        //            if (Convert.ToString(m.Material_List).Trim() == "Grit")
        //            {
        //                Grit = true;
        //            }
        //            if (Convert.ToString(m.Material_List).Trim() == "Stone Dust")
        //            {
        //                SD = true;
        //            }
        //            if (Convert.ToString(m.Material_List).Trim() == "10 mm")
        //            {
        //                Tenmm = true;
        //            }
        //            if (Convert.ToString(m.Material_List).Trim() == "20 mm")
        //            {
        //                Twentymm = true;
        //            }
        //            if (Convert.ToString(m.Material_List).Trim() == "40 mm")
        //            {
        //                Fortymm = true;
        //            }
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.Material_List) + "</font></td>";

        //            for (int j = 0; j < 7; j++)
        //            {
        //                mySql += "<td width= 5% align=center valign=top height=19 > </td>";
        //            }
        //            mySql += "</tr>";
        //        }
        //        MatSrNo = Convert.ToInt32(m.MaterialDetail_SrNo);
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td  width= 5% align=left valign=top height=19 >&nbsp; Total </td>";
        //    for (int j = 0; j < 7; j++)
        //    {
        //        mySql += "<td width= 5% align=center valign=top height=19 > </td>";
        //    }
        //    mySql += "</tr>";

        //    mySql += "<table>";
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //    mySql += "<tr>";

        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2> Natural Sand </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 10% align=center colspan=2 valign=top height=19 ><font size=2> Crushed Sand </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2> Grit </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 2% align=center colspan=2 valign=top height=19 ><font size=2> Stone Dust </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2> 10 mm </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2> 20 mm </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2> 40 mm </font></td>";
        //    }
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Sieve Sizes </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Passing </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Sieve Sizes </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Passing </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Sieve Sizes </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Passing </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Sieve Sizes </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Passing </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Sieve Sizes </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Passing </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Sieve Sizes </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Passing </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Sieve Sizes </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Passing </font></td>";
        //    }
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 85.51 </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 87.5 </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 89.29 </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 77.78 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 40 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 88.38 </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 40 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 88.57 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 40 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 79.81 </font></td>";
        //    }
        //    mySql += "</tr>";

        //    mySql += "<tr>";

        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 4.75 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 71.02 </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 4.75 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 75 </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 4.75 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 78.58 </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 4.75 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 66.67 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 25 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 67.42 </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 25 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 77.05 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 25 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 59.62 </font></td>";
        //    }
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 2.36 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 56.53 </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 2.36 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 62.5 </font></td>";

        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 2.36 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 67.87 </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 2.36 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 55.56 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 20 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 55.8 </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 20 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 63.64 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 20 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 39.43 </font></td>";
        //    }
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 1.18 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 28.97 </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 1.18 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 50 </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 1.18 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 57.16 </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 1.18 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 44.45 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 16 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 34.84 </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 16 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 53.06 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 16 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 39.43 </font></td>";
        //    }
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 600 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 14.48 </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 600 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 37.5 </font></td>";

        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 600 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 46.45 </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 600 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 33.34 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 12.5 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 23.22 </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 12.5 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 31.91 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 12.5 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 300 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 300 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 25 </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 300 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 35.74 </font></td>";

        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 300 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 22.23 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 11.6 </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10.76 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 10 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 150 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 150 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 12.5 </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 150 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 17.87 </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 150 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 11.12 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 4.75 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 4.75 mm </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Specific Gravity </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0.69 </font></td>";
        //    }
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 75 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 75 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0 </font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 75 micron </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0.01 </font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Specific Gravity  </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 0.75 </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Water Absorption </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 22.22% </font></td>";
        //    }
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    if (NS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    if (CS == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    if (Grit == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    if (SD == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    if (Tenmm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    if (Twentymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> Water Absorption  </font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2> 33.33% </font></td>";
        //    }
        //    if (Fortymm == true)
        //    {
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //        mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2></font></td>";
        //    }
        //    mySql += "</tr>";


        //    mySql += "<table>";
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Date </font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2> Follow up </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Contact Person </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Follow up By </font></td>";
        //    mySql += "</tr>";

        //    for (int j = 0; j < 10; j++)
        //    {
        //        mySql += "<tr>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "</tr>";
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial No. </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Ent. By </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Ent. Date </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Chk. By </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Chk. Date </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Prn. By </font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Prn. Date </font></td>";
        //    mySql += "</tr>";

        //    for (int j = 0; j < 2; j++)
        //    {
        //        mySql += "<tr>";
        //        if (j == 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;MD Letter </font></td>";
        //        }
        //        else
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;Final Report </font></td>";
        //        }
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "</tr>";
        //    }

        //    mySql += "</table>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        public void RptTrialInfo( string ReportNo , int TrialId)
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = getTrialInformation(ReportNo,TrialId);
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.TrialInformation_Html(ReportNo, TrialId);
        }
        //protected string getTrialInformation(string ReferenceNo, int TrialId)
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //    mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b> Durocrete </b></font> </td></tr>";
        //    mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=3>Engineering Services Pvt. Ltd (Vitthalwadi,Sinhagad Road, Pune. Tel. No. 24345170,24348027)</font></td></tr>";
        //    mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b><u> Trial Information  </u></b></font> </td></tr>";

        //    var MFInwd = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, ReferenceNo, 0, 0, 0);
        //    foreach (var m in MFInwd)
        //    {
        //        mySql += "<tr>" +
        //            "<td width='20%' align=left valign=top height=19><font size=2>Customer Name</font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='40%' height=19><font size=2>" + m.CL_Name_var + "</font></td>" +
        //            "<td height=19><font size=2>Record No.</font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "MF" + "-" + m.MFINWD_ReferenceNo_var + "</font></td>" +
        //            "<td height=19><font size=2>&nbsp;</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //           "<td width='20%' align=left valign=top height=19><font size=2>Site Name</font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='40%' height=19><font size=2>" + m.SITE_Name_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2>Grade of Concrete</font></td>" +
        //             "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + m.MFINWD_Grade_var + "</font></td>" +
        //             "</tr>";
        //        var trial = dc.TrialDetail_View(ReferenceNo, TrialId);
        //        foreach (var t in trial)
        //        {
        //            mySql += "<tr>" +

        //                "<td width='20%' height=19><font size=2>Special Requirement</font></td>" +
        //                "<td width='2%' height=19><font size=2>:</font></td>" +
        //                "<td width='10%' height=19><font size=2>" + m.MFINWD_SpecialRequirement_var + "</font></td>" +
        //                "<td height=19><font size=2>Trial Date</font></td>" +
        //                "<td width='2%' height=19><font size=2>:</font></td>" +
        //                "<td height=19><font size=2>" + Convert.ToDateTime(t.Trial_Date).ToString("dd/MM/yyyy") + "</font></td>" +
        //                "</tr>";

        //            mySql += "<tr>" +

        //                "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //                "<td width='2%' height=19><font size=2></font></td>" +
        //                "<td width='10%' height=19><font size=2> </font></td>" +
        //                "<td height=19><font size=2>Trial Time</font></td>" +
        //                "<td width='2%' height=19><font size=2>:</font></td>" +
        //                "<td height=19><font size=2>" + Convert.ToString(t.Trial_Time) + "</font></td>" +
        //                "</tr>";

        //            mySql += "<tr>" +

        //               "<td align=left valign=top height=19><font size=2>Admixture Name</font></td>" +
        //               "<td height=19><font size=2>:</font></td>" +
        //               "<td height=19><font size=2>" + Convert.ToString(t.Trial_Admixture) + "</font></td>" +
        //               "<td align=left valign=top height=19><font size=2> Fly Ash Name</font></td>" +
        //               "<td width='2%' height=19><font size=2>:</font></td>" +
        //               "<td height=19><font size=2>" + Convert.ToString(t.Trial_FlyashUsed) + "</font></td>" +
        //               "</tr>";

        //            mySql += "<tr>" +
        //                "<td align=left valign=top height=19><font size=2> Cement Name </font></td>" +
        //                "<td width='2%' height=19><font size=2>:</font></td>" +
        //                "<td height=19><font size=2>" + Convert.ToString(t.Trial_CementUsed) + "</font></td>" +
        //                "<td align=left valign=top height=19><font size=2><b> </b></font></td>" +
        //                "<td height=19><font size=2> </font></td>" +
        //                "<td height=19><font size=2></font></td>" +
        //                "</tr>";
        //            break;
        //        }
        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2>Target Mean Strength</font></td>" +
        //              "<td height=19><font size=2>=</font></td>" +
        //              "<td height=19><font size=2>   </font></td>" +
        //             "<td align=left valign=top height=19><font size=2> </font></td>" +
        //            "<td height=19><font size=2> </font></td>" +
        //            "<td height=19><font size=2>" + "" + "</font></td>" +
        //            "</tr>";
        //    }

        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 15% align=center valign=top height=19 ><font size=2><b>Material</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Material Proportions</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Required Wt.(kg)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Corrections </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Net Wt. Taken(kg) </b></font></td>";
        //    mySql += "</tr>";

        //    var res = dc.TrialDetail_View(ReferenceNo, TrialId);
        //    foreach (var t in res)
        //    {
        //        mySql += "<tr>";
        //        mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_Weight) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(t.TrialDetail_ReqdWt).ToString("0.00") + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_Corrections)+ "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(t.TrialDetail_NetWeight).ToString("0.00") + "</font></td>";
        //        mySql += "</tr>";

        //    }

        //    var data = dc.TrialDetail_View(ReferenceNo, TrialId);
        //    foreach (var t in data)
        //    {

        //        mySql += "<table>";
        //        mySql += "<tr>";
        //        if (t.Trial_MC_NS != "" && t.Trial_MC_NS != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Natural Sand " + " " + ":" + " " + Convert.ToString(t.Trial_MC_NS) + " % " + "</font></td>";
        //        }

        //        if (t.Trial_MC_CS != "" && t.Trial_MC_CS != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Crushed Sand " + " " + ":" + " " + Convert.ToString(t.Trial_MC_CS) + " % " + "</font></td>";

        //        }
        //        mySql += "</tr>";
        //        mySql += "<tr>";
        //        if (t.Trial_MC_SD != "" && t.Trial_MC_SD != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Stone Dust " + " " + ":" + " " + Convert.ToString(t.Trial_MC_SD) + " % " + "</font></td>";
        //        }
        //        if (t.Trial_MC_GT != "" && t.Trial_MC_GT != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>" + "'Moist'. " + "Moisture Content" + "-" + "Grit " + " " + ":" + " " + Convert.ToString(t.Trial_MC_SD) + " % " + "</font></td>";
        //        }
        //        mySql += "</tr>";
        //        mySql += "<tr>";
        //        if (t.Trial_WA_NS != "" && t.Trial_WA_NS != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Natural Sand " + " " + ":" + " " + Convert.ToString(t.Trial_WA_NS) + " % " + "</font></td>";
        //        }
        //        if (t.Trial_WA_CS != "" && t.Trial_WA_CS != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Crushed Sand " + " " + ":" + " " + Convert.ToString(t.Trial_WA_CS) + " % " + "</font></td>";
        //        }
        //        mySql += "</tr>";
        //        mySql += "<tr>";
        //        if (t.Trial_WA_SD != "" && t.Trial_WA_SD != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Stone Dust " + " " + ":" + " " + Convert.ToString(t.Trial_WA_SD) + " % " + "</font></td>";
        //        }
        //        if (t.Trial_WA_GT != "" && t.Trial_WA_GT != null)
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>" + "Water Absorption" + "-" + "Grit " + " " + ":" + " " + Convert.ToString(t.Trial_WA_GT) + " % " + "</font></td>";
        //        }
        //        mySql += "</tr>";

        //        mySql += "<tr>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Final Water Cement Ratio" + " " + ":" + " " + "</font></td>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Wt. Of Compacted Concrete" + " " + ":" + " " + "</font></td>";

        //        mySql += "</tr>";

        //        mySql += "<tr>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Slump " + " " + ":" + " " + "</font></td>";

        //        string Slump = "";
        //        if (t.Trial_RetentionStatus == true)
        //        {
        //            Slump = t.Trial_RetentionSlumpValue.ToString().Replace("|", ",");
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Retention Slump" + " " + ":" + " " + "(" + " " + Slump + " " + ")" + "</font></td>";
        //        }
        //        mySql += "</tr>";

        //        mySql += "<tr>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Yield " + " " + ":" + " " + "</font></td>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> </font></td>";

        //        mySql += "</tr>";
        //        mySql += "<tr>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Compaction Factor " + " " + ":" + " " + "</font></td>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Segregation  " + " " + ":" + " " + "</font></td>";

        //        mySql += "</tr>";
        //        mySql += "<tr>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Remark " + " " + ":" + Convert.ToString(t.Trial_Remark) + "</font></td>";

        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> </font></td>";

        //        mySql += "</tr>";
        //        break;
        //    }

        //    mySql += "<table width=100%>";
        //    mySql += "<tr>";
        //    mySql += "<td width=8% align=left valign=top height=20 ><font size=2>" + "Cast By " + "</font></td>";
        //    mySql += "<td width=25%  align=left valign=top height=20 ><font size=2><p><hr>&nbsp;</p></font></td>";
        //    mySql += "<td width=8% align=right valign=top height=20 ><font size=2>" + "CE By " + "</font></td>";
        //    mySql += "<td width=25%  align=left valign=top height=20 ><font size=2><p><hr>&nbsp;</p></font></td>";
        //    mySql += "<td width=10% align=right valign=top height=20 ><font size=2>" + "Checked By " + "</font></td>";
        //    mySql += "<td width=20%  align=left valign=top height=20 ><font size=2><p><hr></p></font></td>";
        //    mySql += "</tr>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Id Mark </b></font></td>";
        //    mySql += "<td width= 10% align=center colspan=3 valign=top height=19 ><font size=2><b> Dimensions(mm) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Weight </b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Age (Days) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Load (kN) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Str(N/mm<sup>2</sup>) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Avg. Str. </b></font></td>";
        //    mySql += "</tr>";

        //    for (int j = 0; j < 10; j++)
        //    {
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
        //        mySql += "</tr>";
        //    }

        //    mySql += "</table>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
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
    }
}