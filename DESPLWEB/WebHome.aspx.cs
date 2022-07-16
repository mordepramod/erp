using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
//using Ionic.Zip;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace DESPLWEB
{
    public partial class WebHome : System.Web.UI.Page
    {
        //Int32 cnt = 0;
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["clientId"]) == "0")
            {
                Response.Redirect("default.aspx");
            }        
            if (!IsPostBack)
            {
                lblLocation.Text = Session["Location"].ToString();
                lblClientId.Text = Session["clientId"].ToString();
            }
        }

        protected void btnUpdateMobile_Click(object sender, EventArgs e)
        {
                        
            string strURLWithData = "WebMobileUsers.aspx?" + obj.Encrypt(string.Format("Location={0}", lblLocation.Text));
            Response.Redirect(strURLWithData);
                    }

        protected void btnViewReports_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session["clientId"]) == "16129")
            //{
            //    lblLocation.Text = "Pune";
            //    string strURLWithData = "ClientHomeMetro.aspx?" + obj.Encrypt(string.Format("Location={0}", lblLocation.Text));
            //    Response.Redirect(strURLWithData);
            //}
            //else
            //{

                string strURLWithData = "ClientHome.aspx?" + obj.Encrypt(string.Format("Location={0}", lblLocation.Text));
                Response.Redirect(strURLWithData);
            //}
        }

        protected void btnBillApproval_Click(object sender, EventArgs e)
        {
            if (Session["BillApproval"] != null && Session["BillApproval"].ToString() == "True")
            {
                string strURLWithData = "ClientBillApproval.aspx?" + obj.Encrypt(string.Format("Location={0}", lblLocation.Text));
                Response.Redirect(strURLWithData);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Access is Denied..');", true);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            myDataComm cd = new myDataComm();
            string conn = cd.getConnectionStringForWeb(lblLocation.Text); 
            LabDataDataContext dc = new LabDataDataContext(conn);
            dc.ClientWebLoginHistory_Update(Convert.ToInt32(lblClientId.Text), DateTime.Today.Date, null, TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss")), lblLocation.Text.ToString());
            Response.Redirect("default.aspx");
        }
    }

}