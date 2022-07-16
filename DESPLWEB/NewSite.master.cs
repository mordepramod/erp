using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class NewSite : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //DateDisplay.Text = DateTime.Now.ToString("dddd, MMMM dd");
       
    }
    protected void QuickLogin_Authenticate(object sender, AuthenticateEventArgs e)
    {

    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {

    }
    protected void lnkExit_Click(object sender, EventArgs e)
    {
        //Session["Location"] = "0";
        Response.Redirect("WebHome.aspx");
    }
    protected void lnkLogOut_Click(object sender, EventArgs e)
    {
        Response.Redirect("default.aspx");
    }
}
