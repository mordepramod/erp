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


namespace DESPLWEB.Controls
{
    public partial class UC_InwardFooter : System.Web.UI.UserControl
    {
        public delegate void ClickEventHandler(Object sender, EventArgs e);
        public event ClickEventHandler Click = delegate { };
        protected void Page_Load(object sender, EventArgs e)
        {
            LabDataDataContext dc = new LabDataDataContext();
            var user = dc.User_View(0, 0, "","","");
            cmbReceivedBy.DataTextField = "USER_Name_var";
            cmbReceivedBy.DataValueField = "USER_Id";
            cmbReceivedBy.DataSource = user;
            cmbReceivedBy.DataBind();
            cmbReceivedBy.Items.Insert(0, "---Select---");
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            Click(this, e);
        }
        public string ReceivedId
        {
            get { return cmbReceivedBy.SelectedItem.Value; }
            set { cmbReceivedBy.SelectedValue = value; }
        }
        
    }
}