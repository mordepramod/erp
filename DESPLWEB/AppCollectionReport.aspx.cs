using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class AppCollectionReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Collection Status";
                LoadDriver();
                LoadRoutename();
                getCurrentDate();

            }
        }
        public void getCurrentDate()
        {
            txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
            txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            
        }
        private void LoadDriver()
        {
            ddl_DriverName.DataTextField = "USER_Name_var";
            ddl_DriverName.DataValueField = "USER_Id";
            var Driver = dc.Driver_View(false);
            ddl_DriverName.DataSource = Driver;
            ddl_DriverName.DataBind();
            ddl_DriverName.Items.Insert(0, new ListItem("---Select All---", "0"));
        }
        private void LoadRoutename()
        {
            ddl_RouteName.DataTextField = "ROUTE_Name_var";
            ddl_RouteName.DataValueField = "ROUTE_Id";
            var LocationRoute = dc.Route_View(0, "", "False",0);
            ddl_RouteName.DataSource = LocationRoute;
            ddl_RouteName.DataBind();
            ddl_RouteName.Items.Insert(0, new ListItem("---Select All---", "0"));
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DisplayCollectionStatus();
        }

        private void DisplayCollectionStatus()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
          
            //int ClientId = 0;
            //if (chkClientSpecific.Checked == true)
            //{
            //    if (lblClientId.Text != "")
            //        ClientId = Convert.ToInt32(lblClientId.Text);
            //}
            var result = dc.PickUpAlloction_View(Fromdate, Todate, Convert.ToInt32(ddl_DriverName.SelectedValue), Convert.ToInt32(ddl_RouteName.SelectedValue),Convert.ToString(ddl_Status.SelectedValue));
            grdReportStatus.DataSource = result;
            grdReportStatus.DataBind();
            lbl_RecordsNo.Text = "Total Records : " + grdReportStatus.Rows.Count;
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReportStatus.Rows.Count > 0)
            {
                PrintGrid.PrintGridView(grdReportStatus,"Collection Report - Mobile App","AppCollectionReport");
            }
        }

    }
}