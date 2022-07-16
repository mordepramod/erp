using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class ClientNameUpdate : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool userRight = false;
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client Name Updation";
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_SuperAdmin_right_bit == true || u.USER_ClientApproval_right_bit == true)
                        {
                            userRight = true;
                        }
                    }
                }
                if (userRight == false)
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
                else
                {   
                    LoadClientList();
                }
            }
        }

        private void LoadClientList()
        {
            var cl = dc.Client_View(0, -1, "", "");
            ddlClient.DataSource = cl;
            ddlClient.DataTextField = "CL_Name_var";
            ddlClient.DataValueField = "CL_Id";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, new ListItem("-----Select----", "0"));
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNewClientName.Text = "";
            lblMessage.Visible = false;
            if (ddlClient.SelectedIndex > 0)
            {
                txtNewClientName.Text = ddlClient.SelectedItem.Text; 
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            
            lblMessage.Visible = false;
            txtNewClientName.Text = txtNewClientName.Text.Replace("\t", "");
            if (ddlClient.SelectedIndex <= 0)
            {   
                lblMessage.Text = "Select Client ..";
                txtNewClientName.Focus();
                lblMessage.Visible = true;
            }
            else if (txtNewClientName.Text == "")
            {
                lblMessage.Text = "Input New Client Name..";
                txtNewClientName.Focus();
                lblMessage.Visible = true;
            }
            else if (txtNewClientName.Text == ddlClient.SelectedItem.Text)
            {
                lblMessage.Text = "Client Name not changed..";
                txtNewClientName.Focus();
                lblMessage.Visible = true;
            }
            else
            {
                byte clientStatus = 0;
                //check for duplicate client
                var cl = dc.Client_View(0, 0, txtNewClientName.Text, "");
                foreach (var client in cl)
                {
                    if (client.CL_Id != Convert.ToInt32(ddlClient.SelectedValue))
                    {
                        clientStatus = 1;
                        lblMessage.Text = "Duplicate Client Name..";
                        txtNewClientName.Focus();
                        lblMessage.Visible = true;
                        break;
                    }
                }

                if (clientStatus == 0)
                {

                    dc.ClientHistory_Update(Convert.ToInt32(ddlClient.SelectedValue), ddlClient.SelectedItem.Text, txtNewClientName.Text, Convert.ToInt32(Session["LoginId"]), DateTime.Now);
                    dc.Client_Update_Name(Convert.ToInt32(ddlClient.SelectedValue), txtNewClientName.Text);

                    lblMessage.Text = "Updated Successfully";
                    lblMessage.Visible = true;
                    string clientId = ddlClient.SelectedValue;
                    LoadClientList();
                    ddlClient.SelectedValue = clientId;

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