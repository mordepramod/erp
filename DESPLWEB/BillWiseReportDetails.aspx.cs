using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class BillWiseReportDetails : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Bill Report Details";
                LoadInwarDType();
                getCurrentDate();
            }
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        public void getCurrentDate()
        {
            txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
            txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
           
        }

        private void LoadInwarDType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select---");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdReportStatus.Visible = true;
            DisplayReportStatus();
        }

        private void DisplayReportStatus()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            var bill = dc.BillReportDetails_View(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate,  0);
            grdReportStatus.DataSource = bill;
            grdReportStatus.DataBind();
           
        }
       
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if(grdReportStatus.Rows.Count>0)
            PrintGrid.PrintGridView(grdReportStatus,"BillWise Report Status","BillWiseReport");
        }
    }
}