using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportEnquiryStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry Status";
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

                    lblDescription.Text = "<b>No. of enquiries  </b>: All enquiries(Regular & New Client) generated in selected period (Enquiry Date).<br>" +
                        "<b>No. of proposals sent (Current) </b>: All enquiries(Regular & New Client) having proposal sent to client in selected period (Enquiry & Proposal Date).<br>" +
                        "<b>No. of proposals sent </b>: All enquiries(Regular & New Client) having proposal sent to client in selected period (Proposal Date).<br>" +
                        "<b>No. of orders received (Current) </b>: All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Enquiry & Proposal Order Date).<br>" +
                        "<b>No. of orders received </b>: All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>Value of orders received (Current) </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Enquiry & Proposal Order Date).<br>" +
                        "<b>Value of orders received </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>No. of pending enquiries till date </b>: All Open enquiries(Regular & New Client) generated till date .<br>";
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
            dt.Columns.Add(new DataColumn("NoOfEnquiries", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfProposalsSentCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfProposalsSent", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfOrdersReceivedCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersReceivedCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfPendingEnquiries", typeof(string)));

            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int totNoOfEnquiries = 0, totNoOfProposalsSent = 0, totNoOfOrdersReceived = 0, totNoOfProposalsSentCurrent = 0, totNoOfOrdersReceivedCurrent = 0, totNoOfPendingEnquiries = 0;
            decimal totValueOfOrdersReceived = 0, totValueOfOrdersReceivedCurrent = 0;
            var materialType = dc.Material_View("",""); 
            foreach(var mat in materialType)
            {
                dr1 = dt.NewRow();
                dr1["Category"] = mat.MATERIAL_RecordType_var;
                var reportList = dc.ReportEnquiryStatus(Fromdate, Todate, mat.MATERIAL_RecordType_var);
                foreach (var rpt in reportList)
                {
                    dr1["NoOfEnquiries"] = Convert.ToInt32(rpt.NoOfEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfEnquiriesGeneratedNew);
                    dr1["NoOfProposalsSentCurrent"] = Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                    dr1["NoOfProposalsSent"] = Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                    dr1["NoOfOrdersReceivedCurrent"] = Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                    dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    dr1["ValueOfOrdersReceivedCurrent"] = Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                    dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                    dr1["NoOfPendingEnquiries"] = Convert.ToInt32(rpt.NoOfPendingEnquiries) + Convert.ToInt32(rpt.NoOfPendingEnquiriesNew);

                    totNoOfEnquiries += Convert.ToInt32(rpt.NoOfEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfEnquiriesGeneratedNew);
                    totNoOfProposalsSent += Convert.ToInt32(rpt.NoOfProposalSent) + Convert.ToInt32(rpt.NoOfProposalSentNew);
                    totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                    totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                    totNoOfProposalsSentCurrent += Convert.ToInt32(rpt.NoOfProposalSentCurrent) + Convert.ToInt32(rpt.NoOfProposalSentNewCurrent);
                    totNoOfOrdersReceivedCurrent += Convert.ToInt32(rpt.NoOfOrdersReceivedCurrent) + Convert.ToInt32(rpt.NoOfOrdersReceivedNewCurrent);
                    totValueOfOrdersReceivedCurrent += Convert.ToDecimal(rpt.ValueOfOrdersReceivedCurrent) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNewCurrent);
                    totNoOfPendingEnquiries += Convert.ToInt32(rpt.NoOfPendingEnquiries) + Convert.ToInt32(rpt.NoOfPendingEnquiriesNew);
                }
                dt.Rows.Add(dr1);
            }
            dr1 = dt.NewRow();
            dr1["Category"] = "Total = ";
            dr1["NoOfEnquiries"] = totNoOfEnquiries;
            dr1["NoOfProposalsSent"] = totNoOfProposalsSent;
            dr1["NoOfOrdersReceived"] = totNoOfOrdersReceived;
            dr1["ValueOfOrdersReceived"] = totValueOfOrdersReceived.ToString("0.00");
            dr1["NoOfProposalsSentCurrent"] = totNoOfProposalsSentCurrent;
            dr1["NoOfOrdersReceivedCurrent"] = totNoOfOrdersReceivedCurrent;
            dr1["ValueOfOrdersReceivedCurrent"] = totValueOfOrdersReceivedCurrent.ToString("0.00");
            dr1["NoOfPendingEnquiries"] = totNoOfPendingEnquiries;
            dt.Rows.Add(dr1);
            grdReport.DataSource = dt;
            grdReport.DataBind();

            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                LinkButton lnkNoOfEnquiries = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfEnquiries");
                LinkButton lnkNoOfProposalsSentCurrent = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfProposalsSentCurrent");
                LinkButton lnkNoOfProposalsSent = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfProposalsSent");
                LinkButton lnkNoOfOrdersReceivedCurrent = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfOrdersReceivedCurrent");
                LinkButton lnkNoOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfOrdersReceived");
                LinkButton lnkValueOfOrdersReceivedCurrent = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersReceivedCurrent");
                LinkButton lnkValueOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersReceived");
                LinkButton lnkNoOfPendingEnquiries = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfPendingEnquiries");
                if (i == grdReport.Rows.Count - 1)
                {
                    lnkNoOfEnquiries.Enabled = false;
                    lnkNoOfProposalsSentCurrent.Enabled = false;
                    lnkNoOfProposalsSent.Enabled = false;
                    lnkNoOfOrdersReceivedCurrent.Enabled = false;
                    lnkNoOfOrdersReceived.Enabled = false;
                    lnkValueOfOrdersReceivedCurrent.Enabled = false;
                    lnkValueOfOrdersReceived.Enabled = false;
                    lnkNoOfPendingEnquiries.Enabled = false;
                }
                else
                {
                    if (lnkNoOfEnquiries.Text == "0")
                        lnkNoOfEnquiries.Text = "";
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
                    if (lnkNoOfPendingEnquiries.Text == "0")
                        lnkNoOfPendingEnquiries.Text = "";
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

                var fileName = "EnquiryStatus" + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                PrintGrid.PrintGridView_EnquiryStatus(grdReport, Subheader, fileName);

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
                pnlReport.Height = 260;
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
            if (e.CommandName == "lnkNoOfEnquiries" || e.CommandName == "lnkNoOfPendingEnquiries")
            {
                grdDetails.Columns[13].Visible = false;
                grdDetails.Columns[14].Visible = false;
                grdDetails.Columns[15].Visible = false;
                grdDetails.Columns[16].Visible = false;
                grdDetails.Columns[17].Visible = false;
            }
            else if (e.CommandName == "lnkNoOfProposalsSentCurrent" || e.CommandName == "lnkNoOfProposalsSent"
                || e.CommandName == "lnkNoOfOrdersReceivedCurrent" || e.CommandName == "lnkNoOfOrdersReceived"
                || e.CommandName == "lnkValueOfOrdersReceivedCurrent" || e.CommandName == "ValueOfOrdersReceived")
            {
                grdDetails.Columns[13].Visible = true;
                grdDetails.Columns[14].Visible = true;
                grdDetails.Columns[15].Visible = true;
                grdDetails.Columns[16].Visible = true;
                grdDetails.Columns[17].Visible = true;
            }

            //////
            if (e.CommandName == "lnkNoOfEnquiries")
            {
                detailFlag = 0;
            }
            else if (e.CommandName == "lnkNoOfProposalsSentCurrent")
            {
                detailFlag = 1;
            }
            else if (e.CommandName == "lnkNoOfProposalsSent")
            {
                detailFlag = 2;
            }
            else if (e.CommandName == "lnkNoOfOrdersReceivedCurrent")
            {
                detailFlag = 3;
            }
            else if (e.CommandName == "lnkNoOfOrdersReceived")
            {
                detailFlag = 4;
            }
            else if (e.CommandName == "lnkValueOfOrdersReceivedCurrent")
            {
                detailFlag = 3;
            }
            else if (e.CommandName == "lnkValueOfOrdersReceived")
            {
                detailFlag = 4;
            }
            else if (e.CommandName == "lnkNoOfPendingEnquiries")
            {
                detailFlag = 5;
            }
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

            var detail = dc.ReportEnquiryStatus_Details(Fromdate, Todate, recType, Convert.ToByte(detailFlag));
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