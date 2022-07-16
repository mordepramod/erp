using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Supplier : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Supplier Master";

                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (Page.IsValid)
            {
                txtSupplierName.Text = txtSupplierName.Text.Trim();
                bool SupplierStatus = false;
                //check for duplicate supplier
                var Supplier = dc.Supplier_View(txtSupplierName.Text);
                foreach (var supp in Supplier)
                {
                    SupplierStatus = true;
                    lblMessage.Text = "Duplicate Supplier Name..";
                    lblMessage.Visible = true;
                    break;
                }
                //
                if (SupplierStatus == false)
                {
                    dc.Supplier_Update(txtSupplierName.Text, false);

                    lblMessage.Text = "Added Successfully";
                    lblMessage.Visible = true;
                }
            }
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}