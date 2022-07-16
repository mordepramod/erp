using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace DESPLWEB
{
    public partial class ReportStatus : System.Web.UI.Page
    {
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ViewState["ReffUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Report Status";
                LoadInwarDType();
                getCurrentDate();
           
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        public void getCurrentDate()
        {
            txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
            txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtTestingDt.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        private void LoadInwarDType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select---");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdReportStatus.Visible = true;
            DisplayReportStatus();
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
        public void DisplayReportStatus()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            DateTime Testingdt = DateTime.ParseExact(txtTestingDt.Text, "dd/MM/yyyy", null);

            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            
            if (ddl_InwardTestType.SelectedItem.Text == "Steel Testing")
            {
                var Inward = dc.ReportStatus_View_SteelTesting(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, Convert.ToByte(ddl_ReportType.SelectedValue), ClientId, Convert.ToInt32(ddlMailStatus.SelectedValue)).ToList();
                grdReportStatus.DataSource = Inward;
                grdReportStatus.DataBind();

            }
            else if (ddl_InwardTestType.SelectedItem.Text == "Other Testing")
            {
                var Inward = dc.ReportStatus_View_OtherTesting(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, Convert.ToByte(ddl_ReportType.SelectedValue), ClientId, Convert.ToInt32(ddlMailStatus.SelectedValue)).ToList();
                grdReportStatus.DataSource = Inward;
                grdReportStatus.DataBind();
            }
            else if (ddl_InwardTestType.SelectedItem.Text == "GGBS Chemical Testing" || ddl_InwardTestType.SelectedItem.Text == "GGBS Testing")
            {
                var Inward = dc.ReportStatus_View_GGBSTesting(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, Convert.ToByte(ddl_ReportType.SelectedValue), ClientId, Convert.ToInt32(ddlMailStatus.SelectedValue)).ToList();
                grdReportStatus.DataSource = Inward;
                grdReportStatus.DataBind();
            }
            else
            {
                var Inward = dc.ReportStatus_View_All(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate, Convert.ToByte(ddl_ReportType.SelectedValue), ClientId,0, Convert.ToInt32(ddlMailStatus.SelectedValue)).ToList();
                grdReportStatus.DataSource = Inward;
                grdReportStatus.DataBind();
            }
            lbl_RecordsNo.Text = "Total Records   :  " + grdReportStatus.Rows.Count;
            bool entryRight = false, checkRight = false, approveRight = false, viewRight = false, printRight = false;
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                if (u.USER_Entry_right_bit == true)
                    entryRight = true;
                if (u.USER_Check_right_bit == true)
                    checkRight = true;
                if (u.USER_RptApproval_right_bit == true)
                    approveRight = true;
                if (u.USER_View_right_bit == true)
                    viewRight = true;
                if (u.USER_View_right_bit == true)
                    printRight = true;
            }
            for (int i = 0; i < grdReportStatus.Rows.Count; i++)
            {
                LinkButton lnkEnterReport = (LinkButton)grdReportStatus.Rows[i].FindControl("lnkEnterReport");
                LinkButton lnkViewReport = (LinkButton)grdReportStatus.Rows[i].FindControl("lnkViewReport");
                if (lnkEnterReport.Text == "Enter" || lnkEnterReport.Text == "Check")
                {
                    lnkViewReport.Visible = false;
                }
                else if (lnkEnterReport.Text == "Start Test")
                {
                    lnkEnterReport.Visible = false;
                    lnkViewReport.Visible = false;
                }
                else
                {
                    lnkViewReport.Visible = true;
                    lnkEnterReport.Visible = false;
                }
                lnkEnterReport.Enabled = false;
                lnkViewReport.Enabled = false;//(lnkEnterReport.Text == "Start Test") ||
                if ((lnkEnterReport.Text == "Enter" && entryRight == true) ||
                    (lnkEnterReport.Text == "Check" && checkRight == true) ||
                    (lnkEnterReport.Text == "Approve" && approveRight == true) ||
                    (lnkEnterReport.Text == "Print" && printRight == true) ||
                    (lnkEnterReport.Text == "View" && viewRight == true) ||
                    lnkEnterReport.Text == "Outward" || lnkEnterReport.Text == "Physical Outward")
                {
                    lnkEnterReport.Enabled = true;
                }
               
                if (lnkViewReport.Text == "View" && viewRight == true)
                {
                    lnkViewReport.Enabled = true;
                }
            }

        }

        protected void grdReportStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                grdReportStatus.Columns[22].Visible = false;
                grdReportStatus.Columns[7].Visible = false;
                grdReportStatus.Columns[9].Visible = false;

                LinkButton lnkEnterReport = (LinkButton)e.Row.FindControl("lnkEnterReport");
                if (e.Row.Cells[8].Text == "Test To be Started")
                {
                    lnkEnterReport.Text = "Start Test";
                }
                else if (e.Row.Cells[8].Text == "To be Entered")
                {
                    lnkEnterReport.Text = "Enter";
                }
                else if (e.Row.Cells[8].Text == "To be Checked")
                {
                    lnkEnterReport.Text = "Check";
                    if (e.Row.Cells[2].Text == "NDT")
                    {
                        LinkButton lnkReEnterReport = (LinkButton)e.Row.FindControl("lnkReEnterReport");
                        lnkReEnterReport.Visible = true;
                    }
                }
                else if (e.Row.Cells[8].Text == "To be Approved")
                {
                    lnkEnterReport.Text = "Approve";
                    if (e.Row.Cells[2].Text == "NDT")
                    {
                        LinkButton lnkReEnterReport = (LinkButton)e.Row.FindControl("lnkReEnterReport");
                        lnkReEnterReport.Visible = true;
                    }
                }
                else if (e.Row.Cells[8].Text == "To be Printed")
                {
                    lnkEnterReport.Text = "Print";
                }
                else if (e.Row.Cells[8].Text == "To be Outward")
                {
                    lnkEnterReport.Text = "Outward";
                }
                else if (e.Row.Cells[8].Text == "Outward Complete")
                {
                    lnkEnterReport.Text = "Physical Outward";
                }
                else
                {
                    lnkEnterReport.Text = "Invalid";
                }
                if (e.Row.Cells[2].Text == "CEMT" || e.Row.Cells[2].Text == "FLYASH" || e.Row.Cells[2].Text == "GGBS")
                {
                    grdReportStatus.Columns[9].Visible = true;
                    LinkButton lnk_CubeCasting = (LinkButton)e.Row.FindControl("lnk_CubeCasting");
                    if (e.Row.Cells[9].Text != "&nbsp;")
                    {
                        lnk_CubeCasting.Text = e.Row.Cells[9].Text;
                        e.Row.Cells[9].Text = "";
                        lnk_CubeCasting.Visible = true;
                        //grdReportStatus.Columns[9].Visible = true;
                    }
                    grdReportStatus.Columns[22].Visible = true;
                    grdReportStatus.Columns[7].Visible = true;
                }
                if (e.Row.Cells[2].Text == "GT")
                {
                    LinkButton lnkGTSample = (LinkButton)e.Row.FindControl("lnkGTSample");
                    LinkButton lnkGTSummary = (LinkButton)e.Row.FindControl("lnkGTSummary");
                    lnkGTSample.Visible = true;
                    lnkGTSummary.Visible = true;
                    lnkEnterReport.Visible = false;
                }
                if (e.Row.Cells[2].Text == "TILE" || e.Row.Cells[2].Text == "PT" || e.Row.Cells[2].Text == "SOLID")
                {
                    grdReportStatus.Columns[7].Visible = true;
                }
            }

        }

        protected void grdReportStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[1];
            arg = strReportDetails.Split(';');

            string Recordtype = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            DateTime CollectionDate = Convert.ToDateTime(arg[2]);
            string EmailId = Convert.ToString(arg[3]);
            string ReferenceNo = Convert.ToString(arg[arg.GetUpperBound(0)]);

            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            LinkButton lnkEnterReport = (LinkButton)grdrow.FindControl("lnkEnterReport");
            int rowindex = grdrow.RowIndex;
            if (e.CommandName == "EnterReport")
            {
                LinkButton lnkEnterReportt = (LinkButton)grdReportStatus.Rows[rowindex].FindControl("lnkEnterReport");
                BindForm(Recordtype, RecordNo, ReferenceNo, lnkEnterReport.Text);                    
            }
            if (e.CommandName == "ReEnterReport")
            {
                BindForm(Recordtype, RecordNo, ReferenceNo, "ReEnter");
            }
            if (e.CommandName == "ViewReport")
            {
                var chkBalance = dc.Inward_View_ClientBalance(ReferenceNo, Recordtype);
                if (chkBalance.Count() > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client is blocked as per credit policy, can not view report.');", true);
                }
                else
                {
                    LinkButton lnkViewReport = (LinkButton)grdReportStatus.Rows[rowindex].FindControl("lnkViewReport");
                    //PrintReportDetails(Recordtype, ReferenceNo, lnkViewReport.Text);
                    if (Recordtype != "MF")
                    {
                        PrintPDFReport rpt = new PrintPDFReport();
                        rpt.PrintSelectedReport(Recordtype, ReferenceNo, lnkViewReport.Text, "", "", "", "", "", "", "");
                    }
                }
            }
            if (e.CommandName == "CubeCasting")
            {
                string strURLWithData = "CubeCompStrength.aspx?" + obj.Encrypt(string.Format("RecType={0}&RefNo={1}", Recordtype, ReferenceNo));
                Response.Redirect(strURLWithData);
            }
            if (e.CommandName == "EnterEditGTSample")
            {
                string strURLWithData = "GTSamplesDetails.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "GT", RecordNo, ReferenceNo, "", "Edit"));
                Response.Redirect(strURLWithData);
            }
            if (e.CommandName == "EnterEditGTSummary")
            {
                string strURLWithData = "GTSummary.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "GT", RecordNo, ReferenceNo, "", "Edit"));
                Response.Redirect(strURLWithData);
            }
        }

        public void BindForm(string Rectype, int RecordNo, string RefNo, string Action)
        {

            string strURLWithData = "";
            switch (Rectype)
            {
                case "AAC":
                    var detailsAAC = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AAC");
                    foreach (var aac in detailsAAC)
                    {
                        //if (Convert.ToString(aac.AACINWD_TEST_Id) == "714")
                        if (Convert.ToString(aac.TEST_Sr_No) == "1")
                        {
                            strURLWithData = "AAC_Report_CS.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "AAC", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);


                        }
                        else if (Convert.ToString(aac.TEST_Sr_No) == "2")
                        {
                            strURLWithData = "AAC_Report_DS.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "AAC", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);

                        }
                        else if (Convert.ToString(aac.TEST_Sr_No) == "3")
                        {
                            strURLWithData = "AAC_Report_DM.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "AAC", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);


                        }
                        else if (Convert.ToString(aac.TEST_Sr_No) == "4")
                        {
                            strURLWithData = "AAC_Report_SN.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "AAC", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);

                        }
                    }
                    break;
                case "CT":

                    //strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}", "CT", RecordNo, RefNo, Action, "", "", "", "", "", ""));
                    strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}&Days={10}", "CT", RecordNo, RefNo, Action, "", "", "", "", "", "", ""));
                    Response.Redirect(strURLWithData);
                    break;
                case "PILE":
                    strURLWithData = "Pile_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "PILE", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "WT":

                    strURLWithData = "Water_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "WT", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "STC":
                    strURLWithData = "STC_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "STC", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "CCH":
                    strURLWithData = "CementChemical_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "CCH", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "GGBSCH":
                    strURLWithData = "GgbsChemical_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "GGBSCH", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                
                case "ST":
                    strURLWithData = "Steel_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "ST", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "SOILD":
                    strURLWithData = "Solid_Masonary.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "SOLID", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "OT":
                    strURLWithData = "Other_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "OT", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "CEMT":

                    strURLWithData = "Cement_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&CubeCompStr={4}", "CEMT", RecordNo, RefNo, Action, ""));
                    Response.Redirect(strURLWithData);
                    break;
                case "FLYASH":

                    strURLWithData = "FlyAsh_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&CubeCompStr={4}", "FLYASH", RecordNo, RefNo, Action, ""));
                    Response.Redirect(strURLWithData);

                    break;
                case "GGBS":
                    strURLWithData = "Ggbs_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&CubeCompStr={4}", "GGBS", RecordNo, RefNo, Action, ""));
                    Response.Redirect(strURLWithData);

                    break;
                case "SOLID":
                    var details = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "SOLID");
                    foreach (var solid in details)
                    {
                        if (Convert.ToString(solid.SOLIDINWD_TEST_Id) == "66")
                        {
                            strURLWithData = "Solid_Report_CS.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "SOLID", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);


                        }
                        else if (Convert.ToString(solid.SOLIDINWD_TEST_Id) == "67")
                        {
                            strURLWithData = "Solid_Report_WA.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "SOLID", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);

                        }
                    }
                    break;
                case "PT":
                    var PT_Test = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "PT");
                    foreach (var pt in PT_Test)
                    {

                        if (Convert.ToString(pt.PTINWD_TEST_Id) == "62")
                        {
                            strURLWithData = "Pavment_Report_CS.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "PT", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);


                        }
                        else if (Convert.ToString(pt.PTINWD_TEST_Id) == "63")
                        {
                            strURLWithData = "Pavment_Report_WA.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "PT", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);

                        }
                        else if (Convert.ToString(pt.PTINWD_TEST_Id) == "64")
                        {
                            strURLWithData = "Pavment_Report_TS.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "PT", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);


                        }
                        else if (Convert.ToString(pt.PTINWD_TEST_Id) == "65")
                        {
                            strURLWithData = "Pavment_Report_FS.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "PT", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);

                        }
                    }
                    break;
                case "CR":

                    strURLWithData = "Core_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "CR", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "NDT":
                    if (Action != "Enter")
                    {
                        var ndtWbs = dc.NDTWBS_View_All(RefNo, 0, "", "", "", "").ToList();
                        if (ndtWbs.Count() == 0)
                        {
                            strURLWithData = "NDT_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "NDT", RecordNo, RefNo, Action));
                            Response.Redirect(strURLWithData);
                        }
                    }
                    break;
                case "AGGT":
                    strURLWithData = "Aggregate_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}", "AGGT", RecordNo, RefNo, Action, ""));
                    Response.Redirect(strURLWithData);

                    break;
                case "BT-":

                    strURLWithData = "Brick_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "BT-", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);

                    break;
                case "SO":

                    strURLWithData = "Soil_Report_Sample.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "SO", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);
                    break;
                case "TILE":

                    strURLWithData = "Tile_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}", "TILE", RecordNo, RefNo, Action));
                    Response.Redirect(strURLWithData);
                    break;
            }

        }
        
        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdReportStatus.Visible = false;
            lbl_RecordsNo.Text = "";
            txtTestingDt.Visible = false;
            lblTestingDt.Visible = false;
            if (ddl_InwardTestType.SelectedItem.Text == "Cube Testing")
            {
                txtTestingDt.Visible = true;
                lblTestingDt.Visible = true;
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReportStatus.Rows.Count > 0 && grdReportStatus.Visible == true)
            {
                string reportStr = "";
                reportStr = RptReportStatus();
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.DownloadHtmlReport("ReportStatus", reportStr);
            }
        }

        protected string RptReportStatus()
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
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Report  Status </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b>  Mat. Recd  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Total No of Records  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + grdReportStatus.Rows.Count + "</font></td>" +
               "</tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Inward Type  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + ddl_InwardTestType.SelectedItem.Text + "  -  " + " ( " + ddl_ReportType.SelectedItem.Text + " )" + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Mail Status  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + ddlMailStatus.SelectedItem.Text + "</font></td>" +
               "</tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";

            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Client Name </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Site Name  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Record Type </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Record No  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Reference No </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Collection Date </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Received Date </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>  Test  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Status  </b></font></td>";

            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Tested Date  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Entered Date  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Checked Date  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Printed Date  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Approved Date  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Route  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Marketing Executive  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Mail Status  </b></font></td>";


            for (int i = 0; i < grdReportStatus.Rows.Count; i++)
            {
                //Label lbl_RefNo = (Label)grdReportStatus.Rows[i].Cells[2].FindControl("lbl_RefNo");
                //Label lbl_status = (Label)grdReportStatus.Rows[i].Cells[6].FindControl("lbl_status");
                mySql += "<tr>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[0].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[1].Text + "</font></td>";
                //mySql += "<td width = 5% align=center valign=top height=19 ><font size=2>&nbsp;" + lbl_RefNo.Text + "</font></td>";
                mySql += "<td width = 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[2].Text + "</font></td>";
                mySql += "<td width=  5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[3].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[4].Text + "</font></td>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[5].Text + "</font></td>";
                //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + lbl_status.Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[6].Text + "</font></td>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[7].Text + "</font></td>";                
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[8].Text + "</font></td>";

                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[10].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[11].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[12].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[13].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[14].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[15].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[16].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportStatus.Rows[i].Cells[19].Text + "</font></td>";
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
