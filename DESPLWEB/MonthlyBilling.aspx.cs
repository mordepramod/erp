using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;

namespace DESPLWEB
{
    public partial class MonthlyBilling : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Monthly Billing";
                //txtFromDate.Text = DateTime.Today.AddDays(-DateTime.Today.Day + 1).ToString("MM/yyyy");                
                DateTime FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                txtFromDate.Text = FromDate.ToString("MM/yyyy");
                bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_BillPrint_right_bit == true)
                            userRight = true;
                    }
                    if (userRight == false)
                    {
                        pnlContent.Visible = false;
                        lblAccess.Visible = true;
                        lblAccess.Text = "Access is Denied.. ";
                    }
                }
            }
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            LoadClientList();

        }
        private void LoadClientList()
        {
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
            string strFromDate = FromDate.ToString("MM/dd/yyyy");
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), DateTime.DaysInMonth(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0])));
            string strToDate = ToDate.ToString("MM/dd/yyyy");

            var cl = dc.Client_View_MonthlyBilling(FromDate, ToDate, false);
            ddlClient.DataSource = cl;
            ddlClient.DataTextField = "CL_Name_var";
            ddlClient.DataValueField = "CL_Id";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, new ListItem("-----Select----", "0"));

            //clsData obj = new clsData();
            //DataTable dt = obj.getMonthlyClientList(strFromDate, strToDate, 0);
            //ddlClient.DataSource = dt;
            //ddlClient.DataTextField = "CL_Name_var";
            //ddlClient.DataValueField = "CL_Id";
            //ddlClient.DataBind();
            //ddlClient.Items.Insert(0, new ListItem("-----Select----", "0"));

        }
        protected void lnkMonlthlyClientList_Click(object sender, EventArgs e)
        {
            clsData obj = new clsData();
            DataTable dtt = obj.getMonthlyClientList("", "", 1);

            //var cl = dc.Client_View_MonthlyBilling(null, null, true);
            //DataRow row = null;
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Client Name");
            //foreach (var rowObj in cl)
            //{
            //    row = dt.NewRow();
            //    row["Client Name"] = rowObj.CL_Name_var.ToString();
            //    dt.Rows.Add(row);
            //}
            if (dtt.Rows.Count > 0)
            {
                PrintGrid.PrintTimeReport(dtt, "List_Of_MonthlyClient");
            }
        }
        private void LoadSiteList()
        {
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), DateTime.DaysInMonth(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0])));

            var site = dc.Site_View_MonthlyBilling(Convert.ToInt32(ddlClient.SelectedValue), FromDate, ToDate);
            ddlSite.DataSource = site;
            ddlSite.DataTextField = "SITE_Name_var";
            ddlSite.DataValueField = "SITE_Id";
            ddlSite.DataBind();
            ddlSite.Items.Insert(0, new ListItem("-----Select----", "0"));
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSite.Items.Clear();
            if (ddlClient.SelectedIndex > 0)
            {
                LoadSiteList();
            }
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkGenerateBill_Click(object sender, EventArgs e)
        {
            DateTime tempdt;
            bool validData = true;
            string strMsg = "";
            if (DateTime.TryParseExact(txtFromDate.Text, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdt) == false)
            {
                validData = false;
                strMsg = "Select valid date..";
            }
            else if (ddlClient.SelectedIndex <= 0)
            {
                validData = false;
                strMsg = "Select Client..";
            }
            else if (ddlSite.SelectedIndex <= 0)
            {
                validData = false;
                strMsg = "Select Site..";
            }
            else
            {
                string[] strDate = txtFromDate.Text.Split('/');
                DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
                DateTime CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (FromDate >= CurrentDate)
                {
                    validData = false;
                    strMsg = "Select month less than current month..";
                }
            }
            if (validData == true)
            {
                //DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
                //DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                string[] strDate = txtFromDate.Text.Split('/');
                DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
                DateTime ToDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), DateTime.DaysInMonth(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0])));

                var allinward = dc.Inward_View_Monthly(Convert.ToInt32(ddlClient.SelectedValue), Convert.ToInt32(ddlSite.SelectedValue), FromDate, ToDate, false, "");
                if (allinward.Count() > 0)
                {
                    string billNo = "", billingPeriod = "";
                    //billingPeriod = FromDate.ToString("dd/MM/yyyy") + " - " + ToDate.ToString("dd/MM/yyyy");
                    billingPeriod = FromDate.ToString("MMM/yyyy");
                    MonthlyBillUpdation monbill = new MonthlyBillUpdation();
                    billNo = monbill.UpdateBill(Convert.ToInt32(ddlClient.SelectedValue), Convert.ToInt32(ddlSite.SelectedValue), FromDate, ToDate, billingPeriod, "0");
                    strMsg = "Bill - " + billNo + " generated successfully..";
                }
                else
                {
                    strMsg = "Pending report not available for monthly bill generation..";
                }
            }
            if (strMsg != "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + strMsg + " ');", true);
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadBillList();
        }

        public void LoadBillList()
        {
            int ClientId = 0, SiteId = 0;
            if (ddlClient.Items.Count > 0)
            {
                ClientId = Convert.ToInt32(ddlClient.SelectedValue);
            }
            if (ddlSite.Items.Count > 0)
            {
                SiteId = Convert.ToInt32(ddlSite.SelectedValue);
            }
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                {
                    ClientId = Convert.ToInt32(lblClientId.Text);
                    SiteId = 0;
                }
            }
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
            string strBillingPeriod = FromDate.ToString("MMM/yyyy");

            var bill = dc.Bill_View_Monthly(ClientId, SiteId, strBillingPeriod);
            grdBill.DataSource = bill;
            grdBill.DataBind();

            if (grdBill.Rows.Count > 0)
            {
                bool billPrintRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_BillPrint_right_bit == true)
                        billPrintRight = true;
                }
                //decimal billTotal = 0, billPaidAmount = 0, billPendingAmount = 0;
                for (int i = 0; i < grdBill.Rows.Count; i++)
                {
                    LinkButton lnkPrintBill = (LinkButton)grdBill.Rows[i].FindControl("lnkPrintBill");
                    LinkButton lnkPrintBillDetail = (LinkButton)grdBill.Rows[i].FindControl("lnkPrintBillDetail");

                    if (billPrintRight == true)
                    {
                        lnkPrintBill.Enabled = true;
                        lnkPrintBillDetail.Enabled = true;
                    }
                    else
                    {
                        lnkPrintBill.Enabled = false;
                        lnkPrintBillDetail.Enabled = false;
                    }
                    //billTotal += Convert.ToDecimal(grdBill.Rows[i].Cells[5].Text);
                    //if (grdBill.Rows[i].Cells[13].Text != "" && grdBill.Rows[i].Cells[13].Text != "&nbsp;")
                    //    billPaidAmount += Convert.ToDecimal(grdBill.Rows[i].Cells[13].Text);
                    //if (grdBill.Rows[i].Cells[14].Text != "" && grdBill.Rows[i].Cells[14].Text != "&nbsp;")
                    //    billPendingAmount += Convert.ToDecimal(grdBill.Rows[i].Cells[14].Text);
                }
                //lblTotal.Text = "Total : " + billTotal.ToString("0.00");
                //lblPaidAmount.Text = "Total Paid Amount : " + billPaidAmount.ToString("0.00");
                //lblPendingAmount.Text = "Total Pending Amount : " + billPendingAmount.ToString("0.00");
            }
        }

        protected void grdBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;

            Session["BillId"] = null;
            Session["BillId"] = Convert.ToString(e.CommandArgument);
            string billid = Convert.ToString(e.CommandArgument);
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                var bill = dc.Bill_View(billid, 0, 0, "", 0, false, false, null, null).ToList();
                if (e.CommandName == "lnkModifyBill")
                {
                    //MonthlyBillUpdation monbill = new MonthlyBillUpdation();
                    //foreach (var b in bill)
                    //{
                    //    string[] strDate = b.BILL_BillingPeriod_var.Split('/');
                    //    int iMonthNo = Convert.ToDateTime("01-" + strDate[0] + "-2017").Month;
                    //    DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), iMonthNo, 1);
                    //    DateTime ToDate = new DateTime(Convert.ToInt32(strDate[1]), iMonthNo, DateTime.DaysInMonth(Convert.ToInt32(strDate[1]), iMonthNo));
                    //    string billingPeriod = "";
                    //    billingPeriod = FromDate.ToString("MMM/yyyy");

                    //    monbill.UpdateBill(Convert.ToInt32(b.BILL_CL_Id), Convert.ToInt32(b.BILL_SITE_Id), FromDate, ToDate, billingPeriod, billid);
                    //    break;
                    //}
                    //break;

                    bool modifyBillFlag = true;
                    //var bill = dc.Bill_View(Convert.ToInt32(billid), 0, 0, "", 0, false, false, null, null);
                    foreach (var bl in bill)
                    {
                        if (bl.BILL_ApproveStatus_bit == true)
                        {
                            modifyBillFlag = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Can not modify approved bill. ..');", true);
                        }
                        if (modifyBillFlag == true)
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
                        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                        string strURLWithData = "BillMonthly.aspx?" + obj.Encrypt(string.Format("BillId={0}&CouponBill={1}", billid, "False"));
                        PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                    }
                }
                else if (u.USER_BillPrint_right_bit == true && e.CommandName == "lnkPrintBill")
                {
                    BillUpdation billPrint = new BillUpdation();
                    billPrint.getBillPrintString(billid, false);
                    break;
                }
                else if (u.USER_BillPrint_right_bit == true && e.CommandName == "lnkPrintBillDetail")
                {
                    BillUpdation billPrint = new BillUpdation();
                    billPrint.getMonthlyBillDetailPrint(billid);
                    break;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Access is Denied !');", true);
                }
            }
        }

        public void LoadPendingClientSiteList()
        {
            var pendingCl = dc.Bill_View_MonthlyPendingList();
            grdClient.DataSource = pendingCl;
            grdClient.DataBind();
            if (grdClient.Rows.Count == 0)
            {
                string strMsg = "Pending report not available for monthly bill generation..";
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + strMsg + " ');", true);
            }
        }

        public void LoadPendingReportList()
        {
            var pendingCl = dc.Bill_View_ReportPendingList(true);
            grdReport.DataSource = pendingCl;
            grdReport.DataBind();
            if (grdReport.Rows.Count == 0)
            {
                string strMsg = "Pending report not available for monthly bill generation..";
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + strMsg + " ');", true);
            }
        }

        protected void lnkLoadPendingList_Click(object sender, EventArgs e)
        {
            LoadPendingClientSiteList();
            pnlBillList.Visible = false;
            pnlPendingList.Visible = true;
            pnlPendingReportList.Visible = false;
        }

        protected void lnkLoadBillList_Click(object sender, EventArgs e)
        {
            LoadBillList();
            pnlBillList.Visible = true;
            pnlPendingList.Visible = false;
            pnlPendingReportList.Visible = false;
        }

        protected void lnkLoadPendingRptList_Click(object sender, EventArgs e)
        {
            LoadPendingReportList();
            pnlBillList.Visible = false;
            pnlPendingList.Visible = false;
            pnlPendingReportList.Visible = true;
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            ddlClient.Items.Clear();
            ddlSite.Items.Clear();
            DateTime tempdt;
            bool validData = true;
            string strMsg = "";
            if (DateTime.TryParseExact(txtFromDate.Text, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdt) == false)
            {
                validData = false;
                strMsg = "Select valid date..";
            }
            else
            {
                string[] strDate = txtFromDate.Text.Split('/');
                DateTime FromDate = new DateTime(Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]), 1);
                DateTime CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (FromDate >= CurrentDate)
                {
                    validData = false;
                    strMsg = "Select month less than current month..";
                }
            }
            if (validData == true)
            {
                LoadClientList();
            }
            if (strMsg != "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + strMsg + " ');", true);
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

       
    }
}