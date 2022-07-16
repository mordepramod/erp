using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class ReportMailStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Report Status";
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
                
        public void DisplayReportStatus()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            var Inward = dc.ReportMailStatus_View(ddl_InwardTestType.SelectedItem.Text, Fromdate, Todate).ToList();
            grdReportStatus.DataSource = Inward;
            grdReportStatus.DataBind();
            lbl_RecordsNo.Text = "Total Records   :  " + grdReportStatus.Rows.Count;
        }

        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdReportStatus.Visible = false;
            lbl_RecordsNo.Text = "";
        }
        
    }

}
