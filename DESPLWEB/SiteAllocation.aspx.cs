using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class SiteAllocation : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Site Allocation";
                }
                ddlRoute.Focus();
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
                        if (u.USER_Admin_right_bit == true || u.USER_CS_right_bit == true)
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

                LoadRouteList();

            }

        }

        private void LoadRouteList()
        {
            var routeList = dc.Route_View(0, "", "All", 0);
            ddlRoute.DataTextField = "Route_Name_var";
            ddlRoute.DataValueField = "Route_Id";
            ddlRoute.DataSource = routeList;
            ddlRoute.DataBind();
            ddlRoute.Items.Insert(0, "---Select---");
            //ddlRoute.Items.Insert((routeList.Count() + 1), "Other");
        }

        protected void rbMeWiseAllocation_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMeWiseAllocation.Checked)
            {
                lnkClear.Visible = true;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                LoadMeList();
                lblMe.Visible = true;
                ddlME.Visible = true;
                Mainpan1.Visible = true;
                Mainpan.Visible = false;
                Reset();
                lnkPrvAllocatedSite.Visible = true;
            }
        }
        protected void rbRouteWiseAllocation_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRouteWiseAllocation.Checked)
            {
                lnkClear.Visible = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                lblMe.Visible = false;
                ddlME.Visible = false;
                Reset();
                Mainpan1.Visible = false;
                Mainpan.Visible = true;
                lnkPrvAllocatedSite.Visible = false;
            }
        }

        private void LoadMeList()
        {
            var data = dc.User_View_ME(-1);
            ddlME.DataSource = data;
            ddlME.DataTextField = "USER_Name_var";
            ddlME.DataValueField = "USER_Id";
            ddlME.DataBind();
            ddlME.Items.Insert(0, new ListItem("---Select---", "0"));
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


            var res = db.Site_View_RouteWise(0, searchHead, 0);
            dt.Columns.Add("SITE_Name_var");
            foreach (var obj in res)
            {
                row = dt.NewRow();
                string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                dt.Rows.Add(listitem);
            }
            if (row == null)
            {
                var resclnm = db.Site_View_RouteWise(0, "", 0);
                foreach (var obj in resclnm)
                {
                    row = dt.NewRow();
                    string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                    dt.Rows.Add(listitem);
                }
            }

            List<string> SITE_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SITE_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return SITE_Name_var;
        }
        protected void txtSiteName_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            Label lblClientName = (Label)gvRow.FindControl("lblClientName");
            Label lblSiteId = (Label)gvRow.FindControl("lblSiteId");
            Label lblClId = (Label)gvRow.FindControl("lblClId");
            Label lblSiteAddress = (Label)gvRow.FindControl("lblSiteAddress");
            Label lblRouteId = (Label)gvRow.FindControl("lblRouteId");
            if (ChkSiteName(txt.Text))
            {
                int SiteId = 0;
                if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                {
                    //Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                    lbl_SiteId.Text = Request.Form[hfSiteId.UniqueID].ToString();
                }
                else
                {
                    //Session["SITE_ID"] = 0;
                    lbl_SiteId.Text = "0";
                }

                if (Convert.ToInt32(lbl_SiteId.Text) > 0)
                {
                    var s = dc.Site_View_RouteWise(Convert.ToInt32(lbl_SiteId.Text), "", 0);
                    foreach (var site in s)
                    {
                        lblClientName.Text = site.CL_Name_var;
                        lblSiteId.Text = site.SITE_Id.ToString();
                        lblClId.Text = site.SITE_CL_Id.ToString();
                        lblSiteAddress.Text = site.SITE_Address_var;
                        lblRouteId.Text = "";// site.SITE_Route_Id.ToString();
                        break;
                    }
                }
            }
            else
            {
                lblClientName.Text = "";
                lblSiteId.Text = "0";
                lblClId.Text = "0";
                lblSiteAddress.Text = "";
                lbl_SiteId.Text = "0";
                lblRouteId.Text = "";
            }

        }
        protected Boolean ChkSiteName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = false;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            // var res = dc.Site_View(0, 0, 0, searchHead);
            var res = dc.Site_View_RouteWise(0, searchHead, 0);
            if (searchHead != "")
            {
                foreach (var obj in res)
                {
                    valid = true;
                }
            }

            if (valid == false)
            {
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

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowSiteAllocation();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdSiteAllocation.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdSiteAllocation.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowSiteAllocation(gvr.RowIndex);
            }
        }
        protected void DeleteRowSiteAllocation(int rowIndex)
        {
            GetCurrentDataSiteAllocation();
            DataTable dt = ViewState["SiteAllocation_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SiteAllocation_Table"] = dt;
            grdSiteAllocation.DataSource = dt;
            grdSiteAllocation.DataBind();
            SetPreviousDataSiteAllocation();
        }
        protected void AddRowSiteAllocation()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SiteAllocation_Table"] != null)
            {
                GetCurrentDataSiteAllocation();
                dt = (DataTable)ViewState["SiteAllocation_Table"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtSiteName", typeof(string)));
                dt.Columns.Add(new DataColumn("lblClientName", typeof(string)));
                dt.Columns.Add(new DataColumn("lblSiteId", typeof(string)));
                dt.Columns.Add(new DataColumn("lblClId", typeof(string)));
                dt.Columns.Add(new DataColumn("lblSiteAddress", typeof(string)));
                dt.Columns.Add(new DataColumn("lblRouteId", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtSiteName"] = string.Empty;
            dr["lblClientName"] = string.Empty;
            dr["lblSiteId"] = string.Empty;
            dr["lblClId"] = string.Empty;
            dr["lblSiteAddress"] = string.Empty;
            dr["lblRouteId"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["SiteAllocation_Table"] = dt;
            grdSiteAllocation.DataSource = dt;
            grdSiteAllocation.DataBind();
            SetPreviousDataSiteAllocation();
        }
        protected void GetCurrentDataSiteAllocation()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtSiteName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblClientName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblSiteId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblClId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblSiteAddress", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblRouteId", typeof(string)));
            for (int i = 0; i < grdSiteAllocation.Rows.Count; i++)
            {
                TextBox txtSiteName = (TextBox)grdSiteAllocation.Rows[i].Cells[2].FindControl("txtSiteName");
                Label lblClientName = (Label)grdSiteAllocation.Rows[i].Cells[3].FindControl("lblClientName");
                Label lblSiteId = (Label)grdSiteAllocation.Rows[i].Cells[4].FindControl("lblSiteId");
                Label lblClId = (Label)grdSiteAllocation.Rows[i].Cells[5].FindControl("lblClId");
                Label lblSiteAddress = (Label)grdSiteAllocation.Rows[i].Cells[6].FindControl("lblSiteAddress");
                Label lblRouteId = (Label)grdSiteAllocation.Rows[i].Cells[7].FindControl("lblRouteId");

                drRow = dtTable.NewRow();
                drRow["txtSiteName"] = txtSiteName.Text;
                drRow["lblClientName"] = lblClientName.Text;
                drRow["lblSiteId"] = lblSiteId.Text;
                drRow["lblClId"] = lblClId.Text;
                drRow["lblSiteAddress"] = lblSiteAddress.Text;
                drRow["lblRouteId"] = lblRouteId.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SiteAllocation_Table"] = dtTable;

        }
        protected void SetPreviousDataSiteAllocation()
        {
            DataTable dt = (DataTable)ViewState["SiteAllocation_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtSiteName = (TextBox)grdSiteAllocation.Rows[i].Cells[2].FindControl("txtSiteName");
                Label lblClientName = (Label)grdSiteAllocation.Rows[i].Cells[3].FindControl("lblClientName");
                Label lblSiteId = (Label)grdSiteAllocation.Rows[i].Cells[4].FindControl("lblSiteId");
                Label lblClId = (Label)grdSiteAllocation.Rows[i].Cells[5].FindControl("lblClId");
                Label lblSiteAddress = (Label)grdSiteAllocation.Rows[i].Cells[6].FindControl("lblSiteAddress");
                Label lblRouteId = (Label)grdSiteAllocation.Rows[i].Cells[7].FindControl("lblRouteId");

                txtSiteName.Text = dt.Rows[i]["txtSiteName"].ToString();
                lblClientName.Text = dt.Rows[i]["lblClientName"].ToString();
                lblSiteId.Text = dt.Rows[i]["lblSiteId"].ToString();
                lblClId.Text = dt.Rows[i]["lblClId"].ToString();
                lblSiteAddress.Text = dt.Rows[i]["lblSiteAddress"].ToString();
                lblRouteId.Text = dt.Rows[i]["lblRouteId"].ToString();
            }
        }

        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Binding sites
                //TextBox txtSiteName = (TextBox)e.Row.FindControl("txtSiteName");

            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdSiteAllocation.Rows.Count > 0)
            {
                PrintGrid.SiteAllocnPrintGridView(grdSiteAllocation, "Site Allocation Details~Route Name : " + ddlRoute.SelectedItem.Text, "SiteAllocationDetails");
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");

                //clsData obj=new clsData();
                int flag = 0;
                //  int prvRouteId = 0,otherRouteId=0;
                if (rbRouteWiseAllocation.Checked)
                {
                    if (ddlRoute.SelectedItem.Text.Equals("Other"))
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "You can not assign site to Other route.";//"You can not allocte this site to ME as it wont have any route specified.";

                    }
                    else
                    {

                        for (int i = 0; i < grdSiteAllocation.Rows.Count; i++)
                        {
                            Label lblSiteId = (Label)grdSiteAllocation.Rows[i].Cells[4].FindControl("lblSiteId");
                            Label lblRouteId = (Label)grdSiteAllocation.Rows[i].Cells[7].FindControl("lblRouteId");

                            if (lblSiteId.Text != "" && lblRouteId.Text == "")
                            {
                                flag = 1;
                                dc.Site_Update_RouteWise(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(lblSiteId.Text),"","");//update route id against respective site
                            }
                        }
                    }

                }
                else if (rbMeWiseAllocation.Checked)
                {
                    if (ddlRoute.SelectedItem.Text.Equals("Other"))
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "Please allocate route for selected site(s) first and then assign it to particular ME.";//"You can not allocte this site to ME as it wont have any route specified.";

                    }
                    else
                    {
                        flag = 1;
                        dc.SiteAllocation_Update(0, 0, Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), 1);//delete entries respective route of me 
                        for (int i = 0; i < grdSiteAllocationME.Rows.Count; i++)
                        {
                            CheckBox cbxSelect = (CheckBox)grdSiteAllocationME.Rows[i].Cells[0].FindControl("cbxSelect");
                            Label lblSiteId = (Label)grdSiteAllocationME.Rows[i].Cells[3].FindControl("lblSiteId");
                            Label lblClId = (Label)grdSiteAllocationME.Rows[i].Cells[4].FindControl("lblClId");

                            if (cbxSelect.Checked)
                                dc.SiteAllocation_Update(Convert.ToInt32(lblClId.Text), Convert.ToInt32(lblSiteId.Text), Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), 0);

                        }
                    }
                }
                if (flag == 1)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Text = "Successfully Saved!!";
                    //Reset();
                }
            }

        }


        private void Reset()
        {
            ddlME.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;
            grdSiteAllocation.DataSource = null;
            grdSiteAllocation.DataBind();
            grdSiteAllocationME.DataSource = null;
            grdSiteAllocationME.DataBind();
            lblCount.Text = "";
            lblCountRows.Text = "Total No of Records: 0";


        }
        protected Boolean ValidateData()
        {

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            if (rbRouteWiseAllocation.Checked)
            {
                if (ddlRoute.SelectedIndex == 0 || ddlRoute.SelectedItem.Text == "--Select--")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Select Route for Site Allocation.";
                    valid = false;
                }

                for (int i = 0; i < grdSiteAllocation.Rows.Count; i++)
                {
                    Label lblSiteId = (Label)grdSiteAllocation.Rows[i].Cells[5].FindControl("lblSiteId");
                    TextBox txtSiteName = (TextBox)grdSiteAllocation.Rows[i].Cells[2].FindControl("txtSiteName");

                    if (lblSiteId.Text == "")
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Enter Site for Row " + (i + 1) + ".";
                        txtSiteName.Focus();
                        valid = false;
                    }
                }

            }
            else if (rbMeWiseAllocation.Checked)
            {
                if (ddlME.SelectedIndex == 0 || ddlME.SelectedItem.Text == "--Select--")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Select ME for Site Allocation.";
                    valid = false;
                }
                else if (!checkCheckBoxes())
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Select Site for Allocation.";
                    valid = false;
                }


            }
            return valid;
        }

        protected bool checkCheckBoxes()
        {
            bool allCheckBoxesChecked = false;

            for (int i = 0; i < grdSiteAllocationME.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdSiteAllocationME.Rows[i].Cells[0].FindControl("cbxSelect");

                if (cbxSelect.Checked)
                    allCheckBoxesChecked = true;
            }
            return allCheckBoxesChecked;
        }



        private void uncheckedAllChkBox()
        {
            for (int i = 0; i < grdSiteAllocationME.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdSiteAllocationME.Rows[i].Cells[0].FindControl("cbxSelect");
                cbxSelect.Checked = false;
            }
        }

        protected void lnkClearAllocn_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (ddlME.SelectedIndex != -1 && ddlME.SelectedIndex != 0)
            {
                dc.SiteAllocation_Update(0, 0, 0, Convert.ToInt32(ddlME.SelectedValue), 2);//reset
                lblMsg.Text = "Records Clear Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                grdSiteAllocationME.DataSource = null;
                grdSiteAllocationME.DataBind();

            }
            else
            {

                lblMsg.Text = "Please Select ME";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            // int cnt = 0; 
            lblCount.Text = "";
            if (ddlRoute.SelectedIndex != -1)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                grdSiteAllocation.DataSource = null;
                grdSiteAllocation.DataBind();
                grdSiteAllocationME.DataSource = null;
                grdSiteAllocationME.DataBind();
               
                if (rbMeWiseAllocation.Checked)
                    lblCountRows.Text = "Total No of Records:" + grdSiteAllocationME.Rows.Count;
                else
                    lblCountRows.Text = "Total No of Records:" + grdSiteAllocation.Rows.Count;


                //if (rbMeWiseAllocation.Checked)
                //{
                //    if (ddlME.SelectedIndex != -1 && ddlME.SelectedIndex != 0)
                //    {

                //        var siteResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), 0);
                //        grdSiteAllocationME.DataSource = siteResult;
                //        grdSiteAllocationME.DataBind();
                //        //}
                //        lblCountRows.Text = "Total No of Records:" + grdSiteAllocationME.Rows.Count; ;

                //        for (int i = 0; i < grdSiteAllocationME.Rows.Count; i++)
                //        {
                //            CheckBox cbxSelect = (CheckBox)grdSiteAllocationME.Rows[i].Cells[0].FindControl("cbxSelect");
                //            Label lblSiteId = (Label)grdSiteAllocationME.Rows[i].Cells[3].FindControl("lblSiteId");
                //            var chkResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), Convert.ToInt32(lblSiteId.Text)).ToList();
                //            if (chkResult.Count > 0)
                //            {
                //                cbxSelect.Checked = true;
                //                cnt++;
                //            }

                //            if (cnt > 0)
                //                lblCount.Text = "Allocated Site Count : " + cnt;

                //        }
                //    }
                //    else
                //    {
                //        lblMsg.Visible = true;
                //        lblMsg.ForeColor = System.Drawing.Color.Red;
                //        lblMsg.Text = "Please Select ME";
                //    }
                //}
                //else if (rbRouteWiseAllocation.Checked)
                //{
                //    if (!ddlRoute.SelectedItem.Text.Equals("Other"))
                //    {
                //        var siteResult = dc.Site_View_RouteWise(0, "", Convert.ToInt32(ddlRoute.SelectedValue)).ToList();
                //        int vcnt = siteResult.Count();
                //        int i = 0;
                //        foreach (var re in siteResult)
                //        {

                //            AddRowSiteAllocation();

                //            TextBox txtSiteName = (TextBox)grdSiteAllocation.Rows[i].Cells[2].FindControl("txtSiteName");
                //            Label lblClientName = (Label)grdSiteAllocation.Rows[i].Cells[3].FindControl("lblClientName");
                //            Label lblSiteId = (Label)grdSiteAllocation.Rows[i].Cells[4].FindControl("lblSiteId");
                //            Label lblClId = (Label)grdSiteAllocation.Rows[i].Cells[5].FindControl("lblClId");
                //            Label lblSiteAddress = (Label)grdSiteAllocation.Rows[i].Cells[6].FindControl("lblSiteAddress");
                //            Label lblRouteId = (Label)grdSiteAllocation.Rows[i].Cells[7].FindControl("lblRouteId");


                //            txtSiteName.Text = re.SITE_Name_var.ToString();
                //            lblClientName.Text = re.CL_Name_var.ToString();
                //            lblSiteId.Text = re.SITE_Id.ToString();  // inType;
                //            lblClId.Text = re.CL_Id.ToString();
                //            lblSiteAddress.Text = re.SITE_Address_var.ToString();
                //            lblRouteId.Text = re.SITE_Route_Id.ToString();
                //            i++;
                //        }
                //    }
                //    if (grdSiteAllocation.Rows.Count <= 0)
                //        AddRowSiteAllocation();

                //    lblCountRows.Text = "Total No of Records:" + grdSiteAllocation.Rows.Count; ;

                //}

            }
        }

        protected void ddlME_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int cnt = 0; 
            lblCount.Text = "";
            //if (ddlME.SelectedIndex != -1 && ddlME.SelectedIndex != 0 && ddlRoute.SelectedIndex != -1 && ddlRoute.SelectedIndex != 0)
            //{
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            grdSiteAllocation.DataSource = null;
            grdSiteAllocation.DataBind();
            grdSiteAllocationME.DataSource = null;
            grdSiteAllocationME.DataBind();
            lblCountRows.Text = "Total No of Records:" + grdSiteAllocationME.Rows.Count;
        
            //    if (rbMeWiseAllocation.Checked)
            //    {
            //        if (ddlRoute.SelectedItem.Text.Equals("Other"))
            //        {
            //            var siteResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), -1);
            //            grdSiteAllocationME.DataSource = siteResult;
            //            grdSiteAllocationME.DataBind();
            //        }
            //        else
            //        {
            //            var siteResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), 0);
            //            grdSiteAllocationME.DataSource = siteResult;
            //            grdSiteAllocationME.DataBind();
            //        }

            //        lblCountRows.Text = "Total No of Records:" + grdSiteAllocationME.Rows.Count; ;

            //        for (int i = 0; i < grdSiteAllocationME.Rows.Count; i++)
            //        {
            //            CheckBox cbxSelect = (CheckBox)grdSiteAllocationME.Rows[i].Cells[0].FindControl("cbxSelect");
            //            Label lblSiteId = (Label)grdSiteAllocationME.Rows[i].Cells[3].FindControl("lblSiteId");
            //            var chkResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), Convert.ToInt32(lblSiteId.Text)).ToList();
            //            if (chkResult.Count > 0)
            //            {
            //                cbxSelect.Checked = true;
            //                cnt++;
            //            }

            //            if (cnt > 0)
            //                lblCount.Text = "Allocated Site Count : " + cnt;



            //        }
            //    }
            //}
        }


        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            int cnt = 0; lblCount.Text = "";
            if (ddlRoute.SelectedIndex != -1 && ddlRoute.SelectedIndex != 0)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                
                if (rbMeWiseAllocation.Checked)
                {
                    if (ddlME.SelectedIndex != -1 && ddlME.SelectedIndex != 0)
                    {

                        var siteResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), 0);
                        grdSiteAllocationME.DataSource = siteResult;
                        grdSiteAllocationME.DataBind();
                        grdSiteAllocationME.Columns[0].Visible = true;
                        //}
                        lblCountRows.Text = "Total No of Records:" + grdSiteAllocationME.Rows.Count; ;

                        for (int i = 0; i < grdSiteAllocationME.Rows.Count; i++)
                        {
                            CheckBox cbxSelect = (CheckBox)grdSiteAllocationME.Rows[i].Cells[0].FindControl("cbxSelect");
                            Label lblSiteId = (Label)grdSiteAllocationME.Rows[i].Cells[3].FindControl("lblSiteId");
                            var chkResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), Convert.ToInt32(lblSiteId.Text)).ToList();
                            if (chkResult.Count > 0)
                            {
                                cbxSelect.Checked = true;
                                cnt++;
                            }

                            if (cnt > 0)
                                lblCount.Text = "Allocated Site Count : " + cnt;

                        }
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "Please Select ME";
                    }
                }
                else if (rbRouteWiseAllocation.Checked)
                {
                    if (!ddlRoute.SelectedItem.Text.Equals("Other"))
                    {
                        var siteResult = dc.Site_View_RouteWise(0, "", Convert.ToInt32(ddlRoute.SelectedValue)).ToList();
                        int vcnt = siteResult.Count();
                        int i = 0;
                        foreach (var re in siteResult)
                        {

                            AddRowSiteAllocation();

                            TextBox txtSiteName = (TextBox)grdSiteAllocation.Rows[i].Cells[2].FindControl("txtSiteName");
                            Label lblClientName = (Label)grdSiteAllocation.Rows[i].Cells[3].FindControl("lblClientName");
                            Label lblSiteId = (Label)grdSiteAllocation.Rows[i].Cells[4].FindControl("lblSiteId");
                            Label lblClId = (Label)grdSiteAllocation.Rows[i].Cells[5].FindControl("lblClId");
                            Label lblSiteAddress = (Label)grdSiteAllocation.Rows[i].Cells[6].FindControl("lblSiteAddress");
                            Label lblRouteId = (Label)grdSiteAllocation.Rows[i].Cells[7].FindControl("lblRouteId");


                            txtSiteName.Text = re.SITE_Name_var.ToString();
                            lblClientName.Text = re.CL_Name_var.ToString();
                            lblSiteId.Text = re.SITE_Id.ToString();  // inType;
                            lblClId.Text = re.CL_Id.ToString();
                            lblSiteAddress.Text = re.SITE_Address_var.ToString();
                            lblRouteId.Text = re.SITE_Route_Id.ToString();
                            i++;
                        }
                    }
                    if (grdSiteAllocation.Rows.Count <= 0)
                        AddRowSiteAllocation();

                    lblCountRows.Text = "Total No of Records:" + grdSiteAllocation.Rows.Count;

                }

            }
        }

        protected void lnkPrvAllocatedSite_Click(object sender, EventArgs e)
        {
            if (ddlME.SelectedIndex != -1 && ddlME.SelectedIndex != 0 && ddlRoute.SelectedIndex != -1 && ddlRoute.SelectedIndex != 0)
            {
                var siteResult = dc.SiteAllocation_View(Convert.ToInt32(ddlRoute.SelectedValue), Convert.ToInt32(ddlME.SelectedValue), -1);
                grdSiteAllocationME.DataSource = siteResult;
                grdSiteAllocationME.DataBind();
                if (grdSiteAllocationME.Rows.Count > 0)
                    grdSiteAllocationME.Columns[0].Visible = false;


                lblCountRows.Text = "Total No of Records:" + grdSiteAllocationME.Rows.Count;
                lblCount.Text = "Allocated Site Count : " + grdSiteAllocationME.Rows.Count;

            }
        }
    }
}