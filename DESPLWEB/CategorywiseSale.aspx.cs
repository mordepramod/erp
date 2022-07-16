using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class CategorywiseSale : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                    Label lblheading = (Label)Master.FindControl("lblheading");
                    lblheading.Text = "Categorywise Sale";
                    txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DisplayReports();
        }
        public void DisplayReports()
        {
            //DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            //DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            //var Inward = dc.CategorywiseSale_View(Fromdate, Todate).ToList();
            //grdSale.DataSource = Inward;
            //grdSale.DataBind();
            //lbl_RecordsNo.Text = "Total Records   :  " + grdSale.Rows.Count;
        }
     
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}