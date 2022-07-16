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
    public partial class ClientEnquiry : System.Web.UI.Page
    {
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlMeta keywords = new HtmlMeta();
            keywords.Name = "keywords";
            keywords.Content = "Report System";

            if (!IsPostBack)
            {
                lblLoctn.Text= Session["Location"].ToString();
                cmbTestType.Items.Add("---Select---");
                cmbTestType.Items.Add("Compressive Strength Of Concrete Cubes");
                cmbTestType.Items.Add("Compressive Strength Of Pavement Blocks");
                cmbTestType.Items.Add("Concrete Mix Design - Testing");
                cmbTestType.Items.Add("Steel Testing");
                cmbTestType.Items.Add("Aggregate Testing");
                cmbTestType.Items.Add("Cement Testing");
                cmbTestType.Items.Add("Solid Masonary Block Testing");
                cmbTestType.Items.Add("Compressive Strength Of Mortar Cube");
                cmbTestType.Items.Add("Fly Ash Testing");
                cmbTestType.Items.Add("Tile Testing");
                cmbTestType.Items.Add("Brick Testing");
                cmbTestType.Items.Add("Core Cutting With Testing");
                cmbTestType.Items.Add("Core Cutting");
                cmbTestType.Items.Add("Non Destructive Testing");
                cmbTestType.Items.Add("Soil Testing");
                cmbTestType.Items.Add("Pile Testing");
                cmbTestType.Items.Add("Soil Investigation");
                cmbTestType.Items.Add("Other");
                cmbTestType.Items.Add("Steel Chemical Testing");
                cmbTestType.Items.Add("Cement Chemical Testing");
                cmbTestType.Items.Add("Water Testing");

            }
            if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "event 1")
            {
                if (lstClients.SelectedItem != null)
                    txtClient.Text = lstClients.SelectedItem.Text;
            }
            lstClients.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(lstClients, "event 1"));

            if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "event 2")
            {
                if (lstSites.SelectedItem != null)
                    txtSite.Text = lstSites.SelectedItem.Text;
            }
            lstSites.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(lstSites, "event 2"));


        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                clsSendMail obj = new clsSendMail();
                string strBody = "";
                strBody = "<br />";
                strBody += "Company Name : " + txtClient.Text + "<br />";
                strBody += "Site Name : " + txtSite.Text + "<br />";
                strBody += "Site Address : " + txtAddress.Text + "<br />";
                strBody += "Contact Person : " + txtContactPerson.Text + "<br />";
                strBody += "Contact Number : " + txtContactNo.Text + "<br />";
                strBody += "Email Id : " + txtEmailId.Text + "<br />";
                if (cmbTestType.Text != "---Select---")
                    strBody += "Material to be tested : " + cmbTestType.Text + "<br />";
                if (txtDesc.Text != "")
                    strBody += "Any other requirement : " + txtDesc.Text + "<br />";
                obj.SendMail("info@durocrete.acts-int.com", "marketing@durocrete.acts-int.com", "New enquiry added...", strBody, "", "");
                    //("info@durocrete.acts-int.com", "settingmkt", "marketing@durocrete.acts-int.com", "info@durocrete.acts-int.com", "New enquiry added...", strBody, "");
                //SendMail("info@durocrete.acts-int.com", "settingmkt", "shital.bandal@gmail.com", "", "New enquiry added...", strBody, "");
                lblMessage.Text = "Enquiry added successfully.";
                lblMessage.Visible = true;

               
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtClient.Text = "";
            txtSite.Text = "";
            txtContactPerson.Text = "";
            txtAddress.Text = "";
            txtContactNo.Text = "";
            txtDesc.Text = "";
            txtEmailId.Text = "";
            cmbTestType.Text = "---Select---";
            lblMessage.Visible = false;
             string strURLWithData = "ClientHome.aspx?" + obj.Encrypt(string.Format("Location={0}", lblLoctn.Text));
            Response.Redirect(strURLWithData);
           // Response.Redirect("ClientHome.aspx");
        }
        protected void btnCancelClient_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        protected void btnCancelSite_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Hide();
        }
        protected void btnOkClient_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItem != null)
                txtClient.Text = lstClients.SelectedItem.Text;
        }
        protected void btnOkSite_Click(object sender, EventArgs e)
        {
            if (lstSites.SelectedItem != null)
                txtSite.Text = lstSites.SelectedItem.Text;
        }

       protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            string strURLWithData = "ClientHome.aspx?" + obj.Encrypt(string.Format("Location={0}", lblLoctn.Text));
            Response.Redirect(strURLWithData);
        }
    }
}