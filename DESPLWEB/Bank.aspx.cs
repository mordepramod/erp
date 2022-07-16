using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Bank : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Bank Master";

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
                txtBankName.Text = txtBankName.Text.Trim();
                bool bankStatus=false;
                //check for duplicate bank
                var bank = dc.Bank_View(txtBankName.Text);
                foreach (var bnk in bank)
                {
                    bankStatus = true;
                    lblMessage.Text = "Duplicate Bank Name..";
                    lblMessage.Visible = true;
                    break;
                }
                //
                if (bankStatus == false)
                {
                    dc.Bank_Update(txtBankName.Text);

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