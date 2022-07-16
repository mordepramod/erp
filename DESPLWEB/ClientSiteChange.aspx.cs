using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ClientSiteChange : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Change Report Specific Client/ Site";
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
                        if (u.USER_SuperAdmin_right_bit == true || u.USER_Name_var.ToString().Contains("Irsha"))
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
                    Session["CL_ID"] = 0;
                    Session["SITE_ID"] = 0;
                    
                    LoadInwarDType();
                    txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    optRefNo.Checked = true;
                }
            }
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        
        private void LoadInwarDType()
        {
            var inwd = dc.Material_View("", "");
            ddlInwardType.DataTextField = "MATERIAL_Name_var";
            ddlInwardType.DataValueField = "MATERIAL_RecordType_var";
            ddlInwardType.DataSource = inwd;
            ddlInwardType.DataBind();
            ddlInwardType.Items.Insert(0, "Bill");
            ddlInwardType.Items.Insert(0, "---Select---");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DisplayReportList();
        }

        protected void txtNewClient_TextChanged(object sender, EventArgs e)
        {
            txtNewSite.Text = "";
            ddlNewContactPerson.Items.Clear();
            txtNewContactNo.Text = "";
            txtNewEmailId.Text = "";
            if (ChkClientName(txtNewClient.Text) == true)
            {
                if (txtNewClient.Text != "")
                {
                    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                    //lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                else
                {
                    Session["CL_ID"] = 0;
                    //lblClientId.Text = "0";
                }

                var client = dc.Client_View(0, 0, txtNewClient.Text, "");
                foreach (var cl in client)
                {
                    lblClientId.Text = cl.CL_Id.ToString();
                }

                txtNewSite.Focus();
            }
        }

        protected void txtNewSite_TextChanged(object sender, EventArgs e)
        {
            ddlNewContactPerson.Items.Clear();
            txtNewContactNo.Text = "";
            txtNewEmailId.Text = "";

            int cl_Id = 0;
            if (Convert.ToInt32(Session["CL_ID"]) > 0)
            {
                if (int.TryParse(Session["CL_ID"].ToString(), out cl_Id))
                {
                    if (ChkSiteName(txtNewSite.Text) == true)
                    {
                        int SiteId = 0;
                        if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                        {
                            Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                        }
                        else
                        {
                            Session["SITE_ID"] = 0;
                        }

                        var site = dc.Site_View(0, Convert.ToInt32(lblClientId.Text), 0, txtNewSite.Text);
                        foreach (var st in site)
                        {
                            lblSiteId.Text = st.SITE_Id.ToString();
                        }
                        LoadNewContactPersonList(Convert.ToInt32(lblSiteId.Text),Convert.ToInt32(lblClientId.Text));
                       
                        //  txtNewContactPerson.Focus();
                    }
                }
            }
        }

       
        public void DisplayReportList()
        {
            //if (ddlInwardType.SelectedIndex > 0 && ddlInwardType.SelectedValue != "CT")
            if (ddlInwardType.SelectedIndex > 0)
            {
                lblBillNo.Visible = false;
                txtPrvBillNo.Visible = false;
                lnkUpdatePrvBillNo.Visible = false;
                DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
                DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                if (ddlInwardType.SelectedValue == "Bill")
                {
                    var bills = dc.Bill_View("", 0, 0, "", 0, false, false, Fromdate, Todate);
                    ddlRefRecNo.DataTextField = "BILL_Id";
                    ddlRefRecNo.DataSource = bills;
                    ddlRefRecNo.DataBind(); 
                }
                else
                {
                    if (ddlInwardType.SelectedValue == "NDT" && optRefNo.Checked == true)
                    {
                        lblBillNo.Visible = true;
                        txtPrvBillNo.Visible = true;
                        lnkUpdatePrvBillNo.Visible = true;
                    }
                    var Inward = dc.Inward_View(0, 0, ddlInwardType.SelectedValue, Fromdate, Todate);
                    if (optRefNo.Checked == true)
                        ddlRefRecNo.DataTextField = "INWD_ReferenceNo_int";
                    else
                        ddlRefRecNo.DataTextField = "INWD_RecordNo_int";
                    ddlRefRecNo.DataSource = Inward;
                    ddlRefRecNo.DataBind();

                }
                ddlRefRecNo.Items.Insert(0, new ListItem("---Select---", ""));
            }
        }

        protected void ClearAllControls()
        {
            txtClientName.Text = "";
            txtSiteName.Text = "";
            txtContactPerson.Text = "";
            txtContactNo.Text = "";
            txtEmailId.Text = "";

            txtNewClient.Text = "";
            txtNewSite.Text = "";
            ddlNewContactPerson.Items.Clear();
            txtNewContactNo.Text = "";
            txtNewEmailId.Text = "";

            Session["CL_ID"] = 0;
            Session["SITE_ID"] = 0;

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
        }

        protected void ddlInwardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlRefRecNo.Items.Clear();
            ClearAllControls();
            DisplayReportList();
        }

        protected void ddlRefRecNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            if (ddlRefRecNo.SelectedIndex > 0)
            {
                if (ddlInwardType.SelectedValue == "Bill")
                {
                    var bill = dc.Bill_View(ddlRefRecNo.SelectedItem.Text, 0, 0, "", 0, false, false, null, null);
                    foreach (var bl in bill)
                    {
                        txtContactPerson.Text = "";
                        txtEmailId.Text = "";
                        txtContactNo.Text = "";
                        txtClientName.Text = bl.CL_Name_var;
                        txtSiteName.Text = bl.SITE_Name_var;
                    }
                }
                else
                { 
                    int recordNo = 0, referenceNo = 0;
                    if (optRecNo.Checked == true)
                        recordNo = Convert.ToInt32(ddlRefRecNo.SelectedItem.Text);
                    else
                        referenceNo = Convert.ToInt32(ddlRefRecNo.SelectedItem.Text);

                    var Inward = dc.Inward_View(referenceNo, recordNo, ddlInwardType.SelectedValue, null, null);
                    foreach (var inwd in Inward)
                    {
                        txtContactPerson.Text = inwd.ContactName;
                        txtEmailId.Text = inwd.INWD_EmailId_var.ToString();
                        txtContactNo.Text = inwd.INWD_ContactNo_var.ToString();
                        txtClientName.Text = inwd.ClientName;
                        txtSiteName.Text = inwd.SiteName;
                    }
                }
                
            }
        }

        protected void optRefNo_CheckedChanged(object sender, EventArgs e)
        {
            ddlRefRecNo.Items.Clear();
            ClearAllControls();
            DisplayReportList();
        }

        protected void optRecNo_CheckedChanged(object sender, EventArgs e)
        {
            ddlRefRecNo.Items.Clear();
            ClearAllControls();
            DisplayReportList(); 
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;

            if (ddlInwardType.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Inward Type";
                valid = false;
            }
            else if (ddlRefRecNo.Items.Count ==0 || ddlRefRecNo.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Reference No./ Record No./ Bill No.";
                valid = false;
            }
            else if (txtNewClient.Text == "" || Session["CL_ID"].ToString() == "0")
            {
                lblMsg.Text = "Please Select the New Client";
                valid = false;
            }
            else if (txtNewSite.Text == "" || Session["SITE_ID"].ToString() == "0")
            {
                lblMsg.Text = "Please Select the New Site";
                valid = false;
            }
            else if (ddlNewContactPerson.SelectedItem.Text == "---Select---" && ddlInwardType.SelectedIndex != 1)
            {
                lblMsg.Text = "Please Select the New Contact Person";
                valid = false;
            }
            else if (txtNewContactNo.Text == "" && ddlInwardType.SelectedIndex != 1)
            {
                lblMsg.Text = "Please Select the New Contact No";
                valid = false;
            }
            else if (txtNewEmailId.Text == "" && ddlInwardType.SelectedIndex != 1)
            {
                lblMsg.Text = "Please Select the New EmailId";
                valid = false;
            }
            else
            {
                int clientId = 0, siteId = 0, contactPerId = 0;
                if (txtNewClient.Text != "")
                {
                    string ClientId = Request.Form[hfClientId.UniqueID];
                    if (ClientId != "")
                    {
                        Session["CL_ID"] = Convert.ToInt32(ClientId);
                    }
                    var client = dc.Client_View(0, 0, txtNewClient.Text, "");
                    foreach (var cl in client)
                    {
                        clientId = cl.CL_Id;
                    }
                }
                if (txtNewSite.Text != "")
                {
                    string SiteId = Request.Form[hfSiteId.UniqueID];
                    if (SiteId != "")
                    {
                        Session["SITE_ID"] = Convert.ToInt32(SiteId);
                    }
                    var site = dc.Site_View(0, clientId, 0, txtNewSite.Text);
                    foreach (var st in site)
                    {
                        siteId = st.SITE_Id;
                    }
                }
                if (ddlNewContactPerson.SelectedIndex > 0)
                    contactPerId = Convert.ToInt32(ddlNewContactPerson.SelectedValue);

                if (ddlInwardType.SelectedIndex == 1)
                {
                    //dc.Bill_Update_ClientSite(ddlRefRecNo.SelectedItem.Text, Convert.ToInt32(Session["CL_ID"]), txtNewClient.Text, Convert.ToInt32(Session["SITE_ID"]));
                    dc.Bill_Update_ClientSite(ddlRefRecNo.SelectedItem.Text, clientId, txtNewClient.Text, siteId);
                    lblMsg.Text = "Updated Successfully.";
                }
                else
                {
                    int recordNo = 0, referenceNo = 0, enqId = 0;
                    string billId = "0";
                    if (optRecNo.Checked == true)
                        recordNo = Convert.ToInt32(ddlRefRecNo.SelectedItem.Text);
                    else
                        referenceNo = Convert.ToInt32(ddlRefRecNo.SelectedItem.Text);
                    var Inward = dc.Inward_View(referenceNo, recordNo, ddlInwardType.SelectedValue, null, null);
                    foreach (var inwd in Inward)
                    {
                        enqId = Convert.ToInt32(inwd.INWD_ENQ_Id);
                        billId = inwd.INWD_BILL_Id;
                    }
                    //if (billId.Trim() != "0")
                    //{
                    //    var cashd = dc.CashDetail_View_bill(Convert.ToString(billId));
                    //    if (cashd.Count() > 0)
                    //    {
                    //        lblMsg.Text = "Receipt entry done for selected inward bill.";
                    //        valid = false;
                    //    }
                    //}
                    //if (valid == true)
                    //{                    

                    //dc.Inward_Update_Client(referenceNo, recordNo, ddlInwardType.SelectedValue, Convert.ToInt32(Session["CL_ID"]), Convert.ToInt32(Session["SITE_ID"]), Convert.ToInt32(ddlNewContactPerson.SelectedValue), enqId, billId, txtNewEmailId.Text, txtNewContactNo.Text);
                    dc.Inward_Update_Client(referenceNo, recordNo, ddlInwardType.SelectedValue, clientId, siteId, contactPerId, enqId, billId, txtNewEmailId.Text, txtNewContactNo.Text);
                    lblMsg.Text = "Updated Successfully.";                    
                }
            }
            lblMsg.Visible = true;
            
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
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSitename(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            DataRow row = null;
            DataTable dt = new DataTable();
            if (Convert.ToInt32(HttpContext.Current.Session["CL_ID"]) > 0)
            {
                int ClientId = 0;
                if (int.TryParse(HttpContext.Current.Session["CL_ID"].ToString(), out ClientId))
                {

                    var res = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, searchHead);
                    dt.Columns.Add("SITE_Name_var");
                    foreach (var obj in res)
                    {
                        row = dt.NewRow();
                        string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                        dt.Rows.Add(listitem);
                    }
                    if (row == null)
                    {
                        var resclnm = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, "");
                        foreach (var obj in resclnm)
                        {
                            row = dt.NewRow();
                            string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                            dt.Rows.Add(listitem);
                        }
                    }
                }
            }
            List<string> SITE_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SITE_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return SITE_Name_var;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetContactname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            DataRow row = null;
            DataTable dt = new DataTable();
            if (Convert.ToInt32(HttpContext.Current.Session["SITE_ID"]) > 0 && Convert.ToInt32(HttpContext.Current.Session["CL_ID"]) > 0)
            {
                var contactperson = db.Contact_View(0, Convert.ToInt32(HttpContext.Current.Session["SITE_ID"]), Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), searchHead);
                dt.Columns.Add("CONT_Name_var");
                foreach (var obj in contactperson)
                {
                    row = dt.NewRow();
                    string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.CONT_Name_var, obj.CONT_Id.ToString());
                    dt.Rows.Add(listitem);
                }
                if (row == null)
                {

                    var contPerson = db.Contact_View(0, Convert.ToInt32(HttpContext.Current.Session["SITE_ID"]), Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), "");
                    foreach (var obj in contPerson)
                    {
                        row = dt.NewRow();
                        string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.CONT_Name_var, obj.CONT_Id.ToString());
                        dt.Rows.Add(listitem);
                    }
                }
            }
            List<string> CONT_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CONT_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CONT_Name_var;
        }

        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txtNewClient.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txtNewClient.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        protected Boolean ChkSiteName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txtNewSite.Text;
            Boolean valid = false;
            var res = dc.Site_View(0, Convert.ToInt32(Session["CL_ID"]), 0, searchHead);
            foreach (var obj in res)
            {
                valid = true;
            }
            if (valid == false)
            {
                txtNewSite.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Site Name is not in the list ";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }
            

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            DisplayReportList();
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            DisplayReportList();
        }

        protected void lnkUpdatePrvBillNo_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            if (optRefNo.Checked)
            {
                if (ddlRefRecNo.SelectedIndex == 0)
                {
                    valid = false;
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please Select Reference No.";
                }
                else if (txtPrvBillNo.Text == "")
                {
                    valid = false;
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please Enter Prv. Bill No.";
                }
            }
            else
            {
                valid = false;
                lblMsg.Visible = true;
                lblMsg.Text = "Please Select Reference No.";
            }

            if (valid)
            {
                //chk bill no is valid or not
                var dtls = dc.Inward_View(Convert.ToInt32(ddlRefRecNo.SelectedItem.Text),0,"NDT",null,null).ToList();
                int recordNo = Convert.ToInt32(dtls.FirstOrDefault().INWD_RecordNo_int);

                dc.Inward_Update_BillNo(recordNo, "NDT", txtPrvBillNo.Text);
                lblMsg.Text = "Updated Successfully.";
            }
        }

        protected void ddlNewContactPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNewContactPerson.SelectedIndex > 0)
            {
                var contactp = dc.Contact_View(Convert.ToInt32(ddlNewContactPerson.SelectedValue), 0, 0, "");
                foreach (var cont in contactp)
                {
                    txtNewContactNo.Text = cont.CONT_ContactNo_var;
                    txtNewEmailId.Text = cont.CONT_EmailID_var;
                }
            }
            else
            {
                txtNewContactNo.Text = "";
                txtNewEmailId.Text = "";
            }
        }
        
        private void LoadNewContactPersonList(int SiteId, int ClId)
        {
            var cl = dc.Contact_View(0, SiteId, ClId, "");
            ddlNewContactPerson.DataTextField = "CONT_Name_var";
            ddlNewContactPerson.DataValueField = "CONT_Id";
            ddlNewContactPerson.DataSource = cl;
            ddlNewContactPerson.DataBind();
            ddlNewContactPerson.Items.Insert(0, "---Select---");
        }
    }

}