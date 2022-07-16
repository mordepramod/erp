using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportSalesStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Sales Status";
                bool superAdminRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAdmin_right_bit == true)
                        superAdminRight = true;
                }
                if (superAdminRight == true)
                {
                    txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    //LoadMEList();
                    //LoadRouteList();
                    lblDescription.Text = "<b>No. of orders received </b>: All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>Value of orders received </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<bSale </b>: Total value of bill amount in selected period (Bill Date)<br>" +
                        "<b>Outstanding value </b>: Total outstanding amount till date.<br>" + //bill, journal, Cashdetail
                        "<b>Report pending for printing due to credit limit </b>: All reports pending to print having client outstanding amount is greater than credit limit(Excluding cube report having coupons) and approved in selected period (Approval Date).<br>" +  //misdetail table
                        "<b>No of bills modified </b>: All Ok bills modified in selected perion (Modified On Date)<br>" +
                        "<b>Value of bills modified </b>: value of all ok bills modified in selected period (Modified On Date)<br>" +
                        "<b>Money received </b>: Total value of receipt amount with tds in selected period (Cash Date)<br>" +
                        "<b>TDS </b>: Total Value of receipt tds amount in selected period (Receipt Date)<br>" +
                        "<b>Money kept on A/c </b>: Total value of on A/c amount in selected period (Receipt Date)<br>" +
                        "<b>Open advance </b>: Total value of open advance till date <br>";
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
                    
            }
        }
        private void LoadMEList()
        {
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            var me = dc.ReportSalesStatus_ME(Fromdate, Todate);
            ddlME.DataTextField = "MEName";
            ddlME.DataValueField = "ROUTE_ME_Id";
            ddlME.DataSource = me;
            ddlME.DataBind();
            ddlME.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void LoadRouteList()
        {
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            var route = dc.ReportSalesStatus_Route(Fromdate, Todate);
            ddlRoute.DataTextField = "RouteName";
            ddlRoute.DataValueField = "SITE_Route_Id";
            ddlRoute.DataSource = route;
            ddlRoute.DataBind();
            ddlRoute.Items.Insert(0, new ListItem("---Select---", "0"));
            ddlRouteList.Items.Clear();
            ddlRouteList.Items.AddRange(ddlRoute.Items.OfType<ListItem>().ToArray()); 
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            LoadReportList();
            LoadMEList();
            LoadRouteList();
        }

        private void LoadReportList()
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("Category", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersReceived", typeof(string)));
            //dt.Columns.Add(new DataColumn("OutstandingValue", typeof(string)));
            dt.Columns.Add(new DataColumn("Sale", typeof(string)));
            dt.Columns.Add(new DataColumn("ReportPendingForPrintingDueToCRL", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfBillsModified", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfBillsModified", typeof(string)));
            //dt.Columns.Add(new DataColumn("MoneyReceived", typeof(string)));
            //dt.Columns.Add(new DataColumn("TDS", typeof(string)));
            //dt.Columns.Add(new DataColumn("GST", typeof(string)));
            //dt.Columns.Add(new DataColumn("MoneyKeptOnAcc", typeof(string)));
            //dt.Columns.Add(new DataColumn("OpenAdvance", typeof(string)));

            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int totNoOfOrdersReceived = 0, totReportPendingForPrintingDueToCRL = 0, totNoOfBillsModified = 0;
            decimal totValueOfOrdersReceived = 0, totSale = 0, totOutstandingValue = 0, totValueOfBillsModified = 0, totMoneyReceived = 0, 
                totTDS = 0, totGST = 0, totMoneyKeptOnAcc = 0, totOpenAdvance = 0;
            int MEId = 0, RouteId = 0;
            if (ddlME.SelectedIndex > 0)
            {
                MEId = Convert.ToInt32(ddlME.SelectedValue);
            }
            if (ddlRoute.SelectedIndex > 0)
            {
                RouteId = Convert.ToInt32(ddlRoute.SelectedValue);
            }
            var materialType = dc.Material_View("",""); 
            foreach(var mat in materialType)
            {
                dr1 = dt.NewRow();
                dr1["Category"] = mat.MATERIAL_RecordType_var;
                var reportList = dc.ReportSalesStatus(Fromdate, Todate, mat.MATERIAL_RecordType_var, MEId, RouteId);
                foreach (var rpt in reportList)
                {
                    if (MEId > 0 || RouteId > 0)
                    {
                        dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived);
                        dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived);
                        dr1["Sale"] = Convert.ToInt32(rpt.Sale);
                        dr1["ReportPendingForPrintingDueToCRL"] = Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                        dr1["NoOfBillsModified"] = Convert.ToInt32(rpt.NoOfBillsModified);
                        dr1["ValueOfBillsModified"] = Convert.ToDecimal(rpt.ValueOfBillsModified);

                        totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived);
                        totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived);
                        totSale += Convert.ToDecimal(rpt.Sale);
                        totReportPendingForPrintingDueToCRL += Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                        totNoOfBillsModified += Convert.ToInt32(rpt.NoOfBillsModified);
                        totValueOfBillsModified += Convert.ToDecimal(rpt.ValueOfBillsModified);
                    }
                    else
                    {
                        dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                        dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToInt32(rpt.ValueOfOrdersReceivedNew);
                        dr1["Sale"] = Convert.ToInt32(rpt.Sale);
                        dr1["ReportPendingForPrintingDueToCRL"] = Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL) ;
                        dr1["NoOfBillsModified"] = Convert.ToInt32(rpt.NoOfBillsModified);
                        dr1["ValueOfBillsModified"] = Convert.ToDecimal(rpt.ValueOfBillsModified);

                        totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                        totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToInt32(rpt.ValueOfOrdersReceivedNew);
                        totSale += Convert.ToDecimal(rpt.Sale);
                        totReportPendingForPrintingDueToCRL += Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                        totNoOfBillsModified += Convert.ToInt32(rpt.NoOfBillsModified);
                        totValueOfBillsModified += Convert.ToDecimal(rpt.ValueOfBillsModified);
                    }
                }
                dt.Rows.Add(dr1);
            }
            //Coupon
            dr1 = dt.NewRow();
            dr1["Category"] = "Coupon";
            var reportListC = dc.ReportSalesStatus(Fromdate, Todate, "---", MEId, RouteId);
            foreach (var rpt in reportListC)
            {
                if (MEId > 0 || RouteId > 0)
                {
                    dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived);
                    dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived);
                    dr1["Sale"] = Convert.ToInt32(rpt.Sale);
                    dr1["ReportPendingForPrintingDueToCRL"] = Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    dr1["NoOfBillsModified"] = Convert.ToInt32(rpt.NoOfBillsModified);
                    dr1["ValueOfBillsModified"] = Convert.ToDecimal(rpt.ValueOfBillsModified);

                    totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived);
                    totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived);
                    totSale += Convert.ToDecimal(rpt.Sale);
                    totReportPendingForPrintingDueToCRL += Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    totNoOfBillsModified += Convert.ToInt32(rpt.NoOfBillsModified);
                    totValueOfBillsModified += Convert.ToDecimal(rpt.ValueOfBillsModified);
                }
                else
                {
                    dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToInt32(rpt.ValueOfOrdersReceivedNew);
                    dr1["Sale"] = Convert.ToInt32(rpt.Sale);
                    dr1["ReportPendingForPrintingDueToCRL"] = Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    dr1["NoOfBillsModified"] = Convert.ToInt32(rpt.NoOfBillsModified);
                    dr1["ValueOfBillsModified"] = Convert.ToDecimal(rpt.ValueOfBillsModified);

                    totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToInt32(rpt.ValueOfOrdersReceivedNew);
                    totSale += Convert.ToDecimal(rpt.Sale);
                    totReportPendingForPrintingDueToCRL += Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    totNoOfBillsModified += Convert.ToInt32(rpt.NoOfBillsModified);
                    totValueOfBillsModified += Convert.ToDecimal(rpt.ValueOfBillsModified);
                }
            }
            dt.Rows.Add(dr1);
            //Monthly
            dr1 = dt.NewRow();
            dr1["Category"] = "Monthly";
            var reportListM = dc.ReportSalesStatus(Fromdate, Todate, "Monthly", MEId, RouteId);
            foreach (var rpt in reportListM)
            {
                if (MEId > 0 || RouteId > 0)
                {
                    dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived);
                    dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived);
                    dr1["Sale"] = Convert.ToInt32(rpt.Sale);
                    dr1["ReportPendingForPrintingDueToCRL"] = Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    dr1["NoOfBillsModified"] = Convert.ToInt32(rpt.NoOfBillsModified);
                    dr1["ValueOfBillsModified"] = Convert.ToDecimal(rpt.ValueOfBillsModified);

                    totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived);
                    totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived);
                    totSale += Convert.ToDecimal(rpt.Sale);
                    totReportPendingForPrintingDueToCRL += Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    totNoOfBillsModified += Convert.ToInt32(rpt.NoOfBillsModified);
                    totValueOfBillsModified += Convert.ToDecimal(rpt.ValueOfBillsModified);
                }
                else
                {
                    dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToInt32(rpt.ValueOfOrdersReceivedNew);
                    dr1["Sale"] = Convert.ToInt32(rpt.Sale);
                    dr1["ReportPendingForPrintingDueToCRL"] = Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    dr1["NoOfBillsModified"] = Convert.ToInt32(rpt.NoOfBillsModified);
                    dr1["ValueOfBillsModified"] = Convert.ToDecimal(rpt.ValueOfBillsModified);

                    totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToInt32(rpt.ValueOfOrdersReceivedNew);
                    totSale += Convert.ToDecimal(rpt.Sale);
                    totReportPendingForPrintingDueToCRL += Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    totNoOfBillsModified += Convert.ToInt32(rpt.NoOfBillsModified);
                    totValueOfBillsModified += Convert.ToDecimal(rpt.ValueOfBillsModified);
                }
            }
            dt.Rows.Add(dr1);
            ////
            var reportList1 = dc.ReportSalesStatusTotal(Fromdate, Todate);
            foreach (var rpt1 in reportList1)
            {
                totOutstandingValue = Convert.ToDecimal(rpt1.TotalOutstandingValue);
                totMoneyReceived = Convert.ToDecimal(rpt1.TotalMoneyReceived);
                totTDS = Convert.ToDecimal(rpt1.TotalTDSReceived);
                //totGST = Convert.ToDecimal(rpt.TotalGST);
                totMoneyKeptOnAcc = Convert.ToInt32(rpt1.TotalMoneyKeptOnAcc);
                totOpenAdvance = Convert.ToInt32(rpt1.TotalOpenAdvance);
            }
            totGST = (totMoneyReceived * 18) / 100;
            if (totMoneyKeptOnAcc < 0)
                totMoneyKeptOnAcc = totMoneyKeptOnAcc * -1;
            
            if (totOpenAdvance < 0)
                totOpenAdvance = totOpenAdvance * -1;
            
            lblTotal.Text = "Outstanding value : " + totOutstandingValue.ToString("0.00") + "&nbsp;&nbsp;" + " Money received : " + totMoneyReceived.ToString("0.00") +
                "&nbsp;&nbsp;" + "TDS : " + totTDS.ToString("0.00") + "&nbsp;&nbsp;" + "GST : " + totGST.ToString("0.00") + "&nbsp;&nbsp;" + "Money kept on A/c : " + totMoneyKeptOnAcc.ToString("0.00") + "&nbsp;&nbsp;" + "Open advance : " + totOpenAdvance.ToString("0.00");
            dr1 = dt.NewRow();
            dr1["Category"] = "Total = ";
            dr1["NoOfOrdersReceived"] = totNoOfOrdersReceived;
            dr1["ValueOfOrdersReceived"] = totValueOfOrdersReceived;
            dr1["Sale"] = totSale;
            //dr1["OutstandingValue"] = totOutstandingValue;
            dr1["ReportPendingForPrintingDueToCRL"] = totReportPendingForPrintingDueToCRL;
            dr1["NoOfBillsModified"] = totNoOfBillsModified;
            dr1["ValueOfBillsModified"] = totValueOfBillsModified;
            //dr1["MoneyReceived"] = totMoneyReceived;
            //dr1["TDS"] = totTDS;
            //dr1["GST"] = totGST.ToString("0.00");
            //if (totOpenAdvance < 0)
            //    dr1["MoneyKeptOnAcc"] = totMoneyKeptOnAcc * -1;
            //else
            //    dr1["MoneyKeptOnAcc"] = totMoneyKeptOnAcc;
            //if (totOpenAdvance < 0)
            //    dr1["OpenAdvance"] = totOpenAdvance * -1;
            //else
            //    dr1["OpenAdvance"] = totOpenAdvance;
            dt.Rows.Add(dr1);
            grdReport.DataSource = dt;
            grdReport.DataBind();
            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                LinkButton lnkNoOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfOrdersReceived");
                LinkButton lnkValueOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersReceived");
                LinkButton lnkSale = (LinkButton)grdReport.Rows[i].FindControl("lnkSale");
                LinkButton lnkReportPendingForPrintingDueToCRL = (LinkButton)grdReport.Rows[i].FindControl("lnkReportPendingForPrintingDueToCRL");
                LinkButton lnkNoOfBillsModified = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfBillsModified");
                LinkButton lnkValueOfBillsModified = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfBillsModified");
                if (i == grdReport.Rows.Count - 1)
                {
                    lnkNoOfOrdersReceived.Enabled = false;
                    lnkValueOfOrdersReceived.Enabled = false;
                    lnkSale.Enabled = false;
                    lnkReportPendingForPrintingDueToCRL.Enabled = false;
                    lnkNoOfBillsModified.Enabled = false;
                    lnkValueOfBillsModified.Enabled = false;
                }
                else
                {
                    if (lnkNoOfOrdersReceived.Text == "0")
                        lnkNoOfOrdersReceived.Text = "";
                    if (lnkValueOfOrdersReceived.Text == "0")
                        lnkValueOfOrdersReceived.Text = "";
                    if (lnkSale.Text == "0")
                        lnkSale.Text = "";
                    if (lnkReportPendingForPrintingDueToCRL.Text == "0")
                        lnkReportPendingForPrintingDueToCRL.Text = "";
                    if (lnkNoOfBillsModified.Text == "0")
                        lnkNoOfBillsModified.Text = "";
                    if (lnkValueOfBillsModified.Text == "0")
                        lnkValueOfBillsModified.Text = "";
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReport.Rows.Count > 0 && grdReport.Visible == true)
            {
                string Subheader = "";

                Subheader = lblFromDate.Text + "\t" + txtFromDate.Text + "~" + lblToDate.Text + "\t" + txtToDate.Text;
                if (ddlME.SelectedIndex > 0)
                    Subheader += "~" + "Marketing Person" + "\t" + ddlME.SelectedItem.Text;
                if (ddlRoute.SelectedIndex > 0)
                    Subheader += "~" + "Route" + "\t" + ddlRoute.SelectedItem.Text;

                //PrintGrid.PrintGridView(grdReport, Subheader, "SalesStatus_Report");

                var fileName = "SalesStatus" + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                PrintGrid.PrintGridView_SalesStatus(grdReport, Subheader, fileName);

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

        protected void imgCloseDetailPopup_Click(object sender, ImageClickEventArgs e)
        {
            pnlDetails.Visible = false;
            pnlReport.Visible = true;
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            //LoadMEList();
            //LoadRouteList();
            grdReport.DataSource = null;
            grdReport.DataBind();
            ddlME.Items.Clear();
            ddlRoute.Items.Clear();
            lblTotal.Text = ""; 
        }

        protected void lnkViewDescription_Click(object sender, EventArgs e)
        {
            if (lblDescription.Visible == false)
            {
                lblDescription.Visible = true;
                pnlReport.Height = 180;
            }
            else
            {
                lblDescription.Visible = false;
                pnlReport.Height = 380;
            }
        }

        protected void ddlME_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportList();
            ddlRoute.Items.Clear();
            if (ddlME.SelectedIndex > 0)
            {
                var routeName = dc.Route_View_ME(Convert.ToInt32(ddlME.SelectedValue));
                foreach (var rt in routeName)
                {
                    for (int i = 0; i < ddlRouteList.Items.Count; i++)
                    {
                        if (rt.ROUTE_Id.ToString() == ddlRouteList.Items[i].Value)
                        {
                            ddlRoute.Items.Add(new ListItem(rt.ROUTE_Name_var, rt.ROUTE_Id.ToString()));
                        }
                    }
                }
                ddlRoute.Items.Insert(0, new ListItem("---Select---", "0"));
            }
            else
            {
                ddlRoute.Items.AddRange(ddlRouteList.Items.OfType<ListItem>().ToArray());
            }
        }

        protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportList();
            if (ddlRoute.SelectedIndex > 0)
            {
                var routeName = dc.Route_View(0, ddlRoute.SelectedItem.Text, "", 0);
                foreach (var rt in routeName)
                {
                    if (rt.ROUTE_ME_Id != null)
                        ddlME.SelectedValue = rt.ROUTE_ME_Id.ToString() ;
                }
            }
            else
            {
                ddlME.SelectedValue = "0";
            }
        }

        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            pnlDetails.Visible = true;
            pnlReport.Visible = false;

            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;
            string recType = grdReport.Rows[RowIndex].Cells[0].Text;
            int detailFlag = 0;

            for (int i = 1; i < grdDetails.Columns.Count; i++)
            {
                grdDetails.Columns[i].Visible = false;
            }
            if (e.CommandName == "lnkNoOfOrdersReceived" || e.CommandName == "lnkValueOfOrdersReceived")
            {
                detailFlag = 0;
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i <= 17)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            else if (e.CommandName == "lnkReportPendingForPrintingDueToCRL")
            {
                detailFlag = 1;
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i == 1 || i == 2 || i == 18 || i == 19 || i == 20 || i == 21 || i == 22)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            else if (e.CommandName == "lnkNoOfBillsModified" || e.CommandName == "lnkValueOfBillsModified")
            {
                detailFlag = 2;
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i == 1 || i == 2 || i == 23 || i == 24 || i == 25 || i == 26 || i == 27)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            else if (e.CommandName == "lnkSale" )
            {
                detailFlag = 3;
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i == 1 || i == 2 || i == 23 || i == 24 || i == 25 || i == 26 || i == 27)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            if (recType == "Coupon")
            {
                recType = "---";
            }
            int MEId = 0, RouteId = 0;
            if (ddlME.SelectedIndex > 0)
            {
                MEId = Convert.ToInt32(ddlME.SelectedValue);
            }
            if (ddlRoute.SelectedIndex > 0)
            {
                RouteId = Convert.ToInt32(ddlRoute.SelectedValue);
            }
            var detail = dc.ReportSalesStatus_Details(Fromdate, Todate, recType, MEId, RouteId, Convert.ToByte(detailFlag));
            grdDetails.DataSource = detail;
            grdDetails.DataBind();
        }

        protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //7- Enquiry status
                //8 - approved date
                //9 - collected on
                if (e.Row.Cells[7].Text != "" && e.Row.Cells[7].Text != "&nbsp;")
                {
                    if (Convert.ToInt32(e.Row.Cells[7].Text) == 2)
                    {
                        e.Row.Cells[7].Text = "Closed";
                    }
                    else if (e.Row.Cells[8].Text != "" && e.Row.Cells[8].Text != "&nbsp;"
                        && (Convert.ToInt32(e.Row.Cells[7].Text) == 0 || Convert.ToInt32(e.Row.Cells[7].Text) == 1))
                    {
                        e.Row.Cells[7].Text = "Approved";
                    }
                    else if (e.Row.Cells[9].Text != "" && e.Row.Cells[9].Text != "&nbsp;"
                        && (Convert.ToInt32(e.Row.Cells[7].Text) == 1))
                    {
                        e.Row.Cells[7].Text = "Collected";
                    }
                    else //if (Convert.ToInt32(e.Row.Cells[7].Text) < 2)
                    {
                        e.Row.Cells[7].Text = "Pending";
                    }
                }
            }
        }

    }
}