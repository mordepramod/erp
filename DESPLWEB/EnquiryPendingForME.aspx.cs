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

namespace DESPLWEB
{
    public partial class EnquiryPendingForME : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                //lblheading.Text = "Pending Enquiry & Report list";
                lblheading.Text = "List of reports ready but pending because of credit limit and pending enquiries";
                
                if (Session["Superadmin"] != null && Session["Superadmin"].ToString() == "True")
                {
                    lblSuperAdmin.Text = "SuperAdmin";
                    lnkFetch.Visible = true;
                }
                else
                {
                    ddlME.Visible = false;
                    lblME.Visible = false;
                    ddlMERpt.Visible = false;
                    lblMERpt.Visible = false;
                    LoadEnquiryList();
                    LoadReportList();
                }
                
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        
        private void LoadEnquiryList()
        {
            int MEId = Convert.ToInt32(Session["LoginId"]);
            if (lblSuperAdmin.Text == "SuperAdmin")
            {
                MEId = 0;
            }
            string strMEName = "", strInwardType = "", strEnqFrom = "";
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ENQ_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_OpenEnquiryStatus_var", typeof(string)));
            dt.Columns.Add(new DataColumn("EnteredFrom", typeof(string)));
            dt.Columns.Add(new DataColumn("EnteredByUser", typeof(string)));
            dt.Columns.Add(new DataColumn("MEName", typeof(string)));
            dt.Columns.Add(new DataColumn("ROUTE_ME_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_MATERIAL_Id", typeof(string)));

            var enq = dc.Enquiry_View_OpenMEWise(MEId);
            foreach (var c in enq)
            {
                dr = dt.NewRow();
                dr["ENQ_Id"] = c.ENQ_Id.ToString();
                dr["CL_Id"] = c.ENQ_CL_Id.ToString();
                dr["SITE_Id"] = c.ENQ_SITE_Id.ToString();
                dr["CONT_Id"] = c.ENQ_CONT_Id.ToString();
                dr["ENQ_Date_dt"] = Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");
                dr["MATERIAL_Name_var"] = Convert.ToString(c.MATERIAL_Name_var);
                dr["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                dr["SITE_Name_var"] = Convert.ToString(c.SITE_Name_var);
                dr["ENQ_OpenEnquiryStatus_var"] = Convert.ToString(c.ENQ_OpenEnquiryStatus_var);
                dr["EnteredFrom"] = c.EnteredFrom;
                dr["EnteredByUser"] = c.EnteredByUser;
                dr["MEName"] = c.MEName;
                dr["ROUTE_ME_Id"] = c.ROUTE_ME_Id;
                dr["ENQ_MATERIAL_Id"] = c.ENQ_MATERIAL_Id;
                dt.Rows.Add(dr);

                if (strMEName.Contains("," + c.ROUTE_ME_Id + ",") == false)
                {
                    ddlME.Items.Add(new ListItem(c.MEName, c.ROUTE_ME_Id.ToString()));
                    strMEName += "," + c.ROUTE_ME_Id.ToString() + ",";
                }
                if (strInwardType.Contains("," + c.ENQ_MATERIAL_Id + ",") == false)
                {
                    ddlInwardType.Items.Add(new ListItem(c.MATERIAL_Name_var, c.ENQ_MATERIAL_Id.ToString()));
                    strInwardType += "," + c.ENQ_MATERIAL_Id.ToString() + ",";
                }
                if (strEnqFrom.Contains("," + c.EnteredFrom + ",") == false)
                {
                    ddlEnteredFrom.Items.Add(new ListItem(c.EnteredFrom, c.EnteredFrom.ToString()));
                    strEnqFrom += "," + c.EnteredFrom.ToString() + ",";
                }
            }
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();
            ViewState["EnquiryTable"] = dt;
            SortListControl(ddlInwardType, true);
            SortListControl(ddlME, true);
            SortListControl(ddlEnteredFrom, true);
            ddlME.Items.Insert(0, new ListItem("---Select---", "0"));
            ddlInwardType.Items.Insert(0, new ListItem("---Select---", "0"));
            ddlEnteredFrom.Items.Insert(0, new ListItem("---Select---", "0"));

            lblTotalRecords.Text = "Total Records : " + grdEnquiry.Rows.Count;

        }
        
        private void FilterEnquiryList()
        {
            DataTable dt = ViewState["EnquiryTable"] as DataTable;
            int rowCount = 0;
            for (int i = 0; i < grdEnquiry.Rows.Count; i++)
            {
                grdEnquiry.Rows[i].Visible = true;
                //Label lblMEId = (Label)grdEnquiry.Rows[i].FindControl("lblMEId");
                //Label lblMaterialId = (Label)grdEnquiry.Rows[i].FindControl("lblMaterialId");
                //Label lblEnteredFrom = (Label)grdEnquiry.Rows[i].FindControl("lblEnteredFrom");

                //if (ddlInwardType.SelectedValue != lblMaterialId.Text && ddlInwardType.SelectedValue != "0")
                if (ddlInwardType.SelectedValue != dt.Rows[i]["ENQ_MATERIAL_Id"].ToString() && ddlInwardType.SelectedValue != "0")
                {
                    grdEnquiry.Rows[i].Visible = false;
                }
                //if (ddlME.SelectedValue != lblMEId.Text && ddlME.SelectedValue != "0")
                if (ddlME.SelectedValue != dt.Rows[i]["ROUTE_ME_Id"].ToString() && ddlME.SelectedValue != "0")
                {
                    grdEnquiry.Rows[i].Visible = false;
                }
                //if (ddlEnteredFrom.SelectedValue != lblEnteredFrom.Text && ddlEnteredFrom.SelectedValue != "0")
                if (ddlEnteredFrom.SelectedValue != dt.Rows[i]["EnteredFrom"].ToString() && ddlEnteredFrom.SelectedValue != "0")
                {
                    grdEnquiry.Rows[i].Visible = false;
                }
                if (grdEnquiry.Rows[i].Visible == true)
                    rowCount++;
            }
            lblTotalRecords.Text = "Total Records : " + rowCount;
        }
        protected void ddlInwardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterEnquiryList();
        }
        protected void ddlEnteredFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterEnquiryList();
        }
        protected void ddlME_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterEnquiryList();
        }
                
        private void LoadReportList()
        {
            int MEId = Convert.ToInt32(Session["LoginId"]);
            if (lblSuperAdmin.Text == "SuperAdmin")
            {
                MEId = 0;
            }
            string strMEName = "", strInwardType = "";
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Limit_mny", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_BalanceAmt_mny", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("MISRecordNo", typeof(string)));
            dt.Columns.Add(new DataColumn("MISRefNo", typeof(string)));
            dt.Columns.Add(new DataColumn("MISCollectionDt", typeof(string)));
            dt.Columns.Add(new DataColumn("MISRecievedDt", typeof(string)));
            dt.Columns.Add(new DataColumn("MEName", typeof(string)));
            dt.Columns.Add(new DataColumn("ROUTE_ME_Id", typeof(string)));

            var report = dc.ReportPending_View_CRLimitExceed(MEId).ToList();
            foreach (var c in report)
            {
                dr = dt.NewRow();
                dr["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                dr["SITE_Name_var"] = Convert.ToString(c.SITE_Name_var);
                dr["CL_Limit_mny"] = c.CL_Limit_mny.ToString();
                dr["CL_BalanceAmt_mny"] = c.CL_BalanceAmt_mny.ToString();
                dr["MATERIAL_Name_var"] = c.MATERIAL_Name_var.ToString();
                dr["MATERIAL_Id"] = c.MATERIAL_Id.ToString();
                dr["MISRecordNo"] = c.MISRecordNo.ToString();
                dr["MISRefNo"] = c.MISRefNo.ToString();
                dr["MISCollectionDt"] = Convert.ToDateTime(c.MISCollectionDt).ToString("dd/MM/yyyy");
                dr["MISRecievedDt"] = Convert.ToDateTime(c.MISRecievedDt).ToString("dd/MM/yyyy");
                dr["MEName"] = c.MEName;
                dr["ROUTE_ME_Id"] = c.ROUTE_ME_Id.ToString();
                dt.Rows.Add(dr);

                if (strMEName.Contains("," + c.ROUTE_ME_Id + ",") == false)
                {
                    ddlMERpt.Items.Add(new ListItem(c.MEName, c.ROUTE_ME_Id.ToString()));
                    strMEName += "," + c.ROUTE_ME_Id.ToString() + ",";
                }
                if (strInwardType.Contains("," + c.MATERIAL_Id + ",") == false)
                {
                    ddlInwardTypeRpt.Items.Add(new ListItem(c.MATERIAL_Name_var, c.MATERIAL_Id.ToString()));
                    strInwardType += "," + c.MATERIAL_Id.ToString() + ",";
                }
            }
            grdReports.DataSource = dt;
            grdReports.DataBind();
            ViewState["ReportTable"] = dt;
            SortListControl(ddlInwardTypeRpt, true);
            SortListControl(ddlMERpt, true);
            ddlMERpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddlInwardTypeRpt.Items.Insert(0, new ListItem("---Select---", "0"));

            lblTotalRecordsRpt.Text = "Total Records : " + grdReports.Rows.Count;

        }

        private void FilterReportList()
        {
            DataTable dt = ViewState["ReportTable"] as DataTable;
            int rowCount = 0;
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                grdReports.Rows[i].Visible = true;
                //Label lblMEId = (Label)grdReports.Rows[i].FindControl("lblMEId");
                //Label lblMaterialId = (Label)grdReports.Rows[i].FindControl("lblMaterialId"); 
                //if (ddlInwardTypeRpt.SelectedValue != lblMaterialId.Text && ddlInwardTypeRpt.SelectedValue != "0")
                if (ddlInwardTypeRpt.SelectedValue != dt.Rows[i]["MATERIAL_Id"].ToString() && ddlInwardTypeRpt.SelectedValue != "0")
                {
                    grdReports.Rows[i].Visible = false;
                }
                //if (ddlMERpt.SelectedValue != lblMEId.Text && ddlMERpt.SelectedValue != "0")
                if (ddlMERpt.SelectedValue != dt.Rows[i]["ROUTE_ME_Id"].ToString() && ddlMERpt.SelectedValue != "0")
                {
                    grdReports.Rows[i].Visible = false;
                }
                if (grdReports.Rows[i].Visible == true)
                    rowCount++;
            }
            lblTotalRecordsRpt.Text = "Total Records : " + rowCount;
        }
        protected void ddlInwardTypeRpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterReportList();
        }
        protected void ddlMERpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterReportList();
        }
        public static void SortListControl(ListControl control, bool isAscending)
        {
            List<ListItem> collection;

            if (isAscending)
                collection = control.Items.Cast<ListItem>()
                    .Select(x => x)
                    .OrderBy(x => x.Text)
                    .ToList();
            else
                collection = control.Items.Cast<ListItem>()
                    .Select(x => x)
                    .OrderByDescending(x => x.Text)
                    .ToList();

            control.Items.Clear();

            foreach (ListItem item in collection)
                control.Items.Add(item);
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            ddlInwardTypeRpt.Items.Clear();
            ddlMERpt.Items.Clear();
            ddlInwardType.Items.Clear();
            ddlME.Items.Clear();
            ddlEnteredFrom.Items.Clear();

            LoadEnquiryList();
            LoadReportList();
        }

        protected void lnkPrintEnq_Click(object sender, EventArgs e)
        {
            if (grdEnquiry.Rows.Count > 0)
                PrintGrid.PrintGridView(grdEnquiry, "List of pending enquiries", "PendingEnquiry");
        }

        protected void lnkPrintRpt_Click(object sender, EventArgs e)
        {
            if (grdReports.Rows.Count > 0)
                PrintGrid.PrintGridView(grdReports, "List of report ready but pending because of credit limit", "PendingReport");
        }

    }
}