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

namespace DESPLWEB
{
  
    public partial class MstPg_Veena : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoginId"] == null)
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                Response.Redirect("Login.aspx");
            }
            Page.Header.DataBind();
            if (Session["LoginUserName"] != null)
            {
                lblUsername.Text = Convert.ToString(Session["LoginUserName"]);
            }
            lblCurrentDate.Text = System.DateTime.Now.ToLongDateString();            
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
      
    }
}
