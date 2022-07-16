using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportLogisticsStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Logistics Status";

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
                    lblDescription.Text = "<b>No. of orders received </b>: All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>Material collected </b>: All enquiries(Regular) having material collected in selected period (Collected On Date).<br>" +
                        "<b>Reports printed </b>: All reports printed in selected period (Issue Date).<br>" + //misdetail table
                        "<b>Report pending for printing due to credit limit </b>: All reports pending to print having client outstanding amount is greater than credit limit(Excluding cube report having coupons) and approved in selected period (Approval Date).<br>" +  //misdetail table
                        "<b>Reports outward </b>: All reports outward in selected period (Outward Date).<br>" + //misdetail table
                        "<b>Reports received by client </b>: All reports physical outward done in selected period (Physical Outward Date).<br>"; //misdetail table
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
            dt.Columns.Add(new DataColumn("NoOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("MaterialCollected", typeof(string)));
            dt.Columns.Add(new DataColumn("ReportsPrinted", typeof(string)));
            dt.Columns.Add(new DataColumn("ReportPendingForPrintingDueToCRL", typeof(string)));
            dt.Columns.Add(new DataColumn("ReportsOutward", typeof(string)));
            dt.Columns.Add(new DataColumn("ReportsReceivedByClient", typeof(string)));

            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int totNoOfOrdersReceived = 0, totMaterialCollected = 0, totReportsPrinted = 0, totReportPendingForPrintingDueToCRL = 0,
                totReportsOutward = 0, totReportsReceivedByClient = 0;
            var materialType = dc.Material_View("",""); 
            foreach(var mat in materialType)
            {
                dr1 = dt.NewRow();
                dr1["Category"] = mat.MATERIAL_RecordType_var;
                var reportList = dc.ReportLogisticsStatus(Fromdate, Todate, mat.MATERIAL_RecordType_var);
                foreach (var rpt in reportList)
                {
                    dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    dr1["MaterialCollected"] = Convert.ToInt32(rpt.NoOfMaterialCollected);
                    dr1["ReportsPrinted"] = Convert.ToInt32(rpt.NoOfReportsPrinted);
                    dr1["ReportPendingForPrintingDueToCRL"] = Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    dr1["ReportsOutward"] = Convert.ToInt32(rpt.NoOfReportsOutward);
                    dr1["ReportsReceivedByClient"] = Convert.ToInt32(rpt.NoOfReportsReceivedByClient);

                    totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    totMaterialCollected += Convert.ToInt32(rpt.NoOfMaterialCollected);
                    totReportsPrinted += Convert.ToInt32(rpt.NoOfReportsPrinted);
                    totReportPendingForPrintingDueToCRL += Convert.ToInt32(rpt.NoOfReportsPendingForPrintingDueToCRL);
                    totReportsOutward += Convert.ToInt32(rpt.NoOfReportsOutward);
                    totReportsReceivedByClient += Convert.ToInt32(rpt.NoOfReportsReceivedByClient);
                }
                dt.Rows.Add(dr1);
            }
            dr1 = dt.NewRow();
            dr1["Category"] = "Total = ";
            dr1["NoOfOrdersReceived"] = totNoOfOrdersReceived;
            dr1["MaterialCollected"] = totMaterialCollected;
            dr1["ReportsPrinted"] = totReportsPrinted;
            dr1["ReportPendingForPrintingDueToCRL"] = totReportPendingForPrintingDueToCRL;
            dr1["ReportsOutward"] = totReportsOutward;
            dr1["ReportsReceivedByClient"] = totReportsReceivedByClient;
            dt.Rows.Add(dr1);
            grdReport.DataSource = dt;
            grdReport.DataBind();
            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                LinkButton lnkNoOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfOrdersReceived");
                LinkButton lnkMaterialCollected = (LinkButton)grdReport.Rows[i].FindControl("lnkMaterialCollected");
                LinkButton lnkReportsPrinted = (LinkButton)grdReport.Rows[i].FindControl("lnkReportsPrinted");
                LinkButton lnkReportPendingForPrintingDueToCRL = (LinkButton)grdReport.Rows[i].FindControl("lnkReportPendingForPrintingDueToCRL");
                LinkButton lnkReportsOutward = (LinkButton)grdReport.Rows[i].FindControl("lnkReportsOutward");
                LinkButton lnkReportsReceivedByClient = (LinkButton)grdReport.Rows[i].FindControl("lnkReportsReceivedByClient");
                if (i == grdReport.Rows.Count - 1)
                {
                    lnkNoOfOrdersReceived.Enabled = false;
                    lnkMaterialCollected.Enabled = false;
                    lnkReportsPrinted.Enabled = false;
                    lnkReportPendingForPrintingDueToCRL.Enabled = false;
                    lnkReportsOutward.Enabled = false;
                    lnkReportsReceivedByClient.Enabled = false;
                }
                else
                {
                    if (lnkNoOfOrdersReceived.Text == "0")
                        lnkNoOfOrdersReceived.Text = "";
                    if (lnkMaterialCollected.Text == "0")
                        lnkMaterialCollected.Text = "";
                    if (lnkReportsPrinted.Text == "0")
                        lnkReportsPrinted.Text = "";
                    if (lnkReportPendingForPrintingDueToCRL.Text == "0")
                        lnkReportPendingForPrintingDueToCRL.Text = "";
                    if (lnkReportsOutward.Text == "0")
                        lnkReportsOutward.Text = "";
                    if (lnkReportsReceivedByClient.Text == "0")
                        lnkReportsReceivedByClient.Text = "";
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReport.Rows.Count > 0 && grdReport.Visible == true)
            {
                string Subheader = "";

                Subheader = lblFromDate.Text + "\t" + txtFromDate.Text + "~" + lblToDate.Text + "\t" + txtToDate.Text;

                //PrintGrid.PrintGridView(grdReport, Subheader, "LogisticsStatus_Report");

                var fileName = "LogisticsStatus" + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                PrintGrid.PrintGridView_LogisticsStatus(grdReport, Subheader, fileName);

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
                pnlReport.Height = 300;
            }
            else
            {
                lblDescription.Visible = false;
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

            for (int i = 1; i < grdDetails.Columns.Count ; i++)
            {
                grdDetails.Columns[i].Visible = false;
            }
            if (e.CommandName == "lnkNoOfOrdersReceived")
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
            else if (e.CommandName == "lnkMaterialCollected") 
            {
                detailFlag = 1;
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i <= 12)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            else if (e.CommandName == "lnkReportsPrinted") 
            {
                detailFlag = 2; 
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i==1 || i == 2 || i == 18 || i == 19 || i== 23)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            else if (e.CommandName == "lnkReportPendingForPrintingDueToCRL")
            {
                detailFlag = 3; 
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i == 1 || i == 2 || i == 18 || i == 19 || i == 20 || i == 21 || i == 22)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            else if (e.CommandName == "lnkReportsOutward")
            {
                detailFlag = 4;
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i == 1 || i == 2 || i == 18 || i == 19 || i == 24 )
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            else if (e.CommandName == "lnkReportsReceivedByClient")
            {
                detailFlag = 5;
                for (int i = 1; i < grdDetails.Columns.Count; i++)
                {
                    if (i == 1 || i == 2 || i == 18 || i == 19 || i == 25)
                    {
                        grdDetails.Columns[i].Visible = true;
                    }
                }
            }
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            var detail = dc.ReportLogisticsStatus_Details(Fromdate, Todate, recType, Convert.ToByte(detailFlag));
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