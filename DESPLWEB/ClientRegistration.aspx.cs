using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace DESPLWEB
{
    public partial class ClientRegistration : System.Web.UI.Page
    {
        //static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlMeta keywords = new HtmlMeta();
            keywords.Name = "keywords";
            keywords.Content = "Report System";
            LinkButton lnkExit = (LinkButton)Master.FindControl("lnkExit");
            lnkExit.Visible = false;

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                //lblMessage.Text = "Client added successfully.";
                //lblMessage.Visible = true;

                // *temporory commented
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                
                if (ddlLocation.SelectedValue.ToString() == "Pune")
                    cnStr = ConfigurationManager.AppSettings["conStrPune"].ToString();
                else if(ddlLocation.SelectedValue.ToString() == "Mumbai")
                    cnStr = ConfigurationManager.AppSettings["conStrMumbai"].ToString();
                else if (ddlLocation.SelectedValue.ToString() == "Nashik")
                    cnStr = ConfigurationManager.AppSettings["conStrNashik"].ToString();
                else if (ddlLocation.SelectedValue.ToString() == "Metro")
                    cnStr = ConfigurationManager.AppSettings["conStrMetro"].ToString();

                SqlConnection con = new SqlConnection(cnStr);
                con.Open();
                SqlCommand cmd = new SqlCommand("ClientRegistration_Web", con);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CL_Name_var", txtClient.Text));
                    cmd.Parameters.Add(new SqlParameter("@CL_OfficeAddress_var", txtClientAddress.Text));
                    cmd.Parameters.Add(new SqlParameter("@CL_EmailID_var", txtEmailId.Text));
                    cmd.Parameters.Add(new SqlParameter("@CL_mobile_No", txtClientMobNo.Text));
                    cmd.Parameters.Add(new SqlParameter("@Site_Name_var", txtSite.Text));
                    cmd.Parameters.Add(new SqlParameter("@Site_Address_var", txtSiteAddress.Text));
                   cmd.Parameters.Add(new SqlParameter("@Site_mobile_No", txtSiteMobNo.Text));
                   
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr != null)
                    {
                        lblMessage.Text = "Registration done Successfully. You will get password within 2 working days.";
                        ClearData();
                        lblMessage.Visible = true;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
             lblMessage.Visible = false;
            // Session["clientId"] = "0";
            Response.Redirect("default.aspx"); 
        }
      
      public void ClearData()
        {
            ddlLocation.SelectedIndex = 0;
            txtClient.Text = "";
            txtSite.Text = "";
            txtSiteMobNo.Text = "";
            txtSiteAddress.Text = "";
            txtClientMobNo.Text = "";
            txtClientAddress.Text = "";
            txtEmailId.Text = "";
        }
       
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }
}