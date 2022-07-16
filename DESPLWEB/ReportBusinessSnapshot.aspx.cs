using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportBusinessSnapshot : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Business Snapshot";
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

                    lblDescription.Text = "<b>No. of total enquiries </b>: All enquiries(Regular & New Client) generated in selected period (Enquiry Date).<br>" +
                        "<b>No. of auto enquiries </b>: All enquiries(Regular & New Client) generated in selected period (Enquiry Date) having material already collected. <br>" +
                        "<b>No. of proposals sent (Current) </b>: All enquiries(Regular & New Client) having proposal sent to client in selected period (Enquiry & Proposal Date).<br>" +
                        "<b>No. of proposals sent </b>: All enquiries(Regular & New Client) having proposal sent to client in selected period (Proposal Date).<br>" +
                        "<b>No. of orders received (Current) </b>: All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Enquiry & Proposal Order Date).<br>" +
                        "<b>No. of orders received </b>: All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>Value of orders received (Current) </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Enquiry & Proposal Order Date).<br>" +
                        "<b>Value of orders received </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>Value of orders from new clients (Current) </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to new client(No bill generated before 6 months from selected from date) and order received against proposal in selected period (Enquiry & Proposal Order Date).<br>" +
                        "<b>Value of orders from new clients </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to new client(No bill generated before 6 months from selected from date) and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>No. of enquiries against  which material  collected  </b>: All enquiries(Regular & New Client) having material collected in selected period (Collected On Date). <br>" +
                        "<b>No. of tests completed  </b>: <br>" +
                        "<b>No. of Reports approved </b>: All reports approved in selected period (Approved Date).<br>" + //misdetail table
                        "<b>No. of Bills generated  </b>: All Ok bills generated in selected perion (Created On Date)<br>" +
                        "<b>Value of Bills generated </b>: value of all ok bills generated in selected period (Created On Date)<br>" +
                        "<b>No. of Reports printed </b>: All reports issued in selected period (Issue Date).<br>" + //misdetail table
                        "<b>Value of money received </b>: Total value of receipt amount with tds in selected period (Cash Date)<br>" +
                        "<b>Value of outstanding </b>: Total outstanding amount till date.<br>"; //bill, journal, Cashdetail                        
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("Category", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfTotalEnquiries", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfAutoEnquiries", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfProposalsSentCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfProposalsSent", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfOrdersReceivedCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersReceivedCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersFromNewClientCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersFromNewClient", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfEnquiriesAgainstWhichMaterialCollected", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfTestsCompleted", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfReportsApproved", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfBillsGenerated", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfBillsGenerated", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfReportsPrinted", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfBillsAndReportsReceivedByClient", typeof(string)));


            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int totNoOfTotalEnquiries = 0, totNoOfAutoEnquiries = 0, totNoOfProposalsSentCurrent = 0, totNoOfProposalsSent = 0, totNoOfOrdersReceivedCurrent = 0, totNoOfOrdersReceived = 0,
                totNoOfEnquiriesAgainstWhichMaterialCollected = 0, totNoOfReportsApproved = 0, totNoOfBillsGenerated = 0, totNoOfReportsPrinted = 0; //, totNoOfTestsCompleted = 0, totNoOfBillsAndReportsReceivedByClient = 0;
            decimal totValueOfOrdersReceived = 0, totValueOfOrdersReceivedCurrent = 0, totValueOfOrdersFromNewClientCurrent = 0, totValueOfOrdersFromNewClient = 0, totValueOfBillsGenerated = 0,
                totValueOfMoneyReceived = 0, totValueOfOutstanding = 0;
            var materialType = dc.Material_View("",""); 
            foreach(var mat in materialType)
            {
                dr1 = dt.NewRow();
                dr1["Category"] = mat.MATERIAL_RecordType_var;
                var reportList = dc.ReportBusinessSnapshot(Fromdate, Todate, mat.MATERIAL_RecordType_var);
                foreach (var rpt in reportList)
                {
                    dr1["NoOfTotalEnquiries"] = Convert.ToInt32(rpt.NoOfTotalEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfTotalEnquiriesGeneratedNew);
                    dr1["NoOfAutoEnquiries"] = Convert.ToInt32(rpt.NoOfAutoEnquiries) + Convert.ToInt32(rpt.NoOfAutoEnquiriesNew);
                    dr1["NoOfProposalsSentCurrent"] = Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                    dr1["NoOfProposalsSent"] = Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                    dr1["NoOfOrdersReceivedCurrent"] = Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                    dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    dr1["ValueOfOrdersReceivedCurrent"] = Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                    dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                    dr1["ValueOfOrdersFromNewClientCurrent"] = Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNewCurrent);
                    dr1["ValueOfOrdersFromNewClient"] = Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                    dr1["NoOfEnquiriesAgainstWhichMaterialCollected"] = Convert.ToInt32(rpt.NoOfEnquiriesAgainstWhichMaterialCollected);
                    dr1["NoOfTestsCompleted"] = "";
                    dr1["NoOfReportsApproved"] = Convert.ToInt32(rpt.NoOfReportsApproved);
                    dr1["NoOfBillsGenerated"] = Convert.ToInt32(rpt.NoOfBillsGenerated);
                    dr1["ValueOfBillsGenerated"] = Convert.ToDecimal(rpt.ValueOfBillsGenerated);
                    dr1["NoOfReportsPrinted"] = Convert.ToInt32(rpt.NoOfReportsPrinted);
                    dr1["NoOfBillsAndReportsReceivedByClient"] = "";

                    totNoOfTotalEnquiries += Convert.ToInt32(rpt.NoOfTotalEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfTotalEnquiriesGeneratedNew);
                    totNoOfAutoEnquiries += Convert.ToInt32(rpt.NoOfAutoEnquiries) + Convert.ToInt32(rpt.NoOfAutoEnquiriesNew);
                    totNoOfProposalsSent += Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                    totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                    totNoOfProposalsSentCurrent += Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                    totNoOfOrdersReceivedCurrent += Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                    totValueOfOrdersReceivedCurrent += Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                    totValueOfOrdersFromNewClientCurrent += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNewCurrent);
                    totValueOfOrdersFromNewClient += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                    totNoOfEnquiriesAgainstWhichMaterialCollected += Convert.ToInt32(rpt.NoOfEnquiriesAgainstWhichMaterialCollected);
                    totNoOfReportsApproved += Convert.ToInt32(rpt.NoOfReportsApproved);
                    totNoOfBillsGenerated += Convert.ToInt32(rpt.NoOfBillsGenerated);
                    totValueOfBillsGenerated += Convert.ToDecimal(rpt.ValueOfBillsGenerated);
                    totNoOfReportsPrinted += Convert.ToInt32(rpt.NoOfReportsPrinted);
                }
                dt.Rows.Add(dr1);
            }
            //Coupon
            dr1 = dt.NewRow();
            dr1["Category"] = "Coupon";
            var reportListC = dc.ReportBusinessSnapshot(Fromdate, Todate, "---");
            foreach (var rpt in reportListC)
            {
                dr1["NoOfTotalEnquiries"] = Convert.ToInt32(rpt.NoOfTotalEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfTotalEnquiriesGeneratedNew);
                dr1["NoOfAutoEnquiries"] = Convert.ToInt32(rpt.NoOfAutoEnquiries) + Convert.ToInt32(rpt.NoOfAutoEnquiriesNew);
                dr1["NoOfProposalsSentCurrent"] = Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                dr1["NoOfProposalsSent"] = Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                dr1["NoOfOrdersReceivedCurrent"] = Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                dr1["ValueOfOrdersReceivedCurrent"] = Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                dr1["ValueOfOrdersFromNewClientCurrent"] = Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNewCurrent);
                dr1["ValueOfOrdersFromNewClient"] = Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                dr1["NoOfEnquiriesAgainstWhichMaterialCollected"] = Convert.ToInt32(rpt.NoOfEnquiriesAgainstWhichMaterialCollected);
                dr1["NoOfTestsCompleted"] = "";
                dr1["NoOfReportsApproved"] = Convert.ToInt32(rpt.NoOfReportsApproved);
                dr1["NoOfBillsGenerated"] = Convert.ToInt32(rpt.NoOfBillsGenerated);
                dr1["ValueOfBillsGenerated"] = Convert.ToDecimal(rpt.ValueOfBillsGenerated);
                dr1["NoOfReportsPrinted"] = Convert.ToInt32(rpt.NoOfReportsPrinted);
                dr1["NoOfBillsAndReportsReceivedByClient"] = "";

                totNoOfTotalEnquiries += Convert.ToInt32(rpt.NoOfTotalEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfTotalEnquiriesGeneratedNew);
                totNoOfAutoEnquiries += Convert.ToInt32(rpt.NoOfAutoEnquiries) + Convert.ToInt32(rpt.NoOfAutoEnquiriesNew);
                totNoOfProposalsSent += Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                totNoOfProposalsSentCurrent += Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                totNoOfOrdersReceivedCurrent += Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                totValueOfOrdersReceivedCurrent += Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                totValueOfOrdersFromNewClientCurrent += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNewCurrent);
                totValueOfOrdersFromNewClient += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                totNoOfEnquiriesAgainstWhichMaterialCollected += Convert.ToInt32(rpt.NoOfEnquiriesAgainstWhichMaterialCollected);
                totNoOfReportsApproved += Convert.ToInt32(rpt.NoOfReportsApproved);
                totNoOfBillsGenerated += Convert.ToInt32(rpt.NoOfBillsGenerated);
                totValueOfBillsGenerated += Convert.ToDecimal(rpt.ValueOfBillsGenerated);
                totNoOfReportsPrinted += Convert.ToInt32(rpt.NoOfReportsPrinted);
            }
            dt.Rows.Add(dr1);
            //Monthly
            dr1 = dt.NewRow();
            dr1["Category"] = "Monthly";
            var reportListM = dc.ReportBusinessSnapshot(Fromdate, Todate, "Monthly");
            foreach (var rpt in reportListM)
            {
                dr1["NoOfTotalEnquiries"] = Convert.ToInt32(rpt.NoOfTotalEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfTotalEnquiriesGeneratedNew);
                dr1["NoOfAutoEnquiries"] = Convert.ToInt32(rpt.NoOfAutoEnquiries) + Convert.ToInt32(rpt.NoOfAutoEnquiriesNew);
                dr1["NoOfProposalsSentCurrent"] = Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                dr1["NoOfProposalsSent"] = Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                dr1["NoOfOrdersReceivedCurrent"] = Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                dr1["ValueOfOrdersReceivedCurrent"] = Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                dr1["ValueOfOrdersFromNewClientCurrent"] = Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNewCurrent);
                dr1["ValueOfOrdersFromNewClient"] = Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                dr1["NoOfEnquiriesAgainstWhichMaterialCollected"] = Convert.ToInt32(rpt.NoOfEnquiriesAgainstWhichMaterialCollected);
                dr1["NoOfTestsCompleted"] = "";
                dr1["NoOfReportsApproved"] = Convert.ToInt32(rpt.NoOfReportsApproved);
                dr1["NoOfBillsGenerated"] = Convert.ToInt32(rpt.NoOfBillsGenerated);
                dr1["ValueOfBillsGenerated"] = Convert.ToDecimal(rpt.ValueOfBillsGenerated);
                dr1["NoOfReportsPrinted"] = Convert.ToInt32(rpt.NoOfReportsPrinted);
                dr1["NoOfBillsAndReportsReceivedByClient"] = "";

                totNoOfTotalEnquiries += Convert.ToInt32(rpt.NoOfTotalEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfTotalEnquiriesGeneratedNew);
                totNoOfAutoEnquiries += Convert.ToInt32(rpt.NoOfAutoEnquiries) + Convert.ToInt32(rpt.NoOfAutoEnquiriesNew);
                totNoOfProposalsSent += Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                totNoOfProposalsSentCurrent += Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                totNoOfOrdersReceivedCurrent += Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                totValueOfOrdersReceivedCurrent += Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                totValueOfOrdersFromNewClientCurrent += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNewCurrent);
                totValueOfOrdersFromNewClient += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                totNoOfEnquiriesAgainstWhichMaterialCollected += Convert.ToInt32(rpt.NoOfEnquiriesAgainstWhichMaterialCollected);
                totNoOfReportsApproved += Convert.ToInt32(rpt.NoOfReportsApproved);
                totNoOfBillsGenerated += Convert.ToInt32(rpt.NoOfBillsGenerated);
                totValueOfBillsGenerated += Convert.ToDecimal(rpt.ValueOfBillsGenerated);
                totNoOfReportsPrinted += Convert.ToInt32(rpt.NoOfReportsPrinted);
            }
            dt.Rows.Add(dr1);
            ////
            var reportList1 = dc.ReportSalesStatusTotal(Fromdate, Todate);
            foreach (var rpt1 in reportList1)
            {
                totValueOfOutstanding = Convert.ToDecimal(rpt1.TotalOutstandingValue);
                totValueOfMoneyReceived = Convert.ToDecimal(rpt1.TotalMoneyReceived);
            }
            lblTotal.Text = "Value of money received : " + totValueOfMoneyReceived.ToString("0.00") + "&nbsp;&nbsp;&nbsp;&nbsp;" + " Value of outstanding : " + totValueOfOutstanding.ToString("0.00");
            //
            dr1 = dt.NewRow();
            dr1["Category"] = "Total = ";
            dr1["NoOfTotalEnquiries"] = totNoOfTotalEnquiries;
            dr1["NoOfAutoEnquiries"] = totNoOfAutoEnquiries;
            dr1["NoOfProposalsSentCurrent"] = totNoOfProposalsSentCurrent;
            dr1["NoOfProposalsSent"] = totNoOfProposalsSent;
            dr1["NoOfOrdersReceivedCurrent"] = totNoOfOrdersReceivedCurrent;
            dr1["NoOfOrdersReceived"] = totNoOfOrdersReceived;
            dr1["ValueOfOrdersReceivedCurrent"] = totValueOfOrdersReceivedCurrent.ToString("0.00");
            dr1["ValueOfOrdersReceived"] = totValueOfOrdersReceived.ToString("0.00");
            dr1["ValueOfOrdersFromNewClientCurrent"] = totValueOfOrdersFromNewClientCurrent.ToString("0.00");
            dr1["ValueOfOrdersFromNewClient"] = totValueOfOrdersFromNewClient.ToString("0.00");
            dr1["NoOfEnquiriesAgainstWhichMaterialCollected"] = totNoOfEnquiriesAgainstWhichMaterialCollected;
            dr1["NoOfTestsCompleted"] = "";
            dr1["NoOfReportsApproved"] = totNoOfReportsApproved;
            dr1["NoOfBillsGenerated"] = totNoOfBillsGenerated;
            dr1["ValueOfBillsGenerated"] = totValueOfBillsGenerated.ToString("0.00");
            dr1["NoOfReportsPrinted"] = totNoOfReportsPrinted;
            dr1["NoOfBillsAndReportsReceivedByClient"] = "";
            dt.Rows.Add(dr1);
            grdReport.DataSource = dt;
            grdReport.DataBind();

            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                LinkButton lnkNoOfTotalEnquiries = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfTotalEnquiries");
                LinkButton lnkNoOfAutoEnquiries = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfAutoEnquiries");
                LinkButton lnkNoOfProposalsSentCurrent = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfProposalsSentCurrent");
                LinkButton lnkNoOfProposalsSent = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfProposalsSent");
                LinkButton lnkNoOfOrdersReceivedCurrent = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfOrdersReceivedCurrent");
                LinkButton lnkNoOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfOrdersReceived");
                LinkButton lnkValueOfOrdersReceivedCurrent = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersReceivedCurrent");
                LinkButton lnkValueOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersReceived");
                LinkButton lnkValueOfOrdersFromNewClientCurrent = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersFromNewClientCurrent");
                LinkButton lnkValueOfOrdersFromNewClient = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersFromNewClient");
                LinkButton lnkNoOfEnquiriesAgainstWhichMaterialCollected = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfEnquiriesAgainstWhichMaterialCollected");
                LinkButton lnkNoOfTestsCompleted = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfTestsCompleted");
                LinkButton lnkNoOfReportsApproved = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfReportsApproved");
                LinkButton lnkNoOfBillsGenerated = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfBillsGenerated");
                LinkButton lnkValueOfBillsGenerated = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfBillsGenerated");
                LinkButton lnkNoOfReportsPrinted = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfReportsPrinted");
                LinkButton lnkNoOfBillsAndReportsReceivedByClient = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfBillsAndReportsReceivedByClient");
                if (i == grdReport.Rows.Count - 1)
                {
                    lnkNoOfTotalEnquiries.Enabled = false;
                    lnkNoOfAutoEnquiries.Enabled = false;
                    lnkNoOfProposalsSentCurrent.Enabled = false;
                    lnkNoOfProposalsSent.Enabled = false;
                    lnkNoOfOrdersReceivedCurrent.Enabled = false;
                    lnkNoOfOrdersReceived.Enabled = false;
                    lnkValueOfOrdersReceivedCurrent.Enabled = false;
                    lnkValueOfOrdersReceived.Enabled = false;
                    lnkValueOfOrdersFromNewClientCurrent.Enabled = false;
                    lnkValueOfOrdersFromNewClient.Enabled = false;
                    lnkNoOfEnquiriesAgainstWhichMaterialCollected.Enabled = false;
                    lnkNoOfTestsCompleted.Enabled = false;
                    lnkNoOfReportsApproved.Enabled = false;
                    lnkNoOfBillsGenerated.Enabled = false;
                    lnkValueOfBillsGenerated.Enabled = false;
                    lnkNoOfReportsPrinted.Enabled = false;
                    lnkNoOfBillsAndReportsReceivedByClient.Enabled = false;
                }
                else
                {
                    if (lnkNoOfTotalEnquiries.Text == "0")
                        lnkNoOfTotalEnquiries.Text = "";
                    if (lnkNoOfAutoEnquiries.Text == "0")
                        lnkNoOfAutoEnquiries.Text = "";
                    if (lnkNoOfProposalsSentCurrent.Text == "0")
                        lnkNoOfProposalsSentCurrent.Text = "";
                    if (lnkNoOfProposalsSent.Text == "0")
                        lnkNoOfProposalsSent.Text = "";
                    if (lnkNoOfOrdersReceivedCurrent.Text == "0")
                        lnkNoOfOrdersReceivedCurrent.Text = "";
                    if (lnkNoOfOrdersReceived.Text == "0")
                        lnkNoOfOrdersReceived.Text = "";
                    if (lnkValueOfOrdersReceivedCurrent.Text == "0")
                        lnkValueOfOrdersReceivedCurrent.Text = "";
                    if (lnkValueOfOrdersReceived.Text == "0")
                        lnkValueOfOrdersReceived.Text = "";
                    if (lnkValueOfOrdersFromNewClientCurrent.Text == "0")
                        lnkValueOfOrdersFromNewClientCurrent.Text = "";
                    if (lnkValueOfOrdersFromNewClient.Text == "0")
                        lnkValueOfOrdersFromNewClient.Text = "";
                    if (lnkNoOfEnquiriesAgainstWhichMaterialCollected.Text == "0")
                        lnkNoOfEnquiriesAgainstWhichMaterialCollected.Text = "";
                    if (lnkNoOfTestsCompleted.Text == "0")
                        lnkNoOfTestsCompleted.Text = "";
                    if (lnkNoOfReportsApproved.Text == "0")
                        lnkNoOfReportsApproved.Text = "";
                    if (lnkNoOfBillsGenerated.Text == "0")
                        lnkNoOfBillsGenerated.Text = "";
                    if (lnkValueOfBillsGenerated.Text == "0")
                        lnkValueOfBillsGenerated.Text = "";
                    if (lnkNoOfReportsPrinted.Text == "0")
                        lnkNoOfReportsPrinted.Text = "";
                    if (lnkNoOfBillsAndReportsReceivedByClient.Text == "0")
                        lnkNoOfBillsAndReportsReceivedByClient.Text = "";
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReport.Rows.Count > 0 && grdReport.Visible == true)
            {
                string Subheader = "";

                Subheader = lblFromDate.Text + "\t" + txtFromDate.Text + "~" + lblToDate.Text + "\t" + txtToDate.Text;
                
                //PrintGrid.PrintGridView(grdReport, Subheader, "EnquiryStatus_Report");

                var fileName = "BusinessSnapshot" + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                PrintGrid.PrintGridView_BusinessSnapshot(grdReport, Subheader, fileName);

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

        protected void lnkViewDescription_Click(object sender, EventArgs e)
        {
            if (lblDescription.Visible == false)
            {
                lblDescription.Visible = true;
                pnlDescription.Visible = true;
                pnlReport.Height = 300;
            }
            else
            {
                lblDescription.Visible = false;
                pnlDescription.Visible = false;
                pnlReport.Height = 400;
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
            if (e.CommandName == "lnkNoOfTotalEnquiries" || e.CommandName == "lnkNoOfAutoEnquiries" 
                || e.CommandName == "lnkNoOfEnquiriesAgainstWhichMaterialCollected")
            {
                for (int i = 1; i <= 12; i++)
                {
                    grdDetails.Columns[i].Visible = true;
                }
            }
            else if (e.CommandName == "lnkNoOfProposalsSentCurrent" || e.CommandName == "lnkNoOfProposalsSent"
                || e.CommandName == "lnkNoOfOrdersReceivedCurrent" || e.CommandName == "lnkNoOfOrdersReceived"
                || e.CommandName == "lnkValueOfOrdersReceivedCurrent" || e.CommandName == "lnkValueOfOrdersReceived"
                || e.CommandName == "lnkValueOfOrdersFromNewClientCurrent" || e.CommandName == "lnkValueOfOrdersFromNewClient")
            {
                for (int i = 1; i <= 17; i++)
                {
                    grdDetails.Columns[i].Visible = true;
                }
            }
            else if (e.CommandName == "lnkNoOfBillsGenerated" || e.CommandName == "lnkValueOfBillsGenerated")
            {
                grdDetails.Columns[1].Visible = true;
                grdDetails.Columns[2].Visible = true;
                for (int i = 22; i <= 26; i++)
                {
                    grdDetails.Columns[i].Visible = true;
                }
            }
            else if (e.CommandName == "lnkNoOfReportsApproved")
            {
                grdDetails.Columns[1].Visible = true;
                grdDetails.Columns[2].Visible = true;
                for (int i = 18; i <= 20; i++)
                {
                    grdDetails.Columns[i].Visible = true;
                }
            }
            else if (e.CommandName == "lnkNoOfReportsPrinted")
            {
                grdDetails.Columns[1].Visible = true;
                grdDetails.Columns[2].Visible = true;
                for (int i = 18; i <= 21; i++)
                {
                    grdDetails.Columns[i].Visible = true;
                }
                grdDetails.Columns[20].Visible = false;
            }
            //////
            if (e.CommandName == "lnkNoOfTotalEnquiries")
            {
                detailFlag = 0;
            }
            else if (e.CommandName == "lnkNoOfAutoEnquiries")
            {
                detailFlag = 1;
            }
            else if (e.CommandName == "lnkNoOfProposalsSentCurrent")
            {
                detailFlag = 2;
            }
            else if (e.CommandName == "lnkNoOfProposalsSent")
            {
                detailFlag = 3;
            }
            else if (e.CommandName == "lnkNoOfOrdersReceivedCurrent")
            {
                detailFlag = 4;
            }
            else if (e.CommandName == "lnkNoOfOrdersReceived")
            {
                detailFlag = 5;
            }
            else if (e.CommandName == "lnkValueOfOrdersReceivedCurrent")
            {
                detailFlag = 4;
            }
            else if (e.CommandName == "lnkValueOfOrdersReceived")
            {
                detailFlag = 5;
            }
            else if (e.CommandName == "lnkValueOfOrdersFromNewClientCurrent")
            {
                detailFlag = 6;
            }
            else if (e.CommandName == "lnkValueOfOrdersFromNewClient")
            {
                detailFlag = 7;
            }
            else if (e.CommandName == "lnkNoOfEnquiriesAgainstWhichMaterialCollected")
            {
                detailFlag = 8;
            }
            else if (e.CommandName == "lnkNoOfReportsApproved")
            {
                detailFlag = 9;
            }
            else if (e.CommandName == "lnkNoOfBillsGenerated")
            {
                detailFlag = 10;
            }
            else if (e.CommandName == "lnkValueOfBillsGenerated")
            {
                detailFlag = 10;
            }
            else if (e.CommandName == "lnkNoOfReportsPrinted")
            {
                detailFlag = 11;
            }
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

            var detail = dc.ReportBusinessSnapshot_Details(Fromdate, Todate, recType, Convert.ToByte(detailFlag));
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