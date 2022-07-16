using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class ClientBillApproval : System.Web.UI.Page
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
                //temporary commented 08/05/2020
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
                else if (arrMsgs[1].ToString().Equals("Metro")) //Nashik
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrMetro"].ToString();
                //temporary

                lblConnectionLive.Text = lblConnection.Text;
                //  Session["databasename"] = ddl_db.SelectedItem.Value;
                myDataComm myData = new myDataComm(lblConnection.Text);
                lblClientName.Text = myData.getClientName(Convert.ToDouble(Session["ClientID"].ToString()), ddl_db.SelectedItem.Value);
                //set default connection after login

                LoadSiteList();
                optAll.Checked = true;
                DateTime FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtForMonth.Text = FromDate.ToString("MM/yyyy");

                LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
                var client = dc.Client_View(Convert.ToInt32(Session["clientId"]), 0, "", "");
                foreach (var cl in client)
                {
                    if (cl.CL_MOUFileName_var != null && cl.CL_MOUFileName_var != "")
                    {
                        lblMOUFileName.Text = cl.CL_MOUFileName_var;
                        lnkDownloadMOUFile.Visible = true;
                    }
                }
            }
        }

        protected void LoadSiteList()
        {
            myDataComm myData = new myDataComm(lblConnection.Text);
            DataTable dt = new DataTable();

            ddlSite.Items.Clear();
            grdBill.DataSource = null;
            grdBill.DataBind();
            grdView.DataSource = null;
            grdView.DataBind();

            dt = myData.getSiteList(Convert.ToDouble(Session["ClientID"].ToString()), ddl_db.SelectedItem.Value.ToString());
            ddlSite.DataSource = dt;
            ddlSite.DataTextField = dt.Columns[0].ToString();
            ddlSite.DataValueField = dt.Columns[1].ToString();
            ddlSite.DataBind();
            if (ddlSite.Items.Count > 0)
                ddlSite.Items.Insert(0, "---All---");
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            clearData();
            LoadBillList();
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSite.Visible = false;
            Session["siteID"] = ddlSite.SelectedItem.Value.ToString();
            grdBill.DataSource = null;
            grdBill.DataBind();
            grdView.DataSource = null;
            grdView.DataBind();
        }

        protected void grdBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBill.PageIndex = e.NewPageIndex;
            LoadBillList();
            grdBill.Visible = true;
        }

        protected void grdBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            //int RowIndex = gvr.RowIndex;
            string billid = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "lnkViewPO")
            {
                var bill = dc.Bill_View(billid, 0, 0, "", 0, false, false, null, null);
                foreach (var b in bill)
                {
                    if (b.BILL_POFileName_var != null && b.BILL_POFileName_var.ToString() != "")
                    {
                        string fileName = b.BILL_POFileName_var;
                        string filePath = "D:/POFiles/";
                        if (lblConnection.Text.ToLower().Contains("mumbai") == true)
                            filePath += "Mumbai/";
                        else if (lblConnection.Text.ToLower().Contains("nashik") == true)
                            filePath += "Nashik/";
                        else if (lblConnection.Text.ToLower().Contains("metro") == true)
                            filePath += "Metro/";
                        else
                            filePath += "Pune/";

                        filePath += fileName;
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
            else if (e.CommandName == "lnkViewTestReport")
            {
                grdView.DataSource = null;
                grdView.DataBind();
                LoadReportList(billid);
            }
            else if (e.CommandName == "lnkViewBill")
            {
                printBill(billid, "pdf", "Print");
            }
            else if (e.CommandName == "lnkApproveBill")
            {
                dc.Bill_Update_ApproveStatusByClient(billid, Convert.ToInt32(Session["DL_Id"]));
                SendMailOnClientBillApproval(billid);
                clearData();
                LoadBillList();
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Approved Successfully !');", true);
            }
        }

        public void SendMailOnClientBillApproval(string BillNo)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            bool sendMail = true;
            string mTo = "", mCC = "", mSubject = "", mbody = "";

            var bill = dc.Bill_View(BillNo, 0, 0, "", 0, false, false, null, null);
            foreach (var b in bill)
            {
                bool validMailId = true;
                string EmailId = b.CL_AccEmailId_var.Replace("/", ",");
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

                //validMailId = true;
                //EmailId = b.CL_EmailID_var.Replace("/", ",");
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

                if (mTo == "" && mCC == "")
                    sendMail = false;

                //mTo = "shital.bandal@gmail.com";
                //mCC = "";
                if (sendMail == true)
                {
                    mSubject = "Invoice Approved";
                    mbody = "Dear Sir/Madam,<br><br>";
                    mbody = mbody + "This is to inform you that invoice no " + BillNo + " has been approved by " + b.ClientBillApprBy + " at time " + Convert.ToDateTime(b.BILL_ClientApproveDate_dt).ToString("hh:mm tt") + " on date " + Convert.ToDateTime(b.BILL_ClientApproveDate_dt).ToString("dd/MM/yyyy") + " <br><br>";
                    mbody = mbody + "Soft copy of test reports has been dispatched by email. You can log in to our ERP through the <br>";
                    mbody = mbody + "website to view following details about the report : <br><br>";
                    mbody = mbody + "&nbsp;&nbsp;&nbsp; 1.	The copy of work order or email confirmation along with charges <br>";
                    mbody = mbody + "&nbsp;&nbsp;&nbsp; 2.	The copy of report <br>";
                    mbody = mbody + "&nbsp;&nbsp;&nbsp; 3.	The copy of bill generated <br><br>";
                    mbody = mbody + "We request you to process the Bill for payment. Looking forward  to serve you at all times.  <br>";
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
                    objMail.SendMail(mTo, mCC, mSubject, mbody, "", "");
                }
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
            PrintPDFReport obj = new PrintPDFReport(lblConnectionLive.Text);
            //obj.Bill_PDFPrint(billNo, false, Action);
            obj.Bill_PDFPrint(billNo, false, "DisplayLogoEmail");
        }

        public void LoadBillList()
        {
            int siteId = 0;
            if (ddlSite.SelectedIndex > 0)
            {
                siteId = Convert.ToInt32(ddlSite.SelectedItem.Value);
            }
            myDataComm myData = new myDataComm(lblConnection.Text);
            DataTable dt = new DataTable();
            string[] strDate = txtForMonth.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), DateTime.DaysInMonth(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0])));
            string strFromDate = FromDate.ToString("MM/dd/yyyy");
            string strToDate = ToDate.ToString("MM/dd/yyyy");
            string apprStatus = "";
            if (optAll.Checked == true)
                apprStatus = "All";
            else if (optPending.Checked == true)
                apprStatus = "0";
            else if (optApproved.Checked == true)
                apprStatus = "1";
            dt = myData.getBillList(Convert.ToDouble(Session["clientId"].ToString()), Convert.ToDouble(siteId), strFromDate, strToDate, apprStatus, ddl_db.SelectedItem.Value);
            dt.DefaultView.Sort = "BILL_Date_dt desc";
            dt = dt.DefaultView.ToTable();
            grdBill.DataSource = dt;
            grdBill.DataBind();
            grdBill.Visible = true;
            grdBill.SelectedIndex = -1;
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                Label lblBillApprStatus = (Label)grdBill.Rows[i].FindControl("lblBillApprStatus");
                LinkButton lnkApproveBill = (LinkButton)grdBill.Rows[i].FindControl("lnkApproveBill");
                if (lblBillApprStatus.Text == "True")
                {
                    lnkApproveBill.Enabled = false;
                }
            }
        }

        protected void grdView_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblReportMsg.Visible = false;
            string myRefNo;
            string myRecordType = "";
            if (grdView.SelectedRow.Cells[5].Text == "Ready")
            {
                myRefNo = grdView.SelectedRow.Cells[6].Text;
                if (grdView.SelectedRow.Cells[1].Text == "AAC Block")
                    myRecordType = "AAC";
                else if (grdView.SelectedRow.Cells[1].Text == "Aggregate")
                    myRecordType = "AGGT";
                else if (grdView.SelectedRow.Cells[1].Text == "Brick")
                    myRecordType = "BT-";
                else if (grdView.SelectedRow.Cells[1].Text == "Cement Chemical")
                    myRecordType = "CCH";
                else if (grdView.SelectedRow.Cells[1].Text == "Cement")
                    myRecordType = "CEMT";
                else if (grdView.SelectedRow.Cells[1].Text == "Core Cutting")
                    myRecordType = "CORECUT";
                else if (grdView.SelectedRow.Cells[1].Text == "Core")
                    myRecordType = "CR";
                else if (grdView.SelectedRow.Cells[1].Text == "Cube")
                    myRecordType = "CT";
                else if (grdView.SelectedRow.Cells[1].Text == "FlyAsh")
                    myRecordType = "FLYASH";
                else if (grdView.SelectedRow.Cells[1].Text == "Soil Investigation")
                    myRecordType = "GT";
                else if (grdView.SelectedRow.Cells[1].Text == "Mix Design")
                    myRecordType = "MF";
                else if (grdView.SelectedRow.Cells[1].Text == "Non Destructive")
                    myRecordType = "NDT";
                else if (grdView.SelectedRow.Cells[1].Text == "Pile")
                    myRecordType = "PILE";
                else if (grdView.SelectedRow.Cells[1].Text == "Pavement Block")
                    myRecordType = "PT";
                else if (grdView.SelectedRow.Cells[1].Text == "Soil")
                    myRecordType = "SO";
                else if (grdView.SelectedRow.Cells[1].Text == "Masonary Block")
                    myRecordType = "SOLID";
                else if (grdView.SelectedRow.Cells[1].Text == "Steel")
                    myRecordType = "ST";
                else if (grdView.SelectedRow.Cells[1].Text == "Steel Chemical")
                    myRecordType = "STC";
                else if (grdView.SelectedRow.Cells[1].Text == "Tile")
                    myRecordType = "TILE";
                else if (grdView.SelectedRow.Cells[1].Text == "Water")
                    myRecordType = "WT";
                else if (grdView.SelectedRow.Cells[1].Text == "Other")
                    myRecordType = "OT";
                else if (grdView.SelectedRow.Cells[1].Text == "Rain Water Harvesting")
                    myRecordType = "RWH";

                ViewReport(myRefNo, myRecordType);
            }
            else
            {
                lblReportMsg.Visible = true;
            }
        }
        
        protected void grdView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdView.PageIndex = e.NewPageIndex;
            LoadReportList(lblSelectedBillNo.Text);
            grdView.Visible = true;
        }

        protected void ViewReport(string RefNo, string RecType)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            bool blockClient = false;
            var client = dc.Client_View(Convert.ToInt32(Session["clientId"]), 0, "", "");
            foreach (var cl in client)
            {
                if (cl.CL_BlockStatus_bit == true)
                {
                    blockClient = true;
                }
            }
            if (blockClient == true)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client blocked, So can not display report.');", true);
            }
            else
            {
                myDataComm myData = new myDataComm(lblConnection.Text);
                string strAction = "";
                string[] strRefNo = RefNo.Split('/');
                if (myData.checkBillApprovalStatus(strRefNo[0]) == true)
                    strAction = "Check";
                else
                    strAction = "Automail";
                PrintPDFReport rpt = new PrintPDFReport(lblConnection.Text);
                rpt.PrintSelectedReport(RecType, RefNo, strAction, "0", "", "MDL", "", "", "", "");
            }
        }

        public void LoadReportList(string billid)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            int siteId = 0;
            if (ddlSite.SelectedIndex > 0)
            {
                siteId = Convert.ToInt32(ddlSite.SelectedItem.Value);
            }
            var bill = dc.Bill_View(billid, 0, 0, "", 0, false, false, null, null);
            foreach (var bl in bill)
            {
                lblSelectedBillNo.Text = bl.BILL_Id.ToString();
                lblSelectedBillDate.Text = Convert.ToDateTime(bl.BILL_Date_dt).ToString("dd/MM/yyyy");
                lblSelectedBillSiteName.Text = bl.SITE_Name_var;
                siteId = Convert.ToInt32(bl.BILL_SITE_Id);
            }
            lblSelectedBillNo1.Visible = true;
            lblSelectedBillNo.Visible = true;
            lblSelectedBillDate1.Visible = true;
            lblSelectedBillDate.Visible = true;
            lblSelectedBillSiteName1.Visible = true;
            lblSelectedBillSiteName.Visible = true;
            grdView.DataSource = null;
            grdView.DataBind();
            myDataComm myData = new myDataComm(lblConnection.Text);
            DataTable dt = new DataTable();
            dt = myData.getReportList2016_BillApproval(Convert.ToDouble(Session["clientId"].ToString()), Convert.ToDouble(siteId), "---All---", "---All---", "---All---", ddl_db.SelectedItem.Value, billid, "---All---");
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
                }
                else
                {
                    grdView.Rows[i].Cells[5].Text = "In process";
                }
            }


        }
    
        private void clearData()
        {
            grdBill.DataSource = null;
            grdBill.DataBind();
            grdView.DataSource = null;
            grdView.DataBind();
            lblBillMsg.Visible = false;
            lblReportMsg.Visible = false;
            lblSelectedBillNo1.Visible = false;
            lblSelectedBillNo.Visible = false;
            lblSelectedBillDate1.Visible = false;
            lblSelectedBillDate.Visible = false;
            lblSelectedBillSiteName1.Visible = false;
            lblSelectedBillSiteName.Visible = false;
        }

        protected void optAll_CheckedChanged(object sender, EventArgs e)
        {
            clearData();
        }

        protected void optPending_CheckedChanged(object sender, EventArgs e)
        {
            clearData();
        }

        protected void optApproved_CheckedChanged(object sender, EventArgs e)
        {
            clearData();
        }

        protected void lnkDownloadMOUFile_Click(object sender, EventArgs e)
        {
            //string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            string filePath = "D:/MOUFiles/";
            if (lblConnectionLive.Text.ToLower().Contains("mumbai") == true)
                filePath += "Mumbai/";
            else if (lblConnectionLive.Text.ToLower().Contains("nashik") == true)
                filePath += "Nashik/";
            else if (lblConnectionLive.Text.ToLower().Contains("metro") == true)
                filePath += "Metro/";
            else
                filePath += "Pune/";

            filePath += lblMOUFileName.Text;
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