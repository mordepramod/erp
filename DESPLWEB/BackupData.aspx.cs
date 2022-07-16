using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class BackupData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheader = (Label)Master.FindControl("lblHeading");
                lblheader.Text = "Backup";
                //if (Session["LoginId"].ToString() != "2")
                if (Session["LoginUserName"].ToString().ToLower().Contains("sagvekar") != true)
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
               
            }

        }
        protected void lnkBackup_Click(object sender, EventArgs e)
        {
            if (txtBackup.Text.Trim() != "")
            {
                LabDataDataContext dc = new LabDataDataContext();
                try
                {
                    var a = dc.BackupData(txtBackup.Text);
                    lblMessage.Text = "Backup Completed.";
                    lblMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message.ToString();
                    lblMessage.Visible = true;
                }
                finally
                {

                }
            }
        }

    }
}