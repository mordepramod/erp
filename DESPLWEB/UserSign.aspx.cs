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
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace DESPLWEB
{
    public partial class UserSign : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "User Sign";
                LoadAppvrightUser();
            }
        }
     

        private void LoadAppvrightUser()
        {
            ddlUser.DataTextField = "USER_Name_var";
            ddlUser.DataValueField = "USER_Id";
            //var apprv = dc.ReportStatus_View("", null, null, 0, 0, 1, "", 0, 0, 0);
            var apprv = dc.User_View(0,0,"","","");
            ddlUser.DataSource = apprv;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, "---Select---");
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            if (FileUploadSign.HasFile)
            {
                int length = FileUploadSign.PostedFile.ContentLength;
                byte[] imgbyte = new byte[length];
                HttpPostedFile img = FileUploadSign.PostedFile;
                img.InputStream.Read(imgbyte, 0, length);
                if (ddlUser.SelectedItem.Text != "---Select---")
                {
                    dc.Sign_Update(Convert.ToInt32(ddlUser.SelectedValue), imgbyte, true);
                    dc.Sign_Update(Convert.ToInt32(ddlUser.SelectedValue), imgbyte, false);
                    //dc.Sign_Update(12, imgbyte, true);
                    //dc.Sign_Update(12, imgbyte, false);
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Text = "Sign uploaded Successfully";
                }
            }
        }

      
    }
      
}