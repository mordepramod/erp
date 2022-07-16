using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;

namespace DESPLWEB
{
    public partial class ClientHome : System.Web.UI.Page
    {
        Int32 cnt = 0;
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["clientId"]) == "0")
                Response.Redirect("default.aspx");
            if (!IsPostBack)
            {
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                }
                string[] arrMsgs = strReq.Split('=');
                lblLocation.Text = arrMsgs[1].ToString();
                //if (arrMsgs[1].ToString().Equals("Pune"))
                //    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=VeenaLive;User ID=dipl;Password=dipl2020";
                //else if (arrMsgs[1].ToString().Equals("Mumbai"))
                //    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=VeenaMumbai;User ID=dipl;Password=dipl2020";
                //else if (arrMsgs[1].ToString().Equals("Nashik"))
                //    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=VeenaNashik;User ID=dipl;Password=dipl2020";
                if (arrMsgs[1].ToString().Equals("Pune"))
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrPune"].ToString();
                else if (arrMsgs[1].ToString().Equals("Mumbai"))
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrMumbai"].ToString();
                else if (arrMsgs[1].ToString().Equals("Nashik")) //Nashik
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrNashik"].ToString();
                lblConnectionLive.Text = lblConnection.Text;
                myDataComm myData = new myDataComm(lblConnection.Text);
                lblclientName.Text = myData.getClientName(Convert.ToDouble(Session["ClientID"].ToString()), ddl_db.SelectedItem.Value);
                //set default connection after login

                LoadSiteList();
            }
        }

        protected void LoadSiteList()
        {
            myDataComm myData = new myDataComm(lblConnection.Text);
            DataTable dt = new DataTable();

            ddlSite.Items.Clear();
            ddlSupplier.Items.Clear();
            ddlBuildings.Items.Clear();
            //ddlInward.Items.Clear();
            grdView.DataSource = null;
            grdView.DataBind();

            dt = myData.getSiteList(Convert.ToDouble(Session["ClientID"].ToString()), ddl_db.SelectedItem.Value.ToString());
            ddlSite.DataSource = dt;
            ddlSite.DataTextField = dt.Columns[0].ToString();
            ddlSite.DataValueField = dt.Columns[1].ToString();
            ddlSite.DataBind();
            if (ddlSite.Items.Count > 0)
                ddlSite.Items.Insert(0, "---Select---");
        }

        protected void LoadListOfSupplier()
        {
            myDataComm myData = new myDataComm(lblConnection.Text);
            DataTable dt = new DataTable();

            ddlSupplier.Items.Clear();
            ddlBuildings.Items.Clear();
            //ddlInward.Items.Clear();
            grdView.DataSource = null;
            grdView.DataBind();

            dt = myData.getSupplierList(Convert.ToDouble(Session["ClientID"].ToString()), Convert.ToDouble(Session["siteID"].ToString()), ddl_db.SelectedItem.Value);
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = dt.Columns[0].ToString();
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, "---All---");
        }

        protected void LoadListOfBuilding()
        {
            myDataComm myData = new myDataComm(lblConnection.Text);
            DataTable dt = new DataTable();

            ddlBuildings.Items.Clear();
            //ddlInward.Items.Clear();
            grdView.DataSource = null;
            grdView.DataBind();

            dt = myData.getBuildingList(Convert.ToDouble(Session["ClientID"].ToString()), Convert.ToDouble(Session["siteID"].ToString()), ddlSupplier.SelectedValue, ddl_db.SelectedItem.Value);
            ddlBuildings.DataSource = dt;
            ddlBuildings.DataTextField = dt.Columns[0].ToString();
            ddlBuildings.DataBind();
            if (ddlBuildings.Items.Count > 0)
                ddlBuildings.Items.Insert(0, "---All---");
        }

        //protected void LoadListOfTestType()
        //{
        //    myDataComm myData = new myDataComm(lblConnection.Text);
        //    DataTable dt = new DataTable();
        //    ddlInward.Items.Clear();
        //    grdView.DataSource = null;
        //    grdView.DataBind();
        //    dt = myData.getInwardTypeList(Convert.ToDouble(Session["ClientID"].ToString()), Convert.ToDouble(Session["siteID"].ToString()), ddlSupplier.SelectedValue, ddlBuildings.SelectedValue, ddl_db.SelectedItem.Value);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        switch (dt.Rows[i].ItemArray[0].ToString())
        //        {
        //            case "AGGT":
        //                ddlInward.Items.Add(new ListItem("Aggregate Testing", "AGGT"));
        //                break;
        //            case "AAC":
        //                ddlInward.Items.Add(new ListItem("AAC Block Testing", "AAC"));
        //                break;
        //            case "BT-":
        //                ddlInward.Items.Add(new ListItem("Brick Testing", "BT-"));
        //                break;
        //            case "CCH":
        //                ddlInward.Items.Add(new ListItem("Cement Chemical Testing", "CCH"));
        //                break;
        //            case "CEMT":
        //                ddlInward.Items.Add(new ListItem("Cement Testing", "CEMT"));
        //                break;
        //            case "CR":
        //                ddlInward.Items.Add(new ListItem("Core Testing", "CR"));
        //                break;
        //            case "CT":
        //                ddlInward.Items.Add(new ListItem("Cube Testing", "CT"));
        //                break;
        //            case "FLYASH":
        //                ddlInward.Items.Add(new ListItem("Fly Ash Testing", "FLYASH"));
        //                break;
        //            case "SOLID":
        //                ddlInward.Items.Add(new ListItem("Masanry Block Testing", "SOLID"));
        //                break;
        //            case "MF":
        //                ddlInward.Items.Add(new ListItem("Mix Design", "MF"));
        //                break;
        //            case "NDT":
        //                ddlInward.Items.Add(new ListItem("Non Destructive Testing", "NDT"));
        //                break;
        //            case "PT":
        //                ddlInward.Items.Add(new ListItem("Pavement Block Testing", "PT"));
        //                break;
        //            case "PILE":
        //                ddlInward.Items.Add(new ListItem("Pile Testing", "PILE"));
        //                break;
        //            case "ST":
        //                ddlInward.Items.Add(new ListItem("Steel Tesing", "ST"));
        //                break;
        //            case "STC":
        //                ddlInward.Items.Add(new ListItem("Steel Chemical Testing", "STC"));
        //                break;
        //            case "TILE":
        //                ddlInward.Items.Add(new ListItem("Tile Testing", "TILE"));
        //                break;
        //            case "WT":
        //                ddlInward.Items.Add(new ListItem("Water Testing", "WT"));
        //                break;                   
        //            case "SO":
        //                ddlInward.Items.Add(new ListItem("Soil Testing", "SO"));
        //                break;
        //                //case "GT":
        //                //    ddlInward.Items.Add(new ListItem("Soil Investigation", "GT")); 
        //                //    break;

        //                //case "RWH":
        //                //    ddlInward.Items.Add(new ListItem("Rain Water Harvesting", "RWH"));
        //                //    break;
        //        }
        //    }
        //    if (ddlInward.Items.Count > 0)
        //        ddlInward.Items.Insert(0, "---All---");
        //}

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            lblReport.Visible = false;
            //temporary commented on 27-02-2020 needs to change condition of limit
            //LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            //bool balFlag = false;
            //var client = dc.Client_View(Convert.ToInt32(Session["clientId"]), 0, "", "");
            //foreach (var cl in client)
            //{
            //    //if (cl.CL_OutstandingAmt_var != null && cl.CL_Limit_mny != null)
            //    //{
            //    //    string[] strAmt = cl.CL_OutstandingAmt_var.Split('|');
            //    //    decimal BalanceAmt = Convert.ToDecimal(strAmt[0]);
            //    //    if (BalanceAmt >= cl.CL_Limit_mny)
            //    //    {
            //    //        balFlag = true;
            //    //    }
            //    //}
            //    if (cl.CL_BalanceAmt_mny != null && cl.CL_Limit_mny != null)
            //    {
            //        if (cl.CL_BalanceAmt_mny >= cl.CL_Limit_mny && cl.CL_BalanceAmt_mny > 200)
            //        {
            //            balFlag = true;
            //        }
            //    }
            //}
            //if (balFlag == false)
            //{
                bindData();
            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Credit limit exceeded, So can not display report.');", true);
            //}

        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSite.Visible = false;
            Session["siteID"] = ddlSite.SelectedItem.Value.ToString();
            grdView.DataSource = null;
            grdView.DataBind();
            ddlSupplier.Items.Clear();
            ddlBuildings.Items.Clear();
            //ddlInward.Items.Clear();
            if (ddlSite.SelectedIndex > 0)
            {
                LoadListOfSupplier();
                LoadListOfBuilding();
                //LoadListOfTestType();
            }
        }

        protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadListOfBuilding();
            //LoadListOfTestType();
            grdView.DataSource = null;
            grdView.DataBind();
        }

        protected void ddlBuildings_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadListOfTestType();
            grdView.DataSource = null;
            grdView.DataBind();
        }

        //protected void ddlInward_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    lblInward.Visible = false;
        //    grdView.DataSource = null;
        //    grdView.DataBind();
        //}

        protected void grdView_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblReport.Visible = false;
            string myRefNo;
            string myRecordType = "";
            string myTestType = "";
            if (grdView.SelectedRow.Cells[5].Text == "Ready")
            {
                //clsData cd = new clsData();
                myRefNo = grdView.SelectedRow.Cells[6].Text;
                Label lblTestName = (Label)grdView.SelectedRow.FindControl("lblTestName");
                //myTestType = grdView.SelectedRow.Cells[1].Text;
                myTestType = lblTestName.Text;
                if (myTestType == "AAC Block")
                    myRecordType = "AAC";
                else if(myTestType == "Aggregate")
                    myRecordType = "AGGT";
                else if (myTestType == "Brick")
                    myRecordType = "BT-";
                else if (myTestType == "Cement Chemical")                
                    myRecordType = "CCH";
                else if (myTestType == "Cement")
                    myRecordType = "CEMT";
                else if (myTestType == "GGBS")
                    myRecordType = "GGBS";
                else if (myTestType == "GGBS Chemical")
                    myRecordType = "GGBSCH";
                else if (myTestType == "Core Cutting")
                    myRecordType = "CORECUT";
                else if (myTestType == "Core")
                    myRecordType = "CR";
                else if (myTestType == "Cube")
                    myRecordType = "CT";
                else if (myTestType == "FlyAsh")
                    myRecordType = "FLYASH";
                else if (myTestType == "Soil Investigation")
                    myRecordType = "GT";
                else if (myTestType == "Mix Design")
                    myRecordType = "MF";
                else if (myTestType == "Non Destructive")
                    myRecordType = "NDT";
                else if (myTestType == "Pile")
                    myRecordType = "PILE";
                else if (myTestType == "Pavement Block")
                    myRecordType = "PT";
                else if (myTestType == "Soil")
                    myRecordType = "SO";
                else if (myTestType == "Masonary Block")
                    myRecordType = "SOLID";
                else if (myTestType == "Steel")
                    myRecordType = "ST";
                else if (myTestType == "Steel Chemical")
                    myRecordType = "STC";
                else if (myTestType == "Tile")
                    myRecordType = "TILE";
                else if (myTestType == "Water")
                    myRecordType = "WT";
                else if (myTestType == "Other")
                    myRecordType = "OT";
                else if (myTestType == "Rain Water Harvesting")
                    myRecordType = "RWH";
                // bill approval conditions commented on 17/06/21
                //string[] strRefNo = myRefNo.Split('/');
                //myDataComm myData = new myDataComm(lblConnection.Text);
                //if (HttpContext.Current.Session["databasename"].ToString() == "veena2016" &&
                //    myData.checkBillApprovalStatus(strRefNo[0]) == true )
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill approval is pending for selected report " + myRefNo + ", So can not display report.');", true);
                //}
                //else
                //{
                    ViewReport(myRefNo, myRecordType);
                //}
            }
            else
            {
                //lblMessage.Text = "Report do not ready ..";
                //lblMessage.Visible = true;
                lblReport.Visible = true;
            }
        }
    
        protected void ViewReport(string RefNo, string RecType)
        {
            bool blockClient = false;
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);

            var client = dc.Client_View(Convert.ToInt32(Session["clientId"]), 0, "", "");
            foreach (var cl in client)
            {
                if (cl.CL_ByPassCRLimitChecking_bit == false)
                {
                    if (cl.CL_BlockStatus_bit == true
                        || (cl.CL_Limit_mny > 0 && cl.CL_BalanceAmt_mny > cl.CL_Limit_mny))
                    {
                        blockClient = true;
                    }
                }
            }
            if (blockClient == true)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Access has blocked, can not display report.');", true);
            }
            else
            {
                if (ddl_db.SelectedValue == "veena2016" || ddl_db.SelectedValue == "veena2020")
                {
                    //string strAction = "DisplayLogoWithNABL";
                    string strAction = "Automail";
                    PrintPDFReport rpt = new PrintPDFReport(lblConnection.Text);
                    rpt.PrintSelectedReport(RecType, RefNo, strAction, "0", "", "MDL", "", "", "", "");
                    #region dll try commented


                    //LabDataDataContext dc = new LabDataDataContext(lblConnection.Text);
                    //PrintPDFReport rpt = new PrintPDFReport(lblConnection.Text);
                    //switch (RecType)
                    //{
                    //    case "SO":
                    //        var smp = dc.SoilSampleTest_View(RefNo, "");
                    //        foreach (var so in smp)
                    //        {
                    //            rpt.Soil_PDFReport(RefNo, Convert.ToString(so.SOSMPLTEST_SampleName_var), strAction);
                    //            break;
                    //        }
                    //        break;
                    //    case "TILE":
                    //        rpt.Tile_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "BT-":
                    //        rpt.Brick_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "FLYASH":
                    //        rpt.FlyAsh_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "CEMT":
                    //        rpt.Cement_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "CCH":
                    //        rpt.CCH_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "CT":
                    //        rpt.Cube_PDFReport(RefNo, 0, RecType, "", strAction, "", "");
                    //        break;
                    //    case "PILE":
                    //        rpt.Pile_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "STC":
                    //        rpt.STC_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "ST":
                    //        rpt.ST_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "WT":
                    //        rpt.WT_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "AGGT":
                    //        rpt.Aggregate_PDFReport(RefNo, RecType, "", 0, strAction);
                    //        break;
                    //    case "SOLID":
                    //        var details = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "SOLID");
                    //        foreach (var solid in details)
                    //        {
                    //            if (Convert.ToString(solid.TEST_Sr_No) == "1")//(solid.SOLIDINWD_TEST_Id) == "66")
                    //            {
                    //                rpt.SOLID_CS_PDFReport(RefNo, strAction);
                    //            }
                    //            else if (Convert.ToString(solid.TEST_Sr_No) == "2")
                    //            {
                    //                rpt.SOLID_WA_PDFReport(RefNo, strAction);
                    //            }
                    //        }
                    //        break;
                    //    case "OT":
                    //        rpt.OT_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "CR":
                    //        rpt.Core_PDFReport(RefNo, strAction);
                    //        break;
                    //    case "NDT":
                    //        rpt.NDT_PDFReport(RefNo, strAction, "");
                    //        break;
                    //    case "PT":
                    //        var PTdetails = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "PT");
                    //        foreach (var PTWA in PTdetails)
                    //        {
                    //            if (Convert.ToString(PTWA.TEST_Sr_No) == "1")//1
                    //            {
                    //                rpt.Pavement_CS_PDFReport(RefNo, strAction);
                    //            }
                    //            else if (Convert.ToString(PTWA.TEST_Sr_No) == "2")//2 //(Convert.ToString(PTWA.PTINWD_TEST_Id) == "63")
                    //            {
                    //                rpt.Pavement_WA_PDFReport(RefNo, strAction);
                    //            }
                    //            else if (Convert.ToString(PTWA.TEST_Sr_No) == "3")//3
                    //            {
                    //                rpt.Pavement_TS_PDFReport(RefNo, strAction);
                    //            }
                    //            else if (Convert.ToString(PTWA.TEST_Sr_No) == "4")//4
                    //            {
                    //                rpt.Pavement_FS_PDFReport(RefNo, strAction);
                    //            }
                    //        }
                    //        break;
                    //    case "MF":
                    //        int trialId = 0;
                    //        var trial = dc.Trial_View(RefNo, true);
                    //        foreach (var t in trial)
                    //        {
                    //            trialId = t.Trial_Id;
                    //        }
                    //        //rpt.TrialMDLetter_Html(RefNo, trialId, "MF", "MDL", strAction);
                    //        rpt.MF_MDLetter_PDFReport(RefNo, trialId, "MF", "MDL", strAction);
                    //        break;
                    //    case "AAC":
                    //        var detailss = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AAC");
                    //        foreach (var aac in detailss)
                    //        {
                    //            if (Convert.ToString(aac.TEST_Sr_No) == "1")
                    //            {
                    //                rpt.AAC_CS_PDFReport(RefNo, strAction);
                    //            }
                    //            else if (Convert.ToString(aac.TEST_Sr_No) == "2")
                    //            {
                    //                rpt.AAC_DS_PDFReport(RefNo, strAction);
                    //            }
                    //            else if (Convert.ToString(aac.TEST_Sr_No) == "3")
                    //            {
                    //                rpt.AAC_DM_PDFReport(RefNo, strAction);
                    //            }
                    //            else if (Convert.ToString(aac.TEST_Sr_No) == "4")
                    //            {
                    //                rpt.AAC_SN_PDFReport(RefNo, strAction);
                    //            }
                    //        }
                    //        break;
                    //}
                    #endregion
                }
                else
                {
                    myDataComm myData = new myDataComm(lblConnection.Text);
                    string reportPath;
                    string reportStr;
                    StreamWriter sw;
                    ////reportPath = Server.MapPath("~") + "\\duroweb\\report.htm";
                    //reportPath = Server.MapPath("~") + "\\report.htm";
                    //sw = File.CreateText(reportPath);

                    //reportStr = "";
                    //reportStr += myData.getReportinString(mRefNo, mRtype);
                    //sw.WriteLine(reportStr);
                    //sw.Close();
                    //Response.Redirect("report.htm", true);
                    ////Response.Redirect("report.htm");
                    string mfl = RefNo;
                    mfl = mfl.Replace("/", "_");
                    mfl = mfl.Replace("-", "_") + ".html";
                    //mRtype = drpBuildings.SelectedItem.Value.ToString();
                    reportPath = Server.MapPath("~") + "\\report.html";
                    sw = File.CreateText(reportPath);
                    reportStr = "";
                    //DateTime dt1;
                    //if (DateTime.TryParse(grdView.SelectedRow.Cells[4].Text, out dt1) == true)
                    //{
                    //    reportStr += myData.getReportinString(mRefNo, mRtype, true);
                    //}
                    //else
                    //{
                    reportStr += myData.getReportinString(RefNo, RecType, false, ddl_db.SelectedItem.Value);
                    //}
                    sw.WriteLine(reportStr);
                    sw.Close();

                    //Response.Redirect("report.html", false);

                    PrintGrid.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                }
            }
        }

        protected void grdView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdView.PageIndex = e.NewPageIndex;
            bindData();
            //grdView.DataBind();
            grdView.Visible = true;
        }

        protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string referenceNo = Convert.ToString(e.CommandArgument);
            string strMFType = "";
            PrintPDFReport rpt = new PrintPDFReport(lblConnection.Text);
            if (e.CommandName == "lnkViewMDLetter")
            {
                strMFType = "MDL";
                rpt.PrintSelectedReport("MF", referenceNo, "Automail", "0", "", strMFType, "", "", "", "");
            }
            else if (e.CommandName == "lnkViewSieveAnalysis")
            {
                strMFType = "Sieve Analysis";
                rpt.PrintSelectedReport("MF", referenceNo, "Automail", "0", "", strMFType, "", "", "", "");
            }
            else if (e.CommandName == "lnkViewMoistureCorrection")
            {
                strMFType = "Moisture Correction";
                rpt.PrintSelectedReport("MF", referenceNo, "Automail", "0", "", strMFType, "", "", "", "");
            }
            else if (e.CommandName == "lnkViewCoverSheet")
            {
                strMFType = "Cover Sheet";
                rpt.PrintSelectedReport("MF", referenceNo, "Automail", "0", "", strMFType, "", "", "", "");
            }
            else if (e.CommandName == "lnkViewFinalReport")
            {
                strMFType = "Final";
                rpt.PrintSelectedReport("MF", referenceNo, "Automail", "0", "", strMFType, "", "", "", "");
            }
            else if (e.CommandName == "lnkViewImages")
            {
                //LinkButton btn = (LinkButton)sender;
                //GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = gvr.RowIndex;
                Label lblTestName = (Label)grdView.Rows[RowIndex].FindControl("lblTestName");
                
                //if (grdView.Rows[gvr.RowIndex].Cells[1].Text == "Cube")
                if (lblTestName.Text == "Cube")
                {
                    string strImages = getCubeImagesString(referenceNo);
                    PrintHTMLReport rptHtml = new PrintHTMLReport();
                    rptHtml.DownloadHtmlReport("CubeImages", strImages);
                }
                //else if (grdView.Rows[gvr.RowIndex].Cells[1].Text == "Steel")
                else if (lblTestName.Text == "Steel")
                {
                    string strImages = getSteelImagesString(referenceNo);
                    PrintHTMLReport rptHtml = new PrintHTMLReport();
                    rptHtml.DownloadHtmlReport("SteelImages", strImages);
                }
            }
        }

        protected string getCubeImagesString(string referenceNo)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);

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
            myStr += "<tr><td width='99%' colspan=3 align=center valign=top height=19><font size=4><b>Cube Images</b></font></td></tr>";
            myStr += "<tr><td width='99%' colspan=3>&nbsp;</td></tr>";

            myStr += "<tr><td width= 20% align=left valign=top height=19 ><font size=2>" + "Reference No : " + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + ":" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + referenceNo + "</font></td></tr>";
            
            myStr += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            myStr += "<tr><td colspan=3 width= 10% align=left valign=top>";
            myStr += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            string imgLink = "http://92.204.136.64:81/cubephoto/" ;
            if (lblConnectionLive.Text.ToLower().Contains("mumbai") == true)
            {
                imgLink += "mumbai/";
            }
            else if (lblConnectionLive.Text.ToLower().Contains("nashik") == true)
            {
                imgLink += "nashik/";
            }
            else if (lblConnectionLive.Text.ToLower().Contains("metro") == true)
            {
                imgLink += "metro/";
            }
            int srNo = 1;
            var cttest = dc.CubeTestDetails_View(referenceNo, "CT");
            foreach (var ct in cttest)
            {

                myStr += "<tr>";
                //myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + ct.CTTEST_SrNo_int + "</font></td>";
                myStr += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + srNo + "</font></td>";

                myStr += "<td width= 40% >";
                if (NewWindows.CheckFileExist(imgLink + ct.CTTEST_Image_var) == true)
                {
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + ct.CTTEST_Image_var + "' >";
                }
                myStr += "</td>";

                myStr += "<td width= 40% >";
                if (NewWindows.CheckFileExist(imgLink + ct.CTTEST_Image_var1) == true)
                {
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + ct.CTTEST_Image_var1 + "' >";
                }
                myStr += "</td>";

                myStr += "<td width= 40% >";
                if (NewWindows.CheckFileExist(imgLink + ct.CTTEST_Image_var2) == true)
                {
                    myStr += "<img border=0 Width=500 height=200 src='" + imgLink + ct.CTTEST_Image_var2 + "' >";
                }
                myStr += "</td>";

                myStr += "</tr>";

                myStr += "<td width= 5% align=left valign=top height=19 colspan=3><font size=2>&nbsp;</font></td></tr>";

                srNo++;
            }

            myStr += "</table>";
            myStr += "</td></tr>";

            myStr += "</table>";
            myStr += "</html>";
            return reportStr = myStr;

        }

        protected string getSteelImagesString(string referenceNo)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);

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

            //myStr += "<tr><td width= 20% align=left valign=top height=19 ><font size=2>" + "Serial No : " + "</font></td>";
            //myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + ":" + "</font></td>";
            //myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + (serialNo + 1) + "</font></td></tr>";

            myStr += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            myStr += "<tr><td colspan=3 width= 10% align=left valign=top>";
            myStr += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            string imgLink = "http://92.204.136.64:81/steelphoto/";
            //string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            //if (cnStr.ToLower().Contains("mumbai") == true)
            //{
            //    imgLink += "mumbai/";
            //}
            //else if (cnStr.ToLower().Contains("nashik") == true)
            //{
            //    imgLink += "nashik/";
            //}
            //else
            //{
            //    imgLink += "pune/";
            //}
            if (lblConnectionLive.Text.ToLower().Contains("mumbai") == true)
            {
                imgLink += "mumbai/";
            }
            else if (lblConnectionLive.Text.ToLower().Contains("nashik") == true)
            {
                imgLink += "nashik/";
            }
            else if (lblConnectionLive.Text.ToLower().Contains("metro") == true)
            {
                imgLink += "metro/";
            }
            else
            {
                imgLink += "pune/";
            }
            int srNo = 1;
            myStr += "<tr>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Sr. No." + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Weight Per Meter" + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Tensile - Elongation 1" + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Tensile - Elongation 2" + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Tensile - Elongation 3" + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Bend" + "</font></td>";
            myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Rebend" + "</font></td>";
            myStr += "</tr>";
            var details = dc.SteelDetailInward_Update(referenceNo, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, "", "", 0, 0, "", "", "", "", "", "", false, true, false);
            foreach (var st in details)
            {

                if (st.STDETAIL_WtMeterImage1_var != "" || st.STDETAIL_TensileImage1_var != "" || st.STDETAIL_TensileImage2_var != ""
                    || st.STDETAIL_TensileImage3_var != "" || st.STDETAIL_BendImage1_var != "" || st.STDETAIL_RebendImage1_var != "")
                {

                    myStr += "<tr>";
                    myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + srNo.ToString() + "</font></td>";
                    if (st.STDETAIL_WtMeterImage1_var != "" &&
                        NewWindows.CheckFileExist(imgLink + st.STDETAIL_WtMeterImage1_var) == true)
                    {
                        myStr += "<td width= 40% >";
                        myStr += "<img border=0 Width=500 height=200 src='" + imgLink + st.STDETAIL_WtMeterImage1_var + "' >";
                        myStr += "</td>";
                    }
                    
                    if (st.STDETAIL_TensileImage1_var != "" &&
                        NewWindows.CheckFileExist(imgLink + st.STDETAIL_TensileImage1_var) == true)
                    {
                        myStr += "<td width= 40% >";
                        myStr += "<img border=0 Width=500 height=200 src='" + imgLink + st.STDETAIL_TensileImage1_var + "' >";
                        myStr += "</td>";                        
                    }
                    
                    if (st.STDETAIL_TensileImage2_var != "" &&
                        NewWindows.CheckFileExist(imgLink + st.STDETAIL_TensileImage2_var) == true)
                    {
                        myStr += "<td width= 40% >";
                        myStr += "<img border=0 Width=500 height=200 src='" + imgLink + st.STDETAIL_TensileImage2_var + "' >";
                        myStr += "</td>";
                    }
                    
                    if (st.STDETAIL_TensileImage3_var != "" &&
                        NewWindows.CheckFileExist(imgLink + st.STDETAIL_TensileImage3_var) == true)
                    {
                        myStr += "<td width= 40% >";
                        myStr += "<img border=0 Width=500 height=200 src='" + imgLink + st.STDETAIL_TensileImage3_var + "' >";
                        myStr += "</td>";
                    }
                    
                    if (st.STDETAIL_BendImage1_var != "" &&
                        NewWindows.CheckFileExist(imgLink + st.STDETAIL_BendImage1_var) == true)
                    {
                        myStr += "<td width= 40% >";
                        myStr += "<img border=0 Width=500 height=200 src='" + imgLink + st.STDETAIL_BendImage1_var + "' >";
                        myStr += "</td>";
                    }
                    
                    if (st.STDETAIL_RebendImage1_var != "" &&
                        NewWindows.CheckFileExist(imgLink + st.STDETAIL_RebendImage1_var) == true)
                    {
                        myStr += "<td width= 40% >";
                        myStr += "<img border=0 Width=500 height=200 src='" + imgLink + st.STDETAIL_RebendImage1_var + "' >";
                        myStr += "</td>";
                        
                    }
                    myStr += "</tr>";
                    myStr += "<tr><td width= 5% align=left valign=top height=19 colspan=2><font size=2>&nbsp;</font></td></tr>";

                    srNo++;
                }
            }
            myStr += "</table>";
            myStr += "</td></tr>";

            myStr += "</table>";
            myStr += "</html>";
            return reportStr = myStr;

        }

        public void bindData()
        {
            ListItem lstitem = new ListItem();
            try
            {
                lblSite.Visible = false;
                lblMaterial.Visible = false;
                if (ddlSite.SelectedIndex <= 0)
                {
                    //lblMessage.Text = "Select Site Name ..";
                    //grdView.Visible = false;
                    lblSite.Visible = true;
                }
                else if (ddlMaterial.SelectedIndex <= 0)
                {
                   lblMaterial.Visible = true;
                }
                if ((ddlSite.SelectedIndex > 0) && (ddlMaterial.SelectedIndex > 0) && (ddlBuildings.Items.Count > 0))
                {
                    lstitem.Text = ddlSite.SelectedItem.Text.ToString();
                    lstitem.Value = ddlSite.SelectedItem.Value.ToString();
                    Session["siteID"] = ddlSite.SelectedItem.Value.ToString();

                    myDataComm myData = new myDataComm(lblConnection.Text);
                    //DataTable dt = myData.getReportList(ddlInward.SelectedItem.Value.ToString(), Convert.ToDouble(Session["clientId"].ToString()), Convert.ToDouble(lstitem.Value));
                    DataTable dt = new DataTable();
                    if (HttpContext.Current.Session["databasename"].ToString() == "veena2016" || HttpContext.Current.Session["databasename"].ToString() == "veena2020")
                    {
                        dt = myData.getReportList2016(Convert.ToDouble(Session["clientId"].ToString()), Convert.ToDouble(Session["siteId"].ToString()), ddlSupplier.SelectedValue, ddlBuildings.SelectedItem.Text, ddlMaterial.SelectedItem.Value.ToString(), ddl_db.SelectedItem.Value, "", ddlTestType.SelectedValue);
                    }
                    else
                    {
                        dt = myData.getReportList(Convert.ToDouble(Session["clientId"].ToString()), Convert.ToDouble(Session["siteId"].ToString()), ddlSupplier.SelectedValue, ddlBuildings.SelectedItem.Text, ddlMaterial.SelectedItem.Value.ToString(), ddl_db.SelectedItem.Value);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "dateofreceiving desc";
                        dt = dt.DefaultView.ToTable();
                    }
                    grdView.DataSource = dt;
                    grdView.DataBind();
                    grdView.Visible = true;
                    grdView.SelectedIndex = -1;

                    for (Int32 i = 0; i <= grdView.Rows.Count - 1; i++)
                    {
                        DateTime dt1;
                        if (DateTime.TryParse(grdView.Rows[i].Cells[5].Text, out dt1) == true)
                        {
                            grdView.Rows[i].Cells[5].Text = "Ready";
                            if (HttpContext.Current.Session["databasename"].ToString() == "veena2016")
                            {
                                Label lblTestName = (Label)grdView.Rows[i].FindControl("lblTestName");
                                //if (grdView.Rows[i].Cells[1].Text == "Mix Design")
                                if (lblTestName.Text == "Mix Design")
                                {
                                    LinkButton lnkViewMDLetter = (LinkButton)grdView.Rows[i].FindControl("lnkViewMDLetter");
                                    LinkButton lnkViewSieveAnalysis = (LinkButton)grdView.Rows[i].FindControl("lnkViewSieveAnalysis");
                                    LinkButton lnkViewMoistureCorrection = (LinkButton)grdView.Rows[i].FindControl("lnkViewMoistureCorrection");
                                    LinkButton lnkViewCoverSheet = (LinkButton)grdView.Rows[i].FindControl("lnkViewCoverSheet");
                                    LinkButton lnkViewFinalReport = (LinkButton)grdView.Rows[i].FindControl("lnkViewFinalReport");
                                    Label lblMFFinalIssueDt = (Label)grdView.Rows[i].FindControl("lblMFFinalIssueDt");

                                    lnkViewMDLetter.Visible = true;
                                    lnkViewSieveAnalysis.Visible = true;
                                    lnkViewMoistureCorrection.Visible = true;
                                    lnkViewCoverSheet.Visible = true;
                                    if (DateTime.TryParse(lblMFFinalIssueDt.Text, out dt1) == true)
                                        lnkViewFinalReport.Visible = true;
                                }
                                //if (grdView.Rows[i].Cells[1].Text == "Cube" || grdView.Rows[i].Cells[1].Text == "Steel")
                                if (lblTestName.Text == "Cube" || lblTestName.Text == "Steel")
                                {
                                    LinkButton lnkViewImages = (LinkButton)grdView.Rows[i].FindControl("lnkViewImages");
                                    lnkViewImages.Visible = true;
                                }
                            }                       
                        }
                        else
                        {
                            //grdView.Rows[i].Cells[4].Text = "Not Ready";
                            grdView.Rows[i].Cells[5].Text = "In process";
                        }

                        
                    }
                }
                //grdView.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAddEnquiry_Click(object sender, EventArgs e)
        {
            Session["Location"] = lblLocation.Text;
            Response.Redirect("ClientEnquiry.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            myDataComm myData = new myDataComm();
            string reportStr;
            string mRefNo, mRtype;
            for (Int32 i = 1; i <= grdView.Rows.Count - 1; i++)
            {
                mRefNo = grdView.Rows[i].Cells[6].Text;
                string mfl = mRefNo.Replace("/", "_");
                mRtype = ddlBuildings.SelectedItem.Value.ToString();
                reportStr = "";
                reportStr += myData.getReportinString(mRefNo, mRtype, false, ddl_db.SelectedItem.Value);
            }
        }

        protected void DownloadFiles_Click(object sender, EventArgs e)
        {
            myDataComm myData = new myDataComm(lblConnection.Text);
            string reportPath;
            string reportStr;
            string mRefNo, mRtype;
            StreamWriter sw;
            for (Int32 i = 0; i <= grdView.Rows.Count - 1; i++)
            {
                if (grdView.Rows[i].Cells[5].Text == "Ready")
                {
                    mRefNo = grdView.Rows[i].Cells[6].Text;
                    //mRtype = grdView.Rows[i].Cells[1].Text; //drpBuildings.SelectedItem.Value.ToString();
                    Label lblTestName = (Label)grdView.Rows[i].FindControl("lblTestName");
                    mRtype = lblTestName.Text;
                    reportPath = Server.MapPath("~") + "\\report.htm";
                    sw = File.CreateText(reportPath);
                    reportStr = "";
                    //DateTime dt1;
                    //if (DateTime.TryParse(grdView.SelectedRow.Cells[4].Text, out dt1) == true)
                    //{
                    //    reportStr += myData.getReportinString(mRefNo, mRtype, true);
                    //}
                    //else
                    //{
                    reportStr += myData.getReportinString(mRefNo, mRtype, false, ddl_db.SelectedItem.Value);
                    //}
                    sw.WriteLine(reportStr);
                    sw.Close();
                    File.Copy(reportPath, Server.MapPath("~") + "\\Reports\\" + mRefNo.Replace("/", "_") + ".html", true);
                    cnt = cnt + 1;
                }
            }

            //    using (ZipFile zip = new ZipFile())
            //    {
            //        string[] filenames = Directory.GetFiles(Server.MapPath("~\\Reports"));
            //        foreach (string file in filenames)
            //        {
            //            ListItem lstitem = new ListItem();
            //            lstitem.Text = ddlSite.SelectedItem.Text.ToString();
            //            lstitem.Value = ddlBuildings.SelectedItem.Value.ToString();
            //            //Session["Site_Name"] = dropDwnList.SelectedItem.Value.ToString();

            //            zip.AlternateEncodingUsage = ZipOption.AsNecessary;

            //            string zipName = String.Format(ddlSite.SelectedItem.Value.ToString() + ".rar", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
            //            Response.ContentType = "application/rar";
            //            Response.AddHeader("content-disposition", "attachment; filename=" + zipName.ToString());

            //            mRtype = ddlBuildings.SelectedItem.Value.ToString();
            //            //switch (mRtype)
            //            //{l
            //            //    case "CT":
            //            //        zip.AddDirectoryByName("CT");
            //            //        zip.AddFiles(filenames, "CT");
            //            //        break;
            //            //    case "ST":

            //            //        zip.AddDirectoryByName("ST");
            //            //        zip.AddFiles(filenames, "ST");

            //            //        break;
            //            //    case "AGGT":

            //            //        zip.AddDirectoryByName("AGGT");
            //            //        zip.AddFiles(filenames, "AGGT");
            //            //        break;
            //            //    case "MF":
            //            //        zip.AddDirectoryByName("MF");
            //            //        zip.AddFiles(filenames, "MF");

            //            //        break;
            //            //    case "CEMT":

            //            //        zip.AddDirectoryByName("CEMT");
            //            //        zip.AddFiles(filenames, "CEMT");
            //            //        break;
            //            //    case "NDT":
            //            //        zip.AddDirectoryByName("NDT");
            //            //        zip.AddFiles(filenames, "NDT");

            //            //        break;
            //            //    case "CR":

            //            //        zip.AddDirectoryByName("CR");
            //            //        zip.AddFiles(filenames, "CR");
            //            //        break;
            //            //    case "FLYASH":
            //            //        zip.AddDirectoryByName("FLYASH");
            //            //        zip.AddFiles(filenames, "FLYASH");

            //            //        break;

            //            //    case "PILE":
            //            //        zip.AddDirectoryByName("PILE");
            //            //        zip.AddFiles(filenames, "PILE");

            //            //        break;
            //            //    case "SOLID":

            //            //        zip.AddDirectoryByName("SOLID");
            //            //        zip.AddFiles(filenames, "SOLID");
            //            //        break;
            //            //    case "BT-":

            //            //        zip.AddDirectoryByName("BT-");
            //            //        zip.AddFiles(filenames, "BT-");
            //            //        break;
            //            //    case "TILE":

            //            //        zip.AddDirectoryByName("TILE");
            //            //        zip.AddFiles(filenames, "TILE");
            //            //        break;
            //            //    case "PT":
            //            //        zip.AddDirectoryByName("PT");
            //            //        zip.AddFiles(filenames, "PT");

            //            //        break;
            //            //}
            //            zip.AddFiles(filenames, "Reports");
            //            zip.Save(Response.OutputStream);
            //            Response.TransmitFile(file);
            //            Response.End();
            //        }
            //    }
        }

      

        protected void ddl_db_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["databasename"] = ddl_db.SelectedValue.ToString();
            if (lblLocation.Text == "Pune")
            {
                if (ddl_db.SelectedValue.ToString() == "veena2016")
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=VeenaLive;User ID=dipl;Password=dipl2020"; //ConfigurationManager.ConnectionStrings["connStr2016"].ConnectionString;
                else if (ddl_db.SelectedValue.ToString() == "veena2020")
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=Veena2020;User ID=dipl;Password=dipl2020"; //ConfigurationManager.ConnectionStrings["connStr2016"].ConnectionString;
                else if (ddl_db.SelectedValue.ToString() == "veena2013")
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=Veena2013;User ID=dipl;Password=dipl2020";//ConfigurationManager.ConnectionStrings["connStrPre"].ConnectionString;
                else
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=Veena2015;User ID=dipl;Password=dipl2020";//ConfigurationManager.ConnectionStrings["connStrNew"].ConnectionString;
            }
            else if (lblLocation.Text == "Mumbai")
            {
                if (ddl_db.SelectedValue.ToString() == "veena2016" || ddl_db.SelectedValue.ToString() == "veena2020")
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=veenaMumbai;User ID=dipl;Password=dipl2020";// ConfigurationManager.ConnectionStrings["connStr2016"].ConnectionString;
                else if (ddl_db.SelectedValue.ToString() == "veena2013")
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=veenaMumbai2015;User ID=dipl;Password=dipl2020";//  ConfigurationManager.ConnectionStrings["connStrPre"].ConnectionString;
                else
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=veenaMumbai2015;User ID=dipl;Password=dipl2020";// ConfigurationManager.ConnectionStrings["connStrNew"].ConnectionString;
            }
            else if (lblLocation.Text == "Nashik")
            {
                if (ddl_db.SelectedValue.ToString() == "veena2016" || ddl_db.SelectedValue.ToString() == "veena2020")
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=veenaNashik;User ID=dipl;Password=dipl2020";// ConfigurationManager.ConnectionStrings["connStr2016"].ConnectionString;
                else if (ddl_db.SelectedValue.ToString() == "veena2013")
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=veenaNashik2015;User ID=dipl;Password=dipl2020";// ConfigurationManager.ConnectionStrings["connStrPre"].ConnectionString;
                else
                    lblConnection.Text = "Data Source=92.204.136.64;Initial Catalog=veenaNashik2015;User ID=dipl;Password=dipl2020";//ConnectionStrings["connStrNew"].ConnectionString;
            }
            LoadSiteList();
        }

        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMaterial.Visible = false;
            lblTestType.Visible = false;
            ddlTestType.Visible = false;
            lblTestTypeErr.Visible = false; 
            grdView.DataSource = null;
            grdView.DataBind();
            ddlTestType.Items.Clear();
            if (ddlMaterial.SelectedItem.Text == "Cement" || ddlMaterial.SelectedItem.Text == "Fly Ash" ||
                ddlMaterial.SelectedItem.Text == "Steel" || ddlMaterial.SelectedItem.Text == "Tile"
                || ddlMaterial.SelectedItem.Text == "GGBS" )
            {
                lblTestType.Visible = true;
                ddlTestType.Visible = true;
                ddlTestType.Items.Add("---All---");
                ddlTestType.Items.Add("Physical");
                ddlTestType.Items.Add("Chemical");
            }
            else if (ddlMaterial.SelectedItem.Text == "Masonary Blocks / Bricks")
            {
                lblTestType.Visible = true;
                ddlTestType.Visible = true;
                ddlTestType.Items.Add("---All---");
                ddlTestType.Items.Add("Concrete Blocks");
                ddlTestType.Items.Add("AAC Blocks");
                ddlTestType.Items.Add("Bricks");
            }
            if (ddl_db.SelectedValue != "veena2016" && ddl_db.SelectedValue != "veena2020")
            {
                lblTestType.Visible = false;
                ddlTestType.Visible = false;
                lblTestTypeErr.Visible = false;
            }
        }

        protected void ddlTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdView.DataSource = null;
            grdView.DataBind();
        }

        //protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //if (Session["databasename"].ToString().Contains("Pune") == true)
        //{
        //    if (ddlYear.SelectedIndex == 0)
        //    {
        //        Session["databasename"] = "Pune.mdb";
        //        Session["databasepassword"] = "cqra1593";
        //    }
        //    else if (ddlYear.SelectedIndex > 0)
        //    {
        //        Session["databasename"] = "Pune2013.mdb";
        //        Session["databasepassword"] = "cqra1573";
        //    }
        //}
        //}

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ddl_db.SelectedValue.ToString() == "veena2016" || ddl_db.SelectedValue.ToString() == "veena2020")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblTestName = (Label)e.Row.FindControl("lblTestName");
                    if (ddlMaterial.SelectedValue == "CEMT" && lblTestName.Text == "Cement")
                    {
                        e.Row.Cells[1].Text = "Physical";
                    }
                    else if (ddlMaterial.SelectedValue == "CEMT" && lblTestName.Text == "Cement Chemical")
                    {
                        e.Row.Cells[1].Text = "Chemical";
                    }
                    else if (ddlMaterial.SelectedValue == "ST" && lblTestName.Text == "Steel")
                    {
                        e.Row.Cells[1].Text = "Physical";
                    }
                    else if (ddlMaterial.SelectedValue == "ST" && lblTestName.Text == "Steel Chemical")
                    {
                        e.Row.Cells[1].Text = "Chemical";
                    }
                    else if (ddlMaterial.SelectedValue == "FLYASH" && lblTestName.Text == "FlyAsh")
                    {
                        e.Row.Cells[1].Text = "Physical";
                    }
                    else if (ddlMaterial.SelectedValue == "FLYASH" && lblTestName.Text == "Other")
                    {
                        e.Row.Cells[1].Text = "Chemical";
                    }
                    else if (ddlMaterial.SelectedValue == "GGBS" && lblTestName.Text == "GGBS")
                    {
                        e.Row.Cells[1].Text = "Physical";
                    }
                    else if (ddlMaterial.SelectedValue == "GGBS" && lblTestName.Text == "GGBS Chemical")
                    {
                        e.Row.Cells[1].Text = "Chemical";
                    }
                    else if (ddlMaterial.SelectedValue == "TILE" && lblTestName.Text == "FlyAsh")
                    {
                        e.Row.Cells[1].Text = "Physical";
                    }
                    else if (ddlMaterial.SelectedValue == "TILE" && lblTestName.Text == "Other")
                    {
                        e.Row.Cells[1].Text = "Chemical";
                    }
                    else if (ddlMaterial.SelectedValue == "SOLID" && lblTestName.Text == "Masonary Block")
                    {
                        e.Row.Cells[1].Text = "Concrete Blocks";
                    }
                    else if (ddlMaterial.SelectedValue == "SOLID" && lblTestName.Text == "AAC Block")
                    {
                        e.Row.Cells[1].Text = "AAC Blocks";
                    }
                    else if (ddlMaterial.SelectedValue == "SOLID" && lblTestName.Text == "Brick")
                    {
                        e.Row.Cells[1].Text = "Bricks";
                    }
                }
            }
        }
    }

}