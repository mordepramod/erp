using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Site : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        static string prjAdd = "", pincode = "", city = "", state = "", gstNo, gstDt, gstCheked;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Site Updation";

                //txtSearch.Attributes.Add("onkeypress", "return clickButton(event,'" + ImgBtnSearch.ClientID + "')");
                //HtmlGenericControl body = (HtmlGenericControl)this.Page.Master.FindControl("myBody");
                //body.Attributes.Add("onkeypress", "catchEsc(event)");
                //Session["Cl_Id"] = 0;
                //Session["Site_Id"] = 0;
                //Session["ContactPersonId"] = 0;

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
                        if (u.USER_ClientApproval_right_bit == true)
                        {
                            lblClientApprRight.Text = "true";
                        }
                    }
                }
                userRight = true;
                if (userRight == false)
                {
                    pnlContent.Visible = false;
                }
                AddRowOfSite();
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

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (txt_Client.Text != "")
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                if (ChkClientName(txt_Client.Text) == true)
                {
                    //if (txt_Client.Text != "")
                    //{
                    //    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                    //}
                    //else
                    //{
                    //    Session["CL_ID"] = 0;
                    //}
                    LoadSiteList();
                    //grdContact.Visible = false;
                    //lblContPer.Text = "";
                    //lblEditedSite.Text = "";
                }
            }
            else
            {
                ClearAllControls();
                AddRowOfSite();
                //grdContact.Visible = false;
                //lblEditedSite.Visible = false;

            }


        }
        private void LoadContactPersonList()
        {
            int clientId = 0, siteId = 0;
            if (txt_Client.Text != "")
            {
                string ClientId = Request.Form[hfClientId.UniqueID];
                if (ClientId != "")
                {
                    //Session["CL_ID"] = Convert.ToInt32(ClientId);
                    clientId = Convert.ToInt32(ClientId);
                }
                var client = dc.Client_View(0, 0, txt_Client.Text, "");
                foreach (var cl in client)
                {
                    clientId = cl.CL_Id;
                }
            }
            if (lblAddSite.Text == "Add New Site")
            {
                siteId = 0;
            }
            else if (lblAddSite.Text == "Edit Site")
            {
                siteId = Convert.ToInt32(lblCurrentSiteId.Text.Replace("Site Id : ", ""));
            }
            //var contactp = dc.Contact_View(0, Convert.ToInt32(Session["Site_Id"]), Convert.ToInt32(Session["Cl_Id"]), "");
            var contactp = dc.Contact_View(0, siteId, clientId, "");
            ddlContactNm.DataSource = contactp;
            ddlContactNm.DataTextField = "CONT_Name_var";
            ddlContactNm.DataValueField = "CONT_Id";
            ddlContactNm.DataBind();
            ddlContactNm.Items.Insert(0, new ListItem("--Add New--", "0"));
        }
        private void LoadStateList()
        {
            var stateGrp = dc.State_View();
            ddlState.DataSource = stateGrp;
            ddlState.DataTextField = "State_Name_var";
            ddlState.DataValueField = "State_Id";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void LoadLocationList()
        {
            ddlLocation.DataTextField = "LOCATION_Name_var";
            ddlLocation.DataValueField = "LOCATION_Id";
            var Loct = dc.Location_View(0, "", 0);
            ddlLocation.DataSource = Loct;
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("-----------Select-----------", "0"));
        }
        private void LoadSiteList()
        {
            int siteStatus = -1;
            if (chkApprPendingSite.Checked == true)
                siteStatus = 2;
            int clientId = 0;
            if (txt_Client.Text != "")
            {
                string ClientId = Request.Form[hfClientId.UniqueID];
                if (ClientId != "")
                {
                    //Session["CL_ID"] = Convert.ToInt32(ClientId);
                    clientId = Convert.ToInt32(ClientId);
                }
                var client = dc.Client_View(0, 0, txt_Client.Text, "");
                foreach (var cl in client)
                {
                    clientId = cl.CL_Id;
                }
            }
            //var cl = dc.Site_View(0, Convert.ToInt32(Session["Cl_Id"]), siteStatus, "");
            var site = dc.Site_View(0, clientId, siteStatus, "");
            grdSite.DataSource = site;
            grdSite.DataBind();
            //lblSite.Visible = true;
            //chkApprPendingSite.Visible = true;
            if (grdSite.Rows.Count <= 0)
                AddRowOfSite();
        }
        private void LoadRouteList()
        {
            var route = dc.Route_View(0, "", "ALL", 0);
            ddlRoute.DataSource = route;
            ddlRoute.DataTextField = "ROUTE_Name_var";
            ddlRoute.DataValueField = "Route_Id";
            ddlRoute.DataBind();
            ddlRoute.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        //protected void lnkLoadSites(object sender, CommandEventArgs e)
        //{
        //    //Session["Cl_Id"] = Convert.ToInt32(e.CommandArgument);
        //    LoadSiteList();
        //    LinkButton lb = (LinkButton)sender;
        //    GridViewRow gr = (GridViewRow)lb.NamingContainer;
        //    Label lblClientName = (Label)gr.FindControl("lblClientName");
        //    // lblEditedClient.Text = "Client : " + lblClientName.Text;
        //    //lblEditedClient.Visible = true;
        //    grdSite.Visible = true;
        //    //grdContact.Visible = false;
        //    //lblContPer.Visible = false;
        //    //lblEditedSite.Visible = false;

        //}
        //protected void lnkLoadContactPersons(object sender, CommandEventArgs e)
        //{
        //    //Session["Site_Id"] = Convert.ToInt32(e.CommandArgument);
        //    LoadContactPersonList();
        //    LinkButton lb = (LinkButton)sender;
        //    GridViewRow gr = (GridViewRow)lb.NamingContainer;
        //    Label lblSiteName = (Label)gr.FindControl("lblSiteName");
        //    //lblEditedSite.Text = "Site : " + lblSiteName.Text;
        //    //lblEditedSite.Visible = true;
        //    //grdContact.Visible = true;
        //}
        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;

            }
            if (valid == false)
            {
                txt_Client.Focus();
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

        private void AddRowOfSite()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Address_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_EmailID_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Status_bit", typeof(bool)));
            dt.Columns.Add(new DataColumn("SITE_MonthlyBillingStatus_bit", typeof(bool)));
            dr = dt.NewRow();
            dr["SITE_Id"] = 0;
            dr["SITE_Name_var"] = string.Empty;
            dr["SITE_Address_var"] = string.Empty;
            dr["SITE_EmailID_var"] = string.Empty;
            dr["SITE_Status_bit"] = false;
            dr["SITE_MonthlyBillingStatus_bit"] = false;
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

            //grdContact.DataSource = dt;
            //grdContact.DataBind();

        }
        protected void lnkLoadKYCDetails(object sender, CommandEventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;
            ClearAllControls();

            string siteID = lb.CommandArgument.ToString();
            if (siteID != "0")
            {
                Label address = ((Label)gr.FindControl("lblSiteAddress"));
                Label name = ((Label)gr.FindControl("lblSiteName"));

                txtSiteAdd.Text = address.Text;
                lblSiteName.Text = name.Text;
                lblSiteId.Text = siteID;
                lblKycSiteId.Text = "Site Id : " + siteID;
                ModalPopupExtender1.Show();
                addWorkStatus();
                addRowTestingDetails();
                getExistingKYCDetails(siteID);

            }
        }
        private void addRowTestingDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["grdTestingDetails"] != null)
            {

                GetCurrentTestingData();
                dt = (DataTable)ViewState["grdTestingDetails"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("ddlMatSelect", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlLabSelect", typeof(string)));

            }

            dr = dt.NewRow();
            dr["ddlMatSelect"] = string.Empty;
            dr["ddlLabSelect"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["grdTestingDetails"] = dt;
            grdTestingDetails.DataSource = dt;
            grdTestingDetails.DataBind();
            SetPreviousTestingData();
        }
        private void GetCurrentTestingData()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("ddlMatSelect", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlLabSelect", typeof(string)));

            for (int i = 0; i < grdTestingDetails.Rows.Count; i++)
            {
                DropDownList box2 = (DropDownList)grdTestingDetails.Rows[i].Cells[0].FindControl("ddlMatSelect");
                DropDownList box3 = (DropDownList)grdTestingDetails.Rows[i].Cells[1].FindControl("ddlLabSelect");

                drRow = dtTable.NewRow();
                drRow["ddlMatSelect"] = box2.SelectedItem.Text;
                drRow["ddlLabSelect"] = box3.SelectedItem.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["grdTestingDetails"] = dtTable;
        }
        private void SetPreviousTestingData()
        {
            DataTable dt = (DataTable)ViewState["grdTestingDetails"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList box2 = (DropDownList)grdTestingDetails.Rows[i].Cells[0].FindControl("ddlMatSelect");
                DropDownList box3 = (DropDownList)grdTestingDetails.Rows[i].Cells[1].FindControl("ddlLabSelect");

                // box2.SelectedItem.Text = dt.Rows[i]["ddlMatSelect"].ToString();
                box2.ClearSelection(); //making sure the previous selection has been cleared
                if (box2.Items.FindByText(dt.Rows[i]["ddlMatSelect"].ToString()) != null)
                    box2.Items.FindByText(dt.Rows[i]["ddlMatSelect"].ToString()).Selected = true;

                box3.ClearSelection(); //making sure the previous selection has been cleared
                if (box3.Items.FindByText(dt.Rows[i]["ddlLabSelect"].ToString()) != null)
                    box3.Items.FindByText(dt.Rows[i]["ddlLabSelect"].ToString()).Selected = true;

                //box3.SelectedItem.Text = dt.Rows[i]["ddlLabSelect"].ToString();

            }
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            //ClearAllControls();
            ModalPopupExtender1.Hide();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            lblClientMessage.Text = "";
            if (Page.IsValid)
            {
                if (ValidateData())
                {
                    string constMangment = "";
                    if (txtOutSource.Visible)
                        constMangment = ddlConstMangmt.SelectedItem.Text + "-" + txtOutSource.Text;
                    else
                        constMangment = ddlConstMangmt.SelectedItem.Text;


                    string workStatusDetail = "";
                    for (int i = 0; i < 3; i++)
                    {
                        DropDownList status = (DropDownList)grdWorkStatus.Rows[i].Cells[0].FindControl("ddlStatus");
                        workStatusDetail = workStatusDetail + status.Text;
                        if (i != 2)
                            workStatusDetail = workStatusDetail + "~";
                    }

                    string testingDetail = "";
                    for (int i = 0; i < grdTestingDetails.Rows.Count; i++)
                    {
                        DropDownList mat = (DropDownList)grdTestingDetails.Rows[i].Cells[0].FindControl("ddlMatSelect");
                        DropDownList lab = (DropDownList)grdTestingDetails.Rows[i].Cells[1].FindControl("ddlLabSelect");

                        testingDetail = testingDetail + mat.SelectedItem.Text + "/" + lab.SelectedItem.Text;
                        if (i != grdTestingDetails.Rows.Count - 1)
                            testingDetail = testingDetail + "~";
                    }
                    Nullable<DateTime> complitionDate = null;
                    complitionDate = DateTime.ParseExact(txtCompDate.Text, "dd/MM/yyyy", null);

                    Nullable<DateTime> propsStartDate = null;
                    propsStartDate = DateTime.ParseExact(txtProposedDate.Text, "dd/MM/yyyy", null);

                    dc.SiteKYC_Update(Convert.ToInt32(lblSiteId.Text), txtSiteAdd.Text, Convert.ToDecimal(txtArea.Text), txtConsPeriod.Text, complitionDate, txtRccConslt.Text, txtArchitect.Text, constMangment, ddlGeoInvestigatn.SelectedItem.Text, Convert.ToInt32(txtNoOfBldg.Text), workStatusDetail, Convert.ToInt32(txtNoOfProposedBldg.Text), propsStartDate, Convert.ToInt32(txtNoOfBldgComp.Text), testingDetail, Convert.ToInt32(Session["LoginId"]));
                    //Reset();
                    lblClientMessage.Visible = true;
                    lblClientMessage.ForeColor = System.Drawing.Color.Green;
                    lblClientMessage.Text = "Record Saved Successfully";
                    lnkSave.Enabled = false; LoadSiteList();
                }
            }
        }

        private void Reset()
        {
            txtArchitect.Text = "";
            txtArea.Text = "";
            txtCompDate.Text = "";
            txtConsPeriod.Text = "";
            txtNoOfBldg.Text = "";
            txtNoOfBldgComp.Text = "";
            txtNoOfProposedBldg.Text = "";
            txtOutSource.Text = "";
            txtProposedDate.Text = "";
            txtRccConslt.Text = "";
            txtSiteAdd.Text = "";
            lblSiteName.Text = "";
            lblKycSiteId.Text = "";
            addWorkStatus();
            lblCompDt.Visible = false;
            lblOtSource.Visible = false;
            lblPrpDate.Visible = false;
            ddlConstMangmt.SelectedIndex = 0;
            ddlGeoInvestigatn.SelectedIndex = 0;
            ViewState["grdTestingDetails"] = null;
            addRowTestingDetails();
            lblSiteId.Text = "0";
        }
        private bool ValidateData()
        {
            Boolean valid = true;
            lblClientMessage.ForeColor = System.Drawing.Color.Red;

            if (txtCompDate.Text == "")
            {
                lblCompDt.Visible = true;
                valid = false;
            }
            else
                lblCompDt.Visible = false;

            if (txtOutSource.Visible == true)
            {
                if (txtOutSource.Text == "")
                {
                    lblOtSource.Visible = true;
                    txtOutSource.Focus();
                    valid = false;
                }
                else
                    lblOtSource.Visible = false;

            }

            if (txtProposedDate.Text == "")
            {
                lblPrpDate.Visible = true;
                valid = false;
            }
            else
                lblPrpDate.Visible = false;

            if (lblSiteId.Text == "" && lblSiteId.Text == "0")
            {
                lblClientMessage.Text = "Please select site to update KYC details ";
                valid = false;
            }
            else if (ddlConstMangmt.SelectedIndex == 0)
            {
                lblClientMessage.Visible = true;
                lblClientMessage.Text = "Please select Construction Management ";
                ddlConstMangmt.Focus();
                valid = false;
            }
            else if (ddlGeoInvestigatn.SelectedIndex == 0)
            {
                lblClientMessage.Visible = true;
                lblClientMessage.Text = "Please select Geo technical Investigation ";
                ddlGeoInvestigatn.Focus();
                valid = false;
            }
            else if (grdTestingDetails.Rows.Count > 0)
            {
                for (int i = 0; i < grdTestingDetails.Rows.Count; i++)
                {
                    DropDownList box2 = (DropDownList)grdTestingDetails.Rows[i].Cells[0].FindControl("ddlMatSelect");
                    DropDownList box3 = (DropDownList)grdTestingDetails.Rows[i].Cells[1].FindControl("ddlLabSelect");

                    if (box2.SelectedIndex == 0)
                    {
                        lblClientMessage.Text = "Please select material ";
                        lblClientMessage.Visible = true;
                        valid = false;
                    }
                    else if (box3.SelectedIndex == 0)
                    {
                        lblClientMessage.Text = "Please select lab ";
                        lblClientMessage.Visible = true;
                        valid = false;
                    }
                }
            }
            return valid;
        }
        protected void grdTestingDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var inwd = dc.Material_View("", "");
                DropDownList ddlMatSelect = (e.Row.FindControl("ddlMatSelect") as DropDownList);
                ddlMatSelect.DataTextField = "MATERIAL_Name_var";
                ddlMatSelect.DataValueField = "MATERIAL_Id";
                ddlMatSelect.DataSource = inwd;
                ddlMatSelect.DataBind();
                ddlMatSelect.Items.Insert(0, "---Select---");

                var lab = dc.Lab_View();
                DropDownList ddlLabSelect = (e.Row.FindControl("ddlLabSelect") as DropDownList);
                ddlLabSelect.DataTextField = "Lab_Name_var";
                ddlLabSelect.DataValueField = "Lab_Id";
                ddlLabSelect.DataSource = lab;
                ddlLabSelect.DataBind();
                ddlLabSelect.Items.Insert(0, "---Select---");
            }

        }
        protected void imgBtnDeleteRow_TestingDetails(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            DeleteTestingRow(rowIndex);
        }
        private void DeleteTestingRow(int rowIndex)
        {
            int rowCount = grdTestingDetails.Rows.Count;
            if (rowCount != 1)
            {
                GetCurrentTestingData();
                DataTable dt = ViewState["grdTestingDetails"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["grdTestingDetails"] = dt;
                grdTestingDetails.DataSource = dt;
                grdTestingDetails.DataBind();
                SetPreviousTestingData();
            }
        }
        private void addWorkStatus()
        {
            object[] RowValues = { "RCC", "Block Work Plaster", "Finishes" };

            DataTable dt = new DataTable();
            DataRow dr = null;
            //if (ViewState["grdWorkStatus"] != null)
            //{

            //    GetCurrentData();
            //    dt = (DataTable)ViewState["grdWorkStatus"];
            //}
            //else
            //{
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            grdWorkStatus.DataSource = dt;
            grdWorkStatus.DataBind();
            for (int i = 0; i < 3; i++)
            {
                Label lblWork = (Label)grdWorkStatus.Rows[i].FindControl("lblWork");

                lblWork.Text = RowValues[i].ToString();
            }
            //}

            //dr = dt.NewRow();
            //dt.Rows.Add(dr);

            //ViewState["grdWorkStatus"] = dt;

            //grdWorkStatus.DataSource = dt;
            //grdWorkStatus.DataBind();


        }
        private void getExistingKYCDetails(string siteId)
        {
            var kycDetails = dc.SiteKYC_View(Convert.ToInt32(siteId), -1, 1).ToList();
            if (kycDetails != null)
            {
                txtArchitect.Text = kycDetails.FirstOrDefault().SITE_Architect_var;
                txtArea.Text = kycDetails.FirstOrDefault().SITE_BulidUpArea_dec.ToString();
                if (kycDetails.FirstOrDefault().SITE_ComplitionDate_dt.ToString() != "")
                    txtCompDate.Text = Convert.ToDateTime(kycDetails.FirstOrDefault().SITE_ComplitionDate_dt).ToString("dd/MM/yyyy"); ;

                txtConsPeriod.Text = kycDetails.FirstOrDefault().SITE_ConstPeriod_var;
                txtNoOfBldg.Text = kycDetails.FirstOrDefault().SITE_BldgsUnderConst_int.ToString();
                txtNoOfBldgComp.Text = kycDetails.FirstOrDefault().SITE_CompletedBldgs_int.ToString();
                txtNoOfProposedBldg.Text = kycDetails.FirstOrDefault().SITE_ProposedBldgs_int.ToString();
                string mangment = kycDetails.FirstOrDefault().SITE_ConstMangmt_var;
                string[] constMgmt = null;
                if (mangment != null)
                {
                    constMgmt = kycDetails.FirstOrDefault().SITE_ConstMangmt_var.Split('-');
                    if (constMgmt[0] == "Own")
                        ddlConstMangmt.SelectedIndex = 1;
                    else if (constMgmt[0] == "Out Sourced")
                        ddlConstMangmt.SelectedIndex = 2;
                    else
                        ddlConstMangmt.SelectedIndex = 0;
                    //ddlConstMangmt.ClearSelection();
                    //ddlConstMangmt.Items.FindByText(constMgmt[0]).Selected = true;
                    if (constMgmt.Length > 1)
                    {
                        lblOutSource.Visible = true;
                        txtOutSource.Visible = true;
                        txtOutSource.Text = constMgmt[1];
                    }
                    else
                    {
                        lblOutSource.Visible = false;
                        txtOutSource.Visible = false;
                    }


                }

                if (kycDetails.FirstOrDefault().SITE_StartDate_dt.ToString() != "")
                    txtProposedDate.Text = Convert.ToDateTime(kycDetails.FirstOrDefault().SITE_StartDate_dt).ToString("dd/MM/yyyy"); ;

                txtRccConslt.Text = kycDetails.FirstOrDefault().SITE_RccConsltnt_var;
                txtSiteAdd.Text = kycDetails.FirstOrDefault().SITE_Address_var;
                lblSiteName.Text = kycDetails.FirstOrDefault().SITE_Name_var;
                lblKycSiteId.Text = "Site Id : " + kycDetails.FirstOrDefault().SITE_Id.ToString();

                //  ddlGeoInvestigatn.ClearSelection();
                //  ddlGeoInvestigatn.SelectedItem.Text = kycDetails.FirstOrDefault().SITE_GeoInvstgn_var;

                if (kycDetails.FirstOrDefault().SITE_GeoInvstgn_var == "To be done")
                    ddlGeoInvestigatn.SelectedIndex = 1;
                else if (kycDetails.FirstOrDefault().SITE_GeoInvstgn_var == "Complete")
                    ddlGeoInvestigatn.SelectedIndex = 2;
                else
                    ddlGeoInvestigatn.SelectedIndex = 0;

                //if (kycDetails.FirstOrDefault().SITE_GeoInvstgn_var!="")
                //{
                //    ddlConstMangmt.Items.Clear();
                //    ddlGeoInvestigatn.Items.FindByText(kycDetails.FirstOrDefault().SITE_GeoInvstgn_var).Selected = true;
                //}

                if (kycDetails.FirstOrDefault().SITE_WorkStatus != "" && kycDetails.FirstOrDefault().SITE_WorkStatus != null)
                {
                    string[] workStatus = kycDetails.FirstOrDefault().SITE_WorkStatus.Split('~');
                    for (int i = 0; i < 3; i++)
                    {
                        DropDownList lblStatus = (DropDownList)grdWorkStatus.Rows[i].FindControl("ddlStatus");
                        if (workStatus[i] == "Yet to start")
                            lblStatus.SelectedIndex = 0;
                        else if (workStatus[i] == "In Progress")
                            lblStatus.SelectedIndex = 1;
                        else
                            lblStatus.SelectedIndex = 2;
                    }
                }

                if (kycDetails.FirstOrDefault().SITE_CurrTestingDetails_var != "" && kycDetails.FirstOrDefault().SITE_CurrTestingDetails_var != null)
                {
                    string[] currentTesting = kycDetails.FirstOrDefault().SITE_CurrTestingDetails_var.Split('~');
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    string[] value;
                    dt.Columns.Add(new DataColumn("ddlMatSelect", typeof(string)));
                    dt.Columns.Add(new DataColumn("ddlLabSelect", typeof(string)));
                    for (int i = 0; i < currentTesting.Length; i++)
                    {
                        value = currentTesting[i].Split('/');
                        dr = dt.NewRow();
                        dr["ddlMatSelect"] = value[0];
                        dr["ddlLabSelect"] = value[1];
                        dt.Rows.Add(dr);
                    }

                    ViewState["grdTestingDetails"] = dt;
                    grdTestingDetails.DataSource = dt;
                    grdTestingDetails.DataBind();
                    //addRowTestingDetails();
                    //addTestingDetails();
                    SetPreviousTestingData();
                }
            }
        }


        protected void imgInsertTestingDetails_Click(object sender, CommandEventArgs e)
        {
            addRowTestingDetails();
        }

        protected void ddlConstMangmt_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            lblOutSource.Visible = false;
            txtOutSource.Visible = false;
            if (ddlConstMangmt.SelectedItem.Text == "Out Sourced")
            {
                lblOutSource.Visible = true;
                txtOutSource.Visible = true;
            }
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        protected void grdSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSiteId = ((Label)e.Row.FindControl("lblSiteId"));
                if (lblSiteId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgEditSite")).Visible = false;
                    ((LinkButton)e.Row.FindControl("btnSites")).Visible = false;
                }
                if (lblClientApprRight.Text != "true")
                {
                    ((ImageButton)e.Row.FindControl("imgEditSite")).Visible = false;
                    ((LinkButton)e.Row.FindControl("btnSites")).Visible = false;
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
        protected void lnkCancelSite_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender2.Hide();
        }
        protected void imgInsertSite_Click(object sender, EventArgs e)
        {
            //   ClearAllControls();

            if (txt_Client.Text != "")
            {
                chkApprPendingSite.Checked = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                LoadStateList();
                LoadRouteList();
                LoadLocationList();
                BindCity(); valName1.Visible = false;
                valName2.Visible = false;
                txtContactPer.Text = ""; txtContactNo.Text = "";
                rbSiteUnReg.Checked = false;
                rbSiteReg.Checked = false;
                txtGstNo.Enabled = false;
                txtGstDate.Enabled = false;
                ddlCity.Visible = true;
                txtCity.Visible = false; txtCity.Text = "";
                lnkAddCity.Text = "New City";
                lnkSaveCity.Visible = false;
                ModalPopupExtender2.Show();
                lblAddSite.Text = "Add New Site"; prjAdd = ""; pincode = ""; city = ""; state = ""; gstNo = ""; gstDt = "";
                lblSiteClient.Text = txt_Client.Text.Replace("Client : ", "");
                //Session["Site_Id"] = 0;
                chkSiteStatus.Checked = true;
                chkDetailSameAsClient.Checked = false;
                lnkSaveContact.Visible = false;

                ddlContactNm.DataSource = null;
                ddlContactNm.DataBind();
                ddlContactNm.Items.Clear();
                ddlContactNm.Items.Insert(0, new ListItem("--Add New--", "0"));
                //  chkServiceTax.Checked = false;
                //LoadContactPersonList();
                //grdContact.Visible = false;
                //lblEditedSite.Visible = false;
                //lblContPer.Visible = false;
                txtSiteName.ReadOnly = false;
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "Please Select Client";
            }
        }

        protected void chkDetailSameAsClient_CheckedChanged(object sender, EventArgs e)
        {
            // setDetailsOfClientToSite();
            if (chkDetailSameAsClient.Checked)
            {
                var cl = dc.Client_View(0, 0, lblSiteClient.Text, "");
                foreach (var ssss in cl)
                {
                    if (txtSiteAddress.Text == "")
                        txtSiteAddress.Text = ssss.CL_OfficeAddress_var;

                    if (ddlState.Items.FindByText(ssss.CL_State_var) != null)
                    {
                        ddlState.ClearSelection();
                        ddlState.Items.FindByText(ssss.CL_State_var).Selected = true;
                    }
                    else if (ssss.CL_State_var != null && ssss.CL_State_var != "")
                    {
                        ddlState.ClearSelection();
                        ddlState.SelectedItem.Text = ssss.CL_State_var;
                    }
                    //txtCity.Text = ssss.CL_City_var;
                    if (ddlState.SelectedValue != "")
                        BindCity();
                    if (ddlCity.Items.FindByText(ssss.CL_City_var) != null)
                        ddlCity.Items.FindByText(ssss.CL_City_var).Selected = true;
                    else if (ssss.CL_City_var != null && ssss.CL_City_var != "")
                        ddlCity.SelectedItem.Text = ssss.CL_City_var;


                    txtPinCode.Text = ssss.CL_Pin_int.ToString();
                    int gstBit = Convert.ToInt32(ssss.CL_GST_bit);
                    if (gstBit == 1)
                    {
                        rbSiteReg.Checked = true;
                        txtGstDate.Enabled = true;
                        txtGstNo.Enabled = true;
                        txtGstNo.Text = ssss.CL_GstNo_var.ToString();
                        txtGstDate.Text = Convert.ToDateTime(ssss.CL_GstDate_date).ToString();
                        System.Web.UI.WebControls.Calendar cll = new System.Web.UI.WebControls.Calendar();
                        cll.SelectedDate = Convert.ToDateTime(txtGstDate.Text);
                        txtGstDate.Text = cll.SelectedDate.ToString("dd/MM/yyyy");
                        rbSiteUnReg.Checked = false;

                    }
                    else
                    {
                        rbSiteReg.Checked = false;
                        rbSiteUnReg.Checked = true;
                        txtGstDate.Enabled = false;
                        txtGstNo.Enabled = false;
                        txtGstNo.Text = ""; txtGstDate.Text = "";
                    }
                    break;
                }
            }
            else
            {
                txtSiteAddress.Text = prjAdd;
                // txtCity.Text = city;
                txtPinCode.Text = pincode;
                txtGstDate.Text = gstDt;
                txtGstNo.Text = gstNo;

                LoadStateList();                
                if (ddlState.Items.FindByText(state) != null)
                {
                    ddlState.ClearSelection();
                    ddlState.Items.FindByText(state).Selected = true;
                }
                else if (state != null && state != "")
                {
                    ddlState.ClearSelection(); ddlState.SelectedItem.Text = state;
                }



                if (ddlState.SelectedValue != "")
                    BindCity();
                if (ddlCity.Items.FindByText(city) != null)
                    ddlCity.Items.FindByText(city).Selected = true;
                else if (city != null && city != "")
                    ddlCity.SelectedItem.Text = city;


                if (gstCheked == "1")
                {
                    rbSiteReg.Checked = true; rbSiteUnReg.Checked = false;
                    txtGstDate.Enabled = true; txtGstNo.Enabled = true;
                    txtGstNo.Text = gstNo; txtGstDate.Text = gstDt;
                }
                else
                {
                    rbSiteReg.Checked = false; rbSiteUnReg.Checked = true;
                    txtGstNo.Text = ""; txtGstDate.Text = ""; txtGstDate.Enabled = false;
                    txtGstNo.Enabled = false;
                }
            }
        }

        protected void imgEditSite_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender2.Show();
            LoadStateList();
            LoadRouteList();
            LoadLocationList();
            valName1.Visible = false;
            valName2.Visible = false;
            rbSiteUnReg.Checked = false; rbSiteReg.Checked = false; txtGstNo.Enabled = false; txtGstDate.Enabled = false;
            lnkSaveContact.Visible = true;
            ddlCity.Visible = true;
            txtCity.Visible = false; txtCity.Text = "";
            lnkAddCity.Text = "New City";
            lnkSaveCity.Visible = false;
            txtContactPer.Text = ""; txtContactNo.Text = "";
            //lblSiteMessage.Visible = false;
            //lnkSaveSite.Enabled = true;           
            //pnlSite.Visible = true;
            lblAddSite.Text = "Edit Site";
            lblSiteClient.Text = txt_Client.Text.Replace("Client : ", "");
            if (lblClientApprRight.Text != "true")
                txtSiteName.ReadOnly = true;
            else
                txtSiteName.ReadOnly = false;

            //Session["Site_Id"] = Convert.ToInt32(e.CommandArgument);
            int siteId = Convert.ToInt32(e.CommandArgument);
            //if (Session["Site_Id"].ToString() == "0")
            //    pnlSite.Visible = true; 
            chkDetailSameAsClient.Checked = false;
            //var s = dc.Site_View(Convert.ToInt32(Session["Site_Id"]), 0, 0, "");
            var s = dc.Site_View(siteId, 0, 0, "");
            foreach (var site in s)
            {
                lblCurrentSiteName.Text = site.SITE_Name_var;
                lblCurrentSiteId.Text = "Site Id : " + site.SITE_Id;
                txtSiteName.Text = site.SITE_Name_var;
                txtSiteAddress.Text = site.SITE_Address_var;
                txtSiteEmail.Text = site.SITE_EmailID_var;

                txtSiteIncharge.Text = site.SITE_Incharge_var;
                txtPinCode.Text = site.SITE_Pincode_int.ToString();
                pincode = site.SITE_Pincode_int.ToString();
                txtPhno.Text = site.SITE_Phno_int;
                txtNatureOfSite.Text = site.SITE_Nature_var;
                // txtCity.Text = site.SITE_City_var;
                city = site.SITE_City_var;
                state = site.SITE_State_var;
                prjAdd = site.SITE_Address_var;
                txtSiteIncMobNo.Text = site.SITE_IncMobNo_var;
                txtReRa.Text = site.SITE_ReRa_var;


                if (ddlState.Items.FindByText(site.SITE_State_var) != null)
                    ddlState.Items.FindByText(site.SITE_State_var).Selected = true;
                else if (site.SITE_State_var != null && site.SITE_State_var != "")
                    ddlState.SelectedItem.Text = site.SITE_State_var;


                if (ddlState.SelectedValue != "")
                    BindCity();
                if (ddlCity.Items.FindByText(site.SITE_City_var) != null)
                    ddlCity.Items.FindByText(site.SITE_City_var).Selected = true;
                else if (site.SITE_City_var != null && site.SITE_City_var != "")
                    ddlCity.SelectedItem.Text = site.SITE_City_var;

                if (ddlLocation.Items.FindByValue(site.SITE_LocationId_int.ToString()) != null)
                    ddlLocation.Items.FindByValue(site.SITE_LocationId_int.ToString()).Selected = true;
                //if (Convert.ToInt32(ddlLocation.SelectedValue) != 0 && ddlLocation.SelectedValue != "" && ddlLocation.SelectedValue != null)
                if(ddlLocation.SelectedIndex>0)
                {
                    var route = dc.Route_View(0, "", "", Convert.ToInt32(ddlLocation.SelectedValue)).ToList();
                    if (route.Count > 0)
                    {
                        ddlRoute.SelectedValue = route.FirstOrDefault().ROUTE_Id.ToString();
                    }
                }
                else
                    ddlRoute.SelectedValue = "0";

                //if (ddlRoute.Items.FindByValue(site.SITE_Route_Id.ToString()) != null)
                //    ddlRoute.Items.FindByValue(site.SITE_Route_Id.ToString()).Selected = true;

                    //if (site.SITE_Status_bit == false)
                if (site.SITE_Status_bit == 2)
                {
                    chkSiteStatus.Checked = true;
                    lnkSaveSite.Text = "Approve";
                    lnkViewAllSites.Visible = true;
                }
                else if (site.SITE_Status_bit == 0)
                {
                    chkSiteStatus.Checked = true;
                    lnkSaveSite.Text = "Save";
                    lnkViewAllSites.Visible = false;
                }
                else
                {
                    // chkClientStatus.Checked = false;
                    lnkSaveSite.Text = "Save";
                    lnkViewAllSites.Visible = false;
                }

                //if (site.SITE_SameAsClient_bit == true)
                //    chkDetailSameAsClient.Checked = true;
                //else
                //    chkDetailSameAsClient.Checked = false;

                if (site.SITE_SerTaxStatus_bit == true)
                {
                    //chkServiceTax.Checked = true;
                }
                else
                {
                    // chkServiceTax.Checked = false;
                }

                if (site.SITE_SEZStatus_bit == true)
                {
                    chkSEZ.Checked = true;
                }
                else
                {
                    chkSEZ.Checked = false;
                }

                if (site.SITE_GST_bit == true)
                {
                    rbSiteReg.Checked = true; rbSiteUnReg.Checked = false;
                    txtGstNo.Enabled = true;
                    txtGstDate.Enabled = true;
                    txtGstNo.Text = site.SITE_GSTNo_var;
                    gstNo = site.SITE_GSTNo_var;
                    gstCheked = "1";
                    if (site.SITE_GstDate_dt != null)
                    {
                        txtGstDate.Text = Convert.ToDateTime(site.SITE_GstDate_dt).ToString();
                        System.Web.UI.WebControls.Calendar cll = new System.Web.UI.WebControls.Calendar();
                        cll.SelectedDate = Convert.ToDateTime(txtGstDate.Text);
                        txtGstDate.Text = cll.SelectedDate.ToString("dd/MM/yyyy");
                        gstDt = cll.SelectedDate.ToString("dd/MM/yyyy");
                    }
                }
                else if (site.SITE_GST_bit == false)
                {
                    rbSiteUnReg.Checked = true; rbSiteReg.Checked = false;
                    txtGstNo.Enabled = false;
                    txtGstDate.Enabled = false;
                    txtGstNo.Text = ""; gstCheked = "0"; gstNo = ""; gstDt = "";
                    txtGstDate.Text = "";
                }

                break;
            }
            LoadContactPersonList();
            //grdContact.Visible = false;
            //lblEditedSite.Visible = false;
            //lblContPer.Visible = false;
        }

        //protected void lnkCancelContact_Click(object sender, EventArgs e)
        //{
        //    ClearAllControls();

        //    // ModalPopupExtender3.Hide();
        //    LoadContactPersonList();
        //}
        protected void imgCloseSitePopup_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();
            ModalPopupExtender2.Hide();
        }
        private void ClearAllControls()
        {
            //txtContactNo.Text = "";
            //txtContactPer.Text = "";
            //txtContactEmail.Text = "";
            txtSiteAddress.Text = "";
            txtSiteEmail.Text = "";
            txtSiteName.Text = "";
            lblCurrentSiteName.Text = "";
            lblCurrentSiteId.Text = "";
            txtSiteIncharge.Text = "";
            txtPinCode.Text = "";
            txtPhno.Text = "";
            txtNatureOfSite.Text = "";
            txtGstNo.Text = ""; txtCity.Text = "";
            txtProposedDate.Text = "";
            txtCompDate.Text = "";
            txtSiteIncMobNo.Text = "";
            txtOutSource.Text = "";
            ddlConstMangmt.SelectedIndex = 0;
            ddlGeoInvestigatn.SelectedIndex = 0;
            txtGstDate.Text = "";
            ddlState.SelectedIndex = -1;
            ddlRoute.SelectedIndex = -1;
            ddlLocation.SelectedIndex = -1;
            lblSiteMessage.Visible = false;
            ViewState["grdTestingDetails"] = null;
            lblClientMessage.Visible = false;
            ddlContactNm.DataSource = null;
            ddlContactNm.DataBind();
            ddlContactNm.Items.Clear();
            ddlContactNm.Items.Insert(0, new ListItem("--Add New--", "0"));
            lblCompDt.Visible = false;
            lblPrpDate.Visible = false;
            lblOtSource.Visible = false;
            lblOutSource.Visible = false;
            txtOutSource.Visible = false;
            lnkSaveSite.Enabled = true;
            chkSEZ.Checked = false;            
            rbSiteReg.Checked = false;
            rbSiteUnReg.Checked = false;
            txtGstDate.Enabled = false;
            txtGstNo.Enabled = false; lnkSave.Enabled = true;
            lnkSaveSite.Text = "Save";
            //  lnkSaveContact.Enabled = true;
        }
        //protected void imgCloseContactPopup_Click(object sender, ImageClickEventArgs e)
        //{
        //    LoadContactPersonList();
        //    ClearAllControls();
        //    // ModalPopupExtender3.Hide();
        //}
        protected void rbSiteUnReg_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSiteUnReg.Checked)
            {
                txtGstNo.Enabled = false;
                txtGstDate.Enabled = false;
                txtGstNo.Text = ""; txtGstDate.Text = ""; valName1.Visible = false;
                valName2.Visible = false;
            }

        }
        protected void rbSiteReg_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSiteReg.Checked)
            {
                txtGstNo.Enabled = true;
                txtGstDate.Enabled = true;

            }

        }
        protected void lnkSaveCity_Click(object sender, EventArgs e)
        {
            if (ddlState.SelectedIndex != -1 && ddlState.SelectedIndex != 0)
            {
                if (txtCity.Text != "")
                {
                    ListItem item = ddlCity.Items.FindByText(txtCity.Text);
                    if (item == null && txtCity.Text != "")
                    {
                        ddlCity.Items.Add(txtCity.Text);
                    }

                    if (ddlCity.Items.FindByText(txtCity.Text) != null)
                    {
                        ddlCity.ClearSelection();
                        ddlCity.Items.FindByText(txtCity.Text).Selected = true;
                    }

                    ddlCity.Visible = true;
                    lnkAddCity.Text = "New City";
                    txtCity.Visible = false;
                    lnkSaveCity.Visible = false;
                }
            }
            else
            {
                lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                lblSiteMessage.Visible = true;
                lblSiteMessage.Text = "Please Select State";
            }
        }
        protected void lnkAddCity_Click(object sender, EventArgs e)
        {
            if (lnkAddCity.Text == "New City")
            {
                ddlCity.Visible = false;
                lnkAddCity.Text = "Old City";
                txtCity.Visible = true; txtCity.Text = "";
                lnkSaveCity.Visible = true;
            }
            else
            {
                ddlCity.Visible = true;
                lnkAddCity.Text = "New City";
                txtCity.Visible = false;
                // ddlCity.Items.Add(txtClntCity.Text);
                lnkSaveCity.Visible = false;
                //  ddlCity.Items.FindByText(txtClntCity.Text).Selected = true;

            }

        }

        protected void lnkSaveSite_Click(object sender, EventArgs e)
        {
            lblSiteMessage.Visible = false; bool flag = true;
            txtSiteName.Text = txtSiteName.Text.Replace("\t", "");
            valName1.Visible = false;
            valName2.Visible = false;


            if (txtPinCode.Text != "" &&  txtPinCode.Text.Length < 6)
            {
                flag = false;
                lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                lblSiteMessage.Visible = true;
                lblSiteMessage.Text = "Invalid PinCode";
            }
            else if (ddlCity.SelectedItem.Text == "--Select--" || ddlCity.SelectedItem.Text == "")
            {
                flag = false;
                lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                lblSiteMessage.Visible = true;
                lblSiteMessage.Text = "Please Select City";
            }
            else if (txtCity.Visible == true)
            {
                flag = false;
                lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                lblSiteMessage.Visible = true;
                lblSiteMessage.Text = "Please Select City";
            }
            else if (txtReRa.Text != "" && txtReRa.Text.Length < 15)
            {
                //if (txtReRa.Text.Length < 15)
                //{
                    lblSiteMessage.Text = "Invalid ReRa No.";
                    lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                    lblSiteMessage.Visible = true;
                    txtReRa.Focus();
                    flag = false;
                //}
            }
            else if (ddlLocation.SelectedIndex <= 0)
            {
                flag = false;
                lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                lblSiteMessage.Visible = true;
                lblSiteMessage.Text = "Select Location.";
            }
            //else if (ddlRoute.SelectedIndex <= 0)
            //{
            //    flag = false;
            //    lblSiteMessage.ForeColor = System.Drawing.Color.Red;
            //    lblSiteMessage.Visible = true;
            //    lblSiteMessage.Text = "Select route.";
            //}
            else if (rbSiteReg.Checked)
            {
                if (txtGstNo.Text == "")
                {
                    valName1.Visible = true; flag = false;
                }
                else if (txtGstNo.Text.Length < 15)
                {
                    lblSiteMessage.Text = "Invalid GST No.";
                    lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                    lblSiteMessage.Visible = true; flag = false;
                }

                //if (txtGstDate.Text == "")
                //{
                //    valName2.Visible = true; flag = false;
                //}

            }
            else if (rbSiteReg.Checked == false && rbSiteUnReg.Checked == false)
            {
                lblSiteMessage.Text = "Input GST.";
                lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                lblSiteMessage.Visible = true;
                flag = false;
            }
            else if (txtContactPer.Text == "" && txtContactNo.Text != "" || txtContactPer.Text != "" && txtContactNo.Text == "")
            {
                flag = false;
                lblSiteMessage.ForeColor = System.Drawing.Color.Red;
                lblSiteMessage.Visible = true;

                if (txtContactPer.Text == "")
                    lblSiteMessage.Text = "Input Contact Person";

                if (txtContactNo.Text == "")
                    lblSiteMessage.Text = "Input Contact No";

            }
            int clientId = 0, siteId = 0;
            if (txt_Client.Text != "")
            {
                string ClientId = Request.Form[hfClientId.UniqueID];
                if (ClientId != "")
                {
                    //Session["CL_ID"] = Convert.ToInt32(ClientId);
                    clientId = Convert.ToInt32(ClientId);
                }
                var client = dc.Client_View(0, 0, txt_Client.Text, "");
                foreach (var cl in client)
                {
                    clientId = cl.CL_Id;
                }
            }
            if (lblAddSite.Text == "Add New Site")
            {
                siteId = 0;
            }
            else if (lblAddSite.Text == "Edit Site")
            {
                siteId = Convert.ToInt32(lblCurrentSiteId.Text.Replace("Site Id : ",""));                
            }
            if (flag)
            {
                //var client = dc.Client_View(Convert.ToInt32(Session["Cl_Id"]), 0, "", "");
                var client = dc.Client_View(clientId, 0, "", "");
                foreach (var cl in client)
                {
                    if ((rbSiteReg.Checked == true && cl.CL_GST_bit == false) ||
                        (rbSiteReg.Checked == true && cl.CL_GST_bit == null))
                    {
                        lblSiteMessage.Text = "GST is unregister for client, Can not select register for site.";
                        lblSiteMessage.Visible = true;
                        flag = false;
                        break;
                    }
                }
            }
            if (Page.IsValid)
            {
                if (flag)
                {
                    byte siteStatus = 0;
                    //check for duplicate site
                    //var s = dc.Site_View(0, Convert.ToInt32(Session["Cl_Id"]), 0, txtSiteName.Text);
                    var s = dc.Site_View(0, clientId, 0, txtSiteName.Text);
                    foreach (var site in s)
                    {
                        //if (Convert.ToInt32(Session["Site_Id"]) == 0)
                        if (siteId == 0)
                        {
                            siteStatus = 1;
                            lblSiteMessage.Text = "Duplicate Site Name..";
                            lblSiteMessage.Visible = true;
                            break;
                        }
                        //else if (Convert.ToInt32(Session["Site_Id"]) > 0)
                        else if (siteId > 0)
                        {
                            //if (site.SITE_Id != Convert.ToInt32(Session["Site_Id"]))
                            if (site.SITE_Id != siteId)
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

                        bool gstBit = false;

                        if (rbSiteReg.Checked)
                            gstBit = true;


                        if (chkSiteStatus.Checked == true)
                            siteStatus = 0;
                        else
                            siteStatus = 1;

                        bool serTaxStatus = false; //sameAsClient=false;
                        //if (chkServiceTax.Checked == true)
                        //    serTaxStatus = true;
                        //else
                        //    serTaxStatus = false;


                        //if (chkDetailSameAsClient.Checked)
                        //    sameAsClient = true;



                        Nullable<DateTime> dt = null;

                        string cl_City;

                        if (txtGstDate.Text != "")
                            dt = DateTime.ParseExact(txtGstDate.Text, "dd/MM/yyyy", null);

                        if (ddlCity.Visible == true)
                            cl_City = ddlCity.SelectedItem.Text;
                        else
                            cl_City = txtCity.Text;


                        //int siteId = dc.Site_Update(Convert.ToInt32(Session["Site_Id"]), Convert.ToInt32(Session["Cl_Id"]), txtSiteName.Text, siteStatus, txtSiteAddress.Text, txtSiteEmail.Text, Convert.ToInt32(Session["LoginId"]), Convert.ToInt32(Session["LoginId"]), "",Convert.ToInt32(ddlLocation.SelectedValue), serTaxStatus, false, ddlState.SelectedItem.Text, Convert.ToInt32(txtPinCode.Text), txtPhno.Text, txtNatureOfSite.Text, txtGstNo.Text, dt, txtSiteIncharge.Text, Convert.ToBoolean(chkSEZ.Checked), gstBit, cl_City, txtSiteIncMobNo.Text, txtReRa.Text, Convert.ToInt32(ddlRoute.SelectedValue));
                        siteId = dc.Site_Update(siteId, clientId, txtSiteName.Text, siteStatus, txtSiteAddress.Text, txtSiteEmail.Text, Convert.ToInt32(Session["LoginId"]), Convert.ToInt32(Session["LoginId"]), "", Convert.ToInt32(ddlLocation.SelectedValue), serTaxStatus, false, ddlState.SelectedItem.Text, Convert.ToInt32(txtPinCode.Text), txtPhno.Text, txtNatureOfSite.Text, txtGstNo.Text, dt, txtSiteIncharge.Text, Convert.ToBoolean(chkSEZ.Checked), gstBit, cl_City, txtSiteIncMobNo.Text, txtReRa.Text, Convert.ToInt32(ddlRoute.SelectedValue));
                        
                        var cityDetails = dc.City_Update(cl_City, Convert.ToInt32(ddlState.SelectedValue), 0).ToList();

                        if (cityDetails.Count() == 0)
                            dc.City_Update(cl_City, Convert.ToInt32(ddlState.SelectedValue), 1);

                        if (txtContactPer.Text != "" && txtContactNo.Text != "")
                            //dc.Contact_Update(Convert.ToInt32(Session["ContactPersonId"]), Convert.ToInt32(Session["Cl_Id"]), siteId, txtContactPer.Text, txtContactNo.Text, "", null);
                            dc.Contact_Update(0, clientId, siteId, txtContactPer.Text, txtContactNo.Text, "", null);

                        lblSiteMessage.ForeColor = System.Drawing.Color.Green;
                        lblSiteMessage.Text = "Updated Successfully";
                        lblSiteMessage.Visible = true;
                        //ClearAllControls();
                        //ModalPopupExtender2.Hide();
                        lnkSaveSite.Enabled = false;
                        LoadSiteList();
                    }
                }
            }

        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlState.SelectedIndex > 0)
            {
                BindCity(); lblSiteMessage.Visible = false;
            }
        }
        private void BindCity()
        {
            var reg = dc.City_View(Convert.ToInt32(ddlState.SelectedValue));
            ddlCity.DataSource = reg;
            ddlCity.DataTextField = "City_Name_var";
            ddlCity.DataValueField = "City_Name_var";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select--", "0"));
        }


        protected void chkApprPendingSite_CheckedChanged(object sender, EventArgs e)
        {
            if (txt_Client.Text != "")
            {
                LoadSiteList();
            }
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocation.SelectedIndex > 0)
            {
                var route = dc.Route_View(0, "", "", Convert.ToInt32(ddlLocation.SelectedValue)).ToList();
                if (route.Count > 0)
                {
                    ddlRoute.SelectedValue = route.FirstOrDefault().ROUTE_Id.ToString();
                }
            }
        }

        protected void lnkSaveContact_Click(object sender, EventArgs e)
        {
            txtContactPer.Text = txtContactPer.Text.Replace("\t", "");
            lblSiteMessage.ForeColor = System.Drawing.Color.Red;
               
            bool contactStatus = false; lblSiteMessage.Visible = false;
            int clientId = 0, siteId = 0;
            if (txt_Client.Text != "")
            {
                string ClientId = Request.Form[hfClientId.UniqueID];
                if (ClientId != "")
                {
                    //Session["CL_ID"] = Convert.ToInt32(ClientId);
                    clientId = Convert.ToInt32(ClientId);
                }
                var client = dc.Client_View(0, 0, txt_Client.Text, "");
                foreach (var cl in client)
                {
                    clientId = cl.CL_Id;
                }
            }
            if (lblAddSite.Text == "Add New Site")
            {
                siteId = 0;
            }
            else if (lblAddSite.Text == "Edit Site")
            {
                siteId = Convert.ToInt32(lblCurrentSiteId.Text.Replace("Site Id : ", ""));
            }
            //check for duplicate contact
            if (ddlContactNm.SelectedItem.Text != txtContactPer.Text)
            {                
                //var cont = dc.Contact_View(0, Convert.ToInt32(Session["Site_Id"]), Convert.ToInt32(Session["Cl_Id"]), txtContactPer.Text);
                var cont = dc.Contact_View(0, siteId, clientId, txtContactPer.Text);
                foreach (var cn in cont)
                {
                    //if (Convert.ToInt32(Session["ContactPersonId"]) == 0)
                    if (ddlContactNm.SelectedIndex == 0)
                    {
                        contactStatus = true;
                        lblSiteMessage.Text = "Duplicate Contact Name..";
                        lblSiteMessage.Visible = true;
                        break;
                    }
                    //else if (Convert.ToInt32(Session["ContactPersonId"]) > 0)
                    else if (ddlContactNm.SelectedIndex > 0)
                    {
                        //if (cn.CONT_Id != Convert.ToInt32(Session["ContactPersonId"]))
                        if (cn.CONT_Id != Convert.ToInt32(ddlContactNm.SelectedValue))
                        {
                            contactStatus = true;
                            lblSiteMessage.Text = "Duplicate Contact Name..";
                            lblSiteMessage.Visible = true;
                            break;
                        }
                    }
                }
            }
            if (txtContactPer.Text == "")
            {
                contactStatus = true;
                lblSiteMessage.Text = "Input Contact Name";
                lblSiteMessage.Visible = true;
            }
            else if (txtContactNo.Text == "")
            {
                contactStatus = true;
                lblSiteMessage.Text = "Input Contact No";
                lblSiteMessage.Visible = true;
            }


            int ContactPersonId = 0;

            if (ddlContactNm.SelectedIndex != 0)
                ContactPersonId = Convert.ToInt32(ddlContactNm.SelectedValue);


            if (contactStatus == false)
            {
                //dc.Contact_Update(ContactPersonId, Convert.ToInt32(Session["Cl_Id"]), Convert.ToInt32(Session["Site_Id"]), txtContactPer.Text, txtContactNo.Text, "", null);
                dc.Contact_Update(ContactPersonId, clientId, siteId, txtContactPer.Text, txtContactNo.Text, "", null);

                txtContactNo.Text = "";
                txtContactPer.Text = "";
                LoadContactPersonList();

            }


        }

        protected void ddlContactNm_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSiteMessage.Visible = false;
            if (ddlContactNm.SelectedItem.Text == "--Add New--")
            {
                txtContactPer.Text = "";
                txtContactNo.Text = "";
                lnkSaveContact.Text = "Save Contact";
            }
            else
            {
                lnkSaveContact.Text = "Update Contact";
                var c = dc.Contact_View(Convert.ToInt32(ddlContactNm.SelectedValue), 0, 0, "");

                foreach (var contPer in c)
                {
                    txtContactPer.Text = contPer.CONT_Name_var;
                    txtContactNo.Text = contPer.CONT_ContactNo_var;
                    break;
                }
            }
        }

        protected void lnkViewAllSites_Click(object sender, EventArgs e)
        {
            pnlSiteSearch.Visible = true;
            pnlSite1.Visible = false;
            if (txtSiteName.Text != "")
            {
                string searchHead = "";
                if (txtSiteName.Text != "")
                    searchHead = "%" + txtSiteName.Text + "%";

                var site = dc.Site_View_Search(searchHead);
                grdSiteSearch.DataSource = site;
                grdSiteSearch.DataBind();

            }
        }

        protected void imgCloseSiteSearch_Click(object sender, ImageClickEventArgs e)
        {
            pnlSiteSearch.Visible = false;
            pnlSite1.Visible = true;
            grdSiteSearch.DataSource = null;
            grdSiteSearch.DataBind();
        }

        protected void grdSiteSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSiteStatus = ((Label)e.Row.FindControl("lblSiteStatus"));
                Label lblClientStatus = ((Label)e.Row.FindControl("lblClientStatus"));
                if (lblSiteStatus.Text == "0")
                {
                    lblSiteStatus.Text = "Active";
                }
                else if (lblSiteStatus.Text == "1")
                {
                    lblSiteStatus.Text = "Deactive";
                }
                else if (lblSiteStatus.Text == "2")
                {
                    lblSiteStatus.Text = "Approval Pending";
                }
                if (lblClientStatus.Text == "0")
                {
                    lblClientStatus.Text = "Active";
                }
                else if (lblClientStatus.Text == "1")
                {
                    lblClientStatus.Text = "Deactive";
                }
                else if (lblClientStatus.Text == "2")
                {
                    lblClientStatus.Text = "Approval Pending";
                }
            }
        }

        protected void lnkOkSiteSearch_Click(object sender, EventArgs e)
        {
            pnlSiteSearch.Visible = false;
            pnlSite1.Visible = true;
            grdSiteSearch.DataSource = null;
            grdSiteSearch.DataBind();
        }

    }
}