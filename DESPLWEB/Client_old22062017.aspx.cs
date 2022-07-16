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
using System.Collections.Generic;
using System.Web.Script.Services;

namespace DESPLWEB
{
    public partial class Client : System.Web.UI.Page
    {
          LabDataDataContext dc =  new LabDataDataContext();
        //public static int Cl_Id = 0; 
        //public static int Site_Id = 0;
        //public static int ContactPersonId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client - Site Updation";

                txtSearch.Attributes.Add("onkeypress", "return clickButton(event,'" + ImgBtnSearch.ClientID + "')");
                //HtmlGenericControl body = (HtmlGenericControl)this.Page.Master.FindControl("myBody");
                //body.Attributes.Add("onkeypress", "catchEsc(event)");
                Session["Cl_Id"] = 0;
                Session["Site_Id"] = 0;
                Session["ContactPersonId"] = 0;

                bool userRight = false;
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
                        if (u.USER_Admin_right_bit == true)
                        {
                            userRight = true;
                            chkCouponSetting.Enabled = true;
                        }
                        if (u.USER_ClientApproval_right_bit == true)
                        {
                            lblClientApprRight.Text = "true";
                        }
                        if (u.USER_SuperAdmin_right_bit == false)
                        {
                            chkClientStatus.Enabled = false;
                            chkSiteStatus.Enabled = false;
                        }
                        if (u.USER_Account_right_bit == false)
                        {
                            chkServiceTax.Enabled = false;
                        }
                    }
                }
                userRight = true;
                if (userRight == false)
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
                FirstGridViewRowOfClient();
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, 0, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string[] item = rowObj.CL_Name_var.Split(' ');
               // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                for (int i = 0; i < item.Length; i++)
                {
                    dt.Rows.Add(item[i]);
                }
               
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                   // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    string[] item = rowObj.CL_Name_var.Split(' ');
                    // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    for (int i = 0; i < item.Length; i++)
                    {
                        dt.Rows.Add(item[i]);
                    }
                  //  dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            
            DataTable dt1=new DataTable();
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "CL_Name_var");
            if (distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").Length != 0)
                dt1 = distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").CopyToDataTable();
            //else
            //    dt1 = distinctValues.Copy();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                CL_Name_var.Add(dt1.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }
        protected void imgInsertClient_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            lblAddClient.Text = "Add New Client";
            Session["Cl_Id"] = 0;
            chkClientStatus.Checked = true;

            grdSite.Visible = false;
            lblEditedClient.Visible = false;
            lblSite.Visible = false;
            //chkApprPendingSite.Visible = false;

            grdContact.Visible = false;
            lblEditedSite.Visible = false;
            lblContPer.Visible = false;
            txtClientName.ReadOnly = false; 
        }
        protected void imgEditClient_Click(object sender, CommandEventArgs e)
        {

            ModalPopupExtender1.Show();
            LoadClientGroupList();
            lblAddClient.Text = "Edit Client";
            if (lblClientApprRight.Text != "true")
                txtClientName.ReadOnly = true;
            else
                txtClientName.ReadOnly = false;

            Session["Cl_Id"] = Convert.ToInt32(e.CommandArgument);
            var cl = dc.Client_View(Convert.ToInt32(Session["Cl_Id"]), -1, "", "");
            foreach (var client in cl)
            {
                lblCurrentClientName.Text = client.CL_Name_var;
                txtClientName.Text = client.CL_Name_var;
                txtClientEmail.Text = client.CL_EmailID_var;
                txtDirector.Text = client.CL_DirectorName_var;
                txtDirectorEmail.Text = client.CL_DirectorEmailID_var;
                txtLedgerName.Text = client.CL_LedgerName_var;
                txtOffAddress.Text = client.CL_OfficeAddress_var;
                txtOffTelNo.Text = client.CL_OfficeTelNo_var;
                txtPanNo.Text = client.CL_PANNo_var;
                txtTanNo.Text = client.CL_TANNo_var;
                txtOffFaxNo.Text = client.CL_OfficeFaxNo_var;
                txtAuthPerson.Text = client.CL_AuthorisedPerson_var;
                txtAccContact.Text = client.CL_AccContactName_var;
                txtAccContactNo.Text = client.CL_AccContactNo_var;
                txtAccEmail.Text = client.CL_AccEmailId_var;
                txtLoginName.Text = client.CL_LoginId_var;
                txtPassword.Text = client.CL_Password_var;
                if (client.CL_Group_var != null && client.CL_Group_var != "")
                    ddlClientGroup.SelectedValue = client.CL_Group_var;

                //if (client.CL_Status_bit == false)
                if (client.CL_Status_bit == 2)
                {
                    chkClientStatus.Checked = true;
                    lnkSaveClient.Text = "Approve";
                }
                else if (client.CL_Status_bit == 0)
                {
                    chkClientStatus.Checked = true;
                    lnkSaveClient.Text = "Save";
                    txtClientName.ReadOnly = true;
                }
                else
                {
                    chkClientStatus.Checked = false;
                    lnkSaveClient.Text = "Save";
                    txtClientName.ReadOnly = true;
                }

                if (client.CL_SiteSpecificCoupon_bit == true)
                    chkCouponSetting.Checked = true;
                else
                    chkCouponSetting.Checked = false;

                break;
            }
            //LoadSiteList();
            grdSite.Visible = false;
            lblEditedClient.Visible = false;
            lblSite.Visible = false;
            //chkApprPendingSite.Visible = false;

            grdContact.Visible = false;
            lblEditedSite.Visible = false;
            lblContPer.Visible = false;

        }
        protected void imgInsertSite_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender2.Show();
            lblAddSite.Text = "Add New Site";
            lblSiteClient.Text = lblEditedClient.Text.Replace("Client : ", "");
            Session["Site_Id"] = 0;
            chkSiteStatus.Checked = true;
            chkServiceTax.Checked = false;
            //LoadContactPersonList();
            grdContact.Visible = false;
            lblEditedSite.Visible = false;
            lblContPer.Visible = false;
            txtSiteName.ReadOnly = false;
        }
        protected void imgEditSite_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender2.Show();
            lblAddSite.Text = "Edit Site";
            lblSiteClient.Text = lblEditedClient.Text.Replace("Client : ", "");
            if (lblClientApprRight.Text != "true")
                txtSiteName.ReadOnly = true;
            else
                txtSiteName.ReadOnly = false;
             
            Session["Site_Id"] = Convert.ToInt32(e.CommandArgument);
            if (Session["Site_Id"].ToString() == "0")
                ModalPopupExtender2.Hide();
            var s = dc.Site_View(Convert.ToInt32(Session["Site_Id"]),0,0,"");

            foreach (var site in s)
            {
                lblCurrentSiteName.Text = site.SITE_Name_var;
                txtSiteName.Text = site.SITE_Name_var;
                txtSiteAddress.Text = site.SITE_Address_var;
                txtSiteEmail.Text = site.SITE_EmailID_var;

                //if (site.SITE_Status_bit == false)
                if (site.SITE_Status_bit == 2)
                {
                    chkSiteStatus.Checked = true;
                    lnkSaveSite.Text = "Approve";
                }
                else if (site.SITE_Status_bit == 0)
                {
                    chkSiteStatus.Checked = true;
                    lnkSaveSite.Text = "Save";
                }
                else
                {
                    chkClientStatus.Checked = false;
                    lnkSaveSite.Text = "Save";
                }
                if (site.SITE_SerTaxStatus_bit == true)
                {
                    chkServiceTax.Checked = true;
                }
                else
                {
                    chkServiceTax.Checked = false;
                }
                break;
            }
            //LoadContactPersonList();
            grdContact.Visible = false;
            lblEditedSite.Visible = false;
            lblContPer.Visible = false;
        }
        protected void imgInsertContact_Click(object sender, CommandEventArgs e)
        {            
            ModalPopupExtender3.Show();
            lblAddContact.Text = "Add New Contact Person";
            lblContactSite.Text = lblEditedSite.Text.Replace("Site : ", "");
            Session["ContactPersonId"] = 0;
            chkContactStatus.Checked = true;
        }
        protected void imgEditContact_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender3.Show();
            lblAddContact.Text = "Edit Contact Person";
            lblContactSite.Text = lblEditedSite.Text.Replace("Site : ","")  ;
            Session["ContactPersonId"] = Convert.ToInt32(e.CommandArgument);
            var c = dc.Contact_View(Convert.ToInt32(Session["ContactPersonId"]),0,0,"");

            foreach (var contPer in c)
            {
                txtContactPer.Text = contPer.CONT_Name_var;
                txtContactNo.Text = contPer.CONT_ContactNo_var;
                txtContactEmail.Text = contPer.CONT_EmailID_var;

                if (contPer.CONT_Status_bit == false)
                    chkContactStatus.Checked = true;
                else
                    chkContactStatus.Checked = false;
                break;
            }
        } 
        private void LoadClientList()
        {
            string searchHead = "";
            if (txtSearch.Text != "" || chkApprPending.Checked == true)
                searchHead = txtSearch.Text + "%";
                //searchHead = "%" + txtSearch.Text + "%";
            int clientStatus = 0;
            if (chkApprPending.Checked == true)
                clientStatus = 2;
            var cl = dc.Client_View(0, clientStatus, searchHead, ""); 
            grdClient.DataSource = cl;
            grdClient.DataBind();
            lblClient.Visible = true;
            if (grdClient.Rows.Count <= 0)
                FirstGridViewRowOfClient();
            
            grdSite.Visible = false;
            lblEditedClient.Visible = false;
            lblSite.Visible = false;
            //chkApprPendingSite.Visible = false;

            grdContact.Visible = false;
            lblEditedSite.Visible = false;
            lblContPer.Visible = false;
            
        }
        private void LoadSiteList()
        {   
            int siteStatus = 0;
            if (chkApprPendingSite.Checked == true)
                siteStatus = 2;
            var cl = dc.Site_View(0, Convert.ToInt32(Session["Cl_Id"]), siteStatus, "");
            grdSite.DataSource = cl;
            grdSite.DataBind();
            lblSite.Visible = true;
            //chkApprPendingSite.Visible = true;
            if (grdSite.Rows.Count <= 0)
                FirstGridViewRowOfSite();
        }
        private void LoadContactPersonList()
        {
            var cl = dc.Contact_View(0, Convert.ToInt32(Session["Site_Id"]), Convert.ToInt32(Session["Cl_Id"]),"");
            grdContact.DataSource = cl;
            grdContact.DataBind();
            lblContPer.Visible = true;
            if (grdContact.Rows.Count <= 0)
                FirstGridViewRowOfContactPerson();
        }
        private void LoadClientGroupList()
        {
            var clgrp = dc.Client_View_GroupList();
            ddlClientGroup.DataSource = clgrp;
            ddlClientGroup.DataTextField = "CL_Group_var";
            ddlClientGroup.DataBind();
            ddlClientGroup.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void ClearAllControls()
        {
            chkCouponSetting.Checked = false;
            lblCurrentClientName.Text = "";
            txtAccContact.Text = "";
            txtAccContactNo.Text = "";
            txtAccEmail.Text = "";
            txtAuthPerson.Text = "";
            txtClientEmail.Text = "";
            txtClientName.Text = "";
            txtContactNo.Text = "";
            txtContactPer.Text = "";
            txtContactEmail.Text = "";
            txtDirector.Text = "";
            txtDirectorEmail.Text = "";
            txtLedgerName.Text = "";
            txtOffAddress.Text = "";
            txtOffFaxNo.Text = "";
            txtOffTelNo.Text = "";
            txtPanNo.Text = "";
            txtTanNo.Text = "";
            txtLoginName.Text = "";
            txtPassword.Text = "";
            txtSiteAddress.Text = "";
            txtSiteEmail.Text = "";
            txtSiteName.Text = "";
            lblCurrentSiteName.Text  = "";
            txtClientGroup.Text = "";
            ddlClientGroup.SelectedValue = "0";
            lnkNewGroup.Text = "New";
            txtClientGroup.Visible = false;
            ddlClientGroup.Visible = true;
            RequiredFieldValidator26.Visible = false;
            RequiredFieldValidator14.Visible = true;
            lblClientMessage.Visible = false;
            lblSiteMessage.Visible = false;
            lblContMessage.Visible = false;
            lnkSaveClient.Enabled = true;
            lnkSaveSite.Enabled = true;
            lnkSaveContact.Enabled = true;
        }
                
        protected void lnkLoadSites(object sender, CommandEventArgs e)
        {
            Session["Cl_Id"] = Convert.ToInt32(e.CommandArgument);
            LoadSiteList();
            LinkButton lb = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;
            Label lblClientName = (Label)gr.FindControl("lblClientName");
            lblEditedClient.Text = "Client : " + lblClientName.Text;
            lblEditedClient.Visible = true;
            grdSite.Visible = true;
            grdContact.Visible = false;
            lblContPer.Visible = false;
            lblEditedSite.Visible = false; 

        }
        protected void lnkLoadContactPersons(object sender, CommandEventArgs e)
        {
            Session["Site_Id"] = Convert.ToInt32(e.CommandArgument);
            LoadContactPersonList();
            LinkButton lb = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;
            Label lblSiteName = (Label)gr.FindControl("lblSiteName");
            lblEditedSite.Text = "Site : " + lblSiteName.Text;
            lblEditedSite.Visible = true;
            grdContact.Visible = true;
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            lblClientMessage.Visible = false;
            txtClientName.Text = txtClientName.Text.Replace("\t", "");
            txtLedgerName.Text = txtLedgerName.Text.Replace("\t", "");
            if (Page.IsValid)
            {
                byte clientStatus=0;

                //check for duplicate client
                var cl = dc.Client_View(0, 0, txtClientName.Text, "");
                foreach (var client in cl)
                {
                    if (Convert.ToInt32(Session["Cl_Id"]) == 0)
                    {
                        clientStatus = 1;
                        lblClientMessage.Text = "Duplicate Client Name..";
                        lblClientMessage.Visible = true;
                        break;
                    }
                    else if (Convert.ToInt32(Session["Cl_Id"]) > 0)
                    {
                        if (client.CL_Id != Convert.ToInt32(Session["Cl_Id"]))
                        {
                            clientStatus = 1;
                            lblClientMessage.Text = "Duplicate Client Name..";
                            lblClientMessage.Visible = true;
                            break;
                        }
                    }
                }
                //
                //check for duplicate client login name
                //var clogin = dc.Client_View_Login(Convert.ToInt32(Session["Cl_Id"]), txtLoginName.Text, "");
                //foreach (var clientl in clogin)
                //{   
                //    clientStatus = 1;
                //    lblClientMessage.Text = "Duplicate Client Login Name..";
                //    lblClientMessage.Visible = true;
                //    break;
                //}
                //
                if (clientStatus==0 )
                {
                    if (chkClientStatus.Checked == true)
                        clientStatus = 0;
                    else
                        clientStatus = 1;
                    string strClientGroup = "";
                    if (txtClientGroup.Visible == true)
                        strClientGroup = txtClientGroup.Text;
                    else if (ddlClientGroup.Visible == true)
                        strClientGroup = ddlClientGroup.SelectedValue;
                    int clientID= dc.Client_Update(Convert.ToInt32(Session["Cl_Id"]), txtClientName.Text, clientStatus, txtOffAddress.Text, txtOffTelNo.Text, txtOffFaxNo.Text, txtClientEmail.Text, txtDirector.Text, txtDirectorEmail.Text, txtPanNo.Text, txtTanNo.Text, txtAuthPerson.Text, txtAccContact.Text, txtAccContactNo.Text, txtAccEmail.Text, txtLedgerName.Text, "", strClientGroup,chkCouponSetting.Checked, txtLoginName.Text ,txtPassword.Text);
                    if (Convert.ToInt32(Session["Cl_Id"]) == 0)
                    {
                        string cnStr = ConfigurationManager.AppSettings["conStr"].ToString();
                        if (cnStr.ToLower().Contains("mumbai") == true)
                        {
                            dc.Client_Update_Login(clientID, "22m" + (clientID + 22).ToString(), "test" + (clientID).ToString());
                        }
                        else if (cnStr.ToLower().Contains("nashik") == true)
                        {
                            dc.Client_Update_Login(clientID, "25n" + (clientID + 25).ToString(), "test" + (clientID).ToString());
                        }
                        else
                        {
                            dc.Client_Update_Login(clientID, "20p" + (clientID + 20).ToString(), "test" + (clientID).ToString());
                        }
                    }
                    lblClientMessage.Text = "Updated Successfully";
                    lblClientMessage.Visible = true; 
                    if (txtSearch.Text != "")
                        LoadClientList();
                    //ClearAllControls();                 
                    //ModalPopupExtender1.Hide();
                    lnkSaveClient.Enabled = false;
                }
            }            
        }
        protected void lnkSaveSite_Click(object sender, EventArgs e)
        {
            txtSiteName.Text = txtSiteName.Text.Replace("\t","");
            if (Page.IsValid)
            {
                byte siteStatus=0;
                //check for duplicate site
                var s = dc.Site_View(0, Convert.ToInt32(Session["Cl_Id"]), 0, txtSiteName.Text);
                foreach (var site in s)
                {
                    if (Convert.ToInt32(Session["Site_Id"]) == 0)
                    {
                        siteStatus = 1;
                        lblSiteMessage.Text = "Duplicate Site Name..";
                        lblSiteMessage.Visible = true;
                        break;
                    }
                    else if (Convert.ToInt32(Session["Site_Id"]) > 0)
                    {
                        if (site.SITE_Id != Convert.ToInt32(Session["Site_Id"]))
                        {
                            siteStatus = 1;
                            lblSiteMessage.Text = "Duplicate Site Name..";
                            lblSiteMessage.Visible = true;
                            break;
                        }
                    }
                }
                //
                if (siteStatus == 0)
                {
                    if (chkSiteStatus.Checked == true)
                        siteStatus = 0;
                    else
                        siteStatus = 1;

                    bool serTaxStatus;
                    if (chkServiceTax.Checked == true)
                        serTaxStatus = true;
                    else
                        serTaxStatus = false;

                    dc.Site_Update(Convert.ToInt32(Session["Site_Id"]), Convert.ToInt32(Session["Cl_Id"]), txtSiteName.Text, siteStatus, txtSiteAddress.Text, txtSiteEmail.Text, Convert.ToInt32(Session["LoginId"]), Convert.ToInt32(Session["LoginId"]), "", 0, serTaxStatus, false);

                    lblSiteMessage.Text = "Updated Successfully";
                    lblSiteMessage.Visible = true;
                    LoadSiteList();
                    //ClearAllControls();
                    //ModalPopupExtender2.Hide();
                    lnkSaveSite.Enabled = false;
                }
            }
                   
        }

        protected void lnkSaveContact_Click(object sender, EventArgs e)
        {
            txtContactPer.Text = txtContactPer.Text.Replace("\t", "");
            if (Page.IsValid)
            {
                bool contactStatus = false;
                //check for duplicate contact
                var cont = dc.Contact_View(0, Convert.ToInt32(Session["Site_Id"]), Convert.ToInt32(Session["Cl_Id"]), txtContactPer.Text);
                foreach (var cn in cont)
                {
                    if (Convert.ToInt32(Session["ContactPersonId"]) == 0)
                    {
                        contactStatus = true;
                        lblContMessage.Text = "Duplicate Contact Name..";
                        lblContMessage.Visible = true;
                        break;
                    }
                    else if (Convert.ToInt32(Session["ContactPersonId"]) > 0)
                    {
                        if (cn.CONT_Id != Convert.ToInt32(Session["ContactPersonId"]))
                        {
                            contactStatus = true;
                            lblContMessage.Text = "Duplicate Contact Name..";
                            lblContMessage.Visible = true;
                            break;
                        }
                    }
                }

                if (contactStatus == false)
                {
                    if (chkContactStatus.Checked == true)
                        contactStatus = false;
                    else
                        contactStatus = true;

                    dc.Contact_Update(Convert.ToInt32(Session["ContactPersonId"]), Convert.ToInt32(Session["Cl_Id"]), Convert.ToInt32(Session["Site_Id"]), txtContactPer.Text, txtContactNo.Text,txtContactEmail.Text ,contactStatus);

                    lblContMessage.Text = "Updated Successfully";
                    lblContMessage.Visible = true;
                    LoadContactPersonList();
                    //ClearAllControls();
                    //ModalPopupExtender3.Hide();
                    lnkSaveContact.Enabled = false;

                }
            }            
        }
        private void FirstGridViewRowOfClient()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Status_bit", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_OfficeAddress_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_OfficeTelNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_EmailId_var", typeof(string)));
            dr = dt.NewRow();
            dr["CL_Id"] = 0;
            dr["CL_Name_var"] = string.Empty;
            dr["CL_Status_bit"] = string.Empty;
            dr["CL_OfficeAddress_var"] = string.Empty;
            dr["CL_OfficeTelNo_var"] = string.Empty;
            dr["CL_EmailId_var"] = string.Empty;
            dt.Rows.Add(dr);

            grdClient.DataSource = dt;
            grdClient.DataBind();
            
        }
        private void FirstGridViewRowOfSite()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Status_bit", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Address_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_EmailID_var", typeof(string)));
            dr = dt.NewRow();
            dr["SITE_Id"] = 0;
            dr["SITE_Name_var"] = string.Empty;
            dr["SITE_Status_bit"] = string.Empty;
            dr["SITE_Address_var"] = string.Empty;
            dr["SITE_EmailID_var"] = string.Empty;
            dt.Rows.Add(dr);

            grdSite.DataSource = dt;
            grdSite.DataBind();

        }
        private void FirstGridViewRowOfContactPerson()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_ContactNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_EmailID_var", typeof(string)));
            dr = dt.NewRow();
            dr["CONT_Id"] = 0;
            dr["CONT_Name_var"] = string.Empty;
            dr["CONT_ContactNo_var"] = string.Empty;
            dr["CONT_EmailID_var"] = string.Empty;  
            dt.Rows.Add(dr);

            grdContact.DataSource = dt;
            grdContact.DataBind();

        }
        protected void grdClient_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblClientId = ((Label)e.Row.FindControl("lblClientId"));
                if (lblClientId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgEditClient")).Visible = false;
                    ((LinkButton)e.Row.FindControl("btnSites")).Visible = false;
                }
                if (lblClientApprRight.Text != "true")
                {
                    ((ImageButton)e.Row.FindControl("imgEditClient")).Visible = false;
                }
            }
        }
        protected void grdSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSiteId = ((Label)e.Row.FindControl("lblSiteId"));
                if (lblSiteId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgEditSite")).Visible = false;
                    ((LinkButton)e.Row.FindControl("btnContactPersons")).Visible = false;
                }
                if (lblClientApprRight.Text != "true")
                {
                    ((ImageButton)e.Row.FindControl("imgEditSite")).Visible = false;
                }
            }
        }
        protected void grdContact_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblContactPersonId = ((Label)e.Row.FindControl("lblContactPersonId"));
                if (lblContactPersonId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgEditContact")).Visible = false;                    
                }
            }
        }

        protected void lnkNewGroup_Click(object sender, EventArgs e)
        {
            if (ddlClientGroup.Visible == true)
            {
                txtClientGroup.Visible = true;
                ddlClientGroup.Visible = false;
                lnkNewGroup.Text = "Existing";
                RequiredFieldValidator26.Visible = true;
                RequiredFieldValidator14.Visible = false;
            }
            else
            {
                txtClientGroup.Visible = false;
                ddlClientGroup.Visible = true;
                lnkNewGroup.Text = "New";
                txtClientGroup.Text = "";
                RequiredFieldValidator26.Visible = false;
                RequiredFieldValidator14.Visible = true;
            }
        }
        protected void lnkCancelClient_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender1.Hide();  
        }

        protected void lnkCancelSite_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender2.Hide();  
        }

        protected void lnkCancelContact_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender3.Hide();
            LoadContactPersonList();
        }
                
        protected void imgCloseClientPopup_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender1.Hide();
        }

        protected void imgCloseSitePopup_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender2.Hide();
        }

        protected void imgCloseContactPopup_Click(object sender, ImageClickEventArgs e)
        {
            LoadContactPersonList();
            ClearAllControls();
            ModalPopupExtender3.Hide();
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false ;

            if (txtSearch.Text != "" || chkApprPending.Checked == true)
            {
                LoadClientList();
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Enter the Client Name";
               // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please Enter the Client Name');", true);
            }
          
        }


        //protected void lnkLoadAllClient_Click(object sender, EventArgs e)
        //{
        //    txtSearch.Text = "";
        //    LoadClientList();
        //}
        
    }
}
