using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportMarketVisitStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Market Visit Status";
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
                    lblDescription.Text = "<b>No. of enquiries generated </b>: All enquiries(Regular & New Client) generated in selected period (Enquiry Date).<br>" +
                        "<b>No. of hot enquiries </b>: .<br>" +
                        "<b>No. of warm enquiries </b>: .<br>" +
                        "<b>No. of auto enquiries/ enquiries received </b>: .<br>" +
                        "<b>No. of orders received </b>: All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>Value of orders received </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to client and order received against proposal in selected period (Proposal Order Date).<br>" +
                        "<b>Value of orders from new clients </b>: Order Value of All enquiries(Regular & New Client) having proposal sent to new client(No bill generated before 6 months from selected from date) and order received against proposal in selected period (Proposal Order Date).<br>" +
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

        private void LoadMEList()
        {
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            var me = dc.ReportMarketVisitStatus_ME(Fromdate, Todate);
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
            var route = dc.ReportMarketVisitStatus_Route(Fromdate, Todate);
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
            dt.Columns.Add(new DataColumn("NoOfEnquiriesGenerated", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfHotEnquiries", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfWarmEnquiries", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfAutoEnquiries", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("ValueOfOrdersFromNewClient", typeof(string)));
            dt.Columns.Add(new DataColumn("NoOfPendingEnquiries", typeof(string)));

            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int totNoOfEnquiriesGenerated = 0, totNoOfOrdersReceived = 0, totNoOfPendingEnquiries = 0, totNoOfClientsVisited = 0; //, totNoOfHotEnquiries = 0, totNoOfWarmEnquiries = 0, totNoOfAutoEnquiries = 0,
            decimal totValueOfOrdersReceived = 0, totValueOfOrdersFromNewClient = 0;
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
                var reportList = dc.ReportMarketVisitStatus(Fromdate, Todate, mat.MATERIAL_RecordType_var, MEId, RouteId);
                foreach (var rpt in reportList)
                {
                    if (MEId > 0 || RouteId > 0)
                    {
                        dr1["NoOfEnquiriesGenerated"] = Convert.ToInt32(rpt.NoOfEnquiriesGenerated);
                        //dr1["NoOfHotEnquiries"] = rpt.NoOfHotEnquiries.ToString();
                        //dr1["NoOfWarmEnquiries"] = rpt.NoOfHotEnquiries.ToString();
                        //dr1["NoOfAutoEnquiries"] = rpt.NoOfAutoEnquiries.ToString();
                        dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived);
                        dr1["ValueOfOrdersReceived"] = Convert.ToInt32(rpt.ValueOfOrdersReceived);
                        dr1["ValueOfOrdersFromNewClient"] = Convert.ToInt32(rpt.ValueOfOrdersFromNewClient);
                        dr1["NoOfPendingEnquiries"] = Convert.ToInt32(rpt.NoOfPendingEnquiries);

                        totNoOfEnquiriesGenerated += Convert.ToInt32(rpt.NoOfEnquiriesGenerated);
                        //totNoOfHotEnquiries += Convert.ToInt32(rpt.NoOfEnquiriesGenerated);
                        //totNoOfWarmEnquiries += Convert.ToInt32(rpt.NoOfEnquiriesGenerated);
                        //totNoOfAutoEnquiries += Convert.ToInt32(rpt.NoOfAutoEnquiries);
                        totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived);
                        totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived);
                        totValueOfOrdersFromNewClient += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient);
                        totNoOfPendingEnquiries += Convert.ToInt32(rpt.NoOfPendingEnquiries);

                        totNoOfClientsVisited += Convert.ToInt32(rpt.NoOfClientsVisited);
                    }
                    else
                    {
                        dr1["NoOfEnquiriesGenerated"] = Convert.ToInt32(rpt.NoOfEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfEnquiriesGeneratedNew);
                        //dr1["NoOfHotEnquiries"] = rpt.NoOfHotEnquiries.ToString();
                        //dr1["NoOfWarmEnquiries"] = rpt.NoOfHotEnquiries.ToString();
                        //dr1["NoOfAutoEnquiries"] = rpt.NoOfAutoEnquiries.ToString();
                        dr1["NoOfOrdersReceived"] = Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                        dr1["ValueOfOrdersReceived"] = Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                        dr1["ValueOfOrdersFromNewClient"] = Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                        dr1["NoOfPendingEnquiries"] = Convert.ToInt32(rpt.NoOfPendingEnquiries) + Convert.ToInt32(rpt.NoOfPendingEnquiriesNew);

                        totNoOfEnquiriesGenerated += Convert.ToInt32(rpt.NoOfEnquiriesGenerated) + Convert.ToInt32(rpt.NoOfEnquiriesGeneratedNew);
                        //totNoOfHotEnquiries += Convert.ToInt32(rpt.NoOfEnquiriesGenerated);
                        //totNoOfWarmEnquiries += Convert.ToInt32(rpt.NoOfEnquiriesGenerated);
                        //totNoOfAutoEnquiries += Convert.ToInt32(rpt.NoOfAutoEnquiries);
                        totNoOfOrdersReceived += Convert.ToInt32(rpt.NoOfOrdersReceived) + Convert.ToInt32(rpt.NoOfOrdersReceivedNew);
                        totValueOfOrdersReceived += Convert.ToDecimal(rpt.ValueOfOrdersReceived) + Convert.ToDecimal(rpt.ValueOfOrdersReceivedNew);
                        totValueOfOrdersFromNewClient += Convert.ToDecimal(rpt.ValueOfOrdersFromNewClient) + Convert.ToDecimal(rpt.ValueOfOrdersFromNewClientNew);
                        totNoOfPendingEnquiries += Convert.ToInt32(rpt.NoOfPendingEnquiries) + Convert.ToInt32(rpt.NoOfPendingEnquiriesNew);

                        totNoOfClientsVisited += Convert.ToInt32(rpt.NoOfClientsVisited);
                    }
                }
                dt.Rows.Add(dr1);
            }
            dr1 = dt.NewRow();
            dr1["Category"] = "Total = ";
            dr1["NoOfEnquiriesGenerated"] = totNoOfEnquiriesGenerated;
            //dr1["NoOfHotEnquiries"] = totNoOfHotEnquiries;
            //dr1["NoOfWarmEnquiries"] = totNoOfWarmEnquiries;
            //dr1["NoOfAutoEnquiries"] = totNoOfAutoEnquiries;
            dr1["NoOfOrdersReceived"] = totNoOfOrdersReceived;
            dr1["ValueOfOrdersReceived"] = totValueOfOrdersReceived.ToString("0.00");
            dr1["ValueOfOrdersFromNewClient"] = totValueOfOrdersFromNewClient.ToString("0.00");
            dr1["NoOfPendingEnquiries"] = totNoOfPendingEnquiries;
            dt.Rows.Add(dr1);
            lblTotal.Text = "No. of clients visited : " + totNoOfClientsVisited.ToString();
            grdReport.DataSource = dt;
            grdReport.DataBind();

            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                LinkButton lnkNoOfEnquiriesGenerated = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfEnquiriesGenerated");
                LinkButton lnkNoOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfOrdersReceived");
                LinkButton lnkValueOfOrdersReceived = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersReceived");
                LinkButton lnkValueOfOrdersFromNewClient = (LinkButton)grdReport.Rows[i].FindControl("lnkValueOfOrdersFromNewClient");
                LinkButton lnkNoOfPendingEnquiries = (LinkButton)grdReport.Rows[i].FindControl("lnkNoOfPendingEnquiries");
                if (i == grdReport.Rows.Count - 1)
                {
                    lnkNoOfEnquiriesGenerated.Enabled = false;
                    lnkNoOfOrdersReceived.Enabled = false;
                    lnkValueOfOrdersReceived.Enabled = false;
                    lnkValueOfOrdersFromNewClient.Enabled = false;
                    lnkNoOfPendingEnquiries.Enabled = false;
                }
                else
                {
                    if (lnkNoOfEnquiriesGenerated.Text == "0")
                        lnkNoOfEnquiriesGenerated.Text = "";
                    if (lnkNoOfOrdersReceived.Text == "0")
                        lnkNoOfOrdersReceived.Text = "";
                    if (lnkValueOfOrdersReceived.Text == "0")
                        lnkValueOfOrdersReceived.Text = "";
                    if (lnkValueOfOrdersFromNewClient.Text == "0")
                        lnkValueOfOrdersFromNewClient.Text = "";
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
                if (ddlME.SelectedIndex > 0)
                    Subheader += "~" + "Marketing Person" + "\t" + ddlME.SelectedItem.Text;
                if (ddlRoute.SelectedIndex > 0)
                    Subheader += "~" + "Route" + "\t" + ddlRoute.SelectedItem.Text;

                //PrintGrid.PrintGridView(grdReport, Subheader, "MarketVisitStatus_Report");
                
                var fileName = "MarketVisitStatus" + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                PrintGrid.PrintGridView_MarketVisitStatus(grdReport, Subheader, fileName);
                
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
        }

        protected void lnkViewDescription_Click(object sender, EventArgs e)
        {
            if (lblDescription.Visible == false)
            {
                lblDescription.Visible = true;
                pnlReport.Height = 200;
            }
            else
            {
                lblDescription.Visible = false;
                pnlReport.Height = 400;
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
                        ddlME.SelectedValue = rt.ROUTE_ME_Id.ToString();
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
            if (e.CommandName == "lnkNoOfEnquiriesGenerated" || e.CommandName == "lnkNoOfPendingEnquiries")
            {
                grdDetails.Columns[13].Visible = false;
                grdDetails.Columns[14].Visible = false;
                grdDetails.Columns[15].Visible = false;
                grdDetails.Columns[16].Visible = false;
                grdDetails.Columns[17].Visible = false;
            }
            else if (e.CommandName == "lnkNoOfOrdersReceived" || e.CommandName == "lnkValueOfOrdersReceived" || e.CommandName == "lnkValueOfOrdersFromNewClient")
            {
                grdDetails.Columns[13].Visible = true;
                grdDetails.Columns[14].Visible = true;
                grdDetails.Columns[15].Visible = true;
                grdDetails.Columns[16].Visible = true;
                grdDetails.Columns[17].Visible = true;
            }

            if (e.CommandName == "lnkNoOfEnquiriesGenerated")
            {
                detailFlag = 0;
            }
            else if (e.CommandName == "lnkNoOfOrdersReceived" || e.CommandName == "lnkValueOfOrdersReceived")
            {
                detailFlag = 1;
            }
            else if (e.CommandName == "lnkValueOfOrdersFromNewClient")
            {
                detailFlag = 2;
            }
            else if (e.CommandName == "lnkNoOfPendingEnquiries")
            {
                detailFlag = 3;
            }
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int MEId = 0, RouteId = 0;
            if (ddlME.SelectedIndex > 0)
            {
                MEId = Convert.ToInt32(ddlME.SelectedValue);
            }
            if (ddlRoute.SelectedIndex > 0)
            {
                RouteId = Convert.ToInt32(ddlRoute.SelectedValue);
            }
            var detail = dc.ReportMarketVisitStatus_Details(Fromdate, Todate, recType, MEId, RouteId, Convert.ToByte(detailFlag));
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